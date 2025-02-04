namespace SayMore.UI
{
	partial class ShortFileNameWarningDlg
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
			if (disposing)
			{
				// Unsubscribe from the owner's events to avoid memory leaks
				if (Owner != null)
					Owner.FormClosing -= OnOwnerClosing;

				if (components != null)
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
			this._tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
			this._lblFailedActions = new System.Windows.Forms.Label();
			this._lblDoNotReportForVolumes = new System.Windows.Forms.Label();
			this._chkDoNotReportAnymoreThisSession = new System.Windows.Forms.CheckBox();
			this._chkDoNotReportEver = new System.Windows.Forms.CheckBox();
			this._linkLabelFsUtilMsg = new System.Windows.Forms.LinkLabel();
			this._lblDoNotReportForFiles = new System.Windows.Forms.Label();
			this._checkedListBoxFiles = new System.Windows.Forms.CheckedListBox();
			this._checkedListBoxVolumes = new System.Windows.Forms.CheckedListBox();
			this._flowLayoutFailedActions = new System.Windows.Forms.FlowLayoutPanel();
			this._checkDone = new System.Windows.Forms.CheckBox();
			this._btnClose = new System.Windows.Forms.Button();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._tableLayoutPanelMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayoutPanelMain
			// 
			this._tableLayoutPanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutPanelMain.ColumnCount = 1;
			this._tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanelMain.Controls.Add(this._lblFailedActions, 0, 1);
			this._tableLayoutPanelMain.Controls.Add(this._lblDoNotReportForVolumes, 0, 3);
			this._tableLayoutPanelMain.Controls.Add(this._chkDoNotReportAnymoreThisSession, 0, 7);
			this._tableLayoutPanelMain.Controls.Add(this._chkDoNotReportEver, 0, 8);
			this._tableLayoutPanelMain.Controls.Add(this._linkLabelFsUtilMsg, 0, 0);
			this._tableLayoutPanelMain.Controls.Add(this._lblDoNotReportForFiles, 0, 5);
			this._tableLayoutPanelMain.Controls.Add(this._checkedListBoxFiles, 0, 6);
			this._tableLayoutPanelMain.Controls.Add(this._checkedListBoxVolumes, 0, 4);
			this._tableLayoutPanelMain.Controls.Add(this._flowLayoutFailedActions, 0, 2);
			this._tableLayoutPanelMain.Controls.Add(this._checkDone, 0, 9);
			this._tableLayoutPanelMain.Location = new System.Drawing.Point(12, 12);
			this._tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanelMain.Name = "_tableLayoutPanelMain";
			this._tableLayoutPanelMain.RowCount = 10;
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.Size = new System.Drawing.Size(484, 412);
			this._tableLayoutPanelMain.TabIndex = 0;
			// 
			// _lblFailedActions
			// 
			this._lblFailedActions.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._lblFailedActions, null);
			this.locExtender.SetLocalizationComment(this._lblFailedActions, null);
			this.locExtender.SetLocalizingId(this._lblFailedActions, "ShortFileNameWarningDlg._lblFailedActions");
			this._lblFailedActions.Location = new System.Drawing.Point(3, 30);
			this._lblFailedActions.Name = "_lblFailedActions";
			this._lblFailedActions.Size = new System.Drawing.Size(218, 13);
			this._lblFailedActions.TabIndex = 3;
			this._lblFailedActions.Text = "This will help to avoid the following problems:";
			// 
			// _lblDoNotReportForVolumes
			// 
			this._lblDoNotReportForVolumes.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._lblDoNotReportForVolumes, null);
			this.locExtender.SetLocalizationComment(this._lblDoNotReportForVolumes, null);
			this.locExtender.SetLocalizingId(this._lblDoNotReportForVolumes, "ShortFileNameWarningDlg._lblDoNotReportForVolumes");
			this._lblDoNotReportForVolumes.Location = new System.Drawing.Point(3, 57);
			this._lblDoNotReportForVolumes.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
			this._lblDoNotReportForVolumes.Name = "_lblDoNotReportForVolumes";
			this._lblDoNotReportForVolumes.Size = new System.Drawing.Size(282, 13);
			this._lblDoNotReportForVolumes.TabIndex = 4;
			this._lblDoNotReportForVolumes.Text = "Do not report these problems for files on selected volumes:";
			// 
			// _chkDoNotReportAnymoreThisSession
			// 
			this._chkDoNotReportAnymoreThisSession.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._chkDoNotReportAnymoreThisSession, null);
			this.locExtender.SetLocalizationComment(this._chkDoNotReportAnymoreThisSession, null);
			this.locExtender.SetLocalizingId(this._chkDoNotReportAnymoreThisSession, "ShortFileNameWarningDlg._chkDoNotReportAnymoreThisSession");
			this._chkDoNotReportAnymoreThisSession.Location = new System.Drawing.Point(3, 346);
			this._chkDoNotReportAnymoreThisSession.Name = "_chkDoNotReportAnymoreThisSession";
			this._chkDoNotReportAnymoreThisSession.Size = new System.Drawing.Size(268, 17);
			this._chkDoNotReportAnymoreThisSession.TabIndex = 8;
			this._chkDoNotReportAnymoreThisSession.Text = "Do not report these problems again until I restart {0}";
			this._chkDoNotReportAnymoreThisSession.UseVisualStyleBackColor = true;
			this._chkDoNotReportAnymoreThisSession.CheckedChanged += new System.EventHandler(this._chkDoNotReportAnymoreThisSession_CheckedChanged);
			// 
			// _chkDoNotReportEver
			// 
			this._chkDoNotReportEver.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._chkDoNotReportEver, null);
			this.locExtender.SetLocalizationComment(this._chkDoNotReportEver, null);
			this.locExtender.SetLocalizingId(this._chkDoNotReportEver, "ShortFileNameWarningDlg._chkDoNotReportEver");
			this._chkDoNotReportEver.Location = new System.Drawing.Point(3, 369);
			this._chkDoNotReportEver.Name = "_chkDoNotReportEver";
			this._chkDoNotReportEver.Size = new System.Drawing.Size(215, 17);
			this._chkDoNotReportEver.TabIndex = 9;
			this._chkDoNotReportEver.Text = "Do not report these problems ever again";
			this._chkDoNotReportEver.UseVisualStyleBackColor = true;
			this._chkDoNotReportEver.CheckedChanged += new System.EventHandler(this._chkDoNotReportEver_CheckedChanged);
			// 
			// _linkLabelFsUtilMsg
			// 
			this._linkLabelFsUtilMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._linkLabelFsUtilMsg.AutoSize = true;
			this._linkLabelFsUtilMsg.LinkArea = new System.Windows.Forms.LinkArea(56, 3);
			this.locExtender.SetLocalizableToolTip(this._linkLabelFsUtilMsg, null);
			this.locExtender.SetLocalizationComment(this._linkLabelFsUtilMsg, "Param 0: \"fsutil 8dot3name\" (a Microsoft Windows utility - this will link to a " +
        "website); Param 1: A system volume (e.g. \"D:\"");
			this.locExtender.SetLocalizingId(this._linkLabelFsUtilMsg, "ShortFileNameWarningDlg._linkLabelFsUtilMsg");
			this._linkLabelFsUtilMsg.Location = new System.Drawing.Point(3, 0);
			this._linkLabelFsUtilMsg.Name = "_linkLabelFsUtilMsg";
			this._linkLabelFsUtilMsg.Size = new System.Drawing.Size(478, 30);
			this._linkLabelFsUtilMsg.TabIndex = 2;
			this._linkLabelFsUtilMsg.TabStop = true;
			this._linkLabelFsUtilMsg.Text = "If possible, you (or a system administrator) should use {0} to enable creation of" +
    " short \"8.3\" file names for file system volumes where media files are located.";
			this._linkLabelFsUtilMsg.UseCompatibleTextRendering = true;
			this._linkLabelFsUtilMsg.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkLabelFsUtilMsg_LinkClicked);
			// 
			// _lblDoNotReportForFiles
			// 
			this._lblDoNotReportForFiles.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._lblDoNotReportForFiles, null);
			this.locExtender.SetLocalizationComment(this._lblDoNotReportForFiles, null);
			this.locExtender.SetLocalizingId(this._lblDoNotReportForFiles, "ShortFileNameWarningDlg._lblDoNotReportForExtensions");
			this._lblDoNotReportForFiles.Location = new System.Drawing.Point(3, 100);
			this._lblDoNotReportForFiles.Name = "_lblDoNotReportForFiles";
			this._lblDoNotReportForFiles.Size = new System.Drawing.Size(225, 13);
			this._lblDoNotReportForFiles.TabIndex = 5;
			this._lblDoNotReportForFiles.Text = "Do not report these problems for selected files:";
			// 
			// _checkedListBoxFiles
			// 
			this._checkedListBoxFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._checkedListBoxFiles.CheckOnClick = true;
			this._checkedListBoxFiles.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._checkedListBoxFiles, null);
			this.locExtender.SetLocalizationComment(this._checkedListBoxFiles, null);
			this.locExtender.SetLocalizationPriority(this._checkedListBoxFiles, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._checkedListBoxFiles, "ShortFileNameWarningDlg._checkedListBoxFiles");
			this._checkedListBoxFiles.Location = new System.Drawing.Point(10, 116);
			this._checkedListBoxFiles.Margin = new System.Windows.Forms.Padding(10, 3, 0, 8);
			this._checkedListBoxFiles.Name = "_checkedListBoxFiles";
			this._checkedListBoxFiles.Size = new System.Drawing.Size(474, 214);
			this._checkedListBoxFiles.TabIndex = 11;
			this._checkedListBoxFiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.HandleCheckedListBoxItemCheck);
			// 
			// _checkedListBoxVolumes
			// 
			this._checkedListBoxVolumes.CheckOnClick = true;
			this._checkedListBoxVolumes.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._checkedListBoxVolumes, null);
			this.locExtender.SetLocalizationComment(this._checkedListBoxVolumes, null);
			this.locExtender.SetLocalizationPriority(this._checkedListBoxVolumes, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._checkedListBoxVolumes, "ShortFileNameWarningDlg._checkedListBoxVolumes");
			this._checkedListBoxVolumes.Location = new System.Drawing.Point(10, 73);
			this._checkedListBoxVolumes.Margin = new System.Windows.Forms.Padding(10, 3, 3, 8);
			this._checkedListBoxVolumes.Name = "_checkedListBoxVolumes";
			this._checkedListBoxVolumes.Size = new System.Drawing.Size(150, 19);
			this._checkedListBoxVolumes.TabIndex = 10;
			this._checkedListBoxVolumes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.HandleCheckedListBoxItemCheck);
			// 
			// _flowLayoutFailedActions
			// 
			this._flowLayoutFailedActions.AutoSize = true;
			this._flowLayoutFailedActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._flowLayoutFailedActions.Location = new System.Drawing.Point(10, 46);
			this._flowLayoutFailedActions.Margin = new System.Windows.Forms.Padding(10, 3, 0, 3);
			this._flowLayoutFailedActions.Name = "_flowLayoutFailedActions";
			this._flowLayoutFailedActions.Size = new System.Drawing.Size(0, 0);
			this._flowLayoutFailedActions.TabIndex = 12;
			// 
			// _checkDone
			// 
			this._checkDone.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._checkDone, null);
			this.locExtender.SetLocalizationComment(this._checkDone, null);
			this.locExtender.SetLocalizationPriority(this._checkDone, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._checkDone, "ShortFileNameWarningDlg._checkDone");
			this._checkDone.Location = new System.Drawing.Point(3, 392);
			this._checkDone.Name = "_checkDone";
			this._checkDone.Size = new System.Drawing.Size(284, 17);
			this._checkDone.TabIndex = 13;
			this._checkDone.Text = "Solved! I think I\'ve enabled creation of short filenames.";
			this._checkDone.UseVisualStyleBackColor = true;
			this._checkDone.CheckedChanged += new System.EventHandler(this._checkDone_CheckedChanged);
			// 
			// _btnClose
			// 
			this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._btnClose, null);
			this.locExtender.SetLocalizationComment(this._btnClose, null);
			this.locExtender.SetLocalizationPriority(this._btnClose, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._btnClose, "ShortFileNameWarningDlg._btnClose");
			this._btnClose.Location = new System.Drawing.Point(421, 435);
			this._btnClose.Margin = new System.Windows.Forms.Padding(3, 8, 0, 0);
			this._btnClose.Name = "_btnClose";
			this._btnClose.Size = new System.Drawing.Size(75, 23);
			this._btnClose.TabIndex = 0;
			this._btnClose.Text = "Close";
			this._btnClose.UseVisualStyleBackColor = true;
			this._btnClose.Click += new System.EventHandler(this.HandleCloseClick);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = null;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn1.HeaderText = "";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.HeaderText = "";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// ShortFileNameWarningDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.ClientSize = new System.Drawing.Size(508, 467);
			this.ControlBox = false;
			this.Controls.Add(this._tableLayoutPanelMain);
			this.Controls.Add(this._btnClose);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ShortFileNameWarningDlg.WindowTitle");
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(498, 309);
			this.Name = "ShortFileNameWarningDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Unable to Obtain Short Filename";
			this._tableLayoutPanelMain.ResumeLayout(false);
			this._tableLayoutPanelMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanelMain;
		private System.Windows.Forms.Button _btnClose;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.LinkLabel _linkLabelFsUtilMsg;
		private System.Windows.Forms.Label _lblFailedActions;
		private System.Windows.Forms.Label _lblDoNotReportForVolumes;
		private System.Windows.Forms.Label _lblDoNotReportForFiles;
		private System.Windows.Forms.CheckBox _chkDoNotReportAnymoreThisSession;
		private System.Windows.Forms.CheckBox _chkDoNotReportEver;
		private System.Windows.Forms.CheckedListBox _checkedListBoxVolumes;
		private System.Windows.Forms.CheckedListBox _checkedListBoxFiles;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.FlowLayoutPanel _flowLayoutFailedActions;
		private System.Windows.Forms.CheckBox _checkDone;
	}
}