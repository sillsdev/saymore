namespace SayMore.UI.Overview.Statistics
{
    partial class StatisticsView
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
			this.components = new System.ComponentModel.Container();
			this._refreshTimer = new System.Windows.Forms.Timer(this.components);
			this._webBrowser = new System.Windows.Forms.WebBrowser();
			this._panelBrowser = new Palaso.UI.WindowsForms.Widgets.EnhancedPanel();
			this._panelWorking = new Palaso.UI.WindowsForms.Widgets.EnhancedPanel();
			this._tableLayoutWorking = new System.Windows.Forms.TableLayoutPanel();
			this._labelWorking = new System.Windows.Forms.Label();
			this._pictureWorking = new System.Windows.Forms.PictureBox();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._toolStripActions = new SayMore.UI.LowLevelControls.ElementBar();
			this._buttonRefresh = new System.Windows.Forms.ToolStripButton();
			this._buttonCopy = new System.Windows.Forms.ToolStripButton();
			this._buttonSave = new System.Windows.Forms.ToolStripButton();
			this._buttonPrint = new System.Windows.Forms.ToolStripButton();
			this._panelBrowser.SuspendLayout();
			this._panelWorking.SuspendLayout();
			this._tableLayoutWorking.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureWorking)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this._toolStripActions.SuspendLayout();
			this.SuspendLayout();
			// 
			// _refreshTimer
			// 
			this._refreshTimer.Enabled = true;
			this._refreshTimer.Interval = 1000;
			this._refreshTimer.Tick += new System.EventHandler(this.HandleTimerTick);
			// 
			// _webBrowser
			// 
			this._webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this._webBrowser, null);
			this.locExtender.SetLocalizationComment(this._webBrowser, null);
			this.locExtender.SetLocalizationPriority(this._webBrowser, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._webBrowser, "ProjectView.ProgressScreen._webBrowser");
			this._webBrowser.Location = new System.Drawing.Point(0, 0);
			this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this._webBrowser.Name = "_webBrowser";
			this._webBrowser.Size = new System.Drawing.Size(502, 263);
			this._webBrowser.TabIndex = 5;
			// 
			// _panelBrowser
			// 
			this._panelBrowser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelBrowser.ClipTextForChildControls = true;
			this._panelBrowser.ControlReceivingFocusOnMnemonic = null;
			this._panelBrowser.Controls.Add(this._panelWorking);
			this._panelBrowser.Controls.Add(this._webBrowser);
			this._panelBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelBrowser.DoubleBuffered = true;
			this._panelBrowser.DrawOnlyBottomBorder = false;
			this._panelBrowser.DrawOnlyTopBorder = false;
			this._panelBrowser.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelBrowser.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelBrowser, null);
			this.locExtender.SetLocalizationComment(this._panelBrowser, null);
			this.locExtender.SetLocalizationPriority(this._panelBrowser, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._panelBrowser, "ProjectView.ProgressScreen._panelBrowser");
			this._panelBrowser.Location = new System.Drawing.Point(0, 27);
			this._panelBrowser.Margin = new System.Windows.Forms.Padding(0);
			this._panelBrowser.MnemonicGeneratesClick = false;
			this._panelBrowser.Name = "_panelBrowser";
			this._panelBrowser.PaintExplorerBarBackground = false;
			this._panelBrowser.Size = new System.Drawing.Size(504, 265);
			this._panelBrowser.TabIndex = 6;
			// 
			// _panelWorking
			// 
			this._panelWorking.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._panelWorking.AutoSize = true;
			this._panelWorking.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._panelWorking.BackColor = System.Drawing.Color.White;
			this._panelWorking.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelWorking.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelWorking.ClipTextForChildControls = true;
			this._panelWorking.ControlReceivingFocusOnMnemonic = null;
			this._panelWorking.Controls.Add(this._tableLayoutWorking);
			this._panelWorking.DoubleBuffered = true;
			this._panelWorking.DrawOnlyBottomBorder = false;
			this._panelWorking.DrawOnlyTopBorder = false;
			this._panelWorking.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelWorking.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelWorking, null);
			this.locExtender.SetLocalizationComment(this._panelWorking, null);
			this.locExtender.SetLocalizationPriority(this._panelWorking, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._panelWorking, "ProjectView.ProgressScreen._panelWorking");
			this._panelWorking.Location = new System.Drawing.Point(176, 103);
			this._panelWorking.MnemonicGeneratesClick = false;
			this._panelWorking.Name = "_panelWorking";
			this._panelWorking.PaintExplorerBarBackground = false;
			this._panelWorking.Size = new System.Drawing.Size(150, 56);
			this._panelWorking.TabIndex = 7;
			// 
			// _tableLayoutWorking
			// 
			this._tableLayoutWorking.AutoSize = true;
			this._tableLayoutWorking.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutWorking.BackColor = System.Drawing.Color.Transparent;
			this._tableLayoutWorking.ColumnCount = 2;
			this._tableLayoutWorking.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutWorking.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutWorking.Controls.Add(this._labelWorking, 1, 0);
			this._tableLayoutWorking.Controls.Add(this._pictureWorking, 0, 0);
			this._tableLayoutWorking.Location = new System.Drawing.Point(0, 0);
			this._tableLayoutWorking.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutWorking.Name = "_tableLayoutWorking";
			this._tableLayoutWorking.RowCount = 1;
			this._tableLayoutWorking.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutWorking.Size = new System.Drawing.Size(148, 54);
			this._tableLayoutWorking.TabIndex = 0;
			// 
			// _labelWorking
			// 
			this._labelWorking.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelWorking.AutoSize = true;
			this._labelWorking.BackColor = System.Drawing.Color.Transparent;
			this._labelWorking.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelWorking, null);
			this.locExtender.SetLocalizationComment(this._labelWorking, null);
			this.locExtender.SetLocalizingId(this._labelWorking, "ProjectView.ProgressScreen._labelWorking");
			this._labelWorking.Location = new System.Drawing.Point(57, 16);
			this._labelWorking.Name = "_labelWorking";
			this._labelWorking.Size = new System.Drawing.Size(88, 21);
			this._labelWorking.TabIndex = 6;
			this._labelWorking.Text = "Working...";
			// 
			// _pictureWorking
			// 
			this._pictureWorking.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._pictureWorking.Image = global::SayMore.Properties.Resources.BusyWheelLarge;
			this.locExtender.SetLocalizableToolTip(this._pictureWorking, null);
			this.locExtender.SetLocalizationComment(this._pictureWorking, null);
			this.locExtender.SetLocalizationPriority(this._pictureWorking, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pictureWorking, "ProjectView.ProgressScreen._pictureWorking");
			this._pictureWorking.Location = new System.Drawing.Point(3, 3);
			this._pictureWorking.Name = "_pictureWorking";
			this._pictureWorking.Size = new System.Drawing.Size(48, 48);
			this._pictureWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureWorking.TabIndex = 0;
			this._pictureWorking.TabStop = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = null;
			// 
			// _toolStripActions
			// 
			this._toolStripActions.BackColorBegin = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(150)))), ((int)(((byte)(100)))));
			this._toolStripActions.BackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(66)))), ((int)(((byte)(0)))));
			this._toolStripActions.GradientAngle = 0F;
			this._toolStripActions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStripActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._buttonRefresh,
            this._buttonCopy,
            this._buttonSave,
            this._buttonPrint});
			this.locExtender.SetLocalizableToolTip(this._toolStripActions, null);
			this.locExtender.SetLocalizationComment(this._toolStripActions, null);
			this.locExtender.SetLocalizationPriority(this._toolStripActions, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._toolStripActions, "ProjectView.ProgressScreen._toolStripActions");
			this._toolStripActions.Location = new System.Drawing.Point(0, 0);
			this._toolStripActions.Name = "_toolStripActions";
			this._toolStripActions.Padding = new System.Windows.Forms.Padding(7, 0, 7, 2);
			this._toolStripActions.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this._toolStripActions.Size = new System.Drawing.Size(504, 27);
			this._toolStripActions.TabIndex = 8;
			this._toolStripActions.Text = "toolStrip1";
			// 
			// _buttonRefresh
			// 
			this._buttonRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._buttonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this._buttonRefresh.Image = global::SayMore.Properties.Resources.Refresh;
			this._buttonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonRefresh, "Refresh View");
			this.locExtender.SetLocalizationComment(this._buttonRefresh, null);
			this.locExtender.SetLocalizingId(this._buttonRefresh, "ProjectView.ProgressScreen._buttonRefresh");
			this._buttonRefresh.Name = "_buttonRefresh";
			this._buttonRefresh.Padding = new System.Windows.Forms.Padding(1);
			this._buttonRefresh.Size = new System.Drawing.Size(23, 22);
			this._buttonRefresh.Text = "&Refresh";
			this._buttonRefresh.ToolTipText = "Refresh View";
			this._buttonRefresh.Click += new System.EventHandler(this.HandleRefreshButtonClicked);
			// 
			// _buttonCopy
			// 
			this._buttonCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._buttonCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonCopy, null);
			this.locExtender.SetLocalizationComment(this._buttonCopy, null);
			this.locExtender.SetLocalizingId(this._buttonCopy, "ProjectView.ProgressScreen._buttonCopy");
			this._buttonCopy.Name = "_buttonCopy";
			this._buttonCopy.Padding = new System.Windows.Forms.Padding(1);
			this._buttonCopy.Size = new System.Drawing.Size(41, 22);
			this._buttonCopy.Text = "Copy";
			this._buttonCopy.Click += new System.EventHandler(this.HandleCopyToClipboardClick);
			// 
			// _buttonSave
			// 
			this._buttonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._buttonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonSave, null);
			this.locExtender.SetLocalizationComment(this._buttonSave, null);
			this.locExtender.SetLocalizingId(this._buttonSave, "ProjectView.ProgressScreen._buttonSave");
			this._buttonSave.Name = "_buttonSave";
			this._buttonSave.Padding = new System.Windows.Forms.Padding(1);
			this._buttonSave.Size = new System.Drawing.Size(37, 22);
			this._buttonSave.Text = "Save";
			this._buttonSave.ToolTipText = "Save";
			this._buttonSave.Click += new System.EventHandler(this.HandleSaveButtonClicked);
			// 
			// _buttonPrint
			// 
			this._buttonPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this._buttonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.locExtender.SetLocalizableToolTip(this._buttonPrint, null);
			this.locExtender.SetLocalizationComment(this._buttonPrint, null);
			this.locExtender.SetLocalizingId(this._buttonPrint, "ProjectView.ProgressScreen._buttonPrint");
			this._buttonPrint.Name = "_buttonPrint";
			this._buttonPrint.Padding = new System.Windows.Forms.Padding(1);
			this._buttonPrint.Size = new System.Drawing.Size(38, 22);
			this._buttonPrint.Text = "Print";
			this._buttonPrint.Click += new System.EventHandler(this.HandlePrintButtonClicked);
			// 
			// StatisticsView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._panelBrowser);
			this.Controls.Add(this._toolStripActions);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "SayMore");
			this.Name = "StatisticsView";
			this.Size = new System.Drawing.Size(504, 292);
			this._panelBrowser.ResumeLayout(false);
			this._panelBrowser.PerformLayout();
			this._panelWorking.ResumeLayout(false);
			this._panelWorking.PerformLayout();
			this._tableLayoutWorking.ResumeLayout(false);
			this._tableLayoutWorking.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureWorking)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this._toolStripActions.ResumeLayout(false);
			this._toolStripActions.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Timer _refreshTimer;
		private System.Windows.Forms.WebBrowser _webBrowser;
		private SayMore.UI.LowLevelControls.ElementBar _toolStripActions;
		private System.Windows.Forms.ToolStripButton _buttonRefresh;
		private Palaso.UI.WindowsForms.Widgets.EnhancedPanel _panelBrowser;
		private System.Windows.Forms.Label _labelWorking;
		private Palaso.UI.WindowsForms.Widgets.EnhancedPanel _panelWorking;
		private System.Windows.Forms.PictureBox _pictureWorking;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutWorking;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.ToolStripButton _buttonCopy;
		private System.Windows.Forms.ToolStripButton _buttonSave;
		private System.Windows.Forms.ToolStripButton _buttonPrint;
    }
}
