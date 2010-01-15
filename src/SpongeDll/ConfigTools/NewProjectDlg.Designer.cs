namespace SIL.Sponge.ConfigTools
{
    partial class NewProjectDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        protected readonly System.ComponentModel.IContainer components = null;

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
			this.lblMsg = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtProjectName = new System.Windows.Forms.TextBox();
			this.lblPath = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblMsg
			// 
			this.lblMsg.AutoSize = true;
			this.lblMsg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMsg.Location = new System.Drawing.Point(12, 23);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(227, 15);
			this.lblMsg.TabIndex = 0;
			this.lblMsg.Text = "What would you like to call this project?";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(181, 120);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 26);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(267, 120);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 26);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// txtProjectName
			// 
			this.txtProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProjectName.Location = new System.Drawing.Point(12, 48);
			this.txtProjectName.Name = "txtProjectName";
			this.txtProjectName.Size = new System.Drawing.Size(335, 20);
			this.txtProjectName.TabIndex = 1;
			this.txtProjectName.TextChanged += new System.EventHandler(this.txtProjectName_TextChanged);
			// 
			// lblPath
			// 
			this.lblPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPath.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.lblPath.Location = new System.Drawing.Point(12, 71);
			this.lblPath.Name = "lblPath";
			this.lblPath.Size = new System.Drawing.Size(335, 44);
			this.lblPath.TabIndex = 2;
			this.lblPath.Text = "#";
			// 
			// NewProjectDlg
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(359, 158);
			this.Controls.Add(this.lblPath);
			this.Controls.Add(this.txtProjectName);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblMsg);
			this.DoubleBuffered = true;
			this.Name = "NewProjectDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "New Sponge Project";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.Label lblMsg;
        protected System.Windows.Forms.Button btnOK;
        protected System.Windows.Forms.Button btnCancel;
        protected System.Windows.Forms.TextBox txtProjectName;
        protected System.Windows.Forms.Label lblPath;
    }
}