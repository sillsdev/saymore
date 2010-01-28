namespace SIL.Sponge.ConfigTools
{
	partial class NewSessionDlg
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
			this.lblPath = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblMsg = new System.Windows.Forms.Label();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.btnCopyFiles = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// lblPath
			// 
			this.lblPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblPath.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.lblPath.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.lblPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblPath, null);
			this.locExtender.SetLocalizationComment(this.lblPath, "This text is displayed under the project name and includes where it will be creat" +
					"ed.");
			this.locExtender.SetLocalizingId(this.lblPath, "NewSessionDlg.lblPath");
			this.lblPath.Location = new System.Drawing.Point(12, 73);
			this.lblPath.Name = "lblPath";
			this.lblPath.Size = new System.Drawing.Size(335, 39);
			this.lblPath.TabIndex = 2;
			this.lblPath.Text = "Session will be created in: {0}";
			this.lblPath.TextChanged += new System.EventHandler(this.lblPath_TextChanged);
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.locExtender.SetLocalizableToolTip(this.txtName, null);
			this.locExtender.SetLocalizationComment(this.txtName, null);
			this.locExtender.SetLocalizingId(this.txtName, "NewSessionDlg.txtSessionName");
			this.txtName.Location = new System.Drawing.Point(12, 48);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(337, 21);
			this.txtName.TabIndex = 1;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnCancel, null);
			this.locExtender.SetLocalizationComment(this.btnCancel, null);
			this.locExtender.SetLocalizingId(this.btnCancel, "NewSessionDlg.btnCancel");
			this.btnCancel.Location = new System.Drawing.Point(267, 166);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 26);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.btnOK, null);
			this.locExtender.SetLocalizationComment(this.btnOK, null);
			this.locExtender.SetLocalizingId(this.btnOK, "NewSessionDlg.btnOK");
			this.btnOK.Location = new System.Drawing.Point(181, 166);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 26);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// lblMsg
			// 
			this.lblMsg.AutoSize = true;
			this.lblMsg.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
			this.lblMsg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this.lblMsg, null);
			this.locExtender.SetLocalizationComment(this.lblMsg, null);
			this.locExtender.SetLocalizingId(this.lblMsg, "NewSessionDlg.lblMsg");
			this.lblMsg.Location = new System.Drawing.Point(12, 23);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(260, 17);
			this.lblMsg.TabIndex = 0;
			this.lblMsg.Text = "What would you like to call this session?";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// btnCopyFiles
			// 
			this.locExtender.SetLocalizableToolTip(this.btnCopyFiles, null);
			this.locExtender.SetLocalizationComment(this.btnCopyFiles, null);
			this.locExtender.SetLocalizationPriority(this.btnCopyFiles, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnCopyFiles, "NewSessionDlg.btnCopyFiles");
			this.btnCopyFiles.Location = new System.Drawing.Point(181, 124);
			this.btnCopyFiles.Name = "btnCopyFiles";
			this.btnCopyFiles.Size = new System.Drawing.Size(166, 26);
			this.btnCopyFiles.TabIndex = 3;
			this.btnCopyFiles.Text = "Copy Files into the Session...";
			this.btnCopyFiles.UseVisualStyleBackColor = true;
			this.btnCopyFiles.Click += new System.EventHandler(this.btnCopyFiles_Click);
			// 
			// NewSessionDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(359, 204);
			this.Controls.Add(this.btnCopyFiles);
			this.Controls.Add(this.lblPath);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblMsg);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "NewSessionDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewSessionDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Session";
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected System.Windows.Forms.Label lblPath;
		protected System.Windows.Forms.TextBox txtName;
		protected System.Windows.Forms.Button btnCancel;
		protected System.Windows.Forms.Button btnOK;
		protected System.Windows.Forms.Label lblMsg;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button btnCopyFiles;
	}
}