namespace SayMore.UI
{
	partial class SplashScreenForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreenForm));
			this.m_panel = new System.Windows.Forms.Panel();
			this.picLoadingWheel = new System.Windows.Forms.PictureBox();
			this.lblBuildNumber = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.lblProductName = new System.Windows.Forms.Label();
			this.components = new System.ComponentModel.Container();
			this._labelLoading = new System.Windows.Forms.Label();
			this._labelVersionInfo = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.m_panel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// m_panel
			// 
			this.m_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_panel.Controls.Add(this.picLoadingWheel);
			this.m_panel.Controls.Add(this.lblBuildNumber);
			this.m_panel.Controls.Add(this.pictureBox1);
			this.m_panel.Controls.Add(this.lblVersion);
			this.m_panel.Controls.Add(this.lblMessage);
			this.m_panel.Controls.Add(this.lblCopyright);
			this.m_panel.Controls.Add(this.lblProductName);
			resources.ApplyResources(this.m_panel, "m_panel");
			this.m_panel.Name = "m_panel";
			this.m_panel.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleBackgroundPanelPaint);
			this.m_panel.Dock = System.Windows.Forms.DockStyle.None;
			this.m_panel.Location = new System.Drawing.Point(14, 67);
			this.m_panel.Size = new System.Drawing.Size(82, 74);
			// 
			// picLoadingWheel
			// 
			this.picLoadingWheel.BackColor = System.Drawing.Color.Transparent;
			this.picLoadingWheel.Image = global::SayMore.Properties.Resources.LoadingWheel;
			resources.ApplyResources(this.picLoadingWheel, "picLoadingWheel");
			this.picLoadingWheel.Name = "picLoadingWheel";
			this.picLoadingWheel.TabStop = false;
			// 
			// lblBuildNumber
			// 
			resources.ApplyResources(this.lblBuildNumber, "lblBuildNumber");
			this.lblBuildNumber.BackColor = System.Drawing.Color.Transparent;
			this.lblBuildNumber.Name = "lblBuildNumber";
			// 
			// pictureBox1
			// 
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Image = global::SayMore.Properties.Resources.kimidSilLogo;
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			this.locExtender.SetLocalizableToolTip(this.pictureBox1, null);
			this.locExtender.SetLocalizationComment(this.pictureBox1, null);
			this.locExtender.SetLocalizationPriority(this.pictureBox1, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pictureBox1, "UI.SplashScreen.pictureBox1");
			// 
			// lblVersion
			// 
			resources.ApplyResources(this.lblVersion, "lblVersion");
			this.lblVersion.BackColor = System.Drawing.Color.Transparent;
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.UseMnemonic = false;
			this.locExtender.SetLocalizableToolTip(this.lblVersion, null);
			this.locExtender.SetLocalizationComment(this.lblVersion, null);
			this.locExtender.SetLocalizingId(this.lblVersion, "UI.SplashScreen.lblVersion");
			// 
			// lblMessage
			// 
			resources.ApplyResources(this.lblMessage, "lblMessage");
			this.lblMessage.AutoEllipsis = true;
			this.lblMessage.BackColor = System.Drawing.Color.Transparent;
			this.lblMessage.ForeColor = System.Drawing.Color.Black;
			this.lblMessage.Name = "lblMessage";
			this.locExtender.SetLocalizableToolTip(this.lblMessage, null);
			this.locExtender.SetLocalizationComment(this.lblMessage, null);
			this.locExtender.SetLocalizingId(this.lblMessage, "UI.SplashScreen.lblMessage");
			// 
			// lblCopyright
			// 
			this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
			this.lblCopyright.ForeColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.lblCopyright, "lblCopyright");
			this.lblCopyright.Name = "lblCopyright";
			this.locExtender.SetLocalizableToolTip(this.lblCopyright, null);
			this.locExtender.SetLocalizationComment(this.lblCopyright, null);
			this.locExtender.SetLocalizingId(this.lblCopyright, "UI.SplashScreen.lblCopyright");
			// 
			// lblProductName
			// 
			resources.ApplyResources(this.lblProductName, "lblProductName");
			this.lblProductName.BackColor = System.Drawing.Color.Transparent;
			this.lblProductName.ForeColor = System.Drawing.Color.Black;
			this.lblProductName.Name = "lblProductName";
			this.lblProductName.UseMnemonic = false;
			this.locExtender.SetLocalizableToolTip(this.lblProductName, null);
			this.locExtender.SetLocalizationComment(this.lblProductName, null);
			this.locExtender.SetLocalizingId(this.lblProductName, "UI.SplashScreen.lblProductName");
			// 
			// lblBuildNumber
			// 
			this.locExtender.SetLocalizableToolTip(this.lblBuildNumber, null);
			this.locExtender.SetLocalizationComment(this.lblBuildNumber, null);
			this.locExtender.SetLocalizingId(this.lblBuildNumber, "UI.SplashScreen.lblBuildNumber");
			// 
			// _labelLoading
			// 
			this._labelLoading.AutoSize = true;
			this._labelLoading.BackColor = System.Drawing.Color.Transparent;
			this._labelLoading.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelLoading, null);
			this.locExtender.SetLocalizationComment(this._labelLoading, null);
			this.locExtender.SetLocalizingId(this._labelLoading, "DialogBoxes.SplashScreen.LoadingLabel");
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
			this.locExtender.SetLocalizingId(this._labelVersionInfo, "DialogBoxes.SplashScreen.VersionInfoLabel");
			this._labelVersionInfo.Location = new System.Drawing.Point(11, 168);
			this._labelVersionInfo.Name = "_labelVersionInfo";
			this._labelVersionInfo.Size = new System.Drawing.Size(159, 13);
			this._labelVersionInfo.TabIndex = 5;
			this._labelVersionInfo.Text = "Version {0}.{1}.{2} (Beta)    Built on {3}";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// SplashScreenForm
			// 
			resources.ApplyResources(this, "$this");
			this.BackColor = System.Drawing.Color.White;
			this.ControlBox = false;
			this.Controls.Add(this.m_panel);
			this.DoubleBuffered = true;
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SplashScreenForm";
			this.Opacity = 0D;
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(209)))), ((int)(((byte)(227)))));
			this.ClientSize = new System.Drawing.Size(408, 189);
			this.Controls.Add(this._labelVersionInfo);
			this.Controls.Add(this._labelLoading);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.SplashScreen.WindowTitle");
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
		private L10NSharp.UI.L10NSharpExtender locExtender;

	}
}