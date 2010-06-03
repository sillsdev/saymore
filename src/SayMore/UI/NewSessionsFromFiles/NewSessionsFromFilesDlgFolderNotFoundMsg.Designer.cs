namespace SayMore.UI.NewSessionsFromFiles
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
			this._problemOverviewMsgLabel = new SilUtils.Controls.AutoHeightLabel();
			this._possibleProblemsMsg1Label = new SilUtils.Controls.AutoHeightLabel();
			this._driveLetterHintMsgLabel = new SilUtils.Controls.AutoHeightLabel();
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this._possibleProblemsMsg3Label = new SilUtils.Controls.AutoHeightLabel();
			this._possibleProblemsMsg2Label = new SilUtils.Controls.AutoHeightLabel();
			this._tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _problemOverviewMsgLabel
			// 
			this._problemOverviewMsgLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._problemOverviewMsgLabel.AutoEllipsis = true;
			this._problemOverviewMsgLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._problemOverviewMsgLabel.Image = Properties.Resources.kimidWarning;
			this._problemOverviewMsgLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this._problemOverviewMsgLabel.LeftMargin = 10;
			this._problemOverviewMsgLabel.Location = new System.Drawing.Point(0, 0);
			this._problemOverviewMsgLabel.Margin = new System.Windows.Forms.Padding(0);
			this._problemOverviewMsgLabel.Name = "_problemOverviewMsgLabel";
			this._problemOverviewMsgLabel.Size = new System.Drawing.Size(390, 30);
			this._problemOverviewMsgLabel.TabIndex = 1;
			this._problemOverviewMsgLabel.Text = "{0} cannot reach the folder where your files were last time. One of the following" +
				" may be true:";
			// 
			// _possibleProblemsMsg1Label
			// 
			this._possibleProblemsMsg1Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._possibleProblemsMsg1Label.AutoEllipsis = true;
			this._possibleProblemsMsg1Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._possibleProblemsMsg1Label.LeftMargin = 42;
			this._possibleProblemsMsg1Label.Location = new System.Drawing.Point(0, 40);
			this._possibleProblemsMsg1Label.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._possibleProblemsMsg1Label.Name = "_possibleProblemsMsg1Label";
			this._possibleProblemsMsg1Label.Size = new System.Drawing.Size(390, 15);
			this._possibleProblemsMsg1Label.TabIndex = 2;
			this._possibleProblemsMsg1Label.Text = "1) The device is not yet plugged in.";
			// 
			// _driveLetterHintMsgLabel
			// 
			this._driveLetterHintMsgLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._driveLetterHintMsgLabel.AutoEllipsis = true;
			this._driveLetterHintMsgLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._driveLetterHintMsgLabel.LeftMargin = 65;
			this._driveLetterHintMsgLabel.Location = new System.Drawing.Point(0, 100);
			this._driveLetterHintMsgLabel.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this._driveLetterHintMsgLabel.Name = "_driveLetterHintMsgLabel";
			this._driveLetterHintMsgLabel.Size = new System.Drawing.Size(390, 30);
			this._driveLetterHintMsgLabel.TabIndex = 4;
			this._driveLetterHintMsgLabel.Text = "In Windows, if you assign the device to a letter closer to \'z\', it is more likely" +
				" to use that letter each time.";
			// 
			// _tableLayoutPanel
			// 
			this._tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutPanel.AutoScroll = true;
			this._tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutPanel.ColumnCount = 1;
			this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanel.Controls.Add(this._possibleProblemsMsg2Label, 0, 2);
			this._tableLayoutPanel.Controls.Add(this._possibleProblemsMsg1Label, 0, 1);
			this._tableLayoutPanel.Controls.Add(this._problemOverviewMsgLabel, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._possibleProblemsMsg3Label, 0, 4);
			this._tableLayoutPanel.Controls.Add(this._driveLetterHintMsgLabel, 0, 3);
			this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 5;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.Size = new System.Drawing.Size(390, 200);
			this._tableLayoutPanel.TabIndex = 6;
			// 
			// _possibleProblemsMsg3Label
			// 
			this._possibleProblemsMsg3Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._possibleProblemsMsg3Label.AutoEllipsis = true;
			this._possibleProblemsMsg3Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._possibleProblemsMsg3Label.LeftMargin = 42;
			this._possibleProblemsMsg3Label.Location = new System.Drawing.Point(0, 140);
			this._possibleProblemsMsg3Label.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._possibleProblemsMsg3Label.Name = "_possibleProblemsMsg3Label";
			this._possibleProblemsMsg3Label.Size = new System.Drawing.Size(390, 30);
			this._possibleProblemsMsg3Label.TabIndex = 5;
			this._possibleProblemsMsg3Label.Text = "3) Some part of the above path has  changed (e.g. a folder name).";
			// 
			// _possibleProblemsMsg2Label
			// 
			this._possibleProblemsMsg2Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._possibleProblemsMsg2Label.AutoEllipsis = true;
			this._possibleProblemsMsg2Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._possibleProblemsMsg2Label.LeftMargin = 42;
			this._possibleProblemsMsg2Label.Location = new System.Drawing.Point(0, 65);
			this._possibleProblemsMsg2Label.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._possibleProblemsMsg2Label.Name = "_possibleProblemsMsg2Label";
			this._possibleProblemsMsg2Label.Size = new System.Drawing.Size(390, 30);
			this._possibleProblemsMsg2Label.TabIndex = 7;
			this._possibleProblemsMsg2Label.Text = "2) The device is plugged in, but is currently assigned a different drive letter f" +
				"rom \'{0}\'.";
			// 
			// NewSessionsFromFilesDlgFolderNotFoundMsg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayoutPanel);
			this.Name = "NewSessionsFromFilesDlgFolderNotFoundMsg";
			this.Size = new System.Drawing.Size(390, 200);
			this._tableLayoutPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private SilUtils.Controls.AutoHeightLabel _problemOverviewMsgLabel;
		private SilUtils.Controls.AutoHeightLabel _possibleProblemsMsg1Label;
		private SilUtils.Controls.AutoHeightLabel _driveLetterHintMsgLabel;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private SilUtils.Controls.AutoHeightLabel _possibleProblemsMsg2Label;
		private SilUtils.Controls.AutoHeightLabel _possibleProblemsMsg3Label;
	}
}
