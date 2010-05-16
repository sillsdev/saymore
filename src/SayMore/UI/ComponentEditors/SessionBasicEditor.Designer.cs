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
			this._lblid = new System.Windows.Forms.Label();
			this._id = new System.Windows.Forms.TextBox();
			this._lbltitle = new System.Windows.Forms.Label();
			this._title = new System.Windows.Forms.TextBox();
			this._situation = new System.Windows.Forms.TextBox();
			this._lblsynopsis = new System.Windows.Forms.Label();
			this._synopsis = new System.Windows.Forms.TextBox();
			this._tblLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._access = new System.Windows.Forms.TextBox();
			this._lblaccess = new System.Windows.Forms.Label();
			this._eventType = new System.Windows.Forms.ComboBox();
			this._lbleventType = new System.Windows.Forms.Label();
			this._location = new System.Windows.Forms.TextBox();
			this._lbllocation = new System.Windows.Forms.Label();
			this._participants = new System.Windows.Forms.TextBox();
			this._lblparticipants = new System.Windows.Forms.Label();
			this._date = new System.Windows.Forms.DateTimePicker();
			this._lbldate = new System.Windows.Forms.Label();
			this._lblsetting = new System.Windows.Forms.Label();
			this._setting = new System.Windows.Forms.TextBox();
			this._lblsituation = new System.Windows.Forms.Label();
			this._binder = new SayMore.UI.ComponentEditors.BindingHelper(this.components);
			this._tblLayoutOuter.SuspendLayout();
			this.SuspendLayout();
			// 
			// _lblid
			// 
			this._lblid.AutoSize = true;
			this._lblid.BackColor = System.Drawing.Color.Transparent;
			this._lblid.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblid.Location = new System.Drawing.Point(0, 0);
			this._lblid.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this._lblid.Name = "_lblid";
			this._lblid.Size = new System.Drawing.Size(17, 13);
			this._lblid.TabIndex = 0;
			this._lblid.Text = "Id";
			// 
			// _id
			// 
			this._id.Dock = System.Windows.Forms.DockStyle.Top;
			this._id.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._id, true);
			this._id.Location = new System.Drawing.Point(0, 16);
			this._id.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._id.Name = "_id";
			this._id.Size = new System.Drawing.Size(192, 22);
			this._id.TabIndex = 1;
			// 
			// _lbltitle
			// 
			this._lbltitle.AutoSize = true;
			this._lbltitle.BackColor = System.Drawing.Color.Transparent;
			this._lbltitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lbltitle.Location = new System.Drawing.Point(0, 46);
			this._lbltitle.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._lbltitle.Name = "_lbltitle";
			this._lbltitle.Size = new System.Drawing.Size(31, 13);
			this._lbltitle.TabIndex = 2;
			this._lbltitle.Text = "Title:";
			// 
			// _title
			// 
			this._title.Dock = System.Windows.Forms.DockStyle.Top;
			this._title.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._title, true);
			this._title.Location = new System.Drawing.Point(0, 62);
			this._title.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(192, 22);
			this._title.TabIndex = 3;
			// 
			// _situation
			// 
			this._situation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._situation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._situation, true);
			this._situation.Location = new System.Drawing.Point(0, 200);
			this._situation.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._situation.Multiline = true;
			this._situation.Name = "_situation";
			this._situation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._situation.Size = new System.Drawing.Size(192, 96);
			this._situation.TabIndex = 9;
			// 
			// _lblsynopsis
			// 
			this._lblsynopsis.AutoSize = true;
			this._lblsynopsis.BackColor = System.Drawing.Color.Transparent;
			this._lblsynopsis.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblsynopsis.Location = new System.Drawing.Point(202, 184);
			this._lblsynopsis.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._lblsynopsis.Name = "_lblsynopsis";
			this._lblsynopsis.Size = new System.Drawing.Size(52, 13);
			this._lblsynopsis.TabIndex = 18;
			this._lblsynopsis.Text = "Synopsis";
			// 
			// _synopsis
			// 
			this._synopsis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._synopsis.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._synopsis, true);
			this._synopsis.Location = new System.Drawing.Point(202, 200);
			this._synopsis.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._synopsis.Multiline = true;
			this._synopsis.Name = "_synopsis";
			this._synopsis.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._synopsis.Size = new System.Drawing.Size(192, 96);
			this._synopsis.TabIndex = 19;
			// 
			// _tblLayoutOuter
			// 
			this._tblLayoutOuter.AutoSize = true;
			this._tblLayoutOuter.BackColor = System.Drawing.Color.Transparent;
			this._tblLayoutOuter.ColumnCount = 2;
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tblLayoutOuter.Controls.Add(this._access, 1, 7);
			this._tblLayoutOuter.Controls.Add(this._synopsis, 1, 9);
			this._tblLayoutOuter.Controls.Add(this._lblsynopsis, 1, 8);
			this._tblLayoutOuter.Controls.Add(this._situation, 0, 9);
			this._tblLayoutOuter.Controls.Add(this._lblaccess, 1, 6);
			this._tblLayoutOuter.Controls.Add(this._eventType, 0, 7);
			this._tblLayoutOuter.Controls.Add(this._lbleventType, 0, 6);
			this._tblLayoutOuter.Controls.Add(this._location, 1, 5);
			this._tblLayoutOuter.Controls.Add(this._lbllocation, 1, 4);
			this._tblLayoutOuter.Controls.Add(this._participants, 0, 5);
			this._tblLayoutOuter.Controls.Add(this._lblparticipants, 0, 4);
			this._tblLayoutOuter.Controls.Add(this._date, 1, 1);
			this._tblLayoutOuter.Controls.Add(this._lbldate, 1, 0);
			this._tblLayoutOuter.Controls.Add(this._lblid, 0, 0);
			this._tblLayoutOuter.Controls.Add(this._id, 0, 1);
			this._tblLayoutOuter.Controls.Add(this._lbltitle, 0, 2);
			this._tblLayoutOuter.Controls.Add(this._title, 0, 3);
			this._tblLayoutOuter.Controls.Add(this._lblsetting, 1, 2);
			this._tblLayoutOuter.Controls.Add(this._setting, 1, 3);
			this._tblLayoutOuter.Controls.Add(this._lblsituation, 0, 8);
			this._tblLayoutOuter.Dock = System.Windows.Forms.DockStyle.Top;
			this._tblLayoutOuter.Location = new System.Drawing.Point(7, 7);
			this._tblLayoutOuter.Name = "_tblLayoutOuter";
			this._tblLayoutOuter.RowCount = 10;
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
			this._tblLayoutOuter.Size = new System.Drawing.Size(394, 299);
			this._tblLayoutOuter.TabIndex = 0;
			// 
			// _access
			// 
			this._access.Dock = System.Windows.Forms.DockStyle.Top;
			this._access.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._access, true);
			this._access.Location = new System.Drawing.Point(202, 154);
			this._access.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._access.Name = "_access";
			this._access.Size = new System.Drawing.Size(192, 22);
			this._access.TabIndex = 17;
			// 
			// _lblaccess
			// 
			this._lblaccess.AutoSize = true;
			this._lblaccess.BackColor = System.Drawing.Color.Transparent;
			this._lblaccess.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblaccess.Location = new System.Drawing.Point(202, 138);
			this._lblaccess.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._lblaccess.Name = "_lblaccess";
			this._lblaccess.Size = new System.Drawing.Size(40, 13);
			this._lblaccess.TabIndex = 16;
			this._lblaccess.Text = "Access";
			// 
			// _eventType
			// 
			this._eventType.Dock = System.Windows.Forms.DockStyle.Top;
			this._eventType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._eventType.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._eventType.FormattingEnabled = true;
			this._binder.SetIsBound(this._eventType, true);
			this._eventType.Location = new System.Drawing.Point(0, 154);
			this._eventType.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._eventType.Name = "_eventType";
			this._eventType.Size = new System.Drawing.Size(192, 21);
			this._eventType.TabIndex = 7;
			// 
			// _lbleventType
			// 
			this._lbleventType.AutoSize = true;
			this._lbleventType.BackColor = System.Drawing.Color.Transparent;
			this._lbleventType.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lbleventType.Location = new System.Drawing.Point(0, 138);
			this._lbleventType.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._lbleventType.Name = "_lbleventType";
			this._lbleventType.Size = new System.Drawing.Size(61, 13);
			this._lbleventType.TabIndex = 6;
			this._lbleventType.Text = "Event Type";
			// 
			// _location
			// 
			this._location.Dock = System.Windows.Forms.DockStyle.Top;
			this._location.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._location, true);
			this._location.Location = new System.Drawing.Point(202, 108);
			this._location.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._location.Name = "_location";
			this._location.Size = new System.Drawing.Size(192, 22);
			this._location.TabIndex = 15;
			// 
			// _lbllocation
			// 
			this._lbllocation.AutoSize = true;
			this._lbllocation.BackColor = System.Drawing.Color.Transparent;
			this._lbllocation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lbllocation.Location = new System.Drawing.Point(202, 92);
			this._lbllocation.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._lbllocation.Name = "_lbllocation";
			this._lbllocation.Size = new System.Drawing.Size(51, 13);
			this._lbllocation.TabIndex = 14;
			this._lbllocation.Text = "Location";
			// 
			// _participants
			// 
			this._participants.Dock = System.Windows.Forms.DockStyle.Top;
			this._participants.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._participants, true);
			this._participants.Location = new System.Drawing.Point(0, 108);
			this._participants.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
			this._participants.Name = "_participants";
			this._participants.Size = new System.Drawing.Size(192, 22);
			this._participants.TabIndex = 5;
			// 
			// _lblparticipants
			// 
			this._lblparticipants.AutoSize = true;
			this._lblparticipants.BackColor = System.Drawing.Color.Transparent;
			this._lblparticipants.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblparticipants.Location = new System.Drawing.Point(0, 92);
			this._lblparticipants.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._lblparticipants.Name = "_lblparticipants";
			this._lblparticipants.Size = new System.Drawing.Size(67, 13);
			this._lblparticipants.TabIndex = 4;
			this._lblparticipants.Text = "Participants";
			// 
			// _date
			// 
			this._date.CustomFormat = "";
			this._date.Dock = System.Windows.Forms.DockStyle.Top;
			this._date.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this._binder.SetIsBound(this._date, true);
			this._date.Location = new System.Drawing.Point(202, 16);
			this._date.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._date.MinDate = new System.DateTime(1960, 1, 1, 0, 0, 0, 0);
			this._date.Name = "_date";
			this._date.Size = new System.Drawing.Size(192, 22);
			this._date.TabIndex = 11;
			// 
			// _lbldate
			// 
			this._lbldate.AutoSize = true;
			this._lbldate.BackColor = System.Drawing.Color.Transparent;
			this._lbldate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lbldate.Location = new System.Drawing.Point(202, 0);
			this._lbldate.Margin = new System.Windows.Forms.Padding(5, 0, 3, 0);
			this._lbldate.Name = "_lbldate";
			this._lbldate.Size = new System.Drawing.Size(31, 13);
			this._lbldate.TabIndex = 10;
			this._lbldate.Text = "Date";
			// 
			// _lblsetting
			// 
			this._lblsetting.AutoSize = true;
			this._lblsetting.BackColor = System.Drawing.Color.Transparent;
			this._lblsetting.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblsetting.Location = new System.Drawing.Point(202, 46);
			this._lblsetting.Margin = new System.Windows.Forms.Padding(5, 5, 3, 0);
			this._lblsetting.Name = "_lblsetting";
			this._lblsetting.Size = new System.Drawing.Size(44, 13);
			this._lblsetting.TabIndex = 12;
			this._lblsetting.Text = "Setting";
			// 
			// _setting
			// 
			this._setting.Dock = System.Windows.Forms.DockStyle.Top;
			this._setting.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._binder.SetIsBound(this._setting, true);
			this._setting.Location = new System.Drawing.Point(202, 62);
			this._setting.Margin = new System.Windows.Forms.Padding(5, 3, 0, 3);
			this._setting.Name = "_setting";
			this._setting.Size = new System.Drawing.Size(192, 22);
			this._setting.TabIndex = 13;
			// 
			// _lblsituation
			// 
			this._lblsituation.AutoSize = true;
			this._lblsituation.BackColor = System.Drawing.Color.Transparent;
			this._lblsituation.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lblsituation.Location = new System.Drawing.Point(0, 184);
			this._lblsituation.Margin = new System.Windows.Forms.Padding(0, 5, 5, 0);
			this._lblsituation.Name = "_lblsituation";
			this._lblsituation.Size = new System.Drawing.Size(54, 13);
			this._lblsituation.TabIndex = 8;
			this._lblsituation.Text = "Situation";
			// 
			// _binder
			// 
			this._binder.GetBoundControlValue += new SayMore.UI.ComponentEditors.BindingHelper.GetBoundControlValueHandler(this.HandleGetBoundControlValue);
			// 
			// SessionBasicEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this._tblLayoutOuter);
			this.Name = "SessionBasicEditor";
			this.Size = new System.Drawing.Size(408, 331);
			this._tblLayoutOuter.ResumeLayout(false);
			this._tblLayoutOuter.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label _lblid;
		private System.Windows.Forms.TextBox _id;
		private System.Windows.Forms.Label _lbltitle;
		private System.Windows.Forms.TextBox _title;
		private System.Windows.Forms.TextBox _situation;
		private System.Windows.Forms.Label _lblsynopsis;
		private System.Windows.Forms.TextBox _synopsis;
		private System.Windows.Forms.TableLayoutPanel _tblLayoutOuter;
		private System.Windows.Forms.Label _lbldate;
		private System.Windows.Forms.DateTimePicker _date;
		private System.Windows.Forms.Label _lblparticipants;
		private System.Windows.Forms.TextBox _participants;
		private System.Windows.Forms.Label _lblsituation;
		private System.Windows.Forms.Label _lblsetting;
		private BindingHelper _binder;
		private System.Windows.Forms.TextBox _setting;
		private System.Windows.Forms.Label _lbllocation;
		private System.Windows.Forms.TextBox _location;
		private System.Windows.Forms.Label _lbleventType;
		private System.Windows.Forms.ComboBox _eventType;
		private System.Windows.Forms.TextBox _access;
		private System.Windows.Forms.Label _lblaccess;
	}
}
