using Localization.UI;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this._linkSayMoreWebSite = new System.Windows.Forms.LinkLabel();
            this._labelVersionInfo = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.locExtender = new Localization.UI.LocalizationExtender(this.components);
            this._buttonOK = new System.Windows.Forms.Button();
            this._linkSiLWebSite = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            this.SuspendLayout();
            // 
            // _linkSayMoreWebSite
            // 
            this._linkSayMoreWebSite.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this._linkSayMoreWebSite.ActiveLinkColor = System.Drawing.Color.RosyBrown;
            resources.ApplyResources(this._linkSayMoreWebSite, "_linkSayMoreWebSite");
            this._linkSayMoreWebSite.BackColor = System.Drawing.Color.Transparent;
            this._linkSayMoreWebSite.Name = "_linkSayMoreWebSite";
            this._linkSayMoreWebSite.UseCompatibleTextRendering = true;
            this._linkSayMoreWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleWebSiteLinkClicked);
            // 
            // _labelVersionInfo
            // 
            resources.ApplyResources(this._labelVersionInfo, "_labelVersionInfo");
            this._labelVersionInfo.BackColor = System.Drawing.Color.Transparent;
            this._labelVersionInfo.Name = "_labelVersionInfo";
            // 
            // lblSubTitle
            // 
            resources.ApplyResources(this.lblSubTitle, "lblSubTitle");
            this.lblSubTitle.AutoEllipsis = true;
            this.lblSubTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubTitle.Name = "lblSubTitle";
            // 
            // locExtender
            // 
            this.locExtender.LocalizationGroup = "Dialog Boxes";
            // 
            // _buttonOK
            // 
            resources.ApplyResources(this._buttonOK, "_buttonOK");
            this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._buttonOK.Name = "_buttonOK";
            this._buttonOK.UseVisualStyleBackColor = true;
            // 
            // _linkSiLWebSite
            // 
            this._linkSiLWebSite.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this._linkSiLWebSite.ActiveLinkColor = System.Drawing.Color.RosyBrown;
            resources.ApplyResources(this._linkSiLWebSite, "_linkSiLWebSite");
            this._linkSiLWebSite.BackColor = System.Drawing.Color.Transparent;
            this._linkSiLWebSite.Name = "_linkSiLWebSite";
            this._linkSiLWebSite.UseCompatibleTextRendering = true;
            this._linkSiLWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleWebSiteLinkClicked);
            // 
            // AboutDialog
            // 
            this.AcceptButton = this._buttonOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._linkSiLWebSite);
            this.Controls.Add(this._buttonOK);
            this.Controls.Add(this.lblSubTitle);
            this.Controls.Add(this._linkSayMoreWebSite);
            this.Controls.Add(this._labelVersionInfo);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.AboutDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.LinkLabel _linkSayMoreWebSite;
		private System.Windows.Forms.Label _labelVersionInfo;
		private System.Windows.Forms.Label lblSubTitle;
		private LocalizationExtender locExtender;
		private System.Windows.Forms.Button _buttonOK;
		private System.Windows.Forms.LinkLabel _linkSiLWebSite;
	}
}