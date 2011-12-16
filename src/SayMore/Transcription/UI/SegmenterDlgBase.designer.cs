using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class SegmenterDlgBase
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
			this._waveControl = new SayMore.AudioUtils.WaveControl();
			this._panelWaveControl = new SilTools.Controls.SilPanel();
			this._toolStripButtons = new System.Windows.Forms.ToolStrip();
			this._buttonListenToOriginal = new System.Windows.Forms.ToolStripButton();
			this._tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._buttonOK = new System.Windows.Forms.Button();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._tableLayoutTop = new System.Windows.Forms.TableLayoutPanel();
			this._labelOriginalRecording = new System.Windows.Forms.Label();
			this._labelZoom = new System.Windows.Forms.Label();
			this._comboBoxZoom = new System.Windows.Forms.ComboBox();
			this._tableLayoutStatus = new System.Windows.Forms.TableLayoutPanel();
			this._labelSegmentCount = new System.Windows.Forms.Label();
			this._labelTimeDisplay = new System.Windows.Forms.Label();
			this._labelSegment = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._panelWaveControl.SuspendLayout();
			this._toolStripButtons.SuspendLayout();
			this._tableLayoutOuter.SuspendLayout();
			this._tableLayoutTop.SuspendLayout();
			this._tableLayoutStatus.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			//
			// _waveControl
			//
			this._waveControl.AutoScroll = true;
			this._waveControl.BackColor = System.Drawing.SystemColors.Window;
			this._waveControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._waveControl.ForeColor = System.Drawing.SystemColors.WindowText;
			this.locExtender.SetLocalizableToolTip(this._waveControl, null);
			this.locExtender.SetLocalizationComment(this._waveControl, null);
			this.locExtender.SetLocalizationPriority(this._waveControl, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._waveControl, "ManualSegmenterDlg._waveControl");
			this._waveControl.Location = new System.Drawing.Point(0, 0);
			this._waveControl.Name = "_waveControl";
			this._waveControl.ShadePlaybackAreaDuringPlayback = false;
			this._waveControl.Size = new System.Drawing.Size(650, 101);
			this._waveControl.TabIndex = 0;
			//
			// _panelWaveControl
			//
			this._panelWaveControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._panelWaveControl.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelWaveControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelWaveControl.ClipTextForChildControls = true;
			this._tableLayoutOuter.SetColumnSpan(this._panelWaveControl, 4);
			this._panelWaveControl.ControlReceivingFocusOnMnemonic = null;
			this._panelWaveControl.Controls.Add(this._waveControl);
			this._panelWaveControl.DoubleBuffered = true;
			this._panelWaveControl.DrawOnlyBottomBorder = false;
			this._panelWaveControl.DrawOnlyTopBorder = false;
			this._panelWaveControl.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
			this._panelWaveControl.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelWaveControl, null);
			this.locExtender.SetLocalizationComment(this._panelWaveControl, null);
			this.locExtender.SetLocalizingId(this._panelWaveControl, "ManualSegmenterDlg._panelWaveControl");
			this._panelWaveControl.Location = new System.Drawing.Point(0, 24);
			this._panelWaveControl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._panelWaveControl.MnemonicGeneratesClick = false;
			this._panelWaveControl.Name = "_panelWaveControl";
			this._panelWaveControl.PaintExplorerBarBackground = false;
			this._panelWaveControl.Size = new System.Drawing.Size(652, 103);
			this._panelWaveControl.TabIndex = 1;
			//
			// _toolStripButtons
			//
			this._toolStripButtons.BackColor = System.Drawing.Color.Transparent;
			this._toolStripButtons.Dock = System.Windows.Forms.DockStyle.None;
			this._toolStripButtons.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStripButtons.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._buttonListenToOriginal});
			this._toolStripButtons.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this.locExtender.SetLocalizableToolTip(this._toolStripButtons, null);
			this.locExtender.SetLocalizationComment(this._toolStripButtons, null);
			this.locExtender.SetLocalizingId(this._toolStripButtons, "ManualSegmenterDlg.toolStrip1");
			this._toolStripButtons.Location = new System.Drawing.Point(0, 140);
			this._toolStripButtons.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this._toolStripButtons.Name = "_toolStripButtons";
			this._tableLayoutOuter.SetRowSpan(this._toolStripButtons, 2);
			this._toolStripButtons.Size = new System.Drawing.Size(183, 29);
			this._toolStripButtons.TabIndex = 2;
			this._toolStripButtons.Text = "toolStrip1";
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
			this.locExtender.SetLocalizingId(this._buttonListenToOriginal, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._buttonListenToOriginal");
			this._buttonListenToOriginal.Name = "_buttonListenToOriginal";
			this._buttonListenToOriginal.Size = new System.Drawing.Size(181, 24);
			this._buttonListenToOriginal.Text = "Listen (hold CTRL key down)";
			this._buttonListenToOriginal.ToolTipText = "Listen to original recording";
			this._buttonListenToOriginal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleListenToOriginalMouseDown);
			//
			// _tableLayoutOuter
			//
			this._tableLayoutOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutOuter.ColumnCount = 4;
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.Controls.Add(this._panelWaveControl, 0, 1);
			this._tableLayoutOuter.Controls.Add(this._toolStripButtons, 0, 2);
			this._tableLayoutOuter.Controls.Add(this._buttonOK, 2, 3);
			this._tableLayoutOuter.Controls.Add(this._buttonCancel, 3, 3);
			this._tableLayoutOuter.Controls.Add(this._tableLayoutTop, 0, 0);
			this._tableLayoutOuter.Controls.Add(this._tableLayoutStatus, 1, 2);
			this._tableLayoutOuter.Location = new System.Drawing.Point(15, 15);
			this._tableLayoutOuter.Name = "_tableLayoutOuter";
			this._tableLayoutOuter.RowCount = 4;
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.Size = new System.Drawing.Size(652, 308);
			this._tableLayoutOuter.TabIndex = 6;
			//
			// _buttonOK
			//
			this._buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonOK.AutoSize = true;
			this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._buttonOK, null);
			this.locExtender.SetLocalizationComment(this._buttonOK, null);
			this.locExtender.SetLocalizingId(this._buttonOK, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._buttonClose");
			this._buttonOK.Location = new System.Drawing.Point(496, 282);
			this._buttonOK.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this._buttonOK.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(75, 26);
			this._buttonOK.TabIndex = 7;
			this._buttonOK.Text = "OK";
			this._buttonOK.UseVisualStyleBackColor = true;
			//
			// _buttonCancel
			//
			this._buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._buttonCancel");
			this._buttonCancel.Location = new System.Drawing.Point(577, 282);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 8;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			//
			// _tableLayoutTop
			//
			this._tableLayoutTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutTop.AutoSize = true;
			this._tableLayoutTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutTop.ColumnCount = 3;
			this._tableLayoutOuter.SetColumnSpan(this._tableLayoutTop, 4);
			this._tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutTop.Controls.Add(this._labelOriginalRecording, 0, 0);
			this._tableLayoutTop.Controls.Add(this._labelZoom, 1, 0);
			this._tableLayoutTop.Controls.Add(this._comboBoxZoom, 2, 0);
			this._tableLayoutTop.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutTop.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutTop.Name = "_tableLayoutTop";
			this._tableLayoutTop.RowCount = 1;
			this._tableLayoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutTop.Size = new System.Drawing.Size(652, 24);
			this._tableLayoutTop.TabIndex = 9;
			//
			// _labelOriginalRecording
			//
			this._labelOriginalRecording.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelOriginalRecording.AutoSize = true;
			this._labelOriginalRecording.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._labelOriginalRecording, null);
			this.locExtender.SetLocalizationComment(this._labelOriginalRecording, null);
			this.locExtender.SetLocalizingId(this._labelOriginalRecording, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlgDlg._labelOriginalRecording");
			this._labelOriginalRecording.Location = new System.Drawing.Point(0, 5);
			this._labelOriginalRecording.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelOriginalRecording.Name = "_labelOriginalRecording";
			this._labelOriginalRecording.Size = new System.Drawing.Size(94, 13);
			this._labelOriginalRecording.TabIndex = 4;
			this._labelOriginalRecording.Text = "Original Recording";
			//
			// _labelZoom
			//
			this._labelZoom.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._labelZoom.AutoSize = true;
			this._labelZoom.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._labelZoom, null);
			this.locExtender.SetLocalizationComment(this._labelZoom, null);
			this.locExtender.SetLocalizingId(this._labelZoom, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._labelZoom");
			this._labelZoom.Location = new System.Drawing.Point(513, 5);
			this._labelZoom.Name = "_labelZoom";
			this._labelZoom.Size = new System.Drawing.Size(37, 13);
			this._labelZoom.TabIndex = 6;
			this._labelZoom.Text = "&Zoom:";
			//
			// _comboBoxZoom
			//
			this._comboBoxZoom.BackColor = System.Drawing.Color.White;
			this._comboBoxZoom.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._comboBoxZoom, null);
			this.locExtender.SetLocalizationComment(this._comboBoxZoom, null);
			this.locExtender.SetLocalizationPriority(this._comboBoxZoom, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._comboBoxZoom, "ManualSegmenterDlg._comboBoxZoom");
			this._comboBoxZoom.Location = new System.Drawing.Point(556, 0);
			this._comboBoxZoom.Margin = new System.Windows.Forms.Padding(3, 0, 0, 3);
			this._comboBoxZoom.Name = "_comboBoxZoom";
			this._comboBoxZoom.Size = new System.Drawing.Size(96, 21);
			this._comboBoxZoom.TabIndex = 5;
			this._comboBoxZoom.SelectedIndexChanged += new System.EventHandler(this.HandleZoomSelectedIndexChanged);
			this._comboBoxZoom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleZoomKeyDown);
			this._comboBoxZoom.Validating += new System.ComponentModel.CancelEventHandler(this.HandleZoomComboValidating);
			//
			// _tableLayoutStatus
			//
			this._tableLayoutStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutStatus.AutoSize = true;
			this._tableLayoutStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutStatus.ColumnCount = 3;
			this._tableLayoutOuter.SetColumnSpan(this._tableLayoutStatus, 3);
			this._tableLayoutStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutStatus.Controls.Add(this._labelSegmentCount, 0, 0);
			this._tableLayoutStatus.Controls.Add(this._labelTimeDisplay, 2, 0);
			this._tableLayoutStatus.Controls.Add(this._labelSegment, 0, 0);
			this._tableLayoutStatus.Location = new System.Drawing.Point(183, 132);
			this._tableLayoutStatus.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutStatus.Name = "_tableLayoutStatus";
			this._tableLayoutStatus.RowCount = 1;
			this._tableLayoutStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutStatus.Size = new System.Drawing.Size(469, 13);
			this._tableLayoutStatus.TabIndex = 10;
			//
			// _labelSegmentCount
			//
			this._labelSegmentCount.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._labelSegmentCount.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelSegmentCount, null);
			this.locExtender.SetLocalizationComment(this._labelSegmentCount, null);
			this.locExtender.SetLocalizingId(this._labelSegmentCount, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._labelSegment");
			this._labelSegmentCount.Location = new System.Drawing.Point(335, 0);
			this._labelSegmentCount.Margin = new System.Windows.Forms.Padding(0, 0, 40, 0);
			this._labelSegmentCount.Name = "_labelSegmentCount";
			this._labelSegmentCount.Size = new System.Drawing.Size(74, 13);
			this._labelSegmentCount.TabIndex = 5;
			this._labelSegmentCount.Text = "Segments: {0}";
			//
			// _labelTimeDisplay
			//
			this._labelTimeDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelTimeDisplay.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelTimeDisplay, null);
			this.locExtender.SetLocalizationComment(this._labelTimeDisplay, null);
			this.locExtender.SetLocalizationPriority(this._labelTimeDisplay, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelTimeDisplay, "ManualSegmenterDlg._labelTimeDisplay");
			this._labelTimeDisplay.Location = new System.Drawing.Point(452, 0);
			this._labelTimeDisplay.Name = "_labelTimeDisplay";
			this._labelTimeDisplay.Size = new System.Drawing.Size(14, 13);
			this._labelTimeDisplay.TabIndex = 3;
			this._labelTimeDisplay.Text = "#";
			//
			// _labelSegment
			//
			this._labelSegment.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._labelSegment.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelSegment, null);
			this.locExtender.SetLocalizationComment(this._labelSegment, null);
			this.locExtender.SetLocalizingId(this._labelSegment, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._labelSegment");
			this._labelSegment.Location = new System.Drawing.Point(200, 0);
			this._labelSegment.Margin = new System.Windows.Forms.Padding(0, 0, 40, 0);
			this._labelSegment.Name = "_labelSegment";
			this._labelSegment.Size = new System.Drawing.Size(95, 13);
			this._labelSegment.TabIndex = 4;
			this._labelSegment.Text = "Segment {0} of {1}";
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// SegmenterDlgBase
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(682, 338);
			this.Controls.Add(this._tableLayoutOuter);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(330, 300);
			this.Name = "SegmenterDlgBase";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Change this text";
			this._panelWaveControl.ResumeLayout(false);
			this._toolStripButtons.ResumeLayout(false);
			this._toolStripButtons.PerformLayout();
			this._tableLayoutOuter.ResumeLayout(false);
			this._tableLayoutOuter.PerformLayout();
			this._tableLayoutTop.ResumeLayout(false);
			this._tableLayoutTop.PerformLayout();
			this._tableLayoutStatus.ResumeLayout(false);
			this._tableLayoutStatus.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SilPanel _panelWaveControl;
		protected System.Windows.Forms.ToolStripButton _buttonListenToOriginal;
		private System.Windows.Forms.Label _labelTimeDisplay;
		private System.Windows.Forms.Label _labelOriginalRecording;
		private System.Windows.Forms.ComboBox _comboBoxZoom;
		private System.Windows.Forms.Label _labelZoom;
		private System.Windows.Forms.Button _buttonOK;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutTop;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutStatus;
		private System.Windows.Forms.Label _labelSegment;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Label _labelSegmentCount;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
		protected System.Windows.Forms.ToolStrip _toolStripButtons;
		protected AudioUtils.WaveControl _waveControl;
	}
}
