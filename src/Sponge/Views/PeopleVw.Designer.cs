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
			this.m_otherLanguage3 = new System.Windows.Forms.TextBox();
			this.m_otherLanguageParentGender3 = new SIL.Sponge.Controls.PeopleRadioButton();
			this.m_otherLanguage2 = new System.Windows.Forms.TextBox();
			this.m_otherLanguageParentGender2 = new SIL.Sponge.Controls.PeopleRadioButton();
			this.m_otherLanguage1 = new System.Windows.Forms.TextBox();
			this.m_otherLanguageParentGender1 = new SIL.Sponge.Controls.PeopleRadioButton();
			this.m_otherLanguage0 = new System.Windows.Forms.TextBox();
			this.m_otherLanguageParentGender0 = new SIL.Sponge.Controls.PeopleRadioButton();
			this.uhbPrimaryLanguage = new SIL.Sponge.Controls.UnderlinedHdgBox();
			this.m_primaryLanguage = new System.Windows.Forms.TextBox();
			this.m_learnedIn = new System.Windows.Forms.TextBox();
			this.m_primaryLanguageParent = new SIL.Sponge.Controls.PeopleRadioButton();
			this.lblLearnedIn = new System.Windows.Forms.Label();
			this.pnlRightSide = new System.Windows.Forms.Panel();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.m_notes = new System.Windows.Forms.TextBox();
			this.lblNotes = new System.Windows.Forms.Label();
			this.m_picture = new System.Windows.Forms.PictureBox();
			this.lblBirthYear = new System.Windows.Forms.Label();
			this.m_contact = new System.Windows.Forms.TextBox();
			this.m_gender = new System.Windows.Forms.ComboBox();
			this.lblContact = new System.Windows.Forms.Label();
			this.m_birthYear = new System.Windows.Forms.TextBox();
			this.lblHGender = new System.Windows.Forms.Label();
			this.tpgContributors = new System.Windows.Forms.TabPage();
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
			this.m_primaryOccupation.Location = new System.Drawing.Point(5, 363);
			this.m_primaryOccupation.Name = "m_primaryOccupation";
			this.m_primaryOccupation.Size = new System.Drawing.Size(274, 23);
			this.m_primaryOccupation.TabIndex = 7;
			// 
			// lblEducation
			// 
			this.lblEducation.AutoSize = true;
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
			this.m_education.Location = new System.Drawing.Point(5, 312);
			this.m_education.Name = "m_education";
			this.m_education.Size = new System.Drawing.Size(274, 23);
			this.m_education.TabIndex = 5;
			// 
			// lblFullName
			// 
			this.lblFullName.AutoSize = true;
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
			this.uhbOtherLanguages.Controls.Add(this.m_otherLanguage3);
			this.uhbOtherLanguages.Controls.Add(this.m_otherLanguageParentGender3);
			this.uhbOtherLanguages.Controls.Add(this.m_otherLanguage2);
			this.uhbOtherLanguages.Controls.Add(this.m_otherLanguageParentGender2);
			this.uhbOtherLanguages.Controls.Add(this.m_otherLanguage1);
			this.uhbOtherLanguages.Controls.Add(this.m_otherLanguageParentGender1);
			this.uhbOtherLanguages.Controls.Add(this.m_otherLanguage0);
			this.uhbOtherLanguages.Controls.Add(this.m_otherLanguageParentGender0);
			this.uhbOtherLanguages.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.uhbOtherLanguages.LineColor = System.Drawing.SystemColors.ControlDark;
			this.uhbOtherLanguages.LineThickness = 1;
			this.uhbOtherLanguages.Location = new System.Drawing.Point(5, 146);
			this.uhbOtherLanguages.Name = "uhbOtherLanguages";
			this.uhbOtherLanguages.Size = new System.Drawing.Size(274, 138);
			this.uhbOtherLanguages.TabIndex = 3;
			this.uhbOtherLanguages.Text = "&Other Languages";
			// 
			// m_otherLanguage3
			// 
			this.m_otherLanguage3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_otherLanguage3.Location = new System.Drawing.Point(0, 115);
			this.m_otherLanguage3.Name = "m_otherLanguage3";
			this.m_otherLanguage3.Size = new System.Drawing.Size(216, 23);
			this.m_otherLanguage3.TabIndex = 6;
			// 
			// m_otherLanguageParentGender3
			// 
			this.m_otherLanguageParentGender3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_otherLanguageParentGender3.BackColor = System.Drawing.Color.Transparent;
			this.m_otherLanguageParentGender3.Location = new System.Drawing.Point(222, 113);
			this.m_otherLanguageParentGender3.MaximumSize = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender3.MinimumSize = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender3.Name = "m_otherLanguageParentGender3";
			this.m_otherLanguageParentGender3.Size = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender3.TabIndex = 7;
			this.m_otherLanguageParentGender3.Value = SIL.Sponge.Model.Gender.Male;
			// 
			// m_otherLanguage2
			// 
			this.m_otherLanguage2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_otherLanguage2.Location = new System.Drawing.Point(0, 85);
			this.m_otherLanguage2.Name = "m_otherLanguage2";
			this.m_otherLanguage2.Size = new System.Drawing.Size(216, 23);
			this.m_otherLanguage2.TabIndex = 4;
			// 
			// m_otherLanguageParentGender2
			// 
			this.m_otherLanguageParentGender2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_otherLanguageParentGender2.BackColor = System.Drawing.Color.Transparent;
			this.m_otherLanguageParentGender2.Location = new System.Drawing.Point(222, 83);
			this.m_otherLanguageParentGender2.MaximumSize = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender2.MinimumSize = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender2.Name = "m_otherLanguageParentGender2";
			this.m_otherLanguageParentGender2.Size = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender2.TabIndex = 5;
			this.m_otherLanguageParentGender2.Value = SIL.Sponge.Model.Gender.Male;
			// 
			// m_otherLanguage1
			// 
			this.m_otherLanguage1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_otherLanguage1.Location = new System.Drawing.Point(0, 55);
			this.m_otherLanguage1.Name = "m_otherLanguage1";
			this.m_otherLanguage1.Size = new System.Drawing.Size(216, 23);
			this.m_otherLanguage1.TabIndex = 2;
			// 
			// m_otherLanguageParentGender1
			// 
			this.m_otherLanguageParentGender1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_otherLanguageParentGender1.BackColor = System.Drawing.Color.Transparent;
			this.m_otherLanguageParentGender1.Location = new System.Drawing.Point(222, 53);
			this.m_otherLanguageParentGender1.MaximumSize = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender1.MinimumSize = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender1.Name = "m_otherLanguageParentGender1";
			this.m_otherLanguageParentGender1.Size = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender1.TabIndex = 3;
			this.m_otherLanguageParentGender1.Value = SIL.Sponge.Model.Gender.Male;
			// 
			// m_otherLanguage0
			// 
			this.m_otherLanguage0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_otherLanguage0.Location = new System.Drawing.Point(0, 25);
			this.m_otherLanguage0.Name = "m_otherLanguage0";
			this.m_otherLanguage0.Size = new System.Drawing.Size(216, 23);
			this.m_otherLanguage0.TabIndex = 0;
			// 
			// m_otherLanguageParentGender0
			// 
			this.m_otherLanguageParentGender0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_otherLanguageParentGender0.BackColor = System.Drawing.Color.Transparent;
			this.m_otherLanguageParentGender0.Location = new System.Drawing.Point(222, 23);
			this.m_otherLanguageParentGender0.MaximumSize = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender0.MinimumSize = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender0.Name = "m_otherLanguageParentGender0";
			this.m_otherLanguageParentGender0.Size = new System.Drawing.Size(52, 24);
			this.m_otherLanguageParentGender0.TabIndex = 1;
			this.m_otherLanguageParentGender0.Value = SIL.Sponge.Model.Gender.Male;
			// 
			// uhbPrimaryLanguage
			// 
			this.uhbPrimaryLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.uhbPrimaryLanguage.BackColor = System.Drawing.Color.Transparent;
			this.uhbPrimaryLanguage.Controls.Add(this.m_primaryLanguage);
			this.uhbPrimaryLanguage.Controls.Add(this.m_learnedIn);
			this.uhbPrimaryLanguage.Controls.Add(this.m_primaryLanguageParent);
			this.uhbPrimaryLanguage.Controls.Add(this.lblLearnedIn);
			this.uhbPrimaryLanguage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uhbPrimaryLanguage.LineColor = System.Drawing.SystemColors.ControlDark;
			this.uhbPrimaryLanguage.LineThickness = 1;
			this.uhbPrimaryLanguage.Location = new System.Drawing.Point(5, 58);
			this.uhbPrimaryLanguage.Name = "uhbPrimaryLanguage";
			this.uhbPrimaryLanguage.Size = new System.Drawing.Size(274, 78);
			this.uhbPrimaryLanguage.TabIndex = 2;
			this.uhbPrimaryLanguage.Text = "&Primary Language";
			// 
			// m_primaryLanguage
			// 
			this.m_primaryLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_primaryLanguage.Location = new System.Drawing.Point(0, 25);
			this.m_primaryLanguage.Name = "m_primaryLanguage";
			this.m_primaryLanguage.Size = new System.Drawing.Size(216, 23);
			this.m_primaryLanguage.TabIndex = 0;
			// 
			// m_learnedIn
			// 
			this.m_learnedIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_learnedIn.Location = new System.Drawing.Point(82, 55);
			this.m_learnedIn.Name = "m_learnedIn";
			this.m_learnedIn.Size = new System.Drawing.Size(192, 23);
			this.m_learnedIn.TabIndex = 3;
			// 
			// m_primaryLanguageParent
			// 
			this.m_primaryLanguageParent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_primaryLanguageParent.BackColor = System.Drawing.Color.Transparent;
			this.m_primaryLanguageParent.Location = new System.Drawing.Point(222, 23);
			this.m_primaryLanguageParent.MaximumSize = new System.Drawing.Size(52, 24);
			this.m_primaryLanguageParent.MinimumSize = new System.Drawing.Size(52, 24);
			this.m_primaryLanguageParent.Name = "m_primaryLanguageParent";
			this.m_primaryLanguageParent.Size = new System.Drawing.Size(52, 24);
			this.m_primaryLanguageParent.TabIndex = 1;
			this.m_primaryLanguageParent.Value = SIL.Sponge.Model.Gender.Male;
			// 
			// lblLearnedIn
			// 
			this.lblLearnedIn.AutoSize = true;
			this.lblLearnedIn.Location = new System.Drawing.Point(3, 58);
			this.lblLearnedIn.Name = "lblLearnedIn";
			this.lblLearnedIn.Size = new System.Drawing.Size(65, 15);
			this.lblLearnedIn.TabIndex = 2;
			this.lblLearnedIn.Text = "&Learned In:";
			// 
			// pnlRightSide
			// 
			this.pnlRightSide.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlRightSide.Controls.Add(this.btnSave);
			this.pnlRightSide.Controls.Add(this.btnCancel);
			this.pnlRightSide.Controls.Add(this.m_notes);
			this.pnlRightSide.Controls.Add(this.lblNotes);
			this.pnlRightSide.Controls.Add(this.m_picture);
			this.pnlRightSide.Controls.Add(this.lblBirthYear);
			this.pnlRightSide.Controls.Add(this.m_contact);
			this.pnlRightSide.Controls.Add(this.m_gender);
			this.pnlRightSide.Controls.Add(this.lblContact);
			this.pnlRightSide.Controls.Add(this.m_birthYear);
			this.pnlRightSide.Controls.Add(this.lblHGender);
			this.pnlRightSide.Location = new System.Drawing.Point(292, 3);
			this.pnlRightSide.Name = "pnlRightSide";
			this.pnlRightSide.Size = new System.Drawing.Size(231, 390);
			this.pnlRightSide.TabIndex = 1;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(75, 366);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 24);
			this.btnSave.TabIndex = 9;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(156, 366);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 24);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// m_notes
			// 
			this.m_notes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_notes.Location = new System.Drawing.Point(3, 261);
			this.m_notes.Multiline = true;
			this.m_notes.Name = "m_notes";
			this.m_notes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.m_notes.Size = new System.Drawing.Size(228, 99);
			this.m_notes.TabIndex = 7;
			// 
			// lblNotes
			// 
			this.lblNotes.AutoSize = true;
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
			this.m_picture.Image = global::SIL.Sponge.Properties.Resources.kimidNoPhoto;
			this.m_picture.InitialImage = null;
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
			this.m_gender.Location = new System.Drawing.Point(3, 73);
			this.m_gender.Name = "m_gender";
			this.m_gender.Size = new System.Drawing.Size(76, 23);
			this.m_gender.TabIndex = 3;
			// 
			// lblContact
			// 
			this.lblContact.AutoSize = true;
			this.lblContact.Location = new System.Drawing.Point(4, 143);
			this.lblContact.Name = "lblContact";
			this.lblContact.Size = new System.Drawing.Size(94, 15);
			this.lblContact.TabIndex = 4;
			this.lblContact.Text = "How to &Contact:";
			// 
			// m_birthYear
			// 
			this.m_birthYear.Location = new System.Drawing.Point(3, 22);
			this.m_birthYear.Name = "m_birthYear";
			this.m_birthYear.Size = new System.Drawing.Size(76, 23);
			this.m_birthYear.TabIndex = 1;
			// 
			// lblHGender
			// 
			this.lblHGender.AutoSize = true;
			this.lblHGender.Location = new System.Drawing.Point(4, 55);
			this.lblHGender.Name = "lblHGender";
			this.lblHGender.Size = new System.Drawing.Size(48, 15);
			this.lblHGender.TabIndex = 2;
			this.lblHGender.Text = "&Gender:";
			// 
			// tpgContributors
			// 
			this.tpgContributors.Location = new System.Drawing.Point(4, 26);
			this.tpgContributors.Name = "tpgContributors";
			this.tpgContributors.Padding = new System.Windows.Forms.Padding(3);
			this.tpgContributors.Size = new System.Drawing.Size(480, 350);
			this.tpgContributors.TabIndex = 1;
			this.tpgContributors.Text = "Contributors && Permissions";
			this.tpgContributors.ToolTipText = "Contributors & Permissions";
			this.tpgContributors.UseVisualStyleBackColor = true;
			// 
			// PeopleVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.DoubleBuffered = true;
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
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox m_birthYear;
		private System.Windows.Forms.TextBox m_learnedIn;
		private System.Windows.Forms.TextBox m_contact;
		private System.Windows.Forms.TextBox m_primaryLanguage;
		private System.Windows.Forms.TextBox m_otherLanguage0;
		private System.Windows.Forms.TextBox m_otherLanguage3;
		private System.Windows.Forms.TextBox m_otherLanguage2;
		private System.Windows.Forms.TextBox m_otherLanguage1;
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
		private System.Windows.Forms.Label lblHGender;
		private System.Windows.Forms.Label lblNotes;
		private System.Windows.Forms.Label lblEducation;
		private System.Windows.Forms.Label lblPimaryOccupation;
		private PeopleRadioButton m_otherLanguageParentGender0;
		private PeopleRadioButton m_otherLanguageParentGender1;
		private PeopleRadioButton m_otherLanguageParentGender2;
		private PeopleRadioButton m_otherLanguageParentGender3;
		private ListPanel lpPeople;
		private UnderlinedHdgBox uhbPrimaryLanguage;
		private UnderlinedHdgBox uhbOtherLanguages;
		private PeopleRadioButton m_primaryLanguageParent;
		private System.Windows.Forms.TableLayoutPanel tblLayout;
		private System.Windows.Forms.Panel pnlLeftSide;
		private System.Windows.Forms.Panel pnlRightSide;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
	}
}
