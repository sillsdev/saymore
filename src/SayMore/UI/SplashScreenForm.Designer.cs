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
			this.components = new System.ComponentModel.Container();
			this.m_panel = new System.Windows.Forms.Panel();
			this.picLoadingWheel = new System.Windows.Forms.PictureBox();
			this.lblBuildNumber = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.lblProductName = new System.Windows.Forms.Label();
			this._labelLoading = new System.Windows.Forms.Label();
			this._labelVersionInfo = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.m_panel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picLoadingWheel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
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
			this.m_panel.Location = new System.Drawing.Point(14, 67);
			this.m_panel.Name = "m_panel";
			this.m_panel.Size = new System.Drawing.Size(82, 74);
			this.m_panel.TabIndex = 0;
			this.m_panel.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleBackgroundPanelPaint);
			// 
			// picLoadingWheel
			// 
			this.picLoadingWheel.BackColor = System.Drawing.Color.Transparent;
			this.picLoadingWheel.Image = global::SayMore.Properties.Resources.LoadingWheel;
			this.locExtender.SetLocalizableToolTip(this.picLoadingWheel, null);
			this.locExtender.SetLocalizationComment(this.picLoadingWheel, null);
			this.locExtender.SetLocalizingId(this.picLoadingWheel, "SplashScreenForm.picLoadingWheel");
			this.picLoadingWheel.Location = new System.Drawing.Point(0, 0);
			this.picLoadingWheel.Name = "picLoadingWheel";
			this.picLoadingWheel.Size = new System.Drawing.Size(100, 50);
			this.picLoadingWheel.TabIndex = 0;
			this.picLoadingWheel.TabStop = false;
			// 
			// lblBuildNumber
			// 
			this.lblBuildNumber.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblBuildNumber, null);
			this.locExtender.SetLocalizationComment(this.lblBuildNumber, null);
			this.locExtender.SetLocalizingId(this.lblBuildNumber, "UI.SplashScreen.lblBuildNumber");
			this.lblBuildNumber.Location = new System.Drawing.Point(0, 0);
			this.lblBuildNumber.Name = "lblBuildNumber";
			this.lblBuildNumber.Size = new System.Drawing.Size(100, 23);
			this.lblBuildNumber.TabIndex = 1;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::SayMore.Properties.Resources.kimidSilLogo;
			this.locExtender.SetLocalizableToolTip(this.pictureBox1, null);
			this.locExtender.SetLocalizationComment(this.pictureBox1, null);
			this.locExtender.SetLocalizationPriority(this.pictureBox1, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pictureBox1, "UI.SplashScreen.pictureBox1");
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(100, 50);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// lblVersion
			// 
			this.lblVersion.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.lblVersion, null);
			this.locExtender.SetLocalizationComment(this.lblVersion, null);
			this.locExtender.SetLocalizingId(this.lblVersion, "UI.SplashScreen.lblVersion");
			this.lblVersion.Location = new System.Drawing.Point(0, 0);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(100, 23);
			this.lblVersion.TabIndex = 3;
			this.lblVersion.UseMnemonic = false;
			// 
			// lblMessage
			// 
			this.lblMessage.AutoEllipsis = true;
			this.lblMessage.BackColor = System.Drawing.Color.Transparent;
			this.lblMessage.ForeColor = System.Drawing.Color.Black;
			this.locExtender.SetLocalizableToolTip(this.lblMessage, null);
			this.locExtender.SetLocalizationComment(this.lblMessage, null);
			this.locExtender.SetLocalizingId(this.lblMessage, "UI.SplashScreen.lblMessage");
			this.lblMessage.Location = new System.Drawing.Point(0, 0);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(100, 23);
			this.lblMessage.TabIndex = 4;
			// 
			// lblCopyright
			// 
			this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
			this.lblCopyright.ForeColor = System.Drawing.Color.Black;
			this.locExtender.SetLocalizableToolTip(this.lblCopyright, null);
			this.locExtender.SetLocalizationComment(this.lblCopyright, null);
			this.locExtender.SetLocalizingId(this.lblCopyright, "UI.SplashScreen.lblCopyright");
			this.lblCopyright.Location = new System.Drawing.Point(0, 0);
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Size = new System.Drawing.Size(100, 23);
			this.lblCopyright.TabIndex = 5;
			// 
			// lblProductName
			// 
			this.lblProductName.BackColor = System.Drawing.Color.Transparent;
			this.lblProductName.ForeColor = System.Drawing.Color.Black;
			this.locExtender.SetLocalizableToolTip(this.lblProductName, null);
			this.locExtender.SetLocalizationComment(this.lblProductName, null);
			this.locExtender.SetLocalizingId(this.lblProductName, "UI.SplashScreen.lblProductName");
			this.lblProductName.Location = new System.Drawing.Point(0, 0);
			this.lblProductName.Name = "lblProductName";
			this.lblProductName.Size = new System.Drawing.Size(100, 23);
			this.lblProductName.TabIndex = 6;
			this.lblProductName.UseMnemonic = false;
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
			this._labelVersionInfo.Size = new System.Drawing.Size(191, 13);
			this._labelVersionInfo.TabIndex = 5;
			this._labelVersionInfo.Text = "Version {0}.{1}.{2} (Beta)    Built on {3}";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = null;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox2.Image = global::SayMore.Properties.Resources.SILInBlue76;
			this.locExtender.SetLocalizableToolTip(this.pictureBox2, null);
			this.locExtender.SetLocalizationComment(this.pictureBox2, null);
			this.locExtender.SetLocalizingId(this.pictureBox2, "pictureBox2");
			this.pictureBox2.Location = new System.Drawing.Point(354, 130);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(53, 59);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox2.TabIndex = 6;
			this.pictureBox2.TabStop = false;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this.label1, null);
			this.locExtender.SetLocalizationComment(this.label1, null);
			this.locExtender.SetLocalizationPriority(this.label1, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.label1, "SplashScreenForm.label1");
			this.label1.Location = new System.Drawing.Point(11, 151);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(159, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "© 2011-2013 SIL International";
			// 
			// SplashScreenForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(209)))), ((int)(((byte)(227)))));
			this.ClientSize = new System.Drawing.Size(408, 189);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this._labelVersionInfo);
			this.Controls.Add(this._labelLoading);
			this.Controls.Add(this.m_panel);
			this.DoubleBuffered = true;
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.SplashScreen.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SplashScreenForm";
			this.Opacity = 0D;
			this.ShowInTaskbar = false;
			this.Text = "SplashScreen";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.Color.WhiteSmoke;
			this.m_panel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picLoadingWheel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _labelLoading;
		private System.Windows.Forms.Label _labelVersionInfo;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label label1;

	}
}