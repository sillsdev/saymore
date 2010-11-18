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
            this._contributorsControl = new SayMore.ClearShare.ContributorsListControl();
            this.SuspendLayout();
            // 
            // _contributorsControl
            // 
            this._contributorsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._contributorsControl.Location = new System.Drawing.Point(7, 7);
            this._contributorsControl.Name = "_contributorsControl";
            this._contributorsControl.Size = new System.Drawing.Size(435, 265);
            this._contributorsControl.TabIndex = 0;
            // 
            // ContributorsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._contributorsControl);
            this.Name = "ContributorsEditor";
            this.Size = new System.Drawing.Size(449, 279);
            this.ResumeLayout(false);

		}

		#endregion

        private BindingHelper _binder;
        private ClearShare.ContributorsListControl _contributorsControl;
	}
}
