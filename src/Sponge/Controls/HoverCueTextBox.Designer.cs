namespace SIL.Sponge.Controls
{
	partial class HoverCueTextBox
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
			this.m_txtBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// m_txtBox
			// 
			this.m_txtBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_txtBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_txtBox.Location = new System.Drawing.Point(1, 1);
			this.m_txtBox.Name = "m_txtBox";
			this.m_txtBox.Size = new System.Drawing.Size(245, 13);
			this.m_txtBox.TabIndex = 0;
			this.m_txtBox.MouseLeave += new System.EventHandler(this.m_txtBox_MouseLeave);
			this.m_txtBox.Leave += new System.EventHandler(this.m_txtBox_Leave);
			this.m_txtBox.Enter += new System.EventHandler(this.m_txtBox_Enter);
			this.m_txtBox.SizeChanged += new System.EventHandler(this.HandleSizeChanged);
			this.m_txtBox.MouseEnter += new System.EventHandler(this.m_txtBox_MouseEnter);
			// 
			// HoverCueTextBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.m_txtBox);
			this.DoubleBuffered = true;
			this.Name = "HoverCueTextBox";
			this.Padding = new System.Windows.Forms.Padding(1);
			this.Size = new System.Drawing.Size(247, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox m_txtBox;
	}
}
