namespace SayMore.UI.ElementListScreen
{
	partial class ComponentFileRenamingDialog
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._linkWrittenTranslation = new System.Windows.Forms.LinkLabel();
			this._tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
			this._buttonReadAboutRenaming = new System.Windows.Forms.Button();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._buttonRename = new System.Windows.Forms.Button();
			this._labelReadAboutRenaming = new System.Windows.Forms.Label();
			this._linkTranscription = new System.Windows.Forms.LinkLabel();
			this._linkConsent = new System.Windows.Forms.LinkLabel();
			this._tableLayoutTextBox = new System.Windows.Forms.TableLayoutPanel();
			this._textBox = new System.Windows.Forms.TextBox();
			this._labelPrefix = new System.Windows.Forms.Label();
			this._labelExtension = new System.Windows.Forms.Label();
			this._labelChangeNameTo = new System.Windows.Forms.Label();
			this._flowLayoutShortcuts = new System.Windows.Forms.FlowLayoutPanel();
			this._labelShortcuts = new System.Windows.Forms.Label();
			this._labelShortcutsHint = new System.Windows.Forms.Label();
			this._linkSource = new System.Windows.Forms.LinkLabel();
			this._labelNonSayMoreImportHint = new System.Windows.Forms.Label();
			this._linkCareful = new System.Windows.Forms.LinkLabel();
			this._linkOralTranslation = new System.Windows.Forms.LinkLabel();
			this._messagePanel = new System.Windows.Forms.Panel();
			this._warningIcon = new System.Windows.Forms.PictureBox();
			this._labelMessage = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.LocalizationExtender(this.components);
			this._tableLayout.SuspendLayout();
			this._tableLayoutButtons.SuspendLayout();
			this._tableLayoutTextBox.SuspendLayout();
			this._flowLayoutShortcuts.SuspendLayout();
			this._messagePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._warningIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
			this._tableLayout.Controls.Add(this._linkWrittenTranslation, 1, 6);
			this._tableLayout.Controls.Add(this._tableLayoutButtons, 0, 8);
			this._tableLayout.Controls.Add(this._linkTranscription, 0, 6);
			this._tableLayout.Controls.Add(this._linkConsent, 1, 3);
			this._tableLayout.Controls.Add(this._tableLayoutTextBox, 0, 1);
			this._tableLayout.Controls.Add(this._labelChangeNameTo, 0, 0);
			this._tableLayout.Controls.Add(this._flowLayoutShortcuts, 0, 2);
			this._tableLayout.Controls.Add(this._linkSource, 0, 3);
			this._tableLayout.Controls.Add(this._labelNonSayMoreImportHint, 0, 4);
			this._tableLayout.Controls.Add(this._linkCareful, 0, 5);
			this._tableLayout.Controls.Add(this._linkOralTranslation, 1, 5);
			this._tableLayout.Controls.Add(this._messagePanel, 0, 7);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(15, 15);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 9;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(414, 307);
			this._tableLayout.TabIndex = 0;
			// 
			// _linkWrittenTranslation
			// 
			this._linkWrittenTranslation.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._linkWrittenTranslation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkWrittenTranslation, "Written Translation:\\nWritten translation of\\nthe source recording.");
			this.locExtender.SetLocalizationComment(this._linkWrittenTranslation, null);
			this.locExtender.SetLocalizingId(this._linkWrittenTranslation, "DialogBoxes.ComponentFileRenamingDlg._linkWrittenTranslation");
			this._linkWrittenTranslation.Location = new System.Drawing.Point(262, 194);
			this._linkWrittenTranslation.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._linkWrittenTranslation.Name = "_linkWrittenTranslation";
			this._linkWrittenTranslation.Size = new System.Drawing.Size(96, 13);
			this._linkWrittenTranslation.TabIndex = 8;
			this._linkWrittenTranslation.TabStop = true;
			this._linkWrittenTranslation.Text = "Written Translation";
			// 
			// _tableLayoutButtons
			// 
			this._tableLayoutButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutButtons.AutoSize = true;
			this._tableLayoutButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutButtons.ColumnCount = 4;
			this._tableLayout.SetColumnSpan(this._tableLayoutButtons, 2);
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutButtons.Controls.Add(this._buttonReadAboutRenaming, 0, 0);
			this._tableLayoutButtons.Controls.Add(this._buttonCancel, 3, 0);
			this._tableLayoutButtons.Controls.Add(this._buttonRename, 2, 0);
			this._tableLayoutButtons.Controls.Add(this._labelReadAboutRenaming, 1, 0);
			this._tableLayoutButtons.Location = new System.Drawing.Point(0, 281);
			this._tableLayoutButtons.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._tableLayoutButtons.Name = "_tableLayoutButtons";
			this._tableLayoutButtons.RowCount = 1;
			this._tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutButtons.Size = new System.Drawing.Size(414, 26);
			this._tableLayoutButtons.TabIndex = 10;
			// 
			// _buttonReadAboutRenaming
			// 
			this._buttonReadAboutRenaming.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._buttonReadAboutRenaming.AutoSize = true;
			this._buttonReadAboutRenaming.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonReadAboutRenaming.BackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutRenaming.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonReadAboutRenaming.FlatAppearance.BorderSize = 0;
			this._buttonReadAboutRenaming.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutRenaming.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutRenaming.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonReadAboutRenaming.Image = global::SayMore.Properties.Resources.Help;
			this.locExtender.SetLocalizableToolTip(this._buttonReadAboutRenaming, null);
			this.locExtender.SetLocalizationComment(this._buttonReadAboutRenaming, null);
			this.locExtender.SetLocalizationPriority(this._buttonReadAboutRenaming, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonReadAboutRenaming, "_buttonReadAboutRenaming");
			this._buttonReadAboutRenaming.Location = new System.Drawing.Point(0, 2);
			this._buttonReadAboutRenaming.Margin = new System.Windows.Forms.Padding(0);
			this._buttonReadAboutRenaming.Name = "_buttonReadAboutRenaming";
			this._buttonReadAboutRenaming.Size = new System.Drawing.Size(22, 22);
			this._buttonReadAboutRenaming.TabIndex = 1;
			this._buttonReadAboutRenaming.UseVisualStyleBackColor = false;
			this._buttonReadAboutRenaming.Click += new System.EventHandler(this.HandleReadAboutRenamingButtonClick);
			// 
			// _buttonCancel
			// 
			this._buttonCancel.AutoSize = true;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.ComponentFileRenamingDlg._buttonCancel");
			this._buttonCancel.Location = new System.Drawing.Point(339, 0);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this._buttonCancel.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 3;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _buttonRename
			// 
			this._buttonRename.AutoSize = true;
			this._buttonRename.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._buttonRename, null);
			this.locExtender.SetLocalizationComment(this._buttonRename, null);
			this.locExtender.SetLocalizingId(this._buttonRename, "DialogBoxes.ComponentFileRenamingDlg._buttonRename");
			this._buttonRename.Location = new System.Drawing.Point(256, 0);
			this._buttonRename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this._buttonRename.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonRename.Name = "_buttonRename";
			this._buttonRename.Size = new System.Drawing.Size(75, 26);
			this._buttonRename.TabIndex = 2;
			this._buttonRename.Text = "Rename";
			this._buttonRename.UseVisualStyleBackColor = true;
			// 
			// _labelReadAboutRenaming
			// 
			this._labelReadAboutRenaming.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelReadAboutRenaming.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelReadAboutRenaming, null);
			this.locExtender.SetLocalizationComment(this._labelReadAboutRenaming, null);
			this.locExtender.SetLocalizingId(this._labelReadAboutRenaming, "DialogBoxes.ComponentFileRenamingDlg._labelReadAboutRenaming");
			this._labelReadAboutRenaming.Location = new System.Drawing.Point(25, 6);
			this._labelReadAboutRenaming.Name = "_labelReadAboutRenaming";
			this._labelReadAboutRenaming.Size = new System.Drawing.Size(197, 13);
			this._labelReadAboutRenaming.TabIndex = 0;
			this._labelReadAboutRenaming.Text = "Read About How SayMore Uses Names";
			// 
			// _linkTranscription
			// 
			this._linkTranscription.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._linkTranscription.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkTranscription, "Transcription:\\nWritten transcription of\\nthe source recording.");
			this.locExtender.SetLocalizationComment(this._linkTranscription, null);
			this.locExtender.SetLocalizingId(this._linkTranscription, "DialogBoxes.ComponentFileRenamingDlg._linkTranscription");
			this._linkTranscription.Location = new System.Drawing.Point(69, 194);
			this._linkTranscription.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._linkTranscription.Name = "_linkTranscription";
			this._linkTranscription.Size = new System.Drawing.Size(68, 13);
			this._linkTranscription.TabIndex = 7;
			this._linkTranscription.TabStop = true;
			this._linkTranscription.Text = "Transcription";
			// 
			// _linkConsent
			// 
			this._linkConsent.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._linkConsent.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkConsent, "Informed Consent:\\nA scan or recording of consent covering\\nthe use of this perso" +
        "n\'s contribution.");
			this.locExtender.SetLocalizationComment(this._linkConsent, null);
			this.locExtender.SetLocalizingId(this._linkConsent, "DialogBoxes.ComponentFileRenamingDlg._linkConsent");
			this._linkConsent.Location = new System.Drawing.Point(265, 92);
			this._linkConsent.Name = "_linkConsent";
			this._linkConsent.Size = new System.Drawing.Size(90, 13);
			this._linkConsent.TabIndex = 3;
			this._linkConsent.TabStop = true;
			this._linkConsent.Text = "Informed Consent";
			// 
			// _tableLayoutTextBox
			// 
			this._tableLayoutTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutTextBox.AutoSize = true;
			this._tableLayoutTextBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutTextBox.ColumnCount = 3;
			this._tableLayout.SetColumnSpan(this._tableLayoutTextBox, 2);
			this._tableLayoutTextBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutTextBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutTextBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutTextBox.Controls.Add(this._textBox, 1, 0);
			this._tableLayoutTextBox.Controls.Add(this._labelPrefix, 0, 0);
			this._tableLayoutTextBox.Controls.Add(this._labelExtension, 2, 0);
			this._tableLayoutTextBox.Location = new System.Drawing.Point(15, 23);
			this._tableLayoutTextBox.Margin = new System.Windows.Forms.Padding(15, 0, 0, 15);
			this._tableLayoutTextBox.Name = "_tableLayoutTextBox";
			this._tableLayoutTextBox.RowCount = 1;
			this._tableLayoutTextBox.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutTextBox.Size = new System.Drawing.Size(399, 26);
			this._tableLayoutTextBox.TabIndex = 1;
			// 
			// _textBox
			// 
			this._textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._textBox, null);
			this.locExtender.SetLocalizationComment(this._textBox, null);
			this.locExtender.SetLocalizationPriority(this._textBox, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._textBox, "_textBox");
			this._textBox.Location = new System.Drawing.Point(38, 3);
			this._textBox.Name = "_textBox";
			this._textBox.Size = new System.Drawing.Size(323, 20);
			this._textBox.TabIndex = 1;
			this._textBox.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// _labelPrefix
			// 
			this._labelPrefix.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelPrefix.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelPrefix, null);
			this.locExtender.SetLocalizationComment(this._labelPrefix, null);
			this.locExtender.SetLocalizationPriority(this._labelPrefix, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelPrefix, "_labelPrefix");
			this._labelPrefix.Location = new System.Drawing.Point(0, 6);
			this._labelPrefix.Margin = new System.Windows.Forms.Padding(0);
			this._labelPrefix.Name = "_labelPrefix";
			this._labelPrefix.Size = new System.Drawing.Size(35, 13);
			this._labelPrefix.TabIndex = 0;
			this._labelPrefix.Text = "label1";
			// 
			// _labelExtension
			// 
			this._labelExtension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelExtension.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelExtension, null);
			this.locExtender.SetLocalizationComment(this._labelExtension, null);
			this.locExtender.SetLocalizationPriority(this._labelExtension, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelExtension, "_labelExtension");
			this._labelExtension.Location = new System.Drawing.Point(364, 6);
			this._labelExtension.Margin = new System.Windows.Forms.Padding(0);
			this._labelExtension.Name = "_labelExtension";
			this._labelExtension.Size = new System.Drawing.Size(35, 13);
			this._labelExtension.TabIndex = 2;
			this._labelExtension.Text = "label2";
			// 
			// _labelChangeNameTo
			// 
			this._labelChangeNameTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelChangeNameTo.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelChangeNameTo, 2);
			this.locExtender.SetLocalizableToolTip(this._labelChangeNameTo, null);
			this.locExtender.SetLocalizationComment(this._labelChangeNameTo, null);
			this.locExtender.SetLocalizingId(this._labelChangeNameTo, "DialogBoxes.ComponentFileRenamingDlg._labelChangeNameTo");
			this._labelChangeNameTo.Location = new System.Drawing.Point(0, 0);
			this._labelChangeNameTo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
			this._labelChangeNameTo.Name = "_labelChangeNameTo";
			this._labelChangeNameTo.Size = new System.Drawing.Size(414, 13);
			this._labelChangeNameTo.TabIndex = 0;
			this._labelChangeNameTo.Text = "Change Name To:";
			// 
			// _flowLayoutShortcuts
			// 
			this._flowLayoutShortcuts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._flowLayoutShortcuts.AutoSize = true;
			this._flowLayoutShortcuts.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.SetColumnSpan(this._flowLayoutShortcuts, 2);
			this._flowLayoutShortcuts.Controls.Add(this._labelShortcuts);
			this._flowLayoutShortcuts.Controls.Add(this._labelShortcutsHint);
			this._flowLayoutShortcuts.Location = new System.Drawing.Point(0, 64);
			this._flowLayoutShortcuts.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
			this._flowLayoutShortcuts.Name = "_flowLayoutShortcuts";
			this._flowLayoutShortcuts.Size = new System.Drawing.Size(414, 13);
			this._flowLayoutShortcuts.TabIndex = 7;
			this._flowLayoutShortcuts.WrapContents = false;
			// 
			// _labelShortcuts
			// 
			this._labelShortcuts.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelShortcuts, null);
			this.locExtender.SetLocalizationComment(this._labelShortcuts, null);
			this.locExtender.SetLocalizingId(this._labelShortcuts, "DialogBoxes.ComponentFileRenamingDlg._labelShortcuts");
			this._labelShortcuts.Location = new System.Drawing.Point(0, 0);
			this._labelShortcuts.Margin = new System.Windows.Forms.Padding(0);
			this._labelShortcuts.Name = "_labelShortcuts";
			this._labelShortcuts.Size = new System.Drawing.Size(52, 13);
			this._labelShortcuts.TabIndex = 0;
			this._labelShortcuts.Text = "Shortcuts";
			// 
			// _labelShortcutsHint
			// 
			this._labelShortcutsHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelShortcutsHint.AutoEllipsis = true;
			this._labelShortcutsHint.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelShortcutsHint, null);
			this.locExtender.SetLocalizationComment(this._labelShortcutsHint, null);
			this.locExtender.SetLocalizingId(this._labelShortcutsHint, "DialogBoxes.ComponentFileRenamingDlg._labelShortcutsHint");
			this._labelShortcutsHint.Location = new System.Drawing.Point(82, 0);
			this._labelShortcutsHint.Margin = new System.Windows.Forms.Padding(30, 0, 0, 0);
			this._labelShortcutsHint.Name = "_labelShortcutsHint";
			this._labelShortcutsHint.Size = new System.Drawing.Size(243, 13);
			this._labelShortcutsHint.TabIndex = 1;
			this._labelShortcutsHint.Text = "(hover over each link to read about how it is used)";
			// 
			// _linkSource
			// 
			this._linkSource.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._linkSource.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkSource, "Source:\\nThe recording of the session. Used to satisfy\\nthe \'Source Recording\' st" +
        "age of the session.");
			this.locExtender.SetLocalizationComment(this._linkSource, null);
			this.locExtender.SetLocalizingId(this._linkSource, "DialogBoxes.ComponentFileRenamingDlg._linkSource");
			this._linkSource.Location = new System.Drawing.Point(82, 92);
			this._linkSource.Name = "_linkSource";
			this._linkSource.Size = new System.Drawing.Size(41, 13);
			this._linkSource.TabIndex = 2;
			this._linkSource.TabStop = true;
			this._linkSource.Text = "Source";
			// 
			// _labelNonSayMoreImportHint
			// 
			this._labelNonSayMoreImportHint.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelNonSayMoreImportHint.AutoEllipsis = true;
			this._labelNonSayMoreImportHint.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelNonSayMoreImportHint, 2);
			this.locExtender.SetLocalizableToolTip(this._labelNonSayMoreImportHint, null);
			this.locExtender.SetLocalizationComment(this._labelNonSayMoreImportHint, null);
			this.locExtender.SetLocalizingId(this._labelNonSayMoreImportHint, "DialogBoxes.ComponentFileRenamingDlg._labelNonSayMoreImportHint");
			this._labelNonSayMoreImportHint.Location = new System.Drawing.Point(15, 130);
			this._labelNonSayMoreImportHint.Margin = new System.Windows.Forms.Padding(15, 25, 0, 15);
			this._labelNonSayMoreImportHint.Name = "_labelNonSayMoreImportHint";
			this._labelNonSayMoreImportHint.Size = new System.Drawing.Size(380, 26);
			this._labelNonSayMoreImportHint.TabIndex = 4;
			this._labelNonSayMoreImportHint.Text = "Use the following only when you are importing annotations produced outside of Say" +
    "More";
			// 
			// _linkCareful
			// 
			this._linkCareful.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._linkCareful.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkCareful, "Careful Speech:\\nSlow, careful speech re-speaking\\nof the source recording.");
			this.locExtender.SetLocalizationComment(this._linkCareful, null);
			this.locExtender.SetLocalizingId(this._linkCareful, "DialogBoxes.ComponentFileRenamingDlg._linkCareful");
			this._linkCareful.Location = new System.Drawing.Point(63, 171);
			this._linkCareful.Name = "_linkCareful";
			this._linkCareful.Size = new System.Drawing.Size(80, 13);
			this._linkCareful.TabIndex = 5;
			this._linkCareful.TabStop = true;
			this._linkCareful.Text = "Careful Speech";
			// 
			// _linkOralTranslation
			// 
			this._linkOralTranslation.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._linkOralTranslation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkOralTranslation, "Oral Translation:\\nRecording of a translation\\nof the source recording.");
			this.locExtender.SetLocalizationComment(this._linkOralTranslation, null);
			this.locExtender.SetLocalizingId(this._linkOralTranslation, "DialogBoxes.ComponentFileRenamingDlg._linkOralTranslation");
			this._linkOralTranslation.Location = new System.Drawing.Point(269, 171);
			this._linkOralTranslation.Name = "_linkOralTranslation";
			this._linkOralTranslation.Size = new System.Drawing.Size(81, 13);
			this._linkOralTranslation.TabIndex = 6;
			this._linkOralTranslation.TabStop = true;
			this._linkOralTranslation.Text = "Oral Translation";
			// 
			// _messagePanel
			// 
			this._tableLayout.SetColumnSpan(this._messagePanel, 2);
			this._messagePanel.Controls.Add(this._labelMessage);
			this._messagePanel.Controls.Add(this._warningIcon);
			this._messagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._messagePanel.Location = new System.Drawing.Point(3, 210);
			this._messagePanel.Name = "_messagePanel";
			this._messagePanel.Size = new System.Drawing.Size(408, 58);
			this._messagePanel.TabIndex = 11;
			// 
			// _warningIcon
			// 
			this._warningIcon.Dock = System.Windows.Forms.DockStyle.Left;
			this._warningIcon.Image = global::SayMore.Properties.Resources.kimidWarning;
			this.locExtender.SetLocalizableToolTip(this._warningIcon, null);
			this.locExtender.SetLocalizationComment(this._warningIcon, null);
			this.locExtender.SetLocalizationPriority(this._warningIcon, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._warningIcon, "ComponentFileRenamingDialog._warningIcon");
			this._warningIcon.Location = new System.Drawing.Point(0, 0);
			this._warningIcon.Name = "_warningIcon";
			this._warningIcon.Size = new System.Drawing.Size(32, 58);
			this._warningIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this._warningIcon.TabIndex = 10;
			this._warningIcon.TabStop = false;
			this._warningIcon.Visible = false;
			// 
			// _labelMessage
			// 
			this._labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this._labelMessage, null);
			this.locExtender.SetLocalizationComment(this._labelMessage, null);
			this.locExtender.SetLocalizationPriority(this._labelMessage, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelMessage, "_labelMessage");
			this._labelMessage.Location = new System.Drawing.Point(32, 0);
			this._labelMessage.Margin = new System.Windows.Forms.Padding(15, 15, 10, 0);
			this._labelMessage.Name = "_labelMessage";
			this._labelMessage.Size = new System.Drawing.Size(376, 58);
			this._labelMessage.TabIndex = 9;
			this._labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// ComponentFileRenamingDialog
			// 
			this.AcceptButton = this._buttonRename;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(444, 337);
			this.ControlBox = false;
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.ComponentFileRenamingDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(455, 350);
			this.Name = "ComponentFileRenamingDialog";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Rename";
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this._tableLayoutButtons.ResumeLayout(false);
			this._tableLayoutButtons.PerformLayout();
			this._tableLayoutTextBox.ResumeLayout(false);
			this._tableLayoutTextBox.PerformLayout();
			this._flowLayoutShortcuts.ResumeLayout(false);
			this._flowLayoutShortcuts.PerformLayout();
			this._messagePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._warningIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Button _buttonRename;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.TextBox _textBox;
		private System.Windows.Forms.Label _labelPrefix;
		private System.Windows.Forms.Label _labelMessage;
		private System.Windows.Forms.Label _labelExtension;
		private L10NSharp.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Label _labelChangeNameTo;
		private System.Windows.Forms.FlowLayoutPanel _flowLayoutShortcuts;
		private System.Windows.Forms.Label _labelShortcuts;
		private System.Windows.Forms.Label _labelShortcutsHint;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutTextBox;
		private System.Windows.Forms.LinkLabel _linkSource;
		private System.Windows.Forms.LinkLabel _linkConsent;
		private System.Windows.Forms.Label _labelNonSayMoreImportHint;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutButtons;
		private System.Windows.Forms.Label _labelReadAboutRenaming;
		private System.Windows.Forms.LinkLabel _linkCareful;
		private System.Windows.Forms.LinkLabel _linkOralTranslation;
		private System.Windows.Forms.LinkLabel _linkTranscription;
		private System.Windows.Forms.Button _buttonReadAboutRenaming;
		private System.Windows.Forms.LinkLabel _linkWrittenTranslation;
		private System.Windows.Forms.Panel _messagePanel;
		private System.Windows.Forms.PictureBox _warningIcon;
	}
}