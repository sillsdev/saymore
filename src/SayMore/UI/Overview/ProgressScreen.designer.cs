using System;

namespace SayMore.UI.Overview
{
	partial class ProgressScreen
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressScreen));
			this.tsbStatistics = new System.Windows.Forms.ToolStripButton();
			this.tsbGenre = new System.Windows.Forms.ToolStripButton();
			this.tsbContributor = new System.Windows.Forms.ToolStripButton();
			this.tsbTasks = new System.Windows.Forms.ToolStripButton();
			this.SuspendLayout();
			//
			// tsbStatistics
			//
			this.tsbStatistics.AutoSize = false;
			this.tsbStatistics.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbStatistics.Image = ((System.Drawing.Image)(resources.GetObject("tsbStatistics.Image")));
			this.tsbStatistics.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbStatistics.Margin = new System.Windows.Forms.Padding(10, 5, 10, 2);
			this.tsbStatistics.Name = "tsbStatistics";
			this.tsbStatistics.Size = new System.Drawing.Size(100, 24);
			this.tsbStatistics.Text = "Statistics";
			this.tsbStatistics.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// tsbGenre
			//
			this.tsbGenre.AutoSize = false;
			this.tsbGenre.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbGenre.Image = ((System.Drawing.Image)(resources.GetObject("tsbGenre.Image")));
			this.tsbGenre.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbGenre.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
			this.tsbGenre.Name = "tsbGenre";
			this.tsbGenre.Size = new System.Drawing.Size(100, 24);
			this.tsbGenre.Text = "By Genre";
			this.tsbGenre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// tsbContributor
			//
			this.tsbContributor.AutoSize = false;
			this.tsbContributor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbContributor.Image = ((System.Drawing.Image)(resources.GetObject("tsbContributor.Image")));
			this.tsbContributor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbContributor.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
			this.tsbContributor.Name = "tsbContributor";
			this.tsbContributor.Size = new System.Drawing.Size(100, 24);
			this.tsbContributor.Text = "By Contributor";
			this.tsbContributor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// tsbTasks
			//
			this.tsbTasks.AutoSize = false;
			this.tsbTasks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tsbTasks.Image = ((System.Drawing.Image)(resources.GetObject("tsbTasks.Image")));
			this.tsbTasks.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbTasks.Margin = new System.Windows.Forms.Padding(10, 2, 10, 2);
			this.tsbTasks.Name = "tsbTasks";
			this.tsbTasks.Size = new System.Drawing.Size(100, 24);
			this.tsbTasks.Text = "Session Tasks";
			this.tsbTasks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// ProgressScreen
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "ProgressScreen";
			this.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
			this.Size = new System.Drawing.Size(657, 351);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripButton tsbStatistics;
		private System.Windows.Forms.ToolStripButton tsbGenre;
		private System.Windows.Forms.ToolStripButton tsbContributor;
		private System.Windows.Forms.ToolStripButton tsbTasks;

		public string NameForUsageReporting
		{
			get { return "Progress"; }
		}
	}
}
