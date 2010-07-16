namespace SayMore.UI.ComponentEditors
{
	partial class NotesEditor
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
			this._notes = new System.Windows.Forms.TextBox();
			this._binder = new SayMore.UI.ComponentEditors.BindingHelper(this.components);
			this.SuspendLayout();
			// 
			// _notes
			// 
			this._notes.Dock = System.Windows.Forms.DockStyle.Fill;
			this._notes.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._notes, true);
			this._binder.SetIsComponentFileId(this._notes, false);
			this._notes.Location = new System.Drawing.Point(7, 7);
			this._notes.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._notes.Multiline = true;
			this._notes.Name = "_notes";
			this._notes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._notes.Size = new System.Drawing.Size(435, 265);
			this._notes.TabIndex = 17;
			this._notes.TextChanged += new System.EventHandler(this.HandleNotesTextChanged);
			// 
			// NotesEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._notes);
			this.Name = "NotesEditor";
			this.Size = new System.Drawing.Size(449, 279);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BindingHelper _binder;
		private System.Windows.Forms.TextBox _notes;
	}
}
