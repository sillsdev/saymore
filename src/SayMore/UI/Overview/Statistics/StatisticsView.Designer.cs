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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsView));
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this._webBrowser = new System.Windows.Forms.WebBrowser();
			this._toolStripActions = new System.Windows.Forms.ToolStrip();
			this._labelStatus = new System.Windows.Forms.ToolStripLabel();
			this._buttonRefresh = new System.Windows.Forms.ToolStripButton();
			this._panelBrowser = new SilUtils.Controls.SilPanel();
			this._panelWorking = new SilUtils.Controls.SilPanel();
			this._tableLayoutWorking = new System.Windows.Forms.TableLayoutPanel();
			this._labelWorking = new System.Windows.Forms.Label();
			this._pictureWorking = new System.Windows.Forms.PictureBox();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this._toolStripActions.SuspendLayout();
			this._panelBrowser.SuspendLayout();
			this._panelWorking.SuspendLayout();
			this._tableLayoutWorking.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureWorking)).BeginInit();
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
			this._webBrowser.Size = new System.Drawing.Size(502, 265);
			this._webBrowser.TabIndex = 5;
			// 
			// _toolStripActions
			// 
			this._toolStripActions.BackColor = System.Drawing.SystemColors.Control;
			this._toolStripActions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this._toolStripActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._labelStatus,
            this._buttonRefresh,
            this.toolStripButton1});
			this._toolStripActions.Location = new System.Drawing.Point(0, 0);
			this._toolStripActions.Name = "_toolStripActions";
			this._toolStripActions.Padding = new System.Windows.Forms.Padding(7, 0, 7, 2);
			this._toolStripActions.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this._toolStripActions.Size = new System.Drawing.Size(504, 25);
			this._toolStripActions.TabIndex = 8;
			this._toolStripActions.Text = "toolStrip1";
			// 
			// _labelStatus
			// 
			this._labelStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this._labelStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelStatus.Image = global::SayMore.Properties.Resources.BusyWheelSmall;
			this._labelStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this._labelStatus.Name = "_labelStatus";
			this._labelStatus.Size = new System.Drawing.Size(58, 20);
			this._labelStatus.Text = "Status";
			// 
			// _buttonRefresh
			// 
			this._buttonRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
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
			this._panelBrowser.Controls.Add(this._panelWorking);
			this._panelBrowser.Controls.Add(this._webBrowser);
			this._panelBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelBrowser.DoubleBuffered = true;
			this._panelBrowser.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelBrowser.Location = new System.Drawing.Point(0, 25);
			this._panelBrowser.Margin = new System.Windows.Forms.Padding(0);
			this._panelBrowser.MnemonicGeneratesClick = false;
			this._panelBrowser.Name = "_panelBrowser";
			this._panelBrowser.PaintExplorerBarBackground = false;
			this._panelBrowser.Size = new System.Drawing.Size(504, 267);
			this._panelBrowser.TabIndex = 6;
			// 
			// _panelWorking
			// 
			this._panelWorking.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._panelWorking.AutoSize = true;
			this._panelWorking.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._panelWorking.BackColor = System.Drawing.Color.White;
			this._panelWorking.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(110)))), ((int)(((byte)(145)))));
			this._panelWorking.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelWorking.ClipTextForChildControls = true;
			this._panelWorking.ControlReceivingFocusOnMnemonic = null;
			this._panelWorking.Controls.Add(this._tableLayoutWorking);
			this._panelWorking.DoubleBuffered = true;
			this._panelWorking.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelWorking.Location = new System.Drawing.Point(176, 104);
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
			this._pictureWorking.Location = new System.Drawing.Point(3, 3);
			this._pictureWorking.Name = "_pictureWorking";
			this._pictureWorking.Size = new System.Drawing.Size(48, 48);
			this._pictureWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._pictureWorking.TabIndex = 0;
			this._pictureWorking.TabStop = false;
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(75, 20);
			this.toolStripButton1.Text = "Wrap Labels";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// StatisticsView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._panelBrowser);
			this.Controls.Add(this._toolStripActions);
			this.Name = "StatisticsView";
			this.Size = new System.Drawing.Size(504, 292);
			this._toolStripActions.ResumeLayout(false);
			this._toolStripActions.PerformLayout();
			this._panelBrowser.ResumeLayout(false);
			this._panelBrowser.PerformLayout();
			this._panelWorking.ResumeLayout(false);
			this._panelWorking.PerformLayout();
			this._tableLayoutWorking.ResumeLayout(false);
			this._tableLayoutWorking.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._pictureWorking)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.WebBrowser _webBrowser;
		private System.Windows.Forms.ToolStrip _toolStripActions;
		private System.Windows.Forms.ToolStripButton _buttonRefresh;
		private System.Windows.Forms.ToolStripLabel _labelStatus;
		private SilUtils.Controls.SilPanel _panelBrowser;
		private System.Windows.Forms.Label _labelWorking;
		private SilUtils.Controls.SilPanel _panelWorking;
		private System.Windows.Forms.PictureBox _pictureWorking;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutWorking;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}
