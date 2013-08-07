
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
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._labelOralTranslation = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// _labelOralTranslation
			//
			this._labelOralTranslation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOralTranslation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelOralTranslation, null);
			this.locExtender.SetLocalizationComment(this._labelOralTranslation, null);
			this.locExtender.SetLocalizingId(this._labelOralTranslation, "DialogBoxes.Transcription.OralTranslationRecorderDlg._labelOralTranslation");
			this._labelOralTranslation.Location = new System.Drawing.Point(0, 0);
			this._labelOralTranslation.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._labelOralTranslation.Name = "_labelOralTranslation";
			this._labelOralTranslation.Size = new System.Drawing.Size(81, 13);
			this._labelOralTranslation.TabIndex = 10;
			this._labelOralTranslation.Text = "Oral Translation";
			this._labelOralTranslation.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			//
			// OralTranslationRecorderDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 362);
			this.Controls.Add(this._labelOralTranslation);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.OralTranslationRecorderDlg.WindowTitle");
			this.Name = "OralTranslationRecorderDlg";
			this.Text = "Oral Translation Recorder";
			this.Controls.SetChildIndex(this._labelOralTranslation, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.Label _labelOralTranslation;
	}
}
