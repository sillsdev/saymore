using L10NSharp.UI;

namespace SayMore.UI
{
	partial class ExportMediaDlg
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
			if (disposing)
			{
				if (components != null)
					components.Dispose();

				LocalizeItemDlg.StringsLocalized -= HandleStringsLocalized;
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
			this._tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutFFmpegMissing = new System.Windows.Forms.TableLayoutPanel();
			this._pictureInformation = new System.Windows.Forms.PictureBox();
			this._labelDownloadNeeded = new System.Windows.Forms.Label();
			this._buttonDownload = new System.Windows.Forms.Button();
			this._labelStatus = new System.Windows.Forms.Label();
			this._progressBar = new System.Windows.Forms.ProgressBar();
			this._flowLayoutBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
			this._buttonClose = new System.Windows.Forms.Button();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._textBoxOutput = new System.Windows.Forms.TextBox();
			this._labelOutputFile = new System.Windows.Forms.Label();
			this._labelOutputFileValue = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._tableLayoutOuter.SuspendLayout();
			this._tableLayoutFFmpegMissing.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureInformation)).BeginInit();
			this._flowLayoutBottomButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayoutOuter
			// 
			this._tableLayoutOuter.AutoSize = true;
			this._tableLayoutOuter.ColumnCount = 2;
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.Controls.Add(this._tableLayoutFFmpegMissing, 1, 1);
			this._tableLayoutOuter.Controls.Add(this._labelStatus, 0, 2);
			this._tableLayoutOuter.Controls.Add(this._progressBar, 0, 3);
			this._tableLayoutOuter.Controls.Add(this._flowLayoutBottomButtons, 0, 5);
			this._tableLayoutOuter.Controls.Add(this._textBoxOutput, 0, 4);
			this._tableLayoutOuter.Controls.Add(this._labelOutputFile, 0, 0);
			this._tableLayoutOuter.Controls.Add(this._labelOutputFileValue, 1, 0);
			this._tableLayoutOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutOuter.Location = new System.Drawing.Point(15, 15);
			this._tableLayoutOuter.Name = "_tableLayoutOuter";
			this._tableLayoutOuter.RowCount = 4;
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.Size = new System.Drawing.Size(479, 338);
			this._tableLayoutOuter.TabIndex = 0;
			// 
			// _tableLayoutFFmpegMissing
			// 
			this._tableLayoutFFmpegMissing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutFFmpegMissing.AutoSize = true;
			this._tableLayoutFFmpegMissing.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutFFmpegMissing.ColumnCount = 3;
			this._tableLayoutFFmpegMissing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutFFmpegMissing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutFFmpegMissing.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutFFmpegMissing.Controls.Add(this._pictureInformation, 0, 0);
			this._tableLayoutFFmpegMissing.Controls.Add(this._labelDownloadNeeded, 1, 0);
			this._tableLayoutFFmpegMissing.Controls.Add(this._buttonDownload, 2, 0);
			this._tableLayoutFFmpegMissing.Location = new System.Drawing.Point(66, 28);
			this._tableLayoutFFmpegMissing.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutFFmpegMissing.Name = "_tableLayoutFFmpegMissing";
			this._tableLayoutFFmpegMissing.RowCount = 1;
			this._tableLayoutFFmpegMissing.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutFFmpegMissing.Size = new System.Drawing.Size(413, 26);
			this._tableLayoutFFmpegMissing.TabIndex = 15;
			// 
			// _pictureInformation
			// 
			this._pictureInformation.Image = global::SayMore.Properties.Resources.InfoBlue24x24;
			this.locExtender.SetLocalizableToolTip(this._pictureInformation, null);
			this.locExtender.SetLocalizationComment(this._pictureInformation, null);
			this.locExtender.SetLocalizationPriority(this._pictureInformation, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pictureInformation, "ConvertMediaDlg._pictureInformation");
			this._pictureInformation.Location = new System.Drawing.Point(0, 0);
			this._pictureInformation.Margin = new System.Windows.Forms.Padding(0);
			this._pictureInformation.Name = "_pictureInformation";
			this._pictureInformation.Size = new System.Drawing.Size(24, 24);
			this._pictureInformation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureInformation.TabIndex = 13;
			this._pictureInformation.TabStop = false;
			// 
			// _labelDownloadNeeded
			// 
			this._labelDownloadNeeded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelDownloadNeeded.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelDownloadNeeded, null);
			this.locExtender.SetLocalizationComment(this._labelDownloadNeeded, null);
			this.locExtender.SetLocalizingId(this._labelDownloadNeeded, "DialogBoxes.ConvertMediaDlg.DownloadNeededLabel");
			this._labelDownloadNeeded.Location = new System.Drawing.Point(29, 0);
			this._labelDownloadNeeded.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this._labelDownloadNeeded.Name = "_labelDownloadNeeded";
			this._labelDownloadNeeded.Size = new System.Drawing.Size(304, 26);
			this._labelDownloadNeeded.TabIndex = 14;
			this._labelDownloadNeeded.Text = "The selected conversion \'{0}\' requires FFmpeg, which must be installed.";
			// 
			// _buttonDownload
			// 
			this.locExtender.SetLocalizableToolTip(this._buttonDownload, null);
			this.locExtender.SetLocalizationComment(this._buttonDownload, null);
			this.locExtender.SetLocalizingId(this._buttonDownload, "DialogBoxes.ConvertMediaDlg.DownloadButton");
			this._buttonDownload.Location = new System.Drawing.Point(338, 0);
			this._buttonDownload.Margin = new System.Windows.Forms.Padding(0);
			this._buttonDownload.Name = "_buttonDownload";
			this._buttonDownload.Size = new System.Drawing.Size(75, 26);
			this._buttonDownload.TabIndex = 15;
			this._buttonDownload.Text = "Install...";
			this._buttonDownload.UseVisualStyleBackColor = true;
			// 
			// _labelStatus
			// 
			this._labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelStatus.AutoSize = true;
			this._tableLayoutOuter.SetColumnSpan(this._labelStatus, 2);
			this.locExtender.SetLocalizableToolTip(this._labelStatus, null);
			this.locExtender.SetLocalizationComment(this._labelStatus, null);
			this.locExtender.SetLocalizationPriority(this._labelStatus, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelStatus, "ConvertMediaDlg._labelStatus");
			this._labelStatus.Location = new System.Drawing.Point(0, 54);
			this._labelStatus.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
			this._labelStatus.Name = "_labelStatus";
			this._labelStatus.Size = new System.Drawing.Size(479, 13);
			this._labelStatus.TabIndex = 17;
			this._labelStatus.Text = "#";
			// 
			// _progressBar
			// 
			this._progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutOuter.SetColumnSpan(this._progressBar, 2);
			this._progressBar.Location = new System.Drawing.Point(0, 70);
			this._progressBar.Margin = new System.Windows.Forms.Padding(0);
			this._progressBar.Name = "_progressBar";
			this._progressBar.Size = new System.Drawing.Size(479, 17);
			this._progressBar.TabIndex = 18;
			// 
			// _flowLayoutBottomButtons
			// 
			this._flowLayoutBottomButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._flowLayoutBottomButtons.AutoSize = true;
			this._flowLayoutBottomButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutOuter.SetColumnSpan(this._flowLayoutBottomButtons, 2);
			this._flowLayoutBottomButtons.Controls.Add(this._buttonClose);
			this._flowLayoutBottomButtons.Controls.Add(this._buttonCancel);
			this._flowLayoutBottomButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this._flowLayoutBottomButtons.Location = new System.Drawing.Point(0, 312);
			this._flowLayoutBottomButtons.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._flowLayoutBottomButtons.Name = "_flowLayoutBottomButtons";
			this._flowLayoutBottomButtons.Size = new System.Drawing.Size(479, 26);
			this._flowLayoutBottomButtons.TabIndex = 19;
			this._flowLayoutBottomButtons.WrapContents = false;
			// 
			// _buttonClose
			// 
			this._buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonClose.AutoSize = true;
			this._buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._buttonClose, null);
			this.locExtender.SetLocalizationComment(this._buttonClose, null);
			this.locExtender.SetLocalizingId(this._buttonClose, "DialogBoxes.ConvertMediaDlg.CloseButton");
			this._buttonClose.Location = new System.Drawing.Point(404, 0);
			this._buttonClose.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._buttonClose.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonClose.Name = "_buttonClose";
			this._buttonClose.Size = new System.Drawing.Size(75, 26);
			this._buttonClose.TabIndex = 6;
			this._buttonClose.Text = "Close";
			this._buttonClose.UseVisualStyleBackColor = true;
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.AutoSize = true;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.ConvertMediaDlg.CancelButton");
			this._buttonCancel.Location = new System.Drawing.Point(326, 0);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 13;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _textBoxOutput
			// 
			this._textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._textBoxOutput.BackColor = System.Drawing.SystemColors.Window;
			this._tableLayoutOuter.SetColumnSpan(this._textBoxOutput, 2);
			this.locExtender.SetLocalizableToolTip(this._textBoxOutput, null);
			this.locExtender.SetLocalizationComment(this._textBoxOutput, null);
			this.locExtender.SetLocalizationPriority(this._textBoxOutput, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._textBoxOutput, "ConvertMediaDlg._textBoxOutput");
			this._textBoxOutput.Location = new System.Drawing.Point(0, 87);
			this._textBoxOutput.Margin = new System.Windows.Forms.Padding(0);
			this._textBoxOutput.MinimumSize = new System.Drawing.Size(4, 24);
			this._textBoxOutput.Multiline = true;
			this._textBoxOutput.Name = "_textBoxOutput";
			this._textBoxOutput.ReadOnly = true;
			this._textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this._textBoxOutput.Size = new System.Drawing.Size(479, 215);
			this._textBoxOutput.TabIndex = 22;
			this._textBoxOutput.WordWrap = false;
			// 
			// _labelOutputFile
			// 
			this._labelOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOutputFile.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelOutputFile, null);
			this.locExtender.SetLocalizationComment(this._labelOutputFile, null);
			this.locExtender.SetLocalizingId(this._labelOutputFile, "DialogBoxes.ConvertMediaDlg.OutputFileLabel");
			this._labelOutputFile.Location = new System.Drawing.Point(0, 0);
			this._labelOutputFile.Margin = new System.Windows.Forms.Padding(0, 0, 5, 15);
			this._labelOutputFile.Name = "_labelOutputFile";
			this._labelOutputFile.Size = new System.Drawing.Size(61, 13);
			this._labelOutputFile.TabIndex = 23;
			this._labelOutputFile.Text = "Output File:";
			// 
			// _labelOutputFileValue
			// 
			this._labelOutputFileValue.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelOutputFileValue, null);
			this.locExtender.SetLocalizationComment(this._labelOutputFileValue, null);
			this.locExtender.SetLocalizationPriority(this._labelOutputFileValue, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelOutputFileValue, "label1.label1");
			this._labelOutputFileValue.Location = new System.Drawing.Point(66, 0);
			this._labelOutputFileValue.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
			this._labelOutputFileValue.Name = "_labelOutputFileValue";
			this._labelOutputFileValue.Size = new System.Drawing.Size(14, 13);
			this._labelOutputFileValue.TabIndex = 24;
			this._labelOutputFileValue.Text = "#";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = "DialogBoxes.";
			// 
			// ExportMediaDlg
			// 
			this.AcceptButton = this._buttonClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(509, 368);
			this.Controls.Add(this._tableLayoutOuter);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.ConvertMediaDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(525, 320);
			this.Name = "ExportMediaDlg";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Exporting Audio...";
			this._tableLayoutOuter.ResumeLayout(false);
			this._tableLayoutOuter.PerformLayout();
			this._tableLayoutFFmpegMissing.ResumeLayout(false);
			this._tableLayoutFFmpegMissing.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureInformation)).EndInit();
			this._flowLayoutBottomButtons.ResumeLayout(false);
			this._flowLayoutBottomButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
		private System.Windows.Forms.Button _buttonClose;
		private System.Windows.Forms.PictureBox _pictureInformation;
		private System.Windows.Forms.Label _labelDownloadNeeded;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutFFmpegMissing;
		private System.Windows.Forms.Button _buttonDownload;
		private System.Windows.Forms.Label _labelStatus;
		private System.Windows.Forms.ProgressBar _progressBar;
		private System.Windows.Forms.FlowLayoutPanel _flowLayoutBottomButtons;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.TextBox _textBoxOutput;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.Label _labelOutputFile;
		private System.Windows.Forms.Label _labelOutputFileValue;
	}
}