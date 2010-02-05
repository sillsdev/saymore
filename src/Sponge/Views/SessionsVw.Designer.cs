using SIL.Sponge.Controls;

namespace SIL.Sponge
{
	partial class SessionsVw
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tabSessions = new System.Windows.Forms.TabControl();
			this.tpgDescription = new System.Windows.Forms.TabPage();
			this.tpgContributors = new System.Windows.Forms.TabPage();
			this.tpgTaskStatus = new System.Windows.Forms.TabPage();
			this.tpgFiles = new System.Windows.Forms.TabPage();
			this.pnlGrid = new SilUtils.Controls.SilPanel();
			this.lnkSessionPath = new System.Windows.Forms.LinkLabel();
			this.lblEmptySessionMsg = new System.Windows.Forms.Label();
			this.gridFiles = new SilUtils.SilGrid();
			this.iconCol = new System.Windows.Forms.DataGridViewImageColumn();
			this.filesNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesTagsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesDateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesSizeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lpSessions = new SIL.Sponge.Controls.ListPanel();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.lblFile = new System.Windows.Forms.Label();
			this.lblNoSessionsMsg = new System.Windows.Forms.Label();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitRightSide.Panel1.SuspendLayout();
			this.splitRightSide.Panel2.SuspendLayout();
			this.splitRightSide.SuspendLayout();
			this.tabSessions.SuspendLayout();
			this.tpgFiles.SuspendLayout();
			this.pnlGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridFiles)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.lblNoSessionsMsg);
			this.splitOuter.Panel1.Controls.Add(this.lpSessions);
			this.splitOuter.Panel1MinSize = 165;
			this.splitOuter.SplitterDistance = 165;
			// 
			// splitRightSide
			// 
			// 
			// splitRightSide.Panel1
			// 
			this.splitRightSide.Panel1.Controls.Add(this.tabSessions);
			this.splitRightSide.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 2, 0);
			// 
			// splitRightSide.Panel2
			// 
			this.splitRightSide.Panel2.Controls.Add(this.lblFile);
			this.splitRightSide.Panel2.Controls.Add(this.picIcon);
			this.splitRightSide.Size = new System.Drawing.Size(488, 383);
			this.splitRightSide.SplitterDistance = 288;
			// 
			// tabSessions
			// 
			this.tabSessions.Controls.Add(this.tpgDescription);
			this.tabSessions.Controls.Add(this.tpgContributors);
			this.tabSessions.Controls.Add(this.tpgTaskStatus);
			this.tabSessions.Controls.Add(this.tpgFiles);
			this.tabSessions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabSessions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabSessions.ItemSize = new System.Drawing.Size(65, 22);
			this.tabSessions.Location = new System.Drawing.Point(0, 3);
			this.tabSessions.Name = "tabSessions";
			this.tabSessions.SelectedIndex = 0;
			this.tabSessions.Size = new System.Drawing.Size(486, 285);
			this.tabSessions.TabIndex = 0;
			this.tabSessions.SizeChanged += new System.EventHandler(this.tabSessions_SizeChanged);
			// 
			// tpgDescription
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgDescription, null);
			this.locExtender.SetLocalizationComment(this.tpgDescription, null);
			this.locExtender.SetLocalizingId(this.tpgDescription, "SessionsVw.tpgDescription");
			this.tpgDescription.Location = new System.Drawing.Point(4, 26);
			this.tpgDescription.Name = "tpgDescription";
			this.tpgDescription.Padding = new System.Windows.Forms.Padding(3);
			this.tpgDescription.Size = new System.Drawing.Size(478, 255);
			this.tpgDescription.TabIndex = 0;
			this.tpgDescription.Text = "Description";
			this.tpgDescription.ToolTipText = "Description";
			this.tpgDescription.UseVisualStyleBackColor = true;
			// 
			// tpgContributors
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgContributors, null);
			this.locExtender.SetLocalizationComment(this.tpgContributors, null);
			this.locExtender.SetLocalizingId(this.tpgContributors, "SessionsVw.tpgContributors");
			this.tpgContributors.Location = new System.Drawing.Point(4, 26);
			this.tpgContributors.Name = "tpgContributors";
			this.tpgContributors.Padding = new System.Windows.Forms.Padding(3);
			this.tpgContributors.Size = new System.Drawing.Size(478, 255);
			this.tpgContributors.TabIndex = 1;
			this.tpgContributors.Text = "Contributors && Permissions";
			this.tpgContributors.ToolTipText = "Contributors & Permissions";
			this.tpgContributors.UseVisualStyleBackColor = true;
			// 
			// tpgTaskStatus
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgTaskStatus, null);
			this.locExtender.SetLocalizationComment(this.tpgTaskStatus, null);
			this.locExtender.SetLocalizingId(this.tpgTaskStatus, "SessionsVw.tpgTaskStatus");
			this.tpgTaskStatus.Location = new System.Drawing.Point(4, 26);
			this.tpgTaskStatus.Name = "tpgTaskStatus";
			this.tpgTaskStatus.Size = new System.Drawing.Size(478, 255);
			this.tpgTaskStatus.TabIndex = 2;
			this.tpgTaskStatus.Text = "Task Status";
			this.tpgTaskStatus.ToolTipText = "Task Status";
			this.tpgTaskStatus.UseVisualStyleBackColor = true;
			// 
			// tpgFiles
			// 
			this.tpgFiles.Controls.Add(this.pnlGrid);
			this.locExtender.SetLocalizableToolTip(this.tpgFiles, null);
			this.locExtender.SetLocalizationComment(this.tpgFiles, null);
			this.locExtender.SetLocalizingId(this.tpgFiles, "SessionsVw.tpgFiles");
			this.tpgFiles.Location = new System.Drawing.Point(4, 26);
			this.tpgFiles.Name = "tpgFiles";
			this.tpgFiles.Size = new System.Drawing.Size(478, 255);
			this.tpgFiles.TabIndex = 3;
			this.tpgFiles.Text = "Files";
			this.tpgFiles.ToolTipText = "Files";
			this.tpgFiles.UseVisualStyleBackColor = true;
			// 
			// pnlGrid
			// 
			this.pnlGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(166)))), ((int)(((byte)(170)))));
			this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrid.ClipTextForChildControls = true;
			this.pnlGrid.ControlReceivingFocusOnMnemonic = null;
			this.pnlGrid.Controls.Add(this.lnkSessionPath);
			this.pnlGrid.Controls.Add(this.lblEmptySessionMsg);
			this.pnlGrid.Controls.Add(this.gridFiles);
			this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlGrid.DoubleBuffered = true;
			this.pnlGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.locExtender.SetLocalizableToolTip(this.pnlGrid, null);
			this.locExtender.SetLocalizationComment(this.pnlGrid, null);
			this.locExtender.SetLocalizingId(this.pnlGrid, "SessionsVw.pnlGrid");
			this.pnlGrid.Location = new System.Drawing.Point(0, 0);
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			this.pnlGrid.Size = new System.Drawing.Size(478, 255);
			this.pnlGrid.TabIndex = 1;
			this.pnlGrid.SizeChanged += new System.EventHandler(this.pnlGrid_SizeChanged);
			// 
			// lnkSessionPath
			// 
			this.lnkSessionPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lnkSessionPath.AutoEllipsis = true;
			this.locExtender.SetLocalizableToolTip(this.lnkSessionPath, null);
			this.locExtender.SetLocalizationComment(this.lnkSessionPath, null);
			this.locExtender.SetLocalizingId(this.lnkSessionPath, "SessionsVw.lnkSessionPath");
			this.lnkSessionPath.Location = new System.Drawing.Point(21, 58);
			this.lnkSessionPath.Name = "lnkSessionPath";
			this.lnkSessionPath.Size = new System.Drawing.Size(439, 13);
			this.lnkSessionPath.TabIndex = 2;
			this.lnkSessionPath.TabStop = true;
			this.lnkSessionPath.Text = "#";
			this.lnkSessionPath.Visible = false;
			this.lnkSessionPath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSessionPath_LinkClicked);
			// 
			// lblEmptySessionMsg
			// 
			this.lblEmptySessionMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblEmptySessionMsg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this.lblEmptySessionMsg, null);
			this.locExtender.SetLocalizationComment(this.lblEmptySessionMsg, null);
			this.locExtender.SetLocalizingId(this.lblEmptySessionMsg, "SessionsVw.lblEmptySessionMsg");
			this.lblEmptySessionMsg.Location = new System.Drawing.Point(19, 15);
			this.lblEmptySessionMsg.Name = "lblEmptySessionMsg";
			this.lblEmptySessionMsg.Size = new System.Drawing.Size(439, 40);
			this.lblEmptySessionMsg.TabIndex = 1;
			this.lblEmptySessionMsg.Text = "This session does not yet have any files. To add files, you may drag them here or" +
				" directly into the session folder at:";
			this.lblEmptySessionMsg.Visible = false;
			// 
			// gridFiles
			// 
			this.gridFiles.AllowUserToAddRows = false;
			this.gridFiles.AllowUserToDeleteRows = false;
			this.gridFiles.AllowUserToOrderColumns = true;
			this.gridFiles.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			this.gridFiles.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.gridFiles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.gridFiles.BackgroundColor = System.Drawing.SystemColors.Window;
			this.gridFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.gridFiles.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridFiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.gridFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iconCol,
            this.filesNameCol,
            this.filesTypeCol,
            this.filesTagsCol,
            this.filesDateCol,
            this.filesSizeCol});
			this.gridFiles.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(219)))), ((int)(((byte)(180)))));
			this.gridFiles.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.gridFiles, null);
			this.locExtender.SetLocalizationComment(this.gridFiles, null);
			this.locExtender.SetLocalizingId(this.gridFiles, "SessionsVw.gridFiles");
			this.gridFiles.Location = new System.Drawing.Point(57, 111);
			this.gridFiles.MultiSelect = false;
			this.gridFiles.Name = "gridFiles";
			this.gridFiles.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.gridFiles.RowHeadersVisible = false;
			this.gridFiles.RowHeadersWidth = 22;
			this.gridFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridFiles.ShowWaterMarkWhenDirty = false;
			this.gridFiles.Size = new System.Drawing.Size(392, 139);
			this.gridFiles.TabIndex = 0;
			this.gridFiles.VirtualMode = true;
			this.gridFiles.Visible = false;
			this.gridFiles.WaterMark = "!";
			this.gridFiles.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gridFiles_CellValueNeeded);
			// 
			// iconCol
			// 
			this.iconCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.iconCol.DataPropertyName = "SmallIcon";
			this.iconCol.HeaderText = "";
			this.iconCol.Name = "iconCol";
			this.iconCol.ReadOnly = true;
			this.iconCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.iconCol.Width = 5;
			// 
			// filesNameCol
			// 
			this.filesNameCol.DataPropertyName = "FileName";
			this.filesNameCol.HeaderText = "Name";
			this.filesNameCol.Name = "filesNameCol";
			// 
			// filesTypeCol
			// 
			this.filesTypeCol.HeaderText = "Type";
			this.filesTypeCol.Name = "filesTypeCol";
			// 
			// filesTagsCol
			// 
			this.filesTagsCol.HeaderText = "Tags";
			this.filesTagsCol.Name = "filesTagsCol";
			// 
			// filesDateCol
			// 
			this.filesDateCol.HeaderText = "Date Modified";
			this.filesDateCol.Name = "filesDateCol";
			// 
			// filesSizeCol
			// 
			this.filesSizeCol.HeaderText = "Size";
			this.filesSizeCol.Name = "filesSizeCol";
			// 
			// lpSessions
			// 
			this.lpSessions.CurrentItem = null;
			this.lpSessions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lpSessions.Items = new object[0];
			this.locExtender.SetLocalizableToolTip(this.lpSessions, null);
			this.locExtender.SetLocalizationComment(this.lpSessions, null);
			this.locExtender.SetLocalizingId(this.lpSessions, "SessionsVw.ListPanel");
			this.lpSessions.Location = new System.Drawing.Point(0, 0);
			this.lpSessions.MinimumSize = new System.Drawing.Size(165, 0);
			this.lpSessions.Name = "lpSessions";
			this.lpSessions.Size = new System.Drawing.Size(165, 383);
			this.lpSessions.TabIndex = 0;
			this.lpSessions.Text = "Sessions";
			this.lpSessions.SelectedItemChanged += new SIL.Sponge.Controls.ListPanel.SelectedItemChangedHandler(this.lpSessions_SelectedItemChanged);
			this.lpSessions.DeleteButtonClicked += new SIL.Sponge.Controls.ListPanel.DeleteButtonClickHandler(this.lpSessions_DeleteButtonClicked);
			this.lpSessions.NewButtonClicked += new SIL.Sponge.Controls.ListPanel.NewButtonClickedHandler(this.lpSessions_NewButtonClicked);
			// 
			// picIcon
			// 
			this.locExtender.SetLocalizableToolTip(this.picIcon, null);
			this.locExtender.SetLocalizationComment(this.picIcon, null);
			this.locExtender.SetLocalizingId(this.picIcon, "SessionsVw.picIcon");
			this.picIcon.Location = new System.Drawing.Point(6, 21);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(32, 32);
			this.picIcon.TabIndex = 1;
			this.picIcon.TabStop = false;
			// 
			// lblFile
			// 
			this.lblFile.AutoSize = true;
			this.lblFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this.lblFile, null);
			this.locExtender.SetLocalizationComment(this.lblFile, null);
			this.locExtender.SetLocalizingId(this.lblFile, "SessionsVw.lblFile");
			this.lblFile.Location = new System.Drawing.Point(6, 4);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(14, 13);
			this.lblFile.TabIndex = 2;
			this.lblFile.Text = "#";
			// 
			// lblNoSessionsMsg
			// 
			this.lblNoSessionsMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblNoSessionsMsg.BackColor = System.Drawing.Color.Transparent;
			this.lblNoSessionsMsg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this.lblNoSessionsMsg, null);
			this.locExtender.SetLocalizationComment(this.lblNoSessionsMsg, null);
			this.locExtender.SetLocalizingId(this.lblNoSessionsMsg, "SessionsVw.lblNoSessionsMsg");
			this.lblNoSessionsMsg.Location = new System.Drawing.Point(14, 45);
			this.lblNoSessionsMsg.Name = "lblNoSessionsMsg";
			this.lblNoSessionsMsg.Size = new System.Drawing.Size(138, 168);
			this.lblNoSessionsMsg.TabIndex = 3;
			this.lblNoSessionsMsg.Text = "To add sessions, click the \'New\' button below.";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Views";
			// 
			// SessionsVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "SessionsVw.BaseSplitVw");
			this.Name = "SessionsVw";
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitRightSide.Panel1.ResumeLayout(false);
			this.splitRightSide.Panel2.ResumeLayout(false);
			this.splitRightSide.Panel2.PerformLayout();
			this.splitRightSide.ResumeLayout(false);
			this.tabSessions.ResumeLayout(false);
			this.tpgFiles.ResumeLayout(false);
			this.pnlGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridFiles)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabSessions;
		private System.Windows.Forms.TabPage tpgDescription;
		private System.Windows.Forms.TabPage tpgContributors;
		private System.Windows.Forms.TabPage tpgTaskStatus;
		private System.Windows.Forms.TabPage tpgFiles;
		private SilUtils.Controls.SilPanel pnlGrid;
		private SilUtils.SilGrid gridFiles;
		private ListPanel lpSessions;
		private System.Windows.Forms.Label lblFile;
		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.LinkLabel lnkSessionPath;
		private System.Windows.Forms.Label lblEmptySessionMsg;
		private System.Windows.Forms.DataGridViewImageColumn iconCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesNameCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesTypeCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesTagsCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesDateCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesSizeCol;
		private System.Windows.Forms.Label lblNoSessionsMsg;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}
