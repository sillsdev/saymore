namespace SIL.Sponge.ConfigTools
{
	partial class WelcomeDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeDlg));
			this.welcomeControl1 = new SIL.Sponge.ConfigTools.WelcomeControl();
			this.SuspendLayout();
			// 
			// welcomeControl1
			// 
			this.welcomeControl1.BackColor = System.Drawing.Color.White;
			this.welcomeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.welcomeControl1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.welcomeControl1.Location = new System.Drawing.Point(0, 0);
			this.welcomeControl1.Name = "welcomeControl1";
			this.welcomeControl1.Size = new System.Drawing.Size(598, 402);
			this.welcomeControl1.TabIndex = 0;
			// 
			// WelcomeDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(598, 402);
			this.Controls.Add(this.welcomeControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "WelcomeDlg";
			this.Text = "Sponge";
			this.ResumeLayout(false);

		}

		#endregion

		private WelcomeControl welcomeControl1;
	}
}