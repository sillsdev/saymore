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
			this.colDataModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._panelOuter = new SilUtils.Controls.SilPanel();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._buttonAdd = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
			this._panelOuter.SuspendLayout();
			this._tableLayout.SuspendLayout();
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
            this.colDataModified,
            this.colSize});
			this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._grid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(219)))), ((int)(((byte)(180)))));
			this._grid.IsDirty = false;
			this._grid.Location = new System.Drawing.Point(0, 0);
			this._grid.Margin = new System.Windows.Forms.Padding(0);
			this._grid.MultiSelect = false;
			this._grid.Name = "_grid";
			this._grid.PaintHeaderAcrossFullGridWidth = true;
			this._grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._grid.RowHeadersVisible = false;
			this._grid.RowHeadersWidth = 22;
			this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._grid.ShowWaterMarkWhenDirty = false;
			this._grid.Size = new System.Drawing.Size(468, 221);
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
			// _contextMenuStrip
			// 
			this._contextMenuStrip.Name = "_contextMenuStrip";
			this._contextMenuStrip.Size = new System.Drawing.Size(61, 4);
			// 
			// _panelOuter
			// 
			this._panelOuter.BackColor = System.Drawing.SystemColors.Window;
			this._panelOuter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelOuter.ClipTextForChildControls = true;
			this._panelOuter.ControlReceivingFocusOnMnemonic = null;
			this._panelOuter.Controls.Add(this._tableLayout);
			this._panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelOuter.DoubleBuffered = true;
			this._panelOuter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelOuter.Location = new System.Drawing.Point(0, 0);
			this._panelOuter.Margin = new System.Windows.Forms.Padding(0);
			this._panelOuter.MnemonicGeneratesClick = false;
			this._panelOuter.Name = "_panelOuter";
			this._panelOuter.PaintExplorerBarBackground = false;
			this._panelOuter.Size = new System.Drawing.Size(470, 255);
			this._panelOuter.TabIndex = 2;
			this._panelOuter.Paint += new System.Windows.Forms.PaintEventHandler(this.HandlePaintingGridButtonSeparatorLine);
			// 
			// _tableLayout
			// 
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 1;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._grid, 0, 0);
			this._tableLayout.Controls.Add(this._buttonAdd, 0, 1);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(0, 0);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 2;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(468, 253);
			this._tableLayout.TabIndex = 3;
			// 
			// _buttonAdd
			// 
			this._buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonAdd.AutoSize = true;
			this._buttonAdd.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
			this._buttonAdd.FlatAppearance.BorderSize = 0;
			this._buttonAdd.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CadetBlue;
			this._buttonAdd.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
			this._buttonAdd.Location = new System.Drawing.Point(390, 225);
			this._buttonAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 3);
			this._buttonAdd.MinimumSize = new System.Drawing.Size(75, 24);
			this._buttonAdd.Name = "_buttonAdd";
			this._buttonAdd.Size = new System.Drawing.Size(75, 25);
			this._buttonAdd.TabIndex = 3;
			this._buttonAdd.Tag = "";
			this._buttonAdd.Text = "Add...";
			this._buttonAdd.UseVisualStyleBackColor = true;
			this._buttonAdd.Click += new System.EventHandler(this.HandleAddButtonClick);
			// 
			// ComponentFileGrid
			// 
			this.Controls.Add(this._panelOuter);
			this.Name = "ComponentFileGrid";
			this.Size = new System.Drawing.Size(470, 255);
			((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
			this._panelOuter.ResumeLayout(false);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private SilUtils.SilGrid _grid;
		private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
		private System.Windows.Forms.DataGridViewImageColumn colIcon;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colType;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDataModified;
		private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
		private SilUtils.Controls.SilPanel _panelOuter;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Button _buttonAdd;

	}
}
