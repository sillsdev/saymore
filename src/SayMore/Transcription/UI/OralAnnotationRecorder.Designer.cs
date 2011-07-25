namespace SayMore.Transcription.UI
{
	partial class OralAnnotationRecorder
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
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._labelSegmentNumber = new System.Windows.Forms.Label();
			this._buttonEraseAnnotation = new SilTools.Controls.NicerButton();
			this._buttonPlayOriginal = new SayMore.Transcription.UI.StartStopButton();
			this._buttonPlayAnnotation = new SayMore.Transcription.UI.StartStopButton();
			this._buttonRecord = new SayMore.Transcription.UI.StartStopButton();
			this._trackBarSegment = new System.Windows.Forms.TrackBar();
			this._panelMicorphoneLevel = new SilTools.Controls.SilPanel();
			this._tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._trackBarSegment)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.ColumnCount = 1;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._labelSegmentNumber, 0, 1);
			this._tableLayout.Controls.Add(this._buttonEraseAnnotation, 0, 5);
			this._tableLayout.Controls.Add(this._buttonPlayOriginal, 0, 2);
			this._tableLayout.Controls.Add(this._buttonPlayAnnotation, 0, 4);
			this._tableLayout.Controls.Add(this._buttonRecord, 0, 3);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(0, 40);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 6;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(239, 167);
			this._tableLayout.TabIndex = 0;
			// 
			// _labelSegmentNumber
			// 
			this._labelSegmentNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelSegmentNumber.AutoSize = true;
			this._labelSegmentNumber.BackColor = System.Drawing.Color.Transparent;
			this._labelSegmentNumber.Location = new System.Drawing.Point(8, 0);
			this._labelSegmentNumber.Margin = new System.Windows.Forms.Padding(8, 0, 5, 20);
			this._labelSegmentNumber.Name = "_labelSegmentNumber";
			this._labelSegmentNumber.Size = new System.Drawing.Size(226, 13);
			this._labelSegmentNumber.TabIndex = 1;
			this._labelSegmentNumber.Text = "Segment: {0} of {1}";
			// 
			// _buttonEraseAnnotation
			// 
			this._buttonEraseAnnotation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonEraseAnnotation.AutoSize = true;
			this._buttonEraseAnnotation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonEraseAnnotation.BackColor = System.Drawing.Color.Transparent;
			this._buttonEraseAnnotation.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonEraseAnnotation.FlatAppearance.BorderSize = 0;
			this._buttonEraseAnnotation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._buttonEraseAnnotation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._buttonEraseAnnotation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonEraseAnnotation.FocusBackColor = System.Drawing.Color.Empty;
			this._buttonEraseAnnotation.Image = global::SayMore.Properties.Resources.RecordErase;
			this._buttonEraseAnnotation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonEraseAnnotation.Location = new System.Drawing.Point(30, 123);
			this._buttonEraseAnnotation.Margin = new System.Windows.Forms.Padding(30, 0, 8, 0);
			this._buttonEraseAnnotation.Name = "_buttonEraseAnnotation";
			this._buttonEraseAnnotation.ShowFocusRectangle = true;
			this._buttonEraseAnnotation.Size = new System.Drawing.Size(201, 28);
			this._buttonEraseAnnotation.TabIndex = 6;
			this._buttonEraseAnnotation.Text = "  Erase Annotation";
			this._buttonEraseAnnotation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonEraseAnnotation.UseVisualStyleBackColor = true;
			this._buttonEraseAnnotation.Click += new System.EventHandler(this.HandleEraseButtonClick);
			// 
			// _buttonPlayOriginal
			// 
			this._buttonPlayOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonPlayOriginal.AutoSize = true;
			this._buttonPlayOriginal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonPlayOriginal.BackColor = System.Drawing.Color.Transparent;
			this._buttonPlayOriginal.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonPlayOriginal.FlatAppearance.BorderSize = 0;
			this._buttonPlayOriginal.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._buttonPlayOriginal.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._buttonPlayOriginal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonPlayOriginal.FocusBackColor = System.Drawing.Color.Empty;
			this._buttonPlayOriginal.Image = global::SayMore.Properties.Resources.RecordingPlayback;
			this._buttonPlayOriginal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonPlayOriginal.Location = new System.Drawing.Point(0, 33);
			this._buttonPlayOriginal.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
			this._buttonPlayOriginal.Name = "_buttonPlayOriginal";
			this._buttonPlayOriginal.ShowFocusRectangle = true;
			this._buttonPlayOriginal.Size = new System.Drawing.Size(231, 30);
			this._buttonPlayOriginal.TabIndex = 2;
			this._buttonPlayOriginal.Text = "  Listen to Original (press \'O\')";
			this._buttonPlayOriginal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonPlayOriginal.UseVisualStyleBackColor = true;
			// 
			// _buttonPlayAnnotation
			// 
			this._buttonPlayAnnotation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonPlayAnnotation.AutoSize = true;
			this._buttonPlayAnnotation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonPlayAnnotation.BackColor = System.Drawing.Color.Transparent;
			this._buttonPlayAnnotation.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonPlayAnnotation.FlatAppearance.BorderSize = 0;
			this._buttonPlayAnnotation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._buttonPlayAnnotation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._buttonPlayAnnotation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonPlayAnnotation.FocusBackColor = System.Drawing.Color.Empty;
			this._buttonPlayAnnotation.Image = global::SayMore.Properties.Resources.RecordingPlaybackAnnotation;
			this._buttonPlayAnnotation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonPlayAnnotation.Location = new System.Drawing.Point(0, 93);
			this._buttonPlayAnnotation.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
			this._buttonPlayAnnotation.Name = "_buttonPlayAnnotation";
			this._buttonPlayAnnotation.ShowFocusRectangle = true;
			this._buttonPlayAnnotation.Size = new System.Drawing.Size(231, 30);
			this._buttonPlayAnnotation.TabIndex = 5;
			this._buttonPlayAnnotation.Text = "  Check Annotation (press \'A\')";
			this._buttonPlayAnnotation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonPlayAnnotation.UseVisualStyleBackColor = true;
			// 
			// _buttonRecord
			// 
			this._buttonRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonRecord.AutoSize = true;
			this._buttonRecord.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonRecord.BackColor = System.Drawing.Color.Transparent;
			this._buttonRecord.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonRecord.FlatAppearance.BorderSize = 0;
			this._buttonRecord.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._buttonRecord.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this._buttonRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonRecord.FocusBackColor = System.Drawing.Color.Empty;
			this._buttonRecord.Image = global::SayMore.Properties.Resources.RecordStart;
			this._buttonRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonRecord.Location = new System.Drawing.Point(0, 63);
			this._buttonRecord.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
			this._buttonRecord.Name = "_buttonRecord";
			this._buttonRecord.ShowFocusRectangle = true;
			this._buttonRecord.Size = new System.Drawing.Size(231, 30);
			this._buttonRecord.TabIndex = 4;
			this._buttonRecord.Text = "  Record (press SPACE)";
			this._buttonRecord.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonRecord.UseVisualStyleBackColor = true;
			// 
			// _trackBarSegment
			// 
			this._trackBarSegment.Dock = System.Windows.Forms.DockStyle.Top;
			this._trackBarSegment.LargeChange = 1;
			this._trackBarSegment.Location = new System.Drawing.Point(0, 0);
			this._trackBarSegment.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this._trackBarSegment.Maximum = 1000;
			this._trackBarSegment.Minimum = 10;
			this._trackBarSegment.Name = "_trackBarSegment";
			this._trackBarSegment.Size = new System.Drawing.Size(255, 40);
			this._trackBarSegment.TabIndex = 0;
			this._trackBarSegment.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this._trackBarSegment.Value = 10;
			// 
			// _panelMicorphoneLevel
			// 
			this._panelMicorphoneLevel.BackColor = System.Drawing.Color.MediumSeaGreen;
			this._panelMicorphoneLevel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelMicorphoneLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelMicorphoneLevel.ClipTextForChildControls = true;
			this._panelMicorphoneLevel.ControlReceivingFocusOnMnemonic = null;
			this._panelMicorphoneLevel.Dock = System.Windows.Forms.DockStyle.Right;
			this._panelMicorphoneLevel.DoubleBuffered = true;
			this._panelMicorphoneLevel.DrawOnlyBottomBorder = false;
			this._panelMicorphoneLevel.DrawOnlyTopBorder = false;
			this._panelMicorphoneLevel.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(0)));
			this._panelMicorphoneLevel.ForeColor = System.Drawing.SystemColors.ControlText;
			this._panelMicorphoneLevel.Location = new System.Drawing.Point(239, 40);
			this._panelMicorphoneLevel.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this._panelMicorphoneLevel.MnemonicGeneratesClick = false;
			this._panelMicorphoneLevel.Name = "_panelMicorphoneLevel";
			this._panelMicorphoneLevel.PaintExplorerBarBackground = false;
			this._panelMicorphoneLevel.Size = new System.Drawing.Size(16, 167);
			this._panelMicorphoneLevel.TabIndex = 7;
			// 
			// OralAnnotationRecorder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.Controls.Add(this._panelMicorphoneLevel);
			this.Controls.Add(this._trackBarSegment);
			this.Name = "OralAnnotationRecorder";
			this.Size = new System.Drawing.Size(255, 207);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._trackBarSegment)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private StartStopButton _buttonPlayAnnotation;
		private StartStopButton _buttonRecord;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private SilTools.Controls.NicerButton _buttonEraseAnnotation;
		private System.Windows.Forms.TrackBar _trackBarSegment;
		private System.Windows.Forms.Label _labelSegmentNumber;
		private SilTools.Controls.SilPanel _panelMicorphoneLevel;
		private StartStopButton _buttonPlayOriginal;
	}
}
