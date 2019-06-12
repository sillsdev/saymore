using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;
using SIL.WritingSystems;
using SayMore.Properties;

namespace SayMore.Transcription.UI
{
	public partial class ExportToFieldWorksInterlinearDlg : Form
	{
		#region DisplayFriendlyWritingSystem class
		/// ------------------------------------------------------------------------------------
		public class DisplayFriendlyWritingSystem
		{
			public string Id;
			public string Name;

			public override string ToString()
			{
				return string.Format("{0} ({1})", Name, Id);
			}
		}

		#endregion

		public string FileName { get; private set; }
		public DisplayFriendlyWritingSystem TranscriptionWs { get; private set; }
		public DisplayFriendlyWritingSystem FreeTranslationWs { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ExportToFieldWorksInterlinearDlg(string defaultExportFileName) : this()
		{
			FileName = defaultExportFileName + ".flextext";
		}

		/// ------------------------------------------------------------------------------------
		public ExportToFieldWorksInterlinearDlg()
		{
			Logger.WriteEvent("ExportToFieldWorksInterlinearDlg constructor");
			InitializeComponent();

			_labelTranscriptionColumnHeadingText.Text =
				string.Format(_labelTranscriptionColumnHeadingText.Text, LocalizationManager.GetString("SessionsView.Transcription.TierDisplayNames.Transcription", "Transcription"));

			_labelFreeTranslationColumnHeadingText.Text =
				string.Format(_labelFreeTranslationColumnHeadingText.Text, LocalizationManager.GetString("SessionsView.Transcription.TierDisplayNames.FreeTranslation", "Free Translation"));

			_labelOverview.Font = Program.DialogFont;
			_labelTranscriptionColumnHeadingText.Font = Program.DialogFont;
			_labelFreeTranslationColumnHeadingText.Font = Program.DialogFont;
			_comboTranscriptionWs.Font = Program.DialogFont;
			_comboTranslationWs.Font = Program.DialogFont;

		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<WritingSystemDefinition> GetAvailableWritingSystems()
		{
			// FLEx 9 uses C:\ProgramData\SIL\WritingSystemRepository
			var repoPath = Path.Combine(Program.SilCommonDataFolder, "WritingSystemRepository");
			if (Directory.Exists(repoPath))
			{
				var globalRepo = GlobalWritingSystemRepository.Initialize();
				return globalRepo.AllWritingSystems;
			}

			var globalPath = Path.Combine(Program.SilCommonDataFolder, "WritingSystemStore");

			if (!Directory.Exists(globalPath))
			{
				var msg = LocalizationManager.GetString(
					"DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.CannotFindFLExWritingSystemsMsg1",
					"In order to export, we need to find a writing system ID that FLEx will accept. SayMore " +
					"tried to find a list of writing systems which FLEx knows about by looking in {0}, but it " +
					"doesn't exist. We recommend that you let the code be 'en' (English), then change it inside of FLEx.",
					"The parameter is a folder path");

				ErrorReport.NotifyUserOfProblem(msg, globalPath);
				return new[] {new WritingSystemDefinition("en")};
			}

			try
			{
				var repo = LdmlInFolderWritingSystemRepository.Initialize(globalPath);
				return repo.AllWritingSystems;
			}
			catch (Exception ex)
			{
				var msg = LocalizationManager.GetString(
					"DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.FLExWritingSystemRepositoryMsg",
					"There was a problem initializing the Writing System Repository: \"{0}\"." +
					"We recommend that you let the code be 'en' (English), then change it inside of FLEx.",
					"The parameter is an error message");

				ErrorReport.NotifyUserOfProblem(msg, ex.Message);
				return new[] {new WritingSystemDefinition("en")};
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void IntializeWritingSystemCombo(ComboBox combo, string initialWs)
		{
			if (initialWs != null)
			{
				combo.SelectedItem =
					combo.Items.Cast<DisplayFriendlyWritingSystem>().FirstOrDefault(w => w.Id == initialWs);
			}

			if (combo.SelectedItem == null)
				combo.SelectedItem = combo.Items[0];
		}

		/// ------------------------------------------------------------------------------------
		private void HandleExportButtonClick(object sender, EventArgs e)
		{
			var folder = TextAnnotationEditor.GetDefaultExportFolder("LastFlexInterlinearExportDestinationFolder");
			using (var dlg = new SaveFileDialog())
			{
				dlg.Title = LocalizationManager.GetString(
					"DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.ExportSaveFileDlg.Caption",
					"Export to File");

				dlg.Filter = LocalizationManager.GetString(
					"DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.ExportSaveFileDlg.FileTypeString",
					"FLEx Interlinear (*.flextext)|*.flextext|All Files (*.*)|*.*");

				dlg.FileName = FileName;
				dlg.OverwritePrompt = true;
				dlg.CheckPathExists = true;
				dlg.AutoUpgradeEnabled = true;
				dlg.RestoreDirectory = true;
				dlg.InitialDirectory = folder ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					FileName = dlg.FileName;
					Settings.Default.LastFlexInterlinearExportDestinationFolder = Path.GetDirectoryName(FileName);
					DialogResult = DialogResult.OK;
					Close();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWritingSystemChanged(object sender, EventArgs e)
		{
			if (_comboTranscriptionWs.SelectedItem == null || _comboTranslationWs.SelectedItem == null)
			{
				UpdateDisplay();
				return;
			}

			TranscriptionWs = _comboTranscriptionWs.SelectedItem as DisplayFriendlyWritingSystem;
			FreeTranslationWs = _comboTranslationWs.SelectedItem as DisplayFriendlyWritingSystem;

			if (TranscriptionWs != null) Settings.Default.TranscriptionWsForFWInterlinearExport = TranscriptionWs.Id;
			if (FreeTranslationWs != null) Settings.Default.FreeTranslationWsForFWInterlinearExport = FreeTranslationWs.Id;

			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_buttonExport.Enabled = (TranscriptionWs != null && FreeTranslationWs != null);
		}

		private void ExportToFieldWorksInterlinearDlg_Load(object sender, EventArgs e)
		{
			var wsList = GetAvailableWritingSystems()
				.Select(ws => new DisplayFriendlyWritingSystem { Id = ws.Id, Name = ws.Language.Name }).ToArray();

			if (wsList.Length == 0)
			{
				var msg = LocalizationManager.GetString("DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.CannotFindFLExWritingSystemsMsg2",
					"SayMore was unable to find any Writing Systems on this computer. Make sure FLEx version 7.1 or greater is " +
					"installed and has been run at least once. For now, you can export as English, and fix that up after you " +
					"have imported into FLEx.");

				ErrorReport.NotifyUserOfProblem(msg);
				wsList = new DisplayFriendlyWritingSystem[1];
				wsList[0] = new DisplayFriendlyWritingSystem { Id = "en", Name = "English" };
			}

			_comboTranscriptionWs.Items.AddRange(wsList);
			_comboTranslationWs.Items.AddRange(wsList);

			IntializeWritingSystemCombo(_comboTranscriptionWs,
				Settings.Default.TranscriptionWsForFWInterlinearExport);

			IntializeWritingSystemCombo(_comboTranslationWs,
				string.IsNullOrEmpty(Settings.Default.FreeTranslationWsForFWInterlinearExport) ? "en" :
				Settings.Default.FreeTranslationWsForFWInterlinearExport);

			HandleWritingSystemChanged(null, null);
		}
	}
}
