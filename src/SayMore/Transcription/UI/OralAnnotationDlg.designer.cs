using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class OralAnnotationDlg
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._labelRecordingType = new System.Windows.Forms.Label();
			this._oralAnnotationRecorder = new SayMore.Transcription.UI.OralAnnotationRecorder();
			this._buttonClose = new System.Windows.Forms.Button();
			this._labelRecordingFormat = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			//
			// tableLayoutPanel1
			//
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._labelRecordingFormat, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this._labelRecordingType, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._oralAnnotationRecorder, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._buttonClose, 1, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(294, 237);
			this.tableLayoutPanel1.TabIndex = 1;
			//
			// _labelRecordingType
			//
			this._labelRecordingType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelRecordingType.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this._labelRecordingType, 2);
			this._labelRecordingType.Location = new System.Drawing.Point(3, 0);
			this._labelRecordingType.Name = "_labelRecordingType";
			this._labelRecordingType.Size = new System.Drawing.Size(288, 13);
			this._labelRecordingType.TabIndex = 0;
			this._labelRecordingType.Text = "#";
			//
			// _oralAnnotationRecorder
			//
			this._oralAnnotationRecorder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._oralAnnotationRecorder.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.SetColumnSpan(this._oralAnnotationRecorder, 2);
			this._oralAnnotationRecorder.Location = new System.Drawing.Point(3, 13);
			this._oralAnnotationRecorder.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._oralAnnotationRecorder.Name = "_oralAnnotationRecorder";
			this._oralAnnotationRecorder.Size = new System.Drawing.Size(291, 190);
			this._oralAnnotationRecorder.TabIndex = 1;
			//
			// _buttonClose
			//
			this._buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonClose.AutoSize = true;
			this._buttonClose.Location = new System.Drawing.Point(219, 211);
			this._buttonClose.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this._buttonClose.Name = "_buttonClose";
			this._buttonClose.Size = new System.Drawing.Size(75, 26);
			this._buttonClose.TabIndex = 2;
			this._buttonClose.Text = "Close";
			this._buttonClose.UseVisualStyleBackColor = true;
			this._buttonClose.Click += new System.EventHandler(this.HandleCloseClick);
			//
			// _labelRecordingFormat
			//
			this._labelRecordingFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._labelRecordingFormat.AutoEllipsis = true;
			this._labelRecordingFormat.AutoSize = true;
			this._labelRecordingFormat.ForeColor = System.Drawing.Color.DarkRed;
			this._labelRecordingFormat.Location = new System.Drawing.Point(3, 224);
			this._labelRecordingFormat.Name = "_labelRecordingFormat";
			this._labelRecordingFormat.Size = new System.Drawing.Size(213, 13);
			this._labelRecordingFormat.TabIndex = 2;
			this._labelRecordingFormat.Text = "Format: {0} bits, {1} Hz";
			//
			// OralAnnotationDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(318, 261);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(330, 250);
			this.Name = "OralAnnotationDlg";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Record";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private OralAnnotationRecorder _oralAnnotationRecorder;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label _labelRecordingType;
		private System.Windows.Forms.Button _buttonClose;
		private System.Windows.Forms.Label _labelRecordingFormat;




	}
}
