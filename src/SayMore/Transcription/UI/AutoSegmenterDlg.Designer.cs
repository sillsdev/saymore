namespace SayMore.Transcription.UI
{
	partial class AutoSegmenterDlg
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoSegmenterDlg));
			this._upDownClusterDuration = new System.Windows.Forms.NumericUpDown();
			this._labelAudioFileNameLabel = new System.Windows.Forms.Label();
			this._labelAudioFileName = new System.Windows.Forms.Label();
			this._upDnSilenceThreshold = new System.Windows.Forms.NumericUpDown();
			this._labelClusterDurationHint = new System.Windows.Forms.Label();
			this._groupBoxSilenceThreshold = new System.Windows.Forms.GroupBox();
			this._grpBoxClusterDuration = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this._grpBoxOnsetAlgorithmThreshold = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this._upDnOnsetAlgorithmThreshold = new System.Windows.Forms.NumericUpDown();
			this._buttonSegment = new System.Windows.Forms.Button();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._pictureBoxBusyWheel = new System.Windows.Forms.PictureBox();
			this._labelGeneratingSegmentsMsg = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this._upDownClusterDuration)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._upDnSilenceThreshold)).BeginInit();
			this._groupBoxSilenceThreshold.SuspendLayout();
			this._grpBoxClusterDuration.SuspendLayout();
			this._grpBoxOnsetAlgorithmThreshold.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._upDnOnsetAlgorithmThreshold)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureBoxBusyWheel)).BeginInit();
			this.SuspendLayout();
			// 
			// _upDownClusterDuration
			// 
			this._upDownClusterDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._upDownClusterDuration.DecimalPlaces = 2;
			this._upDownClusterDuration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._upDownClusterDuration.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this._upDownClusterDuration.Location = new System.Drawing.Point(370, 21);
			this._upDownClusterDuration.Maximum = new decimal(new int[] {
            22,
            0,
            0,
            65536});
			this._upDownClusterDuration.Name = "_upDownClusterDuration";
			this._upDownClusterDuration.Size = new System.Drawing.Size(63, 23);
			this._upDownClusterDuration.TabIndex = 1;
			this._upDownClusterDuration.Value = new decimal(new int[] {
            3,
            0,
            0,
            65536});
			// 
			// _labelAudioFileNameLabel
			// 
			this._labelAudioFileNameLabel.AutoSize = true;
			this._labelAudioFileNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelAudioFileNameLabel.Location = new System.Drawing.Point(18, 15);
			this._labelAudioFileNameLabel.Name = "_labelAudioFileNameLabel";
			this._labelAudioFileNameLabel.Size = new System.Drawing.Size(63, 15);
			this._labelAudioFileNameLabel.TabIndex = 0;
			this._labelAudioFileNameLabel.Text = "Audio File:";
			// 
			// _labelAudioFileName
			// 
			this._labelAudioFileName.AutoSize = true;
			this._labelAudioFileName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelAudioFileName.Location = new System.Drawing.Point(87, 15);
			this._labelAudioFileName.Name = "_labelAudioFileName";
			this._labelAudioFileName.Size = new System.Drawing.Size(14, 15);
			this._labelAudioFileName.TabIndex = 1;
			this._labelAudioFileName.Text = "#";
			// 
			// _upDnSilenceThreshold
			// 
			this._upDnSilenceThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._upDnSilenceThreshold.Location = new System.Drawing.Point(370, 21);
			this._upDnSilenceThreshold.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this._upDnSilenceThreshold.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
			this._upDnSilenceThreshold.Name = "_upDnSilenceThreshold";
			this._upDnSilenceThreshold.Size = new System.Drawing.Size(63, 23);
			this._upDnSilenceThreshold.TabIndex = 1;
			this._upDnSilenceThreshold.Value = new decimal(new int[] {
            37,
            0,
            0,
            -2147483648});
			// 
			// _labelClusterDurationHint
			// 
			this._labelClusterDurationHint.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._labelClusterDurationHint.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelClusterDurationHint.Location = new System.Drawing.Point(3, 23);
			this._labelClusterDurationHint.Name = "_labelClusterDurationHint";
			this._labelClusterDurationHint.Size = new System.Drawing.Size(361, 67);
			this._labelClusterDurationHint.TabIndex = 0;
			this._labelClusterDurationHint.Text = resources.GetString("_labelClusterDurationHint.Text");
			// 
			// _groupBoxSilenceThreshold
			// 
			this._groupBoxSilenceThreshold.AutoSize = true;
			this._groupBoxSilenceThreshold.Controls.Add(this._labelClusterDurationHint);
			this._groupBoxSilenceThreshold.Controls.Add(this._upDnSilenceThreshold);
			this._groupBoxSilenceThreshold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._groupBoxSilenceThreshold.Location = new System.Drawing.Point(15, 40);
			this._groupBoxSilenceThreshold.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._groupBoxSilenceThreshold.Name = "_groupBoxSilenceThreshold";
			this._groupBoxSilenceThreshold.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this._groupBoxSilenceThreshold.Size = new System.Drawing.Size(442, 94);
			this._groupBoxSilenceThreshold.TabIndex = 2;
			this._groupBoxSilenceThreshold.TabStop = false;
			this._groupBoxSilenceThreshold.Text = "&Silence Threshold";
			// 
			// _grpBoxClusterDuration
			// 
			this._grpBoxClusterDuration.AutoSize = true;
			this._grpBoxClusterDuration.Controls.Add(this.label1);
			this._grpBoxClusterDuration.Controls.Add(this._upDownClusterDuration);
			this._grpBoxClusterDuration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._grpBoxClusterDuration.Location = new System.Drawing.Point(15, 144);
			this._grpBoxClusterDuration.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._grpBoxClusterDuration.Name = "_grpBoxClusterDuration";
			this._grpBoxClusterDuration.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this._grpBoxClusterDuration.Size = new System.Drawing.Size(442, 93);
			this._grpBoxClusterDuration.TabIndex = 3;
			this._grpBoxClusterDuration.TabStop = false;
			this._grpBoxClusterDuration.Text = "&Cluster Duration (in seconds)";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(361, 66);
			this.label1.TabIndex = 0;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// _grpBoxOnsetAlgorithmThreshold
			// 
			this._grpBoxOnsetAlgorithmThreshold.AutoSize = true;
			this._grpBoxOnsetAlgorithmThreshold.Controls.Add(this.label2);
			this._grpBoxOnsetAlgorithmThreshold.Controls.Add(this._upDnOnsetAlgorithmThreshold);
			this._grpBoxOnsetAlgorithmThreshold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._grpBoxOnsetAlgorithmThreshold.Location = new System.Drawing.Point(15, 247);
			this._grpBoxOnsetAlgorithmThreshold.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this._grpBoxOnsetAlgorithmThreshold.Name = "_grpBoxOnsetAlgorithmThreshold";
			this._grpBoxOnsetAlgorithmThreshold.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this._grpBoxOnsetAlgorithmThreshold.Size = new System.Drawing.Size(442, 80);
			this._grpBoxOnsetAlgorithmThreshold.TabIndex = 4;
			this._grpBoxOnsetAlgorithmThreshold.TabStop = false;
			this._grpBoxOnsetAlgorithmThreshold.Text = "&Onset Algorithm Threshold";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(3, 23);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(361, 53);
			this.label2.TabIndex = 0;
			this.label2.Text = "The onset algorithm threshold defines how sensitive is the detection of the onset" +
    " of, for example speech. A low value (0.001) means very sensitive, while a high " +
    "value (2) is not.";
			// 
			// _upDnOnsetAlgorithmThreshold
			// 
			this._upDnOnsetAlgorithmThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._upDnOnsetAlgorithmThreshold.DecimalPlaces = 3;
			this._upDnOnsetAlgorithmThreshold.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._upDnOnsetAlgorithmThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
			this._upDnOnsetAlgorithmThreshold.Location = new System.Drawing.Point(370, 22);
			this._upDnOnsetAlgorithmThreshold.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this._upDnOnsetAlgorithmThreshold.Name = "_upDnOnsetAlgorithmThreshold";
			this._upDnOnsetAlgorithmThreshold.Size = new System.Drawing.Size(63, 23);
			this._upDnOnsetAlgorithmThreshold.TabIndex = 1;
			this._upDnOnsetAlgorithmThreshold.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
			// 
			// _buttonSegment
			// 
			this._buttonSegment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonSegment.Location = new System.Drawing.Point(301, 337);
			this._buttonSegment.Name = "_buttonSegment";
			this._buttonSegment.Size = new System.Drawing.Size(75, 26);
			this._buttonSegment.TabIndex = 6;
			this._buttonSegment.Text = "Segment";
			this._buttonSegment.UseVisualStyleBackColor = true;
			this._buttonSegment.Click += new System.EventHandler(this.HandleSegmentButtonClick);
			// 
			// _buttonCancel
			// 
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._buttonCancel.Location = new System.Drawing.Point(382, 337);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 7;
			this._buttonCancel.Text = "Close";
			this._buttonCancel.UseVisualStyleBackColor = true;
			// 
			// _pictureBoxBusyWheel
			// 
			this._pictureBoxBusyWheel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._pictureBoxBusyWheel.Image = global::SayMore.Properties.Resources.BusyWheelSmall;
			this._pictureBoxBusyWheel.Location = new System.Drawing.Point(21, 340);
			this._pictureBoxBusyWheel.Name = "_pictureBoxBusyWheel";
			this._pictureBoxBusyWheel.Size = new System.Drawing.Size(16, 16);
			this._pictureBoxBusyWheel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureBoxBusyWheel.TabIndex = 8;
			this._pictureBoxBusyWheel.TabStop = false;
			this._pictureBoxBusyWheel.Visible = false;
			// 
			// _labelGeneratingSegmentsMsg
			// 
			this._labelGeneratingSegmentsMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelGeneratingSegmentsMsg.AutoSize = true;
			this._labelGeneratingSegmentsMsg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelGeneratingSegmentsMsg.Location = new System.Drawing.Point(44, 341);
			this._labelGeneratingSegmentsMsg.Name = "_labelGeneratingSegmentsMsg";
			this._labelGeneratingSegmentsMsg.Size = new System.Drawing.Size(138, 15);
			this._labelGeneratingSegmentsMsg.TabIndex = 9;
			this._labelGeneratingSegmentsMsg.Text = "Generating Segments...";
			this._labelGeneratingSegmentsMsg.Visible = false;
			// 
			// AutoSegmenterDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(472, 373);
			this.Controls.Add(this._labelGeneratingSegmentsMsg);
			this.Controls.Add(this._pictureBoxBusyWheel);
			this.Controls.Add(this._buttonCancel);
			this.Controls.Add(this._buttonSegment);
			this.Controls.Add(this._grpBoxOnsetAlgorithmThreshold);
			this.Controls.Add(this._grpBoxClusterDuration);
			this.Controls.Add(this._labelAudioFileName);
			this.Controls.Add(this._groupBoxSilenceThreshold);
			this.Controls.Add(this._labelAudioFileNameLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AutoSegmenterDlg";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Automatic Segment";
			((System.ComponentModel.ISupportInitialize)(this._upDownClusterDuration)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._upDnSilenceThreshold)).EndInit();
			this._groupBoxSilenceThreshold.ResumeLayout(false);
			this._grpBoxClusterDuration.ResumeLayout(false);
			this._grpBoxOnsetAlgorithmThreshold.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._upDnOnsetAlgorithmThreshold)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._pictureBoxBusyWheel)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _labelAudioFileName;
		private System.Windows.Forms.Label _labelAudioFileNameLabel;
		private System.Windows.Forms.NumericUpDown _upDownClusterDuration;
		private System.Windows.Forms.NumericUpDown _upDnSilenceThreshold;
		private System.Windows.Forms.Label _labelClusterDurationHint;
		private System.Windows.Forms.GroupBox _groupBoxSilenceThreshold;
		private System.Windows.Forms.GroupBox _grpBoxClusterDuration;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox _grpBoxOnsetAlgorithmThreshold;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown _upDnOnsetAlgorithmThreshold;
		private System.Windows.Forms.Button _buttonSegment;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.PictureBox _pictureBoxBusyWheel;
		private System.Windows.Forms.Label _labelGeneratingSegmentsMsg;
	}
}