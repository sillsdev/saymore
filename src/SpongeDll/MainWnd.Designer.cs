namespace SIL.Sponge
{
	partial class MainWnd
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
			this.tsMain = new SpongeBar();
			this.tsbSetup = new System.Windows.Forms.ToolStripButton();
			this.tsbOverview = new System.Windows.Forms.ToolStripButton();
			this.tsbSessions = new System.Windows.Forms.ToolStripButton();
			this.tsbPeople = new System.Windows.Forms.ToolStripButton();
			this.tsbSendReceive = new System.Windows.Forms.ToolStripButton();
			this.tsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsMain
			// 
			this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsMain.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSetup,
            this.tsbOverview,
            this.tsbSessions,
            this.tsbPeople,
            this.tsbSendReceive});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.tsMain.Size = new System.Drawing.Size(797, 70);
			this.tsMain.TabIndex = 0;
			this.tsMain.Text = "toolStrip1";
			// 
			// tsbSetup
			// 
			this.tsbSetup.AutoSize = false;
			this.tsbSetup.Image = global::SIL.Sponge.Properties.Resources.kimidSetup;
			this.tsbSetup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbSetup.Margin = new System.Windows.Forms.Padding(10, 10, 0, 10);
			this.tsbSetup.Name = "tsbSetup";
			this.tsbSetup.Size = new System.Drawing.Size(80, 50);
			this.tsbSetup.Text = "Setup";
			this.tsbSetup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// tsbOverview
			// 
			this.tsbOverview.AutoSize = false;
			this.tsbOverview.Image = global::SIL.Sponge.Properties.Resources.kimidOverview;
			this.tsbOverview.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbOverview.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this.tsbOverview.Name = "tsbOverview";
			this.tsbOverview.Size = new System.Drawing.Size(80, 50);
			this.tsbOverview.Text = "Overview";
			this.tsbOverview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// tsbSessions
			// 
			this.tsbSessions.AutoSize = false;
			this.tsbSessions.Image = global::SIL.Sponge.Properties.Resources.kimidSessions;
			this.tsbSessions.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbSessions.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this.tsbSessions.Name = "tsbSessions";
			this.tsbSessions.Size = new System.Drawing.Size(80, 50);
			this.tsbSessions.Text = "Sessions";
			this.tsbSessions.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// tsbPeople
			// 
			this.tsbPeople.AutoSize = false;
			this.tsbPeople.Image = global::SIL.Sponge.Properties.Resources.kimidPeople;
			this.tsbPeople.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbPeople.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbPeople.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this.tsbPeople.Name = "tsbPeople";
			this.tsbPeople.Size = new System.Drawing.Size(80, 50);
			this.tsbPeople.Text = "People";
			this.tsbPeople.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// tsbSendReceive
			// 
			this.tsbSendReceive.AutoSize = false;
			this.tsbSendReceive.Image = global::SIL.Sponge.Properties.Resources.kimidSendReceive;
			this.tsbSendReceive.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.tsbSendReceive.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbSendReceive.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this.tsbSendReceive.Name = "tsbSendReceive";
			this.tsbSendReceive.Size = new System.Drawing.Size(80, 50);
			this.tsbSendReceive.Text = "Send/Receive";
			this.tsbSendReceive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// MainWnd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(797, 469);
			this.Controls.Add(this.tsMain);
			this.Name = "MainWnd";
			this.Text = "Sponge";
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SpongeBar tsMain;
		private System.Windows.Forms.ToolStripButton tsbPeople;
		private System.Windows.Forms.ToolStripButton tsbOverview;
		private System.Windows.Forms.ToolStripButton tsbSetup;
		private System.Windows.Forms.ToolStripButton tsbSendReceive;
		private System.Windows.Forms.ToolStripButton tsbSessions;
	}
}

