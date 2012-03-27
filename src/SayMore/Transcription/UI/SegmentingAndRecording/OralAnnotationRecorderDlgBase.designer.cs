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
			this._buttonListenToAnnotation = new System.Windows.Forms.ToolStripButton();
			this._buttonEraseAnnotation = new System.Windows.Forms.ToolStripButton();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.toolStripButtons = new System.Windows.Forms.ToolStrip();
			this._labelListenButton = new System.Windows.Forms.Label();
			this._labelRecordButton = new System.Windows.Forms.Label();
			this._tableLayoutMediaButtons = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutRecordAnnotations = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.toolStripButtons.SuspendLayout();
			this._tableLayoutMediaButtons.SuspendLayout();
			this._tableLayoutRecordAnnotations.SuspendLayout();
			this.SuspendLayout();
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
			// toolStripButtons
			//
			this.toolStripButtons.BackColor = System.Drawing.Color.Transparent;
			this.toolStripButtons.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripButtons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripButtons.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
			this.toolStripButtons.Size = new System.Drawing.Size(148, 90);
			this.toolStripButtons.TabIndex = 7;
			//
			// _labelListenButton
			//
			this._labelListenButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._labelListenButton.Image = global::SayMore.Properties.Resources.ListenToOriginalRecording;
			this.locExtender.SetLocalizableToolTip(this._labelListenButton, null);
			this.locExtender.SetLocalizationComment(this._labelListenButton, null);
			this.locExtender.SetLocalizingId(this._labelListenButton, "label1.label1");
			this._labelListenButton.Location = new System.Drawing.Point(33, 30);
			this._labelListenButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this._labelListenButton.Name = "_labelListenButton";
			this._labelListenButton.Size = new System.Drawing.Size(50, 50);
			this._labelListenButton.TabIndex = 0;
			this._labelListenButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleListenToOriginalMouseDown);
			this._labelListenButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandleListenToOriginalMouseDown);
			//
			// _labelRecordButton
			//
			this._labelRecordButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._labelRecordButton.Image = global::SayMore.Properties.Resources.RecordOralAnnotation;
			this.locExtender.SetLocalizableToolTip(this._labelRecordButton, null);
			this.locExtender.SetLocalizationComment(this._labelRecordButton, null);
			this.locExtender.SetLocalizingId(this._labelRecordButton, "label1.label1");
			this._labelRecordButton.Location = new System.Drawing.Point(33, 30);
			this._labelRecordButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this._labelRecordButton.Name = "_labelRecordButton";
			this._labelRecordButton.Size = new System.Drawing.Size(50, 50);
			this._labelRecordButton.TabIndex = 0;
			//
			// _tableLayoutMediaButtons
			//
			this._tableLayoutMediaButtons.ColumnCount = 1;
			this._tableLayoutMediaButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMediaButtons.Controls.Add(this._labelListenButton, 0, 1);
			this._tableLayoutMediaButtons.Controls.Add(this._tableLayoutRecordAnnotations, 0, 2);
			this._tableLayoutMediaButtons.Location = new System.Drawing.Point(220, 90);
			this._tableLayoutMediaButtons.Name = "_tableLayoutMediaButtons";
			this._tableLayoutMediaButtons.RowCount = 3;
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this._tableLayoutMediaButtons.Size = new System.Drawing.Size(116, 250);
			this._tableLayoutMediaButtons.TabIndex = 8;
			//
			// _tableLayoutRecordAnnotations
			//
			this._tableLayoutRecordAnnotations.ColumnCount = 1;
			this._tableLayoutRecordAnnotations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutRecordAnnotations.Controls.Add(this._labelRecordButton, 0, 1);
			this._tableLayoutRecordAnnotations.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutRecordAnnotations.Location = new System.Drawing.Point(0, 150);
			this._tableLayoutRecordAnnotations.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutRecordAnnotations.Name = "_tableLayoutRecordAnnotations";
			this._tableLayoutRecordAnnotations.RowCount = 2;
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutRecordAnnotations.Size = new System.Drawing.Size(116, 100);
			this._tableLayoutRecordAnnotations.TabIndex = 1;
			//
			// OralAnnotationRecorderBaseDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 338);
			this.Controls.Add(this._tableLayoutMediaButtons);
			this.Controls.Add(this.toolStripButtons);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, "Localized in subclass");
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.WindowTitle");
			this.Name = "OralAnnotationRecorderBaseDlg";
			this.Opacity = 1D;
			this.Text = "Change my text";
			this.Controls.SetChildIndex(this.toolStripButtons, 0);
			this.Controls.SetChildIndex(this._tableLayoutMediaButtons, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.toolStripButtons.ResumeLayout(false);
			this.toolStripButtons.PerformLayout();
			this._tableLayoutMediaButtons.ResumeLayout(false);
			this._tableLayoutRecordAnnotations.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripButton _buttonListenToAnnotation;
		private System.Windows.Forms.ToolStripButton _buttonEraseAnnotation;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.ToolStrip toolStripButtons;
		private System.Windows.Forms.Label _labelListenButton;
		private System.Windows.Forms.Label _labelRecordButton;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutMediaButtons;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutRecordAnnotations;
	}
}
