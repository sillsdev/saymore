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
			this.locExtender.SetLocalizableToolTip(this._linkSayMoreWebSite, null);
			this.locExtender.SetLocalizationComment(this._linkSayMoreWebSite, null);
			this.locExtender.SetLocalizationPriority(this._linkSayMoreWebSite, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._linkSayMoreWebSite, "AboutDialog.lnkWebSites");
			this._linkSayMoreWebSite.Name = "_linkSayMoreWebSite";
			this._linkSayMoreWebSite.UseCompatibleTextRendering = true;
			this._linkSayMoreWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleWebSiteLinkClicked);
			// 
			// _labelVersionInfo
			// 
			resources.ApplyResources(this._labelVersionInfo, "_labelVersionInfo");
			this._labelVersionInfo.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._labelVersionInfo, null);
			this.locExtender.SetLocalizationComment(this._labelVersionInfo, null);
			this.locExtender.SetLocalizationPriority(this._labelVersionInfo, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelVersionInfo, "AboutDialog.lblVersionInfo");
			this._labelVersionInfo.Name = "_labelVersionInfo";
			// 
			// lblSubTitle
			// 
			resources.ApplyResources(this.lblSubTitle, "lblSubTitle");
			this.lblSubTitle.AutoEllipsis = true;
			this.lblSubTitle.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblSubTitle, null);
			this.locExtender.SetLocalizationComment(this.lblSubTitle, null);
			this.locExtender.SetLocalizationPriority(this.lblSubTitle, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblSubTitle, "AboutDialog.lblSubTitle");
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
			this.locExtender.SetLocalizableToolTip(this._buttonOK, null);
			this.locExtender.SetLocalizationComment(this._buttonOK, null);
			this.locExtender.SetLocalizingId(this._buttonOK, "button1.button1");
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.UseVisualStyleBackColor = true;
			// 
			// _linkSiLWebSite
			// 
			this._linkSiLWebSite.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this._linkSiLWebSite.ActiveLinkColor = System.Drawing.Color.RosyBrown;
			resources.ApplyResources(this._linkSiLWebSite, "_linkSiLWebSite");
			this._linkSiLWebSite.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._linkSiLWebSite, null);
			this.locExtender.SetLocalizationComment(this._linkSiLWebSite, null);
			this.locExtender.SetLocalizationPriority(this._linkSiLWebSite, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._linkSiLWebSite, "AboutDialog.lnkWebSites");
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
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "AboutDialog.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutDialog";
			this.ShowIcon = false;
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