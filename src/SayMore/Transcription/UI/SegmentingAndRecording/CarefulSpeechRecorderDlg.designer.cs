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
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._labelCarefulSpeech = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			//
			// _waveControl
			//
			this._waveControl.AutoScrollMinSize = new System.Drawing.Size(0, 90);
			this.locExtender.SetLocalizableToolTip(this._waveControl, null);
			this.locExtender.SetLocalizationComment(this._waveControl, null);
			this.locExtender.SetLocalizationPriority(this._waveControl, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._waveControl, "CarefulSpeechRecorderDlg._waveControl");
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
			this.locExtender.SetLocalizingId(this._labelCarefulSpeech, "label1.label1");
			this._labelCarefulSpeech.Location = new System.Drawing.Point(-3, -4);
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
			this.ClientSize = new System.Drawing.Size(703, 338);
			this.Controls.Add(this._labelCarefulSpeech);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.CarefulSpeechRecorderDlg.WindowTitle");
			this.Name = "CarefulSpeechRecorderDlg";
			this.Opacity = 1D;
			this.Text = "Careful Speech Recorder";
			this.Controls.SetChildIndex(this._labelCarefulSpeech, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Label _labelCarefulSpeech;
	}
}
