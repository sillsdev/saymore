﻿namespace SayMore.UI.LowLevelControls
{
	partial class LanguageChooser
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
			this.wsPicker = new SIL.Windows.Forms.WritingSystems.WSPickerUsingListView();
			this.SuspendLayout();
			// 
			// wsPicker
			// 
			this.wsPicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.wsPicker.Location = new System.Drawing.Point(12, 12);
			this.wsPicker.Name = "wsPicker";
			this.wsPicker.SelectedIndex = -1;
			this.wsPicker.Size = new System.Drawing.Size(776, 294);
			this.wsPicker.TabIndex = 0;
			// 
			// LanguageChooser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 381);
			this.Controls.Add(this.wsPicker);
			this.Name = "LanguageChooser";
			this.Text = "LanguageChooser";
			this.ResumeLayout(false);

		}

		#endregion

		private SIL.Windows.Forms.WritingSystems.WSPickerUsingListView wsPicker;
	}
}