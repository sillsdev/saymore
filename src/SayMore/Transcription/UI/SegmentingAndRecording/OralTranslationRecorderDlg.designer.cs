using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class OralTranslationRecorderDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
				components.Dispose();

			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// OralTranslationRecorderDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 338);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.OralTranslationRecorderDlg.WindowTitle");
			this.Name = "OralTranslationRecorderDlg";
			this.Text = "Oral Translation Recorder";
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Localization.UI.LocalizationExtender locExtender;
	}
}
