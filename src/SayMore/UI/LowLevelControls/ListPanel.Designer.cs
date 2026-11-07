using L10NSharp;
using L10NSharp.Windows.Forms;
using L10NSharp.Windows.Forms.UIComponents;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListPanel));
            this.locExtender = new L10NSharp.Windows.Forms.L10NSharpExtender(this.components);
            this._outerPanel = new SIL.Windows.Forms.Widgets.EnhancedPanel();
            this._buttonsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this._buttonNew = new System.Windows.Forms.Button();
            this._headerLabel = new SIL.Windows.Forms.Widgets.HeaderLabel();
            this._headerControls = new System.Windows.Forms.TableLayoutPanel();
            this._searchPanel = new System.Windows.Forms.TableLayoutPanel();
            this._searchIcon = new System.Windows.Forms.PictureBox();
            this._searchTextBox = new System.Windows.Forms.TextBox();
            this._buttonColChooser = new SayMore.UI.LowLevelControls.ColumnChooserButton();
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
            this._outerPanel.SuspendLayout();
            this._buttonsFlowLayoutPanel.SuspendLayout();
            this._headerLabel.SuspendLayout();
            this._headerControls.SuspendLayout();
            this._searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._searchIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // locExtender
            // 
            this.locExtender.LocalizationManagerId = "SayMore";
            this.locExtender.PrefixForNewItems = null;
            // 
            // _outerPanel
            // 
            this._outerPanel.BackColor = System.Drawing.SystemColors.Window;
            this._outerPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this._outerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._outerPanel.ClipTextForChildControls = true;
            this._outerPanel.ControlReceivingFocusOnMnemonic = null;
            this._outerPanel.Controls.Add(this._buttonsFlowLayoutPanel);
            this._outerPanel.Controls.Add(this._headerLabel);
            this._outerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._outerPanel.DoubleBuffered = true;
            this._outerPanel.DrawOnlyBottomBorder = false;
            this._outerPanel.DrawOnlyTopBorder = false;
            this._outerPanel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this._outerPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this._outerPanel, null);
            this.locExtender.SetLocalizationComment(this._outerPanel, null);
            this.locExtender.SetLocalizationPriority(this._outerPanel, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._outerPanel, "UI.ListPanel._outerPanel");
            this._outerPanel.Location = new System.Drawing.Point(0, 0);
            this._outerPanel.MnemonicGeneratesClick = false;
            this._outerPanel.Name = "_outerPanel";
            this._outerPanel.PaintExplorerBarBackground = false;
            this._outerPanel.Size = new System.Drawing.Size(405, 317);
            this._outerPanel.TabIndex = 1;
            // 
            // _buttonsFlowLayoutPanel
            // 
            this._buttonsFlowLayoutPanel.AutoSize = true;
            this._buttonsFlowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._buttonsFlowLayoutPanel.Controls.Add(this._buttonNew);
            this._buttonsFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._buttonsFlowLayoutPanel.Location = new System.Drawing.Point(0, 279);
            this._buttonsFlowLayoutPanel.Name = "_buttonsFlowLayoutPanel";
            this._buttonsFlowLayoutPanel.Padding = new System.Windows.Forms.Padding(2);
            this._buttonsFlowLayoutPanel.Size = new System.Drawing.Size(403, 36);
            this._buttonsFlowLayoutPanel.TabIndex = 3;
            this._buttonsFlowLayoutPanel.TabStop = true;
            this._buttonsFlowLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleButtonPanelPaint);
            // 
            // _buttonNew
            // 
            this._buttonNew.AutoSize = true;
            this._buttonNew.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.locExtender.SetLocalizableToolTip(this._buttonNew, null);
            this.locExtender.SetLocalizationComment(this._buttonNew, "Button for adding new sessions and people");
            this.locExtender.SetLocalizingId(this._buttonNew, "CommonToMultipleViews.ElementList.NewButtonText");
            this._buttonNew.Location = new System.Drawing.Point(5, 5);
            this._buttonNew.MinimumSize = new System.Drawing.Size(75, 26);
            this._buttonNew.Name = "_buttonNew";
            this._buttonNew.Size = new System.Drawing.Size(75, 26);
            this._buttonNew.TabIndex = 0;
            this._buttonNew.Text = "&New";
            this._buttonNew.UseVisualStyleBackColor = true;
            this._buttonNew.Click += new System.EventHandler(this.HandleNewButtonClick);
            // 
            // _headerLabel
            // 
            this._headerLabel.BackColor = System.Drawing.SystemColors.Window;
            this._headerLabel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
            this._headerLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._headerLabel.ClipTextForChildControls = true;
            this._headerLabel.ControlReceivingFocusOnMnemonic = null;
            this._headerLabel.Controls.Add(this._headerControls);
            this._headerLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this._headerLabel.DoubleBuffered = true;
            this._headerLabel.DrawOnlyBottomBorder = false;
            this._headerLabel.DrawOnlyTopBorder = false;
            this._headerLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this._headerLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.locExtender.SetLocalizableToolTip(this._headerLabel, null);
            this.locExtender.SetLocalizationComment(this._headerLabel, "Do not translate - Localized in subclass");
            this.locExtender.SetLocalizationPriority(this._headerLabel, L10NSharp.LocalizationPriority.NotLocalizable);
            this.locExtender.SetLocalizingId(this._headerLabel, "UI.ListPanel._headerLabel");
            this._headerLabel.Location = new System.Drawing.Point(0, 0);
            this._headerLabel.MinimumSize = new System.Drawing.Size(165, 2);
            this._headerLabel.MnemonicGeneratesClick = false;
            this._headerLabel.Name = "_headerLabel";
            this._headerLabel.PaintExplorerBarBackground = false;
            this._headerLabel.ShowWindowBackgroudOnTopAndRightEdge = true;
            this._headerLabel.ShowWindowBackgroundOnTopAndRightEdge = true;
            this._headerLabel.Size = new System.Drawing.Size(403, 23);
            this._headerLabel.TabIndex = 0;
            this._headerLabel.Text = "Change this text";
            this._headerLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleHeaderPanelPaint);
            // 
            // _headerControls
            // 
            this._headerControls.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._headerControls.AutoSize = true;
            this._headerControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._headerControls.BackColor = System.Drawing.Color.Transparent;
            this._headerControls.ColumnCount = 2;
            this._headerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._headerControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._headerControls.Controls.Add(this._searchPanel, 0, 0);
            this._headerControls.Controls.Add(this._buttonColChooser, 1, 0);
            this._headerControls.ForeColor = System.Drawing.Color.Transparent;
            this._headerControls.Location = new System.Drawing.Point(168, 0);
            this._headerControls.Margin = new System.Windows.Forms.Padding(0);
            this._headerControls.Name = "_headerControls";
            this._headerControls.RowCount = 1;
            this._headerControls.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._headerControls.Size = new System.Drawing.Size(225, 20);
            this._headerControls.TabIndex = 1;
            // 
            // _searchPanel
            // 
            this._searchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._searchPanel.AutoSize = true;
            this._searchPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._searchPanel.BackColor = System.Drawing.Color.Transparent;
            this._searchPanel.ColumnCount = 2;
            this._searchPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._searchPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._searchPanel.Controls.Add(this._searchIcon, 0, 0);
            this._searchPanel.Controls.Add(this._searchTextBox, 1, 0);
            this._searchPanel.Location = new System.Drawing.Point(0, 1);
            this._searchPanel.Margin = new System.Windows.Forms.Padding(0);
            this._searchPanel.Name = "_searchPanel";
            this._searchPanel.RowCount = 1;
            this._searchPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._searchPanel.Size = new System.Drawing.Size(198, 18);
            this._searchPanel.TabIndex = 4;
            this._searchPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleSearchContainerPaint);
            // 
            // _searchIcon
            // 
            this._searchIcon.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._searchIcon.BackColor = System.Drawing.SystemColors.Window;
            this._searchIcon.Image = global::SayMore.Properties.Resources.Filter;
            this.locExtender.SetLocalizableToolTip(this._searchIcon, "Filter sessions");
            this.locExtender.SetLocalizationComment(this._searchIcon, null);
            this.locExtender.SetLocalizingId(this._searchIcon, "ListPanel._searchIcon");
            this._searchIcon.Location = new System.Drawing.Point(0, 0);
            this._searchIcon.Margin = new System.Windows.Forms.Padding(0);
            this._searchIcon.Name = "_searchIcon";
            this._searchIcon.Size = new System.Drawing.Size(20, 18);
            this._searchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this._searchIcon.TabIndex = 0;
            this._searchIcon.TabStop = false;
            // 
            // _searchTextBox
            // 
            this._searchTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._searchTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._searchTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.locExtender.SetLocalizableToolTip(this._searchTextBox, null);
            this.locExtender.SetLocalizationComment(this._searchTextBox, null);
            this.locExtender.SetLocalizingId(this._searchTextBox, "ListPanel._searchTextBox");
            this._searchTextBox.Location = new System.Drawing.Point(20, 0);
            this._searchTextBox.Margin = new System.Windows.Forms.Padding(0);
            this._searchTextBox.Name = "_searchTextBox";
            this._searchTextBox.Size = new System.Drawing.Size(178, 18);
            this._searchTextBox.TabIndex = 1;
            // 
            // _buttonColChooser
            // 
            this._buttonColChooser.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._buttonColChooser.BackColor = System.Drawing.Color.Transparent;
            this._buttonColChooser.CanBeChecked = false;
            this._buttonColChooser.Checked = false;
            this._buttonColChooser.DrawEmpty = false;
            this._buttonColChooser.DrawLeftArrowButton = false;
            this._buttonColChooser.DrawRightArrowButton = false;
            this._buttonColChooser.Font = new System.Drawing.Font("Marlett", 9F);
            this._buttonColChooser.Image = ((System.Drawing.Image)(resources.GetObject("_buttonColChooser.Image")));
            this.locExtender.SetLocalizableToolTip(this._buttonColChooser, "Choose Columns");
            this.locExtender.SetLocalizationComment(this._buttonColChooser, null);
            this.locExtender.SetLocalizingId(this._buttonColChooser, "CommonToMultipleViews.ElementList.ColumnChooserButton");
            this._buttonColChooser.Location = new System.Drawing.Point(204, 0);
            this._buttonColChooser.Margin = new System.Windows.Forms.Padding(6, 0, 1, 0);
            this._buttonColChooser.Name = "_buttonColChooser";
            this._buttonColChooser.Size = new System.Drawing.Size(20, 20);
            this._buttonColChooser.TabIndex = 0;
            // 
            // ListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._outerPanel);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(125, 0);
            this.Name = "ListPanel";
            this.Size = new System.Drawing.Size(405, 317);
            ((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
            this._outerPanel.ResumeLayout(false);
            this._outerPanel.PerformLayout();
            this._buttonsFlowLayoutPanel.ResumeLayout(false);
            this._buttonsFlowLayoutPanel.PerformLayout();
            this._headerLabel.ResumeLayout(false);
            this._headerLabel.PerformLayout();
            this._headerControls.ResumeLayout(false);
            this._headerControls.PerformLayout();
            this._searchPanel.ResumeLayout(false);
            this._searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._searchIcon)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private SIL.Windows.Forms.Widgets.EnhancedPanel _outerPanel;
		private SIL.Windows.Forms.Widgets.HeaderLabel _headerLabel;
		private L10NSharpExtender locExtender;
		public System.Windows.Forms.Button _buttonNew;
		private System.Windows.Forms.FlowLayoutPanel _buttonsFlowLayoutPanel;
		private ColumnChooserButton _buttonColChooser;
		private System.Windows.Forms.TextBox _searchTextBox;
		private System.Windows.Forms.PictureBox _searchIcon;
		private System.Windows.Forms.TableLayoutPanel _headerControls;
		private System.Windows.Forms.TableLayoutPanel _searchPanel;
	}
}
