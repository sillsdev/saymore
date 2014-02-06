namespace SayMore.UI.Overview
{
	partial class ProjectDocsScreen
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
			this._labelInformation = new System.Windows.Forms.Label();
			this._linkHowArchived = new System.Windows.Forms.LinkLabel();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._descriptionFileGrid = new SayMore.UI.ElementListScreen.ComponentFileGrid();
			this._layoutTable = new System.Windows.Forms.TableLayoutPanel();
			this._splitter = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this._layoutTable.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._splitter)).BeginInit();
			this._splitter.Panel1.SuspendLayout();
			this._splitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// _labelInformation
			// 
			this._labelInformation.AutoSize = true;
			this._labelInformation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelInformation, null);
			this.locExtender.SetLocalizationComment(this._labelInformation, null);
			this.locExtender.SetLocalizationPriority(this._labelInformation, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelInformation, "ProjectView.DescriptionDocuments._labelInformation");
			this._labelInformation.Location = new System.Drawing.Point(6, 6);
			this._labelInformation.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this._labelInformation.Name = "_labelInformation";
			this._labelInformation.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
			this._labelInformation.Size = new System.Drawing.Size(307, 19);
			this._labelInformation.TabIndex = 1;
			this._labelInformation.Text = "Add documents here that describe the project and corpus.";
			// 
			// _linkHowArchived
			// 
			this._linkHowArchived.AutoSize = true;
			this._linkHowArchived.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._linkHowArchived, null);
			this.locExtender.SetLocalizationComment(this._linkHowArchived, null);
			this.locExtender.SetLocalizingId(this._linkHowArchived, "ProjectView.DescriptionDocuments._linkHowArchived");
			this._linkHowArchived.Location = new System.Drawing.Point(6, 25);
			this._linkHowArchived.Name = "_linkHowArchived";
			this._linkHowArchived.Size = new System.Drawing.Size(127, 13);
			this._linkHowArchived.TabIndex = 2;
			this._linkHowArchived.TabStop = true;
			this._linkHowArchived.Text = "How these are archived";
			this._linkHowArchived.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkHowArchived_LinkClicked);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = "ProjectView.DescriptionDocuments";
			// 
			// _descriptionFileGrid
			// 
			this._descriptionFileGrid.AddButtonEnabled = true;
			this._descriptionFileGrid.AddButtonVisible = true;
			this._descriptionFileGrid.ConvertButtonVisible = false;
			this._descriptionFileGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._descriptionFileGrid.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.locExtender.SetLocalizableToolTip(this._descriptionFileGrid, null);
			this.locExtender.SetLocalizationComment(this._descriptionFileGrid, null);
			this.locExtender.SetLocalizationPriority(this._descriptionFileGrid, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._descriptionFileGrid, "ProjectView.DescriptionDocuments._descriptionFileGrid");
			this._descriptionFileGrid.Location = new System.Drawing.Point(0, 0);
			this._descriptionFileGrid.Name = "_descriptionFileGrid";
			this._descriptionFileGrid.RenameButtonVisible = false;
			this._descriptionFileGrid.ShowContextMenu = true;
			this._descriptionFileGrid.Size = new System.Drawing.Size(441, 70);
			this._descriptionFileGrid.TabIndex = 0;
			// 
			// _layoutTable
			// 
			this._layoutTable.ColumnCount = 1;
			this._layoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._layoutTable.Controls.Add(this._linkHowArchived, 0, 1);
			this._layoutTable.Controls.Add(this._labelInformation, 0, 0);
			this._layoutTable.Controls.Add(this._splitter, 0, 3);
			this._layoutTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this._layoutTable.Location = new System.Drawing.Point(7, 7);
			this._layoutTable.Name = "_layoutTable";
			this._layoutTable.Padding = new System.Windows.Forms.Padding(3);
			this._layoutTable.RowCount = 4;
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._layoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._layoutTable.Size = new System.Drawing.Size(447, 235);
			this._layoutTable.TabIndex = 1;
			// 
			// _splitter
			// 
			this._splitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this._splitter.Location = new System.Drawing.Point(3, 68);
			this._splitter.Margin = new System.Windows.Forms.Padding(0);
			this._splitter.Name = "_splitter";
			this._splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// _splitter.Panel1
			// 
			this._splitter.Panel1.Controls.Add(this._descriptionFileGrid);
			this._splitter.Size = new System.Drawing.Size(441, 164);
			this._splitter.SplitterDistance = 70;
			this._splitter.TabIndex = 3;
			// 
			// ProjectDocsScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this._layoutTable);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ProjectDescriptionDocsScreen.EditorBase");
			this.Name = "ProjectDocsScreen";
			this.Size = new System.Drawing.Size(461, 249);
			this.Load += new System.EventHandler(this.ProjectDescriptionDocsScreen_Load);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this._layoutTable.ResumeLayout(false);
			this._layoutTable.PerformLayout();
			this._splitter.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._splitter)).EndInit();
			this._splitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.LinkLabel _linkHowArchived;
		private System.Windows.Forms.TableLayoutPanel _layoutTable;
		protected System.Windows.Forms.Label _labelInformation;
		protected ElementListScreen.ComponentFileGrid _descriptionFileGrid;
		protected System.Windows.Forms.SplitContainer _splitter;
	}
}
