namespace SayMore.Utilities.ProjectWindow
{
	partial class FirstTimeRunDialog
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
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._browser = new System.Windows.Forms.WebBrowser();
			this._buttonOK = new System.Windows.Forms.Button();
			this._panelBrowser = new SilTools.Controls.SilPanel();
			this._tableLayout.SuspendLayout();
			this._panelBrowser.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.ColumnCount = 1;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._panelBrowser, 0, 0);
			this._tableLayout.Controls.Add(this._buttonOK, 0, 1);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(10, 10);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 2;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(594, 442);
			this._tableLayout.TabIndex = 20;
			// 
			// _browser
			// 
			this._browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._browser.Location = new System.Drawing.Point(0, 0);
			this._browser.MinimumSize = new System.Drawing.Size(20, 20);
			this._browser.Name = "_browser";
			this._browser.Size = new System.Drawing.Size(592, 406);
			this._browser.TabIndex = 19;
			// 
			// _buttonOK
			// 
			this._buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonOK.AutoSize = true;
			this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._buttonOK.Location = new System.Drawing.Point(519, 416);
			this._buttonOK.Margin = new System.Windows.Forms.Padding(3, 8, 0, 0);
			this._buttonOK.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(75, 26);
			this._buttonOK.TabIndex = 20;
			this._buttonOK.Text = "OK";
			this._buttonOK.UseVisualStyleBackColor = true;
			// 
			// _panelBrowser
			// 
			this._panelBrowser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelBrowser.ClipTextForChildControls = true;
			this._panelBrowser.ControlReceivingFocusOnMnemonic = null;
			this._panelBrowser.Controls.Add(this._browser);
			this._panelBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelBrowser.DoubleBuffered = true;
			this._panelBrowser.Location = new System.Drawing.Point(0, 0);
			this._panelBrowser.Margin = new System.Windows.Forms.Padding(0);
			this._panelBrowser.MnemonicGeneratesClick = false;
			this._panelBrowser.Name = "_panelBrowser";
			this._panelBrowser.PaintExplorerBarBackground = false;
			this._panelBrowser.Size = new System.Drawing.Size(594, 408);
			this._panelBrowser.TabIndex = 21;
			// 
			// FirstTimeRunDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(614, 462);
			this.Controls.Add(this._tableLayout);
			this.Name = "FirstTimeRunDialog";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.RightToLeftLayout = true;
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SayMore";
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this._panelBrowser.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser _browser;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Button _buttonOK;
		private SilTools.Controls.SilPanel _panelBrowser;
	}
}