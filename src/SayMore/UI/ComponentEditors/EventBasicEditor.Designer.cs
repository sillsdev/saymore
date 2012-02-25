using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ComponentEditors
{
	partial class EventBasicEditor
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
			this._labelId = new System.Windows.Forms.Label();
			this._id = new System.Windows.Forms.TextBox();
			this._labelTitle = new System.Windows.Forms.Label();
			this._title = new System.Windows.Forms.TextBox();
			this._situation = new System.Windows.Forms.TextBox();
			this._labelSynopsis = new System.Windows.Forms.Label();
			this._synopsis = new System.Windows.Forms.TextBox();
			this._tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this._participants = new SayMore.UI.LowLevelControls.MultiValueComboBox();
			this._panelGrid = new System.Windows.Forms.Panel();
			this._labelCustomFields = new System.Windows.Forms.Label();
			this._access = new System.Windows.Forms.TextBox();
			this._labelAccess = new System.Windows.Forms.Label();
			this._genre = new System.Windows.Forms.ComboBox();
			this._labelGenre = new System.Windows.Forms.Label();
			this._location = new System.Windows.Forms.TextBox();
			this._labelLocation = new System.Windows.Forms.Label();
			this._labelParticipants = new System.Windows.Forms.Label();
			this._labelSetting = new System.Windows.Forms.Label();
			this._setting = new System.Windows.Forms.TextBox();
			this._labelSituation = new System.Windows.Forms.Label();
			this._labelDate = new System.Windows.Forms.Label();
			this._labelStatus = new System.Windows.Forms.Label();
			this._status = new System.Windows.Forms.ComboBox();
			this._date = new DatePicker();
			this._binder = new SayMore.UI.ComponentEditors.BindingHelper(this.components);
			this._autoCompleteHelper = new SayMore.UI.ComponentEditors.AutoCompleteHelper(this.components);
			this.locExtender = new Localization.UI.LocalizationExtender(this.components);
			this._tableLayout.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// _labelId
			// 
			this._labelId.AutoSize = true;
			this._labelId.BackColor = System.Drawing.Color.Transparent;
			this._labelId.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelId, null);
			this.locExtender.SetLocalizationComment(this._labelId, null);
			this.locExtender.SetLocalizingId(this._labelId, "EventsView.MetadataEditor._labelId");
			this._labelId.Location = new System.Drawing.Point(0, 0);
			this._labelId.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this._labelId.Name = "_labelId";
			this._labelId.Size = new System.Drawing.Size(17, 13);
			this._labelId.TabIndex = 0;
			this._labelId.Text = "Id";
			// 
			// _id
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._id, "");
			this._id.Dock = System.Windows.Forms.DockStyle.Top;
			this._id.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._id, true);
			this._binder.SetIsComponentFileId(this._id, true);
			this.locExtender.SetLocalizableToolTip(this._id, null);
			this.locExtender.SetLocalizationComment(this._id, null);
			this.locExtender.SetLocalizationPriority(this._id, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._id, "EventBasicEditor._id");
			this._id.Location = new System.Drawing.Point(0, 16);
			this._id.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._id.Name = "_id";
			this._id.Size = new System.Drawing.Size(202, 22);
			this._id.TabIndex = 1;
			this._autoCompleteHelper.SetUpdateGatherer(this._id, false);
			this._id.Enter += new System.EventHandler(this.HandleIdEnter);
			// 
			// _labelTitle
			// 
			this._labelTitle.AutoSize = true;
			this._labelTitle.BackColor = System.Drawing.Color.Transparent;
			this._labelTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelTitle, null);
			this.locExtender.SetLocalizationComment(this._labelTitle, null);
			this.locExtender.SetLocalizingId(this._labelTitle, "EventsView.MetadataEditor._labelTitle");
			this._labelTitle.Location = new System.Drawing.Point(0, 47);
			this._labelTitle.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelTitle.Name = "_labelTitle";
			this._labelTitle.Size = new System.Drawing.Size(28, 13);
			this._labelTitle.TabIndex = 6;
			this._labelTitle.Text = "Title";
			// 
			// _title
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._title, "");
			this._title.Dock = System.Windows.Forms.DockStyle.Top;
			this._title.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._title, true);
			this._binder.SetIsComponentFileId(this._title, false);
			this.locExtender.SetLocalizableToolTip(this._title, null);
			this.locExtender.SetLocalizationComment(this._title, null);
			this.locExtender.SetLocalizationPriority(this._title, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._title, "EventBasicEditor._title");
			this._title.Location = new System.Drawing.Point(0, 63);
			this._title.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(202, 22);
			this._title.TabIndex = 7;
			this._autoCompleteHelper.SetUpdateGatherer(this._title, false);
			// 
			// _situation
			// 
			this._situation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._situation, "");
			this._situation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._situation, true);
			this._binder.SetIsComponentFileId(this._situation, false);
			this.locExtender.SetLocalizableToolTip(this._situation, null);
			this.locExtender.SetLocalizationComment(this._situation, null);
			this.locExtender.SetLocalizationPriority(this._situation, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._situation, "EventBasicEditor._situation");
			this._situation.Location = new System.Drawing.Point(0, 202);
			this._situation.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._situation.Multiline = true;
			this._situation.Name = "_situation";
			this._situation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._situation.Size = new System.Drawing.Size(202, 96);
			this._situation.TabIndex = 19;
			this._autoCompleteHelper.SetUpdateGatherer(this._situation, false);
			// 
			// _labelSynopsis
			// 
			this._labelSynopsis.AutoSize = true;
			this._labelSynopsis.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.SetColumnSpan(this._labelSynopsis, 2);
			this._labelSynopsis.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelSynopsis, null);
			this.locExtender.SetLocalizationComment(this._labelSynopsis, null);
			this.locExtender.SetLocalizingId(this._labelSynopsis, "EventsView.MetadataEditor._labelAbstract");
			this._labelSynopsis.Location = new System.Drawing.Point(212, 186);
			this._labelSynopsis.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._labelSynopsis.Name = "_labelSynopsis";
			this._labelSynopsis.Size = new System.Drawing.Size(49, 13);
			this._labelSynopsis.TabIndex = 20;
			this._labelSynopsis.Text = "Abstract";
			// 
			// _synopsis
			// 
			this._synopsis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._synopsis, "");
			this._tableLayout.SetColumnSpan(this._synopsis, 2);
			this._synopsis.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._synopsis, true);
			this._binder.SetIsComponentFileId(this._synopsis, false);
			this.locExtender.SetLocalizableToolTip(this._synopsis, null);
			this.locExtender.SetLocalizationComment(this._synopsis, null);
			this.locExtender.SetLocalizationPriority(this._synopsis, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._synopsis, "EventBasicEditor._synopsis");
			this._synopsis.Location = new System.Drawing.Point(212, 202);
			this._synopsis.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._synopsis.Multiline = true;
			this._synopsis.Name = "_synopsis";
			this._synopsis.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._synopsis.Size = new System.Drawing.Size(203, 96);
			this._synopsis.TabIndex = 21;
			this._autoCompleteHelper.SetUpdateGatherer(this._synopsis, false);
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 3;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this._tableLayout.Controls.Add(this._participants, 0, 5);
			this._tableLayout.Controls.Add(this._panelGrid, 0, 11);
			this._tableLayout.Controls.Add(this._labelCustomFields, 0, 10);
			this._tableLayout.Controls.Add(this._access, 1, 7);
			this._tableLayout.Controls.Add(this._synopsis, 1, 9);
			this._tableLayout.Controls.Add(this._labelSynopsis, 1, 8);
			this._tableLayout.Controls.Add(this._situation, 0, 9);
			this._tableLayout.Controls.Add(this._labelAccess, 1, 6);
			this._tableLayout.Controls.Add(this._genre, 0, 7);
			this._tableLayout.Controls.Add(this._labelGenre, 0, 6);
			this._tableLayout.Controls.Add(this._location, 1, 5);
			this._tableLayout.Controls.Add(this._labelLocation, 1, 4);
			this._tableLayout.Controls.Add(this._labelParticipants, 0, 4);
			this._tableLayout.Controls.Add(this._labelId, 0, 0);
			this._tableLayout.Controls.Add(this._id, 0, 1);
			this._tableLayout.Controls.Add(this._labelTitle, 0, 2);
			this._tableLayout.Controls.Add(this._title, 0, 3);
			this._tableLayout.Controls.Add(this._labelSetting, 1, 2);
			this._tableLayout.Controls.Add(this._setting, 1, 3);
			this._tableLayout.Controls.Add(this._labelSituation, 0, 8);
			this._tableLayout.Controls.Add(this._labelDate, 1, 0);
			this._tableLayout.Controls.Add(this._labelStatus, 2, 0);
			this._tableLayout.Controls.Add(this._status, 2, 1);
			this._tableLayout.Controls.Add(this._date, 1, 1);
			this._tableLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this._tableLayout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._tableLayout.Location = new System.Drawing.Point(7, 7);
			this._tableLayout.Name = "_tableLayout";
			this._tableLayout.RowCount = 12;
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
			this._tableLayout.Size = new System.Drawing.Size(415, 385);
			this._tableLayout.TabIndex = 0;
			this._tableLayout.TabStop = true;
			// 
			// _participants
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._participants, "person");
			this._participants.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._participants.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._participants.BackColor = System.Drawing.SystemColors.Window;
			this._participants.Dock = System.Windows.Forms.DockStyle.Top;
			this._participants.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._binder.SetIsBound(this._participants, true);
			this._binder.SetIsComponentFileId(this._participants, false);
			this.locExtender.SetLocalizableToolTip(this._participants, null);
			this.locExtender.SetLocalizationComment(this._participants, null);
			this.locExtender.SetLocalizationPriority(this._participants, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._participants, "EventBasicEditor.MultiValueComboBox");
			this._participants.Location = new System.Drawing.Point(0, 109);
			this._participants.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._participants.Name = "_participants";
			this._participants.Padding = new System.Windows.Forms.Padding(2);
			this._participants.ReadOnly = true;
			this._participants.SelectionLength = 0;
			this._participants.SelectionStart = 0;
			this._participants.Size = new System.Drawing.Size(202, 23);
			this._participants.TabIndex = 11;
			this._autoCompleteHelper.SetUpdateGatherer(this._participants, false);
			// 
			// _panelGrid
			// 
			this._panelGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._panelGrid.Location = new System.Drawing.Point(0, 322);
			this._panelGrid.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._panelGrid.Name = "_panelGrid";
			this._panelGrid.Size = new System.Drawing.Size(202, 60);
			this._panelGrid.TabIndex = 23;
			// 
			// _labelCustomFields
			// 
			this._labelCustomFields.AutoSize = true;
			this._labelCustomFields.BackColor = System.Drawing.Color.Transparent;
			this._labelCustomFields.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelCustomFields, null);
			this.locExtender.SetLocalizationComment(this._labelCustomFields, null);
			this.locExtender.SetLocalizingId(this._labelCustomFields, "EventsView.MetadataEditor._labelCustomFields");
			this._labelCustomFields.Location = new System.Drawing.Point(0, 306);
			this._labelCustomFields.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelCustomFields.Name = "_labelCustomFields";
			this._labelCustomFields.Size = new System.Drawing.Size(79, 13);
			this._labelCustomFields.TabIndex = 22;
			this._labelCustomFields.Text = "Custom Fields";
			// 
			// _access
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._access, "access");
			this._access.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._access.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tableLayout.SetColumnSpan(this._access, 2);
			this._access.Dock = System.Windows.Forms.DockStyle.Top;
			this._access.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._access, true);
			this._binder.SetIsComponentFileId(this._access, false);
			this.locExtender.SetLocalizableToolTip(this._access, null);
			this.locExtender.SetLocalizationComment(this._access, null);
			this.locExtender.SetLocalizationPriority(this._access, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._access, "EventBasicEditor._access");
			this._access.Location = new System.Drawing.Point(212, 156);
			this._access.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._access.Name = "_access";
			this._access.Size = new System.Drawing.Size(203, 22);
			this._access.TabIndex = 17;
			this._autoCompleteHelper.SetUpdateGatherer(this._access, false);
			// 
			// _labelAccess
			// 
			this._labelAccess.AutoSize = true;
			this._labelAccess.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.SetColumnSpan(this._labelAccess, 2);
			this._labelAccess.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelAccess, null);
			this.locExtender.SetLocalizationComment(this._labelAccess, null);
			this.locExtender.SetLocalizingId(this._labelAccess, "EventsView.MetadataEditor._labelAccess");
			this._labelAccess.Location = new System.Drawing.Point(212, 140);
			this._labelAccess.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._labelAccess.Name = "_labelAccess";
			this._labelAccess.Size = new System.Drawing.Size(40, 13);
			this._labelAccess.TabIndex = 16;
			this._labelAccess.Text = "Access";
			// 
			// _genre
			// 
			this._genre.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._genre.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._genre.Dock = System.Windows.Forms.DockStyle.Top;
			this._genre.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._genre, true);
			this._binder.SetIsComponentFileId(this._genre, false);
			this.locExtender.SetLocalizableToolTip(this._genre, null);
			this.locExtender.SetLocalizationComment(this._genre, null);
			this.locExtender.SetLocalizationPriority(this._genre, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._genre, "EventBasicEditor._genre");
			this._genre.Location = new System.Drawing.Point(0, 156);
			this._genre.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._genre.Name = "_genre";
			this._genre.Size = new System.Drawing.Size(202, 21);
			this._genre.TabIndex = 15;
			// 
			// _labelGenre
			// 
			this._labelGenre.AutoSize = true;
			this._labelGenre.BackColor = System.Drawing.Color.Transparent;
			this._labelGenre.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelGenre, null);
			this.locExtender.SetLocalizationComment(this._labelGenre, null);
			this.locExtender.SetLocalizingId(this._labelGenre, "EventsView.MetadataEditor._labelGenre");
			this._labelGenre.Location = new System.Drawing.Point(0, 140);
			this._labelGenre.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelGenre.Name = "_labelGenre";
			this._labelGenre.Size = new System.Drawing.Size(38, 13);
			this._labelGenre.TabIndex = 14;
			this._labelGenre.Text = "Genre";
			// 
			// _location
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._location, "location");
			this._location.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._location.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tableLayout.SetColumnSpan(this._location, 2);
			this._location.Dock = System.Windows.Forms.DockStyle.Top;
			this._location.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._location, true);
			this._binder.SetIsComponentFileId(this._location, false);
			this.locExtender.SetLocalizableToolTip(this._location, null);
			this.locExtender.SetLocalizationComment(this._location, null);
			this.locExtender.SetLocalizationPriority(this._location, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._location, "EventBasicEditor._location");
			this._location.Location = new System.Drawing.Point(212, 109);
			this._location.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._location.Name = "_location";
			this._location.Size = new System.Drawing.Size(203, 22);
			this._location.TabIndex = 13;
			this._autoCompleteHelper.SetUpdateGatherer(this._location, false);
			// 
			// _labelLocation
			// 
			this._labelLocation.AutoSize = true;
			this._labelLocation.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.SetColumnSpan(this._labelLocation, 2);
			this._labelLocation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelLocation, null);
			this.locExtender.SetLocalizationComment(this._labelLocation, null);
			this.locExtender.SetLocalizingId(this._labelLocation, "EventsView.MetadataEditor._labelLocation");
			this._labelLocation.Location = new System.Drawing.Point(212, 93);
			this._labelLocation.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._labelLocation.Name = "_labelLocation";
			this._labelLocation.Size = new System.Drawing.Size(51, 13);
			this._labelLocation.TabIndex = 12;
			this._labelLocation.Text = "Location";
			// 
			// _labelParticipants
			// 
			this._labelParticipants.AutoSize = true;
			this._labelParticipants.BackColor = System.Drawing.Color.Transparent;
			this._labelParticipants.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelParticipants, null);
			this.locExtender.SetLocalizationComment(this._labelParticipants, null);
			this.locExtender.SetLocalizingId(this._labelParticipants, "EventsView.MetadataEditor._labelParticipants");
			this._labelParticipants.Location = new System.Drawing.Point(0, 93);
			this._labelParticipants.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelParticipants.Name = "_labelParticipants";
			this._labelParticipants.Size = new System.Drawing.Size(42, 13);
			this._labelParticipants.TabIndex = 10;
			this._labelParticipants.Text = "People";
			// 
			// _labelSetting
			// 
			this._labelSetting.AutoSize = true;
			this._labelSetting.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.SetColumnSpan(this._labelSetting, 2);
			this._labelSetting.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelSetting, null);
			this.locExtender.SetLocalizationComment(this._labelSetting, null);
			this.locExtender.SetLocalizingId(this._labelSetting, "EventsView.MetadataEditor._labelSetting");
			this._labelSetting.Location = new System.Drawing.Point(212, 47);
			this._labelSetting.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._labelSetting.Name = "_labelSetting";
			this._labelSetting.Size = new System.Drawing.Size(44, 13);
			this._labelSetting.TabIndex = 8;
			this._labelSetting.Text = "Setting";
			// 
			// _setting
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._setting, "setting");
			this._setting.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._setting.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tableLayout.SetColumnSpan(this._setting, 2);
			this._setting.Dock = System.Windows.Forms.DockStyle.Top;
			this._setting.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._setting, true);
			this._binder.SetIsComponentFileId(this._setting, false);
			this.locExtender.SetLocalizableToolTip(this._setting, null);
			this.locExtender.SetLocalizationComment(this._setting, null);
			this.locExtender.SetLocalizationPriority(this._setting, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._setting, "EventBasicEditor._setting");
			this._setting.Location = new System.Drawing.Point(212, 63);
			this._setting.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._setting.Name = "_setting";
			this._setting.Size = new System.Drawing.Size(203, 22);
			this._setting.TabIndex = 9;
			this._autoCompleteHelper.SetUpdateGatherer(this._setting, false);
			// 
			// _labelSituation
			// 
			this._labelSituation.AutoSize = true;
			this._labelSituation.BackColor = System.Drawing.Color.Transparent;
			this._labelSituation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelSituation, null);
			this.locExtender.SetLocalizationComment(this._labelSituation, null);
			this.locExtender.SetLocalizingId(this._labelSituation, "EventsView.MetadataEditor._labelSituation");
			this._labelSituation.Location = new System.Drawing.Point(0, 186);
			this._labelSituation.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelSituation.Name = "_labelSituation";
			this._labelSituation.Size = new System.Drawing.Size(54, 13);
			this._labelSituation.TabIndex = 18;
			this._labelSituation.Text = "Situation";
			// 
			// _labelDate
			// 
			this._labelDate.AutoSize = true;
			this._labelDate.BackColor = System.Drawing.Color.Transparent;
			this._labelDate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelDate, null);
			this.locExtender.SetLocalizationComment(this._labelDate, null);
			this.locExtender.SetLocalizingId(this._labelDate, "EventsView.MetadataEditor._labelDate");
			this._labelDate.Location = new System.Drawing.Point(212, 0);
			this._labelDate.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
			this._labelDate.Name = "_labelDate";
			this._labelDate.Size = new System.Drawing.Size(31, 13);
			this._labelDate.TabIndex = 2;
			this._labelDate.Text = "Date";
			// 
			// _labelStatus
			// 
			this._labelStatus.AutoSize = true;
			this._labelStatus.BackColor = System.Drawing.Color.Transparent;
			this._labelStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelStatus, null);
			this.locExtender.SetLocalizationComment(this._labelStatus, null);
			this.locExtender.SetLocalizingId(this._labelStatus, "EventsView.MetadataEditor._labelStatus");
			this._labelStatus.Location = new System.Drawing.Point(316, 0);
			this._labelStatus.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this._labelStatus.Name = "_labelStatus";
			this._labelStatus.Size = new System.Drawing.Size(39, 13);
			this._labelStatus.TabIndex = 4;
			this._labelStatus.Text = "Status";
			// 
			// _status
			// 
			this._status.Dock = System.Windows.Forms.DockStyle.Top;
			this._status.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this._status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._status.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this._status.FormattingEnabled = true;
			this._binder.SetIsBound(this._status, true);
			this._binder.SetIsComponentFileId(this._status, false);
			this.locExtender.SetLocalizableToolTip(this._status, null);
			this.locExtender.SetLocalizationComment(this._status, null);
			this.locExtender.SetLocalizationPriority(this._status, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._status, "EventBasicEditor._status");
			this._status.Location = new System.Drawing.Point(314, 16);
			this._status.Margin = new System.Windows.Forms.Padding(4, 3, 3, 3);
			this._status.Name = "_status";
			this._status.Size = new System.Drawing.Size(98, 23);
			this._status.TabIndex = 5;
			this._status.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.HandleStatusDrawItem);
			// 
			// _date
			// 
			this._date.CustomFormat = "";
			this._date.Dock = System.Windows.Forms.DockStyle.Top;
			this._date.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this._binder.SetIsBound(this._date, true);
			this._binder.SetIsComponentFileId(this._date, false);
			this.locExtender.SetLocalizableToolTip(this._date, null);
			this.locExtender.SetLocalizationComment(this._date, null);
			this.locExtender.SetLocalizationPriority(this._date, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._date, "EventBasicEditor._date");
			this._date.Location = new System.Drawing.Point(212, 16);
			this._date.Margin = new System.Windows.Forms.Padding(5, 3, 2, 3);
			this._date.Name = "_date";
			this._date.Size = new System.Drawing.Size(96, 22);
			this._date.TabIndex = 3;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			// 
			// EventBasicEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, Localization.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "EventBasicEditor.EditorBase");
			this.Name = "EventBasicEditor";
			this.Size = new System.Drawing.Size(429, 400);
			this._tableLayout.ResumeLayout(false);
			this._tableLayout.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _labelId;
		private System.Windows.Forms.TextBox _id;
		private System.Windows.Forms.Label _labelTitle;
		private System.Windows.Forms.TextBox _title;
		private System.Windows.Forms.TextBox _situation;
		private System.Windows.Forms.Label _labelSynopsis;
		private System.Windows.Forms.TextBox _synopsis;
		private System.Windows.Forms.TableLayoutPanel _tableLayout;
		private System.Windows.Forms.Label _labelStatus;
		private DatePicker _date;
		private System.Windows.Forms.Label _labelParticipants;
		private System.Windows.Forms.Label _labelSituation;
		private System.Windows.Forms.Label _labelSetting;
		private System.Windows.Forms.TextBox _setting;
		private System.Windows.Forms.Label _labelLocation;
		private System.Windows.Forms.TextBox _location;
		private System.Windows.Forms.Label _labelGenre;
		private System.Windows.Forms.ComboBox _genre;
		private System.Windows.Forms.TextBox _access;
		private System.Windows.Forms.Label _labelAccess;
		private BindingHelper _binder;
		private System.Windows.Forms.Label _labelCustomFields;
		private AutoCompleteHelper _autoCompleteHelper;
		private System.Windows.Forms.Panel _panelGrid;
		private SayMore.UI.LowLevelControls.MultiValueComboBox _participants;
		private System.Windows.Forms.Label _labelDate;
		private System.Windows.Forms.ComboBox _status;
		private Localization.UI.LocalizationExtender locExtender;
	}
}
