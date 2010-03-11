namespace SIL.Sponge.Dialogs
{
	partial class NewSessionsFromFilesDlgFolderNotFoundMsg
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
			this.m_problemOverviewMsgLabel = new SilUtils.Controls.AutoHeightLabel();
			this.m_possibleProblemsMsg1Label = new SilUtils.Controls.AutoHeightLabel();
			this.m_driveLetterHintMsgLabel = new SilUtils.Controls.AutoHeightLabel();
			this.m_tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.m_possibleProblemsMsg3Label = new SilUtils.Controls.AutoHeightLabel();
			this.m_possibleProblemsMsg2Label = new SilUtils.Controls.AutoHeightLabel();
			this.m_tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_problemOverviewMsgLabel
			// 
			this.m_problemOverviewMsgLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_problemOverviewMsgLabel.AutoEllipsis = true;
			this.m_problemOverviewMsgLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_problemOverviewMsgLabel.Image = global::SIL.Sponge.Properties.Resources.kimidWarning;
			this.m_problemOverviewMsgLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.m_problemOverviewMsgLabel.LeftMargin = 10;
			this.m_problemOverviewMsgLabel.Location = new System.Drawing.Point(0, 0);
			this.m_problemOverviewMsgLabel.Margin = new System.Windows.Forms.Padding(0);
			this.m_problemOverviewMsgLabel.Name = "m_problemOverviewMsgLabel";
			this.m_problemOverviewMsgLabel.Size = new System.Drawing.Size(390, 30);
			this.m_problemOverviewMsgLabel.TabIndex = 1;
			this.m_problemOverviewMsgLabel.Text = "{0} cannot reach the folder where your files were last time. One of the following" +
				" may be true:";
			// 
			// m_possibleProblemsMsg1Label
			// 
			this.m_possibleProblemsMsg1Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_possibleProblemsMsg1Label.AutoEllipsis = true;
			this.m_possibleProblemsMsg1Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_possibleProblemsMsg1Label.LeftMargin = 42;
			this.m_possibleProblemsMsg1Label.Location = new System.Drawing.Point(0, 40);
			this.m_possibleProblemsMsg1Label.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.m_possibleProblemsMsg1Label.Name = "m_possibleProblemsMsg1Label";
			this.m_possibleProblemsMsg1Label.Size = new System.Drawing.Size(390, 15);
			this.m_possibleProblemsMsg1Label.TabIndex = 2;
			this.m_possibleProblemsMsg1Label.Text = "1) The device is not yet plugged in.";
			// 
			// m_driveLetterHintMsgLabel
			// 
			this.m_driveLetterHintMsgLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_driveLetterHintMsgLabel.AutoEllipsis = true;
			this.m_driveLetterHintMsgLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_driveLetterHintMsgLabel.LeftMargin = 65;
			this.m_driveLetterHintMsgLabel.Location = new System.Drawing.Point(0, 100);
			this.m_driveLetterHintMsgLabel.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.m_driveLetterHintMsgLabel.Name = "m_driveLetterHintMsgLabel";
			this.m_driveLetterHintMsgLabel.Size = new System.Drawing.Size(390, 30);
			this.m_driveLetterHintMsgLabel.TabIndex = 4;
			this.m_driveLetterHintMsgLabel.Text = "In Windows, if you assign the device to a letter closer to \'z\', it is more likely" +
				" to use that letter each time.";
			// 
			// m_tableLayoutPanel
			// 
			this.m_tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_tableLayoutPanel.AutoScroll = true;
			this.m_tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.m_tableLayoutPanel.ColumnCount = 1;
			this.m_tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.m_tableLayoutPanel.Controls.Add(this.m_possibleProblemsMsg2Label, 0, 2);
			this.m_tableLayoutPanel.Controls.Add(this.m_possibleProblemsMsg1Label, 0, 1);
			this.m_tableLayoutPanel.Controls.Add(this.m_problemOverviewMsgLabel, 0, 0);
			this.m_tableLayoutPanel.Controls.Add(this.m_possibleProblemsMsg3Label, 0, 4);
			this.m_tableLayoutPanel.Controls.Add(this.m_driveLetterHintMsgLabel, 0, 3);
			this.m_tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.m_tableLayoutPanel.Name = "m_tableLayoutPanel";
			this.m_tableLayoutPanel.RowCount = 5;
			this.m_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.m_tableLayoutPanel.Size = new System.Drawing.Size(390, 200);
			this.m_tableLayoutPanel.TabIndex = 6;
			// 
			// m_possibleProblemsMsg3Label
			// 
			this.m_possibleProblemsMsg3Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_possibleProblemsMsg3Label.AutoEllipsis = true;
			this.m_possibleProblemsMsg3Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_possibleProblemsMsg3Label.LeftMargin = 42;
			this.m_possibleProblemsMsg3Label.Location = new System.Drawing.Point(0, 140);
			this.m_possibleProblemsMsg3Label.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.m_possibleProblemsMsg3Label.Name = "m_possibleProblemsMsg3Label";
			this.m_possibleProblemsMsg3Label.Size = new System.Drawing.Size(390, 30);
			this.m_possibleProblemsMsg3Label.TabIndex = 5;
			this.m_possibleProblemsMsg3Label.Text = "3) Some part of the above path has  changed (e.g. a folder name).";
			// 
			// m_possibleProblemsMsg2Label
			// 
			this.m_possibleProblemsMsg2Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_possibleProblemsMsg2Label.AutoEllipsis = true;
			this.m_possibleProblemsMsg2Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.m_possibleProblemsMsg2Label.LeftMargin = 42;
			this.m_possibleProblemsMsg2Label.Location = new System.Drawing.Point(0, 65);
			this.m_possibleProblemsMsg2Label.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.m_possibleProblemsMsg2Label.Name = "m_possibleProblemsMsg2Label";
			this.m_possibleProblemsMsg2Label.Size = new System.Drawing.Size(390, 30);
			this.m_possibleProblemsMsg2Label.TabIndex = 7;
			this.m_possibleProblemsMsg2Label.Text = "2) The device is plugged in, but is currently assigned a different drive letter f" +
				"rom \'{0}\'.";
			// 
			// NewSessionsFromFilesDlgFolderNotFoundMsg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_tableLayoutPanel);
			this.Name = "NewSessionsFromFilesDlgFolderNotFoundMsg";
			this.Size = new System.Drawing.Size(390, 200);
			this.m_tableLayoutPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private SilUtils.Controls.AutoHeightLabel m_problemOverviewMsgLabel;
		private SilUtils.Controls.AutoHeightLabel m_possibleProblemsMsg1Label;
		private SilUtils.Controls.AutoHeightLabel m_driveLetterHintMsgLabel;
		private System.Windows.Forms.TableLayoutPanel m_tableLayoutPanel;
		private SilUtils.Controls.AutoHeightLabel m_possibleProblemsMsg2Label;
		private SilUtils.Controls.AutoHeightLabel m_possibleProblemsMsg3Label;
	}
}
