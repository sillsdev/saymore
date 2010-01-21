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
			this.pnlOptions = new System.Windows.Forms.Panel();
			this.tsOptions = new SIL.Sponge.Controls.SpongeBar();
			this.tslblCreate = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbCreate = new System.Windows.Forms.ToolStripButton();
			this.tslblOpen = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbMru0 = new System.Windows.Forms.ToolStripButton();
			this.tsbBrowse = new System.Windows.Forms.ToolStripButton();
			this.lnkSpongeWebSite = new System.Windows.Forms.LinkLabel();
			this.lnkSIL = new System.Windows.Forms.LinkLabel();
			this.lblProdInfo1 = new System.Windows.Forms.Label();
			this.lblVersionInfo = new System.Windows.Forms.Label();
			this.lblProdInfo2 = new System.Windows.Forms.Label();
			this.lblSubTitle = new System.Windows.Forms.Label();
			this.pnlOptions.SuspendLayout();
			this.tsOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlOptions
			// 
			this.pnlOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlOptions.BackColor = System.Drawing.Color.LightGray;
			this.pnlOptions.Controls.Add(this.tsOptions);
			this.pnlOptions.Location = new System.Drawing.Point(30, 96);
			this.pnlOptions.Name = "pnlOptions";
			this.pnlOptions.Size = new System.Drawing.Size(418, 230);
			this.pnlOptions.TabIndex = 17;
			// 
			// tsOptions
			// 
			this.tsOptions.AutoSize = false;
			this.tsOptions.BackColor = System.Drawing.Color.White;
			this.tsOptions.BackColorBegin = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(208)))), ((int)(((byte)(229)))));
			this.tsOptions.BackColorEnd = System.Drawing.Color.SteelBlue;
			this.tsOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tsOptions.GradientAngle = 0F;
			this.tsOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblCreate,
            this.toolStripSeparator1,
            this.tsbCreate,
            this.tslblOpen,
            this.toolStripSeparator2,
            this.tsbMru0,
            this.tsbBrowse});
			this.tsOptions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this.tsOptions.Location = new System.Drawing.Point(0, 0);
			this.tsOptions.Name = "tsOptions";
			this.tsOptions.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.tsOptions.Size = new System.Drawing.Size(418, 230);
			this.tsOptions.TabIndex = 0;
			this.tsOptions.Text = "toolStrip1";
			// 
			// tslblCreate
			// 
			this.tslblCreate.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tslblCreate.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.tslblCreate.Name = "tslblCreate";
			this.tslblCreate.Size = new System.Drawing.Size(416, 20);
			this.tslblCreate.Text = "Create";
			this.tslblCreate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(416, 6);
			// 
			// tsbCreate
			// 
			this.tsbCreate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tsbCreate.Image = global::SIL.Sponge.Properties.Resources.kimidNewProject;
			this.tsbCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbCreate.Margin = new System.Windows.Forms.Padding(4, 1, 4, 15);
			this.tsbCreate.Name = "tsbCreate";
			this.tsbCreate.Size = new System.Drawing.Size(408, 21);
			this.tsbCreate.Text = "Create new blank project...";
			this.tsbCreate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbCreate.Click += new System.EventHandler(this.tsbCreate_Click);
			// 
			// tslblOpen
			// 
			this.tslblOpen.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tslblOpen.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.tslblOpen.Name = "tslblOpen";
			this.tslblOpen.Size = new System.Drawing.Size(416, 20);
			this.tslblOpen.Text = "Open";
			this.tslblOpen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(416, 6);
			// 
			// tsbMru0
			// 
			this.tsbMru0.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tsbMru0.Image = global::SIL.Sponge.Properties.Resources.kimidSpongeSmall;
			this.tsbMru0.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbMru0.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbMru0.Margin = new System.Windows.Forms.Padding(4, 1, 4, 2);
			this.tsbMru0.Name = "tsbMru0";
			this.tsbMru0.Size = new System.Drawing.Size(408, 21);
			this.tsbMru0.Text = "#";
			this.tsbMru0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbMru0.Click += new System.EventHandler(this.tsbMru_Click);
			// 
			// tsbBrowse
			// 
			this.tsbBrowse.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tsbBrowse.Image = global::SIL.Sponge.Properties.Resources.kimidBrowseForProject;
			this.tsbBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbBrowse.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbBrowse.Margin = new System.Windows.Forms.Padding(4, 1, 4, 15);
			this.tsbBrowse.Name = "tsbBrowse";
			this.tsbBrowse.Size = new System.Drawing.Size(408, 21);
			this.tsbBrowse.Text = "Browse for project...";
			this.tsbBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.tsbBrowse.Click += new System.EventHandler(this.tsbBrowse_Click);
			// 
			// lnkSpongeWebSite
			// 
			this.lnkSpongeWebSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkSpongeWebSite.AutoSize = true;
			this.lnkSpongeWebSite.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkSpongeWebSite.Location = new System.Drawing.Point(296, 345);
			this.lnkSpongeWebSite.Name = "lnkSpongeWebSite";
			this.lnkSpongeWebSite.Size = new System.Drawing.Size(96, 13);
			this.lnkSpongeWebSite.TabIndex = 22;
			this.lnkSpongeWebSite.TabStop = true;
			this.lnkSpongeWebSite.Text = "Sponge web site.";
			this.lnkSpongeWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSpongeWebSite_LinkClicked);
			// 
			// lnkSIL
			// 
			this.lnkSIL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lnkSIL.AutoSize = true;
			this.lnkSIL.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkSIL.Location = new System.Drawing.Point(153, 345);
			this.lnkSIL.Name = "lnkSIL";
			this.lnkSIL.Size = new System.Drawing.Size(94, 13);
			this.lnkSIL.TabIndex = 20;
			this.lnkSIL.TabStop = true;
			this.lnkSIL.Text = "SIL International.";
			this.lnkSIL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSIL_LinkClicked);
			// 
			// lblProdInfo1
			// 
			this.lblProdInfo1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblProdInfo1.AutoSize = true;
			this.lblProdInfo1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProdInfo1.Location = new System.Drawing.Point(31, 345);
			this.lblProdInfo1.Name = "lblProdInfo1";
			this.lblProdInfo1.Size = new System.Drawing.Size(128, 13);
			this.lblProdInfo1.TabIndex = 19;
			this.lblProdInfo1.Text = "Sponge is a product of ";
			// 
			// lblVersionInfo
			// 
			this.lblVersionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblVersionInfo.AutoSize = true;
			this.lblVersionInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVersionInfo.Location = new System.Drawing.Point(31, 329);
			this.lblVersionInfo.Name = "lblVersionInfo";
			this.lblVersionInfo.Size = new System.Drawing.Size(14, 13);
			this.lblVersionInfo.TabIndex = 18;
			this.lblVersionInfo.Text = "#";
			// 
			// lblProdInfo2
			// 
			this.lblProdInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblProdInfo2.AutoSize = true;
			this.lblProdInfo2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblProdInfo2.Location = new System.Drawing.Point(251, 345);
			this.lblProdInfo2.Name = "lblProdInfo2";
			this.lblProdInfo2.Size = new System.Drawing.Size(49, 13);
			this.lblProdInfo2.TabIndex = 21;
			this.lblProdInfo2.Text = "Visit the";
			// 
			// lblSubTitle
			// 
			this.lblSubTitle.AutoSize = true;
			this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSubTitle.Location = new System.Drawing.Point(141, 68);
			this.lblSubTitle.Name = "lblSubTitle";
			this.lblSubTitle.Size = new System.Drawing.Size(300, 17);
			this.lblSubTitle.TabIndex = 23;
			this.lblSubTitle.Text = "Language Documentation Project Management";
			// 
			// WelcomeDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(460, 367);
			this.Controls.Add(this.lblSubTitle);
			this.Controls.Add(this.lnkSpongeWebSite);
			this.Controls.Add(this.lnkSIL);
			this.Controls.Add(this.lblProdInfo1);
			this.Controls.Add(this.lblVersionInfo);
			this.Controls.Add(this.lblProdInfo2);
			this.Controls.Add(this.pnlOptions);
			this.DoubleBuffered = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(450, 400);
			this.Name = "WelcomeDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Sponge";
			this.pnlOptions.ResumeLayout(false);
			this.tsOptions.ResumeLayout(false);
			this.tsOptions.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlOptions;
		private SIL.Sponge.Controls.SpongeBar tsOptions;
		private System.Windows.Forms.ToolStripLabel tslblCreate;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tsbCreate;
		private System.Windows.Forms.ToolStripLabel tslblOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton tsbMru0;
		private System.Windows.Forms.ToolStripButton tsbBrowse;
		private System.Windows.Forms.LinkLabel lnkSpongeWebSite;
		private System.Windows.Forms.LinkLabel lnkSIL;
		private System.Windows.Forms.Label lblProdInfo1;
		private System.Windows.Forms.Label lblVersionInfo;
		private System.Windows.Forms.Label lblProdInfo2;
		private System.Windows.Forms.Label lblSubTitle;


	}
}