using SIL.Localization;
using SIL.Sponge.Controls;

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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWnd));
			this.locExtender = new LocalizationExtender(this.components);
			this.tsMain = new SIL.Sponge.Controls.SpongeBar();
			this.tsbSessions = new System.Windows.Forms.ToolStripButton();
			this.tsbPeople = new System.Windows.Forms.ToolStripButton();
			this.tsbSendReceive = new System.Windows.Forms.ToolStripButton();
			this.tsbOverview = new System.Windows.Forms.ToolStripButton();
			this.tsbSetup = new System.Windows.Forms.ToolStripButton();
			this.tsbChangeProjects = new System.Windows.Forms.ToolStripButton();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.tsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Main Window";
			// 
			// tsMain
			// 
			this.tsMain.GradientAngle = 0F;
			this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsMain.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOverview,
            this.tsbSessions,
            this.tsbPeople,
            this.tsbSendReceive,
            this.tsbSetup,
            this.tsbChangeProjects});
			this.locExtender.SetLocalizableToolTip(this.tsMain, null);
			this.locExtender.SetLocalizationComment(this.tsMain, null);
			this.locExtender.SetLocalizingId(this.tsMain, "MainWnd.tsMain");
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.tsMain.Size = new System.Drawing.Size(697, 70);
			this.tsMain.TabIndex = 0;
			this.tsMain.Text = "toolStrip1";
			// 
			// tsbSessions
			// 
			this.tsbSessions.AutoSize = false;
			this.tsbSessions.Image = global::SIL.Sponge.Properties.Resources.kimidSessions;
			this.tsbSessions.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this.tsbSessions, "Manage Sessions");
			this.locExtender.SetLocalizationComment(this.tsbSessions, null);
			this.locExtender.SetLocalizingId(this.tsbSessions, "MainWnd.tsbSessions");
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
			this.locExtender.SetLocalizableToolTip(this.tsbPeople, "Manage List of People");
			this.locExtender.SetLocalizationComment(this.tsbPeople, null);
			this.locExtender.SetLocalizingId(this.tsbPeople, "MainWnd.tsbPeople");
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
			this.locExtender.SetLocalizableToolTip(this.tsbSendReceive, "Send/Receive");
			this.locExtender.SetLocalizationComment(this.tsbSendReceive, null);
			this.locExtender.SetLocalizingId(this.tsbSendReceive, "MainWnd.tsbSendReceive");
			this.tsbSendReceive.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this.tsbSendReceive.Name = "tsbSendReceive";
			this.tsbSendReceive.Size = new System.Drawing.Size(80, 50);
			this.tsbSendReceive.Text = "Send/Receive";
			this.tsbSendReceive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// tsbOverview
			// 
			this.tsbOverview.AutoSize = false;
			this.tsbOverview.Image = global::SIL.Sponge.Properties.Resources.kimidOverview;
			this.tsbOverview.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this.tsbOverview, "Project Overview");
			this.locExtender.SetLocalizationComment(this.tsbOverview, null);
			this.locExtender.SetLocalizingId(this.tsbOverview, "MainWnd.tsbOverview");
			this.tsbOverview.Margin = new System.Windows.Forms.Padding(10, 10, 0, 10);
			this.tsbOverview.Name = "tsbOverview";
			this.tsbOverview.Size = new System.Drawing.Size(80, 50);
			this.tsbOverview.Text = "Overview";
			this.tsbOverview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// tsbSetup
			// 
			this.tsbSetup.AutoSize = false;
			this.tsbSetup.Image = global::SIL.Sponge.Properties.Resources.kimidSetup;
			this.tsbSetup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this.tsbSetup, "Project Setup");
			this.locExtender.SetLocalizationComment(this.tsbSetup, null);
			this.locExtender.SetLocalizingId(this.tsbSetup, "MainWnd.tsbSetup");
			this.tsbSetup.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
			this.tsbSetup.Name = "tsbSetup";
			this.tsbSetup.Size = new System.Drawing.Size(80, 50);
			this.tsbSetup.Text = "Setup";
			this.tsbSetup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			// 
			// tsbChangeProjects
			// 
			this.tsbChangeProjects.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tsbChangeProjects.AutoSize = false;
			this.tsbChangeProjects.Image = global::SIL.Sponge.Properties.Resources.kimidChangeProject;
			this.tsbChangeProjects.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this.tsbChangeProjects, "Open a Different Project");
			this.locExtender.SetLocalizationComment(this.tsbChangeProjects, null);
			this.locExtender.SetLocalizationPriority(this.tsbChangeProjects, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tsbChangeProjects, "MainWnd.tsbChangeProjects");
			this.tsbChangeProjects.Margin = new System.Windows.Forms.Padding(0, 10, 10, 10);
			this.tsbChangeProjects.Name = "tsbChangeProjects";
			this.tsbChangeProjects.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this.tsbChangeProjects.Size = new System.Drawing.Size(110, 50);
			this.tsbChangeProjects.Text = "Change Projects...";
			this.tsbChangeProjects.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.tsbChangeProjects.ToolTipText = "Open a Different Project";
			this.tsbChangeProjects.Click += new System.EventHandler(this.tsbChangeProjects_Click);
			// 
			// MainWnd
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(697, 469);
			this.Controls.Add(this.tsMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "MainWnd.WindowTitle");
			this.MinimumSize = new System.Drawing.Size(600, 450);
			this.Name = "MainWnd";
			this.Text = "{0} - Sponge";
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
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
		private LocalizationExtender locExtender;
		private System.Windows.Forms.ToolStripButton tsbChangeProjects;
	}
}

