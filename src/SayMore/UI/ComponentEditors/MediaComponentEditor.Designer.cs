namespace SayMore.UI.ComponentEditors
{
	partial class MediaComponentEditor
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
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this._buttonPresets = new System.Windows.Forms.ToolStripDropDownButton();
			this._buttonMoreInfo = new System.Windows.Forms.ToolStripButton();
			this._presetMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.locExtender = new L10NSharp.UI.LocalizationExtender(this.components);
			this._tableLayout.SuspendLayout();
			this._toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.ColumnCount = 1;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._toolStrip, 0, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(7, 7);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 2;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayout.Size = new System.Drawing.Size(435, 194);
			this._tableLayout.TabIndex = 0;
			// 
			// _toolStrip
			// 
			this._toolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._toolStrip.BackColor = System.Drawing.Color.Transparent;
			this._toolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.locExtender.SetLocalizableToolTip(this._toolStrip, null);
			this.locExtender.SetLocalizationComment(this._toolStrip, null);
			this.locExtender.SetLocalizationPriority(this._toolStrip, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._toolStrip, "Transcription.UI.TextAnnotationEditor._toolStrip");
			this._toolStrip.Location = new System.Drawing.Point(0, 0);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(435, 25);
			this._toolStrip.TabIndex = 17;
			_toolStrip.Items.Add(_buttonPresets);
			_toolStrip.Items.Add(_buttonMoreInfo);
			// 
			// _buttonPresets
			// 
			this._buttonPresets.Image = global::SayMore.Properties.Resources.Presets;
			this.locExtender.SetLocalizableToolTip(this._buttonPresets, "Select presets for media file");
			this.locExtender.SetLocalizationComment(this._buttonPresets, null);
			this.locExtender.SetLocalizingId(this._buttonPresets, "CommonToMultipleViews.MediaPropertiesEditor.PresetsButton");
			this._buttonPresets.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
			this._buttonPresets.Name = "_buttonPresets";
			this._buttonPresets.Size = new System.Drawing.Size(73, 22);
			this._buttonPresets.Text = "Presets";
			this._buttonPresets.DropDownClosed += new System.EventHandler(this.HandlePresetsDropDownClosed);
			this._buttonPresets.DropDownOpening += new System.EventHandler(this.HandlePresetsDropDownOpening);
			// 
			// _buttonMoreInfo
			// 
			this._buttonMoreInfo.Image = global::SayMore.Properties.Resources.InfoBlue16x16;
			this.locExtender.SetLocalizableToolTip(this._buttonMoreInfo, "More information about this file");
			this.locExtender.SetLocalizationComment(this._buttonMoreInfo, null);
			this.locExtender.SetLocalizingId(this._buttonMoreInfo, "CommonToMultipleViews.MediaPropertiesEditor.MoreInfoButton");
			this._buttonMoreInfo.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
			this._buttonMoreInfo.Name = "_buttonMoreInfo";
			this._buttonMoreInfo.Size = new System.Drawing.Size(130, 22);
			this._buttonMoreInfo.Text = "More Information...";
			this._buttonMoreInfo.Click += new System.EventHandler(this.HandleMoreInfoButtonClick);
			// 
			// _presetMenu
			// 
			this.locExtender.SetLocalizableToolTip(this._presetMenu, null);
			this.locExtender.SetLocalizationComment(this._presetMenu, null);
			this.locExtender.SetLocalizationPriority(this._presetMenu, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._presetMenu, "_presetMenu._presetMenu");
			this._presetMenu.Name = "_presetMenu";
			this._presetMenu.Size = new System.Drawing.Size(61, 4);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// MediaComponentEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "MediaComponentEditor.EditorBase");
			this.Name = "MediaComponentEditor";
			this.Size = new System.Drawing.Size(449, 208);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private L10NSharp.UI.LocalizationExtender locExtender;
		protected System.Windows.Forms.ContextMenuStrip _presetMenu;
		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ToolStripButton _buttonMoreInfo;
		private System.Windows.Forms.ToolStripDropDownButton _buttonPresets;
	}
}
