namespace SayMore.UI
{
	partial class ConvertMediaDlg
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._labelOverview = new System.Windows.Forms.Label();
			this._labelFileToConvert = new System.Windows.Forms.Label();
			this._labelAvailableConversions = new System.Windows.Forms.Label();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._buttonBeginConversion = new System.Windows.Forms.Button();
			this._pictureInformation = new System.Windows.Forms.PictureBox();
			this._labelDownloadNeeded = new System.Windows.Forms.Label();
			this._tableLayoutFFmpegMissing = new System.Windows.Forms.TableLayoutPanel();
			this._buttonDownload = new System.Windows.Forms.Button();
			this._labelFileToConvertValue = new System.Windows.Forms.Label();
			this._comboAvailableConversions = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureInformation)).BeginInit();
			this._tableLayoutFFmpegMissing.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._labelOverview, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._labelFileToConvert, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._labelAvailableConversions, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this._buttonCancel, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this._buttonBeginConversion, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this._tableLayoutFFmpegMissing, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this._labelFileToConvertValue, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this._comboAvailableConversions, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 15);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(486, 210);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _labelOverview
			// 
			this._labelOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOverview.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this._labelOverview, 3);
			this._labelOverview.Location = new System.Drawing.Point(0, 0);
			this._labelOverview.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
			this._labelOverview.Name = "_labelOverview";
			this._labelOverview.Size = new System.Drawing.Size(486, 26);
			this._labelOverview.TabIndex = 2;
			this._labelOverview.Text = "To convert your media file to a different format, choose from one of the availabl" +
    "e conversions below and click \'Begin Conversion\'.";
			// 
			// _labelFileToConvert
			// 
			this._labelFileToConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelFileToConvert.AutoSize = true;
			this._labelFileToConvert.Location = new System.Drawing.Point(0, 41);
			this._labelFileToConvert.Margin = new System.Windows.Forms.Padding(0, 0, 5, 15);
			this._labelFileToConvert.Name = "_labelFileToConvert";
			this._labelFileToConvert.Size = new System.Drawing.Size(114, 13);
			this._labelFileToConvert.TabIndex = 3;
			this._labelFileToConvert.Text = "File to Convert:";
			// 
			// _labelAvailableConversions
			// 
			this._labelAvailableConversions.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelAvailableConversions.AutoSize = true;
			this._labelAvailableConversions.Location = new System.Drawing.Point(0, 73);
			this._labelAvailableConversions.Margin = new System.Windows.Forms.Padding(0, 0, 5, 15);
			this._labelAvailableConversions.Name = "_labelAvailableConversions";
			this._labelAvailableConversions.Size = new System.Drawing.Size(114, 13);
			this._labelAvailableConversions.TabIndex = 0;
			this._labelAvailableConversions.Text = "Available Conversions:";
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.AutoSize = true;
			this._buttonCancel.Location = new System.Drawing.Point(411, 184);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._buttonCancel.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 6;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _buttonBeginConversion
			// 
			this._buttonBeginConversion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonBeginConversion.AutoSize = true;
			this._buttonBeginConversion.Location = new System.Drawing.Point(305, 184);
			this._buttonBeginConversion.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._buttonBeginConversion.MinimumSize = new System.Drawing.Size(0, 26);
			this._buttonBeginConversion.Name = "_buttonBeginConversion";
			this._buttonBeginConversion.Size = new System.Drawing.Size(100, 26);
			this._buttonBeginConversion.TabIndex = 12;
			this._buttonBeginConversion.Text = "Begin Conversion";
			this._buttonBeginConversion.UseVisualStyleBackColor = true;
			// 
			// _pictureInformation
			// 
			this._pictureInformation.Image = global::SayMore.Properties.Resources.Information;
			this._pictureInformation.Location = new System.Drawing.Point(0, 0);
			this._pictureInformation.Margin = new System.Windows.Forms.Padding(0);
			this._pictureInformation.Name = "_pictureInformation";
			this._pictureInformation.Size = new System.Drawing.Size(16, 16);
			this._pictureInformation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureInformation.TabIndex = 13;
			this._pictureInformation.TabStop = false;
			// 
			// _labelDownloadNeeded
			// 
			this._labelDownloadNeeded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelDownloadNeeded.AutoSize = true;
			this._labelDownloadNeeded.Location = new System.Drawing.Point(21, 0);
			this._labelDownloadNeeded.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this._labelDownloadNeeded.Name = "_labelDownloadNeeded";
			this._labelDownloadNeeded.Size = new System.Drawing.Size(266, 26);
			this._labelDownloadNeeded.TabIndex = 14;
			this._labelDownloadNeeded.Text = "The selected conversion \'{0}\' requires FFmpeg which must be downloaded.";
			// 
			// _tableLayoutFFmpegMissing
			// 
			this._tableLayoutFFmpegMissing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutFFmpegMissing.AutoSize = true;
			this._tableLayoutFFmpegMissing.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutFFmpegMissing.ColumnCount = 3;
			this.tableLayoutPanel1.SetColumnSpan(this._tableLayoutFFmpegMissing, 2);
			this._tableLayoutFFmpegMissing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutFFmpegMissing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutFFmpegMissing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutFFmpegMissing.Controls.Add(this._pictureInformation, 0, 0);
			this._tableLayoutFFmpegMissing.Controls.Add(this._labelDownloadNeeded, 1, 0);
			this._tableLayoutFFmpegMissing.Controls.Add(this._buttonDownload, 2, 0);
			this._tableLayoutFFmpegMissing.Location = new System.Drawing.Point(119, 105);
			this._tableLayoutFFmpegMissing.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutFFmpegMissing.Name = "_tableLayoutFFmpegMissing";
			this._tableLayoutFFmpegMissing.RowCount = 1;
			this._tableLayoutFFmpegMissing.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutFFmpegMissing.Size = new System.Drawing.Size(367, 26);
			this._tableLayoutFFmpegMissing.TabIndex = 15;
			// 
			// _buttonDownload
			// 
			this._buttonDownload.Location = new System.Drawing.Point(292, 0);
			this._buttonDownload.Margin = new System.Windows.Forms.Padding(0);
			this._buttonDownload.Name = "_buttonDownload";
			this._buttonDownload.Size = new System.Drawing.Size(75, 26);
			this._buttonDownload.TabIndex = 15;
			this._buttonDownload.Text = "Download...";
			this._buttonDownload.UseVisualStyleBackColor = true;
			// 
			// _labelFileToConvertValue
			// 
			this._labelFileToConvertValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelFileToConvertValue.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this._labelFileToConvertValue, 2);
			this._labelFileToConvertValue.Location = new System.Drawing.Point(119, 41);
			this._labelFileToConvertValue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
			this._labelFileToConvertValue.Name = "_labelFileToConvertValue";
			this._labelFileToConvertValue.Size = new System.Drawing.Size(367, 13);
			this._labelFileToConvertValue.TabIndex = 4;
			this._labelFileToConvertValue.Text = "#";
			// 
			// _comboAvailableConversions
			// 
			this._comboAvailableConversions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this._comboAvailableConversions, 2);
			this._comboAvailableConversions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._comboAvailableConversions.FormattingEnabled = true;
			this._comboAvailableConversions.Location = new System.Drawing.Point(119, 69);
			this._comboAvailableConversions.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
			this._comboAvailableConversions.Name = "_comboAvailableConversions";
			this._comboAvailableConversions.Size = new System.Drawing.Size(367, 21);
			this._comboAvailableConversions.TabIndex = 16;
			// 
			// ConvertMediaDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(516, 240);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConvertMediaDlg";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Convert Media";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureInformation)).EndInit();
			this._tableLayoutFFmpegMissing.ResumeLayout(false);
			this._tableLayoutFFmpegMissing.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label _labelAvailableConversions;
		private System.Windows.Forms.Label _labelOverview;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Label _labelFileToConvert;
		private System.Windows.Forms.Button _buttonBeginConversion;
		private System.Windows.Forms.PictureBox _pictureInformation;
		private System.Windows.Forms.Label _labelDownloadNeeded;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutFFmpegMissing;
		private System.Windows.Forms.Button _buttonDownload;
		private System.Windows.Forms.Label _labelFileToConvertValue;
		private System.Windows.Forms.ComboBox _comboAvailableConversions;
	}
}