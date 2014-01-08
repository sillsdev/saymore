using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ComponentEditors
{
	sealed partial class PersonBasicEditor
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
			this._labelAge = new System.Windows.Forms.Label();
			this._age = new System.Windows.Forms.TextBox();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._ethnicGroup = new System.Windows.Forms.TextBox();
			this._labelEthnicGroup = new System.Windows.Forms.Label();
			this._nickName = new System.Windows.Forms.TextBox();
			this._labelNickName = new System.Windows.Forms.Label();
			this._pbOtherLangMother3 = new SayMore.UI.LowLevelControls.ParentButton();
			this._primaryLanguageLearnedIn = new System.Windows.Forms.TextBox();
			this._labelPrimaryLanguageLearnedIn = new System.Windows.Forms.Label();
			this._pbOtherLangMother1 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangFather3 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangFather2 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangFather1 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangMother0 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangFather0 = new SayMore.UI.LowLevelControls.ParentButton();
			this._otherLanguage3 = new System.Windows.Forms.TextBox();
			this._otherLanguage0 = new System.Windows.Forms.TextBox();
			this._otherLanguage1 = new System.Windows.Forms.TextBox();
			this._otherLanguage2 = new System.Windows.Forms.TextBox();
			this._primaryLanguage = new System.Windows.Forms.TextBox();
			this._labelPrimaryLanguage = new System.Windows.Forms.Label();
			this._pbPrimaryLangMother = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbPrimaryLangFather = new SayMore.UI.LowLevelControls.ParentButton();
			this._labelOtherLanguages = new System.Windows.Forms.Label();
			this._pbOtherLangMother2 = new SayMore.UI.LowLevelControls.ParentButton();
			this._labelEducation = new System.Windows.Forms.Label();
			this._education = new System.Windows.Forms.TextBox();
			this._labelPrimaryOccupation = new System.Windows.Forms.Label();
			this._primaryOccupation = new System.Windows.Forms.TextBox();
			this._labelgender = new System.Windows.Forms.Label();
			this._gender = new System.Windows.Forms.ComboBox();
			this._labelHowToContact = new System.Windows.Forms.Label();
			this._howToContact = new System.Windows.Forms.TextBox();
			this._panelGrid = new System.Windows.Forms.Panel();
			this._panelPicture = new System.Windows.Forms.Panel();
			this._personsPicture = new System.Windows.Forms.PictureBox();
			this._labelCustomFields = new System.Windows.Forms.Label();
			this._labelSessions = new System.Windows.Forms.Label();
			this._sessionList = new System.Windows.Forms.ListBox();
			this._panelPrivacy = new System.Windows.Forms.FlowLayoutPanel();
			this._labelPrivacy = new System.Windows.Forms.Label();
			this._privacyProtection = new System.Windows.Forms.CheckBox();
			this._binder = new SayMore.UI.ComponentEditors.BindingHelper(this.components);
			this._autoCompleteHelper = new SayMore.UI.ComponentEditors.AutoCompleteHelper(this.components);
			this._tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._tableLayout.SuspendLayout();
			this._panelPicture.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._personsPicture)).BeginInit();
			this._panelPrivacy.SuspendLayout();
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
			this.locExtender.SetLocalizationPriority(this._id, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._id, "PersonBasicEditor._id");
			this._id.Location = new System.Drawing.Point(0, 16);
			this._id.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._id.Name = "_id";
			this._id.Size = new System.Drawing.Size(173, 20);
			this._id.TabIndex = 1;
			this._autoCompleteHelper.SetUpdateGatherer(this._id, false);
			// 
			// _labelAge
			// 
			this._labelAge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelAge.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelAge, null);
			this.locExtender.SetLocalizationComment(this._labelAge, null);
			this.locExtender.SetLocalizingId(this._labelAge, "PeopleView.MetadataEditor._labelAge");
			this._labelAge.Location = new System.Drawing.Point(245, 0);
			this._labelAge.Margin = new System.Windows.Forms.Padding(7, 0, 3, 0);
			this._labelAge.Name = "_labelAge";
			this._labelAge.Size = new System.Drawing.Size(26, 13);
			this._labelAge.TabIndex = 2;
			this._labelAge.Text = "Age";
			// 
			// _age
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._age, "");
			this._age.Dock = System.Windows.Forms.DockStyle.Top;
			this._binder.SetIsBound(this._age, true);
			this._binder.SetIsComponentFileId(this._age, false);
			this.locExtender.SetLocalizableToolTip(this._age, null);
			this.locExtender.SetLocalizationComment(this._age, null);
			this.locExtender.SetLocalizationPriority(this._age, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._age, "PersonBasicEditor._birthYear");
			this._age.Location = new System.Drawing.Point(245, 16);
			this._age.Margin = new System.Windows.Forms.Padding(7, 3, 3, 0);
			this._age.Name = "_age";
			this._age.Size = new System.Drawing.Size(81, 20);
			this._age.TabIndex = 3;
			this._autoCompleteHelper.SetUpdateGatherer(this._age, false);
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
			this._tableLayout.Controls.Add(this._ethnicGroup, 4, 12);
			this._tableLayout.Controls.Add(this._labelEthnicGroup, 4, 11);
			this._tableLayout.Controls.Add(this._nickName, 0, 3);
			this._tableLayout.Controls.Add(this._labelNickName, 0, 2);
			this._tableLayout.Controls.Add(this._pbOtherLangMother3, 3, 12);
			this._tableLayout.Controls.Add(this._primaryLanguageLearnedIn, 1, 7);
			this._tableLayout.Controls.Add(this._labelPrimaryLanguageLearnedIn, 0, 7);
			this._tableLayout.Controls.Add(this._pbOtherLangMother1, 3, 10);
			this._tableLayout.Controls.Add(this._pbOtherLangFather3, 2, 12);
			this._tableLayout.Controls.Add(this._pbOtherLangFather2, 2, 11);
			this._tableLayout.Controls.Add(this._pbOtherLangFather1, 2, 10);
			this._tableLayout.Controls.Add(this._pbOtherLangMother0, 3, 9);
			this._tableLayout.Controls.Add(this._pbOtherLangFather0, 2, 9);
			this._tableLayout.Controls.Add(this._otherLanguage3, 0, 12);
			this._tableLayout.Controls.Add(this._labelFullName, 0, 0);
			this._tableLayout.Controls.Add(this._otherLanguage0, 0, 9);
			this._tableLayout.Controls.Add(this._otherLanguage1, 0, 10);
			this._tableLayout.Controls.Add(this._otherLanguage2, 0, 11);
			this._tableLayout.Controls.Add(this._primaryLanguage, 0, 6);
			this._tableLayout.Controls.Add(this._id, 0, 1);
			this._tableLayout.Controls.Add(this._labelPrimaryLanguage, 0, 5);
			this._tableLayout.Controls.Add(this._pbPrimaryLangMother, 3, 6);
			this._tableLayout.Controls.Add(this._pbPrimaryLangFather, 2, 6);
			this._tableLayout.Controls.Add(this._labelOtherLanguages, 0, 8);
			this._tableLayout.Controls.Add(this._pbOtherLangMother2, 3, 11);
			this._tableLayout.Controls.Add(this._labelEducation, 0, 13);
			this._tableLayout.Controls.Add(this._education, 0, 14);
			this._tableLayout.Controls.Add(this._labelPrimaryOccupation, 4, 13);
			this._tableLayout.Controls.Add(this._primaryOccupation, 4, 14);
			this._tableLayout.Controls.Add(this._labelAge, 4, 0);
			this._tableLayout.Controls.Add(this._age, 4, 1);
			this._tableLayout.Controls.Add(this._labelgender, 4, 2);
			this._tableLayout.Controls.Add(this._gender, 4, 3);
			this._tableLayout.Controls.Add(this._labelHowToContact, 4, 5);
			this._tableLayout.Controls.Add(this._howToContact, 4, 6);
			this._tableLayout.Controls.Add(this._panelGrid, 4, 17);
			this._tableLayout.Controls.Add(this._panelPicture, 5, 0);
			this._tableLayout.Controls.Add(this._labelCustomFields, 4, 16);
			this._tableLayout.Controls.Add(this._labelSessions, 0, 16);
			this._tableLayout.Controls.Add(this._sessionList, 0, 17);
			this._tableLayout.Controls.Add(this._panelPrivacy, 4, 10);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayout.Location = new System.Drawing.Point(7, 7);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 18;
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
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this._tableLayout.Size = new System.Drawing.Size(485, 422);
			this._tableLayout.TabIndex = 0;
			this._tableLayout.TabStop = true;
			this._tableLayout.Paint += new System.Windows.Forms.PaintEventHandler(this.HandleTableLayoutPaint);
			// 
			// _ethnicGroup
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._ethnicGroup, "");
			this._tableLayout.SetColumnSpan(this._ethnicGroup, 2);
			this._ethnicGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this._binder.SetIsBound(this._ethnicGroup, true);
			this._binder.SetIsComponentFileId(this._ethnicGroup, false);
			this.locExtender.SetLocalizableToolTip(this._ethnicGroup, null);
			this.locExtender.SetLocalizationComment(this._ethnicGroup, null);
			this.locExtender.SetLocalizationPriority(this._ethnicGroup, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._ethnicGroup, "PersonBasicEditor._birthYear");
			this._ethnicGroup.Location = new System.Drawing.Point(245, 291);
			this._ethnicGroup.Margin = new System.Windows.Forms.Padding(7, 3, 0, 3);
			this._ethnicGroup.Name = "_ethnicGroup";
			this._ethnicGroup.Size = new System.Drawing.Size(240, 20);
			this._ethnicGroup.TabIndex = 32;
			this._autoCompleteHelper.SetUpdateGatherer(this._ethnicGroup, false);
			// 
			// _labelEthnicGroup
			// 
			this._labelEthnicGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelEthnicGroup.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelEthnicGroup, null);
			this.locExtender.SetLocalizationComment(this._labelEthnicGroup, null);
			this.locExtender.SetLocalizingId(this._labelEthnicGroup, "PeopleView.MetadataEditor._labelEthnicGroup");
			this._labelEthnicGroup.Location = new System.Drawing.Point(245, 275);
			this._labelEthnicGroup.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelEthnicGroup.Name = "_labelEthnicGroup";
			this._labelEthnicGroup.Size = new System.Drawing.Size(69, 13);
			this._labelEthnicGroup.TabIndex = 31;
			this._labelEthnicGroup.Text = "Ethnic Group";
			// 
			// _nickName
			// 
			this._nickName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._nickName, "");
			this._tableLayout.SetColumnSpan(this._nickName, 2);
			this._binder.SetIsBound(this._nickName, true);
			this._binder.SetIsComponentFileId(this._nickName, false);
			this.locExtender.SetLocalizableToolTip(this._nickName, null);
			this.locExtender.SetLocalizationComment(this._nickName, null);
			this.locExtender.SetLocalizationPriority(this._nickName, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._nickName, "PersonBasicEditor._nickName");
			this._nickName.Location = new System.Drawing.Point(0, 60);
			this._nickName.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._nickName.Name = "_nickName";
			this._nickName.Size = new System.Drawing.Size(173, 20);
			this._nickName.TabIndex = 5;
			this._autoCompleteHelper.SetUpdateGatherer(this._nickName, false);
			// 
			// _labelNickName
			// 
			this._labelNickName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelNickName.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelNickName, 4);
			this.locExtender.SetLocalizableToolTip(this._labelNickName, null);
			this.locExtender.SetLocalizationComment(this._labelNickName, null);
			this.locExtender.SetLocalizingId(this._labelNickName, "PeopleView.MetadataEditor._labelNickName");
			this._labelNickName.Location = new System.Drawing.Point(0, 44);
			this._labelNickName.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this._labelNickName.Name = "_labelNickName";
			this._labelNickName.Size = new System.Drawing.Size(60, 13);
			this._labelNickName.TabIndex = 4;
			this._labelNickName.Text = "Nick Name";
			// 
			// _pbOtherLangMother3
			// 
			this._pbOtherLangMother3.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangMother3, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangMother3, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangMother3, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangMother3, "PersonBasicEditor._pbOtherLangMother3");
			this._pbOtherLangMother3.Location = new System.Drawing.Point(200, 289);
			this._pbOtherLangMother3.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangMother3.Name = "_pbOtherLangMother3";
			this._pbOtherLangMother3.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother3.Selected = false;
			this._pbOtherLangMother3.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother3.TabIndex = 28;
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
			this.locExtender.SetLocalizationPriority(this._primaryLanguageLearnedIn, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._primaryLanguageLearnedIn, "PersonBasicEditor._primaryLanguageLearnedIn");
			this._primaryLanguageLearnedIn.Location = new System.Drawing.Point(64, 153);
			this._primaryLanguageLearnedIn.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
			this._primaryLanguageLearnedIn.Name = "_primaryLanguageLearnedIn";
			this._primaryLanguageLearnedIn.Size = new System.Drawing.Size(109, 20);
			this._primaryLanguageLearnedIn.TabIndex = 13;
			this._autoCompleteHelper.SetUpdateGatherer(this._primaryLanguageLearnedIn, false);
			// 
			// _labelPrimaryLanguageLearnedIn
			// 
			this._labelPrimaryLanguageLearnedIn.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelPrimaryLanguageLearnedIn, null);
			this.locExtender.SetLocalizationComment(this._labelPrimaryLanguageLearnedIn, null);
			this.locExtender.SetLocalizingId(this._labelPrimaryLanguageLearnedIn, "PeopleView.MetadataEditor._labelPrimaryLanguageLearnedIn");
			this._labelPrimaryLanguageLearnedIn.Location = new System.Drawing.Point(0, 157);
			this._labelPrimaryLanguageLearnedIn.Margin = new System.Windows.Forms.Padding(0, 7, 3, 7);
			this._labelPrimaryLanguageLearnedIn.Name = "_labelPrimaryLanguageLearnedIn";
			this._labelPrimaryLanguageLearnedIn.Size = new System.Drawing.Size(61, 13);
			this._labelPrimaryLanguageLearnedIn.TabIndex = 12;
			this._labelPrimaryLanguageLearnedIn.Text = "Learned &In:";
			// 
			// _pbOtherLangMother1
			// 
			this._pbOtherLangMother1.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangMother1, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangMother1, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangMother1, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangMother1, "PersonBasicEditor._pbOtherLangMother1");
			this._pbOtherLangMother1.Location = new System.Drawing.Point(200, 233);
			this._pbOtherLangMother1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangMother1.Name = "_pbOtherLangMother1";
			this._pbOtherLangMother1.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother1.Selected = false;
			this._pbOtherLangMother1.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother1.TabIndex = 22;
			this._pbOtherLangMother1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			this._pbOtherLangMother1.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbOtherLangFather3
			// 
			this._pbOtherLangFather3.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangFather3, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangFather3, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangFather3, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangFather3, "PersonBasicEditor._pbOtherLangFather3");
			this._pbOtherLangFather3.Location = new System.Drawing.Point(176, 289);
			this._pbOtherLangFather3.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangFather3.Name = "_pbOtherLangFather3";
			this._pbOtherLangFather3.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather3.Selected = false;
			this._pbOtherLangFather3.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather3.TabIndex = 27;
			this._pbOtherLangFather3.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			this._pbOtherLangFather3.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbOtherLangFather2
			// 
			this._pbOtherLangFather2.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangFather2, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangFather2, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangFather2, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangFather2, "PersonBasicEditor._pbOtherLangFather2");
			this._pbOtherLangFather2.Location = new System.Drawing.Point(176, 261);
			this._pbOtherLangFather2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangFather2.Name = "_pbOtherLangFather2";
			this._pbOtherLangFather2.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather2.Selected = false;
			this._pbOtherLangFather2.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather2.TabIndex = 24;
			this._pbOtherLangFather2.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			this._pbOtherLangFather2.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbOtherLangFather1
			// 
			this._pbOtherLangFather1.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangFather1, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangFather1, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangFather1, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangFather1, "PersonBasicEditor._pbOtherLangFather1");
			this._pbOtherLangFather1.Location = new System.Drawing.Point(176, 233);
			this._pbOtherLangFather1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangFather1.Name = "_pbOtherLangFather1";
			this._pbOtherLangFather1.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather1.Selected = false;
			this._pbOtherLangFather1.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather1.TabIndex = 21;
			this._pbOtherLangFather1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			this._pbOtherLangFather1.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbOtherLangMother0
			// 
			this._pbOtherLangMother0.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangMother0, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangMother0, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangMother0, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangMother0, "PersonBasicEditor._pbOtherLangMother0");
			this._pbOtherLangMother0.Location = new System.Drawing.Point(200, 207);
			this._pbOtherLangMother0.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangMother0.Name = "_pbOtherLangMother0";
			this._pbOtherLangMother0.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother0.Selected = false;
			this._pbOtherLangMother0.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother0.TabIndex = 19;
			this._pbOtherLangMother0.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			this._pbOtherLangMother0.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbOtherLangFather0
			// 
			this._pbOtherLangFather0.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangFather0, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangFather0, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangFather0, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangFather0, "PersonBasicEditor._pbOtherLangFather0");
			this._pbOtherLangFather0.Location = new System.Drawing.Point(176, 207);
			this._pbOtherLangFather0.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangFather0.Name = "_pbOtherLangFather0";
			this._pbOtherLangFather0.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather0.Selected = false;
			this._pbOtherLangFather0.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather0.TabIndex = 18;
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
			this.locExtender.SetLocalizationPriority(this._otherLanguage3, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._otherLanguage3, "PersonBasicEditor._otherLanguage3");
			this._otherLanguage3.Location = new System.Drawing.Point(0, 291);
			this._otherLanguage3.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage3.Name = "_otherLanguage3";
			this._otherLanguage3.Size = new System.Drawing.Size(173, 20);
			this._otherLanguage3.TabIndex = 26;
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
			this.locExtender.SetLocalizationPriority(this._otherLanguage0, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._otherLanguage0, "PersonBasicEditor._otherLanguage0");
			this._otherLanguage0.Location = new System.Drawing.Point(0, 209);
			this._otherLanguage0.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage0.Name = "_otherLanguage0";
			this._otherLanguage0.Size = new System.Drawing.Size(173, 20);
			this._otherLanguage0.TabIndex = 17;
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
			this.locExtender.SetLocalizationPriority(this._otherLanguage1, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._otherLanguage1, "PersonBasicEditor._otherLanguage1");
			this._otherLanguage1.Location = new System.Drawing.Point(0, 235);
			this._otherLanguage1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage1.Name = "_otherLanguage1";
			this._otherLanguage1.Size = new System.Drawing.Size(173, 20);
			this._otherLanguage1.TabIndex = 20;
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
			this.locExtender.SetLocalizationPriority(this._otherLanguage2, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._otherLanguage2, "PersonBasicEditor._otherLanguage2");
			this._otherLanguage2.Location = new System.Drawing.Point(0, 263);
			this._otherLanguage2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage2.Name = "_otherLanguage2";
			this._otherLanguage2.Size = new System.Drawing.Size(173, 20);
			this._otherLanguage2.TabIndex = 23;
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
			this.locExtender.SetLocalizationPriority(this._primaryLanguage, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._primaryLanguage, "PersonBasicEditor._primaryLanguage");
			this._primaryLanguage.Location = new System.Drawing.Point(0, 127);
			this._primaryLanguage.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._primaryLanguage.Name = "_primaryLanguage";
			this._primaryLanguage.Size = new System.Drawing.Size(173, 20);
			this._primaryLanguage.TabIndex = 9;
			this._autoCompleteHelper.SetUpdateGatherer(this._primaryLanguage, false);
			// 
			// _labelPrimaryLanguage
			// 
			this._labelPrimaryLanguage.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelPrimaryLanguage, 4);
			this.locExtender.SetLocalizableToolTip(this._labelPrimaryLanguage, null);
			this.locExtender.SetLocalizationComment(this._labelPrimaryLanguage, null);
			this.locExtender.SetLocalizingId(this._labelPrimaryLanguage, "PeopleView.MetadataEditor._labelPrimaryLanguage");
			this._labelPrimaryLanguage.Location = new System.Drawing.Point(0, 111);
			this._labelPrimaryLanguage.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelPrimaryLanguage.Name = "_labelPrimaryLanguage";
			this._labelPrimaryLanguage.Size = new System.Drawing.Size(92, 13);
			this._labelPrimaryLanguage.TabIndex = 8;
			this._labelPrimaryLanguage.Text = "Primary &Language";
			// 
			// _pbPrimaryLangMother
			// 
			this._pbPrimaryLangMother.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbPrimaryLangMother, null);
			this.locExtender.SetLocalizationComment(this._pbPrimaryLangMother, null);
			this.locExtender.SetLocalizationPriority(this._pbPrimaryLangMother, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbPrimaryLangMother, "PersonBasicEditor._pbPrimaryLangMother");
			this._pbPrimaryLangMother.Location = new System.Drawing.Point(200, 125);
			this._pbPrimaryLangMother.Margin = new System.Windows.Forms.Padding(0, 1, 14, 0);
			this._pbPrimaryLangMother.Name = "_pbPrimaryLangMother";
			this._pbPrimaryLangMother.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbPrimaryLangMother.Selected = false;
			this._pbPrimaryLangMother.Size = new System.Drawing.Size(24, 24);
			this._pbPrimaryLangMother.TabIndex = 11;
			this._pbPrimaryLangMother.MouseEnter += new System.EventHandler(this.HandleParentLanguageButtonMouseEnter);
			// 
			// _pbPrimaryLangFather
			// 
			this._pbPrimaryLangFather.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbPrimaryLangFather, null);
			this.locExtender.SetLocalizationComment(this._pbPrimaryLangFather, null);
			this.locExtender.SetLocalizationPriority(this._pbPrimaryLangFather, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbPrimaryLangFather, "PersonBasicEditor._pbPrimaryLangFather");
			this._pbPrimaryLangFather.Location = new System.Drawing.Point(176, 125);
			this._pbPrimaryLangFather.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbPrimaryLangFather.Name = "_pbPrimaryLangFather";
			this._pbPrimaryLangFather.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbPrimaryLangFather.Selected = false;
			this._pbPrimaryLangFather.Size = new System.Drawing.Size(24, 24);
			this._pbPrimaryLangFather.TabIndex = 10;
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
			this._labelOtherLanguages.Location = new System.Drawing.Point(0, 193);
			this._labelOtherLanguages.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelOtherLanguages.Name = "_labelOtherLanguages";
			this._labelOtherLanguages.Size = new System.Drawing.Size(89, 13);
			this._labelOtherLanguages.TabIndex = 16;
			this._labelOtherLanguages.Text = "Other L&anguages";
			// 
			// _pbOtherLangMother2
			// 
			this._pbOtherLangMother2.AllowDrop = true;
			this._pbOtherLangMother2.BackColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._pbOtherLangMother2, null);
			this.locExtender.SetLocalizationComment(this._pbOtherLangMother2, null);
			this.locExtender.SetLocalizationPriority(this._pbOtherLangMother2, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._pbOtherLangMother2, "PersonBasicEditor._pbOtherLangMother2");
			this._pbOtherLangMother2.Location = new System.Drawing.Point(200, 261);
			this._pbOtherLangMother2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangMother2.Name = "_pbOtherLangMother2";
			this._pbOtherLangMother2.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother2.Selected = false;
			this._pbOtherLangMother2.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother2.TabIndex = 25;
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
			this._labelEducation.Location = new System.Drawing.Point(0, 319);
			this._labelEducation.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelEducation.Name = "_labelEducation";
			this._labelEducation.Size = new System.Drawing.Size(55, 13);
			this._labelEducation.TabIndex = 33;
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
			this.locExtender.SetLocalizationPriority(this._education, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._education, "PersonBasicEditor._education");
			this._education.Location = new System.Drawing.Point(0, 335);
			this._education.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._education.Name = "_education";
			this._education.Size = new System.Drawing.Size(173, 20);
			this._education.TabIndex = 34;
			this._autoCompleteHelper.SetUpdateGatherer(this._education, false);
			// 
			// _labelPrimaryOccupation
			// 
			this._labelPrimaryOccupation.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelPrimaryOccupation, 2);
			this.locExtender.SetLocalizableToolTip(this._labelPrimaryOccupation, null);
			this.locExtender.SetLocalizationComment(this._labelPrimaryOccupation, null);
			this.locExtender.SetLocalizingId(this._labelPrimaryOccupation, "PeopleView.MetadataEditor._labelPrimaryOccupation");
			this._labelPrimaryOccupation.Location = new System.Drawing.Point(245, 319);
			this._labelPrimaryOccupation.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelPrimaryOccupation.Name = "_labelPrimaryOccupation";
			this._labelPrimaryOccupation.Size = new System.Drawing.Size(99, 13);
			this._labelPrimaryOccupation.TabIndex = 35;
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
			this.locExtender.SetLocalizationPriority(this._primaryOccupation, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._primaryOccupation, "PersonBasicEditor._primaryOccupation");
			this._primaryOccupation.Location = new System.Drawing.Point(245, 335);
			this._primaryOccupation.Margin = new System.Windows.Forms.Padding(7, 3, 0, 3);
			this._primaryOccupation.Name = "_primaryOccupation";
			this._primaryOccupation.Size = new System.Drawing.Size(240, 20);
			this._primaryOccupation.TabIndex = 36;
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
			this._labelgender.Location = new System.Drawing.Point(245, 44);
			this._labelgender.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelgender.MinimumSize = new System.Drawing.Size(42, 0);
			this._labelgender.Name = "_labelgender";
			this._labelgender.Size = new System.Drawing.Size(42, 13);
			this._labelgender.TabIndex = 6;
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
			this.locExtender.SetLocalizationPriority(this._gender, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._gender, "PeopleView.MetadataEditor._genderDropDownList");
			this._gender.Location = new System.Drawing.Point(245, 60);
			this._gender.Margin = new System.Windows.Forms.Padding(7, 3, 3, 0);
			this._gender.Name = "_gender";
			this._gender.Size = new System.Drawing.Size(76, 21);
			this._gender.TabIndex = 7;
			// 
			// _labelHowToContact
			// 
			this._labelHowToContact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelHowToContact.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelHowToContact, null);
			this.locExtender.SetLocalizationComment(this._labelHowToContact, null);
			this.locExtender.SetLocalizingId(this._labelHowToContact, "PeopleView.MetadataEditor._labelHowToContact");
			this._labelHowToContact.Location = new System.Drawing.Point(245, 111);
			this._labelHowToContact.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelHowToContact.Name = "_labelHowToContact";
			this._labelHowToContact.Size = new System.Drawing.Size(81, 13);
			this._labelHowToContact.TabIndex = 14;
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
			this.locExtender.SetLocalizationPriority(this._howToContact, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._howToContact, "PersonBasicEditor._howToContact");
			this._howToContact.Location = new System.Drawing.Point(245, 127);
			this._howToContact.Margin = new System.Windows.Forms.Padding(7, 3, 0, 3);
			this._howToContact.Multiline = true;
			this._howToContact.Name = "_howToContact";
			this._tableLayout.SetRowSpan(this._howToContact, 3);
			this._howToContact.Size = new System.Drawing.Size(240, 76);
			this._howToContact.TabIndex = 15;
			this._autoCompleteHelper.SetUpdateGatherer(this._howToContact, false);
			// 
			// _panelGrid
			// 
			this._panelGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._panelGrid.AutoSize = true;
			this._panelGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.SetColumnSpan(this._panelGrid, 2);
			this._panelGrid.Location = new System.Drawing.Point(245, 399);
			this._panelGrid.Margin = new System.Windows.Forms.Padding(7, 3, 0, 3);
			this._panelGrid.MinimumSize = new System.Drawing.Size(0, 20);
			this._panelGrid.Name = "_panelGrid";
			this._panelGrid.Size = new System.Drawing.Size(240, 20);
			this._panelGrid.TabIndex = 40;
			// 
			// _panelPicture
			// 
			this._panelPicture.AllowDrop = true;
			this._panelPicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._panelPicture.Controls.Add(this._personsPicture);
			this._panelPicture.Location = new System.Drawing.Point(343, 0);
			this._panelPicture.Margin = new System.Windows.Forms.Padding(14, 0, 0, 0);
			this._panelPicture.Name = "_panelPicture";
			this._tableLayout.SetRowSpan(this._panelPicture, 6);
			this._panelPicture.Size = new System.Drawing.Size(142, 124);
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
			this._personsPicture.Size = new System.Drawing.Size(142, 124);
			this._personsPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._personsPicture.TabIndex = 9;
			this._personsPicture.TabStop = false;
			this._personsPicture.Paint += new System.Windows.Forms.PaintEventHandler(this.HandlePersonPicturePaint);
			this._personsPicture.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HandlePersonPictureMouseClick);
			this._personsPicture.MouseEnter += new System.EventHandler(this.HandlePersonPictureMouseEnterLeave);
			this._personsPicture.MouseLeave += new System.EventHandler(this.HandlePersonPictureMouseEnterLeave);
			// 
			// _labelCustomFields
			// 
			this._labelCustomFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelCustomFields.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelCustomFields, 2);
			this.locExtender.SetLocalizableToolTip(this._labelCustomFields, null);
			this.locExtender.SetLocalizationComment(this._labelCustomFields, null);
			this.locExtender.SetLocalizingId(this._labelCustomFields, "PeopleView.MetadataEditor._labelCustomFields");
			this._labelCustomFields.Location = new System.Drawing.Point(245, 383);
			this._labelCustomFields.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelCustomFields.Name = "_labelCustomFields";
			this._labelCustomFields.Size = new System.Drawing.Size(72, 13);
			this._labelCustomFields.TabIndex = 39;
			this._labelCustomFields.Text = "&Custom Fields";
			// 
			// _labelSessions
			// 
			this._labelSessions.AutoSize = true;
			this._tableLayout.SetColumnSpan(this._labelSessions, 2);
			this.locExtender.SetLocalizableToolTip(this._labelSessions, null);
			this.locExtender.SetLocalizationComment(this._labelSessions, null);
			this.locExtender.SetLocalizingId(this._labelSessions, "PeopleView.MetadataEditor._labelAssociatedSessions");
			this._labelSessions.Location = new System.Drawing.Point(0, 383);
			this._labelSessions.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelSessions.Name = "_labelSessions";
			this._labelSessions.Size = new System.Drawing.Size(104, 13);
			this._labelSessions.TabIndex = 37;
			this._labelSessions.Text = "Associated Sessions";
			// 
			// _sessionList
			// 
			this._tableLayout.SetColumnSpan(this._sessionList, 2);
			this._sessionList.Dock = System.Windows.Forms.DockStyle.Top;
			this._sessionList.FormattingEnabled = true;
			this._sessionList.IntegralHeight = false;
			this._sessionList.Location = new System.Drawing.Point(0, 399);
			this._sessionList.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._sessionList.MinimumSize = new System.Drawing.Size(4, 20);
			this._sessionList.Name = "_sessionList";
			this._sessionList.Size = new System.Drawing.Size(173, 20);
			this._sessionList.TabIndex = 38;
			// 
			// _panelPrivacy
			// 
			this._panelPrivacy.AutoSize = true;
			this._panelPrivacy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayout.SetColumnSpan(this._panelPrivacy, 2);
			this._panelPrivacy.Controls.Add(this._labelPrivacy);
			this._panelPrivacy.Controls.Add(this._privacyProtection);
			this._panelPrivacy.Dock = System.Windows.Forms.DockStyle.Top;
			this._panelPrivacy.Location = new System.Drawing.Point(245, 235);
			this._panelPrivacy.Margin = new System.Windows.Forms.Padding(7, 3, 0, 3);
			this._panelPrivacy.Name = "_panelPrivacy";
			this._panelPrivacy.Size = new System.Drawing.Size(240, 22);
			this._panelPrivacy.TabIndex = 36;
			// 
			// _labelPrivacy
			// 
			this._labelPrivacy.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this._labelPrivacy, null);
			this.locExtender.SetLocalizationComment(this._labelPrivacy, null);
			this.locExtender.SetLocalizingId(this._labelPrivacy, "PeopleView.MetadataEditor._labelPrivacyProtection");
			this._labelPrivacy.Location = new System.Drawing.Point(0, 5);
			this._labelPrivacy.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelPrivacy.Name = "_labelPrivacy";
			this._labelPrivacy.Size = new System.Drawing.Size(93, 13);
			this._labelPrivacy.TabIndex = 29;
			this._labelPrivacy.Text = "Privacy Protection";
			// 
			// _privacyProtection
			// 
			this._privacyProtection.AutoSize = true;
			this._binder.SetIsBound(this._privacyProtection, true);
			this._binder.SetIsComponentFileId(this._privacyProtection, false);
			this.locExtender.SetLocalizableToolTip(this._privacyProtection, null);
			this.locExtender.SetLocalizationComment(this._privacyProtection, null);
			this.locExtender.SetLocalizationPriority(this._privacyProtection, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._privacyProtection, "PeopleView.MetadataEditor.PrivacyProtection");
			this._privacyProtection.Location = new System.Drawing.Point(99, 5);
			this._privacyProtection.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this._privacyProtection.Name = "_privacyProtection";
			this._privacyProtection.Size = new System.Drawing.Size(15, 14);
			this._privacyProtection.TabIndex = 30;
			this._privacyProtection.UseVisualStyleBackColor = true;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = null;
			// 
			// PersonBasicEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(207)))), ((int)(((byte)(219)))));
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "PersonBasicEditor.EditorBase");
			this.Name = "PersonBasicEditor";
			this.Size = new System.Drawing.Size(499, 439);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			this._panelPicture.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this._personsPicture)).EndInit();
			this._panelPrivacy.ResumeLayout(false);
			this._panelPrivacy.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BindingHelper _binder;
		private AutoCompleteHelper _autoCompleteHelper;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Label _labelFullName;
		private System.Windows.Forms.Label _labelAge;
		private System.Windows.Forms.Label _labelgender;
		private System.Windows.Forms.Label _labelHowToContact;
		private System.Windows.Forms.Label _labelCustomFields;
		private System.Windows.Forms.Label _labelEducation;
		private System.Windows.Forms.Label _labelPrimaryOccupation;
		private System.Windows.Forms.ComboBox _gender;
		private System.Windows.Forms.TextBox _id;
		private System.Windows.Forms.TextBox _age;
		private System.Windows.Forms.TextBox _otherLanguage0;
		private System.Windows.Forms.TextBox _otherLanguage1;
		private System.Windows.Forms.TextBox _otherLanguage2;
		private System.Windows.Forms.TextBox _otherLanguage3;
		private System.Windows.Forms.TextBox _howToContact;
		private System.Windows.Forms.TextBox _education;
		private System.Windows.Forms.TextBox _primaryOccupation;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangMother0;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangFather0;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangMother3;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangMother2;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangMother1;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangFather1;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangFather2;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangFather3;
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
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.Label _labelSessions;
		private System.Windows.Forms.ListBox _sessionList;
		private System.Windows.Forms.FlowLayoutPanel _panelPrivacy;
		private System.Windows.Forms.Label _labelPrivacy;
		private System.Windows.Forms.CheckBox _privacyProtection;
		private System.Windows.Forms.TextBox _nickName;
		private System.Windows.Forms.Label _labelNickName;
		private System.Windows.Forms.TextBox _ethnicGroup;
		private System.Windows.Forms.Label _labelEthnicGroup;
	}
}
