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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentFileGrid));
			this._grid = new SayMore.UI.ElementListScreen.InternalComponentFileGrid();
			this.colIcon = new System.Windows.Forms.DataGridViewImageColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDataModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this._panelOuter = new SilUtils.Controls.SilPanel();
			this._toolStripActions = new System.Windows.Forms.ToolStrip();
			this._buttonOpen = new System.Windows.Forms.ToolStripDropDownButton();
			this._buttonRename = new System.Windows.Forms.ToolStripDropDownButton();
			this._buttonConvert = new System.Windows.Forms.ToolStripDropDownButton();
			this._buttonAddFiles = new System.Windows.Forms.ToolStripButton();
			this._buttonDelete = new System.Windows.Forms.ToolStripButton();
			((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
			this._panelOuter.SuspendLayout();
			this._toolStripActions.SuspendLayout();
			this.SuspendLayout();
			// 
			// _grid
			// 
			this._grid.AllowUserToAddRows = false;
			this._grid.AllowUserToDeleteRows = false;
			this._grid.AllowUserToOrderColumns = true;
			this._grid.AllowUserToResizeRows = false;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
			this._grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
			this._grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this._grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this._grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIcon,
            this.colName,
            this.colType,
            this.colDataModified,
            this.colSize,
            this.colDuration});
			this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._grid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._grid.FullRowFocusRectangleColor = System.Drawing.SystemColors.ControlDark;
			this._grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(219)))), ((int)(((byte)(180)))));
			this._grid.IsDirty = false;
			this._grid.Location = new System.Drawing.Point(0, 25);
			this._grid.Margin = new System.Windows.Forms.Padding(0);
			this._grid.MultiSelect = false;
			this._grid.Name = "_grid";
			this._grid.PaintFullRowFocusRectangle = true;
			this._grid.PaintHeaderAcrossFullGridWidth = true;
			this._grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._grid.RowHeadersVisible = false;
			this._grid.RowHeadersWidth = 22;
			this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._grid.ShowWaterMarkWhenDirty = false;
			this._grid.Size = new System.Drawing.Size(468, 228);
			this._grid.StandardTab = true;
			this._grid.TabIndex = 1;
			this._grid.VirtualMode = true;
			this._grid.WaterMark = "!";
			// 
			// colIcon
			// 
			this.colIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colIcon.DataPropertyName = "SmallIcon";
			this.colIcon.Name = "colIcon";
			this.colIcon.ReadOnly = true;
			this.colIcon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colIcon.Width = 52;
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
			this.colType.DataPropertyName = "FileTypeDescription";
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
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colSize.DefaultCellStyle = dataGridViewCellStyle7;
			this.colSize.HeaderText = "Size";
			this.colSize.Name = "colSize";
			this.colSize.ReadOnly = true;
			this.colSize.Width = 52;
			// 
			// colDuration
			// 
			this.colDuration.DataPropertyName = "DurationString";
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colDuration.DefaultCellStyle = dataGridViewCellStyle8;
			this.colDuration.HeaderText = "Duration";
			this.colDuration.Name = "colDuration";
			this.colDuration.ReadOnly = true;
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
			this._panelOuter.Controls.Add(this._grid);
			this._panelOuter.Controls.Add(this._toolStripActions);
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
			// 
			// _toolStripActions
			// 
			this._toolStripActions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStripActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonOpen,
            this._buttonRename,
            this._buttonConvert,
            this._buttonAddFiles,
            this._buttonDelete});
			this._toolStripActions.Location = new System.Drawing.Point(0, 0);
			this._toolStripActions.Name = "_toolStripActions";
			this._toolStripActions.Padding = new System.Windows.Forms.Padding(7, 0, 7, 2);
			this._toolStripActions.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this._toolStripActions.Size = new System.Drawing.Size(468, 25);
			this._toolStripActions.TabIndex = 4;
			this._toolStripActions.Text = "toolStrip1";
			this._toolStripActions.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleToolStripActionsPaint);
			// 
			// _buttonOpen
			// 
			this._buttonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._buttonOpen.Enabled = false;
			this._buttonOpen.Image = ((System.Drawing.Image)(resources.GetObject("_buttonOpen.Image")));
			this._buttonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonOpen.Name = "_buttonOpen";
			this._buttonOpen.Size = new System.Drawing.Size(49, 20);
			this._buttonOpen.Text = "Open";
			this._buttonOpen.ToolTipText = "Open Selected File";
			this._buttonOpen.DropDownOpening += new System.EventHandler(this.HandleActionsDropDownOpening);
			// 
			// _buttonRename
			// 
			this._buttonRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._buttonRename.Enabled = false;
			this._buttonRename.Image = ((System.Drawing.Image)(resources.GetObject("_buttonRename.Image")));
			this._buttonRename.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonRename.Margin = new System.Windows.Forms.Padding(4, 1, 0, 2);
			this._buttonRename.Name = "_buttonRename";
			this._buttonRename.Size = new System.Drawing.Size(63, 20);
			this._buttonRename.Text = "Rename";
			this._buttonRename.ToolTipText = "Rename Selected File";
			this._buttonRename.DropDownOpening += new System.EventHandler(this.HandleActionsDropDownOpening);
			// 
			// _buttonConvert
			// 
			this._buttonConvert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._buttonConvert.Enabled = false;
			this._buttonConvert.Image = ((System.Drawing.Image)(resources.GetObject("_buttonConvert.Image")));
			this._buttonConvert.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonConvert.Margin = new System.Windows.Forms.Padding(4, 1, 0, 2);
			this._buttonConvert.Name = "_buttonConvert";
			this._buttonConvert.Size = new System.Drawing.Size(62, 20);
			this._buttonConvert.Text = "Convert";
			this._buttonConvert.ToolTipText = "Convert Selected File";
			this._buttonConvert.DropDownOpening += new System.EventHandler(this.HandleActionsDropDownOpening);
			// 
			// _buttonAddFiles
			// 
			this._buttonAddFiles.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonAddFiles.Image = global::SayMore.Properties.Resources.Add;
			this._buttonAddFiles.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonAddFiles.Name = "_buttonAddFiles";
			this._buttonAddFiles.Size = new System.Drawing.Size(84, 20);
			this._buttonAddFiles.Text = "Add Files...";
			this._buttonAddFiles.ToolTipText = "Add Files to the event";
			this._buttonAddFiles.Click += new System.EventHandler(this.HandleAddButtonClick);
			// 
			// _buttonDelete
			// 
			this._buttonDelete.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonDelete.Image = global::SayMore.Properties.Resources.Delete;
			this._buttonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonDelete.Margin = new System.Windows.Forms.Padding(0, 1, 7, 2);
			this._buttonDelete.Name = "_buttonDelete";
			this._buttonDelete.Size = new System.Drawing.Size(60, 20);
			this._buttonDelete.Text = "Delete";
			this._buttonDelete.ToolTipText = "Delete Selected File";
			this._buttonDelete.Click += new System.EventHandler(this.HandleDeleteButtonClick);
			// 
			// ComponentFileGrid
			// 
			this.Controls.Add(this._panelOuter);
			this.Name = "ComponentFileGrid";
			this.Size = new System.Drawing.Size(470, 255);
			((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
			this._panelOuter.ResumeLayout(false);
			this._panelOuter.PerformLayout();
			this._toolStripActions.ResumeLayout(false);
			this._toolStripActions.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private InternalComponentFileGrid _grid;
		private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
		private SilUtils.Controls.SilPanel _panelOuter;
		private System.Windows.Forms.ToolStrip _toolStripActions;
		private System.Windows.Forms.ToolStripDropDownButton _buttonOpen;
		private System.Windows.Forms.ToolStripDropDownButton _buttonRename;
		private System.Windows.Forms.ToolStripDropDownButton _buttonConvert;
		private System.Windows.Forms.ToolStripButton _buttonAddFiles;
		private System.Windows.Forms.DataGridViewImageColumn colIcon;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colType;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDataModified;
		private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDuration;
		private System.Windows.Forms.ToolStripButton _buttonDelete;

	}
}
