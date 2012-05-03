namespace SayMore.UI.FFmpegForSayMore
{
	partial class FFmpegForSayMoreDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FFmpegForSayMoreDlg));
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this._labelOverview = new System.Windows.Forms.Label();
			this._linkDownload = new System.Windows.Forms.LinkLabel();
			this._buttonClose = new System.Windows.Forms.Button();
			this._tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.ColumnCount = 1;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.Controls.Add(this._labelOverview, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._linkDownload, 0, 1);
			this._tableLayoutPanel.Controls.Add(this._buttonClose, 0, 3);
			this._tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutPanel.Location = new System.Drawing.Point(15, 15);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 4;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.Size = new System.Drawing.Size(437, 339);
			this._tableLayoutPanel.TabIndex = 0;
			// 
			// _labelOverview
			// 
			this._labelOverview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOverview.AutoSize = true;
			this._labelOverview.Location = new System.Drawing.Point(0, 0);
			this._labelOverview.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
			this._labelOverview.Name = "_labelOverview";
			this._labelOverview.Size = new System.Drawing.Size(437, 39);
			this._labelOverview.TabIndex = 0;
			this._labelOverview.Text = resources.GetString("_labelOverview.Text");
			// 
			// _linkDownload
			// 
			this._linkDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._linkDownload.AutoSize = true;
			this._linkDownload.Location = new System.Drawing.Point(0, 54);
			this._linkDownload.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
			this._linkDownload.Name = "_linkDownload";
			this._linkDownload.Size = new System.Drawing.Size(437, 13);
			this._linkDownload.TabIndex = 3;
			this._linkDownload.TabStop = true;
			this._linkDownload.Text = "Click here to download and unpack the zip file that contains FFmpeg for SayMore.";
			this._linkDownload.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleDownloadLinkClicked);
			// 
			// _buttonClose
			// 
			this._buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonClose.Location = new System.Drawing.Point(362, 313);
			this._buttonClose.Margin = new System.Windows.Forms.Padding(3, 10, 0, 0);
			this._buttonClose.Name = "_buttonClose";
			this._buttonClose.Size = new System.Drawing.Size(75, 26);
			this._buttonClose.TabIndex = 6;
			this._buttonClose.Text = "Close";
			this._buttonClose.UseVisualStyleBackColor = true;
			// 
			// FFmpegForSayMoreDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(467, 369);
			this.Controls.Add(this._tableLayoutPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FFmpegForSayMoreDlg";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "SayMore Needs FFmpeg";
			this._tableLayoutPanel.ResumeLayout(false);
			this._tableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private System.Windows.Forms.Label _labelOverview;
		private System.Windows.Forms.LinkLabel _linkDownload;
		private System.Windows.Forms.Button _buttonClose;
	}
}