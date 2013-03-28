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
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this._buttonClose = new System.Windows.Forms.Button();
			this._labelOverview = new System.Windows.Forms.Label();
			this._linkAutoDownload = new System.Windows.Forms.LinkLabel();
			this._labelOr = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._linkManualDownload = new System.Windows.Forms.LinkLabel();
			this._buttonInstall = new System.Windows.Forms.Button();
			this._progressControl = new SayMore.UI.LowLevelControls.ProgressControl();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._labelStatus = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._tableLayoutPanel.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.ColumnCount = 3;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanel.Controls.Add(this._buttonClose, 2, 6);
			this._tableLayoutPanel.Controls.Add(this._labelOverview, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._linkAutoDownload, 0, 1);
			this._tableLayoutPanel.Controls.Add(this._labelOr, 0, 2);
			this._tableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 3);
			this._tableLayoutPanel.Controls.Add(this._progressControl, 0, 5);
			this._tableLayoutPanel.Controls.Add(this._buttonCancel, 1, 6);
			this._tableLayoutPanel.Controls.Add(this._labelStatus, 0, 4);
			this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel.Location = new System.Drawing.Point(15, 15);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 7;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutPanel.Size = new System.Drawing.Size(437, 242);
			this._tableLayoutPanel.TabIndex = 0;
			// 
			// _buttonClose
			// 
			this._buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._buttonClose, null);
			this.locExtender.SetLocalizationComment(this._buttonClose, null);
			this.locExtender.SetLocalizingId(this._buttonClose, "DialogBoxes.FFmpegDownloadDlg.CloseButton");
			this._buttonClose.Location = new System.Drawing.Point(362, 216);
			this._buttonClose.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._buttonClose.Name = "_buttonClose";
			this._buttonClose.Size = new System.Drawing.Size(75, 26);
			this._buttonClose.TabIndex = 6;
			this._buttonClose.Text = "Close";
			this._buttonClose.UseVisualStyleBackColor = true;
			// 
			// _labelOverview
			// 
			this._labelOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOverview.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._labelOverview, 3);
			this.locExtender.SetLocalizableToolTip(this._labelOverview, null);
			this.locExtender.SetLocalizationComment(this._labelOverview, null);
			this.locExtender.SetLocalizingId(this._labelOverview, "DialogBoxes.FFmpegDownloadDlg.OverviewLabel");
			this._labelOverview.Location = new System.Drawing.Point(0, 0);
			this._labelOverview.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
			this._labelOverview.Name = "_labelOverview";
			this._labelOverview.Size = new System.Drawing.Size(437, 13);
			this._labelOverview.TabIndex = 10;
			this._labelOverview.Text = "You have two options for downloading FFmpeg.";
			// 
			// _linkAutoDownload
			// 
			this._linkAutoDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._linkAutoDownload.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._linkAutoDownload, 3);
			this.locExtender.SetLocalizableToolTip(this._linkAutoDownload, null);
			this.locExtender.SetLocalizationComment(this._linkAutoDownload, null);
			this.locExtender.SetLocalizingId(this._linkAutoDownload, "DialogBoxes.FFmpegDownloadDlg._linkAutoDownload");
			this._linkAutoDownload.Location = new System.Drawing.Point(15, 28);
			this._linkAutoDownload.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
			this._linkAutoDownload.Name = "_linkAutoDownload";
			this._linkAutoDownload.Size = new System.Drawing.Size(422, 26);
			this._linkAutoDownload.TabIndex = 3;
			this._linkAutoDownload.TabStop = true;
			this._linkAutoDownload.Text = "1) Click here and SayMore will automatically download and install FFmpeg in the p" +
    "roper location.";
			// 
			// _labelOr
			// 
			this._labelOr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOr.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._labelOr, 3);
			this.locExtender.SetLocalizableToolTip(this._labelOr, null);
			this.locExtender.SetLocalizationComment(this._labelOr, null);
			this.locExtender.SetLocalizingId(this._labelOr, "DialogBoxes.FFmpegDownloadDlg.OrLabel");
			this._labelOr.Location = new System.Drawing.Point(0, 69);
			this._labelOr.Margin = new System.Windows.Forms.Padding(0, 15, 0, 15);
			this._labelOr.Name = "_labelOr";
			this._labelOr.Size = new System.Drawing.Size(437, 13);
			this._labelOr.TabIndex = 11;
			this._labelOr.Text = "- OR -";
			this._labelOr.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this._tableLayoutPanel.SetColumnSpan(this.tableLayoutPanel1, 3);
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._linkManualDownload, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._buttonInstall, 1, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 97);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(437, 41);
			this.tableLayoutPanel1.TabIndex = 12;
			// 
			// _linkManualDownload
			// 
			this._linkManualDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._linkManualDownload.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkManualDownload, null);
			this.locExtender.SetLocalizationComment(this._linkManualDownload, null);
			this.locExtender.SetLocalizingId(this._linkManualDownload, "DialogBoxes.FFmpegDownloadDlg._linkManualDownload");
			this._linkManualDownload.Location = new System.Drawing.Point(15, 0);
			this._linkManualDownload.Margin = new System.Windows.Forms.Padding(15, 0, 0, 15);
			this._linkManualDownload.Name = "_linkManualDownload";
			this._linkManualDownload.Size = new System.Drawing.Size(347, 26);
			this._linkManualDownload.TabIndex = 9;
			this._linkManualDownload.TabStop = true;
			this._linkManualDownload.Text = "2) Click here to download FFmpeg manually and then click \'Install\' to specify the" +
    " location of the downloaded file for SayMore to install.";
			// 
			// _buttonInstall
			// 
			this.locExtender.SetLocalizableToolTip(this._buttonInstall, null);
			this.locExtender.SetLocalizationComment(this._buttonInstall, null);
			this.locExtender.SetLocalizingId(this._buttonInstall, "DialogBoxes.FFmpegDownloadDlg.InstallButton");
			this._buttonInstall.Location = new System.Drawing.Point(362, 0);
			this._buttonInstall.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
			this._buttonInstall.Name = "_buttonInstall";
			this._buttonInstall.Size = new System.Drawing.Size(75, 26);
			this._buttonInstall.TabIndex = 13;
			this._buttonInstall.Text = "Install...";
			this._buttonInstall.UseVisualStyleBackColor = true;
			this._buttonInstall.Click += new System.EventHandler(this.HandleInstallClick);
			// 
			// _progressControl
			// 
			this._progressControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutPanel.SetColumnSpan(this._progressControl, 3);
			this.locExtender.SetLocalizableToolTip(this._progressControl, null);
			this.locExtender.SetLocalizationComment(this._progressControl, null);
			this.locExtender.SetLocalizationPriority(this._progressControl, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._progressControl, "FFmpegDownloadDlg.ProgressControl");
			this._progressControl.Location = new System.Drawing.Point(0, 151);
			this._progressControl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._progressControl.Name = "_progressControl";
			this._progressControl.Size = new System.Drawing.Size(437, 60);
			this._progressControl.TabIndex = 13;
			this._progressControl.Visible = false;
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.AutoSize = true;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.FFmpegDownloadDlg.CancelButton");
			this._buttonCancel.Location = new System.Drawing.Point(281, 216);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 14;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _labelStatus
			// 
			this._labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelStatus.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._labelStatus, 3);
			this.locExtender.SetLocalizableToolTip(this._labelStatus, null);
			this.locExtender.SetLocalizationComment(this._labelStatus, null);
			this.locExtender.SetLocalizationPriority(this._labelStatus, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelStatus, "FFmpegDownloadDlg._labelStatus");
			this._labelStatus.Location = new System.Drawing.Point(0, 138);
			this._labelStatus.Margin = new System.Windows.Forms.Padding(0);
			this._labelStatus.Name = "_labelStatus";
			this._labelStatus.Size = new System.Drawing.Size(437, 13);
			this._labelStatus.TabIndex = 15;
			this._labelStatus.Text = "#";
			this._labelStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// FFmpegDownloadDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(467, 272);
			this.Controls.Add(this._tableLayoutPanel);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.FFmpegDownloadDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FFmpegDownloadDlg";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Download FFmpeg";
			this._tableLayoutPanel.ResumeLayout(false);
			this._tableLayoutPanel.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private System.Windows.Forms.LinkLabel _linkAutoDownload;
		private System.Windows.Forms.Button _buttonClose;
		private System.Windows.Forms.LinkLabel _linkManualDownload;
		private System.Windows.Forms.Label _labelOverview;
		private System.Windows.Forms.Label _labelOr;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button _buttonInstall;
		private ProgressControl _progressControl;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Label _labelStatus;
		private L10NSharp.UI.L10NSharpExtender locExtender;
	}
}