using SilUtils;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ElementListScreen
{
	partial class PersonListScreen
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
			this._elementListSplitter = new System.Windows.Forms.SplitContainer();
			this._peopleListPanel = new SayMore.UI.LowLevelControls.ListPanel();
			this._componentsSplitter = new System.Windows.Forms.SplitContainer();
			this._personComponentFileGrid = new SayMore.UI.ElementListScreen.ComponentFileGrid();
			this._labelHelp = new System.Windows.Forms.Label();
			this._elementListSplitter.Panel1.SuspendLayout();
			this._elementListSplitter.Panel2.SuspendLayout();
			this._elementListSplitter.SuspendLayout();
			this._componentsSplitter.Panel1.SuspendLayout();
			this._componentsSplitter.Panel2.SuspendLayout();
			this._componentsSplitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// _elementListSplitter
			// 
			this._elementListSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._elementListSplitter.Location = new System.Drawing.Point(0, 0);
			this._elementListSplitter.Name = "_elementListSplitter";
			// 
			// _elementListSplitter.Panel1
			// 
			this._elementListSplitter.Panel1.Controls.Add(this._peopleListPanel);
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
			// _peopleListPanel
			// 
			this._peopleListPanel.ButtonPanelBackColor1 = System.Drawing.SystemColors.Control;
			this._peopleListPanel.ButtonPanelBackColor2 = System.Drawing.SystemColors.Control;
			this._peopleListPanel.ButtonPanelTopBorderColor = System.Drawing.SystemColors.ControlDark;
			this._peopleListPanel.CurrentItem = null;
			this._peopleListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._peopleListPanel.Items = new object[0];
			// 
			// 
			// 
			this._peopleListPanel.ListView.BackColor = System.Drawing.SystemColors.Window;
			this._peopleListPanel.ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._peopleListPanel.ListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._peopleListPanel.ListView.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._peopleListPanel.ListView.FullRowSelect = true;
			this._peopleListPanel.ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this._peopleListPanel.ListView.HideSelection = false;
			this._peopleListPanel.ListView.Location = new System.Drawing.Point(0, 30);
			this._peopleListPanel.ListView.Name = "ListPanel._itemsListView";
			this._peopleListPanel.ListView.Size = new System.Drawing.Size(180, 282);
			this._peopleListPanel.ListView.TabIndex = 0;
			this._peopleListPanel.ListView.UseCompatibleStateImageBehavior = false;
			this._peopleListPanel.ListView.View = System.Windows.Forms.View.Details;
			this._peopleListPanel.Location = new System.Drawing.Point(0, 0);
			this._peopleListPanel.MinimumSize = new System.Drawing.Size(165, 0);
			this._peopleListPanel.Name = "_peopleListPanel";
			this._peopleListPanel.ReSortWhenItemTextChanges = false;
			this._peopleListPanel.Size = new System.Drawing.Size(182, 350);
			this._peopleListPanel.TabIndex = 8;
			this._peopleListPanel.Text = "People";
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
			this._componentsSplitter.Panel1.Controls.Add(this._personComponentFileGrid);
			// 
			// _componentsSplitter.Panel2
			// 
			this._componentsSplitter.Panel2.Controls.Add(this._labelHelp);
			this._componentsSplitter.Size = new System.Drawing.Size(315, 350);
			this._componentsSplitter.SplitterDistance = 147;
			this._componentsSplitter.SplitterWidth = 6;
			this._componentsSplitter.TabIndex = 0;
			this._componentsSplitter.TabStop = false;
			// 
			// _personComponentFileGrid
			// 
			this._personComponentFileGrid.AddButtonEnabled = false;
			this._personComponentFileGrid.AddButtonVisible = true;
			this._personComponentFileGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._personComponentFileGrid.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._personComponentFileGrid.Location = new System.Drawing.Point(0, 0);
			this._personComponentFileGrid.Name = "_personComponentFileGrid";
			this._personComponentFileGrid.ShowContextMenu = true;
			this._personComponentFileGrid.Size = new System.Drawing.Size(315, 147);
			this._personComponentFileGrid.TabIndex = 1;
			// 
			// _labelHelp
			// 
			this._labelHelp.Dock = System.Windows.Forms.DockStyle.Fill;
			this._labelHelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelHelp.Location = new System.Drawing.Point(0, 0);
			this._labelHelp.Name = "_labelHelp";
			this._labelHelp.Size = new System.Drawing.Size(315, 197);
			this._labelHelp.TabIndex = 1;
			this._labelHelp.Text = "Click \'New\' to add a new person.";
			this._labelHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PersonListScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._elementListSplitter);
			this.Name = "PersonListScreen";
			this.Size = new System.Drawing.Size(503, 350);
			this._elementListSplitter.Panel1.ResumeLayout(false);
			this._elementListSplitter.Panel2.ResumeLayout(false);
			this._elementListSplitter.ResumeLayout(false);
			this._componentsSplitter.Panel1.ResumeLayout(false);
			this._componentsSplitter.Panel2.ResumeLayout(false);
			this._componentsSplitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private ListPanel _peopleListPanel;
		private System.Windows.Forms.SplitContainer _elementListSplitter;
		private System.Windows.Forms.SplitContainer _componentsSplitter;
		private ComponentFileGrid _personComponentFileGrid;
		private System.Windows.Forms.Label _labelHelp;
	}
}
