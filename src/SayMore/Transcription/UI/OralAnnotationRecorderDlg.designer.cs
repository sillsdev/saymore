using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class OralAnnotationRecorderDlg
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
			this._buttonRecordAnnotation = new System.Windows.Forms.ToolStripButton();
			this._buttonListenToAnnotation = new System.Windows.Forms.ToolStripButton();
			this._buttonEraseAnnotation = new System.Windows.Forms.ToolStripButton();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			//
			// _waveControl
			//
			this._waveControl.AutoScrollMinSize = new System.Drawing.Size(0, 99);
			this.locExtender.SetLocalizableToolTip(this._waveControl, null);
			this.locExtender.SetLocalizationComment(this._waveControl, null);
			this.locExtender.SetLocalizingId(this._waveControl, "ManualSegmenterDlg._waveControl");
			this._waveControl.Size = new System.Drawing.Size(650, 99);
			//
			// _buttonRecordAnnotation
			//
			this._buttonRecordAnnotation.BackColor = System.Drawing.Color.Transparent;
			this._buttonRecordAnnotation.Image = global::SayMore.Properties.Resources.RecordStart;
			this._buttonRecordAnnotation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonRecordAnnotation.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonRecordAnnotation.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonRecordAnnotation, null);
			this.locExtender.SetLocalizationComment(this._buttonRecordAnnotation, null);
			this.locExtender.SetLocalizingId(this._buttonRecordAnnotation, "DialogBoxes.Transcription.CarefulSpeechAnnotationDlg._buttonRecordAnnotation");
			this._buttonRecordAnnotation.Margin = new System.Windows.Forms.Padding(0, 10, 0, 2);
			this._buttonRecordAnnotation.Name = "_buttonRecordAnnotation";
			this._buttonRecordAnnotation.Size = new System.Drawing.Size(240, 24);
			this._buttonRecordAnnotation.Text = "Record (hold SPACE key down and talk)";
			this._buttonRecordAnnotation.ToolTipText = "Record oral annotation";
			this._buttonRecordAnnotation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleRecordAnnotationMouseDown);
			this._buttonRecordAnnotation.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandleRecordAnnotationMouseUp);
			//
			// _buttonListenToAnnotation
			//
			this._buttonListenToAnnotation.BackColor = System.Drawing.Color.Transparent;
			this._buttonListenToAnnotation.Image = global::SayMore.Properties.Resources.RecordingPlaybackAnnotation;
			this._buttonListenToAnnotation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonListenToAnnotation.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonListenToAnnotation.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonListenToAnnotation, null);
			this.locExtender.SetLocalizationComment(this._buttonListenToAnnotation, null);
			this.locExtender.SetLocalizingId(this._buttonListenToAnnotation, "DialogBoxes.Transcription.CarefulSpeechAnnotationDlg._buttonListenToAnnotation");
			this._buttonListenToAnnotation.Margin = new System.Windows.Forms.Padding(0, 10, 0, 2);
			this._buttonListenToAnnotation.Name = "_buttonListenToAnnotation";
			this._buttonListenToAnnotation.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this._buttonListenToAnnotation.Size = new System.Drawing.Size(127, 24);
			this._buttonListenToAnnotation.Text = "Check Annotation";
			//
			// _buttonEraseAnnotation
			//
			this._buttonEraseAnnotation.BackColor = System.Drawing.Color.Transparent;
			this._buttonEraseAnnotation.Image = global::SayMore.Properties.Resources.RecordErase;
			this._buttonEraseAnnotation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonEraseAnnotation.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonEraseAnnotation.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonEraseAnnotation, null);
			this.locExtender.SetLocalizationComment(this._buttonEraseAnnotation, null);
			this.locExtender.SetLocalizingId(this._buttonEraseAnnotation, "DialogBoxes.Transcription.CarefulSpeechAnnotationDlg._buttonEraseAnnotation");
			this._buttonEraseAnnotation.Margin = new System.Windows.Forms.Padding(24, 5, 0, 2);
			this._buttonEraseAnnotation.Name = "_buttonEraseAnnotation";
			this._buttonEraseAnnotation.Size = new System.Drawing.Size(123, 29);
			this._buttonEraseAnnotation.Text = "Erase Annotation";
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// toolStrip1
			//
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._buttonRecordAnnotation,
			this._buttonListenToAnnotation,
			this._buttonEraseAnnotation});
			this.locExtender.SetLocalizableToolTip(this.toolStrip1, null);
			this.locExtender.SetLocalizationComment(this.toolStrip1, null);
			this.locExtender.SetLocalizationPriority(this.toolStrip1, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.toolStrip1, "toolStrip1.toolStrip1");
			this.toolStrip1.Location = new System.Drawing.Point(3, 305);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(557, 36);
			this.toolStrip1.TabIndex = 7;
			this.toolStrip1.Text = "toolStrip1";
			//
			// ManualSegmenterDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(682, 338);
			this.Controls.Add(this.toolStrip1);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.WindowTitle");
			this.Name = "ManualSegmenterDlg";
			this.Text = "Careful Speech Recorder";
			this.Controls.SetChildIndex(this.toolStrip1, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		//private AudioUtils.WaveControl _waveControl;
		//private SilPanel _panelWaveControl;
		//private System.Windows.Forms.ToolStripButton _buttonListenToOriginal;
		private System.Windows.Forms.ToolStripButton _buttonRecordAnnotation;
		private System.Windows.Forms.ToolStripButton _buttonListenToAnnotation;
		private System.Windows.Forms.ToolStripButton _buttonEraseAnnotation;
		//private System.Windows.Forms.Label _labelTimeDisplay;
		//private System.Windows.Forms.Label _labelOriginalRecording;
		//private System.Windows.Forms.ComboBox _comboBoxZoom;
		//private System.Windows.Forms.Label _labelZoom;
		//private System.Windows.Forms.Button _buttonClose;
		//private System.Windows.Forms.Button _buttonCancel;
		//private System.Windows.Forms.TableLayoutPanel _tableLayoutTop;
		//private System.Windows.Forms.TableLayoutPanel _tableLayoutStatus;
		//private System.Windows.Forms.Label _labelSegment;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.ToolStrip toolStrip1;
		//private System.Windows.Forms.Label _labelSegmentCount;
		//protected System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
		//protected System.Windows.Forms.ToolStrip _toolStripButtons;
	}
}
