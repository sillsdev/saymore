using SilUtils;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ElementListScreen
{
	partial class SessionScreen
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
			this._tabComponentEditors = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this._elementListSplitter = new System.Windows.Forms.SplitContainer();
			this._sessionsListPanel = new SayMore.UI.LowLevelControls.ListPanel();
			this._componentsSplitter = new System.Windows.Forms.SplitContainer();
			this._componentFileGrid = new SayMore.UI.ElementListScreen.ComponentFileGrid();
			this._tabComponentEditors.SuspendLayout();
			this._elementListSplitter.Panel1.SuspendLayout();
			this._elementListSplitter.Panel2.SuspendLayout();
			this._elementListSplitter.SuspendLayout();
			this._componentsSplitter.Panel1.SuspendLayout();
			this._componentsSplitter.Panel2.SuspendLayout();
			this._componentsSplitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// _tabComponentEditors
			// 
			this._tabComponentEditors.Controls.Add(this.tabPage1);
			this._tabComponentEditors.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tabComponentEditors.Location = new System.Drawing.Point(0, 0);
			this._tabComponentEditors.Name = "_tabComponentEditors";
			this._tabComponentEditors.SelectedIndex = 0;
			this._tabComponentEditors.Size = new System.Drawing.Size(315, 197);
			this._tabComponentEditors.TabIndex = 7;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(307, 171);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// _elementListSplitter
			// 
			this._elementListSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._elementListSplitter.Location = new System.Drawing.Point(0, 0);
			this._elementListSplitter.Name = "_elementListSplitter";
			// 
			// _elementListSplitter.Panel1
			// 
			this._elementListSplitter.Panel1.Controls.Add(this._sessionsListPanel);
			// 
			// _elementListSplitter.Panel2
			// 
			this._elementListSplitter.Panel2.Controls.Add(this._componentsSplitter);
			this._elementListSplitter.Size = new System.Drawing.Size(503, 350);
			this._elementListSplitter.SplitterDistance = 182;
			this._elementListSplitter.SplitterWidth = 6;
			this._elementListSplitter.TabIndex = 9;
			this._elementListSplitter.TabStop = false;
			// 
			// _sessionsListPanel
			// 
			this._sessionsListPanel.CurrentItem = null;
			this._sessionsListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sessionsListPanel.Items = new object[0];
			// 
			// 
			// 
			this._sessionsListPanel.ListView.BackColor = System.Drawing.SystemColors.Window;
			this._sessionsListPanel.ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._sessionsListPanel.ListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sessionsListPanel.ListView.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._sessionsListPanel.ListView.FullRowSelect = true;
			this._sessionsListPanel.ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this._sessionsListPanel.ListView.HideSelection = false;
			this._sessionsListPanel.ListView.Location = new System.Drawing.Point(0, 30);
			this._sessionsListPanel.ListView.Name = "lvItems";
			this._sessionsListPanel.ListView.Size = new System.Drawing.Size(180, 284);
			this._sessionsListPanel.ListView.TabIndex = 0;
			this._sessionsListPanel.ListView.UseCompatibleStateImageBehavior = false;
			this._sessionsListPanel.ListView.View = System.Windows.Forms.View.Details;
			this._sessionsListPanel.Location = new System.Drawing.Point(0, 0);
			this._sessionsListPanel.MinimumSize = new System.Drawing.Size(165, 0);
			this._sessionsListPanel.Name = "_sessionsListPanel";
			this._sessionsListPanel.ReSortWhenItemTextChanges = false;
			this._sessionsListPanel.Size = new System.Drawing.Size(182, 350);
			this._sessionsListPanel.TabIndex = 8;
			this._sessionsListPanel.Text = "Sessions";
			// 
			// _componentsSplitter
			// 
			this._componentsSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._componentsSplitter.Location = new System.Drawing.Point(0, 0);
			this._componentsSplitter.Name = "_componentsSplitter";
			this._componentsSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// _componentsSplitter.Panel1
			// 
			this._componentsSplitter.Panel1.Controls.Add(this._componentFileGrid);
			// 
			// _componentsSplitter.Panel2
			// 
			this._componentsSplitter.Panel2.Controls.Add(this._tabComponentEditors);
			this._componentsSplitter.Size = new System.Drawing.Size(315, 350);
			this._componentsSplitter.SplitterDistance = 147;
			this._componentsSplitter.SplitterWidth = 6;
			this._componentsSplitter.TabIndex = 0;
			this._componentsSplitter.TabStop = false;
			// 
			// _componentFileGrid
			// 
			this._componentFileGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._componentFileGrid.Location = new System.Drawing.Point(0, 0);
			this._componentFileGrid.Name = "_componentFileGrid";
			this._componentFileGrid.Size = new System.Drawing.Size(315, 147);
			this._componentFileGrid.TabIndex = 0;
			// 
			// SessionScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._elementListSplitter);
			this.Name = "SessionScreen";
			this.Size = new System.Drawing.Size(503, 350);
			this._tabComponentEditors.ResumeLayout(false);
			this._elementListSplitter.Panel1.ResumeLayout(false);
			this._elementListSplitter.Panel2.ResumeLayout(false);
			this._elementListSplitter.ResumeLayout(false);
			this._componentsSplitter.Panel1.ResumeLayout(false);
			this._componentsSplitter.Panel2.ResumeLayout(false);
			this._componentsSplitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl _tabComponentEditors;
		private System.Windows.Forms.TabPage tabPage1;
		private ListPanel _sessionsListPanel;
		private System.Windows.Forms.SplitContainer _elementListSplitter;
		private System.Windows.Forms.SplitContainer _componentsSplitter;
		private ComponentFileGrid _componentFileGrid;
	}
}
