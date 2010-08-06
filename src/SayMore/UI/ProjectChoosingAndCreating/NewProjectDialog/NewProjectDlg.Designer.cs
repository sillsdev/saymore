using SIL.Localization;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectDlg));
			this._messageLabel = new System.Windows.Forms.Label();
			this._buttonOK = new System.Windows.Forms.Button();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._nameTextBox = new System.Windows.Forms.TextBox();
			this._newProjectPathLabel = new System.Windows.Forms.Label();
			this.locExtender = new SIL.Localization.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _messageLabel
			// 
			resources.ApplyResources(this._messageLabel, "_messageLabel");
			this.locExtender.SetLocalizableToolTip(this._messageLabel, null);
			this.locExtender.SetLocalizationComment(this._messageLabel, null);
			this.locExtender.SetLocalizingId(this._messageLabel, "NewProjectDlg._messageLabel");
			this._messageLabel.Name = "_messageLabel";
			// 
			// _buttonOK
			// 
			resources.ApplyResources(this._buttonOK, "_buttonOK");
			this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._buttonOK, null);
			this.locExtender.SetLocalizationComment(this._buttonOK, null);
			this.locExtender.SetLocalizingId(this._buttonOK, "NewProjectDlg._buttonOK");
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.UseVisualStyleBackColor = true;
			// 
			// _buttonCancel
			// 
			resources.ApplyResources(this._buttonCancel, "_buttonCancel");
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "NewProjectDlg._buttonCancel");
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _nameTextBox
			// 
			resources.ApplyResources(this._nameTextBox, "_nameTextBox");
			this.locExtender.SetLocalizableToolTip(this._nameTextBox, null);
			this.locExtender.SetLocalizationComment(this._nameTextBox, null);
			this.locExtender.SetLocalizationPriority(this._nameTextBox, SIL.Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._nameTextBox, "NewProjectDlg._newNameTextBox");
			this._nameTextBox.Name = "_nameTextBox";
			this._nameTextBox.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// _newProjectPathLabel
			// 
			this._newProjectPathLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			resources.ApplyResources(this._newProjectPathLabel, "_newProjectPathLabel");
			this._newProjectPathLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.locExtender.SetLocalizableToolTip(this._newProjectPathLabel, null);
			this.locExtender.SetLocalizationComment(this._newProjectPathLabel, "This text is displayed under the project name and includes where it will be creat" +
					"ed.");
			this.locExtender.SetLocalizingId(this._newProjectPathLabel, "NewProjectDlg._newProjectPathLabel");
			this._newProjectPathLabel.Name = "_newProjectPathLabel";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Dialog Boxes";
			// 
			// NewProjectDlg
			// 
			this.AcceptButton = this._buttonOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this._buttonCancel;
			this.Controls.Add(this._newProjectPathLabel);
			this.Controls.Add(this._nameTextBox);
			this.Controls.Add(this._buttonCancel);
			this.Controls.Add(this._buttonOK);
			this.Controls.Add(this._messageLabel);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "NewProjectDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewProjectDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected System.Windows.Forms.Label _messageLabel;
		protected System.Windows.Forms.Button _buttonOK;
		protected System.Windows.Forms.Button _buttonCancel;
		protected System.Windows.Forms.TextBox _nameTextBox;
		protected System.Windows.Forms.Label _newProjectPathLabel;
		private LocalizationExtender locExtender;
		private System.ComponentModel.IContainer components;
	}
}