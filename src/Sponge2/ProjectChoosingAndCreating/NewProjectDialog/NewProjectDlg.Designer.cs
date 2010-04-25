using SIL.Localization;

namespace Sponge2.ProjectChoosingAndCreating.NewProjectDialog
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
			this._OKButton = new System.Windows.Forms.Button();
			this._cancelButton = new System.Windows.Forms.Button();
			this._newNameTextBox = new System.Windows.Forms.TextBox();
			this._newProjectPathLabel = new System.Windows.Forms.Label();
			this.locExtender = new LocalizationExtender(this.components);
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
			// _OKButton
			// 
			resources.ApplyResources(this._OKButton, "_OKButton");
			this._OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._OKButton, null);
			this.locExtender.SetLocalizationComment(this._OKButton, null);
			this.locExtender.SetLocalizingId(this._OKButton, "NewProjectDlg._OKButton");
			this._OKButton.Name = "_OKButton";
			this._OKButton.UseVisualStyleBackColor = true;
			// 
			// _cancelButton
			// 
			resources.ApplyResources(this._cancelButton, "_cancelButton");
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._cancelButton, null);
			this.locExtender.SetLocalizationComment(this._cancelButton, null);
			this.locExtender.SetLocalizingId(this._cancelButton, "NewProjectDlg._cancelButton");
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.UseVisualStyleBackColor = true;
			// 
			// _newNameTextBox
			// 
			resources.ApplyResources(this._newNameTextBox, "_newNameTextBox");
			this.locExtender.SetLocalizableToolTip(this._newNameTextBox, null);
			this.locExtender.SetLocalizationComment(this._newNameTextBox, null);
			this.locExtender.SetLocalizationPriority(this._newNameTextBox, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._newNameTextBox, "NewProjectDlg._newNameTextBox");
			this._newNameTextBox.Name = "_newNameTextBox";
			this._newNameTextBox.TextChanged += new System.EventHandler(this.txtName_TextChanged);
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
			this.AcceptButton = this._OKButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this._cancelButton;
			this.Controls.Add(this._newProjectPathLabel);
			this.Controls.Add(this._newNameTextBox);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._OKButton);
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
		protected System.Windows.Forms.Button _OKButton;
		protected System.Windows.Forms.Button _cancelButton;
		protected System.Windows.Forms.TextBox _newNameTextBox;
		protected System.Windows.Forms.Label _newProjectPathLabel;
		private LocalizationExtender locExtender;
		private System.ComponentModel.IContainer components;
	}
}