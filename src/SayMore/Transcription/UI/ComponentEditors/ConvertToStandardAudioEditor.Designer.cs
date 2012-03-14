using System.Windows.Forms;
using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class ConvertToStandardAudioEditor
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
			this.components = new System.ComponentModel.Container();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._pictureInfo = new System.Windows.Forms.PictureBox();
			this._labelOriginalFileName = new System.Windows.Forms.Label();
			this._labelConvertIntroduction = new System.Windows.Forms.Label();
			this._labelConvertHeading = new System.Windows.Forms.Label();
			this._labelOriginalFileNameValue = new System.Windows.Forms.Label();
			this._labelStandardAudioFileName = new System.Windows.Forms.Label();
			this._labelStandardAudioFileNameValue = new System.Windows.Forms.Label();
			this._buttonConvert = new System.Windows.Forms.Button();
			this._tableLayoutConvert = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureInfo)).BeginInit();
			this._tableLayoutConvert.SuspendLayout();
			this.SuspendLayout();
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// _pictureInfo
			// 
			this._pictureInfo.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pictureInfo, null);
			this.locExtender.SetLocalizationComment(this._pictureInfo, null);
			this.locExtender.SetLocalizationPriority(this._pictureInfo, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pictureInfo, "pictureBox1.pictureBox1");
			this._pictureInfo.Location = new System.Drawing.Point(15, 5);
			this._pictureInfo.Margin = new System.Windows.Forms.Padding(15, 5, 0, 0);
			this._pictureInfo.Name = "_pictureInfo";
			this._pictureInfo.Size = new System.Drawing.Size(100, 50);
			this._pictureInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureInfo.TabIndex = 1;
			this._pictureInfo.TabStop = false;
			// 
			// _labelOriginalFileName
			// 
			this._labelOriginalFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOriginalFileName.AutoSize = true;
			this._tableLayoutConvert.SetColumnSpan(this._labelOriginalFileName, 2);
			this.locExtender.SetLocalizableToolTip(this._labelOriginalFileName, null);
			this.locExtender.SetLocalizationComment(this._labelOriginalFileName, null);
			this.locExtender.SetLocalizingId(this._labelOriginalFileName, "EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio._labelOriginal" +
        "FileName");
			this._labelOriginalFileName.Location = new System.Drawing.Point(15, 98);
			this._labelOriginalFileName.Margin = new System.Windows.Forms.Padding(15, 15, 15, 0);
			this._labelOriginalFileName.Name = "_labelOriginalFileName";
			this._labelOriginalFileName.Size = new System.Drawing.Size(482, 13);
			this._labelOriginalFileName.TabIndex = 2;
			this._labelOriginalFileName.Text = "Name of original media file:";
			// 
			// _labelConvertIntroduction
			// 
			this._labelConvertIntroduction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelConvertIntroduction.AutoSize = true;
			this._tableLayoutConvert.SetColumnSpan(this._labelConvertIntroduction, 2);
			this.locExtender.SetLocalizableToolTip(this._labelConvertIntroduction, null);
			this.locExtender.SetLocalizationComment(this._labelConvertIntroduction, null);
			this.locExtender.SetLocalizingId(this._labelConvertIntroduction, "EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio._labelIntroduc" +
        "tion");
			this._labelConvertIntroduction.Location = new System.Drawing.Point(15, 70);
			this._labelConvertIntroduction.Margin = new System.Windows.Forms.Padding(15, 15, 15, 0);
			this._labelConvertIntroduction.Name = "_labelConvertIntroduction";
			this._labelConvertIntroduction.Size = new System.Drawing.Size(482, 13);
			this._labelConvertIntroduction.TabIndex = 0;
			this._labelConvertIntroduction.Text = "#";
			// 
			// _labelConvertHeading
			// 
			this._labelConvertHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelConvertHeading.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelConvertHeading, null);
			this.locExtender.SetLocalizationComment(this._labelConvertHeading, null);
			this.locExtender.SetLocalizingId(this._labelConvertHeading, "EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio._labelHeading");
			this._labelConvertHeading.Location = new System.Drawing.Point(130, 21);
			this._labelConvertHeading.Margin = new System.Windows.Forms.Padding(15, 10, 15, 10);
			this._labelConvertHeading.Name = "_labelConvertHeading";
			this._labelConvertHeading.Size = new System.Drawing.Size(367, 13);
			this._labelConvertHeading.TabIndex = 1;
			this._labelConvertHeading.Text = "File Conversion Required";
			// 
			// _labelOriginalFileNameValue
			// 
			this._labelOriginalFileNameValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOriginalFileNameValue.AutoSize = true;
			this._tableLayoutConvert.SetColumnSpan(this._labelOriginalFileNameValue, 2);
			this.locExtender.SetLocalizableToolTip(this._labelOriginalFileNameValue, null);
			this.locExtender.SetLocalizationComment(this._labelOriginalFileNameValue, null);
			this.locExtender.SetLocalizationPriority(this._labelOriginalFileNameValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelOriginalFileNameValue, "EventsView.Transcription.StartAnnotatingTab._labelSegmentationMethod");
			this._labelOriginalFileNameValue.Location = new System.Drawing.Point(32, 116);
			this._labelOriginalFileNameValue.Margin = new System.Windows.Forms.Padding(32, 5, 15, 0);
			this._labelOriginalFileNameValue.Name = "_labelOriginalFileNameValue";
			this._labelOriginalFileNameValue.Size = new System.Drawing.Size(465, 13);
			this._labelOriginalFileNameValue.TabIndex = 15;
			this._labelOriginalFileNameValue.Text = "#";
			// 
			// _labelStandardAudioFileName
			// 
			this._labelStandardAudioFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelStandardAudioFileName.AutoSize = true;
			this._tableLayoutConvert.SetColumnSpan(this._labelStandardAudioFileName, 2);
			this.locExtender.SetLocalizableToolTip(this._labelStandardAudioFileName, null);
			this.locExtender.SetLocalizationComment(this._labelStandardAudioFileName, null);
			this.locExtender.SetLocalizingId(this._labelStandardAudioFileName, "EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio._labelStandard" +
        "AudioFileName");
			this._labelStandardAudioFileName.Location = new System.Drawing.Point(15, 144);
			this._labelStandardAudioFileName.Margin = new System.Windows.Forms.Padding(15, 15, 15, 0);
			this._labelStandardAudioFileName.Name = "_labelStandardAudioFileName";
			this._labelStandardAudioFileName.Size = new System.Drawing.Size(482, 13);
			this._labelStandardAudioFileName.TabIndex = 14;
			this._labelStandardAudioFileName.Text = "Name of new, standard audio file:";
			// 
			// _labelStandardAudioFileNameValue
			// 
			this._labelStandardAudioFileNameValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelStandardAudioFileNameValue.AutoSize = true;
			this._tableLayoutConvert.SetColumnSpan(this._labelStandardAudioFileNameValue, 2);
			this.locExtender.SetLocalizableToolTip(this._labelStandardAudioFileNameValue, null);
			this.locExtender.SetLocalizationComment(this._labelStandardAudioFileNameValue, null);
			this.locExtender.SetLocalizationPriority(this._labelStandardAudioFileNameValue, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelStandardAudioFileNameValue, "EventsView.Transcription.StartAnnotatingTab._labelSegmentationMethod");
			this._labelStandardAudioFileNameValue.Location = new System.Drawing.Point(32, 162);
			this._labelStandardAudioFileNameValue.Margin = new System.Windows.Forms.Padding(32, 5, 15, 0);
			this._labelStandardAudioFileNameValue.Name = "_labelStandardAudioFileNameValue";
			this._labelStandardAudioFileNameValue.Size = new System.Drawing.Size(465, 13);
			this._labelStandardAudioFileNameValue.TabIndex = 16;
			this._labelStandardAudioFileNameValue.Text = "#";
			// 
			// _buttonConvert
			// 
			this._buttonConvert.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._buttonConvert.AutoSize = true;
			this._tableLayoutConvert.SetColumnSpan(this._buttonConvert, 2);
			this.locExtender.SetLocalizableToolTip(this._buttonConvert, null);
			this.locExtender.SetLocalizationComment(this._buttonConvert, null);
			this.locExtender.SetLocalizingId(this._buttonConvert, "EventsView.Transcription.StartAnnotatingTab.ConvertToStandardAudio._buttonConvert" +
        "");
			this._buttonConvert.Location = new System.Drawing.Point(206, 187);
			this._buttonConvert.Margin = new System.Windows.Forms.Padding(0, 12, 10, 3);
			this._buttonConvert.MinimumSize = new System.Drawing.Size(89, 26);
			this._buttonConvert.Name = "_buttonConvert";
			this._buttonConvert.Size = new System.Drawing.Size(89, 26);
			this._buttonConvert.TabIndex = 0;
			this._buttonConvert.Text = "Convert";
			this._buttonConvert.UseVisualStyleBackColor = true;
			this._buttonConvert.Click += new System.EventHandler(this.HandleConvertButtonClick);
			// 
			// _tableLayoutConvert
			// 
			this._tableLayoutConvert.AutoSize = true;
			this._tableLayoutConvert.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutConvert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(240)))), ((int)(((byte)(159)))));
			this._tableLayoutConvert.ColumnCount = 2;
			this._tableLayoutConvert.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutConvert.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutConvert.Controls.Add(this._pictureInfo, 0, 0);
			this._tableLayoutConvert.Controls.Add(this._labelOriginalFileName, 0, 2);
			this._tableLayoutConvert.Controls.Add(this._labelConvertIntroduction, 0, 1);
			this._tableLayoutConvert.Controls.Add(this._labelConvertHeading, 1, 0);
			this._tableLayoutConvert.Controls.Add(this._labelOriginalFileNameValue, 0, 3);
			this._tableLayoutConvert.Controls.Add(this._labelStandardAudioFileName, 0, 4);
			this._tableLayoutConvert.Controls.Add(this._labelStandardAudioFileNameValue, 0, 5);
			this._tableLayoutConvert.Controls.Add(this._buttonConvert, 0, 6);
			this._tableLayoutConvert.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayoutConvert.Location = new System.Drawing.Point(12, 6);
			this._tableLayoutConvert.Name = "_tableLayoutConvert";
			this._tableLayoutConvert.RowCount = 7;
			this._tableLayoutConvert.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutConvert.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutConvert.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutConvert.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutConvert.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutConvert.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutConvert.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutConvert.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutConvert.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutConvert.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutConvert.Size = new System.Drawing.Size(512, 216);
			this._tableLayoutConvert.TabIndex = 1;
			// 
			// ConvertToStandardAudioEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayoutConvert);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "EventsView.Transcription.StartAnnotatingTab.EditorBase");
			this.Name = "ConvertToStandardAudioEditor";
			this.Padding = new System.Windows.Forms.Padding(12, 6, 12, 12);
			this.Size = new System.Drawing.Size(536, 265);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureInfo)).EndInit();
			this._tableLayoutConvert.ResumeLayout(false);
			this._tableLayoutConvert.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Localization.UI.LocalizationExtender locExtender;
		private TableLayoutPanel _tableLayoutConvert;
		private PictureBox _pictureInfo;
		private Label _labelOriginalFileName;
		private Label _labelConvertIntroduction;
		private Label _labelConvertHeading;
		private Label _labelOriginalFileNameValue;
		private Label _labelStandardAudioFileName;
		private Label _labelStandardAudioFileNameValue;
		private Button _buttonConvert;


	}
}
