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
			this.components = new System.ComponentModel.Container();
			this._labelProblemOverviewMsg = new SilTools.Controls.AutoHeightLabel();
			this._labelPossibleProblemsMsg1 = new SilTools.Controls.AutoHeightLabel();
			this._labelDriveLetterHintMsg = new SilTools.Controls.AutoHeightLabel();
			this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this._labelPossibleProblemsMsg2 = new SilTools.Controls.AutoHeightLabel();
			this._labelPossibleProblemsMsg3 = new SilTools.Controls.AutoHeightLabel();
			this.locExtender = new L10NSharp.UI.LocalizationExtender(this.components);
			this._tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			//
			// _labelProblemOverviewMsg
			//
			this._labelProblemOverviewMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._labelProblemOverviewMsg.AutoEllipsis = true;
			this._labelProblemOverviewMsg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelProblemOverviewMsg.Image = global::SayMore.Properties.Resources.kimidWarning;
			this._labelProblemOverviewMsg.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this._labelProblemOverviewMsg.LeftMargin = 10;
			this.locExtender.SetLocalizableToolTip(this._labelProblemOverviewMsg, null);
			this.locExtender.SetLocalizationComment(this._labelProblemOverviewMsg, null);
			this.locExtender.SetLocalizingId(this._labelProblemOverviewMsg, "DialogBoxes.NewSessionsFromFilesDlgFolderNotFoundMesssageBox.ProblemOverviewMsgLabel");
			this._labelProblemOverviewMsg.Location = new System.Drawing.Point(0, 0);
			this._labelProblemOverviewMsg.Margin = new System.Windows.Forms.Padding(0);
			this._labelProblemOverviewMsg.Name = "_labelProblemOverviewMsg";
			this._labelProblemOverviewMsg.Size = new System.Drawing.Size(390, 32);
			this._labelProblemOverviewMsg.TabIndex = 1;
			this._labelProblemOverviewMsg.Text = "SayMore cannot reach the folder where your files were last time. One of the follo" +
	"wing may be true:";
			//
			// _labelPossibleProblemsMsg1
			//
			this._labelPossibleProblemsMsg1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._labelPossibleProblemsMsg1.AutoEllipsis = true;
			this._labelPossibleProblemsMsg1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelPossibleProblemsMsg1.Image = null;
			this._labelPossibleProblemsMsg1.LeftMargin = 42;
			this.locExtender.SetLocalizableToolTip(this._labelPossibleProblemsMsg1, null);
			this.locExtender.SetLocalizationComment(this._labelPossibleProblemsMsg1, null);
			this.locExtender.SetLocalizingId(this._labelPossibleProblemsMsg1, "DialogBoxes.NewSessionsFromFilesDlgFolderNotFoundMesssageBox.PossibleProblemsMsgLabel1");
			this._labelPossibleProblemsMsg1.Location = new System.Drawing.Point(0, 42);
			this._labelPossibleProblemsMsg1.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._labelPossibleProblemsMsg1.Name = "_labelPossibleProblemsMsg1";
			this._labelPossibleProblemsMsg1.Size = new System.Drawing.Size(390, 15);
			this._labelPossibleProblemsMsg1.TabIndex = 2;
			this._labelPossibleProblemsMsg1.Text = "1) The device is not yet plugged in.";
			//
			// _labelDriveLetterHintMsg
			//
			this._labelDriveLetterHintMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._labelDriveLetterHintMsg.AutoEllipsis = true;
			this._labelDriveLetterHintMsg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelDriveLetterHintMsg.Image = null;
			this._labelDriveLetterHintMsg.LeftMargin = 65;
			this.locExtender.SetLocalizableToolTip(this._labelDriveLetterHintMsg, null);
			this.locExtender.SetLocalizationComment(this._labelDriveLetterHintMsg, null);
			this.locExtender.SetLocalizingId(this._labelDriveLetterHintMsg, "DialogBoxes.NewSessionsFromFilesDlgFolderNotFoundMesssageBox.DriveLetterHintMsgLabel");
			this._labelDriveLetterHintMsg.Location = new System.Drawing.Point(0, 102);
			this._labelDriveLetterHintMsg.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this._labelDriveLetterHintMsg.Name = "_labelDriveLetterHintMsg";
			this._labelDriveLetterHintMsg.Size = new System.Drawing.Size(390, 30);
			this._labelDriveLetterHintMsg.TabIndex = 4;
			this._labelDriveLetterHintMsg.Text = "In Windows, if you assign the device to a letter closer to \'z\', it is more likely" +
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
			this._tableLayoutPanel.Controls.Add(this._labelPossibleProblemsMsg2, 0, 2);
			this._tableLayoutPanel.Controls.Add(this._labelPossibleProblemsMsg1, 0, 1);
			this._tableLayoutPanel.Controls.Add(this._labelProblemOverviewMsg, 0, 0);
			this._tableLayoutPanel.Controls.Add(this._labelPossibleProblemsMsg3, 0, 4);
			this._tableLayoutPanel.Controls.Add(this._labelDriveLetterHintMsg, 0, 3);
			this._tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutPanel.Name = "_tableLayoutPanel";
			this._tableLayoutPanel.RowCount = 5;
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanel.Size = new System.Drawing.Size(390, 197);
			this._tableLayoutPanel.TabIndex = 6;
			//
			// _labelPossibleProblemsMsg2
			//
			this._labelPossibleProblemsMsg2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._labelPossibleProblemsMsg2.AutoEllipsis = true;
			this._labelPossibleProblemsMsg2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelPossibleProblemsMsg2.Image = null;
			this._labelPossibleProblemsMsg2.LeftMargin = 42;
			this.locExtender.SetLocalizableToolTip(this._labelPossibleProblemsMsg2, null);
			this.locExtender.SetLocalizationComment(this._labelPossibleProblemsMsg2, null);
			this.locExtender.SetLocalizingId(this._labelPossibleProblemsMsg2, "DialogBoxes.NewSessionsFromFilesDlgFolderNotFoundMesssageBox.PossibleProblemsMsgLabel2");
			this._labelPossibleProblemsMsg2.Location = new System.Drawing.Point(0, 67);
			this._labelPossibleProblemsMsg2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._labelPossibleProblemsMsg2.Name = "_labelPossibleProblemsMsg2";
			this._labelPossibleProblemsMsg2.Size = new System.Drawing.Size(390, 30);
			this._labelPossibleProblemsMsg2.TabIndex = 7;
			this._labelPossibleProblemsMsg2.Text = "2) The device is plugged in but is currently assigned a different drive letter f" +
	"rom \'{0}\'.";
			//
			// _labelPossibleProblemsMsg3
			//
			this._labelPossibleProblemsMsg3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._labelPossibleProblemsMsg3.AutoEllipsis = true;
			this._labelPossibleProblemsMsg3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelPossibleProblemsMsg3.Image = null;
			this._labelPossibleProblemsMsg3.LeftMargin = 42;
			this.locExtender.SetLocalizableToolTip(this._labelPossibleProblemsMsg3, null);
			this.locExtender.SetLocalizationComment(this._labelPossibleProblemsMsg3, null);
			this.locExtender.SetLocalizingId(this._labelPossibleProblemsMsg3, "DialogBoxes.NewSessionsFromFilesDlgFolderNotFoundMesssageBox.PossibleProblemsMsgLabel3");
			this._labelPossibleProblemsMsg3.Location = new System.Drawing.Point(0, 142);
			this._labelPossibleProblemsMsg3.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._labelPossibleProblemsMsg3.Name = "_labelPossibleProblemsMsg3";
			this._labelPossibleProblemsMsg3.Size = new System.Drawing.Size(390, 30);
			this._labelPossibleProblemsMsg3.TabIndex = 5;
			this._labelPossibleProblemsMsg3.Text = "3) Some part of the above path has changed (e.g. a folder name).";
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// NewSessionsFromFilesDlgFolderNotFoundMsg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayoutPanel);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.NewSessionsFromFilesDlgFolderNotFoundMesssageBox.WindowTitle");
			this.Name = "NewSessionsFromFilesDlgFolderNotFoundMsg";
			this.Size = new System.Drawing.Size(390, 200);
			this._tableLayoutPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SilTools.Controls.AutoHeightLabel _labelProblemOverviewMsg;
		private SilTools.Controls.AutoHeightLabel _labelPossibleProblemsMsg1;
		private SilTools.Controls.AutoHeightLabel _labelDriveLetterHintMsg;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
		private SilTools.Controls.AutoHeightLabel _labelPossibleProblemsMsg2;
		private SilTools.Controls.AutoHeightLabel _labelPossibleProblemsMsg3;
		private L10NSharp.UI.LocalizationExtender locExtender;
	}
}
