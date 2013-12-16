namespace SayMore.UI.Overview
{
	partial class ProjectAccessScreen
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
			this._layoutTable = new System.Windows.Forms.TableLayoutPanel();
			this._labelProjectAccess = new System.Windows.Forms.Label();
			this._projectAccess = new System.Windows.Forms.ComboBox();
			this._labelDescription = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._layoutTable.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _layoutTable
			// 
			this._layoutTable.AutoSize = true;
			this._layoutTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._layoutTable.ColumnCount = 2;
			this._layoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._layoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._layoutTable.Controls.Add(this._labelProjectAccess, 0, 0);
			this._layoutTable.Controls.Add(this._projectAccess, 1, 0);
			this._layoutTable.Controls.Add(this._labelDescription, 1, 2);
			this._layoutTable.Dock = System.Windows.Forms.DockStyle.Top;
			this._layoutTable.Location = new System.Drawing.Point(7, 7);
			this._layoutTable.Name = "_layoutTable";
			this._layoutTable.Padding = new System.Windows.Forms.Padding(3);
			this._layoutTable.RowCount = 3;
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._layoutTable.Size = new System.Drawing.Size(362, 68);
			this._layoutTable.TabIndex = 0;
			// 
			// _labelProjectAccess
			// 
			this._labelProjectAccess.AutoSize = true;
			this._labelProjectAccess.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelProjectAccess.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelProjectAccess, null);
			this.locExtender.SetLocalizationComment(this._labelProjectAccess, null);
			this.locExtender.SetLocalizingId(this._labelProjectAccess, "ProjectAccessScreen._labelProjectAccess");
			this._labelProjectAccess.Location = new System.Drawing.Point(6, 6);
			this._labelProjectAccess.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this._labelProjectAccess.Name = "_labelProjectAccess";
			this._labelProjectAccess.Size = new System.Drawing.Size(116, 26);
			this._labelProjectAccess.TabIndex = 0;
			this._labelProjectAccess.Text = "Access Protocol used\r\nby this project";
			this._labelProjectAccess.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _projectAccess
			// 
			this._projectAccess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._projectAccess.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._projectAccess, null);
			this.locExtender.SetLocalizationComment(this._projectAccess, null);
			this.locExtender.SetLocalizationPriority(this._projectAccess, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._projectAccess, "ProjectAccessScreen._projectAccess");
			this._projectAccess.Location = new System.Drawing.Point(128, 6);
			this._projectAccess.Name = "_projectAccess";
			this._projectAccess.Size = new System.Drawing.Size(121, 21);
			this._projectAccess.TabIndex = 2;
			this._projectAccess.SelectedIndexChanged += new System.EventHandler(this._projectAccess_SelectedIndexChanged);
			// 
			// _labelDescription
			// 
			this._labelDescription.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelDescription, null);
			this.locExtender.SetLocalizationComment(this._labelDescription, null);
			this.locExtender.SetLocalizationPriority(this._labelDescription, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelDescription, "ProjectAccessScreen._labelDescription");
			this._labelDescription.Location = new System.Drawing.Point(128, 52);
			this._labelDescription.Name = "_labelDescription";
			this._labelDescription.Size = new System.Drawing.Size(133, 13);
			this._labelDescription.TabIndex = 1;
			this._labelDescription.Text = "Description will go here.";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = null;
			// 
			// ProjectAccessScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this._layoutTable);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ProjectAccessScreen.ProjectAccessScreen");
			this.Name = "ProjectAccessScreen";
			this.Padding = new System.Windows.Forms.Padding(7);
			this.Size = new System.Drawing.Size(376, 150);
			this.Load += new System.EventHandler(this.ProjectAccessScreen_Load);
			this.Leave += new System.EventHandler(this.ProjectAccessScreen_Leave);
			this._layoutTable.ResumeLayout(false);
			this._layoutTable.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _layoutTable;
		private System.Windows.Forms.Label _labelProjectAccess;
		private System.Windows.Forms.Label _labelDescription;
		private System.Windows.Forms.ComboBox _projectAccess;
		private L10NSharp.UI.L10NSharpExtender locExtender;
	}
}
