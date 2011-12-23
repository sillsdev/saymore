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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			//
			// _waveControl
			//
			this._waveControl.AutoScrollMinSize = new System.Drawing.Size(0, 99);
			this.locExtender.SetLocalizableToolTip(this._waveControl, null);
			this.locExtender.SetLocalizationComment(this._waveControl, null);
			this.locExtender.SetLocalizationPriority(this._waveControl, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._waveControl, "ManualSegmenterDlg._waveControl");
			this._waveControl.Size = new System.Drawing.Size(650, 99);
			//
			// _buttonAddSegmentBoundary
			//
			this._buttonAddSegmentBoundary.BackColor = System.Drawing.Color.Transparent;
			this._buttonAddSegmentBoundary.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonAddSegmentBoundary.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._buttonAddSegmentBoundary.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonAddSegmentBoundary, null);
			this.locExtender.SetLocalizationComment(this._buttonAddSegmentBoundary, null);
			this.locExtender.SetLocalizingId(this._buttonAddSegmentBoundary, "DialogBoxes.Transcription.ManualSegmenterDlg._buttonAddSegmentBoundary.Normal");
			this._buttonAddSegmentBoundary.Margin = new System.Windows.Forms.Padding(0, 10, 0, 2);
			this._buttonAddSegmentBoundary.Name = "_buttonAddSegmentBoundary";
			this._buttonAddSegmentBoundary.Size = new System.Drawing.Size(234, 19);
			this._buttonAddSegmentBoundary.Text = "Add Segment Boundary (press ENTER key)";
			this._buttonAddSegmentBoundary.ToolTipText = "Add segment boundary";
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// toolStrip1
			//
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._buttonAddSegmentBoundary});
			this.locExtender.SetLocalizableToolTip(this.toolStrip1, null);
			this.locExtender.SetLocalizationComment(this.toolStrip1, null);
			this.locExtender.SetLocalizationPriority(this.toolStrip1, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.toolStrip1, "toolStrip1.toolStrip1");
			this.toolStrip1.Location = new System.Drawing.Point(3, 305);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(246, 31);
			this.toolStrip1.TabIndex = 7;
			this.toolStrip1.Text = "toolStrip1";
			//
			// ManualSegmenterDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(682, 338);
			this.Controls.Add(this.toolStrip1);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.ManualSegmenterDlg.WindowTitle");
			this.Name = "ManualSegmenterDlg";
			this.Opacity = 1D;
			this.Text = "Manual Segmenter";
			this.Controls.SetChildIndex(this.toolStrip1, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStripButton _buttonAddSegmentBoundary;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.ToolStrip toolStrip1;
	}
}
