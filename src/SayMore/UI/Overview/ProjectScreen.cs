using System.Collections.Generic;
using System.Drawing;
using L10NSharp;
using System.Windows.Forms;
using L10NSharp.UI;
using Palaso.Reporting;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.Overview
{
	public interface ISaveable
	{
		void Save();
	}

	public partial class ProjectScreen : UserControl, ISayMoreView, ISaveable
	{
		private readonly ProgressScreen _progressView;
		private readonly ProjectMetadataScreen _metadataView;
		private readonly ProjectAccessScreen _accessView;
		private readonly ProjectDocsScreen _descriptionDocsView;
		private readonly ProjectOtherDocsScreen _otherDocsView;
		private bool _statsViewActivated;

		/// ------------------------------------------------------------------------------------
		public ProjectScreen(ProjectMetadataScreen metadataView, ProjectAccessScreen accessView, ProgressScreen progressView, ProjectDescriptionDocsScreen descriptionDocsView, ProjectOtherDocsScreen otherDocsView)
		{
			Logger.WriteEvent("ProjectScreen constructor");

			_metadataView = metadataView;
			_progressView = progressView;
			_accessView = accessView;
			_descriptionDocsView = descriptionDocsView;
			_otherDocsView = otherDocsView;

			InitializeComponent();

			HandleStringsLocalized();
			_splitter.Panel2.BackColor = Color.FromArgb(230, 150, 100);
			_metadataView.BackColor = _splitter.Panel2.BackColor;
			_progressView.BackColor = _splitter.Panel2.BackColor;
			_accessView.BackColor = _splitter.Panel2.BackColor;
			_descriptionDocsView.BackColor = _splitter.Panel2.BackColor;
			_otherDocsView.BackColor = _splitter.Panel2.BackColor;

			LocalizeItemDlg.StringsLocalized += HandleStringsLocalized;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// SP-788: "Cannot access a disposed object" when changing UI language
				LocalizeItemDlg.StringsLocalized -= HandleStringsLocalized;

				if (components != null)
					components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStringsLocalized()
		{
			// Just to be safe and prevent re-entrant call.
			_projectPages.RowEnter -= _projectPages_RowEnter;

			string[] viewNames = {
				LocalizationManager.GetString("ProjectView.AboutProjectViewTitle", "About This Project"),
				LocalizationManager.GetString("ProjectView.AccessProtocolViewTitle", "Access Protocol"),
				LocalizationManager.GetString("ProjectView.DescriptionDocumentsTitle", "Description Documents"),
				LocalizationManager.GetString("ProjectView.OtherDocumentsTitle", "Other Documents"),
				LocalizationManager.GetString("ProjectView.ProgressViewTitle", "Progress")
			};

			if (_projectPages.RowCount != viewNames.Length)
			{
				_projectPages.RowCount = 0;
				foreach (string viewName in viewNames)
					_projectPages.AddRow(new object[] {viewName});
			}
			else
			{
				for (int i = 0; i < viewNames.Length; i++)
					_projectPages.Rows[i].Cells[0].Value = viewNames[i];
			}

			_projectPages.RowEnter += _projectPages_RowEnter;
		}

		#region ISayMoreView Members
		/// ------------------------------------------------------------------------------------
		public void AddTabToTabGroup(ViewTabGroup viewTabGroup)
		{
			var tab = viewTabGroup.AddTab(this);
			tab.Text = LocalizationManager.GetString("ProjectView.TabText", "Project", null, "Project View", null, tab);
			Text = tab.Text;
		}

		/// ------------------------------------------------------------------------------------
		public void ViewActivated(bool firstTime)
		{
			Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		public void ViewDeactivated()
		{
			_accessView.Save();

			// set the access code choices for sessions
			foreach (var editor in Program.GetControlsOfType<SessionBasicEditor>(Program.ProjectWindow))
				editor.SetAccessProtocol();

			Enabled = false;
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ToolStripMenuItem> GetMainMenuCommands()
		{
			return null;
		}

		/// ------------------------------------------------------------------------------------
		public bool IsOKToLeaveView(bool showMsgWhenNotOK)
		{
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public Image Image
		{
			get { return Resources.project; }
		}

// ReSharper disable once UnusedAutoPropertyAccessor.Local
		public ToolStripMenuItem MainMenuItem { get; private set; }
		#endregion

		/// ------------------------------------------------------------------------------------
		private void _projectPages_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			switch (e.RowIndex)
			{
				case 0:
					ShowControl(_metadataView);
					break;

				case 1:
					ShowControl(_accessView);
					break;

				case 2:
					ShowControl(_descriptionDocsView);
					break;

				case 3:
					ShowControl(_otherDocsView);
					break;

				case 4:
					ShowControl(_progressView);
					_progressView.ViewActivated(!_statsViewActivated);
					_statsViewActivated = true;
					break;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ShowControl(Control control)
		{
			while (_splitter.Panel2.Controls.Count > 0)
				_splitter.Panel2.Controls.RemoveAt(0);

			_splitter.Panel2.Controls.Add(control);
			control.Dock = DockStyle.Fill;
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			_metadataView.Save();
			_accessView.Save();
		}
	}
}
