using System.Windows.Forms;
using SilTools;

namespace SayMore.UI.NewSessionsFromFiles
{
	partial class NewSessionsFromFilesDlg
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._linkFindFiles = new System.Windows.Forms.LinkLabel();
			this._buttonCreateSessions = new System.Windows.Forms.Button();
			this._buttonCancel = new System.Windows.Forms.Button();
			this._labelSourceFolder = new System.Windows.Forms.Label();
			this._labelIncomingFiles = new System.Windows.Forms.Label();
			this._labelInstructions = new System.Windows.Forms.Label();
			this._mediaPlayerPanel = new SilTools.Controls.SilPanel();
			this._panelProgress = new System.Windows.Forms.Panel();
			this._labelProgress = new System.Windows.Forms.Label();
			this._progressBar = new System.Windows.Forms.ProgressBar();
			this._outerTableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutButtons = new System.Windows.Forms.TableLayoutPanel();
			this._gridFiles = new SayMore.UI.ElementListScreen.ComponentFileGrid();
			this._panelMetadata = new System.Windows.Forms.Panel();
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._mediaPlayerPanel.SuspendLayout();
			this._panelProgress.SuspendLayout();
			this._outerTableLayout.SuspendLayout();
			this._tableLayoutButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			//
			// _linkFindFiles
			//
			this._linkFindFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._linkFindFiles.AutoSize = true;
			this._linkFindFiles.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._linkFindFiles, null);
			this.locExtender.SetLocalizationComment(this._linkFindFiles, null);
			this.locExtender.SetLocalizingId(this._linkFindFiles, "DialogBoxes.NewSessionsFromFilesDlg._linkFindFiles");
			this._linkFindFiles.Location = new System.Drawing.Point(3, 10);
			this._linkFindFiles.Margin = new System.Windows.Forms.Padding(3, 10, 7, 20);
			this._linkFindFiles.Name = "_linkFindFiles";
			this._linkFindFiles.Size = new System.Drawing.Size(149, 13);
			this._linkFindFiles.TabIndex = 0;
			this._linkFindFiles.TabStop = true;
			this._linkFindFiles.Text = "Find Files on Recorder/Card...";
			this._linkFindFiles.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleFindFilesLinkClicked);
			//
			// _buttonCreateSessions
			//
			this._buttonCreateSessions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCreateSessions.AutoSize = true;
			this._buttonCreateSessions.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.locExtender.SetLocalizableToolTip(this._buttonCreateSessions, null);
			this.locExtender.SetLocalizationComment(this._buttonCreateSessions, "Parameter is for the number of sessions that will be created.");
			this.locExtender.SetLocalizingId(this._buttonCreateSessions, "DialogBoxes.NewSessionsFromFilesDlg._buttonCreateSessions");
			this._buttonCreateSessions.Location = new System.Drawing.Point(0, 0);
			this._buttonCreateSessions.Margin = new System.Windows.Forms.Padding(0);
			this._buttonCreateSessions.MinimumSize = new System.Drawing.Size(130, 26);
			this._buttonCreateSessions.Name = "_buttonCreateSessions";
			this._buttonCreateSessions.Size = new System.Drawing.Size(142, 26);
			this._buttonCreateSessions.TabIndex = 3;
			this._buttonCreateSessions.Text = "Create {0} Sessions";
			this._buttonCreateSessions.UseVisualStyleBackColor = true;
			this._buttonCreateSessions.Click += new System.EventHandler(this.HandleCreateSessionsButtonClick);
			//
			// _buttonCancel
			//
			this._buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._buttonCancel.AutoSize = true;
			this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.locExtender.SetLocalizableToolTip(this._buttonCancel, null);
			this.locExtender.SetLocalizationComment(this._buttonCancel, null);
			this.locExtender.SetLocalizingId(this._buttonCancel, "DialogBoxes.NewSessionsFromFilesDlg._buttonCancel");
			this._buttonCancel.Location = new System.Drawing.Point(150, 0);
			this._buttonCancel.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
			this._buttonCancel.Name = "_buttonCancel";
			this._buttonCancel.Size = new System.Drawing.Size(80, 26);
			this._buttonCancel.TabIndex = 4;
			this._buttonCancel.Text = "Cancel";
			this._buttonCancel.UseVisualStyleBackColor = true;
			//
			// _labelSourceFolder
			//
			this._labelSourceFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._labelSourceFolder.AutoSize = true;
			this._labelSourceFolder.BackColor = System.Drawing.Color.Transparent;
			this._outerTableLayout.SetColumnSpan(this._labelSourceFolder, 2);
			this.locExtender.SetLocalizableToolTip(this._labelSourceFolder, null);
			this.locExtender.SetLocalizationComment(this._labelSourceFolder, null);
			this.locExtender.SetLocalizationPriority(this._labelSourceFolder, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelSourceFolder, "DialogBoxes.NewSessionsFromFilesDlg._labelSourceFolder");
			this._labelSourceFolder.Location = new System.Drawing.Point(162, 10);
			this._labelSourceFolder.Margin = new System.Windows.Forms.Padding(3, 10, 3, 20);
			this._labelSourceFolder.Name = "_labelSourceFolder";
			this._labelSourceFolder.Size = new System.Drawing.Size(524, 13);
			this._labelSourceFolder.TabIndex = 1;
			this._labelSourceFolder.Text = "#";
			//
			// _labelIncomingFiles
			//
			this._labelIncomingFiles.AutoSize = true;
			this._labelIncomingFiles.BackColor = System.Drawing.Color.Transparent;
			this._outerTableLayout.SetColumnSpan(this._labelIncomingFiles, 2);
			this.locExtender.SetLocalizableToolTip(this._labelIncomingFiles, null);
			this.locExtender.SetLocalizationComment(this._labelIncomingFiles, null);
			this.locExtender.SetLocalizingId(this._labelIncomingFiles, "DialogBoxes.NewSessionsFromFilesDlg._labelIncomingFiles");
			this._labelIncomingFiles.Location = new System.Drawing.Point(3, 43);
			this._labelIncomingFiles.Margin = new System.Windows.Forms.Padding(3, 0, 3, 5);
			this._labelIncomingFiles.Name = "_labelIncomingFiles";
			this._labelIncomingFiles.Size = new System.Drawing.Size(74, 13);
			this._labelIncomingFiles.TabIndex = 0;
			this._labelIncomingFiles.Text = "Incoming &Files";
			//
			// _labelInstructions
			//
			this._labelInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._labelInstructions.AutoEllipsis = true;
			this._labelInstructions.AutoSize = true;
			this._labelInstructions.BackColor = System.Drawing.Color.Transparent;
			this._outerTableLayout.SetColumnSpan(this._labelInstructions, 2);
			this.locExtender.SetLocalizableToolTip(this._labelInstructions, null);
			this.locExtender.SetLocalizationComment(this._labelInstructions, null);
			this.locExtender.SetLocalizingId(this._labelInstructions, "DialogBoxes.NewSessionsFromFilesDlg._labelInstructions");
			this._labelInstructions.Location = new System.Drawing.Point(3, 266);
			this._labelInstructions.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this._labelInstructions.Name = "_labelInstructions";
			this._labelInstructions.Size = new System.Drawing.Size(419, 26);
			this._labelInstructions.TabIndex = 2;
			this._labelInstructions.Text = "Mark each file which represents a source recording of a session. For each one, " +
	"SayMore will create a new session and copy the file into it.";
			//
			// _mediaPlayerPanel
			//
			this._mediaPlayerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._mediaPlayerPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this._mediaPlayerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._mediaPlayerPanel.ClipTextForChildControls = true;
			this._mediaPlayerPanel.ControlReceivingFocusOnMnemonic = null;
			this._mediaPlayerPanel.Controls.Add(this._panelProgress);
			this._mediaPlayerPanel.DoubleBuffered = true;
			this._mediaPlayerPanel.DrawOnlyBottomBorder = false;
			this._mediaPlayerPanel.DrawOnlyTopBorder = false;
			this._mediaPlayerPanel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this._mediaPlayerPanel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.locExtender.SetLocalizableToolTip(this._mediaPlayerPanel, null);
			this.locExtender.SetLocalizationComment(this._mediaPlayerPanel, null);
			this.locExtender.SetLocalizingId(this._mediaPlayerPanel, "DialogBoxes.NewSessionsFromFilesDlg._mediaPlayerPanel");
			this._mediaPlayerPanel.Location = new System.Drawing.Point(445, 61);
			this._mediaPlayerPanel.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
			this._mediaPlayerPanel.MnemonicGeneratesClick = false;
			this._mediaPlayerPanel.Name = "_mediaPlayerPanel";
			this._mediaPlayerPanel.PaintExplorerBarBackground = false;
			this._mediaPlayerPanel.Size = new System.Drawing.Size(244, 198);
			this._mediaPlayerPanel.TabIndex = 6;
			//
			// _panelProgress
			//
			this._panelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._panelProgress.BackColor = System.Drawing.Color.Transparent;
			this._panelProgress.Controls.Add(this._labelProgress);
			this._panelProgress.Controls.Add(this._progressBar);
			this._panelProgress.Location = new System.Drawing.Point(14, 102);
			this._panelProgress.Name = "_panelProgress";
			this._panelProgress.Size = new System.Drawing.Size(214, 41);
			this._panelProgress.TabIndex = 7;
			//
			// _labelProgress
			//
			this._labelProgress.AutoEllipsis = true;
			this._labelProgress.BackColor = System.Drawing.Color.Transparent;
			this._labelProgress.Dock = System.Windows.Forms.DockStyle.Top;
			this._labelProgress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelProgress, null);
			this.locExtender.SetLocalizationComment(this._labelProgress, null);
			this.locExtender.SetLocalizingId(this._labelProgress, "DialogBoxes.NewSessionsFromFilesDlg._labelProgress");
			this._labelProgress.Location = new System.Drawing.Point(0, 0);
			this._labelProgress.Name = "_labelProgress";
			this._labelProgress.Size = new System.Drawing.Size(214, 18);
			this._labelProgress.TabIndex = 0;
			this._labelProgress.Text = "Reading Files...";
			this._labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// _progressBar
			//
			this._progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._progressBar.Location = new System.Drawing.Point(0, 21);
			this._progressBar.Name = "_progressBar";
			this._progressBar.Size = new System.Drawing.Size(214, 20);
			this._progressBar.Step = 1;
			this._progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this._progressBar.TabIndex = 1;
			//
			// _outerTableLayout
			//
			this._outerTableLayout.ColumnCount = 3;
			this._outerTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._outerTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._outerTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._outerTableLayout.Controls.Add(this._tableLayoutButtons, 2, 4);
			this._outerTableLayout.Controls.Add(this._gridFiles, 0, 2);
			this._outerTableLayout.Controls.Add(this._labelInstructions, 0, 3);
			this._outerTableLayout.Controls.Add(this._mediaPlayerPanel, 2, 2);
			this._outerTableLayout.Controls.Add(this._labelIncomingFiles, 0, 1);
			this._outerTableLayout.Controls.Add(this._panelMetadata, 0, 4);
			this._outerTableLayout.Controls.Add(this._linkFindFiles, 0, 0);
			this._outerTableLayout.Controls.Add(this._labelSourceFolder, 1, 0);
			this._outerTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._outerTableLayout.Location = new System.Drawing.Point(15, 15);
			this._outerTableLayout.Name = "_outerTableLayout";
			this._outerTableLayout.RowCount = 5;
			this._outerTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._outerTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._outerTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
			this._outerTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._outerTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this._outerTableLayout.Size = new System.Drawing.Size(689, 382);
			this._outerTableLayout.TabIndex = 8;
			this._outerTableLayout.SizeChanged += new System.EventHandler(this.HandleOuterTableLayoutSizeChanged);
			//
			// _tableLayoutButtons
			//
			this._tableLayoutButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutButtons.AutoSize = true;
			this._tableLayoutButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutButtons.ColumnCount = 2;
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutButtons.Controls.Add(this._buttonCreateSessions, 0, 0);
			this._tableLayoutButtons.Controls.Add(this._buttonCancel, 1, 0);
			this._tableLayoutButtons.Location = new System.Drawing.Point(459, 356);
			this._tableLayoutButtons.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
			this._tableLayoutButtons.Name = "_tableLayoutButtons";
			this._tableLayoutButtons.RowCount = 1;
			this._tableLayoutButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutButtons.Size = new System.Drawing.Size(230, 26);
			this._tableLayoutButtons.TabIndex = 9;
			//
			// _gridFiles
			//
			this._gridFiles.AddButtonEnabled = false;
			this._gridFiles.AddButtonVisible = false;
			this._gridFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this._outerTableLayout.SetColumnSpan(this._gridFiles, 2);
			this._gridFiles.ConvertButtonVisible = false;
			this._gridFiles.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
			this.locExtender.SetLocalizableToolTip(this._gridFiles, null);
			this.locExtender.SetLocalizationComment(this._gridFiles, null);
			this.locExtender.SetLocalizationPriority(this._gridFiles, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._gridFiles, "DialogBoxes.NewSessionsFromFilesDlg.ComponentFileGrid");
			this._gridFiles.Location = new System.Drawing.Point(0, 61);
			this._gridFiles.Margin = new System.Windows.Forms.Padding(0);
			this._gridFiles.Name = "_gridFiles";
			this._gridFiles.RenameButtonVisible = false;
			this._gridFiles.ShowContextMenu = false;
			this._gridFiles.Size = new System.Drawing.Size(425, 200);
			this._gridFiles.TabIndex = 0;
			//
			// _panelMetadata
			//
			this._outerTableLayout.SetColumnSpan(this._panelMetadata, 2);
			this._panelMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
			this._panelMetadata.Location = new System.Drawing.Point(3, 298);
			this._panelMetadata.Name = "_panelMetadata";
			this._panelMetadata.Size = new System.Drawing.Size(419, 81);
			this._panelMetadata.TabIndex = 3;
			//
			// locExtender
			//
			this.locExtender.LocalizationManagerId = "SayMore";
			//
			// NewSessionsFromFilesDlg
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(719, 412);
			this.Controls.Add(this._outerTableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "DialogBoxes.NewSessionsFromFilesDlg.WindowTitle");
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(650, 380);
			this.Name = "NewSessionsFromFilesDlg";
			this.Padding = new System.Windows.Forms.Padding(15);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "New Sessions From Device";
			this._mediaPlayerPanel.ResumeLayout(false);
			this._panelProgress.ResumeLayout(false);
			this._outerTableLayout.ResumeLayout(false);
			this._outerTableLayout.PerformLayout();
			this._tableLayoutButtons.ResumeLayout(false);
			this._tableLayoutButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.LinkLabel _linkFindFiles;
		private System.Windows.Forms.Button _buttonCreateSessions;
		private System.Windows.Forms.Button _buttonCancel;
		private System.Windows.Forms.Label _labelSourceFolder;
		private System.Windows.Forms.Label _labelIncomingFiles;
		private System.Windows.Forms.Label _labelInstructions;
		private SilTools.Controls.SilPanel _mediaPlayerPanel;
		private System.Windows.Forms.Panel _panelProgress;
		private System.Windows.Forms.TableLayoutPanel _outerTableLayout;
		private System.Windows.Forms.Label _labelProgress;
		private System.Windows.Forms.ProgressBar _progressBar;
		private System.Windows.Forms.Panel _panelMetadata;
		private SayMore.UI.ElementListScreen.ComponentFileGrid _gridFiles;
		private System.Windows.Forms.TableLayoutPanel _tableLayoutButtons;
		private Localization.UI.LocalizationExtender locExtender;
	}
}