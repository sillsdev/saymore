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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this._tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
			this._btnOK = new System.Windows.Forms.Button();
			this._lblFailedAction = new System.Windows.Forms.Label();
			this._lblDoNotReportForVolumes = new System.Windows.Forms.Label();
			this._lblDoNotReportForExtensions = new System.Windows.Forms.Label();
			this._chkDoNotReportForFolders = new System.Windows.Forms.CheckBox();
			this._chkDoNotReportForFilesContaining = new System.Windows.Forms.CheckBox();
			this._chkDoNotReportAnymoreThisSession = new System.Windows.Forms.CheckBox();
			this._chkDoNotReportEver = new System.Windows.Forms.CheckBox();
			this._checkedListBoxVolumes = new System.Windows.Forms.CheckedListBox();
			this._checkedListBoxExtensions = new System.Windows.Forms.CheckedListBox();
			this._gridFolders = new SIL.Windows.Forms.Widgets.BetterGrid.BetterGrid();
			this.colFolders = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._gridFilenameContains = new SIL.Windows.Forms.Widgets.BetterGrid.BetterGrid();
			this.colContains = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._linkLabelFsUtilMsg = new System.Windows.Forms.LinkLabel();
			this._lblMsg = new System.Windows.Forms.Label();
			this._lblFilePath = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._tableLayoutPanelMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._gridFolders)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._gridFilenameContains)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayoutPanelMain
			// 
			this._tableLayoutPanelMain.ColumnCount = 2;
			this._tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanelMain.Controls.Add(this._btnOK, 1, 12);
			this._tableLayoutPanelMain.Controls.Add(this._lblFailedAction, 0, 4);
			this._tableLayoutPanelMain.Controls.Add(this._lblDoNotReportForVolumes, 0, 6);
			this._tableLayoutPanelMain.Controls.Add(this._lblDoNotReportForExtensions, 0, 7);
			this._tableLayoutPanelMain.Controls.Add(this._chkDoNotReportForFolders, 0, 8);
			this._tableLayoutPanelMain.Controls.Add(this._chkDoNotReportForFilesContaining, 0, 9);
			this._tableLayoutPanelMain.Controls.Add(this._chkDoNotReportAnymoreThisSession, 0, 10);
			this._tableLayoutPanelMain.Controls.Add(this._chkDoNotReportEver, 0, 11);
			this._tableLayoutPanelMain.Controls.Add(this._checkedListBoxVolumes, 1, 6);
			this._tableLayoutPanelMain.Controls.Add(this._checkedListBoxExtensions, 1, 7);
			this._tableLayoutPanelMain.Controls.Add(this._gridFolders, 1, 8);
			this._tableLayoutPanelMain.Controls.Add(this._gridFilenameContains, 1, 9);
			this._tableLayoutPanelMain.Controls.Add(this._linkLabelFsUtilMsg, 0, 2);
			this._tableLayoutPanelMain.Controls.Add(this._lblMsg, 0, 0);
			this._tableLayoutPanelMain.Controls.Add(this._lblFilePath, 0, 1);
			this._tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanelMain.Location = new System.Drawing.Point(15, 20);
			this._tableLayoutPanelMain.Name = "_tableLayoutPanelMain";
			this._tableLayoutPanelMain.RowCount = 13;
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelMain.Size = new System.Drawing.Size(629, 565);
			this._tableLayoutPanelMain.TabIndex = 0;
			// 
			// _btnOK
			// 
			this._btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._btnOK, null);
			this.locExtender.SetLocalizationComment(this._btnOK, null);
			this.locExtender.SetLocalizingId(this._btnOK, "ShortFileNameWarningDlg._btnOK");
			this._btnOK.Location = new System.Drawing.Point(554, 542);
			this._btnOK.Margin = new System.Windows.Forms.Padding(3, 8, 0, 0);
			this._btnOK.Name = "_btnOK";
			this._btnOK.Size = new System.Drawing.Size(75, 23);
			this._btnOK.TabIndex = 0;
			this._btnOK.Text = "OK";
			this._btnOK.UseVisualStyleBackColor = true;
			this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
			// 
			// _lblFailedAction
			// 
			this._lblFailedAction.AutoSize = true;
			this._tableLayoutPanelMain.SetColumnSpan(this._lblFailedAction, 2);
			this.locExtender.SetLocalizableToolTip(this._lblFailedAction, null);
			this.locExtender.SetLocalizationComment(this._lblFailedAction, null);
			this.locExtender.SetLocalizationPriority(this._lblFailedAction, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._lblFailedAction, "ShortFileNameWarningDlg._lblFailedAction");
			this._lblFailedAction.Location = new System.Drawing.Point(3, 75);
			this._lblFailedAction.Name = "_lblFailedAction";
			this._lblFailedAction.Size = new System.Drawing.Size(14, 13);
			this._lblFailedAction.TabIndex = 3;
			this._lblFailedAction.Text = "#";
			// 
			// _lblDoNotReportForVolumes
			// 
			this._lblDoNotReportForVolumes.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._lblDoNotReportForVolumes, null);
			this.locExtender.SetLocalizationComment(this._lblDoNotReportForVolumes, null);
			this.locExtender.SetLocalizingId(this._lblDoNotReportForVolumes, "ShortFileNameWarningDlg._lblDoNotReportForVolumes");
			this._lblDoNotReportForVolumes.Location = new System.Drawing.Point(3, 96);
			this._lblDoNotReportForVolumes.Name = "_lblDoNotReportForVolumes";
			this._lblDoNotReportForVolumes.Size = new System.Drawing.Size(253, 13);
			this._lblDoNotReportForVolumes.TabIndex = 4;
			this._lblDoNotReportForVolumes.Text = "Do not report this problem for files on these volumes:";
			// 
			// _lblDoNotReportForExtensions
			// 
			this._lblDoNotReportForExtensions.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._lblDoNotReportForExtensions, null);
			this.locExtender.SetLocalizationComment(this._lblDoNotReportForExtensions, null);
			this.locExtender.SetLocalizingId(this._lblDoNotReportForExtensions, "ShortFileNameWarningDlg._lblDoNotReportForExtensions");
			this._lblDoNotReportForExtensions.Location = new System.Drawing.Point(3, 135);
			this._lblDoNotReportForExtensions.Name = "_lblDoNotReportForExtensions";
			this._lblDoNotReportForExtensions.Size = new System.Drawing.Size(271, 13);
			this._lblDoNotReportForExtensions.TabIndex = 5;
			this._lblDoNotReportForExtensions.Text = "Do not report this problem for files with these extensions:";
			// 
			// _chkDoNotReportForFolders
			// 
			this._chkDoNotReportForFolders.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._chkDoNotReportForFolders, null);
			this.locExtender.SetLocalizationComment(this._chkDoNotReportForFolders, null);
			this.locExtender.SetLocalizingId(this._chkDoNotReportForFolders, "ShortFileNameWarningDlg._chkDoNotReportForFolders");
			this._chkDoNotReportForFolders.Location = new System.Drawing.Point(3, 216);
			this._chkDoNotReportForFolders.Name = "_chkDoNotReportForFolders";
			this._chkDoNotReportForFolders.Size = new System.Drawing.Size(260, 17);
			this._chkDoNotReportForFolders.TabIndex = 6;
			this._chkDoNotReportForFolders.Text = "Do not report this problem for files in these folders:";
			this._chkDoNotReportForFolders.UseVisualStyleBackColor = true;
			this._chkDoNotReportForFolders.CheckedChanged += new System.EventHandler(this._chkDoNotReportForFolders_CheckedChanged);
			// 
			// _chkDoNotReportForFilesContaining
			// 
			this._chkDoNotReportForFilesContaining.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._chkDoNotReportForFilesContaining, null);
			this.locExtender.SetLocalizationComment(this._chkDoNotReportForFilesContaining, null);
			this.locExtender.SetLocalizingId(this._chkDoNotReportForFilesContaining, "ShortFileNameWarningDlg._chkDoNotReportForFilesContaining");
			this._chkDoNotReportForFilesContaining.Location = new System.Drawing.Point(3, 353);
			this._chkDoNotReportForFilesContaining.Name = "_chkDoNotReportForFilesContaining";
			this._chkDoNotReportForFilesContaining.Size = new System.Drawing.Size(292, 17);
			this._chkDoNotReportForFilesContaining.TabIndex = 7;
			this._chkDoNotReportForFilesContaining.Text = "Do not report this problem for files whose names contain:";
			this._chkDoNotReportForFilesContaining.UseVisualStyleBackColor = true;
			this._chkDoNotReportForFilesContaining.CheckedChanged += new System.EventHandler(this._chkDoNotReportForFilesContaining_CheckedChanged);
			// 
			// _chkDoNotReportAnymoreThisSession
			// 
			this._chkDoNotReportAnymoreThisSession.AutoSize = true;
			this._tableLayoutPanelMain.SetColumnSpan(this._chkDoNotReportAnymoreThisSession, 2);
			this.locExtender.SetLocalizableToolTip(this._chkDoNotReportAnymoreThisSession, null);
			this.locExtender.SetLocalizationComment(this._chkDoNotReportAnymoreThisSession, null);
			this.locExtender.SetLocalizingId(this._chkDoNotReportAnymoreThisSession, "ShortFileNameWarningDlg._chkDoNotReportAnymoreThisSession");
			this._chkDoNotReportAnymoreThisSession.Location = new System.Drawing.Point(3, 490);
			this._chkDoNotReportAnymoreThisSession.Name = "_chkDoNotReportAnymoreThisSession";
			this._chkDoNotReportAnymoreThisSession.Size = new System.Drawing.Size(253, 17);
			this._chkDoNotReportAnymoreThisSession.TabIndex = 8;
			this._chkDoNotReportAnymoreThisSession.Text = "Do not report this problem again until I restart {0}";
			this._chkDoNotReportAnymoreThisSession.UseVisualStyleBackColor = true;
			// 
			// _chkDoNotReportEver
			// 
			this._chkDoNotReportEver.AutoSize = true;
			this._tableLayoutPanelMain.SetColumnSpan(this._chkDoNotReportEver, 2);
			this.locExtender.SetLocalizableToolTip(this._chkDoNotReportEver, null);
			this.locExtender.SetLocalizationComment(this._chkDoNotReportEver, null);
			this.locExtender.SetLocalizingId(this._chkDoNotReportEver, "ShortFileNameWarningDlg._chkDoNotReportEver");
			this._chkDoNotReportEver.Location = new System.Drawing.Point(3, 513);
			this._chkDoNotReportEver.Name = "_chkDoNotReportEver";
			this._chkDoNotReportEver.Size = new System.Drawing.Size(200, 17);
			this._chkDoNotReportEver.TabIndex = 9;
			this._chkDoNotReportEver.Text = "Do not report this problem again ever";
			this._chkDoNotReportEver.UseVisualStyleBackColor = true;
			this._chkDoNotReportEver.CheckedChanged += new System.EventHandler(this._chkDoNotReportEver_CheckedChanged);
			// 
			// _checkedListBoxVolumes
			// 
			this._checkedListBoxVolumes.Dock = System.Windows.Forms.DockStyle.Fill;
			this._checkedListBoxVolumes.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._checkedListBoxVolumes, null);
			this.locExtender.SetLocalizationComment(this._checkedListBoxVolumes, null);
			this.locExtender.SetLocalizationPriority(this._checkedListBoxVolumes, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._checkedListBoxVolumes, "ShortFileNameWarningDlg._checkedListBoxVolumes");
			this._checkedListBoxVolumes.Location = new System.Drawing.Point(301, 99);
			this._checkedListBoxVolumes.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
			this._checkedListBoxVolumes.Name = "_checkedListBoxVolumes";
			this._checkedListBoxVolumes.Size = new System.Drawing.Size(325, 28);
			this._checkedListBoxVolumes.TabIndex = 10;
			// 
			// _checkedListBoxExtensions
			// 
			this._checkedListBoxExtensions.Dock = System.Windows.Forms.DockStyle.Fill;
			this._checkedListBoxExtensions.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._checkedListBoxExtensions, null);
			this.locExtender.SetLocalizationComment(this._checkedListBoxExtensions, null);
			this.locExtender.SetLocalizationPriority(this._checkedListBoxExtensions, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._checkedListBoxExtensions, "ShortFileNameWarningDlg._checkedListBoxExtensions");
			this._checkedListBoxExtensions.Location = new System.Drawing.Point(301, 138);
			this._checkedListBoxExtensions.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
			this._checkedListBoxExtensions.Name = "_checkedListBoxExtensions";
			this._checkedListBoxExtensions.Size = new System.Drawing.Size(325, 67);
			this._checkedListBoxExtensions.TabIndex = 11;
			// 
			// _gridFolders
			// 
			this._gridFolders.AllowUserToAddRows = false;
			this._gridFolders.AllowUserToDeleteRows = false;
			this._gridFolders.AllowUserToOrderColumns = true;
			this._gridFolders.AllowUserToResizeColumns = false;
			this._gridFolders.AllowUserToResizeRows = false;
			this._gridFolders.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this._gridFolders.BackgroundColor = System.Drawing.SystemColors.Window;
			this._gridFolders.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._gridFolders.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._gridFolders.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this._gridFolders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this._gridFolders.ColumnHeadersVisible = false;
			this._gridFolders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFolders});
			this._gridFolders.Dock = System.Windows.Forms.DockStyle.Fill;
			this._gridFolders.DrawTextBoxEditControlBorder = false;
			this._gridFolders.Enabled = false;
			this._gridFolders.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._gridFolders.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this._gridFolders.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this.locExtender.SetLocalizableToolTip(this._gridFolders, null);
			this.locExtender.SetLocalizationComment(this._gridFolders, null);
			this.locExtender.SetLocalizationPriority(this._gridFolders, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._gridFolders, "ShortFileNameWarningDlg._gridFolders");
			this._gridFolders.Location = new System.Drawing.Point(301, 216);
			this._gridFolders.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
			this._gridFolders.MultiSelect = false;
			this._gridFolders.Name = "_gridFolders";
			this._gridFolders.PaintHeaderAcrossFullGridWidth = true;
			this._gridFolders.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._gridFolders.RowHeadersWidth = 22;
			this._gridFolders.SelectedCellBackColor = System.Drawing.Color.Empty;
			this._gridFolders.SelectedCellForeColor = System.Drawing.Color.Empty;
			this._gridFolders.SelectedRowBackColor = System.Drawing.Color.Empty;
			this._gridFolders.SelectedRowForeColor = System.Drawing.Color.Empty;
			this._gridFolders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._gridFolders.ShowWaterMarkWhenDirty = false;
			this._gridFolders.Size = new System.Drawing.Size(325, 126);
			this._gridFolders.TabIndex = 12;
			this._gridFolders.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this._gridFolders.WaterMark = "!";
			this._gridFolders.CurrentCellDirtyStateChanged += new System.EventHandler(this.CurrentCellDirtyStateChanged);
			this._gridFolders.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.RowEnter);
			this._gridFolders.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.RowValidating);
			this._gridFolders.Leave += new System.EventHandler(this.GridLeave);
			// 
			// colFolders
			// 
			this.colFolders.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colFolders.HeaderText = "";
			this.colFolders.Name = "colFolders";
			// 
			// _gridFilenameContains
			// 
			this._gridFilenameContains.AllowUserToAddRows = false;
			this._gridFilenameContains.AllowUserToDeleteRows = false;
			this._gridFilenameContains.AllowUserToOrderColumns = true;
			this._gridFilenameContains.AllowUserToResizeColumns = false;
			this._gridFilenameContains.AllowUserToResizeRows = false;
			this._gridFilenameContains.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this._gridFilenameContains.BackgroundColor = System.Drawing.SystemColors.Window;
			this._gridFilenameContains.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._gridFilenameContains.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._gridFilenameContains.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this._gridFilenameContains.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this._gridFilenameContains.ColumnHeadersVisible = false;
			this._gridFilenameContains.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colContains});
			this._gridFilenameContains.Dock = System.Windows.Forms.DockStyle.Fill;
			this._gridFilenameContains.DrawTextBoxEditControlBorder = false;
			this._gridFilenameContains.Enabled = false;
			this._gridFilenameContains.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._gridFilenameContains.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this._gridFilenameContains.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this.locExtender.SetLocalizableToolTip(this._gridFilenameContains, null);
			this.locExtender.SetLocalizationComment(this._gridFilenameContains, null);
			this.locExtender.SetLocalizationPriority(this._gridFilenameContains, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._gridFilenameContains, "ShortFileNameWarningDlg._gridFilenameContains");
			this._gridFilenameContains.Location = new System.Drawing.Point(301, 353);
			this._gridFilenameContains.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
			this._gridFilenameContains.MultiSelect = false;
			this._gridFilenameContains.Name = "_gridFilenameContains";
			this._gridFilenameContains.PaintHeaderAcrossFullGridWidth = true;
			this._gridFilenameContains.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._gridFilenameContains.RowHeadersWidth = 22;
			this._gridFilenameContains.SelectedCellBackColor = System.Drawing.Color.Empty;
			this._gridFilenameContains.SelectedCellForeColor = System.Drawing.Color.Empty;
			this._gridFilenameContains.SelectedRowBackColor = System.Drawing.Color.Empty;
			this._gridFilenameContains.SelectedRowForeColor = System.Drawing.Color.Empty;
			this._gridFilenameContains.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._gridFilenameContains.ShowWaterMarkWhenDirty = false;
			this._gridFilenameContains.Size = new System.Drawing.Size(325, 126);
			this._gridFilenameContains.TabIndex = 13;
			this._gridFilenameContains.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this._gridFilenameContains.WaterMark = "!";
			this._gridFilenameContains.CurrentCellDirtyStateChanged += new System.EventHandler(this.CurrentCellDirtyStateChanged);
			this._gridFilenameContains.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.RowEnter);
			this._gridFilenameContains.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.RowValidating);
			// 
			// colContains
			// 
			this.colContains.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colContains.HeaderText = "";
			this.colContains.Name = "colContains";
			// 
			// _linkLabelFsUtilMsg
			// 
			this._linkLabelFsUtilMsg.AutoSize = true;
			this._tableLayoutPanelMain.SetColumnSpan(this._linkLabelFsUtilMsg, 2);
			this._linkLabelFsUtilMsg.LinkArea = new System.Windows.Forms.LinkArea(56, 3);
			this.locExtender.SetLocalizableToolTip(this._linkLabelFsUtilMsg, null);
			this.locExtender.SetLocalizationComment(this._linkLabelFsUtilMsg, "Param 0: \\\"fsutil 8dot3name\\\" (a Microsoft Windows utility - this will link to a " +
        "website); Param 1: A system volume (e.g. \\\"D:\\\"");
			this.locExtender.SetLocalizingId(this._linkLabelFsUtilMsg, "ShortFileNameWarningDlg._linkLabelFsUtilMsg");
			this._linkLabelFsUtilMsg.Location = new System.Drawing.Point(3, 37);
			this._linkLabelFsUtilMsg.Name = "_linkLabelFsUtilMsg";
			this._linkLabelFsUtilMsg.Size = new System.Drawing.Size(601, 30);
			this._linkLabelFsUtilMsg.TabIndex = 2;
			this._linkLabelFsUtilMsg.TabStop = true;
			this._linkLabelFsUtilMsg.Text = "If possible, you (or a system administrator) should use {0} to enable creation of" +
    " short \"8.3\" file names for the file system volume ({1}) where this file is loca" +
    "ted.";
			this._linkLabelFsUtilMsg.UseCompatibleTextRendering = true;
			this._linkLabelFsUtilMsg.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkLabelFsUtilMsg_LinkClicked);
			// 
			// _lblMsg
			// 
			this._lblMsg.AutoSize = true;
			this._tableLayoutPanelMain.SetColumnSpan(this._lblMsg, 2);
			this.locExtender.SetLocalizableToolTip(this._lblMsg, null);
			this.locExtender.SetLocalizationComment(this._lblMsg, "Param 0: \\\"SayMore\\\" (product name)");
			this.locExtender.SetLocalizingId(this._lblMsg, "ShortFileNameWarningDlg._lblMsg");
			this._lblMsg.Location = new System.Drawing.Point(3, 0);
			this._lblMsg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this._lblMsg.Name = "_lblMsg";
			this._lblMsg.Size = new System.Drawing.Size(249, 13);
			this._lblMsg.TabIndex = 14;
			this._lblMsg.Text = "{0} was unable to obtain a \"short name\" for this file:";
			// 
			// _lblFilePath
			// 
			this._lblFilePath.AutoSize = true;
			this._tableLayoutPanelMain.SetColumnSpan(this._lblFilePath, 2);
			this.locExtender.SetLocalizableToolTip(this._lblFilePath, null);
			this.locExtender.SetLocalizationComment(this._lblFilePath, null);
			this.locExtender.SetLocalizationPriority(this._lblFilePath, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._lblFilePath, "ShortFileNameWarningDlg._lblFilePath");
			this._lblFilePath.Location = new System.Drawing.Point(3, 16);
			this._lblFilePath.Margin = new System.Windows.Forms.Padding(3, 0, 3, 8);
			this._lblFilePath.Name = "_lblFilePath";
			this._lblFilePath.Size = new System.Drawing.Size(14, 13);
			this._lblFilePath.TabIndex = 15;
			this._lblFilePath.Text = "#";
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
			this.AcceptButton = this._btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(659, 600);
			this.Controls.Add(this._tableLayoutPanelMain);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ShortFileNameWarningDlg.WindowTitle");
			this.MinimizeBox = false;
			this.Name = "ShortFileNameWarningDlg";
			this.Padding = new System.Windows.Forms.Padding(15, 20, 15, 15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Unable to Obtain Short Filename";
			this._tableLayoutPanelMain.ResumeLayout(false);
			this._tableLayoutPanelMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._gridFolders)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._gridFilenameContains)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanelMain;
		private System.Windows.Forms.Button _btnOK;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.LinkLabel _linkLabelFsUtilMsg;
		private System.Windows.Forms.Label _lblFailedAction;
		private System.Windows.Forms.Label _lblDoNotReportForVolumes;
		private System.Windows.Forms.Label _lblDoNotReportForExtensions;
		private System.Windows.Forms.CheckBox _chkDoNotReportForFolders;
		private System.Windows.Forms.CheckBox _chkDoNotReportForFilesContaining;
		private System.Windows.Forms.CheckBox _chkDoNotReportAnymoreThisSession;
		private System.Windows.Forms.CheckBox _chkDoNotReportEver;
		private System.Windows.Forms.CheckedListBox _checkedListBoxVolumes;
		private System.Windows.Forms.CheckedListBox _checkedListBoxExtensions;
		private SIL.Windows.Forms.Widgets.BetterGrid.BetterGrid _gridFolders;
		private SIL.Windows.Forms.Widgets.BetterGrid.BetterGrid _gridFilenameContains;
		private System.Windows.Forms.DataGridViewTextBoxColumn colFolders;
		private System.Windows.Forms.DataGridViewTextBoxColumn colContains;
		private System.Windows.Forms.Label _lblMsg;
		private System.Windows.Forms.Label _lblFilePath;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
	}
}