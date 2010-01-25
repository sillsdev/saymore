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
			this.pnlList = new SilUtils.Controls.SilPanel();
			this.lvItems = new System.Windows.Forms.ListView();
			this.hdrList = new System.Windows.Forms.ColumnHeader();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnNew = new System.Windows.Forms.Button();
			this.hlblItems = new SilUtils.Controls.HeaderLabel();
			this.pnlList.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlList
			// 
			this.pnlList.BackColor = System.Drawing.SystemColors.Window;
			this.pnlList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(166)))), ((int)(((byte)(170)))));
			this.pnlList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlList.ClipTextForChildControls = true;
			this.pnlList.ControlReceivingFocusOnMnemonic = null;
			this.pnlList.Controls.Add(this.lvItems);
			this.pnlList.Controls.Add(this.btnDelete);
			this.pnlList.Controls.Add(this.btnNew);
			this.pnlList.Controls.Add(this.hlblItems);
			this.pnlList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlList.DoubleBuffered = true;
			this.pnlList.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.pnlList.Location = new System.Drawing.Point(0, 0);
			this.pnlList.MnemonicGeneratesClick = false;
			this.pnlList.Name = "pnlList";
			this.pnlList.PaintExplorerBarBackground = false;
			this.pnlList.Size = new System.Drawing.Size(165, 277);
			this.pnlList.TabIndex = 1;
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
			this.lvItems.LabelEdit = true;
			this.lvItems.Location = new System.Drawing.Point(2, 31);
			this.lvItems.Name = "lvItems";
			this.lvItems.Size = new System.Drawing.Size(159, 213);
			this.lvItems.TabIndex = 0;
			this.lvItems.UseCompatibleStateImageBehavior = false;
			this.lvItems.View = System.Windows.Forms.View.Details;
			// 
			// hdrList
			// 
			this.hdrList.Text = "Events";
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Location = new System.Drawing.Point(84, 248);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 24);
			this.btnDelete.TabIndex = 1;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnNew
			// 
			this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnNew.Location = new System.Drawing.Point(3, 248);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(75, 24);
			this.btnNew.TabIndex = 0;
			this.btnNew.Text = "&New";
			this.btnNew.UseVisualStyleBackColor = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// hlblItems
			// 
			this.hlblItems.ClipTextForChildControls = true;
			this.hlblItems.ControlReceivingFocusOnMnemonic = null;
			this.hlblItems.Dock = System.Windows.Forms.DockStyle.Top;
			this.hlblItems.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.hlblItems.Location = new System.Drawing.Point(0, 0);
			this.hlblItems.MinimumSize = new System.Drawing.Size(165, 0);
			this.hlblItems.MnemonicGeneratesClick = false;
			this.hlblItems.Name = "hlblItems";
			this.hlblItems.ShowWindowBackgroudOnTopAndRightEdge = true;
			this.hlblItems.Size = new System.Drawing.Size(165, 27);
			this.hlblItems.TabIndex = 0;
			this.hlblItems.Text = "Items";
			// 
			// ListPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlList);
			this.MinimumSize = new System.Drawing.Size(165, 0);
			this.Name = "ListPanel";
			this.Size = new System.Drawing.Size(165, 277);
			this.pnlList.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private SilUtils.Controls.SilPanel pnlList;
		private System.Windows.Forms.ListView lvItems;
		private System.Windows.Forms.ColumnHeader hdrList;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnNew;
		private HeaderLabel hlblItems;
	}
}
