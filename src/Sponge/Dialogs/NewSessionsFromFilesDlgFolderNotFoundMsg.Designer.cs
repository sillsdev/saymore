namespace SIL.Sponge.Dialogs
{
	partial class NewSessionsFromFilesDlgFolderNotFoundMsg
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.m_overviewMessageLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::SIL.Sponge.Properties.Resources.kimidWarning;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// m_overviewMessageLabel
			// 
			this.m_overviewMessageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_overviewMessageLabel.AutoEllipsis = true;
			this.m_overviewMessageLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_overviewMessageLabel.Location = new System.Drawing.Point(43, 4);
			this.m_overviewMessageLabel.Name = "m_overviewMessageLabel";
			this.m_overviewMessageLabel.Size = new System.Drawing.Size(270, 51);
			this.m_overviewMessageLabel.TabIndex = 1;
			this.m_overviewMessageLabel.Text = "{0} cannot reach the folder where your files were last time. One of the following" +
				" may be true:";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoEllipsis = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 45);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(313, 22);
			this.label1.TabIndex = 2;
			this.label1.Text = "Advice will be forthcoming.";
			// 
			// NewSessionsFromFilesDlgFolderNotFoundMsg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_overviewMessageLabel);
			this.Controls.Add(this.pictureBox1);
			this.Name = "NewSessionsFromFilesDlgFolderNotFoundMsg";
			this.Size = new System.Drawing.Size(316, 127);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label m_overviewMessageLabel;
		private System.Windows.Forms.Label label1;
	}
}
