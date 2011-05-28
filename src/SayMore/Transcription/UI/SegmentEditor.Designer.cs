namespace SayMore.Transcription.UI
{
	partial class SegmentEditor
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
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._buttonLoadSegFile = new System.Windows.Forms.Button();
			this._tableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.ColumnCount = 1;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.Controls.Add(this._buttonLoadSegFile, 0, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(12, 12);
			this._tableLayout.Margin = new System.Windows.Forms.Padding(3, 3, 0, 10);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 2;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Size = new System.Drawing.Size(425, 255);
			this._tableLayout.TabIndex = 0;
			// 
			// _buttonLoadSegFile
			// 
			this._buttonLoadSegFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonLoadSegFile.AutoSize = true;
			this._buttonLoadSegFile.Location = new System.Drawing.Point(311, 0);
			this._buttonLoadSegFile.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
			this._buttonLoadSegFile.Name = "_buttonLoadSegFile";
			this._buttonLoadSegFile.Size = new System.Drawing.Size(114, 26);
			this._buttonLoadSegFile.TabIndex = 3;
			this._buttonLoadSegFile.Text = "Load Segment File...";
			this._buttonLoadSegFile.UseVisualStyleBackColor = true;
			this._buttonLoadSegFile.Click += new System.EventHandler(this.HandleLoadSegmentFileClick);
			// 
			// SegmentEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.Name = "SegmentEditor";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.Size = new System.Drawing.Size(449, 279);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Button _buttonLoadSegFile;

	}
}
