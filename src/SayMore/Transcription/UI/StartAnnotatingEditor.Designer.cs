using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class StartAnnotatingEditor
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
			this._radioButtonAutoSegmenter = new System.Windows.Forms.RadioButton();
			this._radioButtonAudacity = new System.Windows.Forms.RadioButton();
			this._radioButtonElan = new System.Windows.Forms.RadioButton();
			this._radioButtonCarefulSpeech = new System.Windows.Forms.RadioButton();
			this._labelSegmentationMethod = new System.Windows.Forms.Label();
			this._labelIntroduction = new System.Windows.Forms.Label();
			this._labelSegmentationMethodQuestion = new System.Windows.Forms.Label();
			this._radioButtonManual = new System.Windows.Forms.RadioButton();
			this._buttonAudacityHelp = new SilTools.Controls.ImageButton();
			this._buttonELANFileHelp = new SilTools.Controls.ImageButton();
			this._buttonAutoSegmenterHelp = new SilTools.Controls.ImageButton();
			this._buttonCarefulSpeechToolHelp = new SilTools.Controls.ImageButton();
			this._buttonManualSegmentationHelp = new SilTools.Controls.ImageButton();
			this._buttonGetStarted = new System.Windows.Forms.Button();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(240)))), ((int)(((byte)(159)))));
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._radioButtonAutoSegmenter, 0, 7);
			this._tableLayout.Controls.Add(this._radioButtonAudacity, 0, 6);
			this._tableLayout.Controls.Add(this._radioButtonElan, 0, 5);
			this._tableLayout.Controls.Add(this._radioButtonCarefulSpeech, 0, 4);
			this._tableLayout.Controls.Add(this._labelSegmentationMethod, 0, 2);
			this._tableLayout.Controls.Add(this._labelIntroduction, 0, 0);
			this._tableLayout.Controls.Add(this._labelSegmentationMethodQuestion, 0, 1);
			this._tableLayout.Controls.Add(this._radioButtonManual, 0, 3);
			this._tableLayout.Controls.Add(this._buttonAudacityHelp, 1, 6);
			this._tableLayout.Controls.Add(this._buttonELANFileHelp, 1, 5);
			this._tableLayout.Controls.Add(this._buttonAutoSegmenterHelp, 1, 7);
			this._tableLayout.Controls.Add(this._buttonCarefulSpeechToolHelp, 1, 4);
			this._tableLayout.Controls.Add(this._buttonManualSegmentationHelp, 1, 3);
			this._tableLayout.Controls.Add(this._buttonGetStarted, 0, 8);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayout.Location = new System.Drawing.Point(12, 6);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 9;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(488, 273);
			this._tableLayout.TabIndex = 0;
			// 
			// _radioButtonAutoSegmenter
			// 
			this._radioButtonAutoSegmenter.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._radioButtonAutoSegmenter.AutoSize = true;
			this._radioButtonAutoSegmenter.Enabled = false;
			this.locExtender.SetLocalizableToolTip(this._radioButtonAutoSegmenter, null);
			this.locExtender.SetLocalizationComment(this._radioButtonAutoSegmenter, null);
			this.locExtender.SetLocalizingId(this._radioButtonAutoSegmenter, "StartAnnotatiingEditor._radioButtonAutoSegmenter");
			this._radioButtonAutoSegmenter.Location = new System.Drawing.Point(25, 212);
			this._radioButtonAutoSegmenter.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
			this._radioButtonAutoSegmenter.Name = "_radioButtonAutoSegmenter";
			this._radioButtonAutoSegmenter.Size = new System.Drawing.Size(188, 17);
			this._radioButtonAutoSegmenter.TabIndex = 7;
			this._radioButtonAutoSegmenter.TabStop = true;
			this._radioButtonAutoSegmenter.Text = "Use auto segmenter (experimental)";
			this._radioButtonAutoSegmenter.UseVisualStyleBackColor = true;
			// 
			// _radioButtonAudacity
			// 
			this._radioButtonAudacity.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._radioButtonAudacity.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._radioButtonAudacity, null);
			this.locExtender.SetLocalizationComment(this._radioButtonAudacity, null);
			this.locExtender.SetLocalizingId(this._radioButtonAudacity, "StartAnnotatiingEditor._radioButtonAudacity");
			this._radioButtonAudacity.Location = new System.Drawing.Point(25, 186);
			this._radioButtonAudacity.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
			this._radioButtonAudacity.Name = "_radioButtonAudacity";
			this._radioButtonAudacity.Size = new System.Drawing.Size(155, 17);
			this._radioButtonAudacity.TabIndex = 6;
			this._radioButtonAudacity.TabStop = true;
			this._radioButtonAudacity.Text = "Read an Audacity Label file";
			this._radioButtonAudacity.UseVisualStyleBackColor = true;
			// 
			// _radioButtonElan
			// 
			this._radioButtonElan.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._radioButtonElan.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._radioButtonElan, null);
			this.locExtender.SetLocalizationComment(this._radioButtonElan, null);
			this.locExtender.SetLocalizingId(this._radioButtonElan, "StartAnnotatiingEditor._radioButtonElan");
			this._radioButtonElan.Location = new System.Drawing.Point(25, 160);
			this._radioButtonElan.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
			this._radioButtonElan.Name = "_radioButtonElan";
			this._radioButtonElan.Size = new System.Drawing.Size(149, 17);
			this._radioButtonElan.TabIndex = 5;
			this._radioButtonElan.TabStop = true;
			this._radioButtonElan.Text = "Copy an existing ELAN file";
			this._radioButtonElan.UseVisualStyleBackColor = true;
			// 
			// _radioButtonCarefulSpeech
			// 
			this._radioButtonCarefulSpeech.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._radioButtonCarefulSpeech.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._radioButtonCarefulSpeech, null);
			this.locExtender.SetLocalizationComment(this._radioButtonCarefulSpeech, null);
			this.locExtender.SetLocalizingId(this._radioButtonCarefulSpeech, "StartAnnotatiingEditor._radioButtonCarefulSpeech");
			this._radioButtonCarefulSpeech.Location = new System.Drawing.Point(25, 134);
			this._radioButtonCarefulSpeech.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
			this._radioButtonCarefulSpeech.Name = "_radioButtonCarefulSpeech";
			this._radioButtonCarefulSpeech.Size = new System.Drawing.Size(144, 17);
			this._radioButtonCarefulSpeech.TabIndex = 4;
			this._radioButtonCarefulSpeech.TabStop = true;
			this._radioButtonCarefulSpeech.Text = "Use Careful Speech Tool";
			this._radioButtonCarefulSpeech.UseVisualStyleBackColor = true;
			// 
			// _labelSegmentationMethod
			// 
			this._labelSegmentationMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelSegmentationMethod.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelSegmentationMethod, 2);
			this.locExtender.SetLocalizableToolTip(this._labelSegmentationMethod, null);
			this.locExtender.SetLocalizationComment(this._labelSegmentationMethod, null);
			this.locExtender.SetLocalizingId(this._labelSegmentationMethod, "label1.label1");
			this._labelSegmentationMethod.Location = new System.Drawing.Point(15, 87);
			this._labelSegmentationMethod.Margin = new System.Windows.Forms.Padding(15, 0, 15, 4);
			this._labelSegmentationMethod.Name = "_labelSegmentationMethod";
			this._labelSegmentationMethod.Size = new System.Drawing.Size(458, 13);
			this._labelSegmentationMethod.TabIndex = 2;
			this._labelSegmentationMethod.Text = "Segmentation Method";
			// 
			// _labelIntroduction
			// 
			this._labelIntroduction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelIntroduction.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelIntroduction, 2);
			this.locExtender.SetLocalizableToolTip(this._labelIntroduction, null);
			this.locExtender.SetLocalizationComment(this._labelIntroduction, null);
			this.locExtender.SetLocalizingId(this._labelIntroduction, "label1.label1");
			this._labelIntroduction.Location = new System.Drawing.Point(15, 10);
			this._labelIntroduction.Margin = new System.Windows.Forms.Padding(15, 10, 15, 15);
			this._labelIntroduction.Name = "_labelIntroduction";
			this._labelIntroduction.Size = new System.Drawing.Size(458, 26);
			this._labelIntroduction.TabIndex = 1;
			this._labelIntroduction.Text = "You can add transcription, translation, careful speech and audio translation to t" +
    "his media file. But first...";
			// 
			// _labelSegmentationMethodQuestion
			// 
			this._labelSegmentationMethodQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelSegmentationMethodQuestion.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelSegmentationMethodQuestion, 2);
			this.locExtender.SetLocalizableToolTip(this._labelSegmentationMethodQuestion, null);
			this.locExtender.SetLocalizationComment(this._labelSegmentationMethodQuestion, null);
			this.locExtender.SetLocalizingId(this._labelSegmentationMethodQuestion, "label1.label1");
			this._labelSegmentationMethodQuestion.Location = new System.Drawing.Point(15, 51);
			this._labelSegmentationMethodQuestion.Margin = new System.Windows.Forms.Padding(15, 0, 15, 10);
			this._labelSegmentationMethodQuestion.Name = "_labelSegmentationMethodQuestion";
			this._labelSegmentationMethodQuestion.Size = new System.Drawing.Size(458, 26);
			this._labelSegmentationMethodQuestion.TabIndex = 0;
			this._labelSegmentationMethodQuestion.Text = "Each annotation will be linked to a single time segment of media. How would you l" +
    "ike to identify those segments?";
			// 
			// _radioButtonManual
			// 
			this._radioButtonManual.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._radioButtonManual.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._radioButtonManual, null);
			this.locExtender.SetLocalizationComment(this._radioButtonManual, null);
			this.locExtender.SetLocalizingId(this._radioButtonManual, "StartAnnotatiingEditor._radioButtonManual");
			this._radioButtonManual.Location = new System.Drawing.Point(25, 108);
			this._radioButtonManual.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
			this._radioButtonManual.Name = "_radioButtonManual";
			this._radioButtonManual.Size = new System.Drawing.Size(174, 17);
			this._radioButtonManual.TabIndex = 3;
			this._radioButtonManual.TabStop = true;
			this._radioButtonManual.Text = "Use Manual Segmentation Tool";
			this._radioButtonManual.UseVisualStyleBackColor = true;
			// 
			// _buttonAudacityHelp
			// 
			this._buttonAudacityHelp.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonAudacityHelp.AutoSize = true;
			this._buttonAudacityHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonAudacityHelp.BackColor = System.Drawing.Color.Transparent;
			this._buttonAudacityHelp.ButtonImage = global::SayMore.Properties.Resources.Help;
			this._buttonAudacityHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonAudacityHelp.FlatAppearance.BorderSize = 0;
			this._buttonAudacityHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonAudacityHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonAudacityHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonAudacityHelp.FocusBackColor = System.Drawing.Color.Empty;
			this._buttonAudacityHelp.Image = null;
			this._buttonAudacityHelp.ImageMargin = new System.Drawing.Size(2, 2);
			this.locExtender.SetLocalizableToolTip(this._buttonAudacityHelp, null);
			this.locExtender.SetLocalizationComment(this._buttonAudacityHelp, null);
			this.locExtender.SetLocalizationPriority(this._buttonAudacityHelp, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonAudacityHelp, "DialogBoxes.Transcription.CreateAnnotationFileDlg.AudacityHelpButtonText");
			this._buttonAudacityHelp.Location = new System.Drawing.Point(219, 185);
			this._buttonAudacityHelp.Name = "_buttonAudacityHelp";
			this._buttonAudacityHelp.ShowFocusRectangle = true;
			this._buttonAudacityHelp.Size = new System.Drawing.Size(20, 20);
			this._buttonAudacityHelp.TabIndex = 9;
			this._buttonAudacityHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonAudacityHelp.UseVisualStyleBackColor = false;
			// 
			// _buttonELANFileHelp
			// 
			this._buttonELANFileHelp.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonELANFileHelp.AutoSize = true;
			this._buttonELANFileHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonELANFileHelp.BackColor = System.Drawing.Color.Transparent;
			this._buttonELANFileHelp.ButtonImage = global::SayMore.Properties.Resources.Help;
			this._buttonELANFileHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonELANFileHelp.FlatAppearance.BorderSize = 0;
			this._buttonELANFileHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonELANFileHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonELANFileHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonELANFileHelp.FocusBackColor = System.Drawing.Color.Empty;
			this._buttonELANFileHelp.Image = null;
			this._buttonELANFileHelp.ImageMargin = new System.Drawing.Size(2, 2);
			this.locExtender.SetLocalizableToolTip(this._buttonELANFileHelp, null);
			this.locExtender.SetLocalizationComment(this._buttonELANFileHelp, null);
			this.locExtender.SetLocalizationPriority(this._buttonELANFileHelp, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonELANFileHelp, "DialogBoxes.Transcription.CreateAnnotationFileDlg.ELANFileHelpButtonText");
			this._buttonELANFileHelp.Location = new System.Drawing.Point(219, 159);
			this._buttonELANFileHelp.Name = "_buttonELANFileHelp";
			this._buttonELANFileHelp.ShowFocusRectangle = true;
			this._buttonELANFileHelp.Size = new System.Drawing.Size(20, 20);
			this._buttonELANFileHelp.TabIndex = 10;
			this._buttonELANFileHelp.UseVisualStyleBackColor = false;
			// 
			// _buttonAutoSegmenterHelp
			// 
			this._buttonAutoSegmenterHelp.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonAutoSegmenterHelp.AutoSize = true;
			this._buttonAutoSegmenterHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonAutoSegmenterHelp.BackColor = System.Drawing.Color.Transparent;
			this._buttonAutoSegmenterHelp.ButtonImage = global::SayMore.Properties.Resources.Help;
			this._buttonAutoSegmenterHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonAutoSegmenterHelp.FlatAppearance.BorderSize = 0;
			this._buttonAutoSegmenterHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonAutoSegmenterHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonAutoSegmenterHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonAutoSegmenterHelp.FocusBackColor = System.Drawing.Color.Empty;
			this._buttonAutoSegmenterHelp.Image = null;
			this._buttonAutoSegmenterHelp.ImageMargin = new System.Drawing.Size(2, 2);
			this.locExtender.SetLocalizableToolTip(this._buttonAutoSegmenterHelp, null);
			this.locExtender.SetLocalizationComment(this._buttonAutoSegmenterHelp, null);
			this.locExtender.SetLocalizationPriority(this._buttonAutoSegmenterHelp, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonAutoSegmenterHelp, "DialogBoxes.Transcription.CreateAnnotationFileDlg.ELANFileHelpButtonText");
			this._buttonAutoSegmenterHelp.Location = new System.Drawing.Point(219, 211);
			this._buttonAutoSegmenterHelp.Name = "_buttonAutoSegmenterHelp";
			this._buttonAutoSegmenterHelp.ShowFocusRectangle = true;
			this._buttonAutoSegmenterHelp.Size = new System.Drawing.Size(20, 20);
			this._buttonAutoSegmenterHelp.TabIndex = 11;
			this._buttonAutoSegmenterHelp.UseVisualStyleBackColor = false;
			// 
			// _buttonCarefulSpeechToolHelp
			// 
			this._buttonCarefulSpeechToolHelp.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonCarefulSpeechToolHelp.AutoSize = true;
			this._buttonCarefulSpeechToolHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonCarefulSpeechToolHelp.BackColor = System.Drawing.Color.Transparent;
			this._buttonCarefulSpeechToolHelp.ButtonImage = global::SayMore.Properties.Resources.Help;
			this._buttonCarefulSpeechToolHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonCarefulSpeechToolHelp.FlatAppearance.BorderSize = 0;
			this._buttonCarefulSpeechToolHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonCarefulSpeechToolHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonCarefulSpeechToolHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonCarefulSpeechToolHelp.FocusBackColor = System.Drawing.Color.Empty;
			this._buttonCarefulSpeechToolHelp.Image = null;
			this._buttonCarefulSpeechToolHelp.ImageMargin = new System.Drawing.Size(2, 2);
			this.locExtender.SetLocalizableToolTip(this._buttonCarefulSpeechToolHelp, null);
			this.locExtender.SetLocalizationComment(this._buttonCarefulSpeechToolHelp, null);
			this.locExtender.SetLocalizationPriority(this._buttonCarefulSpeechToolHelp, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonCarefulSpeechToolHelp, "DialogBoxes.Transcription.CreateAnnotationFileDlg.ELANFileHelpButtonText");
			this._buttonCarefulSpeechToolHelp.Location = new System.Drawing.Point(219, 133);
			this._buttonCarefulSpeechToolHelp.Name = "_buttonCarefulSpeechToolHelp";
			this._buttonCarefulSpeechToolHelp.ShowFocusRectangle = true;
			this._buttonCarefulSpeechToolHelp.Size = new System.Drawing.Size(20, 20);
			this._buttonCarefulSpeechToolHelp.TabIndex = 12;
			this._buttonCarefulSpeechToolHelp.UseVisualStyleBackColor = false;
			// 
			// _buttonManualSegmentationHelp
			// 
			this._buttonManualSegmentationHelp.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonManualSegmentationHelp.AutoSize = true;
			this._buttonManualSegmentationHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonManualSegmentationHelp.BackColor = System.Drawing.Color.Transparent;
			this._buttonManualSegmentationHelp.ButtonImage = global::SayMore.Properties.Resources.Help;
			this._buttonManualSegmentationHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonManualSegmentationHelp.FlatAppearance.BorderSize = 0;
			this._buttonManualSegmentationHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonManualSegmentationHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonManualSegmentationHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonManualSegmentationHelp.FocusBackColor = System.Drawing.Color.Empty;
			this._buttonManualSegmentationHelp.Image = null;
			this._buttonManualSegmentationHelp.ImageMargin = new System.Drawing.Size(2, 2);
			this.locExtender.SetLocalizableToolTip(this._buttonManualSegmentationHelp, null);
			this.locExtender.SetLocalizationComment(this._buttonManualSegmentationHelp, null);
			this.locExtender.SetLocalizationPriority(this._buttonManualSegmentationHelp, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonManualSegmentationHelp, "DialogBoxes.Transcription.CreateAnnotationFileDlg.ELANFileHelpButtonText");
			this._buttonManualSegmentationHelp.Location = new System.Drawing.Point(219, 107);
			this._buttonManualSegmentationHelp.Name = "_buttonManualSegmentationHelp";
			this._buttonManualSegmentationHelp.ShowFocusRectangle = true;
			this._buttonManualSegmentationHelp.Size = new System.Drawing.Size(20, 20);
			this._buttonManualSegmentationHelp.TabIndex = 13;
			this._buttonManualSegmentationHelp.UseVisualStyleBackColor = false;
			// 
			// _buttonGetStarted
			// 
			this._buttonGetStarted.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._buttonGetStarted.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._buttonGetStarted, null);
			this.locExtender.SetLocalizationComment(this._buttonGetStarted, null);
			this.locExtender.SetLocalizingId(this._buttonGetStarted, "StartAnnotatiingEditor._buttonGetStarted");
			this._buttonGetStarted.Location = new System.Drawing.Point(65, 244);
			this._buttonGetStarted.Margin = new System.Windows.Forms.Padding(15, 10, 0, 3);
			this._buttonGetStarted.MinimumSize = new System.Drawing.Size(100, 26);
			this._buttonGetStarted.Name = "_buttonGetStarted";
			this._buttonGetStarted.Size = new System.Drawing.Size(100, 26);
			this._buttonGetStarted.TabIndex = 8;
			this._buttonGetStarted.Text = "Get Started...";
			this._buttonGetStarted.UseVisualStyleBackColor = true;
			this._buttonGetStarted.Click += new System.EventHandler(this.HandleGetStartedButtonClick);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// StartAnnotatingEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "OralAnnotationEditor.EditorBase");
			this.Name = "StartAnnotatingEditor";
			this.Padding = new System.Windows.Forms.Padding(12, 6, 12, 12);
			this.Size = new System.Drawing.Size(512, 364);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Label _labelSegmentationMethodQuestion;
		private System.Windows.Forms.Label _labelSegmentationMethod;
		private System.Windows.Forms.Label _labelIntroduction;
		private System.Windows.Forms.RadioButton _radioButtonAutoSegmenter;
		private System.Windows.Forms.RadioButton _radioButtonAudacity;
		private System.Windows.Forms.RadioButton _radioButtonElan;
		private System.Windows.Forms.RadioButton _radioButtonCarefulSpeech;
		private System.Windows.Forms.RadioButton _radioButtonManual;
		private System.Windows.Forms.Button _buttonGetStarted;
		private ImageButton _buttonAudacityHelp;
		private ImageButton _buttonELANFileHelp;
		private ImageButton _buttonAutoSegmenterHelp;
		private ImageButton _buttonCarefulSpeechToolHelp;
		private ImageButton _buttonManualSegmentationHelp;


	}
}
