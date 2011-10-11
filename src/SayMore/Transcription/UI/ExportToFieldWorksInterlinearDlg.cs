using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Palaso.Reporting;
using Palaso.WritingSystems;
using SayMore.Properties;
using SayMore.Transcription.Model;
using Palaso.Extensions;

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
			FileName = defaultExportFileName + ".xml";
		}

		/// ------------------------------------------------------------------------------------
		public ExportToFieldWorksInterlinearDlg()
		{
			InitializeComponent();

			_labelTranscriptionColumnHeadingText.Text =
				string.Format(_labelTranscriptionColumnHeadingText.Text, TextTier.TranscriptionTierName);

			_labelFreeTranslationColumnHeadingText.Text =
				string.Format(_labelFreeTranslationColumnHeadingText.Text, TextTier.SayMoreFreeTranslationTierName);

			_labelOverview.Font = SystemFonts.IconTitleFont;
			_labelTranscriptionColumnHeadingText.Font = SystemFonts.IconTitleFont;
			_labelFreeTranslationColumnHeadingText.Font = SystemFonts.IconTitleFont;
			_comboTranscriptionWs.Font = SystemFonts.IconTitleFont;
			_comboTranslationWs.Font = SystemFonts.IconTitleFont;

		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<WritingSystemDefinition> GetAvailableWritingSystems()
		{
			//TODO: someday, this may be safe to call. But not yet.
			//return (new LdmlInFolderWritingSystemRepository()).AllWritingSystems;

			var globalPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData).CombineForPath(
				"SIL", "WritingSystemStore"); //NB: flex 7.1 is using this.  Palaso head has WritingSystemRepository/2 instead. Sigh...

			if (!Directory.Exists(globalPath))
			{
				var msg = LocalizationManager.GetString(
					"DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.CannotFindFLExWritingSystemsMsg1",
					"In order to export, we need to find a writing system ID that FLEx will accept. SayMore " +
					"tried to find a list of writing systems which FLEx knows about by looking in {0}, but it " +
					"doesn't exist. We recommend that you let the code be 'en' (English), then change it inside of FLEx.",
					"The parameter is a folder path");

				ErrorReport.NotifyUserOfProblem(msg, globalPath);
				yield return WritingSystemDefinition.Parse("en");
			}
			else
			{
				foreach (string path in Directory.GetFiles(globalPath, "*.ldml"))
				{
					var name = Path.GetFileNameWithoutExtension(path);
					WritingSystemDefinition x=null;
					try
					{
						x = WritingSystemDefinition.Parse(name);
					}
					catch (Exception e)
					{
						var msg = LocalizationManager.GetString(
							"DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.OldWritingSystemsFoundMsg",
							"Sorry, the writing system {0} does not conform to current standards. Please first upgrade to FLEx 7.1 or greater.");

						ErrorReport.NotifyUserOfProblem(e, msg, name);
					}

					if (x != null)
						yield return x;
				}
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
			using (var dlg = new SaveFileDialog())
			{
				dlg.Title = LocalizationManager.GetString(
					"DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.ExportSaveFileDlg.Caption",
					"Export to File");

				dlg.Filter = LocalizationManager.GetString(
					"DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.ExportSaveFileDlg.FileTypeString",
					"FLEx Interlinear XML (*.xml)|*.xml|All Files (*.*)|*.*");

				dlg.FileName = FileName;
				dlg.OverwritePrompt = true;
				dlg.CheckPathExists = true;

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					FileName = dlg.FileName;
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

			Settings.Default.TranscriptionWsForFWInterlinearExport = TranscriptionWs.Id;
			Settings.Default.FreeTranslationWsForFWInterlinearExport = FreeTranslationWs.Id;

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
				.Select(ws => new DisplayFriendlyWritingSystem { Id = ws.Id, Name = ws.LanguageName }).ToArray();

			if (wsList.Length == 0)
			{
				var msg = LocalizationManager.GetString("DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.CannotFindFLExWritingSystemsMsg2",
					"SayMore was unable to find any Writing Systems on this computer. Make sure FLEx version 7.1 or greater is " +
					"installed as has been run at least once. For now, you can export as English, and fix that up after you " +
					"have imported into FLEx.");

				ErrorReport.NotifyUserOfProblem(msg);
				wsList = new DisplayFriendlyWritingSystem[1];
				wsList[0] = new DisplayFriendlyWritingSystem {Id = "en", Name = "English" };
			}
			_comboTranscriptionWs.Items.AddRange(wsList);
			_comboTranslationWs.Items.AddRange(wsList);


			IntializeWritingSystemCombo(_comboTranscriptionWs,
										Settings.Default.TranscriptionWsForFWInterlinearExport);

			IntializeWritingSystemCombo(_comboTranslationWs,
										string.IsNullOrEmpty(Settings.Default.FreeTranslationWsForFWInterlinearExport)
											? "en"
											: Settings.Default.FreeTranslationWsForFWInterlinearExport);


			HandleWritingSystemChanged(null, null);
		}
	}
}
