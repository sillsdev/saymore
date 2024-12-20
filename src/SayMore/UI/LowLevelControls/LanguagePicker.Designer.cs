namespace SayMore.UI.LowLevelControls
{
	partial class LanguagePicker
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
			this.webViewLanguage = new Microsoft.Web.WebView2.WinForms.WebView2();
			((System.ComponentModel.ISupportInitialize)(this.webViewLanguage)).BeginInit();
			this.SuspendLayout();
			// 
			// webViewLanguage
			// 
			this.webViewLanguage.AllowExternalDrop = true;
			this.webViewLanguage.CreationProperties = null;
			this.webViewLanguage.DefaultBackgroundColor = System.Drawing.Color.White;
			this.webViewLanguage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webViewLanguage.Location = new System.Drawing.Point(0, 0);
			this.webViewLanguage.Name = "webViewLanguage";
			this.webViewLanguage.Size = new System.Drawing.Size(800, 450);
			this.webViewLanguage.TabIndex = 0;
			this.webViewLanguage.ZoomFactor = 1D;
			// 
			// LanguagePicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.webViewLanguage);
			this.Name = "LanguagePicker";
			this.Text = "LanguagePicker";
			this.Load += new System.EventHandler(this.OnLoad);
			((System.ComponentModel.ISupportInitialize)(this.webViewLanguage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.Web.WebView2.WinForms.WebView2 webViewLanguage;
	}
}