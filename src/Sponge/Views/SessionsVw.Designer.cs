using SIL.Sponge.Controls;

namespace SIL.Sponge.Views
{
	partial class SessionsVw
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tabSessions = new System.Windows.Forms.TabControl();
			this.tpgDescription = new System.Windows.Forms.TabPage();
			this.tblDescription = new System.Windows.Forms.TableLayoutPanel();
			this.pnlDescriptionLeftSide = new System.Windows.Forms.Panel();
			this._eventType = new System.Windows.Forms.ComboBox();
			this._date = new System.Windows.Forms.DateTimePicker();
			this.lblId = new System.Windows.Forms.Label();
			this._situation = new System.Windows.Forms.TextBox();
			this._id = new System.Windows.Forms.TextBox();
			this.lblSituation = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblEventType = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this._location = new System.Windows.Forms.TextBox();
			this._title = new System.Windows.Forms.TextBox();
			this.lblLocation = new System.Windows.Forms.Label();
			this.lblParticipants = new System.Windows.Forms.Label();
			this._setting = new System.Windows.Forms.TextBox();
			this._participants = new System.Windows.Forms.TextBox();
			this.lblSetting = new System.Windows.Forms.Label();
			this.lblAccess = new System.Windows.Forms.Label();
			this._access = new System.Windows.Forms.TextBox();
			this.pnlDescriptionRightSide = new System.Windows.Forms.Panel();
			this.lblMedia = new System.Windows.Forms.Label();
			this.m_mediaPanel = new SIL.Sponge.Controls.MultimediaScroll();
			this._synopsis = new System.Windows.Forms.TextBox();
			this.lblSynopsis = new System.Windows.Forms.Label();
			this.tpgFiles = new System.Windows.Forms.TabPage();
			this.splitFileTab = new System.Windows.Forms.SplitContainer();
			this.lnkSessionPath = new System.Windows.Forms.LinkLabel();
			this.lblEmptySessionMsg = new System.Windows.Forms.Label();
			this.gridFiles = new SilUtils.SilGrid();
			this.iconCol = new System.Windows.Forms.DataGridViewImageColumn();
			this.filesNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesTypeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesTagsCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesDateCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filesSizeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this._infoPanel = new SIL.Sponge.Controls.InfoPanel();
			this.pnlFileInfoNotes = new System.Windows.Forms.Panel();
			this._fileInfoNotes = new SIL.Sponge.Controls.HoverCueTextBox();
			this.lblFileInfoNotes = new System.Windows.Forms.Label();
			this.btnNewFromFiles = new System.Windows.Forms.Button();
			this.lpSessions = new SIL.Sponge.Controls.ListPanel();
			this.lblNoSessionsMsg = new System.Windows.Forms.Label();
			this.locExtender = new SIL.Localize.LocalizationUtils.LocalizationExtender(this.components);
			this._fileContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.textBoxSpellChecker1 = new Palaso.UI.WindowsForms.Spelling.TextBoxSpellChecker();
			this.splitOuter.Panel1.SuspendLayout();
			this.splitOuter.Panel2.SuspendLayout();
			this.splitOuter.SuspendLayout();
			this.splitRightSide.Panel1.SuspendLayout();
			this.splitRightSide.SuspendLayout();
			this.tabSessions.SuspendLayout();
			this.tpgDescription.SuspendLayout();
			this.tblDescription.SuspendLayout();
			this.pnlDescriptionLeftSide.SuspendLayout();
			this.pnlDescriptionRightSide.SuspendLayout();
			this.tpgFiles.SuspendLayout();
			this.splitFileTab.Panel1.SuspendLayout();
			this.splitFileTab.Panel2.SuspendLayout();
			this.splitFileTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridFiles)).BeginInit();
			this.pnlFileInfoNotes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).BeginInit();
			this.SuspendLayout();
			// 
			// splitOuter
			// 
			// 
			// splitOuter.Panel1
			// 
			this.splitOuter.Panel1.Controls.Add(this.lblNoSessionsMsg);
			this.splitOuter.Panel1.Controls.Add(this.btnNewFromFiles);
			this.splitOuter.Panel1.Controls.Add(this.lpSessions);
			this.splitOuter.Panel1MinSize = 165;
			this.splitOuter.Size = new System.Drawing.Size(800, 466);
			this.splitOuter.SplitterDistance = 173;
			this.splitOuter.TabStop = false;
			// 
			// splitRightSide
			// 
			// 
			// splitRightSide.Panel1
			// 
			this.splitRightSide.Panel1.Controls.Add(this.tabSessions);
			this.splitRightSide.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 2, 0);
			this.splitRightSide.Panel2Collapsed = true;
			this.splitRightSide.Size = new System.Drawing.Size(623, 466);
			this.splitRightSide.SplitterDistance = 358;
			this.splitRightSide.TabStop = false;
			// 
			// tabSessions
			// 
			this.tabSessions.Controls.Add(this.tpgDescription);
			this.tabSessions.Controls.Add(this.tpgFiles);
			this.tabSessions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabSessions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabSessions.ItemSize = new System.Drawing.Size(65, 22);
			this.tabSessions.Location = new System.Drawing.Point(0, 3);
			this.tabSessions.Name = "tabSessions";
			this.tabSessions.SelectedIndex = 0;
			this.tabSessions.Size = new System.Drawing.Size(621, 463);
			this.tabSessions.TabIndex = 0;
			this.tabSessions.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabSessions_Selected);
			this.tabSessions.SizeChanged += new System.EventHandler(this.tabSessions_SizeChanged);
			// 
			// tpgDescription
			// 
			this.tpgDescription.Controls.Add(this.tblDescription);
			this.locExtender.SetLocalizableToolTip(this.tpgDescription, null);
			this.locExtender.SetLocalizationComment(this.tpgDescription, null);
			this.locExtender.SetLocalizingId(this.tpgDescription, "SessionsVw.tpgDescription");
			this.tpgDescription.Location = new System.Drawing.Point(4, 26);
			this.tpgDescription.Name = "tpgDescription";
			this.tpgDescription.Padding = new System.Windows.Forms.Padding(0, 2, 2, 1);
			this.tpgDescription.Size = new System.Drawing.Size(613, 433);
			this.tpgDescription.TabIndex = 0;
			this.tpgDescription.Text = "Description";
			this.tpgDescription.ToolTipText = "Description";
			this.tpgDescription.UseVisualStyleBackColor = true;
			// 
			// tblDescription
			// 
			this.tblDescription.BackColor = System.Drawing.Color.Transparent;
			this.tblDescription.ColumnCount = 2;
			this.tblDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tblDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
			this.tblDescription.Controls.Add(this.pnlDescriptionLeftSide, 0, 0);
			this.tblDescription.Controls.Add(this.pnlDescriptionRightSide, 1, 0);
			this.tblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tblDescription.Location = new System.Drawing.Point(0, 2);
			this.tblDescription.Name = "tblDescription";
			this.tblDescription.RowCount = 1;
			this.tblDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tblDescription.Size = new System.Drawing.Size(611, 430);
			this.tblDescription.TabIndex = 0;
			// 
			// pnlDescriptionLeftSide
			// 
			this.pnlDescriptionLeftSide.Controls.Add(this._eventType);
			this.pnlDescriptionLeftSide.Controls.Add(this._date);
			this.pnlDescriptionLeftSide.Controls.Add(this.lblId);
			this.pnlDescriptionLeftSide.Controls.Add(this._situation);
			this.pnlDescriptionLeftSide.Controls.Add(this._id);
			this.pnlDescriptionLeftSide.Controls.Add(this.lblSituation);
			this.pnlDescriptionLeftSide.Controls.Add(this.label1);
			this.pnlDescriptionLeftSide.Controls.Add(this.lblEventType);
			this.pnlDescriptionLeftSide.Controls.Add(this.lblTitle);
			this.pnlDescriptionLeftSide.Controls.Add(this._location);
			this.pnlDescriptionLeftSide.Controls.Add(this._title);
			this.pnlDescriptionLeftSide.Controls.Add(this.lblLocation);
			this.pnlDescriptionLeftSide.Controls.Add(this.lblParticipants);
			this.pnlDescriptionLeftSide.Controls.Add(this._setting);
			this.pnlDescriptionLeftSide.Controls.Add(this._participants);
			this.pnlDescriptionLeftSide.Controls.Add(this.lblSetting);
			this.pnlDescriptionLeftSide.Controls.Add(this.lblAccess);
			this.pnlDescriptionLeftSide.Controls.Add(this._access);
			this.pnlDescriptionLeftSide.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlDescriptionLeftSide.Location = new System.Drawing.Point(3, 3);
			this.pnlDescriptionLeftSide.Name = "pnlDescriptionLeftSide";
			this.pnlDescriptionLeftSide.Size = new System.Drawing.Size(268, 424);
			this.pnlDescriptionLeftSide.TabIndex = 0;
			// 
			// _eventType
			// 
			this._eventType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._eventType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._eventType.DropDownWidth = 200;
			this._eventType.FormattingEnabled = true;
			this.locExtender.SetLocalizableToolTip(this._eventType, null);
			this.locExtender.SetLocalizationComment(this._eventType, null);
			this.locExtender.SetLocalizationPriority(this._eventType, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._eventType, "SessionsVw.cboEventType");
			this._eventType.Location = new System.Drawing.Point(6, 160);
			this._eventType.MaxDropDownItems = 15;
			this._eventType.Name = "_eventType";
			this._eventType.Size = new System.Drawing.Size(107, 23);
			this._eventType.TabIndex = 18;
			// 
			// _date
			// 
			this._date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._date.CustomFormat = "";
			this._date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.locExtender.SetLocalizableToolTip(this._date, null);
			this.locExtender.SetLocalizationComment(this._date, null);
			this.locExtender.SetLocalizationPriority(this._date, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._date, "SessionsVw._date");
			this._date.Location = new System.Drawing.Point(136, 22);
			this._date.MinDate = new System.DateTime(1960, 1, 1, 0, 0, 0, 0);
			this._date.Name = "_date";
			this._date.Size = new System.Drawing.Size(129, 23);
			this._date.TabIndex = 3;
			// 
			// lblId
			// 
			this.lblId.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblId, null);
			this.locExtender.SetLocalizationComment(this.lblId, null);
			this.locExtender.SetLocalizingId(this.lblId, "SessionsVw.lblId");
			this.lblId.Location = new System.Drawing.Point(6, 4);
			this.lblId.Name = "lblId";
			this.lblId.Size = new System.Drawing.Size(18, 15);
			this.lblId.TabIndex = 0;
			this.lblId.Text = "ID";
			// 
			// _situation
			// 
			this._situation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxSpellChecker1.SetLanguageForSpellChecking(this._situation, "en");
			this.locExtender.SetLocalizableToolTip(this._situation, null);
			this.locExtender.SetLocalizationComment(this._situation, null);
			this.locExtender.SetLocalizationPriority(this._situation, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._situation, "SessionsVw._situation");
			this._situation.Location = new System.Drawing.Point(6, 298);
			this._situation.Multiline = true;
			this._situation.Name = "_situation";
			this._situation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._situation.Size = new System.Drawing.Size(259, 120);
			this._situation.TabIndex = 17;
			// 
			// _id
			// 
			this._id.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._id, null);
			this.locExtender.SetLocalizationComment(this._id, null);
			this.locExtender.SetLocalizationPriority(this._id, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._id, "SessionsVw._id");
			this._id.Location = new System.Drawing.Point(6, 22);
			this._id.Name = "_id";
			this._id.Size = new System.Drawing.Size(121, 23);
			this._id.TabIndex = 1;
			this._id.Validating += new System.ComponentModel.CancelEventHandler(this.HandleIdValidating);
			// 
			// lblSituation
			// 
			this.lblSituation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblSituation, null);
			this.locExtender.SetLocalizationComment(this.lblSituation, null);
			this.locExtender.SetLocalizingId(this.lblSituation, "SessionsVw.lblSituation");
			this.lblSituation.Location = new System.Drawing.Point(6, 280);
			this.lblSituation.Name = "lblSituation";
			this.lblSituation.Size = new System.Drawing.Size(54, 15);
			this.lblSituation.TabIndex = 16;
			this.lblSituation.Text = "Situation";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.label1, null);
			this.locExtender.SetLocalizationComment(this.label1, null);
			this.locExtender.SetLocalizingId(this.label1, "SessionsVw.label1");
			this.label1.Location = new System.Drawing.Point(136, 4);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(31, 15);
			this.label1.TabIndex = 2;
			this.label1.Text = "Date";
			// 
			// lblEventType
			// 
			this.lblEventType.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblEventType, null);
			this.locExtender.SetLocalizationComment(this.lblEventType, null);
			this.locExtender.SetLocalizingId(this.lblEventType, "SessionsVw.lblEventType");
			this.lblEventType.Location = new System.Drawing.Point(6, 142);
			this.lblEventType.Name = "lblEventType";
			this.lblEventType.Size = new System.Drawing.Size(65, 15);
			this.lblEventType.TabIndex = 8;
			this.lblEventType.Text = "Event Type";
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblTitle, null);
			this.locExtender.SetLocalizationComment(this.lblTitle, null);
			this.locExtender.SetLocalizingId(this.lblTitle, "SessionsVw.lblTitle");
			this.lblTitle.Location = new System.Drawing.Point(2, 50);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(30, 15);
			this.lblTitle.TabIndex = 4;
			this.lblTitle.Text = "Title";
			// 
			// _location
			// 
			this._location.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._location.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this._location.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this._location, null);
			this.locExtender.SetLocalizationComment(this._location, null);
			this.locExtender.SetLocalizationPriority(this._location, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._location, "SessionsVw._location");
			this._location.Location = new System.Drawing.Point(6, 252);
			this._location.Name = "_location";
			this._location.Size = new System.Drawing.Size(259, 23);
			this._location.TabIndex = 15;
			this._location.Enter += new System.EventHandler(this._location_Enter);
			// 
			// _title
			// 
			this._title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.locExtender.SetLocalizableToolTip(this._title, null);
			this.locExtender.SetLocalizationComment(this._title, null);
			this.locExtender.SetLocalizationPriority(this._title, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._title, "SessionsVw._title");
			this._title.Location = new System.Drawing.Point(6, 68);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(259, 23);
			this._title.TabIndex = 5;
			// 
			// lblLocation
			// 
			this.lblLocation.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblLocation, null);
			this.locExtender.SetLocalizationComment(this.lblLocation, null);
			this.locExtender.SetLocalizingId(this.lblLocation, "SessionsVw.lblLocation");
			this.lblLocation.Location = new System.Drawing.Point(6, 234);
			this.lblLocation.Name = "lblLocation";
			this.lblLocation.Size = new System.Drawing.Size(53, 15);
			this.lblLocation.TabIndex = 14;
			this.lblLocation.Text = "Location";
			// 
			// lblParticipants
			// 
			this.lblParticipants.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblParticipants, null);
			this.locExtender.SetLocalizationComment(this.lblParticipants, null);
			this.locExtender.SetLocalizingId(this.lblParticipants, "SessionsVw.lblParticipants");
			this.lblParticipants.Location = new System.Drawing.Point(6, 96);
			this.lblParticipants.Name = "lblParticipants";
			this.lblParticipants.Size = new System.Drawing.Size(69, 15);
			this.lblParticipants.TabIndex = 6;
			this.lblParticipants.Text = "Participants";
			// 
			// _setting
			// 
			this._setting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._setting.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this._setting.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.textBoxSpellChecker1.SetLanguageForSpellChecking(this._setting, "en");
			this.locExtender.SetLocalizableToolTip(this._setting, null);
			this.locExtender.SetLocalizationComment(this._setting, null);
			this.locExtender.SetLocalizationPriority(this._setting, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._setting, "SessionsVw._setting");
			this._setting.Location = new System.Drawing.Point(6, 206);
			this._setting.Name = "_setting";
			this._setting.Size = new System.Drawing.Size(259, 23);
			this._setting.TabIndex = 13;
			this._setting.Enter += new System.EventHandler(this._setting_Enter);
			// 
			// _participants
			// 
			this._participants.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._participants.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this._participants.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this._participants, null);
			this.locExtender.SetLocalizationComment(this._participants, null);
			this.locExtender.SetLocalizationPriority(this._participants, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._participants, "SessionsVw._participants");
			this._participants.Location = new System.Drawing.Point(6, 114);
			this._participants.Name = "_participants";
			this._participants.Size = new System.Drawing.Size(259, 23);
			this._participants.TabIndex = 7;
			this._participants.Enter += new System.EventHandler(this.HandleParticipants_Enter);
			// 
			// lblSetting
			// 
			this.lblSetting.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblSetting, null);
			this.locExtender.SetLocalizationComment(this.lblSetting, null);
			this.locExtender.SetLocalizingId(this.lblSetting, "label1.label1");
			this.lblSetting.Location = new System.Drawing.Point(6, 188);
			this.lblSetting.Name = "lblSetting";
			this.lblSetting.Size = new System.Drawing.Size(44, 15);
			this.lblSetting.TabIndex = 12;
			this.lblSetting.Text = "Setting";
			// 
			// lblAccess
			// 
			this.lblAccess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblAccess.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblAccess, null);
			this.locExtender.SetLocalizationComment(this.lblAccess, null);
			this.locExtender.SetLocalizingId(this.lblAccess, "SessionsVw.lblAccess");
			this.lblAccess.Location = new System.Drawing.Point(122, 142);
			this.lblAccess.Name = "lblAccess";
			this.lblAccess.Size = new System.Drawing.Size(43, 15);
			this.lblAccess.TabIndex = 10;
			this.lblAccess.Text = "Access";
			// 
			// _access
			// 
			this._access.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._access.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._access.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.locExtender.SetLocalizableToolTip(this._access, null);
			this.locExtender.SetLocalizationComment(this._access, null);
			this.locExtender.SetLocalizationPriority(this._access, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._access, "SessionsVw._access");
			this._access.Location = new System.Drawing.Point(122, 160);
			this._access.Name = "_access";
			this._access.Size = new System.Drawing.Size(143, 23);
			this._access.TabIndex = 11;
			this._access.Enter += new System.EventHandler(this._access_Enter);
			// 
			// pnlDescriptionRightSide
			// 
			this.pnlDescriptionRightSide.Controls.Add(this.lblMedia);
			this.pnlDescriptionRightSide.Controls.Add(this.m_mediaPanel);
			this.pnlDescriptionRightSide.Controls.Add(this._synopsis);
			this.pnlDescriptionRightSide.Controls.Add(this.lblSynopsis);
			this.pnlDescriptionRightSide.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlDescriptionRightSide.Location = new System.Drawing.Point(277, 3);
			this.pnlDescriptionRightSide.Name = "pnlDescriptionRightSide";
			this.pnlDescriptionRightSide.Size = new System.Drawing.Size(331, 424);
			this.pnlDescriptionRightSide.TabIndex = 1;
			// 
			// lblMedia
			// 
			this.lblMedia.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblMedia, null);
			this.locExtender.SetLocalizationComment(this.lblMedia, null);
			this.locExtender.SetLocalizingId(this.lblMedia, "SessionsVw.lblMedia");
			this.lblMedia.Location = new System.Drawing.Point(3, 126);
			this.lblMedia.Name = "lblMedia";
			this.lblMedia.Size = new System.Drawing.Size(40, 15);
			this.lblMedia.TabIndex = 3;
			this.lblMedia.Text = "Media";
			// 
			// m_mediaPanel
			// 
			this.m_mediaPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_mediaPanel.AutoScroll = true;
			this.m_mediaPanel.BackColor = System.Drawing.Color.DimGray;
			this.locExtender.SetLocalizableToolTip(this.m_mediaPanel, null);
			this.locExtender.SetLocalizationComment(this.m_mediaPanel, null);
			this.locExtender.SetLocalizingId(this.m_mediaPanel, "multimediaScroll1.multimediaScroll1");
			this.m_mediaPanel.Location = new System.Drawing.Point(3, 144);
			this.m_mediaPanel.Name = "m_mediaPanel";
			this.m_mediaPanel.Size = new System.Drawing.Size(322, 274);
			this.m_mediaPanel.TabIndex = 2;
			// 
			// _synopsis
			// 
			this._synopsis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxSpellChecker1.SetLanguageForSpellChecking(this._synopsis, "en");
			this.locExtender.SetLocalizableToolTip(this._synopsis, null);
			this.locExtender.SetLocalizationComment(this._synopsis, null);
			this.locExtender.SetLocalizationPriority(this._synopsis, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._synopsis, "SessionsVw._synopsis");
			this._synopsis.Location = new System.Drawing.Point(3, 22);
			this._synopsis.Multiline = true;
			this._synopsis.Name = "_synopsis";
			this._synopsis.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._synopsis.Size = new System.Drawing.Size(322, 99);
			this._synopsis.TabIndex = 1;
			// 
			// lblSynopsis
			// 
			this.lblSynopsis.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblSynopsis, null);
			this.locExtender.SetLocalizationComment(this.lblSynopsis, null);
			this.locExtender.SetLocalizingId(this.lblSynopsis, "SessionsVw.lblSynopsis");
			this.lblSynopsis.Location = new System.Drawing.Point(3, 4);
			this.lblSynopsis.Name = "lblSynopsis";
			this.lblSynopsis.Size = new System.Drawing.Size(53, 15);
			this.lblSynopsis.TabIndex = 0;
			this.lblSynopsis.Text = "Synopsis";
			// 
			// tpgFiles
			// 
			this.tpgFiles.Controls.Add(this.splitFileTab);
			this.locExtender.SetLocalizableToolTip(this.tpgFiles, null);
			this.locExtender.SetLocalizationComment(this.tpgFiles, null);
			this.locExtender.SetLocalizingId(this.tpgFiles, "SessionsVw.tpgFiles");
			this.tpgFiles.Location = new System.Drawing.Point(4, 26);
			this.tpgFiles.Name = "tpgFiles";
			this.tpgFiles.Padding = new System.Windows.Forms.Padding(0, 2, 2, 1);
			this.tpgFiles.Size = new System.Drawing.Size(613, 433);
			this.tpgFiles.TabIndex = 3;
			this.tpgFiles.Text = "Files";
			this.tpgFiles.ToolTipText = "Files";
			this.tpgFiles.UseVisualStyleBackColor = true;
			// 
			// splitFileTab
			// 
			this.splitFileTab.BackColor = System.Drawing.SystemColors.Control;
			this.splitFileTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitFileTab.Location = new System.Drawing.Point(0, 2);
			this.splitFileTab.Name = "splitFileTab";
			this.splitFileTab.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitFileTab.Panel1
			// 
			this.splitFileTab.Panel1.BackColor = System.Drawing.Color.White;
			this.splitFileTab.Panel1.Controls.Add(this.lnkSessionPath);
			this.splitFileTab.Panel1.Controls.Add(this.lblEmptySessionMsg);
			this.splitFileTab.Panel1.Controls.Add(this.gridFiles);
			this.splitFileTab.Panel1.SizeChanged += new System.EventHandler(this.HandleFileListPanelSizeChanged);
			// 
			// splitFileTab.Panel2
			// 
			this.splitFileTab.Panel2.Controls.Add(this._infoPanel);
			this.splitFileTab.Panel2.Controls.Add(this.pnlFileInfoNotes);
			this.splitFileTab.Panel2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 2);
			this.splitFileTab.SplitterDistance = 320;
			this.splitFileTab.TabIndex = 2;
			// 
			// lnkSessionPath
			// 
			this.lnkSessionPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lnkSessionPath.AutoEllipsis = true;
			this.locExtender.SetLocalizableToolTip(this.lnkSessionPath, null);
			this.locExtender.SetLocalizationComment(this.lnkSessionPath, null);
			this.locExtender.SetLocalizingId(this.lnkSessionPath, "SessionsVw.lnkSessionPath");
			this.lnkSessionPath.Location = new System.Drawing.Point(21, 58);
			this.lnkSessionPath.Name = "lnkSessionPath";
			this.lnkSessionPath.Size = new System.Drawing.Size(577, 21);
			this.lnkSessionPath.TabIndex = 2;
			this.lnkSessionPath.TabStop = true;
			this.lnkSessionPath.Text = "#";
			this.lnkSessionPath.Visible = false;
			this.lnkSessionPath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSessionPath_LinkClicked);
			// 
			// lblEmptySessionMsg
			// 
			this.lblEmptySessionMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblEmptySessionMsg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this.lblEmptySessionMsg, null);
			this.locExtender.SetLocalizationComment(this.lblEmptySessionMsg, null);
			this.locExtender.SetLocalizingId(this.lblEmptySessionMsg, "SessionsVw.lblEmptySessionMsg");
			this.lblEmptySessionMsg.Location = new System.Drawing.Point(19, 15);
			this.lblEmptySessionMsg.Name = "lblEmptySessionMsg";
			this.lblEmptySessionMsg.Size = new System.Drawing.Size(579, 40);
			this.lblEmptySessionMsg.TabIndex = 1;
			this.lblEmptySessionMsg.Text = "This session does not yet have any files. To add files, you may drag them here or" +
				" directly into the session folder at:";
			this.lblEmptySessionMsg.Visible = false;
			// 
			// gridFiles
			// 
			this.gridFiles.AllowUserToAddRows = false;
			this.gridFiles.AllowUserToDeleteRows = false;
			this.gridFiles.AllowUserToOrderColumns = true;
			this.gridFiles.AllowUserToResizeRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			this.gridFiles.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.gridFiles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			this.gridFiles.BackgroundColor = System.Drawing.SystemColors.Window;
			this.gridFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.gridFiles.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridFiles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.gridFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iconCol,
            this.filesNameCol,
            this.filesTypeCol,
            this.filesTagsCol,
            this.filesDateCol,
            this.filesSizeCol});
			this.gridFiles.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(219)))), ((int)(((byte)(180)))));
			this.gridFiles.IsDirty = false;
			this.locExtender.SetLocalizableToolTip(this.gridFiles, null);
			this.locExtender.SetLocalizationComment(this.gridFiles, null);
			this.locExtender.SetLocalizingId(this.gridFiles, "SessionsVw.gridFiles");
			this.gridFiles.Location = new System.Drawing.Point(3, 109);
			this.gridFiles.MultiSelect = false;
			this.gridFiles.Name = "gridFiles";
			this.gridFiles.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.gridFiles.RowHeadersVisible = false;
			this.gridFiles.RowHeadersWidth = 22;
			this.gridFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridFiles.ShowWaterMarkWhenDirty = false;
			this.gridFiles.Size = new System.Drawing.Size(580, 180);
			this.gridFiles.TabIndex = 0;
			this.gridFiles.VirtualMode = true;
			this.gridFiles.Visible = false;
			this.gridFiles.WaterMark = "!";
			this.gridFiles.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.HandleFilesGridCellMouseClick);
			this.gridFiles.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridFiles_RowEnter);
			this.gridFiles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.HandleFileGridCellDoubleClick);
			this.gridFiles.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gridFiles_CellValueNeeded);
			this.gridFiles.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gridFiles_CellValuePushed);
			// 
			// iconCol
			// 
			this.iconCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.iconCol.DataPropertyName = "SmallIcon";
			this.iconCol.HeaderText = "";
			this.iconCol.Name = "iconCol";
			this.iconCol.ReadOnly = true;
			this.iconCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.iconCol.Width = 5;
			// 
			// filesNameCol
			// 
			this.filesNameCol.DataPropertyName = "FileName";
			this.filesNameCol.HeaderText = "Name";
			this.filesNameCol.Name = "filesNameCol";
			this.filesNameCol.ReadOnly = true;
			// 
			// filesTypeCol
			// 
			this.filesTypeCol.DataPropertyName = "FileType";
			this.filesTypeCol.HeaderText = "Type";
			this.filesTypeCol.Name = "filesTypeCol";
			this.filesTypeCol.ReadOnly = true;
			// 
			// filesTagsCol
			// 
			this.filesTagsCol.DataPropertyName = "Tags";
			this.filesTagsCol.HeaderText = "Tags";
			this.filesTagsCol.Name = "filesTagsCol";
			// 
			// filesDateCol
			// 
			this.filesDateCol.DataPropertyName = "DateModified";
			this.filesDateCol.HeaderText = "Date Modified";
			this.filesDateCol.Name = "filesDateCol";
			this.filesDateCol.ReadOnly = true;
			this.filesDateCol.Width = 107;
			// 
			// filesSizeCol
			// 
			this.filesSizeCol.DataPropertyName = "FileSize";
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.filesSizeCol.DefaultCellStyle = dataGridViewCellStyle3;
			this.filesSizeCol.HeaderText = "Size";
			this.filesSizeCol.Name = "filesSizeCol";
			this.filesSizeCol.ReadOnly = true;
			this.filesSizeCol.Width = 52;
			// 
			// _infoPanel
			// 
			this._infoPanel.BackColor = System.Drawing.Color.Transparent;
			this._infoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._infoPanel.FileName = "#";
			this._infoPanel.Icon = null;
			this._infoPanel.LabeledTextBoxBackgroundColor = System.Drawing.Color.Transparent;
			this.locExtender.SetLocalizableToolTip(this._infoPanel, null);
			this.locExtender.SetLocalizationComment(this._infoPanel, "Localized in base class");
			this.locExtender.SetLocalizationPriority(this._infoPanel, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._infoPanel, "SessionsVw.InfoPanel");
			this._infoPanel.Location = new System.Drawing.Point(2, 3);
			this._infoPanel.Name = "_infoPanel";
			this._infoPanel.PresetProvider = null;
			this._infoPanel.Size = new System.Drawing.Size(437, 102);
			this._infoPanel.TabIndex = 0;
			this._infoPanel.MoreActionButtonClicked += new System.EventHandler(this._infoPanel_MoreActionButtonClicked);
			// 
			// pnlFileInfoNotes
			// 
			this.pnlFileInfoNotes.BackColor = System.Drawing.Color.Transparent;
			this.pnlFileInfoNotes.Controls.Add(this._fileInfoNotes);
			this.pnlFileInfoNotes.Controls.Add(this.lblFileInfoNotes);
			this.pnlFileInfoNotes.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlFileInfoNotes.Name = "pnlFileInfoNotes";
			this.pnlFileInfoNotes.Size = new System.Drawing.Size(170, 101);
			this.pnlFileInfoNotes.TabIndex = 1;
			// 
			// _fileInfoNotes
			// 
			this._fileInfoNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._fileInfoNotes.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._fileInfoNotes.BackColor = System.Drawing.SystemColors.Window;
			this._fileInfoNotes.DynamicBorder = false;
			this._fileInfoNotes.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(173)))), ((int)(((byte)(179)))));
			// 
			// 
			// 
			this._fileInfoNotes.InnerTextBox.BackColor = System.Drawing.SystemColors.Control;
			this._fileInfoNotes.InnerTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._fileInfoNotes.InnerTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._fileInfoNotes.InnerTextBox.Location = new System.Drawing.Point(1, 1);
			this._fileInfoNotes.InnerTextBox.Multiline = true;
			this._fileInfoNotes.InnerTextBox.Name = "_txtBox";
			this._fileInfoNotes.InnerTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._fileInfoNotes.InnerTextBox.Size = new System.Drawing.Size(148, 2820);
			this._fileInfoNotes.InnerTextBox.TabIndex = 0;
			this.locExtender.SetLocalizableToolTip(this._fileInfoNotes, null);
			this.locExtender.SetLocalizationComment(this._fileInfoNotes, null);
			this.locExtender.SetLocalizationPriority(this._fileInfoNotes, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._fileInfoNotes, "SessionsVw.HoverCueTextBox");
			this._fileInfoNotes.Location = new System.Drawing.Point(3, 19);
			this._fileInfoNotes.Name = "_fileInfoNotes";
			this._fileInfoNotes.Padding = new System.Windows.Forms.Padding(1);
			this._fileInfoNotes.Size = new System.Drawing.Size(150, 2822);
			this._fileInfoNotes.TabIndex = 1;
			// 
			// lblFileInfoNotes
			// 
			this.lblFileInfoNotes.AutoSize = true;
			this.locExtender.SetLocalizableToolTip(this.lblFileInfoNotes, null);
			this.locExtender.SetLocalizationComment(this.lblFileInfoNotes, null);
			this.locExtender.SetLocalizingId(this.lblFileInfoNotes, "SessionsVw.lblFileInfoNotes");
			this.lblFileInfoNotes.Location = new System.Drawing.Point(3, 1);
			this.lblFileInfoNotes.Name = "lblFileInfoNotes";
			this.lblFileInfoNotes.Size = new System.Drawing.Size(41, 15);
			this.lblFileInfoNotes.TabIndex = 0;
			this.lblFileInfoNotes.Text = "Notes:";
			// 
			// btnNewFromFiles
			// 
			this.btnNewFromFiles.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.locExtender.SetLocalizableToolTip(this.btnNewFromFiles, "Create New Sessions from Files");
			this.locExtender.SetLocalizationComment(this.btnNewFromFiles, null);
			this.locExtender.SetLocalizingId(this.btnNewFromFiles, "SessionsVw.btnNewFromFiles");
			this.btnNewFromFiles.Location = new System.Drawing.Point(43, 350);
			this.btnNewFromFiles.Name = "btnNewFromFiles";
			this.btnNewFromFiles.Size = new System.Drawing.Size(117, 24);
			this.btnNewFromFiles.TabIndex = 1;
			this.btnNewFromFiles.Text = "New From Files...";
			this.btnNewFromFiles.UseVisualStyleBackColor = true;
			this.btnNewFromFiles.Click += new System.EventHandler(this.btnNewFromFiles_Click);
			// 
			// lpSessions
			// 
			this.lpSessions.CurrentItem = null;
			this.lpSessions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lpSessions.Items = new object[0];
			// 
			// 
			// 
			this.lpSessions.ListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lpSessions.ListView.BackColor = System.Drawing.SystemColors.Window;
			this.lpSessions.ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lpSessions.ListView.FullRowSelect = true;
			this.lpSessions.ListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lpSessions.ListView.HideSelection = false;
			this.lpSessions.ListView.Location = new System.Drawing.Point(2, 31);
			this.lpSessions.ListView.Name = "lvItems";
			this.lpSessions.ListView.Size = new System.Drawing.Size(862, 15526);
			this.lpSessions.ListView.TabIndex = 0;
			this.lpSessions.ListView.UseCompatibleStateImageBehavior = false;
			this.lpSessions.ListView.View = System.Windows.Forms.View.Details;
			this.locExtender.SetLocalizableToolTip(this.lpSessions, null);
			this.locExtender.SetLocalizationComment(this.lpSessions, null);
			this.locExtender.SetLocalizingId(this.lpSessions, "SessionsVw.lpSessions");
			this.lpSessions.Location = new System.Drawing.Point(0, 0);
			this.lpSessions.MinimumSize = new System.Drawing.Size(165, 0);
			this.lpSessions.Name = "lpSessions";
			this.lpSessions.ReSortWhenItemTextChanges = false;
			this.lpSessions.Size = new System.Drawing.Size(173, 466);
			this.lpSessions.TabIndex = 0;
			this.lpSessions.Text = "Sessions";
			this.lpSessions.BeforeItemsDeleted += new SIL.Sponge.Controls.ListPanel.BeforeItemsDeletedHandler(this.BeforeSessionsDeleted);
			this.lpSessions.SelectedItemChanged += new SIL.Sponge.Controls.ListPanel.SelectedItemChangedHandler(this.lpSessions_SelectedItemChanged);
			this.lpSessions.AfterItemsDeleted += new SIL.Sponge.Controls.ListPanel.AfterItemsDeletedHandler(this.AfterSessionsDeleted);
			this.lpSessions.NewButtonClicked += new SIL.Sponge.Controls.ListPanel.NewButtonClickedHandler(this.lpSessions_NewButtonClicked);
			// 
			// lblNoSessionsMsg
			// 
			this.lblNoSessionsMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblNoSessionsMsg.BackColor = System.Drawing.Color.Transparent;
			this.lblNoSessionsMsg.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.locExtender.SetLocalizableToolTip(this.lblNoSessionsMsg, null);
			this.locExtender.SetLocalizationComment(this.lblNoSessionsMsg, null);
			this.locExtender.SetLocalizingId(this.lblNoSessionsMsg, "SessionsVw.lblNoSessionsMsg");
			this.lblNoSessionsMsg.Location = new System.Drawing.Point(14, 45);
			this.lblNoSessionsMsg.Name = "lblNoSessionsMsg";
			this.lblNoSessionsMsg.Size = new System.Drawing.Size(146, 251);
			this.lblNoSessionsMsg.TabIndex = 3;
			this.lblNoSessionsMsg.Text = "To add a session, click the \'New\' button below.";
			// 
			// locExtender
			// 
			this.locExtender.LocalizationGroup = "Views";
			// 
			// _fileContextMenu
			// 
			this.locExtender.SetLocalizableToolTip(this._fileContextMenu, null);
			this.locExtender.SetLocalizationComment(this._fileContextMenu, null);
			this.locExtender.SetLocalizationPriority(this._fileContextMenu, SIL.Localize.LocalizationUtils.LocalizationPriority.NotLocalizable);
			this.locExtender.SetLocalizingId(this._fileContextMenu, "cmnuMoreActions");
			this._fileContextMenu.Name = "cmnuMoreActions";
			this._fileContextMenu.Size = new System.Drawing.Size(61, 4);
			// 
			// SessionsVw
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.locExtender.SetLocalizableToolTip(this, null);
			this.locExtender.SetLocalizationComment(this, null);
			this.locExtender.SetLocalizingId(this, "SessionsVw.BaseSplitVw");
			this.Name = "SessionsVw";
			this.ShowRightBottomPanel = false;
			this.Size = new System.Drawing.Size(800, 466);
			this.Controls.SetChildIndex(this.splitOuter, 0);
			this.splitOuter.Panel1.ResumeLayout(false);
			this.splitOuter.Panel2.ResumeLayout(false);
			this.splitOuter.ResumeLayout(false);
			this.splitRightSide.Panel1.ResumeLayout(false);
			this.splitRightSide.ResumeLayout(false);
			this.tabSessions.ResumeLayout(false);
			this.tpgDescription.ResumeLayout(false);
			this.tblDescription.ResumeLayout(false);
			this.pnlDescriptionLeftSide.ResumeLayout(false);
			this.pnlDescriptionLeftSide.PerformLayout();
			this.pnlDescriptionRightSide.ResumeLayout(false);
			this.pnlDescriptionRightSide.PerformLayout();
			this.tpgFiles.ResumeLayout(false);
			this.splitFileTab.Panel1.ResumeLayout(false);
			this.splitFileTab.Panel2.ResumeLayout(false);
			this.splitFileTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridFiles)).EndInit();
			this.pnlFileInfoNotes.ResumeLayout(false);
			this.pnlFileInfoNotes.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.locExtender)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabSessions;
        private System.Windows.Forms.TabPage tpgDescription;
		private System.Windows.Forms.TabPage tpgFiles;
		private SilUtils.SilGrid gridFiles;
		private ListPanel lpSessions;
		private System.Windows.Forms.LinkLabel lnkSessionPath;
		private System.Windows.Forms.Label lblEmptySessionMsg;
		private System.Windows.Forms.Label lblNoSessionsMsg;
		private Localize.LocalizationUtils.LocalizationExtender locExtender;
		private InfoPanel _infoPanel;
		private System.Windows.Forms.ContextMenuStrip _fileContextMenu;
		private System.Windows.Forms.DataGridViewImageColumn iconCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesNameCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesTypeCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesTagsCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesDateCol;
		private System.Windows.Forms.DataGridViewTextBoxColumn filesSizeCol;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox _id;
		private System.Windows.Forms.Label lblId;
		private System.Windows.Forms.TextBox _participants;
		private System.Windows.Forms.Label lblParticipants;
		private System.Windows.Forms.TextBox _title;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.TextBox _access;
		private System.Windows.Forms.Label lblAccess;
		private System.Windows.Forms.TextBox _setting;
		private System.Windows.Forms.Label lblSetting;
		private System.Windows.Forms.Label lblEventType;
		private System.Windows.Forms.TextBox _location;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.TextBox _situation;
		private System.Windows.Forms.Label lblSituation;
		private System.Windows.Forms.TableLayoutPanel tblDescription;
		private System.Windows.Forms.Panel pnlDescriptionLeftSide;
		private System.Windows.Forms.Panel pnlDescriptionRightSide;
		private System.Windows.Forms.TextBox _synopsis;
		private System.Windows.Forms.Label lblSynopsis;
		private System.Windows.Forms.SplitContainer splitFileTab;
		private System.Windows.Forms.Panel pnlFileInfoNotes;
		private HoverCueTextBox _fileInfoNotes;
		private System.Windows.Forms.Label lblFileInfoNotes;
		private System.Windows.Forms.DateTimePicker _date;
		private MultimediaScroll m_mediaPanel;
		private System.Windows.Forms.Label lblMedia;
		private System.Windows.Forms.ComboBox _eventType;
		private System.Windows.Forms.Button btnNewFromFiles;
		private Palaso.UI.WindowsForms.Spelling.TextBoxSpellChecker textBoxSpellChecker1;
	}
}
