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
			this._labelInstallFromZipFile1 = new System.Windows.Forms.Label();
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this._buttonAbort = new System.Windows.Forms.Button();
			this._buttonOpenFolderLocation = new System.Windows.Forms.Button();
			this._buttonDownloadAndInstall = new System.Windows.Forms.Button();
			this._buttonInstallFromZipFile = new System.Windows.Forms.Button();
			this._labelOverview = new System.Windows.Forms.Label();
			this._progressControl = new SayMore.UI.LowLevelControls.ProgressControl();
			this._labelStatus = new System.Windows.Forms.Label();
			this._labelFFmpegFolderLocation = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this._labelInstallFromZipFile2 = new System.Windows.Forms.Label();
			this._linkManualDownload = new System.Windows.Forms.LinkLabel();
			this._labelFinishedCopying = new System.Windows.Forms.Label();
			this._buttonCancel = new System.Windows.Forms.Button();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._timerCheckForFFmpeg = new System.Windows.Forms.Timer(this.components);
			this._tableLayoutPanel.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _labelDownloadAndInstall
			// 
			this._labelDownloadAndInstall.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelDownloadAndInstall, null);
			this.locExtender.SetLocalizationComment(this._labelDownloadAndInstall, null);
			this.locExtender.SetLocalizationPriority(this._labelDownloadAndInstall, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this._labelDownloadAndInstall, "DialogBoxes.FFmpegDownloadDlg._labelDownloadAndInstall");
			this._labelDownloadAndInstall.Location = new System.Drawing.Point(96, 51);
			this._labelDownloadAndInstall.Margin = new System.Windows.Forms.Padding(3, 0, 3, 15);
			this._labelDownloadAndInstall.Name = "_labelDownloadAndInstall";
			this._labelDownloadAndInstall.Size = new System.Drawing.Size(380, 26);
			this._labelDownloadAndInstall.TabIndex = 18;
			this._labelDownloadAndInstall.Text = "SayMore can download and install FFmpeg automatically if this computer has a reli" +
    "able Internet connection and can download a large file (about 20 MB).";
			// 
			// _labelInstallFromZipFile1
			// 
			this._labelInstallFromZipFile1.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelInstallFromZipFile1, null);
			this.locExtender.SetLocalizationComment(this._labelInstallFromZipFile1, null);
			this.locExtender.SetLocalizationPriority(this._labelInstallFromZipFile1, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this._labelInstallFromZipFile1, "DialogBoxes.FFmpegDownloadDlg._labelInstallFromZipFile1");
			this._labelInstallFromZipFile1.Location = new System.Drawing.Point(3, 0);
			this._labelInstallFromZipFile1.Name = "_labelInstallFromZipFile1";
			this._labelInstallFromZipFile1.Size = new System.Drawing.Size(358, 26);
			this._labelInstallFromZipFile1.TabIndex = 19;
			this._labelInstallFromZipFile1.Text = "If you use a download utility to deal with an unreliable Internet connection, man" +
    "ually download this file:";
			// 
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.ColumnCount = 2;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.Controls.Add(this._buttonAbort, 0, 6);
			this._tableLayoutPanel.Controls.Add(this._buttonOpenFolderLocation, 0, 3);
			this._tableLayoutPanel.Controls.Add(this._buttonDownloadAndInstall, 0, 2);
			this._tableLayoutPanel.Controls.Add(this._buttonInstallFromZipFile, 0, 4);
			this._tableLayoutPanel.Controls.Add(this._labelOverview, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._progressControl, 1, 6);
			this._tableLayoutPanel.Controls.Add(this._labelStatus, 1, 5);
			this._tableLayoutPanel.Controls.Add(this._labelFFmpegFolderLocation, 0, 1);
			this._tableLayoutPanel.Controls.Add(this._labelDownloadAndInstall, 1, 2);
			this._tableLayoutPanel.Controls.Add(this.tableLayoutPanel2, 1, 4);
			this._tableLayoutPanel.Controls.Add(this._labelFinishedCopying, 1, 3);
			this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel.Location = new System.Drawing.Point(15, 15);
			this._tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 7;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.Size = new System.Drawing.Size(484, 350);
			this._tableLayoutPanel.TabIndex = 0;
			// 
			// _buttonAbort
			// 
			this.locExtender.SetLocalizableToolTip(this._buttonAbort, null);
			this.locExtender.SetLocalizationComment(this._buttonAbort, null);
			this.locExtender.SetLocalizingId(this._buttonAbort, "DialogBoxes.FFmpegDownloadDlg._buttonAbort");
			this._buttonAbort.Location = new System.Drawing.Point(0, 266);
			this._buttonAbort.Margin = new System.Windows.Forms.Padding(0, 0, 5, 15);
			this._buttonAbort.Name = "_buttonAbort";
			this._buttonAbort.Size = new System.Drawing.Size(75, 26);
			this._buttonAbort.TabIndex = 21;
			this._buttonAbort.Text = "Abort";
			this._buttonAbort.UseVisualStyleBackColor = true;
			this._buttonAbort.Visible = false;
			this._buttonAbort.Click += new System.EventHandler(this.HandleAbortButtonClick);
			// 
			// _buttonOpenFolderLocation
			// 
			this.locExtender.SetLocalizableToolTip(this._buttonOpenFolderLocation, null);
			this.locExtender.SetLocalizationComment(this._buttonOpenFolderLocation, null);
			this.locExtender.SetLocalizingId(this._buttonOpenFolderLocation, "DialogBoxes.FFmpegDownloadDlg._buttonFinishedCopying");
			this._buttonOpenFolderLocation.Location = new System.Drawing.Point(0, 105);
			this._buttonOpenFolderLocation.Margin = new System.Windows.Forms.Padding(0, 0, 5, 15);
			this._buttonOpenFolderLocation.Name = "_buttonOpenFolderLocation";
			this._buttonOpenFolderLocation.Size = new System.Drawing.Size(88, 39);
			this._buttonOpenFolderLocation.TabIndex = 17;
			this._buttonOpenFolderLocation.Text = "Open Folder Location";
			this._buttonOpenFolderLocation.UseVisualStyleBackColor = true;
			this._buttonOpenFolderLocation.Click += new System.EventHandler(this.HandleOpenFolderLocationClick);
			// 
			// _buttonDownloadAndInstall
			// 
			this.locExtender.SetLocalizableToolTip(this._buttonDownloadAndInstall, null);
			this.locExtender.SetLocalizationComment(this._buttonDownloadAndInstall, null);
			this.locExtender.SetLocalizingId(this._buttonDownloadAndInstall, "DialogBoxes.FFmpegDownloadDlg._buttonDownloadAndInstall");
			this._buttonDownloadAndInstall.Location = new System.Drawing.Point(0, 51);
			this._buttonDownloadAndInstall.Margin = new System.Windows.Forms.Padding(0, 0, 5, 15);
			this._buttonDownloadAndInstall.Name = "_buttonDownloadAndInstall";
			this._buttonDownloadAndInstall.Size = new System.Drawing.Size(88, 39);
			this._buttonDownloadAndInstall.TabIndex = 16;
			this._buttonDownloadAndInstall.Text = "Download and Install";
			this._buttonDownloadAndInstall.UseVisualStyleBackColor = true;
			this._buttonDownloadAndInstall.Click += new System.EventHandler(this.HandleDownloadAndInstallClicked);
			// 
			// _buttonInstallFromZipFile
			// 
			this.locExtender.SetLocalizableToolTip(this._buttonInstallFromZipFile, null);
			this.locExtender.SetLocalizationComment(this._buttonInstallFromZipFile, null);
			this.locExtender.SetLocalizingId(this._buttonInstallFromZipFile, "DialogBoxes.FFmpegDownloadDlg.InstallButton");
			this._buttonInstallFromZipFile.Location = new System.Drawing.Point(0, 159);
			this._buttonInstallFromZipFile.Margin = new System.Windows.Forms.Padding(0, 0, 5, 15);
			this._buttonInstallFromZipFile.Name = "_buttonInstallFromZipFile";
			this._buttonInstallFromZipFile.Size = new System.Drawing.Size(88, 39);
			this._buttonInstallFromZipFile.TabIndex = 13;
			this._buttonInstallFromZipFile.Text = "Install from\r\nZip File...";
			this._buttonInstallFromZipFile.UseVisualStyleBackColor = true;
			this._buttonInstallFromZipFile.Click += new System.EventHandler(this.HandleInstallFromZipFileClick);
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
			this._labelOverview.Margin = new System.Windows.Forms.Padding(0);
			this._labelOverview.Name = "_labelOverview";
			this._labelOverview.Size = new System.Drawing.Size(484, 13);
			this._labelOverview.TabIndex = 10;
			this._labelOverview.Text = "SayMore is looking for the FFmpeg program in this folder:";
			// 
			// _progressControl
			// 
			this._progressControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._progressControl, null);
			this.locExtender.SetLocalizationComment(this._progressControl, null);
			this.locExtender.SetLocalizationPriority(this._progressControl, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._progressControl, "DialogBoxes.FFmpegDownloadDlg.ProgressControl");
			this._progressControl.Location = new System.Drawing.Point(93, 266);
			this._progressControl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._progressControl.Name = "_progressControl";
			this._progressControl.Size = new System.Drawing.Size(391, 79);
			this._progressControl.TabIndex = 13;
			this._progressControl.Visible = false;
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
			this._labelStatus.Location = new System.Drawing.Point(93, 253);
			this._labelStatus.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
			this._labelStatus.Name = "_labelStatus";
			this._labelStatus.Size = new System.Drawing.Size(391, 13);
			this._labelStatus.TabIndex = 15;
			this._labelStatus.Text = "#";
			this._labelStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// _labelFFmpegFolderLocation
			// 
			this._labelFFmpegFolderLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelFFmpegFolderLocation.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._labelFFmpegFolderLocation, 2);
			this._labelFFmpegFolderLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelFFmpegFolderLocation, null);
			this.locExtender.SetLocalizationComment(this._labelFFmpegFolderLocation, null);
			this.locExtender.SetLocalizationPriority(this._labelFFmpegFolderLocation, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelFFmpegFolderLocation, "DialogBoxes.FFmpegDownloadDlg.OrLabel");
			this._labelFFmpegFolderLocation.Location = new System.Drawing.Point(10, 23);
			this._labelFFmpegFolderLocation.Margin = new System.Windows.Forms.Padding(10, 10, 0, 15);
			this._labelFFmpegFolderLocation.Name = "_labelFFmpegFolderLocation";
			this._labelFFmpegFolderLocation.Size = new System.Drawing.Size(474, 13);
			this._labelFFmpegFolderLocation.TabIndex = 11;
			this._labelFFmpegFolderLocation.Text = "#";
			this._labelFFmpegFolderLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this._labelInstallFromZipFile2, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this._labelInstallFromZipFile1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this._linkManualDownload, 0, 1);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(96, 162);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(385, 73);
			this.tableLayoutPanel2.TabIndex = 19;
			// 
			// _labelInstallFromZipFile2
			// 
			this._labelInstallFromZipFile2.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelInstallFromZipFile2, null);
			this.locExtender.SetLocalizationComment(this._labelInstallFromZipFile2, null);
			this.locExtender.SetLocalizationPriority(this._labelInstallFromZipFile2, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this._labelInstallFromZipFile2, "DialogBoxes.FFmpegDownloadDlg._labelInstallFromZipFile2");
			this._labelInstallFromZipFile2.Location = new System.Drawing.Point(3, 47);
			this._labelInstallFromZipFile2.Name = "_labelInstallFromZipFile2";
			this._labelInstallFromZipFile2.Size = new System.Drawing.Size(343, 26);
			this._labelInstallFromZipFile2.TabIndex = 20;
			this._labelInstallFromZipFile2.Text = "When the download is complete, click {0} to specify the location of the downloade" +
    "d file for SayMore to install.";
			// 
			// _linkManualDownload
			// 
			this._linkManualDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._linkManualDownload.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkManualDownload, null);
			this.locExtender.SetLocalizationComment(this._linkManualDownload, null);
			this.locExtender.SetLocalizationPriority(this._linkManualDownload, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._linkManualDownload, "DialogBoxes.FFmpegDownloadDlg._linkManualDownload");
			this._linkManualDownload.Location = new System.Drawing.Point(10, 30);
			this._linkManualDownload.Margin = new System.Windows.Forms.Padding(10, 4, 0, 4);
			this._linkManualDownload.Name = "_linkManualDownload";
			this._linkManualDownload.Size = new System.Drawing.Size(375, 13);
			this._linkManualDownload.TabIndex = 9;
			this._linkManualDownload.TabStop = true;
			this._linkManualDownload.Text = "FFmpegForSayMore.zip";
			this._linkManualDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleManualDownloadLinkClicked);
			// 
			// _labelFinishedCopying
			// 
			this._labelFinishedCopying.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelFinishedCopying, null);
			this.locExtender.SetLocalizationComment(this._labelFinishedCopying, null);
			this.locExtender.SetLocalizationPriority(this._labelFinishedCopying, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this._labelFinishedCopying, "DialogBoxes.FFmpegDownloadDlg._labelFinishedCopying");
			this._labelFinishedCopying.Location = new System.Drawing.Point(96, 105);
			this._labelFinishedCopying.Margin = new System.Windows.Forms.Padding(3, 0, 3, 15);
			this._labelFinishedCopying.Name = "_labelFinishedCopying";
			this._labelFinishedCopying.Size = new System.Drawing.Size(384, 39);
			this._labelFinishedCopying.TabIndex = 20;
			this._labelFinishedCopying.Text = "If you have access to FFmpeg for SayMore on another computer, manually copy the F" +
    "FmpegForSayMore folder to this computer. Then this dialog box will close automat" +
    "ically.";
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.AutoSize = true;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.FFmpegDownloadDlg.CancelButton");
			this._buttonCancel.Location = new System.Drawing.Point(424, 365);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 14;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = null;
			// 
			// _timerCheckForFFmpeg
			// 
			this._timerCheckForFFmpeg.Interval = 500;
			this._timerCheckForFFmpeg.Tick += new System.EventHandler(this.HandleCheckForFFmpegTimerTick);
			// 
			// FFmpegDownloadDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(514, 403);
			this.Controls.Add(this._tableLayoutPanel);
			this.Controls.Add(this._buttonCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.FFmpegDownloadDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(520, 431);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(520, 431);
			this.Name = "FFmpegDownloadDlg";
			this.Padding = new System.Windows.Forms.Padding(15, 15, 15, 38);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Download FFmpeg";
			this._tableLayoutPanel.ResumeLayout(false);
			this._tableLayoutPanel.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private System.Windows.Forms.LinkLabel _linkManualDownload;
		private System.Windows.Forms.Label _labelOverview;
		private System.Windows.Forms.Label _labelFFmpegFolderLocation;
		private System.Windows.Forms.Button _buttonInstallFromZipFile;
		private ProgressControl _progressControl;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Label _labelStatus;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.Button _buttonOpenFolderLocation;
		private System.Windows.Forms.Button _buttonDownloadAndInstall;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label _labelDownloadAndInstall;
		private System.Windows.Forms.Label _labelInstallFromZipFile2;
		private System.Windows.Forms.Label _labelInstallFromZipFile1;
		private System.Windows.Forms.Label _labelFinishedCopying;
		private System.Windows.Forms.Button _buttonAbort;
		private System.Windows.Forms.Timer _timerCheckForFFmpeg;
	}
}