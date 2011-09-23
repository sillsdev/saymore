using Localization.UI;

namespace SayMore.UI.ProjectChoosingAndCreating.NewProjectDialog
{
	partial class NewProjectDlg
	{

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
			this._labelMessage = new System.Windows.Forms.Label();
			this._buttonOK = new System.Windows.Forms.Button();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._textBoxName = new System.Windows.Forms.TextBox();
			this._labelNewProjectPath = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _labelMessage
			// 
			this._labelMessage.AutoSize = true;
			this._labelMessage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
			this.locExtender.SetLocalizableToolTip(this._labelMessage, null);
			this.locExtender.SetLocalizationComment(this._labelMessage, null);
			this.locExtender.SetLocalizingId(this._labelMessage, "NewProjectDialog._labelMessage");
			this._labelMessage.Location = new System.Drawing.Point(12, 22);
			this._labelMessage.Name = "_labelMessage";
			this._labelMessage.Size = new System.Drawing.Size(258, 17);
			this._labelMessage.TabIndex = 0;
			this._labelMessage.Text = "What would you like to call this project?";
			// 
			// _buttonOK
			// 
			this._buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._buttonOK, null);
			this.locExtender.SetLocalizationComment(this._buttonOK, null);
			this.locExtender.SetLocalizingId(this._buttonOK, "NewProjectDialog._buttonOK");
			this._buttonOK.Location = new System.Drawing.Point(181, 120);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(80, 26);
			this._buttonOK.TabIndex = 3;
			this._buttonOK.Text = "&OK";
			this._buttonOK.UseVisualStyleBackColor = true;
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "NewProjectDialog._buttonCancel");
			this._buttonCancel.Location = new System.Drawing.Point(267, 120);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(80, 26);
			this._buttonCancel.TabIndex = 4;
			this._buttonCancel.Text = "&Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _textBoxName
			// 
			this._textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._textBoxName.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.locExtender.SetLocalizableToolTip(this._textBoxName, null);
			this.locExtender.SetLocalizationComment(this._textBoxName, null);
			this.locExtender.SetLocalizationPriority(this._textBoxName, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._textBoxName, "NewProjectDlg._textBoxName");
			this._textBoxName.Location = new System.Drawing.Point(12, 46);
			this._textBoxName.Name = "_textBoxName";
			this._textBoxName.Size = new System.Drawing.Size(335, 23);
			this._textBoxName.TabIndex = 1;
			this._textBoxName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// _labelNewProjectPath
			// 
			this._labelNewProjectPath.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this._labelNewProjectPath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelNewProjectPath.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this._labelNewProjectPath.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.locExtender.SetLocalizableToolTip(this._labelNewProjectPath, null);
			this.locExtender.SetLocalizationComment(this._labelNewProjectPath, "This text is displayed under the project name and includes where it will be creat" +
        "ed.");
			this.locExtender.SetLocalizingId(this._labelNewProjectPath, "NewProjectDialog._labelNewProjectPath");
			this._labelNewProjectPath.Location = new System.Drawing.Point(12, 73);
			this._labelNewProjectPath.Name = "_labelNewProjectPath";
			this._labelNewProjectPath.Size = new System.Drawing.Size(335, 44);
			this._labelNewProjectPath.TabIndex = 2;
			this._labelNewProjectPath.Text = "Project will be created in: {0}";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// NewProjectDlg
			// 
			this.AcceptButton = this._buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(359, 158);
			this.Controls.Add(this._labelNewProjectPath);
			this.Controls.Add(this._textBoxName);
			this.Controls.Add(this._buttonCancel);
			this.Controls.Add(this._buttonOK);
			this.Controls.Add(this._labelMessage);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "NewProjectDialog.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewProjectDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New SayMore Project";
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected System.Windows.Forms.Label _labelMessage;
		protected System.Windows.Forms.Button _buttonOK;
		protected System.Windows.Forms.Button _buttonCancel;
		protected System.Windows.Forms.TextBox _textBoxName;
		protected System.Windows.Forms.Label _labelNewProjectPath;
		private LocalizationExtender locExtender;
		private System.ComponentModel.IContainer components;
	}
}