using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class OralAnnotationDlg
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
			this._oralAnnotationRecorder = new SayMore.Transcription.UI.OralAnnotationRecorder();
			this.SuspendLayout();
			//
			// _oralAnnotationRecorder
			//
			this._oralAnnotationRecorder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._oralAnnotationRecorder.Location = new System.Drawing.Point(15, 15);
			this._oralAnnotationRecorder.Name = "_oralAnnotationRecorder";
			this._oralAnnotationRecorder.Padding = new System.Windows.Forms.Padding(10);
			this._oralAnnotationRecorder.Size = new System.Drawing.Size(283, 185);
			this._oralAnnotationRecorder.TabIndex = 0;
			//
			// OralAnnotationDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(313, 215);
			this.Controls.Add(this._oralAnnotationRecorder);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OralAnnotationDlg";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Record Oral Annotations";
			this.ResumeLayout(false);

		}

		#endregion

		private OralAnnotationRecorder _oralAnnotationRecorder;




	}
}
