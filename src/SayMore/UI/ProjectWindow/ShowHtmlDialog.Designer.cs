namespace SayMore.UI.ProjectWindow
{
	partial class ShowHtmlDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowHtmlDialog));
			this._browser = new System.Windows.Forms.WebBrowser();
			this._close = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _browser
			// 
			this._browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._browser.Location = new System.Drawing.Point(12, 12);
			this._browser.MinimumSize = new System.Drawing.Size(20, 20);
			this._browser.Name = "_browser";
			this._browser.Size = new System.Drawing.Size(625, 275);
			this._browser.TabIndex = 0;
			// 
			// _close
			// 
			this._close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._close.Location = new System.Drawing.Point(562, 306);
			this._close.Name = "_close";
			this._close.Size = new System.Drawing.Size(75, 23);
			this._close.TabIndex = 1;
			this._close.Text = "&Close";
			this._close.UseVisualStyleBackColor = true;
			this._close.Click += new System.EventHandler(this.HandleClose_Click);
			// 
			// ShowHtmlDialog
			// 
			this.AcceptButton = this._close;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._close;
			this.ClientSize = new System.Drawing.Size(649, 341);
			this.Controls.Add(this._close);
			this.Controls.Add(this._browser);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ShowHtmlDialog";
			this.Text = "ShowHtmlDialog";
			this.Load += new System.EventHandler(this.ShowHtmlDialog_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser _browser;
		private System.Windows.Forms.Button _close;
	}
}