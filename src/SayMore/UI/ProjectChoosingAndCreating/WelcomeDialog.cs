using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;
using SIL.Windows.Forms.PortableSettingsProvider;
using SayMore.Model;
using SayMore.Properties;
using SayMore.UI;
using SayMore.UI.ProjectChoosingAndCreating.NewProjectDialog;
using SayMore.Utilities;

namespace SayMore.UI.ProjectChoosingAndCreating
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates the welcome dialog box, in which users may create new projects, or open
	/// existing projects via browsing the file system or by choosing a recently used project.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public sealed partial class WelcomeDialog : SayMoreFormBase
	{
		public WelcomeDialogViewModel Model { get; set; }

		// Captured once, immediately after InitializeComponent() and before anything has a
		// chance to scale a font or resize the window, so later layout passes always have
		// the dialog's true, pristine Designer-authored baseline to measure against instead
		// of whatever an earlier pass already changed it to.
		private readonly int _designTimeClientHeight;
		private readonly int _designTimeMinimumHeight;
		private readonly int _designTimeLinksRowBottomMargin;
		private bool _constrainingWelcomeLinksRowLayout;

		/// ------------------------------------------------------------------------------------
		public WelcomeDialog(WelcomeDialogViewModel viewModel)
		{
			Logger.WriteEvent("WelcomeDialog constructor");

			InitializeComponent();

			_designTimeClientHeight = ClientSize.Height;
			_designTimeMinimumHeight = MinimumSize.Height;
			_designTimeLinksRowBottomMargin = ClientSize.Height - flowLayoutPanel1.Bottom;

			Font = SystemFonts.MessageBoxFont; //use the default OS UI font

			Model = viewModel;

			if (Settings.Default.WelcomeDialog == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.WelcomeDialog = FormSettings.Create(this);
			}

			tsOptions.Renderer = new SIL.Windows.Forms.NoToolStripBorderRenderer();
			tsOptions.BackColorBegin = Color.White;
			tsOptions.BackColorEnd = Color.White;
			DialogResult = DialogResult.Cancel;

			LoadMRUButtons();

			LocalizationInitiated();

			UITextScaler.ScaleChanged += HandleUiTextScaleChangedForSubTitle;
			Disposed += (s, e) => UITextScaler.ScaleChanged -= HandleUiTextScaleChangedForSubTitle;

			UITextScaler.ScaleChanged += HandleUiTextScaleChangedForLinksRow;
			Disposed += (s, e) => UITextScaler.ScaleChanged -= HandleUiTextScaleChangedForLinksRow;

			// Set once here (in addition to the Resize- and ScaleChanged-driven updates
			// elsewhere) so the very first layout pass -- including a cold start directly
			// into a language whose recommended text scale is already applied before the
			// dialog is first shown -- already has the correct wrap width, instead of only
			// correcting itself after a later event fires.
			ConstrainWelcomeLinksRowLayout();
			Resize += (s, e) => ConstrainWelcomeLinksRowLayout();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadMRUButtons()
		{
			_buttonMru0.Visible = false;

			int i = 0;
			foreach (var recentProjectInfo in Model.RecentlyUsedProjects)
			{
				ToolStripButton tsb;

				if (i++ == 0)
					tsb = _buttonMru0;
				else
				{
					tsb = new ToolStripButton(_buttonMru0.Image)
					{
						DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
						ImageAlign = _buttonMru0.ImageAlign,
						TextAlign = _buttonMru0.TextAlign,
						Font = _buttonMru0.Font,
						Margin = _buttonMru0.Margin
					};
					//tsb.Name = "tsbMru" + i;
					tsb.Click += HandleMruClick;
					var index = tsOptions.Items.IndexOf(_buttonBrowse);
					tsOptions.Items.Insert(index, tsb);
				}

				tsb.Visible = true;
				tsb.Name = recentProjectInfo.Key;
				tsb.ToolTipText = recentProjectInfo.Key;
				tsb.Text = recentProjectInfo.Value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets up the link labels with proper localizations.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LocalizationInitiated()
		{
			_labelVersionInfo.Text = ApplicationContainer.GetVersionInfo(_labelVersionInfo.Text, BuildType.Current);

			_linkSILWebsite.Text = String.Format(_linkSILWebsite.Text, Application.CompanyName);
			_linkSayMoreWebsite.Text = String.Format(_linkSayMoreWebsite.Text, Application.ProductName);

			_linkSILWebsite.Links.Clear();
			_linkSayMoreWebsite.Links.Clear();

			// Add the underline and link for the SIL Global website.
			int i = _linkSILWebsite.Text.IndexOf(Application.CompanyName, StringComparison.Ordinal);
			if (i >= 0)
				_linkSILWebsite.Links.Add(i, Application.CompanyName.Length, Settings.Default.SilWebSite);

			// Add the underline and link for application's website.
			i = _linkSayMoreWebsite.Text.IndexOf(Application.ProductName, StringComparison.Ordinal);
			if (i >= 0)
				_linkSayMoreWebsite.Links.Add(i, Application.ProductName.Length, Settings.Default.ProgramsWebSite);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.WelcomeDialog.InitializeForm(this);
			base.OnLoad(e);

			// base.OnLoad(e) just raised Load, which SayMoreFormBase's own handler used to
			// apply the current text scale's font to every control, including _labelSubTitle.
			// Re-measuring here, now that the scaled font is in place, keeps its Height in
			// sync with that same pass from the moment the dialog first appears.
			ResizeSubTitleForCurrentTextScale();

			// Same reasoning as above, for the website links row: now that the scaled font
			// is in place and flowLayoutPanel1 has already re-measured against it, re-check
			// whether that row now needs more room than the dialog has from the moment the
			// dialog first appears, rather than only on a later Resize/ScaleChanged event.
			ConstrainWelcomeLinksRowLayout();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			Settings.Default.Save();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleUiTextScaleChangedForSubTitle(object sender, EventArgs e)
		{
			ResizeSubTitleForCurrentTextScale();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleUiTextScaleChangedForLinksRow(object sender, EventArgs e)
		{
			ConstrainWelcomeLinksRowLayout();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// _labelSubTitle has AutoSize off (needed so AutoEllipsis can truncate it against the
		/// dialog's width instead of growing wider than its anchored space), so nothing resizes
		/// it in response to a Font change the way the generic recursive UI scaling pass does
		/// for an AutoSize label. Its Height stays at the Designer's fixed, unscaled value no
		/// matter how large its Font grows, leaving the same unscaled sliver of room for a
		/// taller scaled line and clipping its descenders (e.g. the "g" in "Language" and
		/// "Management") off the bottom. Re-measuring the current Font's own line height and
		/// growing the label to match keeps the full scaled line, descenders included, inside
		/// its bounds at every scale.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ResizeSubTitleForCurrentTextScale()
		{
			using (var g = CreateGraphics())
			{
				_labelSubTitle.Height = TextRenderer.MeasureText(g, "X", _labelSubTitle.Font,
					Size.Empty, TextFormatFlags.NoPadding).Height;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// _linkSILWebsite and _linkSayMoreWebsite sit side by side in flowLayoutPanel1, and
		/// each one is already AutoSize, so the second link's position already comes from the
		/// first link's actual measured width -- there is no fixed pixel offset between them.
		/// But flowLayoutPanel1 is itself AutoSize with no MaximumSize, so its WrapContents
		/// setting never has a width to wrap against: the panel just keeps growing its own
		/// bounds to fit both links on one row no matter how wide their combined (localized,
		/// scaled) text gets, carrying the second link past the dialog's visible right edge
		/// once that combined width exceeds the space actually available there. English and
		/// Spanish's translated strings stay well within the dialog's width at every supported
		/// text scale, so this never shows up for them; Tibetan's longer translated strings,
		/// especially at a larger scale, routinely do not. Constraining the panel's
		/// MaximumSize to the dialog's own current width lets WrapContents move the
		/// second link onto its own line -- derived from the first link's real measured
		/// bounds -- only once that's genuinely needed to stay on screen, rather than every
		/// time the row happens to be wider than the full width the dialog actually has
		/// available for it.
		///
		/// Wrapping onto a second line (or simply a single scaled line growing taller than
		/// the Designer ever budgeted room for) can need more height than this small,
		/// fixed-size dialog currently has below this row. Growing the dialog to fit is safe
		/// on its own, but flowLayoutPanel1 stays Anchor = Top|Left (fixed, absolute
		/// position) while _labelVersionInfo above it is Anchor = Bottom: growing
		/// ClientSize.Height would otherwise drag _labelVersionInfo down toward/into this
		/// row as a side effect, since its Bottom anchor was captured against the dialog's
		/// original (smaller) height. Restoring _labelVersionInfo's own position afterward
		/// keeps it exactly where it was, so growing the dialog only ever adds room below
		/// this row, never disturbs anything above it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ConstrainWelcomeLinksRowLayout()
		{
			// Re-entrancy guard: changing ClientSize.Height below raises this form's own
			// Resize event synchronously, which is also wired to this same method.
			if (_constrainingWelcomeLinksRowLayout)
				return;

			_constrainingWelcomeLinksRowLayout = true;
			try
			{
				const int rightMargin = 2; // small cosmetic gap from the dialog's own edge

				var availableWidth = ClientSize.Width - flowLayoutPanel1.Left - rightMargin;
				if (availableWidth > 0)
					flowLayoutPanel1.MaximumSize = new Size(availableWidth, 0);
				flowLayoutPanel1.PerformLayout();

				var labelVersionInfoTopBeforeResize = _labelVersionInfo.Top;
				var neededClientHeight = flowLayoutPanel1.Bottom + _designTimeLinksRowBottomMargin;
				var targetClientHeight = Math.Max(_designTimeClientHeight, neededClientHeight);
				if (ClientSize.Height != targetClientHeight)
				{
					var growth = Math.Max(0, targetClientHeight - _designTimeClientHeight);
					MinimumSize = new Size(MinimumSize.Width, _designTimeMinimumHeight + growth);
					ClientSize = new Size(ClientSize.Width, targetClientHeight);
					_labelVersionInfo.Top = labelVersionInfoTopBeforeResize;
				}
			}
			finally
			{
				_constrainingWelcomeLinksRowLayout = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWebSiteLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string tgt = e.Link.LinkData as string;

			if (!string.IsNullOrEmpty(tgt))
				System.Diagnostics.Process.Start(tgt);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleBrowseForExistingProjectClick(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.Title = LocalizationManager.GetString(
					"DialogBoxes.WelcomeDlg.OpenFileDlgCaption", "Open SayMore Project");
				var prjFilterText = LocalizationManager.GetString("DialogBoxes.WelcomeDlg.ProjectFileType",
					"SayMore Project") + String.Format("(*.{0})|*.{0}", Project.ProjectSettingsFileExtension);

				// TODO: This should really be a static or at least in a class that is accessible
				// from anywhere because this is the second place it's used. I'm hesitant to use
				// a static, because I don't understand the DI, app. container and project
				// context stuff well enough.

				// JH says: The di approach is to inject, not reach out.
				// I.e., it should be a parameter to the constructor of this class.
				var projPath = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SayMore");

				dlg.Filter = prjFilterText;
				dlg.InitialDirectory = projPath;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				if (dlg.ShowDialog(this) == DialogResult.Cancel)
					return;

				Model.ProjectSettingsFilePath = dlg.FileName;
			}

			DialogResult = DialogResult.OK;
			Close();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCreateProjectClick(object sender, EventArgs e)
		{
			var viewModel = new NewProjectDlgViewModel();

			using var dlg = new NewProjectDlg(viewModel);
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				Model.SetRequestedPath(NewProjectDlgViewModel.ParentFolderPathForNewProject, viewModel.NewProjectName);
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMruClick(object sender, EventArgs e)
		{
			if (sender is ToolStripButton tsb)
			{
				Model.ProjectSettingsFilePath = tsb.Name;
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			var rc = new Rectangle(0, 0, ClientSize.Width, 45);

			// Draw the gradient blue bar.
			using (var br = new LinearGradientBrush(rc, AppColors.BarBegin, AppColors.BarEnd, 0.0f))
				e.Graphics.FillRectangle(br, rc);

			// Draw a line at the bottom of the gradient blue bar.
			using (var pen = new Pen(AppColors.BarBorder))
				e.Graphics.DrawLine(pen, 0, rc.Bottom, rc.Right, rc.Bottom);

			rc = new Rectangle(new Point(_labelSubTitle.Left - 6, 18), ResourceImageCache.SayMoreText.Size);
			//rc.Inflate(-4, -4);
			e.Graphics.DrawImage(ResourceImageCache.SayMoreText, rc);

			// Draw the application's logo image.
			rc = new Rectangle(new Point(pnlOptions.Left - 10, 0), ResourceImageCache.LargeSayMoreLogo.Size);
			e.Graphics.DrawImage(ResourceImageCache.LargeSayMoreLogo, rc);
		}
	}
}