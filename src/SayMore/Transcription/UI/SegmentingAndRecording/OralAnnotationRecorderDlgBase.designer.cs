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
			this._labelListenButton = new System.Windows.Forms.Label();
			this._labelRecordButton = new System.Windows.Forms.Label();
			this._labelTotalDuration = new System.Windows.Forms.Label();
			this._labelTotalSegments = new System.Windows.Forms.Label();
			this._labelHighlightedSegment = new System.Windows.Forms.Label();
			this._labelSegmentDuration = new System.Windows.Forms.Label();
			this._labelSegmentStart = new System.Windows.Forms.Label();
			this._tableLayoutMediaButtons = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutRecordAnnotations = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutSegmentInfo = new System.Windows.Forms.TableLayoutPanel();
			this._scrollTimer = new System.Windows.Forms.Timer(this.components);
			this._cursorBlinkTimer = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this._tableLayoutMediaButtons.SuspendLayout();
			this._tableLayoutRecordAnnotations.SuspendLayout();
			this._tableLayoutSegmentInfo.SuspendLayout();
			this.SuspendLayout();
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// _labelListenButton
			//
			this._labelListenButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._labelListenButton.Image = global::SayMore.Properties.Resources.ListenToOriginalRecording;
			this.locExtender.SetLocalizableToolTip(this._labelListenButton, null);
			this.locExtender.SetLocalizationComment(this._labelListenButton, null);
			this.locExtender.SetLocalizingId(this._labelListenButton, "label1.label1");
			this._labelListenButton.Location = new System.Drawing.Point(41, 30);
			this._labelListenButton.Margin = new System.Windows.Forms.Padding(0, 10, 1, 10);
			this._labelListenButton.Name = "_labelListenButton";
			this._labelListenButton.Size = new System.Drawing.Size(50, 50);
			this._labelListenButton.TabIndex = 0;
			this._labelListenButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleListenToOriginalMouseDown);
			//
			// _labelRecordButton
			//
			this._labelRecordButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._labelRecordButton.Image = global::SayMore.Properties.Resources.RecordOralAnnotation;
			this.locExtender.SetLocalizableToolTip(this._labelRecordButton, null);
			this.locExtender.SetLocalizationComment(this._labelRecordButton, null);
			this.locExtender.SetLocalizingId(this._labelRecordButton, "label1.label1");
			this._labelRecordButton.Location = new System.Drawing.Point(41, 30);
			this._labelRecordButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this._labelRecordButton.Name = "_labelRecordButton";
			this._labelRecordButton.Size = new System.Drawing.Size(50, 50);
			this._labelRecordButton.TabIndex = 0;
			//
			// _labelTotalDuration
			//
			this._labelTotalDuration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelTotalDuration.AutoEllipsis = true;
			this._labelTotalDuration.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelTotalDuration, null);
			this.locExtender.SetLocalizationComment(this._labelTotalDuration, null);
			this.locExtender.SetLocalizingId(this._labelTotalDuration, "label1.label1");
			this._labelTotalDuration.Location = new System.Drawing.Point(4, 90);
			this._labelTotalDuration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this._labelTotalDuration.Name = "_labelTotalDuration";
			this._labelTotalDuration.Size = new System.Drawing.Size(126, 13);
			this._labelTotalDuration.TabIndex = 2;
			this._labelTotalDuration.Text = "Total Duration: {0}";
			//
			// _labelTotalSegments
			//
			this._labelTotalSegments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelTotalSegments.AutoEllipsis = true;
			this._labelTotalSegments.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelTotalSegments, null);
			this.locExtender.SetLocalizationComment(this._labelTotalSegments, null);
			this.locExtender.SetLocalizingId(this._labelTotalSegments, "label1.label1");
			this._labelTotalSegments.Location = new System.Drawing.Point(4, 108);
			this._labelTotalSegments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 0);
			this._labelTotalSegments.Name = "_labelTotalSegments";
			this._labelTotalSegments.Size = new System.Drawing.Size(126, 13);
			this._labelTotalSegments.TabIndex = 3;
			this._labelTotalSegments.Text = "Segments: {0}";
			//
			// _labelHighlightedSegment
			//
			this._labelHighlightedSegment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelHighlightedSegment.AutoEllipsis = true;
			this._labelHighlightedSegment.AutoSize = true;
			this._labelHighlightedSegment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelHighlightedSegment, null);
			this.locExtender.SetLocalizationComment(this._labelHighlightedSegment, null);
			this.locExtender.SetLocalizingId(this._labelHighlightedSegment, "label1.label1");
			this._labelHighlightedSegment.Location = new System.Drawing.Point(4, 0);
			this._labelHighlightedSegment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 5);
			this._labelHighlightedSegment.Name = "_labelHighlightedSegment";
			this._labelHighlightedSegment.Size = new System.Drawing.Size(125, 13);
			this._labelHighlightedSegment.TabIndex = 3;
			this._labelHighlightedSegment.Text = "Segment: {0}";
			//
			// _labelSegmentDuration
			//
			this._labelSegmentDuration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelSegmentDuration.AutoEllipsis = true;
			this._labelSegmentDuration.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelSegmentDuration, null);
			this.locExtender.SetLocalizationComment(this._labelSegmentDuration, null);
			this.locExtender.SetLocalizingId(this._labelSegmentDuration, "label1.label1");
			this._labelSegmentDuration.Location = new System.Drawing.Point(15, 36);
			this._labelSegmentDuration.Margin = new System.Windows.Forms.Padding(15, 0, 4, 0);
			this._labelSegmentDuration.Name = "_labelSegmentDuration";
			this._labelSegmentDuration.Size = new System.Drawing.Size(114, 13);
			this._labelSegmentDuration.TabIndex = 4;
			this._labelSegmentDuration.Text = "Duration: {0}";
			//
			// _labelSegmentStart
			//
			this._labelSegmentStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelSegmentStart.AutoEllipsis = true;
			this._labelSegmentStart.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelSegmentStart, null);
			this.locExtender.SetLocalizationComment(this._labelSegmentStart, null);
			this.locExtender.SetLocalizingId(this._labelSegmentStart, "label1.label1");
			this._labelSegmentStart.Location = new System.Drawing.Point(15, 18);
			this._labelSegmentStart.Margin = new System.Windows.Forms.Padding(15, 0, 4, 5);
			this._labelSegmentStart.Name = "_labelSegmentStart";
			this._labelSegmentStart.Size = new System.Drawing.Size(114, 13);
			this._labelSegmentStart.TabIndex = 5;
			this._labelSegmentStart.Text = "Start: {0}";
			//
			// _tableLayoutMediaButtons
			//
			this._tableLayoutMediaButtons.ColumnCount = 1;
			this._tableLayoutMediaButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMediaButtons.Controls.Add(this._labelListenButton, 0, 1);
			this._tableLayoutMediaButtons.Controls.Add(this._tableLayoutRecordAnnotations, 0, 5);
			this._tableLayoutMediaButtons.Controls.Add(this._labelTotalDuration, 0, 2);
			this._tableLayoutMediaButtons.Controls.Add(this._labelTotalSegments, 0, 3);
			this._tableLayoutMediaButtons.Controls.Add(this._tableLayoutSegmentInfo, 0, 4);
			this._tableLayoutMediaButtons.Location = new System.Drawing.Point(220, 16);
			this._tableLayoutMediaButtons.Name = "_tableLayoutMediaButtons";
			this._tableLayoutMediaButtons.RowCount = 6;
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this._tableLayoutMediaButtons.Size = new System.Drawing.Size(134, 319);
			this._tableLayoutMediaButtons.TabIndex = 8;
			this._tableLayoutMediaButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleMediaButtonTableLayoutPaint);
			//
			// _tableLayoutRecordAnnotations
			//
			this._tableLayoutRecordAnnotations.ColumnCount = 1;
			this._tableLayoutRecordAnnotations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutRecordAnnotations.Controls.Add(this._labelRecordButton, 0, 1);
			this._tableLayoutRecordAnnotations.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutRecordAnnotations.Location = new System.Drawing.Point(0, 219);
			this._tableLayoutRecordAnnotations.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this._tableLayoutRecordAnnotations.Name = "_tableLayoutRecordAnnotations";
			this._tableLayoutRecordAnnotations.RowCount = 2;
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutRecordAnnotations.Size = new System.Drawing.Size(133, 100);
			this._tableLayoutRecordAnnotations.TabIndex = 1;
			//
			// _tableLayoutSegmentInfo
			//
			this._tableLayoutSegmentInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutSegmentInfo.AutoSize = true;
			this._tableLayoutSegmentInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutSegmentInfo.ColumnCount = 1;
			this._tableLayoutSegmentInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutSegmentInfo.Controls.Add(this._labelSegmentStart, 0, 1);
			this._tableLayoutSegmentInfo.Controls.Add(this._labelSegmentDuration, 0, 1);
			this._tableLayoutSegmentInfo.Controls.Add(this._labelHighlightedSegment, 0, 0);
			this._tableLayoutSegmentInfo.Location = new System.Drawing.Point(0, 136);
			this._tableLayoutSegmentInfo.Margin = new System.Windows.Forms.Padding(0, 15, 1, 10);
			this._tableLayoutSegmentInfo.Name = "_tableLayoutSegmentInfo";
			this._tableLayoutSegmentInfo.RowCount = 2;
			this._tableLayoutSegmentInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutSegmentInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutSegmentInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutSegmentInfo.Size = new System.Drawing.Size(133, 49);
			this._tableLayoutSegmentInfo.TabIndex = 4;
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
			// OralAnnotationRecorderBaseDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 338);
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
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this._tableLayoutMediaButtons.ResumeLayout(false);
			this._tableLayoutMediaButtons.PerformLayout();
			this._tableLayoutRecordAnnotations.ResumeLayout(false);
			this._tableLayoutSegmentInfo.ResumeLayout(false);
			this._tableLayoutSegmentInfo.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Label _labelListenButton;
		private System.Windows.Forms.Label _labelRecordButton;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutMediaButtons;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutRecordAnnotations;
		private System.Windows.Forms.Label _labelTotalDuration;
		private System.Windows.Forms.Label _labelTotalSegments;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutSegmentInfo;
		private System.Windows.Forms.Label _labelSegmentStart;
		private System.Windows.Forms.Label _labelSegmentDuration;
		private System.Windows.Forms.Label _labelHighlightedSegment;
		private System.Windows.Forms.Timer _scrollTimer;
		private System.Windows.Forms.Timer _cursorBlinkTimer;
	}
}
