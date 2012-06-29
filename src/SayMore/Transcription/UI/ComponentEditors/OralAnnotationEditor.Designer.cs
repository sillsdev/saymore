using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class OralAnnotationEditor
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
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._buttonHelp = new System.Windows.Forms.ToolStripButton();
			this._buttonPlay = new System.Windows.Forms.ToolStripButton();
			this._buttonStop = new System.Windows.Forms.ToolStripButton();
			this._buttonRegenerate = new System.Windows.Forms.ToolStripButton();
			this._panelOralAnnotationWaveViewer = new SilTools.Controls.SilPanel();
			this._oralAnnotationWaveViewer = new SayMore.Transcription.UI.OralAnnotationWaveViewer();
			this._labelCursorTime = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._tableLayout.SuspendLayout();
			this._toolStrip.SuspendLayout();
			this._panelOralAnnotationWaveViewer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 1;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._toolStrip, 0, 0);
			this._tableLayout.Controls.Add(this._panelOralAnnotationWaveViewer, 0, 1);
			this._tableLayout.Controls.Add(this._labelCursorTime, 0, 2);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(12, 6);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 3;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(488, 346);
			this._tableLayout.TabIndex = 0;
			// 
			// _toolStrip
			// 
			this._toolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._toolStrip.BackColor = System.Drawing.Color.Transparent;
			this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonHelp,
            this._buttonPlay,
            this._buttonStop,
            this._buttonRegenerate});
			this.locExtender.SetLocalizableToolTip(this._toolStrip, null);
			this.locExtender.SetLocalizationComment(this._toolStrip, null);
			this.locExtender.SetLocalizationPriority(this._toolStrip, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._toolStrip, "OralAnnotationEditor._toolStrip");
			this._toolStrip.Location = new System.Drawing.Point(0, 0);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(488, 25);
			this._toolStrip.TabIndex = 1;
			// 
			// _buttonHelp
			// 
			this._buttonHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._buttonHelp.Image = global::SayMore.Properties.Resources.Help;
			this._buttonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonHelp, "Help");
			this.locExtender.SetLocalizationComment(this._buttonHelp, null);
			this.locExtender.SetLocalizingId(this._buttonHelp, "SessionsView.Transcription.GeneratedOralAnnotationView.HelpButton");
			this._buttonHelp.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
			this._buttonHelp.Name = "_buttonHelp";
			this._buttonHelp.Size = new System.Drawing.Size(23, 22);
			this._buttonHelp.Text = "Help";
			// 
			// _buttonPlay
			// 
			this._buttonPlay.Image = global::SayMore.Properties.Resources.PlaySegment;
			this._buttonPlay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonPlay, "Playback Oral Annotation Audio File");
			this.locExtender.SetLocalizationComment(this._buttonPlay, null);
			this.locExtender.SetLocalizingId(this._buttonPlay, "SessionsView.Transcription.GeneratedOralAnnotationView.PlayButton");
			this._buttonPlay.Margin = new System.Windows.Forms.Padding(0, 1, 4, 2);
			this._buttonPlay.Name = "_buttonPlay";
			this._buttonPlay.Size = new System.Drawing.Size(49, 22);
			this._buttonPlay.Text = "Play";
			// 
			// _buttonStop
			// 
			this._buttonStop.Enabled = false;
			this._buttonStop.Image = global::SayMore.Properties.Resources.StopSegment;
			this._buttonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonStop, "Stop Playback");
			this.locExtender.SetLocalizationComment(this._buttonStop, null);
			this.locExtender.SetLocalizingId(this._buttonStop, "SessionsView.Transcription.GeneratedOralAnnotationView.StopButton");
			this._buttonStop.Name = "_buttonStop";
			this._buttonStop.Size = new System.Drawing.Size(51, 22);
			this._buttonStop.Text = "Stop";
			// 
			// _buttonRegenerate
			// 
			this._buttonRegenerate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonRegenerate.Image = global::SayMore.Properties.Resources.RegenerateAnnotationFile;
			this._buttonRegenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonRegenerate, "Regenerate Oral Annotation Audio File");
			this.locExtender.SetLocalizationComment(this._buttonRegenerate, null);
			this.locExtender.SetLocalizingId(this._buttonRegenerate, "SessionsView.Transcription.GeneratedOralAnnotationView.RegenerateButton");
			this._buttonRegenerate.Name = "_buttonRegenerate";
			this._buttonRegenerate.Size = new System.Drawing.Size(86, 22);
			this._buttonRegenerate.Text = "Regenerate";
			// 
			// _panelOralAnnotationWaveViewer
			// 
			this._panelOralAnnotationWaveViewer.AutoSize = true;
			this._panelOralAnnotationWaveViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._panelOralAnnotationWaveViewer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelOralAnnotationWaveViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelOralAnnotationWaveViewer.ClipTextForChildControls = true;
			this._panelOralAnnotationWaveViewer.ControlReceivingFocusOnMnemonic = null;
			this._panelOralAnnotationWaveViewer.Controls.Add(this._oralAnnotationWaveViewer);
			this._panelOralAnnotationWaveViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelOralAnnotationWaveViewer.DoubleBuffered = true;
			this._panelOralAnnotationWaveViewer.DrawOnlyBottomBorder = false;
			this._panelOralAnnotationWaveViewer.DrawOnlyTopBorder = false;
			this._panelOralAnnotationWaveViewer.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
			this._panelOralAnnotationWaveViewer.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelOralAnnotationWaveViewer, null);
			this.locExtender.SetLocalizationComment(this._panelOralAnnotationWaveViewer, null);
			this.locExtender.SetLocalizationPriority(this._panelOralAnnotationWaveViewer, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._panelOralAnnotationWaveViewer, "OralAnnotationEditor._panelOralAnnotationWaveViewer");
			this._panelOralAnnotationWaveViewer.Location = new System.Drawing.Point(0, 33);
			this._panelOralAnnotationWaveViewer.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this._panelOralAnnotationWaveViewer.MnemonicGeneratesClick = false;
			this._panelOralAnnotationWaveViewer.Name = "_panelOralAnnotationWaveViewer";
			this._panelOralAnnotationWaveViewer.PaintExplorerBarBackground = false;
			this._panelOralAnnotationWaveViewer.Size = new System.Drawing.Size(488, 296);
			this._panelOralAnnotationWaveViewer.TabIndex = 2;
			// 
			// _oralAnnotationWaveViewer
			// 
			this._oralAnnotationWaveViewer.BackColor = System.Drawing.SystemColors.Window;
			this._oralAnnotationWaveViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this._oralAnnotationWaveViewer, null);
			this.locExtender.SetLocalizationComment(this._oralAnnotationWaveViewer, null);
			this.locExtender.SetLocalizationPriority(this._oralAnnotationWaveViewer, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._oralAnnotationWaveViewer, "OralAnnotationEditor.OralAnnotationWaveViewer");
			this._oralAnnotationWaveViewer.Location = new System.Drawing.Point(0, 0);
			this._oralAnnotationWaveViewer.Margin = new System.Windows.Forms.Padding(0);
			this._oralAnnotationWaveViewer.Name = "_oralAnnotationWaveViewer";
			this._oralAnnotationWaveViewer.Size = new System.Drawing.Size(486, 294);
			this._oralAnnotationWaveViewer.TabIndex = 0;
			this._oralAnnotationWaveViewer.ZoomPercentage = 100F;
			// 
			// _labelCursorTime
			// 
			this._labelCursorTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelCursorTime.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelCursorTime, null);
			this.locExtender.SetLocalizationComment(this._labelCursorTime, null);
			this.locExtender.SetLocalizationPriority(this._labelCursorTime, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelCursorTime, "label1.label1");
			this._labelCursorTime.Location = new System.Drawing.Point(3, 333);
			this._labelCursorTime.Margin = new System.Windows.Forms.Padding(3, 4, 0, 0);
			this._labelCursorTime.Name = "_labelCursorTime";
			this._labelCursorTime.Size = new System.Drawing.Size(485, 13);
			this._labelCursorTime.TabIndex = 3;
			this._labelCursorTime.Text = "#";
			this._labelCursorTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// OralAnnotationEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "OralAnnotationEditor.EditorBase");
			this.Name = "OralAnnotationEditor";
			this.Padding = new System.Windows.Forms.Padding(12, 6, 12, 12);
			this.Size = new System.Drawing.Size(512, 364);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			this._panelOralAnnotationWaveViewer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ToolStripButton _buttonHelp;
		private SilPanel _panelOralAnnotationWaveViewer;
		private OralAnnotationWaveViewer _oralAnnotationWaveViewer;
		private System.Windows.Forms.ToolStripButton _buttonPlay;
		private System.Windows.Forms.ToolStripButton _buttonStop;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.ToolStripButton _buttonRegenerate;
		private System.Windows.Forms.Label _labelCursorTime;


	}
}
