namespace SayMore.Transcription.UI
{
	partial class ExportToFieldWorksInterlinearDlg
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
            this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
            this._buttonCancel = new System.Windows.Forms.Button();
            this._buttonExport = new System.Windows.Forms.Button();
            this._labelTranscriptionColumnHeadingText = new System.Windows.Forms.Label();
            this._comboTranscriptionWs = new System.Windows.Forms.ComboBox();
            this._labelFreeTranslationColumnHeadingText = new System.Windows.Forms.Label();
            this._comboTranslationWs = new System.Windows.Forms.ComboBox();
            this._labelOverview = new System.Windows.Forms.Label();
            this.locExtender = new Localization.UI.LocalizationExtender(this.components);
            this._tableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            this.SuspendLayout();
            // 
            // _tableLayout
            // 
            this._tableLayout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._tableLayout.AutoSize = true;
            this._tableLayout.ColumnCount = 4;
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayout.Controls.Add(this._buttonCancel, 3, 3);
            this._tableLayout.Controls.Add(this._buttonExport, 2, 3);
            this._tableLayout.Controls.Add(this._labelTranscriptionColumnHeadingText, 0, 1);
            this._tableLayout.Controls.Add(this._comboTranscriptionWs, 1, 1);
            this._tableLayout.Controls.Add(this._labelFreeTranslationColumnHeadingText, 0, 2);
            this._tableLayout.Controls.Add(this._comboTranslationWs, 1, 2);
            this._tableLayout.Controls.Add(this._labelOverview, 0, 0);
            this._tableLayout.Location = new System.Drawing.Point(18, 18);
            this._tableLayout.Name = "_tableLayout";
            this._tableLayout.RowCount = 4;
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayout.Size = new System.Drawing.Size(284, 136);
            this._tableLayout.TabIndex = 0;
            // 
            // _buttonCancel
            // 
            this._buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._buttonCancel.AutoSize = true;
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
            this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.CancelButton");
            this._buttonCancel.Location = new System.Drawing.Point(209, 110);
            this._buttonCancel.Margin = new System.Windows.Forms.Padding(4, 12, 0, 0);
            this._buttonCancel.Name = "_buttonCancel";
            this._buttonCancel.Size = new System.Drawing.Size(75, 26);
            this._buttonCancel.TabIndex = 1;
            this._buttonCancel.Text = "Cancel";
            this._buttonCancel.UseVisualStyleBackColor = true;
            // 
            // _buttonExport
            // 
            this._buttonExport.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._buttonExport.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this._buttonExport, null);
            this.locExtender.SetLocalizationComment(this._buttonExport, null);
			this.locExtender.SetLocalizingId(this._buttonExport, "DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.ExportButton");
            this._buttonExport.Location = new System.Drawing.Point(126, 110);
            this._buttonExport.Margin = new System.Windows.Forms.Padding(0, 12, 4, 0);
            this._buttonExport.Name = "_buttonExport";
            this._buttonExport.Size = new System.Drawing.Size(75, 26);
            this._buttonExport.TabIndex = 2;
            this._buttonExport.Text = "Export...";
            this._buttonExport.UseVisualStyleBackColor = true;
            this._buttonExport.Click += new System.EventHandler(this.HandleExportButtonClick);
            // 
            // _labelTranscriptionColumnHeadingText
            // 
            this._labelTranscriptionColumnHeadingText.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._labelTranscriptionColumnHeadingText.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this._labelTranscriptionColumnHeadingText, null);
            this.locExtender.SetLocalizationComment(this._labelTranscriptionColumnHeadingText, null);
			this.locExtender.SetLocalizingId(this._labelTranscriptionColumnHeadingText, "DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.TranscriptionLabel");
            this._labelTranscriptionColumnHeadingText.Location = new System.Drawing.Point(0, 45);
            this._labelTranscriptionColumnHeadingText.Margin = new System.Windows.Forms.Padding(0);
            this._labelTranscriptionColumnHeadingText.Name = "_labelTranscriptionColumnHeadingText";
            this._labelTranscriptionColumnHeadingText.Size = new System.Drawing.Size(24, 13);
            this._labelTranscriptionColumnHeadingText.TabIndex = 3;
            this._labelTranscriptionColumnHeadingText.Text = "{0}:";
            // 
            // _comboTranscriptionWs
            // 
            this._comboTranscriptionWs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._tableLayout.SetColumnSpan(this._comboTranscriptionWs, 3);
            this._comboTranscriptionWs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboTranscriptionWs.FormattingEnabled = true;
            this.locExtender.SetLocalizableToolTip(this._comboTranscriptionWs, null);
            this.locExtender.SetLocalizationComment(this._comboTranscriptionWs, null);
            this.locExtender.SetLocalizationPriority(this._comboTranscriptionWs, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._comboTranscriptionWs, "_comboTranscriptionWs");
            this._comboTranscriptionWs.Location = new System.Drawing.Point(27, 41);
            this._comboTranscriptionWs.Margin = new System.Windows.Forms.Padding(3, 5, 0, 5);
            this._comboTranscriptionWs.Name = "_comboTranscriptionWs";
            this._comboTranscriptionWs.Size = new System.Drawing.Size(257, 21);
            this._comboTranscriptionWs.TabIndex = 4;
            this._comboTranscriptionWs.SelectionChangeCommitted += new System.EventHandler(this.HandleWritingSystemChanged);
            // 
            // _labelFreeTranslationColumnHeadingText
            // 
            this._labelFreeTranslationColumnHeadingText.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._labelFreeTranslationColumnHeadingText.AutoSize = true;
            this.locExtender.SetLocalizableToolTip(this._labelFreeTranslationColumnHeadingText, null);
            this.locExtender.SetLocalizationComment(this._labelFreeTranslationColumnHeadingText, null);
			this.locExtender.SetLocalizingId(this._labelFreeTranslationColumnHeadingText, "DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.FreeTranslationLabel");
            this._labelFreeTranslationColumnHeadingText.Location = new System.Drawing.Point(0, 76);
            this._labelFreeTranslationColumnHeadingText.Margin = new System.Windows.Forms.Padding(0);
            this._labelFreeTranslationColumnHeadingText.Name = "_labelFreeTranslationColumnHeadingText";
            this._labelFreeTranslationColumnHeadingText.Size = new System.Drawing.Size(24, 13);
            this._labelFreeTranslationColumnHeadingText.TabIndex = 5;
            this._labelFreeTranslationColumnHeadingText.Text = "{0}:";
            // 
            // _comboTranslationWs
            // 
            this._comboTranslationWs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._tableLayout.SetColumnSpan(this._comboTranslationWs, 3);
            this._comboTranslationWs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._comboTranslationWs.FormattingEnabled = true;
            this.locExtender.SetLocalizableToolTip(this._comboTranslationWs, null);
            this.locExtender.SetLocalizationComment(this._comboTranslationWs, null);
            this.locExtender.SetLocalizationPriority(this._comboTranslationWs, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._comboTranslationWs, "_comboTranslationWs");
            this._comboTranslationWs.Location = new System.Drawing.Point(27, 72);
            this._comboTranslationWs.Margin = new System.Windows.Forms.Padding(3, 5, 0, 5);
            this._comboTranslationWs.Name = "_comboTranslationWs";
            this._comboTranslationWs.Size = new System.Drawing.Size(257, 21);
            this._comboTranslationWs.TabIndex = 6;
            this._comboTranslationWs.SelectionChangeCommitted += new System.EventHandler(this.HandleWritingSystemChanged);
            // 
            // _labelOverview
            // 
            this._labelOverview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._labelOverview.AutoSize = true;
            this._tableLayout.SetColumnSpan(this._labelOverview, 4);
            this.locExtender.SetLocalizableToolTip(this._labelOverview, null);
            this.locExtender.SetLocalizationComment(this._labelOverview, null);
			this.locExtender.SetLocalizingId(this._labelOverview, "DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.OverviewLabel");
            this._labelOverview.Location = new System.Drawing.Point(0, 0);
            this._labelOverview.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this._labelOverview.Name = "_labelOverview";
            this._labelOverview.Size = new System.Drawing.Size(284, 26);
            this._labelOverview.TabIndex = 7;
            this._labelOverview.Text = "Specify the writing systems for the transcriptions and free translations.";
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "SayMore";
            // 
            // ExportToFieldWorksInterlinearDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this._buttonCancel;
            this.ClientSize = new System.Drawing.Size(320, 146);
            this.Controls.Add(this._tableLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.locExtender.SetLocalizableToolTip(this, null);
            this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.ExportToFieldWorksInterlinearDlg.WindowTitle");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportToFieldWorksInterlinearDlg";
            this.Padding = new System.Windows.Forms.Padding(15);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Annotations For FieldWorks Interlinear";
            this.Load += new System.EventHandler(this.ExportToFieldWorksInterlinearDlg_Load);
            this._tableLayout.ResumeLayout(false);
            this._tableLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Button _buttonExport;
		private System.Windows.Forms.Label _labelTranscriptionColumnHeadingText;
		private System.Windows.Forms.ComboBox _comboTranscriptionWs;
		private System.Windows.Forms.Label _labelFreeTranslationColumnHeadingText;
		private System.Windows.Forms.ComboBox _comboTranslationWs;
		private System.Windows.Forms.Label _labelOverview;
		private Localization.UI.LocalizationExtender locExtender;
	}
}