namespace SIL.Sponge.Controls
{
	partial class LabeledTextBox
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
			this.m_label = new System.Windows.Forms.Label();
			this.m_txtBox = new SIL.Sponge.Controls.HoverCueTextBox();
			this.SuspendLayout();
			// 
			// m_label
			// 
			this.m_label.AutoEllipsis = true;
			this.m_label.Dock = System.Windows.Forms.DockStyle.Left;
			this.m_label.Location = new System.Drawing.Point(0, 0);
			this.m_label.Name = "m_label";
			this.m_label.Size = new System.Drawing.Size(35, 17);
			this.m_label.TabIndex = 0;
			this.m_label.Text = "#";
			this.m_label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// m_txtBox
			// 
			this.m_txtBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_txtBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_txtBox.DynamicBorder = true;
			this.m_txtBox.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			// 
			// 
			// 
			this.m_txtBox.InnerTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.m_txtBox.InnerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.m_txtBox.InnerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_txtBox.InnerTextBox.Location = new System.Drawing.Point(1, 1);
			this.m_txtBox.InnerTextBox.Name = "m_txtBox";
			this.m_txtBox.InnerTextBox.Size = new System.Drawing.Size(187, 13);
			this.m_txtBox.InnerTextBox.TabIndex = 0;
			this.m_txtBox.Location = new System.Drawing.Point(35, 0);
			this.m_txtBox.Name = "m_txtBox";
			this.m_txtBox.Padding = new System.Windows.Forms.Padding(1);
			this.m_txtBox.Size = new System.Drawing.Size(189, 15);
			this.m_txtBox.TabIndex = 1;
			this.m_txtBox.SizeChanged += new System.EventHandler(this.HandleSizeChanged);
			// 
			// LabeledTextBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_txtBox);
			this.Controls.Add(this.m_label);
			this.Name = "LabeledTextBox";
			this.Size = new System.Drawing.Size(224, 17);
			this.ResumeLayout(false);

		}

		#endregion

		private HoverCueTextBox m_txtBox;
		private System.Windows.Forms.Label m_label;
	}
}
