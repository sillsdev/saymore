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
			this._labelAccessProtocol = new System.Windows.Forms.Label();
			this._projectAccess = new System.Windows.Forms.ComboBox();
			this._labelDescription = new System.Windows.Forms.Label();
			this._labelCustomAccess = new System.Windows.Forms.Label();
			this._labelCustomInstructions = new System.Windows.Forms.Label();
			this._customAccessChoices = new System.Windows.Forms.TextBox();
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
			this._layoutTable.Controls.Add(this._labelAccessProtocol, 0, 0);
			this._layoutTable.Controls.Add(this._projectAccess, 1, 0);
			this._layoutTable.Controls.Add(this._labelDescription, 1, 5);
			this._layoutTable.Controls.Add(this._labelCustomAccess, 1, 2);
			this._layoutTable.Controls.Add(this._labelCustomInstructions, 1, 3);
			this._layoutTable.Controls.Add(this._customAccessChoices, 1, 4);
			this._layoutTable.Dock = System.Windows.Forms.DockStyle.Top;
			this._layoutTable.Location = new System.Drawing.Point(7, 7);
			this._layoutTable.Name = "_layoutTable";
			this._layoutTable.Padding = new System.Windows.Forms.Padding(3);
			this._layoutTable.RowCount = 6;
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._layoutTable.Size = new System.Drawing.Size(493, 122);
			this._layoutTable.TabIndex = 0;
			// 
			// _labelAccessProtocol
			// 
			this._labelAccessProtocol.AutoSize = true;
			this._labelAccessProtocol.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelAccessProtocol.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelAccessProtocol, null);
			this.locExtender.SetLocalizationComment(this._labelAccessProtocol, null);
			this.locExtender.SetLocalizingId(this._labelAccessProtocol, "ProjectAccessScreen._labelAccessProtocol");
			this._labelAccessProtocol.Location = new System.Drawing.Point(6, 6);
			this._labelAccessProtocol.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this._labelAccessProtocol.Name = "_labelAccessProtocol";
			this._labelAccessProtocol.Size = new System.Drawing.Size(116, 26);
			this._labelAccessProtocol.TabIndex = 0;
			this._labelAccessProtocol.Text = "Access Protocol used\r\nby this project";
			this._labelAccessProtocol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			this._labelDescription.Dock = System.Windows.Forms.DockStyle.Left;
			this.locExtender.SetLocalizableToolTip(this._labelDescription, null);
			this.locExtender.SetLocalizationComment(this._labelDescription, null);
			this.locExtender.SetLocalizationPriority(this._labelDescription, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelDescription, "ProjectAccessScreen._labelDescription");
			this._labelDescription.Location = new System.Drawing.Point(128, 106);
			this._labelDescription.Name = "_labelDescription";
			this._labelDescription.Size = new System.Drawing.Size(133, 13);
			this._labelDescription.TabIndex = 1;
			this._labelDescription.Text = "Description will go here.";
			this._labelDescription.Visible = false;
			// 
			// _labelCustomAccess
			// 
			this._labelCustomAccess.AutoSize = true;
			this._labelCustomAccess.Dock = System.Windows.Forms.DockStyle.Left;
			this._labelCustomAccess.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelCustomAccess, null);
			this.locExtender.SetLocalizationComment(this._labelCustomAccess, null);
			this.locExtender.SetLocalizationPriority(this._labelCustomAccess, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelCustomAccess, "ProjectAccessScreen._labelDescription");
			this._labelCustomAccess.Location = new System.Drawing.Point(128, 52);
			this._labelCustomAccess.Name = "_labelCustomAccess";
			this._labelCustomAccess.Size = new System.Drawing.Size(127, 13);
			this._labelCustomAccess.TabIndex = 3;
			this._labelCustomAccess.Text = "Custom Access Choices";
			this._labelCustomAccess.Visible = false;
			// 
			// _labelCustomInstructions
			// 
			this._labelCustomInstructions.AutoSize = true;
			this._labelCustomInstructions.Dock = System.Windows.Forms.DockStyle.Left;
			this.locExtender.SetLocalizableToolTip(this._labelCustomInstructions, null);
			this.locExtender.SetLocalizationComment(this._labelCustomInstructions, null);
			this.locExtender.SetLocalizationPriority(this._labelCustomInstructions, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelCustomInstructions, "ProjectAccessScreen._labelCustomAccessInstructions");
			this._labelCustomInstructions.Location = new System.Drawing.Point(128, 65);
			this._labelCustomInstructions.Name = "_labelCustomInstructions";
			this._labelCustomInstructions.Size = new System.Drawing.Size(213, 13);
			this._labelCustomInstructions.TabIndex = 4;
			this._labelCustomInstructions.Text = "Enter each choice, separated by commas";
			this._labelCustomInstructions.Visible = false;
			// 
			// _customAccessChoices
			// 
			this._customAccessChoices.Dock = System.Windows.Forms.DockStyle.Top;
			this.locExtender.SetLocalizableToolTip(this._customAccessChoices, null);
			this.locExtender.SetLocalizationComment(this._customAccessChoices, null);
			this.locExtender.SetLocalizationPriority(this._customAccessChoices, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._customAccessChoices, "ProjectAccessScreen._customAccessChoices");
			this._customAccessChoices.Location = new System.Drawing.Point(128, 81);
			this._customAccessChoices.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
			this._customAccessChoices.Name = "_customAccessChoices";
			this._customAccessChoices.Size = new System.Drawing.Size(356, 22);
			this._customAccessChoices.TabIndex = 5;
			this._customAccessChoices.Visible = false;
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
			this.Size = new System.Drawing.Size(507, 150);
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
		private System.Windows.Forms.Label _labelAccessProtocol;
		private System.Windows.Forms.Label _labelDescription;
		private System.Windows.Forms.ComboBox _projectAccess;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.Label _labelCustomAccess;
		private System.Windows.Forms.Label _labelCustomInstructions;
		private System.Windows.Forms.TextBox _customAccessChoices;
	}
}
