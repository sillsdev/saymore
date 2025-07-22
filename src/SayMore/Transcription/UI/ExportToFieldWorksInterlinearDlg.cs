using L10NSharp;
using L10NSharp.UI;
using L10NSharp.XLiffUtils;
using SayMore.Properties;
using SIL.Reporting;
using SIL.WritingSystems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static SayMore.Utilities.FileSystemUtils;
using static System.String;

namespace SayMore.Transcription.UI
{
	public partial class ExportToFieldWorksInterlinearDlg : Form
	{
		private const string kFlexTextExt = ".flextext";
		private const string kFlexProgramName = "FLEx";

		#region DisplayFriendlyWritingSystem class
		/// ------------------------------------------------------------------------------------
		public class DisplayFriendlyWritingSystem
		{
			public string Id;
			public string Name;

			public override string ToString() => $"{Name} ({Id})";
		}

		#endregion

		private string _intructionsFmt;
		
		public string FileName { get; private set; }
		public DisplayFriendlyWritingSystem TranscriptionWs { get; private set; }
		public DisplayFriendlyWritingSystem FreeTranslationWs { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ExportToFieldWorksInterlinearDlg(string defaultExportFileName) : this()
		{
			FileName = defaultExportFileName + kFlexTextExt;
		}

		/// ------------------------------------------------------------------------------------
		public ExportToFieldWorksInterlinearDlg()
		{
			Logger.WriteEvent("ExportToFieldWorksInterlinearDlg constructor");
			InitializeComponent();

			_labelTranscriptionColumnHeadingText.Text =
				Format(_labelTranscriptionColumnHeadingText.Text, CommonUIStrings.TranscriptionTierDisplayName);

			_labelFreeTranslationColumnHeadingText.Text =
				Format(_labelFreeTranslationColumnHeadingText.Text, CommonUIStrings.TranslationTierDisplayName);

			_labelOverview.Font = Program.DialogFont;
			_labelTranscriptionColumnHeadingText.Font = Program.DialogFont;
			_labelFreeTranslationColumnHeadingText.Font = Program.DialogFont;
			_comboTranscriptionWs.Font = Program.DialogFont;
			_comboTranslationWs.Font = Program.DialogFont;

			HandleStringsLocalized();
			LocalizeItemDlg<XLiffDocument>.StringsLocalized += HandleStringsLocalized;
		}

		/// ------------------------------------------------------------------------------------
		protected void HandleStringsLocalized(ILocalizationManager lm = null)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
			{
				_intructionsFmt = _labelImportInstructions.Text;
				FormatImportInstructions();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void FormatImportInstructions()
		{
			if (_comboTranslationWs.SelectedIndex >= 0)
			{
				_labelImportInstructions.Text = Format(_intructionsFmt, kFlexProgramName,
					(DisplayFriendlyWritingSystem)_comboTranslationWs.SelectedItem);
			}
		}

		/// ------------------------------------------------------------------------------------
		private static IEnumerable<WritingSystemDefinition> GetAvailableWritingSystems()
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
					"In order to export, we need to find a writing system ID that {1} will accept. {2} " +
					"tried to find a list of writing systems which {1} knows about by looking in {0}, but it " +
					"doesn't exist. We recommend that you let the code be 'en' (English), then change it inside of {1}.",
					"Param 0: folder path; Param 1: \"FLEx\" (product name); Param 2: \"SayMore\" (product name)");

				ErrorReport.NotifyUserOfProblem(msg, globalPath, kFlexProgramName, Program.ProductName);
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
					"We recommend that you let the code be '{1}' (English), then change it inside of {2}.",
					"Param 0: error message; Param 1: \"en\" (writing system locale); Param 2: \"FLEx\" (product name)");

				ErrorReport.NotifyUserOfProblem(msg, ex.Message, kFlexProgramName);
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
		/// <summary>Select the desired writing system in the given combo.</summary>
		/// <param name="combo">The writing system combo box (whose items are expected to be of
		/// type <see cref="DisplayFriendlyWritingSystem"/>.</param>
		/// <param name="initialWss">An array of BCP-47 writing system locale identifiers. If more
		/// than one is provided, they should be given in order of descending preference; the first
		/// one that corresponds to an existing writing system in the combo box will be selected.
		/// </param>
		/// ------------------------------------------------------------------------------------
		private static void InitializeWritingSystemCombo(ComboBox combo, params string[] initialWss)
		{
			if (initialWss != null)
			{
				foreach (var initialWs in initialWss)
				{
					var itemToSelect = combo.Items.Cast<DisplayFriendlyWritingSystem>()
						.FirstOrDefault(w => w.Id == initialWs);
					if (itemToSelect != null)
					{
						combo.SelectedItem = itemToSelect;
						return;
					}
				}
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

				var flexInterlinearFilesDesc = LocalizationManager.GetString(
					"DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.ExportSaveFileDlg.InterlinearFilesDesc",
					"FLEx Interlinear ({0})", "Parameter is a file-matching pattern: \"*.flextext\"");

				dlg.Filter = Format("{0}|{1}|{2}|{3}",
					Format(flexInterlinearFilesDesc, "*" + kFlexTextExt),
					"*" + kFlexTextExt,
					Format(LocalizedVersionOfAllFilesDescriptor, kAllFilesFilter),
					kAllFilesFilter);

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
			_buttonExport.Enabled = TranscriptionWs != null && FreeTranslationWs != null;
		}

		/// ------------------------------------------------------------------------------------
		private void ExportToFieldWorksInterlinearDlg_Load(object sender, EventArgs e)
		{
			var wsList = GetAvailableWritingSystems()
				.Select(ws => new DisplayFriendlyWritingSystem { Id = ws.Id, Name = ws.Language.Name }).ToArray();

			if (wsList.Length == 0)
			{
				var msg = LocalizationManager.GetString(
					"DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.CannotFindFLExWritingSystemsMsg2",
					"{0} was unable to find any Writing Systems on this computer. Make sure {1} " +
					"version {2} or greater is installed and has been run at least once. " +
					"For now, you can export as English, and fix that up after you have " +
					"imported into {1}.",
					"Param 0: \"SayMore\" (product name); " +
					"Param 1: \"FLEx\" (product name); " +
					"Param 2: minimum supported FLEx version number");

				ErrorReport.NotifyUserOfProblem(msg, Program.ProductName, kFlexProgramName, "7.1");
				wsList = new DisplayFriendlyWritingSystem[1];
				wsList[0] = new DisplayFriendlyWritingSystem { Id = "en", Name = "English" };
			}

			_comboTranscriptionWs.Items.AddRange(wsList);
			_comboTranslationWs.Items.AddRange(wsList);

			InitializeWritingSystemCombo(_comboTranscriptionWs,
				Settings.Default.TranscriptionWsForFWInterlinearExport);

			string[] wss;
			if (IsNullOrEmpty(Settings.Default.FreeTranslationWsForFWInterlinearExport))
			{
				var analysisCode = Program.CurrentProject.AnalysisISO3CodeAndName?.Split(':')[0];
				wss = analysisCode != null ? new[] { analysisCode, "en" } : new[] { "en" };
			}
			else
			{
				wss = new[] { Settings.Default.FreeTranslationWsForFWInterlinearExport, "en" };
			}

			InitializeWritingSystemCombo(_comboTranslationWs, wss);

			HandleWritingSystemChanged(null, null);
		}

		/// ------------------------------------------------------------------------------------
		private void _comboTranslationWs_SelectedIndexChanged(object sender, EventArgs e)
		{
			FormatImportInstructions();
		}
	}
}
