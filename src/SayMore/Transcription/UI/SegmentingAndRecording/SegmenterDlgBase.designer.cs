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

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._panelWaveControl = new System.Windows.Forms.Panel();
			this._lastSegmentMenuStrip = new System.Windows.Forms.MenuStrip();
			this._undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._currentSegmentMenuStrip = new System.Windows.Forms.MenuStrip();
			this._ignoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutTop = new System.Windows.Forms.TableLayoutPanel();
			this._labelSourceRecording = new System.Windows.Forms.Label();
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
			this._panelWaveControl.SuspendLayout();
			this._lastSegmentMenuStrip.SuspendLayout();
			this._currentSegmentMenuStrip.SuspendLayout();
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
			this._panelWaveControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._tableLayoutOuter.SetColumnSpan(this._panelWaveControl, 2);
			this._panelWaveControl.Controls.Add(this._lastSegmentMenuStrip);
			this._panelWaveControl.Controls.Add(this._currentSegmentMenuStrip);
			this._panelWaveControl.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.World);
			this._panelWaveControl.ForeColor = System.Drawing.SystemColors.ControlText;
			this._panelWaveControl.Location = new System.Drawing.Point(0, 29);
			this._panelWaveControl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
			this._panelWaveControl.Name = "_panelWaveControl";
			this._panelWaveControl.Padding = new System.Windows.Forms.Padding(1);
			this._panelWaveControl.Size = new System.Drawing.Size(739, 170);
			this._panelWaveControl.TabIndex = 1;
			//
			// _lastSegmentMenuStrip
			//
			this._lastSegmentMenuStrip.AllowMerge = false;
			this._lastSegmentMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(129)))), ((int)(((byte)(199)))));
			this._lastSegmentMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
			this._lastSegmentMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._undoToolStripMenuItem});
			this.locExtender.SetLocalizableToolTip(this._lastSegmentMenuStrip, null);
			this.locExtender.SetLocalizationComment(this._lastSegmentMenuStrip, null);
			this.locExtender.SetLocalizationPriority(this._lastSegmentMenuStrip, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._lastSegmentMenuStrip, "OralAnnotationRecorderBaseDlg._lastSegmentMenuStrip");
			this._lastSegmentMenuStrip.Location = new System.Drawing.Point(311, 11);
			this._lastSegmentMenuStrip.Name = "_lastSegmentMenuStrip";
			this._lastSegmentMenuStrip.Padding = new System.Windows.Forms.Padding(1, 1, 0, 2);
			this._lastSegmentMenuStrip.ShowItemToolTips = true;
			this._lastSegmentMenuStrip.Size = new System.Drawing.Size(68, 24);
			this._lastSegmentMenuStrip.TabIndex = 16;
			this._lastSegmentMenuStrip.Visible = false;
			//
			// _undoToolStripMenuItem
			//
			this._undoToolStripMenuItem.BackColor = System.Drawing.Color.AliceBlue;
			this._undoToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._undoToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(129)))), ((int)(((byte)(199)))));
			this._undoToolStripMenuItem.Image = global::SayMore.Properties.Resources.undo;
			this.locExtender.SetLocalizableToolTip(this._undoToolStripMenuItem, null);
			this.locExtender.SetLocalizationComment(this._undoToolStripMenuItem, null);
			this.locExtender.SetLocalizingId(this._undoToolStripMenuItem, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._undoToolStripMenuItem");
			this._undoToolStripMenuItem.Name = "_undoToolStripMenuItem";
			this._undoToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this._undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this._undoToolStripMenuItem.ShowShortcutKeys = false;
			this._undoToolStripMenuItem.Size = new System.Drawing.Size(65, 21);
			this._undoToolStripMenuItem.Text = "Undo";
			this._undoToolStripMenuItem.Click += new System.EventHandler(this.HandleUndoButtonClick);
			//
			// _currentSegmentMenuStrip
			//
			this._currentSegmentMenuStrip.AllowMerge = false;
			this._currentSegmentMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(129)))), ((int)(((byte)(199)))));
			this._currentSegmentMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
			this._currentSegmentMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this._ignoreToolStripMenuItem});
			this.locExtender.SetLocalizableToolTip(this._currentSegmentMenuStrip, null);
			this.locExtender.SetLocalizationComment(this._currentSegmentMenuStrip, null);
			this.locExtender.SetLocalizationPriority(this._currentSegmentMenuStrip, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._currentSegmentMenuStrip, "OralAnnotationRecorderBaseDlg._lastSegmentMenuStrip");
			this._currentSegmentMenuStrip.Location = new System.Drawing.Point(532, 86);
			this._currentSegmentMenuStrip.Name = "_currentSegmentMenuStrip";
			this._currentSegmentMenuStrip.Padding = new System.Windows.Forms.Padding(1, 1, 0, 2);
			this._currentSegmentMenuStrip.ShowItemToolTips = true;
			this._currentSegmentMenuStrip.Size = new System.Drawing.Size(160, 24);
			this._currentSegmentMenuStrip.TabIndex = 20;
			this._currentSegmentMenuStrip.Visible = false;
			//
			// _ignoreToolStripMenuItem
			//
			this._ignoreToolStripMenuItem.BackColor = System.Drawing.Color.AliceBlue;
			this._ignoreToolStripMenuItem.CheckOnClick = true;
			this._ignoreToolStripMenuItem.Enabled = false;
			this._ignoreToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(129)))), ((int)(((byte)(199)))));
			this._ignoreToolStripMenuItem.Image = global::SayMore.Properties.Resources.UncheckedBox;
			this.locExtender.SetLocalizableToolTip(this._ignoreToolStripMenuItem, "Mark this segment as Ignored to skip annotating it.");
			this.locExtender.SetLocalizationComment(this._ignoreToolStripMenuItem, null);
			this.locExtender.SetLocalizationPriority(this._ignoreToolStripMenuItem, Localization.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._ignoreToolStripMenuItem, "OralAnnotationRecorderBaseDlg._skipToolStripMenuItem");
			this._ignoreToolStripMenuItem.Name = "_ignoreToolStripMenuItem";
			this._ignoreToolStripMenuItem.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this._ignoreToolStripMenuItem.ShowShortcutKeys = false;
			this._ignoreToolStripMenuItem.Size = new System.Drawing.Size(65, 21);
			this._ignoreToolStripMenuItem.Text = "Ignored";
			this._ignoreToolStripMenuItem.CheckedChanged += new System.EventHandler(this.HandleIgnoreToolStripMenuItemCheckedChanged);
			this._ignoreToolStripMenuItem.Click += new System.EventHandler(this.HandleIgnoreButtonClick);
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
			this._tableLayoutOuter.Location = new System.Drawing.Point(17, 17);
			this._tableLayoutOuter.Name = "_tableLayoutOuter";
			this._tableLayoutOuter.RowCount = 3;
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
			this._tableLayoutOuter.Size = new System.Drawing.Size(739, 352);
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
			this._tableLayoutTop.Controls.Add(this._labelSourceRecording, 0, 0);
			this._tableLayoutTop.Controls.Add(this._labelZoom, 1, 0);
			this._tableLayoutTop.Controls.Add(this._comboBoxZoom, 2, 0);
			this._tableLayoutTop.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutTop.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
			this._tableLayoutTop.Name = "_tableLayoutTop";
			this._tableLayoutTop.RowCount = 1;
			this._tableLayoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutTop.Size = new System.Drawing.Size(739, 24);
			this._tableLayoutTop.TabIndex = 9;
			//
			// _labelSourceRecording
			//
			this._labelSourceRecording.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelSourceRecording.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelSourceRecording, null);
			this.locExtender.SetLocalizationComment(this._labelSourceRecording, null);
			this.locExtender.SetLocalizingId(this._labelSourceRecording, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._labelOriginalRecording");
			this._labelSourceRecording.Location = new System.Drawing.Point(0, 5);
			this._labelSourceRecording.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelSourceRecording.Name = "_labelSourceRecording";
			this._labelSourceRecording.Size = new System.Drawing.Size(71, 13);
			this._labelSourceRecording.TabIndex = 4;
			this._labelSourceRecording.Text = "Source Audio";
			//
			// _labelZoom
			//
			this._labelZoom.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._labelZoom.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelZoom, null);
			this.locExtender.SetLocalizationComment(this._labelZoom, null);
			this.locExtender.SetLocalizingId(this._labelZoom, "DialogBoxes.Transcription.CommonAnnotationSegmenterDlg._labelZoom");
			this._labelZoom.Location = new System.Drawing.Point(600, 5);
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
			this._comboBoxZoom.Location = new System.Drawing.Point(643, 0);
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
			this._toolStripStatus.Location = new System.Drawing.Point(506, 202);
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
			this._buttonOK.Location = new System.Drawing.Point(583, 17);
			this._buttonOK.Margin = new System.Windows.Forms.Padding(3, 12, 3, 12);
			this._buttonOK.MinimumSize = new System.Drawing.Size(75, 26);
			this._buttonOK.Name = "_buttonOK";
			this._tableLayoutButtons.SetRowSpan(this._buttonOK, 3);
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
			this._buttonCancel.Location = new System.Drawing.Point(664, 17);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(3, 12, 0, 12);
			this._buttonCancel.Name = "_buttonCancel";
			this._tableLayoutButtons.SetRowSpan(this._buttonCancel, 3);
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
			this._tableLayoutButtons.Location = new System.Drawing.Point(17, 369);
			this._tableLayoutButtons.Name = "_tableLayoutButtons";
			this._tableLayoutButtons.RowCount = 3;
			this._tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this._tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this._tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this._tableLayoutButtons.Size = new System.Drawing.Size(739, 60);
			this._tableLayoutButtons.TabIndex = 7;
			this._tableLayoutButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleTableLayoutButtonsPaint);
			//
			// SegmenterDlgBase
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this._buttonCancel;
			this.ClientSize = new System.Drawing.Size(773, 429);
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
			this.Padding = new System.Windows.Forms.Padding(17, 17, 17, 0);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Change this text";
			this._panelWaveControl.ResumeLayout(false);
			this._panelWaveControl.PerformLayout();
			this._lastSegmentMenuStrip.ResumeLayout(false);
			this._lastSegmentMenuStrip.PerformLayout();
			this._currentSegmentMenuStrip.ResumeLayout(false);
			this._currentSegmentMenuStrip.PerformLayout();
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
		protected System.Windows.Forms.Button _buttonOK;
		private Localization.UI.LocalizationExtender locExtender;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
		protected Panel _panelWaveControl;
		private System.Windows.Forms.ToolStripLabel _labelTimeDisplay;
		protected System.Windows.Forms.ToolStripLabel _labelSegmentXofY;
		protected System.Windows.Forms.ToolStripLabel _labelSegmentNumber;
		protected System.Windows.Forms.Label _labelSourceRecording;
		protected System.Windows.Forms.TableLayoutPanel _tableLayoutTop;
		protected TableLayoutPanel _tableLayoutButtons;
		protected ToolStrip _toolStripStatus;
		protected Button _buttonCancel;
		protected MenuStrip _currentSegmentMenuStrip;
		protected ToolStripMenuItem _ignoreToolStripMenuItem;
		private MenuStrip _lastSegmentMenuStrip;
		private ToolStripMenuItem _undoToolStripMenuItem;
	}
}
