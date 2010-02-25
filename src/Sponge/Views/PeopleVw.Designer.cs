using SIL.Sponge.Controls;

namespace SIL.Sponge
{
	partial class PeopleVw
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
			this.lpPeople = new SIL.Sponge.Controls.ListPanel();
			this.tabPeople = new System.Windows.Forms.TabControl();
			this.tpgAbout = new System.Windows.Forms.TabPage();
			this.tblLayout = new System.Windows.Forms.TableLayoutPanel();
			this.pnlLeftSide = new System.Windows.Forms.Panel();
			this.lblPimaryOccupation = new System.Windows.Forms.Label();
			this.m_primaryOccupation = new System.Windows.Forms.TextBox();
			this.lblEducation = new System.Windows.Forms.Label();
			this.m_education = new System.Windows.Forms.TextBox();
			this.lblFullName = new System.Windows.Forms.Label();
			this.m_fullName = new System.Windows.Forms.TextBox();
			this.uhbOtherLanguages = new SIL.Sponge.Controls.UnderlinedHdgBox();
			this.m_languageMother4 = new SIL.Sponge.Controls.ParentButton();
			this.m_languageMother3 = new SIL.Sponge.Controls.ParentButton();
			this.m_languageMother2 = new SIL.Sponge.Controls.ParentButton();
			this.m_languageMother1 = new SIL.Sponge.Controls.ParentButton();
			this.m_language4 = new System.Windows.Forms.TextBox();
			this.m_languageFather4 = new SIL.Sponge.Controls.ParentButton();
			this.m_language3 = new System.Windows.Forms.TextBox();
			this.m_languageFather3 = new SIL.Sponge.Controls.ParentButton();
			this.m_language2 = new System.Windows.Forms.TextBox();
			this.m_languageFather2 = new SIL.Sponge.Controls.ParentButton();
			this.m_language1 = new System.Windows.Forms.TextBox();
			this.m_languageFather1 = new SIL.Sponge.Controls.ParentButton();
			this.uhbPrimaryLanguage = new SIL.Sponge.Controls.UnderlinedHdgBox();
			this.m_languageMother0 = new SIL.Sponge.Controls.ParentButton();
			this.m_language0 = new System.Windows.Forms.TextBox();
			this.m_learnedIn = new System.Windows.Forms.TextBox();
			this.lblLearnedIn = new System.Windows.Forms.Label();
			this.m_languageFather0 = new SIL.Sponge.Controls.ParentButton();
			this.pnlRightSide = new System.Windows.Forms.Panel();
			this.m_notes = new System.Windows.Forms.TextBox();
			this.lblNotes = new System.Windows.Forms.Label();
			this.m_picture = new System.Windows.Forms.PictureBox();
			this.lblBirthYear = new System.Windows.Forms.Label();
			this.m_contact = new System.Windows.Forms.TextBox();
			this.m_gender = new System.Windows.Forms.ComboBox();
			this.lblContact = new System.Windows.Forms.Label();
			this.m_birthYear = new System.Windows.Forms.TextBox();
			this.lblGender = new System.Windows.Forms.Label();
			this.tpgContributors = new System.Windows.Forms.TabPage();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitRightSide.Panel1.SuspendLayout();
			this.splitRightSide.SuspendLayout();
			this.tabPeople.SuspendLayout();
			this.tpgAbout.SuspendLayout();
			this.tblLayout.SuspendLayout();
			this.pnlLeftSide.SuspendLayout();
			this.uhbOtherLanguages.SuspendLayout();
			this.uhbPrimaryLanguage.SuspendLayout();
			this.pnlRightSide.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_picture)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.lpPeople);
			this.splitOuter.Panel1MinSize = 165;
			this.splitOuter.Size = new System.Drawing.Size(709, 432);
			this.splitOuter.SplitterDistance = 165;
			this.splitOuter.TabStop = false;
			// 
			// splitRightSide
			// 
			// 
			// splitRightSide.Panel1
			// 
			this.splitRightSide.Panel1.Controls.Add(this.tabPeople);
			this.splitRightSide.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
			this.splitRightSide.Panel2Collapsed = true;
			this.splitRightSide.Size = new System.Drawing.Size(540, 432);
			// 
			// lpPeople
			// 
			this.lpPeople.CurrentItem = null;
			this.lpPeople.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lpPeople.Items = new object[0];
			// 
			// 
			// 
			this.lpPeople.ListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lpPeople.ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lpPeople.ListView.FullRowSelect = true;
			this.lpPeople.ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lpPeople.ListView.HideSelection = false;
			this.lpPeople.ListView.Location = new System.Drawing.Point(2, 31);
			this.lpPeople.ListView.Name = "lvItems";
			this.lpPeople.ListView.Size = new System.Drawing.Size(159, 984);
			this.lpPeople.ListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lpPeople.ListView.TabIndex = 0;
			this.lpPeople.ListView.UseCompatibleStateImageBehavior = false;
			this.lpPeople.ListView.View = System.Windows.Forms.View.Details;
			this.locExtender.SetLocalizableToolTip(this.lpPeople, null);
			this.locExtender.SetLocalizationComment(this.lpPeople, null);
			this.locExtender.SetLocalizingId(this.lpPeople, "PeopleVw.ListPanel");
			this.lpPeople.Location = new System.Drawing.Point(0, 0);
			this.lpPeople.MinimumSize = new System.Drawing.Size(165, 0);
			this.lpPeople.Name = "lpPeople";
			this.lpPeople.Size = new System.Drawing.Size(165, 432);
			this.lpPeople.TabIndex = 0;
			this.lpPeople.Text = "People";
			this.lpPeople.BeforeItemsDeleted += new SIL.Sponge.Controls.ListPanel.BeforeItemsDeletedHandler(this.lpPeople_BeforeItemsDeleted);
			this.lpPeople.SelectedItemChanged += new SIL.Sponge.Controls.ListPanel.SelectedItemChangedHandler(this.lpPeople_SelectedItemChanged);
			this.lpPeople.AfterItemsDeleted += new SIL.Sponge.Controls.ListPanel.AfterItemsDeletedHandler(this.lpPeople_AfterItemsDeleted);
			this.lpPeople.NewButtonClicked += new SIL.Sponge.Controls.ListPanel.NewButtonClickedHandler(this.lpPeople_NewButtonClicked);
			// 
			// tabPeople
			// 
			this.tabPeople.Controls.Add(this.tpgAbout);
			this.tabPeople.Controls.Add(this.tpgContributors);
			this.tabPeople.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabPeople.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabPeople.ItemSize = new System.Drawing.Size(65, 22);
			this.tabPeople.Location = new System.Drawing.Point(0, 3);
			this.tabPeople.Name = "tabPeople";
			this.tabPeople.SelectedIndex = 0;
			this.tabPeople.Size = new System.Drawing.Size(540, 429);
			this.tabPeople.TabIndex = 0;
			this.tabPeople.SizeChanged += new System.EventHandler(this.tabPeople_SizeChanged);
			// 
			// tpgAbout
			// 
			this.tpgAbout.Controls.Add(this.tblLayout);
			this.locExtender.SetLocalizableToolTip(this.tpgAbout, null);
			this.locExtender.SetLocalizationComment(this.tpgAbout, null);
			this.locExtender.SetLocalizingId(this.tpgAbout, "PeopleVw.tpgAbout");
			this.tpgAbout.Location = new System.Drawing.Point(4, 26);
			this.tpgAbout.Name = "tpgAbout";
			this.tpgAbout.Padding = new System.Windows.Forms.Padding(3);
			this.tpgAbout.Size = new System.Drawing.Size(532, 399);
			this.tpgAbout.TabIndex = 0;
			this.tpgAbout.Text = "About";
			this.tpgAbout.ToolTipText = "Description";
			this.tpgAbout.UseVisualStyleBackColor = true;
			// 
			// tblLayout
			// 
			this.tblLayout.ColumnCount = 2;
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
			this.tblLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tblLayout.Controls.Add(this.pnlLeftSide, 0, 0);
			this.tblLayout.Controls.Add(this.pnlRightSide, 1, 0);
			this.tblLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tblLayout.Location = new System.Drawing.Point(3, 3);
			this.tblLayout.Name = "tblLayout";
			this.tblLayout.RowCount = 1;
			this.tblLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblLayout.Size = new System.Drawing.Size(526, 393);
			this.tblLayout.TabIndex = 22;
			// 
			// pnlLeftSide
			// 
			this.pnlLeftSide.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlLeftSide.Controls.Add(this.lblPimaryOccupation);
			this.pnlLeftSide.Controls.Add(this.m_primaryOccupation);
			this.pnlLeftSide.Controls.Add(this.lblEducation);
			this.pnlLeftSide.Controls.Add(this.m_education);
			this.pnlLeftSide.Controls.Add(this.lblFullName);
			this.pnlLeftSide.Controls.Add(this.m_fullName);
			this.pnlLeftSide.Controls.Add(this.uhbOtherLanguages);
			this.pnlLeftSide.Controls.Add(this.uhbPrimaryLanguage);
			this.pnlLeftSide.Location = new System.Drawing.Point(0, 0);
			this.pnlLeftSide.Margin = new System.Windows.Forms.Padding(0);
			this.pnlLeftSide.Name = "pnlLeftSide";
			this.pnlLeftSide.Size = new System.Drawing.Size(289, 396);
			this.pnlLeftSide.TabIndex = 0;
			// 
			// lblPimaryOccupation
			// 
			this.lblPimaryOccupation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblPimaryOccupation, null);
			this.locExtender.SetLocalizationComment(this.lblPimaryOccupation, null);
			this.locExtender.SetLocalizingId(this.lblPimaryOccupation, "PeopleVw.lblPimaryOccupation");
			this.lblPimaryOccupation.Location = new System.Drawing.Point(8, 345);
			this.lblPimaryOccupation.Name = "lblPimaryOccupation";
			this.lblPimaryOccupation.Size = new System.Drawing.Size(112, 15);
			this.lblPimaryOccupation.TabIndex = 6;
			this.lblPimaryOccupation.Text = "&Pimary Occupation:";
			// 
			// m_primaryOccupation
			// 
			this.m_primaryOccupation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.m_primaryOccupation, null);
			this.locExtender.SetLocalizationComment(this.m_primaryOccupation, null);
			this.locExtender.SetLocalizationPriority(this.m_primaryOccupation, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_primaryOccupation, "PeopleVw.m_primaryOccupation");
			this.m_primaryOccupation.Location = new System.Drawing.Point(5, 363);
			this.m_primaryOccupation.Name = "m_primaryOccupation";
			this.m_primaryOccupation.Size = new System.Drawing.Size(274, 23);
			this.m_primaryOccupation.TabIndex = 7;
			// 
			// lblEducation
			// 
			this.lblEducation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblEducation, null);
			this.locExtender.SetLocalizationComment(this.lblEducation, null);
			this.locExtender.SetLocalizingId(this.lblEducation, "PeopleVw.lblEducation");
			this.lblEducation.Location = new System.Drawing.Point(8, 294);
			this.lblEducation.Name = "lblEducation";
			this.lblEducation.Size = new System.Drawing.Size(63, 15);
			this.lblEducation.TabIndex = 4;
			this.lblEducation.Text = "&Education:";
			// 
			// m_education
			// 
			this.m_education.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.m_education, null);
			this.locExtender.SetLocalizationComment(this.m_education, null);
			this.locExtender.SetLocalizationPriority(this.m_education, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_education, "PeopleVw.m_education");
			this.m_education.Location = new System.Drawing.Point(5, 312);
			this.m_education.Name = "m_education";
			this.m_education.Size = new System.Drawing.Size(274, 23);
			this.m_education.TabIndex = 5;
			// 
			// lblFullName
			// 
			this.lblFullName.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblFullName, null);
			this.locExtender.SetLocalizationComment(this.lblFullName, null);
			this.locExtender.SetLocalizingId(this.lblFullName, "PeopleVw.lblFullName");
			this.lblFullName.Location = new System.Drawing.Point(8, 7);
			this.lblFullName.Name = "lblFullName";
			this.lblFullName.Size = new System.Drawing.Size(64, 15);
			this.lblFullName.TabIndex = 0;
			this.lblFullName.Text = "&Full Name:";
			// 
			// m_fullName
			// 
			this.m_fullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.m_fullName, null);
			this.locExtender.SetLocalizationComment(this.m_fullName, null);
			this.locExtender.SetLocalizationPriority(this.m_fullName, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_fullName, "PeopleVw.m_fullName");
			this.m_fullName.Location = new System.Drawing.Point(5, 25);
			this.m_fullName.Name = "m_fullName";
			this.m_fullName.Size = new System.Drawing.Size(274, 23);
			this.m_fullName.TabIndex = 1;
			// 
			// uhbOtherLanguages
			// 
			this.uhbOtherLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.uhbOtherLanguages.BackColor = System.Drawing.Color.Transparent;
			this.uhbOtherLanguages.Controls.Add(this.m_languageMother4);
			this.uhbOtherLanguages.Controls.Add(this.m_languageMother3);
			this.uhbOtherLanguages.Controls.Add(this.m_languageMother2);
			this.uhbOtherLanguages.Controls.Add(this.m_languageMother1);
			this.uhbOtherLanguages.Controls.Add(this.m_language4);
			this.uhbOtherLanguages.Controls.Add(this.m_languageFather4);
			this.uhbOtherLanguages.Controls.Add(this.m_language3);
			this.uhbOtherLanguages.Controls.Add(this.m_languageFather3);
			this.uhbOtherLanguages.Controls.Add(this.m_language2);
			this.uhbOtherLanguages.Controls.Add(this.m_languageFather2);
			this.uhbOtherLanguages.Controls.Add(this.m_language1);
			this.uhbOtherLanguages.Controls.Add(this.m_languageFather1);
			this.uhbOtherLanguages.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.uhbOtherLanguages.LineColor = System.Drawing.SystemColors.ControlDark;
			this.uhbOtherLanguages.LineThickness = 1;
			this.locExtender.SetLocalizableToolTip(this.uhbOtherLanguages, null);
			this.locExtender.SetLocalizationComment(this.uhbOtherLanguages, null);
			this.locExtender.SetLocalizingId(this.uhbOtherLanguages, "PeopleVw.lblOtherLanguages");
			this.uhbOtherLanguages.Location = new System.Drawing.Point(5, 146);
			this.uhbOtherLanguages.Name = "uhbOtherLanguages";
			this.uhbOtherLanguages.Size = new System.Drawing.Size(274, 138);
			this.uhbOtherLanguages.TabIndex = 3;
			this.uhbOtherLanguages.Text = "&Other Languages";
			// 
			// m_languageMother4
			// 
			this.m_languageMother4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_languageMother4.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.m_languageMother4, "Mother\'s Language");
			this.locExtender.SetLocalizationComment(this.m_languageMother4, null);
			this.locExtender.SetLocalizationPriority(this.m_languageMother4, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.m_languageMother4, "PeopleVw.m_languageMother4");
			this.m_languageMother4.Location = new System.Drawing.Point(250, 113);
			this.m_languageMother4.Name = "m_languageMother4";
			this.m_languageMother4.ParentType = SIL.Sponge.Model.ParentType.Mother;
			this.m_languageMother4.Selected = false;
			this.m_languageMother4.Size = new System.Drawing.Size(24, 24);
			this.m_languageMother4.TabIndex = 11;
			this.m_languageMother4.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMotherSelectedChanging);
			// 
			// m_languageMother3
			// 
			this.m_languageMother3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_languageMother3.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.m_languageMother3, "Mother\'s Language");
			this.locExtender.SetLocalizationComment(this.m_languageMother3, null);
			this.locExtender.SetLocalizationPriority(this.m_languageMother3, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.m_languageMother3, "PeopleVw.m_languageMother3");
			this.m_languageMother3.Location = new System.Drawing.Point(250, 83);
			this.m_languageMother3.Name = "m_languageMother3";
			this.m_languageMother3.ParentType = SIL.Sponge.Model.ParentType.Mother;
			this.m_languageMother3.Selected = false;
			this.m_languageMother3.Size = new System.Drawing.Size(24, 24);
			this.m_languageMother3.TabIndex = 8;
			this.m_languageMother3.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMotherSelectedChanging);
			// 
			// m_languageMother2
			// 
			this.m_languageMother2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_languageMother2.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.m_languageMother2, "Mother\'s Language");
			this.locExtender.SetLocalizationComment(this.m_languageMother2, null);
			this.locExtender.SetLocalizationPriority(this.m_languageMother2, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.m_languageMother2, "PeopleVw.m_languageMother2");
			this.m_languageMother2.Location = new System.Drawing.Point(250, 53);
			this.m_languageMother2.Name = "m_languageMother2";
			this.m_languageMother2.ParentType = SIL.Sponge.Model.ParentType.Mother;
			this.m_languageMother2.Selected = false;
			this.m_languageMother2.Size = new System.Drawing.Size(24, 24);
			this.m_languageMother2.TabIndex = 5;
			this.m_languageMother2.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMotherSelectedChanging);
			// 
			// m_languageMother1
			// 
			this.m_languageMother1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_languageMother1.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.m_languageMother1, "Mother\'s Language");
			this.locExtender.SetLocalizationComment(this.m_languageMother1, null);
			this.locExtender.SetLocalizationPriority(this.m_languageMother1, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.m_languageMother1, "PeopleVw.m_languageMother1");
			this.m_languageMother1.Location = new System.Drawing.Point(250, 23);
			this.m_languageMother1.Name = "m_languageMother1";
			this.m_languageMother1.ParentType = SIL.Sponge.Model.ParentType.Mother;
			this.m_languageMother1.Selected = false;
			this.m_languageMother1.Size = new System.Drawing.Size(24, 24);
			this.m_languageMother1.TabIndex = 2;
			this.m_languageMother1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMotherSelectedChanging);
			// 
			// m_language4
			// 
			this.m_language4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_language4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.m_language4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this.m_language4, null);
			this.locExtender.SetLocalizationComment(this.m_language4, null);
			this.locExtender.SetLocalizationPriority(this.m_language4, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_language4, "PeopleVw.m_otherLanguage3");
			this.m_language4.Location = new System.Drawing.Point(0, 115);
			this.m_language4.Name = "m_language4";
			this.m_language4.Size = new System.Drawing.Size(216, 23);
			this.m_language4.TabIndex = 9;
			this.m_language4.Leave += new System.EventHandler(this.HandleLanguageNameLeave);
			this.m_language4.Enter += new System.EventHandler(this.HandleLanguageNameEnter);
			// 
			// m_languageFather4
			// 
			this.m_languageFather4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_languageFather4.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.m_languageFather4, "Father\'s Language");
			this.locExtender.SetLocalizationComment(this.m_languageFather4, null);
			this.locExtender.SetLocalizationPriority(this.m_languageFather4, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.m_languageFather4, "PeopleVw.m_languageFather4");
			this.m_languageFather4.Location = new System.Drawing.Point(222, 113);
			this.m_languageFather4.Name = "m_languageFather4";
			this.m_languageFather4.ParentType = SIL.Sponge.Model.ParentType.Father;
			this.m_languageFather4.Selected = false;
			this.m_languageFather4.Size = new System.Drawing.Size(24, 24);
			this.m_languageFather4.TabIndex = 10;
			this.m_languageFather4.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFatherSelectedChanging);
			// 
			// m_language3
			// 
			this.m_language3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_language3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.m_language3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this.m_language3, null);
			this.locExtender.SetLocalizationComment(this.m_language3, null);
			this.locExtender.SetLocalizationPriority(this.m_language3, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_language3, "PeopleVw.m_otherLanguage2");
			this.m_language3.Location = new System.Drawing.Point(0, 85);
			this.m_language3.Name = "m_language3";
			this.m_language3.Size = new System.Drawing.Size(216, 23);
			this.m_language3.TabIndex = 6;
			this.m_language3.Leave += new System.EventHandler(this.HandleLanguageNameLeave);
			this.m_language3.Enter += new System.EventHandler(this.HandleLanguageNameEnter);
			// 
			// m_languageFather3
			// 
			this.m_languageFather3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_languageFather3.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.m_languageFather3, "Father\'s Language");
			this.locExtender.SetLocalizationComment(this.m_languageFather3, null);
			this.locExtender.SetLocalizationPriority(this.m_languageFather3, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.m_languageFather3, "PeopleVw.m_languageFather3");
			this.m_languageFather3.Location = new System.Drawing.Point(222, 83);
			this.m_languageFather3.Name = "m_languageFather3";
			this.m_languageFather3.ParentType = SIL.Sponge.Model.ParentType.Father;
			this.m_languageFather3.Selected = false;
			this.m_languageFather3.Size = new System.Drawing.Size(24, 24);
			this.m_languageFather3.TabIndex = 7;
			this.m_languageFather3.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFatherSelectedChanging);
			// 
			// m_language2
			// 
			this.m_language2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_language2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.m_language2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this.m_language2, null);
			this.locExtender.SetLocalizationComment(this.m_language2, null);
			this.locExtender.SetLocalizationPriority(this.m_language2, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_language2, "PeopleVw.m_otherLanguage1");
			this.m_language2.Location = new System.Drawing.Point(0, 55);
			this.m_language2.Name = "m_language2";
			this.m_language2.Size = new System.Drawing.Size(216, 23);
			this.m_language2.TabIndex = 3;
			this.m_language2.Leave += new System.EventHandler(this.HandleLanguageNameLeave);
			this.m_language2.Enter += new System.EventHandler(this.HandleLanguageNameEnter);
			// 
			// m_languageFather2
			// 
			this.m_languageFather2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_languageFather2.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.m_languageFather2, "Father\'s Language");
			this.locExtender.SetLocalizationComment(this.m_languageFather2, null);
			this.locExtender.SetLocalizationPriority(this.m_languageFather2, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.m_languageFather2, "PeopleVw.m_languageFather2");
			this.m_languageFather2.Location = new System.Drawing.Point(222, 53);
			this.m_languageFather2.Name = "m_languageFather2";
			this.m_languageFather2.ParentType = SIL.Sponge.Model.ParentType.Father;
			this.m_languageFather2.Selected = false;
			this.m_languageFather2.Size = new System.Drawing.Size(24, 24);
			this.m_languageFather2.TabIndex = 4;
			this.m_languageFather2.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFatherSelectedChanging);
			// 
			// m_language1
			// 
			this.m_language1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_language1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.m_language1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this.m_language1, null);
			this.locExtender.SetLocalizationComment(this.m_language1, null);
			this.locExtender.SetLocalizationPriority(this.m_language1, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_language1, "PeopleVw.m_otherLanguage0");
			this.m_language1.Location = new System.Drawing.Point(0, 25);
			this.m_language1.Name = "m_language1";
			this.m_language1.Size = new System.Drawing.Size(216, 23);
			this.m_language1.TabIndex = 0;
			this.m_language1.Leave += new System.EventHandler(this.HandleLanguageNameLeave);
			this.m_language1.Enter += new System.EventHandler(this.HandleLanguageNameEnter);
			// 
			// m_languageFather1
			// 
			this.m_languageFather1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_languageFather1.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.m_languageFather1, "Father\'s Language");
			this.locExtender.SetLocalizationComment(this.m_languageFather1, null);
			this.locExtender.SetLocalizationPriority(this.m_languageFather1, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.m_languageFather1, "PeopleVw.m_languageFather1");
			this.m_languageFather1.Location = new System.Drawing.Point(222, 23);
			this.m_languageFather1.Name = "m_languageFather1";
			this.m_languageFather1.ParentType = SIL.Sponge.Model.ParentType.Father;
			this.m_languageFather1.Selected = false;
			this.m_languageFather1.Size = new System.Drawing.Size(24, 24);
			this.m_languageFather1.TabIndex = 1;
			this.m_languageFather1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFatherSelectedChanging);
			// 
			// uhbPrimaryLanguage
			// 
			this.uhbPrimaryLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.uhbPrimaryLanguage.BackColor = System.Drawing.Color.Transparent;
			this.uhbPrimaryLanguage.Controls.Add(this.m_languageMother0);
			this.uhbPrimaryLanguage.Controls.Add(this.m_language0);
			this.uhbPrimaryLanguage.Controls.Add(this.m_learnedIn);
			this.uhbPrimaryLanguage.Controls.Add(this.lblLearnedIn);
			this.uhbPrimaryLanguage.Controls.Add(this.m_languageFather0);
			this.uhbPrimaryLanguage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uhbPrimaryLanguage.LineColor = System.Drawing.SystemColors.ControlDark;
			this.uhbPrimaryLanguage.LineThickness = 1;
			this.locExtender.SetLocalizableToolTip(this.uhbPrimaryLanguage, null);
			this.locExtender.SetLocalizationComment(this.uhbPrimaryLanguage, null);
			this.locExtender.SetLocalizingId(this.uhbPrimaryLanguage, "PeopleVw.lblPrimaryLanguage");
			this.uhbPrimaryLanguage.Location = new System.Drawing.Point(5, 58);
			this.uhbPrimaryLanguage.Name = "uhbPrimaryLanguage";
			this.uhbPrimaryLanguage.Size = new System.Drawing.Size(274, 78);
			this.uhbPrimaryLanguage.TabIndex = 2;
			this.uhbPrimaryLanguage.Text = "&Primary Language";
			// 
			// m_languageMother0
			// 
			this.m_languageMother0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_languageMother0.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.m_languageMother0, "Mother\'s Language");
			this.locExtender.SetLocalizationComment(this.m_languageMother0, null);
			this.locExtender.SetLocalizationPriority(this.m_languageMother0, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.m_languageMother0, "PeopleVw.m_languageMother0");
			this.m_languageMother0.Location = new System.Drawing.Point(250, 23);
			this.m_languageMother0.Name = "m_languageMother0";
			this.m_languageMother0.ParentType = SIL.Sponge.Model.ParentType.Mother;
			this.m_languageMother0.Selected = true;
			this.m_languageMother0.Size = new System.Drawing.Size(24, 24);
			this.m_languageMother0.TabIndex = 2;
			this.m_languageMother0.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMotherSelectedChanging);
			// 
			// m_language0
			// 
			this.m_language0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_language0.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.m_language0.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this.m_language0, null);
			this.locExtender.SetLocalizationComment(this.m_language0, null);
			this.locExtender.SetLocalizationPriority(this.m_language0, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_language0, "PeopleVw.m_primaryLanguage");
			this.m_language0.Location = new System.Drawing.Point(0, 25);
			this.m_language0.Name = "m_language0";
			this.m_language0.Size = new System.Drawing.Size(216, 23);
			this.m_language0.TabIndex = 0;
			this.m_language0.Leave += new System.EventHandler(this.HandleLanguageNameLeave);
			this.m_language0.Enter += new System.EventHandler(this.HandleLanguageNameEnter);
			// 
			// m_learnedIn
			// 
			this.m_learnedIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.m_learnedIn, null);
			this.locExtender.SetLocalizationComment(this.m_learnedIn, null);
			this.locExtender.SetLocalizationPriority(this.m_learnedIn, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_learnedIn, "PeopleVw.m_learnedIn");
			this.m_learnedIn.Location = new System.Drawing.Point(82, 55);
			this.m_learnedIn.Name = "m_learnedIn";
			this.m_learnedIn.Size = new System.Drawing.Size(192, 23);
			this.m_learnedIn.TabIndex = 4;
			// 
			// lblLearnedIn
			// 
			this.lblLearnedIn.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblLearnedIn, null);
			this.locExtender.SetLocalizationComment(this.lblLearnedIn, null);
			this.locExtender.SetLocalizingId(this.lblLearnedIn, "PeopleVw.lblLearnedIn");
			this.lblLearnedIn.Location = new System.Drawing.Point(3, 58);
			this.lblLearnedIn.Name = "lblLearnedIn";
			this.lblLearnedIn.Size = new System.Drawing.Size(65, 15);
			this.lblLearnedIn.TabIndex = 3;
			this.lblLearnedIn.Text = "&Learned In:";
			// 
			// m_languageFather0
			// 
			this.m_languageFather0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_languageFather0.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this.m_languageFather0, "Father\'s Language");
			this.locExtender.SetLocalizationComment(this.m_languageFather0, null);
			this.locExtender.SetLocalizationPriority(this.m_languageFather0, SIL.Localize.LocalizationUtils.LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this.m_languageFather0, "PeopleVw.m_languageFather0");
			this.m_languageFather0.Location = new System.Drawing.Point(222, 23);
			this.m_languageFather0.Name = "m_languageFather0";
			this.m_languageFather0.ParentType = SIL.Sponge.Model.ParentType.Father;
			this.m_languageFather0.Selected = true;
			this.m_languageFather0.Size = new System.Drawing.Size(24, 24);
			this.m_languageFather0.TabIndex = 1;
			this.m_languageFather0.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFatherSelectedChanging);
			// 
			// pnlRightSide
			// 
			this.pnlRightSide.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlRightSide.Controls.Add(this.m_notes);
			this.pnlRightSide.Controls.Add(this.lblNotes);
			this.pnlRightSide.Controls.Add(this.m_picture);
			this.pnlRightSide.Controls.Add(this.lblBirthYear);
			this.pnlRightSide.Controls.Add(this.m_contact);
			this.pnlRightSide.Controls.Add(this.m_gender);
			this.pnlRightSide.Controls.Add(this.lblContact);
			this.pnlRightSide.Controls.Add(this.m_birthYear);
			this.pnlRightSide.Controls.Add(this.lblGender);
			this.pnlRightSide.Location = new System.Drawing.Point(292, 3);
			this.pnlRightSide.Name = "pnlRightSide";
			this.pnlRightSide.Size = new System.Drawing.Size(231, 390);
			this.pnlRightSide.TabIndex = 1;
			// 
			// m_notes
			// 
			this.m_notes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.m_notes, null);
			this.locExtender.SetLocalizationComment(this.m_notes, null);
			this.locExtender.SetLocalizationPriority(this.m_notes, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_notes, "PeopleVw.m_notes");
			this.m_notes.Location = new System.Drawing.Point(3, 261);
			this.m_notes.Multiline = true;
			this.m_notes.Name = "m_notes";
			this.m_notes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.m_notes.Size = new System.Drawing.Size(228, 129);
			this.m_notes.TabIndex = 7;
			// 
			// lblNotes
			// 
			this.lblNotes.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblNotes, null);
			this.locExtender.SetLocalizationComment(this.lblNotes, null);
			this.locExtender.SetLocalizingId(this.lblNotes, "PeopleVw.lblNotes");
			this.lblNotes.Location = new System.Drawing.Point(4, 243);
			this.lblNotes.Name = "lblNotes";
			this.lblNotes.Size = new System.Drawing.Size(41, 15);
			this.lblNotes.TabIndex = 6;
			this.lblNotes.Text = "No&tes:";
			// 
			// m_picture
			// 
			this.m_picture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_picture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_picture.Cursor = System.Windows.Forms.Cursors.Hand;
			this.m_picture.Image = global::SIL.Sponge.Properties.Resources.kimidNoPhoto;
			this.m_picture.InitialImage = null;
			this.locExtender.SetLocalizableToolTip(this.m_picture, "Click to Change Picture");
			this.locExtender.SetLocalizationComment(this.m_picture, null);
			this.locExtender.SetLocalizingId(this.m_picture, "PeopleVw.personsPicture");
			this.m_picture.Location = new System.Drawing.Point(101, 0);
			this.m_picture.Name = "m_picture";
			this.m_picture.Size = new System.Drawing.Size(130, 130);
			this.m_picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.m_picture.TabIndex = 2;
			this.m_picture.TabStop = false;
			this.m_picture.MouseLeave += new System.EventHandler(this.HandleMouseEnterLeaveOnPicture);
			this.m_picture.MouseClick += new System.Windows.Forms.MouseEventHandler(this.m_picture_MouseClick);
			this.m_picture.Paint += new System.Windows.Forms.PaintEventHandler(this.m_picture_Paint);
			this.m_picture.MouseEnter += new System.EventHandler(this.HandleMouseEnterLeaveOnPicture);
			// 
			// lblBirthYear
			// 
			this.lblBirthYear.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblBirthYear, null);
			this.locExtender.SetLocalizationComment(this.lblBirthYear, null);
			this.locExtender.SetLocalizingId(this.lblBirthYear, "PeopleVw.lblBirthYear");
			this.lblBirthYear.Location = new System.Drawing.Point(4, 4);
			this.lblBirthYear.Name = "lblBirthYear";
			this.lblBirthYear.Size = new System.Drawing.Size(61, 15);
			this.lblBirthYear.TabIndex = 0;
			this.lblBirthYear.Text = "&Birth Year:";
			// 
			// m_contact
			// 
			this.m_contact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this.m_contact, null);
			this.locExtender.SetLocalizationComment(this.m_contact, null);
			this.locExtender.SetLocalizationPriority(this.m_contact, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_contact, "PeopleVw.m_contact");
			this.m_contact.Location = new System.Drawing.Point(3, 161);
			this.m_contact.Multiline = true;
			this.m_contact.Name = "m_contact";
			this.m_contact.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.m_contact.Size = new System.Drawing.Size(228, 72);
			this.m_contact.TabIndex = 5;
			// 
			// m_gender
			// 
			this.m_gender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_gender.FormattingEnabled = true;
			this.m_gender.Items.AddRange(new object[] {
            "Male",
            "Female"});
			this.locExtender.SetLocalizableToolTip(this.m_gender, null);
			this.locExtender.SetLocalizationComment(this.m_gender, null);
			this.locExtender.SetLocalizingId(this.m_gender, "PeopleVw.m_gender");
			this.m_gender.Location = new System.Drawing.Point(3, 73);
			this.m_gender.Name = "m_gender";
			this.m_gender.Size = new System.Drawing.Size(76, 23);
			this.m_gender.TabIndex = 3;
			// 
			// lblContact
			// 
			this.lblContact.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblContact, null);
			this.locExtender.SetLocalizationComment(this.lblContact, null);
			this.locExtender.SetLocalizingId(this.lblContact, "PeopleVw.lblContact");
			this.lblContact.Location = new System.Drawing.Point(4, 143);
			this.lblContact.Name = "lblContact";
			this.lblContact.Size = new System.Drawing.Size(94, 15);
			this.lblContact.TabIndex = 4;
			this.lblContact.Text = "How to &Contact:";
			// 
			// m_birthYear
			// 
			this.locExtender.SetLocalizableToolTip(this.m_birthYear, null);
			this.locExtender.SetLocalizationComment(this.m_birthYear, null);
			this.locExtender.SetLocalizationPriority(this.m_birthYear, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.m_birthYear, "PeopleVw.m_birthYear");
			this.m_birthYear.Location = new System.Drawing.Point(3, 22);
			this.m_birthYear.Name = "m_birthYear";
			this.m_birthYear.Size = new System.Drawing.Size(76, 23);
			this.m_birthYear.TabIndex = 1;
			// 
			// lblGender
			// 
			this.lblGender.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblGender, null);
			this.locExtender.SetLocalizationComment(this.lblGender, null);
			this.locExtender.SetLocalizationPriority(this.lblGender, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.lblGender, "PeopleVw.lblGender");
			this.lblGender.Location = new System.Drawing.Point(4, 55);
			this.lblGender.Name = "lblGender";
			this.lblGender.Size = new System.Drawing.Size(48, 15);
			this.lblGender.TabIndex = 2;
			this.lblGender.Text = "&Gender:";
			// 
			// tpgContributors
			// 
			this.locExtender.SetLocalizableToolTip(this.tpgContributors, null);
			this.locExtender.SetLocalizationComment(this.tpgContributors, null);
			this.locExtender.SetLocalizingId(this.tpgContributors, "PeopleVw.tpgContributors");
			this.tpgContributors.Location = new System.Drawing.Point(4, 26);
			this.tpgContributors.Name = "tpgContributors";
			this.tpgContributors.Padding = new System.Windows.Forms.Padding(3);
			this.tpgContributors.Size = new System.Drawing.Size(480, 350);
			this.tpgContributors.TabIndex = 1;
			this.tpgContributors.Text = "Contributors && Permissions";
			this.tpgContributors.ToolTipText = "Contributors & Permissions";
			this.tpgContributors.UseVisualStyleBackColor = true;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Views";
			// 
			// PeopleVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.DoubleBuffered = true;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "PeopleVw.BaseSplitVw");
			this.Name = "PeopleVw";
			this.ShowRightBottomPanel = false;
			this.Size = new System.Drawing.Size(709, 432);
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitRightSide.Panel1.ResumeLayout(false);
			this.splitRightSide.ResumeLayout(false);
			this.tabPeople.ResumeLayout(false);
			this.tpgAbout.ResumeLayout(false);
			this.tblLayout.ResumeLayout(false);
			this.pnlLeftSide.ResumeLayout(false);
			this.pnlLeftSide.PerformLayout();
			this.uhbOtherLanguages.ResumeLayout(false);
			this.uhbOtherLanguages.PerformLayout();
			this.uhbPrimaryLanguage.ResumeLayout(false);
			this.uhbPrimaryLanguage.PerformLayout();
			this.pnlRightSide.ResumeLayout(false);
			this.pnlRightSide.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_picture)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox m_birthYear;
		private System.Windows.Forms.TextBox m_learnedIn;
		private System.Windows.Forms.TextBox m_contact;
		private System.Windows.Forms.TextBox m_language0;
		private System.Windows.Forms.TextBox m_language1;
		private System.Windows.Forms.TextBox m_language4;
		private System.Windows.Forms.TextBox m_language3;
		private System.Windows.Forms.TextBox m_language2;
		private System.Windows.Forms.TextBox m_education;
		private System.Windows.Forms.TextBox m_notes;
		private System.Windows.Forms.TextBox m_primaryOccupation;
		private System.Windows.Forms.TextBox m_fullName;
		private System.Windows.Forms.ComboBox m_gender;
		private System.Windows.Forms.PictureBox m_picture;
		private System.Windows.Forms.TabControl tabPeople;
		private System.Windows.Forms.TabPage tpgAbout;
		private System.Windows.Forms.TabPage tpgContributors;
		private System.Windows.Forms.Label lblLearnedIn;
		private System.Windows.Forms.Label lblFullName;
		private System.Windows.Forms.Label lblBirthYear;
		private System.Windows.Forms.Label lblContact;
		private System.Windows.Forms.Label lblGender;
		private System.Windows.Forms.Label lblNotes;
		private System.Windows.Forms.Label lblEducation;
		private System.Windows.Forms.Label lblPimaryOccupation;
		private ParentButton m_languageFather1;
		private ParentButton m_languageFather2;
		private ParentButton m_languageFather3;
		private ParentButton m_languageFather4;
		private ListPanel lpPeople;
		private UnderlinedHdgBox uhbPrimaryLanguage;
		private UnderlinedHdgBox uhbOtherLanguages;
		private ParentButton m_languageFather0;
		private System.Windows.Forms.TableLayoutPanel tblLayout;
		private System.Windows.Forms.Panel pnlLeftSide;
		private System.Windows.Forms.Panel pnlRightSide;
		private SIL.Localize.LocalizationUtils.LocalizationExtender locExtender;
		private ParentButton m_languageMother0;
		private ParentButton m_languageMother4;
		private ParentButton m_languageMother3;
		private ParentButton m_languageMother2;
		private ParentButton m_languageMother1;
	}
}
