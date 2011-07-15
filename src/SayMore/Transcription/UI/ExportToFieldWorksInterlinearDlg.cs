using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Palaso.WritingSystems;
using SayMore.Transcription.Model;
using Palaso.Extensions;

namespace SayMore.Transcription.UI
{
	public partial class ExportToFieldWorksInterlinearDlg : Form
	{
		public string FileName { get; private set; }
		public WritingSystemDefinition TranscriptionWs { get; private set; }
		public WritingSystemDefinition FreeTranslationWs { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ExportToFieldWorksInterlinearDlg(string defaultExportFileName) : this()
		{
			FileName = defaultExportFileName + ".xml";
		}

		/// ------------------------------------------------------------------------------------
		public ExportToFieldWorksInterlinearDlg()
		{
			InitializeComponent();

			_labelTranscriptionWs.Text =
				string.Format(_labelTranscriptionWs.Text, TextTier.TranscriptionTierName);

			_labelTranslationWs.Text =
				string.Format(_labelTranslationWs.Text, TextTier.FreeTranslationTierName);

			_labelOverview.Font = SystemFonts.IconTitleFont;
			_labelTranscriptionWs.Font = SystemFonts.IconTitleFont;
			_labelTranslationWs.Font = SystemFonts.IconTitleFont;
			_comboTranscriptionWs.Font = SystemFonts.IconTitleFont;
			_comboTranslationWs.Font = SystemFonts.IconTitleFont;

			var wsList = GetAvailableWritingSystems().ToArray();
			_comboTranscriptionWs.Items.AddRange(wsList);
			_comboTranslationWs.Items.AddRange(wsList);

			if (wsList.Length > 0)
			{
				_comboTranscriptionWs.SelectedItem = wsList[0];
				_comboTranslationWs.SelectedItem = wsList[0];
			}
			_comboTranslationWs.SelectedItem = wsList.FirstOrDefault(w => w.Id == "en");

			HandleWritingSystemChanged(null, null);
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<WritingSystemDefinition> GetAvailableWritingSystems()
		{
			//TODO: someday, this may be safe to call. But not yet.
			//return (new LdmlInFolderWritingSystemRepository()).AllWritingSystems;

			var globalPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData).CombineForPath(
				"SIL", "WritingSystemStore");
			if (!Directory.Exists(globalPath))
			{
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(
					"In order to export, we need to find a writing system ID that FLEx will accept. SayMore tried to find a list of writing systems which FLEx knows about by looking in {0}, but it doesn't exist. We recommend that you let the code be 'en' (English), then change it inside of FLEx.",
					globalPath);
				yield return WritingSystemDefinition.FromLanguage("en");
			}
			else
			{
				foreach (string path in Directory.GetFiles(globalPath, "*.ldml"))
				{
					var name = Path.GetFileNameWithoutExtension(path);
					yield return  WritingSystemDefinition.FromLanguage(name);
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
		private void HandleExportButtonClick(object sender, EventArgs e)
		{
			using (var dlg = new SaveFileDialog())
			{
				dlg.Title = "Export to File";
				dlg.FileName = FileName;
				dlg.OverwritePrompt = true;
				dlg.CheckPathExists = true;
				dlg.Filter = "FLEx Interlinear XML (*.xml)|*.xml|All Files (*.*)|*.*";

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
			TranscriptionWs = _comboTranscriptionWs.SelectedItem as WritingSystemDefinition;
			FreeTranslationWs = _comboTranslationWs.SelectedItem as WritingSystemDefinition;
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_buttonExport.Enabled = (TranscriptionWs != null && FreeTranslationWs != null);
		}
	}
}
