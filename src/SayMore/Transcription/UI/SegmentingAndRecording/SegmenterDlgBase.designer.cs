using System.Windows.Forms;
using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	partial class SegmenterDlgBase
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
			this._panelWaveControl = new System.Windows.Forms.Panel();
			this._tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutTop = new System.Windows.Forms.TableLayoutPanel();
			this._labelOriginalRecording = new System.Windows.Forms.Label();
			this._labelZoom = new System.Windows.Forms.Label();
			this._comboBoxZoom = new System.Windows.Forms.ComboBox();
			this._toolStripStatus = new System.Windows.Forms.ToolStrip();
			this._labelTimeDisplay = new System.Windows.Forms.ToolStripLabel();
			this._labelSegmentXofY = new System.Windows.Forms.ToolStripLabel();
			this._labelSegmentNumber = new System.Windows.Forms.ToolStripLabel();
			this._buttonOK = new System.Windows.Forms.Button();
			this._buttonCancel = new System.Windows.Forms.Button();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutOuter.SuspendLayout();
			this._tableLayoutTop.SuspendLayout();
			this._toolStripStatus.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this._tableLayoutButtons.SuspendLayout();
			this.SuspendLayout();
			//
			// _panelWaveControl
			//
			this._panelWaveControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutOuter.SetColumnSpan(this._panelWaveControl, 2);
			this._panelWaveControl.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
			this._panelWaveControl.ForeColor = System.Drawing.SystemColors.ControlText;
			this._panelWaveControl.Location = new System.Drawing.Point(0, 29);
			this._panelWaveControl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
			this._panelWaveControl.Name = "_panelWaveControl";
			this._panelWaveControl.Padding = new System.Windows.Forms.Padding(1);
			this._panelWaveControl.Size = new System.Drawing.Size(703, 108);
			this._panelWaveControl.TabIndex = 1;
			//
			// _tableLayoutOuter
			//
			this._tableLayoutOuter.ColumnCount = 2;
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.Controls.Add(this._panelWaveControl, 0, 1);
			this._tableLayoutOuter.Controls.Add(this._tableLayoutTop, 0, 0);
			this._tableLayoutOuter.Controls.Add(this._toolStripStatus, 1, 2);
			this._tableLayoutOuter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayoutOuter.Location = new System.Drawing.Point(0, 12);
			this._tableLayoutOuter.Name = "_tableLayoutOuter";
			this._tableLayoutOuter.RowCount = 3;
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this._tableLayoutOuter.Size = new System.Drawing.Size(703, 290);
			this._tableLayoutOuter.TabIndex = 6;
			//
			// _tableLayoutTop
			//
			this._tableLayoutTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutTop.ColumnCount = 3;
			this._tableLayoutOuter.SetColumnSpan(this._tableLayoutTop, 2);
			this._tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutTop.Controls.Add(this._labelOriginalRecording, 0, 0);
			this._tableLayoutTop.Controls.Add(this._labelZoom, 1, 0);
			this._tableLayoutTop.Controls.Add(this._comboBoxZoom, 2, 0);
			this._tableLayoutTop.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutTop.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._tableLayoutTop.Name = "_tableLayoutTop";
			this._tableLayoutTop.RowCount = 1;
			this._tableLayoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutTop.Size = new System.Drawing.Size(703, 24);
			this._tableLayoutTop.TabIndex = 9;
			//
			// _labelOriginalRecording
			//
			this._labelOriginalRecording.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelOriginalRecording.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelOriginalRecording, null);
			this.locExtender.SetLocalizationComment(this._labelOriginalRecording, null);
			this.locExtender.SetLocalizingId(this._labelOriginalRecording, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlgDlg._labelOriginalRecording" +
					"");
			this._labelOriginalRecording.Location = new System.Drawing.Point(0, 5);
			this._labelOriginalRecording.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelOriginalRecording.Name = "_labelOriginalRecording";
			this._labelOriginalRecording.Size = new System.Drawing.Size(94, 13);
			this._labelOriginalRecording.TabIndex = 4;
			this._labelOriginalRecording.Text = "Original Recording";
			//
			// _labelZoom
			//
			this._labelZoom.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._labelZoom.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelZoom, null);
			this.locExtender.SetLocalizationComment(this._labelZoom, null);
			this.locExtender.SetLocalizingId(this._labelZoom, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._labelZoom");
			this._labelZoom.Location = new System.Drawing.Point(564, 5);
			this._labelZoom.Name = "_labelZoom";
			this._labelZoom.Size = new System.Drawing.Size(37, 13);
			this._labelZoom.TabIndex = 6;
			this._labelZoom.Text = "&Zoom:";
			//
			// _comboBoxZoom
			//
			this._comboBoxZoom.BackColor = System.Drawing.Color.White;
			this._comboBoxZoom.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._comboBoxZoom, null);
			this.locExtender.SetLocalizationComment(this._comboBoxZoom, null);
			this.locExtender.SetLocalizationPriority(this._comboBoxZoom, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._comboBoxZoom, "ManualSegmenterDlg._comboBoxZoom");
			this._comboBoxZoom.Location = new System.Drawing.Point(607, 0);
			this._comboBoxZoom.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._comboBoxZoom.Name = "_comboBoxZoom";
			this._comboBoxZoom.Size = new System.Drawing.Size(96, 21);
			this._comboBoxZoom.TabIndex = 5;
			this._comboBoxZoom.SelectedIndexChanged += new System.EventHandler(this.HandleZoomSelectedIndexChanged);
			this._comboBoxZoom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleZoomKeyDown);
			this._comboBoxZoom.Validating += new System.ComponentModel.CancelEventHandler(this.HandleZoomComboValidating);
			//
			// _toolStripStatus
			//
			this._toolStripStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this._toolStripStatus.BackColor = System.Drawing.SystemColors.Control;
			this._toolStripStatus.CanOverflow = false;
			this._toolStripStatus.Dock = System.Windows.Forms.DockStyle.None;
			this._toolStripStatus.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._labelTimeDisplay,
			this._labelSegmentXofY,
			this._labelSegmentNumber});
			this._toolStripStatus.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.locExtender.SetLocalizableToolTip(this._toolStripStatus, null);
			this.locExtender.SetLocalizationComment(this._toolStripStatus, null);
			this.locExtender.SetLocalizationPriority(this._toolStripStatus, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._toolStripStatus, "toolStrip1.toolStrip1");
			this._toolStripStatus.Location = new System.Drawing.Point(470, 140);
			this._toolStripStatus.Name = "_toolStripStatus";
			this._toolStripStatus.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this._toolStripStatus.Size = new System.Drawing.Size(233, 25);
			this._toolStripStatus.TabIndex = 10;
			//
			// _labelTimeDisplay
			//
			this._labelTimeDisplay.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.locExtender.SetLocalizableToolTip(this._labelTimeDisplay, null);
			this.locExtender.SetLocalizationComment(this._labelTimeDisplay, null);
			this.locExtender.SetLocalizationPriority(this._labelTimeDisplay, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelTimeDisplay, ".toolStripLabel1");
			this._labelTimeDisplay.Margin = new System.Windows.Forms.Padding(35, 1, 0, 2);
			this._labelTimeDisplay.Name = "_labelTimeDisplay";
			this._labelTimeDisplay.Size = new System.Drawing.Size(14, 22);
			this._labelTimeDisplay.Text = "#";
			//
			// _labelSegmentXofY
			//
			this._labelSegmentXofY.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.locExtender.SetLocalizableToolTip(this._labelSegmentXofY, null);
			this.locExtender.SetLocalizationComment(this._labelSegmentXofY, null);
			this.locExtender.SetLocalizingId(this._labelSegmentXofY, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._labelSegmentXofY");
			this._labelSegmentXofY.Name = "_labelSegmentXofY";
			this._labelSegmentXofY.Size = new System.Drawing.Size(102, 22);
			this._labelSegmentXofY.Text = "Segment {0} of {1}";
			//
			// _labelSegmentNumber
			//
			this._labelSegmentNumber.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.locExtender.SetLocalizableToolTip(this._labelSegmentNumber, null);
			this.locExtender.SetLocalizationComment(this._labelSegmentNumber, null);
			this.locExtender.SetLocalizingId(this._labelSegmentNumber, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._labelSegmentNumber");
			this._labelSegmentNumber.Name = "_labelSegmentNumber";
			this._labelSegmentNumber.Size = new System.Drawing.Size(79, 22);
			this._labelSegmentNumber.Text = "Segments: {0}";
			//
			// _buttonOK
			//
			this._buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonOK.AutoSize = true;
			this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._buttonOK, null);
			this.locExtender.SetLocalizationComment(this._buttonOK, null);
			this.locExtender.SetLocalizingId(this._buttonOK, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._buttonClose");
			this._buttonOK.Location = new System.Drawing.Point(535, 17);
			this._buttonOK.Margin = new System.Windows.Forms.Padding(3, 12, 3, 12);
			this._buttonOK.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonOK.Name = "_buttonOK";
			this._buttonOK.Size = new System.Drawing.Size(75, 26);
			this._buttonOK.TabIndex = 7;
			this._buttonOK.Text = "OK";
			this._buttonOK.UseVisualStyleBackColor = true;
			//
			// _buttonCancel
			//
			this._buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._buttonCancel");
			this._buttonCancel.Location = new System.Drawing.Point(616, 17);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(3, 12, 12, 12);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(75, 26);
			this._buttonCancel.TabIndex = 8;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// _tableLayoutButtons
			//
			this._tableLayoutButtons.ColumnCount = 4;
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutButtons.Controls.Add(this._buttonOK, 2, 0);
			this._tableLayoutButtons.Controls.Add(this._buttonCancel, 3, 0);
			this._tableLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._tableLayoutButtons.Location = new System.Drawing.Point(0, 302);
			this._tableLayoutButtons.Name = "_tableLayoutButtons";
			this._tableLayoutButtons.RowCount = 1;
			this._tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutButtons.Size = new System.Drawing.Size(703, 60);
			this._tableLayoutButtons.TabIndex = 7;
			this._tableLayoutButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleTableLayoutButtonsPaint);
			//
			// SegmenterDlgBase
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(703, 362);
			this.Controls.Add(this._tableLayoutOuter);
			this.Controls.Add(this._tableLayoutButtons);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(330, 400);
			this.Name = "SegmenterDlgBase";
			this.Opacity = 0D;
			this.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Change this text";
			this._tableLayoutOuter.ResumeLayout(false);
			this._tableLayoutOuter.PerformLayout();
			this._tableLayoutTop.ResumeLayout(false);
			this._tableLayoutTop.PerformLayout();
			this._toolStripStatus.ResumeLayout(false);
			this._toolStripStatus.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this._tableLayoutButtons.ResumeLayout(false);
			this._tableLayoutButtons.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox _comboBoxZoom;
		private System.Windows.Forms.Label _labelZoom;
		private System.Windows.Forms.Button _buttonOK;
		private System.Windows.Forms.Button _buttonCancel;
		private Localization.UI.LocalizationExtender locExtender;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
		protected Panel _panelWaveControl;
		private System.Windows.Forms.ToolStripLabel _labelTimeDisplay;
		protected System.Windows.Forms.ToolStripLabel _labelSegmentXofY;
		protected System.Windows.Forms.ToolStripLabel _labelSegmentNumber;
		protected System.Windows.Forms.Label _labelOriginalRecording;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutTop;
		protected TableLayoutPanel _tableLayoutButtons;
		protected ToolStrip _toolStripStatus;
	}
}
