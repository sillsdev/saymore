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
			this._elementListSplitter = new System.Windows.Forms.SplitContainer();
			this._buttonNewFromFiles = new System.Windows.Forms.Button();
			this._sessionsListPanel = new SayMore.UI.LowLevelControls.ListPanel();
			this._componentsSplitter = new System.Windows.Forms.SplitContainer();
			this._sessionComponentFileGrid = new SayMore.UI.ElementListScreen.ComponentFileGrid();
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
			this._elementListSplitter.Panel1.Controls.Add(this._buttonNewFromFiles);
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
			// _buttonNewFromFiles
			// 
			this._buttonNewFromFiles.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonNewFromFiles.AutoSize = true;
			this._buttonNewFromFiles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._buttonNewFromFiles.Location = new System.Drawing.Point(33, 161);
			this._buttonNewFromFiles.MinimumSize = new System.Drawing.Size(117, 26);
			this._buttonNewFromFiles.Name = "_buttonNewFromFiles";
			this._buttonNewFromFiles.Size = new System.Drawing.Size(117, 26);
			this._buttonNewFromFiles.TabIndex = 1;
			this._buttonNewFromFiles.Text = "New From Device...";
			this._buttonNewFromFiles.UseVisualStyleBackColor = true;
			this._buttonNewFromFiles.Click += new System.EventHandler(this.HandleButtonNewFromFilesClick);
			// 
			// _sessionsListPanel
			// 
			this._sessionsListPanel.ButtonPanelBackColor1 = System.Drawing.SystemColors.Control;
			this._sessionsListPanel.ButtonPanelBackColor2 = System.Drawing.SystemColors.Control;
			this._sessionsListPanel.ButtonPanelTopBorderColor = System.Drawing.SystemColors.ControlDark;
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
			this._sessionsListPanel.ListView.Name = "_itemsListView";
			this._sessionsListPanel.ListView.Size = new System.Drawing.Size(180, 282);
			this._sessionsListPanel.ListView.TabIndex = 0;
			this._sessionsListPanel.ListView.UseCompatibleStateImageBehavior = false;
			this._sessionsListPanel.ListView.View = System.Windows.Forms.View.Details;
			this._sessionsListPanel.Location = new System.Drawing.Point(0, 0);
			this._sessionsListPanel.MinimumSize = new System.Drawing.Size(165, 0);
			this._sessionsListPanel.Name = "_sessionsListPanel";
			this._sessionsListPanel.ReSortWhenItemTextChanges = false;
			this._sessionsListPanel.Size = new System.Drawing.Size(182, 350);
			this._sessionsListPanel.TabIndex = 0;
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
			this._componentsSplitter.Panel1.Controls.Add(this._sessionComponentFileGrid);
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
			// _sessionComponentFileGrid
			// 
			this._sessionComponentFileGrid.AddButtonEnabled = false;
			this._sessionComponentFileGrid.AddButtonVisible = true;
			this._sessionComponentFileGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sessionComponentFileGrid.Location = new System.Drawing.Point(0, 0);
			this._sessionComponentFileGrid.Name = "_sessionComponentFileGrid";
			this._sessionComponentFileGrid.ShowContextMenu = true;
			this._sessionComponentFileGrid.Size = new System.Drawing.Size(315, 147);
			this._sessionComponentFileGrid.TabIndex = 0;
			// 
			// _labelHelp
			// 
			this._labelHelp.Dock = System.Windows.Forms.DockStyle.Fill;
			this._labelHelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelHelp.Location = new System.Drawing.Point(0, 0);
			this._labelHelp.Name = "_labelHelp";
			this._labelHelp.Size = new System.Drawing.Size(315, 197);
			this._labelHelp.TabIndex = 0;
			this._labelHelp.Text = "Click \'New\' to add a new session.";
			this._labelHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SessionScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._elementListSplitter);
			this.Name = "SessionScreen";
			this.Size = new System.Drawing.Size(503, 350);
			this._elementListSplitter.Panel1.ResumeLayout(false);
			this._elementListSplitter.Panel1.PerformLayout();
			this._elementListSplitter.Panel2.ResumeLayout(false);
			this._elementListSplitter.ResumeLayout(false);
			this._componentsSplitter.Panel1.ResumeLayout(false);
			this._componentsSplitter.Panel2.ResumeLayout(false);
			this._componentsSplitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private ListPanel _sessionsListPanel;
		private System.Windows.Forms.SplitContainer _elementListSplitter;
		private System.Windows.Forms.SplitContainer _componentsSplitter;
		private ComponentFileGrid _sessionComponentFileGrid;
		private System.Windows.Forms.Button _buttonNewFromFiles;
		private System.Windows.Forms.Label _labelHelp;
	}
}
