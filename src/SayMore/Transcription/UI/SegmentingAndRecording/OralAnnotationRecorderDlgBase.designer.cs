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
			this._labelRecordButton = new System.Windows.Forms.Label();
			this._labelListenButton = new System.Windows.Forms.Label();
			this._pictureRecording = new System.Windows.Forms.PictureBox();
			this._labelSegmentTooShort = new System.Windows.Forms.Label();
			this._labelRecordHint = new System.Windows.Forms.Label();
			this._panelPeakMeter = new SilTools.Controls.SilPanel();
			this._labelListenHint = new System.Windows.Forms.Label();
			this._scrollTimer = new System.Windows.Forms.Timer(this.components);
			this._cursorBlinkTimer = new System.Windows.Forms.Timer(this.components);
			this._tableLayoutRecordAnnotations = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutMediaButtons = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureRecording)).BeginInit();
			this._tableLayoutRecordAnnotations.SuspendLayout();
			this._tableLayoutMediaButtons.SuspendLayout();
			this.SuspendLayout();
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// _labelRecordButton
			//
			this._labelRecordButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._tableLayoutRecordAnnotations.SetColumnSpan(this._labelRecordButton, 2);
			this._labelRecordButton.Image = global::SayMore.Properties.Resources.RecordOralAnnotation;
			this.locExtender.SetLocalizableToolTip(this._labelRecordButton, "Hold button down to record");
			this.locExtender.SetLocalizationComment(this._labelRecordButton, null);
			this.locExtender.SetLocalizingId(this._labelRecordButton, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordButton");
			this._labelRecordButton.Location = new System.Drawing.Point(47, 30);
			this._labelRecordButton.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this._labelRecordButton.Name = "_labelRecordButton";
			this._labelRecordButton.Size = new System.Drawing.Size(50, 50);
			this._labelRecordButton.TabIndex = 0;
			//
			// _labelListenButton
			//
			this._labelListenButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._labelListenButton.Image = global::SayMore.Properties.Resources.ListenToOriginalRecording;
			this.locExtender.SetLocalizableToolTip(this._labelListenButton, "Hold button down to listen\\nto original recording");
			this.locExtender.SetLocalizationComment(this._labelListenButton, null);
			this.locExtender.SetLocalizingId(this._labelListenButton, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.ListenButton");
			this._labelListenButton.Location = new System.Drawing.Point(47, 30);
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
			this._pictureRecording.Location = new System.Drawing.Point(58, 5);
			this._pictureRecording.Name = "_pictureRecording";
			this._pictureRecording.Size = new System.Drawing.Size(16, 16);
			this._pictureRecording.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureRecording.TabIndex = 9;
			this._pictureRecording.TabStop = false;
			this._pictureRecording.Visible = false;
			//
			// _labelSegmentTooShort
			//
			this._labelSegmentTooShort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelSegmentTooShort.AutoSize = true;
			this._labelSegmentTooShort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelSegmentTooShort.ForeColor = System.Drawing.Color.Red;
			this.locExtender.SetLocalizableToolTip(this._labelSegmentTooShort, null);
			this.locExtender.SetLocalizationComment(this._labelSegmentTooShort, null);
			this.locExtender.SetLocalizationPriority(this._labelSegmentTooShort, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelSegmentTooShort, "OralAnnotationRecorderBaseDlg._labelListenHint");
			this._labelSegmentTooShort.Location = new System.Drawing.Point(8, 204);
			this._labelSegmentTooShort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this._labelSegmentTooShort.Name = "_labelSegmentTooShort";
			this._labelSegmentTooShort.Size = new System.Drawing.Size(326, 13);
			this._labelSegmentTooShort.TabIndex = 2;
			this._labelSegmentTooShort.Text = "Segment too short - this text will be set programmatically";
			//
			// _labelRecordHint
			//
			this._labelRecordHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelRecordHint.AutoSize = true;
			this._labelRecordHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelRecordHint, null);
			this.locExtender.SetLocalizationComment(this._labelRecordHint, null);
			this.locExtender.SetLocalizingId(this._labelRecordHint, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelRecordHint");
			this._labelRecordHint.Location = new System.Drawing.Point(9, 271);
			this._labelRecordHint.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this._labelRecordHint.Name = "_labelRecordHint";
			this._labelRecordHint.Size = new System.Drawing.Size(243, 13);
			this._labelRecordHint.TabIndex = 3;
			this._labelRecordHint.Text = "To record, press and hold the SPACE key";
			//
			// _panelPeakMeter
			//
			this._panelPeakMeter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._panelPeakMeter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelPeakMeter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelPeakMeter.ClipTextForChildControls = true;
			this._panelPeakMeter.ControlReceivingFocusOnMnemonic = null;
			this._panelPeakMeter.DoubleBuffered = true;
			this._panelPeakMeter.DrawOnlyBottomBorder = false;
			this._panelPeakMeter.DrawOnlyTopBorder = false;
			this._panelPeakMeter.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._panelPeakMeter.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelPeakMeter, null);
			this.locExtender.SetLocalizationComment(this._panelPeakMeter, null);
			this.locExtender.SetLocalizingId(this._panelPeakMeter, "OralAnnotationRecorderBaseDlg._panelPeakMeter");
			this._panelPeakMeter.Location = new System.Drawing.Point(28, 93);
			this._panelPeakMeter.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
			this._panelPeakMeter.MnemonicGeneratesClick = false;
			this._panelPeakMeter.Name = "_panelPeakMeter";
			this._panelPeakMeter.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
			this._panelPeakMeter.PaintExplorerBarBackground = false;
			this._panelPeakMeter.Size = new System.Drawing.Size(108, 17);
			this._panelPeakMeter.TabIndex = 1;
			//
			// _labelListenHint
			//
			this._labelListenHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelListenHint.AutoSize = true;
			this._labelListenHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelListenHint, null);
			this.locExtender.SetLocalizationComment(this._labelListenHint, null);
			this.locExtender.SetLocalizingId(this._labelListenHint, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelListenHint");
			this._labelListenHint.Location = new System.Drawing.Point(9, 234);
			this._labelListenHint.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this._labelListenHint.Name = "_labelListenHint";
			this._labelListenHint.Size = new System.Drawing.Size(376, 13);
			this._labelListenHint.TabIndex = 10;
			this._labelListenHint.Text = "To listen to the original recording, press and hold the SPACE key";
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
			// _tableLayoutRecordAnnotations
			//
			this._tableLayoutRecordAnnotations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutRecordAnnotations.ColumnCount = 2;
			this._tableLayoutRecordAnnotations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutRecordAnnotations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutRecordAnnotations.Controls.Add(this._labelRecordButton, 0, 1);
			this._tableLayoutRecordAnnotations.Controls.Add(this._panelPeakMeter, 1, 2);
			this._tableLayoutRecordAnnotations.Location = new System.Drawing.Point(1, 163);
			this._tableLayoutRecordAnnotations.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
			this._tableLayoutRecordAnnotations.Name = "_tableLayoutRecordAnnotations";
			this._tableLayoutRecordAnnotations.RowCount = 3;
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutRecordAnnotations.Size = new System.Drawing.Size(144, 150);
			this._tableLayoutRecordAnnotations.TabIndex = 1;
			//
			// _tableLayoutMediaButtons
			//
			this._tableLayoutMediaButtons.ColumnCount = 1;
			this._tableLayoutMediaButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMediaButtons.Controls.Add(this._labelListenButton, 0, 1);
			this._tableLayoutMediaButtons.Controls.Add(this._tableLayoutRecordAnnotations, 0, 2);
			this._tableLayoutMediaButtons.Location = new System.Drawing.Point(357, 16);
			this._tableLayoutMediaButtons.Name = "_tableLayoutMediaButtons";
			this._tableLayoutMediaButtons.RowCount = 3;
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this._tableLayoutMediaButtons.Size = new System.Drawing.Size(146, 313);
			this._tableLayoutMediaButtons.TabIndex = 8;
			this._tableLayoutMediaButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleMediaButtonTableLayoutPaint);
			//
			// OralAnnotationRecorderBaseDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 412);
			this.Controls.Add(this._labelRecordHint);
			this.Controls.Add(this._pictureRecording);
			this.Controls.Add(this._labelListenHint);
			this.Controls.Add(this._labelSegmentTooShort);
			this.Controls.Add(this._tableLayoutMediaButtons);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, "Localized in subclass");
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(330, 415);
			this.Name = "OralAnnotationRecorderBaseDlg";
			this.Opacity = 1D;
			this.Text = "Change my text";
			this.Controls.SetChildIndex(this._tableLayoutMediaButtons, 0);
			this.Controls.SetChildIndex(this._labelSegmentTooShort, 0);
			this.Controls.SetChildIndex(this._labelListenHint, 0);
			this.Controls.SetChildIndex(this._pictureRecording, 0);
			this.Controls.SetChildIndex(this._labelRecordHint, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureRecording)).EndInit();
			this._tableLayoutRecordAnnotations.ResumeLayout(false);
			this._tableLayoutMediaButtons.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Timer _scrollTimer;
		private System.Windows.Forms.Timer _cursorBlinkTimer;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutRecordAnnotations;
		private System.Windows.Forms.Label _labelRecordButton;
		private System.Windows.Forms.Label _labelListenButton;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutMediaButtons;
		private System.Windows.Forms.PictureBox _pictureRecording;
		private System.Windows.Forms.Label _labelSegmentTooShort;
		private System.Windows.Forms.Label _labelRecordHint;
		private SilPanel _panelPeakMeter;
		private System.Windows.Forms.Label _labelListenHint;
	}
}
