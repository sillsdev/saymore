namespace SIL.Sponge.Dialogs
{
	partial class NewSessionsFromFilesDlg
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this._findFilesLink = new System.Windows.Forms.LinkLabel();
			this._createSessionsButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._sourceFolderLabel = new System.Windows.Forms.Label();
			this._filesGrid = new SilUtils.SilGrid();
			this._selectedCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this._iconCol = new System.Windows.Forms.DataGridViewImageColumn();
			this._fileNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._fileTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._dateModifiedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._sizeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._lengthCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._incomingFilesLabel = new System.Windows.Forms.Label();
			this._instructionsLabel = new System.Windows.Forms.Label();
			this._filesPanel = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this._filesGrid)).BeginInit();
			this._filesPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _findFilesLink
			// 
			this._findFilesLink.AutoSize = true;
			this._findFilesLink.BackColor = System.Drawing.Color.Transparent;
			this._findFilesLink.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._findFilesLink.Location = new System.Drawing.Point(18, 25);
			this._findFilesLink.Name = "_findFilesLink";
			this._findFilesLink.Size = new System.Drawing.Size(162, 15);
			this._findFilesLink.TabIndex = 0;
			this._findFilesLink.TabStop = true;
			this._findFilesLink.Text = "Find Files on Recorder/Card...";
			this._findFilesLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleFindFilesLinkClicked);
			// 
			// _createSessionsButton
			// 
			this._createSessionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._createSessionsButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._createSessionsButton.Location = new System.Drawing.Point(323, 301);
			this._createSessionsButton.Name = "_createSessionsButton";
			this._createSessionsButton.Size = new System.Drawing.Size(142, 26);
			this._createSessionsButton.TabIndex = 1;
			this._createSessionsButton.Text = "Create {0} Sessions";
			this._createSessionsButton.UseVisualStyleBackColor = true;
			this._createSessionsButton.Click += new System.EventHandler(this.HandleCreateSessionsButtonClick);
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point(471, 301);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(80, 26);
			this._cancelButton.TabIndex = 2;
			this._cancelButton.Text = "Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// _sourceFolderLabel
			// 
			this._sourceFolderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._sourceFolderLabel.AutoEllipsis = true;
			this._sourceFolderLabel.BackColor = System.Drawing.Color.Transparent;
			this._sourceFolderLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._sourceFolderLabel.Location = new System.Drawing.Point(204, 25);
			this._sourceFolderLabel.Name = "_sourceFolderLabel";
			this._sourceFolderLabel.Size = new System.Drawing.Size(347, 23);
			this._sourceFolderLabel.TabIndex = 3;
			this._sourceFolderLabel.Text = "#";
			// 
			// _filesGrid
			// 
			this._filesGrid.AllowUserToAddRows = false;
			this._filesGrid.AllowUserToDeleteRows = false;
			this._filesGrid.AllowUserToOrderColumns = true;
			this._filesGrid.AllowUserToResizeRows = false;
			this._filesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._filesGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this._filesGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this._filesGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this._filesGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this._filesGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this._filesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this._filesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._selectedCol,
            this._iconCol,
            this._fileNameCol,
            this._fileTypeCol,
            this._dateModifiedCol,
            this._sizeCol,
            this._lengthCol});
			this._filesGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this._filesGrid.IsDirty = false;
			this._filesGrid.Location = new System.Drawing.Point(0, 22);
			this._filesGrid.MultiSelect = false;
			this._filesGrid.Name = "_filesGrid";
			this._filesGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this._filesGrid.RowHeadersVisible = false;
			this._filesGrid.RowHeadersWidth = 22;
			this._filesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this._filesGrid.ShowWaterMarkWhenDirty = false;
			this._filesGrid.Size = new System.Drawing.Size(536, 165);
			this._filesGrid.TabIndex = 4;
			this._filesGrid.VirtualMode = true;
			this._filesGrid.WaterMark = "!";
			this._filesGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleFilesGridCellContentClick);
			// 
			// _selectedCol
			// 
			this._selectedCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this._selectedCol.DataPropertyName = "Selected";
			this._selectedCol.HeaderText = "";
			this._selectedCol.Name = "_selectedCol";
			this._selectedCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this._selectedCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this._selectedCol.Width = 19;
			// 
			// _iconCol
			// 
			this._iconCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this._iconCol.DataPropertyName = "SmallIcon";
			this._iconCol.HeaderText = "";
			this._iconCol.Name = "_iconCol";
			this._iconCol.ReadOnly = true;
			this._iconCol.Width = 5;
			// 
			// _fileNameCol
			// 
			this._fileNameCol.DataPropertyName = "FileName";
			this._fileNameCol.HeaderText = "Name";
			this._fileNameCol.Name = "_fileNameCol";
			this._fileNameCol.ReadOnly = true;
			this._fileNameCol.Width = 175;
			// 
			// _fileTypeCol
			// 
			this._fileTypeCol.DataPropertyName = "FileType";
			this._fileTypeCol.HeaderText = "Type";
			this._fileTypeCol.Name = "_fileTypeCol";
			this._fileTypeCol.ReadOnly = true;
			this._fileTypeCol.Width = 150;
			// 
			// _dateModifiedCol
			// 
			this._dateModifiedCol.DataPropertyName = "DateModified";
			this._dateModifiedCol.HeaderText = "Date Modified";
			this._dateModifiedCol.Name = "_dateModifiedCol";
			this._dateModifiedCol.ReadOnly = true;
			this._dateModifiedCol.Width = 150;
			// 
			// _sizeCol
			// 
			this._sizeCol.DataPropertyName = "FileSize";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this._sizeCol.DefaultCellStyle = dataGridViewCellStyle2;
			this._sizeCol.HeaderText = "Size";
			this._sizeCol.Name = "_sizeCol";
			this._sizeCol.ReadOnly = true;
			this._sizeCol.Width = 75;
			// 
			// _lengthCol
			// 
			this._lengthCol.DataPropertyName = "DisplayableDuration";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this._lengthCol.DefaultCellStyle = dataGridViewCellStyle3;
			this._lengthCol.HeaderText = "Length";
			this._lengthCol.Name = "_lengthCol";
			this._lengthCol.ReadOnly = true;
			this._lengthCol.Width = 60;
			// 
			// _incomingFilesLabel
			// 
			this._incomingFilesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._incomingFilesLabel.AutoSize = true;
			this._incomingFilesLabel.BackColor = System.Drawing.Color.Transparent;
			this._incomingFilesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._incomingFilesLabel.Location = new System.Drawing.Point(3, 0);
			this._incomingFilesLabel.Name = "_incomingFilesLabel";
			this._incomingFilesLabel.Size = new System.Drawing.Size(84, 15);
			this._incomingFilesLabel.TabIndex = 5;
			this._incomingFilesLabel.Text = "Incoming Files";
			// 
			// _instructionsLabel
			// 
			this._instructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._instructionsLabel.AutoEllipsis = true;
			this._instructionsLabel.BackColor = System.Drawing.Color.Transparent;
			this._instructionsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._instructionsLabel.Location = new System.Drawing.Point(3, 194);
			this._instructionsLabel.Name = "_instructionsLabel";
			this._instructionsLabel.Size = new System.Drawing.Size(532, 41);
			this._instructionsLabel.TabIndex = 6;
			this._instructionsLabel.Text = "Mark each file which represents an original recording of an event. For each one, " +
				"{0} will create a new session and copy the file into it.";
			// 
			// _filesPanel
			// 
			this._filesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._filesPanel.Controls.Add(this._incomingFilesLabel);
			this._filesPanel.Controls.Add(this._instructionsLabel);
			this._filesPanel.Controls.Add(this._filesGrid);
			this._filesPanel.Location = new System.Drawing.Point(15, 58);
			this._filesPanel.Name = "_filesPanel";
			this._filesPanel.Size = new System.Drawing.Size(536, 237);
			this._filesPanel.TabIndex = 7;
			// 
			// NewSessionsFromFilesDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(565, 342);
			this.Controls.Add(this._filesPanel);
			this.Controls.Add(this._sourceFolderLabel);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._createSessionsButton);
			this.Controls.Add(this._findFilesLink);
			this.Name = "NewSessionsFromFilesDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "New Sessions From Files";
			((System.ComponentModel.ISupportInitialize)(this._filesGrid)).EndInit();
			this._filesPanel.ResumeLayout(false);
			this._filesPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.LinkLabel _findFilesLink;
		private System.Windows.Forms.Button _createSessionsButton;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.Label _sourceFolderLabel;
		private SilUtils.SilGrid _filesGrid;
		private System.Windows.Forms.Label _incomingFilesLabel;
		private System.Windows.Forms.Label _instructionsLabel;
		private System.Windows.Forms.Panel _filesPanel;
		private System.Windows.Forms.DataGridViewCheckBoxColumn _selectedCol;
		private System.Windows.Forms.DataGridViewImageColumn _iconCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn _fileNameCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn _fileTypeCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn _dateModifiedCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn _sizeCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn _lengthCol;
	}
}