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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tabSessions = new System.Windows.Forms.TabControl();
			this.tpgDescription = new System.Windows.Forms.TabPage();
			this.tpgContributors = new System.Windows.Forms.TabPage();
			this.tpgTaskStatus = new System.Windows.Forms.TabPage();
			this.tpgFiles = new System.Windows.Forms.TabPage();
			this.pnlGrid = new SilUtils.Controls.SilPanel();
			this.gridFiles = new SilUtils.SilGrid();
			this.iconCol = new System.Windows.Forms.DataGridViewImageColumn();
			this.filesNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesTagsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesDateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesSizeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.lpSessions = new ListPanel();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.lblFile = new System.Windows.Forms.Label();
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
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			// 
			// splitOuter.Panel1
			// 
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
			this.splitRightSide.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
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
			this.tabSessions.Size = new System.Drawing.Size(488, 285);
			this.tabSessions.TabIndex = 0;
			this.tabSessions.SizeChanged += new System.EventHandler(this.tabSessions_SizeChanged);
			// 
			// tpgDescription
			// 
			this.tpgDescription.Location = new System.Drawing.Point(4, 26);
			this.tpgDescription.Name = "tpgDescription";
			this.tpgDescription.Padding = new System.Windows.Forms.Padding(3);
			this.tpgDescription.Size = new System.Drawing.Size(480, 255);
			this.tpgDescription.TabIndex = 0;
			this.tpgDescription.Text = "Description";
			this.tpgDescription.ToolTipText = "Description";
			this.tpgDescription.UseVisualStyleBackColor = true;
			// 
			// tpgContributors
			// 
			this.tpgContributors.Location = new System.Drawing.Point(4, 26);
			this.tpgContributors.Name = "tpgContributors";
			this.tpgContributors.Padding = new System.Windows.Forms.Padding(3);
			this.tpgContributors.Size = new System.Drawing.Size(480, 255);
			this.tpgContributors.TabIndex = 1;
			this.tpgContributors.Text = "Contributors && Permissions";
			this.tpgContributors.ToolTipText = "Contributors & Permissions";
			this.tpgContributors.UseVisualStyleBackColor = true;
			// 
			// tpgTaskStatus
			// 
			this.tpgTaskStatus.Location = new System.Drawing.Point(4, 26);
			this.tpgTaskStatus.Name = "tpgTaskStatus";
			this.tpgTaskStatus.Size = new System.Drawing.Size(480, 255);
			this.tpgTaskStatus.TabIndex = 2;
			this.tpgTaskStatus.Text = "Task Status";
			this.tpgTaskStatus.ToolTipText = "Task Status";
			this.tpgTaskStatus.UseVisualStyleBackColor = true;
			// 
			// tpgFiles
			// 
			this.tpgFiles.Controls.Add(this.pnlGrid);
			this.tpgFiles.Location = new System.Drawing.Point(4, 26);
			this.tpgFiles.Name = "tpgFiles";
			this.tpgFiles.Size = new System.Drawing.Size(480, 255);
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
			this.pnlGrid.Controls.Add(this.gridFiles);
			this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlGrid.DoubleBuffered = true;
			this.pnlGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlGrid.Location = new System.Drawing.Point(0, 0);
			this.pnlGrid.MnemonicGeneratesClick = false;
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.PaintExplorerBarBackground = false;
			this.pnlGrid.Size = new System.Drawing.Size(480, 255);
			this.pnlGrid.TabIndex = 1;
			// 
			// gridFiles
			// 
			this.gridFiles.AllowUserToAddRows = false;
			this.gridFiles.AllowUserToDeleteRows = false;
			this.gridFiles.AllowUserToOrderColumns = true;
			this.gridFiles.AllowUserToResizeRows = false;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
			this.gridFiles.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
			this.gridFiles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.gridFiles.BackgroundColor = System.Drawing.SystemColors.Window;
			this.gridFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.gridFiles.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridFiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.gridFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iconCol,
            this.filesNameCol,
            this.filesTypeCol,
            this.filesTagsCol,
            this.filesDateCol,
            this.filesSizeCol});
			this.gridFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridFiles.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(219)))), ((int)(((byte)(180)))));
			this.gridFiles.IsDirty = false;
			this.gridFiles.Location = new System.Drawing.Point(0, 0);
			this.gridFiles.MultiSelect = false;
			this.gridFiles.Name = "gridFiles";
			this.gridFiles.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.gridFiles.RowHeadersVisible = false;
			this.gridFiles.RowHeadersWidth = 22;
			this.gridFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridFiles.ShowWaterMarkWhenDirty = false;
			this.gridFiles.Size = new System.Drawing.Size(478, 253);
			this.gridFiles.TabIndex = 0;
			this.gridFiles.WaterMark = "!";
			// 
			// iconCol
			// 
			this.iconCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.iconCol.HeaderText = "";
			this.iconCol.Name = "iconCol";
			this.iconCol.ReadOnly = true;
			this.iconCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.iconCol.Width = 5;
			// 
			// filesNameCol
			// 
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
			this.lpSessions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lpSessions.Location = new System.Drawing.Point(0, 0);
			this.lpSessions.MinimumSize = new System.Drawing.Size(165, 0);
			this.lpSessions.Name = "lpSessions";
			this.lpSessions.Size = new System.Drawing.Size(165, 383);
			this.lpSessions.TabIndex = 0;
			this.lpSessions.Text = "Sessions";
			// 
			// picIcon
			// 
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
			this.lblFile.Location = new System.Drawing.Point(6, 4);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(14, 13);
			this.lblFile.TabIndex = 2;
			this.lblFile.Text = "#";
			// 
			// SessionsVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
		private System.Windows.Forms.DataGridViewImageColumn iconCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesNameCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesTypeCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesTagsCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesDateCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesSizeCol;
		private System.Windows.Forms.Label lblFile;
		private System.Windows.Forms.PictureBox picIcon;
	}
}
