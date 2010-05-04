namespace Sponge2.UI.ComponentEditors
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
			this.lblFullName = new System.Windows.Forms.Label();
			this._id = new System.Windows.Forms.TextBox();
			this.lblBirthYear = new System.Windows.Forms.Label();
			this._birthYear = new System.Windows.Forms.TextBox();
			this._tblLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
			this._gender = new System.Windows.Forms.ComboBox();
			this._pnlPrimaryLanguage = new Sponge2.UI.LowLevelControls.UnderlinedHdgBox();
			this._tblLayoutPrimaryLanguage = new System.Windows.Forms.TableLayoutPanel();
			this._lblPrimaryLanguageLearnedIn = new System.Windows.Forms.Label();
			this._primaryLanguage = new System.Windows.Forms.TextBox();
			this._primaryLanguageLearnedIn = new System.Windows.Forms.TextBox();
			this._lblgender = new System.Windows.Forms.Label();
			this._binder = new Sponge2.UI.ComponentEditors.BindingHelper(this.components);
			this._tblLayoutOuter.SuspendLayout();
			this._pnlPrimaryLanguage.SuspendLayout();
			this._tblLayoutPrimaryLanguage.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblFullName
			// 
			this.lblFullName.AutoSize = true;
			this.lblFullName.Location = new System.Drawing.Point(3, 0);
			this.lblFullName.Name = "lblFullName";
			this.lblFullName.Size = new System.Drawing.Size(54, 13);
			this.lblFullName.TabIndex = 4;
			this.lblFullName.Text = "&Full Name";
			// 
			// _id
			// 
			this._id.Dock = System.Windows.Forms.DockStyle.Top;
			this._binder.SetIsBound(this._id, true);
			this._id.Location = new System.Drawing.Point(3, 16);
			this._id.Name = "_id";
			this._id.Size = new System.Drawing.Size(320, 20);
			this._id.TabIndex = 5;
			// 
			// lblBirthYear
			// 
			this.lblBirthYear.AutoSize = true;
			this.lblBirthYear.Location = new System.Drawing.Point(329, 0);
			this.lblBirthYear.Name = "lblBirthYear";
			this.lblBirthYear.Size = new System.Drawing.Size(53, 13);
			this.lblBirthYear.TabIndex = 6;
			this.lblBirthYear.Text = "&Birth Year";
			// 
			// _birthYear
			// 
			this._birthYear.Dock = System.Windows.Forms.DockStyle.Top;
			this._binder.SetIsBound(this._birthYear, true);
			this._birthYear.Location = new System.Drawing.Point(329, 16);
			this._birthYear.Name = "_birthYear";
			this._birthYear.Size = new System.Drawing.Size(171, 20);
			this._birthYear.TabIndex = 7;
			// 
			// _tblLayoutOuter
			// 
			this._tblLayoutOuter.BackColor = System.Drawing.Color.Transparent;
			this._tblLayoutOuter.ColumnCount = 2;
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64.93507F));
			this._tblLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.06493F));
			this._tblLayoutOuter.Controls.Add(this._gender, 1, 3);
			this._tblLayoutOuter.Controls.Add(this._pnlPrimaryLanguage, 0, 2);
			this._tblLayoutOuter.Controls.Add(this.lblFullName, 0, 0);
			this._tblLayoutOuter.Controls.Add(this.lblBirthYear, 1, 0);
			this._tblLayoutOuter.Controls.Add(this._id, 0, 1);
			this._tblLayoutOuter.Controls.Add(this._birthYear, 1, 1);
			this._tblLayoutOuter.Controls.Add(this._lblgender, 1, 2);
			this._tblLayoutOuter.Dock = System.Windows.Forms.DockStyle.Top;
			this._tblLayoutOuter.Location = new System.Drawing.Point(0, 0);
			this._tblLayoutOuter.Name = "_tblLayoutOuter";
			this._tblLayoutOuter.RowCount = 5;
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tblLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tblLayoutOuter.Size = new System.Drawing.Size(503, 153);
			this._tblLayoutOuter.TabIndex = 8;
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
			this._gender.Location = new System.Drawing.Point(329, 62);
			this._gender.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
			this._gender.Name = "_gender";
			this._gender.Size = new System.Drawing.Size(171, 21);
			this._gender.TabIndex = 9;
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
			this._pnlPrimaryLanguage.Location = new System.Drawing.Point(3, 42);
			this._pnlPrimaryLanguage.Name = "_pnlPrimaryLanguage";
			this._tblLayoutOuter.SetRowSpan(this._pnlPrimaryLanguage, 2);
			this._pnlPrimaryLanguage.Size = new System.Drawing.Size(320, 81);
			this._pnlPrimaryLanguage.TabIndex = 9;
			this._pnlPrimaryLanguage.Text = "Primary Language";
			// 
			// _tblLayoutPrimaryLanguage
			// 
			this._tblLayoutPrimaryLanguage.AutoSize = true;
			this._tblLayoutPrimaryLanguage.ColumnCount = 2;
			this._tblLayoutPrimaryLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tblLayoutPrimaryLanguage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tblLayoutPrimaryLanguage.Controls.Add(this._lblPrimaryLanguageLearnedIn, 0, 1);
			this._tblLayoutPrimaryLanguage.Controls.Add(this._primaryLanguage, 0, 0);
			this._tblLayoutPrimaryLanguage.Controls.Add(this._primaryLanguageLearnedIn, 1, 1);
			this._tblLayoutPrimaryLanguage.Dock = System.Windows.Forms.DockStyle.Top;
			this._tblLayoutPrimaryLanguage.Location = new System.Drawing.Point(0, 15);
			this._tblLayoutPrimaryLanguage.Name = "_tblLayoutPrimaryLanguage";
			this._tblLayoutPrimaryLanguage.RowCount = 2;
			this._tblLayoutPrimaryLanguage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tblLayoutPrimaryLanguage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this._tblLayoutPrimaryLanguage.Size = new System.Drawing.Size(320, 66);
			this._tblLayoutPrimaryLanguage.TabIndex = 1;
			// 
			// _lblPrimaryLanguageLearnedIn
			// 
			this._lblPrimaryLanguageLearnedIn.AutoSize = true;
			this._lblPrimaryLanguageLearnedIn.Location = new System.Drawing.Point(0, 38);
			this._lblPrimaryLanguageLearnedIn.Margin = new System.Windows.Forms.Padding(0, 5, 3, 0);
			this._lblPrimaryLanguageLearnedIn.Name = "_lblPrimaryLanguageLearnedIn";
			this._lblPrimaryLanguageLearnedIn.Size = new System.Drawing.Size(65, 15);
			this._lblPrimaryLanguageLearnedIn.TabIndex = 6;
			this._lblPrimaryLanguageLearnedIn.Text = "&Learned In:";
			// 
			// _primaryLanguage
			// 
			this._primaryLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._primaryLanguage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this._primaryLanguage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this._tblLayoutPrimaryLanguage.SetColumnSpan(this._primaryLanguage, 2);
			this._binder.SetIsBound(this._primaryLanguage, true);
			this._primaryLanguage.Location = new System.Drawing.Point(3, 7);
			this._primaryLanguage.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
			this._primaryLanguage.Name = "_primaryLanguage";
			this._primaryLanguage.Size = new System.Drawing.Size(314, 23);
			this._primaryLanguage.TabIndex = 5;
			// 
			// _primaryLanguageLearnedIn
			// 
			this._primaryLanguageLearnedIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._binder.SetIsBound(this._primaryLanguageLearnedIn, true);
			this._primaryLanguageLearnedIn.Location = new System.Drawing.Point(68, 36);
			this._primaryLanguageLearnedIn.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
			this._primaryLanguageLearnedIn.Name = "_primaryLanguageLearnedIn";
			this._primaryLanguageLearnedIn.Size = new System.Drawing.Size(249, 23);
			this._primaryLanguageLearnedIn.TabIndex = 7;
			// 
			// _lblgender
			// 
			this._lblgender.AutoSize = true;
			this._lblgender.Location = new System.Drawing.Point(329, 44);
			this._lblgender.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
			this._lblgender.Name = "_lblgender";
			this._lblgender.Size = new System.Drawing.Size(42, 13);
			this._lblgender.TabIndex = 10;
			this._lblgender.Text = "Gender";
			// 
			// PersonBasicEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this._tblLayoutOuter);
			this.Name = "PersonBasicEditor";
			this.Size = new System.Drawing.Size(503, 296);
			this._tblLayoutOuter.ResumeLayout(false);
			this._tblLayoutOuter.PerformLayout();
			this._pnlPrimaryLanguage.ResumeLayout(false);
			this._pnlPrimaryLanguage.PerformLayout();
			this._tblLayoutPrimaryLanguage.ResumeLayout(false);
			this._tblLayoutPrimaryLanguage.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblFullName;
		private System.Windows.Forms.TextBox _id;
		private System.Windows.Forms.Label lblBirthYear;
		private System.Windows.Forms.TextBox _birthYear;
		private System.Windows.Forms.TableLayoutPanel _tblLayoutOuter;
		private Sponge2.UI.LowLevelControls.UnderlinedHdgBox _pnlPrimaryLanguage;
		private System.Windows.Forms.TextBox _primaryLanguage;
		private System.Windows.Forms.Label _lblPrimaryLanguageLearnedIn;
		private System.Windows.Forms.TextBox _primaryLanguageLearnedIn;
		private System.Windows.Forms.TableLayoutPanel _tblLayoutPrimaryLanguage;
		private System.Windows.Forms.Label _lblgender;
		private System.Windows.Forms.ComboBox _gender;
		private BindingHelper _binder;
	}
}
