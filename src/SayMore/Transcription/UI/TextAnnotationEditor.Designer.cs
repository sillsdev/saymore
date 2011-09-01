using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class TextAnnotationEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextAnnotationEditor));
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._splitter = new System.Windows.Forms.SplitContainer();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._buttonHelp = new System.Windows.Forms.ToolStripButton();
			this._buttonExport = new System.Windows.Forms.ToolStripButton();
			this._buttonRecordings = new System.Windows.Forms.ToolStripDropDownButton();
			this._buttonCarefulSpeech = new System.Windows.Forms.ToolStripMenuItem();
			this._buttonOralTranslation = new System.Windows.Forms.ToolStripMenuItem();
			this._buttonResegment = new System.Windows.Forms.ToolStripButton();
			this._tableLayoutPlaybackSpeed = new System.Windows.Forms.TableLayoutPanel();
			this._comboPlaybackSpeed = new System.Windows.Forms.ComboBox();
			this._labelPlaybackSpeed = new System.Windows.Forms.Label();
			this._tableLayout.SuspendLayout();
			this._splitter.SuspendLayout();
			this._toolStrip.SuspendLayout();
			this._tableLayoutPlaybackSpeed.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._splitter, 0, 1);
			this._tableLayout.Controls.Add(this._toolStrip, 1, 0);
			this._tableLayout.Controls.Add(this._tableLayoutPlaybackSpeed, 0, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(12, 6);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 2;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Size = new System.Drawing.Size(493, 346);
			this._tableLayout.TabIndex = 0;
			// 
			// _splitter
			// 
			this._splitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayout.SetColumnSpan(this._splitter, 2);
			this._splitter.Location = new System.Drawing.Point(0, 33);
			this._splitter.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this._splitter.Name = "_splitter";
			this._splitter.Size = new System.Drawing.Size(493, 313);
			this._splitter.SplitterDistance = 160;
			this._splitter.SplitterWidth = 8;
			this._splitter.TabIndex = 3;
			// 
			// _toolStrip
			// 
			this._toolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._toolStrip.BackColor = System.Drawing.Color.Transparent;
			this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonHelp,
            this._buttonExport,
            this._buttonRecordings,
            this._buttonResegment});
			this._toolStrip.Location = new System.Drawing.Point(191, 0);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(302, 25);
			this._toolStrip.TabIndex = 1;
			// 
			// _buttonHelp
			// 
			this._buttonHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._buttonHelp.Image = global::SayMore.Properties.Resources.Help;
			this._buttonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonHelp.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
			this._buttonHelp.Name = "_buttonHelp";
			this._buttonHelp.Size = new System.Drawing.Size(23, 22);
			this._buttonHelp.Text = "Help";
			// 
			// _buttonExport
			// 
			this._buttonExport.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonExport.Image = global::SayMore.Properties.Resources.InterlinearExport;
			this._buttonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonExport.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
			this._buttonExport.Name = "_buttonExport";
			this._buttonExport.Size = new System.Drawing.Size(70, 22);
			this._buttonExport.Text = "Export...";
			this._buttonExport.ToolTipText = "Export to FieldWorks Interlinear";
			this._buttonExport.Click += new System.EventHandler(this.HandleExportButtonClick);
			// 
			// _buttonRecordings
			// 
			this._buttonRecordings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonRecordings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonCarefulSpeech,
            this._buttonOralTranslation});
			this._buttonRecordings.Image = global::SayMore.Properties.Resources.RecordedOralAnnotations;
			this._buttonRecordings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonRecordings.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
			this._buttonRecordings.Name = "_buttonRecordings";
			this._buttonRecordings.Size = new System.Drawing.Size(72, 22);
			this._buttonRecordings.Text = "Record";
			this._buttonRecordings.ToolTipText = "Record Oral Annotations";
			// 
			// _buttonCarefulSpeech
			// 
			this._buttonCarefulSpeech.Image = global::SayMore.Properties.Resources.CarefulSpeech;
			this._buttonCarefulSpeech.Name = "_buttonCarefulSpeech";
			this._buttonCarefulSpeech.Size = new System.Drawing.Size(163, 22);
			this._buttonCarefulSpeech.Text = "&Careful Speech...";
			this._buttonCarefulSpeech.Click += new System.EventHandler(this.HandleRecordedAnnotationButtonClick);
			// 
			// _buttonOralTranslation
			// 
			this._buttonOralTranslation.Image = global::SayMore.Properties.Resources.OralTranslation;
			this._buttonOralTranslation.Name = "_buttonOralTranslation";
			this._buttonOralTranslation.Size = new System.Drawing.Size(163, 22);
			this._buttonOralTranslation.Text = "&Oral Translation...";
			this._buttonOralTranslation.Click += new System.EventHandler(this.HandleRecordedAnnotationButtonClick);
			// 
			// _buttonResegment
			// 
			this._buttonResegment.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonResegment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._buttonResegment.Image = ((System.Drawing.Image)(resources.GetObject("_buttonResegment.Image")));
			this._buttonResegment.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonResegment.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
			this._buttonResegment.Name = "_buttonResegment";
			this._buttonResegment.Size = new System.Drawing.Size(77, 22);
			this._buttonResegment.Text = "Resegment...";
			this._buttonResegment.ToolTipText = "Regenerate Segments";
			this._buttonResegment.Click += new System.EventHandler(this.HandleResegmentButtonClick);
			// 
			// _tableLayoutPlaybackSpeed
			// 
			this._tableLayoutPlaybackSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._tableLayoutPlaybackSpeed.AutoSize = true;
			this._tableLayoutPlaybackSpeed.ColumnCount = 2;
			this._tableLayoutPlaybackSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPlaybackSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPlaybackSpeed.Controls.Add(this._comboPlaybackSpeed, 0, 0);
			this._tableLayoutPlaybackSpeed.Controls.Add(this._labelPlaybackSpeed, 0, 0);
			this._tableLayoutPlaybackSpeed.Location = new System.Drawing.Point(0, 2);
			this._tableLayoutPlaybackSpeed.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPlaybackSpeed.Name = "_tableLayoutPlaybackSpeed";
			this._tableLayoutPlaybackSpeed.RowCount = 1;
			this._tableLayoutPlaybackSpeed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPlaybackSpeed.Size = new System.Drawing.Size(191, 21);
			this._tableLayoutPlaybackSpeed.TabIndex = 0;
			// 
			// _comboPlaybackSpeed
			// 
			this._comboPlaybackSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._comboPlaybackSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._comboPlaybackSpeed.FormattingEnabled = true;
			this._comboPlaybackSpeed.Location = new System.Drawing.Point(94, 0);
			this._comboPlaybackSpeed.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._comboPlaybackSpeed.Name = "_comboPlaybackSpeed";
			this._comboPlaybackSpeed.Size = new System.Drawing.Size(97, 21);
			this._comboPlaybackSpeed.TabIndex = 1;
			// 
			// _labelPlaybackSpeed
			// 
			this._labelPlaybackSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelPlaybackSpeed.AutoSize = true;
			this._labelPlaybackSpeed.Location = new System.Drawing.Point(0, 4);
			this._labelPlaybackSpeed.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelPlaybackSpeed.Name = "_labelPlaybackSpeed";
			this._labelPlaybackSpeed.Size = new System.Drawing.Size(88, 13);
			this._labelPlaybackSpeed.TabIndex = 0;
			this._labelPlaybackSpeed.Text = "Playback &Speed:";
			// 
			// TextAnnotationEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.Name = "TextAnnotationEditor";
			this.Padding = new System.Windows.Forms.Padding(12, 6, 12, 12);
			this.Size = new System.Drawing.Size(517, 364);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this._splitter.ResumeLayout(false);
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			this._tableLayoutPlaybackSpeed.ResumeLayout(false);
			this._tableLayoutPlaybackSpeed.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.SplitContainer _splitter;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPlaybackSpeed;
        private System.Windows.Forms.ComboBox _comboPlaybackSpeed;
		private System.Windows.Forms.Label _labelPlaybackSpeed;
		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ToolStripDropDownButton _buttonRecordings;
		private System.Windows.Forms.ToolStripButton _buttonExport;
		private System.Windows.Forms.ToolStripMenuItem _buttonCarefulSpeech;
		private System.Windows.Forms.ToolStripMenuItem _buttonOralTranslation;
		private System.Windows.Forms.ToolStripButton _buttonHelp;
		private System.Windows.Forms.ToolStripButton _buttonResegment;


	}
}
