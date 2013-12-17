using System;

namespace SayMore.UI.Overview
{
	partial class ProjectScreen
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectScreen));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tsbStatistics = new System.Windows.Forms.ToolStripButton();
			this.tsbGenre = new System.Windows.Forms.ToolStripButton();
			this.tsbContributor = new System.Windows.Forms.ToolStripButton();
			this.tsbTasks = new System.Windows.Forms.ToolStripButton();
			this._sidebar = new Palaso.UI.WindowsForms.Widgets.EnhancedPanel();
			this._projectPages = new Palaso.UI.WindowsForms.Widgets.BetterGrid.BetterGrid();
			this.colIcon = new System.Windows.Forms.DataGridViewImageColumn();
			this._splitter = new System.Windows.Forms.SplitContainer();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colPageNames = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._sidebar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._projectPages)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._splitter)).BeginInit();
			this._splitter.Panel1.SuspendLayout();
			this._splitter.SuspendLayout();
			this.SuspendLayout();
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
			// _sidebar
			//
			this._sidebar.BackColor = System.Drawing.SystemColors.Window;
			this._sidebar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._sidebar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._sidebar.ClipTextForChildControls = true;
			this._sidebar.ControlReceivingFocusOnMnemonic = null;
			this._sidebar.Controls.Add(this._projectPages);
			this._sidebar.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sidebar.DoubleBuffered = true;
			this._sidebar.DrawOnlyBottomBorder = false;
			this._sidebar.DrawOnlyTopBorder = false;
			this._sidebar.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._sidebar.ForeColor = System.Drawing.SystemColors.ControlText;
			this._sidebar.Location = new System.Drawing.Point(0, 0);
			this._sidebar.MnemonicGeneratesClick = false;
			this._sidebar.Name = "_sidebar";
			this._sidebar.PaintExplorerBarBackground = false;
			this._sidebar.Size = new System.Drawing.Size(175, 351);
			this._sidebar.TabIndex = 2;
			//
			// _projectPages
			//
			this._projectPages.AllowUserToAddRows = false;
			this._projectPages.AllowUserToDeleteRows = false;
			this._projectPages.AllowUserToOrderColumns = true;
			this._projectPages.AllowUserToResizeColumns = false;
			this._projectPages.AllowUserToResizeRows = false;
			this._projectPages.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this._projectPages.BackgroundColor = System.Drawing.SystemColors.Window;
			this._projectPages.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._projectPages.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._projectPages.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this._projectPages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._projectPages.ColumnHeadersVisible = false;
			this._projectPages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.colIcon,
			this.colPageNames});
			this._projectPages.Dock = System.Windows.Forms.DockStyle.Fill;
			this._projectPages.DrawTextBoxEditControlBorder = false;
			this._projectPages.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._projectPages.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this._projectPages.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this._projectPages.Location = new System.Drawing.Point(0, 0);
			this._projectPages.MultiSelect = false;
			this._projectPages.Name = "_projectPages";
			this._projectPages.PaintHeaderAcrossFullGridWidth = true;
			this._projectPages.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._projectPages.RowHeadersVisible = false;
			this._projectPages.RowHeadersWidth = 22;
			this._projectPages.SelectedCellBackColor = System.Drawing.Color.Empty;
			this._projectPages.SelectedCellForeColor = System.Drawing.Color.Empty;
			this._projectPages.SelectedRowBackColor = System.Drawing.Color.Empty;
			this._projectPages.SelectedRowForeColor = System.Drawing.Color.Empty;
			this._projectPages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._projectPages.ShowWaterMarkWhenDirty = false;
			this._projectPages.Size = new System.Drawing.Size(173, 349);
			this._projectPages.TabIndex = 0;
			this._projectPages.TextBoxEditControlBorderColor = System.Drawing.Color.Silver;
			this._projectPages.WaterMark = "!";
			this._projectPages.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this._projectPages_RowEnter);
			//
			// colIcon
			//
			this.colIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colIcon.HeaderText = "";
			this.colIcon.MinimumWidth = 26;
			this.colIcon.Name = "colIcon";
			this.colIcon.ReadOnly = true;
			this.colIcon.Width = 26;
			//
			// _splitter
			//
			this._splitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this._splitter.Location = new System.Drawing.Point(0, 0);
			this._splitter.Name = "_splitter";
			//
			// _splitter.Panel1
			//
			this._splitter.Panel1.Controls.Add(this._sidebar);
			this._splitter.Size = new System.Drawing.Size(657, 351);
			this._splitter.SplitterDistance = 175;
			this._splitter.TabIndex = 3;
			//
			// dataGridViewTextBoxColumn1
			//
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn1.HeaderText = "";
			this.dataGridViewTextBoxColumn1.MinimumWidth = 50;
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			//
			// colPageNames
			//
			this.colPageNames.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colPageNames.HeaderText = "";
			this.colPageNames.MinimumWidth = 50;
			this.colPageNames.Name = "colPageNames";
			this.colPageNames.ReadOnly = true;
			this.colPageNames.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			//
			// ProjectScreen
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._splitter);
			this.Name = "ProjectScreen";
			this.Size = new System.Drawing.Size(657, 351);
			this._sidebar.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._projectPages)).EndInit();
			this._splitter.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._splitter)).EndInit();
			this._splitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripButton tsbStatistics;
		private System.Windows.Forms.ToolStripButton tsbGenre;
		private System.Windows.Forms.ToolStripButton tsbContributor;
		private System.Windows.Forms.ToolStripButton tsbTasks;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private Palaso.UI.WindowsForms.Widgets.EnhancedPanel _sidebar;
		private Palaso.UI.WindowsForms.Widgets.BetterGrid.BetterGrid _projectPages;
		private System.Windows.Forms.SplitContainer _splitter;
		private System.Windows.Forms.DataGridViewImageColumn colIcon;
		private System.Windows.Forms.DataGridViewTextBoxColumn colPageNames;

		public string NameForUsageReporting
		{
			get { return "Progress"; }
		}
	}
}
