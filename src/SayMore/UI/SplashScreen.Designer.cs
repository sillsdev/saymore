namespace SayMore.UI
{
	partial class SplashScreenForm
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
			this._labelLoading = new System.Windows.Forms.Label();
			this._labelVersionInfo = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this.m_panel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// m_panel
			// 
			this.m_panel.Dock = System.Windows.Forms.DockStyle.None;
			this.m_panel.Location = new System.Drawing.Point(14, 67);
			this.m_panel.Size = new System.Drawing.Size(82, 74);
			// 
			// pictureBox1
			// 
			this.locExtender.SetLocalizableToolTip(this.pictureBox1, null);
			this.locExtender.SetLocalizationComment(this.pictureBox1, null);
			this.locExtender.SetLocalizationPriority(this.pictureBox1, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pictureBox1, "SplashScreen.pictureBox1");
			// 
			// lblVersion
			// 
			this.locExtender.SetLocalizableToolTip(this.lblVersion, null);
			this.locExtender.SetLocalizationComment(this.lblVersion, null);
			this.locExtender.SetLocalizingId(this.lblVersion, "SplashScreen.lblVersion");
			// 
			// lblMessage
			// 
			this.locExtender.SetLocalizableToolTip(this.lblMessage, null);
			this.locExtender.SetLocalizationComment(this.lblMessage, null);
			this.locExtender.SetLocalizingId(this.lblMessage, "SplashScreen.lblMessage");
			// 
			// lblCopyright
			// 
			this.locExtender.SetLocalizableToolTip(this.lblCopyright, null);
			this.locExtender.SetLocalizationComment(this.lblCopyright, null);
			this.locExtender.SetLocalizingId(this.lblCopyright, "SplashScreen.lblCopyright");
			// 
			// lblProductName
			// 
			this.locExtender.SetLocalizableToolTip(this.lblProductName, null);
			this.locExtender.SetLocalizationComment(this.lblProductName, null);
			this.locExtender.SetLocalizingId(this.lblProductName, "SplashScreen.lblProductName");
			// 
			// lblBuildNumber
			// 
			this.locExtender.SetLocalizableToolTip(this.lblBuildNumber, null);
			this.locExtender.SetLocalizationComment(this.lblBuildNumber, null);
			this.locExtender.SetLocalizingId(this.lblBuildNumber, "SplashScreen.lblBuildNumber");
			// 
			// _labelLoading
			// 
			this._labelLoading.AutoSize = true;
			this._labelLoading.BackColor = System.Drawing.Color.Transparent;
			this._labelLoading.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelLoading, null);
			this.locExtender.SetLocalizationComment(this._labelLoading, null);
			this.locExtender.SetLocalizingId(this._labelLoading, "SplashScreen._labelLoading");
			this._labelLoading.Location = new System.Drawing.Point(121, 130);
			this._labelLoading.Name = "_labelLoading";
			this._labelLoading.Size = new System.Drawing.Size(59, 15);
			this._labelLoading.TabIndex = 3;
			this._labelLoading.Text = "Loading...";
			// 
			// _labelVersionInfo
			// 
			this._labelVersionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelVersionInfo.AutoSize = true;
			this._labelVersionInfo.BackColor = System.Drawing.Color.Transparent;
			this._labelVersionInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelVersionInfo, null);
			this.locExtender.SetLocalizationComment(this._labelVersionInfo, null);
			this.locExtender.SetLocalizingId(this._labelVersionInfo, "SplashScreen._labelVersionInfo");
			this._labelVersionInfo.Location = new System.Drawing.Point(11, 168);
			this._labelVersionInfo.Name = "_labelVersionInfo";
			this._labelVersionInfo.Size = new System.Drawing.Size(159, 13);
			this._labelVersionInfo.TabIndex = 5;
			this._labelVersionInfo.Text = "Version {0}.{1}.{2}    Built on {3}";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "UI.SplashScreen";
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// SplashScreenForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(209)))), ((int)(((byte)(227)))));
			this.ClientSize = new System.Drawing.Size(408, 189);
			this.Controls.Add(this._labelVersionInfo);
			this.Controls.Add(this._labelLoading);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "SplashScreen.WindowTitle");
			this.Name = "SplashScreenForm";
			this.Padding = new System.Windows.Forms.Padding(0);
			this.Text = "SplashScreen";
			this.TransparencyKey = System.Drawing.Color.WhiteSmoke;
			this.Controls.SetChildIndex(this.m_panel, 0);
			this.Controls.SetChildIndex(this._labelLoading, 0);
			this.Controls.SetChildIndex(this._labelVersionInfo, 0);
			this.m_panel.ResumeLayout(false);
			this.m_panel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _labelLoading;
		private System.Windows.Forms.Label _labelVersionInfo;
		private Localization.UI.LocalizationExtender locExtender;

	}
}