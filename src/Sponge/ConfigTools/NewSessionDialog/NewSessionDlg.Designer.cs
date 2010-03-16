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
			this._newSessionPathLabel = new System.Windows.Forms.Label();
			this._idTextBox = new System.Windows.Forms.TextBox();
			this._cancelButton = new System.Windows.Forms.Button();
			this._OKButton = new System.Windows.Forms.Button();
			this._messageLabel = new System.Windows.Forms.Label();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this._copyFilesButton = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _newSessionPathLabel
			// 
			this._newSessionPathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._newSessionPathLabel.AutoEllipsis = true;
			this._newSessionPathLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this._newSessionPathLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this._newSessionPathLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._newSessionPathLabel, null);
			this.locExtender.SetLocalizationComment(this._newSessionPathLabel, "This text is displayed under the project name and includes where it will be creat" +
					"ed.");
			this.locExtender.SetLocalizingId(this._newSessionPathLabel, "NewSessionDlg.lblPath");
			this._newSessionPathLabel.Location = new System.Drawing.Point(12, 75);
			this._newSessionPathLabel.Name = "_newSessionPathLabel";
			this._newSessionPathLabel.Size = new System.Drawing.Size(335, 46);
			this._newSessionPathLabel.TabIndex = 2;
			this._newSessionPathLabel.Text = "Session will be created in: {0}";
			// 
			// _idTextBox
			// 
			this._idTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._idTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._idTextBox, null);
			this.locExtender.SetLocalizationComment(this._idTextBox, null);
			this.locExtender.SetLocalizingId(this._idTextBox, "NewSessionDlg.txtSessionName");
			this._idTextBox.Location = new System.Drawing.Point(12, 47);
			this._idTextBox.Name = "_idTextBox";
			this._idTextBox.Size = new System.Drawing.Size(337, 23);
			this._idTextBox.TabIndex = 1;
			this._idTextBox.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._cancelButton, null);
			this.locExtender.SetLocalizationComment(this._cancelButton, null);
			this.locExtender.SetLocalizingId(this._cancelButton, "NewSessionDlg.btnCancel");
			this._cancelButton.Location = new System.Drawing.Point(267, 166);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(80, 26);
			this._cancelButton.TabIndex = 5;
			this._cancelButton.Text = "&Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// _OKButton
			// 
			this._OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._OKButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._OKButton, null);
			this.locExtender.SetLocalizationComment(this._OKButton, null);
			this.locExtender.SetLocalizingId(this._OKButton, "NewSessionDlg.btnOK");
			this._OKButton.Location = new System.Drawing.Point(181, 166);
			this._OKButton.Name = "_OKButton";
			this._OKButton.Size = new System.Drawing.Size(80, 26);
			this._OKButton.TabIndex = 4;
			this._OKButton.Text = "&OK";
			this._OKButton.UseVisualStyleBackColor = true;
			// 
			// _messageLabel
			// 
			this._messageLabel.AutoSize = true;
			this._messageLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
			this._messageLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._messageLabel, null);
			this.locExtender.SetLocalizationComment(this._messageLabel, null);
			this.locExtender.SetLocalizingId(this._messageLabel, "NewSessionDlg.lblMsg");
			this._messageLabel.Location = new System.Drawing.Point(12, 23);
			this._messageLabel.Name = "_messageLabel";
			this._messageLabel.Size = new System.Drawing.Size(260, 17);
			this._messageLabel.TabIndex = 0;
			this._messageLabel.Text = "What would you like to call this session?";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// _copyFilesButton
			// 
			this.locExtender.SetLocalizableToolTip(this._copyFilesButton, null);
			this.locExtender.SetLocalizationComment(this._copyFilesButton, null);
			this.locExtender.SetLocalizationPriority(this._copyFilesButton, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._copyFilesButton, "NewSessionDlg.btnCopyFiles");
			this._copyFilesButton.Location = new System.Drawing.Point(181, 124);
			this._copyFilesButton.Name = "_copyFilesButton";
			this._copyFilesButton.Size = new System.Drawing.Size(166, 26);
			this._copyFilesButton.TabIndex = 3;
			this._copyFilesButton.Text = "Copy Files into the Session...";
			this._copyFilesButton.UseVisualStyleBackColor = true;
			this._copyFilesButton.Click += new System.EventHandler(this.btnCopyFiles_Click);
			// 
			// NewSessionDlg
			// 
			this.AcceptButton = this._OKButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(359, 204);
			this.Controls.Add(this._copyFilesButton);
			this.Controls.Add(this._newSessionPathLabel);
			this.Controls.Add(this._idTextBox);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._OKButton);
			this.Controls.Add(this._messageLabel);
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

		protected System.Windows.Forms.Label _newSessionPathLabel;
		protected System.Windows.Forms.TextBox _idTextBox;
		protected System.Windows.Forms.Button _cancelButton;
		protected System.Windows.Forms.Button _OKButton;
		protected System.Windows.Forms.Label _messageLabel;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button _copyFilesButton;
	}
}