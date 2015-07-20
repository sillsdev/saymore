namespace SayMore.UI.ComponentEditors
{
	partial class MissingMediaFileEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissingMediaFileEditor));
			this.lblMediaFileMissing = new System.Windows.Forms.Label();
			this.lblExplanation = new System.Windows.Forms.Label();
			this._panelBrowser = new SIL.Windows.Forms.Widgets.EnhancedPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.linkHelpTopic = new System.Windows.Forms.LinkLabel();
			this.txtMissingMediaFilePath = new System.Windows.Forms.TextBox();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._panelBrowser.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			//
			// lblMediaFileMissing
			//
			this.lblMediaFileMissing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.lblMediaFileMissing.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblMediaFileMissing, null);
			this.locExtender.SetLocalizationComment(this.lblMediaFileMissing, null);
			this.locExtender.SetLocalizationPriority(this.lblMediaFileMissing, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this.lblMediaFileMissing, "SessionsView.MissingMediaFileEditor.lblMediaFileMissing");
			this.lblMediaFileMissing.Location = new System.Drawing.Point(3, 8);
			this.lblMediaFileMissing.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
			this.lblMediaFileMissing.Name = "lblMediaFileMissing";
			this.lblMediaFileMissing.Size = new System.Drawing.Size(427, 30);
			this.lblMediaFileMissing.TabIndex = 0;
			this.lblMediaFileMissing.Text = "The media file for this ELAN file could not be found. SayMore expected to find it" +
	" here:";
			//
			// lblExplanation
			//
			this.lblExplanation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.lblExplanation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblExplanation, null);
			this.locExtender.SetLocalizationComment(this.lblExplanation, null);
			this.locExtender.SetLocalizationPriority(this.lblExplanation, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this.lblExplanation, "SessionsView.MissingMediaFileEditor.lblExplanation");
			this.lblExplanation.Location = new System.Drawing.Point(3, 78);
			this.lblExplanation.Name = "lblExplanation";
			this.lblExplanation.Size = new System.Drawing.Size(427, 75);
			this.lblExplanation.TabIndex = 2;
			this.lblExplanation.Text = resources.GetString("lblExplanation.Text");
			//
			// _panelBrowser
			//
			this._panelBrowser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._panelBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._panelBrowser.ClipTextForChildControls = true;
			this._panelBrowser.ControlReceivingFocusOnMnemonic = null;
			this._panelBrowser.Controls.Add(this.tableLayoutPanel1);
			this._panelBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelBrowser.DoubleBuffered = true;
			this._panelBrowser.DrawOnlyBottomBorder = false;
			this._panelBrowser.DrawOnlyTopBorder = false;
			this._panelBrowser.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._panelBrowser.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._panelBrowser, null);
			this.locExtender.SetLocalizationComment(this._panelBrowser, null);
			this.locExtender.SetLocalizingId(this._panelBrowser, "MissingMediaFileEditor._panelBrowser");
			this._panelBrowser.Location = new System.Drawing.Point(7, 7);
			this._panelBrowser.MnemonicGeneratesClick = false;
			this._panelBrowser.Name = "_panelBrowser";
			this._panelBrowser.PaintExplorerBarBackground = false;
			this._panelBrowser.Size = new System.Drawing.Size(435, 265);
			this._panelBrowser.TabIndex = 19;
			//
			// tableLayoutPanel1
			//
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.linkHelpTopic, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblExplanation, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.txtMissingMediaFilePath, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblMediaFileMissing, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(433, 263);
			this.tableLayoutPanel1.TabIndex = 5;
			//
			// linkHelpTopic
			//
			this.linkHelpTopic.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.linkHelpTopic, null);
			this.locExtender.SetLocalizationComment(this.linkHelpTopic, null);
			this.locExtender.SetLocalizationPriority(this.linkHelpTopic, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this.linkHelpTopic, "SessionsView.MissingMediaFileEditor.linkHelpTopic");
			this.linkHelpTopic.Location = new System.Drawing.Point(3, 165);
			this.linkHelpTopic.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
			this.linkHelpTopic.Name = "linkHelpTopic";
			this.linkHelpTopic.Size = new System.Drawing.Size(144, 15);
			this.linkHelpTopic.TabIndex = 3;
			this.linkHelpTopic.TabStop = true;
			this.linkHelpTopic.Text = "Show Help topic for ELAN";
			this.linkHelpTopic.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleHelpTopicLinkClicked);
			//
			// txtMissingMediaFilePath
			//
			this.txtMissingMediaFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.txtMissingMediaFilePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(240)))), ((int)(((byte)(159)))));
			this.txtMissingMediaFilePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtMissingMediaFilePath.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtMissingMediaFilePath.HideSelection = false;
			this.locExtender.SetLocalizableToolTip(this.txtMissingMediaFilePath, null);
			this.locExtender.SetLocalizationComment(this.txtMissingMediaFilePath, null);
			this.locExtender.SetLocalizationPriority(this.txtMissingMediaFilePath, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.txtMissingMediaFilePath, "SessionsView.MissingMediaFileEditor.txtMissingMediaFilePath");
			this.txtMissingMediaFilePath.Location = new System.Drawing.Point(12, 50);
			this.txtMissingMediaFilePath.Margin = new System.Windows.Forms.Padding(12, 12, 3, 12);
			this.txtMissingMediaFilePath.Name = "txtMissingMediaFilePath";
			this.txtMissingMediaFilePath.ReadOnly = true;
			this.txtMissingMediaFilePath.Size = new System.Drawing.Size(418, 16);
			this.txtMissingMediaFilePath.TabIndex = 1;
			this.txtMissingMediaFilePath.Text = "#";
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// MissingMediaFileEditor
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._panelBrowser);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.MediumLow);
			this.locExtender.SetLocalizingId(this, "SessionsView.MissingMediaFileEditor");
			this.Name = "MissingMediaFileEditor";
			this.Size = new System.Drawing.Size(449, 279);
			this._panelBrowser.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SIL.Windows.Forms.Widgets.EnhancedPanel _panelBrowser;
		private System.Windows.Forms.LinkLabel linkHelpTopic;
		private System.Windows.Forms.TextBox txtMissingMediaFilePath;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.Label lblMediaFileMissing;
		private System.Windows.Forms.Label lblExplanation;
	}
}
