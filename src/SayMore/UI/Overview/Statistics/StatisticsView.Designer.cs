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
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this._webBrowser = new System.Windows.Forms.WebBrowser();
			this._toolStripActions = new System.Windows.Forms.ToolStrip();
			this._labelStatus = new System.Windows.Forms.ToolStripLabel();
			this._buttonCopyToClipboard = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this._buttonSaveToFile = new System.Windows.Forms.ToolStripButton();
			this._buttonPrint = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this._buttonRefresh = new System.Windows.Forms.ToolStripButton();
			this._panelBrowser = new SilUtils.Controls.SilPanel();
			this._toolStripActions.SuspendLayout();
			this._panelBrowser.SuspendLayout();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.HandleTimerTick);
			// 
			// _webBrowser
			// 
			this._webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._webBrowser.Location = new System.Drawing.Point(0, 0);
			this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this._webBrowser.Name = "_webBrowser";
			this._webBrowser.Size = new System.Drawing.Size(533, 285);
			this._webBrowser.TabIndex = 5;
			// 
			// _toolStripActions
			// 
			this._toolStripActions.BackColor = System.Drawing.SystemColors.Control;
			this._toolStripActions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStripActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._labelStatus,
            this._buttonCopyToClipboard,
            this.toolStripSeparator1,
            this._buttonSaveToFile,
            this._buttonPrint,
            this.toolStripSeparator2,
            this._buttonRefresh});
			this._toolStripActions.Location = new System.Drawing.Point(0, 0);
			this._toolStripActions.Name = "_toolStripActions";
			this._toolStripActions.Padding = new System.Windows.Forms.Padding(7, 0, 7, 2);
			this._toolStripActions.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this._toolStripActions.Size = new System.Drawing.Size(535, 25);
			this._toolStripActions.TabIndex = 8;
			this._toolStripActions.Text = "toolStrip1";
			// 
			// _labelStatus
			// 
			this._labelStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._labelStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._labelStatus.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
			this._labelStatus.Name = "_labelStatus";
			this._labelStatus.Size = new System.Drawing.Size(42, 20);
			this._labelStatus.Text = "Status";
			// 
			// _buttonCopyToClipboard
			// 
			this._buttonCopyToClipboard.Image = global::SayMore.Properties.Resources.Copy;
			this._buttonCopyToClipboard.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonCopyToClipboard.Name = "_buttonCopyToClipboard";
			this._buttonCopyToClipboard.Size = new System.Drawing.Size(55, 20);
			this._buttonCopyToClipboard.Text = "Copy";
			this._buttonCopyToClipboard.ToolTipText = "Copies entire chart to clipboard";
			this._buttonCopyToClipboard.Click += new System.EventHandler(this.HandleCopyToClipboardClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
			// 
			// _buttonSaveToFile
			// 
			this._buttonSaveToFile.Image = global::SayMore.Properties.Resources.Save;
			this._buttonSaveToFile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonSaveToFile.Name = "_buttonSaveToFile";
			this._buttonSaveToFile.Size = new System.Drawing.Size(60, 20);
			this._buttonSaveToFile.Text = "Save...";
			this._buttonSaveToFile.ToolTipText = "Save to file";
			this._buttonSaveToFile.Click += new System.EventHandler(this.HandleSaveButtonClicked);
			// 
			// _buttonPrint
			// 
			this._buttonPrint.Image = global::SayMore.Properties.Resources.Print;
			this._buttonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonPrint.Name = "_buttonPrint";
			this._buttonPrint.Size = new System.Drawing.Size(61, 20);
			this._buttonPrint.Text = "Print...";
			this._buttonPrint.ToolTipText = "Print";
			this._buttonPrint.Click += new System.EventHandler(this.HandlePrintButtonClicked);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
			// 
			// _buttonRefresh
			// 
			this._buttonRefresh.Image = global::SayMore.Properties.Resources.Refresh;
			this._buttonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this._buttonRefresh.Name = "_buttonRefresh";
			this._buttonRefresh.Size = new System.Drawing.Size(66, 20);
			this._buttonRefresh.Text = "Refresh";
			this._buttonRefresh.ToolTipText = "Refresh View";
			this._buttonRefresh.Click += new System.EventHandler(this.HandleRefreshButtonClicked);
			// 
			// _panelBrowser
			// 
			this._panelBrowser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelBrowser.ClipTextForChildControls = true;
			this._panelBrowser.ControlReceivingFocusOnMnemonic = null;
			this._panelBrowser.Controls.Add(this._webBrowser);
			this._panelBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelBrowser.DoubleBuffered = true;
			this._panelBrowser.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelBrowser.Location = new System.Drawing.Point(0, 25);
			this._panelBrowser.Margin = new System.Windows.Forms.Padding(0);
			this._panelBrowser.MnemonicGeneratesClick = false;
			this._panelBrowser.Name = "_panelBrowser";
			this._panelBrowser.PaintExplorerBarBackground = false;
			this._panelBrowser.Size = new System.Drawing.Size(535, 287);
			this._panelBrowser.TabIndex = 6;
			// 
			// StatisticsView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._panelBrowser);
			this.Controls.Add(this._toolStripActions);
			this.Name = "StatisticsView";
			this.Size = new System.Drawing.Size(535, 312);
			this._toolStripActions.ResumeLayout(false);
			this._toolStripActions.PerformLayout();
			this._panelBrowser.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.WebBrowser _webBrowser;
		private System.Windows.Forms.ToolStrip _toolStripActions;
		private System.Windows.Forms.ToolStripButton _buttonRefresh;
		private System.Windows.Forms.ToolStripButton _buttonPrint;
		private System.Windows.Forms.ToolStripButton _buttonSaveToFile;
		private System.Windows.Forms.ToolStripLabel _labelStatus;
		private SilUtils.Controls.SilPanel _panelBrowser;
		private System.Windows.Forms.ToolStripButton _buttonCopyToClipboard;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}
