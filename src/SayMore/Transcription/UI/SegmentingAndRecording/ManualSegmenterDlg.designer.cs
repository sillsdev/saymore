using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class ManualSegmenterDlg
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
			this._buttonAddSegmentBoundary = new System.Windows.Forms.ToolStripButton();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.toolStripButtons = new System.Windows.Forms.ToolStrip();
			this._buttonListenToOriginal = new System.Windows.Forms.ToolStripButton();
			this._buttonStopOriginal = new System.Windows.Forms.ToolStripButton();
			this._buttonDeleteSegment = new System.Windows.Forms.ToolStripButton();
			this._clearWarningMessageTimer = new System.Windows.Forms.Timer(this.components);
			this._pictureIcon = new System.Windows.Forms.PictureBox();
			this._labelInfo = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.toolStripButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureIcon)).BeginInit();
			this.SuspendLayout();
			//
			// _buttonAddSegmentBoundary
			//
			this._buttonAddSegmentBoundary.BackColor = System.Drawing.Color.Transparent;
			this._buttonAddSegmentBoundary.Image = global::SayMore.Properties.Resources.AddBoundary;
			this._buttonAddSegmentBoundary.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonAddSegmentBoundary.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonAddSegmentBoundary.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonAddSegmentBoundary, null);
			this.locExtender.SetLocalizationComment(this._buttonAddSegmentBoundary, null);
			this.locExtender.SetLocalizingId(this._buttonAddSegmentBoundary, "DialogBoxes.Transcription.ManualSegmenterDlg._buttonAddSegmentBoundary.Normal");
			this._buttonAddSegmentBoundary.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._buttonAddSegmentBoundary.Name = "_buttonAddSegmentBoundary";
			this._buttonAddSegmentBoundary.Size = new System.Drawing.Size(246, 24);
			this._buttonAddSegmentBoundary.Text = "Add Segment Boundary (press ENTER)";
			this._buttonAddSegmentBoundary.ToolTipText = "Add segment boundary";
			this._buttonAddSegmentBoundary.Click += new System.EventHandler(this.HandleAddSegmentBoundaryClick);
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
			this._buttonListenToOriginal,
			this._buttonStopOriginal,
			this._buttonAddSegmentBoundary,
			this._buttonDeleteSegment});
			this.toolStripButtons.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this.locExtender.SetLocalizableToolTip(this.toolStripButtons, null);
			this.locExtender.SetLocalizationComment(this.toolStripButtons, null);
			this.locExtender.SetLocalizationPriority(this.toolStripButtons, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.toolStripButtons, "toolStrip1.toolStrip1");
			this.toolStripButtons.Location = new System.Drawing.Point(0, 106);
			this.toolStripButtons.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this.toolStripButtons.Name = "toolStripButtons";
			this.toolStripButtons.Size = new System.Drawing.Size(248, 122);
			this.toolStripButtons.TabIndex = 7;
			this.toolStripButtons.Text = "toolStrip1";
			this.toolStripButtons.MouseEnter += new System.EventHandler(this.toolStripButtons_MouseEnter);
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
			this.locExtender.SetLocalizingId(this._buttonListenToOriginal, "DialogBoxes.Transcription.ManualSegmenterDlg._buttonListenToOriginal");
			this._buttonListenToOriginal.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._buttonListenToOriginal.Name = "_buttonListenToOriginal";
			this._buttonListenToOriginal.Size = new System.Drawing.Size(246, 24);
			this._buttonListenToOriginal.Text = "Listen (press the SPACE BAR)";
			this._buttonListenToOriginal.ToolTipText = "Listen to source recording";
			this._buttonListenToOriginal.Click += new System.EventHandler(this.HandleListenToOriginalClick);
			//
			// _buttonStopOriginal
			//
			this._buttonStopOriginal.Image = global::SayMore.Properties.Resources.RecordStop;
			this._buttonStopOriginal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonStopOriginal.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.locExtender.SetLocalizableToolTip(this._buttonStopOriginal, null);
			this.locExtender.SetLocalizationComment(this._buttonStopOriginal, null);
			this.locExtender.SetLocalizingId(this._buttonStopOriginal, "DialogBoxes.Transcription.ManualSegmenterDlg._buttonStopOriginal");
			this._buttonStopOriginal.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._buttonStopOriginal.Name = "_buttonStopOriginal";
			this._buttonStopOriginal.Size = new System.Drawing.Size(246, 24);
			this._buttonStopOriginal.Text = "Stop (press the SPACE BAR)";
			//
			// _buttonDeleteSegment
			//
			this._buttonDeleteSegment.Image = global::SayMore.Properties.Resources.RecordErase;
			this._buttonDeleteSegment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonDeleteSegment.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonDeleteSegment.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonDeleteSegment, null);
			this.locExtender.SetLocalizationComment(this._buttonDeleteSegment, null);
			this.locExtender.SetLocalizingId(this._buttonDeleteSegment, "DialogBoxes.Transcription.ManualSegmenterDlg._buttonDeleteBoundary");
			this._buttonDeleteSegment.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._buttonDeleteSegment.Name = "_buttonDeleteSegment";
			this._buttonDeleteSegment.Size = new System.Drawing.Size(246, 26);
			this._buttonDeleteSegment.Text = "Delete Selected Boundary (press DELETE)";
			this._buttonDeleteSegment.Click += new System.EventHandler(this.HandleDeleteSegmentClick);
			//
			// _clearWarningMessageTimer
			//
			this._clearWarningMessageTimer.Interval = 4000;
			//
			// _pictureIcon
			//
			this._pictureIcon.BackColor = System.Drawing.Color.Transparent;
			this._pictureIcon.Image = global::SayMore.Properties.Resources.Information_blue;
			this.locExtender.SetLocalizableToolTip(this._pictureIcon, null);
			this.locExtender.SetLocalizationComment(this._pictureIcon, null);
			this.locExtender.SetLocalizationPriority(this._pictureIcon, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pictureIcon, "pictureBox1.pictureBox1");
			this._pictureIcon.Location = new System.Drawing.Point(8, 316);
			this._pictureIcon.Name = "_pictureIcon";
			this._pictureIcon.Size = new System.Drawing.Size(30, 30);
			this._pictureIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureIcon.TabIndex = 13;
			this._pictureIcon.TabStop = false;
			this._pictureIcon.Visible = false;
			//
			// _labelInfo
			//
			this._labelInfo.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelInfo.AutoSize = true;
			this._labelInfo.BackColor = System.Drawing.Color.Transparent;
			this._labelInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelInfo.ForeColor = System.Drawing.Color.White;
			this.locExtender.SetLocalizableToolTip(this._labelInfo, null);
			this.locExtender.SetLocalizationComment(this._labelInfo, null);
			this.locExtender.SetLocalizationPriority(this._labelInfo, Localization.LocalizationPriority.Medium);
			this.locExtender.SetLocalizingId(this._labelInfo, "ManualSegmenterDlg._labelInfo");
			this._labelInfo.Location = new System.Drawing.Point(41, 326);
			this._labelInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this._labelInfo.Name = "_labelInfo";
			this._labelInfo.Size = new System.Drawing.Size(482, 13);
			this._labelInfo.TabIndex = 14;
			this._labelInfo.Text = "One of the segments delineated by the selected break already has oral annotations" +
	".";
			this._labelInfo.Visible = false;
			//
			// ManualSegmenterDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 362);
			this.Controls.Add(this._labelInfo);
			this.Controls.Add(this._pictureIcon);
			this.Controls.Add(this.toolStripButtons);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.ManualSegmenterDlg.WindowTitle");
			this.Name = "ManualSegmenterDlg";
			this.Opacity = 1D;
			this.Text = "Manual Segmenter";
			this.Controls.SetChildIndex(this.toolStripButtons, 0);
			this.Controls.SetChildIndex(this._pictureIcon, 0);
			this.Controls.SetChildIndex(this._labelInfo, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.toolStripButtons.ResumeLayout(false);
			this.toolStripButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripButton _buttonAddSegmentBoundary;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.ToolStrip toolStripButtons;
		private System.Windows.Forms.ToolStripButton _buttonListenToOriginal;
		private System.Windows.Forms.ToolStripButton _buttonStopOriginal;
		private System.Windows.Forms.ToolStripButton _buttonDeleteSegment;
		private System.Windows.Forms.Timer _clearWarningMessageTimer;
		private System.Windows.Forms.PictureBox _pictureIcon;
		private System.Windows.Forms.Label _labelInfo;
	}
}
