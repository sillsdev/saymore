using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ComponentEditors
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
			this._tblLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._picture = new System.Windows.Forms.PictureBox();
			this._pbOtherLangMother3 = new SayMore.UI.LowLevelControls.ParentButton();
			this._panelGrid = new System.Windows.Forms.Panel();
			this._primaryLanguageLearnedIn = new System.Windows.Forms.TextBox();
			this._labelPrimaryLanguageLearnedIn = new System.Windows.Forms.Label();
			this._pbOtherLangMother1 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangFather3 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangFather2 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangFather1 = new SayMore.UI.LowLevelControls.ParentButton();
			this._gender = new System.Windows.Forms.ComboBox();
			this._pbOtherLangMother0 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangFather0 = new SayMore.UI.LowLevelControls.ParentButton();
			this._otherLanguage3 = new System.Windows.Forms.TextBox();
			this._otherLanguage0 = new System.Windows.Forms.TextBox();
			this._otherLanguage1 = new System.Windows.Forms.TextBox();
			this._otherLanguage2 = new System.Windows.Forms.TextBox();
			this._primaryLanguage = new System.Windows.Forms.TextBox();
			this._labelgender = new System.Windows.Forms.Label();
			this._howToContact = new System.Windows.Forms.TextBox();
			this._labelHowToContact = new System.Windows.Forms.Label();
			this._labelPrimaryLanguage = new System.Windows.Forms.Label();
			this._pbPrimaryLangMother = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbPrimaryLangFather = new SayMore.UI.LowLevelControls.ParentButton();
			this._labelOtherLanguages = new System.Windows.Forms.Label();
			this._pbOtherLangMother2 = new SayMore.UI.LowLevelControls.ParentButton();
			this._labelEducation = new System.Windows.Forms.Label();
			this._education = new System.Windows.Forms.TextBox();
			this._labelPrimaryOccupation = new System.Windows.Forms.Label();
			this._labelCustomFields = new System.Windows.Forms.Label();
			this._primaryOccupation = new System.Windows.Forms.TextBox();
			this._binder = new SayMore.UI.ComponentEditors.BindingHelper(this.components);
			this._autoCompleteHelper = new SayMore.UI.ComponentEditors.AutoCompleteHelper(this.components);
			this._tblLayoutOuter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._picture)).BeginInit();
			this.SuspendLayout();
			// 
			// _labelFullName
			// 
			this._labelFullName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelFullName.AutoSize = true;
			this._tblLayoutOuter.SetColumnSpan(this._labelFullName, 2);
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
			this._id.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._id.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tblLayoutOuter.SetColumnSpan(this._id, 2);
			this._binder.SetIsBound(this._id, true);
			this._binder.SetIsComponentFileId(this._id, true);
			this._id.Location = new System.Drawing.Point(0, 16);
			this._id.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._id.Name = "_id";
			this._id.Size = new System.Drawing.Size(188, 20);
			this._id.TabIndex = 1;
			this._autoCompleteHelper.SetUpdateGatherer(this._id, false);
			this._id.Enter += new System.EventHandler(this.HandleIdEnter);
			// 
			// _labelBirthYear
			// 
			this._labelBirthYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelBirthYear.AutoSize = true;
			this._labelBirthYear.Location = new System.Drawing.Point(260, 0);
			this._labelBirthYear.Margin = new System.Windows.Forms.Padding(7, 0, 3, 0);
			this._labelBirthYear.Name = "_labelBirthYear";
			this._labelBirthYear.Size = new System.Drawing.Size(53, 13);
			this._labelBirthYear.TabIndex = 2;
			this._labelBirthYear.Text = "&Birth Year";
			// 
			// _birthYear
			// 
			this._birthYear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._birthYear, "");
			this._birthYear.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._birthYear.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._binder.SetIsBound(this._birthYear, true);
			this._binder.SetIsComponentFileId(this._birthYear, false);
			this._birthYear.Location = new System.Drawing.Point(260, 16);
			this._birthYear.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
			this._birthYear.Name = "_birthYear";
			this._birthYear.Size = new System.Drawing.Size(58, 20);
			this._birthYear.TabIndex = 3;
			this._autoCompleteHelper.SetUpdateGatherer(this._birthYear, false);
			// 
			// _tblLayoutOuter
			// 
			this._tblLayoutOuter.AutoSize = true;
			this._tblLayoutOuter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tblLayoutOuter.BackColor = System.Drawing.Color.Transparent;
			this._tblLayoutOuter.ColumnCount = 6;
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tblLayoutOuter.Controls.Add(this._picture, 5, 0);
			this._tblLayoutOuter.Controls.Add(this._pbOtherLangMother3, 3, 9);
			this._tblLayoutOuter.Controls.Add(this._panelGrid, 4, 11);
			this._tblLayoutOuter.Controls.Add(this._primaryLanguageLearnedIn, 1, 4);
			this._tblLayoutOuter.Controls.Add(this._labelPrimaryLanguageLearnedIn, 0, 4);
			this._tblLayoutOuter.Controls.Add(this._pbOtherLangMother1, 3, 7);
			this._tblLayoutOuter.Controls.Add(this._pbOtherLangFather3, 2, 9);
			this._tblLayoutOuter.Controls.Add(this._pbOtherLangFather2, 2, 8);
			this._tblLayoutOuter.Controls.Add(this._pbOtherLangFather1, 2, 7);
			this._tblLayoutOuter.Controls.Add(this._gender, 4, 3);
			this._tblLayoutOuter.Controls.Add(this._pbOtherLangMother0, 3, 6);
			this._tblLayoutOuter.Controls.Add(this._pbOtherLangFather0, 2, 6);
			this._tblLayoutOuter.Controls.Add(this._otherLanguage3, 0, 9);
			this._tblLayoutOuter.Controls.Add(this._labelFullName, 0, 0);
			this._tblLayoutOuter.Controls.Add(this._otherLanguage0, 0, 6);
			this._tblLayoutOuter.Controls.Add(this._otherLanguage1, 0, 7);
			this._tblLayoutOuter.Controls.Add(this._otherLanguage2, 0, 8);
			this._tblLayoutOuter.Controls.Add(this._primaryLanguage, 0, 3);
			this._tblLayoutOuter.Controls.Add(this._labelBirthYear, 4, 0);
			this._tblLayoutOuter.Controls.Add(this._id, 0, 1);
			this._tblLayoutOuter.Controls.Add(this._birthYear, 4, 1);
			this._tblLayoutOuter.Controls.Add(this._labelgender, 4, 2);
			this._tblLayoutOuter.Controls.Add(this._howToContact, 4, 6);
			this._tblLayoutOuter.Controls.Add(this._labelHowToContact, 4, 5);
			this._tblLayoutOuter.Controls.Add(this._labelPrimaryLanguage, 0, 2);
			this._tblLayoutOuter.Controls.Add(this._pbPrimaryLangMother, 3, 3);
			this._tblLayoutOuter.Controls.Add(this._pbPrimaryLangFather, 2, 3);
			this._tblLayoutOuter.Controls.Add(this._labelOtherLanguages, 0, 5);
			this._tblLayoutOuter.Controls.Add(this._pbOtherLangMother2, 3, 8);
			this._tblLayoutOuter.Controls.Add(this._labelEducation, 0, 10);
			this._tblLayoutOuter.Controls.Add(this._education, 0, 11);
			this._tblLayoutOuter.Controls.Add(this._labelPrimaryOccupation, 0, 12);
			this._tblLayoutOuter.Controls.Add(this._labelCustomFields, 4, 10);
			this._tblLayoutOuter.Controls.Add(this._primaryOccupation, 0, 13);
			this._tblLayoutOuter.Dock = System.Windows.Forms.DockStyle.Top;
			this._tblLayoutOuter.Location = new System.Drawing.Point(7, 7);
			this._tblLayoutOuter.Name = "_tblLayoutOuter";
			this._tblLayoutOuter.RowCount = 15;
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.Size = new System.Drawing.Size(502, 327);
			this._tblLayoutOuter.TabIndex = 0;
			this._tblLayoutOuter.TabStop = true;
			this._tblLayoutOuter.Paint += new System.Windows.Forms.PaintEventHandler(this._tblLayoutOuter_Paint);
			// 
			// _picture
			// 
			this._picture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._picture.Cursor = System.Windows.Forms.Cursors.Hand;
			this._picture.ErrorImage = null;
			this._picture.Image = global::SayMore.Properties.Resources.kimidNoPhoto;
			this._picture.InitialImage = null;
			this._picture.Location = new System.Drawing.Point(336, 0);
			this._picture.Margin = new System.Windows.Forms.Padding(14, 0, 0, 0);
			this._picture.Name = "_picture";
			this._tblLayoutOuter.SetRowSpan(this._picture, 5);
			this._picture.Size = new System.Drawing.Size(166, 106);
			this._picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._picture.TabIndex = 9;
			this._picture.TabStop = false;
			this._picture.MouseLeave += new System.EventHandler(this.HandlePersonPictureMouseEnterLeave);
			this._picture.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HandlePersonPictureMouseClick);
			this._picture.Paint += new System.Windows.Forms.PaintEventHandler(this.HandlePersonPicturePaint);
			this._picture.MouseEnter += new System.EventHandler(this.HandlePersonPictureMouseEnterLeave);
			// 
			// _pbOtherLangMother3
			// 
			this._pbOtherLangMother3.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangMother3.Location = new System.Drawing.Point(215, 214);
			this._pbOtherLangMother3.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangMother3.Name = "_pbOtherLangMother3";
			this._pbOtherLangMother3.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother3.Selected = false;
			this._pbOtherLangMother3.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother3.TabIndex = 24;
			this._pbOtherLangMother3.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			// 
			// _panelGrid
			// 
			this._panelGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tblLayoutOuter.SetColumnSpan(this._panelGrid, 2);
			this._panelGrid.Location = new System.Drawing.Point(260, 260);
			this._panelGrid.Margin = new System.Windows.Forms.Padding(7, 3, 0, 3);
			this._panelGrid.Name = "_panelGrid";
			this._tblLayoutOuter.SetRowSpan(this._panelGrid, 3);
			this._panelGrid.Size = new System.Drawing.Size(242, 64);
			this._panelGrid.TabIndex = 30;
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
			this._primaryLanguageLearnedIn.Location = new System.Drawing.Point(64, 86);
			this._primaryLanguageLearnedIn.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
			this._primaryLanguageLearnedIn.Name = "_primaryLanguageLearnedIn";
			this._primaryLanguageLearnedIn.Size = new System.Drawing.Size(124, 20);
			this._primaryLanguageLearnedIn.TabIndex = 9;
			this._autoCompleteHelper.SetUpdateGatherer(this._primaryLanguageLearnedIn, false);
			// 
			// _labelPrimaryLanguageLearnedIn
			// 
			this._labelPrimaryLanguageLearnedIn.AutoSize = true;
			this._labelPrimaryLanguageLearnedIn.Location = new System.Drawing.Point(0, 90);
			this._labelPrimaryLanguageLearnedIn.Margin = new System.Windows.Forms.Padding(0, 7, 3, 10);
			this._labelPrimaryLanguageLearnedIn.Name = "_labelPrimaryLanguageLearnedIn";
			this._labelPrimaryLanguageLearnedIn.Size = new System.Drawing.Size(61, 13);
			this._labelPrimaryLanguageLearnedIn.TabIndex = 8;
			this._labelPrimaryLanguageLearnedIn.Text = "&Learned In:";
			// 
			// _pbOtherLangMother1
			// 
			this._pbOtherLangMother1.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangMother1.Location = new System.Drawing.Point(215, 158);
			this._pbOtherLangMother1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangMother1.Name = "_pbOtherLangMother1";
			this._pbOtherLangMother1.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother1.Selected = false;
			this._pbOtherLangMother1.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother1.TabIndex = 18;
			this._pbOtherLangMother1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			// 
			// _pbOtherLangFather3
			// 
			this._pbOtherLangFather3.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangFather3.Location = new System.Drawing.Point(191, 214);
			this._pbOtherLangFather3.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangFather3.Name = "_pbOtherLangFather3";
			this._pbOtherLangFather3.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather3.Selected = false;
			this._pbOtherLangFather3.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather3.TabIndex = 23;
			this._pbOtherLangFather3.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			// 
			// _pbOtherLangFather2
			// 
			this._pbOtherLangFather2.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangFather2.Location = new System.Drawing.Point(191, 186);
			this._pbOtherLangFather2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangFather2.Name = "_pbOtherLangFather2";
			this._pbOtherLangFather2.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather2.Selected = false;
			this._pbOtherLangFather2.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather2.TabIndex = 20;
			this._pbOtherLangFather2.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			// 
			// _pbOtherLangFather1
			// 
			this._pbOtherLangFather1.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangFather1.Location = new System.Drawing.Point(191, 158);
			this._pbOtherLangFather1.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangFather1.Name = "_pbOtherLangFather1";
			this._pbOtherLangFather1.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather1.Selected = false;
			this._pbOtherLangFather1.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather1.TabIndex = 17;
			this._pbOtherLangFather1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			// 
			// _gender
			// 
			this._gender.Dock = System.Windows.Forms.DockStyle.Top;
			this._gender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._gender.FormattingEnabled = true;
			this._binder.SetIsBound(this._gender, true);
			this._binder.SetIsComponentFileId(this._gender, false);
			this._gender.Items.AddRange(new object[] {
            "Male",
            "Female"});
			this._gender.Location = new System.Drawing.Point(260, 60);
			this._gender.Margin = new System.Windows.Forms.Padding(7, 3, 3, 0);
			this._gender.Name = "_gender";
			this._gender.Size = new System.Drawing.Size(58, 21);
			this._gender.TabIndex = 11;
			// 
			// _pbOtherLangMother0
			// 
			this._pbOtherLangMother0.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangMother0.Location = new System.Drawing.Point(215, 132);
			this._pbOtherLangMother0.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangMother0.Name = "_pbOtherLangMother0";
			this._pbOtherLangMother0.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother0.Selected = false;
			this._pbOtherLangMother0.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother0.TabIndex = 15;
			this._pbOtherLangMother0.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			// 
			// _pbOtherLangFather0
			// 
			this._pbOtherLangFather0.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangFather0.Location = new System.Drawing.Point(191, 132);
			this._pbOtherLangFather0.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbOtherLangFather0.Name = "_pbOtherLangFather0";
			this._pbOtherLangFather0.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather0.Selected = false;
			this._pbOtherLangFather0.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather0.TabIndex = 14;
			this._pbOtherLangFather0.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			// 
			// _otherLanguage3
			// 
			this._otherLanguage3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._otherLanguage3, "language");
			this._otherLanguage3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._otherLanguage3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tblLayoutOuter.SetColumnSpan(this._otherLanguage3, 2);
			this._binder.SetIsBound(this._otherLanguage3, true);
			this._binder.SetIsComponentFileId(this._otherLanguage3, false);
			this._otherLanguage3.Location = new System.Drawing.Point(0, 216);
			this._otherLanguage3.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage3.Name = "_otherLanguage3";
			this._otherLanguage3.Size = new System.Drawing.Size(188, 20);
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
			this._tblLayoutOuter.SetColumnSpan(this._otherLanguage0, 2);
			this._binder.SetIsBound(this._otherLanguage0, true);
			this._binder.SetIsComponentFileId(this._otherLanguage0, false);
			this._otherLanguage0.Location = new System.Drawing.Point(0, 134);
			this._otherLanguage0.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage0.Name = "_otherLanguage0";
			this._otherLanguage0.Size = new System.Drawing.Size(188, 20);
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
			this._tblLayoutOuter.SetColumnSpan(this._otherLanguage1, 2);
			this._binder.SetIsBound(this._otherLanguage1, true);
			this._binder.SetIsComponentFileId(this._otherLanguage1, false);
			this._otherLanguage1.Location = new System.Drawing.Point(0, 160);
			this._otherLanguage1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage1.Name = "_otherLanguage1";
			this._otherLanguage1.Size = new System.Drawing.Size(188, 20);
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
			this._tblLayoutOuter.SetColumnSpan(this._otherLanguage2, 2);
			this._binder.SetIsBound(this._otherLanguage2, true);
			this._binder.SetIsComponentFileId(this._otherLanguage2, false);
			this._otherLanguage2.Location = new System.Drawing.Point(0, 188);
			this._otherLanguage2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage2.Name = "_otherLanguage2";
			this._otherLanguage2.Size = new System.Drawing.Size(188, 20);
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
			this._tblLayoutOuter.SetColumnSpan(this._primaryLanguage, 2);
			this._binder.SetIsBound(this._primaryLanguage, true);
			this._binder.SetIsComponentFileId(this._primaryLanguage, false);
			this._primaryLanguage.Location = new System.Drawing.Point(0, 60);
			this._primaryLanguage.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._primaryLanguage.Name = "_primaryLanguage";
			this._primaryLanguage.Size = new System.Drawing.Size(188, 20);
			this._primaryLanguage.TabIndex = 5;
			this._autoCompleteHelper.SetUpdateGatherer(this._primaryLanguage, false);
			// 
			// _labelgender
			// 
			this._labelgender.AutoEllipsis = true;
			this._labelgender.AutoSize = true;
			this._labelgender.Location = new System.Drawing.Point(260, 44);
			this._labelgender.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelgender.MinimumSize = new System.Drawing.Size(42, 0);
			this._labelgender.Name = "_labelgender";
			this._labelgender.Size = new System.Drawing.Size(42, 13);
			this._labelgender.TabIndex = 10;
			this._labelgender.Text = "Gender";
			// 
			// _howToContact
			// 
			this._howToContact.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._howToContact, "education");
			this._howToContact.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._howToContact.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tblLayoutOuter.SetColumnSpan(this._howToContact, 2);
			this._binder.SetIsBound(this._howToContact, true);
			this._binder.SetIsComponentFileId(this._howToContact, false);
			this._howToContact.Location = new System.Drawing.Point(260, 134);
			this._howToContact.Margin = new System.Windows.Forms.Padding(7, 3, 0, 3);
			this._howToContact.Multiline = true;
			this._howToContact.Name = "_howToContact";
			this._tblLayoutOuter.SetRowSpan(this._howToContact, 4);
			this._howToContact.Size = new System.Drawing.Size(242, 74);
			this._howToContact.TabIndex = 26;
			this._autoCompleteHelper.SetUpdateGatherer(this._howToContact, false);
			// 
			// _labelHowToContact
			// 
			this._labelHowToContact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelHowToContact.AutoSize = true;
			this._tblLayoutOuter.SetColumnSpan(this._labelHowToContact, 2);
			this._labelHowToContact.Location = new System.Drawing.Point(260, 118);
			this._labelHowToContact.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelHowToContact.Name = "_labelHowToContact";
			this._labelHowToContact.Size = new System.Drawing.Size(81, 13);
			this._labelHowToContact.TabIndex = 25;
			this._labelHowToContact.Text = "How to Contact";
			// 
			// _labelPrimaryLanguage
			// 
			this._labelPrimaryLanguage.AutoSize = true;
			this._tblLayoutOuter.SetColumnSpan(this._labelPrimaryLanguage, 4);
			this._labelPrimaryLanguage.Location = new System.Drawing.Point(0, 44);
			this._labelPrimaryLanguage.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelPrimaryLanguage.Name = "_labelPrimaryLanguage";
			this._labelPrimaryLanguage.Size = new System.Drawing.Size(92, 13);
			this._labelPrimaryLanguage.TabIndex = 4;
			this._labelPrimaryLanguage.Text = "Primary Language";
			// 
			// _pbPrimaryLangMother
			// 
			this._pbPrimaryLangMother.BackColor = System.Drawing.Color.Transparent;
			this._pbPrimaryLangMother.Location = new System.Drawing.Point(215, 58);
			this._pbPrimaryLangMother.Margin = new System.Windows.Forms.Padding(0, 1, 14, 0);
			this._pbPrimaryLangMother.Name = "_pbPrimaryLangMother";
			this._pbPrimaryLangMother.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbPrimaryLangMother.Selected = false;
			this._pbPrimaryLangMother.Size = new System.Drawing.Size(24, 24);
			this._pbPrimaryLangMother.TabIndex = 7;
			// 
			// _pbPrimaryLangFather
			// 
			this._pbPrimaryLangFather.BackColor = System.Drawing.Color.Transparent;
			this._pbPrimaryLangFather.Location = new System.Drawing.Point(191, 58);
			this._pbPrimaryLangFather.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
			this._pbPrimaryLangFather.Name = "_pbPrimaryLangFather";
			this._pbPrimaryLangFather.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbPrimaryLangFather.Selected = false;
			this._pbPrimaryLangFather.Size = new System.Drawing.Size(24, 24);
			this._pbPrimaryLangFather.TabIndex = 6;
			// 
			// _labelOtherLanguages
			// 
			this._labelOtherLanguages.AutoSize = true;
			this._tblLayoutOuter.SetColumnSpan(this._labelOtherLanguages, 4);
			this._labelOtherLanguages.Location = new System.Drawing.Point(0, 118);
			this._labelOtherLanguages.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelOtherLanguages.Name = "_labelOtherLanguages";
			this._labelOtherLanguages.Size = new System.Drawing.Size(89, 13);
			this._labelOtherLanguages.TabIndex = 12;
			this._labelOtherLanguages.Text = "Other Languages";
			// 
			// _pbOtherLangMother2
			// 
			this._pbOtherLangMother2.AllowDrop = true;
			this._pbOtherLangMother2.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangMother2.Location = new System.Drawing.Point(215, 186);
			this._pbOtherLangMother2.Margin = new System.Windows.Forms.Padding(0, 1, 0, 3);
			this._pbOtherLangMother2.Name = "_pbOtherLangMother2";
			this._pbOtherLangMother2.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother2.Selected = false;
			this._pbOtherLangMother2.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother2.TabIndex = 21;
			this._pbOtherLangMother2.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			// 
			// _labelEducation
			// 
			this._labelEducation.AutoSize = true;
			this._tblLayoutOuter.SetColumnSpan(this._labelEducation, 2);
			this._labelEducation.Location = new System.Drawing.Point(0, 244);
			this._labelEducation.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelEducation.Name = "_labelEducation";
			this._labelEducation.Size = new System.Drawing.Size(55, 13);
			this._labelEducation.TabIndex = 27;
			this._labelEducation.Text = "Education";
			// 
			// _education
			// 
			this._education.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._education, "");
			this._tblLayoutOuter.SetColumnSpan(this._education, 2);
			this._binder.SetIsBound(this._education, true);
			this._binder.SetIsComponentFileId(this._education, false);
			this._education.Location = new System.Drawing.Point(0, 260);
			this._education.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._education.Name = "_education";
			this._education.Size = new System.Drawing.Size(188, 20);
			this._education.TabIndex = 28;
			this._autoCompleteHelper.SetUpdateGatherer(this._education, false);
			// 
			// _labelPrimaryOccupation
			// 
			this._labelPrimaryOccupation.AutoSize = true;
			this._tblLayoutOuter.SetColumnSpan(this._labelPrimaryOccupation, 2);
			this._labelPrimaryOccupation.Location = new System.Drawing.Point(0, 288);
			this._labelPrimaryOccupation.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._labelPrimaryOccupation.Name = "_labelPrimaryOccupation";
			this._labelPrimaryOccupation.Size = new System.Drawing.Size(99, 13);
			this._labelPrimaryOccupation.TabIndex = 31;
			this._labelPrimaryOccupation.Text = "Primary Occupation";
			// 
			// _labelCustomFields
			// 
			this._labelCustomFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._labelCustomFields.AutoSize = true;
			this._tblLayoutOuter.SetColumnSpan(this._labelCustomFields, 2);
			this._labelCustomFields.Location = new System.Drawing.Point(260, 244);
			this._labelCustomFields.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelCustomFields.Name = "_labelCustomFields";
			this._labelCustomFields.Size = new System.Drawing.Size(72, 13);
			this._labelCustomFields.TabIndex = 29;
			this._labelCustomFields.Text = "Custom Fields";
			// 
			// _primaryOccupation
			// 
			this._primaryOccupation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._primaryOccupation, "");
			this._tblLayoutOuter.SetColumnSpan(this._primaryOccupation, 2);
			this._binder.SetIsBound(this._primaryOccupation, true);
			this._binder.SetIsComponentFileId(this._primaryOccupation, false);
			this._primaryOccupation.Location = new System.Drawing.Point(0, 304);
			this._primaryOccupation.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._primaryOccupation.Name = "_primaryOccupation";
			this._primaryOccupation.Size = new System.Drawing.Size(188, 20);
			this._primaryOccupation.TabIndex = 32;
			this._autoCompleteHelper.SetUpdateGatherer(this._primaryOccupation, false);
			// 
			// PersonBasicEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(207)))), ((int)(((byte)(219)))));
			this.Controls.Add(this._tblLayoutOuter);
			this.Name = "PersonBasicEditor";
			this.Size = new System.Drawing.Size(516, 350);
			this._tblLayoutOuter.ResumeLayout(false);
			this._tblLayoutOuter.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._picture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private BindingHelper _binder;
		private AutoCompleteHelper _autoCompleteHelper;
		private System.Windows.Forms.TableLayoutPanel _tblLayoutOuter;
		private System.Windows.Forms.Label _labelFullName;
		private System.Windows.Forms.Label _labelBirthYear;
		private System.Windows.Forms.Label _labelgender;
		private System.Windows.Forms.Label _labelHowToContact;
		private System.Windows.Forms.Label _labelCustomFields;
		private System.Windows.Forms.Label _labelEducation;
		private System.Windows.Forms.Label _labelPrimaryOccupation;
		private System.Windows.Forms.ComboBox _gender;
		private System.Windows.Forms.PictureBox _picture;
		private System.Windows.Forms.TextBox _id;
		private System.Windows.Forms.TextBox _birthYear;
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
	}
}
