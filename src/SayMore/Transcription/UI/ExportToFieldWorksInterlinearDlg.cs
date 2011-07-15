using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Palaso.WritingSystems;
using SayMore.Transcription.Model;

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

			HandleWritingSystemChanged(null, null);
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<WritingSystemDefinition> GetAvailableWritingSystems()
		{
			var x = new LdmlInFolderWritingSystemRepository().AllWritingSystems;

			// TODO: JohnH - get writing systems using Palaso.
			yield return new WritingSystemDefinition("seh", "", "", "", "Sen", false);
			yield return new WritingSystemDefinition("pt", "", "", "", "Por", false);
			yield return new WritingSystemDefinition("en", "", "", "", "Eng", false);


			//return (new LdmlInFolderWritingSystemRepository()).AllWritingSystems;
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
