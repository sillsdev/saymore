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
			this._pnlOtherLanguages = new SayMore.UI.LowLevelControls.UnderlinedHdgBox();
			this._tblLayoutOtherLanguages = new System.Windows.Forms.TableLayoutPanel();
			this._pbOtherLangMother3 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangMother2 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangMother1 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangFather1 = new SayMore.UI.LowLevelControls.ParentButton();
			this._otherLanguage3 = new AutoCompleteTextBox();
			this._otherLanguage0 = new AutoCompleteTextBox();
			this._pbOtherLangFather0 = new SayMore.UI.LowLevelControls.ParentButton();
			this._otherLanguage1 = new AutoCompleteTextBox();
			this._pbOtherLangMother0 = new SayMore.UI.LowLevelControls.ParentButton();
			this._otherLanguage2 = new AutoCompleteTextBox();
			this._pbOtherLangFather2 = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbOtherLangFather3 = new SayMore.UI.LowLevelControls.ParentButton();
			this._gender = new System.Windows.Forms.ComboBox();
			this._pnlPrimaryLanguage = new SayMore.UI.LowLevelControls.UnderlinedHdgBox();
			this._tblLayoutPrimaryLanguage = new System.Windows.Forms.TableLayoutPanel();
			this._pbPrimaryLangMother = new SayMore.UI.LowLevelControls.ParentButton();
			this._pbPrimaryLangFather = new SayMore.UI.LowLevelControls.ParentButton();
			this._labelPrimaryLanguageLearnedIn = new System.Windows.Forms.Label();
			this._primaryLanguage = new AutoCompleteTextBox();
			this._primaryLanguageLearnedIn = new System.Windows.Forms.TextBox();
			this._labelgender = new System.Windows.Forms.Label();
			this._labelEducation = new System.Windows.Forms.Label();
			this._education = new System.Windows.Forms.TextBox();
			this._labelPrimaryOccupation = new System.Windows.Forms.Label();
			this._primaryOccupation = new System.Windows.Forms.TextBox();
			this._howToContact = new System.Windows.Forms.TextBox();
			this._labelHowToContact = new System.Windows.Forms.Label();
			this._binder = new SayMore.UI.ComponentEditors.BindingHelper(this.components);
			this._tblLayoutOuter.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._picture)).BeginInit();
			this._pnlOtherLanguages.SuspendLayout();
			this._tblLayoutOtherLanguages.SuspendLayout();
			this._pnlPrimaryLanguage.SuspendLayout();
			this._tblLayoutPrimaryLanguage.SuspendLayout();
			this.SuspendLayout();
			// 
			// _labelFullName
			// 
			this._labelFullName.AutoSize = true;
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
			this._binder.SetIsBound(this._id, true);
			this._id.Location = new System.Drawing.Point(0, 16);
			this._id.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._id.Name = "_id";
			this._id.Size = new System.Drawing.Size(355, 20);
			this._id.TabIndex = 1;
			// 
			// _labelBirthYear
			// 
			this._labelBirthYear.AutoSize = true;
			this._labelBirthYear.Location = new System.Drawing.Point(365, 0);
			this._labelBirthYear.Margin = new System.Windows.Forms.Padding(7, 0, 3, 0);
			this._labelBirthYear.Name = "_labelBirthYear";
			this._labelBirthYear.Size = new System.Drawing.Size(53, 13);
			this._labelBirthYear.TabIndex = 4;
			this._labelBirthYear.Text = "&Birth Year";
			// 
			// _birthYear
			// 
			this._birthYear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._binder.SetIsBound(this._birthYear, true);
			this._birthYear.Location = new System.Drawing.Point(365, 16);
			this._birthYear.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
			this._birthYear.Name = "_birthYear";
			this._birthYear.Size = new System.Drawing.Size(91, 20);
			this._birthYear.TabIndex = 5;
			// 
			// _tblLayoutOuter
			// 
			this._tblLayoutOuter.AutoSize = true;
			this._tblLayoutOuter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tblLayoutOuter.BackColor = System.Drawing.Color.Transparent;
			this._tblLayoutOuter.ColumnCount = 3;
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78F));
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tblLayoutOuter.Controls.Add(this._picture, 2, 0);
			this._tblLayoutOuter.Controls.Add(this._pnlOtherLanguages, 0, 4);
			this._tblLayoutOuter.Controls.Add(this._gender, 1, 3);
			this._tblLayoutOuter.Controls.Add(this._pnlPrimaryLanguage, 0, 2);
			this._tblLayoutOuter.Controls.Add(this._labelFullName, 0, 0);
			this._tblLayoutOuter.Controls.Add(this._labelBirthYear, 1, 0);
			this._tblLayoutOuter.Controls.Add(this._id, 0, 1);
			this._tblLayoutOuter.Controls.Add(this._birthYear, 1, 1);
			this._tblLayoutOuter.Controls.Add(this._labelgender, 1, 2);
			this._tblLayoutOuter.Controls.Add(this._labelEducation, 1, 4);
			this._tblLayoutOuter.Controls.Add(this._education, 1, 5);
			this._tblLayoutOuter.Controls.Add(this._labelPrimaryOccupation, 1, 6);
			this._tblLayoutOuter.Controls.Add(this._primaryOccupation, 1, 7);
			this._tblLayoutOuter.Controls.Add(this._howToContact, 1, 9);
			this._tblLayoutOuter.Controls.Add(this._labelHowToContact, 1, 8);
			this._tblLayoutOuter.Dock = System.Windows.Forms.DockStyle.Top;
			this._tblLayoutOuter.Location = new System.Drawing.Point(7, 7);
			this._tblLayoutOuter.Name = "_tblLayoutOuter";
			this._tblLayoutOuter.RowCount = 12;
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
			this._tblLayoutOuter.Size = new System.Drawing.Size(593, 312);
			this._tblLayoutOuter.TabIndex = 0;
			this._tblLayoutOuter.TabStop = true;
			// 
			// _picture
			// 
			this._picture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._picture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._picture.Cursor = System.Windows.Forms.Cursors.Hand;
			this._picture.ErrorImage = null;
			this._picture.Image = global::SayMore.Properties.Resources.kimidNoPhoto;
			this._picture.InitialImage = null;
			this._picture.Location = new System.Drawing.Point(463, 0);
			this._picture.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this._picture.Name = "_picture";
			this._tblLayoutOuter.SetRowSpan(this._picture, 4);
			this._picture.Size = new System.Drawing.Size(130, 130);
			this._picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this._picture.TabIndex = 9;
			this._picture.TabStop = false;
			this._picture.MouseLeave += new System.EventHandler(this.HandlePersonPictureMouseEnterLeave);
			this._picture.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HandlePersonPictureMouseClick);
			this._picture.Paint += new System.Windows.Forms.PaintEventHandler(this.HandlePersonPicturePaint);
			this._picture.MouseEnter += new System.EventHandler(this.HandlePersonPictureMouseEnterLeave);
			// 
			// _pnlOtherLanguages
			// 
			this._pnlOtherLanguages.AutoSize = true;
			this._pnlOtherLanguages.BackColor = System.Drawing.Color.Transparent;
			this._pnlOtherLanguages.Controls.Add(this._tblLayoutOtherLanguages);
			this._pnlOtherLanguages.Dock = System.Windows.Forms.DockStyle.Top;
			this._pnlOtherLanguages.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._pnlOtherLanguages.LineColor = System.Drawing.SystemColors.ControlDark;
			this._pnlOtherLanguages.LineThickness = 1;
			this._pnlOtherLanguages.Location = new System.Drawing.Point(0, 133);
			this._pnlOtherLanguages.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._pnlOtherLanguages.Name = "_pnlOtherLanguages";
			this._tblLayoutOuter.SetRowSpan(this._pnlOtherLanguages, 6);
			this._pnlOtherLanguages.Size = new System.Drawing.Size(355, 138);
			this._pnlOtherLanguages.TabIndex = 3;
			this._pnlOtherLanguages.Text = "Other Languages";
			// 
			// _tblLayoutOtherLanguages
			// 
			this._tblLayoutOtherLanguages.AutoSize = true;
			this._tblLayoutOtherLanguages.ColumnCount = 3;
			this._tblLayoutOtherLanguages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tblLayoutOtherLanguages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tblLayoutOtherLanguages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tblLayoutOtherLanguages.Controls.Add(this._pbOtherLangMother3, 2, 3);
			this._tblLayoutOtherLanguages.Controls.Add(this._pbOtherLangMother2, 2, 2);
			this._tblLayoutOtherLanguages.Controls.Add(this._pbOtherLangMother1, 2, 1);
			this._tblLayoutOtherLanguages.Controls.Add(this._pbOtherLangFather1, 1, 1);
			this._tblLayoutOtherLanguages.Controls.Add(this._otherLanguage3, 0, 3);
			this._tblLayoutOtherLanguages.Controls.Add(this._otherLanguage0, 0, 0);
			this._tblLayoutOtherLanguages.Controls.Add(this._pbOtherLangFather0, 1, 0);
			this._tblLayoutOtherLanguages.Controls.Add(this._otherLanguage1, 0, 1);
			this._tblLayoutOtherLanguages.Controls.Add(this._pbOtherLangMother0, 2, 0);
			this._tblLayoutOtherLanguages.Controls.Add(this._otherLanguage2, 0, 2);
			this._tblLayoutOtherLanguages.Controls.Add(this._pbOtherLangFather2, 1, 2);
			this._tblLayoutOtherLanguages.Controls.Add(this._pbOtherLangFather3, 1, 3);
			this._tblLayoutOtherLanguages.Dock = System.Windows.Forms.DockStyle.Top;
			this._tblLayoutOtherLanguages.Location = new System.Drawing.Point(0, 15);
			this._tblLayoutOtherLanguages.Margin = new System.Windows.Forms.Padding(0);
			this._tblLayoutOtherLanguages.Name = "_tblLayoutOtherLanguages";
			this._tblLayoutOtherLanguages.RowCount = 4;
			this._tblLayoutOtherLanguages.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOtherLanguages.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOtherLanguages.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOtherLanguages.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOtherLanguages.Size = new System.Drawing.Size(355, 123);
			this._tblLayoutOtherLanguages.TabIndex = 0;
			this._tblLayoutOtherLanguages.TabStop = true;
			// 
			// _pbOtherLangMother3
			// 
			this._pbOtherLangMother3.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangMother3.Location = new System.Drawing.Point(331, 96);
			this._pbOtherLangMother3.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this._pbOtherLangMother3.Name = "_pbOtherLangMother3";
			this._pbOtherLangMother3.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother3.Selected = false;
			this._pbOtherLangMother3.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother3.TabIndex = 11;
			this._pbOtherLangMother3.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			// 
			// _pbOtherLangMother2
			// 
			this._pbOtherLangMother2.AllowDrop = true;
			this._pbOtherLangMother2.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangMother2.Location = new System.Drawing.Point(331, 66);
			this._pbOtherLangMother2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this._pbOtherLangMother2.Name = "_pbOtherLangMother2";
			this._pbOtherLangMother2.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother2.Selected = false;
			this._pbOtherLangMother2.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother2.TabIndex = 8;
			this._pbOtherLangMother2.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			// 
			// _pbOtherLangMother1
			// 
			this._pbOtherLangMother1.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangMother1.Location = new System.Drawing.Point(331, 36);
			this._pbOtherLangMother1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this._pbOtherLangMother1.Name = "_pbOtherLangMother1";
			this._pbOtherLangMother1.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother1.Selected = false;
			this._pbOtherLangMother1.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother1.TabIndex = 5;
			this._pbOtherLangMother1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			// 
			// _pbOtherLangFather1
			// 
			this._pbOtherLangFather1.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangFather1.Location = new System.Drawing.Point(307, 36);
			this._pbOtherLangFather1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this._pbOtherLangFather1.Name = "_pbOtherLangFather1";
			this._pbOtherLangFather1.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather1.Selected = false;
			this._pbOtherLangFather1.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather1.TabIndex = 4;
			this._pbOtherLangFather1.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			// 
			// _otherLanguage3
			// 
			this._otherLanguage3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._otherLanguage3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._otherLanguage3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._binder.SetIsBound(this._otherLanguage3, true);
			this._otherLanguage3.Location = new System.Drawing.Point(0, 96);
			this._otherLanguage3.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage3.Name = "_otherLanguage3";
			this._otherLanguage3.Size = new System.Drawing.Size(304, 23);
			this._otherLanguage3.TabIndex = 9;
			// 
			// _otherLanguage0
			// 
			this._otherLanguage0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._otherLanguage0.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._otherLanguage0.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._binder.SetIsBound(this._otherLanguage0, true);
			this._otherLanguage0.Location = new System.Drawing.Point(0, 7);
			this._otherLanguage0.Margin = new System.Windows.Forms.Padding(0, 7, 3, 3);
			this._otherLanguage0.Name = "_otherLanguage0";
			this._otherLanguage0.Size = new System.Drawing.Size(304, 23);
			this._otherLanguage0.TabIndex = 0;
			// 
			// _pbOtherLangFather0
			// 
			this._pbOtherLangFather0.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangFather0.Location = new System.Drawing.Point(307, 7);
			this._pbOtherLangFather0.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
			this._pbOtherLangFather0.Name = "_pbOtherLangFather0";
			this._pbOtherLangFather0.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather0.Selected = false;
			this._pbOtherLangFather0.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather0.TabIndex = 1;
			this._pbOtherLangFather0.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			// 
			// _otherLanguage1
			// 
			this._otherLanguage1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._otherLanguage1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._otherLanguage1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._binder.SetIsBound(this._otherLanguage1, true);
			this._otherLanguage1.Location = new System.Drawing.Point(0, 36);
			this._otherLanguage1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage1.Name = "_otherLanguage1";
			this._otherLanguage1.Size = new System.Drawing.Size(304, 23);
			this._otherLanguage1.TabIndex = 3;
			// 
			// _pbOtherLangMother0
			// 
			this._pbOtherLangMother0.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangMother0.Location = new System.Drawing.Point(331, 7);
			this._pbOtherLangMother0.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
			this._pbOtherLangMother0.Name = "_pbOtherLangMother0";
			this._pbOtherLangMother0.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbOtherLangMother0.Selected = false;
			this._pbOtherLangMother0.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangMother0.TabIndex = 2;
			this._pbOtherLangMother0.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			// 
			// _otherLanguage2
			// 
			this._otherLanguage2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._otherLanguage2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._otherLanguage2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._binder.SetIsBound(this._otherLanguage2, true);
			this._otherLanguage2.Location = new System.Drawing.Point(0, 66);
			this._otherLanguage2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._otherLanguage2.Name = "_otherLanguage2";
			this._otherLanguage2.Size = new System.Drawing.Size(304, 23);
			this._otherLanguage2.TabIndex = 6;
			// 
			// _pbOtherLangFather2
			// 
			this._pbOtherLangFather2.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangFather2.Location = new System.Drawing.Point(307, 66);
			this._pbOtherLangFather2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this._pbOtherLangFather2.Name = "_pbOtherLangFather2";
			this._pbOtherLangFather2.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather2.Selected = false;
			this._pbOtherLangFather2.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather2.TabIndex = 7;
			this._pbOtherLangFather2.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			// 
			// _pbOtherLangFather3
			// 
			this._pbOtherLangFather3.BackColor = System.Drawing.Color.Transparent;
			this._pbOtherLangFather3.Location = new System.Drawing.Point(307, 96);
			this._pbOtherLangFather3.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this._pbOtherLangFather3.Name = "_pbOtherLangFather3";
			this._pbOtherLangFather3.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbOtherLangFather3.Selected = false;
			this._pbOtherLangFather3.Size = new System.Drawing.Size(24, 24);
			this._pbOtherLangFather3.TabIndex = 10;
			this._pbOtherLangFather3.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			// 
			// _gender
			// 
			this._gender.Dock = System.Windows.Forms.DockStyle.Top;
			this._gender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._gender.FormattingEnabled = true;
			this._binder.SetIsBound(this._gender, true);
			this._gender.Items.AddRange(new object[] {
            "Male",
            "Female"});
			this._gender.Location = new System.Drawing.Point(365, 60);
			this._gender.Margin = new System.Windows.Forms.Padding(7, 3, 3, 3);
			this._gender.Name = "_gender";
			this._gender.Size = new System.Drawing.Size(91, 21);
			this._gender.TabIndex = 7;
			// 
			// _pnlPrimaryLanguage
			// 
			this._pnlPrimaryLanguage.AutoSize = true;
			this._pnlPrimaryLanguage.BackColor = System.Drawing.Color.Transparent;
			this._pnlPrimaryLanguage.Controls.Add(this._tblLayoutPrimaryLanguage);
			this._pnlPrimaryLanguage.Dock = System.Windows.Forms.DockStyle.Top;
			this._pnlPrimaryLanguage.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._pnlPrimaryLanguage.LineColor = System.Drawing.SystemColors.ControlDark;
			this._pnlPrimaryLanguage.LineThickness = 1;
			this._pnlPrimaryLanguage.Location = new System.Drawing.Point(0, 46);
			this._pnlPrimaryLanguage.Margin = new System.Windows.Forms.Padding(0, 7, 3, 3);
			this._pnlPrimaryLanguage.Name = "_pnlPrimaryLanguage";
			this._tblLayoutOuter.SetRowSpan(this._pnlPrimaryLanguage, 2);
			this._pnlPrimaryLanguage.Size = new System.Drawing.Size(355, 81);
			this._pnlPrimaryLanguage.TabIndex = 2;
			this._pnlPrimaryLanguage.Text = "Primary Language";
			// 
			// _tblLayoutPrimaryLanguage
			// 
			this._tblLayoutPrimaryLanguage.AutoSize = true;
			this._tblLayoutPrimaryLanguage.ColumnCount = 4;
			this._tblLayoutPrimaryLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tblLayoutPrimaryLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tblLayoutPrimaryLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tblLayoutPrimaryLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tblLayoutPrimaryLanguage.Controls.Add(this._pbPrimaryLangMother, 3, 0);
			this._tblLayoutPrimaryLanguage.Controls.Add(this._pbPrimaryLangFather, 2, 0);
			this._tblLayoutPrimaryLanguage.Controls.Add(this._labelPrimaryLanguageLearnedIn, 0, 1);
			this._tblLayoutPrimaryLanguage.Controls.Add(this._primaryLanguage, 0, 0);
			this._tblLayoutPrimaryLanguage.Controls.Add(this._primaryLanguageLearnedIn, 1, 1);
			this._tblLayoutPrimaryLanguage.Dock = System.Windows.Forms.DockStyle.Top;
			this._tblLayoutPrimaryLanguage.Location = new System.Drawing.Point(0, 15);
			this._tblLayoutPrimaryLanguage.Margin = new System.Windows.Forms.Padding(0);
			this._tblLayoutPrimaryLanguage.Name = "_tblLayoutPrimaryLanguage";
			this._tblLayoutPrimaryLanguage.RowCount = 2;
			this._tblLayoutPrimaryLanguage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tblLayoutPrimaryLanguage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tblLayoutPrimaryLanguage.Size = new System.Drawing.Size(355, 66);
			this._tblLayoutPrimaryLanguage.TabIndex = 0;
			this._tblLayoutPrimaryLanguage.TabStop = true;
			// 
			// _pbPrimaryLangMother
			// 
			this._pbPrimaryLangMother.BackColor = System.Drawing.Color.Transparent;
			this._pbPrimaryLangMother.Location = new System.Drawing.Point(331, 7);
			this._pbPrimaryLangMother.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
			this._pbPrimaryLangMother.Name = "_pbPrimaryLangMother";
			this._pbPrimaryLangMother.ParentType = SayMore.UI.LowLevelControls.ParentType.Mother;
			this._pbPrimaryLangMother.Selected = false;
			this._pbPrimaryLangMother.Size = new System.Drawing.Size(24, 24);
			this._pbPrimaryLangMother.TabIndex = 2;
			this._pbPrimaryLangMother.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleMothersLanguageChanging);
			// 
			// _pbPrimaryLangFather
			// 
			this._pbPrimaryLangFather.BackColor = System.Drawing.Color.Transparent;
			this._pbPrimaryLangFather.Location = new System.Drawing.Point(307, 7);
			this._pbPrimaryLangFather.Margin = new System.Windows.Forms.Padding(0, 7, 0, 0);
			this._pbPrimaryLangFather.Name = "_pbPrimaryLangFather";
			this._pbPrimaryLangFather.ParentType = SayMore.UI.LowLevelControls.ParentType.Father;
			this._pbPrimaryLangFather.Selected = false;
			this._pbPrimaryLangFather.Size = new System.Drawing.Size(24, 24);
			this._pbPrimaryLangFather.TabIndex = 1;
			this._pbPrimaryLangFather.SelectedChanging += new System.ComponentModel.CancelEventHandler(this.HandleFathersLanguageChanging);
			// 
			// _labelPrimaryLanguageLearnedIn
			// 
			this._labelPrimaryLanguageLearnedIn.AutoSize = true;
			this._labelPrimaryLanguageLearnedIn.Location = new System.Drawing.Point(0, 40);
			this._labelPrimaryLanguageLearnedIn.Margin = new System.Windows.Forms.Padding(0, 7, 3, 0);
			this._labelPrimaryLanguageLearnedIn.Name = "_labelPrimaryLanguageLearnedIn";
			this._labelPrimaryLanguageLearnedIn.Size = new System.Drawing.Size(65, 15);
			this._labelPrimaryLanguageLearnedIn.TabIndex = 3;
			this._labelPrimaryLanguageLearnedIn.Text = "&Learned In:";
			// 
			// _primaryLanguage
			// 
			this._primaryLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this._primaryLanguage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._primaryLanguage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tblLayoutPrimaryLanguage.SetColumnSpan(this._primaryLanguage, 2);
			this._binder.SetIsBound(this._primaryLanguage, true);
			this._primaryLanguage.Location = new System.Drawing.Point(0, 7);
			this._primaryLanguage.Margin = new System.Windows.Forms.Padding(0, 7, 3, 3);
			this._primaryLanguage.Name = "_primaryLanguage";
			this._primaryLanguage.Size = new System.Drawing.Size(304, 23);
			this._primaryLanguage.TabIndex = 0;
			// 
			// _primaryLanguageLearnedIn
			// 
			this._tblLayoutPrimaryLanguage.SetColumnSpan(this._primaryLanguageLearnedIn, 3);
			this._primaryLanguageLearnedIn.Dock = System.Windows.Forms.DockStyle.Top;
			this._binder.SetIsBound(this._primaryLanguageLearnedIn, true);
			this._primaryLanguageLearnedIn.Location = new System.Drawing.Point(68, 36);
			this._primaryLanguageLearnedIn.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
			this._primaryLanguageLearnedIn.Name = "_primaryLanguageLearnedIn";
			this._primaryLanguageLearnedIn.Size = new System.Drawing.Size(284, 23);
			this._primaryLanguageLearnedIn.TabIndex = 4;
			// 
			// _labelgender
			// 
			this._labelgender.AutoEllipsis = true;
			this._labelgender.AutoSize = true;
			this._labelgender.Location = new System.Drawing.Point(365, 44);
			this._labelgender.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelgender.MinimumSize = new System.Drawing.Size(42, 0);
			this._labelgender.Name = "_labelgender";
			this._labelgender.Size = new System.Drawing.Size(42, 13);
			this._labelgender.TabIndex = 6;
			this._labelgender.Text = "Gender";
			// 
			// _labelEducation
			// 
			this._labelEducation.AutoSize = true;
			this._tblLayoutOuter.SetColumnSpan(this._labelEducation, 2);
			this._labelEducation.Location = new System.Drawing.Point(365, 135);
			this._labelEducation.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelEducation.Name = "_labelEducation";
			this._labelEducation.Size = new System.Drawing.Size(55, 13);
			this._labelEducation.TabIndex = 8;
			this._labelEducation.Text = "Education";
			// 
			// _education
			// 
			this._education.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tblLayoutOuter.SetColumnSpan(this._education, 2);
			this._binder.SetIsBound(this._education, true);
			this._education.Location = new System.Drawing.Point(365, 151);
			this._education.Margin = new System.Windows.Forms.Padding(7, 3, 3, 0);
			this._education.Name = "_education";
			this._education.Size = new System.Drawing.Size(225, 20);
			this._education.TabIndex = 9;
			// 
			// _labelPrimaryOccupation
			// 
			this._labelPrimaryOccupation.AutoSize = true;
			this._tblLayoutOuter.SetColumnSpan(this._labelPrimaryOccupation, 2);
			this._labelPrimaryOccupation.Location = new System.Drawing.Point(365, 176);
			this._labelPrimaryOccupation.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelPrimaryOccupation.Name = "_labelPrimaryOccupation";
			this._labelPrimaryOccupation.Size = new System.Drawing.Size(99, 13);
			this._labelPrimaryOccupation.TabIndex = 10;
			this._labelPrimaryOccupation.Text = "Primary Occupation";
			// 
			// _primaryOccupation
			// 
			this._primaryOccupation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tblLayoutOuter.SetColumnSpan(this._primaryOccupation, 2);
			this._binder.SetIsBound(this._primaryOccupation, true);
			this._primaryOccupation.Location = new System.Drawing.Point(365, 192);
			this._primaryOccupation.Margin = new System.Windows.Forms.Padding(7, 3, 3, 0);
			this._primaryOccupation.Name = "_primaryOccupation";
			this._primaryOccupation.Size = new System.Drawing.Size(225, 20);
			this._primaryOccupation.TabIndex = 11;
			// 
			// _howToContact
			// 
			this._howToContact.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._tblLayoutOuter.SetColumnSpan(this._howToContact, 2);
			this._binder.SetIsBound(this._howToContact, true);
			this._howToContact.Location = new System.Drawing.Point(365, 233);
			this._howToContact.Margin = new System.Windows.Forms.Padding(7, 3, 0, 3);
			this._howToContact.Multiline = true;
			this._howToContact.Name = "_howToContact";
			this._howToContact.Size = new System.Drawing.Size(228, 76);
			this._howToContact.TabIndex = 13;
			// 
			// _labelHowToContact
			// 
			this._labelHowToContact.AutoSize = true;
			this._tblLayoutOuter.SetColumnSpan(this._labelHowToContact, 2);
			this._labelHowToContact.Location = new System.Drawing.Point(365, 217);
			this._labelHowToContact.Margin = new System.Windows.Forms.Padding(7, 5, 3, 0);
			this._labelHowToContact.Name = "_labelHowToContact";
			this._labelHowToContact.Size = new System.Drawing.Size(81, 13);
			this._labelHowToContact.TabIndex = 12;
			this._labelHowToContact.Text = "How to Contact";
			// 
			// PersonBasicEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tblLayoutOuter);
			this.Name = "PersonBasicEditor";
			this.Size = new System.Drawing.Size(607, 335);
			this._tblLayoutOuter.ResumeLayout(false);
			this._tblLayoutOuter.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._picture)).EndInit();
			this._pnlOtherLanguages.ResumeLayout(false);
			this._pnlOtherLanguages.PerformLayout();
			this._tblLayoutOtherLanguages.ResumeLayout(false);
			this._tblLayoutOtherLanguages.PerformLayout();
			this._pnlPrimaryLanguage.ResumeLayout(false);
			this._pnlPrimaryLanguage.PerformLayout();
			this._tblLayoutPrimaryLanguage.ResumeLayout(false);
			this._tblLayoutPrimaryLanguage.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _labelFullName;
		private System.Windows.Forms.TextBox _id;
		private System.Windows.Forms.Label _labelBirthYear;
		private System.Windows.Forms.TextBox _birthYear;
		private System.Windows.Forms.TableLayoutPanel _tblLayoutOuter;
		private SayMore.UI.LowLevelControls.UnderlinedHdgBox _pnlPrimaryLanguage;
		private AutoCompleteTextBox _primaryLanguage;
		private System.Windows.Forms.Label _labelPrimaryLanguageLearnedIn;
		private System.Windows.Forms.TextBox _primaryLanguageLearnedIn;
		private System.Windows.Forms.TableLayoutPanel _tblLayoutPrimaryLanguage;
		private System.Windows.Forms.Label _labelgender;
		private System.Windows.Forms.ComboBox _gender;
		private BindingHelper _binder;
		private SayMore.UI.LowLevelControls.ParentButton _pbPrimaryLangMother;
		private SayMore.UI.LowLevelControls.ParentButton _pbPrimaryLangFather;
		private SayMore.UI.LowLevelControls.UnderlinedHdgBox _pnlOtherLanguages;
		private System.Windows.Forms.TableLayoutPanel _tblLayoutOtherLanguages;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangMother0;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangFather0;
		private AutoCompleteTextBox _otherLanguage0;
		private AutoCompleteTextBox _otherLanguage1;
		private AutoCompleteTextBox _otherLanguage2;
		private AutoCompleteTextBox _otherLanguage3;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangMother3;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangMother2;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangMother1;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangFather1;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangFather2;
		private SayMore.UI.LowLevelControls.ParentButton _pbOtherLangFather3;
		private System.Windows.Forms.PictureBox _picture;
		private System.Windows.Forms.Label _labelHowToContact;
		private System.Windows.Forms.Label _labelEducation;
		private System.Windows.Forms.Label _labelPrimaryOccupation;
		private System.Windows.Forms.TextBox _education;
		private System.Windows.Forms.TextBox _primaryOccupation;
		private System.Windows.Forms.TextBox _howToContact;
	}
}
