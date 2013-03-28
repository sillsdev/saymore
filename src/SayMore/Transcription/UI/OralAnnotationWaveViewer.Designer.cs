namespace SayMore.Transcription.UI
{
	partial class OralAnnotationWaveViewer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._panelLabels = new System.Windows.Forms.Panel();
			this._labelTranslation = new System.Windows.Forms.Label();
			this._labelCareful = new System.Windows.Forms.Label();
			this._labelSource = new System.Windows.Forms.Label();
			this._waveControl = new SayMore.Media.Audio.WaveControlBasic();
			this.locExtender = new L10NSharp.UI.LocalizationExtender(this.components);
			this._tableLayout.SuspendLayout();
			this._panelLabels.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.BackColor = System.Drawing.SystemColors.Window;
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._panelLabels, 0, 0);
			this._tableLayout.Controls.Add(this._waveControl, 1, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(0, 0);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 1;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Size = new System.Drawing.Size(383, 171);
			this._tableLayout.TabIndex = 5;
			// 
			// _panelLabels
			// 
			this._panelLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this._panelLabels.AutoSize = true;
			this._panelLabels.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._panelLabels.Controls.Add(this._labelTranslation);
			this._panelLabels.Controls.Add(this._labelCareful);
			this._panelLabels.Controls.Add(this._labelSource);
			this._panelLabels.Location = new System.Drawing.Point(0, 0);
			this._panelLabels.Margin = new System.Windows.Forms.Padding(0);
			this._panelLabels.Name = "_panelLabels";
			this._panelLabels.Size = new System.Drawing.Size(64, 171);
			this._panelLabels.TabIndex = 7;
			this._panelLabels.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleLabelPanelPaint);
			// 
			// _labelTranslation
			// 
			this._labelTranslation.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelTranslation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelTranslation, null);
			this.locExtender.SetLocalizationComment(this._labelTranslation, null);
			this.locExtender.SetLocalizingId(this._labelTranslation, "SessionsView.GeneratedOralAnnotationView.TranslationLabel");
			this._labelTranslation.Location = new System.Drawing.Point(0, 98);
			this._labelTranslation.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._labelTranslation.Name = "_labelTranslation";
			this._labelTranslation.Size = new System.Drawing.Size(59, 13);
			this._labelTranslation.TabIndex = 6;
			this._labelTranslation.Text = "Translation";
			// 
			// _labelCareful
			// 
			this._labelCareful.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelCareful.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelCareful, null);
			this.locExtender.SetLocalizationComment(this._labelCareful, null);
			this.locExtender.SetLocalizingId(this._labelCareful, "SessionsView.GeneratedOralAnnotationView.CarefulLabel");
			this._labelCareful.Location = new System.Drawing.Point(0, 61);
			this._labelCareful.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._labelCareful.Name = "_labelCareful";
			this._labelCareful.Size = new System.Drawing.Size(40, 13);
			this._labelCareful.TabIndex = 5;
			this._labelCareful.Text = "Careful";
			// 
			// _labelSource
			// 
			this._labelSource.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelSource.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelSource, null);
			this.locExtender.SetLocalizationComment(this._labelSource, null);
			this.locExtender.SetLocalizingId(this._labelSource, "SessionsView.GeneratedOralAnnotationView.SourceLabel");
			this._labelSource.Location = new System.Drawing.Point(0, 31);
			this._labelSource.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._labelSource.Name = "_labelOriginal";
			this._labelSource.Size = new System.Drawing.Size(42, 13);
			this._labelSource.TabIndex = 4;
			this._labelSource.Text = "Source";
			// 
			// _waveControl
			// 
			this._waveControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._waveControl.AutoScroll = true;
			this._waveControl.AutoScrollMinSize = new System.Drawing.Size(0, 171);
			this._waveControl.BackColor = System.Drawing.SystemColors.Window;
			this._waveControl.ForeColor = System.Drawing.SystemColors.WindowText;
			this.locExtender.SetLocalizableToolTip(this._waveControl, null);
			this.locExtender.SetLocalizationComment(this._waveControl, null);
			this.locExtender.SetLocalizationPriority(this._waveControl, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._waveControl, "_wavePanelOriginal");
			this._waveControl.Location = new System.Drawing.Point(64, 0);
			this._waveControl.Margin = new System.Windows.Forms.Padding(0);
			this._waveControl.Name = "_waveControl";
			this._waveControl.Size = new System.Drawing.Size(319, 171);
			this._waveControl.TabIndex = 0;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// OralAnnotationWaveViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "Transcription.UI.OralAnnotationWaveViewer.OralAnnotationWaveViewer");
			this.Name = "OralAnnotationWaveViewer";
			this.Size = new System.Drawing.Size(383, 171);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this._panelLabels.ResumeLayout(false);
			this._panelLabels.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private SayMore.Media.Audio.WaveControlBasic _waveControl;
		private System.Windows.Forms.Label _labelSource;
		private System.Windows.Forms.Label _labelTranslation;
		private System.Windows.Forms.Label _labelCareful;
		private L10NSharp.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Panel _panelLabels;
	}
}
