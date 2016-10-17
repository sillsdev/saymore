namespace SayMore.UI
{
	partial class FileLoadErrorsReportDlg
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
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.m_btnSave = new System.Windows.Forms.Button();
			this.m_btnContinue = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.m_btnExit = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.m_listFileErrors = new System.Windows.Forms.ListView();
			this.colHeaderFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colHeaderError = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openContainingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label1.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
			this.label1.Location = new System.Drawing.Point(46, 4);
			this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(271, 26);
			this.label1.TabIndex = 0;
			this.label1.Text = "This project contains one or more files that could not be loaded.";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.m_btnSave, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.m_btnContinue, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.m_btnExit, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.m_listFileErrors, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(325, 318);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::SayMore.Properties.Resources.kimidWarning;
			this.pictureBox1.Location = new System.Drawing.Point(3, 3);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// m_btnSave
			// 
			this.m_btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnSave.AutoSize = true;
			this.m_btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_btnSave.Location = new System.Drawing.Point(238, 169);
			this.m_btnSave.Name = "m_btnSave";
			this.m_btnSave.Size = new System.Drawing.Size(84, 23);
			this.m_btnSave.TabIndex = 5;
			this.m_btnSave.Text = "Save to Log...";
			this.m_btnSave.UseVisualStyleBackColor = true;
			this.m_btnSave.Click += new System.EventHandler(this.m_btnSave_Click);
			// 
			// m_btnContinue
			// 
			this.m_btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnContinue.AutoSize = true;
			this.m_btnContinue.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_btnContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_btnContinue.Location = new System.Drawing.Point(238, 219);
			this.m_btnContinue.Name = "m_btnContinue";
			this.m_btnContinue.Size = new System.Drawing.Size(84, 23);
			this.m_btnContinue.TabIndex = 2;
			this.m_btnContinue.Text = "Continue";
			this.m_btnContinue.UseVisualStyleBackColor = true;
			this.m_btnContinue.Click += new System.EventHandler(this.m_btnContinue_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
			this.label2.Location = new System.Drawing.Point(3, 216);
			this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(217, 26);
			this.label2.TabIndex = 3;
			this.label2.Text = "Ignore these problems and continue to work with this project without the problem " +
    "files.";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.label3, 2);
			this.label3.Location = new System.Drawing.Point(3, 166);
			this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(214, 39);
			this.label3.TabIndex = 6;
			this.label3.Text = "Save these error messages in a log fiile that you can use to track down the probl" +
    "ems or request technical support.";
			// 
			// m_btnExit
			// 
			this.m_btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_btnExit.AutoSize = true;
			this.m_btnExit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_btnExit.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.m_btnExit.Location = new System.Drawing.Point(238, 269);
			this.m_btnExit.Name = "m_btnExit";
			this.m_btnExit.Size = new System.Drawing.Size(84, 23);
			this.m_btnExit.TabIndex = 7;
			this.m_btnExit.Text = "Exit";
			this.m_btnExit.UseVisualStyleBackColor = true;
			this.m_btnExit.Click += new System.EventHandler(this.m_btnExit_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.label4, 2);
			this.label4.Location = new System.Drawing.Point(3, 266);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(220, 52);
			this.label4.TabIndex = 8;
			this.label4.Text = "To restore the problem files from a backup or fix them manually, exit SayMore, ta" +
    "ke the necessary action, and then re-open this project.";
			// 
			// m_listFileErrors
			// 
			this.m_listFileErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.m_listFileErrors.AutoArrange = false;
			this.m_listFileErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHeaderFile,
            this.colHeaderError});
			this.tableLayoutPanel1.SetColumnSpan(this.m_listFileErrors, 3);
			this.m_listFileErrors.ContextMenuStrip = this.contextMenuStrip1;
			this.m_listFileErrors.HideSelection = false;
			this.m_listFileErrors.Location = new System.Drawing.Point(3, 48);
			this.m_listFileErrors.Name = "m_listFileErrors";
			this.m_listFileErrors.ShowGroups = false;
			this.m_listFileErrors.Size = new System.Drawing.Size(319, 115);
			this.m_listFileErrors.TabIndex = 9;
			this.m_listFileErrors.UseCompatibleStateImageBehavior = false;
			this.m_listFileErrors.View = System.Windows.Forms.View.Details;
			this.m_listFileErrors.Resize += new System.EventHandler(this.m_listFileErrors_Resize);
			// 
			// colHeaderFile
			// 
			this.colHeaderFile.Text = "File Error";
			this.colHeaderFile.Width = 157;
			// 
			// colHeaderError
			// 
			this.colHeaderError.Text = "Details";
			this.colHeaderError.Width = 158;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openContainingFolderToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(211, 26);
			// 
			// openContainingFolderToolStripMenuItem
			// 
			this.openContainingFolderToolStripMenuItem.Name = "openContainingFolderToolStripMenuItem";
			this.openContainingFolderToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.openContainingFolderToolStripMenuItem.Text = "Open Containing Folder...";
			// 
			// FileLoadErrorsReportDlg
			// 
			this.AcceptButton = this.m_btnContinue;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.m_btnExit;
			this.ClientSize = new System.Drawing.Size(345, 338);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MinimumSize = new System.Drawing.Size(361, 376);
			this.Name = "FileLoadErrorsReportDlg";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.Text = "FileLoadErrorsReportDlg";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button m_btnContinue;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem openContainingFolderToolStripMenuItem;
		private System.Windows.Forms.Button m_btnSave;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button m_btnExit;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListView m_listFileErrors;
		private System.Windows.Forms.ColumnHeader colHeaderFile;
		private System.Windows.Forms.ColumnHeader colHeaderError;
	}
}