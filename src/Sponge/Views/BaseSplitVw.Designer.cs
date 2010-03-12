namespace SIL.Sponge.Views
{
	partial class BaseSplitVw
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
			this.splitOuter = new System.Windows.Forms.SplitContainer();
			this.splitRightSide = new System.Windows.Forms.SplitContainer();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitRightSide.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			this.splitOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitOuter.Location = new System.Drawing.Point(0, 0);
			this.splitOuter.Name = "splitOuter";
			// 
			// splitOuter.Panel2
			// 
			this.splitOuter.Panel2.Controls.Add(this.splitRightSide);
			this.splitOuter.Size = new System.Drawing.Size(657, 383);
			this.splitOuter.SplitterDistance = 171;
			this.splitOuter.TabIndex = 0;
			// 
			// splitRightSide
			// 
			this.splitRightSide.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitRightSide.Location = new System.Drawing.Point(0, 0);
			this.splitRightSide.Name = "splitRightSide";
			this.splitRightSide.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.splitRightSide.Size = new System.Drawing.Size(482, 383);
			this.splitRightSide.SplitterDistance = 264;
			this.splitRightSide.TabIndex = 0;
			// 
			// BaseSplitVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitOuter);
			this.Name = "BaseSplitVw";
			this.Size = new System.Drawing.Size(657, 383);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitRightSide.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		protected System.Windows.Forms.SplitContainer splitOuter;
		protected System.Windows.Forms.SplitContainer splitRightSide;
	}
}
