namespace Sponge2.UI.ComponentEditors
{
	partial class SessionBasicEditor
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
			this.lblId = new System.Windows.Forms.Label();
			this._id = new System.Windows.Forms.TextBox();
			this.lblTitle = new System.Windows.Forms.Label();
			this._title = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lblId
			// 
			this.lblId.AutoSize = true;
			this.lblId.Location = new System.Drawing.Point(19, 13);
			this.lblId.Name = "lblId";
			this.lblId.Size = new System.Drawing.Size(18, 13);
			this.lblId.TabIndex = 6;
			this.lblId.Text = "ID";
			// 
			// _id
			// 
			this._id.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._id.Location = new System.Drawing.Point(19, 31);
			this._id.Name = "_id";
			this._id.Size = new System.Drawing.Size(121, 20);
			this._id.TabIndex = 7;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Location = new System.Drawing.Point(15, 59);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(27, 13);
			this.lblTitle.TabIndex = 8;
			this.lblTitle.Text = "Title";
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._title.Location = new System.Drawing.Point(19, 77);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(259, 20);
			this._title.TabIndex = 9;
			// 
			// SessionBasicEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblId);
			this.Controls.Add(this._id);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this._title);
			this.Name = "SessionBasicEditor";
			this.Size = new System.Drawing.Size(451, 359);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblId;
		private System.Windows.Forms.TextBox _id;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox _title;
	}
}
