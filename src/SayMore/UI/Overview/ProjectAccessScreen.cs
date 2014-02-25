using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using Palaso.IO;
using SIL.Archiving.Generic.AccessProtocol;

namespace SayMore.UI.Overview
{
	public partial class ProjectAccessScreen : UserControl
	{
		private bool _isLoaded;
		private string _currentUri;
		private string _archivingFileDirectoryName;

		/// ------------------------------------------------------------------------------------
		public ProjectAccessScreen()
		{
			InitializeComponent();

			// access protocol list
			HandleStringsLocalized();
			LocalizeItemDlg.StringsLocalized += HandleStringsLocalized;

			_linkHelp.Click += (s, e) =>
				Program.ShowHelpTopic("/Using_Tools/Project_tab/Choose_Access_Protocol.htm");
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);

			LocalizeItemDlg.StringsLocalized += HandleStringsLocalized;
		}

		private string GetBaseUriDirectory()
		{
			var fileName = FileLocator.GetFileDistributedWithApplication("Archiving", AccessProtocols.kProtocolFileName);
			return Path.GetDirectoryName(fileName);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStringsLocalized()
		{
			_archivingFileDirectoryName = GetBaseUriDirectory();
			Debug.Assert(_archivingFileDirectoryName != null);
			if (LocalizationManager.UILanguageId != "en" && Directory.Exists(Path.Combine(_archivingFileDirectoryName, LocalizationManager.UILanguageId)))
				_archivingFileDirectoryName = Path.Combine(_archivingFileDirectoryName, LocalizationManager.UILanguageId);

			var protocols = AccessProtocols.LoadStandardAndCustom(_archivingFileDirectoryName);
			protocols.Insert(0, new ArchiveAccessProtocol { ProtocolName = "None" });
			_projectAccess.DataSource = protocols;

			SizeProtocolsComboBox(_projectAccess);
		}

		/// ------------------------------------------------------------------------------------
		private void SizeProtocolsComboBox(ComboBox comboBox)
		{
			var maxWidth = 0;
			foreach (var item in comboBox.Items)
			{
				var itmWidth = TextRenderer.MeasureText(item.ToString(), comboBox.Font).Width;
				if (itmWidth > maxWidth)
					maxWidth = itmWidth;
			}

			comboBox.Width = maxWidth + 30;
		}

		/// ------------------------------------------------------------------------------------
		private void _projectAccess_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_projectAccess.SelectedItem == null) return;

			if (_isLoaded) SetForCustom();

			var item = (ArchiveAccessProtocol)_projectAccess.SelectedItem;

			// was the last item (Custom) selected?
			if (_projectAccess.SelectedIndex == _projectAccess.Items.Count - 1)
			{
				_customAccessChoices.Text = item.ChoicesToCsv();
			}
			else
			{
				_currentUri = GetDocumentationUri(item);
				_webBrowser.Navigate(_currentUri);
			}

		}

		/// <remarks>If the documentation has not been localized, return the English version</remarks>
		private string GetDocumentationUri(ArchiveAccessProtocol item)
		{
			try
			{
				return item.GetDocumentaionUri(_archivingFileDirectoryName);
			}
			catch (DirectoryNotFoundException)
			{
				return item.GetDocumentaionUri(GetBaseUriDirectory());
			}
			catch (FileNotFoundException)
			{
				return item.GetDocumentaionUri(GetBaseUriDirectory());
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SetForCustom()
		{
			var isCustom = _projectAccess.SelectedIndex == _projectAccess.Items.Count - 1;
			var isNone = _projectAccess.SelectedIndex == 0;

			_labelCustomAccess.Visible = isCustom;
			_labelCustomInstructions.Visible = isCustom;
			_customAccessChoices.Visible = isCustom;
			_webBrowser.Visible = !isCustom && !isNone;
		}

		/// ------------------------------------------------------------------------------------
		private void ProjectAccessScreen_Leave(object sender, EventArgs e)
		{
			Save();
		}

		/// ------------------------------------------------------------------------------------
		private void ProjectAccessScreen_Load(object sender, EventArgs e)
		{
			// show values from project file
			var project = Program.CurrentProject;

			foreach (var item in _projectAccess.Items.Cast<object>().Where(i => i.ToString() == project.AccessProtocol))
				_projectAccess.SelectedItem = item;

			_isLoaded = true;

			SetForCustom();

			foreach (Control control in Controls)
				control.Validated += delegate { Save(); };

		}

		/// ------------------------------------------------------------------------------------
		private void _webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			if (e.Url.AbsoluteUri != _currentUri)
				e.Cancel = true;
		}

		/// ------------------------------------------------------------------------------------
		internal void Save()
		{
			// check for changes
			var project = Program.CurrentProject;

			// happens during testing
			if (project == null) return;

			var changed = (_projectAccess.Text != project.AccessProtocol);

			// check if custom access choices changed
			ArchiveAccessProtocol custom = (ArchiveAccessProtocol)_projectAccess.Items[_projectAccess.Items.Count - 1];
			if (_customAccessChoices.Text != custom.ChoicesToCsv())
			{
				var customs = AccessProtocols.LoadCustom();
				var firstCustom = customs.First();
				firstCustom.SetChoicesFromCsv(_customAccessChoices.Text);
				_customAccessChoices.Text = firstCustom.ChoicesToCsv();
				AccessProtocols.SaveCustom(customs);
				changed = true;
			}

			if (!changed) return;

			// save changes
			project.AccessProtocol = _projectAccess.Text;
			project.Save();
		}
	}
}
