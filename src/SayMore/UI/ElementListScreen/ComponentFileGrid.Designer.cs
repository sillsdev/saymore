namespace SayMore.UI.ElementListScreen
{
	partial class ComponentFileGrid
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this._grid = new SilUtils.SilGrid();
			this.colIcon = new System.Windows.Forms.DataGridViewImageColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDataModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
			this.SuspendLayout();
			// 
			// _grid
			// 
			this._grid.AllowUserToAddRows = false;
			this._grid.AllowUserToDeleteRows = false;
			this._grid.AllowUserToOrderColumns = true;
			this._grid.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			this._grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this._grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this._grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIcon,
            this.colName,
            this.colType,
            this.colTags,
            this.colDataModified,
            this.colSize});
			this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._grid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(219)))), ((int)(((byte)(180)))));
			this._grid.IsDirty = false;
			this._grid.Location = new System.Drawing.Point(0, 0);
			this._grid.MultiSelect = false;
			this._grid.Name = "_grid";
			this._grid.PaintHeaderAcrossFullGridWidth = true;
			this._grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._grid.RowHeadersVisible = false;
			this._grid.RowHeadersWidth = 22;
			this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._grid.ShowWaterMarkWhenDirty = false;
			this._grid.Size = new System.Drawing.Size(580, 260);
			this._grid.TabIndex = 1;
			this._grid.VirtualMode = true;
			this._grid.WaterMark = "!";
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
			// contextMenuStrip1
			// 
			this._contextMenuStrip.Name = "_contextMenuStrip";
			this._contextMenuStrip.Size = new System.Drawing.Size(153, 26);
			// 
			// ComponentFileGrid
			// 
			this.Controls.Add(this._grid);
			this.Name = "ComponentFileGrid";
			this.Size = new System.Drawing.Size(580, 260);
			((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SilUtils.SilGrid _grid;
		private System.Windows.Forms.DataGridViewImageColumn colIcon;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colType;
		private System.Windows.Forms.DataGridViewTextBoxColumn colTags;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDataModified;
		private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
		private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;

	}
}
