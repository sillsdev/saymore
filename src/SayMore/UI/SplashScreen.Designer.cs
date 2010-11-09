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
			this._labelLoading = new System.Windows.Forms.Label();
			this._labelVersionInfo = new System.Windows.Forms.Label();
			this.m_panel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// m_panel
			// 
			this.m_panel.Dock = System.Windows.Forms.DockStyle.None;
			this.m_panel.Location = new System.Drawing.Point(14, 67);
			this.m_panel.Size = new System.Drawing.Size(82, 74);
			// 
			// _labelLoading
			// 
			this._labelLoading.AutoSize = true;
			this._labelLoading.BackColor = System.Drawing.Color.Transparent;
			this._labelLoading.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			this._labelVersionInfo.Location = new System.Drawing.Point(11, 168);
			this._labelVersionInfo.Name = "_labelVersionInfo";
			this._labelVersionInfo.Size = new System.Drawing.Size(159, 13);
			this._labelVersionInfo.TabIndex = 5;
			this._labelVersionInfo.Text = "Version {0}.{1}.{2}    Built on {3}";
			// 
			// SplashScreenForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(209)))), ((int)(((byte)(227)))));
			this.ClientSize = new System.Drawing.Size(408, 189);
			this.Controls.Add(this._labelVersionInfo);
			this.Controls.Add(this._labelLoading);
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
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _labelLoading;
		private System.Windows.Forms.Label _labelVersionInfo;

	}
}