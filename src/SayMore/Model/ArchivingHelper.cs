using System;
using System.IO;
using System.Windows.Forms;
using L10NSharp;
using Palaso.Reporting;
using SIL.Archiving;
using SIL.Archiving.IMDI;
using SayMore.Properties;

namespace SayMore.Model
{
	static class ArchivingHelper
	{
		/// ------------------------------------------------------------------------------------
		internal static void ArchiveUsingIMDI(IIMDIArchivable element)
		{
			string destFolder;
			using (var chooseFolder = new FolderBrowserDialog())
			{
				chooseFolder.Description = LocalizationManager.GetString(
					"DialogBoxes.ArchivingDlg.ArchivingIMDILocationDescription",
					"Select a base folder where the IMDI directory structure should be created.");
				chooseFolder.ShowNewFolderButton = true;
				chooseFolder.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				if (chooseFolder.ShowDialog() == DialogResult.Cancel)
					return;
				destFolder = chooseFolder.SelectedPath;
			}

			var model = new IMDIArchivingDlgViewModel(Application.ProductName, element.Title, element.Id,
				element.ArchiveInfoDetails, element is Project, element.SetFilesToArchive, destFolder);

			model.HandleNonFatalError = (exception, s) => ErrorReport.NotifyUserOfProblem(exception, s);

			model.PathToProgramToLaunch = Settings.Default.ProgramToLaunchForIMDIPackage = GetProgramToLaunchForIMDIPackage();

			element.InitializeModel(model);

			using (var dlg = new ArchivingDlg(model, ApplicationContainer.kSayMoreLocalizationId,
				Program.DialogFont, Settings.Default.ArchivingDialog))
			{
				dlg.ShowDialog();
				Settings.Default.ArchivingDialog = dlg.FormSettings;
			}
		}

		/// ------------------------------------------------------------------------------------
		private static string GetProgramToLaunchForIMDIPackage()
		{
			string defaultProgram = Settings.Default.ProgramToLaunchForIMDIPackage;
			if (!string.IsNullOrEmpty(defaultProgram))
			{
				if (File.Exists(defaultProgram))
					return defaultProgram;

				if (!Path.IsPathRooted(defaultProgram))
				{
					string rootedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), defaultProgram);
					if (File.Exists(rootedPath))
						return rootedPath;
					rootedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), defaultProgram);
					if (File.Exists(rootedPath))
						return rootedPath;
				}
			}

			using (var chooseIMDIProgram = new OpenFileDialog())
			{
				chooseIMDIProgram.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
				chooseIMDIProgram.RestoreDirectory = true;
				chooseIMDIProgram.CheckFileExists = true;
				chooseIMDIProgram.CheckPathExists = true;
				chooseIMDIProgram.Filter = string.Format("{0} ({1})|{1}|{2} ({3})|{3}",
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ProgramsFileTypeLabel", "Programs"),
					"*.exe;*.pif;*.com;*.bat;*.cmd",
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.AllFilesLabel", "All Files"),
					"*.*");
				chooseIMDIProgram.FilterIndex = 0;
				chooseIMDIProgram.Multiselect = false;
				chooseIMDIProgram.Title = LocalizationManager.GetString(
					"DialogBoxes.ArchivingDlg.SelectIMDIProgram", "Select the program to launch after IMDI package is created");
				chooseIMDIProgram.ValidateNames = true;
				if (chooseIMDIProgram.ShowDialog() == DialogResult.OK && File.Exists(chooseIMDIProgram.FileName))
					return chooseIMDIProgram.FileName;
			}

			return string.Empty;
		}

		/// ------------------------------------------------------------------------------------
		static internal bool IncludeFileInArchive(string path, Type typeOfArchive, string metadataFileExtension)
		{
			var ext = Path.GetExtension(path).ToLower();
			bool imdi = typeof(IMDIArchivingDlgViewModel).IsAssignableFrom(typeOfArchive);
			return (ext != ".pfsx" && (!imdi || (ext != metadataFileExtension)));
		}
	}
}
