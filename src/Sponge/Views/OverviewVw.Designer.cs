using SIL.Sponge.Controls;

namespace SIL.Sponge
{
	partial class OverviewVw
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OverviewVw));
			this.pnlStatistics = new SilUtils.Controls.SilPanel();
			this.gridGenre = new SilUtils.SilGrid();
			this.genreNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.genreRecordedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.genreMinRepeatedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.genreTranscribedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gridTasks = new SilUtils.SilGrid();
			this.taskNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.taskGenreCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.taskDescribedCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.taskRecordedCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.taskRepeatedCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.taskTranscribedCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.taskSubmittedCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.pnlContributor = new SilUtils.Controls.SilPanel();
			this.tsOverview = new SpongeBar();
			this.tsbStatistics = new System.Windows.Forms.ToolStripButton();
			this.tsbGenre = new System.Windows.Forms.ToolStripButton();
			this.tsbContributor = new System.Windows.Forms.ToolStripButton();
			this.tsbTasks = new System.Windows.Forms.ToolStripButton();
			((System.ComponentModel.ISupportInitialize)(this.gridGenre)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridTasks)).BeginInit();
			this.tsOverview.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlStatistics
			// 
			this.pnlStatistics.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(166)))), ((int)(((byte)(170)))));
			this.pnlStatistics.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlStatistics.ClipTextForChildControls = true;
			this.pnlStatistics.ControlReceivingFocusOnMnemonic = null;
			this.pnlStatistics.DoubleBuffered = true;
			this.pnlStatistics.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlStatistics.Location = new System.Drawing.Point(157, 3);
			this.pnlStatistics.MnemonicGeneratesClick = false;
			this.pnlStatistics.Name = "pnlStatistics";
			this.pnlStatistics.PaintExplorerBarBackground = false;
			this.pnlStatistics.Size = new System.Drawing.Size(152, 119);
			this.pnlStatistics.TabIndex = 1;
			this.pnlStatistics.Text = "Statistics";
			this.pnlStatistics.Visible = false;
			// 
			// gridGenre
			// 
			this.gridGenre.AllowUserToAddRows = false;
			this.gridGenre.AllowUserToDeleteRows = false;
			this.gridGenre.AllowUserToOrderColumns = true;
			this.gridGenre.AllowUserToResizeRows = false;
			this.gridGenre.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.gridGenre.BackgroundColor = System.Drawing.SystemColors.Window;
			this.gridGenre.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gridGenre.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridGenre.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridGenre.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridGenre.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.genreNameCol,
            this.genreRecordedCol,
            this.genreMinRepeatedCol,
            this.genreTranscribedCol});
			this.gridGenre.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(219)))), ((int)(((byte)(180)))));
			this.gridGenre.IsDirty = false;
			this.gridGenre.Location = new System.Drawing.Point(157, 138);
			this.gridGenre.MultiSelect = false;
			this.gridGenre.Name = "gridGenre";
			this.gridGenre.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.gridGenre.RowHeadersVisible = false;
			this.gridGenre.RowHeadersWidth = 22;
			this.gridGenre.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridGenre.ShowWaterMarkWhenDirty = false;
			this.gridGenre.Size = new System.Drawing.Size(475, 84);
			this.gridGenre.TabIndex = 2;
			this.gridGenre.Visible = false;
			this.gridGenre.WaterMark = "!";
			// 
			// genreNameCol
			// 
			this.genreNameCol.HeaderText = "Name";
			this.genreNameCol.Name = "genreNameCol";
			// 
			// genreRecordedCol
			// 
			this.genreRecordedCol.HeaderText = "Minutes Recorded";
			this.genreRecordedCol.Name = "genreRecordedCol";
			// 
			// genreMinRepeatedCol
			// 
			this.genreMinRepeatedCol.HeaderText = "Minutes Repeated";
			this.genreMinRepeatedCol.Name = "genreMinRepeatedCol";
			// 
			// genreTranscribedCol
			// 
			this.genreTranscribedCol.HeaderText = "Minutes Transcribed";
			this.genreTranscribedCol.Name = "genreTranscribedCol";
			// 
			// gridTasks
			// 
			this.gridTasks.AllowUserToAddRows = false;
			this.gridTasks.AllowUserToDeleteRows = false;
			this.gridTasks.AllowUserToOrderColumns = true;
			this.gridTasks.AllowUserToResizeRows = false;
			this.gridTasks.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.gridTasks.BackgroundColor = System.Drawing.SystemColors.Window;
			this.gridTasks.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gridTasks.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridTasks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.gridTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.taskNameCol,
            this.taskGenreCol,
            this.taskDescribedCol,
            this.taskRecordedCol,
            this.taskRepeatedCol,
            this.taskTranscribedCol,
            this.taskSubmittedCol});
			this.gridTasks.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(219)))), ((int)(((byte)(180)))));
			this.gridTasks.IsDirty = false;
			this.gridTasks.Location = new System.Drawing.Point(157, 240);
			this.gridTasks.MultiSelect = false;
			this.gridTasks.Name = "gridTasks";
			this.gridTasks.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.gridTasks.RowHeadersVisible = false;
			this.gridTasks.RowHeadersWidth = 22;
			this.gridTasks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridTasks.ShowWaterMarkWhenDirty = false;
			this.gridTasks.Size = new System.Drawing.Size(475, 84);
			this.gridTasks.TabIndex = 3;
			this.gridTasks.Visible = false;
			this.gridTasks.WaterMark = "!";
			// 
			// taskNameCol
			// 
			this.taskNameCol.HeaderText = "Name";
			this.taskNameCol.Name = "taskNameCol";
			// 
			// taskGenreCol
			// 
			this.taskGenreCol.HeaderText = "Genre";
			this.taskGenreCol.Name = "taskGenreCol";
			// 
			// taskDescribedCol
			// 
			this.taskDescribedCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.taskDescribedCol.HeaderText = "Described";
			this.taskDescribedCol.Name = "taskDescribedCol";
			this.taskDescribedCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.taskDescribedCol.Width = 62;
			// 
			// taskRecordedCol
			// 
			this.taskRecordedCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.taskRecordedCol.HeaderText = "Recorded";
			this.taskRecordedCol.Name = "taskRecordedCol";
			this.taskRecordedCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.taskRecordedCol.Width = 60;
			// 
			// taskRepeatedCol
			// 
			this.taskRepeatedCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.taskRepeatedCol.HeaderText = "Repeated";
			this.taskRepeatedCol.Name = "taskRepeatedCol";
			this.taskRepeatedCol.Width = 60;
			// 
			// taskTranscribedCol
			// 
			this.taskTranscribedCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.taskTranscribedCol.HeaderText = "Transcribed";
			this.taskTranscribedCol.Name = "taskTranscribedCol";
			this.taskTranscribedCol.Width = 70;
			// 
			// taskSubmittedCol
			// 
			this.taskSubmittedCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.taskSubmittedCol.HeaderText = "Submitted";
			this.taskSubmittedCol.Name = "taskSubmittedCol";
			this.taskSubmittedCol.Width = 64;
			// 
			// pnlContributor
			// 
			this.pnlContributor.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(166)))), ((int)(((byte)(170)))));
			this.pnlContributor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlContributor.ClipTextForChildControls = true;
			this.pnlContributor.ControlReceivingFocusOnMnemonic = null;
			this.pnlContributor.DoubleBuffered = true;
			this.pnlContributor.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlContributor.Location = new System.Drawing.Point(383, 3);
			this.pnlContributor.MnemonicGeneratesClick = false;
			this.pnlContributor.Name = "pnlContributor";
			this.pnlContributor.PaintExplorerBarBackground = false;
			this.pnlContributor.Size = new System.Drawing.Size(152, 119);
			this.pnlContributor.TabIndex = 2;
			this.pnlContributor.Text = "By Contributor";
			this.pnlContributor.Visible = false;
			// 
			// tsOverview
			// 
			this.tsOverview.Dock = System.Windows.Forms.DockStyle.Left;
			this.tsOverview.GradientAngle = 65F;
			this.tsOverview.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsOverview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbStatistics,
            this.tsbGenre,
            this.tsbContributor,
            this.tsbTasks});
			this.tsOverview.Location = new System.Drawing.Point(0, 0);
			this.tsOverview.Name = "tsOverview";
			this.tsOverview.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.tsOverview.Size = new System.Drawing.Size(121, 351);
			this.tsOverview.TabIndex = 0;
			this.tsOverview.Text = "toolStrip1";
			// 
			// tsbStatistics
			// 
			this.tsbStatistics.AutoSize = false;
			this.tsbStatistics.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbStatistics.Image = ((System.Drawing.Image)(resources.GetObject("tsbStatistics.Image")));
			this.tsbStatistics.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbStatistics.Margin = new System.Windows.Forms.Padding(10, 5, 10, 2);
			this.tsbStatistics.Name = "tsbStatistics";
			this.tsbStatistics.Size = new System.Drawing.Size(100, 24);
			this.tsbStatistics.Text = "Statistics";
			this.tsbStatistics.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tsbGenre
			// 
			this.tsbGenre.AutoSize = false;
			this.tsbGenre.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbGenre.Image = ((System.Drawing.Image)(resources.GetObject("tsbGenre.Image")));
			this.tsbGenre.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbGenre.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
			this.tsbGenre.Name = "tsbGenre";
			this.tsbGenre.Size = new System.Drawing.Size(100, 24);
			this.tsbGenre.Text = "By Genre";
			this.tsbGenre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tsbContributor
			// 
			this.tsbContributor.AutoSize = false;
			this.tsbContributor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbContributor.Image = ((System.Drawing.Image)(resources.GetObject("tsbContributor.Image")));
			this.tsbContributor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbContributor.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
			this.tsbContributor.Name = "tsbContributor";
			this.tsbContributor.Size = new System.Drawing.Size(100, 24);
			this.tsbContributor.Text = "By Contributor";
			this.tsbContributor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tsbTasks
			// 
			this.tsbTasks.AutoSize = false;
			this.tsbTasks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbTasks.Image = ((System.Drawing.Image)(resources.GetObject("tsbTasks.Image")));
			this.tsbTasks.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbTasks.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
			this.tsbTasks.Name = "tsbTasks";
			this.tsbTasks.Size = new System.Drawing.Size(100, 24);
			this.tsbTasks.Text = "Session Tasks";
			this.tsbTasks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// OverviewVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlContributor);
			this.Controls.Add(this.gridTasks);
			this.Controls.Add(this.gridGenre);
			this.Controls.Add(this.pnlStatistics);
			this.Controls.Add(this.tsOverview);
			this.Name = "OverviewVw";
			this.Size = new System.Drawing.Size(657, 351);
			((System.ComponentModel.ISupportInitialize)(this.gridGenre)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridTasks)).EndInit();
			this.tsOverview.ResumeLayout(false);
			this.tsOverview.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SpongeBar tsOverview;
		private System.Windows.Forms.ToolStripButton tsbStatistics;
		private System.Windows.Forms.ToolStripButton tsbGenre;
		private System.Windows.Forms.ToolStripButton tsbContributor;
		private System.Windows.Forms.ToolStripButton tsbTasks;
		private SilUtils.Controls.SilPanel pnlStatistics;
		private SilUtils.SilGrid gridGenre;
		private System.Windows.Forms.DataGridViewTextBoxColumn genreNameCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn genreRecordedCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn genreMinRepeatedCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn genreTranscribedCol;
		private SilUtils.SilGrid gridTasks;
		private SilUtils.Controls.SilPanel pnlContributor;
		private System.Windows.Forms.DataGridViewTextBoxColumn taskNameCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn taskGenreCol;
		private System.Windows.Forms.DataGridViewCheckBoxColumn taskDescribedCol;
		private System.Windows.Forms.DataGridViewCheckBoxColumn taskRecordedCol;
		private System.Windows.Forms.DataGridViewCheckBoxColumn taskRepeatedCol;
		private System.Windows.Forms.DataGridViewCheckBoxColumn taskTranscribedCol;
		private System.Windows.Forms.DataGridViewCheckBoxColumn taskSubmittedCol;
	}
}
