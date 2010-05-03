namespace Sponge2.UI.ComponentEditors
{
	partial class PersonBasicEditor
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
			this.lblFullName = new System.Windows.Forms.Label();
			this._fullName = new System.Windows.Forms.TextBox();
			this.lblBirthYear = new System.Windows.Forms.Label();
			this._birthYear = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lblFullName
			// 
			this.lblFullName.AutoSize = true;
			this.lblFullName.Location = new System.Drawing.Point(20, 14);
			this.lblFullName.Name = "lblFullName";
			this.lblFullName.Size = new System.Drawing.Size(54, 13);
			this.lblFullName.TabIndex = 4;
			this.lblFullName.Text = "&Full Name";
			// 
			// _fullName
			// 
			this._fullName.Location = new System.Drawing.Point(20, 32);
			this._fullName.Name = "_fullName";
			this._fullName.Size = new System.Drawing.Size(259, 20);
			this._fullName.TabIndex = 5;
			// 
			// lblBirthYear
			// 
			this.lblBirthYear.AutoSize = true;
			this.lblBirthYear.Location = new System.Drawing.Point(348, 14);
			this.lblBirthYear.Name = "lblBirthYear";
			this.lblBirthYear.Size = new System.Drawing.Size(53, 13);
			this.lblBirthYear.TabIndex = 6;
			this.lblBirthYear.Text = "&Birth Year";
			// 
			// _birthYear
			// 
			this._birthYear.Location = new System.Drawing.Point(348, 32);
			this._birthYear.Name = "_birthYear";
			this._birthYear.Size = new System.Drawing.Size(76, 20);
			this._birthYear.TabIndex = 7;
			// 
			// PersonBasicEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblBirthYear);
			this.Controls.Add(this._birthYear);
			this.Controls.Add(this.lblFullName);
			this.Controls.Add(this._fullName);
			this.Name = "PersonBasicEditor";
			this.Size = new System.Drawing.Size(656, 579);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFullName;
		private System.Windows.Forms.TextBox _fullName;
		private System.Windows.Forms.Label lblBirthYear;
		private System.Windows.Forms.TextBox _birthYear;
	}
}
