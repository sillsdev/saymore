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
			this._labelContinent = new System.Windows.Forms.Label();
			this._projectTitle = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this._linkHelp = new System.Windows.Forms.LinkLabel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._labelSelectedContentLanguage = new System.Windows.Forms.Label();
			this._labelSelectedWorkingLanguage = new System.Windows.Forms.Label();
			this._linkSelectContentLanguage = new System.Windows.Forms.LinkLabel();
			this._linkSelectWorkingLanguage = new System.Windows.Forms.LinkLabel();
			this._linkSelectFontForWorkingLanguage = new System.Windows.Forms.LinkLabel();
			this._location = new System.Windows.Forms.TextBox();
			this._region = new System.Windows.Forms.TextBox();
			this._depositor = new System.Windows.Forms.TextBox();
			this._rightsHolder = new System.Windows.Forms.TextBox();
			this._dateAvailable = new SayMore.UI.LowLevelControls.DatePicker();
			this._fundingProjectTitle = new System.Windows.Forms.TextBox();
			this._labelProjectBasics = new System.Windows.Forms.Label();
			this._labelMainLocation = new System.Windows.Forms.Label();
			this._labelResponsibilities = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this._labelsubOrSimilar = new System.Windows.Forms.Label();
			this._labelAddress = new System.Windows.Forms.Label();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this._labelsubLikeStateOrProvince = new System.Windows.Forms.Label();
			this._labelRegion = new System.Windows.Forms.Label();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this._labelsubPerson1 = new System.Windows.Forms.Label();
			this._labelContact = new System.Windows.Forms.Label();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this._labelsubTitle = new System.Windows.Forms.Label();
			this._labelFundingProject = new System.Windows.Forms.Label();
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this._labelsubDate = new System.Windows.Forms.Label();
			this._labelDateAvailable = new System.Windows.Forms.Label();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this._labelRightsHolder = new System.Windows.Forms.Label();
			this._labelsubPerson2 = new System.Windows.Forms.Label();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this._labelsubPerson3 = new System.Windows.Forms.Label();
			this._labelDepositor = new System.Windows.Forms.Label();
			this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
			this._labelsubLong = new System.Windows.Forms.Label();
			this._labelProjectTitle = new System.Windows.Forms.Label();
			this._country = new System.Windows.Forms.ComboBox();
			this._continent = new System.Windows.Forms.ComboBox();
			this._labelCountry = new System.Windows.Forms.Label();
			this._contactPerson = new System.Windows.Forms.TextBox();
			this._description = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this._labelsubOfTheProject = new System.Windows.Forms.Label();
			this._labelDescription = new System.Windows.Forms.Label();
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this._tableLayout.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.tableLayoutPanel9.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.ColumnCount = 6;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this._tableLayout.Controls.Add(this._labelContinent, 4, 8);
			this._tableLayout.Controls.Add(this._projectTitle, 2, 3);
			this._tableLayout.Controls.Add(this.flowLayoutPanel1, 2, 4);
			this._tableLayout.Controls.Add(this._linkHelp, 5, 0);
			this._tableLayout.Controls.Add(this.tableLayoutPanel1, 2, 6);
			this._tableLayout.Controls.Add(this._location, 5, 3);
			this._tableLayout.Controls.Add(this._region, 5, 6);
			this._tableLayout.Controls.Add(this._depositor, 5, 15);
			this._tableLayout.Controls.Add(this._rightsHolder, 5, 14);
			this._tableLayout.Controls.Add(this._dateAvailable, 5, 13);
			this._tableLayout.Controls.Add(this._fundingProjectTitle, 5, 12);
			this._tableLayout.Controls.Add(this._labelProjectBasics, 2, 1);
			this._tableLayout.Controls.Add(this._labelMainLocation, 5, 1);
			this._tableLayout.Controls.Add(this._labelResponsibilities, 4, 10);
			this._tableLayout.Controls.Add(this.tableLayoutPanel2, 4, 3);
			this._tableLayout.Controls.Add(this.tableLayoutPanel3, 4, 6);
			this._tableLayout.Controls.Add(this.tableLayoutPanel5, 1, 12);
			this._tableLayout.Controls.Add(this.tableLayoutPanel6, 4, 12);
			this._tableLayout.Controls.Add(this.tableLayoutPanel9, 4, 13);
			this._tableLayout.Controls.Add(this.tableLayoutPanel7, 4, 14);
			this._tableLayout.Controls.Add(this.tableLayoutPanel8, 4, 15);
			this._tableLayout.Controls.Add(this.tableLayoutPanel10, 1, 3);
			this._tableLayout.Controls.Add(this._country, 5, 7);
			this._tableLayout.Controls.Add(this._continent, 5, 8);
			this._tableLayout.Controls.Add(this._labelCountry, 4, 7);
			this._tableLayout.Controls.Add(this._contactPerson, 2, 12);
			this._tableLayout.Controls.Add(this._description, 2, 8);
			this._tableLayout.Controls.Add(this.tableLayoutPanel4, 1, 8);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tableLayout.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._tableLayout.Location = new System.Drawing.Point(7, 7);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.Padding = new System.Windows.Forms.Padding(3);
			this._tableLayout.RowCount = 16;
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(961, 611);
			this._tableLayout.TabIndex = 0;
			// 
			// _labelContinent
			// 
			this._labelContinent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelContinent.AutoSize = true;
			this._labelContinent.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelContinent, null);
			this.locExtender.SetLocalizationComment(this._labelContinent, null);
			this.locExtender.SetLocalizingId(this._labelContinent, "ProjectView.MetadataScreen._labelContinent");
			this._labelContinent.Location = new System.Drawing.Point(571, 175);
			this._labelContinent.Name = "_labelContinent";
			this._labelContinent.Size = new System.Drawing.Size(69, 17);
			this._labelContinent.TabIndex = 10;
			this._labelContinent.Text = "Con&tinent";
			this._labelContinent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _projectTitle
			// 
			this._projectTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._projectTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._projectTitle, null);
			this.locExtender.SetLocalizationComment(this._projectTitle, null);
			this.locExtender.SetLocalizationPriority(this._projectTitle, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._projectTitle, "ProjectView.MetadataScreen._projectTitle");
			this._projectTitle.Location = new System.Drawing.Point(112, 44);
			this._projectTitle.Name = "_projectTitle";
			this._projectTitle.Size = new System.Drawing.Size(377, 29);
			this._projectTitle.TabIndex = 0;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(109, 101);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(0, 0);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// _linkHelp
			// 
			this._linkHelp.AutoSize = true;
			this._linkHelp.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._linkHelp, null);
			this.locExtender.SetLocalizationComment(this._linkHelp, null);
			this.locExtender.SetLocalizingId(this._linkHelp, "ProjectView.MetadataScreen._linkHelp");
			this._linkHelp.Location = new System.Drawing.Point(844, 3);
			this._linkHelp.Name = "_linkHelp";
			this._linkHelp.Size = new System.Drawing.Size(111, 13);
			this._linkHelp.TabIndex = 0;
			this._linkHelp.TabStop = true;
			this._linkHelp.Text = "Help for these fields";
			this._linkHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._labelSelectedContentLanguage, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._labelSelectedWorkingLanguage, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this._linkSelectContentLanguage, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._linkSelectWorkingLanguage, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this._linkSelectFontForWorkingLanguage, 1, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(109, 101);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this._tableLayout.SetRowSpan(this.tableLayoutPanel1, 2);
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(383, 47);
			this.tableLayoutPanel1.TabIndex = 5;
			// 
			// _labelSelectedContentLanguage
			// 
			this._labelSelectedContentLanguage.AutoSize = true;
			this._labelSelectedContentLanguage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelSelectedContentLanguage, null);
			this.locExtender.SetLocalizationComment(this._labelSelectedContentLanguage, null);
			this.locExtender.SetLocalizationPriority(this._labelSelectedContentLanguage, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelSelectedContentLanguage, "ProjectView.MetadataScreen._labelSelectedContentLanguage");
			this._labelSelectedContentLanguage.Location = new System.Drawing.Point(3, 0);
			this._labelSelectedContentLanguage.Name = "_labelSelectedContentLanguage";
			this._labelSelectedContentLanguage.Size = new System.Drawing.Size(80, 17);
			this._labelSelectedContentLanguage.TabIndex = 1;
			this._labelSelectedContentLanguage.Text = "Unspecified";
			this._labelSelectedContentLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _labelSelectedWorkingLanguage
			// 
			this._labelSelectedWorkingLanguage.AutoSize = true;
			this._labelSelectedWorkingLanguage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelSelectedWorkingLanguage, null);
			this.locExtender.SetLocalizationComment(this._labelSelectedWorkingLanguage, null);
			this.locExtender.SetLocalizationPriority(this._labelSelectedWorkingLanguage, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelSelectedWorkingLanguage, "ProjectView.MetadataScreen._labelSelectedWorkingLanguage");
			this._labelSelectedWorkingLanguage.Location = new System.Drawing.Point(194, 0);
			this._labelSelectedWorkingLanguage.Name = "_labelSelectedWorkingLanguage";
			this._labelSelectedWorkingLanguage.Size = new System.Drawing.Size(80, 17);
			this._labelSelectedWorkingLanguage.TabIndex = 3;
			this._labelSelectedWorkingLanguage.Text = "Unspecified";
			this._labelSelectedWorkingLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this._labelSelectedWorkingLanguage.TextChanged += new System.EventHandler(this.WorkingLanguageChanged);
			// 
			// _linkSelectContentLanguage
			// 
			this._linkSelectContentLanguage.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkSelectContentLanguage, null);
			this.locExtender.SetLocalizationComment(this._linkSelectContentLanguage, null);
			this.locExtender.SetLocalizingId(this._linkSelectContentLanguage, "ProjectView.MetadataScreen._linkSelectContentLanguage");
			this._linkSelectContentLanguage.Location = new System.Drawing.Point(3, 17);
			this._linkSelectContentLanguage.Name = "_linkSelectContentLanguage";
			this._linkSelectContentLanguage.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._linkSelectContentLanguage.Size = new System.Drawing.Size(103, 14);
			this._linkSelectContentLanguage.TabIndex = 0;
			this._linkSelectContentLanguage.TabStop = true;
			this._linkSelectContentLanguage.Text = "Content Language";
			this._linkSelectContentLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._linkSelectContentLanguage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkSelectContentLanguage_LinkClicked);
			// 
			// _linkSelectWorkingLanguage
			// 
			this._linkSelectWorkingLanguage.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkSelectWorkingLanguage, null);
			this.locExtender.SetLocalizationComment(this._linkSelectWorkingLanguage, null);
			this.locExtender.SetLocalizingId(this._linkSelectWorkingLanguage, "ProjectView.MetadataScreen._linkSelectWorkingLanguage");
			this._linkSelectWorkingLanguage.Location = new System.Drawing.Point(194, 17);
			this._linkSelectWorkingLanguage.Name = "_linkSelectWorkingLanguage";
			this._linkSelectWorkingLanguage.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._linkSelectWorkingLanguage.Size = new System.Drawing.Size(106, 14);
			this._linkSelectWorkingLanguage.TabIndex = 4;
			this._linkSelectWorkingLanguage.TabStop = true;
			this._linkSelectWorkingLanguage.Text = "Working Language";
			this._linkSelectWorkingLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this._linkSelectWorkingLanguage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkSelectWorkingLanguage_LinkClicked);
			// 
			// _linkSelectFontForWorkingLanguage
			// 
			this._linkSelectFontForWorkingLanguage.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._linkSelectFontForWorkingLanguage, null);
			this.locExtender.SetLocalizationComment(this._linkSelectFontForWorkingLanguage, null);
			this.locExtender.SetLocalizingId(this._linkSelectFontForWorkingLanguage, "ProjectMetadataScreen.linkLabel1");
			this._linkSelectFontForWorkingLanguage.Location = new System.Drawing.Point(194, 31);
			this._linkSelectFontForWorkingLanguage.Name = "_linkSelectFontForWorkingLanguage";
			this._linkSelectFontForWorkingLanguage.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this._linkSelectFontForWorkingLanguage.Size = new System.Drawing.Size(103, 16);
			this._linkSelectFontForWorkingLanguage.TabIndex = 5;
			this._linkSelectFontForWorkingLanguage.TabStop = true;
			this._linkSelectFontForWorkingLanguage.Text = "Default font for {0}";
			this._linkSelectFontForWorkingLanguage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._linkSelectFontForWorkingLanguage_LinkClicked);
			// 
			// _location
			// 
			this._location.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._location.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._location, null);
			this.locExtender.SetLocalizationComment(this._location, null);
			this.locExtender.SetLocalizationPriority(this._location, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._location, "ProjectView.MetadataScreen._location");
			this._location.Location = new System.Drawing.Point(646, 44);
			this._location.Multiline = true;
			this._location.Name = "_location";
			this._location.Size = new System.Drawing.Size(309, 54);
			this._location.TabIndex = 3;
			// 
			// _region
			// 
			this._region.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._region.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._region, null);
			this.locExtender.SetLocalizationComment(this._region, null);
			this.locExtender.SetLocalizationPriority(this._region, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._region, "ProjectView.MetadataScreen._region");
			this._region.Location = new System.Drawing.Point(646, 104);
			this._region.Name = "_region";
			this._region.Size = new System.Drawing.Size(309, 29);
			this._region.TabIndex = 4;
			// 
			// _depositor
			// 
			this._depositor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._depositor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._depositor, null);
			this.locExtender.SetLocalizationComment(this._depositor, null);
			this.locExtender.SetLocalizationPriority(this._depositor, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._depositor, "ProjectView.MetadataScreen._depositor");
			this._depositor.Location = new System.Drawing.Point(646, 566);
			this._depositor.Name = "_depositor";
			this._depositor.Size = new System.Drawing.Size(309, 29);
			this._depositor.TabIndex = 9;
			// 
			// _rightsHolder
			// 
			this._rightsHolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._rightsHolder.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._rightsHolder, null);
			this.locExtender.SetLocalizationComment(this._rightsHolder, null);
			this.locExtender.SetLocalizationPriority(this._rightsHolder, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._rightsHolder, "ProjectView.MetadataScreen._rightsHolder");
			this._rightsHolder.Location = new System.Drawing.Point(646, 530);
			this._rightsHolder.Name = "_rightsHolder";
			this._rightsHolder.Size = new System.Drawing.Size(309, 29);
			this._rightsHolder.TabIndex = 8;
			// 
			// _dateAvailable
			// 
			this._dateAvailable.CustomFormat = "";
			this._dateAvailable.Dock = System.Windows.Forms.DockStyle.Left;
			this._dateAvailable.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._dateAvailable.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.locExtender.SetLocalizableToolTip(this._dateAvailable, null);
			this.locExtender.SetLocalizationComment(this._dateAvailable, null);
			this.locExtender.SetLocalizationPriority(this._dateAvailable, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._dateAvailable, "ProjectView.MetadataScreen._dateAvailable");
			this._dateAvailable.Location = new System.Drawing.Point(646, 494);
			this._dateAvailable.Name = "_dateAvailable";
			this._dateAvailable.Size = new System.Drawing.Size(254, 29);
			this._dateAvailable.TabIndex = 7;
			this._dateAvailable.Value = new System.DateTime(2013, 12, 18, 9, 55, 23, 0);
			this._dateAvailable.Validated += new System.EventHandler(this._dateAvailable_Validated);
			// 
			// _fundingProjectTitle
			// 
			this._fundingProjectTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._fundingProjectTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._fundingProjectTitle, null);
			this.locExtender.SetLocalizationComment(this._fundingProjectTitle, null);
			this.locExtender.SetLocalizationPriority(this._fundingProjectTitle, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._fundingProjectTitle, "ProjectView.MetadataScreen._fundingProjectTitle");
			this._fundingProjectTitle.Location = new System.Drawing.Point(646, 458);
			this._fundingProjectTitle.Name = "_fundingProjectTitle";
			this._fundingProjectTitle.Size = new System.Drawing.Size(309, 29);
			this._fundingProjectTitle.TabIndex = 6;
			// 
			// _labelProjectBasics
			// 
			this._labelProjectBasics.AutoSize = true;
			this._labelProjectBasics.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelProjectBasics, null);
			this.locExtender.SetLocalizationComment(this._labelProjectBasics, null);
			this.locExtender.SetLocalizingId(this._labelProjectBasics, "ProjectView.MetadataScreen._labelProjectBasics");
			this._labelProjectBasics.Location = new System.Drawing.Point(112, 16);
			this._labelProjectBasics.Name = "_labelProjectBasics";
			this._labelProjectBasics.Size = new System.Drawing.Size(92, 17);
			this._labelProjectBasics.TabIndex = 24;
			this._labelProjectBasics.Text = "Project Basics";
			// 
			// _labelMainLocation
			// 
			this._labelMainLocation.AutoSize = true;
			this._labelMainLocation.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelMainLocation, null);
			this.locExtender.SetLocalizationComment(this._labelMainLocation, null);
			this.locExtender.SetLocalizingId(this._labelMainLocation, "ProjectView.MetadataScreen._labelMainLocation");
			this._labelMainLocation.Location = new System.Drawing.Point(646, 16);
			this._labelMainLocation.Name = "_labelMainLocation";
			this._labelMainLocation.Size = new System.Drawing.Size(96, 17);
			this._labelMainLocation.TabIndex = 26;
			this._labelMainLocation.Text = "Main Location";
			// 
			// _labelResponsibilities
			// 
			this._labelResponsibilities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelResponsibilities.AutoSize = true;
			this._labelResponsibilities.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelResponsibilities, null);
			this.locExtender.SetLocalizationComment(this._labelResponsibilities, null);
			this.locExtender.SetLocalizingId(this._labelResponsibilities, "ProjectView.MetadataScreen._labelResponsibilities");
			this._labelResponsibilities.Location = new System.Drawing.Point(535, 218);
			this._labelResponsibilities.Name = "_labelResponsibilities";
			this._labelResponsibilities.Size = new System.Drawing.Size(105, 17);
			this._labelResponsibilities.TabIndex = 27;
			this._labelResponsibilities.Text = "Responsibilities";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this._labelsubOrSimilar, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this._labelAddress, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(577, 44);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(63, 54);
			this.tableLayoutPanel2.TabIndex = 28;
			// 
			// _labelsubOrSimilar
			// 
			this._labelsubOrSimilar.AutoSize = true;
			this._labelsubOrSimilar.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelsubOrSimilar, null);
			this.locExtender.SetLocalizationComment(this._labelsubOrSimilar, null);
			this.locExtender.SetLocalizingId(this._labelsubOrSimilar, "ProjectView.MetadataScreen._labelsubOrSimilar");
			this._labelsubOrSimilar.Location = new System.Drawing.Point(6, 17);
			this._labelsubOrSimilar.Name = "_labelsubOrSimilar";
			this._labelsubOrSimilar.Size = new System.Drawing.Size(54, 37);
			this._labelsubOrSimilar.TabIndex = 33;
			this._labelsubOrSimilar.Text = "or similar";
			this._labelsubOrSimilar.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// _labelAddress
			// 
			this._labelAddress.AutoSize = true;
			this._labelAddress.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelAddress.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelAddress, null);
			this.locExtender.SetLocalizationComment(this._labelAddress, null);
			this.locExtender.SetLocalizingId(this._labelAddress, "ProjectView.MetadataScreen._labelAddress");
			this._labelAddress.Location = new System.Drawing.Point(3, 0);
			this._labelAddress.Name = "_labelAddress";
			this._labelAddress.Size = new System.Drawing.Size(57, 17);
			this._labelAddress.TabIndex = 6;
			this._labelAddress.Text = "A&ddress";
			this._labelAddress.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this._labelsubLikeStateOrProvince, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this._labelRegion, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(520, 104);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(120, 33);
			this.tableLayoutPanel3.TabIndex = 29;
			// 
			// _labelsubLikeStateOrProvince
			// 
			this._labelsubLikeStateOrProvince.AutoSize = true;
			this._labelsubLikeStateOrProvince.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelsubLikeStateOrProvince, null);
			this.locExtender.SetLocalizationComment(this._labelsubLikeStateOrProvince, null);
			this.locExtender.SetLocalizingId(this._labelsubLikeStateOrProvince, "ProjectView.MetadataScreen._labelsubLikeStateOrProvince");
			this._labelsubLikeStateOrProvince.Location = new System.Drawing.Point(3, 17);
			this._labelsubLikeStateOrProvince.Name = "_labelsubLikeStateOrProvince";
			this._labelsubLikeStateOrProvince.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
			this._labelsubLikeStateOrProvince.Size = new System.Drawing.Size(114, 16);
			this._labelsubLikeStateOrProvince.TabIndex = 33;
			this._labelsubLikeStateOrProvince.Text = "like state or province";
			this._labelsubLikeStateOrProvince.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// _labelRegion
			// 
			this._labelRegion.AutoSize = true;
			this._labelRegion.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelRegion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelRegion, null);
			this.locExtender.SetLocalizationComment(this._labelRegion, null);
			this.locExtender.SetLocalizingId(this._labelRegion, "ProjectView.MetadataScreen._labelRegion");
			this._labelRegion.Location = new System.Drawing.Point(66, 0);
			this._labelRegion.Name = "_labelRegion";
			this._labelRegion.Size = new System.Drawing.Size(51, 17);
			this._labelRegion.TabIndex = 8;
			this._labelRegion.Text = "Re&gion";
			this._labelRegion.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel5.ColumnCount = 1;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Controls.Add(this._labelsubPerson1, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this._labelContact, 0, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(45, 458);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 2;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.Size = new System.Drawing.Size(61, 30);
			this.tableLayoutPanel5.TabIndex = 31;
			// 
			// _labelsubPerson1
			// 
			this._labelsubPerson1.AutoSize = true;
			this._labelsubPerson1.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelsubPerson1, null);
			this.locExtender.SetLocalizationComment(this._labelsubPerson1, null);
			this.locExtender.SetLocalizingId(this._labelsubPerson1, "ProjectView.MetadataScreen._labelsubPerson1");
			this._labelsubPerson1.Location = new System.Drawing.Point(15, 17);
			this._labelsubPerson1.Name = "_labelsubPerson1";
			this._labelsubPerson1.Size = new System.Drawing.Size(43, 13);
			this._labelsubPerson1.TabIndex = 32;
			this._labelsubPerson1.Text = "person";
			this._labelsubPerson1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// _labelContact
			// 
			this._labelContact.AutoSize = true;
			this._labelContact.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelContact.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelContact, null);
			this.locExtender.SetLocalizationComment(this._labelContact, null);
			this.locExtender.SetLocalizingId(this._labelContact, "ProjectView.MetadataScreen._labelContact");
			this._labelContact.Location = new System.Drawing.Point(3, 0);
			this._labelContact.Name = "_labelContact";
			this._labelContact.Size = new System.Drawing.Size(55, 17);
			this._labelContact.TabIndex = 14;
			this._labelContact.Text = "Co&ntact";
			this._labelContact.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel6.ColumnCount = 1;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.Controls.Add(this._labelsubTitle, 0, 1);
			this.tableLayoutPanel6.Controls.Add(this._labelFundingProject, 0, 0);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(528, 458);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 2;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.Size = new System.Drawing.Size(112, 30);
			this.tableLayoutPanel6.TabIndex = 33;
			// 
			// _labelsubTitle
			// 
			this._labelsubTitle.AutoSize = true;
			this._labelsubTitle.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelsubTitle, null);
			this.locExtender.SetLocalizationComment(this._labelsubTitle, null);
			this.locExtender.SetLocalizingId(this._labelsubTitle, "ProjectView.MetadataScreen._labelsubTitle");
			this._labelsubTitle.Location = new System.Drawing.Point(82, 17);
			this._labelsubTitle.Name = "_labelsubTitle";
			this._labelsubTitle.Size = new System.Drawing.Size(27, 13);
			this._labelsubTitle.TabIndex = 32;
			this._labelsubTitle.Text = "title";
			this._labelsubTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// _labelFundingProject
			// 
			this._labelFundingProject.AutoSize = true;
			this._labelFundingProject.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelFundingProject.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelFundingProject, null);
			this.locExtender.SetLocalizationComment(this._labelFundingProject, null);
			this.locExtender.SetLocalizingId(this._labelFundingProject, "ProjectView.MetadataScreen._labelFundingProject");
			this._labelFundingProject.Location = new System.Drawing.Point(3, 0);
			this._labelFundingProject.Name = "_labelFundingProject";
			this._labelFundingProject.Size = new System.Drawing.Size(106, 17);
			this._labelFundingProject.TabIndex = 16;
			this._labelFundingProject.Text = "&Funding Project";
			this._labelFundingProject.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tableLayoutPanel9
			// 
			this.tableLayoutPanel9.AutoSize = true;
			this.tableLayoutPanel9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel9.ColumnCount = 1;
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.Controls.Add(this._labelsubDate, 0, 1);
			this.tableLayoutPanel9.Controls.Add(this._labelDateAvailable, 0, 0);
			this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel9.Location = new System.Drawing.Point(547, 494);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.tableLayoutPanel9.RowCount = 2;
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.Size = new System.Drawing.Size(93, 30);
			this.tableLayoutPanel9.TabIndex = 36;
			// 
			// _labelsubDate
			// 
			this._labelsubDate.AutoSize = true;
			this._labelsubDate.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelsubDate, null);
			this.locExtender.SetLocalizationComment(this._labelsubDate, null);
			this.locExtender.SetLocalizingId(this._labelsubDate, "ProjectView.MetadataScreen._labelsubDate");
			this._labelsubDate.Location = new System.Drawing.Point(60, 17);
			this._labelsubDate.Name = "_labelsubDate";
			this._labelsubDate.Size = new System.Drawing.Size(30, 13);
			this._labelsubDate.TabIndex = 37;
			this._labelsubDate.Text = "date";
			this._labelsubDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// _labelDateAvailable
			// 
			this._labelDateAvailable.AutoSize = true;
			this._labelDateAvailable.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelDateAvailable.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelDateAvailable, null);
			this.locExtender.SetLocalizationComment(this._labelDateAvailable, null);
			this.locExtender.SetLocalizingId(this._labelDateAvailable, "ProjectView.MetadataScreen._labelDateAvailable");
			this._labelDateAvailable.Location = new System.Drawing.Point(3, 0);
			this._labelDateAvailable.Name = "_labelDateAvailable";
			this._labelDateAvailable.Size = new System.Drawing.Size(87, 17);
			this._labelDateAvailable.TabIndex = 18;
			this._labelDateAvailable.Text = "Avai&lable On";
			this._labelDateAvailable.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.AutoSize = true;
			this.tableLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel7.ColumnCount = 1;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel7.Controls.Add(this._labelRightsHolder, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this._labelsubPerson2, 0, 1);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(541, 530);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 2;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.Size = new System.Drawing.Size(99, 30);
			this.tableLayoutPanel7.TabIndex = 34;
			// 
			// _labelRightsHolder
			// 
			this._labelRightsHolder.AutoSize = true;
			this._labelRightsHolder.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelRightsHolder.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelRightsHolder, null);
			this.locExtender.SetLocalizationComment(this._labelRightsHolder, null);
			this.locExtender.SetLocalizingId(this._labelRightsHolder, "ProjectView.MetadataScreen._labelRightsHolder");
			this._labelRightsHolder.Location = new System.Drawing.Point(3, 0);
			this._labelRightsHolder.Name = "_labelRightsHolder";
			this._labelRightsHolder.Size = new System.Drawing.Size(93, 17);
			this._labelRightsHolder.TabIndex = 20;
			this._labelRightsHolder.Text = "R&ights Holder";
			this._labelRightsHolder.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// _labelsubPerson2
			// 
			this._labelsubPerson2.AutoSize = true;
			this._labelsubPerson2.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelsubPerson2, null);
			this.locExtender.SetLocalizationComment(this._labelsubPerson2, null);
			this.locExtender.SetLocalizingId(this._labelsubPerson2, "ProjectView.MetadataScreen._labelsubPerson2");
			this._labelsubPerson2.Location = new System.Drawing.Point(53, 17);
			this._labelsubPerson2.Name = "_labelsubPerson2";
			this._labelsubPerson2.Size = new System.Drawing.Size(43, 13);
			this._labelsubPerson2.TabIndex = 37;
			this._labelsubPerson2.Text = "person";
			this._labelsubPerson2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.AutoSize = true;
			this.tableLayoutPanel8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel8.ColumnCount = 1;
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.Controls.Add(this._labelsubPerson3, 0, 1);
			this.tableLayoutPanel8.Controls.Add(this._labelDepositor, 0, 0);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel8.Location = new System.Drawing.Point(565, 566);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 2;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.Size = new System.Drawing.Size(75, 39);
			this.tableLayoutPanel8.TabIndex = 35;
			// 
			// _labelsubPerson3
			// 
			this._labelsubPerson3.AutoSize = true;
			this._labelsubPerson3.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelsubPerson3, null);
			this.locExtender.SetLocalizationComment(this._labelsubPerson3, null);
			this.locExtender.SetLocalizingId(this._labelsubPerson3, "ProjectView.MetadataScreen._labelsubPerson3");
			this._labelsubPerson3.Location = new System.Drawing.Point(29, 17);
			this._labelsubPerson3.Name = "_labelsubPerson3";
			this._labelsubPerson3.Size = new System.Drawing.Size(43, 22);
			this._labelsubPerson3.TabIndex = 37;
			this._labelsubPerson3.Text = "person";
			this._labelsubPerson3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// _labelDepositor
			// 
			this._labelDepositor.AutoSize = true;
			this._labelDepositor.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelDepositor.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelDepositor, null);
			this.locExtender.SetLocalizationComment(this._labelDepositor, null);
			this.locExtender.SetLocalizingId(this._labelDepositor, "ProjectView.MetadataScreen._labelDepositor");
			this._labelDepositor.Location = new System.Drawing.Point(3, 0);
			this._labelDepositor.Name = "_labelDepositor";
			this._labelDepositor.Size = new System.Drawing.Size(69, 17);
			this._labelDepositor.TabIndex = 22;
			this._labelDepositor.Text = "D&epositor";
			this._labelDepositor.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tableLayoutPanel10
			// 
			this.tableLayoutPanel10.AutoSize = true;
			this.tableLayoutPanel10.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel10.ColumnCount = 1;
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel10.Controls.Add(this._labelsubLong, 0, 1);
			this.tableLayoutPanel10.Controls.Add(this._labelProjectTitle, 0, 0);
			this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel10.Location = new System.Drawing.Point(64, 44);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 2;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel10.Size = new System.Drawing.Size(42, 54);
			this.tableLayoutPanel10.TabIndex = 38;
			// 
			// _labelsubLong
			// 
			this._labelsubLong.AutoSize = true;
			this._labelsubLong.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelsubLong.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelsubLong, null);
			this.locExtender.SetLocalizationComment(this._labelsubLong, null);
			this.locExtender.SetLocalizationPriority(this._labelsubLong, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._labelsubLong, "ProjectMetadataScreen._labelsubLong");
			this._labelsubLong.Location = new System.Drawing.Point(8, 17);
			this._labelsubLong.Name = "_labelsubLong";
			this._labelsubLong.Size = new System.Drawing.Size(31, 37);
			this._labelsubLong.TabIndex = 37;
			this._labelsubLong.Text = "long";
			this._labelsubLong.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// _labelProjectTitle
			// 
			this._labelProjectTitle.AutoSize = true;
			this._labelProjectTitle.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelProjectTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelProjectTitle, null);
			this.locExtender.SetLocalizationComment(this._labelProjectTitle, null);
			this.locExtender.SetLocalizationPriority(this._labelProjectTitle, L10NSharp.LocalizationPriority.High);
			this.locExtender.SetLocalizingId(this._labelProjectTitle, "ProjectView.MetadataScreen._labelProjectTitle");
			this._labelProjectTitle.Location = new System.Drawing.Point(3, 0);
			this._labelProjectTitle.Name = "_labelProjectTitle";
			this._labelProjectTitle.Size = new System.Drawing.Size(36, 17);
			this._labelProjectTitle.TabIndex = 0;
			this._labelProjectTitle.Text = "&Title";
			this._labelProjectTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// _country
			// 
			this._country.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._country.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._country.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._country, null);
			this.locExtender.SetLocalizationComment(this._country, null);
			this.locExtender.SetLocalizationPriority(this._country, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._country, "ProjectView.MetadataScreen._comboCountry");
			this._country.Location = new System.Drawing.Point(646, 143);
			this._country.Name = "_country";
			this._country.Size = new System.Drawing.Size(309, 29);
			this._country.TabIndex = 1;
			// 
			// _continent
			// 
			this._continent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._continent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._continent.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._continent.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._continent, null);
			this.locExtender.SetLocalizationComment(this._continent, null);
			this.locExtender.SetLocalizationPriority(this._continent, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._continent, "ProjectView.MetadataScreen._comboContinent");
			this._continent.Location = new System.Drawing.Point(646, 178);
			this._continent.MaximumSize = new System.Drawing.Size(254, 0);
			this._continent.Name = "_continent";
			this._continent.Size = new System.Drawing.Size(254, 29);
			this._continent.TabIndex = 2;
			// 
			// _labelCountry
			// 
			this._labelCountry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._labelCountry.AutoSize = true;
			this._labelCountry.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelCountry, null);
			this.locExtender.SetLocalizationComment(this._labelCountry, null);
			this.locExtender.SetLocalizingId(this._labelCountry, "ProjectView.MetadataScreen._labelCountry");
			this._labelCountry.Location = new System.Drawing.Point(582, 140);
			this._labelCountry.Name = "_labelCountry";
			this._labelCountry.Size = new System.Drawing.Size(58, 17);
			this._labelCountry.TabIndex = 9;
			this._labelCountry.Text = "Countr&y";
			this._labelCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _contactPerson
			// 
			this._contactPerson.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._contactPerson, null);
			this.locExtender.SetLocalizationComment(this._contactPerson, null);
			this.locExtender.SetLocalizationPriority(this._contactPerson, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._contactPerson, "ProjectView.MetadataScreen._contactPerson");
			this._contactPerson.Location = new System.Drawing.Point(112, 458);
			this._contactPerson.Name = "_contactPerson";
			this._contactPerson.Size = new System.Drawing.Size(377, 29);
			this._contactPerson.TabIndex = 2;
			// 
			// _description
			// 
			this._description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._description.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._description, null);
			this.locExtender.SetLocalizationComment(this._description, null);
			this.locExtender.SetLocalizationPriority(this._description, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._description, "ProjectView.MetadataScreen._description");
			this._description.Location = new System.Drawing.Point(112, 178);
			this._description.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this._description.Multiline = true;
			this._description.Name = "_description";
			this._tableLayout.SetRowSpan(this._description, 3);
			this._description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._description.Size = new System.Drawing.Size(377, 277);
			this._description.TabIndex = 1;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this._labelsubOfTheProject, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this._labelDescription, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(21, 178);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this._tableLayout.SetRowSpan(this.tableLayoutPanel4, 2);
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(85, 37);
			this.tableLayoutPanel4.TabIndex = 30;
			// 
			// _labelsubOfTheProject
			// 
			this._labelsubOfTheProject.AutoSize = true;
			this._labelsubOfTheProject.Dock = System.Windows.Forms.DockStyle.Right;
			this.locExtender.SetLocalizableToolTip(this._labelsubOfTheProject, null);
			this.locExtender.SetLocalizationComment(this._labelsubOfTheProject, null);
			this.locExtender.SetLocalizingId(this._labelsubOfTheProject, "ProjectView.MetadataScreen._labelsubOfTheProject");
			this._labelsubOfTheProject.Location = new System.Drawing.Point(5, 17);
			this._labelsubOfTheProject.Name = "_labelsubOfTheProject";
			this._labelsubOfTheProject.Size = new System.Drawing.Size(77, 20);
			this._labelsubOfTheProject.TabIndex = 32;
			this._labelsubOfTheProject.Text = "of the project";
			this._labelsubOfTheProject.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// _labelDescription
			// 
			this._labelDescription.AutoSize = true;
			this._labelDescription.Dock = System.Windows.Forms.DockStyle.Right;
			this._labelDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelDescription, null);
			this.locExtender.SetLocalizationComment(this._labelDescription, null);
			this.locExtender.SetLocalizingId(this._labelDescription, "ProjectView.MetadataScreen._labelDescription");
			this._labelDescription.Location = new System.Drawing.Point(3, 0);
			this._labelDescription.Name = "_labelDescription";
			this._labelDescription.Size = new System.Drawing.Size(79, 17);
			this._labelDescription.TabIndex = 3;
			this._labelDescription.Text = "Desc&ription";
			this._labelDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = null;
			// 
			// _errorProvider
			// 
			this._errorProvider.ContainerControl = this;
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
			this.Size = new System.Drawing.Size(975, 625);
			this.VisibleChanged += new System.EventHandler(this.ProjectMetadataScreen_VisibleChanged);
			this.LostFocus += new System.EventHandler(this.ProjectMetadataScreen_LostFocus);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.tableLayoutPanel10.ResumeLayout(false);
			this.tableLayoutPanel10.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Label _labelProjectTitle;
		private System.Windows.Forms.Label _labelFundingProject;
		private System.Windows.Forms.Label _labelAddress;
		private System.Windows.Forms.Label _labelContact;
		private System.Windows.Forms.TextBox _projectTitle;
		private System.Windows.Forms.TextBox _contactPerson;
		private System.Windows.Forms.TextBox _location;
		private System.Windows.Forms.TextBox _description;
		private System.Windows.Forms.TextBox _fundingProjectTitle;
		private System.Windows.Forms.ComboBox _country;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label _labelSelectedContentLanguage;
		private System.Windows.Forms.LinkLabel _linkSelectContentLanguage;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.Label _labelRegion;
		private System.Windows.Forms.TextBox _region;
		private System.Windows.Forms.Label _labelDateAvailable;
		private System.Windows.Forms.Label _labelRightsHolder;
		private System.Windows.Forms.TextBox _rightsHolder;
		private System.Windows.Forms.Label _labelDepositor;
		private System.Windows.Forms.TextBox _depositor;
		private LowLevelControls.DatePicker _dateAvailable;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.LinkLabel _linkHelp;
		private System.Windows.Forms.Label _labelProjectBasics;
		private System.Windows.Forms.LinkLabel _linkSelectWorkingLanguage;
		private System.Windows.Forms.Label _labelSelectedWorkingLanguage;
		private System.Windows.Forms.Label _labelMainLocation;
		private System.Windows.Forms.Label _labelResponsibilities;
		private System.Windows.Forms.Label _labelsubTitle;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label _labelsubPerson1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.Label _labelsubDate;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.Label _labelsubPerson2;
		private System.Windows.Forms.Label _labelsubPerson3;
		private System.Windows.Forms.Label _labelsubLong;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
		private System.Windows.Forms.Label _labelsubOrSimilar;
		private System.Windows.Forms.Label _labelsubLikeStateOrProvince;
		private System.Windows.Forms.ComboBox _continent;
		private System.Windows.Forms.Label _labelContinent;
		private System.Windows.Forms.Label _labelCountry;
		private System.Windows.Forms.Label _labelsubOfTheProject;
		private System.Windows.Forms.Label _labelDescription;
		private System.Windows.Forms.LinkLabel _linkSelectFontForWorkingLanguage;
		private System.Windows.Forms.ErrorProvider _errorProvider;
	}
}
