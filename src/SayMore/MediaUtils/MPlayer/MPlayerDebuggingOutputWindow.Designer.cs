namespace SayMore.Media.MPlayer
{
	partial class MPlayerDebuggingOutputWindow
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._textOutput = new System.Windows.Forms.TextBox();
			this._buttonClose = new System.Windows.Forms.Button();
			this._buttonClear = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._textOutput, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._buttonClose, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this._buttonClear, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(435, 425);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _textOutput
			// 
			this._textOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this._textOutput, 2);
			this._textOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._textOutput.Location = new System.Drawing.Point(0, 0);
			this._textOutput.Margin = new System.Windows.Forms.Padding(0);
			this._textOutput.Multiline = true;
			this._textOutput.Name = "_textOutput";
			this._textOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this._textOutput.Size = new System.Drawing.Size(435, 394);
			this._textOutput.TabIndex = 1;
			// 
			// _buttonClose
			// 
			this._buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._buttonClose.Location = new System.Drawing.Point(360, 399);
			this._buttonClose.Margin = new System.Windows.Forms.Padding(3, 5, 0, 0);
			this._buttonClose.Name = "_buttonClose";
			this._buttonClose.Size = new System.Drawing.Size(75, 26);
			this._buttonClose.TabIndex = 0;
			this._buttonClose.Text = "Close";
			this._buttonClose.UseVisualStyleBackColor = true;
			// 
			// _buttonClear
			// 
			this._buttonClear.Location = new System.Drawing.Point(0, 399);
			this._buttonClear.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._buttonClear.Name = "_buttonClear";
			this._buttonClear.Size = new System.Drawing.Size(75, 26);
			this._buttonClear.TabIndex = 2;
			this._buttonClear.Text = "Clear";
			this._buttonClear.UseVisualStyleBackColor = true;
			this._buttonClear.Click += new System.EventHandler(this.HandleClearClick);
			// 
			// MPlayerDebuggingOutputWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._buttonClose;
			this.ClientSize = new System.Drawing.Size(455, 445);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "MPlayerDebuggingOutputWindow";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "MPlayer Output";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button _buttonClose;
		private System.Windows.Forms.TextBox _textOutput;
		private System.Windows.Forms.Button _buttonClear;
	}
}