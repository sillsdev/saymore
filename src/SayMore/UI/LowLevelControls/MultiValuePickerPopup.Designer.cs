namespace SayMore.UI.LowLevelControls
{
	partial class MultiValuePickerPopup
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
			this._panelCheckboxes = new System.Windows.Forms.Panel();
			this._toolStripItems = new System.Windows.Forms.ToolStrip();
			this._panelTextBox = new System.Windows.Forms.Panel();
			this._textBoxPrompt = new SayMore.UI.LowLevelControls.PromptTextBox();
			this._panelItems = new System.Windows.Forms.Panel();
			this._panelTextBox.SuspendLayout();
			this._panelItems.SuspendLayout();
			this.SuspendLayout();
			// 
			// _panelCheckboxes
			// 
			this._panelCheckboxes.BackColor = System.Drawing.Color.Transparent;
			this._panelCheckboxes.Location = new System.Drawing.Point(0, 0);
			this._panelCheckboxes.Name = "_panelCheckboxes";
			this._panelCheckboxes.Size = new System.Drawing.Size(25, 58);
			this._panelCheckboxes.TabIndex = 0;
			this._panelCheckboxes.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleCheckBoxPanelPaint);
			// 
			// _toolStripItems
			// 
			this._toolStripItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._toolStripItems.AutoSize = false;
			this._toolStripItems.BackColor = System.Drawing.Color.Transparent;
			this._toolStripItems.Dock = System.Windows.Forms.DockStyle.None;
			this._toolStripItems.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStripItems.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this._toolStripItems.Location = new System.Drawing.Point(29, 0);
			this._toolStripItems.Name = "_toolStripItems";
			this._toolStripItems.Size = new System.Drawing.Size(130, 58);
			this._toolStripItems.TabIndex = 1;
			this._toolStripItems.Text = "toolStrip1";
			// 
			// _panelTextBox
			// 
			this._panelTextBox.BackColor = System.Drawing.Color.Transparent;
			this._panelTextBox.Controls.Add(this._textBoxPrompt);
			this._panelTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this._panelTextBox.Location = new System.Drawing.Point(0, 0);
			this._panelTextBox.Name = "_panelTextBox";
			this._panelTextBox.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
			this._panelTextBox.Size = new System.Drawing.Size(159, 29);
			this._panelTextBox.TabIndex = 2;
			this._panelTextBox.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleTextBoxPanelPaint);
			// 
			// _textBoxPrompt
			// 
			this._textBoxPrompt.Dock = System.Windows.Forms.DockStyle.Top;
			this._textBoxPrompt.Location = new System.Drawing.Point(5, 5);
			this._textBoxPrompt.Name = "_textBoxPrompt";
			this._textBoxPrompt.PromptText = "Put Prompt Here";
			this._textBoxPrompt.Size = new System.Drawing.Size(149, 20);
			this._textBoxPrompt.TabIndex = 0;
			// 
			// _panelItems
			// 
			this._panelItems.AutoScroll = true;
			this._panelItems.Controls.Add(this._toolStripItems);
			this._panelItems.Controls.Add(this._panelCheckboxes);
			this._panelItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelItems.Location = new System.Drawing.Point(0, 29);
			this._panelItems.Name = "_panelItems";
			this._panelItems.Size = new System.Drawing.Size(159, 102);
			this._panelItems.TabIndex = 3;
			// 
			// MultiValuePickerPopup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this._panelItems);
			this.Controls.Add(this._panelTextBox);
			this.DoubleBuffered = true;
			this.Name = "MultiValuePickerPopup";
			this.Size = new System.Drawing.Size(159, 131);
			this._panelTextBox.ResumeLayout(false);
			this._panelTextBox.PerformLayout();
			this._panelItems.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel _panelCheckboxes;
		private System.Windows.Forms.ToolStrip _toolStripItems;
		private System.Windows.Forms.Panel _panelTextBox;
		private PromptTextBox _textBoxPrompt;
		private System.Windows.Forms.Panel _panelItems;
	}
}
