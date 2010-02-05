using SilUtils.Controls;

namespace SIL.Sponge.Controls
{
	partial class ListPanel
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
			this.pnlList = new SilUtils.Controls.SilPanel();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnNew = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.lvItems = new System.Windows.Forms.ListView();
			this.hdrList = new System.Windows.Forms.ColumnHeader();
			this.hlblItems = new SilUtils.Controls.HeaderLabel();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.pnlList.SuspendLayout();
			this.pnlButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlList
			// 
			this.pnlList.BackColor = System.Drawing.SystemColors.Window;
			this.pnlList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(166)))), ((int)(((byte)(170)))));
			this.pnlList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlList.ClipTextForChildControls = true;
			this.pnlList.ControlReceivingFocusOnMnemonic = null;
			this.pnlList.Controls.Add(this.pnlButtons);
			this.pnlList.Controls.Add(this.lvItems);
			this.pnlList.Controls.Add(this.hlblItems);
			this.pnlList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlList.DoubleBuffered = true;
			this.pnlList.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.locExtender.SetLocalizableToolTip(this.pnlList, null);
			this.locExtender.SetLocalizationComment(this.pnlList, null);
			this.locExtender.SetLocalizingId(this.pnlList, "ListPanel.pnlList");
			this.pnlList.Location = new System.Drawing.Point(0, 0);
			this.pnlList.MnemonicGeneratesClick = false;
			this.pnlList.Name = "pnlList";
			this.pnlList.PaintExplorerBarBackground = false;
			this.pnlList.Size = new System.Drawing.Size(165, 277);
			this.pnlList.TabIndex = 1;
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnNew);
			this.pnlButtons.Controls.Add(this.btnDelete);
			this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlButtons.Location = new System.Drawing.Point(0, 241);
			this.pnlButtons.Name = "pnlButtons";
			this.pnlButtons.Size = new System.Drawing.Size(163, 34);
			this.pnlButtons.TabIndex = 2;
			this.pnlButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlButtons_Paint);
			// 
			// btnNew
			// 
			this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.locExtender.SetLocalizableToolTip(this.btnNew, null);
			this.locExtender.SetLocalizationComment(this.btnNew, null);
			this.locExtender.SetLocalizingId(this.btnNew, "ListPanel.btnNew");
			this.btnNew.Location = new System.Drawing.Point(4, 5);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(75, 24);
			this.btnNew.TabIndex = 0;
			this.btnNew.Text = "&New";
			this.btnNew.UseVisualStyleBackColor = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.locExtender.SetLocalizableToolTip(this.btnDelete, null);
			this.locExtender.SetLocalizationComment(this.btnDelete, null);
			this.locExtender.SetLocalizingId(this.btnDelete, "ListPanel.btnDelete");
			this.btnDelete.Location = new System.Drawing.Point(84, 5);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 24);
			this.btnDelete.TabIndex = 1;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// lvItems
			// 
			this.lvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrList});
			this.lvItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvItems.HideSelection = false;
			this.lvItems.Location = new System.Drawing.Point(2, 31);
			this.lvItems.Name = "lvItems";
			this.lvItems.Size = new System.Drawing.Size(159, 209);
			this.lvItems.TabIndex = 0;
			this.lvItems.UseCompatibleStateImageBehavior = false;
			this.lvItems.View = System.Windows.Forms.View.Details;
			this.lvItems.SelectedIndexChanged += new System.EventHandler(this.lvItems_SelectedIndexChanged);
			// 
			// hdrList
			// 
			this.locExtender.SetLocalizableToolTip(this.hdrList, null);
			this.locExtender.SetLocalizationComment(this.hdrList, null);
			this.locExtender.SetLocalizingId(this.hdrList, "ListPanel.lvItems");
			this.hdrList.Text = "Events";
			// 
			// hlblItems
			// 
			this.hlblItems.ClipTextForChildControls = true;
			this.hlblItems.ControlReceivingFocusOnMnemonic = null;
			this.hlblItems.Dock = System.Windows.Forms.DockStyle.Top;
			this.hlblItems.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.locExtender.SetLocalizableToolTip(this.hlblItems, null);
			this.locExtender.SetLocalizationComment(this.hlblItems, "Localized in controls that host this one.");
			this.locExtender.SetLocalizationPriority(this.hlblItems, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.hlblItems, "ListPanel.hlblItems");
			this.hlblItems.Location = new System.Drawing.Point(0, 0);
			this.hlblItems.MinimumSize = new System.Drawing.Size(165, 0);
			this.hlblItems.MnemonicGeneratesClick = false;
			this.hlblItems.Name = "hlblItems";
			this.hlblItems.ShowWindowBackgroudOnTopAndRightEdge = true;
			this.hlblItems.Size = new System.Drawing.Size(165, 27);
			this.hlblItems.TabIndex = 0;
			this.hlblItems.Text = "Items";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Misc. Controls";
			// 
			// ListPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlList);
			this.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ListPanel.ListPanel");
			this.MinimumSize = new System.Drawing.Size(165, 0);
			this.Name = "ListPanel";
			this.Size = new System.Drawing.Size(165, 277);
			this.pnlList.ResumeLayout(false);
			this.pnlButtons.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SilUtils.Controls.SilPanel pnlList;
		private System.Windows.Forms.ListView lvItems;
		private System.Windows.Forms.ColumnHeader hdrList;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnNew;
		private HeaderLabel hlblItems;
		private System.Windows.Forms.Panel pnlButtons;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
	}
}
