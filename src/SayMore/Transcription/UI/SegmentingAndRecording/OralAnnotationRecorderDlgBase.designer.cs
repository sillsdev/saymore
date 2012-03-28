using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class OralAnnotationRecorderBaseDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
				components.Dispose();

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
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._labelListenButton = new System.Windows.Forms.Label();
			this._labelRecordButton = new System.Windows.Forms.Label();
			this._tableLayoutMediaButtons = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutRecordAnnotations = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this._tableLayoutMediaButtons.SuspendLayout();
			this._tableLayoutRecordAnnotations.SuspendLayout();
			this.SuspendLayout();
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// _labelListenButton
			//
			this._labelListenButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._labelListenButton.Image = global::SayMore.Properties.Resources.ListenToOriginalRecording;
			this.locExtender.SetLocalizableToolTip(this._labelListenButton, null);
			this.locExtender.SetLocalizationComment(this._labelListenButton, null);
			this.locExtender.SetLocalizingId(this._labelListenButton, "label1.label1");
			this._labelListenButton.Location = new System.Drawing.Point(32, 30);
			this._labelListenButton.Margin = new System.Windows.Forms.Padding(0, 10, 1, 0);
			this._labelListenButton.Name = "_labelListenButton";
			this._labelListenButton.Size = new System.Drawing.Size(50, 50);
			this._labelListenButton.TabIndex = 0;
			this._labelListenButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleListenToOriginalMouseDown);
			this._labelListenButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandleListenToOriginalMouseDown);
			//
			// _labelRecordButton
			//
			this._labelRecordButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._labelRecordButton.Image = global::SayMore.Properties.Resources.RecordOralAnnotation;
			this.locExtender.SetLocalizableToolTip(this._labelRecordButton, null);
			this.locExtender.SetLocalizationComment(this._labelRecordButton, null);
			this.locExtender.SetLocalizingId(this._labelRecordButton, "label1.label1");
			this._labelRecordButton.Location = new System.Drawing.Point(32, 30);
			this._labelRecordButton.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
			this._labelRecordButton.Name = "_labelRecordButton";
			this._labelRecordButton.Size = new System.Drawing.Size(50, 50);
			this._labelRecordButton.TabIndex = 0;
			//
			// _tableLayoutMediaButtons
			//
			this._tableLayoutMediaButtons.ColumnCount = 1;
			this._tableLayoutMediaButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMediaButtons.Controls.Add(this._labelListenButton, 0, 1);
			this._tableLayoutMediaButtons.Controls.Add(this._tableLayoutRecordAnnotations, 0, 2);
			this._tableLayoutMediaButtons.Location = new System.Drawing.Point(220, 70);
			this._tableLayoutMediaButtons.Name = "_tableLayoutMediaButtons";
			this._tableLayoutMediaButtons.RowCount = 3;
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutMediaButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this._tableLayoutMediaButtons.Size = new System.Drawing.Size(116, 250);
			this._tableLayoutMediaButtons.TabIndex = 8;
			this._tableLayoutMediaButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleMediaButtonTableLayoutPaint);
			//
			// _tableLayoutRecordAnnotations
			//
			this._tableLayoutRecordAnnotations.ColumnCount = 1;
			this._tableLayoutRecordAnnotations.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutRecordAnnotations.Controls.Add(this._labelRecordButton, 0, 1);
			this._tableLayoutRecordAnnotations.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutRecordAnnotations.Location = new System.Drawing.Point(0, 150);
			this._tableLayoutRecordAnnotations.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this._tableLayoutRecordAnnotations.Name = "_tableLayoutRecordAnnotations";
			this._tableLayoutRecordAnnotations.RowCount = 2;
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutRecordAnnotations.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutRecordAnnotations.Size = new System.Drawing.Size(115, 100);
			this._tableLayoutRecordAnnotations.TabIndex = 1;
			//
			// OralAnnotationRecorderBaseDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 338);
			this.Controls.Add(this._tableLayoutMediaButtons);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, "Localized in subclass");
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.WindowTitle");
			this.Name = "OralAnnotationRecorderBaseDlg";
			this.Opacity = 1D;
			this.Text = "Change my text";
			this.Controls.SetChildIndex(this._tableLayoutMediaButtons, 0);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this._tableLayoutMediaButtons.ResumeLayout(false);
			this._tableLayoutRecordAnnotations.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.Label _labelListenButton;
		private System.Windows.Forms.Label _labelRecordButton;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutMediaButtons;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutRecordAnnotations;
	}
}
