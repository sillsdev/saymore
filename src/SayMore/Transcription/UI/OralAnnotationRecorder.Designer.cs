using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

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
			this._trackBarSegment = new System.Windows.Forms.TrackBar();
			this._labelSegmentNumber = new System.Windows.Forms.Label();
			this._trackBarMicLevel = new System.Windows.Forms.TrackBar();
			this._panelMicorphoneLevel = new SilTools.Controls.SilPanel();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._buttonEraseAnnotation = new SilTools.Controls.NicerButton();
			this._buttonPlayAnnotation = new SayMore.Transcription.UI.StartStopButton();
			this._buttonPlayOriginal = new SayMore.Transcription.UI.StartStopButton();
			this._buttonRecord = new SayMore.Transcription.UI.StartStopButton();
			((System.ComponentModel.ISupportInitialize)(this._trackBarSegment)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._trackBarMicLevel)).BeginInit();
			this._tableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// _trackBarSegment
			// 
			this._trackBarSegment.AutoSize = false;
			this._trackBarSegment.Dock = System.Windows.Forms.DockStyle.Top;
			this._trackBarSegment.LargeChange = 1;
			this._trackBarSegment.Location = new System.Drawing.Point(0, 0);
			this._trackBarSegment.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._trackBarSegment.Maximum = 1000;
			this._trackBarSegment.Minimum = 10;
			this._trackBarSegment.Name = "_trackBarSegment";
			this._trackBarSegment.Size = new System.Drawing.Size(249, 28);
			this._trackBarSegment.TabIndex = 0;
			this._trackBarSegment.TabStop = false;
			this._trackBarSegment.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this._trackBarSegment.Value = 10;
			// 
			// _labelSegmentNumber
			// 
			this._labelSegmentNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._labelSegmentNumber.AutoEllipsis = true;
			this._labelSegmentNumber.AutoSize = true;
			this._labelSegmentNumber.BackColor = System.Drawing.Color.Transparent;
			this._labelSegmentNumber.Location = new System.Drawing.Point(8, 0);
			this._labelSegmentNumber.Margin = new System.Windows.Forms.Padding(8, 0, 5, 15);
			this._labelSegmentNumber.Name = "_labelSegmentNumber";
			this._labelSegmentNumber.Size = new System.Drawing.Size(188, 13);
			this._labelSegmentNumber.TabIndex = 1;
			this._labelSegmentNumber.Text = "{0}/{1}";
			// 
			// _trackBarMicLevel
			// 
			this._trackBarMicLevel.AutoSize = false;
			this._trackBarMicLevel.Dock = System.Windows.Forms.DockStyle.Right;
			this._trackBarMicLevel.LargeChange = 20;
			this._trackBarMicLevel.Location = new System.Drawing.Point(201, 28);
			this._trackBarMicLevel.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this._trackBarMicLevel.Maximum = 100;
			this._trackBarMicLevel.Name = "_trackBarMicLevel";
			this._trackBarMicLevel.Orientation = System.Windows.Forms.Orientation.Vertical;
			this._trackBarMicLevel.Size = new System.Drawing.Size(33, 162);
			this._trackBarMicLevel.SmallChange = 5;
			this._trackBarMicLevel.TabIndex = 8;
			this._trackBarMicLevel.TabStop = false;
			this._trackBarMicLevel.TickFrequency = 10;
			this._trackBarMicLevel.Value = 10;
			this._trackBarMicLevel.Visible = false;
			// 
			// _panelMicorphoneLevel
			// 
			this._panelMicorphoneLevel.BackColor = System.Drawing.Color.White;
			this._panelMicorphoneLevel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelMicorphoneLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelMicorphoneLevel.ClipTextForChildControls = true;
			this._panelMicorphoneLevel.ControlReceivingFocusOnMnemonic = null;
			this._panelMicorphoneLevel.Dock = System.Windows.Forms.DockStyle.Right;
			this._panelMicorphoneLevel.DoubleBuffered = true;
			this._panelMicorphoneLevel.DrawOnlyBottomBorder = false;
			this._panelMicorphoneLevel.DrawOnlyTopBorder = false;
			this._panelMicorphoneLevel.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World, ((byte)(0)));
			this._panelMicorphoneLevel.ForeColor = System.Drawing.Color.LightGreen;
			this._panelMicorphoneLevel.Location = new System.Drawing.Point(234, 28);
			this._panelMicorphoneLevel.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
			this._panelMicorphoneLevel.MnemonicGeneratesClick = false;
			this._panelMicorphoneLevel.Name = "_panelMicorphoneLevel";
			this._panelMicorphoneLevel.PaintExplorerBarBackground = false;
			this._panelMicorphoneLevel.Size = new System.Drawing.Size(15, 162);
			this._panelMicorphoneLevel.TabIndex = 7;
			// 
			// _tableLayout
			// 
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.Controls.Add(this._buttonEraseAnnotation, 0, 4);
			this._tableLayout.Controls.Add(this._buttonPlayAnnotation, 0, 3);
			this._tableLayout.Controls.Add(this._buttonPlayOriginal, 0, 1);
			this._tableLayout.Controls.Add(this._labelSegmentNumber, 0, 0);
			this._tableLayout.Controls.Add(this._buttonRecord, 0, 2);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(0, 28);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 5;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayout.Size = new System.Drawing.Size(201, 162);
			this._tableLayout.TabIndex = 9;
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
			this._buttonEraseAnnotation.Location = new System.Drawing.Point(30, 106);
			this._buttonEraseAnnotation.Margin = new System.Windows.Forms.Padding(30, 0, 10, 0);
			this._buttonEraseAnnotation.Name = "_buttonEraseAnnotation";
			this._buttonEraseAnnotation.ShowFocusRectangle = true;
			this._buttonEraseAnnotation.Size = new System.Drawing.Size(161, 28);
			this._buttonEraseAnnotation.TabIndex = 6;
			this._buttonEraseAnnotation.Text = " Erase Annotation";
			this._buttonEraseAnnotation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonEraseAnnotation.UseVisualStyleBackColor = true;
			this._buttonEraseAnnotation.Click += new System.EventHandler(this.HandleEraseButtonClick);
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
			this._buttonPlayAnnotation.Location = new System.Drawing.Point(0, 80);
			this._buttonPlayAnnotation.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
			this._buttonPlayAnnotation.Name = "_buttonPlayAnnotation";
			this._buttonPlayAnnotation.ShowFocusRectangle = true;
			this._buttonPlayAnnotation.Size = new System.Drawing.Size(191, 26);
			this._buttonPlayAnnotation.TabIndex = 5;
			this._buttonPlayAnnotation.Text = " Check Annotation (press \'A\' to start)";
			this._buttonPlayAnnotation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonPlayAnnotation.UseVisualStyleBackColor = true;
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
			this._buttonPlayOriginal.Location = new System.Drawing.Point(0, 28);
			this._buttonPlayOriginal.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
			this._buttonPlayOriginal.Name = "_buttonPlayOriginal";
			this._buttonPlayOriginal.ShowFocusRectangle = true;
			this._buttonPlayOriginal.Size = new System.Drawing.Size(191, 26);
			this._buttonPlayOriginal.TabIndex = 2;
			this._buttonPlayOriginal.Text = " Listen to Original (press \'O\' to start)";
			this._buttonPlayOriginal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonPlayOriginal.UseVisualStyleBackColor = true;
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
			this._buttonRecord.Location = new System.Drawing.Point(0, 54);
			this._buttonRecord.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
			this._buttonRecord.Name = "_buttonRecord";
			this._buttonRecord.ShowFocusRectangle = true;
			this._buttonRecord.Size = new System.Drawing.Size(191, 26);
			this._buttonRecord.TabIndex = 4;
			this._buttonRecord.Text = " Record (press SPACE to start)";
			this._buttonRecord.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this._buttonRecord.UseVisualStyleBackColor = true;
			// 
			// OralAnnotationRecorder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this._tableLayout);
			this.Controls.Add(this._trackBarMicLevel);
			this.Controls.Add(this._panelMicorphoneLevel);
			this.Controls.Add(this._trackBarSegment);
			this.Name = "OralAnnotationRecorder";
			this.Size = new System.Drawing.Size(249, 190);
			((System.ComponentModel.ISupportInitialize)(this._trackBarSegment)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._trackBarMicLevel)).EndInit();
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private StartStopButton _buttonPlayAnnotation;
		private StartStopButton _buttonRecord;
		private System.Windows.Forms.TrackBar _trackBarSegment;
		private System.Windows.Forms.Label _labelSegmentNumber;
		private SilTools.Controls.SilPanel _panelMicorphoneLevel;
		private StartStopButton _buttonPlayOriginal;
		private TrackBar _trackBarMicLevel;
		private TableLayoutPanel _tableLayout;
		private SilTools.Controls.NicerButton _buttonEraseAnnotation;
	}
}
