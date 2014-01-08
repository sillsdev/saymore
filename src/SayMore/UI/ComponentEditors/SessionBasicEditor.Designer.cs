
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ComponentEditors
{
	partial class SessionBasicEditor
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
			this._labelMoreFields = new System.Windows.Forms.Label();
			this._panelAdditionalGrid = new System.Windows.Forms.Panel();
			this._access = new System.Windows.Forms.ComboBox();
			this._participants = new SayMore.UI.LowLevelControls.MultiValueDropDownBox();
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
			this._date = new SayMore.UI.LowLevelControls.DatePicker();
			this._labelCustomFields = new System.Windows.Forms.Label();
			this._panelGrid = new System.Windows.Forms.Panel();
			this._binder = new SayMore.UI.ComponentEditors.BindingHelper(this.components);
			this._autoCompleteHelper = new SayMore.UI.ComponentEditors.AutoCompleteHelper(this.components);
			this.locExtender = new L10NSharp.UI.L10NSharpExtender(this.components);
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
			this.locExtender.SetLocalizingId(this._labelId, "SessionsView.MetadataEditor._labelId");
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
			this.locExtender.SetLocalizationPriority(this._id, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._id, "SessionBasicEditor._id");
			this._id.Location = new System.Drawing.Point(0, 16);
			this._id.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._id.Name = "_id";
			this._id.Size = new System.Drawing.Size(202, 22);
			this._id.TabIndex = 1;
			this._autoCompleteHelper.SetUpdateGatherer(this._id, false);
			// 
			// _labelTitle
			// 
			this._labelTitle.AutoSize = true;
			this._labelTitle.BackColor = System.Drawing.Color.Transparent;
			this._labelTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelTitle, null);
			this.locExtender.SetLocalizationComment(this._labelTitle, null);
			this.locExtender.SetLocalizingId(this._labelTitle, "SessionsView.MetadataEditor._labelTitle");
			this._labelTitle.Location = new System.Drawing.Point(0, 46);
			this._labelTitle.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelTitle.Name = "_labelTitle";
			this._labelTitle.Size = new System.Drawing.Size(28, 13);
			this._labelTitle.TabIndex = 4;
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
			this.locExtender.SetLocalizationPriority(this._title, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._title, "SessionBasicEditor._title");
			this._title.Location = new System.Drawing.Point(0, 62);
			this._title.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(202, 22);
			this._title.TabIndex = 5;
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
			this.locExtender.SetLocalizationPriority(this._situation, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._situation, "SessionBasicEditor._situation");
			this._situation.Location = new System.Drawing.Point(0, 200);
			this._situation.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._situation.Multiline = true;
			this._situation.Name = "_situation";
			this._situation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._situation.Size = new System.Drawing.Size(202, 96);
			this._situation.TabIndex = 17;
			this._autoCompleteHelper.SetUpdateGatherer(this._situation, false);
			// 
			// _labelSynopsis
			// 
			this._labelSynopsis.AutoSize = true;
			this._labelSynopsis.BackColor = System.Drawing.Color.Transparent;
			this._labelSynopsis.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelSynopsis, null);
			this.locExtender.SetLocalizationComment(this._labelSynopsis, null);
			this.locExtender.SetLocalizingId(this._labelSynopsis, "SessionsView.MetadataEditor._labelDescription");
			this._labelSynopsis.Location = new System.Drawing.Point(212, 184);
			this._labelSynopsis.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._labelSynopsis.Name = "_labelSynopsis";
			this._labelSynopsis.Size = new System.Drawing.Size(66, 13);
			this._labelSynopsis.TabIndex = 18;
			this._labelSynopsis.Text = "Description";
			// 
			// _synopsis
			// 
			this._synopsis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._autoCompleteHelper.SetAutoCompleteKey(this._synopsis, "");
			this._synopsis.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._synopsis, true);
			this._binder.SetIsComponentFileId(this._synopsis, false);
			this.locExtender.SetLocalizableToolTip(this._synopsis, null);
			this.locExtender.SetLocalizationComment(this._synopsis, null);
			this.locExtender.SetLocalizationPriority(this._synopsis, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._synopsis, "SessionBasicEditor._synopsis");
			this._synopsis.Location = new System.Drawing.Point(212, 200);
			this._synopsis.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._synopsis.Multiline = true;
			this._synopsis.Name = "_synopsis";
			this._synopsis.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._synopsis.Size = new System.Drawing.Size(203, 96);
			this._synopsis.TabIndex = 19;
			this._autoCompleteHelper.SetUpdateGatherer(this._synopsis, false);
			// 
			// _tableLayout
			// 
			this._tableLayout.AutoSize = true;
			this._tableLayout.BackColor = System.Drawing.Color.Transparent;
			this._tableLayout.ColumnCount = 2;
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tableLayout.Controls.Add(this._labelMoreFields, 0, 10);
			this._tableLayout.Controls.Add(this._panelAdditionalGrid, 0, 11);
			this._tableLayout.Controls.Add(this._access, 1, 7);
			this._tableLayout.Controls.Add(this._participants, 0, 5);
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
			this._tableLayout.Controls.Add(this._date, 1, 1);
			this._tableLayout.Controls.Add(this._labelCustomFields, 1, 10);
			this._tableLayout.Controls.Add(this._panelGrid, 1, 11);
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
			this._tableLayout.Size = new System.Drawing.Size(415, 383);
			this._tableLayout.TabIndex = 0;
			this._tableLayout.TabStop = true;
			// 
			// _labelMoreFields
			// 
			this._labelMoreFields.AutoSize = true;
			this._labelMoreFields.BackColor = System.Drawing.Color.Transparent;
			this._labelMoreFields.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelMoreFields, null);
			this.locExtender.SetLocalizationComment(this._labelMoreFields, null);
			this.locExtender.SetLocalizingId(this._labelMoreFields, "SessionsView.MetadataEditor._labelMoreFields");
			this._labelMoreFields.Location = new System.Drawing.Point(0, 304);
			this._labelMoreFields.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelMoreFields.Name = "_labelMoreFields";
			this._labelMoreFields.Size = new System.Drawing.Size(67, 13);
			this._labelMoreFields.TabIndex = 20;
			this._labelMoreFields.Text = "More Fields";
			// 
			// _panelAdditionalGrid
			// 
			this._panelAdditionalGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._panelAdditionalGrid.Location = new System.Drawing.Point(0, 320);
			this._panelAdditionalGrid.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._panelAdditionalGrid.Name = "_panelAdditionalGrid";
			this._panelAdditionalGrid.Size = new System.Drawing.Size(202, 60);
			this._panelAdditionalGrid.TabIndex = 21;
			// 
			// _access
			// 
			this._access.Dock = System.Windows.Forms.DockStyle.Top;
			this._access.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._access, true);
			this._binder.SetIsComponentFileId(this._access, false);
			this.locExtender.SetLocalizableToolTip(this._access, null);
			this.locExtender.SetLocalizationComment(this._access, null);
			this.locExtender.SetLocalizationPriority(this._access, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._access, "SessionBasicEditor._access");
			this._access.Location = new System.Drawing.Point(212, 155);
			this._access.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._access.Name = "_access";
			this._access.Size = new System.Drawing.Size(203, 21);
			this._access.TabIndex = 15;
			// 
			// _participants
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._participants, "person");
			this._participants.BackColor = System.Drawing.SystemColors.Window;
			this._participants.Dock = System.Windows.Forms.DockStyle.Top;
			this._participants.Font = new System.Drawing.Font("Segoe UI", 9F);
			this._binder.SetIsBound(this._participants, true);
			this._binder.SetIsComponentFileId(this._participants, false);
			this._participants.JITListAcquisition = null;
			this.locExtender.SetLocalizableToolTip(this._participants, null);
			this.locExtender.SetLocalizationComment(this._participants, null);
			this.locExtender.SetLocalizationPriority(this._participants, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._participants, "SessionBasicEditor._participants");
			this._participants.Location = new System.Drawing.Point(0, 108);
			this._participants.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._participants.Name = "_participants";
			this._participants.Padding = new System.Windows.Forms.Padding(2);
			this._participants.Size = new System.Drawing.Size(202, 23);
			this._participants.TabIndex = 9;
			this._autoCompleteHelper.SetUpdateGatherer(this._participants, false);
			// 
			// _labelAccess
			// 
			this._labelAccess.AutoSize = true;
			this._labelAccess.BackColor = System.Drawing.Color.Transparent;
			this._labelAccess.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelAccess, null);
			this.locExtender.SetLocalizationComment(this._labelAccess, null);
			this.locExtender.SetLocalizingId(this._labelAccess, "SessionsView.MetadataEditor._labelAccess");
			this._labelAccess.Location = new System.Drawing.Point(212, 139);
			this._labelAccess.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._labelAccess.Name = "_labelAccess";
			this._labelAccess.Size = new System.Drawing.Size(40, 13);
			this._labelAccess.TabIndex = 14;
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
			this.locExtender.SetLocalizationPriority(this._genre, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._genre, "SessionBasicEditor._genre");
			this._genre.Location = new System.Drawing.Point(0, 155);
			this._genre.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._genre.Name = "_genre";
			this._genre.Size = new System.Drawing.Size(202, 21);
			this._genre.TabIndex = 13;
			this._genre.Leave += new System.EventHandler(this.HideTextSelection);
			// 
			// _labelGenre
			// 
			this._labelGenre.AutoSize = true;
			this._labelGenre.BackColor = System.Drawing.Color.Transparent;
			this._labelGenre.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelGenre, null);
			this.locExtender.SetLocalizationComment(this._labelGenre, null);
			this.locExtender.SetLocalizingId(this._labelGenre, "SessionsView.MetadataEditor._labelGenre");
			this._labelGenre.Location = new System.Drawing.Point(0, 139);
			this._labelGenre.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelGenre.Name = "_labelGenre";
			this._labelGenre.Size = new System.Drawing.Size(38, 13);
			this._labelGenre.TabIndex = 12;
			this._labelGenre.Text = "Genre";
			// 
			// _location
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._location, "");
			this._location.Dock = System.Windows.Forms.DockStyle.Top;
			this._location.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._location, true);
			this._binder.SetIsComponentFileId(this._location, false);
			this.locExtender.SetLocalizableToolTip(this._location, null);
			this.locExtender.SetLocalizationComment(this._location, null);
			this.locExtender.SetLocalizationPriority(this._location, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._location, "SessionBasicEditor._location");
			this._location.Location = new System.Drawing.Point(212, 108);
			this._location.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._location.Name = "_location";
			this._location.Size = new System.Drawing.Size(203, 22);
			this._location.TabIndex = 11;
			this._autoCompleteHelper.SetUpdateGatherer(this._location, false);
			// 
			// _labelLocation
			// 
			this._labelLocation.AutoSize = true;
			this._labelLocation.BackColor = System.Drawing.Color.Transparent;
			this._labelLocation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelLocation, null);
			this.locExtender.SetLocalizationComment(this._labelLocation, null);
			this.locExtender.SetLocalizingId(this._labelLocation, "SessionsView.MetadataEditor._labelLocation");
			this._labelLocation.Location = new System.Drawing.Point(212, 92);
			this._labelLocation.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._labelLocation.Name = "_labelLocation";
			this._labelLocation.Size = new System.Drawing.Size(51, 13);
			this._labelLocation.TabIndex = 10;
			this._labelLocation.Text = "Location";
			// 
			// _labelParticipants
			// 
			this._labelParticipants.AutoSize = true;
			this._labelParticipants.BackColor = System.Drawing.Color.Transparent;
			this._labelParticipants.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelParticipants, null);
			this.locExtender.SetLocalizationComment(this._labelParticipants, null);
			this.locExtender.SetLocalizingId(this._labelParticipants, "SessionsView.MetadataEditor._labelParticipants");
			this._labelParticipants.Location = new System.Drawing.Point(0, 92);
			this._labelParticipants.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelParticipants.Name = "_labelParticipants";
			this._labelParticipants.Size = new System.Drawing.Size(42, 13);
			this._labelParticipants.TabIndex = 8;
			this._labelParticipants.Text = "People";
			// 
			// _labelSetting
			// 
			this._labelSetting.AutoSize = true;
			this._labelSetting.BackColor = System.Drawing.Color.Transparent;
			this._labelSetting.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelSetting, null);
			this.locExtender.SetLocalizationComment(this._labelSetting, null);
			this.locExtender.SetLocalizingId(this._labelSetting, "SessionsView.MetadataEditor._labelSetting");
			this._labelSetting.Location = new System.Drawing.Point(212, 46);
			this._labelSetting.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._labelSetting.Name = "_labelSetting";
			this._labelSetting.Size = new System.Drawing.Size(44, 13);
			this._labelSetting.TabIndex = 6;
			this._labelSetting.Text = "Setting";
			// 
			// _setting
			// 
			this._autoCompleteHelper.SetAutoCompleteKey(this._setting, "");
			this._setting.Dock = System.Windows.Forms.DockStyle.Top;
			this._setting.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._setting, true);
			this._binder.SetIsComponentFileId(this._setting, false);
			this.locExtender.SetLocalizableToolTip(this._setting, null);
			this.locExtender.SetLocalizationComment(this._setting, null);
			this.locExtender.SetLocalizationPriority(this._setting, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._setting, "SessionBasicEditor._setting");
			this._setting.Location = new System.Drawing.Point(212, 62);
			this._setting.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._setting.Name = "_setting";
			this._setting.Size = new System.Drawing.Size(203, 22);
			this._setting.TabIndex = 7;
			this._autoCompleteHelper.SetUpdateGatherer(this._setting, false);
			// 
			// _labelSituation
			// 
			this._labelSituation.AutoSize = true;
			this._labelSituation.BackColor = System.Drawing.Color.Transparent;
			this._labelSituation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelSituation, null);
			this.locExtender.SetLocalizationComment(this._labelSituation, null);
			this.locExtender.SetLocalizingId(this._labelSituation, "SessionsView.MetadataEditor._labelSituation");
			this._labelSituation.Location = new System.Drawing.Point(0, 184);
			this._labelSituation.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelSituation.Name = "_labelSituation";
			this._labelSituation.Size = new System.Drawing.Size(54, 13);
			this._labelSituation.TabIndex = 116;
			this._labelSituation.Text = "Situation";
			// 
			// _labelDate
			// 
			this._labelDate.AutoSize = true;
			this._labelDate.BackColor = System.Drawing.Color.Transparent;
			this._labelDate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelDate, null);
			this.locExtender.SetLocalizationComment(this._labelDate, null);
			this.locExtender.SetLocalizingId(this._labelDate, "SessionsView.MetadataEditor._labelDate");
			this._labelDate.Location = new System.Drawing.Point(212, 0);
			this._labelDate.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
			this._labelDate.Name = "_labelDate";
			this._labelDate.Size = new System.Drawing.Size(31, 13);
			this._labelDate.TabIndex = 2;
			this._labelDate.Text = "Date";
			// 
			// _date
			// 
			this._date.CustomFormat = "";
			this._date.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this._binder.SetIsBound(this._date, true);
			this._binder.SetIsComponentFileId(this._date, false);
			this.locExtender.SetLocalizableToolTip(this._date, null);
			this.locExtender.SetLocalizationComment(this._date, null);
			this.locExtender.SetLocalizationPriority(this._date, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._date, "SessionBasicEditor._date");
			this._date.Location = new System.Drawing.Point(212, 16);
			this._date.Margin = new System.Windows.Forms.Padding(5, 3, 2, 3);
			this._date.Name = "_date";
			this._date.Size = new System.Drawing.Size(96, 22);
			this._date.TabIndex = 3;
			// 
			// _labelCustomFields
			// 
			this._labelCustomFields.AutoSize = true;
			this._labelCustomFields.BackColor = System.Drawing.Color.Transparent;
			this._labelCustomFields.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this._labelCustomFields, null);
			this.locExtender.SetLocalizationComment(this._labelCustomFields, null);
			this.locExtender.SetLocalizingId(this._labelCustomFields, "SessionsView.MetadataEditor._labelCustomFields");
			this._labelCustomFields.Location = new System.Drawing.Point(207, 304);
			this._labelCustomFields.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._labelCustomFields.Name = "_labelCustomFields";
			this._labelCustomFields.Size = new System.Drawing.Size(79, 13);
			this._labelCustomFields.TabIndex = 22;
			this._labelCustomFields.Text = "Custom Fields";
			// 
			// _panelGrid
			// 
			this._panelGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._panelGrid.Location = new System.Drawing.Point(207, 320);
			this._panelGrid.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._panelGrid.Name = "_panelGrid";
			this._panelGrid.Size = new System.Drawing.Size(203, 60);
			this._panelGrid.TabIndex = 23;
			// 
			// locExtender
			// 
			this.locExtender.LocalizationManagerId = "SayMore";
			this.locExtender.PrefixForNewItems = null;
			// 
			// SessionBasicEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tableLayout);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this, "SessionBasicEditor.EditorBase");
			this.Name = "SessionBasicEditor";
			this.Size = new System.Drawing.Size(429, 400);
			this.VisibleChanged += new System.EventHandler(this.HideTextSelection);
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
		private DatePicker _date;
		private System.Windows.Forms.Label _labelParticipants;
		private System.Windows.Forms.Label _labelSituation;
		private System.Windows.Forms.Label _labelSetting;
		private System.Windows.Forms.TextBox _setting;
		private System.Windows.Forms.Label _labelLocation;
		private System.Windows.Forms.TextBox _location;
		private System.Windows.Forms.Label _labelGenre;
		private System.Windows.Forms.ComboBox _genre;
		private System.Windows.Forms.Label _labelAccess;
		private BindingHelper _binder;
		private System.Windows.Forms.Label _labelCustomFields;
		private AutoCompleteHelper _autoCompleteHelper;
		private System.Windows.Forms.Panel _panelGrid;
		private SayMore.UI.LowLevelControls.MultiValueDropDownBox _participants;
		private System.Windows.Forms.Label _labelDate;
		private L10NSharp.UI.L10NSharpExtender locExtender;
		private System.Windows.Forms.ComboBox _access;
		private System.Windows.Forms.Label _labelMoreFields;
		private System.Windows.Forms.Panel _panelAdditionalGrid;
	}
}
