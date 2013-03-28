using L10NSharp.UI;

namespace SayMore.UI.ProjectWindow
{
	partial class AboutDialog
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
			this._linkSayMoreWebSite = new System.Windows.Forms.LinkLabel();
			this._labelVersionInfo = new System.Windows.Forms.Label();
			this._labelSubTitle = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.LocalizationExtender(this.components);
			this._buttonOK = new System.Windows.Forms.Button();
			this._linkSiLWebSite = new System.Windows.Forms.LinkLabel();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _linkSayMoreWebSite
			// 
			this._linkSayMoreWebSite.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this._linkSayMoreWebSite.ActiveLinkColor = System.Drawing.Color.RosyBrown;
			this._linkSayMoreWebSite.AutoSize = true;
			this._linkSayMoreWebSite.BackColor = System.Drawing.Color.Transparent;
			this._linkSayMoreWebSite.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this._linkSayMoreWebSite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this._linkSayMoreWebSite.LinkArea = new System.Windows.Forms.LinkArea(28, 17);
			this.locExtender.SetLocalizableToolTip(this._linkSayMoreWebSite, null);
			this.locExtender.SetLocalizationComment(this._linkSayMoreWebSite, "Parameter is the program name, \"SayMore\"");
			this.locExtender.SetLocalizingId(this._linkSayMoreWebSite, "DialogBoxes.AboutDlg._linkSayMoreWebsite");
			this._linkSayMoreWebSite.Location = new System.Drawing.Point(139, 146);
			this._linkSayMoreWebSite.Name = "_linkSayMoreWebSite";
			this._linkSayMoreWebSite.Size = new System.Drawing.Size(124, 21);
			this._linkSayMoreWebSite.TabIndex = 2;
			this._linkSayMoreWebSite.Text = "Visit {0} on the Web.";
			this._linkSayMoreWebSite.UseCompatibleTextRendering = true;
			this._linkSayMoreWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleWebSiteLinkClicked);
			// 
			// _labelVersionInfo
			// 
			this._labelVersionInfo.AutoSize = true;
			this._labelVersionInfo.BackColor = System.Drawing.Color.Transparent;
			this._labelVersionInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._labelVersionInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._labelVersionInfo, null);
			this.locExtender.SetLocalizationComment(this._labelVersionInfo, null);
			this.locExtender.SetLocalizingId(this._labelVersionInfo, "DialogBoxes.AboutDlg._labelVersionInfo");
			this._labelVersionInfo.Location = new System.Drawing.Point(136, 108);
			this._labelVersionInfo.Name = "_labelVersionInfo";
			this._labelVersionInfo.Size = new System.Drawing.Size(201, 15);
			this._labelVersionInfo.TabIndex = 1;
			this._labelVersionInfo.Text = "Version {0}.{1}.{2} (Beta)    Built on {3}";
			// 
			// _labelSubTitle
			// 
			this._labelSubTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelSubTitle.AutoEllipsis = true;
			this._labelSubTitle.BackColor = System.Drawing.Color.Transparent;
			this._labelSubTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
			this._labelSubTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.locExtender.SetLocalizableToolTip(this._labelSubTitle, null);
			this.locExtender.SetLocalizationComment(this._labelSubTitle, null);
			this.locExtender.SetLocalizingId(this._labelSubTitle, "DialogBoxes.AboutDlg._labelSubTitle");
			this._labelSubTitle.Location = new System.Drawing.Point(136, 87);
			this._labelSubTitle.Name = "_labelSubTitle";
			this._labelSubTitle.Size = new System.Drawing.Size(307, 21);
			this._labelSubTitle.TabIndex = 0;
			this._labelSubTitle.Text = "Language Documentation Project Management";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// _buttonOK
			// 
			this._buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._buttonOK, null);
			this.locExtender.SetLocalizationComment(this._buttonOK, null);
			this.locExtender.SetLocalizingId(this._buttonOK, "DialogBoxes.AboutDlg._buttonOK");
			this._buttonOK.Location = new System.Drawing.Point(373, 184);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(75, 26);
			this._buttonOK.TabIndex = 3;
			this._buttonOK.Text = "OK";
			this._buttonOK.UseVisualStyleBackColor = true;
			// 
			// _linkSiLWebSite
			// 
			this._linkSiLWebSite.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this._linkSiLWebSite.ActiveLinkColor = System.Drawing.Color.RosyBrown;
			this._linkSiLWebSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._linkSiLWebSite.AutoSize = true;
			this._linkSiLWebSite.BackColor = System.Drawing.Color.Transparent;
			this._linkSiLWebSite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this._linkSiLWebSite.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
			this.locExtender.SetLocalizableToolTip(this._linkSiLWebSite, null);
			this.locExtender.SetLocalizationComment(this._linkSiLWebSite, "Parameter is the publisher of SayMore, \"SIL International\"");
			this.locExtender.SetLocalizingId(this._linkSiLWebSite, "DialogBoxes.AboutDlg._linkSILWebsite");
			this._linkSiLWebSite.Location = new System.Drawing.Point(12, 192);
			this._linkSiLWebSite.Name = "_linkSiLWebSite";
			this._linkSiLWebSite.Size = new System.Drawing.Size(183, 13);
			this._linkSiLWebSite.TabIndex = 4;
			this._linkSiLWebSite.Text = "This free software is published by {0}.";
			this._linkSiLWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleWebSiteLinkClicked);
			// 
			// AboutDialog
			// 
			this.AcceptButton = this._buttonOK;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(460, 222);
			this.Controls.Add(this._linkSiLWebSite);
			this.Controls.Add(this._buttonOK);
			this.Controls.Add(this._labelSubTitle);
			this.Controls.Add(this._linkSayMoreWebSite);
			this.Controls.Add(this._labelVersionInfo);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.AboutDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutDialog";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About SayMore";
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.LinkLabel _linkSayMoreWebSite;
		private System.Windows.Forms.Label _labelVersionInfo;
		private System.Windows.Forms.Label _labelSubTitle;
		private LocalizationExtender locExtender;
		private System.Windows.Forms.Button _buttonOK;
		private System.Windows.Forms.LinkLabel _linkSiLWebSite;
	}
}