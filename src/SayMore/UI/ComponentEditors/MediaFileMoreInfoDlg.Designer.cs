namespace SayMore.UI.ComponentEditors
{
	partial class MediaFileMoreInfoDlg
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
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._buttonClose = new System.Windows.Forms.Button();
			this._panelBrowser = new Palaso.UI.WindowsForms.Widgets.EnhancedPanel();
			this._webBrowserInfo = new System.Windows.Forms.WebBrowser();
			this._flowLayoutButtons = new System.Windows.Forms.FlowLayoutPanel();
			this._buttonEvenMoreInfo = new System.Windows.Forms.Button();
			this._buttonLessInfo = new System.Windows.Forms.Button();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this._panelBrowser.SuspendLayout();
			this._flowLayoutButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._buttonClose, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this._panelBrowser, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._flowLayoutButtons, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 15);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(523, 365);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _buttonClose
			// 
			this._buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonClose.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._buttonClose, null);
			this.locExtender.SetLocalizationComment(this._buttonClose, null);
			this.locExtender.SetLocalizingId(this._buttonClose, "DialogBoxes.MediaFileMoreInfoDlg.CloseButton");
			this._buttonClose.Location = new System.Drawing.Point(448, 339);
			this._buttonClose.Margin = new System.Windows.Forms.Padding(6, 15, 0, 0);
			this._buttonClose.Name = "_buttonClose";
			this._buttonClose.Size = new System.Drawing.Size(75, 26);
			this._buttonClose.TabIndex = 0;
			this._buttonClose.Text = "Close";
			this._buttonClose.UseVisualStyleBackColor = true;
			// 
			// _panelBrowser
			// 
			this._panelBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._panelBrowser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelBrowser.ClipTextForChildControls = true;
			this.tableLayoutPanel1.SetColumnSpan(this._panelBrowser, 2);
			this._panelBrowser.ControlReceivingFocusOnMnemonic = null;
			this._panelBrowser.Controls.Add(this._webBrowserInfo);
			this._panelBrowser.DoubleBuffered = true;
			this._panelBrowser.DrawOnlyBottomBorder = false;
			this._panelBrowser.DrawOnlyTopBorder = false;
			this._panelBrowser.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._panelBrowser.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelBrowser, null);
			this.locExtender.SetLocalizationComment(this._panelBrowser, null);
			this.locExtender.SetLocalizingId(this._panelBrowser, "MediaFileMoreInfoDlg._panelBrowser");
			this._panelBrowser.Location = new System.Drawing.Point(0, 0);
			this._panelBrowser.Margin = new System.Windows.Forms.Padding(0);
			this._panelBrowser.MnemonicGeneratesClick = false;
			this._panelBrowser.Name = "_panelBrowser";
			this._panelBrowser.PaintExplorerBarBackground = false;
			this._panelBrowser.Size = new System.Drawing.Size(523, 324);
			this._panelBrowser.TabIndex = 4;
			// 
			// _webBrowserInfo
			// 
			this._webBrowserInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this._webBrowserInfo, null);
			this.locExtender.SetLocalizationComment(this._webBrowserInfo, null);
			this.locExtender.SetLocalizingId(this._webBrowserInfo, "MediaFileMoreInfoDlg._webBrowserInfo");
			this._webBrowserInfo.Location = new System.Drawing.Point(0, 0);
			this._webBrowserInfo.MinimumSize = new System.Drawing.Size(20, 20);
			this._webBrowserInfo.Name = "_webBrowserInfo";
			this._webBrowserInfo.Size = new System.Drawing.Size(521, 322);
			this._webBrowserInfo.TabIndex = 4;
			// 
			// _flowLayoutButtons
			// 
			this._flowLayoutButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._flowLayoutButtons.AutoSize = true;
			this._flowLayoutButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._flowLayoutButtons.Controls.Add(this._buttonEvenMoreInfo);
			this._flowLayoutButtons.Controls.Add(this._buttonLessInfo);
			this._flowLayoutButtons.Location = new System.Drawing.Point(0, 339);
			this._flowLayoutButtons.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
			this._flowLayoutButtons.Name = "_flowLayoutButtons";
			this._flowLayoutButtons.Size = new System.Drawing.Size(442, 26);
			this._flowLayoutButtons.TabIndex = 3;
			this._flowLayoutButtons.WrapContents = false;
			// 
			// _buttonEvenMoreInfo
			// 
			this._buttonEvenMoreInfo.AutoSize = true;
			this._buttonEvenMoreInfo.Image = global::SayMore.Properties.Resources.MoreArrows;
			this._buttonEvenMoreInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.locExtender.SetLocalizableToolTip(this._buttonEvenMoreInfo, null);
			this.locExtender.SetLocalizationComment(this._buttonEvenMoreInfo, null);
			this.locExtender.SetLocalizingId(this._buttonEvenMoreInfo, "DialogBoxes.MediaFileMoreInfoDlg.EvenMoreInfoButton");
			this._buttonEvenMoreInfo.Location = new System.Drawing.Point(0, 0);
			this._buttonEvenMoreInfo.Margin = new System.Windows.Forms.Padding(0);
			this._buttonEvenMoreInfo.Name = "_buttonEvenMoreInfo";
			this._buttonEvenMoreInfo.Size = new System.Drawing.Size(135, 26);
			this._buttonEvenMoreInfo.TabIndex = 1;
			this._buttonEvenMoreInfo.Text = "Even More Information";
			this._buttonEvenMoreInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonEvenMoreInfo.UseVisualStyleBackColor = true;
			this._buttonEvenMoreInfo.Click += new System.EventHandler(this.HandleEvenMoreInfoButtonClick);
			// 
			// _buttonLessInfo
			// 
			this._buttonLessInfo.AutoSize = true;
			this._buttonLessInfo.Image = global::SayMore.Properties.Resources.LessArrows;
			this._buttonLessInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.locExtender.SetLocalizableToolTip(this._buttonLessInfo, null);
			this.locExtender.SetLocalizationComment(this._buttonLessInfo, null);
			this.locExtender.SetLocalizingId(this._buttonLessInfo, "DialogBoxes.MediaFileMoreInfoDlg.LessInfoButton");
			this._buttonLessInfo.Location = new System.Drawing.Point(135, 0);
			this._buttonLessInfo.Margin = new System.Windows.Forms.Padding(0);
			this._buttonLessInfo.Name = "_buttonLessInfo";
			this._buttonLessInfo.Size = new System.Drawing.Size(106, 26);
			this._buttonLessInfo.TabIndex = 5;
			this._buttonLessInfo.Text = "Less Information";
			this._buttonLessInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._buttonLessInfo.UseVisualStyleBackColor = true;
			this._buttonLessInfo.Visible = false;
			this._buttonLessInfo.Click += new System.EventHandler(this.HandleLessInfoButtonClick);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// MediaFileMoreInfoDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(553, 395);
			this.Controls.Add(this.tableLayoutPanel1);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.MediaFileMoreInfoDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(450, 350);
			this.Name = "MediaFileMoreInfoDlg";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "More Information for Media File";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this._panelBrowser.ResumeLayout(false);
			this._flowLayoutButtons.ResumeLayout(false);
			this._flowLayoutButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel _flowLayoutButtons;
		private System.Windows.Forms.Button _buttonClose;
		private System.Windows.Forms.Button _buttonEvenMoreInfo;
		private System.Windows.Forms.WebBrowser _webBrowserInfo;
		private Palaso.UI.WindowsForms.Widgets.EnhancedPanel _panelBrowser;
		private System.Windows.Forms.Button _buttonLessInfo;
		private L10NSharp.UI.L10NSharpExtender locExtender;
	}
}