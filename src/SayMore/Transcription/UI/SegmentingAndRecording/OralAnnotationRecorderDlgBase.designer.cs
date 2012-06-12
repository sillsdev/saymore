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
			this._panelListen = new System.Windows.Forms.Panel();
			this._labelListenButton = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._labelRecordButton = new System.Windows.Forms.Label();
			this._pictureRecording = new System.Windows.Forms.PictureBox();
			this._labelErrorInfo = new System.Windows.Forms.Label();
			this._labelRecordHint = new System.Windows.Forms.Label();
			this._panelPeakMeter = new SilTools.Controls.SilPanel();
			this._labelListenHint = new System.Windows.Forms.Label();
			this._labelFinishedHint = new System.Windows.Forms.Label();
			this._pictureIcon = new System.Windows.Forms.PictureBox();
			this._lastSegmentMenuStrip = new System.Windows.Forms.MenuStrip();
			this._undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._videoHelpMenuStrip = new System.Windows.Forms.MenuStrip();
			this._videoHelpMenu = new System.Windows.Forms.ToolStripMenuItem();
			this._scrollTimer = new System.Windows.Forms.Timer(this.components);
			this._cursorBlinkTimer = new System.Windows.Forms.Timer(this.components);
			this._tableLayoutRecordAnnotations = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutMediaButtons = new System.Windows.Forms.TableLayoutPanel();
			this._checkForRecordingDevice = new System.Windows.Forms.Timer(this.components);
			this._panelListen.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureRecording)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureIcon)).BeginInit();
			this._lastSegmentMenuStrip.SuspendLayout();
			this._videoHelpMenuStrip.SuspendLayout();
			this._tableLayoutRecordAnnotations.SuspendLayout();
			this._tableLayoutMediaButtons.SuspendLayout();
			this.SuspendLayout();
			//
			// _panelListen
			//
			this._panelListen.BackColor = System.Drawing.Color.AliceBlue;
			this._panelListen.Controls.Add(this._labelListenButton);
			this._panelListen.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelListen.Location = new System.Drawing.Point(0, 20);
			this._panelListen.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this._panelListen.Name = "_panelListen";
			this._panelListen.Size = new System.Drawing.Size(145, 143);
			this._panelListen.TabIndex = 2;
			//
			// _labelListenButton
			//
			this._labelListenButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelListenButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(129)))), ((int)(((byte)(199)))));
			this._labelListenButton.Image = global::SayMore.Properties.Resources.ListenToOriginalRecording;
			this._labelListenButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.locExtender.SetLocalizableToolTip(this._labelListenButton, "Hold button down to listen\\nto original recording");
			this.locExtender.SetLocalizationComment(this._labelListenButton, null);
			this.locExtender.SetLocalizingId(this._labelListenButton, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.ListenButton");
			this._labelListenButton.Location = new System.Drawing.Point(0, 10);
			this._labelListenButton.Margin = new System.Windows.Forms.Padding(1, 10, 1, 10);
			this._labelListenButton.Name = "_labelListenButton";
			this._labelListenButton.Size = new System.Drawing.Size(142, 65);
			this._labelListenButton.TabIndex = 0;
			this._labelListenButton.Text = "Listen";
			this._labelListenButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this._labelListenButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleListenToSourceMouseDown);
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// _labelRecordButton
			//
			this._labelRecordButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutRecordAnnotations.SetColumnSpan(this._labelRecordButton, 2);
			this._labelRecordButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(95)))), ((int)(((byte)(14)))));
			this._labelRecordButton.Image = global::SayMore.Properties.Resources.RecordOralAnnotation;
			this._labelRecordButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.locExtender.SetLocalizableToolTip(this._labelRecordButton, "Hold button down to record");
			this.locExtender.SetLocalizationComment(this._labelRecordButton, null);
			this.locExtender.SetLocalizingId(this._labelRecordButton, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase.RecordButton");
			this._labelRecordButton.Location = new System.Drawing.Point(1, 30);
			this._labelRecordButton.Margin = new System.Windows.Forms.Padding(1, 10, 1, 10);
			this._labelRecordButton.Name = "_labelRecordButton";
			this._labelRecordButton.Size = new System.Drawing.Size(143, 65);
			this._labelRecordButton.TabIndex = 0;
			this._labelRecordButton.Text = "Speak";
			this._labelRecordButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
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
			// _labelErrorInfo
			//
			this._labelErrorInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelErrorInfo.AutoSize = true;
			this._labelErrorInfo.BackColor = System.Drawing.Color.Transparent;
			this._labelErrorInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelErrorInfo.ForeColor = System.Drawing.Color.White;
			this.locExtender.SetLocalizableToolTip(this._labelErrorInfo, null);
			this.locExtender.SetLocalizationComment(this._labelErrorInfo, null);
			this.locExtender.SetLocalizationPriority(this._labelErrorInfo, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelErrorInfo, "OralAnnotationRecorderBaseDlg._labelListenHint");
			this._labelErrorInfo.Location = new System.Drawing.Point(8, 242);
			this._labelErrorInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this._labelErrorInfo.Name = "_labelErrorInfo";
			this._labelErrorInfo.Size = new System.Drawing.Size(215, 13);
			this._labelErrorInfo.TabIndex = 2;
			this._labelErrorInfo.Text = "This text will be set programmatically";
			this._labelErrorInfo.Visible = false;
			//
			// _labelRecordHint
			//
			this._labelRecordHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelRecordHint.AutoSize = true;
			this._labelRecordHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelRecordHint.ForeColor = System.Drawing.Color.White;
			this.locExtender.SetLocalizableToolTip(this._labelRecordHint, null);
			this.locExtender.SetLocalizationComment(this._labelRecordHint, null);
			this.locExtender.SetLocalizingId(this._labelRecordHint, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelRecordHint");
			this._labelRecordHint.Location = new System.Drawing.Point(9, 309);
			this._labelRecordHint.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this._labelRecordHint.Name = "_labelRecordHint";
			this._labelRecordHint.Size = new System.Drawing.Size(248, 13);
			this._labelRecordHint.TabIndex = 3;
			this._labelRecordHint.Text = "To record, press and hold the SPACE BAR";
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
			this._panelPeakMeter.Location = new System.Drawing.Point(28, 108);
			this._panelPeakMeter.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
			this._panelPeakMeter.MnemonicGeneratesClick = false;
			this._panelPeakMeter.Name = "_panelPeakMeter";
			this._panelPeakMeter.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
			this._panelPeakMeter.PaintExplorerBarBackground = false;
			this._panelPeakMeter.Size = new System.Drawing.Size(109, 17);
			this._panelPeakMeter.TabIndex = 1;
			//
			// _labelListenHint
			//
			this._labelListenHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelListenHint.AutoSize = true;
			this._labelListenHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelListenHint.ForeColor = System.Drawing.Color.White;
			this.locExtender.SetLocalizableToolTip(this._labelListenHint, null);
			this.locExtender.SetLocalizationComment(this._labelListenHint, null);
			this.locExtender.SetLocalizingId(this._labelListenHint, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelListenHint");
			this._labelListenHint.Location = new System.Drawing.Point(9, 272);
			this._labelListenHint.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this._labelListenHint.Name = "_labelListenHint";
			this._labelListenHint.Size = new System.Drawing.Size(381, 13);
			this._labelListenHint.TabIndex = 10;
			this._labelListenHint.Text = "To listen to the original recording, press and hold the SPACE BAR";
			//
			// _labelFinishedHint
			//
			this._labelFinishedHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelFinishedHint.AutoSize = true;
			this._labelFinishedHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelFinishedHint.ForeColor = System.Drawing.Color.White;
			this.locExtender.SetLocalizableToolTip(this._labelFinishedHint, null);
			this.locExtender.SetLocalizationComment(this._labelFinishedHint, null);
			this.locExtender.SetLocalizingId(this._labelFinishedHint, "DialogBoxes.Transcription.OralAnnotationRecorderDlgBase._labelFinishedHint");
			this._labelFinishedHint.Location = new System.Drawing.Point(11, 344);
			this._labelFinishedHint.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this._labelFinishedHint.Name = "_labelFinishedHint";
			this._labelFinishedHint.Size = new System.Drawing.Size(54, 13);
			this._labelFinishedHint.TabIndex = 11;
			this._labelFinishedHint.Text = "Finished";
			this._labelFinishedHint.Visible = false;
			//
			// _pictureIcon
			//
			this._pictureIcon.BackColor = System.Drawing.Color.Transparent;
			this._pictureIcon.Image = global::SayMore.Properties.Resources.Information_blue;
			this.locExtender.SetLocalizableToolTip(this._pictureIcon, null);
			this.locExtender.SetLocalizationComment(this._pictureIcon, null);
			this.locExtender.SetLocalizationPriority(this._pictureIcon, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pictureIcon, "pictureBox1.pictureBox1");
			this._pictureIcon.Location = new System.Drawing.Point(12, 375);
			this._pictureIcon.Name = "_pictureIcon";
			this._pictureIcon.Size = new System.Drawing.Size(30, 30);
			this._pictureIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureIcon.TabIndex = 12;
			this._pictureIcon.TabStop = false;
			//
			// _lastSegmentMenuStrip
			//
			this._lastSegmentMenuStrip.AllowMerge = false;
			this._lastSegmentMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(129)))), ((int)(((byte)(199)))));
			this._lastSegmentMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
			this._lastSegmentMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._undoToolStripMenuItem});
			this.locExtender.SetLocalizableToolTip(this._lastSegmentMenuStrip, null);
			this.locExtender.SetLocalizationComment(this._lastSegmentMenuStrip, null);
			this.locExtender.SetLocalizationPriority(this._lastSegmentMenuStrip, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._lastSegmentMenuStrip, "OralAnnotationRecorderBaseDlg._lastSegmentMenuStrip");
			this._lastSegmentMenuStrip.Location = new System.Drawing.Point(169, 5);
			this._lastSegmentMenuStrip.Name = "_lastSegmentMenuStrip";
			this._lastSegmentMenuStrip.Padding = new System.Windows.Forms.Padding(1, 1, 0, 2);
			this._lastSegmentMenuStrip.ShowItemToolTips = true;
			this._lastSegmentMenuStrip.Size = new System.Drawing.Size(63, 24);
			this._lastSegmentMenuStrip.TabIndex = 15;
			this._lastSegmentMenuStrip.Visible = false;
			//
			// _undoToolStripMenuItem
			//
			this._undoToolStripMenuItem.BackColor = System.Drawing.Color.AliceBlue;
			this._undoToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(129)))), ((int)(((byte)(199)))));
			this._undoToolStripMenuItem.Image = global::SayMore.Properties.Resources.undo;
			this.locExtender.SetLocalizableToolTip(this._undoToolStripMenuItem, null);
			this.locExtender.SetLocalizationComment(this._undoToolStripMenuItem, null);
			this.locExtender.SetLocalizingId(this._undoToolStripMenuItem, "OralAnnotationRecorderBaseDlg._undoToolStripMenuItem");
			this._undoToolStripMenuItem.Name = "_undoToolStripMenuItem";
			this._undoToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this._undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this._undoToolStripMenuItem.ShowShortcutKeys = false;
			this._undoToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
			this._undoToolStripMenuItem.Text = "Undo";
			this._undoToolStripMenuItem.Click += new System.EventHandler(this.HandleUndoButtonClick);
			//
			// _videoHelpMenuStrip
			//
			this._videoHelpMenuStrip.AllowMerge = false;
			this._videoHelpMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(129)))), ((int)(((byte)(199)))));
			this._videoHelpMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
			this._videoHelpMenuStrip.ImageScalingSize = new System.Drawing.Size(30, 30);
			this._videoHelpMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._videoHelpMenu});
			this.locExtender.SetLocalizableToolTip(this._videoHelpMenuStrip, null);
			this.locExtender.SetLocalizationComment(this._videoHelpMenuStrip, null);
			this.locExtender.SetLocalizationPriority(this._videoHelpMenuStrip, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._videoHelpMenuStrip, "OralAnnotationRecorderBaseDlg._videoHelpMenuStrip");
			this._videoHelpMenuStrip.Location = new System.Drawing.Point(50, 385);
			this._videoHelpMenuStrip.Name = "_videoHelpMenuStrip";
			this._videoHelpMenuStrip.Padding = new System.Windows.Forms.Padding(1, 1, 0, 2);
			this._videoHelpMenuStrip.ShowItemToolTips = true;
			this._videoHelpMenuStrip.Size = new System.Drawing.Size(201, 37);
			this._videoHelpMenuStrip.TabIndex = 18;
			//
			// _videoHelpMenu
			//
			this._videoHelpMenu.BackColor = System.Drawing.Color.Transparent;
			this._videoHelpMenu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._videoHelpMenu.ForeColor = System.Drawing.Color.White;
			this._videoHelpMenu.Image = global::SayMore.Properties.Resources.ShowVideoHelp;
			this.locExtender.SetLocalizableToolTip(this._videoHelpMenu, null);
			this.locExtender.SetLocalizationComment(this._videoHelpMenu, null);
			this.locExtender.SetLocalizationPriority(this._videoHelpMenu, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._videoHelpMenu, "OralAnnotationRecorderBaseDlg._videoHelpMenu");
			this._videoHelpMenu.Name = "_videoHelpMenu";
			this._videoHelpMenu.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this._videoHelpMenu.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this._videoHelpMenu.ShowShortcutKeys = false;
			this._videoHelpMenu.Size = new System.Drawing.Size(106, 34);
			this._videoHelpMenu.Text = "Video Help";
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
			this._tableLayoutRecordAnnotations.BackColor = System.Drawing.Color.MintCream;
			this._tableLayoutRecordAnnotations.ColumnCount = 2;
			this._tableLayoutRecordAnnotations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutRecordAnnotations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutRecordAnnotations.Controls.Add(this._labelRecordButton, 0, 1);
			this._tableLayoutRecordAnnotations.Controls.Add(this._panelPeakMeter, 1, 2);
			this._tableLayoutRecordAnnotations.Location = new System.Drawing.Point(0, 164);
			this._tableLayoutRecordAnnotations.Margin = new System.Windows.Forms.Padding(0, 1, 1, 0);
			this._tableLayoutRecordAnnotations.Name = "_tableLayoutRecordAnnotations";
			this._tableLayoutRecordAnnotations.RowCount = 3;
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutRecordAnnotations.Size = new System.Drawing.Size(145, 149);
			this._tableLayoutRecordAnnotations.TabIndex = 1;
			//
			// _tableLayoutMediaButtons
			//
			this._tableLayoutMediaButtons.ColumnCount = 1;
			this._tableLayoutMediaButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMediaButtons.Controls.Add(this._tableLayoutRecordAnnotations, 0, 2);
			this._tableLayoutMediaButtons.Controls.Add(this._panelListen, 0, 1);
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
			// _checkForRecordingDevice
			//
			this._checkForRecordingDevice.Interval = 500;
			this._checkForRecordingDevice.Tick += new System.EventHandler(this.CheckForRecordingDevice);
			//
			// OralAnnotationRecorderBaseDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 488);
			this.Controls.Add(this._videoHelpMenuStrip);
			this.Controls.Add(this._lastSegmentMenuStrip);
			this.Controls.Add(this._pictureIcon);
			this.Controls.Add(this._labelFinishedHint);
			this.Controls.Add(this._labelRecordHint);
			this.Controls.Add(this._pictureRecording);
			this.Controls.Add(this._labelListenHint);
			this.Controls.Add(this._labelErrorInfo);
			this.Controls.Add(this._tableLayoutMediaButtons);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, "Localized in subclass");
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.WindowTitle");
			this.MainMenuStrip = this._lastSegmentMenuStrip;
			this.MinimumSize = new System.Drawing.Size(446, 526);
			this.Name = "OralAnnotationRecorderBaseDlg";
			this.Opacity = 1D;
			this.Text = "Change my text";
			this.Controls.SetChildIndex(this._tableLayoutMediaButtons, 0);
			this.Controls.SetChildIndex(this._labelErrorInfo, 0);
			this.Controls.SetChildIndex(this._labelListenHint, 0);
			this.Controls.SetChildIndex(this._pictureRecording, 0);
			this.Controls.SetChildIndex(this._labelRecordHint, 0);
			this.Controls.SetChildIndex(this._labelFinishedHint, 0);
			this.Controls.SetChildIndex(this._pictureIcon, 0);
			this.Controls.SetChildIndex(this._lastSegmentMenuStrip, 0);
			this.Controls.SetChildIndex(this._videoHelpMenuStrip, 0);
			this._panelListen.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureRecording)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureIcon)).EndInit();
			this._lastSegmentMenuStrip.ResumeLayout(false);
			this._lastSegmentMenuStrip.PerformLayout();
			this._videoHelpMenuStrip.ResumeLayout(false);
			this._videoHelpMenuStrip.PerformLayout();
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
		private System.Windows.Forms.Label _labelErrorInfo;
		private System.Windows.Forms.Label _labelRecordHint;
		private SilPanel _panelPeakMeter;
		private System.Windows.Forms.Label _labelListenHint;
		private System.Windows.Forms.Label _labelFinishedHint;
		private System.Windows.Forms.Timer _checkForRecordingDevice;
		private System.Windows.Forms.Panel _panelListen;
		private System.Windows.Forms.PictureBox _pictureIcon;
		private System.Windows.Forms.MenuStrip _lastSegmentMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem _undoToolStripMenuItem;
		private System.Windows.Forms.MenuStrip _videoHelpMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem _videoHelpMenu;
	}
}
