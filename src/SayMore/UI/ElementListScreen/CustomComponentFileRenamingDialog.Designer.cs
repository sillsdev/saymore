namespace SayMore.UI.ElementListScreen
{
	partial class CustomComponentFileRenamingDialog
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
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._labelMessage = new System.Windows.Forms.Label();
			this._textBox = new System.Windows.Forms.TextBox();
			this._labelPrefix = new System.Windows.Forms.Label();
			this._buttonOK = new System.Windows.Forms.Button();
			this._labelExtension = new System.Windows.Forms.Label();
			this._tableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 4;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.Controls.Add(this._buttonCancel, 3, 2);
			this._tableLayout.Controls.Add(this._labelMessage, 0, 1);
			this._tableLayout.Controls.Add(this._textBox, 1, 0);
			this._tableLayout.Controls.Add(this._labelPrefix, 0, 0);
			this._tableLayout.Controls.Add(this._buttonOK, 2, 2);
			this._tableLayout.Controls.Add(this._labelExtension, 3, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayout.Location = new System.Drawing.Point(15, 15);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 3;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(333, 93);
			this._tableLayout.TabIndex = 0;
			this._tableLayout.SizeChanged += new System.EventHandler(this.HandleTableLayoutSizeChanged);
			// 
			// _buttonCancel
			// 
			this._buttonCancel.AutoSize = true;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._buttonCancel.Location = new System.Drawing.Point(258, 67);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this._buttonCancel.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 2;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _labelMessage
			// 
			this._labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayout.SetColumnSpan(this._labelMessage, 4);
			this._labelMessage.Location = new System.Drawing.Point(0, 34);
			this._labelMessage.Margin = new System.Windows.Forms.Padding(0, 8, 3, 20);
			this._labelMessage.Name = "_labelMessage";
			this._labelMessage.Size = new System.Drawing.Size(330, 13);
			this._labelMessage.TabIndex = 4;
			this._labelMessage.Text = "label1";
			// 
			// _textBox
			// 
			this._textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayout.SetColumnSpan(this._textBox, 2);
			this._textBox.Location = new System.Drawing.Point(38, 3);
			this._textBox.Name = "_textBox";
			this._textBox.Size = new System.Drawing.Size(213, 20);
			this._textBox.TabIndex = 1;
			this._textBox.TextChanged += new System.EventHandler(this.HandleTextBoxTextChanged);
			// 
			// _labelPrefix
			// 
			this._labelPrefix.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._labelPrefix.AutoSize = true;
			this._labelPrefix.Location = new System.Drawing.Point(0, 6);
			this._labelPrefix.Margin = new System.Windows.Forms.Padding(0);
			this._labelPrefix.Name = "_labelPrefix";
			this._labelPrefix.Size = new System.Drawing.Size(35, 13);
			this._labelPrefix.TabIndex = 1;
			this._labelPrefix.Text = "label1";
			// 
			// _buttonOK
			// 
			this._buttonOK.AutoSize = true;
			this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._buttonOK.Location = new System.Drawing.Point(175, 67);
			this._buttonOK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this._buttonOK.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(75, 26);
			this._buttonOK.TabIndex = 1;
			this._buttonOK.Text = "OK";
			this._buttonOK.UseVisualStyleBackColor = true;
			// 
			// _labelExtension
			// 
			this._labelExtension.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelExtension.AutoSize = true;
			this._labelExtension.Location = new System.Drawing.Point(254, 6);
			this._labelExtension.Margin = new System.Windows.Forms.Padding(0);
			this._labelExtension.Name = "_labelExtension";
			this._labelExtension.Size = new System.Drawing.Size(79, 13);
			this._labelExtension.TabIndex = 3;
			this._labelExtension.Text = "label2";
			// 
			// CustomComponentFileRenamingDialog
			// 
			this.AcceptButton = this._buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(363, 160);
			this.ControlBox = false;
			this.Controls.Add(this._tableLayout);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CustomComponentFileRenamingDialog";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Rename";
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Button _buttonOK;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.TextBox _textBox;
		private System.Windows.Forms.Label _labelPrefix;
		private System.Windows.Forms.Label _labelMessage;
		private System.Windows.Forms.Label _labelExtension;
	}
}