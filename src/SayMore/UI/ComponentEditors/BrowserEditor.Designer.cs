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
			this.SuspendLayout();
			// 
			// _browser
			// 
			this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._browser.Location = new System.Drawing.Point(7, 7);
			this._browser.MinimumSize = new System.Drawing.Size(20, 20);
			this._browser.Name = "_browser";
			this._browser.Size = new System.Drawing.Size(435, 265);
			this._browser.TabIndex = 18;
			// 
			// NotesEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._browser);
			this.Name = "NotesEditor";
			this.Size = new System.Drawing.Size(449, 279);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser _browser;
	}
}
