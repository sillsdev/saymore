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
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._buttonHelp = new System.Windows.Forms.ToolStripButton();
            this._exportMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this._audacityExportFreeTranslationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._audacityExportTranscription = new System.Windows.Forms.ToolStripMenuItem();
            this._exportElanMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._flexInterlinearExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._plainTextExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._exportFreeTranslationSubtitlesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._exportSubtitlesTranscription = new System.Windows.Forms.ToolStripMenuItem();
            this._exportFreeTranslationVideoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._exportVideoTranscription = new System.Windows.Forms.ToolStripMenuItem();
            this._csvExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._toolboxInterlinearExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._buttonResegment = new System.Windows.Forms.ToolStripButton();
            this._buttonRecordings = new System.Windows.Forms.ToolStripDropDownButton();
            this._buttonCarefulSpeech = new System.Windows.Forms.ToolStripMenuItem();
            this._buttonOralTranslation = new System.Windows.Forms.ToolStripMenuItem();
            this._comboPlaybackSpeed = new System.Windows.Forms.ToolStripComboBox();
            this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
            this._splitter = new System.Windows.Forms.SplitContainer();
            this._toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._splitter)).BeginInit();
            this._splitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.Transparent;
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonHelp,
            this._exportMenu,
            this._buttonResegment,
            this._buttonRecordings,
            this._comboPlaybackSpeed});
            this.locExtender.SetLocalizableToolTip(this._toolStrip, null);
            this.locExtender.SetLocalizationComment(this._toolStrip, null);
            this.locExtender.SetLocalizationPriority(this._toolStrip, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._toolStrip, "Transcription.UI.TextAnnotationEditor._toolStrip");
            this._toolStrip.Location = new System.Drawing.Point(12, 6);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 1, 11);
            this._toolStrip.Size = new System.Drawing.Size(597, 34);
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
            this._buttonHelp.Size = new System.Drawing.Size(23, 20);
            this._buttonHelp.Text = "Help";
            // 
            // _exportMenu
            // 
            this._exportMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._exportMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._audacityExportFreeTranslationMenuItem,
            this._audacityExportTranscription,
            this._exportElanMenuItem,
            this._flexInterlinearExportMenuItem,
            this._plainTextExportMenuItem,
            this._exportFreeTranslationSubtitlesMenuItem,
            this._exportSubtitlesTranscription,
            this._csvExportMenuItem,
            this._toolboxInterlinearExportMenuItem,
            this._exportFreeTranslationVideoMenuItem,
            this._exportVideoTranscription});
            this._exportMenu.Image = global::SayMore.Properties.Resources.InterlinearExport;
            this._exportMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.locExtender.SetLocalizableToolTip(this._exportMenu, null);
            this.locExtender.SetLocalizationComment(this._exportMenu, null);
            this.locExtender.SetLocalizingId(this._exportMenu, "SessionsView.Transcription.TextAnnotation.ExportMenu");
            this._exportMenu.Name = "_exportMenu";
            this._exportMenu.Size = new System.Drawing.Size(69, 20);
            this._exportMenu.Text = "Export";
            // 
            // _audacityExportFreeTranslationMenuItem
            // 
            this.locExtender.SetLocalizableToolTip(this._audacityExportFreeTranslationMenuItem, null);
            this.locExtender.SetLocalizationComment(this._audacityExportFreeTranslationMenuItem, null);
            this.locExtender.SetLocalizingId(this._audacityExportFreeTranslationMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.AudacityFreeTranslation");
            this._audacityExportFreeTranslationMenuItem.Name = "_audacityExportFreeTranslationMenuItem";
            this._audacityExportFreeTranslationMenuItem.Size = new System.Drawing.Size(277, 22);
            this._audacityExportFreeTranslationMenuItem.Text = "Audacity Label File (Free Translation)...";
            this._audacityExportFreeTranslationMenuItem.Click += new System.EventHandler(this.OnAudacityExportFreeTranslation);
            // 
            // _audacityExportTranscription
            // 
            this.locExtender.SetLocalizableToolTip(this._audacityExportTranscription, null);
            this.locExtender.SetLocalizationComment(this._audacityExportTranscription, null);
            this.locExtender.SetLocalizingId(this._audacityExportTranscription, "SessionsView.Transcription.TextAnnotation.ExportMenu.AudacityTranscription");
            this._audacityExportTranscription.Name = "_audacityExportTranscription";
            this._audacityExportTranscription.Size = new System.Drawing.Size(277, 22);
            this._audacityExportTranscription.Text = "Audacity Label File (Transcription)...";
            this._audacityExportTranscription.Click += new System.EventHandler(this.OnAudacityExportTranscription);
            // 
            // _exportElanMenuItem
            // 
            this.locExtender.SetLocalizableToolTip(this._exportElanMenuItem, null);
            this.locExtender.SetLocalizationComment(this._exportElanMenuItem, null);
            this.locExtender.SetLocalizingId(this._exportElanMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.ExportElanMenuItem");
            this._exportElanMenuItem.Name = "_exportElanMenuItem";
            this._exportElanMenuItem.Size = new System.Drawing.Size(277, 22);
            this._exportElanMenuItem.Text = "ELAN File...";
            this._exportElanMenuItem.Click += new System.EventHandler(this.OnExportElanMenuItem_Click);
            // 
            // _flexInterlinearExportMenuItem
            // 
            this.locExtender.SetLocalizableToolTip(this._flexInterlinearExportMenuItem, null);
            this.locExtender.SetLocalizationComment(this._flexInterlinearExportMenuItem, null);
            this.locExtender.SetLocalizingId(this._flexInterlinearExportMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.FLExTextExport");
            this._flexInterlinearExportMenuItem.Name = "_flexInterlinearExportMenuItem";
            this._flexInterlinearExportMenuItem.Size = new System.Drawing.Size(277, 22);
            this._flexInterlinearExportMenuItem.Text = "FLEx Interlinear Text...";
            this._flexInterlinearExportMenuItem.Click += new System.EventHandler(this.OnFLexTextExportClick);
            // 
            // _plainTextExportMenuItem
            // 
            this.locExtender.SetLocalizableToolTip(this._plainTextExportMenuItem, null);
            this.locExtender.SetLocalizationComment(this._plainTextExportMenuItem, null);
            this.locExtender.SetLocalizingId(this._plainTextExportMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.plainTextExport");
            this._plainTextExportMenuItem.Name = "_plainTextExportMenuItem";
            this._plainTextExportMenuItem.Size = new System.Drawing.Size(277, 22);
            this._plainTextExportMenuItem.Text = "Plain Text...";
            this._plainTextExportMenuItem.Click += new System.EventHandler(this.OnPlainTextExportMenuItem_Click);
            // 
            // _exportFreeTranslationSubtitlesMenuItem
            // 
            this.locExtender.SetLocalizableToolTip(this._exportFreeTranslationSubtitlesMenuItem, null);
            this.locExtender.SetLocalizationComment(this._exportFreeTranslationSubtitlesMenuItem, null);
            this.locExtender.SetLocalizingId(this._exportFreeTranslationSubtitlesMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.srtSubtitlestFreeTranslation" +
        "");
            this._exportFreeTranslationSubtitlesMenuItem.Name = "_exportFreeTranslationSubtitlesMenuItem";
            this._exportFreeTranslationSubtitlesMenuItem.Size = new System.Drawing.Size(277, 22);
            this._exportFreeTranslationSubtitlesMenuItem.Text = "Subtitles File (Free Translation)...";
            this._exportFreeTranslationSubtitlesMenuItem.ToolTipText = "The SRT format can be used to create subtitle (captioned) video";
            this._exportFreeTranslationSubtitlesMenuItem.Click += new System.EventHandler(this.OnExportSubtitlesFreeTranslation);
            // 
            // _exportSubtitlesTranscription
            // 
            this.locExtender.SetLocalizableToolTip(this._exportSubtitlesTranscription, null);
            this.locExtender.SetLocalizationComment(this._exportSubtitlesTranscription, null);
            this.locExtender.SetLocalizingId(this._exportSubtitlesTranscription, "SessionsView.Transcription.TextAnnotation.ExportMenu.srtSubtitlesTranscription");
            this._exportSubtitlesTranscription.Name = "_exportSubtitlesTranscription";
            this._exportSubtitlesTranscription.Size = new System.Drawing.Size(277, 22);
            this._exportSubtitlesTranscription.Text = "Subtitles File (Transcription)...";
            this._exportSubtitlesTranscription.ToolTipText = "The SRT format can be used to create subtitle (captioned) video";
            this._exportSubtitlesTranscription.Click += new System.EventHandler(this.OnExportSubtitlesVernacular);
            // 
            // _exportFreeTranslationVideoMenuItem
            // 
            this.locExtender.SetLocalizableToolTip(this._exportFreeTranslationVideoMenuItem, null);
            this.locExtender.SetLocalizationComment(this._exportFreeTranslationVideoMenuItem, null);
            this.locExtender.SetLocalizingId(this._exportFreeTranslationVideoMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.srtVideoFreeTranslation" +
        "");
            this._exportFreeTranslationVideoMenuItem.Name = "_exportFreeTranslationVideoMenuItem";
            this._exportFreeTranslationVideoMenuItem.Size = new System.Drawing.Size(277, 22);
            this._exportFreeTranslationVideoMenuItem.Text = "Video (Free Translation)...";
            this._exportFreeTranslationVideoMenuItem.ToolTipText = "Video with free translation subtitle will be created";
            this._exportFreeTranslationVideoMenuItem.Click += new System.EventHandler(this.OnExportVideoFreeTranslation);            // Once Enabled is set for this menu, it won't change until the program restarts.
            // Be sure to restart the pprogram whenever there is a new video added.
            this._exportFreeTranslationVideoMenuItem.Enabled = false;
            int index = _file.PathToAnnotatedFile.IndexOf(SayMore.Properties.Settings.Default.StandardAudioFileSuffix);
            string sourceVideoPath = _file.PathToAnnotatedFile.Substring(0, index) + "_Source.mp4";
            if (System.IO.File.Exists(sourceVideoPath))
                this._exportFreeTranslationVideoMenuItem.Enabled = true;

            // 
            // _exportVideoTranscription
            // 
            this.locExtender.SetLocalizableToolTip(this._exportVideoTranscription, null);
            this.locExtender.SetLocalizationComment(this._exportVideoTranscription, null);
            this.locExtender.SetLocalizingId(this._exportVideoTranscription, "SessionsView.Transcription.TextAnnotation.ExportMenu.srtVideoTranscription");
            this._exportVideoTranscription.Name = "_exportVideoTranscription";
            this._exportVideoTranscription.Size = new System.Drawing.Size(277, 22);
            this._exportVideoTranscription.Text = "Video (Transcription)...";
            this._exportVideoTranscription.ToolTipText = "Video with transcription subtitle will be created";
            this._exportVideoTranscription.Click += new System.EventHandler(this.OnExportVideoTranscription);
            // Sharing the value with this._exportFreeTranslationVideoMenuItem.Enabled
            this._exportVideoTranscription.Enabled = this._exportFreeTranslationVideoMenuItem.Enabled;

            // 
            // _csvExportMenuItem
            // 
            this.locExtender.SetLocalizableToolTip(this._csvExportMenuItem, null);
            this.locExtender.SetLocalizationComment(this._csvExportMenuItem, null);
            this.locExtender.SetLocalizingId(this._csvExportMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.commaSeparatedValueExport");
            this._csvExportMenuItem.Name = "_csvExportMenuItem";
            this._csvExportMenuItem.Size = new System.Drawing.Size(277, 22);
            this._csvExportMenuItem.Text = "Spreadsheet (CSV) File...";
            this._csvExportMenuItem.ToolTipText = "Use this for getting data into a spreadsheet application.";
            this._csvExportMenuItem.Click += new System.EventHandler(this.OnCsvExportMenuItem_Click);
            // 
            // _toolboxInterlinearExportMenuItem
            // 
            this.locExtender.SetLocalizableToolTip(this._toolboxInterlinearExportMenuItem, null);
            this.locExtender.SetLocalizationComment(this._toolboxInterlinearExportMenuItem, null);
            this.locExtender.SetLocalizingId(this._toolboxInterlinearExportMenuItem, "SessionsView.Transcription.TextAnnotation.ExportMenu.ToolboxExport");
            this._toolboxInterlinearExportMenuItem.Name = "_toolboxInterlinearExportMenuItem";
            this._toolboxInterlinearExportMenuItem.Size = new System.Drawing.Size(277, 22);
            this._toolboxInterlinearExportMenuItem.Text = "Toolbox File...";
            this._toolboxInterlinearExportMenuItem.Click += new System.EventHandler(this.OnToolboxInterlinearExportMenuItem_Click);
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
            this._buttonResegment.Size = new System.Drawing.Size(83, 20);
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
            this._buttonRecordings.Size = new System.Drawing.Size(158, 20);
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
            // _comboPlaybackSpeed
            // 
            this._comboPlaybackSpeed.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._comboPlaybackSpeed.AutoSize = false;
            this._comboPlaybackSpeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(240)))), ((int)(((byte)(159)))));
            this._comboPlaybackSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboPlaybackSpeed.DropDownWidth = 49;
            this.locExtender.SetLocalizableToolTip(this._comboPlaybackSpeed, "Playback speed");
            this.locExtender.SetLocalizationComment(this._comboPlaybackSpeed, null);
            this.locExtender.SetLocalizingId(this._comboPlaybackSpeed, "SessionsView.Transcription.TextAnnotationEditor._comboPlaybackSpeed");
            this._comboPlaybackSpeed.Name = "_comboPlaybackSpeed";
            this._comboPlaybackSpeed.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this._comboPlaybackSpeed.Size = new System.Drawing.Size(55, 23);
            this._comboPlaybackSpeed.ToolTipText = "Playback speed";
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "SayMore";
            this.locExtender.PrefixForNewItems = null;
            // 
            // _splitter
            // 
            this._splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitter.Location = new System.Drawing.Point(12, 40);
            this._splitter.Margin = new System.Windows.Forms.Padding(3, 21, 3, 3);
            this._splitter.Name = "_splitter";
            this._splitter.Size = new System.Drawing.Size(597, 312);
            this._splitter.SplitterDistance = 192;
            this._splitter.SplitterWidth = 8;
            this._splitter.TabIndex = 3;
            // 
            // TextAnnotationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._splitter);
            this.Controls.Add(this._toolStrip);
            this.locExtender.SetLocalizableToolTip(this, null);
            this.locExtender.SetLocalizationComment(this, null);
            this.locExtender.SetLocalizingId(this, "Transcription.UI.TextAnnotationEditor.EditorBase");
            this.Name = "TextAnnotationEditor";
            this.Padding = new System.Windows.Forms.Padding(12, 6, 12, 12);
            this.Size = new System.Drawing.Size(621, 364);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._splitter)).EndInit();
            this._splitter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ToolStripDropDownButton _buttonRecordings;
		private System.Windows.Forms.ToolStripMenuItem _buttonCarefulSpeech;
		private System.Windows.Forms.ToolStripMenuItem _buttonOralTranslation;
		private System.Windows.Forms.ToolStripButton _buttonHelp;
		private System.Windows.Forms.ToolStripButton _buttonResegment;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.ToolStripDropDownButton _exportMenu;
		private System.Windows.Forms.ToolStripMenuItem _plainTextExportMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _flexInterlinearExportMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _exportFreeTranslationSubtitlesMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _exportSubtitlesTranscription;
        private System.Windows.Forms.ToolStripMenuItem _exportFreeTranslationVideoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _exportVideoTranscription;
        private System.Windows.Forms.ToolStripMenuItem _csvExportMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _exportElanMenuItem;
		private System.Windows.Forms.ToolStripMenuItem _toolboxInterlinearExportMenuItem;
		private System.Windows.Forms.ToolStripComboBox _comboPlaybackSpeed;
		private System.Windows.Forms.SplitContainer _splitter;
        private System.Windows.Forms.ToolStripMenuItem _audacityExportTranscription;
        private System.Windows.Forms.ToolStripMenuItem _audacityExportFreeTranslationMenuItem;
        //private System.Windows.Forms.ToolStripMenuItem videoWithTranscriptionToolStripMenuItem;
        //private System.Windows.Forms.ToolStripMenuItem videoWithTranslationToolStripMenuItem;
	}
}
