namespace SayMore.UI.ComponentEditors
{
	partial class StatusAndStagesEditor
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
			this._tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._labelReadAboutStatus = new System.Windows.Forms.Label();
			this._labelStatus = new System.Windows.Forms.Label();
			this._buttonReadAboutStatus = new System.Windows.Forms.Button();
			this._labelStatusHint = new System.Windows.Forms.Label();
			this._labelStages = new System.Windows.Forms.Label();
			this._labelReadAboutStages = new System.Windows.Forms.Label();
			this._labelStagesHint = new System.Windows.Forms.Label();
			this._buttonReadAboutStages = new System.Windows.Forms.Button();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._toolTip = new System.Windows.Forms.ToolTip(this.components);
			this._tableLayoutOuter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayoutOuter
			// 
			this._tableLayoutOuter.AutoSize = true;
			this._tableLayoutOuter.ColumnCount = 4;
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this._tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutOuter.Controls.Add(this._labelReadAboutStatus, 2, 4);
			this._tableLayoutOuter.Controls.Add(this._labelStatus, 0, 0);
			this._tableLayoutOuter.Controls.Add(this._buttonReadAboutStatus, 3, 4);
			this._tableLayoutOuter.Controls.Add(this._labelStatusHint, 2, 1);
			this._tableLayoutOuter.Controls.Add(this._labelStages, 0, 5);
			this._tableLayoutOuter.Controls.Add(this._labelReadAboutStages, 2, 11);
			this._tableLayoutOuter.Controls.Add(this._labelStagesHint, 2, 6);
			this._tableLayoutOuter.Controls.Add(this._buttonReadAboutStages, 3, 11);
			this._tableLayoutOuter.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayoutOuter.Location = new System.Drawing.Point(15, 15);
			this._tableLayoutOuter.Name = "_tableLayoutOuter";
			this._tableLayoutOuter.RowCount = 12;
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutOuter.Size = new System.Drawing.Size(431, 248);
			this._tableLayoutOuter.TabIndex = 0;
			this._tableLayoutOuter.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleTableLayoutOuterPaint);
			// 
			// _labelReadAboutStatus
			// 
			this._labelReadAboutStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelReadAboutStatus.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelReadAboutStatus, null);
			this.locExtender.SetLocalizationComment(this._labelReadAboutStatus, null);
			this.locExtender.SetLocalizingId(this._labelReadAboutStatus, "EventsView.StatusAndStagesEditor._labelReadAboutStatus");
			this._labelReadAboutStatus.Location = new System.Drawing.Point(257, 81);
			this._labelReadAboutStatus.Margin = new System.Windows.Forms.Padding(15, 4, 0, 0);
			this._labelReadAboutStatus.Name = "_labelReadAboutStatus";
			this._labelReadAboutStatus.Size = new System.Drawing.Size(145, 16);
			this._labelReadAboutStatus.TabIndex = 25;
			this._labelReadAboutStatus.Text = "Read About SayMore Status Indicators";
			// 
			// _labelStatus
			// 
			this._labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelStatus.AutoSize = true;
			this._tableLayoutOuter.SetColumnSpan(this._labelStatus, 4);
			this.locExtender.SetLocalizableToolTip(this._labelStatus, null);
			this.locExtender.SetLocalizationComment(this._labelStatus, null);
			this.locExtender.SetLocalizingId(this._labelStatus, "EventsView.StatusAndStagesEditor._labelStatus");
			this._labelStatus.Location = new System.Drawing.Point(0, 0);
			this._labelStatus.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
			this._labelStatus.Name = "_labelStatus";
			this._labelStatus.Size = new System.Drawing.Size(37, 13);
			this._labelStatus.TabIndex = 0;
			this._labelStatus.Text = "Status";
			// 
			// _buttonReadAboutStatus
			// 
			this._buttonReadAboutStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._buttonReadAboutStatus.AutoSize = true;
			this._buttonReadAboutStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonReadAboutStatus.BackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStatus.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonReadAboutStatus.FlatAppearance.BorderSize = 0;
			this._buttonReadAboutStatus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStatus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonReadAboutStatus.Image = global::SayMore.Properties.Resources.Help;
			this.locExtender.SetLocalizableToolTip(this._buttonReadAboutStatus, null);
			this.locExtender.SetLocalizationComment(this._buttonReadAboutStatus, null);
			this.locExtender.SetLocalizationPriority(this._buttonReadAboutStatus, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonReadAboutStatus, "StatusAndStagesEditor._buttonReadAboutStatus");
			this._buttonReadAboutStatus.Location = new System.Drawing.Point(405, 77);
			this._buttonReadAboutStatus.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this._buttonReadAboutStatus.Name = "_buttonReadAboutStatus";
			this._buttonReadAboutStatus.Size = new System.Drawing.Size(22, 20);
			this._buttonReadAboutStatus.TabIndex = 24;
			this._buttonReadAboutStatus.UseVisualStyleBackColor = false;
			// 
			// _labelStatusHint
			// 
			this._labelStatusHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelStatusHint.AutoSize = true;
			this._tableLayoutOuter.SetColumnSpan(this._labelStatusHint, 2);
			this._labelStatusHint.ForeColor = System.Drawing.Color.DimGray;
			this.locExtender.SetLocalizableToolTip(this._labelStatusHint, null);
			this.locExtender.SetLocalizationComment(this._labelStatusHint, null);
			this.locExtender.SetLocalizingId(this._labelStatusHint, "EventsView.StatusAndStagesEditor._labelStatusHint");
			this._labelStatusHint.Location = new System.Drawing.Point(245, 21);
			this._labelStatusHint.Margin = new System.Windows.Forms.Padding(15, 4, 0, 0);
			this._labelStatusHint.Name = "_labelStatusHint";
			this._tableLayoutOuter.SetRowSpan(this._labelStatusHint, 3);
			this._labelStatusHint.Size = new System.Drawing.Size(186, 26);
			this._labelStatusHint.TabIndex = 22;
			this._labelStatusHint.Text = "Use the status to mark the big picture of this event.";
			// 
			// _labelStages
			// 
			this._labelStages.AutoSize = true;
			this._tableLayoutOuter.SetColumnSpan(this._labelStages, 4);
			this.locExtender.SetLocalizableToolTip(this._labelStages, null);
			this.locExtender.SetLocalizationComment(this._labelStages, null);
			this.locExtender.SetLocalizingId(this._labelStages, "EventsView.StatusAndStagesEditor._labelStages");
			this._labelStages.Location = new System.Drawing.Point(0, 109);
			this._labelStages.Margin = new System.Windows.Forms.Padding(0, 12, 0, 4);
			this._labelStages.Name = "_labelStages";
			this._labelStages.Size = new System.Drawing.Size(40, 13);
			this._labelStages.TabIndex = 9;
			this._labelStages.Text = "Stages";
			// 
			// _labelReadAboutStages
			// 
			this._labelReadAboutStages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelReadAboutStages.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelReadAboutStages, null);
			this.locExtender.SetLocalizationComment(this._labelReadAboutStages, null);
			this.locExtender.SetLocalizingId(this._labelReadAboutStages, "EventsView.StatusAndStagesEditor._labelReadAboutStages");
			this._labelReadAboutStages.Location = new System.Drawing.Point(257, 230);
			this._labelReadAboutStages.Margin = new System.Windows.Forms.Padding(15, 4, 0, 0);
			this._labelReadAboutStages.Name = "_labelReadAboutStages";
			this._labelReadAboutStages.Size = new System.Drawing.Size(145, 13);
			this._labelReadAboutStages.TabIndex = 27;
			this._labelReadAboutStages.Text = "Read About SayMore Stages";
			// 
			// _labelStagesHint
			// 
			this._labelStagesHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelStagesHint.AutoSize = true;
			this._tableLayoutOuter.SetColumnSpan(this._labelStagesHint, 2);
			this._labelStagesHint.ForeColor = System.Drawing.Color.DimGray;
			this.locExtender.SetLocalizableToolTip(this._labelStagesHint, null);
			this.locExtender.SetLocalizationComment(this._labelStagesHint, null);
			this.locExtender.SetLocalizingId(this._labelStagesHint, "EventsView.StatusAndStagesEditor._labelStagesHint");
			this._labelStagesHint.Location = new System.Drawing.Point(253, 130);
			this._labelStagesHint.Margin = new System.Windows.Forms.Padding(15, 4, 0, 0);
			this._labelStagesHint.Name = "_labelStagesHint";
			this._tableLayoutOuter.SetRowSpan(this._labelStagesHint, 5);
			this._labelStagesHint.Size = new System.Drawing.Size(178, 65);
			this._labelStagesHint.TabIndex = 28;
			this._labelStagesHint.Text = "Stages are normally automatic indicators of what  has been done, based on file na" +
    "mes and annotation work you\'ve done. Click any item to take control of this indi" +
    "cator.";
			// 
			// _buttonReadAboutStages
			// 
			this._buttonReadAboutStages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonReadAboutStages.AutoSize = true;
			this._buttonReadAboutStages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonReadAboutStages.BackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStages.Cursor = System.Windows.Forms.Cursors.Hand;
			this._buttonReadAboutStages.FlatAppearance.BorderSize = 0;
			this._buttonReadAboutStages.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStages.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this._buttonReadAboutStages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this._buttonReadAboutStages.Image = global::SayMore.Properties.Resources.Help;
			this.locExtender.SetLocalizableToolTip(this._buttonReadAboutStages, null);
			this.locExtender.SetLocalizationComment(this._buttonReadAboutStages, null);
			this.locExtender.SetLocalizationPriority(this._buttonReadAboutStages, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._buttonReadAboutStages, "StatusAndStagesEditor._buttonReadAboutStages");
			this._buttonReadAboutStages.Location = new System.Drawing.Point(406, 226);
			this._buttonReadAboutStages.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this._buttonReadAboutStages.Name = "_buttonReadAboutStages";
			this._buttonReadAboutStages.Size = new System.Drawing.Size(22, 22);
			this._buttonReadAboutStages.TabIndex = 29;
			this._buttonReadAboutStages.UseVisualStyleBackColor = false;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// _toolTip
			// 
			this._toolTip.IsBalloon = true;
			this._toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			// 
			// StatusAndStagesEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayoutOuter);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "StatusAndStagesEditor.EditorBase");
			this.Name = "StatusAndStagesEditor";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.Size = new System.Drawing.Size(461, 304);
			this._tableLayoutOuter.ResumeLayout(false);
			this._tableLayoutOuter.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayoutOuter;
		private System.Windows.Forms.Label _labelStatus;
		private System.Windows.Forms.Label _labelStages;
		private System.Windows.Forms.Label _labelStatusHint;
		private System.Windows.Forms.Button _buttonReadAboutStatus;
		private System.Windows.Forms.Label _labelReadAboutStatus;
		private System.Windows.Forms.Label _labelReadAboutStages;
		private System.Windows.Forms.Label _labelStagesHint;
		private System.Windows.Forms.Button _buttonReadAboutStages;
		private Localization.UI.LocalizationExtender locExtender;
		private System.Windows.Forms.ToolTip _toolTip;
	}
}
