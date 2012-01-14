using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class OralAnnotationRecorderBaseDlg
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
			this._buttonListenToOriginal = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtons = new System.Windows.Forms.ToolStrip();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.toolStripButtons.SuspendLayout();
			this.SuspendLayout();
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
			this.locExtender.SetLocalizingId(this._buttonRecordAnnotation, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._buttonRecordAnnotation");
			this._buttonRecordAnnotation.Margin = new System.Windows.Forms.Padding(0, 10, 0, 2);
			this._buttonRecordAnnotation.Name = "_buttonRecordAnnotation";
			this._buttonRecordAnnotation.Size = new System.Drawing.Size(239, 24);
			this._buttonRecordAnnotation.Text = "Record (hold SPACE key down and talk)";
			this._buttonRecordAnnotation.ToolTipText = "Record oral annotation";
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
			this.locExtender.SetLocalizingId(this._buttonListenToAnnotation, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._buttonListenToAnnotation" +
		"");
			this._buttonListenToAnnotation.Margin = new System.Windows.Forms.Padding(0, 10, 0, 2);
			this._buttonListenToAnnotation.Name = "_buttonListenToAnnotation";
			this._buttonListenToAnnotation.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this._buttonListenToAnnotation.Size = new System.Drawing.Size(239, 24);
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
			this.locExtender.SetLocalizingId(this._buttonEraseAnnotation, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._buttonEraseAnnotation");
			this._buttonEraseAnnotation.Margin = new System.Windows.Forms.Padding(24, 5, 0, 2);
			this._buttonEraseAnnotation.Name = "_buttonEraseAnnotation";
			this._buttonEraseAnnotation.Size = new System.Drawing.Size(215, 26);
			this._buttonEraseAnnotation.Text = "Erase Annotation";
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// _buttonListenToOriginal
			//
			this._buttonListenToOriginal.BackColor = System.Drawing.Color.Transparent;
			this._buttonListenToOriginal.Image = global::SayMore.Properties.Resources.RecordingPlayback;
			this._buttonListenToOriginal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonListenToOriginal.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonListenToOriginal.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonListenToOriginal, null);
			this.locExtender.SetLocalizationComment(this._buttonListenToOriginal, null);
			this.locExtender.SetLocalizingId(this._buttonListenToOriginal, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._buttonListenToOriginal");
			this._buttonListenToOriginal.Name = "_buttonListenToOriginal";
			this._buttonListenToOriginal.Size = new System.Drawing.Size(239, 24);
			this._buttonListenToOriginal.Text = "Listen (hold CTRL key down)";
			this._buttonListenToOriginal.ToolTipText = "Listen to original recording";
			this._buttonListenToOriginal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleListenToOriginalMouseDown);
			//
			// toolStripButtons
			//
			this.toolStripButtons.BackColor = System.Drawing.Color.Transparent;
			this.toolStripButtons.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripButtons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripButtons.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._buttonListenToOriginal,
			this._buttonRecordAnnotation,
			this._buttonListenToAnnotation,
			this._buttonEraseAnnotation});
			this.toolStripButtons.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this.locExtender.SetLocalizableToolTip(this.toolStripButtons, null);
			this.locExtender.SetLocalizationComment(this.toolStripButtons, null);
			this.locExtender.SetLocalizationPriority(this.toolStripButtons, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.toolStripButtons, "toolStrip1.toolStrip1");
			this.toolStripButtons.Location = new System.Drawing.Point(0, 158);
			this.toolStripButtons.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this.toolStripButtons.Name = "toolStripButtons";
			this.toolStripButtons.Size = new System.Drawing.Size(241, 153);
			this.toolStripButtons.TabIndex = 7;
			//
			// OralAnnotationRecorderBaseDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(682, 338);
			this.Controls.Add(this.toolStripButtons);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, "Localized in subclass");
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.WindowTitle");
			this.Name = "OralAnnotationRecorderBaseDlg";
			this.Text = "Change my text";
			this.Controls.SetChildIndex(this.toolStripButtons, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.toolStripButtons.ResumeLayout(false);
			this.toolStripButtons.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripButton _buttonRecordAnnotation;
		private System.Windows.Forms.ToolStripButton _buttonListenToAnnotation;
		private System.Windows.Forms.ToolStripButton _buttonEraseAnnotation;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.ToolStrip toolStripButtons;

		protected System.Windows.Forms.ToolStripButton _buttonListenToOriginal;
	}
}
