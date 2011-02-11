namespace SayMore.UI.LowLevelControls
{
	partial class DeleteMessageBox
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
			this._tableLayoutMessage = new System.Windows.Forms.TableLayoutPanel();
			this._pictureDeleteX = new System.Windows.Forms.PictureBox();
			this._labelMessage = new SilUtils.Controls.AutoHeightLabel();
			this._buttonDelete = new System.Windows.Forms.Button();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutMessage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureDeleteX)).BeginInit();
			this._tableLayoutButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayoutMessage
			// 
			this._tableLayoutMessage.AutoSize = true;
			this._tableLayoutMessage.ColumnCount = 2;
			this._tableLayoutMessage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutMessage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMessage.Controls.Add(this._pictureDeleteX, 0, 0);
			this._tableLayoutMessage.Controls.Add(this._labelMessage, 1, 0);
			this._tableLayoutMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutMessage.Location = new System.Drawing.Point(15, 15);
			this._tableLayoutMessage.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutMessage.Name = "_tableLayoutMessage";
			this._tableLayoutMessage.RowCount = 1;
			this._tableLayoutMessage.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutMessage.Size = new System.Drawing.Size(339, 46);
			this._tableLayoutMessage.TabIndex = 0;
			// 
			// _pictureDeleteX
			// 
			this._pictureDeleteX.Image = global::SayMore.Properties.Resources.DeleteMessageBoxImage;
			this._pictureDeleteX.Location = new System.Drawing.Point(0, 0);
			this._pictureDeleteX.Margin = new System.Windows.Forms.Padding(0, 0, 15, 3);
			this._pictureDeleteX.Name = "_pictureDeleteX";
			this._pictureDeleteX.Size = new System.Drawing.Size(24, 25);
			this._pictureDeleteX.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureDeleteX.TabIndex = 0;
			this._pictureDeleteX.TabStop = false;
			// 
			// _labelMessage
			// 
			this._labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._labelMessage.AutoEllipsis = true;
			this._labelMessage.Image = null;
			this._labelMessage.Location = new System.Drawing.Point(39, 0);
			this._labelMessage.Margin = new System.Windows.Forms.Padding(0);
			this._labelMessage.Name = "_labelMessage";
			this._labelMessage.Size = new System.Drawing.Size(300, 13);
			this._labelMessage.TabIndex = 0;
			this._labelMessage.Text = "#";
			// 
			// _buttonDelete
			// 
			this._buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonDelete.AutoSize = true;
			this._buttonDelete.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._buttonDelete.Image = global::SayMore.Properties.Resources.DeleteMessageBoxButtonImage;
			this._buttonDelete.Location = new System.Drawing.Point(90, 20);
			this._buttonDelete.Margin = new System.Windows.Forms.Padding(0, 20, 4, 0);
			this._buttonDelete.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonDelete.Name = "_buttonDelete";
			this._buttonDelete.Size = new System.Drawing.Size(75, 26);
			this._buttonDelete.TabIndex = 1;
			this._buttonDelete.Text = "Delete";
			this._buttonDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._buttonDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonDelete.UseVisualStyleBackColor = true;
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._buttonCancel.AutoSize = true;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._buttonCancel.Location = new System.Drawing.Point(173, 20);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(4, 20, 0, 0);
			this._buttonCancel.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 0;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _tableLayoutButtons
			// 
			this._tableLayoutButtons.AutoSize = true;
			this._tableLayoutButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutButtons.ColumnCount = 2;
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutButtons.Controls.Add(this._buttonDelete, 0, 0);
			this._tableLayoutButtons.Controls.Add(this._buttonCancel, 1, 0);
			this._tableLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._tableLayoutButtons.Location = new System.Drawing.Point(15, 61);
			this._tableLayoutButtons.Name = "_tableLayoutButtons";
			this._tableLayoutButtons.RowCount = 1;
			this._tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutButtons.Size = new System.Drawing.Size(339, 46);
			this._tableLayoutButtons.TabIndex = 1;
			// 
			// DeleteMessageBox
			// 
			this.AcceptButton = this._buttonDelete;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(369, 122);
			this.ControlBox = false;
			this.Controls.Add(this._tableLayoutMessage);
			this.Controls.Add(this._tableLayoutButtons);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(375, 100);
			this.Name = "DeleteMessageBox";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Confirm Delete";
			this._tableLayoutMessage.ResumeLayout(false);
			this._tableLayoutMessage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureDeleteX)).EndInit();
			this._tableLayoutButtons.ResumeLayout(false);
			this._tableLayoutButtons.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutMessage;
		private System.Windows.Forms.PictureBox _pictureDeleteX;
		private System.Windows.Forms.Button _buttonDelete;
		private System.Windows.Forms.Button _buttonCancel;
		private SilUtils.Controls.AutoHeightLabel _labelMessage;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutButtons;
	}
}