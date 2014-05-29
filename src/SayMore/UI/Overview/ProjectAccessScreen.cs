using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using Palaso.IO;

namespace SayMore.UI.Overview
{
	public partial class ProjectAccessScreen : UserControl, ISaveable
	{
		private bool _isLoaded;
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

		private string GetBaseUriDirectory()
		{
			var fileName = FileLocator.GetFileDistributedWithApplication("Archiving", "blkah");
			return Path.GetDirectoryName(fileName);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStringsLocalized()
		{
			_archivingFileDirectoryName = GetBaseUriDirectory();
			Debug.Assert(_archivingFileDirectoryName != null);
			if (LocalizationManager.UILanguageId != "en" && Directory.Exists(Path.Combine(_archivingFileDirectoryName, LocalizationManager.UILanguageId)))
				_archivingFileDirectoryName = Path.Combine(_archivingFileDirectoryName, LocalizationManager.UILanguageId);
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
			e.Cancel = true;
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			// SP-875: Project Access field reverting to "None"
			if (!_isLoaded) return;

			// check for changes
			var project = Program.CurrentProject;

			// happens during testing
			if (project == null) return;

			var changed = (_projectAccess.Text != project.AccessProtocol);

			// check if custom access choices changed

			if (!changed) return;

			// save changes
			project.AccessProtocol = _projectAccess.Text;
			project.Save();
		}
	}
}
