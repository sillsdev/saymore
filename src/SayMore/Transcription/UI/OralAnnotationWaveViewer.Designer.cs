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
			this._labelTranslation = new System.Windows.Forms.Label();
			this._labelCareful = new System.Windows.Forms.Label();
			this._wavePanelOriginal = new SayMore.AudioUtils.WaveControlBasic();
			this._wavePanelTranslation = new SayMore.AudioUtils.WaveControlBasic();
			this._wavePanelCareful = new SayMore.AudioUtils.WaveControlBasic();
			this._labelOriginal = new System.Windows.Forms.Label();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._labelTranslation, 0, 2);
			this._tableLayout.Controls.Add(this._labelCareful, 0, 1);
			this._tableLayout.Controls.Add(this._wavePanelOriginal, 1, 0);
			this._tableLayout.Controls.Add(this._wavePanelTranslation, 1, 2);
			this._tableLayout.Controls.Add(this._wavePanelCareful, 1, 1);
			this._tableLayout.Controls.Add(this._labelOriginal, 0, 0);
			this._tableLayout.Location = new System.Drawing.Point(0, 0);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 3;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this._tableLayout.Size = new System.Drawing.Size(383, 171);
			this._tableLayout.TabIndex = 5;
			this._tableLayout.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleTableLayoutPaint);
			// 
			// _labelTranslation
			// 
			this._labelTranslation.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelTranslation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelTranslation, null);
			this.locExtender.SetLocalizationComment(this._labelTranslation, null);
			this.locExtender.SetLocalizingId(this._labelTranslation, "EventsView.GeneratedOralAnnotationView.TranslationLabel");
			this._labelTranslation.Location = new System.Drawing.Point(0, 136);
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
			this.locExtender.SetLocalizingId(this._labelCareful, "EventsView.GeneratedOralAnnotationView.CarefulLabel");
			this._labelCareful.Location = new System.Drawing.Point(0, 79);
			this._labelCareful.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._labelCareful.Name = "_labelCareful";
			this._labelCareful.Size = new System.Drawing.Size(40, 13);
			this._labelCareful.TabIndex = 5;
			this._labelCareful.Text = "Careful";
			// 
			// _wavePanelOriginal
			// 
			this._wavePanelOriginal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._wavePanelOriginal.AutoScroll = true;
			this._wavePanelOriginal.AutoScrollMinSize = new System.Drawing.Size(0, 57);
			this._wavePanelOriginal.BackColor = System.Drawing.Color.Snow;
			this._wavePanelOriginal.ForeColor = System.Drawing.SystemColors.WindowText;
			this.locExtender.SetLocalizableToolTip(this._wavePanelOriginal, null);
			this.locExtender.SetLocalizationComment(this._wavePanelOriginal, null);
			this.locExtender.SetLocalizationPriority(this._wavePanelOriginal, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._wavePanelOriginal, "_wavePanelOriginal");
			this._wavePanelOriginal.Location = new System.Drawing.Point(64, 0);
			this._wavePanelOriginal.Margin = new System.Windows.Forms.Padding(0);
			this._wavePanelOriginal.Name = "_wavePanelOriginal";
			this._wavePanelOriginal.Size = new System.Drawing.Size(319, 57);
			this._wavePanelOriginal.TabIndex = 0;
			// 
			// _wavePanelTranslation
			// 
			this._wavePanelTranslation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._wavePanelTranslation.AutoScroll = true;
			this._wavePanelTranslation.AutoScrollMinSize = new System.Drawing.Size(0, 57);
			this._wavePanelTranslation.BackColor = System.Drawing.Color.Snow;
			this._wavePanelTranslation.ForeColor = System.Drawing.SystemColors.WindowText;
			this.locExtender.SetLocalizableToolTip(this._wavePanelTranslation, null);
			this.locExtender.SetLocalizationComment(this._wavePanelTranslation, null);
			this.locExtender.SetLocalizationPriority(this._wavePanelTranslation, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._wavePanelTranslation, "_wavePanelTranslation");
			this._wavePanelTranslation.Location = new System.Drawing.Point(64, 114);
			this._wavePanelTranslation.Margin = new System.Windows.Forms.Padding(0);
			this._wavePanelTranslation.Name = "_wavePanelTranslation";
			this._wavePanelTranslation.Size = new System.Drawing.Size(319, 57);
			this._wavePanelTranslation.TabIndex = 3;
			// 
			// _wavePanelCareful
			// 
			this._wavePanelCareful.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._wavePanelCareful.AutoScroll = true;
			this._wavePanelCareful.AutoScrollMinSize = new System.Drawing.Size(0, 57);
			this._wavePanelCareful.BackColor = System.Drawing.Color.Snow;
			this._wavePanelCareful.ForeColor = System.Drawing.SystemColors.WindowText;
			this.locExtender.SetLocalizableToolTip(this._wavePanelCareful, null);
			this.locExtender.SetLocalizationComment(this._wavePanelCareful, null);
			this.locExtender.SetLocalizationPriority(this._wavePanelCareful, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._wavePanelCareful, "_wavePanelCareful");
			this._wavePanelCareful.Location = new System.Drawing.Point(64, 57);
			this._wavePanelCareful.Margin = new System.Windows.Forms.Padding(0);
			this._wavePanelCareful.Name = "_wavePanelCareful";
			this._wavePanelCareful.Size = new System.Drawing.Size(319, 57);
			this._wavePanelCareful.TabIndex = 2;
			// 
			// _labelOriginal
			// 
			this._labelOriginal.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelOriginal.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelOriginal, null);
			this.locExtender.SetLocalizationComment(this._labelOriginal, null);
			this.locExtender.SetLocalizingId(this._labelOriginal, "EventsView.GeneratedOralAnnotationView.OriginalLabel");
			this._labelOriginal.Location = new System.Drawing.Point(0, 22);
			this._labelOriginal.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._labelOriginal.Name = "_labelOriginal";
			this._labelOriginal.Size = new System.Drawing.Size(42, 13);
			this._labelOriginal.TabIndex = 4;
			this._labelOriginal.Text = "Original";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// OralAnnotationWaveViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "Transcription.UI.OralAnnotationWaveViewer.OralAnnotationWaveViewer");
			this.Name = "OralAnnotationWaveViewer";
			this.Size = new System.Drawing.Size(383, 171);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private SayMore.AudioUtils.WaveControlBasic _wavePanelOriginal;
		private SayMore.AudioUtils.WaveControlBasic _wavePanelTranslation;
		private SayMore.AudioUtils.WaveControlBasic _wavePanelCareful;
		private System.Windows.Forms.Label _labelOriginal;
		private System.Windows.Forms.Label _labelTranslation;
		private System.Windows.Forms.Label _labelCareful;
		private Localization.UI.LocalizationExtender locExtender;
	}
}
