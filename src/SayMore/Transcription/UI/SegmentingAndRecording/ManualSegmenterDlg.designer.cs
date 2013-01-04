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
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.toolStripButtons.SuspendLayout();
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
			this.toolStripButtons.Size = new System.Drawing.Size(248, 141);
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
			// ManualSegmenterDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 362);
			this.Controls.Add(this.toolStripButtons);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.ManualSegmenterDlg.WindowTitle");
			this.Name = "ManualSegmenterDlg";
			this.Opacity = 1D;
			this.Text = "Manual Segmenter";
			this.Controls.SetChildIndex(this.toolStripButtons, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.toolStripButtons.ResumeLayout(false);
			this.toolStripButtons.PerformLayout();
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
	}
}
