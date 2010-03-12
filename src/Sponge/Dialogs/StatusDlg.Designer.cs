namespace SIL.Sponge.Dialogs
{
	partial class StatusDlg
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
			this._OKButton = new System.Windows.Forms.Button();
			this._statusLabel = new System.Windows.Forms.Label();
			this._messageLabel = new SilUtils.Controls.AutoHeightLabel();
			this.SuspendLayout();
			// 
			// _OKButton
			// 
			this._OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._OKButton.Location = new System.Drawing.Point(336, 110);
			this._OKButton.Name = "_OKButton";
			this._OKButton.Size = new System.Drawing.Size(80, 26);
			this._OKButton.TabIndex = 3;
			this._OKButton.Text = "OK";
			this._OKButton.UseVisualStyleBackColor = true;
			// 
			// _statusLabel
			// 
			this._statusLabel.AutoSize = true;
			this._statusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._statusLabel.Location = new System.Drawing.Point(21, 21);
			this._statusLabel.Name = "_statusLabel";
			this._statusLabel.Size = new System.Drawing.Size(42, 15);
			this._statusLabel.TabIndex = 4;
			this._statusLabel.Text = "Status:";
			// 
			// _messageLabel
			// 
			this._messageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._messageLabel.AutoEllipsis = true;
			this._messageLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._messageLabel.Image = null;
			this._messageLabel.Location = new System.Drawing.Point(69, 21);
			this._messageLabel.Name = "_messageLabel";
			this._messageLabel.Size = new System.Drawing.Size(347, 15);
			this._messageLabel.TabIndex = 5;
			this._messageLabel.Text = "#";
			// 
			// StatusDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(428, 148);
			this.ControlBox = false;
			this.Controls.Add(this._messageLabel);
			this.Controls.Add(this._statusLabel);
			this.Controls.Add(this._OKButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StatusDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _OKButton;
		private System.Windows.Forms.Label _statusLabel;
		private SilUtils.Controls.AutoHeightLabel _messageLabel;
	}
}