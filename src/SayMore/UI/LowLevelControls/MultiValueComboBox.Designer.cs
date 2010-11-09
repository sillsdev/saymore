namespace SayMore.UI.LowLevelControls
{
	partial class MultiValueComboBox
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
			this._textBox = new System.Windows.Forms.TextBox();
			this._panelButton = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// _textBox
			// 
			this._textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._textBox.Location = new System.Drawing.Point(2, 0);
			this._textBox.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._textBox.Name = "_textBox";
			this._textBox.Size = new System.Drawing.Size(123, 20);
			this._textBox.TabIndex = 0;
			this._textBox.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			this._textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleTextBoxKeyDown);
			this._textBox.Leave += new System.EventHandler(this.HandleTextBoxLeave);
			this._textBox.Enter += new System.EventHandler(this.HandleTextBoxEnter);
			// 
			// _panelButton
			// 
			this._panelButton.Dock = System.Windows.Forms.DockStyle.Right;
			this._panelButton.Location = new System.Drawing.Point(130, 0);
			this._panelButton.Name = "_panelButton";
			this._panelButton.Size = new System.Drawing.Size(18, 28);
			this._panelButton.TabIndex = 1;
			this._panelButton.MouseLeave += new System.EventHandler(this.HandleButtonMouseEnterLeave);
			this._panelButton.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleButtonPaint);
			this._panelButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClickOnDropDownButton);
			this._panelButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleButtonMouseDownUp);
			this._panelButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandleButtonMouseDownUp);
			this._panelButton.SizeChanged += new System.EventHandler(this.HandleButtonSizeChanged);
			this._panelButton.MouseEnter += new System.EventHandler(this.HandleButtonMouseEnterLeave);
			// 
			// MultiValueComboBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._textBox);
			this.Controls.Add(this._panelButton);
			this.Name = "MultiValueComboBox";
			this.Size = new System.Drawing.Size(148, 28);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox _textBox;
		private System.Windows.Forms.Panel _panelButton;
	}
}
