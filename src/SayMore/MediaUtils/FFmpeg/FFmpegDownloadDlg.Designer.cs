using SayMore.UI.LowLevelControls;

namespace SayMore.Media.FFmpeg
{
	partial class FFmpegDownloadDlg
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
			this._labelDownloadAndInstall = new System.Windows.Forms.Label();
			this._labelInstallFromZipFile = new System.Windows.Forms.Label();
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._labelOr2 = new System.Windows.Forms.Label();
			this._labelOr1 = new System.Windows.Forms.Label();
			this._linkDownloadAndInstall = new System.Windows.Forms.LinkLabel();
			this._labelOptionA = new System.Windows.Forms.Label();
			this._labelOptionB = new System.Windows.Forms.Label();
			this._labelCopyFromAnotherComputer = new System.Windows.Forms.Label();
			this._labelOptionC = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this._linkManualDownload = new System.Windows.Forms.LinkLabel();
			this._labelStepB1 = new System.Windows.Forms.Label();
			this._labelStepB2 = new System.Windows.Forms.Label();
			this._linkInstallFromZipFile = new System.Windows.Forms.LinkLabel();
			this._linkCopyFromFolder = new System.Windows.Forms.LinkLabel();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._labelOverview = new System.Windows.Forms.Label();
			this._buttonAbort = new System.Windows.Forms.Button();
			this._labelStatus = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._progressControl = new SayMore.UI.LowLevelControls.ProgressControl();
			this._tableLayoutPanel.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _labelDownloadAndInstall
			// 
			this._labelDownloadAndInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelDownloadAndInstall.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelDownloadAndInstall, null);
			this.locExtender.SetLocalizationComment(this._labelDownloadAndInstall, null);
			this.locExtender.SetLocalizationPriority(this._labelDownloadAndInstall, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this._labelDownloadAndInstall, "DialogBoxes.FFmpegDownloadDlg._labelDownloadAndInstall");
			this._labelDownloadAndInstall.Location = new System.Drawing.Point(20, 0);
			this._labelDownloadAndInstall.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
			this._labelDownloadAndInstall.Name = "_labelDownloadAndInstall";
			this._labelDownloadAndInstall.Size = new System.Drawing.Size(458, 26);
			this._labelDownloadAndInstall.TabIndex = 18;
			this._labelDownloadAndInstall.Text = "If you have a reliable Internet connection, you can let SayMore download and inst" +
    "all FFmpeg automatically. The file is large (about 20 MB).";
			// 
			// _labelInstallFromZipFile
			// 
			this._labelInstallFromZipFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelInstallFromZipFile.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelInstallFromZipFile, null);
			this.locExtender.SetLocalizationComment(this._labelInstallFromZipFile, null);
			this.locExtender.SetLocalizationPriority(this._labelInstallFromZipFile, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this._labelInstallFromZipFile, "DialogBoxes.FFmpegDownloadDlg._labelInstallFromZipFile");
			this._labelInstallFromZipFile.Location = new System.Drawing.Point(20, 71);
			this._labelInstallFromZipFile.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
			this._labelInstallFromZipFile.Name = "_labelInstallFromZipFile";
			this._labelInstallFromZipFile.Size = new System.Drawing.Size(458, 13);
			this._labelInstallFromZipFile.TabIndex = 19;
			this._labelInstallFromZipFile.Text = "If you use a download utility to deal with an unreliable Internet connection, fol" +
    "low these steps:";
			// 
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.ColumnCount = 2;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 1);
			this._tableLayoutPanel.Controls.Add(this._buttonCancel, 1, 5);
			this._tableLayoutPanel.Controls.Add(this._labelOverview, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._buttonAbort, 0, 4);
			this._tableLayoutPanel.Controls.Add(this._progressControl, 1, 4);
			this._tableLayoutPanel.Controls.Add(this._labelStatus, 1, 3);
			this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel.Location = new System.Drawing.Point(15, 15);
			this._tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 6;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.Size = new System.Drawing.Size(484, 405);
			this._tableLayoutPanel.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this._tableLayoutPanel.SetColumnSpan(this.tableLayoutPanel1, 2);
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this._linkDownloadAndInstall, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this._labelOr2, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this._labelOptionA, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._labelDownloadAndInstall, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this._labelOr1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this._labelOptionB, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this._labelCopyFromAnotherComputer, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this._labelInstallFromZipFile, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this._labelOptionC, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this._linkCopyFromFolder, 1, 8);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 24);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 9;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(478, 214);
			this.tableLayoutPanel1.TabIndex = 15;
			// 
			// _labelOr2
			// 
			this._labelOr2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOr2.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this._labelOr2, 2);
			this.locExtender.SetLocalizableToolTip(this._labelOr2, null);
			this.locExtender.SetLocalizationComment(this._labelOr2, null);
			this.locExtender.SetLocalizationPriority(this._labelOr2, L10NSharp.LocalizationPriority.Low);
			this.locExtender.SetLocalizingId(this._labelOr2, "DialogBoxes.FFmpegDownloadDlg._labelOr2");
			this._labelOr2.Location = new System.Drawing.Point(3, 139);
			this._labelOr2.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
			this._labelOr2.Name = "_labelOr2";
			this._labelOr2.Size = new System.Drawing.Size(472, 13);
			this._labelOr2.TabIndex = 24;
			this._labelOr2.Text = "OR";
			this._labelOr2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _labelOr1
			// 
			this._labelOr1.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this._labelOr1, 2);
			this._labelOr1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this._labelOr1, null);
			this.locExtender.SetLocalizationComment(this._labelOr1, null);
			this.locExtender.SetLocalizationPriority(this._labelOr1, L10NSharp.LocalizationPriority.Low);
			this.locExtender.SetLocalizingId(this._labelOr1, "DialogBoxes.FFmpegDownloadDlg._labelOr1");
			this._labelOr1.Location = new System.Drawing.Point(3, 50);
			this._labelOr1.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
			this._labelOr1.Name = "_labelOr1";
			this._labelOr1.Size = new System.Drawing.Size(472, 13);
			this._labelOr1.TabIndex = 23;
			this._labelOr1.Text = "OR";
			this._labelOr1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _linkDownloadAndInstall
			// 
			this._linkDownloadAndInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._linkDownloadAndInstall.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkDownloadAndInstall, null);
			this.locExtender.SetLocalizationComment(this._linkDownloadAndInstall, null);
			this.locExtender.SetLocalizationPriority(this._linkDownloadAndInstall, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._linkDownloadAndInstall, "DialogBoxes.FFmpegDownloadDlg.FFmpegDownloadDlg._linkDownloadAndInstall");
			this._linkDownloadAndInstall.Location = new System.Drawing.Point(20, 29);
			this._linkDownloadAndInstall.Margin = new System.Windows.Forms.Padding(0);
			this._linkDownloadAndInstall.Name = "_linkDownloadAndInstall";
			this._linkDownloadAndInstall.Size = new System.Drawing.Size(458, 13);
			this._linkDownloadAndInstall.TabIndex = 22;
			this._linkDownloadAndInstall.TabStop = true;
			this._linkDownloadAndInstall.Text = "Download and install FFmpeg";
			this._linkDownloadAndInstall.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleDownloadAndInstallClicked);
			// 
			// _labelOptionA
			// 
			this._labelOptionA.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelOptionA, null);
			this.locExtender.SetLocalizationComment(this._labelOptionA, null);
			this.locExtender.SetLocalizationPriority(this._labelOptionA, L10NSharp.LocalizationPriority.Low);
			this.locExtender.SetLocalizingId(this._labelOptionA, "DialogBoxes.FFmpegDownloadDlg._labelOptionA");
			this._labelOptionA.Location = new System.Drawing.Point(0, 0);
			this._labelOptionA.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelOptionA.Name = "_labelOptionA";
			this._labelOptionA.Size = new System.Drawing.Size(17, 13);
			this._labelOptionA.TabIndex = 25;
			this._labelOptionA.Text = "A)";
			// 
			// _labelOptionB
			// 
			this._labelOptionB.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelOptionB, null);
			this.locExtender.SetLocalizationComment(this._labelOptionB, null);
			this.locExtender.SetLocalizationPriority(this._labelOptionB, L10NSharp.LocalizationPriority.Low);
			this.locExtender.SetLocalizingId(this._labelOptionB, "DialogBoxes.FFmpegDownloadDlg._labelOptionB");
			this._labelOptionB.Location = new System.Drawing.Point(0, 71);
			this._labelOptionB.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelOptionB.Name = "_labelOptionB";
			this._labelOptionB.Size = new System.Drawing.Size(17, 13);
			this._labelOptionB.TabIndex = 26;
			this._labelOptionB.Text = "B)";
			// 
			// _labelCopyFromAnotherComputer
			// 
			this._labelCopyFromAnotherComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelCopyFromAnotherComputer.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelCopyFromAnotherComputer, null);
			this.locExtender.SetLocalizationComment(this._labelCopyFromAnotherComputer, "Parameter is the full path to the \"FFmpeg for SayMore\" folder on this computer.");
			this.locExtender.SetLocalizationPriority(this._labelCopyFromAnotherComputer, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelCopyFromAnotherComputer, "DialogBoxes.FFmpegDownloadDlg.FFmpegDownloadDlg._labelCopyFromAnotherComputer");
			this._labelCopyFromAnotherComputer.Location = new System.Drawing.Point(20, 160);
			this._labelCopyFromAnotherComputer.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
			this._labelCopyFromAnotherComputer.Name = "_labelCopyFromAnotherComputer";
			this._labelCopyFromAnotherComputer.Size = new System.Drawing.Size(458, 26);
			this._labelCopyFromAnotherComputer.TabIndex = 20;
			this._labelCopyFromAnotherComputer.Text = "If you have the folder \"FFmpeg for SayMore\" from another computer (e.g., {0}), Sa" +
    "yMore can copy it to the proper location on this computer:";
			// 
			// _labelOptionC
			// 
			this._labelOptionC.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelOptionC, null);
			this.locExtender.SetLocalizationComment(this._labelOptionC, null);
			this.locExtender.SetLocalizationPriority(this._labelOptionC, L10NSharp.LocalizationPriority.Low);
			this.locExtender.SetLocalizingId(this._labelOptionC, "DialogBoxes.FFmpegDownloadDlg._labelOptionC");
			this._labelOptionC.Location = new System.Drawing.Point(0, 160);
			this._labelOptionC.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelOptionC.Name = "_labelOptionC";
			this._labelOptionC.Size = new System.Drawing.Size(17, 13);
			this._labelOptionC.TabIndex = 26;
			this._labelOptionC.Text = "C)";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this._linkManualDownload, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this._labelStepB1, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this._labelStepB2, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this._linkInstallFromZipFile, 1, 2);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(23, 90);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(342, 38);
			this.tableLayoutPanel2.TabIndex = 19;
			// 
			// _linkManualDownload
			// 
			this._linkManualDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._linkManualDownload.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkManualDownload, null);
			this.locExtender.SetLocalizationComment(this._linkManualDownload, null);
			this.locExtender.SetLocalizationPriority(this._linkManualDownload, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this._linkManualDownload, "DialogBoxes.FFmpegDownloadDlg._linkManualDownload");
			this._linkManualDownload.Location = new System.Drawing.Point(22, 0);
			this._linkManualDownload.Margin = new System.Windows.Forms.Padding(0, 0, 3, 4);
			this._linkManualDownload.Name = "_linkManualDownload";
			this._linkManualDownload.Size = new System.Drawing.Size(317, 13);
			this._linkManualDownload.TabIndex = 9;
			this._linkManualDownload.TabStop = true;
			this._linkManualDownload.Text = "Open Internet browser to page containing FFmpegForSayMore.zip";
			this._linkManualDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleManualDownloadLinkClicked);
			// 
			// _labelStepB1
			// 
			this._labelStepB1.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelStepB1, null);
			this.locExtender.SetLocalizationComment(this._labelStepB1, null);
			this.locExtender.SetLocalizationPriority(this._labelStepB1, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelStepB1, "DialogBoxes.FFmpegDownloadDlg._labelStepB1");
			this._labelStepB1.Location = new System.Drawing.Point(3, 0);
			this._labelStepB1.Name = "_labelStepB1";
			this._labelStepB1.Size = new System.Drawing.Size(16, 13);
			this._labelStepB1.TabIndex = 21;
			this._labelStepB1.Text = "1:";
			// 
			// _labelStepB2
			// 
			this._labelStepB2.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelStepB2, null);
			this.locExtender.SetLocalizationComment(this._labelStepB2, null);
			this.locExtender.SetLocalizationPriority(this._labelStepB2, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelStepB2, "DialogBoxes.FFmpegDownloadDlg._labelStepB2");
			this._labelStepB2.Location = new System.Drawing.Point(3, 17);
			this._labelStepB2.Name = "_labelStepB2";
			this._labelStepB2.Size = new System.Drawing.Size(16, 13);
			this._labelStepB2.TabIndex = 22;
			this._labelStepB2.Text = "2:";
			// 
			// _linkInstallFromZipFile
			// 
			this._linkInstallFromZipFile.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkInstallFromZipFile, null);
			this.locExtender.SetLocalizationComment(this._linkInstallFromZipFile, null);
			this.locExtender.SetLocalizationPriority(this._linkInstallFromZipFile, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._linkInstallFromZipFile, "DialogBoxes.FFmpegDownloadDlg.FFmpegDownloadDlg._linkInstallFromZipFile");
			this._linkInstallFromZipFile.Location = new System.Drawing.Point(22, 17);
			this._linkInstallFromZipFile.Margin = new System.Windows.Forms.Padding(0, 0, 3, 8);
			this._linkInstallFromZipFile.Name = "_linkInstallFromZipFile";
			this._linkInstallFromZipFile.Size = new System.Drawing.Size(171, 13);
			this._linkInstallFromZipFile.TabIndex = 23;
			this._linkInstallFromZipFile.TabStop = true;
			this._linkInstallFromZipFile.Text = "Show SayMore where you saved it";
			this._linkInstallFromZipFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleInstallFromZipFileClick);
			// 
			// _linkCopyFromFolder
			// 
			this._linkCopyFromFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._linkCopyFromFolder.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkCopyFromFolder, null);
			this.locExtender.SetLocalizationComment(this._linkCopyFromFolder, null);
			this.locExtender.SetLocalizationPriority(this._linkCopyFromFolder, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this._linkCopyFromFolder, "DialogBoxes.FFmpegDownloadDlg._linkCopyFromFolder");
			this._linkCopyFromFolder.Location = new System.Drawing.Point(20, 189);
			this._linkCopyFromFolder.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
			this._linkCopyFromFolder.Name = "_linkCopyFromFolder";
			this._linkCopyFromFolder.Size = new System.Drawing.Size(458, 13);
			this._linkCopyFromFolder.TabIndex = 27;
			this._linkCopyFromFolder.TabStop = true;
			this._linkCopyFromFolder.Text = "Show SayMore where to find the folder";
			this._linkCopyFromFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleCopyFromFolderClick);
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.FFmpegDownloadDlg.CancelButton");
			this._buttonCancel.Location = new System.Drawing.Point(406, 379);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 14;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _labelOverview
			// 
			this._labelOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOverview.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._labelOverview, 2);
			this.locExtender.SetLocalizableToolTip(this._labelOverview, null);
			this.locExtender.SetLocalizationComment(this._labelOverview, null);
			this.locExtender.SetLocalizingId(this._labelOverview, "DialogBoxes.FFmpegDownloadDlg.OverviewLabel");
			this._labelOverview.Location = new System.Drawing.Point(0, 0);
			this._labelOverview.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this._labelOverview.Name = "_labelOverview";
			this._labelOverview.Size = new System.Drawing.Size(484, 13);
			this._labelOverview.TabIndex = 10;
			this._labelOverview.Text = "You have three options for getting FFmpeg:";
			// 
			// _buttonAbort
			// 
			this.locExtender.SetLocalizableToolTip(this._buttonAbort, null);
			this.locExtender.SetLocalizationComment(this._buttonAbort, null);
			this.locExtender.SetLocalizingId(this._buttonAbort, "DialogBoxes.FFmpegDownloadDlg._buttonAbort");
			this._buttonAbort.Location = new System.Drawing.Point(0, 269);
			this._buttonAbort.Margin = new System.Windows.Forms.Padding(0, 0, 5, 15);
			this._buttonAbort.Name = "_buttonAbort";
			this._buttonAbort.Size = new System.Drawing.Size(75, 26);
			this._buttonAbort.TabIndex = 21;
			this._buttonAbort.Text = "Abort";
			this._buttonAbort.UseVisualStyleBackColor = true;
			this._buttonAbort.Visible = false;
			this._buttonAbort.Click += new System.EventHandler(this.HandleAbortButtonClick);
			// 
			// _labelStatus
			// 
			this._labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelStatus.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelStatus, null);
			this.locExtender.SetLocalizationComment(this._labelStatus, null);
			this.locExtender.SetLocalizationPriority(this._labelStatus, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelStatus, "DialogBoxes.FFmpegDownloadDlg._labelStatus");
			this._labelStatus.Location = new System.Drawing.Point(80, 256);
			this._labelStatus.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
			this._labelStatus.Name = "_labelStatus";
			this._labelStatus.Size = new System.Drawing.Size(404, 13);
			this._labelStatus.TabIndex = 15;
			this._labelStatus.Text = "#";
			this._labelStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = "DialogBoxes.FFmpegDownloadDlg";
			// 
			// _progressControl
			// 
			this._progressControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._progressControl, null);
			this.locExtender.SetLocalizationComment(this._progressControl, null);
			this.locExtender.SetLocalizationPriority(this._progressControl, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._progressControl, "DialogBoxes.FFmpegDownloadDlg.ProgressControl");
			this._progressControl.Location = new System.Drawing.Point(80, 269);
			this._progressControl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._progressControl.Name = "_progressControl";
			this._progressControl.Size = new System.Drawing.Size(404, 72);
			this._progressControl.TabIndex = 13;
			this._progressControl.Visible = false;
			// 
			// FFmpegDownloadDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(514, 435);
			this.Controls.Add(this._tableLayoutPanel);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.FFmpegDownloadDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(520, 431);
			this.Name = "FFmpegDownloadDlg";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Install FFmpeg";
			this._tableLayoutPanel.ResumeLayout(false);
			this._tableLayoutPanel.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private System.Windows.Forms.LinkLabel _linkManualDownload;
		private System.Windows.Forms.Label _labelOverview;
		private ProgressControl _progressControl;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Label _labelStatus;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label _labelDownloadAndInstall;
		private System.Windows.Forms.Label _labelInstallFromZipFile;
		private System.Windows.Forms.Label _labelCopyFromAnotherComputer;
		private System.Windows.Forms.Button _buttonAbort;
		private System.Windows.Forms.LinkLabel _linkDownloadAndInstall;
		private System.Windows.Forms.Label _labelStepB1;
		private System.Windows.Forms.Label _labelStepB2;
		private System.Windows.Forms.LinkLabel _linkInstallFromZipFile;
		private System.Windows.Forms.Label _labelOr1;
		private System.Windows.Forms.Label _labelOr2;
		private System.Windows.Forms.Label _labelOptionA;
		private System.Windows.Forms.Label _labelOptionC;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label _labelOptionB;
		private System.Windows.Forms.LinkLabel _linkCopyFromFolder;
	}
}