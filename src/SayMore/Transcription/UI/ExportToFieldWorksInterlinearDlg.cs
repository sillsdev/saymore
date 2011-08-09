using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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

			_labelTranscriptionWs.Text =
				string.Format(_labelTranscriptionWs.Text, TextTier.TranscriptionTierName);

			_labelTranslationWs.Text =
				string.Format(_labelTranslationWs.Text, TextTier.SayMoreFreeTranslationTierName);

			_labelOverview.Font = SystemFonts.IconTitleFont;
			_labelTranscriptionWs.Font = SystemFonts.IconTitleFont;
			_labelTranslationWs.Font = SystemFonts.IconTitleFont;
			_comboTranscriptionWs.Font = SystemFonts.IconTitleFont;
			_comboTranslationWs.Font = SystemFonts.IconTitleFont;

			var wsList = GetAvailableWritingSystems().Select(ws =>
				new DisplayFriendlyWritingSystem { Id = ws.Id, Name = ws.LanguageName }).ToArray();

			_comboTranscriptionWs.Items.AddRange(wsList);
			_comboTranslationWs.Items.AddRange(wsList);

			if (wsList.Length > 0)
			{
				IntializeWritingSystemCombo(_comboTranscriptionWs,
					Settings.Default.TranscriptionWsForFWInterlinearExport);

				IntializeWritingSystemCombo(_comboTranslationWs,
					string.IsNullOrEmpty(Settings.Default.FreeTranslationWsForFWInterlinearExport) ?
					"en" : Settings.Default.FreeTranslationWsForFWInterlinearExport);
			}

			HandleWritingSystemChanged(null, null);
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<WritingSystemDefinition> GetAvailableWritingSystems()
		{
			//TODO: someday, this may be safe to call. But not yet.
			//return (new LdmlInFolderWritingSystemRepository()).AllWritingSystems;

			var globalPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData).CombineForPath(
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
					yield return WritingSystemDefinition.FromLanguage(name);
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
	}
}
