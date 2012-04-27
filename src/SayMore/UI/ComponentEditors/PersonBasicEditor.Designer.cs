using SayMore.Utilities.LowLevelControls;

namespace SayMore.Utilities.ComponentEditors
{
	partial class PersonBasicEditor
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
			this._labelFullName = new System.Windows.Forms.Label();
			this._id = new System.Windows.Forms.TextBox();
			this._labelBirthYear = new System.Windows.Forms.Label();
			this._birthYear = new System.Windows.Forms.TextBox();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._pbOtherLangMother3 = new SayMore.Utilities.LowLevelControls.ParentButton();
			this._primaryLanguageLearnedIn = new System.Windows.Forms.TextBox();
			this._labelPrimaryLanguageLearnedIn = new System.Windows.Forms.Label();
			this._pbOtherLangMother1 = new SayMore.Utilities.LowLevelControls.ParentButton();
			this._pbOtherLangFather3 = new SayMore.Utilities.LowLevelControls.ParentButton();
			this._pbOtherLangFather2 = new SayMore.Utilities.LowLevelControls.ParentButton();
			this._pbOtherLangFather1 = new SayMore.Utilities.LowLevelControls.ParentButton();
			this._pbOtherLangMother0 = new SayMore.Utilities.LowLevelControls.ParentButton();
			this._pbOtherLangFather0 = new SayMore.Utilities.LowLevelControls.ParentButton();
			this._otherLanguage3 = new System.Windows.Forms.TextBox();
			this._otherLanguage0 = new System.Windows.Forms.TextBox();
			this._otherLanguage1 = new System.Windows.Forms.TextBox();
			this._otherLanguage2 = new System.Windows.Forms.TextBox();
			this._primaryLanguage = new System.Windows.Forms.TextBox();
			this._labelPrimaryLanguage = new System.Windows.Forms.Label();
			this._pbPrimaryLangMother = new SayMore.Utilities.LowLevelControls.ParentButton();
			this._pbPrimaryLangFather = new SayMore.Utilities.LowLevelControls.ParentButton();
			this._labelOtherLanguages = new System.Windows.Forms.Label();
			this._pbOtherLangMother2 = new SayMore.Utilities.LowLevelControls.ParentButton();
			this._labelEducation = new System.Windows.Forms.Label();
			this._education = new System.Windows.Forms.TextBox();
			this._labelPrimaryOccupation = new System.Windows.Forms.Label();
			this._primaryOccupation = new System.Windows.Forms.TextBox();
			this._labelgender = new System.Windows.Forms.Label();
			this._gender = new System.Windows.Forms.ComboBox();
			this._labelHowToContact = new System.Windows.Forms.Label();
			this._howToContact = new System.Windows.Forms.TextBox();
			this._labelCustomFields = new System.Windows.Forms.Label();
			this._panelGrid = new System.Windows.Forms.Panel();
			this._panelPicture = new System.Windows.Forms.Panel();
			this._personsPicture = new System.Windows.Forms.PictureBox();
			this._binder = new SayMore.Utilities.ComponentEditors.BindingHelper(this.components);
			this._autoCompleteHelper = new SayMore.Utilities.ComponentEditors.AutoCompleteHelper(this.components);
			this._tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._tableLayout.SuspendLayout();
			this._panelPicture.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._personsPicture)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _labelFullName
			// 
			this._labelFullName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelFullName.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelFullName, 4);
			this.locExtender.SetLocalizableToolTip(this._labelFullName, null);
			this.locExtender.SetLocalizationComment(this._labelFullName, null);
			this.locExtender.SetLocalizingId(this._labelFullName, "PeopleView.MetadataEditor._labelFullName");
			this._labelFullName.Location = new System.Drawing.Point(0, 0);
			this._labelFullName.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelFullName.Name = "_labelFullName";
			this._labelFullName.Size = new System.Drawing.Size(54, 13);
			this._labelFullName.TabIndex = 0;
			this._labelFullName.Text = "&Full Name";
			// 
			// _id
			// 
			this._id.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._id, "");
			this._tableLayout.SetColumnSpan(this._id, 2);
			this._binder.SetIsBound(this._id, true);
			this._binder.SetIsComponentFileId(this._id, true);
			this.locExtender.SetLocalizableToolTip(this._id, null);
			this.locExtender.SetLocalizationComment(this._id, null);
			this.locExtender.SetLocalizationPriority(this._id, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._id, "PersonBasicEditor._id");
			this._id.Location = new System.Drawing.Point(0, 16);
			this._id.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._id.Name = "_id";
			this._id.Size = new System.Drawing.Size(180, 20);
			this._id.TabIndex = 1;
			this._autoCompleteHelper.SetUpdateGatherer(this._id, false);
			// 
			// _labelBirthYear
			// 
			this._labelBirthYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelBirthYear.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelBirthYear, null);
			this.locExtender.SetLocalizationComment(this._labelBirthYear, null);
			this.locExtender.SetLocalizingId(this._labelBirthYear, "PeopleView.MetadataEditor._labelBirthYear");
			this._labelBirthYear.Location = new System.Drawing.Point(252, 0);
			this._labelBirthYear.Margin = new System.Windows.Forms.Padding(7, 0, 3, 0);
			this._labelBirthYear.Name = "_labelBirthYear";
			this._labelBirthYear.Size = new System.Drawing.Size(53, 13);
			this._labelBirthYear.TabIndex = 2;
			this._labelBirthYear.Text = "&Birth Year";
			// 
			// _birthYear
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._birthYear, "");
			this._birthYear.Dock = System.Windows.Forms.DockStyle.Top;
			this._binder.SetIsBound(this._birthYear, true);
			this._binder.SetIsComponentFileId(this._birthYear, false);
			this.locExtender.SetLocalizableToolTip(this._birthYear, null);
			this.locExtender.SetLocalizationComment(this._birthYear, null);
			this.locExtender.SetLocalizationPriority(this._birthYear, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._birthYear, "PersonBasicEditor._birthYear");
			this._birthYear.Location = new System.Drawing.Point(252, 16);
			this._birthYear.Margin = new System.Windows.Forms.Padding(7, 3, 3, 0);
			this._birthYear.Name = "_birthYear";
			this._birthYear.Size = new System.Drawing.Size(81, 20);
			this._birthYear.TabIndex = 3;
			this._autoCompleteHelper.SetUpdateGatherer(this._birthYear, false);
			// 
			// _tableLayout
			// 
			this._tableLayout.AllowDrop = true;
			this._tableLayout.AutoSize = true;
			this._tableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 6;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58F));
			this._tableLayout.Controls.Add(this._pbOtherLangMother3, 3, 9);
			this._tableLayout.Controls.Add(this._primaryLanguageLearnedIn, 1, 4);
			this._tableLayout.Controls.Add(this._labelPrimaryLanguageLearnedIn, 0, 4);
			this._tableLayout.Controls.Add(this._pbOtherLangMother1, 3, 7);
			this._tableLayout.Controls.Add(this._pbOtherLangFather3, 2, 9);
			this._tableLayout.Controls.Add(this._pbOtherLangFather2, 2, 8);
			this._tableLayout.Controls.Add(this._pbOtherLangFather1, 2, 7);
			this._tableLayout.Controls.Add(this._pbOtherLangMother0, 3, 6);
			this._tableLayout.Controls.Add(this._pbOtherLangFather0, 2, 6);
			this._tableLayout.Controls.Add(this._otherLanguage3, 0, 9);
			this._tableLayout.Controls.Add(this._labelFullName, 0, 0);
			this._tableLayout.Controls.Add(this._otherLanguage0, 0, 6);
			this._tableLayout.Controls.Add(this._otherLanguage1, 0, 7);
			this._tableLayout.Controls.Add(this._otherLanguage2, 0, 8);
			this._tableLayout.Controls.Add(this._primaryLanguage, 0, 3);
			this._tableLayout.Controls.Add(this._id, 0, 1);
			this._tableLayout.Controls.Add(this._labelPrimaryLanguage, 0, 2);
			this._tableLayout.Controls.Add(this._pbPrimaryLangMother, 3, 3);
			this._tableLayout.Controls.Add(this._pbPrimaryLangFather, 2, 3);
			this._tableLayout.Controls.Add(this._labelOtherLanguages, 0, 5);
			this._tableLayout.Controls.Add(this._pbOtherLangMother2, 3, 8);
			this._tableLayout.Controls.Add(this._labelEducation, 0, 10);
			this._tableLayout.Controls.Add(this._education, 0, 11);
			this._tableLayout.Controls.Add(this._labelPrimaryOccupation, 0, 12);
			this._tableLayout.Controls.Add(this._primaryOccupation, 0, 13);
			this._tableLayout.Controls.Add(this._labelBirthYear, 4, 0);
			this._tableLayout.Controls.Add(this._birthYear, 4, 1);
			this._tableLayout.Controls.Add(this._labelgender, 4, 2);
			this._tableLayout.Controls.Add(this._gender, 4, 3);
			this._tableLayout.Controls.Add(this._labelHowToContact, 4, 5);
			this._tableLayout.Controls.Add(this._howToContact, 4, 6);
			this._tableLayout.Controls.Add(this._labelCustomFields, 4, 9);
			this._tableLayout.Controls.Add(this._panelGrid, 4, 10);
			this._tableLayout.Controls.Add(this._panelPicture, 5, 0);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayout.Location = new System.Drawing.Point(7, 7);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 15;
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
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.Size = new System.Drawing.Size(502, 324);
			this._tableLayout.TabIndex = 0;
			this._tableLayout.TabStop = true;
			this._tableLayout.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleTableLayoutPaint);
			// 
			// _pbOtherLangMother3
			// 
			this._pbOtherLangMother3.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangMother3, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangMother3, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangMother3, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangMother3, "PersonBasicEditor._pbOtherLangMother3");
			this._pbOtherLangMother3.Location = new System.Drawing.Point(207, 211);
			this._pbOtherLangMother3.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangMother3.Name = "_pbOtherLangMother3";
			this._pbOtherLangMother3.ParentType = SayMore.Utilities.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother3.Selected = false;
			this._pbOtherLangMother3.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother3.TabIndex = 24;
			this._pbOtherLangMother3.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			this._pbOtherLangMother3.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _primaryLanguageLearnedIn
			// 
			this._primaryLanguageLearnedIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._primaryLanguageLearnedIn, "primaryLanguageLearnedIn");
			this._primaryLanguageLearnedIn.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._primaryLanguageLearnedIn.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._binder.SetIsBound(this._primaryLanguageLearnedIn, true);
			this._binder.SetIsComponentFileId(this._primaryLanguageLearnedIn, false);
			this.locExtender.SetLocalizableToolTip(this._primaryLanguageLearnedIn, null);
			this.locExtender.SetLocalizationComment(this._primaryLanguageLearnedIn, null);
			this.locExtender.SetLocalizationPriority(this._primaryLanguageLearnedIn, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._primaryLanguageLearnedIn, "PersonBasicEditor._primaryLanguageLearnedIn");
			this._primaryLanguageLearnedIn.Location = new System.Drawing.Point(64, 86);
			this._primaryLanguageLearnedIn.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
			this._primaryLanguageLearnedIn.Name = "_primaryLanguageLearnedIn";
			this._primaryLanguageLearnedIn.Size = new System.Drawing.Size(116, 20);
			this._primaryLanguageLearnedIn.TabIndex = 9;
			this._autoCompleteHelper.SetUpdateGatherer(this._primaryLanguageLearnedIn, false);
			// 
			// _labelPrimaryLanguageLearnedIn
			// 
			this._labelPrimaryLanguageLearnedIn.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelPrimaryLanguageLearnedIn, null);
			this.locExtender.SetLocalizationComment(this._labelPrimaryLanguageLearnedIn, null);
			this.locExtender.SetLocalizingId(this._labelPrimaryLanguageLearnedIn, "PeopleView.MetadataEditor._labelPrimaryLanguageLearnedIn");
			this._labelPrimaryLanguageLearnedIn.Location = new System.Drawing.Point(0, 90);
			this._labelPrimaryLanguageLearnedIn.Margin = new System.Windows.Forms.Padding(0, 7, 3, 7);
			this._labelPrimaryLanguageLearnedIn.Name = "_labelPrimaryLanguageLearnedIn";
			this._labelPrimaryLanguageLearnedIn.Size = new System.Drawing.Size(61, 13);
			this._labelPrimaryLanguageLearnedIn.TabIndex = 8;
			this._labelPrimaryLanguageLearnedIn.Text = "Learned &In:";
			// 
			// _pbOtherLangMother1
			// 
			this._pbOtherLangMother1.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangMother1, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangMother1, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangMother1, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangMother1, "PersonBasicEditor._pbOtherLangMother1");
			this._pbOtherLangMother1.Location = new System.Drawing.Point(207, 155);
			this._pbOtherLangMother1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangMother1.Name = "_pbOtherLangMother1";
			this._pbOtherLangMother1.ParentType = SayMore.Utilities.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother1.Selected = false;
			this._pbOtherLangMother1.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother1.TabIndex = 18;
			this._pbOtherLangMother1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			this._pbOtherLangMother1.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbOtherLangFather3
			// 
			this._pbOtherLangFather3.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangFather3, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangFather3, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangFather3, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangFather3, "PersonBasicEditor._pbOtherLangFather3");
			this._pbOtherLangFather3.Location = new System.Drawing.Point(183, 211);
			this._pbOtherLangFather3.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangFather3.Name = "_pbOtherLangFather3";
			this._pbOtherLangFather3.ParentType = SayMore.Utilities.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather3.Selected = false;
			this._pbOtherLangFather3.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather3.TabIndex = 23;
			this._pbOtherLangFather3.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			this._pbOtherLangFather3.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbOtherLangFather2
			// 
			this._pbOtherLangFather2.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangFather2, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangFather2, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangFather2, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangFather2, "PersonBasicEditor._pbOtherLangFather2");
			this._pbOtherLangFather2.Location = new System.Drawing.Point(183, 183);
			this._pbOtherLangFather2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangFather2.Name = "_pbOtherLangFather2";
			this._pbOtherLangFather2.ParentType = SayMore.Utilities.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather2.Selected = false;
			this._pbOtherLangFather2.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather2.TabIndex = 20;
			this._pbOtherLangFather2.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			this._pbOtherLangFather2.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbOtherLangFather1
			// 
			this._pbOtherLangFather1.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangFather1, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangFather1, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangFather1, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangFather1, "PersonBasicEditor._pbOtherLangFather1");
			this._pbOtherLangFather1.Location = new System.Drawing.Point(183, 155);
			this._pbOtherLangFather1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangFather1.Name = "_pbOtherLangFather1";
			this._pbOtherLangFather1.ParentType = SayMore.Utilities.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather1.Selected = false;
			this._pbOtherLangFather1.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather1.TabIndex = 17;
			this._pbOtherLangFather1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			this._pbOtherLangFather1.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbOtherLangMother0
			// 
			this._pbOtherLangMother0.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangMother0, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangMother0, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangMother0, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangMother0, "PersonBasicEditor._pbOtherLangMother0");
			this._pbOtherLangMother0.Location = new System.Drawing.Point(207, 129);
			this._pbOtherLangMother0.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangMother0.Name = "_pbOtherLangMother0";
			this._pbOtherLangMother0.ParentType = SayMore.Utilities.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother0.Selected = false;
			this._pbOtherLangMother0.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother0.TabIndex = 15;
			this._pbOtherLangMother0.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			this._pbOtherLangMother0.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbOtherLangFather0
			// 
			this._pbOtherLangFather0.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangFather0, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangFather0, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangFather0, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangFather0, "PersonBasicEditor._pbOtherLangFather0");
			this._pbOtherLangFather0.Location = new System.Drawing.Point(183, 129);
			this._pbOtherLangFather0.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangFather0.Name = "_pbOtherLangFather0";
			this._pbOtherLangFather0.ParentType = SayMore.Utilities.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather0.Selected = false;
			this._pbOtherLangFather0.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather0.TabIndex = 14;
			this._pbOtherLangFather0.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			this._pbOtherLangFather0.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _otherLanguage3
			// 
			this._otherLanguage3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._otherLanguage3, "language");
			this._otherLanguage3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._otherLanguage3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tableLayout.SetColumnSpan(this._otherLanguage3, 2);
			this._binder.SetIsBound(this._otherLanguage3, true);
			this._binder.SetIsComponentFileId(this._otherLanguage3, false);
			this.locExtender.SetLocalizableToolTip(this._otherLanguage3, null);
			this.locExtender.SetLocalizationComment(this._otherLanguage3, null);
			this.locExtender.SetLocalizationPriority(this._otherLanguage3, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._otherLanguage3, "PersonBasicEditor._otherLanguage3");
			this._otherLanguage3.Location = new System.Drawing.Point(0, 213);
			this._otherLanguage3.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage3.Name = "_otherLanguage3";
			this._otherLanguage3.Size = new System.Drawing.Size(180, 20);
			this._otherLanguage3.TabIndex = 22;
			this._autoCompleteHelper.SetUpdateGatherer(this._otherLanguage3, false);
			// 
			// _otherLanguage0
			// 
			this._otherLanguage0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._otherLanguage0, "language");
			this._otherLanguage0.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._otherLanguage0.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tableLayout.SetColumnSpan(this._otherLanguage0, 2);
			this._binder.SetIsBound(this._otherLanguage0, true);
			this._binder.SetIsComponentFileId(this._otherLanguage0, false);
			this.locExtender.SetLocalizableToolTip(this._otherLanguage0, null);
			this.locExtender.SetLocalizationComment(this._otherLanguage0, null);
			this.locExtender.SetLocalizationPriority(this._otherLanguage0, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._otherLanguage0, "PersonBasicEditor._otherLanguage0");
			this._otherLanguage0.Location = new System.Drawing.Point(0, 131);
			this._otherLanguage0.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage0.Name = "_otherLanguage0";
			this._otherLanguage0.Size = new System.Drawing.Size(180, 20);
			this._otherLanguage0.TabIndex = 13;
			this._autoCompleteHelper.SetUpdateGatherer(this._otherLanguage0, false);
			// 
			// _otherLanguage1
			// 
			this._otherLanguage1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._otherLanguage1, "language");
			this._otherLanguage1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._otherLanguage1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tableLayout.SetColumnSpan(this._otherLanguage1, 2);
			this._binder.SetIsBound(this._otherLanguage1, true);
			this._binder.SetIsComponentFileId(this._otherLanguage1, false);
			this.locExtender.SetLocalizableToolTip(this._otherLanguage1, null);
			this.locExtender.SetLocalizationComment(this._otherLanguage1, null);
			this.locExtender.SetLocalizationPriority(this._otherLanguage1, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._otherLanguage1, "PersonBasicEditor._otherLanguage1");
			this._otherLanguage1.Location = new System.Drawing.Point(0, 157);
			this._otherLanguage1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage1.Name = "_otherLanguage1";
			this._otherLanguage1.Size = new System.Drawing.Size(180, 20);
			this._otherLanguage1.TabIndex = 16;
			this._autoCompleteHelper.SetUpdateGatherer(this._otherLanguage1, false);
			// 
			// _otherLanguage2
			// 
			this._otherLanguage2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._otherLanguage2, "language");
			this._otherLanguage2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._otherLanguage2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tableLayout.SetColumnSpan(this._otherLanguage2, 2);
			this._binder.SetIsBound(this._otherLanguage2, true);
			this._binder.SetIsComponentFileId(this._otherLanguage2, false);
			this.locExtender.SetLocalizableToolTip(this._otherLanguage2, null);
			this.locExtender.SetLocalizationComment(this._otherLanguage2, null);
			this.locExtender.SetLocalizationPriority(this._otherLanguage2, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._otherLanguage2, "PersonBasicEditor._otherLanguage2");
			this._otherLanguage2.Location = new System.Drawing.Point(0, 185);
			this._otherLanguage2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage2.Name = "_otherLanguage2";
			this._otherLanguage2.Size = new System.Drawing.Size(180, 20);
			this._otherLanguage2.TabIndex = 19;
			this._autoCompleteHelper.SetUpdateGatherer(this._otherLanguage2, false);
			// 
			// _primaryLanguage
			// 
			this._primaryLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._primaryLanguage, "language");
			this._primaryLanguage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._primaryLanguage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tableLayout.SetColumnSpan(this._primaryLanguage, 2);
			this._binder.SetIsBound(this._primaryLanguage, true);
			this._binder.SetIsComponentFileId(this._primaryLanguage, false);
			this.locExtender.SetLocalizableToolTip(this._primaryLanguage, null);
			this.locExtender.SetLocalizationComment(this._primaryLanguage, null);
			this.locExtender.SetLocalizationPriority(this._primaryLanguage, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._primaryLanguage, "PersonBasicEditor._primaryLanguage");
			this._primaryLanguage.Location = new System.Drawing.Point(0, 60);
			this._primaryLanguage.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._primaryLanguage.Name = "_primaryLanguage";
			this._primaryLanguage.Size = new System.Drawing.Size(180, 20);
			this._primaryLanguage.TabIndex = 5;
			this._autoCompleteHelper.SetUpdateGatherer(this._primaryLanguage, false);
			// 
			// _labelPrimaryLanguage
			// 
			this._labelPrimaryLanguage.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelPrimaryLanguage, 4);
			this.locExtender.SetLocalizableToolTip(this._labelPrimaryLanguage, null);
			this.locExtender.SetLocalizationComment(this._labelPrimaryLanguage, null);
			this.locExtender.SetLocalizingId(this._labelPrimaryLanguage, "PeopleView.MetadataEditor._labelPrimaryLanguage");
			this._labelPrimaryLanguage.Location = new System.Drawing.Point(0, 44);
			this._labelPrimaryLanguage.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelPrimaryLanguage.Name = "_labelPrimaryLanguage";
			this._labelPrimaryLanguage.Size = new System.Drawing.Size(92, 13);
			this._labelPrimaryLanguage.TabIndex = 4;
			this._labelPrimaryLanguage.Text = "Primary &Language";
			// 
			// _pbPrimaryLangMother
			// 
			this._pbPrimaryLangMother.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbPrimaryLangMother, null);
			this.locExtender.SetLocalizationComment(this._pbPrimaryLangMother, null);
			this.locExtender.SetLocalizationPriority(this._pbPrimaryLangMother, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbPrimaryLangMother, "PersonBasicEditor._pbPrimaryLangMother");
			this._pbPrimaryLangMother.Location = new System.Drawing.Point(207, 58);
			this._pbPrimaryLangMother.Margin = new System.Windows.Forms.Padding(0, 1, 14, 0);
			this._pbPrimaryLangMother.Name = "_pbPrimaryLangMother";
			this._pbPrimaryLangMother.ParentType = SayMore.Utilities.LowLevelControls.ParentType.Mother;
			this._pbPrimaryLangMother.Selected = false;
			this._pbPrimaryLangMother.Size = new System.Drawing.Size(24, 24);
			this._pbPrimaryLangMother.TabIndex = 7;
			this._pbPrimaryLangMother.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbPrimaryLangFather
			// 
			this._pbPrimaryLangFather.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbPrimaryLangFather, null);
			this.locExtender.SetLocalizationComment(this._pbPrimaryLangFather, null);
			this.locExtender.SetLocalizationPriority(this._pbPrimaryLangFather, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbPrimaryLangFather, "PersonBasicEditor._pbPrimaryLangFather");
			this._pbPrimaryLangFather.Location = new System.Drawing.Point(183, 58);
			this._pbPrimaryLangFather.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbPrimaryLangFather.Name = "_pbPrimaryLangFather";
			this._pbPrimaryLangFather.ParentType = SayMore.Utilities.LowLevelControls.ParentType.Father;
			this._pbPrimaryLangFather.Selected = false;
			this._pbPrimaryLangFather.Size = new System.Drawing.Size(24, 24);
			this._pbPrimaryLangFather.TabIndex = 6;
			this._pbPrimaryLangFather.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _labelOtherLanguages
			// 
			this._labelOtherLanguages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelOtherLanguages.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelOtherLanguages, 4);
			this.locExtender.SetLocalizableToolTip(this._labelOtherLanguages, null);
			this.locExtender.SetLocalizationComment(this._labelOtherLanguages, null);
			this.locExtender.SetLocalizingId(this._labelOtherLanguages, "PeopleView.MetadataEditor._labelOtherLanguages");
			this._labelOtherLanguages.Location = new System.Drawing.Point(0, 115);
			this._labelOtherLanguages.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelOtherLanguages.Name = "_labelOtherLanguages";
			this._labelOtherLanguages.Size = new System.Drawing.Size(89, 13);
			this._labelOtherLanguages.TabIndex = 12;
			this._labelOtherLanguages.Text = "Other L&anguages";
			// 
			// _pbOtherLangMother2
			// 
			this._pbOtherLangMother2.AllowDrop = true;
			this._pbOtherLangMother2.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangMother2, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangMother2, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangMother2, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangMother2, "PersonBasicEditor._pbOtherLangMother2");
			this._pbOtherLangMother2.Location = new System.Drawing.Point(207, 183);
			this._pbOtherLangMother2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangMother2.Name = "_pbOtherLangMother2";
			this._pbOtherLangMother2.ParentType = SayMore.Utilities.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother2.Selected = false;
			this._pbOtherLangMother2.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother2.TabIndex = 21;
			this._pbOtherLangMother2.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			this._pbOtherLangMother2.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _labelEducation
			// 
			this._labelEducation.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelEducation, 4);
			this.locExtender.SetLocalizableToolTip(this._labelEducation, null);
			this.locExtender.SetLocalizationComment(this._labelEducation, null);
			this.locExtender.SetLocalizingId(this._labelEducation, "PeopleView.MetadataEditor._labelEducation");
			this._labelEducation.Location = new System.Drawing.Point(0, 241);
			this._labelEducation.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelEducation.Name = "_labelEducation";
			this._labelEducation.Size = new System.Drawing.Size(55, 13);
			this._labelEducation.TabIndex = 27;
			this._labelEducation.Text = "&Education";
			// 
			// _education
			// 
			this._education.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._education, "education");
			this._education.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._education.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tableLayout.SetColumnSpan(this._education, 2);
			this._binder.SetIsBound(this._education, true);
			this._binder.SetIsComponentFileId(this._education, false);
			this.locExtender.SetLocalizableToolTip(this._education, null);
			this.locExtender.SetLocalizationComment(this._education, null);
			this.locExtender.SetLocalizationPriority(this._education, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._education, "PersonBasicEditor._education");
			this._education.Location = new System.Drawing.Point(0, 257);
			this._education.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._education.Name = "_education";
			this._education.Size = new System.Drawing.Size(180, 20);
			this._education.TabIndex = 28;
			this._autoCompleteHelper.SetUpdateGatherer(this._education, false);
			// 
			// _labelPrimaryOccupation
			// 
			this._labelPrimaryOccupation.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelPrimaryOccupation, 4);
			this.locExtender.SetLocalizableToolTip(this._labelPrimaryOccupation, null);
			this.locExtender.SetLocalizationComment(this._labelPrimaryOccupation, null);
			this.locExtender.SetLocalizingId(this._labelPrimaryOccupation, "PeopleView.MetadataEditor._labelPrimaryOccupation");
			this._labelPrimaryOccupation.Location = new System.Drawing.Point(0, 285);
			this._labelPrimaryOccupation.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelPrimaryOccupation.Name = "_labelPrimaryOccupation";
			this._labelPrimaryOccupation.Size = new System.Drawing.Size(99, 13);
			this._labelPrimaryOccupation.TabIndex = 31;
			this._labelPrimaryOccupation.Text = "Primary &Occupation";
			// 
			// _primaryOccupation
			// 
			this._primaryOccupation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._primaryOccupation, "");
			this._tableLayout.SetColumnSpan(this._primaryOccupation, 2);
			this._binder.SetIsBound(this._primaryOccupation, true);
			this._binder.SetIsComponentFileId(this._primaryOccupation, false);
			this.locExtender.SetLocalizableToolTip(this._primaryOccupation, null);
			this.locExtender.SetLocalizationComment(this._primaryOccupation, null);
			this.locExtender.SetLocalizationPriority(this._primaryOccupation, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._primaryOccupation, "PersonBasicEditor._primaryOccupation");
			this._primaryOccupation.Location = new System.Drawing.Point(0, 301);
			this._primaryOccupation.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._primaryOccupation.Name = "_primaryOccupation";
			this._primaryOccupation.Size = new System.Drawing.Size(180, 20);
			this._primaryOccupation.TabIndex = 32;
			this._autoCompleteHelper.SetUpdateGatherer(this._primaryOccupation, false);
			// 
			// _labelgender
			// 
			this._labelgender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelgender.AutoEllipsis = true;
			this._labelgender.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelgender, null);
			this.locExtender.SetLocalizationComment(this._labelgender, null);
			this.locExtender.SetLocalizingId(this._labelgender, "PeopleView.MetadataEditor._labelgender");
			this._labelgender.Location = new System.Drawing.Point(252, 44);
			this._labelgender.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelgender.MinimumSize = new System.Drawing.Size(42, 0);
			this._labelgender.Name = "_labelgender";
			this._labelgender.Size = new System.Drawing.Size(42, 13);
			this._labelgender.TabIndex = 10;
			this._labelgender.Text = "&Gender";
			// 
			// _gender
			// 
			this._gender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._gender.FormattingEnabled = true;
			this._binder.SetIsBound(this._gender, true);
			this._binder.SetIsComponentFileId(this._gender, false);
			this.locExtender.SetLocalizableToolTip(this._gender, null);
			this.locExtender.SetLocalizationComment(this._gender, null);
			this.locExtender.SetLocalizationPriority(this._gender, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._gender, "PeopleView.MetadataEditor._genderDropDownList");
			this._gender.Location = new System.Drawing.Point(252, 60);
			this._gender.Margin = new System.Windows.Forms.Padding(7, 3, 3, 0);
			this._gender.Name = "_gender";
			this._gender.Size = new System.Drawing.Size(76, 21);
			this._gender.TabIndex = 11;
			// 
			// _labelHowToContact
			// 
			this._labelHowToContact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelHowToContact.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelHowToContact, null);
			this.locExtender.SetLocalizationComment(this._labelHowToContact, null);
			this.locExtender.SetLocalizingId(this._labelHowToContact, "PeopleView.MetadataEditor._labelHowToContact");
			this._labelHowToContact.Location = new System.Drawing.Point(252, 115);
			this._labelHowToContact.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelHowToContact.Name = "_labelHowToContact";
			this._labelHowToContact.Size = new System.Drawing.Size(81, 13);
			this._labelHowToContact.TabIndex = 25;
			this._labelHowToContact.Text = "&How to Contact";
			// 
			// _howToContact
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._howToContact, "");
			this._tableLayout.SetColumnSpan(this._howToContact, 2);
			this._howToContact.Dock = System.Windows.Forms.DockStyle.Top;
			this._binder.SetIsBound(this._howToContact, true);
			this._binder.SetIsComponentFileId(this._howToContact, false);
			this.locExtender.SetLocalizableToolTip(this._howToContact, null);
			this.locExtender.SetLocalizationComment(this._howToContact, null);
			this.locExtender.SetLocalizationPriority(this._howToContact, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._howToContact, "PersonBasicEditor._howToContact");
			this._howToContact.Location = new System.Drawing.Point(252, 131);
			this._howToContact.Margin = new System.Windows.Forms.Padding(7, 3, 0, 3);
			this._howToContact.Multiline = true;
			this._howToContact.Name = "_howToContact";
			this._tableLayout.SetRowSpan(this._howToContact, 3);
			this._howToContact.Size = new System.Drawing.Size(250, 76);
			this._howToContact.TabIndex = 26;
			this._autoCompleteHelper.SetUpdateGatherer(this._howToContact, false);
			// 
			// _labelCustomFields
			// 
			this._labelCustomFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelCustomFields.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelCustomFields, 2);
			this.locExtender.SetLocalizableToolTip(this._labelCustomFields, null);
			this.locExtender.SetLocalizationComment(this._labelCustomFields, null);
			this.locExtender.SetLocalizingId(this._labelCustomFields, "PeopleView.MetadataEditor._labelCustomFields");
			this._labelCustomFields.Location = new System.Drawing.Point(252, 223);
			this._labelCustomFields.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelCustomFields.Name = "_labelCustomFields";
			this._labelCustomFields.Size = new System.Drawing.Size(72, 13);
			this._labelCustomFields.TabIndex = 29;
			this._labelCustomFields.Text = "&Custom Fields";
			// 
			// _panelGrid
			// 
			this._panelGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayout.SetColumnSpan(this._panelGrid, 2);
			this._panelGrid.Location = new System.Drawing.Point(252, 239);
			this._panelGrid.Margin = new System.Windows.Forms.Padding(7, 3, 0, 3);
			this._panelGrid.Name = "_panelGrid";
			this._tableLayout.SetRowSpan(this._panelGrid, 4);
			this._panelGrid.Size = new System.Drawing.Size(250, 82);
			this._panelGrid.TabIndex = 30;
			// 
			// _panelPicture
			// 
			this._panelPicture.AllowDrop = true;
			this._panelPicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._panelPicture.Controls.Add(this._personsPicture);
			this._panelPicture.Location = new System.Drawing.Point(350, 0);
			this._panelPicture.Margin = new System.Windows.Forms.Padding(14, 0, 0, 0);
			this._panelPicture.Name = "_panelPicture";
			this._tableLayout.SetRowSpan(this._panelPicture, 6);
			this._panelPicture.Size = new System.Drawing.Size(152, 124);
			this._panelPicture.TabIndex = 33;
			this._panelPicture.DragDrop += new System.Windows.Forms.DragEventHandler(this.HandlePictureDragDrop);
			this._panelPicture.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandlePictureDragEnter);
			this._panelPicture.DragLeave += new System.EventHandler(this.HandlePictureDragLeave);
			// 
			// _personsPicture
			// 
			this._personsPicture.Cursor = System.Windows.Forms.Cursors.Hand;
			this._personsPicture.Dock = System.Windows.Forms.DockStyle.Fill;
			this._personsPicture.ErrorImage = null;
			this._personsPicture.Image = global::SayMore.Properties.Resources.kimidNoPhoto;
			this._personsPicture.InitialImage = null;
			this.locExtender.SetLocalizableToolTip(this._personsPicture, "Click to Change Picture");
			this.locExtender.SetLocalizationComment(this._personsPicture, null);
			this.locExtender.SetLocalizingId(this._personsPicture, "PeopleView.MetadataEditor._personsPicture");
			this._personsPicture.Location = new System.Drawing.Point(0, 0);
			this._personsPicture.Margin = new System.Windows.Forms.Padding(14, 0, 0, 0);
			this._personsPicture.Name = "_personsPicture";
			this._personsPicture.Size = new System.Drawing.Size(152, 124);
			this._personsPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._personsPicture.TabIndex = 9;
			this._personsPicture.TabStop = false;
			this._personsPicture.Paint += new System.Windows.Forms.PaintEventHandler(this.HandlePersonPicturePaint);
			this._personsPicture.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HandlePersonPictureMouseClick);
			this._personsPicture.MouseEnter += new System.EventHandler(this.HandlePersonPictureMouseEnterLeave);
			this._personsPicture.MouseLeave += new System.EventHandler(this.HandlePersonPictureMouseEnterLeave);
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// PersonBasicEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(207)))), ((int)(((byte)(219)))));
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "PersonBasicEditor.EditorBase");
			this.Name = "PersonBasicEditor";
			this.Size = new System.Drawing.Size(516, 347);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this._panelPicture.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._personsPicture)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BindingHelper _binder;
		private AutoCompleteHelper _autoCompleteHelper;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Label _labelFullName;
		private System.Windows.Forms.Label _labelBirthYear;
		private System.Windows.Forms.Label _labelgender;
		private System.Windows.Forms.Label _labelHowToContact;
		private System.Windows.Forms.Label _labelCustomFields;
		private System.Windows.Forms.Label _labelEducation;
		private System.Windows.Forms.Label _labelPrimaryOccupation;
		private System.Windows.Forms.ComboBox _gender;
		private System.Windows.Forms.TextBox _id;
		private System.Windows.Forms.TextBox _birthYear;
		private System.Windows.Forms.TextBox _otherLanguage0;
		private System.Windows.Forms.TextBox _otherLanguage1;
		private System.Windows.Forms.TextBox _otherLanguage2;
		private System.Windows.Forms.TextBox _otherLanguage3;
		private System.Windows.Forms.TextBox _howToContact;
		private System.Windows.Forms.TextBox _education;
		private System.Windows.Forms.TextBox _primaryOccupation;
		private SayMore.Utilities.LowLevelControls.ParentButton _pbOtherLangMother0;
		private SayMore.Utilities.LowLevelControls.ParentButton _pbOtherLangFather0;
		private SayMore.Utilities.LowLevelControls.ParentButton _pbOtherLangMother3;
		private SayMore.Utilities.LowLevelControls.ParentButton _pbOtherLangMother2;
		private SayMore.Utilities.LowLevelControls.ParentButton _pbOtherLangMother1;
		private SayMore.Utilities.LowLevelControls.ParentButton _pbOtherLangFather1;
		private SayMore.Utilities.LowLevelControls.ParentButton _pbOtherLangFather2;
		private SayMore.Utilities.LowLevelControls.ParentButton _pbOtherLangFather3;
		private System.Windows.Forms.Panel _panelGrid;
		private ParentButton _pbPrimaryLangMother;
		private ParentButton _pbPrimaryLangFather;
		private System.Windows.Forms.Label _labelPrimaryLanguageLearnedIn;
		private System.Windows.Forms.TextBox _primaryLanguage;
		private System.Windows.Forms.TextBox _primaryLanguageLearnedIn;
		private System.Windows.Forms.Label _labelPrimaryLanguage;
		private System.Windows.Forms.Label _labelOtherLanguages;
		private System.Windows.Forms.ToolTip _tooltip;
		private System.Windows.Forms.PictureBox _personsPicture;
		private System.Windows.Forms.Panel _panelPicture;
		private Localization.UI.LocalizationExtender locExtender;
	}
}
