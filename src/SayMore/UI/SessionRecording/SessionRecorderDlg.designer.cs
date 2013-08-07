
namespace SayMore.UI.SessionRecording
{
	partial class SessionRecorderDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._buttonOK = new System.Windows.Forms.Button();
			this._labelRecordingFormat = new System.Windows.Forms.Label();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._panelPeakMeter = new Palaso.UI.WindowsForms.Widgets.EnhancedPanel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this._buttonRecord = new System.Windows.Forms.ToolStripButton();
			this._buttonPlayback = new System.Windows.Forms.ToolStripButton();
			this._buttonStop = new System.Windows.Forms.ToolStripButton();
			this._labelRecLength = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			//
			// tableLayoutPanel1
			//
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._buttonOK, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this._labelRecordingFormat, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this._buttonCancel, 3, 3);
			this.tableLayoutPanel1.Controls.Add(this._panelPeakMeter, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._labelRecLength, 0, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(354, 187);
			this.tableLayoutPanel1.TabIndex = 1;
			//
			// _buttonOK
			//
			this._buttonOK.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._buttonOK, null);
			this.locExtender.SetLocalizationComment(this._buttonOK, null);
			this.locExtender.SetLocalizingId(this._buttonOK, "DialogBoxes.SessionRecorderDlg.OKButtonText");
			this._buttonOK.Location = new System.Drawing.Point(198, 161);
			this._buttonOK.Margin = new System.Windows.Forms.Padding(0, 8, 3, 0);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(75, 26);
			this._buttonOK.TabIndex = 2;
			this._buttonOK.Text = "OK";
			this._buttonOK.UseVisualStyleBackColor = true;
			//
			// _labelRecordingFormat
			//
			this._labelRecordingFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._labelRecordingFormat.AutoEllipsis = true;
			this._labelRecordingFormat.AutoSize = true;
			this._labelRecordingFormat.ForeColor = System.Drawing.Color.DarkRed;
			this.locExtender.SetLocalizableToolTip(this._labelRecordingFormat, null);
			this.locExtender.SetLocalizationComment(this._labelRecordingFormat, null);
			this.locExtender.SetLocalizingId(this._labelRecordingFormat, "DialogBoxes.SessionRecorderDlg.RecordingFormatLabel");
			this._labelRecordingFormat.Location = new System.Drawing.Point(23, 171);
			this._labelRecordingFormat.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this._labelRecordingFormat.Name = "_labelRecordingFormat";
			this._labelRecordingFormat.Size = new System.Drawing.Size(172, 13);
			this._labelRecordingFormat.TabIndex = 2;
			this._labelRecordingFormat.Text = "Format: {0} bits, {1} Hz";
			//
			// _buttonCancel
			//
			this._buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonCancel.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this._buttonCancel, 2);
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.SessionRecorderDlg.CancelButtonText");
			this._buttonCancel.Location = new System.Drawing.Point(279, 161);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(3, 8, 0, 0);
			this._buttonCancel.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 2;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			//
			// _panelPeakMeter
			//
			this._panelPeakMeter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this._panelPeakMeter.BackColor = System.Drawing.Color.White;
			this._panelPeakMeter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelPeakMeter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelPeakMeter.ClipTextForChildControls = true;
			this._panelPeakMeter.ControlReceivingFocusOnMnemonic = null;
			this._panelPeakMeter.DoubleBuffered = true;
			this._panelPeakMeter.DrawOnlyBottomBorder = false;
			this._panelPeakMeter.DrawOnlyTopBorder = false;
			this._panelPeakMeter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelPeakMeter.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelPeakMeter, null);
			this.locExtender.SetLocalizationComment(this._panelPeakMeter, null);
			this.locExtender.SetLocalizationPriority(this._panelPeakMeter, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._panelPeakMeter, "SessionRecorderDlg.panel1");
			this._panelPeakMeter.Location = new System.Drawing.Point(337, 0);
			this._panelPeakMeter.Margin = new System.Windows.Forms.Padding(0);
			this._panelPeakMeter.MnemonicGeneratesClick = false;
			this._panelPeakMeter.Name = "_panelPeakMeter";
			this._panelPeakMeter.Padding = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this._panelPeakMeter.PaintExplorerBarBackground = false;
			this._panelPeakMeter.Size = new System.Drawing.Size(17, 135);
			this._panelPeakMeter.TabIndex = 4;
			//
			// toolStrip1
			//
			this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
			this.tableLayoutPanel1.SetColumnSpan(this.toolStrip1, 4);
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._buttonRecord,
			this._buttonPlayback,
			this._buttonStop});
			this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
			this.locExtender.SetLocalizableToolTip(this.toolStrip1, null);
			this.locExtender.SetLocalizationComment(this.toolStrip1, null);
			this.locExtender.SetLocalizationPriority(this.toolStrip1, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.toolStrip1, "toolStrip1.toolStrip1");
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(226, 87);
			this.toolStrip1.TabIndex = 12;
			//
			// _buttonRecord
			//
			this._buttonRecord.AutoSize = false;
			this._buttonRecord.Image = global::SayMore.Properties.Resources.RecordStart;
			this._buttonRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonRecord.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonRecord, null);
			this.locExtender.SetLocalizationComment(this._buttonRecord, null);
			this.locExtender.SetLocalizingId(this._buttonRecord, "DialogBoxes.SessionRecorderDlg.RecordButtonText");
			this._buttonRecord.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._buttonRecord.Name = "_buttonRecord";
			this._buttonRecord.Size = new System.Drawing.Size(225, 24);
			this._buttonRecord.Text = "Record";
			this._buttonRecord.Click += new System.EventHandler(this.HandleRecordClick);
			//
			// _buttonPlayback
			//
			this._buttonPlayback.AutoSize = false;
			this._buttonPlayback.Image = global::SayMore.Properties.Resources.RecordingPlayback;
			this._buttonPlayback.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonPlayback.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonPlayback.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonPlayback, null);
			this.locExtender.SetLocalizationComment(this._buttonPlayback, null);
			this.locExtender.SetLocalizingId(this._buttonPlayback, "DialogBoxes.SessionRecorderDlg.PlaybackButtonText");
			this._buttonPlayback.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._buttonPlayback.Name = "_buttonPlayback";
			this._buttonPlayback.Size = new System.Drawing.Size(225, 24);
			this._buttonPlayback.Text = "Playback Recording";
			this._buttonPlayback.Click += new System.EventHandler(this.HandlePlaybackButtonClick);
			//
			// _buttonStop
			//
			this._buttonStop.AutoSize = false;
			this._buttonStop.Image = global::SayMore.Properties.Resources.RecordStop;
			this._buttonStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonStop, null);
			this.locExtender.SetLocalizationComment(this._buttonStop, null);
			this.locExtender.SetLocalizingId(this._buttonStop, "DialogBoxes.SessionRecorderDlg.StopButtonText");
			this._buttonStop.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._buttonStop.Name = "_buttonStop";
			this._buttonStop.Size = new System.Drawing.Size(225, 24);
			this._buttonStop.Text = "Stop";
			this._buttonStop.Click += new System.EventHandler(this.HandleStopClick);
			//
			// _labelRecLength
			//
			this._labelRecLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._labelRecLength.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this._labelRecLength, 4);
			this.locExtender.SetLocalizableToolTip(this._labelRecLength, null);
			this.locExtender.SetLocalizationComment(this._labelRecLength, null);
			this.locExtender.SetLocalizingId(this._labelRecLength, "DialogBoxes.SessionRecorderDlg.RecordedLengthLabel");
			this._labelRecLength.Location = new System.Drawing.Point(0, 140);
			this._labelRecLength.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelRecLength.Name = "_labelRecLength";
			this._labelRecLength.Size = new System.Drawing.Size(332, 13);
			this._labelRecLength.TabIndex = 2;
			this._labelRecLength.Text = "Recorded Length: {0} sec.";
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// SessionRecorderDlg
			//
			this.AcceptButton = this._buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(378, 211);
			this.Controls.Add(this.tableLayoutPanel1);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.SessionRecorderDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(340, 225);
			this.Name = "SessionRecorderDlg";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Session Recorder";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label _labelRecordingFormat;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton _buttonRecord;
		private System.Windows.Forms.ToolStripButton _buttonPlayback;
		private System.Windows.Forms.Button _buttonCancel;
		private Palaso.UI.WindowsForms.Widgets.EnhancedPanel _panelPeakMeter;
		private System.Windows.Forms.ToolStripButton _buttonStop;
		private System.Windows.Forms.Button _buttonOK;
		private System.Windows.Forms.Label _labelRecLength;
	}
}
