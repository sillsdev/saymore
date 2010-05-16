using SIL.Localization;
using SilUtils.Controls;

namespace SayMore.UI.LowLevelControls
{
	partial class ListPanel
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
			this._outerPanel = new SilUtils.Controls.SilPanel();
			this._buttonsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this._newButton = new System.Windows.Forms.Button();
			this._deleteButton = new System.Windows.Forms.Button();
			this._itemsListView = new System.Windows.Forms.ListView();
			this.hdrList = new System.Windows.Forms.ColumnHeader();
			this._headerLabel = new SilUtils.Controls.HeaderLabel();
			this.locExtender = new SIL.Localization.LocalizationExtender(this.components);
			this._outerPanel.SuspendLayout();
			this._buttonsFlowLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _outerPanel
			// 
			this._outerPanel.BackColor = System.Drawing.SystemColors.Window;
			this._outerPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(166)))), ((int)(((byte)(170)))));
			this._outerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._outerPanel.ClipTextForChildControls = true;
			this._outerPanel.ControlReceivingFocusOnMnemonic = null;
			this._outerPanel.Controls.Add(this._itemsListView);
			this._outerPanel.Controls.Add(this._buttonsFlowLayoutPanel);
			this._outerPanel.Controls.Add(this._headerLabel);
			this._outerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._outerPanel.DoubleBuffered = true;
			this._outerPanel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.locExtender.SetLocalizableToolTip(this._outerPanel, null);
			this.locExtender.SetLocalizationComment(this._outerPanel, null);
			this.locExtender.SetLocalizationPriority(this._outerPanel, SIL.Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._outerPanel, "ListPanel._outerPanel");
			this._outerPanel.Location = new System.Drawing.Point(0, 0);
			this._outerPanel.MnemonicGeneratesClick = false;
			this._outerPanel.Name = "_outerPanel";
			this._outerPanel.PaintExplorerBarBackground = false;
			this._outerPanel.Size = new System.Drawing.Size(170, 277);
			this._outerPanel.TabIndex = 1;
			// 
			// _buttonsFlowLayoutPanel
			// 
			this._buttonsFlowLayoutPanel.AutoSize = true;
			this._buttonsFlowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonsFlowLayoutPanel.Controls.Add(this._newButton);
			this._buttonsFlowLayoutPanel.Controls.Add(this._deleteButton);
			this._buttonsFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._buttonsFlowLayoutPanel.Location = new System.Drawing.Point(0, 241);
			this._buttonsFlowLayoutPanel.Name = "_buttonsFlowLayoutPanel";
			this._buttonsFlowLayoutPanel.Padding = new System.Windows.Forms.Padding(2);
			this._buttonsFlowLayoutPanel.Size = new System.Drawing.Size(168, 34);
			this._buttonsFlowLayoutPanel.TabIndex = 3;
			this._buttonsFlowLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleButtonPanelPaint);
			// 
			// _newButton
			// 
			this._newButton.AutoSize = true;
			this._newButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.locExtender.SetLocalizableToolTip(this._newButton, null);
			this.locExtender.SetLocalizationComment(this._newButton, null);
			this.locExtender.SetLocalizingId(this._newButton, "ListPanel._newButton");
			this._newButton.Location = new System.Drawing.Point(5, 5);
			this._newButton.MinimumSize = new System.Drawing.Size(75, 24);
			this._newButton.Name = "_newButton";
			this._newButton.Size = new System.Drawing.Size(75, 24);
			this._newButton.TabIndex = 0;
			this._newButton.Text = "&New";
			this._newButton.UseVisualStyleBackColor = true;
			this._newButton.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// _deleteButton
			// 
			this._deleteButton.AutoSize = true;
			this._deleteButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.locExtender.SetLocalizableToolTip(this._deleteButton, null);
			this.locExtender.SetLocalizationComment(this._deleteButton, null);
			this.locExtender.SetLocalizingId(this._deleteButton, "ListPanel._deleteButton");
			this._deleteButton.Location = new System.Drawing.Point(86, 5);
			this._deleteButton.MinimumSize = new System.Drawing.Size(75, 24);
			this._deleteButton.Name = "_deleteButton";
			this._deleteButton.Size = new System.Drawing.Size(75, 24);
			this._deleteButton.TabIndex = 1;
			this._deleteButton.Text = "&Delete";
			this._deleteButton.UseVisualStyleBackColor = true;
			this._deleteButton.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// _itemsListView
			// 
			this._itemsListView.BackColor = System.Drawing.SystemColors.Window;
			this._itemsListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._itemsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrList});
			this._itemsListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._itemsListView.FullRowSelect = true;
			this._itemsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this._itemsListView.HideSelection = false;
			this._itemsListView.Location = new System.Drawing.Point(0, 30);
			this._itemsListView.Name = "_itemsListView";
			this._itemsListView.Size = new System.Drawing.Size(168, 211);
			this._itemsListView.TabIndex = 0;
			this._itemsListView.UseCompatibleStateImageBehavior = false;
			this._itemsListView.View = System.Windows.Forms.View.Details;
			this._itemsListView.FontChanged += new System.EventHandler(this.lvItems_FontChanged);
			// 
			// hdrList
			// 
			this.locExtender.SetLocalizableToolTip(this.hdrList, null);
			this.locExtender.SetLocalizationComment(this.hdrList, null);
			this.locExtender.SetLocalizingId(this.hdrList, "ListPanel.lvItems");
			this.hdrList.Text = "Events";
			// 
			// _headerLabel
			// 
			this._headerLabel.ClipTextForChildControls = true;
			this._headerLabel.ControlReceivingFocusOnMnemonic = null;
			this._headerLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this._headerLabel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.locExtender.SetLocalizableToolTip(this._headerLabel, null);
			this.locExtender.SetLocalizationComment(this._headerLabel, "Localized in controls that host this one.");
			this.locExtender.SetLocalizationPriority(this._headerLabel, SIL.Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._headerLabel, "ListPanel._headerLabel");
			this._headerLabel.Location = new System.Drawing.Point(0, 0);
			this._headerLabel.MinimumSize = new System.Drawing.Size(165, 0);
			this._headerLabel.MnemonicGeneratesClick = false;
			this._headerLabel.Name = "_headerLabel";
			this._headerLabel.ShowWindowBackgroudOnTopAndRightEdge = true;
			this._headerLabel.Size = new System.Drawing.Size(168, 30);
			this._headerLabel.TabIndex = 0;
			this._headerLabel.Text = "Items";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Misc. Controls";
			// 
			// ListPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._outerPanel);
			this.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ListPanel.ListPanel");
			this.MinimumSize = new System.Drawing.Size(125, 0);
			this.Name = "ListPanel";
			this.Size = new System.Drawing.Size(170, 277);
			this._outerPanel.ResumeLayout(false);
			this._outerPanel.PerformLayout();
			this._buttonsFlowLayoutPanel.ResumeLayout(false);
			this._buttonsFlowLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SilUtils.Controls.SilPanel _outerPanel;
		private System.Windows.Forms.ListView _itemsListView;
		private System.Windows.Forms.ColumnHeader hdrList;
		private HeaderLabel _headerLabel;
		private LocalizationExtender locExtender;
		public System.Windows.Forms.Button _deleteButton;
		public System.Windows.Forms.Button _newButton;
		private System.Windows.Forms.FlowLayoutPanel _buttonsFlowLayoutPanel;
	}
}
