using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class CarefulSpeechRecorderDlg
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
			this._labelCarefulSpeech = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// _labelCarefulSpeech
			//
			this._labelCarefulSpeech.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelCarefulSpeech.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelCarefulSpeech, null);
			this.locExtender.SetLocalizationComment(this._labelCarefulSpeech, null);
			this.locExtender.SetLocalizingId(this._labelCarefulSpeech, "DialogBoxes.Transcription.CarefulSpeechRecorderDlg._labelCarefulSpeech");
			this._labelCarefulSpeech.Location = new System.Drawing.Point(-3, 6);
			this._labelCarefulSpeech.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._labelCarefulSpeech.Name = "_labelCarefulSpeech";
			this._labelCarefulSpeech.Size = new System.Drawing.Size(80, 13);
			this._labelCarefulSpeech.TabIndex = 9;
			this._labelCarefulSpeech.Text = "Careful Speech";
			this._labelCarefulSpeech.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			//
			// CarefulSpeechRecorderDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 362);
			this.Controls.Add(this._labelCarefulSpeech);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.CarefulSpeechRecorderDlg.WindowTitle");
			this.Name = "CarefulSpeechRecorderDlg";
			this.Text = "Careful Speech Recorder";
			this.Controls.SetChildIndex(this._labelCarefulSpeech, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.Label _labelCarefulSpeech;
	}
}
