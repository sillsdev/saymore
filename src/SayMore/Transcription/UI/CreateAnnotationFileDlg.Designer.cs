namespace SayMore.Transcription.UI
{
	partial class CreateAnnotationFileDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateAnnotationFileDlg));
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._labelElanOverview = new System.Windows.Forms.Label();
			this._labelAudacityOverview = new System.Windows.Forms.Label();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._labelOverview = new System.Windows.Forms.Label();
			this._labelAnnoatationType1 = new System.Windows.Forms.Label();
			this._labelAnnoatationType2 = new System.Windows.Forms.Label();
			this._buttonLoadAudacityLabelFile = new System.Windows.Forms.Button();
			this._buttonLoadEafFile = new System.Windows.Forms.Button();
			this._tableLayoutCopyELANFileButton = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutReadAudacityFileButton = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayout.SuspendLayout();
			this._tableLayoutCopyELANFileButton.SuspendLayout();
			this._tableLayoutReadAudacityFileButton.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayout.AutoSize = true;
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.Controls.Add(this._tableLayoutReadAudacityFileButton, 0, 4);
			this._tableLayout.Controls.Add(this._tableLayoutCopyELANFileButton, 0, 6);
			this._tableLayout.Controls.Add(this._labelElanOverview, 0, 5);
			this._tableLayout.Controls.Add(this._labelAudacityOverview, 0, 3);
			this._tableLayout.Controls.Add(this._buttonCancel, 1, 7);
			this._tableLayout.Controls.Add(this._labelOverview, 0, 0);
			this._tableLayout.Controls.Add(this._labelAnnoatationType1, 0, 1);
			this._tableLayout.Controls.Add(this._labelAnnoatationType2, 0, 2);
			this._tableLayout.Location = new System.Drawing.Point(18, 18);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 8;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(450, 280);
			this._tableLayout.TabIndex = 0;
			// 
			// _labelElanOverview
			// 
			this._labelElanOverview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelElanOverview.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelElanOverview, 2);
			this._labelElanOverview.Location = new System.Drawing.Point(0, 189);
			this._labelElanOverview.Margin = new System.Windows.Forms.Padding(0, 15, 0, 8);
			this._labelElanOverview.Name = "_labelElanOverview";
			this._labelElanOverview.Size = new System.Drawing.Size(450, 26);
			this._labelElanOverview.TabIndex = 6;
			this._labelElanOverview.Text = "Alternatively, you can add an ELAN \"eaf\" file which already contains segments and" +
    " annotations for this media file:";
			// 
			// _labelAudacityOverview
			// 
			this._labelAudacityOverview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelAudacityOverview.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelAudacityOverview, 2);
			this._labelAudacityOverview.Location = new System.Drawing.Point(0, 75);
			this._labelAudacityOverview.Margin = new System.Windows.Forms.Padding(0, 15, 0, 8);
			this._labelAudacityOverview.Name = "_labelAudacityOverview";
			this._labelAudacityOverview.Size = new System.Drawing.Size(450, 65);
			this._labelAudacityOverview.TabIndex = 4;
			this._labelAudacityOverview.Text = resources.GetString("_labelAudacityOverview.Text");
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonCancel.AutoSize = true;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._buttonCancel.Location = new System.Drawing.Point(375, 254);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(0);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 0;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _labelOverview
			// 
			this._labelOverview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOverview.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelOverview, 2);
			this._labelOverview.Location = new System.Drawing.Point(0, 0);
			this._labelOverview.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._labelOverview.Name = "_labelOverview";
			this._labelOverview.Size = new System.Drawing.Size(450, 26);
			this._labelOverview.TabIndex = 1;
			this._labelOverview.Text = "This will create a file to hold annotations of the media file \'{0}\'. SayMore curr" +
    "ently supports the following annotations:";
			// 
			// _labelAnnoatationType1
			// 
			this._labelAnnoatationType1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelAnnoatationType1.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelAnnoatationType1, 2);
			this._labelAnnoatationType1.Location = new System.Drawing.Point(15, 31);
			this._labelAnnoatationType1.Margin = new System.Windows.Forms.Padding(15, 0, 0, 3);
			this._labelAnnoatationType1.Name = "_labelAnnoatationType1";
			this._labelAnnoatationType1.Size = new System.Drawing.Size(435, 13);
			this._labelAnnoatationType1.TabIndex = 2;
			this._labelAnnoatationType1.Text = "· Transcription";
			// 
			// _labelAnnoatationType2
			// 
			this._labelAnnoatationType2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelAnnoatationType2.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelAnnoatationType2, 2);
			this._labelAnnoatationType2.Location = new System.Drawing.Point(15, 47);
			this._labelAnnoatationType2.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
			this._labelAnnoatationType2.Name = "_labelAnnoatationType2";
			this._labelAnnoatationType2.Size = new System.Drawing.Size(435, 13);
			this._labelAnnoatationType2.TabIndex = 3;
			this._labelAnnoatationType2.Text = "· Translation";
			// 
			// _buttonLoadAudacityLabelFile
			// 
			this._buttonLoadAudacityLabelFile.AutoSize = true;
			this._buttonLoadAudacityLabelFile.Location = new System.Drawing.Point(0, 0);
			this._buttonLoadAudacityLabelFile.Margin = new System.Windows.Forms.Padding(0);
			this._buttonLoadAudacityLabelFile.Name = "_buttonLoadAudacityLabelFile";
			this._buttonLoadAudacityLabelFile.Size = new System.Drawing.Size(175, 26);
			this._buttonLoadAudacityLabelFile.TabIndex = 5;
			this._buttonLoadAudacityLabelFile.Text = "Read Audacity Label Track File...";
			this._buttonLoadAudacityLabelFile.UseVisualStyleBackColor = true;
			this._buttonLoadAudacityLabelFile.Click += new System.EventHandler(this.HandleLoadAudacityLabelFileClick);
			// 
			// _buttonLoadEafFile
			// 
			this._buttonLoadEafFile.AutoSize = true;
			this._buttonLoadEafFile.Location = new System.Drawing.Point(0, 0);
			this._buttonLoadEafFile.Margin = new System.Windows.Forms.Padding(0);
			this._buttonLoadEafFile.Name = "_buttonLoadEafFile";
			this._buttonLoadEafFile.Size = new System.Drawing.Size(115, 26);
			this._buttonLoadEafFile.TabIndex = 7;
			this._buttonLoadEafFile.Text = "Copy an ELAN File...";
			this._buttonLoadEafFile.UseVisualStyleBackColor = true;
			this._buttonLoadEafFile.Click += new System.EventHandler(this.HandleLoadSegmentFileClick);
			// 
			// _tableLayoutCopyELANFileButton
			// 
			this._tableLayoutCopyELANFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutCopyELANFileButton.AutoSize = true;
			this._tableLayoutCopyELANFileButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutCopyELANFileButton.ColumnCount = 2;
			this._tableLayout.SetColumnSpan(this._tableLayoutCopyELANFileButton, 2);
			this._tableLayoutCopyELANFileButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutCopyELANFileButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutCopyELANFileButton.Controls.Add(this._buttonLoadEafFile, 0, 0);
			this._tableLayoutCopyELANFileButton.Location = new System.Drawing.Point(0, 223);
			this._tableLayoutCopyELANFileButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._tableLayoutCopyELANFileButton.Name = "_tableLayoutCopyELANFileButton";
			this._tableLayoutCopyELANFileButton.RowCount = 1;
			this._tableLayoutCopyELANFileButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutCopyELANFileButton.Size = new System.Drawing.Size(450, 26);
			this._tableLayoutCopyELANFileButton.TabIndex = 1;
			// 
			// _tableLayoutReadAudacityFileButton
			// 
			this._tableLayoutReadAudacityFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutReadAudacityFileButton.AutoSize = true;
			this._tableLayoutReadAudacityFileButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutReadAudacityFileButton.ColumnCount = 2;
			this._tableLayout.SetColumnSpan(this._tableLayoutReadAudacityFileButton, 2);
			this._tableLayoutReadAudacityFileButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutReadAudacityFileButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutReadAudacityFileButton.Controls.Add(this._buttonLoadAudacityLabelFile, 0, 0);
			this._tableLayoutReadAudacityFileButton.Location = new System.Drawing.Point(0, 148);
			this._tableLayoutReadAudacityFileButton.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutReadAudacityFileButton.Name = "_tableLayoutReadAudacityFileButton";
			this._tableLayoutReadAudacityFileButton.RowCount = 1;
			this._tableLayoutReadAudacityFileButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutReadAudacityFileButton.Size = new System.Drawing.Size(450, 26);
			this._tableLayoutReadAudacityFileButton.TabIndex = 2;
			// 
			// CreateAnnotationFileDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(486, 292);
			this.Controls.Add(this._tableLayout);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CreateAnnotationFileDlg";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Create Annotation File";
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this._tableLayoutCopyELANFileButton.ResumeLayout(false);
			this._tableLayoutCopyELANFileButton.PerformLayout();
			this._tableLayoutReadAudacityFileButton.ResumeLayout(false);
			this._tableLayoutReadAudacityFileButton.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Label _labelOverview;
		private System.Windows.Forms.Label _labelAnnoatationType1;
		private System.Windows.Forms.Label _labelAnnoatationType2;
		private System.Windows.Forms.Label _labelAudacityOverview;
		private System.Windows.Forms.Button _buttonLoadAudacityLabelFile;
		private System.Windows.Forms.Label _labelElanOverview;
		private System.Windows.Forms.Button _buttonLoadEafFile;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutReadAudacityFileButton;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutCopyELANFileButton;
	}
}