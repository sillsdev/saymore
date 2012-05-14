namespace SayMore.UI.ElementListScreen
{
	partial class StagesControlToolTip
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
			this._labelComponentTemplate = new System.Windows.Forms.Label();
			this._labelCompleteTemplate = new System.Windows.Forms.Label();
			this._picBoxTemplate = new System.Windows.Forms.PictureBox();
			this._tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._picBoxTemplate)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 3;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.Controls.Add(this._labelComponentTemplate, 1, 0);
			this._tableLayout.Controls.Add(this._labelCompleteTemplate, 2, 0);
			this._tableLayout.Controls.Add(this._picBoxTemplate, 0, 0);
			this._tableLayout.Location = new System.Drawing.Point(8, 8);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 1;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(57, 19);
			this._tableLayout.TabIndex = 1;
			// 
			// _labelComponentTemplate
			// 
			this._labelComponentTemplate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelComponentTemplate.AutoSize = true;
			this._labelComponentTemplate.Location = new System.Drawing.Point(23, 3);
			this._labelComponentTemplate.Name = "_labelComponentTemplate";
			this._labelComponentTemplate.Size = new System.Drawing.Size(14, 13);
			this._labelComponentTemplate.TabIndex = 0;
			this._labelComponentTemplate.Text = "#";
			// 
			// _labelCompleteTemplate
			// 
			this._labelCompleteTemplate.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelCompleteTemplate.AutoSize = true;
			this._labelCompleteTemplate.Location = new System.Drawing.Point(40, 3);
			this._labelCompleteTemplate.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelCompleteTemplate.Name = "_labelCompleteTemplate";
			this._labelCompleteTemplate.Size = new System.Drawing.Size(14, 13);
			this._labelCompleteTemplate.TabIndex = 2;
			this._labelCompleteTemplate.Text = "#";
			// 
			// _picBoxTemplate
			// 
			this._picBoxTemplate.Location = new System.Drawing.Point(1, 1);
			this._picBoxTemplate.Margin = new System.Windows.Forms.Padding(1, 1, 2, 1);
			this._picBoxTemplate.Name = "_picBoxTemplate";
			this._picBoxTemplate.Size = new System.Drawing.Size(17, 17);
			this._picBoxTemplate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._picBoxTemplate.TabIndex = 1;
			this._picBoxTemplate.TabStop = false;
			// 
			// StagesControlToolTip
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Info;
			this.ClientSize = new System.Drawing.Size(266, 273);
			this.Controls.Add(this._tableLayout);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StagesControlToolTip";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._picBoxTemplate)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.PictureBox _picBoxTemplate;
		private System.Windows.Forms.Label _labelCompleteTemplate;
		private System.Windows.Forms.Label _labelComponentTemplate;
	}
}
