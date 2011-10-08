namespace SayMore.UI.ProjectWindow
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._comboUILanguage = new Localization.UI.UILanguageComboBox();
			this._labelLanguage = new System.Windows.Forms.Label();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._buttonOK = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._comboUILanguage, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this._labelLanguage, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._buttonCancel, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this._buttonOK, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(20, 20);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(266, 84);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _comboUILanguage
			// 
			this._comboUILanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this._comboUILanguage, 2);
			this._comboUILanguage.DropDownHeight = 200;
			this._comboUILanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._comboUILanguage.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
			this._comboUILanguage.FormattingEnabled = true;
			this._comboUILanguage.IntegralHeight = false;
			this._comboUILanguage.Location = new System.Drawing.Point(63, 11);
			this._comboUILanguage.Margin = new System.Windows.Forms.Padding(0);
			this._comboUILanguage.Name = "_comboUILanguage";
			this._comboUILanguage.ShowOnlyLanguagesHavingLocalizations = true;
			this._comboUILanguage.Size = new System.Drawing.Size(203, 21);
			this._comboUILanguage.TabIndex = 1;
			// 
			// _labelLanguage
			// 
			this._labelLanguage.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelLanguage.AutoSize = true;
			this._labelLanguage.Location = new System.Drawing.Point(0, 15);
			this._labelLanguage.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this._labelLanguage.Name = "_labelLanguage";
			this._labelLanguage.Size = new System.Drawing.Size(58, 13);
			this._labelLanguage.TabIndex = 1;
			this._labelLanguage.Text = "&Language:";
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.AutoSize = true;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._buttonCancel.Location = new System.Drawing.Point(191, 58);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(3, 15, 0, 0);
			this._buttonCancel.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 1;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _buttonOK
			// 
			this._buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonOK.AutoSize = true;
			this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._buttonOK.Location = new System.Drawing.Point(110, 58);
			this._buttonOK.Margin = new System.Windows.Forms.Padding(0, 15, 3, 0);
			this._buttonOK.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(75, 26);
			this._buttonOK.TabIndex = 2;
			this._buttonOK.Text = "OK";
			this._buttonOK.UseVisualStyleBackColor = true;
			// 
			// UILanguageDlg
			// 
			this.AcceptButton = this._buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(306, 124);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UILanguageDlg";
			this.Padding = new System.Windows.Forms.Padding(20);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "User Interface Lanaguage";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private Localization.UI.UILanguageComboBox _comboUILanguage;
		private System.Windows.Forms.Label _labelLanguage;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Button _buttonOK;
	}
}