using SilUtils;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ElementListScreen
{
	partial class EventsListScreen
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
			this._eventsListPanel = new SayMore.UI.LowLevelControls.ListPanel();
			this._componentsSplitter = new System.Windows.Forms.SplitContainer();
			this._eventComponentFileGrid = new SayMore.UI.ElementListScreen.ComponentFileGrid();
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
			this._elementListSplitter.Panel1.Controls.Add(this._eventsListPanel);
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
			// _eventsListPanel
			// 
			this._eventsListPanel.ButtonPanelBackColor1 = System.Drawing.SystemColors.Control;
			this._eventsListPanel.ButtonPanelBackColor2 = System.Drawing.SystemColors.Control;
			this._eventsListPanel.ButtonPanelTopBorderColor = System.Drawing.SystemColors.ControlDark;
			this._eventsListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			// 
			// 
			// 
			this._eventsListPanel.Location = new System.Drawing.Point(0, 0);
			this._eventsListPanel.MinimumSize = new System.Drawing.Size(165, 0);
			this._eventsListPanel.Name = "_eventsListPanel";
			this._eventsListPanel.Size = new System.Drawing.Size(182, 350);
			this._eventsListPanel.TabIndex = 0;
			this._eventsListPanel.Text = "Events";
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
			this._componentsSplitter.Panel1.Controls.Add(this._eventComponentFileGrid);
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
			// _eventComponentFileGrid
			// 
			this._eventComponentFileGrid.AddButtonEnabled = false;
			this._eventComponentFileGrid.AddButtonVisible = true;
			this._eventComponentFileGrid.ConvertButtonVisible = true;
			this._eventComponentFileGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._eventComponentFileGrid.Location = new System.Drawing.Point(0, 0);
			this._eventComponentFileGrid.Name = "_eventComponentFileGrid";
			this._eventComponentFileGrid.RenameButtonVisible = true;
			this._eventComponentFileGrid.ShowContextMenu = true;
			this._eventComponentFileGrid.Size = new System.Drawing.Size(315, 147);
			this._eventComponentFileGrid.TabIndex = 0;
			// 
			// _labelHelp
			// 
			this._labelHelp.Dock = System.Windows.Forms.DockStyle.Fill;
			this._labelHelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._labelHelp.Location = new System.Drawing.Point(0, 0);
			this._labelHelp.Name = "_labelHelp";
			this._labelHelp.Size = new System.Drawing.Size(315, 197);
			this._labelHelp.TabIndex = 0;
			this._labelHelp.Text = "Click \'New\' to add a new event.";
			this._labelHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// EventScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._elementListSplitter);
			this.Name = "EventScreen";
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

		private ListPanel _eventsListPanel;
		private System.Windows.Forms.SplitContainer _elementListSplitter;
		private System.Windows.Forms.SplitContainer _componentsSplitter;
		private ComponentFileGrid _eventComponentFileGrid;
		private System.Windows.Forms.Button _buttonNewFromFiles;
		private System.Windows.Forms.Label _labelHelp;
	}
}
