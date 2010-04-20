using SIL.Localization;
using SIL.Sponge.Controls;

namespace SIL.Sponge.Views
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
			this.tblAbout = new System.Windows.Forms.TableLayoutPanel();
			this.pnlLeftSide = new System.Windows.Forms.Panel();
			this.lblPimaryOccupation = new System.Windows.Forms.Label();
			this._primaryOccupation = new System.Windows.Forms.TextBox();
			this.lblEducation = new System.Windows.Forms.Label();
			this._education = new System.Windows.Forms.TextBox();
			this.lblFullName = new System.Windows.Forms.Label();
			this._fullName = new System.Windows.Forms.TextBox();
			this.uhbOtherLanguages = new SIL.Sponge.Controls.UnderlinedHdgBox();
			this._languageMother4 = new SIL.Sponge.Controls.ParentButton();
			this._languageMother3 = new SIL.Sponge.Controls.ParentButton();
			this._languageMother2 = new SIL.Sponge.Controls.ParentButton();
			this._languageMother1 = new SIL.Sponge.Controls.ParentButton();
			this._language4 = new System.Windows.Forms.TextBox();
			this._languageFather4 = new SIL.Sponge.Controls.ParentButton();
			this._language3 = new System.Windows.Forms.TextBox();
			this._languageFather3 = new SIL.Sponge.Controls.ParentButton();
			this._language2 = new System.Windows.Forms.TextBox();
			this._languageFather2 = new SIL.Sponge.Controls.ParentButton();
			this._language1 = new System.Windows.Forms.TextBox();
			this._languageFather1 = new SIL.Sponge.Controls.ParentButton();
			this.uhbPrimaryLanguage = new SIL.Sponge.Controls.UnderlinedHdgBox();
			this._languageMother0 = new SIL.Sponge.Controls.ParentButton();
			this._language0 = new System.Windows.Forms.TextBox();
			this._learnedIn = new System.Windows.Forms.TextBox();
			this.lblLearnedIn = new System.Windows.Forms.Label();
			this._languageFather0 = new SIL.Sponge.Controls.ParentButton();
			this.pnlRightSide = new System.Windows.Forms.Panel();
			this._notes = new System.Windows.Forms.TextBox();
			this.lblNotes = new System.Windows.Forms.Label();
			this._picture = new System.Windows.Forms.PictureBox();
			this.m_pictureMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.m_editImageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lblBirthYear = new System.Windows.Forms.Label();
			this._contact = new System.Windows.Forms.TextBox();
			this._gender = new System.Windows.Forms.ComboBox();
			this.lblContact = new System.Windows.Forms.Label();
			this._birthYear = new System.Windows.Forms.TextBox();
			this.lblGender = new System.Windows.Forms.Label();
			this.tpgInformedConsent = new System.Windows.Forms.TabPage();
			this.pnlPermissions = new System.Windows.Forms.Panel();
			this.lstPermissionFiles = new System.Windows.Forms.ListBox();
			this.btnDeletePermissionFile = new System.Windows.Forms.Button();
			this.lblHeading = new System.Windows.Forms.Label();
			this.pnlBrowser = new SilUtils.Controls.SilPanel();
			this.webConsent = new System.Windows.Forms.WebBrowser();
			this.btnAddPermissionFile = new System.Windows.Forms.Button();
			this.locExtender = new LocalizationExtender(this.components);
			this.lblNoPeopleMsg = new System.Windows.Forms.Label();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitRightSide.Panel1.SuspendLayout();
			this.splitRightSide.SuspendLayout();
			this.tabPeople.SuspendLayout();
			this.tpgAbout.SuspendLayout();
			this.tblAbout.SuspendLayout();
			this.pnlLeftSide.SuspendLayout();
			this.uhbOtherLanguages.SuspendLayout();
			this.uhbPrimaryLanguage.SuspendLayout();
			this.pnlRightSide.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._picture)).BeginInit();
			this.m_pictureMenu.SuspendLayout();
			this.tpgInformedConsent.SuspendLayout();
			this.pnlPermissions.SuspendLayout();
			this.pnlBrowser.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.lblNoPeopleMsg);
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
			this.lpPeople.ListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lpPeople.ListView.BackColor = System.Drawing.SystemColors.Window;
			this.lpPeople.ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lpPeople.ListView.FullRowSelect = true;
			this.lpPeople.ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lpPeople.ListView.HideSelection = false;
			this.lpPeople.ListView.Location = new System.Drawing.Point(2, 31);
			this.lpPeople.ListView.Name = "lvItems";
			this.lpPeople.ListView.Size = new System.Drawing.Size(159, 368);
			this.lpPeople.ListView.TabIndex = 0;
			this.lpPeople.ListView.UseCompatibleStateImageBehavior = false;
			this.lpPeople.ListView.View = System.Windows.Forms.View.Details;
			this.locExtender.SetLocalizableToolTip(this.lpPeople, null);
			this.locExtender.SetLocalizationComment(this.lpPeople, null);
			this.locExtender.SetLocalizingId(this.lpPeople, "PeopleVw.ListPanel");
			this.lpPeople.Location = new System.Drawing.Point(0, 0);
			this.lpPeople.MinimumSize = new System.Drawing.Size(165, 0);
			this.lpPeople.Name = "lpPeople";
			this.lpPeople.ReSortWhenItemTextChanges = true;
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
			this.tabPeople.Controls.Add(this.tpgInformedConsent);
			this.tabPeople.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabPeople.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabPeople.ItemSize = new System.Drawing.Size(65, 22);
			this.tabPeople.Location = new System.Drawing.Point(0, 3);
			this.tabPeople.Name = "tabPeople";
			this.tabPeople.SelectedIndex = 0;
			this.tabPeople.Size = new System.Drawing.Size(540, 429);
			this.tabPeople.TabIndex = 0;
			this.tabPeople.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabPeople_Selected);
			this.tabPeople.SizeChanged += new System.EventHandler(this.tabPeople_SizeChanged);
			// 
			// tpgAbout
			// 
			this.tpgAbout.Controls.Add(this.tblAbout);
			this.locExtender.SetLocalizableToolTip(this.tpgAbout, null);
			this.locExtender.SetLocalizationComment(this.tpgAbout, null);
			this.locExtender.SetLocalizingId(this.tpgAbout, "PeopleVw.tpgAbout");
			this.tpgAbout.Location = new System.Drawing.Point(4, 26);
			this.tpgAbout.Name = "tpgAbout";
			this.tpgAbout.Padding = new System.Windows.Forms.Padding(0, 2, 2, 1);
			this.tpgAbout.Size = new System.Drawing.Size(532, 399);
			this.tpgAbout.TabIndex = 0;
			this.tpgAbout.Text = "About";
			this.tpgAbout.ToolTipText = "Description";
			this.tpgAbout.UseVisualStyleBackColor = true;
			// 
			// tblAbout
			// 
			this.tblAbout.ColumnCount = 2;
			this.tblAbout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
			this.tblAbout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tblAbout.Controls.Add(this.pnlLeftSide, 0, 0);
			this.tblAbout.Controls.Add(this.pnlRightSide, 1, 0);
			this.tblAbout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tblAbout.Location = new System.Drawing.Point(0, 2);
			this.tblAbout.Name = "tblAbout";
			this.tblAbout.RowCount = 1;
			this.tblAbout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tblAbout.Size = new System.Drawing.Size(530, 396);
			this.tblAbout.TabIndex = 22;
			// 
			// pnlLeftSide
			// 
			this.pnlLeftSide.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlLeftSide.Controls.Add(this.lblPimaryOccupation);
			this.pnlLeftSide.Controls.Add(this._primaryOccupation);
			this.pnlLeftSide.Controls.Add(this.lblEducation);
			this.pnlLeftSide.Controls.Add(this._education);
			this.pnlLeftSide.Controls.Add(this.lblFullName);
			this.pnlLeftSide.Controls.Add(this._fullName);
			this.pnlLeftSide.Controls.Add(this.uhbOtherLanguages);
			this.pnlLeftSide.Controls.Add(this.uhbPrimaryLanguage);
			this.pnlLeftSide.Location = new System.Drawing.Point(0, 0);
			this.pnlLeftSide.Margin = new System.Windows.Forms.Padding(0);
			this.pnlLeftSide.Name = "pnlLeftSide";
			this.pnlLeftSide.Size = new System.Drawing.Size(291, 396);
			this.pnlLeftSide.TabIndex = 0;
			// 
			// lblPimaryOccupation
			// 
			this.lblPimaryOccupation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblPimaryOccupation, null);
			this.locExtender.SetLocalizationComment(this.lblPimaryOccupation, null);
			this.locExtender.SetLocalizingId(this.lblPimaryOccupation, "PeopleVw.lblPimaryOccupation");
			this.lblPimaryOccupation.Location = new System.Drawing.Point(9, 345);
			this.lblPimaryOccupation.Name = "lblPimaryOccupation";
			this.lblPimaryOccupation.Size = new System.Drawing.Size(109, 15);
			this.lblPimaryOccupation.TabIndex = 6;
			this.lblPimaryOccupation.Text = "&Pimary Occupation";
			// 
			// _primaryOccupation
			// 
			this._primaryOccupation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._primaryOccupation, null);
			this.locExtender.SetLocalizationComment(this._primaryOccupation, null);
			this.locExtender.SetLocalizationPriority(this._primaryOccupation, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._primaryOccupation, "PeopleVw._primaryOccupation");
			this._primaryOccupation.Location = new System.Drawing.Point(9, 363);
			this._primaryOccupation.Name = "_primaryOccupation";
			this._primaryOccupation.Size = new System.Drawing.Size(276, 23);
			this._primaryOccupation.TabIndex = 7;
			// 
			// lblEducation
			// 
			this.lblEducation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblEducation, null);
			this.locExtender.SetLocalizationComment(this.lblEducation, null);
			this.locExtender.SetLocalizingId(this.lblEducation, "PeopleVw.lblEducation");
			this.lblEducation.Location = new System.Drawing.Point(9, 294);
			this.lblEducation.Name = "lblEducation";
			this.lblEducation.Size = new System.Drawing.Size(60, 15);
			this.lblEducation.TabIndex = 4;
			this.lblEducation.Text = "&Education";
			// 
			// _education
			// 
			this._education.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._education, null);
			this.locExtender.SetLocalizationComment(this._education, null);
			this.locExtender.SetLocalizationPriority(this._education, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._education, "PeopleVw._education");
			this._education.Location = new System.Drawing.Point(9, 312);
			this._education.Name = "_education";
			this._education.Size = new System.Drawing.Size(276, 23);
			this._education.TabIndex = 5;
			// 
			// lblFullName
			// 
			this.lblFullName.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblFullName, null);
			this.locExtender.SetLocalizationComment(this.lblFullName, null);
			this.locExtender.SetLocalizingId(this.lblFullName, "PeopleVw.lblFullName");
			this.lblFullName.Location = new System.Drawing.Point(9, 7);
			this.lblFullName.Name = "lblFullName";
			this.lblFullName.Size = new System.Drawing.Size(61, 15);
			this.lblFullName.TabIndex = 0;
			this.lblFullName.Text = "&Full Name";
			// 
			// _fullName
			// 
			this._fullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._fullName, null);
			this.locExtender.SetLocalizationComment(this._fullName, null);
			this.locExtender.SetLocalizationPriority(this._fullName, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._fullName, "PeopleVw._fullName");
			this._fullName.Location = new System.Drawing.Point(9, 25);
			this._fullName.Name = "_fullName";
			this._fullName.Size = new System.Drawing.Size(276, 23);
			this._fullName.TabIndex = 1;
			this._fullName.TextChanged += new System.EventHandler(this._fullName_TextChanged);
			this._fullName.Validating += new System.ComponentModel.CancelEventHandler(this.HandleValidatingFullName);
			// 
			// uhbOtherLanguages
			// 
			this.uhbOtherLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.uhbOtherLanguages.BackColor = System.Drawing.Color.Transparent;
			this.uhbOtherLanguages.Controls.Add(this._languageMother4);
			this.uhbOtherLanguages.Controls.Add(this._languageMother3);
			this.uhbOtherLanguages.Controls.Add(this._languageMother2);
			this.uhbOtherLanguages.Controls.Add(this._languageMother1);
			this.uhbOtherLanguages.Controls.Add(this._language4);
			this.uhbOtherLanguages.Controls.Add(this._languageFather4);
			this.uhbOtherLanguages.Controls.Add(this._language3);
			this.uhbOtherLanguages.Controls.Add(this._languageFather3);
			this.uhbOtherLanguages.Controls.Add(this._language2);
			this.uhbOtherLanguages.Controls.Add(this._languageFather2);
			this.uhbOtherLanguages.Controls.Add(this._language1);
			this.uhbOtherLanguages.Controls.Add(this._languageFather1);
			this.uhbOtherLanguages.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.uhbOtherLanguages.LineColor = System.Drawing.SystemColors.ControlDark;
			this.uhbOtherLanguages.LineThickness = 1;
			this.locExtender.SetLocalizableToolTip(this.uhbOtherLanguages, null);
			this.locExtender.SetLocalizationComment(this.uhbOtherLanguages, null);
			this.locExtender.SetLocalizingId(this.uhbOtherLanguages, "PeopleVw.lblOtherLanguages");
			this.uhbOtherLanguages.Location = new System.Drawing.Point(9, 146);
			this.uhbOtherLanguages.Name = "uhbOtherLanguages";
			this.uhbOtherLanguages.Size = new System.Drawing.Size(276, 138);
			this.uhbOtherLanguages.TabIndex = 3;
			this.uhbOtherLanguages.Text = "&Other Languages";
			// 
			// _languageMother4
			// 
			this._languageMother4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._languageMother4.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._languageMother4, "Mother\'s Language");
			this.locExtender.SetLocalizationComment(this._languageMother4, null);
			this.locExtender.SetLocalizationPriority(this._languageMother4, LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._languageMother4, "PeopleVw._languageMother4");
			this._languageMother4.Location = new System.Drawing.Point(252, 113);
			this._languageMother4.Name = "_languageMother4";
			this._languageMother4.ParentType = SIL.Sponge.Model.ParentType.Mother;
			this._languageMother4.Selected = false;
			this._languageMother4.Size = new System.Drawing.Size(24, 24);
			this._languageMother4.TabIndex = 11;
			this._languageMother4.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMotherSelectedChanging);
			// 
			// _languageMother3
			// 
			this._languageMother3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._languageMother3.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._languageMother3, "Mother\'s Language");
			this.locExtender.SetLocalizationComment(this._languageMother3, null);
			this.locExtender.SetLocalizationPriority(this._languageMother3, LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._languageMother3, "PeopleVw._languageMother3");
			this._languageMother3.Location = new System.Drawing.Point(252, 83);
			this._languageMother3.Name = "_languageMother3";
			this._languageMother3.ParentType = SIL.Sponge.Model.ParentType.Mother;
			this._languageMother3.Selected = false;
			this._languageMother3.Size = new System.Drawing.Size(24, 24);
			this._languageMother3.TabIndex = 8;
			this._languageMother3.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMotherSelectedChanging);
			// 
			// _languageMother2
			// 
			this._languageMother2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._languageMother2.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._languageMother2, "Mother\'s Language");
			this.locExtender.SetLocalizationComment(this._languageMother2, null);
			this.locExtender.SetLocalizationPriority(this._languageMother2, LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._languageMother2, "PeopleVw._languageMother2");
			this._languageMother2.Location = new System.Drawing.Point(252, 53);
			this._languageMother2.Name = "_languageMother2";
			this._languageMother2.ParentType = SIL.Sponge.Model.ParentType.Mother;
			this._languageMother2.Selected = false;
			this._languageMother2.Size = new System.Drawing.Size(24, 24);
			this._languageMother2.TabIndex = 5;
			this._languageMother2.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMotherSelectedChanging);
			// 
			// _languageMother1
			// 
			this._languageMother1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._languageMother1.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._languageMother1, "Mother\'s Language");
			this.locExtender.SetLocalizationComment(this._languageMother1, null);
			this.locExtender.SetLocalizationPriority(this._languageMother1, LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._languageMother1, "PeopleVw._languageMother1");
			this._languageMother1.Location = new System.Drawing.Point(252, 23);
			this._languageMother1.Name = "_languageMother1";
			this._languageMother1.ParentType = SIL.Sponge.Model.ParentType.Mother;
			this._languageMother1.Selected = false;
			this._languageMother1.Size = new System.Drawing.Size(24, 24);
			this._languageMother1.TabIndex = 2;
			this._languageMother1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMotherSelectedChanging);
			// 
			// _language4
			// 
			this._language4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._language4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._language4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this._language4, null);
			this.locExtender.SetLocalizationComment(this._language4, null);
			this.locExtender.SetLocalizationPriority(this._language4, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._language4, "PeopleVw._otherLanguage3");
			this._language4.Location = new System.Drawing.Point(0, 115);
			this._language4.Name = "_language4";
			this._language4.Size = new System.Drawing.Size(218, 23);
			this._language4.TabIndex = 9;
			this._language4.Leave += new System.EventHandler(this.HandleLanguageNameLeave);
			this._language4.Enter += new System.EventHandler(this.HandleLanguageNameEnter);
			// 
			// _languageFather4
			// 
			this._languageFather4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._languageFather4.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._languageFather4, "Father\'s Language");
			this.locExtender.SetLocalizationComment(this._languageFather4, null);
			this.locExtender.SetLocalizationPriority(this._languageFather4, LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._languageFather4, "PeopleVw._languageFather4");
			this._languageFather4.Location = new System.Drawing.Point(224, 113);
			this._languageFather4.Name = "_languageFather4";
			this._languageFather4.ParentType = SIL.Sponge.Model.ParentType.Father;
			this._languageFather4.Selected = false;
			this._languageFather4.Size = new System.Drawing.Size(24, 24);
			this._languageFather4.TabIndex = 10;
			this._languageFather4.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFatherSelectedChanging);
			// 
			// _language3
			// 
			this._language3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._language3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._language3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this._language3, null);
			this.locExtender.SetLocalizationComment(this._language3, null);
			this.locExtender.SetLocalizationPriority(this._language3, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._language3, "PeopleVw._otherLanguage2");
			this._language3.Location = new System.Drawing.Point(0, 85);
			this._language3.Name = "_language3";
			this._language3.Size = new System.Drawing.Size(218, 23);
			this._language3.TabIndex = 6;
			this._language3.Leave += new System.EventHandler(this.HandleLanguageNameLeave);
			this._language3.Enter += new System.EventHandler(this.HandleLanguageNameEnter);
			// 
			// _languageFather3
			// 
			this._languageFather3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._languageFather3.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._languageFather3, "Father\'s Language");
			this.locExtender.SetLocalizationComment(this._languageFather3, null);
			this.locExtender.SetLocalizationPriority(this._languageFather3, LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._languageFather3, "PeopleVw._languageFather3");
			this._languageFather3.Location = new System.Drawing.Point(224, 83);
			this._languageFather3.Name = "_languageFather3";
			this._languageFather3.ParentType = SIL.Sponge.Model.ParentType.Father;
			this._languageFather3.Selected = false;
			this._languageFather3.Size = new System.Drawing.Size(24, 24);
			this._languageFather3.TabIndex = 7;
			this._languageFather3.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFatherSelectedChanging);
			// 
			// _language2
			// 
			this._language2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._language2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._language2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this._language2, null);
			this.locExtender.SetLocalizationComment(this._language2, null);
			this.locExtender.SetLocalizationPriority(this._language2, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._language2, "PeopleVw._otherLanguage1");
			this._language2.Location = new System.Drawing.Point(0, 55);
			this._language2.Name = "_language2";
			this._language2.Size = new System.Drawing.Size(218, 23);
			this._language2.TabIndex = 3;
			this._language2.Leave += new System.EventHandler(this.HandleLanguageNameLeave);
			this._language2.Enter += new System.EventHandler(this.HandleLanguageNameEnter);
			// 
			// _languageFather2
			// 
			this._languageFather2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._languageFather2.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._languageFather2, "Father\'s Language");
			this.locExtender.SetLocalizationComment(this._languageFather2, null);
			this.locExtender.SetLocalizationPriority(this._languageFather2, LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._languageFather2, "PeopleVw._languageFather2");
			this._languageFather2.Location = new System.Drawing.Point(224, 53);
			this._languageFather2.Name = "_languageFather2";
			this._languageFather2.ParentType = SIL.Sponge.Model.ParentType.Father;
			this._languageFather2.Selected = false;
			this._languageFather2.Size = new System.Drawing.Size(24, 24);
			this._languageFather2.TabIndex = 4;
			this._languageFather2.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFatherSelectedChanging);
			// 
			// _language1
			// 
			this._language1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._language1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._language1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this._language1, null);
			this.locExtender.SetLocalizationComment(this._language1, null);
			this.locExtender.SetLocalizationPriority(this._language1, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._language1, "PeopleVw._otherLanguage0");
			this._language1.Location = new System.Drawing.Point(0, 25);
			this._language1.Name = "_language1";
			this._language1.Size = new System.Drawing.Size(218, 23);
			this._language1.TabIndex = 0;
			this._language1.Leave += new System.EventHandler(this.HandleLanguageNameLeave);
			this._language1.Enter += new System.EventHandler(this.HandleLanguageNameEnter);
			// 
			// _languageFather1
			// 
			this._languageFather1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._languageFather1.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._languageFather1, "Father\'s Language");
			this.locExtender.SetLocalizationComment(this._languageFather1, null);
			this.locExtender.SetLocalizationPriority(this._languageFather1, LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._languageFather1, "PeopleVw._languageFather1");
			this._languageFather1.Location = new System.Drawing.Point(224, 23);
			this._languageFather1.Name = "_languageFather1";
			this._languageFather1.ParentType = SIL.Sponge.Model.ParentType.Father;
			this._languageFather1.Selected = false;
			this._languageFather1.Size = new System.Drawing.Size(24, 24);
			this._languageFather1.TabIndex = 1;
			this._languageFather1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFatherSelectedChanging);
			// 
			// uhbPrimaryLanguage
			// 
			this.uhbPrimaryLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.uhbPrimaryLanguage.BackColor = System.Drawing.Color.Transparent;
			this.uhbPrimaryLanguage.Controls.Add(this._languageMother0);
			this.uhbPrimaryLanguage.Controls.Add(this._language0);
			this.uhbPrimaryLanguage.Controls.Add(this._learnedIn);
			this.uhbPrimaryLanguage.Controls.Add(this.lblLearnedIn);
			this.uhbPrimaryLanguage.Controls.Add(this._languageFather0);
			this.uhbPrimaryLanguage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uhbPrimaryLanguage.LineColor = System.Drawing.SystemColors.ControlDark;
			this.uhbPrimaryLanguage.LineThickness = 1;
			this.locExtender.SetLocalizableToolTip(this.uhbPrimaryLanguage, null);
			this.locExtender.SetLocalizationComment(this.uhbPrimaryLanguage, null);
			this.locExtender.SetLocalizingId(this.uhbPrimaryLanguage, "PeopleVw.lblPrimaryLanguage");
			this.uhbPrimaryLanguage.Location = new System.Drawing.Point(9, 58);
			this.uhbPrimaryLanguage.Name = "uhbPrimaryLanguage";
			this.uhbPrimaryLanguage.Size = new System.Drawing.Size(276, 78);
			this.uhbPrimaryLanguage.TabIndex = 2;
			this.uhbPrimaryLanguage.Text = "&Primary Language";
			// 
			// _languageMother0
			// 
			this._languageMother0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._languageMother0.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._languageMother0, "Mother\'s Language");
			this.locExtender.SetLocalizationComment(this._languageMother0, null);
			this.locExtender.SetLocalizationPriority(this._languageMother0, LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._languageMother0, "PeopleVw._languageMother0");
			this._languageMother0.Location = new System.Drawing.Point(252, 23);
			this._languageMother0.Name = "_languageMother0";
			this._languageMother0.ParentType = SIL.Sponge.Model.ParentType.Mother;
			this._languageMother0.Selected = true;
			this._languageMother0.Size = new System.Drawing.Size(24, 24);
			this._languageMother0.TabIndex = 2;
			this._languageMother0.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMotherSelectedChanging);
			// 
			// _language0
			// 
			this._language0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._language0.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._language0.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this._language0, null);
			this.locExtender.SetLocalizationComment(this._language0, null);
			this.locExtender.SetLocalizationPriority(this._language0, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._language0, "PeopleVw._primaryLanguage");
			this._language0.Location = new System.Drawing.Point(0, 25);
			this._language0.Name = "_language0";
			this._language0.Size = new System.Drawing.Size(218, 23);
			this._language0.TabIndex = 0;
			this._language0.Leave += new System.EventHandler(this.HandleLanguageNameLeave);
			this._language0.Enter += new System.EventHandler(this.HandleLanguageNameEnter);
			// 
			// _learnedIn
			// 
			this._learnedIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._learnedIn, null);
			this.locExtender.SetLocalizationComment(this._learnedIn, null);
			this.locExtender.SetLocalizationPriority(this._learnedIn, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._learnedIn, "PeopleVw._learnedIn");
			this._learnedIn.Location = new System.Drawing.Point(79, 55);
			this._learnedIn.Name = "_learnedIn";
			this._learnedIn.Size = new System.Drawing.Size(197, 23);
			this._learnedIn.TabIndex = 4;
			// 
			// lblLearnedIn
			// 
			this.lblLearnedIn.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblLearnedIn, null);
			this.locExtender.SetLocalizationComment(this.lblLearnedIn, null);
			this.locExtender.SetLocalizingId(this.lblLearnedIn, "PeopleVw.lblLearnedIn");
			this.lblLearnedIn.Location = new System.Drawing.Point(0, 58);
			this.lblLearnedIn.Name = "lblLearnedIn";
			this.lblLearnedIn.Size = new System.Drawing.Size(65, 15);
			this.lblLearnedIn.TabIndex = 3;
			this.lblLearnedIn.Text = "&Learned In:";
			// 
			// _languageFather0
			// 
			this._languageFather0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._languageFather0.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._languageFather0, "Father\'s Language");
			this.locExtender.SetLocalizationComment(this._languageFather0, null);
			this.locExtender.SetLocalizationPriority(this._languageFather0, LocalizationPriority.MediumHigh);
			this.locExtender.SetLocalizingId(this._languageFather0, "PeopleVw._languageFather0");
			this._languageFather0.Location = new System.Drawing.Point(224, 23);
			this._languageFather0.Name = "_languageFather0";
			this._languageFather0.ParentType = SIL.Sponge.Model.ParentType.Father;
			this._languageFather0.Selected = true;
			this._languageFather0.Size = new System.Drawing.Size(24, 24);
			this._languageFather0.TabIndex = 1;
			this._languageFather0.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFatherSelectedChanging);
			// 
			// pnlRightSide
			// 
			this.pnlRightSide.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlRightSide.Controls.Add(this._notes);
			this.pnlRightSide.Controls.Add(this.lblNotes);
			this.pnlRightSide.Controls.Add(this._picture);
			this.pnlRightSide.Controls.Add(this.lblBirthYear);
			this.pnlRightSide.Controls.Add(this._contact);
			this.pnlRightSide.Controls.Add(this._gender);
			this.pnlRightSide.Controls.Add(this.lblContact);
			this.pnlRightSide.Controls.Add(this._birthYear);
			this.pnlRightSide.Controls.Add(this.lblGender);
			this.pnlRightSide.Location = new System.Drawing.Point(294, 3);
			this.pnlRightSide.Name = "pnlRightSide";
			this.pnlRightSide.Size = new System.Drawing.Size(233, 390);
			this.pnlRightSide.TabIndex = 1;
			// 
			// _notes
			// 
			this._notes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._notes, null);
			this.locExtender.SetLocalizationComment(this._notes, null);
			this.locExtender.SetLocalizationPriority(this._notes, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._notes, "PeopleVw._notes");
			this._notes.Location = new System.Drawing.Point(3, 261);
			this._notes.Multiline = true;
			this._notes.Name = "_notes";
			this._notes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._notes.Size = new System.Drawing.Size(224, 123);
			this._notes.TabIndex = 7;
			// 
			// lblNotes
			// 
			this.lblNotes.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblNotes, null);
			this.locExtender.SetLocalizationComment(this.lblNotes, null);
			this.locExtender.SetLocalizingId(this.lblNotes, "PeopleVw.lblNotes");
			this.lblNotes.Location = new System.Drawing.Point(3, 243);
			this.lblNotes.Name = "lblNotes";
			this.lblNotes.Size = new System.Drawing.Size(38, 15);
			this.lblNotes.TabIndex = 6;
			this.lblNotes.Text = "No&tes";
			// 
			// _picture
			// 
			this._picture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._picture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._picture.ContextMenuStrip = this.m_pictureMenu;
			this._picture.Cursor = System.Windows.Forms.Cursors.Hand;
			this._picture.Image = global::SIL.Sponge.Properties.Resources.kimidNoPhoto;
			this._picture.InitialImage = null;
			this.locExtender.SetLocalizableToolTip(this._picture, "Click to Change Picture");
			this.locExtender.SetLocalizationComment(this._picture, null);
			this.locExtender.SetLocalizingId(this._picture, "PeopleVw.personsPicture");
			this._picture.Location = new System.Drawing.Point(97, 6);
			this._picture.Name = "_picture";
			this._picture.Size = new System.Drawing.Size(130, 130);
			this._picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._picture.TabIndex = 2;
			this._picture.TabStop = false;
			this._picture.MouseLeave += new System.EventHandler(this.HandleMouseEnterLeaveOnPicture);
			this._picture.MouseClick += new System.Windows.Forms.MouseEventHandler(this._picture_MouseClick);
			this._picture.Paint += new System.Windows.Forms.PaintEventHandler(this._picture_Paint);
			this._picture.MouseEnter += new System.EventHandler(this.HandleMouseEnterLeaveOnPicture);
			// 
			// m_pictureMenu
			// 
			this.m_pictureMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_editImageMenuItem});
			this.locExtender.SetLocalizableToolTip(this.m_pictureMenu, null);
			this.locExtender.SetLocalizationComment(this.m_pictureMenu, null);
			this.locExtender.SetLocalizingId(this.m_pictureMenu, "contextMenuStrip1.contextMenuStrip1");
			this.m_pictureMenu.Name = "m_pictureMenu";
			this.m_pictureMenu.Size = new System.Drawing.Size(200, 26);
			// 
			// m_editImageMenuItem
			// 
			this.locExtender.SetLocalizableToolTip(this.m_editImageMenuItem, null);
			this.locExtender.SetLocalizationComment(this.m_editImageMenuItem, null);
			this.locExtender.SetLocalizingId(this.m_editImageMenuItem, ".openInDefaultEditorToolStripMenuItem");
			this.m_editImageMenuItem.Name = "m_editImageMenuItem";
			this.m_editImageMenuItem.Size = new System.Drawing.Size(199, 22);
			this.m_editImageMenuItem.Text = "Open in default editor...";
			this.m_editImageMenuItem.Click += new System.EventHandler(this.m_editImageMenuItem_Click);
			// 
			// lblBirthYear
			// 
			this.lblBirthYear.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblBirthYear, null);
			this.locExtender.SetLocalizationComment(this.lblBirthYear, null);
			this.locExtender.SetLocalizingId(this.lblBirthYear, "PeopleVw.lblBirthYear");
			this.lblBirthYear.Location = new System.Drawing.Point(3, 4);
			this.lblBirthYear.Name = "lblBirthYear";
			this.lblBirthYear.Size = new System.Drawing.Size(58, 15);
			this.lblBirthYear.TabIndex = 0;
			this.lblBirthYear.Text = "&Birth Year";
			// 
			// _contact
			// 
			this._contact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._contact, null);
			this.locExtender.SetLocalizationComment(this._contact, null);
			this.locExtender.SetLocalizationPriority(this._contact, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._contact, "PeopleVw._contact");
			this._contact.Location = new System.Drawing.Point(3, 161);
			this._contact.Multiline = true;
			this._contact.Name = "_contact";
			this._contact.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._contact.Size = new System.Drawing.Size(224, 72);
			this._contact.TabIndex = 5;
			// 
			// _gender
			// 
			this._gender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._gender.FormattingEnabled = true;
			this._gender.Items.AddRange(new object[] {
            "Male",
            "Female"});
			this.locExtender.SetLocalizableToolTip(this._gender, null);
			this.locExtender.SetLocalizationComment(this._gender, null);
			this.locExtender.SetLocalizingId(this._gender, "PeopleVw._gender");
			this._gender.Location = new System.Drawing.Point(3, 73);
			this._gender.Name = "_gender";
			this._gender.Size = new System.Drawing.Size(76, 23);
			this._gender.TabIndex = 3;
			// 
			// lblContact
			// 
			this.lblContact.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblContact, null);
			this.locExtender.SetLocalizationComment(this.lblContact, null);
			this.locExtender.SetLocalizingId(this.lblContact, "PeopleVw.lblContact");
			this.lblContact.Location = new System.Drawing.Point(3, 143);
			this.lblContact.Name = "lblContact";
			this.lblContact.Size = new System.Drawing.Size(91, 15);
			this.lblContact.TabIndex = 4;
			this.lblContact.Text = "How to &Contact";
			// 
			// _birthYear
			// 
			this.locExtender.SetLocalizableToolTip(this._birthYear, null);
			this.locExtender.SetLocalizationComment(this._birthYear, null);
			this.locExtender.SetLocalizationPriority(this._birthYear, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._birthYear, "PeopleVw._birthYear");
			this._birthYear.Location = new System.Drawing.Point(3, 22);
			this._birthYear.Name = "_birthYear";
			this._birthYear.Size = new System.Drawing.Size(76, 23);
			this._birthYear.TabIndex = 1;
			// 
			// lblGender
			// 
			this.lblGender.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblGender, null);
			this.locExtender.SetLocalizationComment(this.lblGender, null);
			this.locExtender.SetLocalizingId(this.lblGender, "PeopleVw.lblGender");
			this.lblGender.Location = new System.Drawing.Point(3, 55);
			this.lblGender.Name = "lblGender";
			this.lblGender.Size = new System.Drawing.Size(45, 15);
			this.lblGender.TabIndex = 2;
			this.lblGender.Text = "&Gender";
			// 
			// tpgInformedConsent
			// 
			this.tpgInformedConsent.Controls.Add(this.pnlPermissions);
			this.locExtender.SetLocalizableToolTip(this.tpgInformedConsent, "");
			this.locExtender.SetLocalizationComment(this.tpgInformedConsent, null);
			this.locExtender.SetLocalizationPriority(this.tpgInformedConsent, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.tpgInformedConsent, "PeopleVw.tpgInformedConsent");
			this.tpgInformedConsent.Location = new System.Drawing.Point(4, 26);
			this.tpgInformedConsent.Name = "tpgInformedConsent";
			this.tpgInformedConsent.Padding = new System.Windows.Forms.Padding(0, 2, 2, 1);
			this.tpgInformedConsent.Size = new System.Drawing.Size(532, 399);
			this.tpgInformedConsent.TabIndex = 1;
			this.tpgInformedConsent.Text = "Informed Consent";
			this.tpgInformedConsent.UseVisualStyleBackColor = true;
			// 
			// pnlPermissions
			// 
			this.pnlPermissions.Controls.Add(this.lstPermissionFiles);
			this.pnlPermissions.Controls.Add(this.btnDeletePermissionFile);
			this.pnlPermissions.Controls.Add(this.lblHeading);
			this.pnlPermissions.Controls.Add(this.pnlBrowser);
			this.pnlPermissions.Controls.Add(this.btnAddPermissionFile);
			this.pnlPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlPermissions.Location = new System.Drawing.Point(0, 2);
			this.pnlPermissions.Name = "pnlPermissions";
			this.pnlPermissions.Size = new System.Drawing.Size(530, 396);
			this.pnlPermissions.TabIndex = 6;
			// 
			// lstPermissionFiles
			// 
			this.lstPermissionFiles.FormattingEnabled = true;
			this.lstPermissionFiles.ItemHeight = 15;
			this.lstPermissionFiles.Location = new System.Drawing.Point(10, 41);
			this.lstPermissionFiles.Name = "lstPermissionFiles";
			this.lstPermissionFiles.Size = new System.Drawing.Size(199, 124);
			this.lstPermissionFiles.Sorted = true;
			this.lstPermissionFiles.TabIndex = 0;
			this.lstPermissionFiles.SelectedIndexChanged += new System.EventHandler(this.lstPermissionFiles_SelectedIndexChanged);
			// 
			// btnDeletePermissionFile
			// 
			this.locExtender.SetLocalizableToolTip(this.btnDeletePermissionFile, null);
			this.locExtender.SetLocalizationComment(this.btnDeletePermissionFile, null);
			this.locExtender.SetLocalizingId(this.btnDeletePermissionFile, "PeopleVw.btnDeletePermissionFile");
			this.btnDeletePermissionFile.Location = new System.Drawing.Point(114, 171);
			this.btnDeletePermissionFile.Name = "btnDeletePermissionFile";
			this.btnDeletePermissionFile.Size = new System.Drawing.Size(95, 24);
			this.btnDeletePermissionFile.TabIndex = 5;
			this.btnDeletePermissionFile.Text = "Delete File";
			this.btnDeletePermissionFile.UseVisualStyleBackColor = true;
			this.btnDeletePermissionFile.Click += new System.EventHandler(this.btnDeletePermissionFile_Click);
			// 
			// lblHeading
			// 
			this.lblHeading.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblHeading, null);
			this.locExtender.SetLocalizationComment(this.lblHeading, null);
			this.locExtender.SetLocalizingId(this.lblHeading, "PeopleVw.lblHeading");
			this.lblHeading.Location = new System.Drawing.Point(12, 15);
			this.lblHeading.Name = "lblHeading";
			this.lblHeading.Size = new System.Drawing.Size(257, 15);
			this.lblHeading.TabIndex = 1;
			this.lblHeading.Text = "Files related to informed consent by this person";
			// 
			// pnlBrowser
			// 
			this.pnlBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlBrowser.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			this.pnlBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlBrowser.ClipTextForChildControls = true;
			this.pnlBrowser.ControlReceivingFocusOnMnemonic = null;
			this.pnlBrowser.Controls.Add(this.webConsent);
			this.pnlBrowser.DoubleBuffered = true;
			this.pnlBrowser.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.locExtender.SetLocalizableToolTip(this.pnlBrowser, null);
			this.locExtender.SetLocalizationComment(this.pnlBrowser, null);
			this.locExtender.SetLocalizationPriority(this.pnlBrowser, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.pnlBrowser, "PeopleVw.pnlBrowser");
			this.pnlBrowser.Location = new System.Drawing.Point(219, 41);
			this.pnlBrowser.MnemonicGeneratesClick = false;
			this.pnlBrowser.Name = "pnlBrowser";
			this.pnlBrowser.PaintExplorerBarBackground = false;
			this.pnlBrowser.Size = new System.Drawing.Size(237, 114);
			this.pnlBrowser.TabIndex = 4;
			// 
			// webConsent
			// 
			this.webConsent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.locExtender.SetLocalizableToolTip(this.webConsent, null);
			this.locExtender.SetLocalizationComment(this.webConsent, null);
			this.locExtender.SetLocalizationPriority(this.webConsent, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.webConsent, "webBrowser1.webBrowser1");
			this.webConsent.Location = new System.Drawing.Point(0, 0);
			this.webConsent.MinimumSize = new System.Drawing.Size(20, 20);
			this.webConsent.Name = "webConsent";
			this.webConsent.Size = new System.Drawing.Size(235, 112);
			this.webConsent.TabIndex = 3;
			// 
			// btnAddPermissionFile
			// 
			this.locExtender.SetLocalizableToolTip(this.btnAddPermissionFile, null);
			this.locExtender.SetLocalizationComment(this.btnAddPermissionFile, null);
			this.locExtender.SetLocalizationPriority(this.btnAddPermissionFile, LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this.btnAddPermissionFile, "PeopleVw.btnAddPermissionFile");
			this.btnAddPermissionFile.Location = new System.Drawing.Point(10, 171);
			this.btnAddPermissionFile.Name = "btnAddPermissionFile";
			this.btnAddPermissionFile.Size = new System.Drawing.Size(95, 24);
			this.btnAddPermissionFile.TabIndex = 2;
			this.btnAddPermissionFile.Text = "Add File...";
			this.btnAddPermissionFile.UseVisualStyleBackColor = true;
			this.btnAddPermissionFile.Click += new System.EventHandler(this.btnAddPermissionFile_Click);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Views";
			// 
			// lblNoPeopleMsg
			// 
			this.lblNoPeopleMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblNoPeopleMsg.BackColor = System.Drawing.Color.Transparent;
			this.lblNoPeopleMsg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this.lblNoPeopleMsg, null);
			this.locExtender.SetLocalizationComment(this.lblNoPeopleMsg, null);
			this.locExtender.SetLocalizingId(this.lblNoPeopleMsg, "PeopleVw.lblNoPeopleMsg");
			this.lblNoPeopleMsg.Location = new System.Drawing.Point(14, 45);
			this.lblNoPeopleMsg.Name = "lblNoPeopleMsg";
			this.lblNoPeopleMsg.Size = new System.Drawing.Size(138, 168);
			this.lblNoPeopleMsg.TabIndex = 4;
			this.lblNoPeopleMsg.Text = "To add a person, click the \'New\' button below.";
			// 
			// PeopleVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "PeopleVw.BaseSplitVw");
			this.Name = "PeopleVw";
			this.ShowRightBottomPanel = false;
			this.Size = new System.Drawing.Size(709, 432);
			this.Controls.SetChildIndex(this.splitOuter, 0);
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitRightSide.Panel1.ResumeLayout(false);
			this.splitRightSide.ResumeLayout(false);
			this.tabPeople.ResumeLayout(false);
			this.tpgAbout.ResumeLayout(false);
			this.tblAbout.ResumeLayout(false);
			this.pnlLeftSide.ResumeLayout(false);
			this.pnlLeftSide.PerformLayout();
			this.uhbOtherLanguages.ResumeLayout(false);
			this.uhbOtherLanguages.PerformLayout();
			this.uhbPrimaryLanguage.ResumeLayout(false);
			this.uhbPrimaryLanguage.PerformLayout();
			this.pnlRightSide.ResumeLayout(false);
			this.pnlRightSide.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._picture)).EndInit();
			this.m_pictureMenu.ResumeLayout(false);
			this.tpgInformedConsent.ResumeLayout(false);
			this.pnlPermissions.ResumeLayout(false);
			this.pnlPermissions.PerformLayout();
			this.pnlBrowser.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox _birthYear;
		private System.Windows.Forms.TextBox _learnedIn;
		private System.Windows.Forms.TextBox _contact;
		private System.Windows.Forms.TextBox _language0;
		private System.Windows.Forms.TextBox _language1;
		private System.Windows.Forms.TextBox _language4;
		private System.Windows.Forms.TextBox _language3;
		private System.Windows.Forms.TextBox _language2;
		private System.Windows.Forms.TextBox _education;
		private System.Windows.Forms.TextBox _notes;
		private System.Windows.Forms.TextBox _primaryOccupation;
		private System.Windows.Forms.TextBox _fullName;
		private System.Windows.Forms.ComboBox _gender;
		private System.Windows.Forms.PictureBox _picture;
		private System.Windows.Forms.TabControl tabPeople;
		private System.Windows.Forms.TabPage tpgAbout;
		private System.Windows.Forms.TabPage tpgInformedConsent;
		private System.Windows.Forms.Label lblLearnedIn;
		private System.Windows.Forms.Label lblFullName;
		private System.Windows.Forms.Label lblBirthYear;
		private System.Windows.Forms.Label lblContact;
		private System.Windows.Forms.Label lblGender;
		private System.Windows.Forms.Label lblNotes;
		private System.Windows.Forms.Label lblEducation;
		private System.Windows.Forms.Label lblPimaryOccupation;
		private ParentButton _languageFather1;
		private ParentButton _languageFather2;
		private ParentButton _languageFather3;
		private ParentButton _languageFather4;
		private ListPanel lpPeople;
		private UnderlinedHdgBox uhbPrimaryLanguage;
		private UnderlinedHdgBox uhbOtherLanguages;
		private ParentButton _languageFather0;
		private System.Windows.Forms.TableLayoutPanel tblAbout;
		private System.Windows.Forms.Panel pnlLeftSide;
		private System.Windows.Forms.Panel pnlRightSide;
		private LocalizationExtender locExtender;
		private ParentButton _languageMother0;
		private ParentButton _languageMother4;
		private ParentButton _languageMother3;
		private ParentButton _languageMother2;
		private ParentButton _languageMother1;
		private System.Windows.Forms.Label lblNoPeopleMsg;
		private System.Windows.Forms.Button btnAddPermissionFile;
		private System.Windows.Forms.Label lblHeading;
		private System.Windows.Forms.ListBox lstPermissionFiles;
		private SilUtils.Controls.SilPanel pnlBrowser;
		private System.Windows.Forms.WebBrowser webConsent;
		private System.Windows.Forms.Button btnDeletePermissionFile;
		private System.Windows.Forms.Panel pnlPermissions;
		private System.Windows.Forms.ContextMenuStrip m_pictureMenu;
		private System.Windows.Forms.ToolStripMenuItem m_editImageMenuItem;
	}
}
