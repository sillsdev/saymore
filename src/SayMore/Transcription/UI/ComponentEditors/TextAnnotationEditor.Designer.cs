using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class TextAnnotationEditor
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
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._splitter = new System.Windows.Forms.SplitContainer();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._buttonHelp = new System.Windows.Forms.ToolStripButton();
			this._exportMenu = new System.Windows.Forms.ToolStripDropDownButton();
			this._csvExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._exportElanMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._flexInterlinearExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._plainTextExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._exportVernacularSubtitlesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._exportFreeTranslationSubtitlesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._buttonResegment = new System.Windows.Forms.ToolStripButton();
			this._buttonRecordings = new System.Windows.Forms.ToolStripDropDownButton();
			this._buttonCarefulSpeech = new System.Windows.Forms.ToolStripMenuItem();
			this._buttonOralTranslation = new System.Windows.Forms.ToolStripMenuItem();
			this._buttonFonts = new System.Windows.Forms.ToolStripDropDownButton();
			this._buttonTranscriptionFont = new System.Windows.Forms.ToolStripMenuItem();
			this._buttonFreeTranslationFont = new System.Windows.Forms.ToolStripMenuItem();
			this._tableLayoutPlaybackSpeed = new System.Windows.Forms.TableLayoutPanel();
			this._comboPlaybackSpeed = new System.Windows.Forms.ComboBox();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._toolboxInterlinearExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._splitter)).BeginInit();
			this._splitter.SuspendLayout();
			this._toolStrip.SuspendLayout();
			this._tableLayoutPlaybackSpeed.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
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
			this._tableLayout.Size = new System.Drawing.Size(597, 346);
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
			this._splitter.Size = new System.Drawing.Size(597, 313);
			this._splitter.SplitterDistance = 192;
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
            this._exportMenu,
            this._buttonResegment,
            this._buttonRecordings,
            this._buttonFonts});
			this.locExtender.SetLocalizableToolTip(this._toolStrip, null);
			this.locExtender.SetLocalizationComment(this._toolStrip, null);
			this.locExtender.SetLocalizationPriority(this._toolStrip, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._toolStrip, "Transcription.UI.TextAnnotationEditor._toolStrip");
			this._toolStrip.Location = new System.Drawing.Point(52, 0);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(545, 25);
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
			this.locExtender.SetLocalizingId(this._buttonHelp, "SessionsView.Transcription.TextAnnotationEditor.HelpButton");
			this._buttonHelp.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
			this._buttonHelp.Name = "_buttonHelp";
			this._buttonHelp.Size = new System.Drawing.Size(23, 22);
			this._buttonHelp.Text = "Help";
			// 
			// _exportMenu
			// 
			this._exportMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._exportMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._csvExportMenuItem,
            this._exportElanMenuItem,
            this._flexInterlinearExportMenuItem,
            this._plainTextExportMenuItem,
            this._exportVernacularSubtitlesMenuItem,
            this._exportFreeTranslationSubtitlesMenuItem,
            this._toolboxInterlinearExportMenuItem});
			this._exportMenu.Image = global::SayMore.Properties.Resources.InterlinearExport;
			this._exportMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._exportMenu, null);
			this.locExtender.SetLocalizationComment(this._exportMenu, null);
			this.locExtender.SetLocalizationPriority(this._exportMenu, Localization.LocalizationPriority.Medium);
			this.locExtender.SetLocalizingId(this._exportMenu, "SessionsView.Transcription.TextAnnotation.ExportMenu");
			this._exportMenu.Name = "_exportMenu";
			this._exportMenu.Size = new System.Drawing.Size(69, 22);
			this._exportMenu.Text = "Export";
			// 
			// _csvExportMenuItem
			// 
			this.locExtender.SetLocalizableToolTip(this._csvExportMenuItem, null);
			this.locExtender.SetLocalizationComment(this._csvExportMenuItem, null);
			this.locExtender.SetLocalizingId(this._csvExportMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.commaSeparatedValueExport");
			this._csvExportMenuItem.Name = "_csvExportMenuItem";
			this._csvExportMenuItem.Size = new System.Drawing.Size(239, 22);
			this._csvExportMenuItem.Text = "Comma Separated Values File...";
			this._csvExportMenuItem.ToolTipText = "Use this for getting data into a spreadsheet application.";
			this._csvExportMenuItem.Click += new System.EventHandler(this.OnCsvExportMenuItem_Click);
			// 
			// _exportElanMenuItem
			// 
			this.locExtender.SetLocalizableToolTip(this._exportElanMenuItem, null);
			this.locExtender.SetLocalizationComment(this._exportElanMenuItem, null);
			this.locExtender.SetLocalizingId(this._exportElanMenuItem, ".eLANFileToolStripMenuItem");
			this._exportElanMenuItem.Name = "_exportElanMenuItem";
			this._exportElanMenuItem.Size = new System.Drawing.Size(239, 22);
			this._exportElanMenuItem.Text = "ELAN File...";
			this._exportElanMenuItem.Click += new System.EventHandler(this._exportElanMenuItem_Click);
			// 
			// _flexInterlinearExportMenuItem
			// 
			this.locExtender.SetLocalizableToolTip(this._flexInterlinearExportMenuItem, null);
			this.locExtender.SetLocalizationComment(this._flexInterlinearExportMenuItem, null);
			this.locExtender.SetLocalizationPriority(this._flexInterlinearExportMenuItem, Localization.LocalizationPriority.Medium);
			this.locExtender.SetLocalizingId(this._flexInterlinearExportMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.FLExTextExport");
			this._flexInterlinearExportMenuItem.Name = "_flexInterlinearExportMenuItem";
			this._flexInterlinearExportMenuItem.Size = new System.Drawing.Size(239, 22);
			this._flexInterlinearExportMenuItem.Text = "FLEx Interlinear Text...";
			this._flexInterlinearExportMenuItem.Click += new System.EventHandler(this.OnFLexTextExportClick);
			// 
			// _plainTextExportMenuItem
			// 
			this.locExtender.SetLocalizableToolTip(this._plainTextExportMenuItem, null);
			this.locExtender.SetLocalizationComment(this._plainTextExportMenuItem, null);
			this.locExtender.SetLocalizationPriority(this._plainTextExportMenuItem, Localization.LocalizationPriority.Medium);
			this.locExtender.SetLocalizingId(this._plainTextExportMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.plainTextExport");
			this._plainTextExportMenuItem.Name = "_plainTextExportMenuItem";
			this._plainTextExportMenuItem.Size = new System.Drawing.Size(239, 22);
			this._plainTextExportMenuItem.Text = "Plain Text...";
			this._plainTextExportMenuItem.Click += new System.EventHandler(this.OnPlainTextExportMenuItem_Click);
			// 
			// _exportVernacularSubtitlesMenuItem
			// 
			this.locExtender.SetLocalizableToolTip(this._exportVernacularSubtitlesMenuItem, null);
			this.locExtender.SetLocalizationComment(this._exportVernacularSubtitlesMenuItem, null);
			this.locExtender.SetLocalizationPriority(this._exportVernacularSubtitlesMenuItem, Localization.LocalizationPriority.Medium);
			this.locExtender.SetLocalizingId(this._exportVernacularSubtitlesMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.srtVernacularSubtitlesExport" +
        "");
			this._exportVernacularSubtitlesMenuItem.Name = "_exportVernacularSubtitlesMenuItem";
			this._exportVernacularSubtitlesMenuItem.Size = new System.Drawing.Size(239, 22);
			this._exportVernacularSubtitlesMenuItem.Text = "Vernacular Subtitles File...";
			this._exportVernacularSubtitlesMenuItem.ToolTipText = "The SRT format can be used to create subtitle (captioned) video";
			this._exportVernacularSubtitlesMenuItem.Click += new System.EventHandler(this.OnExportVernacularSubtitlesMenuItem_Click);
			// 
			// _exportFreeTranslationSubtitlesMenuItem
			// 
			this.locExtender.SetLocalizableToolTip(this._exportFreeTranslationSubtitlesMenuItem, null);
			this.locExtender.SetLocalizationComment(this._exportFreeTranslationSubtitlesMenuItem, null);
			this.locExtender.SetLocalizationPriority(this._exportFreeTranslationSubtitlesMenuItem, Localization.LocalizationPriority.Medium);
			this.locExtender.SetLocalizingId(this._exportFreeTranslationSubtitlesMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.srtFreeTranslationSubtitlesE" +
        "xport");
			this._exportFreeTranslationSubtitlesMenuItem.Name = "_exportFreeTranslationSubtitlesMenuItem";
			this._exportFreeTranslationSubtitlesMenuItem.Size = new System.Drawing.Size(239, 22);
			this._exportFreeTranslationSubtitlesMenuItem.Text = "Free Translation Subtitles File...";
			this._exportFreeTranslationSubtitlesMenuItem.ToolTipText = "The SRT format can be used to create subtitle (captioned) video";
			this._exportFreeTranslationSubtitlesMenuItem.Click += new System.EventHandler(this.OnExportFreeTranslationSubtitlesMenuItem_Click);
			// 
			// _buttonResegment
			// 
			this._buttonResegment.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonResegment.Image = global::SayMore.Properties.Resources.Segment;
			this._buttonResegment.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonResegment, "Add, remove or move segment boundaries");
			this.locExtender.SetLocalizationComment(this._buttonResegment, null);
			this.locExtender.SetLocalizingId(this._buttonResegment, "SessionsView.Transcription.TextAnnotationEditor.ResegmentButton");
			this._buttonResegment.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
			this._buttonResegment.Name = "_buttonResegment";
			this._buttonResegment.Size = new System.Drawing.Size(83, 22);
			this._buttonResegment.Text = "Segment...";
			this._buttonResegment.ToolTipText = "Add, remove or move segment boundaries";
			this._buttonResegment.Click += new System.EventHandler(this.HandleResegmentButtonClick);
			// 
			// _buttonRecordings
			// 
			this._buttonRecordings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonRecordings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonCarefulSpeech,
            this._buttonOralTranslation});
			this._buttonRecordings.Image = global::SayMore.Properties.Resources.RecordedOralAnnotations;
			this._buttonRecordings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonRecordings, "Record Oral Annotations");
			this.locExtender.SetLocalizationComment(this._buttonRecordings, null);
			this.locExtender.SetLocalizingId(this._buttonRecordings, "SessionsView.Transcription.TextAnnotationEditor.RecordButton");
			this._buttonRecordings.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
			this._buttonRecordings.Name = "_buttonRecordings";
			this._buttonRecordings.Size = new System.Drawing.Size(158, 22);
			this._buttonRecordings.Text = "Oral Annotations Tools";
			this._buttonRecordings.ToolTipText = "Record audio annotations";
			// 
			// _buttonCarefulSpeech
			// 
			this.locExtender.SetLocalizableToolTip(this._buttonCarefulSpeech, null);
			this.locExtender.SetLocalizationComment(this._buttonCarefulSpeech, null);
			this.locExtender.SetLocalizingId(this._buttonCarefulSpeech, "SessionsView.Transcription.TextAnnotationEditor.CarefulSpeechMenuText");
			this._buttonCarefulSpeech.Name = "_buttonCarefulSpeech";
			this._buttonCarefulSpeech.Size = new System.Drawing.Size(167, 22);
			this._buttonCarefulSpeech.Text = "&Careful Speech...";
			this._buttonCarefulSpeech.ToolTipText = "Record slow and careful repetitions";
			this._buttonCarefulSpeech.Click += new System.EventHandler(this.HandleRecordedAnnotationButtonClick);
			// 
			// _buttonOralTranslation
			// 
			this.locExtender.SetLocalizableToolTip(this._buttonOralTranslation, null);
			this.locExtender.SetLocalizationComment(this._buttonOralTranslation, null);
			this.locExtender.SetLocalizingId(this._buttonOralTranslation, "SessionsView.Transcription.TextAnnotationEditor.OralTranslationMenu");
			this._buttonOralTranslation.Name = "_buttonOralTranslation";
			this._buttonOralTranslation.Size = new System.Drawing.Size(167, 22);
			this._buttonOralTranslation.Text = "&Oral Translation...";
			this._buttonOralTranslation.ToolTipText = "Record audio translations";
			this._buttonOralTranslation.Click += new System.EventHandler(this.HandleRecordedAnnotationButtonClick);
			// 
			// _buttonFonts
			// 
			this._buttonFonts.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonFonts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonTranscriptionFont,
            this._buttonFreeTranslationFont});
			this._buttonFonts.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonFonts, null);
			this.locExtender.SetLocalizationComment(this._buttonFonts, null);
			this.locExtender.SetLocalizingId(this._buttonFonts, "SessionsView.Transcription.TextAnnotationEditor.FontsButton");
			this._buttonFonts.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
			this._buttonFonts.Name = "_buttonFonts";
			this._buttonFonts.Size = new System.Drawing.Size(49, 22);
			this._buttonFonts.Text = "Fonts";
			this._buttonFonts.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.HandleFontClick);
			// 
			// _buttonTranscriptionFont
			// 
			this.locExtender.SetLocalizableToolTip(this._buttonTranscriptionFont, null);
			this.locExtender.SetLocalizationComment(this._buttonTranscriptionFont, null);
			this.locExtender.SetLocalizingId(this._buttonTranscriptionFont, "SessionsView.Transcription.TextAnnotationEditor.TranscriptionFontsButton");
			this._buttonTranscriptionFont.Name = "_buttonTranscriptionFont";
			this._buttonTranscriptionFont.Size = new System.Drawing.Size(167, 22);
			this._buttonTranscriptionFont.Text = "&Transcription...";
			// 
			// _buttonFreeTranslationFont
			// 
			this.locExtender.SetLocalizableToolTip(this._buttonFreeTranslationFont, null);
			this.locExtender.SetLocalizationComment(this._buttonFreeTranslationFont, null);
			this.locExtender.SetLocalizingId(this._buttonFreeTranslationFont, "SessionsView.Transcription.TextAnnotationEditor.FreeTranslationFontsButton");
			this._buttonFreeTranslationFont.Name = "_buttonFreeTranslationFont";
			this._buttonFreeTranslationFont.Size = new System.Drawing.Size(167, 22);
			this._buttonFreeTranslationFont.Text = "Free Translation...";
			// 
			// _tableLayoutPlaybackSpeed
			// 
			this._tableLayoutPlaybackSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._tableLayoutPlaybackSpeed.AutoSize = true;
			this._tableLayoutPlaybackSpeed.ColumnCount = 2;
			this._tableLayoutPlaybackSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPlaybackSpeed.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPlaybackSpeed.Controls.Add(this._comboPlaybackSpeed, 0, 0);
			this._tableLayoutPlaybackSpeed.Location = new System.Drawing.Point(0, 2);
			this._tableLayoutPlaybackSpeed.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPlaybackSpeed.Name = "_tableLayoutPlaybackSpeed";
			this._tableLayoutPlaybackSpeed.RowCount = 1;
			this._tableLayoutPlaybackSpeed.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPlaybackSpeed.Size = new System.Drawing.Size(52, 21);
			this._tableLayoutPlaybackSpeed.TabIndex = 0;
			// 
			// _comboPlaybackSpeed
			// 
			this._comboPlaybackSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._comboPlaybackSpeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(240)))), ((int)(((byte)(159)))));
			this._comboPlaybackSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._comboPlaybackSpeed.DropDownWidth = 97;
			this._comboPlaybackSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this._comboPlaybackSpeed.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._comboPlaybackSpeed, null);
			this.locExtender.SetLocalizationComment(this._comboPlaybackSpeed, null);
			this.locExtender.SetLocalizationPriority(this._comboPlaybackSpeed, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._comboPlaybackSpeed, "Transcription.UI.TextAnnotationEditor._comboPlaybackSpeed");
			this._comboPlaybackSpeed.Location = new System.Drawing.Point(3, 0);
			this._comboPlaybackSpeed.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._comboPlaybackSpeed.Name = "_comboPlaybackSpeed";
			this._comboPlaybackSpeed.Size = new System.Drawing.Size(49, 21);
			this._comboPlaybackSpeed.TabIndex = 1;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// _toolboxInterlinearExportMenuItem
			// 
			this.locExtender.SetLocalizableToolTip(this._toolboxInterlinearExportMenuItem, null);
			this.locExtender.SetLocalizationComment(this._toolboxInterlinearExportMenuItem, null);
			this.locExtender.SetLocalizingId(this._toolboxInterlinearExportMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.ToolboxExport");
			this._toolboxInterlinearExportMenuItem.Name = "_toolboxInterlinearExportMenuItem";
			this._toolboxInterlinearExportMenuItem.Size = new System.Drawing.Size(239, 22);
			this._toolboxInterlinearExportMenuItem.Text = "Toolbox File...";
			this._toolboxInterlinearExportMenuItem.Click += new System.EventHandler(this._toolboxInterlinearExportMenuItem_Click);
			// 
			// TextAnnotationEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "Transcription.UI.TextAnnotationEditor.EditorBase");
			this.Name = "TextAnnotationEditor";
			this.Padding = new System.Windows.Forms.Padding(12, 6, 12, 12);
			this.Size = new System.Drawing.Size(621, 364);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._splitter)).EndInit();
			this._splitter.ResumeLayout(false);
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			this._tableLayoutPlaybackSpeed.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.SplitContainer _splitter;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPlaybackSpeed;
		private System.Windows.Forms.ComboBox _comboPlaybackSpeed;
		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ToolStripDropDownButton _buttonRecordings;
		private System.Windows.Forms.ToolStripMenuItem _buttonCarefulSpeech;
		private System.Windows.Forms.ToolStripMenuItem _buttonOralTranslation;
		private System.Windows.Forms.ToolStripButton _buttonHelp;
		private System.Windows.Forms.ToolStripButton _buttonResegment;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.ToolStripDropDownButton _buttonFonts;
		private System.Windows.Forms.ToolStripMenuItem _buttonTranscriptionFont;
		private System.Windows.Forms.ToolStripMenuItem _buttonFreeTranslationFont;
		private System.Windows.Forms.ToolStripDropDownButton _exportMenu;
		private System.Windows.Forms.ToolStripMenuItem _plainTextExportMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _flexInterlinearExportMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _exportFreeTranslationSubtitlesMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _exportVernacularSubtitlesMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _csvExportMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _exportElanMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _toolboxInterlinearExportMenuItem;


	}
}
