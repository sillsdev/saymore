namespace SayMore.Utilities.ComponentEditors
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
			this._presetMenuButton = new System.Windows.Forms.Button();
			this._presetMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.ColumnCount = 1;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._presetMenuButton, 0, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Location = new System.Drawing.Point(7, 7);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 2;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Size = new System.Drawing.Size(435, 194);
			this._tableLayout.TabIndex = 0;
			// 
			// _presetMenuButton
			// 
			this._presetMenuButton.AutoSize = true;
			this._presetMenuButton.Image = global::SayMore.Properties.Resources.DropDownArrow;
			this._presetMenuButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.locExtender.SetLocalizableToolTip(this._presetMenuButton, null);
			this.locExtender.SetLocalizationComment(this._presetMenuButton, null);
			this.locExtender.SetLocalizingId(this._presetMenuButton, "CommonToMultipleViews.MediaPropertiesEditor.PresetsButton");
			this._presetMenuButton.Location = new System.Drawing.Point(0, 0);
			this._presetMenuButton.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
			this._presetMenuButton.MinimumSize = new System.Drawing.Size(73, 26);
			this._presetMenuButton.Name = "_presetMenuButton";
			this._presetMenuButton.Size = new System.Drawing.Size(73, 26);
			this._presetMenuButton.TabIndex = 16;
			this._presetMenuButton.Text = "Presets";
			this._presetMenuButton.UseVisualStyleBackColor = true;
			this._presetMenuButton.Click += new System.EventHandler(this.HandlePresetMenuButtonClick);
			// 
			// _presetMenu
			// 
			this.locExtender.SetLocalizableToolTip(this._presetMenu, null);
			this.locExtender.SetLocalizationComment(this._presetMenu, null);
			this.locExtender.SetLocalizationPriority(this._presetMenu, Localization.LocalizationPriority.NotLocalizable);
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
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "MediaComponentEditor.EditorBase");
			this.Name = "MediaComponentEditor";
			this.Size = new System.Drawing.Size(449, 208);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private Localization.UI.LocalizationExtender locExtender;
		protected System.Windows.Forms.Button _presetMenuButton;
		protected System.Windows.Forms.ContextMenuStrip _presetMenu;
	}
}
