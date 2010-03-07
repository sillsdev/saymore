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
			this.m_findFilesLink = new System.Windows.Forms.LinkLabel();
			this.m_createSessionsButton = new System.Windows.Forms.Button();
			this.m_cancelButton = new System.Windows.Forms.Button();
			this.m_sourceFolderLabel = new System.Windows.Forms.Label();
			this.m_filesGrid = new SilUtils.SilGrid();
			this.m_selectedCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.m_iconCol = new System.Windows.Forms.DataGridViewImageColumn();
			this.m_fileNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_fileTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_dateModifiedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_sizeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_lengthCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.m_incomingFilesLabel = new System.Windows.Forms.Label();
			this.m_instructionsLabel = new System.Windows.Forms.Label();
			this.m_filesPanel = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.m_filesGrid)).BeginInit();
			this.m_filesPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_findFilesLink
			// 
			this.m_findFilesLink.AutoSize = true;
			this.m_findFilesLink.BackColor = System.Drawing.Color.Transparent;
			this.m_findFilesLink.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_findFilesLink.Location = new System.Drawing.Point(18, 25);
			this.m_findFilesLink.Name = "m_findFilesLink";
			this.m_findFilesLink.Size = new System.Drawing.Size(162, 15);
			this.m_findFilesLink.TabIndex = 0;
			this.m_findFilesLink.TabStop = true;
			this.m_findFilesLink.Text = "Find Files on Recorder/Card...";
			this.m_findFilesLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleFindFilesLinkClicked);
			// 
			// m_createSessionsButton
			// 
			this.m_createSessionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_createSessionsButton.Location = new System.Drawing.Point(323, 301);
			this.m_createSessionsButton.Name = "m_createSessionsButton";
			this.m_createSessionsButton.Size = new System.Drawing.Size(142, 26);
			this.m_createSessionsButton.TabIndex = 1;
			this.m_createSessionsButton.Text = "Create {0} Sessions";
			this.m_createSessionsButton.UseVisualStyleBackColor = true;
			this.m_createSessionsButton.Click += new System.EventHandler(this.HandleCreateSessionsButtonClick);
			// 
			// m_cancelButton
			// 
			this.m_cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_cancelButton.Location = new System.Drawing.Point(471, 301);
			this.m_cancelButton.Name = "m_cancelButton";
			this.m_cancelButton.Size = new System.Drawing.Size(80, 26);
			this.m_cancelButton.TabIndex = 2;
			this.m_cancelButton.Text = "Cancel";
			this.m_cancelButton.UseVisualStyleBackColor = true;
			// 
			// m_sourceFolderLabel
			// 
			this.m_sourceFolderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_sourceFolderLabel.AutoEllipsis = true;
			this.m_sourceFolderLabel.BackColor = System.Drawing.Color.Transparent;
			this.m_sourceFolderLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_sourceFolderLabel.Location = new System.Drawing.Point(204, 25);
			this.m_sourceFolderLabel.Name = "m_sourceFolderLabel";
			this.m_sourceFolderLabel.Size = new System.Drawing.Size(347, 23);
			this.m_sourceFolderLabel.TabIndex = 3;
			this.m_sourceFolderLabel.Text = "#";
			// 
			// m_filesGrid
			// 
			this.m_filesGrid.AllowUserToAddRows = false;
			this.m_filesGrid.AllowUserToDeleteRows = false;
			this.m_filesGrid.AllowUserToOrderColumns = true;
			this.m_filesGrid.AllowUserToResizeRows = false;
			this.m_filesGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_filesGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.m_filesGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.m_filesGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.m_filesGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_filesGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.m_filesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.m_filesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_selectedCol,
            this.m_iconCol,
            this.m_fileNameCol,
            this.m_fileTypeCol,
            this.m_dateModifiedCol,
            this.m_sizeCol,
            this.m_lengthCol});
			this.m_filesGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
			this.m_filesGrid.IsDirty = false;
			this.m_filesGrid.Location = new System.Drawing.Point(0, 22);
			this.m_filesGrid.MultiSelect = false;
			this.m_filesGrid.Name = "m_filesGrid";
			this.m_filesGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.m_filesGrid.RowHeadersVisible = false;
			this.m_filesGrid.RowHeadersWidth = 22;
			this.m_filesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.m_filesGrid.ShowWaterMarkWhenDirty = false;
			this.m_filesGrid.Size = new System.Drawing.Size(536, 165);
			this.m_filesGrid.TabIndex = 4;
			this.m_filesGrid.VirtualMode = true;
			this.m_filesGrid.WaterMark = "!";
			this.m_filesGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleFilesGridCellContentClick);
			// 
			// m_selectedCol
			// 
			this.m_selectedCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.m_selectedCol.HeaderText = "";
			this.m_selectedCol.Name = "m_selectedCol";
			this.m_selectedCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.m_selectedCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.m_selectedCol.Width = 19;
			// 
			// m_iconCol
			// 
			this.m_iconCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.m_iconCol.HeaderText = "";
			this.m_iconCol.Name = "m_iconCol";
			this.m_iconCol.ReadOnly = true;
			this.m_iconCol.Width = 5;
			// 
			// m_fileNameCol
			// 
			this.m_fileNameCol.HeaderText = "Name";
			this.m_fileNameCol.Name = "m_fileNameCol";
			this.m_fileNameCol.ReadOnly = true;
			this.m_fileNameCol.Width = 175;
			// 
			// m_fileTypeCol
			// 
			this.m_fileTypeCol.HeaderText = "Type";
			this.m_fileTypeCol.Name = "m_fileTypeCol";
			this.m_fileTypeCol.ReadOnly = true;
			this.m_fileTypeCol.Width = 150;
			// 
			// m_dateModifiedCol
			// 
			this.m_dateModifiedCol.HeaderText = "Date Modified";
			this.m_dateModifiedCol.Name = "m_dateModifiedCol";
			this.m_dateModifiedCol.ReadOnly = true;
			this.m_dateModifiedCol.Width = 150;
			// 
			// m_sizeCol
			// 
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.m_sizeCol.DefaultCellStyle = dataGridViewCellStyle2;
			this.m_sizeCol.HeaderText = "Size";
			this.m_sizeCol.Name = "m_sizeCol";
			this.m_sizeCol.ReadOnly = true;
			this.m_sizeCol.Width = 75;
			// 
			// m_lengthCol
			// 
			this.m_lengthCol.HeaderText = "Length";
			this.m_lengthCol.Name = "m_lengthCol";
			this.m_lengthCol.ReadOnly = true;
			this.m_lengthCol.Width = 60;
			// 
			// m_incomingFilesLabel
			// 
			this.m_incomingFilesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_incomingFilesLabel.AutoSize = true;
			this.m_incomingFilesLabel.BackColor = System.Drawing.Color.Transparent;
			this.m_incomingFilesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_incomingFilesLabel.Location = new System.Drawing.Point(3, 0);
			this.m_incomingFilesLabel.Name = "m_incomingFilesLabel";
			this.m_incomingFilesLabel.Size = new System.Drawing.Size(84, 15);
			this.m_incomingFilesLabel.TabIndex = 5;
			this.m_incomingFilesLabel.Text = "Incoming Files";
			// 
			// m_instructionsLabel
			// 
			this.m_instructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_instructionsLabel.AutoEllipsis = true;
			this.m_instructionsLabel.BackColor = System.Drawing.Color.Transparent;
			this.m_instructionsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_instructionsLabel.Location = new System.Drawing.Point(3, 194);
			this.m_instructionsLabel.Name = "m_instructionsLabel";
			this.m_instructionsLabel.Size = new System.Drawing.Size(532, 41);
			this.m_instructionsLabel.TabIndex = 6;
			this.m_instructionsLabel.Text = "Mark each file which represents an original recording of an event. For each one, " +
				"{0} will create a new session and copy the file into it.";
			// 
			// m_filesPanel
			// 
			this.m_filesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_filesPanel.Controls.Add(this.m_incomingFilesLabel);
			this.m_filesPanel.Controls.Add(this.m_instructionsLabel);
			this.m_filesPanel.Controls.Add(this.m_filesGrid);
			this.m_filesPanel.Location = new System.Drawing.Point(15, 58);
			this.m_filesPanel.Name = "m_filesPanel";
			this.m_filesPanel.Size = new System.Drawing.Size(536, 237);
			this.m_filesPanel.TabIndex = 7;
			// 
			// NewSessionsFromFilesDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(565, 342);
			this.Controls.Add(this.m_filesPanel);
			this.Controls.Add(this.m_sourceFolderLabel);
			this.Controls.Add(this.m_cancelButton);
			this.Controls.Add(this.m_createSessionsButton);
			this.Controls.Add(this.m_findFilesLink);
			this.Name = "NewSessionsFromFilesDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "New Sessions From Files";
			((System.ComponentModel.ISupportInitialize)(this.m_filesGrid)).EndInit();
			this.m_filesPanel.ResumeLayout(false);
			this.m_filesPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.LinkLabel m_findFilesLink;
		private System.Windows.Forms.Button m_createSessionsButton;
		private System.Windows.Forms.Button m_cancelButton;
		private System.Windows.Forms.Label m_sourceFolderLabel;
		private SilUtils.SilGrid m_filesGrid;
		private System.Windows.Forms.Label m_incomingFilesLabel;
		private System.Windows.Forms.Label m_instructionsLabel;
		private System.Windows.Forms.Panel m_filesPanel;
		private System.Windows.Forms.DataGridViewCheckBoxColumn m_selectedCol;
		private System.Windows.Forms.DataGridViewImageColumn m_iconCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn m_fileNameCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn m_fileTypeCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn m_dateModifiedCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn m_sizeCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn m_lengthCol;
	}
}