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
			this._label = new System.Windows.Forms.Label();
			this._txtBox = new SIL.Sponge.Controls.HoverCueTextBox();
			this.SuspendLayout();
			// 
			// _label
			// 
			this._label.AutoEllipsis = true;
			this._label.Dock = System.Windows.Forms.DockStyle.Left;
			this._label.Location = new System.Drawing.Point(0, 0);
			this._label.Name = "_label";
			this._label.Size = new System.Drawing.Size(35, 17);
			this._label.TabIndex = 0;
			this._label.Text = "#";
			this._label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _txtBox
			// 
			this._txtBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._txtBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._txtBox.DynamicBorder = true;
			this._txtBox.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			// 
			// 
			// 
			this._txtBox.InnerTextBox.BackColor = System.Drawing.SystemColors.Control;
			this._txtBox.InnerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._txtBox.InnerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._txtBox.InnerTextBox.Location = new System.Drawing.Point(1, 1);
			this._txtBox.InnerTextBox.Name = "_txtBox";
			this._txtBox.InnerTextBox.Size = new System.Drawing.Size(187, 13);
			this._txtBox.InnerTextBox.TabIndex = 0;
			this._txtBox.Location = new System.Drawing.Point(35, 0);
			this._txtBox.Name = "_txtBox";
			this._txtBox.Padding = new System.Windows.Forms.Padding(1);
			this._txtBox.Size = new System.Drawing.Size(189, 15);
			this._txtBox.TabIndex = 1;
			this._txtBox.SizeChanged += new System.EventHandler(this.HandleSizeChanged);
			// 
			// LabeledTextBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._txtBox);
			this.Controls.Add(this._label);
			this.Name = "LabeledTextBox";
			this.Size = new System.Drawing.Size(224, 17);
			this.ResumeLayout(false);

		}

		#endregion

		private HoverCueTextBox _txtBox;
		private System.Windows.Forms.Label _label;
	}
}
