namespace SayMore.UI.ComponentEditors
{
	partial class BrowserEditor
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._browser = new System.Windows.Forms.WebBrowser();
			this._panelBrowser = new SilUtils.Controls.SilPanel();
			this._panelBrowser.SuspendLayout();
			this.SuspendLayout();
			// 
			// _browser
			// 
			this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._browser.Location = new System.Drawing.Point(0, 0);
			this._browser.MinimumSize = new System.Drawing.Size(20, 20);
			this._browser.Name = "_browser";
			this._browser.Size = new System.Drawing.Size(433, 263);
			this._browser.TabIndex = 18;
			// 
			// _panelBrowser
			// 
			this._panelBrowser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelBrowser.ClipTextForChildControls = true;
			this._panelBrowser.ControlReceivingFocusOnMnemonic = null;
			this._panelBrowser.Controls.Add(this._browser);
			this._panelBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelBrowser.DoubleBuffered = true;
			this._panelBrowser.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelBrowser.Location = new System.Drawing.Point(7, 7);
			this._panelBrowser.MnemonicGeneratesClick = false;
			this._panelBrowser.Name = "_panelBrowser";
			this._panelBrowser.PaintExplorerBarBackground = false;
			this._panelBrowser.Size = new System.Drawing.Size(435, 265);
			this._panelBrowser.TabIndex = 19;
			// 
			// BrowserEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._panelBrowser);
			this.Name = "BrowserEditor";
			this.Size = new System.Drawing.Size(449, 279);
			this._panelBrowser.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser _browser;
		private SilUtils.Controls.SilPanel _panelBrowser;
	}
}
