using System.Windows.Forms;

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
			this._tableLayoutGetStarted = new System.Windows.Forms.TableLayoutPanel();
			this._radioButtonElan = new System.Windows.Forms.RadioButton();
			this._radioButtonCarefulSpeech = new System.Windows.Forms.RadioButton();
			this._labelSegmentationMethod = new System.Windows.Forms.Label();
			this._labelIntroduction = new System.Windows.Forms.Label();
			this._labelSegmentationMethodQuestion = new System.Windows.Forms.Label();
			this._radioButtonManual = new System.Windows.Forms.RadioButton();
			this._buttonELANFileHelp = new System.Windows.Forms.Button();
			this._buttonCarefulSpeechToolHelp = new System.Windows.Forms.Button();
			this._buttonManualSegmentationHelp = new System.Windows.Forms.Button();
			this._buttonGetStarted = new System.Windows.Forms.Button();
			this._buttonAudacityHelp = new System.Windows.Forms.Button();
			this._radioButtonAudacity = new System.Windows.Forms.RadioButton();
			this._radioButtonAutoSegmenter = new System.Windows.Forms.RadioButton();
			this._buttonAutoSegmenterHelp = new System.Windows.Forms.Button();
			this._cboAudacityLabelTier = new System.Windows.Forms.ComboBox();
			this._lblAudacityLabelTier = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._tableLayoutGetStarted.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayoutGetStarted
			// 
			this._tableLayoutGetStarted.AutoSize = true;
			this._tableLayoutGetStarted.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutGetStarted.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(240)))), ((int)(((byte)(159)))));
			this._tableLayoutGetStarted.ColumnCount = 4;
			this._tableLayoutGetStarted.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutGetStarted.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutGetStarted.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutGetStarted.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutGetStarted.Controls.Add(this._radioButtonElan, 0, 5);
			this._tableLayoutGetStarted.Controls.Add(this._radioButtonCarefulSpeech, 0, 4);
			this._tableLayoutGetStarted.Controls.Add(this._labelSegmentationMethod, 0, 2);
			this._tableLayoutGetStarted.Controls.Add(this._labelIntroduction, 0, 0);
			this._tableLayoutGetStarted.Controls.Add(this._labelSegmentationMethodQuestion, 0, 1);
			this._tableLayoutGetStarted.Controls.Add(this._radioButtonManual, 0, 3);
			this._tableLayoutGetStarted.Controls.Add(this._buttonELANFileHelp, 1, 5);
			this._tableLayoutGetStarted.Controls.Add(this._buttonCarefulSpeechToolHelp, 1, 4);
			this._tableLayoutGetStarted.Controls.Add(this._buttonManualSegmentationHelp, 1, 3);
			this._tableLayoutGetStarted.Controls.Add(this._buttonGetStarted, 0, 8);
			this._tableLayoutGetStarted.Controls.Add(this._buttonAudacityHelp, 3, 3);
			this._tableLayoutGetStarted.Controls.Add(this._radioButtonAudacity, 2, 3);
			this._tableLayoutGetStarted.Controls.Add(this._radioButtonAutoSegmenter, 2, 5);
			this._tableLayoutGetStarted.Controls.Add(this._buttonAutoSegmenterHelp, 3, 5);
			this._tableLayoutGetStarted.Controls.Add(this._cboAudacityLabelTier, 3, 4);
			this._tableLayoutGetStarted.Controls.Add(this._lblAudacityLabelTier, 2, 4);
			this._tableLayoutGetStarted.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayoutGetStarted.Location = new System.Drawing.Point(12, 6);
			this._tableLayoutGetStarted.Name = "_tableLayoutGetStarted";
			this._tableLayoutGetStarted.RowCount = 9;
			this._tableLayoutGetStarted.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutGetStarted.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutGetStarted.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutGetStarted.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutGetStarted.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutGetStarted.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutGetStarted.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutGetStarted.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutGetStarted.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutGetStarted.Size = new System.Drawing.Size(512, 227);
			this._tableLayoutGetStarted.TabIndex = 0;
			// 
			// _radioButtonElan
			// 
			this._radioButtonElan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._radioButtonElan.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._radioButtonElan, null);
			this.locExtender.SetLocalizationComment(this._radioButtonElan, null);
			this.locExtender.SetLocalizingId(this._radioButtonElan, "SessionsView.Transcription.StartAnnotatingTab._radioButtonElan");
			this._radioButtonElan.Location = new System.Drawing.Point(25, 165);
			this._radioButtonElan.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
			this._radioButtonElan.Name = "_radioButtonElan";
			this._radioButtonElan.Size = new System.Drawing.Size(174, 17);
			this._radioButtonElan.TabIndex = 7;
			this._radioButtonElan.TabStop = true;
			this._radioButtonElan.Text = "Copy an existing ELAN file";
			this._radioButtonElan.UseVisualStyleBackColor = true;
			// 
			// _radioButtonCarefulSpeech
			// 
			this._radioButtonCarefulSpeech.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._radioButtonCarefulSpeech.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._radioButtonCarefulSpeech, null);
			this.locExtender.SetLocalizationComment(this._radioButtonCarefulSpeech, null);
			this.locExtender.SetLocalizingId(this._radioButtonCarefulSpeech, "SessionsView.Transcription.StartAnnotatingTab._radioButtonCarefulSpeech");
			this._radioButtonCarefulSpeech.Location = new System.Drawing.Point(25, 137);
			this._radioButtonCarefulSpeech.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
			this._radioButtonCarefulSpeech.Name = "_radioButtonCarefulSpeech";
			this._radioButtonCarefulSpeech.Size = new System.Drawing.Size(174, 17);
			this._radioButtonCarefulSpeech.TabIndex = 5;
			this._radioButtonCarefulSpeech.TabStop = true;
			this._radioButtonCarefulSpeech.Text = "Use Careful Speech Tool";
			this._radioButtonCarefulSpeech.UseVisualStyleBackColor = true;
			// 
			// _labelSegmentationMethod
			// 
			this._labelSegmentationMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelSegmentationMethod.AutoSize = true;
			this._tableLayoutGetStarted.SetColumnSpan(this._labelSegmentationMethod, 4);
			this.locExtender.SetLocalizableToolTip(this._labelSegmentationMethod, null);
			this.locExtender.SetLocalizationComment(this._labelSegmentationMethod, null);
			this.locExtender.SetLocalizingId(this._labelSegmentationMethod, "SessionsView.Transcription.StartAnnotatingTab._labelSegmentationMethod");
			this._labelSegmentationMethod.Location = new System.Drawing.Point(15, 87);
			this._labelSegmentationMethod.Margin = new System.Windows.Forms.Padding(15, 0, 15, 4);
			this._labelSegmentationMethod.Name = "_labelSegmentationMethod";
			this._labelSegmentationMethod.Size = new System.Drawing.Size(482, 13);
			this._labelSegmentationMethod.TabIndex = 2;
			this._labelSegmentationMethod.Text = "Segmentation Method";
			// 
			// _labelIntroduction
			// 
			this._labelIntroduction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelIntroduction.AutoSize = true;
			this._tableLayoutGetStarted.SetColumnSpan(this._labelIntroduction, 4);
			this.locExtender.SetLocalizableToolTip(this._labelIntroduction, null);
			this.locExtender.SetLocalizationComment(this._labelIntroduction, null);
			this.locExtender.SetLocalizingId(this._labelIntroduction, "SessionsView.Transcription.StartAnnotatingTab._labelIntroduction");
			this._labelIntroduction.Location = new System.Drawing.Point(15, 10);
			this._labelIntroduction.Margin = new System.Windows.Forms.Padding(15, 10, 15, 15);
			this._labelIntroduction.Name = "_labelIntroduction";
			this._labelIntroduction.Size = new System.Drawing.Size(482, 26);
			this._labelIntroduction.TabIndex = 0;
			this._labelIntroduction.Text = "You can add transcription, translation, careful speech and audio translation to t" +
				"his media file. But first...";
			// 
			// _labelSegmentationMethodQuestion
			// 
			this._labelSegmentationMethodQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelSegmentationMethodQuestion.AutoSize = true;
			this._tableLayoutGetStarted.SetColumnSpan(this._labelSegmentationMethodQuestion, 4);
			this.locExtender.SetLocalizableToolTip(this._labelSegmentationMethodQuestion, null);
			this.locExtender.SetLocalizationComment(this._labelSegmentationMethodQuestion, null);
			this.locExtender.SetLocalizingId(this._labelSegmentationMethodQuestion, "SessionsView.Transcription.StartAnnotatingTab._labelSegmentationMethodQuestion");
			this._labelSegmentationMethodQuestion.Location = new System.Drawing.Point(15, 51);
			this._labelSegmentationMethodQuestion.Margin = new System.Windows.Forms.Padding(15, 0, 15, 10);
			this._labelSegmentationMethodQuestion.Name = "_labelSegmentationMethodQuestion";
			this._labelSegmentationMethodQuestion.Size = new System.Drawing.Size(482, 26);
			this._labelSegmentationMethodQuestion.TabIndex = 1;
			this._labelSegmentationMethodQuestion.Text = "Each annotation will be linked to a single time segment of media. How would you l" +
				"ike to identify those segments?";
			// 
			// _radioButtonManual
			// 
			this._radioButtonManual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._radioButtonManual.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._radioButtonManual, null);
			this.locExtender.SetLocalizationComment(this._radioButtonManual, null);
			this.locExtender.SetLocalizingId(this._radioButtonManual, "SessionsView.Transcription.StartAnnotatingTab._radioButtonManual");
			this._radioButtonManual.Location = new System.Drawing.Point(25, 109);
			this._radioButtonManual.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
			this._radioButtonManual.Name = "_radioButtonManual";
			this._radioButtonManual.Size = new System.Drawing.Size(174, 17);
			this._radioButtonManual.TabIndex = 3;
			this._radioButtonManual.TabStop = true;
			this._radioButtonManual.Text = "Use Manual Segmentation Tool";
			this._radioButtonManual.UseVisualStyleBackColor = true;
			// 
			// _buttonELANFileHelp
			// 
			this._buttonELANFileHelp.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonELANFileHelp.AutoSize = true;
			this._buttonELANFileHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonELANFileHelp.BackColor = System.Drawing.Color.Transparent;
			this._buttonELANFileHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonELANFileHelp.FlatAppearance.BorderSize = 0;
			this._buttonELANFileHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonELANFileHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonELANFileHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonELANFileHelp.Image = global::SayMore.Properties.Resources.Help;
			this.locExtender.SetLocalizableToolTip(this._buttonELANFileHelp, null);
			this.locExtender.SetLocalizationComment(this._buttonELANFileHelp, null);
			this.locExtender.SetLocalizationPriority(this._buttonELANFileHelp, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonELANFileHelp, "SessionsView.Transcription.StartAnnotatingTab.ELANFileHelpButtonText");
			this._buttonELANFileHelp.Location = new System.Drawing.Point(205, 163);
			this._buttonELANFileHelp.Name = "_buttonELANFileHelp";
			this._buttonELANFileHelp.Size = new System.Drawing.Size(22, 22);
			this._buttonELANFileHelp.TabIndex = 8;
			this._buttonELANFileHelp.UseVisualStyleBackColor = false;
			// 
			// _buttonCarefulSpeechToolHelp
			// 
			this._buttonCarefulSpeechToolHelp.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonCarefulSpeechToolHelp.AutoSize = true;
			this._buttonCarefulSpeechToolHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonCarefulSpeechToolHelp.BackColor = System.Drawing.Color.Transparent;
			this._buttonCarefulSpeechToolHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonCarefulSpeechToolHelp.FlatAppearance.BorderSize = 0;
			this._buttonCarefulSpeechToolHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonCarefulSpeechToolHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonCarefulSpeechToolHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonCarefulSpeechToolHelp.Image = global::SayMore.Properties.Resources.Help;
			this.locExtender.SetLocalizableToolTip(this._buttonCarefulSpeechToolHelp, null);
			this.locExtender.SetLocalizationComment(this._buttonCarefulSpeechToolHelp, null);
			this.locExtender.SetLocalizationPriority(this._buttonCarefulSpeechToolHelp, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonCarefulSpeechToolHelp, "SessionsView.Transcription.StartAnnotatingTab.ELANFileHelpButtonText");
			this._buttonCarefulSpeechToolHelp.Location = new System.Drawing.Point(205, 135);
			this._buttonCarefulSpeechToolHelp.Name = "_buttonCarefulSpeechToolHelp";
			this._buttonCarefulSpeechToolHelp.Size = new System.Drawing.Size(22, 22);
			this._buttonCarefulSpeechToolHelp.TabIndex = 6;
			this._buttonCarefulSpeechToolHelp.UseVisualStyleBackColor = false;
			// 
			// _buttonManualSegmentationHelp
			// 
			this._buttonManualSegmentationHelp.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonManualSegmentationHelp.AutoSize = true;
			this._buttonManualSegmentationHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonManualSegmentationHelp.BackColor = System.Drawing.Color.Transparent;
			this._buttonManualSegmentationHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonManualSegmentationHelp.FlatAppearance.BorderSize = 0;
			this._buttonManualSegmentationHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonManualSegmentationHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonManualSegmentationHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonManualSegmentationHelp.Image = global::SayMore.Properties.Resources.Help;
			this.locExtender.SetLocalizableToolTip(this._buttonManualSegmentationHelp, null);
			this.locExtender.SetLocalizationComment(this._buttonManualSegmentationHelp, null);
			this.locExtender.SetLocalizationPriority(this._buttonManualSegmentationHelp, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonManualSegmentationHelp, "SessionsView.Transcription.StartAnnotatingTab.ELANFileHelpButtonText");
			this._buttonManualSegmentationHelp.Location = new System.Drawing.Point(205, 107);
			this._buttonManualSegmentationHelp.Name = "_buttonManualSegmentationHelp";
			this._buttonManualSegmentationHelp.Size = new System.Drawing.Size(22, 22);
			this._buttonManualSegmentationHelp.TabIndex = 4;
			this._buttonManualSegmentationHelp.UseVisualStyleBackColor = false;
			// 
			// _buttonGetStarted
			// 
			this._buttonGetStarted.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._buttonGetStarted.AutoSize = true;
			this._tableLayoutGetStarted.SetColumnSpan(this._buttonGetStarted, 4);
			this.locExtender.SetLocalizableToolTip(this._buttonGetStarted, null);
			this.locExtender.SetLocalizationComment(this._buttonGetStarted, null);
			this.locExtender.SetLocalizingId(this._buttonGetStarted, "SessionsView.Transcription.StartAnnotatingTab._buttonGetStarted");
			this._buttonGetStarted.Location = new System.Drawing.Point(206, 198);
			this._buttonGetStarted.Margin = new System.Windows.Forms.Padding(0, 10, 0, 3);
			this._buttonGetStarted.MinimumSize = new System.Drawing.Size(100, 26);
			this._buttonGetStarted.Name = "_buttonGetStarted";
			this._buttonGetStarted.Size = new System.Drawing.Size(100, 26);
			this._buttonGetStarted.TabIndex = 0;
			this._buttonGetStarted.Text = "Get Started...";
			this._buttonGetStarted.UseVisualStyleBackColor = true;
			this._buttonGetStarted.Click += new System.EventHandler(this.HandleGetStartedButtonClick);
			// 
			// _buttonAudacityHelp
			// 
			this._buttonAudacityHelp.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonAudacityHelp.AutoSize = true;
			this._buttonAudacityHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonAudacityHelp.BackColor = System.Drawing.Color.Transparent;
			this._buttonAudacityHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonAudacityHelp.FlatAppearance.BorderSize = 0;
			this._buttonAudacityHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonAudacityHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonAudacityHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonAudacityHelp.Image = global::SayMore.Properties.Resources.Help;
			this.locExtender.SetLocalizableToolTip(this._buttonAudacityHelp, null);
			this.locExtender.SetLocalizationComment(this._buttonAudacityHelp, null);
			this.locExtender.SetLocalizationPriority(this._buttonAudacityHelp, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonAudacityHelp, "SessionsView.Transcription.StartAnnotatingTab.AudacityHelpButtonText");
			this._buttonAudacityHelp.Location = new System.Drawing.Point(416, 107);
			this._buttonAudacityHelp.Name = "_buttonAudacityHelp";
			this._buttonAudacityHelp.Size = new System.Drawing.Size(22, 22);
			this._buttonAudacityHelp.TabIndex = 10;
			this._buttonAudacityHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonAudacityHelp.UseVisualStyleBackColor = false;
			// 
			// _radioButtonAudacity
			// 
			this._radioButtonAudacity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._radioButtonAudacity.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._radioButtonAudacity, null);
			this.locExtender.SetLocalizationComment(this._radioButtonAudacity, null);
			this.locExtender.SetLocalizingId(this._radioButtonAudacity, "SessionsView.Transcription.StartAnnotatingTab._radioButtonAudacity");
			this._radioButtonAudacity.Location = new System.Drawing.Point(255, 109);
			this._radioButtonAudacity.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
			this._radioButtonAudacity.Name = "_radioButtonAudacity";
			this._radioButtonAudacity.Size = new System.Drawing.Size(155, 17);
			this._radioButtonAudacity.TabIndex = 9;
			this._radioButtonAudacity.TabStop = true;
			this._radioButtonAudacity.Text = "Read an Audacity Label file";
			this._radioButtonAudacity.UseVisualStyleBackColor = true;
			this._radioButtonAudacity.CheckedChanged += new System.EventHandler(this._radioButtonAudacity_CheckedChanged);
			// 
			// _radioButtonAutoSegmenter
			// 
			this._radioButtonAutoSegmenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._radioButtonAutoSegmenter.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._radioButtonAutoSegmenter, null);
			this.locExtender.SetLocalizationComment(this._radioButtonAutoSegmenter, null);
			this.locExtender.SetLocalizingId(this._radioButtonAutoSegmenter, "SessionsView.Transcription.StartAnnotatingTab._radioButtonAutoSegmenter");
			this._radioButtonAutoSegmenter.Location = new System.Drawing.Point(255, 165);
			this._radioButtonAutoSegmenter.Margin = new System.Windows.Forms.Padding(25, 3, 3, 3);
			this._radioButtonAutoSegmenter.Name = "_radioButtonAutoSegmenter";
			this._radioButtonAutoSegmenter.Size = new System.Drawing.Size(155, 17);
			this._radioButtonAutoSegmenter.TabIndex = 11;
			this._radioButtonAutoSegmenter.TabStop = true;
			this._radioButtonAutoSegmenter.Text = "Use auto segmenter";
			this._radioButtonAutoSegmenter.UseVisualStyleBackColor = true;
			// 
			// _buttonAutoSegmenterHelp
			// 
			this._buttonAutoSegmenterHelp.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonAutoSegmenterHelp.AutoSize = true;
			this._buttonAutoSegmenterHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonAutoSegmenterHelp.BackColor = System.Drawing.Color.Transparent;
			this._buttonAutoSegmenterHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonAutoSegmenterHelp.FlatAppearance.BorderSize = 0;
			this._buttonAutoSegmenterHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonAutoSegmenterHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonAutoSegmenterHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonAutoSegmenterHelp.Image = global::SayMore.Properties.Resources.Help;
			this.locExtender.SetLocalizableToolTip(this._buttonAutoSegmenterHelp, null);
			this.locExtender.SetLocalizationComment(this._buttonAutoSegmenterHelp, null);
			this.locExtender.SetLocalizationPriority(this._buttonAutoSegmenterHelp, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonAutoSegmenterHelp, "SessionsView.Transcription.StartAnnotatingTab.ELANFileHelpButtonText");
			this._buttonAutoSegmenterHelp.Location = new System.Drawing.Point(416, 163);
			this._buttonAutoSegmenterHelp.Name = "_buttonAutoSegmenterHelp";
			this._buttonAutoSegmenterHelp.Size = new System.Drawing.Size(22, 22);
			this._buttonAutoSegmenterHelp.TabIndex = 13;
			this._buttonAutoSegmenterHelp.UseVisualStyleBackColor = false;
			// 
			// _cboAudacityLabelTier
			// 
			this._cboAudacityLabelTier.Enabled = false;
			this._cboAudacityLabelTier.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._cboAudacityLabelTier, null);
			this.locExtender.SetLocalizationComment(this._cboAudacityLabelTier, null);
			this.locExtender.SetLocalizationPriority(this._cboAudacityLabelTier, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._cboAudacityLabelTier, "StartAnnotatingEditor._cboAudacityLabelTier");
			this._cboAudacityLabelTier.Location = new System.Drawing.Point(416, 135);
			this._cboAudacityLabelTier.MinimumSize = new System.Drawing.Size(100, 0);
			this._cboAudacityLabelTier.Name = "_cboAudacityLabelTier";
			this._cboAudacityLabelTier.Size = new System.Drawing.Size(100, 21);
			this._cboAudacityLabelTier.TabIndex = 14;
			// 
			// _lblAudacityLabelTier
			// 
			this._lblAudacityLabelTier.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._lblAudacityLabelTier.AutoSize = true;
			this._lblAudacityLabelTier.Enabled = false;
			this.locExtender.SetLocalizableToolTip(this._lblAudacityLabelTier, null);
			this.locExtender.SetLocalizationComment(this._lblAudacityLabelTier, null);
			this.locExtender.SetLocalizationPriority(this._lblAudacityLabelTier, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._lblAudacityLabelTier, "StartAnnotatingEditor._lblAudacityLabelTier");
			this._lblAudacityLabelTier.Location = new System.Drawing.Point(309, 139);
			this._lblAudacityLabelTier.Name = "_lblAudacityLabelTier";
			this._lblAudacityLabelTier.Size = new System.Drawing.Size(101, 13);
			this._lblAudacityLabelTier.TabIndex = 15;
			this._lblAudacityLabelTier.Text = "Import label text into";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = null;
			// 
			// StartAnnotatingEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayoutGetStarted);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "SessionsView.Transcription.StartAnnotatingTab.EditorBase");
			this.Name = "StartAnnotatingEditor";
			this.Padding = new System.Windows.Forms.Padding(12, 6, 12, 12);
			this.Size = new System.Drawing.Size(536, 320);
			this._tableLayoutGetStarted.ResumeLayout(false);
			this._tableLayoutGetStarted.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutGetStarted;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.Label _labelSegmentationMethodQuestion;
		private System.Windows.Forms.Label _labelSegmentationMethod;
		private System.Windows.Forms.Label _labelIntroduction;
		private System.Windows.Forms.RadioButton _radioButtonAutoSegmenter;
		private System.Windows.Forms.RadioButton _radioButtonAudacity;
		private System.Windows.Forms.RadioButton _radioButtonElan;
		private System.Windows.Forms.RadioButton _radioButtonCarefulSpeech;
		private System.Windows.Forms.RadioButton _radioButtonManual;
		private System.Windows.Forms.Button _buttonGetStarted;
		private Button _buttonAudacityHelp;
		private Button _buttonELANFileHelp;
		private Button _buttonAutoSegmenterHelp;
		private Button _buttonCarefulSpeechToolHelp;
		private Button _buttonManualSegmentationHelp;
		private ComboBox _cboAudacityLabelTier;
		private Label _lblAudacityLabelTier;
	}
}
