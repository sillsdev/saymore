using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class ManualSegmenterDlg
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
			this._waveControl = new SayMore.AudioUtils.WaveControl();
			this._panelWaveForm = new SilTools.Controls.SilPanel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this._buttonListenToOriginal = new System.Windows.Forms.ToolStripButton();
			this._buttonRecordAnnotation = new System.Windows.Forms.ToolStripButton();
			this._buttonListenToAnnotation = new System.Windows.Forms.ToolStripButton();
			this._buttonEraseAnnotation = new System.Windows.Forms.ToolStripButton();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._labelRecordingType = new System.Windows.Forms.Label();
			this._labelTimeDisplay = new System.Windows.Forms.Label();
			this._comboBoxZoom = new System.Windows.Forms.ComboBox();
			this._labelZoom = new System.Windows.Forms.Label();
			this._buttonClose = new System.Windows.Forms.Button();
			this._panelWaveForm.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			//
			// _waveControl
			//
			this._waveControl.AutoScroll = true;
			this._waveControl.AutoScrollMinSize = new System.Drawing.Size(0, 101);
			this._waveControl.BackColor = System.Drawing.SystemColors.Window;
			this._waveControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._waveControl.ForeColor = System.Drawing.SystemColors.WindowText;
			this._waveControl.Location = new System.Drawing.Point(0, 0);
			this._waveControl.Name = "_waveControl";
			this._waveControl.ShadePlaybackAreaDuringPlayback = false;
			this._waveControl.Size = new System.Drawing.Size(650, 101);
			this._waveControl.TabIndex = 0;
			this._waveControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HandleWaveControlMouseClick);
			//
			// _panelWaveForm
			//
			this._panelWaveForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._panelWaveForm.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelWaveForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelWaveForm.ClipTextForChildControls = true;
			this.tableLayoutPanel1.SetColumnSpan(this._panelWaveForm, 3);
			this._panelWaveForm.ControlReceivingFocusOnMnemonic = null;
			this._panelWaveForm.Controls.Add(this._waveControl);
			this._panelWaveForm.DoubleBuffered = true;
			this._panelWaveForm.DrawOnlyBottomBorder = false;
			this._panelWaveForm.DrawOnlyTopBorder = false;
			this._panelWaveForm.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
			this._panelWaveForm.ForeColor = System.Drawing.SystemColors.ControlText;
			this._panelWaveForm.Location = new System.Drawing.Point(0, 24);
			this._panelWaveForm.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._panelWaveForm.MnemonicGeneratesClick = false;
			this._panelWaveForm.Name = "_panelWaveForm";
			this._panelWaveForm.PaintExplorerBarBackground = false;
			this._panelWaveForm.Size = new System.Drawing.Size(652, 103);
			this._panelWaveForm.TabIndex = 1;
			//
			// toolStrip1
			//
			this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._buttonListenToOriginal,
			this._buttonRecordAnnotation,
			this._buttonListenToAnnotation,
			this._buttonEraseAnnotation});
			this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this.toolStrip1.Location = new System.Drawing.Point(0, 140);
			this.toolStrip1.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.tableLayoutPanel1.SetRowSpan(this.toolStrip1, 2);
			this.toolStrip1.Size = new System.Drawing.Size(158, 134);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			//
			// _buttonListenToOriginal
			//
			this._buttonListenToOriginal.BackColor = System.Drawing.Color.Transparent;
			this._buttonListenToOriginal.Image = global::SayMore.Properties.Resources.RecordingPlayback;
			this._buttonListenToOriginal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonListenToOriginal.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonListenToOriginal.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonListenToOriginal.Name = "_buttonListenToOriginal";
			this._buttonListenToOriginal.Size = new System.Drawing.Size(156, 24);
			this._buttonListenToOriginal.Text = "Listen (hold CTRL key)";
			this._buttonListenToOriginal.ToolTipText = "Listen to original recording";
			this._buttonListenToOriginal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleListenToOriginalMouseDown);
			this._buttonListenToOriginal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandleListenToOriginalMouseUp);
			//
			// _buttonRecordAnnotation
			//
			this._buttonRecordAnnotation.BackColor = System.Drawing.Color.Transparent;
			this._buttonRecordAnnotation.Image = global::SayMore.Properties.Resources.RecordStart;
			this._buttonRecordAnnotation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonRecordAnnotation.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonRecordAnnotation.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonRecordAnnotation.Margin = new System.Windows.Forms.Padding(0, 10, 0, 2);
			this._buttonRecordAnnotation.Name = "_buttonRecordAnnotation";
			this._buttonRecordAnnotation.Size = new System.Drawing.Size(156, 24);
			this._buttonRecordAnnotation.Text = "Record (hold SPACE key)";
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
			this._buttonListenToAnnotation.Margin = new System.Windows.Forms.Padding(0, 10, 0, 2);
			this._buttonListenToAnnotation.Name = "_buttonListenToAnnotation";
			this._buttonListenToAnnotation.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this._buttonListenToAnnotation.Size = new System.Drawing.Size(156, 24);
			this._buttonListenToAnnotation.Text = "Listen To Annotation";
			this._buttonListenToAnnotation.Click += new System.EventHandler(this.HandlePlaybackAnnotationClick);
			//
			// _buttonEraseAnnotation
			//
			this._buttonEraseAnnotation.BackColor = System.Drawing.Color.Transparent;
			this._buttonEraseAnnotation.Image = global::SayMore.Properties.Resources.RecordErase;
			this._buttonEraseAnnotation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonEraseAnnotation.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonEraseAnnotation.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonEraseAnnotation.Margin = new System.Windows.Forms.Padding(24, 5, 0, 2);
			this._buttonEraseAnnotation.Name = "_buttonEraseAnnotation";
			this._buttonEraseAnnotation.Size = new System.Drawing.Size(132, 26);
			this._buttonEraseAnnotation.Text = "Erase Annotation";
			this._buttonEraseAnnotation.Click += new System.EventHandler(this.HandleEraseAnnotationClick);
			//
			// tableLayoutPanel1
			//
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._panelWaveForm, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.toolStrip1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this._labelRecordingType, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._labelTimeDisplay, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this._comboBoxZoom, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this._labelZoom, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this._buttonClose, 2, 3);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 15);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(652, 308);
			this.tableLayoutPanel1.TabIndex = 6;
			//
			// _labelRecordingType
			//
			this._labelRecordingType.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelRecordingType.AutoSize = true;
			this._labelRecordingType.Location = new System.Drawing.Point(0, 5);
			this._labelRecordingType.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelRecordingType.Name = "_labelRecordingType";
			this._labelRecordingType.Size = new System.Drawing.Size(80, 13);
			this._labelRecordingType.TabIndex = 4;
			this._labelRecordingType.Text = "Careful Speech";
			//
			// _labelTimeDisplay
			//
			this._labelTimeDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelTimeDisplay.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this._labelTimeDisplay, 2);
			this._labelTimeDisplay.Location = new System.Drawing.Point(635, 132);
			this._labelTimeDisplay.Name = "_labelTimeDisplay";
			this._labelTimeDisplay.Size = new System.Drawing.Size(14, 13);
			this._labelTimeDisplay.TabIndex = 3;
			this._labelTimeDisplay.Text = "#";
			//
			// _comboBoxZoom
			//
			this._comboBoxZoom.BackColor = System.Drawing.Color.White;
			this._comboBoxZoom.FormattingEnabled = true;
			this._comboBoxZoom.Items.AddRange(new object[] {
			"100%",
			"125%",
			"150%",
			"175%",
			"200%",
			"250%",
			"300%",
			"500%",
			"750%",
			"1000%"});
			this._comboBoxZoom.Location = new System.Drawing.Point(556, 0);
			this._comboBoxZoom.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
			this._comboBoxZoom.Name = "_comboBoxZoom";
			this._comboBoxZoom.Size = new System.Drawing.Size(96, 21);
			this._comboBoxZoom.TabIndex = 5;
			this._comboBoxZoom.SelectedIndexChanged += new System.EventHandler(this.HandleZoomSelectedIndexChanged);
			this._comboBoxZoom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleZoomKeyDown);
			this._comboBoxZoom.Validating += new System.ComponentModel.CancelEventHandler(this.HandleZoomComboValidating);
			//
			// _labelZoom
			//
			this._labelZoom.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._labelZoom.AutoSize = true;
			this._labelZoom.Location = new System.Drawing.Point(513, 5);
			this._labelZoom.Name = "_labelZoom";
			this._labelZoom.Size = new System.Drawing.Size(37, 13);
			this._labelZoom.TabIndex = 6;
			this._labelZoom.Text = "&Zoom:";
			//
			// _buttonClose
			//
			this._buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonClose.Location = new System.Drawing.Point(577, 282);
			this._buttonClose.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._buttonClose.Name = "_buttonClose";
			this._buttonClose.Size = new System.Drawing.Size(75, 26);
			this._buttonClose.TabIndex = 7;
			this._buttonClose.Text = "Close";
			this._buttonClose.UseVisualStyleBackColor = true;
			this._buttonClose.Click += new System.EventHandler(this.HandleCloseClick);
			//
			// ManualSegmenterDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(682, 338);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(330, 300);
			this.Name = "ManualSegmenterDlg";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Manual Segmenter";
			this._panelWaveForm.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private AudioUtils.WaveControl _waveControl;
		private SilPanel _panelWaveForm;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton _buttonListenToOriginal;
		private System.Windows.Forms.ToolStripButton _buttonRecordAnnotation;
		private System.Windows.Forms.ToolStripButton _buttonListenToAnnotation;
		private System.Windows.Forms.ToolStripButton _buttonEraseAnnotation;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label _labelTimeDisplay;
		private System.Windows.Forms.Label _labelRecordingType;
		private System.Windows.Forms.ComboBox _comboBoxZoom;
		private System.Windows.Forms.Label _labelZoom;
		private System.Windows.Forms.Button _buttonClose;
	}
}
