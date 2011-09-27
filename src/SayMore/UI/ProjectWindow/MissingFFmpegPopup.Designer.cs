namespace SayMore.UI.ProjectWindow
{
	partial class MissingFFmpegPopup
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissingFFmpegPopup));
			this._linkSayMoreDownloadPage = new System.Windows.Forms.LinkLabel();
			this._labelMessage = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _linkSayMoreDownloadPage
			// 
			this._linkSayMoreDownloadPage.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkSayMoreDownloadPage, null);
			this.locExtender.SetLocalizationComment(this._linkSayMoreDownloadPage, null);
			this.locExtender.SetLocalizingId(this._linkSayMoreDownloadPage, "MissingFFmpegPopup._linkSayMoreDownloadPage");
			this._linkSayMoreDownloadPage.Location = new System.Drawing.Point(21, 101);
			this._linkSayMoreDownloadPage.Name = "_linkSayMoreDownloadPage";
			this._linkSayMoreDownloadPage.Size = new System.Drawing.Size(150, 13);
			this._linkSayMoreDownloadPage.TabIndex = 0;
			this._linkSayMoreDownloadPage.TabStop = true;
			this._linkSayMoreDownloadPage.Text = "Visit SayMore Download Page";
			this._linkSayMoreDownloadPage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// _labelMessage
			// 
			this._labelMessage.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelMessage, null);
			this.locExtender.SetLocalizationComment(this._labelMessage, null);
			this.locExtender.SetLocalizingId(this._labelMessage, "MissingFFmpegPopup._labelMessage");
			this._labelMessage.Location = new System.Drawing.Point(21, 22);
			this._labelMessage.MaximumSize = new System.Drawing.Size(250, 0);
			this._labelMessage.Name = "_labelMessage";
			this._labelMessage.Size = new System.Drawing.Size(249, 52);
			this._labelMessage.TabIndex = 1;
			this._labelMessage.Text = "If you install FFmpeg, SayMore can automatically report technical details of your" +
    " media files, and offer some handy conversion functions.  FFmpeg is (free, open " +
    "source).\r\n";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "UI.MissingFFmpegPopup";
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// MissingFFmpegPopup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(281, 145);
			this.ControlBox = false;
			this.Controls.Add(this._labelMessage);
			this.Controls.Add(this._linkSayMoreDownloadPage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "MissingFFmpegPopup.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MissingFFmpegPopup";
			this.Text = "Could not locate FFmpeg";
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.LinkLabel _linkSayMoreDownloadPage;
		private System.Windows.Forms.Label _labelMessage;
		private Localization.UI.LocalizationExtender locExtender;
	}
}