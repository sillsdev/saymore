namespace SayMore.UI.ComponentEditors
{
	partial class ContributorsEditor
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
			this.components = new System.ComponentModel.Container();
			this._binder = new SayMore.UI.ComponentEditors.BindingHelper(this.components);
			this._labelComingSoon = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _labelComingSoon
			// 
			this._labelComingSoon.AutoSize = true;
			this._labelComingSoon.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelComingSoon.Location = new System.Drawing.Point(21, 20);
			this._labelComingSoon.Name = "_labelComingSoon";
			this._labelComingSoon.Size = new System.Drawing.Size(125, 21);
			this._labelComingSoon.TabIndex = 0;
			this._labelComingSoon.Text = "Coming Soon...";
			// 
			// ContributorsEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._labelComingSoon);
			this.Name = "ContributorsEditor";
			this.Size = new System.Drawing.Size(449, 279);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BindingHelper _binder;
		private System.Windows.Forms.Label _labelComingSoon;
	}
}
