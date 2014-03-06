using System;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ElementListScreen
{
	partial class SessionsListScreen
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
			this._elementListSplitter = new System.Windows.Forms.SplitContainer();
			this._buttonNewFromRecording = new System.Windows.Forms.Button();
			this._buttonNewFromFiles = new System.Windows.Forms.Button();
			this._sessionsListPanel = new SayMore.UI.LowLevelControls.ListPanel();
			this._componentsSplitter = new System.Windows.Forms.SplitContainer();
			this._sessionComponentFileGrid = new SayMore.UI.ElementListScreen.ComponentFileGrid();
			this._labelClickNewHelpPrompt = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			((System.ComponentModel.ISupportInitialize)(this._elementListSplitter)).BeginInit();
			this._elementListSplitter.Panel1.SuspendLayout();
			this._elementListSplitter.Panel2.SuspendLayout();
			this._elementListSplitter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._componentsSplitter)).BeginInit();
			this._componentsSplitter.Panel1.SuspendLayout();
			this._componentsSplitter.Panel2.SuspendLayout();
			this._componentsSplitter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
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
			this._elementListSplitter.Panel1.Controls.Add(this._buttonNewFromRecording);
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
			// _buttonNewFromRecording
			// 
			this._buttonNewFromRecording.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonNewFromRecording.AutoSize = true;
			this._buttonNewFromRecording.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.locExtender.SetLocalizableToolTip(this._buttonNewFromRecording, null);
			this.locExtender.SetLocalizationComment(this._buttonNewFromRecording, null);
			this.locExtender.SetLocalizingId(this._buttonNewFromRecording, "SessionsView.SessionsList.NewFilesFromRecordingButtonText");
			this._buttonNewFromRecording.Location = new System.Drawing.Point(24, 216);
			this._buttonNewFromRecording.MinimumSize = new System.Drawing.Size(117, 26);
			this._buttonNewFromRecording.Name = "_buttonNewFromRecording";
			this._buttonNewFromRecording.Size = new System.Drawing.Size(126, 26);
			this._buttonNewFromRecording.TabIndex = 2;
			this._buttonNewFromRecording.Text = "New From &Recording...";
			this._buttonNewFromRecording.UseVisualStyleBackColor = true;
			this._buttonNewFromRecording.Click += new System.EventHandler(this.HandleButtonNewFromRecordingsClick);
			// 
			// _buttonNewFromFiles
			// 
			this._buttonNewFromFiles.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._buttonNewFromFiles.AutoSize = true;
			this._buttonNewFromFiles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.locExtender.SetLocalizableToolTip(this._buttonNewFromFiles, null);
			this.locExtender.SetLocalizationComment(this._buttonNewFromFiles, null);
			this.locExtender.SetLocalizingId(this._buttonNewFromFiles, "SessionsView.SessionsList.NewFilesFromDeviceButtonText");
			this._buttonNewFromFiles.Location = new System.Drawing.Point(33, 161);
			this._buttonNewFromFiles.MinimumSize = new System.Drawing.Size(117, 26);
			this._buttonNewFromFiles.Name = "_buttonNewFromFiles";
			this._buttonNewFromFiles.Size = new System.Drawing.Size(117, 26);
			this._buttonNewFromFiles.TabIndex = 1;
			this._buttonNewFromFiles.Text = "New From De&vice...";
			this._buttonNewFromFiles.UseVisualStyleBackColor = true;
			this._buttonNewFromFiles.Click += new System.EventHandler(this.HandleButtonNewFromFilesClick);
			// 
			// _sessionsListPanel
			// 
			this._sessionsListPanel.ButtonPanelBackColor1 = System.Drawing.SystemColors.Control;
			this._sessionsListPanel.ButtonPanelBackColor2 = System.Drawing.SystemColors.Control;
			this._sessionsListPanel.ButtonPanelTopBorderColor = System.Drawing.SystemColors.ControlDark;
			this._sessionsListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sessionsListPanel.HeaderPanelBackColor1 = System.Drawing.SystemColors.Control;
			this._sessionsListPanel.HeaderPanelBackColor2 = System.Drawing.SystemColors.Control;
			this._sessionsListPanel.HeaderPanelBottomBorderColor = System.Drawing.SystemColors.ControlDark;
			this.locExtender.SetLocalizableToolTip(this._sessionsListPanel, null);
			this.locExtender.SetLocalizationComment(this._sessionsListPanel, null);
			this.locExtender.SetLocalizationPriority(this._sessionsListPanel, L10NSharp.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._sessionsListPanel, "SessionsView.SessionsList.HeadingText");
			this._sessionsListPanel.Location = new System.Drawing.Point(0, 0);
			this._sessionsListPanel.MinimumSize = new System.Drawing.Size(165, 0);
			this._sessionsListPanel.Name = "_sessionsListPanel";
			this._sessionsListPanel.ShowColumnChooserButton = true;
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
			this._componentsSplitter.Panel2.Controls.Add(this._labelClickNewHelpPrompt);
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
			this._sessionComponentFileGrid.ConvertButtonVisible = true;
			this._sessionComponentFileGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._sessionComponentFileGrid.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
			this.locExtender.SetLocalizableToolTip(this._sessionComponentFileGrid, null);
			this.locExtender.SetLocalizationComment(this._sessionComponentFileGrid, null);
			this.locExtender.SetLocalizationPriority(this._sessionComponentFileGrid, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._sessionComponentFileGrid, "UI.SessionsView.ComponentFileGrid");
			this._sessionComponentFileGrid.Location = new System.Drawing.Point(0, 0);
			this._sessionComponentFileGrid.Name = "_sessionComponentFileGrid";
			this._sessionComponentFileGrid.RenameButtonVisible = true;
			this._sessionComponentFileGrid.ShowContextMenu = true;
			this._sessionComponentFileGrid.Size = new System.Drawing.Size(315, 147);
			this._sessionComponentFileGrid.TabIndex = 0;
			// 
			// _labelClickNewHelpPrompt
			// 
			this._labelClickNewHelpPrompt.Dock = System.Windows.Forms.DockStyle.Fill;
			this._labelClickNewHelpPrompt.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelClickNewHelpPrompt, null);
			this.locExtender.SetLocalizationComment(this._labelClickNewHelpPrompt, null);
			this.locExtender.SetLocalizingId(this._labelClickNewHelpPrompt, "SessionsView.SessionsList.ClickNewButtonHelpPrompt");
			this._labelClickNewHelpPrompt.Location = new System.Drawing.Point(0, 0);
			this._labelClickNewHelpPrompt.Name = "_labelClickNewHelpPrompt";
			this._labelClickNewHelpPrompt.Size = new System.Drawing.Size(315, 197);
			this._labelClickNewHelpPrompt.TabIndex = 0;
			this._labelClickNewHelpPrompt.Text = "Click \'New\' to add a new session.";
			this._labelClickNewHelpPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = null;
			// 
			// SessionsListScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._elementListSplitter);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "UI.SessionsView.SessionsListScreen");
			this.Name = "SessionsListScreen";
			this.Size = new System.Drawing.Size(503, 350);
			this._elementListSplitter.Panel1.ResumeLayout(false);
			this._elementListSplitter.Panel1.PerformLayout();
			this._elementListSplitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._elementListSplitter)).EndInit();
			this._elementListSplitter.ResumeLayout(false);
			this._componentsSplitter.Panel1.ResumeLayout(false);
			this._componentsSplitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._componentsSplitter)).EndInit();
			this._componentsSplitter.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private ListPanel _sessionsListPanel;
		private System.Windows.Forms.SplitContainer _elementListSplitter;
		private System.Windows.Forms.SplitContainer _componentsSplitter;
		private ComponentFileGrid _sessionComponentFileGrid;
		private System.Windows.Forms.Button _buttonNewFromFiles;
		private System.Windows.Forms.Label _labelClickNewHelpPrompt;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.Button _buttonNewFromRecording;

	    public string NameForUsageReporting
	    {
            get { return "Sessions"; }
	    }
	}
}
