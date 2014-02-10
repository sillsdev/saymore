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
			this._description = new System.Windows.Forms.TextBox();
			this._labelProjectTitle = new System.Windows.Forms.Label();
			this._labelFundingProject = new System.Windows.Forms.Label();
			this._labelDescription = new System.Windows.Forms.Label();
			this._labelVernacular = new System.Windows.Forms.Label();
			this._labelLocation = new System.Windows.Forms.Label();
			this._labelCountry = new System.Windows.Forms.Label();
			this._labelContinent = new System.Windows.Forms.Label();
			this._labelContact = new System.Windows.Forms.Label();
			this._continent = new System.Windows.Forms.ComboBox();
			this._country = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._labelSelectedVernacular = new System.Windows.Forms.Label();
			this._linkSelectVernacular = new System.Windows.Forms.LinkLabel();
			this._location = new System.Windows.Forms.TextBox();
			this._labelRegion = new System.Windows.Forms.Label();
			this._region = new System.Windows.Forms.TextBox();
			this._labelContentType = new System.Windows.Forms.Label();
			this._labelApplications = new System.Windows.Forms.Label();
			this._labelDateAvailable = new System.Windows.Forms.Label();
			this._labelRightsHolder = new System.Windows.Forms.Label();
			this._labelDepositor = new System.Windows.Forms.Label();
			this._labelRelatedPublications = new System.Windows.Forms.Label();
			this._relatedPublications = new System.Windows.Forms.TextBox();
			this._dateAvailable = new SayMore.UI.LowLevelControls.DatePicker();
			this._applications = new System.Windows.Forms.TextBox();
			this._depositor = new System.Windows.Forms.TextBox();
			this._rightsHolder = new System.Windows.Forms.TextBox();
			this._contentType = new System.Windows.Forms.TextBox();
			this._fundingProjectTitle = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this._projectTitle = new System.Windows.Forms.TextBox();
			this._linkHelp = new System.Windows.Forms.LinkLabel();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._tableLayout.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
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
			this._tableLayout.Controls.Add(this._description, 1, 1);
			this._tableLayout.Controls.Add(this._labelProjectTitle, 0, 0);
			this._tableLayout.Controls.Add(this._labelFundingProject, 0, 10);
			this._tableLayout.Controls.Add(this._labelDescription, 0, 1);
			this._tableLayout.Controls.Add(this._labelVernacular, 0, 3);
			this._tableLayout.Controls.Add(this._labelLocation, 0, 4);
			this._tableLayout.Controls.Add(this._labelCountry, 0, 6);
			this._tableLayout.Controls.Add(this._labelContinent, 0, 7);
			this._tableLayout.Controls.Add(this._labelContact, 0, 8);
			this._tableLayout.Controls.Add(this._continent, 1, 7);
			this._tableLayout.Controls.Add(this._country, 1, 6);
			this._tableLayout.Controls.Add(this.tableLayoutPanel1, 1, 3);
			this._tableLayout.Controls.Add(this._location, 1, 4);
			this._tableLayout.Controls.Add(this._labelRegion, 0, 5);
			this._tableLayout.Controls.Add(this._region, 1, 5);
			this._tableLayout.Controls.Add(this._labelContentType, 0, 15);
			this._tableLayout.Controls.Add(this._labelApplications, 0, 16);
			this._tableLayout.Controls.Add(this._labelDateAvailable, 0, 11);
			this._tableLayout.Controls.Add(this._labelRightsHolder, 0, 12);
			this._tableLayout.Controls.Add(this._labelDepositor, 0, 13);
			this._tableLayout.Controls.Add(this._labelRelatedPublications, 0, 17);
			this._tableLayout.Controls.Add(this._relatedPublications, 1, 17);
			this._tableLayout.Controls.Add(this._dateAvailable, 1, 11);
			this._tableLayout.Controls.Add(this._applications, 1, 16);
			this._tableLayout.Controls.Add(this._depositor, 1, 13);
			this._tableLayout.Controls.Add(this._rightsHolder, 1, 12);
			this._tableLayout.Controls.Add(this._contentType, 1, 15);
			this._tableLayout.Controls.Add(this._fundingProjectTitle, 1, 10);
			this._tableLayout.Controls.Add(this.flowLayoutPanel1, 1, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayout.Location = new System.Drawing.Point(7, 7);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.Padding = new System.Windows.Forms.Padding(3);
			this._tableLayout.RowCount = 19;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayout.Size = new System.Drawing.Size(639, 515);
			this._tableLayout.TabIndex = 0;
			// 
			// _contactPerson
			// 
			this.locExtender.SetLocalizableToolTip(this._contactPerson, null);
			this.locExtender.SetLocalizationComment(this._contactPerson, null);
			this.locExtender.SetLocalizationPriority(this._contactPerson, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._contactPerson, "ProjectView.MetadataScreen._contactPerson");
			this._contactPerson.Location = new System.Drawing.Point(125, 231);
			this._contactPerson.Name = "_contactPerson";
			this._contactPerson.Size = new System.Drawing.Size(350, 22);
			this._contactPerson.TabIndex = 14;
			// 
			// _description
			// 
			this.locExtender.SetLocalizableToolTip(this._description, null);
			this.locExtender.SetLocalizationComment(this._description, null);
			this.locExtender.SetLocalizationPriority(this._description, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._description, "ProjectView.MetadataScreen._description");
			this._description.Location = new System.Drawing.Point(125, 34);
			this._description.Multiline = true;
			this._description.Name = "_description";
			this._tableLayout.SetRowSpan(this._description, 2);
			this._description.Size = new System.Drawing.Size(350, 60);
			this._description.TabIndex = 3;
			// 
			// _labelProjectTitle
			// 
			this._labelProjectTitle.AutoSize = true;
			this._labelProjectTitle.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelProjectTitle, null);
			this.locExtender.SetLocalizationComment(this._labelProjectTitle, null);
			this.locExtender.SetLocalizingId(this._labelProjectTitle, "ProjectView.MetadataScreen._labelProjectTitle");
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
			this.locExtender.SetLocalizableToolTip(this._labelFundingProject, null);
			this.locExtender.SetLocalizationComment(this._labelFundingProject, null);
			this.locExtender.SetLocalizingId(this._labelFundingProject, "ProjectView.MetadataScreen._labelFundingProject");
			this._labelFundingProject.Location = new System.Drawing.Point(6, 276);
			this._labelFundingProject.Name = "_labelFundingProject";
			this._labelFundingProject.Size = new System.Drawing.Size(113, 28);
			this._labelFundingProject.TabIndex = 15;
			this._labelFundingProject.Text = "Funding Project Title";
			this._labelFundingProject.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelDescription
			// 
			this._labelDescription.AutoSize = true;
			this._labelDescription.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelDescription, null);
			this.locExtender.SetLocalizationComment(this._labelDescription, null);
			this.locExtender.SetLocalizingId(this._labelDescription, "ProjectView.MetadataScreen._labelDescription");
			this._labelDescription.Location = new System.Drawing.Point(53, 31);
			this._labelDescription.Name = "_labelDescription";
			this._labelDescription.Size = new System.Drawing.Size(66, 28);
			this._labelDescription.TabIndex = 2;
			this._labelDescription.Text = "Description";
			this._labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelVernacular
			// 
			this._labelVernacular.AutoSize = true;
			this._labelVernacular.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelVernacular, null);
			this.locExtender.SetLocalizationComment(this._labelVernacular, null);
			this.locExtender.SetLocalizingId(this._labelVernacular, "ProjectView.MetadataScreen._labelVernacular");
			this._labelVernacular.Location = new System.Drawing.Point(57, 101);
			this._labelVernacular.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this._labelVernacular.Name = "_labelVernacular";
			this._labelVernacular.Size = new System.Drawing.Size(62, 13);
			this._labelVernacular.TabIndex = 4;
			this._labelVernacular.Text = "Vernacular";
			this._labelVernacular.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelLocation
			// 
			this._labelLocation.AutoSize = true;
			this._labelLocation.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelLocation, null);
			this.locExtender.SetLocalizationComment(this._labelLocation, null);
			this.locExtender.SetLocalizingId(this._labelLocation, "ProjectView.MetadataScreen._labelLocationAddress");
			this._labelLocation.Location = new System.Drawing.Point(23, 118);
			this._labelLocation.Name = "_labelLocation";
			this._labelLocation.Size = new System.Drawing.Size(96, 28);
			this._labelLocation.TabIndex = 5;
			this._labelLocation.Text = "Location/Address";
			this._labelLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelCountry
			// 
			this._labelCountry.AutoSize = true;
			this._labelCountry.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelCountry, null);
			this.locExtender.SetLocalizationComment(this._labelCountry, null);
			this.locExtender.SetLocalizingId(this._labelCountry, "ProjectView.MetadataScreen._labelCountry");
			this._labelCountry.Location = new System.Drawing.Point(71, 174);
			this._labelCountry.Name = "_labelCountry";
			this._labelCountry.Size = new System.Drawing.Size(48, 27);
			this._labelCountry.TabIndex = 9;
			this._labelCountry.Text = "Country";
			this._labelCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelContinent
			// 
			this._labelContinent.AutoSize = true;
			this._labelContinent.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelContinent, null);
			this.locExtender.SetLocalizationComment(this._labelContinent, null);
			this.locExtender.SetLocalizingId(this._labelContinent, "ProjectView.MetadataScreen._labelContinent");
			this._labelContinent.Location = new System.Drawing.Point(60, 201);
			this._labelContinent.Name = "_labelContinent";
			this._labelContinent.Size = new System.Drawing.Size(59, 27);
			this._labelContinent.TabIndex = 11;
			this._labelContinent.Text = "Continent";
			this._labelContinent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelContact
			// 
			this._labelContact.AutoSize = true;
			this._labelContact.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelContact, null);
			this.locExtender.SetLocalizationComment(this._labelContact, null);
			this.locExtender.SetLocalizingId(this._labelContact, "ProjectView.MetadataScreen._labelContact");
			this._labelContact.Location = new System.Drawing.Point(34, 228);
			this._labelContact.Name = "_labelContact";
			this._labelContact.Size = new System.Drawing.Size(85, 28);
			this._labelContact.TabIndex = 13;
			this._labelContact.Text = "Contact Person";
			this._labelContact.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _continent
			// 
			this._continent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._continent.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._continent, null);
			this.locExtender.SetLocalizationComment(this._continent, null);
			this.locExtender.SetLocalizationPriority(this._continent, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._continent, "ProjectView.MetadataScreen._comboContinent");
			this._continent.Location = new System.Drawing.Point(125, 204);
			this._continent.Name = "_continent";
			this._continent.Size = new System.Drawing.Size(169, 21);
			this._continent.TabIndex = 12;
			// 
			// _country
			// 
			this._country.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._country, null);
			this.locExtender.SetLocalizationComment(this._country, null);
			this.locExtender.SetLocalizationPriority(this._country, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._country, "ProjectView.MetadataScreen._comboCountry");
			this._country.Location = new System.Drawing.Point(125, 177);
			this._country.Name = "_country";
			this._country.Size = new System.Drawing.Size(350, 21);
			this._country.TabIndex = 10;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._labelSelectedVernacular, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._linkSelectVernacular, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(122, 97);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(353, 21);
			this.tableLayoutPanel1.TabIndex = 19;
			// 
			// _labelSelectedVernacular
			// 
			this._labelSelectedVernacular.AutoSize = true;
			this._labelSelectedVernacular.Dock = System.Windows.Forms.DockStyle.Left;
			this.locExtender.SetLocalizableToolTip(this._labelSelectedVernacular, null);
			this.locExtender.SetLocalizationComment(this._labelSelectedVernacular, null);
			this.locExtender.SetLocalizationPriority(this._labelSelectedVernacular, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelSelectedVernacular, "ProjectView.MetadataScreen._labelSelectedVernacular");
			this._labelSelectedVernacular.Location = new System.Drawing.Point(3, 0);
			this._labelSelectedVernacular.Name = "_labelSelectedVernacular";
			this._labelSelectedVernacular.Size = new System.Drawing.Size(68, 21);
			this._labelSelectedVernacular.TabIndex = 0;
			this._labelSelectedVernacular.Text = "Unspecified";
			this._labelSelectedVernacular.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _linkSelectVernacular
			// 
			this._linkSelectVernacular.AutoSize = true;
			this._linkSelectVernacular.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._linkSelectVernacular, null);
			this.locExtender.SetLocalizationComment(this._linkSelectVernacular, null);
			this.locExtender.SetLocalizingId(this._linkSelectVernacular, "ProjectView.MetadataScreen._linkSelectVernacular");
			this._linkSelectVernacular.Location = new System.Drawing.Point(249, 0);
			this._linkSelectVernacular.Name = "_linkSelectVernacular";
			this._linkSelectVernacular.Size = new System.Drawing.Size(101, 21);
			this._linkSelectVernacular.TabIndex = 1;
			this._linkSelectVernacular.TabStop = true;
			this._linkSelectVernacular.Text = "Change Language";
			this._linkSelectVernacular.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._linkSelectVernacular.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkSelectVernacular_LinkClicked);
			// 
			// _location
			// 
			this.locExtender.SetLocalizableToolTip(this._location, null);
			this.locExtender.SetLocalizationComment(this._location, null);
			this.locExtender.SetLocalizationPriority(this._location, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._location, "ProjectView.MetadataScreen._location");
			this._location.Location = new System.Drawing.Point(125, 121);
			this._location.Name = "_location";
			this._location.Size = new System.Drawing.Size(350, 22);
			this._location.TabIndex = 6;
			// 
			// _labelRegion
			// 
			this._labelRegion.AutoSize = true;
			this._labelRegion.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelRegion, null);
			this.locExtender.SetLocalizationComment(this._labelRegion, null);
			this.locExtender.SetLocalizingId(this._labelRegion, "ProjectView.MetadataScreen._labelLocationRegion");
			this._labelRegion.Location = new System.Drawing.Point(75, 146);
			this._labelRegion.Name = "_labelRegion";
			this._labelRegion.Size = new System.Drawing.Size(44, 28);
			this._labelRegion.TabIndex = 7;
			this._labelRegion.Text = "Region";
			this._labelRegion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _region
			// 
			this.locExtender.SetLocalizableToolTip(this._region, null);
			this.locExtender.SetLocalizationComment(this._region, null);
			this.locExtender.SetLocalizationPriority(this._region, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._region, "ProjectView.MetadataScreen._region");
			this._region.Location = new System.Drawing.Point(125, 149);
			this._region.Name = "_region";
			this._region.Size = new System.Drawing.Size(350, 22);
			this._region.TabIndex = 8;
			// 
			// _labelContentType
			// 
			this._labelContentType.AutoSize = true;
			this._labelContentType.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelContentType, null);
			this.locExtender.SetLocalizationComment(this._labelContentType, null);
			this.locExtender.SetLocalizingId(this._labelContentType, "ProjectView.MetadataScreen._labelContentType");
			this._labelContentType.Location = new System.Drawing.Point(44, 408);
			this._labelContentType.Name = "_labelContentType";
			this._labelContentType.Size = new System.Drawing.Size(75, 28);
			this._labelContentType.TabIndex = 23;
			this._labelContentType.Text = "Content Type";
			this._labelContentType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelApplications
			// 
			this._labelApplications.AutoSize = true;
			this._labelApplications.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelApplications, null);
			this.locExtender.SetLocalizationComment(this._labelApplications, null);
			this.locExtender.SetLocalizingId(this._labelApplications, "ProjectView.MetadataScreen._labelApplications");
			this._labelApplications.Location = new System.Drawing.Point(48, 436);
			this._labelApplications.Name = "_labelApplications";
			this._labelApplications.Size = new System.Drawing.Size(71, 28);
			this._labelApplications.TabIndex = 25;
			this._labelApplications.Text = "Applications";
			this._labelApplications.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelDateAvailable
			// 
			this._labelDateAvailable.AutoSize = true;
			this._labelDateAvailable.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelDateAvailable, null);
			this.locExtender.SetLocalizationComment(this._labelDateAvailable, null);
			this.locExtender.SetLocalizingId(this._labelDateAvailable, "ProjectView.MetadataScreen._labelDateAvailable");
			this._labelDateAvailable.Location = new System.Drawing.Point(39, 304);
			this._labelDateAvailable.Name = "_labelDateAvailable";
			this._labelDateAvailable.Size = new System.Drawing.Size(80, 28);
			this._labelDateAvailable.TabIndex = 17;
			this._labelDateAvailable.Text = "Date Available";
			this._labelDateAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelRightsHolder
			// 
			this._labelRightsHolder.AutoSize = true;
			this._labelRightsHolder.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelRightsHolder, null);
			this.locExtender.SetLocalizationComment(this._labelRightsHolder, null);
			this.locExtender.SetLocalizingId(this._labelRightsHolder, "ProjectView.MetadataScreen._labelRightsHolder");
			this._labelRightsHolder.Location = new System.Drawing.Point(41, 332);
			this._labelRightsHolder.Name = "_labelRightsHolder";
			this._labelRightsHolder.Size = new System.Drawing.Size(78, 28);
			this._labelRightsHolder.TabIndex = 19;
			this._labelRightsHolder.Text = "Rights Holder";
			this._labelRightsHolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelDepositor
			// 
			this._labelDepositor.AutoSize = true;
			this._labelDepositor.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelDepositor, null);
			this.locExtender.SetLocalizationComment(this._labelDepositor, null);
			this.locExtender.SetLocalizingId(this._labelDepositor, "ProjectView.MetadataScreen._labelDepositor");
			this._labelDepositor.Location = new System.Drawing.Point(61, 360);
			this._labelDepositor.Name = "_labelDepositor";
			this._labelDepositor.Size = new System.Drawing.Size(58, 28);
			this._labelDepositor.TabIndex = 21;
			this._labelDepositor.Text = "Depositor";
			this._labelDepositor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _labelRelatedPublications
			// 
			this._labelRelatedPublications.AutoSize = true;
			this._labelRelatedPublications.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelRelatedPublications, null);
			this.locExtender.SetLocalizationComment(this._labelRelatedPublications, null);
			this.locExtender.SetLocalizingId(this._labelRelatedPublications, "ProjectView.MetadataScreen._labelRelatedPublications");
			this._labelRelatedPublications.Location = new System.Drawing.Point(7, 464);
			this._labelRelatedPublications.Name = "_labelRelatedPublications";
			this._labelRelatedPublications.Size = new System.Drawing.Size(112, 28);
			this._labelRelatedPublications.TabIndex = 27;
			this._labelRelatedPublications.Text = "Related Publications";
			this._labelRelatedPublications.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _relatedPublications
			// 
			this.locExtender.SetLocalizableToolTip(this._relatedPublications, null);
			this.locExtender.SetLocalizationComment(this._relatedPublications, null);
			this.locExtender.SetLocalizationPriority(this._relatedPublications, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._relatedPublications, "ProjectView.MetadataScreen._relatedPublications");
			this._relatedPublications.Location = new System.Drawing.Point(125, 467);
			this._relatedPublications.Name = "_relatedPublications";
			this._relatedPublications.Size = new System.Drawing.Size(350, 22);
			this._relatedPublications.TabIndex = 28;
			// 
			// _dateAvailable
			// 
			this._dateAvailable.CustomFormat = "";
			this._dateAvailable.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._dateAvailable.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.locExtender.SetLocalizableToolTip(this._dateAvailable, null);
			this.locExtender.SetLocalizationComment(this._dateAvailable, null);
			this.locExtender.SetLocalizationPriority(this._dateAvailable, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._dateAvailable, "ProjectView.MetadataScreen._dateAvailable");
			this._dateAvailable.Location = new System.Drawing.Point(125, 307);
			this._dateAvailable.Name = "_dateAvailable";
			this._dateAvailable.Size = new System.Drawing.Size(96, 22);
			this._dateAvailable.TabIndex = 18;
			this._dateAvailable.Value = new System.DateTime(2013, 12, 18, 9, 55, 23, 0);
			// 
			// _applications
			// 
			this.locExtender.SetLocalizableToolTip(this._applications, null);
			this.locExtender.SetLocalizationComment(this._applications, null);
			this.locExtender.SetLocalizationPriority(this._applications, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._applications, "ProjectView.MetadataScreen._applications");
			this._applications.Location = new System.Drawing.Point(125, 439);
			this._applications.Name = "_applications";
			this._applications.Size = new System.Drawing.Size(350, 22);
			this._applications.TabIndex = 26;
			// 
			// _depositor
			// 
			this.locExtender.SetLocalizableToolTip(this._depositor, null);
			this.locExtender.SetLocalizationComment(this._depositor, null);
			this.locExtender.SetLocalizationPriority(this._depositor, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._depositor, "ProjectView.MetadataScreen._depositor");
			this._depositor.Location = new System.Drawing.Point(125, 363);
			this._depositor.Name = "_depositor";
			this._depositor.Size = new System.Drawing.Size(350, 22);
			this._depositor.TabIndex = 22;
			// 
			// _rightsHolder
			// 
			this.locExtender.SetLocalizableToolTip(this._rightsHolder, null);
			this.locExtender.SetLocalizationComment(this._rightsHolder, null);
			this.locExtender.SetLocalizationPriority(this._rightsHolder, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._rightsHolder, "ProjectView.MetadataScreen._rightsHolder");
			this._rightsHolder.Location = new System.Drawing.Point(125, 335);
			this._rightsHolder.Name = "_rightsHolder";
			this._rightsHolder.Size = new System.Drawing.Size(350, 22);
			this._rightsHolder.TabIndex = 20;
			// 
			// _contentType
			// 
			this.locExtender.SetLocalizableToolTip(this._contentType, null);
			this.locExtender.SetLocalizationComment(this._contentType, null);
			this.locExtender.SetLocalizationPriority(this._contentType, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._contentType, "ProjectView.MetadataScreen._contentType");
			this._contentType.Location = new System.Drawing.Point(125, 411);
			this._contentType.Name = "_contentType";
			this._contentType.Size = new System.Drawing.Size(350, 22);
			this._contentType.TabIndex = 24;
			// 
			// _fundingProjectTitle
			// 
			this.locExtender.SetLocalizableToolTip(this._fundingProjectTitle, null);
			this.locExtender.SetLocalizationComment(this._fundingProjectTitle, null);
			this.locExtender.SetLocalizationPriority(this._fundingProjectTitle, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._fundingProjectTitle, "ProjectView.MetadataScreen._fundingProjectTitle");
			this._fundingProjectTitle.Location = new System.Drawing.Point(125, 279);
			this._fundingProjectTitle.Name = "_fundingProjectTitle";
			this._fundingProjectTitle.Size = new System.Drawing.Size(350, 22);
			this._fundingProjectTitle.TabIndex = 16;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this._projectTitle);
			this.flowLayoutPanel1.Controls.Add(this._linkHelp);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(122, 3);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(473, 28);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// _projectTitle
			// 
			this.locExtender.SetLocalizableToolTip(this._projectTitle, null);
			this.locExtender.SetLocalizationComment(this._projectTitle, null);
			this.locExtender.SetLocalizationPriority(this._projectTitle, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._projectTitle, "ProjectView.MetadataScreen._projectTitle");
			this._projectTitle.Location = new System.Drawing.Point(3, 3);
			this._projectTitle.Name = "_projectTitle";
			this._projectTitle.Size = new System.Drawing.Size(350, 22);
			this._projectTitle.TabIndex = 0;
			// 
			// _linkHelp
			// 
			this._linkHelp.AutoSize = true;
			this._linkHelp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this._linkHelp, null);
			this.locExtender.SetLocalizationComment(this._linkHelp, null);
			this.locExtender.SetLocalizingId(this._linkHelp, "ProjectView.MetadataScreen._linkHelp");
			this._linkHelp.Location = new System.Drawing.Point(359, 0);
			this._linkHelp.Name = "_linkHelp";
			this._linkHelp.Size = new System.Drawing.Size(111, 28);
			this._linkHelp.TabIndex = 0;
			this._linkHelp.TabStop = true;
			this._linkHelp.Text = "Help for these fields";
			this._linkHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this._tableLayout);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "ProjectView.MetadataScreen");
			this.Name = "ProjectMetadataScreen";
			this.Size = new System.Drawing.Size(653, 608);
			this.Load += new System.EventHandler(this.ProjectMetadataScreen_Load);
			this.Leave += new System.EventHandler(this.ProjectMetadataScreen_Leave);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
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
		private System.Windows.Forms.Label _labelRegion;
		private System.Windows.Forms.TextBox _region;
		private System.Windows.Forms.Label _labelContentType;
		private System.Windows.Forms.TextBox _contentType;
		private System.Windows.Forms.Label _labelApplications;
		private System.Windows.Forms.TextBox _applications;
		private System.Windows.Forms.Label _labelDateAvailable;
		private System.Windows.Forms.Label _labelRightsHolder;
		private System.Windows.Forms.TextBox _rightsHolder;
		private System.Windows.Forms.Label _labelDepositor;
		private System.Windows.Forms.TextBox _depositor;
		private System.Windows.Forms.Label _labelRelatedPublications;
		private System.Windows.Forms.TextBox _relatedPublications;
		private LowLevelControls.DatePicker _dateAvailable;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.LinkLabel _linkHelp;

	}
}
