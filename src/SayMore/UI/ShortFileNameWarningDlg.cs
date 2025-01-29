using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DesktopAnalytics;
using L10NSharp;
using L10NSharp.UI;
using L10NSharp.XLiffUtils;
using SayMore.Utilities;
using SIL.Windows.Forms.Extensions;
using SIL.Windows.Forms.PortableSettingsProvider;
using static System.String;
using Process = SIL.Program.Process;
using Settings = SayMore.Properties.Settings;

namespace SayMore.UI
{
	public partial class ShortFileNameWarningDlg : Form
	{
		private bool _suppressWarnings;
		private string _failedActionsLabelOrigText;
		private static readonly ShortFileNameWarningDlg Singleton = new ShortFileNameWarningDlg();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Call this method to handle a short filename warning. It will handle the decision of
		/// whether to display the form, update the details already being displayed, or merely
		/// note the failure (if they user has already asked not to be advised).
		/// </summary>
		/// <param name="owner">The owner of the form (set only if the form has not already been
		/// shown, but always expected to be the same form every time this -- or
		/// <see cref="ViewSettings"/> -- is called)</param>.
		/// <param name="path">Path of the file for which a short (8.3) name could not be
		/// obtained.</param>
		/// <param name="failedAction">A short description of the action that was being attempted
		/// that was wanting to use a short filename. This description will be presented to the
		/// user when this form is shown, so the description should be localized.</param>
		/// <remarks>Caller is responsible for ensuring this is called on the UI thead. (We could
		/// do it, but in practice the only caller already needs to do it, so why bother?)
		/// </remarks>
		/// ------------------------------------------------------------------------------------
		public static void NoteFailure(Form owner, string path, string failedAction)
		{
			try
			{
				Singleton.NoteFailureInternal(owner, path, failedAction);
			}
			catch (ObjectDisposedException)
			{
				// The user apparently closed the owning window (which causes this form to be
				// disposed) before this got invoked or while we were updating everything.
				// No worries. There will be another day.
			}
		}

		private void NoteFailureInternal(Form owner, string path, string failedAction)
		{
			if (IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			_checkDone.Visible = false;
			_checkDone.Checked = false;

			// Note: Even if we are suppressing all warnings, we go ahead and add the failure
			// information in case the user later opens this to view the settings.

			if (!_flowLayoutFailedActions.Controls.OfType<Label>().Select(lbl => lbl.Text)
				    .Any(failure => failure.Equals(failedAction)))
			{
				var newFailedActionLabel = new Label
				{
					Text = failedAction,
					AutoSize = true
				};
				_flowLayoutFailedActions.Controls.Add(newFailedActionLabel);
				_flowLayoutFailedActions.SetFlowBreak(newFailedActionLabel, true);
				if (_failedActionsLabelOrigText != null)
					_lblFailedActions.Text = _failedActionsLabelOrigText;
			}

			if (IsSuppressedVolume(path) || IsSuppressedFilename(path))
				return;

			if (!_suppressWarnings)
				ShowNow(owner);
		}

		private bool IsSuppressedVolume(string path)
		{
			var i = TryAddVolume(path);
			return i >= 0 && _checkedListBoxVolumes.GetItemChecked(i);
		}

		private int TryAddVolume(string path)
		{
			var volume = FileSystemUtils.GetVolume(path);

			if (volume == null) // Really unlikely
				return -1;

			int insertAtIndex = 0;
			for (var i = 0; i < _checkedListBoxVolumes.Items.Count; i++)
			{
				var compareVal = StringComparer.OrdinalIgnoreCase.Compare(
					_checkedListBoxVolumes.Items[i].ToString(),
					volume
				);

				if (compareVal == 0)
				{
					// The volume already exists.
					return i;
				}
				if (compareVal > 0)
				{
					// We've found the position where the new volume should be inserted
					insertAtIndex = i;
					break;
				}
			}

			// Insert the new volume at the determined index
			_checkedListBoxVolumes.Items.Insert(insertAtIndex, volume);
			return insertAtIndex;
		}

		private bool IsSuppressedFilename(string path)
		{
			var i = TryAddFilename(path);
			return i >= 0 && _checkedListBoxFiles.GetItemChecked(i);
		}

		private int TryAddFilename(string path)
		{
			for (var i = 0; i < _checkedListBoxFiles.Items.Count; i++)
			{
				var item = _checkedListBoxFiles.Items[i];
				if (item.ToString().Equals(path, StringComparison.OrdinalIgnoreCase))
					return i;
			}

			_checkedListBoxFiles.Items.Add(path);
			return _checkedListBoxFiles.Items.Count - 1;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Call this method to display this form (modeless) if it is not already being displayed
		/// </summary>
		/// <param name="owner">The owner of the form (set only if the form has not already been
		/// shown, but always expected to be the same form every time this -- or
		/// <see cref="NoteFailure"/> -- is called)</param>.
		/// <param name="projectPath">Path of the current project file, which is used to get the
		/// volume so we can display a reasonable value in the list of volumes for which warnings
		/// should be suppressed. In most (maybe all) cases, any actual warnings will be for files
		/// on the same volume as the project.</param>
		/// ------------------------------------------------------------------------------------
		public static void ViewSettings(Form owner, string projectPath) =>
			Singleton.ViewSettingsInternal(owner, projectPath);
		
		/// ------------------------------------------------------------------------------------
		private void ViewSettingsInternal(Form owner, string projectPath)
		{
			try
			{
				TryAddVolume(projectPath);
				// If no failures have been reported this session, give them a chance to say they
				// have fixed it. This doesn't do anything but reset the settings so this dialog
				// box will become *forever* unavailable unless, in fact, another failure is noted.
				_checkDone.Visible = _flowLayoutFailedActions.Controls.Count == 0;
				ShowNow(owner);
			}
			catch (ObjectDisposedException)
			{
				// Since this is modeless, there is the (very slight) chance the user could close
				// the owning window (which causes this form to be disposed) before we finish
				// updating everything.
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ShowNow(Form owner)
		{
			if (InvokeRequired)
			{
				Invoke(new Action(() => ShowInternal(owner)));
				return;
			}
			ShowInternal(owner);
		}

		/// ------------------------------------------------------------------------------------
		private void ShowInternal(Form owner)
		{
			if (owner == null)
				throw new ArgumentNullException(nameof(owner));

			if (owner.IsDisposed || !owner.IsHandleCreated) // Unlikely
				return;

			Debug.Assert(!IsDisposed);

			if (!Visible)
			{
				// Not sure that this is strictly required, but in reality, the owner should never
				// change.
				if (IsHandleCreated)
				{
					Debug.Assert(owner == Owner);
					Show();
				}
				else
				{
					owner.FormClosing += OnOwnerClosing;
					Show(owner);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void OnOwnerClosing(object sender, FormClosingEventArgs e)
		{
			Dispose();
		}

		/// ------------------------------------------------------------------------------------
		private ShortFileNameWarningDlg()
		{
			InitializeComponent();
			
			// Initialize controls and checkboxes from settings
			InitializeVolumesChecklist();
			InitializeFilenamesChecklist();
			_chkDoNotReportEver.Checked = Settings.Default.SuppressAllShortFilenameWarnings;

			if (Settings.Default.ShortFileNameWarningDlg == null)
			{
				StartPosition = FormStartPosition.CenterParent;
				Settings.Default.ShortFileNameWarningDlg = FormSettings.Create(this);
			}

			LocalizeItemDlg<XLiffDocument>.StringsLocalized += HandleStringsLocalized;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			Settings.Default.ShortFileNameWarningDlg.InitializeForm(this);
			base.OnLoad(e);
			HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeVolumesChecklist()
		{
			var regexVol = new Regex($@"[^{Path.VolumeSeparatorChar}]+{Path.VolumeSeparatorChar}");
			var volumes = regexVol.Matches(Settings.Default.ShortFilenameWarningsToSuppressByVolume)
				.Cast<Match>().Select(m => m.Value);

			PopulateChecklist(_checkedListBoxVolumes, volumes);
			_checkedListBoxVolumes.Tag = new Action(() =>
			{
				var list = GetCheckedItems(_checkedListBoxVolumes, true);
				Settings.Default.ShortFilenameWarningsToSuppressByVolume = list;
				if (list != Empty)
					Analytics.Track("Suppress Short name warnings by volume");
			});
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeFilenamesChecklist()
		{
			var filenameWarningsToSuppress = Settings.Default.ShortFilenameWarningsToSuppress
				.Split(new [] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

			PopulateChecklist(_checkedListBoxFiles, filenameWarningsToSuppress);
			_checkedListBoxFiles.Tag = new Action(() =>
			{
				var list = GetCheckedItems(_checkedListBoxFiles, false, Environment.NewLine);
				Settings.Default.ShortFilenameWarningsToSuppress = list;
				if (list != Empty)
					Analytics.Track("Suppress Short name warnings by filename");
			});
		}

		/// ------------------------------------------------------------------------------------
		private static void PopulateChecklist(CheckedListBox listbox,
			IEnumerable<string> existingCheckedValues)
		{
			foreach (var value in existingCheckedValues)
				listbox.Items.Add(value, CheckState.Checked);
		}

		/// ------------------------------------------------------------------------------------
		protected void HandleStringsLocalized(ILocalizationManager lm = null)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
			{
				const string fsUtil8dot3 = "fsutil 8dot3name";

				_linkLabelFsUtilMsg.Text = Format(_linkLabelFsUtilMsg.Text, fsUtil8dot3);

				_linkLabelFsUtilMsg.LinkArea = new LinkArea(
					_linkLabelFsUtilMsg.Text.IndexOf(fsUtil8dot3, StringComparison.Ordinal),
					fsUtil8dot3.Length);

				if (_flowLayoutFailedActions.Controls.Count == 0)
				{
					_failedActionsLabelOrigText = _lblFailedActions.Text;
					_lblFailedActions.Text = Format(LocalizationManager.GetString(
							"ShortFileNameWarningDlg.lblFailedActionsNoCurrentFailures",
							"This will help to avoid problems with certain utilities that {0} uses.",
							"Param is \"SayMore\" (program name)"),
						Program.ProductName);
				}

				_chkDoNotReportAnymoreThisSession.Text =
					Format(_chkDoNotReportAnymoreThisSession.Text, Program.ProductName);
			}
		}
		
		/// ------------------------------------------------------------------------------------
		private void _linkLabelFsUtilMsg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Analytics.Track("Navigating to FsUtil URL");
			Process.SafeStart("https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/fsutil-8dot3name");
		}

		/// ------------------------------------------------------------------------------------
		private static string GetCheckedItems(CheckedListBox listbox, bool sort, string separator = "")
		{
			var items = (from object item in listbox.CheckedItems select item.ToString()).ToList();
			if (sort)
				items.Sort(StringComparer.OrdinalIgnoreCase);
			return Join(separator, items);
		}

		/// ------------------------------------------------------------------------------------
		private void _chkDoNotReportAnymoreThisSession_CheckedChanged(object sender, EventArgs e)
		{
			SetSuppressWarnings();
			if (_chkDoNotReportAnymoreThisSession.Checked)
				Analytics.Track("Suppress Short name warnings for session");
		}

		/// ------------------------------------------------------------------------------------
		private void _chkDoNotReportEver_CheckedChanged(object sender, EventArgs e)
		{
			UpdateControlsEnabledState();
			Settings.Default.SuppressAllShortFilenameWarnings = _chkDoNotReportEver.Checked;
			SetSuppressWarnings();
			Analytics.Track("Suppress Short name warnings forever", new Dictionary<string, string> {
				{"checked", _chkDoNotReportEver.Checked.ToString()} });

		}

		/// ------------------------------------------------------------------------------------
		private void _checkDone_CheckedChanged(object sender, EventArgs e)
		{
			bool done = _checkDone.Checked;
			// We want to avoid the potentially confusing state where the user says they think
			// the problem is solved (so no more failures should be reported) but they also say
			// not to tell them if there is another failure. If they've really fixed it, they
			// should be forced to find out if they haven't really.
			if (done)
				_chkDoNotReportEver.Checked = _chkDoNotReportAnymoreThisSession.Checked = false;
			
			// If checked, we'll go ahead and disable most of the other controls this form, but
			// we won't actually clear the settings until they close it, just in case they change
			// their mind.
			UpdateControlsEnabledState();
			_chkDoNotReportEver.Enabled = !done;
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateControlsEnabledState()
		{
			var enable = !_chkDoNotReportEver.Checked && !_checkDone.Checked;
			_lblDoNotReportForVolumes.Enabled = enable;
			_checkedListBoxVolumes.Enabled = enable;
			_lblDoNotReportForFiles.Enabled = enable;
			_checkedListBoxFiles.Enabled = enable;
			_chkDoNotReportAnymoreThisSession.Enabled = enable;
		}

		/// ------------------------------------------------------------------------------------
		private void SetSuppressWarnings()
		{
			_suppressWarnings = _chkDoNotReportAnymoreThisSession.Checked ||
				_chkDoNotReportEver.Checked;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCheckedListBoxItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (((CheckedListBox)sender).Tag is Action action)
			{
				// Defer processing until after the state is updated
				BeginInvoke(action);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCloseClick(object sender, EventArgs e)
		{
			Hide();

			if (_checkDone.Checked)
			{
				Settings.Default.ShortFilenameWarningsToSuppressByVolume = Empty;
				Settings.Default.ShortFilenameWarningsToSuppress = Empty;
				Settings.Default.SuppressAllShortFilenameWarnings = false;
				// For now, let's go ahead and remember the volumes we've known about. There's
				// most likely just one, and we'd have to re-add it anyway if we re-show this.
				// If there were any others, remembering them won't hurt.
				foreach (int index in _checkedListBoxVolumes.CheckedIndices)
					_checkedListBoxVolumes.SetItemChecked(index, false);
				// Let's clear the list of files and failures we've seen. That will reduce
				// noise if we do have another failure.
				_checkedListBoxFiles.Items.Clear();
				_flowLayoutFailedActions.Controls.Clear();

				Analytics.Track("User claims to have solved short filename problem");
			}
		}
	}
}
