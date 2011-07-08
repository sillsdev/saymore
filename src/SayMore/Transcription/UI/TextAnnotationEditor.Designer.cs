namespace SayMore.Transcription.UI
{
	partial class TextAnnotationEditor
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
			this._labelPlaybackSpeed = new System.Windows.Forms.Label();
			this._comboPlaybackSpeed = new System.Windows.Forms.ComboBox();
			this._tableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._labelPlaybackSpeed, 0, 0);
			this._tableLayout.Controls.Add(this._comboPlaybackSpeed, 1, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(12, 12);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 2;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Size = new System.Drawing.Size(425, 255);
			this._tableLayout.TabIndex = 0;
			// 
			// _labelPlaybackSpeed
			// 
			this._labelPlaybackSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelPlaybackSpeed.AutoSize = true;
			this._labelPlaybackSpeed.Location = new System.Drawing.Point(0, 4);
			this._labelPlaybackSpeed.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelPlaybackSpeed.Name = "_labelPlaybackSpeed";
			this._labelPlaybackSpeed.Size = new System.Drawing.Size(88, 13);
			this._labelPlaybackSpeed.TabIndex = 0;
			this._labelPlaybackSpeed.Text = "Playback &Speed:";
			// 
			// _comboPlaybackSpeed
			// 
			this._comboPlaybackSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._comboPlaybackSpeed.FormattingEnabled = true;
			this._comboPlaybackSpeed.Location = new System.Drawing.Point(3, 0);
			this._comboPlaybackSpeed.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._comboPlaybackSpeed.Name = "_comboPlaybackSpeed";
			this._comboPlaybackSpeed.Size = new System.Drawing.Size(137, 21);
			this._comboPlaybackSpeed.TabIndex = 1;
			// 
			// TextAnnotationEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.Name = "TextAnnotationEditor";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.Size = new System.Drawing.Size(449, 279);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Label _labelPlaybackSpeed;
		private System.Windows.Forms.ComboBox _comboPlaybackSpeed;


	}
}
