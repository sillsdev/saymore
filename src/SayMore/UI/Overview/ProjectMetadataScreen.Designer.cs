namespace SayMore.UI.Overview
{
	sealed partial class ProjectMetadataScreen
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
			this._contactPerson = new System.Windows.Forms.TextBox();
			this._location = new System.Windows.Forms.TextBox();
			this._description = new System.Windows.Forms.TextBox();
			this._fundingProjectTitle = new System.Windows.Forms.TextBox();
			this._labelProjectTitle = new System.Windows.Forms.Label();
			this._labelFundingProject = new System.Windows.Forms.Label();
			this._labelDescription = new System.Windows.Forms.Label();
			this._labelVernacular = new System.Windows.Forms.Label();
			this._labelLocation = new System.Windows.Forms.Label();
			this._labelCountry = new System.Windows.Forms.Label();
			this._labelContinent = new System.Windows.Forms.Label();
			this._labelContact = new System.Windows.Forms.Label();
			this._projectTitle = new System.Windows.Forms.TextBox();
			this._continent = new System.Windows.Forms.ComboBox();
			this._country = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._labelSelectedVernacular = new System.Windows.Forms.Label();
			this._linkSelectVernacular = new System.Windows.Forms.LinkLabel();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._tableLayout.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayout.Controls.Add(this._contactPerson, 1, 8);
			this._tableLayout.Controls.Add(this._location, 1, 5);
			this._tableLayout.Controls.Add(this._description, 1, 2);
			this._tableLayout.Controls.Add(this._fundingProjectTitle, 1, 1);
			this._tableLayout.Controls.Add(this._labelProjectTitle, 0, 0);
			this._tableLayout.Controls.Add(this._labelFundingProject, 0, 1);
			this._tableLayout.Controls.Add(this._labelDescription, 0, 2);
			this._tableLayout.Controls.Add(this._labelVernacular, 0, 4);
			this._tableLayout.Controls.Add(this._labelLocation, 0, 5);
			this._tableLayout.Controls.Add(this._labelCountry, 0, 6);
			this._tableLayout.Controls.Add(this._labelContinent, 0, 7);
			this._tableLayout.Controls.Add(this._labelContact, 0, 8);
			this._tableLayout.Controls.Add(this._projectTitle, 1, 0);
			this._tableLayout.Controls.Add(this._continent, 1, 7);
			this._tableLayout.Controls.Add(this._country, 1, 6);
			this._tableLayout.Controls.Add(this.tableLayoutPanel1, 1, 4);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayout.Location = new System.Drawing.Point(7, 7);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.Padding = new System.Windows.Forms.Padding(3);
			this._tableLayout.RowCount = 9;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(639, 251);
			this._tableLayout.TabIndex = 0;
			// 
			// _contactPerson
			// 
			this._contactPerson.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._contactPerson, null);
			this.locExtender.SetLocalizationComment(this._contactPerson, null);
			this.locExtender.SetLocalizationPriority(this._contactPerson, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._contactPerson, "ProjectMetadataScreen._contactPerson");
			this._contactPerson.Location = new System.Drawing.Point(125, 223);
			this._contactPerson.Name = "_contactPerson";
			this._contactPerson.Size = new System.Drawing.Size(350, 22);
			this._contactPerson.TabIndex = 15;
			// 
			// _location
			// 
			this._location.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._location, null);
			this.locExtender.SetLocalizationComment(this._location, null);
			this.locExtender.SetLocalizationPriority(this._location, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._location, "ProjectMetadataScreen._location");
			this._location.Location = new System.Drawing.Point(125, 141);
			this._location.Name = "_location";
			this._location.Size = new System.Drawing.Size(350, 22);
			this._location.TabIndex = 12;
			// 
			// _description
			// 
			this._description.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._description, null);
			this.locExtender.SetLocalizationComment(this._description, null);
			this.locExtender.SetLocalizationPriority(this._description, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._description, "ProjectMetadataScreen._description");
			this._description.Location = new System.Drawing.Point(125, 62);
			this._description.Multiline = true;
			this._description.Name = "_description";
			this._tableLayout.SetRowSpan(this._description, 2);
			this._description.Size = new System.Drawing.Size(350, 60);
			this._description.TabIndex = 10;
			// 
			// _fundingProjectTitle
			// 
			this._fundingProjectTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._fundingProjectTitle, null);
			this.locExtender.SetLocalizationComment(this._fundingProjectTitle, null);
			this.locExtender.SetLocalizationPriority(this._fundingProjectTitle, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._fundingProjectTitle, "ProjectMetadataScreen._fundingProjectTitle");
			this._fundingProjectTitle.Location = new System.Drawing.Point(125, 34);
			this._fundingProjectTitle.Name = "_fundingProjectTitle";
			this._fundingProjectTitle.Size = new System.Drawing.Size(350, 22);
			this._fundingProjectTitle.TabIndex = 9;
			// 
			// _labelProjectTitle
			// 
			this._labelProjectTitle.AutoSize = true;
			this._labelProjectTitle.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelProjectTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelProjectTitle, null);
			this.locExtender.SetLocalizationComment(this._labelProjectTitle, null);
			this.locExtender.SetLocalizingId(this._labelProjectTitle, "ProjectMetadataScreen._labelProjectTitle");
			this._labelProjectTitle.Location = new System.Drawing.Point(91, 3);
			this._labelProjectTitle.Name = "_labelProjectTitle";
			this._labelProjectTitle.Size = new System.Drawing.Size(28, 28);
			this._labelProjectTitle.TabIndex = 0;
			this._labelProjectTitle.Text = "Title";
			this._labelProjectTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelFundingProject
			// 
			this._labelFundingProject.AutoSize = true;
			this._labelFundingProject.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelFundingProject.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelFundingProject, null);
			this.locExtender.SetLocalizationComment(this._labelFundingProject, null);
			this.locExtender.SetLocalizingId(this._labelFundingProject, "ProjectMetadataScreen._labelFundingProject");
			this._labelFundingProject.Location = new System.Drawing.Point(6, 31);
			this._labelFundingProject.Name = "_labelFundingProject";
			this._labelFundingProject.Size = new System.Drawing.Size(113, 28);
			this._labelFundingProject.TabIndex = 1;
			this._labelFundingProject.Text = "Funding Project Title";
			this._labelFundingProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelDescription
			// 
			this._labelDescription.AutoSize = true;
			this._labelDescription.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelDescription.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelDescription, null);
			this.locExtender.SetLocalizationComment(this._labelDescription, null);
			this.locExtender.SetLocalizingId(this._labelDescription, "ProjectMetadataScreen._labelDescription");
			this._labelDescription.Location = new System.Drawing.Point(53, 59);
			this._labelDescription.Name = "_labelDescription";
			this._labelDescription.Size = new System.Drawing.Size(66, 20);
			this._labelDescription.TabIndex = 2;
			this._labelDescription.Text = "Description";
			this._labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelVernacular
			// 
			this._labelVernacular.AutoSize = true;
			this._labelVernacular.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelVernacular.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelVernacular, null);
			this.locExtender.SetLocalizationComment(this._labelVernacular, null);
			this.locExtender.SetLocalizingId(this._labelVernacular, "ProjectMetadataScreen._labelVernacular");
			this._labelVernacular.Location = new System.Drawing.Point(57, 125);
			this._labelVernacular.Name = "_labelVernacular";
			this._labelVernacular.Size = new System.Drawing.Size(62, 13);
			this._labelVernacular.TabIndex = 3;
			this._labelVernacular.Text = "Vernacular";
			this._labelVernacular.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelLocation
			// 
			this._labelLocation.AutoSize = true;
			this._labelLocation.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelLocation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelLocation, null);
			this.locExtender.SetLocalizationComment(this._labelLocation, null);
			this.locExtender.SetLocalizingId(this._labelLocation, "ProjectMetadataScreen._labelLocation");
			this._labelLocation.Location = new System.Drawing.Point(68, 138);
			this._labelLocation.Name = "_labelLocation";
			this._labelLocation.Size = new System.Drawing.Size(51, 28);
			this._labelLocation.TabIndex = 4;
			this._labelLocation.Text = "Location";
			this._labelLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelCountry
			// 
			this._labelCountry.AutoSize = true;
			this._labelCountry.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelCountry.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelCountry, null);
			this.locExtender.SetLocalizationComment(this._labelCountry, null);
			this.locExtender.SetLocalizingId(this._labelCountry, "ProjectMetadataScreen._labelCountry");
			this._labelCountry.Location = new System.Drawing.Point(71, 166);
			this._labelCountry.Name = "_labelCountry";
			this._labelCountry.Size = new System.Drawing.Size(48, 27);
			this._labelCountry.TabIndex = 5;
			this._labelCountry.Text = "Country";
			this._labelCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelContinent
			// 
			this._labelContinent.AutoSize = true;
			this._labelContinent.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelContinent.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelContinent, null);
			this.locExtender.SetLocalizationComment(this._labelContinent, null);
			this.locExtender.SetLocalizingId(this._labelContinent, "ProjectMetadataScreen._labelContinent");
			this._labelContinent.Location = new System.Drawing.Point(60, 193);
			this._labelContinent.Name = "_labelContinent";
			this._labelContinent.Size = new System.Drawing.Size(59, 27);
			this._labelContinent.TabIndex = 6;
			this._labelContinent.Text = "Continent";
			this._labelContinent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelContact
			// 
			this._labelContact.AutoSize = true;
			this._labelContact.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelContact.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelContact, null);
			this.locExtender.SetLocalizationComment(this._labelContact, null);
			this.locExtender.SetLocalizingId(this._labelContact, "ProjectMetadataScreen._labelContact");
			this._labelContact.Location = new System.Drawing.Point(34, 220);
			this._labelContact.Name = "_labelContact";
			this._labelContact.Size = new System.Drawing.Size(85, 28);
			this._labelContact.TabIndex = 7;
			this._labelContact.Text = "Contact Person";
			this._labelContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _projectTitle
			// 
			this._projectTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._projectTitle, null);
			this.locExtender.SetLocalizationComment(this._projectTitle, null);
			this.locExtender.SetLocalizationPriority(this._projectTitle, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._projectTitle, "ProjectMetadataScreen._projectTitle");
			this._projectTitle.Location = new System.Drawing.Point(125, 6);
			this._projectTitle.Name = "_projectTitle";
			this._projectTitle.Size = new System.Drawing.Size(350, 22);
			this._projectTitle.TabIndex = 8;
			// 
			// _continent
			// 
			this._continent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._continent.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._continent.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._continent, null);
			this.locExtender.SetLocalizationComment(this._continent, null);
			this.locExtender.SetLocalizationPriority(this._continent, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._continent, "ProjectMetadataScreen._comboContinent");
			this._continent.Location = new System.Drawing.Point(125, 196);
			this._continent.Name = "_continent";
			this._continent.Size = new System.Drawing.Size(169, 21);
			this._continent.TabIndex = 16;
			// 
			// _country
			// 
			this._country.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._country.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._country, null);
			this.locExtender.SetLocalizationComment(this._country, null);
			this.locExtender.SetLocalizationPriority(this._country, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._country, "ProjectMetadataScreen._comboCountry");
			this._country.Location = new System.Drawing.Point(125, 169);
			this._country.Name = "_country";
			this._country.Size = new System.Drawing.Size(350, 21);
			this._country.TabIndex = 17;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._labelSelectedVernacular, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._linkSelectVernacular, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(122, 125);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(353, 13);
			this.tableLayoutPanel1.TabIndex = 19;
			// 
			// _labelSelectedVernacular
			// 
			this._labelSelectedVernacular.AutoSize = true;
			this._labelSelectedVernacular.Dock = System.Windows.Forms.DockStyle.Left;
			this._labelSelectedVernacular.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelSelectedVernacular, null);
			this.locExtender.SetLocalizationComment(this._labelSelectedVernacular, null);
			this.locExtender.SetLocalizationPriority(this._labelSelectedVernacular, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelSelectedVernacular, "ProjectMetadataScreen._labelSelectedVernacular");
			this._labelSelectedVernacular.Location = new System.Drawing.Point(3, 0);
			this._labelSelectedVernacular.Name = "_labelSelectedVernacular";
			this._labelSelectedVernacular.Size = new System.Drawing.Size(68, 13);
			this._labelSelectedVernacular.TabIndex = 18;
			this._labelSelectedVernacular.Text = "Unspecified";
			this._labelSelectedVernacular.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _linkSelectVernacular
			// 
			this._linkSelectVernacular.AutoSize = true;
			this._linkSelectVernacular.Dock = System.Windows.Forms.DockStyle.Right;
			this._linkSelectVernacular.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._linkSelectVernacular, null);
			this.locExtender.SetLocalizationComment(this._linkSelectVernacular, null);
			this.locExtender.SetLocalizingId(this._linkSelectVernacular, "ProjectMetadataScreen._linkSelectVernacular");
			this._linkSelectVernacular.Location = new System.Drawing.Point(249, 0);
			this._linkSelectVernacular.Name = "_linkSelectVernacular";
			this._linkSelectVernacular.Size = new System.Drawing.Size(101, 13);
			this._linkSelectVernacular.TabIndex = 19;
			this._linkSelectVernacular.TabStop = true;
			this._linkSelectVernacular.Text = "Change Language";
			this._linkSelectVernacular.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._linkSelectVernacular.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkSelectVernacular_LinkClicked);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = null;
			// 
			// ProjectMetadataScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ProjectMetadataScreen.ProjectMetadataScreen");
			this.Name = "ProjectMetadataScreen";
			this.Size = new System.Drawing.Size(653, 346);
			this.Load += new System.EventHandler(this.ProjectMetadataScreen_Load);
			this.Leave += new System.EventHandler(this.ProjectMetadataScreen_Leave);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Label _labelProjectTitle;
		private System.Windows.Forms.Label _labelFundingProject;
		private System.Windows.Forms.Label _labelDescription;
		private System.Windows.Forms.Label _labelVernacular;
		private System.Windows.Forms.Label _labelLocation;
		private System.Windows.Forms.Label _labelCountry;
		private System.Windows.Forms.Label _labelContinent;
		private System.Windows.Forms.Label _labelContact;
		private System.Windows.Forms.TextBox _projectTitle;
		private System.Windows.Forms.TextBox _contactPerson;
		private System.Windows.Forms.TextBox _location;
		private System.Windows.Forms.TextBox _description;
		private System.Windows.Forms.TextBox _fundingProjectTitle;
		private System.Windows.Forms.ComboBox _continent;
		private System.Windows.Forms.ComboBox _country;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label _labelSelectedVernacular;
		private System.Windows.Forms.LinkLabel _linkSelectVernacular;
		private L10NSharp.UI.L10NSharpExtender locExtender;

	}
}
