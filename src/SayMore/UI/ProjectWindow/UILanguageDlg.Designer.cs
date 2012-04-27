namespace SayMore.Utilities.ProjectWindow
{
	partial class UILanguageDlg
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
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this._comboUILanguage = new Localization.UI.UILanguageComboBox();
			this._labelLanguage = new System.Windows.Forms.Label();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._buttonOK = new System.Windows.Forms.Button();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._linkIWantToLocalize = new System.Windows.Forms.LinkLabel();
			this._linkHelpOnLocalizing = new System.Windows.Forms.LinkLabel();
			this._tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.ColumnCount = 3;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanel.Controls.Add(this._comboUILanguage, 1, 0);
			this._tableLayoutPanel.Controls.Add(this._labelLanguage, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._buttonCancel, 2, 3);
			this._tableLayoutPanel.Controls.Add(this._buttonOK, 1, 3);
			this._tableLayoutPanel.Controls.Add(this._linkIWantToLocalize, 0, 1);
			this._tableLayoutPanel.Controls.Add(this._linkHelpOnLocalizing, 0, 2);
			this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel.Location = new System.Drawing.Point(20, 20);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 4;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.Size = new System.Drawing.Size(298, 126);
			this._tableLayoutPanel.TabIndex = 0;
			// 
			// _comboUILanguage
			// 
			this._comboUILanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutPanel.SetColumnSpan(this._comboUILanguage, 2);
			this._comboUILanguage.DropDownHeight = 200;
			this._comboUILanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._comboUILanguage.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
			this._comboUILanguage.FormattingEnabled = true;
			this._comboUILanguage.IntegralHeight = false;
			this.locExtender.SetLocalizableToolTip(this._comboUILanguage, null);
			this.locExtender.SetLocalizationComment(this._comboUILanguage, null);
			this.locExtender.SetLocalizationPriority(this._comboUILanguage, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._comboUILanguage, "UILanguageDlg._comboUILanguage");
			this._comboUILanguage.Location = new System.Drawing.Point(63, 6);
			this._comboUILanguage.Margin = new System.Windows.Forms.Padding(0);
			this._comboUILanguage.Name = "_comboUILanguage";
			this._comboUILanguage.ShowOnlyLanguagesHavingLocalizations = true;
			this._comboUILanguage.Size = new System.Drawing.Size(235, 21);
			this._comboUILanguage.TabIndex = 1;
			// 
			// _labelLanguage
			// 
			this._labelLanguage.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelLanguage.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelLanguage, null);
			this.locExtender.SetLocalizationComment(this._labelLanguage, null);
			this.locExtender.SetLocalizingId(this._labelLanguage, "DialogBoxes.UserInterfaceLanguageDlg.LanguageLabel");
			this._labelLanguage.Location = new System.Drawing.Point(0, 10);
			this._labelLanguage.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this._labelLanguage.Name = "_labelLanguage";
			this._labelLanguage.Size = new System.Drawing.Size(58, 13);
			this._labelLanguage.TabIndex = 0;
			this._labelLanguage.Text = "&Language:";
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.AutoSize = true;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.UserInterfaceLanguageDlg.CancelButton");
			this._buttonCancel.Location = new System.Drawing.Point(223, 100);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(3, 20, 0, 0);
			this._buttonCancel.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 5;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _buttonOK
			// 
			this._buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonOK.AutoSize = true;
			this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._buttonOK, null);
			this.locExtender.SetLocalizationComment(this._buttonOK, null);
			this.locExtender.SetLocalizingId(this._buttonOK, "DialogBoxes.UserInterfaceLanguageDlg.OKButton");
			this._buttonOK.Location = new System.Drawing.Point(142, 100);
			this._buttonOK.Margin = new System.Windows.Forms.Padding(0, 20, 3, 0);
			this._buttonOK.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(75, 26);
			this._buttonOK.TabIndex = 4;
			this._buttonOK.Text = "OK";
			this._buttonOK.UseVisualStyleBackColor = true;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// _linkIWantToLocalize
			// 
			this._linkIWantToLocalize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._linkIWantToLocalize.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._linkIWantToLocalize, 3);
			this.locExtender.SetLocalizableToolTip(this._linkIWantToLocalize, null);
			this.locExtender.SetLocalizationComment(this._linkIWantToLocalize, null);
			this.locExtender.SetLocalizingId(this._linkIWantToLocalize, "DialogBoxes.UserInterfaceLanguageDlg.IWantToLocalizeLink");
			this._linkIWantToLocalize.Location = new System.Drawing.Point(0, 44);
			this._linkIWantToLocalize.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._linkIWantToLocalize.Name = "_linkIWantToLocalize";
			this._linkIWantToLocalize.Size = new System.Drawing.Size(298, 13);
			this._linkIWantToLocalize.TabIndex = 2;
			this._linkIWantToLocalize.TabStop = true;
			this._linkIWantToLocalize.Text = "I want to localize SayMore for another language...";
			this._linkIWantToLocalize.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleIWantToLocalizeLinkClicked);
			// 
			// _linkHelpOnLocalizing
			// 
			this._linkHelpOnLocalizing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._linkHelpOnLocalizing.AutoSize = true;
			this._tableLayoutPanel.SetColumnSpan(this._linkHelpOnLocalizing, 3);
			this.locExtender.SetLocalizableToolTip(this._linkHelpOnLocalizing, null);
			this.locExtender.SetLocalizationComment(this._linkHelpOnLocalizing, null);
			this.locExtender.SetLocalizingId(this._linkHelpOnLocalizing, "DialogBoxes.UserInterfaceLanguageDlg.HelpOnLocalizingLink");
			this._linkHelpOnLocalizing.Location = new System.Drawing.Point(0, 67);
			this._linkHelpOnLocalizing.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._linkHelpOnLocalizing.Name = "_linkHelpOnLocalizing";
			this._linkHelpOnLocalizing.Size = new System.Drawing.Size(298, 13);
			this._linkHelpOnLocalizing.TabIndex = 3;
			this._linkHelpOnLocalizing.TabStop = true;
			this._linkHelpOnLocalizing.Text = "Help on localization";
			this._linkHelpOnLocalizing.Visible = false;
			// 
			// UILanguageDlg
			// 
			this.AcceptButton = this._buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(338, 166);
			this.Controls.Add(this._tableLayoutPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.UserInterfaceLanguageDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UILanguageDlg";
			this.Padding = new System.Windows.Forms.Padding(20);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "User Interface Lanaguage";
			this._tableLayoutPanel.ResumeLayout(false);
			this._tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private Localization.UI.UILanguageComboBox _comboUILanguage;
		private System.Windows.Forms.Label _labelLanguage;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Button _buttonOK;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.LinkLabel _linkIWantToLocalize;
		private System.Windows.Forms.LinkLabel _linkHelpOnLocalizing;
	}
}