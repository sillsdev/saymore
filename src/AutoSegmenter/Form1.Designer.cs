namespace AutoSegmenter
{
	partial class Form1
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
			this._textBoxAudioFile = new System.Windows.Forms.TextBox();
			this._labelAudioFile = new System.Windows.Forms.Label();
			this._labelSilenceThreshold = new System.Windows.Forms.Label();
			this._labelClusterThreshold = new System.Windows.Forms.Label();
			this._textBoxSilenceThreshold = new System.Windows.Forms.TextBox();
			this._textBoxClusterThreshold = new System.Windows.Forms.TextBox();
			this._buttonBrowse = new System.Windows.Forms.Button();
			this._textBoxSegments = new System.Windows.Forms.TextBox();
			this._buttonClose = new System.Windows.Forms.Button();
			this._labelClusterDuration = new System.Windows.Forms.Label();
			this._textBoxClusterDuration = new System.Windows.Forms.TextBox();
			this._labelOnsetDectectionValues = new System.Windows.Forms.Label();
			this._textBoxOnsetDectectionValues = new System.Windows.Forms.TextBox();
			this._labelOnsetDetectionValuesHelp = new System.Windows.Forms.Label();
			this._buttonSegment = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._textBoxAudioFile, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this._labelAudioFile, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._labelSilenceThreshold, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._labelClusterThreshold, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this._textBoxSilenceThreshold, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this._textBoxClusterThreshold, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this._buttonBrowse, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this._textBoxSegments, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this._buttonClose, 2, 7);
			this.tableLayoutPanel1.Controls.Add(this._labelClusterDuration, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this._textBoxClusterDuration, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this._labelOnsetDectectionValues, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this._textBoxOnsetDectectionValues, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this._labelOnsetDetectionValuesHelp, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this._buttonSegment, 0, 7);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 8;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(441, 418);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _textBoxAudioFile
			// 
			this._textBoxAudioFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._textBoxAudioFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._textBoxAudioFile.Location = new System.Drawing.Point(141, 3);
			this._textBoxAudioFile.Name = "_textBoxAudioFile";
			this._textBoxAudioFile.ReadOnly = true;
			this._textBoxAudioFile.Size = new System.Drawing.Size(216, 23);
			this._textBoxAudioFile.TabIndex = 1;
			// 
			// _labelAudioFile
			// 
			this._labelAudioFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelAudioFile.AutoSize = true;
			this._labelAudioFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelAudioFile.Location = new System.Drawing.Point(3, 7);
			this._labelAudioFile.Name = "_labelAudioFile";
			this._labelAudioFile.Size = new System.Drawing.Size(63, 15);
			this._labelAudioFile.TabIndex = 0;
			this._labelAudioFile.Text = "Audio File:";
			// 
			// _labelSilenceThreshold
			// 
			this._labelSilenceThreshold.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelSilenceThreshold.AutoSize = true;
			this._labelSilenceThreshold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelSilenceThreshold.Location = new System.Drawing.Point(3, 36);
			this._labelSilenceThreshold.Name = "_labelSilenceThreshold";
			this._labelSilenceThreshold.Size = new System.Drawing.Size(103, 15);
			this._labelSilenceThreshold.TabIndex = 3;
			this._labelSilenceThreshold.Text = "&Silence Threshold:";
			// 
			// _labelClusterThreshold
			// 
			this._labelClusterThreshold.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelClusterThreshold.AutoSize = true;
			this._labelClusterThreshold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelClusterThreshold.Location = new System.Drawing.Point(3, 94);
			this._labelClusterThreshold.Name = "_labelClusterThreshold";
			this._labelClusterThreshold.Size = new System.Drawing.Size(103, 15);
			this._labelClusterThreshold.TabIndex = 7;
			this._labelClusterThreshold.Text = "Cluster &Threshold:";
			// 
			// _textBoxSilenceThreshold
			// 
			this._textBoxSilenceThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this._textBoxSilenceThreshold, 2);
			this._textBoxSilenceThreshold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._textBoxSilenceThreshold.Location = new System.Drawing.Point(141, 32);
			this._textBoxSilenceThreshold.Name = "_textBoxSilenceThreshold";
			this._textBoxSilenceThreshold.Size = new System.Drawing.Size(297, 23);
			this._textBoxSilenceThreshold.TabIndex = 4;
			// 
			// _textBoxClusterThreshold
			// 
			this._textBoxClusterThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this._textBoxClusterThreshold, 2);
			this._textBoxClusterThreshold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._textBoxClusterThreshold.Location = new System.Drawing.Point(141, 90);
			this._textBoxClusterThreshold.Name = "_textBoxClusterThreshold";
			this._textBoxClusterThreshold.Size = new System.Drawing.Size(297, 23);
			this._textBoxClusterThreshold.TabIndex = 8;
			// 
			// _buttonBrowse
			// 
			this._buttonBrowse.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._buttonBrowse.Location = new System.Drawing.Point(363, 3);
			this._buttonBrowse.Name = "_buttonBrowse";
			this._buttonBrowse.Size = new System.Drawing.Size(75, 23);
			this._buttonBrowse.TabIndex = 2;
			this._buttonBrowse.Text = "&Browse...";
			this._buttonBrowse.UseVisualStyleBackColor = true;
			this._buttonBrowse.Click += new System.EventHandler(this.HandleBrowseButtonClicked);
			// 
			// _textBoxSegments
			// 
			this._textBoxSegments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this._textBoxSegments, 3);
			this._textBoxSegments.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._textBoxSegments.Location = new System.Drawing.Point(3, 187);
			this._textBoxSegments.Multiline = true;
			this._textBoxSegments.Name = "_textBoxSegments";
			this._textBoxSegments.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this._textBoxSegments.Size = new System.Drawing.Size(435, 199);
			this._textBoxSegments.TabIndex = 12;
			// 
			// _buttonClose
			// 
			this._buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonClose.Location = new System.Drawing.Point(363, 392);
			this._buttonClose.Name = "_buttonClose";
			this._buttonClose.Size = new System.Drawing.Size(75, 23);
			this._buttonClose.TabIndex = 14;
			this._buttonClose.Text = "Close";
			this._buttonClose.UseVisualStyleBackColor = true;
			// 
			// _labelClusterDuration
			// 
			this._labelClusterDuration.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelClusterDuration.AutoSize = true;
			this._labelClusterDuration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelClusterDuration.Location = new System.Drawing.Point(3, 65);
			this._labelClusterDuration.Name = "_labelClusterDuration";
			this._labelClusterDuration.Size = new System.Drawing.Size(96, 15);
			this._labelClusterDuration.TabIndex = 5;
			this._labelClusterDuration.Text = "Cluster &Duration:";
			// 
			// _textBoxClusterDuration
			// 
			this._textBoxClusterDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this._textBoxClusterDuration, 2);
			this._textBoxClusterDuration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._textBoxClusterDuration.Location = new System.Drawing.Point(141, 61);
			this._textBoxClusterDuration.Name = "_textBoxClusterDuration";
			this._textBoxClusterDuration.Size = new System.Drawing.Size(297, 23);
			this._textBoxClusterDuration.TabIndex = 6;
			// 
			// _labelOnsetDectectionValues
			// 
			this._labelOnsetDectectionValues.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelOnsetDectectionValues.AutoSize = true;
			this._labelOnsetDectectionValues.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelOnsetDectectionValues.Location = new System.Drawing.Point(3, 123);
			this._labelOnsetDectectionValues.Name = "_labelOnsetDectectionValues";
			this._labelOnsetDectectionValues.Size = new System.Drawing.Size(132, 15);
			this._labelOnsetDectectionValues.TabIndex = 9;
			this._labelOnsetDectectionValues.Text = "&Onset Detection Values:";
			// 
			// _textBoxOnsetDectectionValues
			// 
			this._textBoxOnsetDectectionValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this._textBoxOnsetDectectionValues, 2);
			this._textBoxOnsetDectectionValues.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._textBoxOnsetDectectionValues.Location = new System.Drawing.Point(141, 119);
			this._textBoxOnsetDectectionValues.Name = "_textBoxOnsetDectectionValues";
			this._textBoxOnsetDectectionValues.Size = new System.Drawing.Size(297, 23);
			this._textBoxOnsetDectectionValues.TabIndex = 10;
			// 
			// _labelOnsetDetectionValuesHelp
			// 
			this._labelOnsetDetectionValuesHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelOnsetDetectionValuesHelp.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this._labelOnsetDetectionValuesHelp, 2);
			this._labelOnsetDetectionValuesHelp.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelOnsetDetectionValuesHelp.Location = new System.Drawing.Point(141, 145);
			this._labelOnsetDetectionValuesHelp.Name = "_labelOnsetDetectionValuesHelp";
			this._labelOnsetDetectionValuesHelp.Size = new System.Drawing.Size(297, 39);
			this._labelOnsetDetectionValuesHelp.TabIndex = 11;
			this._labelOnsetDetectionValuesHelp.Text = "Specify one or more floating-point onset detection values. When specifying more t" +
    "han one, separate values with a comma.";
			// 
			// _buttonSegment
			// 
			this._buttonSegment.Location = new System.Drawing.Point(3, 392);
			this._buttonSegment.Name = "_buttonSegment";
			this._buttonSegment.Size = new System.Drawing.Size(75, 23);
			this._buttonSegment.TabIndex = 13;
			this._buttonSegment.Text = "Segment";
			this._buttonSegment.UseVisualStyleBackColor = true;
			this._buttonSegment.Click += new System.EventHandler(this.HandleSegmentButtonClicked);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(461, 438);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "Form1";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Auto Segmenter Tool";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox _textBoxAudioFile;
		private System.Windows.Forms.Button _buttonBrowse;
		private System.Windows.Forms.Label _labelAudioFile;
		private System.Windows.Forms.Label _labelSilenceThreshold;
		private System.Windows.Forms.Label _labelOnsetDectectionValues;
		private System.Windows.Forms.TextBox _textBoxOnsetDectectionValues;
		private System.Windows.Forms.Label _labelClusterDuration;
		private System.Windows.Forms.Label _labelClusterThreshold;
		private System.Windows.Forms.TextBox _textBoxSilenceThreshold;
		private System.Windows.Forms.TextBox _textBoxClusterDuration;
		private System.Windows.Forms.TextBox _textBoxClusterThreshold;
		private System.Windows.Forms.TextBox _textBoxSegments;
		private System.Windows.Forms.Button _buttonClose;
		private System.Windows.Forms.Label _labelOnsetDetectionValuesHelp;
		private System.Windows.Forms.Button _buttonSegment;
	}
}

