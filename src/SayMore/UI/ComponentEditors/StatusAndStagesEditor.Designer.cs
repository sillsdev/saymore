namespace SayMore.UI.ComponentEditors
{
	partial class StatusAndStagesEditor
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
			this._tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._labelStatus = new System.Windows.Forms.Label();
			this._radioIncoming = new System.Windows.Forms.RadioButton();
			this._radioInProgress = new System.Windows.Forms.RadioButton();
			this._radioFinished = new System.Windows.Forms.RadioButton();
			this._radioSkipped = new System.Windows.Forms.RadioButton();
			this._pictureIncoming = new System.Windows.Forms.PictureBox();
			this._pictureInProgress = new System.Windows.Forms.PictureBox();
			this._pictureFinished = new System.Windows.Forms.PictureBox();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this._labelStages = new System.Windows.Forms.Label();
			this._tableLayoutStatusHelp = new System.Windows.Forms.TableLayoutPanel();
			this._labelReadAboutStatus = new System.Windows.Forms.Label();
			this._buttonReadAboutStatus = new System.Windows.Forms.Button();
			this._labelStatusHint = new System.Windows.Forms.Label();
			this._labelReadAboutStages = new System.Windows.Forms.Label();
			this._labelStagesHint = new System.Windows.Forms.Label();
			this._buttonReadAboutStages = new System.Windows.Forms.Button();
			this._tableLayoutOuter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureIncoming)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureInProgress)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureFinished)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			this._tableLayoutStatusHelp.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tableLayoutOuter
			// 
			this._tableLayoutOuter.AutoSize = true;
			this._tableLayoutOuter.ColumnCount = 4;
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.Controls.Add(this._labelStatus, 0, 0);
			this._tableLayoutOuter.Controls.Add(this._radioIncoming, 1, 1);
			this._tableLayoutOuter.Controls.Add(this._radioInProgress, 1, 2);
			this._tableLayoutOuter.Controls.Add(this._radioFinished, 1, 3);
			this._tableLayoutOuter.Controls.Add(this._radioSkipped, 1, 4);
			this._tableLayoutOuter.Controls.Add(this._pictureIncoming, 0, 1);
			this._tableLayoutOuter.Controls.Add(this._pictureInProgress, 0, 2);
			this._tableLayoutOuter.Controls.Add(this._pictureFinished, 0, 3);
			this._tableLayoutOuter.Controls.Add(this.pictureBox3, 0, 4);
			this._tableLayoutOuter.Controls.Add(this._labelStages, 0, 5);
			this._tableLayoutOuter.Controls.Add(this._tableLayoutStatusHelp, 2, 1);
			this._tableLayoutOuter.Controls.Add(this._labelReadAboutStages, 2, 11);
			this._tableLayoutOuter.Controls.Add(this._labelStagesHint, 2, 6);
			this._tableLayoutOuter.Controls.Add(this._buttonReadAboutStages, 3, 11);
			this._tableLayoutOuter.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayoutOuter.Location = new System.Drawing.Point(15, 15);
			this._tableLayoutOuter.Name = "_tableLayoutOuter";
			this._tableLayoutOuter.RowCount = 12;
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.Size = new System.Drawing.Size(431, 252);
			this._tableLayoutOuter.TabIndex = 0;
			// 
			// _labelStatus
			// 
			this._labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelStatus.AutoSize = true;
			this._tableLayoutOuter.SetColumnSpan(this._labelStatus, 4);
			this._labelStatus.Location = new System.Drawing.Point(0, 0);
			this._labelStatus.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this._labelStatus.Name = "_labelStatus";
			this._labelStatus.Size = new System.Drawing.Size(431, 13);
			this._labelStatus.TabIndex = 0;
			this._labelStatus.Text = "Status";
			// 
			// _radioIncoming
			// 
			this._radioIncoming.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._radioIncoming.AutoSize = true;
			this._radioIncoming.Location = new System.Drawing.Point(31, 19);
			this._radioIncoming.Margin = new System.Windows.Forms.Padding(10, 4, 0, 2);
			this._radioIncoming.Name = "_radioIncoming";
			this._radioIncoming.Size = new System.Drawing.Size(78, 17);
			this._radioIncoming.TabIndex = 1;
			this._radioIncoming.TabStop = true;
			this._radioIncoming.Text = "Incoming";
			this._radioIncoming.UseVisualStyleBackColor = true;
			// 
			// _radioInProgress
			// 
			this._radioInProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._radioInProgress.AutoSize = true;
			this._radioInProgress.Location = new System.Drawing.Point(31, 40);
			this._radioInProgress.Margin = new System.Windows.Forms.Padding(10, 2, 0, 2);
			this._radioInProgress.Name = "_radioInProgress";
			this._radioInProgress.Size = new System.Drawing.Size(78, 17);
			this._radioInProgress.TabIndex = 2;
			this._radioInProgress.TabStop = true;
			this._radioInProgress.Text = "In Progress";
			this._radioInProgress.UseVisualStyleBackColor = true;
			// 
			// _radioFinished
			// 
			this._radioFinished.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._radioFinished.AutoSize = true;
			this._radioFinished.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._radioFinished.Location = new System.Drawing.Point(31, 61);
			this._radioFinished.Margin = new System.Windows.Forms.Padding(10, 2, 0, 2);
			this._radioFinished.Name = "_radioFinished";
			this._radioFinished.Size = new System.Drawing.Size(78, 17);
			this._radioFinished.TabIndex = 3;
			this._radioFinished.TabStop = true;
			this._radioFinished.Text = "Finished";
			this._radioFinished.UseVisualStyleBackColor = true;
			// 
			// _radioSkipped
			// 
			this._radioSkipped.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._radioSkipped.AutoSize = true;
			this._radioSkipped.Location = new System.Drawing.Point(31, 82);
			this._radioSkipped.Margin = new System.Windows.Forms.Padding(10, 2, 0, 2);
			this._radioSkipped.Name = "_radioSkipped";
			this._radioSkipped.Size = new System.Drawing.Size(78, 17);
			this._radioSkipped.TabIndex = 4;
			this._radioSkipped.TabStop = true;
			this._radioSkipped.Text = "Skipped";
			this._radioSkipped.UseVisualStyleBackColor = true;
			// 
			// _pictureIncoming
			// 
			this._pictureIncoming.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._pictureIncoming.Image = global::SayMore.Properties.Resources.StatusIncoming;
			this._pictureIncoming.Location = new System.Drawing.Point(5, 19);
			this._pictureIncoming.Margin = new System.Windows.Forms.Padding(5, 2, 0, 0);
			this._pictureIncoming.Name = "_pictureIncoming";
			this._pictureIncoming.Size = new System.Drawing.Size(16, 16);
			this._pictureIncoming.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureIncoming.TabIndex = 5;
			this._pictureIncoming.TabStop = false;
			// 
			// _pictureInProgress
			// 
			this._pictureInProgress.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._pictureInProgress.Image = global::SayMore.Properties.Resources.StatusIn_Progress;
			this._pictureInProgress.Location = new System.Drawing.Point(5, 40);
			this._pictureInProgress.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this._pictureInProgress.Name = "_pictureInProgress";
			this._pictureInProgress.Size = new System.Drawing.Size(16, 16);
			this._pictureInProgress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureInProgress.TabIndex = 6;
			this._pictureInProgress.TabStop = false;
			// 
			// _pictureFinished
			// 
			this._pictureFinished.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._pictureFinished.Image = global::SayMore.Properties.Resources.StatusFinished;
			this._pictureFinished.Location = new System.Drawing.Point(5, 61);
			this._pictureFinished.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this._pictureFinished.Name = "_pictureFinished";
			this._pictureFinished.Size = new System.Drawing.Size(16, 16);
			this._pictureFinished.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureFinished.TabIndex = 7;
			this._pictureFinished.TabStop = false;
			// 
			// pictureBox3
			// 
			this.pictureBox3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.pictureBox3.Image = global::SayMore.Properties.Resources.StatusSkipped;
			this.pictureBox3.Location = new System.Drawing.Point(5, 82);
			this.pictureBox3.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(16, 16);
			this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox3.TabIndex = 8;
			this.pictureBox3.TabStop = false;
			// 
			// _labelStages
			// 
			this._labelStages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelStages.AutoSize = true;
			this._tableLayoutOuter.SetColumnSpan(this._labelStages, 4);
			this._labelStages.Location = new System.Drawing.Point(0, 113);
			this._labelStages.Margin = new System.Windows.Forms.Padding(0, 12, 0, 4);
			this._labelStages.Name = "_labelStages";
			this._labelStages.Size = new System.Drawing.Size(431, 13);
			this._labelStages.TabIndex = 9;
			this._labelStages.Text = "Stages";
			// 
			// _tableLayoutStatusHelp
			// 
			this._tableLayoutStatusHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutStatusHelp.AutoSize = true;
			this._tableLayoutStatusHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutStatusHelp.ColumnCount = 2;
			this._tableLayoutOuter.SetColumnSpan(this._tableLayoutStatusHelp, 2);
			this._tableLayoutStatusHelp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutStatusHelp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutStatusHelp.Controls.Add(this._labelReadAboutStatus, 0, 1);
			this._tableLayoutStatusHelp.Controls.Add(this._buttonReadAboutStatus, 1, 1);
			this._tableLayoutStatusHelp.Controls.Add(this._labelStatusHint, 0, 0);
			this._tableLayoutStatusHelp.Location = new System.Drawing.Point(184, 15);
			this._tableLayoutStatusHelp.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
			this._tableLayoutStatusHelp.Name = "_tableLayoutStatusHelp";
			this._tableLayoutStatusHelp.RowCount = 2;
			this._tableLayoutOuter.SetRowSpan(this._tableLayoutStatusHelp, 4);
			this._tableLayoutStatusHelp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutStatusHelp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutStatusHelp.Size = new System.Drawing.Size(247, 40);
			this._tableLayoutStatusHelp.TabIndex = 26;
			// 
			// _labelReadAboutStatus
			// 
			this._labelReadAboutStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelReadAboutStatus.AutoSize = true;
			this._labelReadAboutStatus.Location = new System.Drawing.Point(77, 19);
			this._labelReadAboutStatus.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
			this._labelReadAboutStatus.Name = "_labelReadAboutStatus";
			this._labelReadAboutStatus.Size = new System.Drawing.Size(142, 13);
			this._labelReadAboutStatus.TabIndex = 25;
			this._labelReadAboutStatus.Text = "Read About SayMore Status";
			// 
			// _buttonReadAboutStatus
			// 
			this._buttonReadAboutStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonReadAboutStatus.AutoSize = true;
			this._buttonReadAboutStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonReadAboutStatus.BackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStatus.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonReadAboutStatus.FlatAppearance.BorderSize = 0;
			this._buttonReadAboutStatus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStatus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonReadAboutStatus.Image = global::SayMore.Properties.Resources.Help;
			this._buttonReadAboutStatus.Location = new System.Drawing.Point(222, 15);
			this._buttonReadAboutStatus.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this._buttonReadAboutStatus.Name = "_buttonReadAboutStatus";
			this._buttonReadAboutStatus.Size = new System.Drawing.Size(22, 22);
			this._buttonReadAboutStatus.TabIndex = 24;
			this._buttonReadAboutStatus.UseVisualStyleBackColor = false;
			// 
			// _labelStatusHint
			// 
			this._labelStatusHint.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._labelStatusHint.AutoSize = true;
			this._tableLayoutStatusHelp.SetColumnSpan(this._labelStatusHint, 2);
			this._labelStatusHint.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this._labelStatusHint.Location = new System.Drawing.Point(0, 0);
			this._labelStatusHint.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
			this._labelStatusHint.Name = "_labelStatusHint";
			this._labelStatusHint.Size = new System.Drawing.Size(247, 13);
			this._labelStatusHint.TabIndex = 22;
			this._labelStatusHint.Text = "Use the status to mark the big picture of this event.";
			// 
			// _labelReadAboutStages
			// 
			this._labelReadAboutStages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelReadAboutStages.AutoSize = true;
			this._labelReadAboutStages.Location = new System.Drawing.Point(258, 234);
			this._labelReadAboutStages.Margin = new System.Windows.Forms.Padding(15, 4, 0, 0);
			this._labelReadAboutStages.Name = "_labelReadAboutStages";
			this._labelReadAboutStages.Size = new System.Drawing.Size(145, 13);
			this._labelReadAboutStages.TabIndex = 27;
			this._labelReadAboutStages.Text = "Read About SayMore Stages";
			// 
			// _labelStagesHint
			// 
			this._labelStagesHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelStagesHint.AutoSize = true;
			this._tableLayoutOuter.SetColumnSpan(this._labelStagesHint, 2);
			this._labelStagesHint.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this._labelStagesHint.Location = new System.Drawing.Point(133, 134);
			this._labelStagesHint.Margin = new System.Windows.Forms.Padding(15, 4, 0, 0);
			this._labelStagesHint.Name = "_labelStagesHint";
			this._tableLayoutOuter.SetRowSpan(this._labelStagesHint, 5);
			this._labelStagesHint.Size = new System.Drawing.Size(298, 39);
			this._labelStagesHint.TabIndex = 28;
			this._labelStagesHint.Text = "Stages are normally automatic indicators of what  has been done, based on file na" +
    "mes and annotation work you\'ve done. Click any item to take control of this indi" +
    "cator.";
			// 
			// _buttonReadAboutStages
			// 
			this._buttonReadAboutStages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonReadAboutStages.AutoSize = true;
			this._buttonReadAboutStages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonReadAboutStages.BackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStages.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonReadAboutStages.FlatAppearance.BorderSize = 0;
			this._buttonReadAboutStages.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStages.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonReadAboutStages.Image = global::SayMore.Properties.Resources.Help;
			this._buttonReadAboutStages.Location = new System.Drawing.Point(406, 230);
			this._buttonReadAboutStages.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this._buttonReadAboutStages.Name = "_buttonReadAboutStages";
			this._buttonReadAboutStages.Size = new System.Drawing.Size(22, 22);
			this._buttonReadAboutStages.TabIndex = 29;
			this._buttonReadAboutStages.UseVisualStyleBackColor = false;
			// 
			// StatusAndStagesEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayoutOuter);
			this.Name = "StatusAndStagesEditor";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.Size = new System.Drawing.Size(461, 304);
			this._tableLayoutOuter.ResumeLayout(false);
			this._tableLayoutOuter.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureIncoming)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureInProgress)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureFinished)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			this._tableLayoutStatusHelp.ResumeLayout(false);
			this._tableLayoutStatusHelp.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
		private System.Windows.Forms.Label _labelStatus;
		private System.Windows.Forms.RadioButton _radioIncoming;
		private System.Windows.Forms.RadioButton _radioInProgress;
		private System.Windows.Forms.RadioButton _radioFinished;
		private System.Windows.Forms.RadioButton _radioSkipped;
		private System.Windows.Forms.PictureBox _pictureIncoming;
		private System.Windows.Forms.PictureBox _pictureInProgress;
		private System.Windows.Forms.PictureBox _pictureFinished;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.Label _labelStages;
		private System.Windows.Forms.Label _labelStatusHint;
		private System.Windows.Forms.Button _buttonReadAboutStatus;
		private System.Windows.Forms.Label _labelReadAboutStatus;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutStatusHelp;
		private System.Windows.Forms.Label _labelReadAboutStages;
		private System.Windows.Forms.Label _labelStagesHint;
		private System.Windows.Forms.Button _buttonReadAboutStages;
	}
}
