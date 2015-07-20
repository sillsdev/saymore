using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;
using SIL.Windows.Forms.Miscellaneous;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.Overview
{
	public abstract partial class ProjectDocsScreen : EditorBase, ISayMoreView
	{
		protected abstract string FolderName { get; }
		protected abstract string ArchiveSessionName { get; }

		private readonly ImageList _tabControlImages = new ImageList();
		private ComponentEditorsTabControl _tabCtrl;
		protected string _toolTipText;

		protected ProjectDocsScreen()
		{
			Logger.WriteEvent("ProjectDocsScreen constructor");

			InitializeComponent();

			InitializeFileGrid();

			_splitter.SplitterDistance = (int) (_splitter.Height*0.4);
		}

		#region ISayMoreView Members

		public void AddTabToTabGroup(ViewTabGroup viewTabGroup)
		{
			throw new NotImplementedException();
		}

		public void ViewActivated(bool firstTime)
		{
			throw new NotImplementedException();
		}

		public void ViewDeactivated()
		{
			throw new NotImplementedException();
		}

		public bool IsOKToLeaveView(bool showMsgWhenNotOK)
		{
			throw new NotImplementedException();
		}

		public Image Image
		{
			get { throw new NotImplementedException(); }
		}

		public ToolStripMenuItem MainMenuItem
		{
			get { throw new NotImplementedException(); }
		}

		public string NameForUsageReporting
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		private void InitializeFileGrid()
		{
			_descriptionFileGrid.AfterComponentSelectionChanged = HandleAfterComponentFileSelected;
			_descriptionFileGrid.FilesAdded = HandleFilesAddedToComponentGrid;
			_descriptionFileGrid.FileDeletionAction = file => ComponentFile.MoveToRecycleBin(file, true);
			_descriptionFileGrid.FilesBeingDraggedOverGrid = HandleFilesBeingDraggedOverComponentGrid;
			_descriptionFileGrid.FilesDroppedOnGrid = HandleFilesAddedToComponentGrid;
			_descriptionFileGrid.PostMenuCommandRefreshAction = file => _descriptionFileGrid.UpdateComponentFileList(GetFiles());
			_descriptionFileGrid.IsOKToSelectDifferentFile = GetIsOKToLeaveCurrentEditor;
			_descriptionFileGrid.IsOKToDoFileOperation = GetIsOKToLeaveCurrentEditor;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			// HandleStringsLocalized gets called from the constructor (in a base class), but it's too
			// early for the classes that derive from this class to do someof their localization, so
			// we have a special method to handle that.
			LocalizeStrings();
		}

		protected override void HandleStringsLocalized()
		{
			base.HandleStringsLocalized();
			if (_descriptionFileGrid != null)
				LocalizeStrings();
		}

		protected abstract void LocalizeStrings();

		private bool HandleFilesAddedToComponentGrid(string[] files)
		{
			if (files.Length == 0) return false;

			// make sure the directory exists
			var dir = Path.Combine(Program.CurrentProject.FolderPath, FolderName);
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

			// copy the selected files to the directory
			foreach (var file in files)
			{
				var fileName = (new FileInfo(file)).Name;
				var newFile = Path.Combine(dir, fileName);

				// make sure the files don't already exist
				if (File.Exists(newFile))
				{
					var txt = LocalizationManager.GetString("CommonToMultipleViews.FileList.FileAlreadyExistsMessage",
						"There is already a file named {0} in this project. Do you want to replace the old one with this one?");

					var msg = string.Format(txt, fileName);
					var result = MessageBox.Show(Program.ProjectWindow, msg, Program.ProjectWindow.Text, MessageBoxButtons.YesNo);
					if (result != DialogResult.Yes) continue;
				}

				// copy the file
				File.Copy(file, newFile, true);
			}

			_descriptionFileGrid.UpdateComponentFileList(GetFiles());

			return true;
		}

		private IEnumerable<ComponentFile> GetFiles()
		{
			var dir = Path.Combine(Program.CurrentProject.FolderPath, FolderName);
			if (!Directory.Exists(dir)) yield break;

			var unknownFileType = new FileType[]
			{new UnknownFileType(null, null), new AudioFileType(null, null, null), new VideoFileType(null, null, null), new ImageFileType(null, null) };
			var blankRoles = new ComponentRole[] {};
			var blankSerializer = new XmlFileSerializer(null);

			var files = Directory.GetFiles(dir);
			foreach (var file in files.Where(f => !f.EndsWith(Settings.Default.MetadataFileExtension)))
				yield return new ComponentFile(null, file, unknownFileType, blankRoles, blankSerializer, null, null, null);
		}

		private void ProjectDescriptionDocsScreen_Load(object sender, EventArgs e)
		{
			_descriptionFileGrid.UpdateComponentFileList(GetFiles());
			_descriptionFileGrid.HideDuration = true;

			_tabControlImages.ColorDepth = ColorDepth.Depth32Bit;
			_tabControlImages.ImageSize = ResourceImageCache.PlayTabImage.Size;
			_tabControlImages.Images.Add("Notes", ResourceImageCache.NotesTabImage);
		}

		private void HandleAfterComponentFileSelected(int index)
		{
			WaitCursor.Show();

			SuspendLayout();

			try
			{
				_splitter.Panel2.Controls.Clear();

				if (_tabCtrl != null)
				{
					_tabCtrl.Dispose();
					_tabCtrl = null;
				}

				if (index < 0)
					return;

				var file = _descriptionFileGrid.GetFileAt(index);
				List<IEditorProvider> providers = new List<IEditorProvider>();

				if ((file.FileType is AudioFileType) || (file.FileType is VideoFileType))
					providers.Add(new AudioVideoPlayer(file, null));
				else if (file.FileType is ImageFileType)
					providers.Add(new ImageViewer(file));
				else
					providers.Add(new BrowserEditor(file, null));

				providers.Add(new NotesEditor(file));
				_tabCtrl = new ComponentEditorsTabControl("UnknownFileType", _tabControlImages, providers,
					BackColor, SystemColors.ControlDark);
				_tabCtrl.MakeAppropriateEditorsVisible();

				_splitter.Panel2.Controls.Add(_tabCtrl);
				_tabCtrl.Dock = DockStyle.Fill;
				_tabCtrl.Visible = true;
			}
			finally
			{
				ResumeLayout();
				WaitCursor.Hide();
			}
		}

		private DragDropEffects HandleFilesBeingDraggedOverComponentGrid(string[] files)
		{
			return DragDropEffects.Copy;
		}

		protected virtual bool GetIsOKToLeaveCurrentEditor()
		{
			if ((_tabCtrl == null) || (_tabCtrl.CurrentEditor == null)) return true;

			return _tabCtrl.CurrentEditor.IsOKToLeaveEditor;
		}

		private void _linkHowArchived_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string howTheseAreArchivedMsg = string.Format(
				LocalizationManager.GetString("ProjectView.ProjectDocuments.HowArchivedMsg",
					"In an IMDI project archive, these files will be exported in a special session named {0}"),
				ArchiveSessionName);

			MessageBox.Show(Program.ProjectWindow, howTheseAreArchivedMsg, Program.ProjectWindow.Text);
		}
	}

	public class ProjectDescriptionDocsScreen : ProjectDocsScreen
	{
		internal static string kFolderName = "DescriptionDocuments";
		internal static string kArchiveSessionName = "Project Description Documents";

		public ProjectDescriptionDocsScreen()
		{
			_descriptionFileGrid.InitializeGrid("ProjectDescriptionDocuments");
		}

		protected override string FolderName
		{
			get { return kFolderName; }
		}

		protected override string ArchiveSessionName
		{
			get { return kArchiveSessionName; }
		}

		protected override void LocalizeStrings()
		{
			_descriptionFileGrid.AddFileButtonTooltipText =
				LocalizationManager.GetString("ProjectView.ProjectDocuments.AddDescriptionFileToolTip",
					"Add Description Documents to the Project");

			_labelInformation.Text =
				LocalizationManager.GetString("ProjectView.ProjectDocuments.DescriptionDocumentsInformationLabel",
					"Add documents here that describe the project and corpus.");
		}
	}

	public class ProjectOtherDocsScreen : ProjectDocsScreen
	{
		internal static string kFolderName = "OtherDocuments";
		internal static string kArchiveSessionName = "Other Project Documents";

		public ProjectOtherDocsScreen()
		{
			_descriptionFileGrid.InitializeGrid("ProjectOtherDocuments");
		}

		protected override string FolderName
		{
			get { return kFolderName; }
		}

		protected override string ArchiveSessionName
		{
			get { return kArchiveSessionName; }
		}

		protected override void LocalizeStrings()
		{
			_descriptionFileGrid.AddFileButtonTooltipText = LocalizationManager.GetString("ProjectView.ProjectDocuments.AddOtherFileToolTip",
				"Add Other Documents to the Project");

			_labelInformation.Text = LocalizationManager.GetString("ProjectView.ProjectDocuments.OtherDocumentsInformationLabel",
				"Add documents here that don't seem to fit anywhere else, for example about how the project was funded.");
		}
	}
}
