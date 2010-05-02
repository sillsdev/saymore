using SilUtils;
using Sponge2.UI.LowLevelControls;

namespace Sponge2.UI.ElementListScreen
{
	partial class SessionScreen
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this._componentEditorsTabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this._outerSplitter = new System.Windows.Forms.SplitContainer();
			this._sessionsListPanel = new Sponge2.UI.LowLevelControls.ListPanel();
			this._sessionComponentsSplitter = new System.Windows.Forms.SplitContainer();
			this._componentGridPanel = new SilUtils.Controls.SilPanel();
			this._componentGrid = new SilUtils.SilGrid();
			this.colIcon = new System.Windows.Forms.DataGridViewImageColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDataModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._componentEditorsTabControl.SuspendLayout();
			this._outerSplitter.Panel1.SuspendLayout();
			this._outerSplitter.Panel2.SuspendLayout();
			this._outerSplitter.SuspendLayout();
			this._sessionComponentsSplitter.Panel1.SuspendLayout();
			this._sessionComponentsSplitter.Panel2.SuspendLayout();
			this._sessionComponentsSplitter.SuspendLayout();
			this._componentGridPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._componentGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// _componentEditorsTabControl
			// 
			this._componentEditorsTabControl.Controls.Add(this.tabPage1);
			this._componentEditorsTabControl.Controls.Add(this.tabPage2);
			this._componentEditorsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this._componentEditorsTabControl.Location = new System.Drawing.Point(0, 0);
			this._componentEditorsTabControl.Name = "_componentEditorsTabControl";
			this._componentEditorsTabControl.SelectedIndex = 0;
			this._componentEditorsTabControl.Size = new System.Drawing.Size(315, 197);
			this._componentEditorsTabControl.TabIndex = 7;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(307, 171);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(307, 171);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// _outerSplitter
			// 
			this._outerSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._outerSplitter.Location = new System.Drawing.Point(0, 0);
			this._outerSplitter.Name = "_outerSplitter";
			// 
			// _outerSplitter.Panel1
			// 
			this._outerSplitter.Panel1.Controls.Add(this._sessionsListPanel);
			// 
			// _outerSplitter.Panel2
			// 
			this._outerSplitter.Panel2.Controls.Add(this._sessionComponentsSplitter);
			this._outerSplitter.Size = new System.Drawing.Size(503, 350);
			this._outerSplitter.SplitterDistance = 182;
			this._outerSplitter.SplitterWidth = 6;
			this._outerSplitter.TabIndex = 9;
			this._outerSplitter.TabStop = false;
			// 
			// _sessionsListPanel
			// 
			this._sessionsListPanel.CurrentItem = null;
			this._sessionsListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sessionsListPanel.Items = new object[0];
			// 
			// 
			// 
			this._sessionsListPanel.ListView.BackColor = System.Drawing.SystemColors.Window;
			this._sessionsListPanel.ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._sessionsListPanel.ListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sessionsListPanel.ListView.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._sessionsListPanel.ListView.FullRowSelect = true;
			this._sessionsListPanel.ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this._sessionsListPanel.ListView.HideSelection = false;
			this._sessionsListPanel.ListView.Location = new System.Drawing.Point(0, 30);
			this._sessionsListPanel.ListView.Name = "lvItems";
			this._sessionsListPanel.ListView.Size = new System.Drawing.Size(180, 284);
			this._sessionsListPanel.ListView.TabIndex = 0;
			this._sessionsListPanel.ListView.UseCompatibleStateImageBehavior = false;
			this._sessionsListPanel.ListView.View = System.Windows.Forms.View.Details;
			this._sessionsListPanel.Location = new System.Drawing.Point(0, 0);
			this._sessionsListPanel.MinimumSize = new System.Drawing.Size(165, 0);
			this._sessionsListPanel.Name = "_sessionsListPanel";
			this._sessionsListPanel.ReSortWhenItemTextChanges = false;
			this._sessionsListPanel.Size = new System.Drawing.Size(182, 350);
			this._sessionsListPanel.TabIndex = 8;
			this._sessionsListPanel.Text = "Sessions";
			// 
			// _sessionComponentsSplitter
			// 
			this._sessionComponentsSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sessionComponentsSplitter.Location = new System.Drawing.Point(0, 0);
			this._sessionComponentsSplitter.Name = "_sessionComponentsSplitter";
			this._sessionComponentsSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// _sessionComponentsSplitter.Panel1
			// 
			this._sessionComponentsSplitter.Panel1.Controls.Add(this._componentGridPanel);
			// 
			// _sessionComponentsSplitter.Panel2
			// 
			this._sessionComponentsSplitter.Panel2.Controls.Add(this._componentEditorsTabControl);
			this._sessionComponentsSplitter.Size = new System.Drawing.Size(315, 350);
			this._sessionComponentsSplitter.SplitterDistance = 147;
			this._sessionComponentsSplitter.SplitterWidth = 6;
			this._sessionComponentsSplitter.TabIndex = 0;
			this._sessionComponentsSplitter.TabStop = false;
			// 
			// _componentGridPanel
			// 
			this._componentGridPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._componentGridPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._componentGridPanel.ClipTextForChildControls = true;
			this._componentGridPanel.ControlReceivingFocusOnMnemonic = null;
			this._componentGridPanel.Controls.Add(this._componentGrid);
			this._componentGridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._componentGridPanel.DoubleBuffered = true;
			this._componentGridPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._componentGridPanel.Location = new System.Drawing.Point(0, 0);
			this._componentGridPanel.MnemonicGeneratesClick = false;
			this._componentGridPanel.Name = "_componentGridPanel";
			this._componentGridPanel.PaintExplorerBarBackground = false;
			this._componentGridPanel.Size = new System.Drawing.Size(315, 147);
			this._componentGridPanel.TabIndex = 1;
			// 
			// _componentGrid
			// 
			this._componentGrid.AllowUserToAddRows = false;
			this._componentGrid.AllowUserToDeleteRows = false;
			this._componentGrid.AllowUserToOrderColumns = true;
			this._componentGrid.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			this._componentGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this._componentGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this._componentGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this._componentGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._componentGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._componentGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this._componentGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._componentGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIcon,
            this.colName,
            this.colType,
            this.colTags,
            this.colDataModified,
            this.colSize});
			this._componentGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._componentGrid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._componentGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(219)))), ((int)(((byte)(180)))));
			this._componentGrid.IsDirty = false;
			this._componentGrid.Location = new System.Drawing.Point(0, 0);
			this._componentGrid.MultiSelect = false;
			this._componentGrid.Name = "_componentGrid";
			this._componentGrid.PaintHeaderAcrossFullGridWidth = true;
			this._componentGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._componentGrid.RowHeadersVisible = false;
			this._componentGrid.RowHeadersWidth = 22;
			this._componentGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._componentGrid.ShowWaterMarkWhenDirty = false;
			this._componentGrid.Size = new System.Drawing.Size(313, 145);
			this._componentGrid.TabIndex = 0;
			this._componentGrid.VirtualMode = true;
			this._componentGrid.WaterMark = "!";
			// 
			// colIcon
			// 
			this.colIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colIcon.DataPropertyName = "SmallIcon";
			this.colIcon.HeaderText = "";
			this.colIcon.Name = "colIcon";
			this.colIcon.ReadOnly = true;
			this.colIcon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colIcon.Width = 5;
			// 
			// colName
			// 
			this.colName.DataPropertyName = "FileName";
			this.colName.HeaderText = "Name";
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			// 
			// colType
			// 
			this.colType.DataPropertyName = "FileType";
			this.colType.HeaderText = "Type";
			this.colType.Name = "colType";
			this.colType.ReadOnly = true;
			// 
			// colTags
			// 
			this.colTags.DataPropertyName = "Tags";
			this.colTags.HeaderText = "Tags";
			this.colTags.Name = "colTags";
			// 
			// colDataModified
			// 
			this.colDataModified.DataPropertyName = "DateModified";
			this.colDataModified.HeaderText = "Date Modified";
			this.colDataModified.Name = "colDataModified";
			this.colDataModified.ReadOnly = true;
			this.colDataModified.Width = 107;
			// 
			// colSize
			// 
			this.colSize.DataPropertyName = "FileSize";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colSize.DefaultCellStyle = dataGridViewCellStyle3;
			this.colSize.HeaderText = "Size";
			this.colSize.Name = "colSize";
			this.colSize.ReadOnly = true;
			this.colSize.Width = 52;
			// 
			// SessionScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._outerSplitter);
			this.Name = "SessionScreen";
			this.Size = new System.Drawing.Size(503, 350);
			this._componentEditorsTabControl.ResumeLayout(false);
			this._outerSplitter.Panel1.ResumeLayout(false);
			this._outerSplitter.Panel2.ResumeLayout(false);
			this._outerSplitter.ResumeLayout(false);
			this._sessionComponentsSplitter.Panel1.ResumeLayout(false);
			this._sessionComponentsSplitter.Panel2.ResumeLayout(false);
			this._sessionComponentsSplitter.ResumeLayout(false);
			this._componentGridPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._componentGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl _componentEditorsTabControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private ListPanel _sessionsListPanel;
		private System.Windows.Forms.SplitContainer _outerSplitter;
		private System.Windows.Forms.SplitContainer _sessionComponentsSplitter;
		private SilGrid _componentGrid;
		private System.Windows.Forms.DataGridViewImageColumn colIcon;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colType;
		private System.Windows.Forms.DataGridViewTextBoxColumn colTags;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDataModified;
		private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
		private SilUtils.Controls.SilPanel _componentGridPanel;
	}
}
