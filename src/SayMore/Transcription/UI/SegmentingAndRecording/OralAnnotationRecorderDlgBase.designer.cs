using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class OralAnnotationRecorderBaseDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._labelHighlightedSegment = new System.Windows.Forms.Label();
			this._labelTotalDuration = new System.Windows.Forms.Label();
			this._labelTotalSegments = new System.Windows.Forms.Label();
			this._labelRecordButton = new System.Windows.Forms.Label();
			this._labelListenButton = new System.Windows.Forms.Label();
			this._pictureRecording = new System.Windows.Forms.PictureBox();
			this._labelListenHint = new System.Windows.Forms.Label();
			this._labelRecordHint = new System.Windows.Forms.Label();
			this._scrollTimer = new System.Windows.Forms.Timer(this.components);
			this._cursorBlinkTimer = new System.Windows.Forms.Timer(this.components);
			this._tableLayoutSegmentInfo = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutRecordAnnotations = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutMediaButtons = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureRecording)).BeginInit();
			this._tableLayoutSegmentInfo.SuspendLayout();
			this._tableLayoutRecordAnnotations.SuspendLayout();
			this._tableLayoutMediaButtons.SuspendLayout();
			this.SuspendLayout();
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// _labelHighlightedSegment
			//
			this._labelHighlightedSegment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelHighlightedSegment.AutoEllipsis = true;
			this._labelHighlightedSegment.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelHighlightedSegment, null);
			this.locExtender.SetLocalizationComment(this._labelHighlightedSegment, null);
			this.locExtender.SetLocalizingId(this._labelHighlightedSegment, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelHighlightedSegment");
			this._labelHighlightedSegment.Location = new System.Drawing.Point(0, 36);
			this._labelHighlightedSegment.Margin = new System.Windows.Forms.Padding(0);
			this._labelHighlightedSegment.Name = "_labelHighlightedSegment";
			this._labelHighlightedSegment.Size = new System.Drawing.Size(195, 13);
			this._labelHighlightedSegment.TabIndex = 3;
			this._labelHighlightedSegment.Text = "Segment {0}: {1} - {2}";
			//
			// _labelTotalDuration
			//
			this._labelTotalDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelTotalDuration.AutoEllipsis = true;
			this._labelTotalDuration.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelTotalDuration, null);
			this.locExtender.SetLocalizationComment(this._labelTotalDuration, null);
			this.locExtender.SetLocalizingId(this._labelTotalDuration, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelTotalDuration");
			this._labelTotalDuration.Location = new System.Drawing.Point(0, 0);
			this._labelTotalDuration.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._labelTotalDuration.Name = "_labelTotalDuration";
			this._labelTotalDuration.Size = new System.Drawing.Size(195, 13);
			this._labelTotalDuration.TabIndex = 2;
			this._labelTotalDuration.Text = "Original Recording Duration: {0}";
			//
			// _labelTotalSegments
			//
			this._labelTotalSegments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelTotalSegments.AutoEllipsis = true;
			this._labelTotalSegments.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelTotalSegments, null);
			this.locExtender.SetLocalizationComment(this._labelTotalSegments, null);
			this.locExtender.SetLocalizingId(this._labelTotalSegments, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelTotalSegments");
			this._labelTotalSegments.Location = new System.Drawing.Point(0, 18);
			this._labelTotalSegments.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._labelTotalSegments.Name = "_labelTotalSegments";
			this._labelTotalSegments.Size = new System.Drawing.Size(195, 13);
			this._labelTotalSegments.TabIndex = 3;
			this._labelTotalSegments.Text = "Number of Segments: {0}";
			//
			// _labelRecordButton
			//
			this._labelRecordButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._labelRecordButton.Image = global::SayMore.Properties.Resources.RecordOralAnnotation;
			this.locExtender.SetLocalizableToolTip(this._labelRecordButton, null);
			this.locExtender.SetLocalizationComment(this._labelRecordButton, null);
			this.locExtender.SetLocalizingId(this._labelRecordButton, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordButton");
			this._labelRecordButton.Location = new System.Drawing.Point(41, 30);
			this._labelRecordButton.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this._labelRecordButton.Name = "_labelRecordButton";
			this._labelRecordButton.Size = new System.Drawing.Size(50, 50);
			this._labelRecordButton.TabIndex = 0;
			//
			// _labelListenButton
			//
			this._labelListenButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._labelListenButton.Image = global::SayMore.Properties.Resources.ListenToOriginalRecording;
			this.locExtender.SetLocalizableToolTip(this._labelListenButton, null);
			this.locExtender.SetLocalizationComment(this._labelListenButton, null);
			this.locExtender.SetLocalizingId(this._labelListenButton, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.ListenButton");
			this._labelListenButton.Location = new System.Drawing.Point(41, 30);
			this._labelListenButton.Margin = new System.Windows.Forms.Padding(0, 10, 1, 10);
			this._labelListenButton.Name = "_labelListenButton";
			this._labelListenButton.Size = new System.Drawing.Size(50, 50);
			this._labelListenButton.TabIndex = 0;
			this._labelListenButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleListenToOriginalMouseDown);
			//
			// _pictureRecording
			//
			this._pictureRecording.BackColor = System.Drawing.Color.Transparent;
			this._pictureRecording.Image = global::SayMore.Properties.Resources.BusyWheelSmall;
			this.locExtender.SetLocalizableToolTip(this._pictureRecording, null);
			this.locExtender.SetLocalizationComment(this._pictureRecording, null);
			this.locExtender.SetLocalizationPriority(this._pictureRecording, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pictureRecording, "pictureBox1.pictureBox1");
			this._pictureRecording.Location = new System.Drawing.Point(189, 5);
			this._pictureRecording.Name = "_pictureRecording";
			this._pictureRecording.Size = new System.Drawing.Size(16, 16);
			this._pictureRecording.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureRecording.TabIndex = 9;
			this._pictureRecording.TabStop = false;
			this._pictureRecording.Visible = false;
			//
			// _labelListenHint
			//
			this._labelListenHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelListenHint.AutoSize = true;
			this._labelListenHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelListenHint, null);
			this.locExtender.SetLocalizationComment(this._labelListenHint, null);
			this.locExtender.SetLocalizationPriority(this._labelListenHint, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelListenHint, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelListenHint");
			this._labelListenHint.Location = new System.Drawing.Point(3, 90);
			this._labelListenHint.Name = "_labelListenHint";
			this._labelListenHint.Size = new System.Drawing.Size(128, 26);
			this._labelListenHint.TabIndex = 2;
			this._labelListenHint.Text = "Internationalized in Code file";
			this._labelListenHint.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			//
			// _labelRecordHint
			//
			this._labelRecordHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelRecordHint.AutoSize = true;
			this._labelRecordHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelRecordHint, null);
			this.locExtender.SetLocalizationComment(this._labelRecordHint, null);
			this.locExtender.SetLocalizationPriority(this._labelRecordHint, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelRecordHint, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelRecordHint");
			this._labelRecordHint.Location = new System.Drawing.Point(3, 90);
			this._labelRecordHint.Name = "_labelRecordHint";
			this._labelRecordHint.Size = new System.Drawing.Size(127, 10);
			this._labelRecordHint.TabIndex = 3;
			this._labelRecordHint.Text = "Internationalized in Code file";
			this._labelRecordHint.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			//
			// _scrollTimer
			//
			this._scrollTimer.Interval = 500;
			//
			// _cursorBlinkTimer
			//
			this._cursorBlinkTimer.Interval = 600;
			this._cursorBlinkTimer.Tick += new System.EventHandler(this.HandleCursorBlinkTimerTick);
			//
			// _tableLayoutSegmentInfo
			//
			this._tableLayoutSegmentInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._tableLayoutSegmentInfo.BackColor = System.Drawing.Color.Transparent;
			this._tableLayoutSegmentInfo.ColumnCount = 1;
			this._tableLayoutSegmentInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutSegmentInfo.Controls.Add(this._labelTotalSegments, 0, 1);
			this._tableLayoutSegmentInfo.Controls.Add(this._labelTotalDuration, 0, 0);
			this._tableLayoutSegmentInfo.Controls.Add(this._labelHighlightedSegment, 0, 2);
			this._tableLayoutSegmentInfo.Location = new System.Drawing.Point(6, 241);
			this._tableLayoutSegmentInfo.Margin = new System.Windows.Forms.Padding(5, 5, 5, 3);
			this._tableLayoutSegmentInfo.Name = "_tableLayoutSegmentInfo";
			this._tableLayoutSegmentInfo.RowCount = 2;
			this._tableLayoutSegmentInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutSegmentInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutSegmentInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutSegmentInfo.Size = new System.Drawing.Size(195, 74);
			this._tableLayoutSegmentInfo.TabIndex = 4;
			//
			// _tableLayoutRecordAnnotations
			//
			this._tableLayoutRecordAnnotations.ColumnCount = 1;
			this._tableLayoutRecordAnnotations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutRecordAnnotations.Controls.Add(this._labelRecordHint, 0, 2);
			this._tableLayoutRecordAnnotations.Controls.Add(this._labelRecordButton, 0, 1);
			this._tableLayoutRecordAnnotations.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutRecordAnnotations.Location = new System.Drawing.Point(0, 219);
			this._tableLayoutRecordAnnotations.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this._tableLayoutRecordAnnotations.Name = "_tableLayoutRecordAnnotations";
			this._tableLayoutRecordAnnotations.RowCount = 3;
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutRecordAnnotations.Size = new System.Drawing.Size(133, 100);
			this._tableLayoutRecordAnnotations.TabIndex = 1;
			//
			// _tableLayoutMediaButtons
			//
			this._tableLayoutMediaButtons.ColumnCount = 1;
			this._tableLayoutMediaButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMediaButtons.Controls.Add(this._labelListenButton, 0, 1);
			this._tableLayoutMediaButtons.Controls.Add(this._tableLayoutRecordAnnotations, 0, 3);
			this._tableLayoutMediaButtons.Controls.Add(this._labelListenHint, 0, 2);
			this._tableLayoutMediaButtons.Location = new System.Drawing.Point(357, 16);
			this._tableLayoutMediaButtons.Name = "_tableLayoutMediaButtons";
			this._tableLayoutMediaButtons.RowCount = 4;
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutMediaButtons.Size = new System.Drawing.Size(134, 319);
			this._tableLayoutMediaButtons.TabIndex = 8;
			this._tableLayoutMediaButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleMediaButtonTableLayoutPaint);
			//
			// OralAnnotationRecorderBaseDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 362);
			this.Controls.Add(this._pictureRecording);
			this.Controls.Add(this._tableLayoutSegmentInfo);
			this.Controls.Add(this._tableLayoutMediaButtons);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, "Localized in subclass");
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.WindowTitle");
			this.Name = "OralAnnotationRecorderBaseDlg";
			this.Opacity = 1D;
			this.Text = "Change my text";
			this.Controls.SetChildIndex(this._tableLayoutMediaButtons, 0);
			this.Controls.SetChildIndex(this._tableLayoutSegmentInfo, 0);
			this.Controls.SetChildIndex(this._pictureRecording, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureRecording)).EndInit();
			this._tableLayoutSegmentInfo.ResumeLayout(false);
			this._tableLayoutSegmentInfo.PerformLayout();
			this._tableLayoutRecordAnnotations.ResumeLayout(false);
			this._tableLayoutRecordAnnotations.PerformLayout();
			this._tableLayoutMediaButtons.ResumeLayout(false);
			this._tableLayoutMediaButtons.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Timer _scrollTimer;
		private System.Windows.Forms.Timer _cursorBlinkTimer;
		private System.Windows.Forms.Label _labelHighlightedSegment;
		private System.Windows.Forms.Label _labelTotalDuration;
		private System.Windows.Forms.Label _labelTotalSegments;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutSegmentInfo;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutRecordAnnotations;
		private System.Windows.Forms.Label _labelRecordButton;
		private System.Windows.Forms.Label _labelListenButton;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutMediaButtons;
		private System.Windows.Forms.PictureBox _pictureRecording;
		private System.Windows.Forms.Label _labelListenHint;
		private System.Windows.Forms.Label _labelRecordHint;
	}
}
