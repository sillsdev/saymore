namespace SIL.Sponge.Controls
{
	partial class InfoPanel
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
			this.lblFile = new System.Windows.Forms.Label();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.btnMoreAction = new SilUtils.Controls.XButton();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// lblFile
			// 
			this.lblFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblFile.AutoEllipsis = true;
			this.lblFile.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFile.Location = new System.Drawing.Point(30, 0);
			this.lblFile.Name = "lblFile";
			this.lblFile.Size = new System.Drawing.Size(299, 15);
			this.lblFile.TabIndex = 1;
			this.lblFile.Text = "#";
			// 
			// picIcon
			// 
			this.picIcon.Location = new System.Drawing.Point(0, 18);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(32, 32);
			this.picIcon.TabIndex = 7;
			this.picIcon.TabStop = false;
			// 
			// btnMoreAction
			// 
			this.btnMoreAction.BackColor = System.Drawing.Color.Transparent;
			this.btnMoreAction.CanBeChecked = false;
			this.btnMoreAction.Checked = false;
			this.btnMoreAction.DrawEmpty = false;
			this.btnMoreAction.DrawLeftArrowButton = false;
			this.btnMoreAction.DrawRightArrowButton = false;
			this.btnMoreAction.Font = new System.Drawing.Font("Marlett", 9F);
			this.btnMoreAction.Image = global::SIL.Sponge.Properties.Resources.kimidMoreAction;
			this.btnMoreAction.Location = new System.Drawing.Point(7, 0);
			this.btnMoreAction.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.btnMoreAction.Name = "btnMoreAction";
			this.btnMoreAction.Size = new System.Drawing.Size(16, 16);
			this.btnMoreAction.TabIndex = 0;
			this.btnMoreAction.Click += new System.EventHandler(this.btnMoreAction_Click);
			// 
			// InfoPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.picIcon);
			this.Controls.Add(this.btnMoreAction);
			this.Controls.Add(this.lblFile);
			this.Name = "InfoPanel";
			this.Size = new System.Drawing.Size(333, 106);
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picIcon;
		private SilUtils.Controls.XButton btnMoreAction;
		private System.Windows.Forms.Label lblFile;
	}
}
