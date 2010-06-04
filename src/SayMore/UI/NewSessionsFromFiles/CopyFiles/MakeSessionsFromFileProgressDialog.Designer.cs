namespace SayMore.UI.NewSessionsFromFiles
{
	partial class MakeSessionsFromFileProgressDialog
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
			this.components = new System.ComponentModel.Container();
			this._buttonOK = new System.Windows.Forms.Button();
			this._timer = new System.Windows.Forms.Timer(this.components);
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _buttonOK
			// 
			this._buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonOK.AutoSize = true;
			this._buttonOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._buttonOK.Location = new System.Drawing.Point(369, 138);
			this._buttonOK.Margin = new System.Windows.Forms.Padding(3, 8, 0, 0);
			this._buttonOK.MinimumSize = new System.Drawing.Size(80, 26);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(80, 26);
			this._buttonOK.TabIndex = 0;
			this._buttonOK.Text = "&OK";
			this._buttonOK.UseVisualStyleBackColor = true;
			// 
			// _timer
			// 
			this._timer.Enabled = true;
			this._timer.Interval = 500;
			this._timer.Tick += new System.EventHandler(this.HandleTimerTick);
			// 
			// _tableLayout
			// 
			this._tableLayout.ColumnCount = 1;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._buttonOK, 0, 1);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(15, 20);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 2;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(449, 164);
			this._tableLayout.TabIndex = 1;
			// 
			// MakeSessionsFromFileProgressDialog
			// 
			this.AcceptButton = this._buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(479, 199);
			this.ControlBox = false;
			this.Controls.Add(this._tableLayout);
			this.Name = "MakeSessionsFromFileProgressDialog";
			this.Padding = new System.Windows.Forms.Padding(15, 20, 15, 15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Making Sessions...";
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button _buttonOK;
		private System.Windows.Forms.Timer _timer;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
	}
}