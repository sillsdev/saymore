using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.Controls;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;
using SIL.Sponge.Utilities;
using SilUtils;

namespace SIL.Sponge.Views
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// UI Class for managing the people associated with a project.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PeopleVw : BaseSplitVw
	{
		private readonly SpongeProject _currProj;
		private Person _currPerson;

		private readonly Dictionary<TextBox, ParentButton> _langFathers =
			new Dictionary<TextBox, ParentButton>(5);

		private readonly Dictionary<TextBox, ParentButton> _langMothers =
			new Dictionary<TextBox, ParentButton>(5);

		#region Constructors
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="PeopleVw"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PeopleVw()
		{
			InitializeComponent();
			_gender.SelectedIndex = 0;
			tblAbout.Enabled = false;
			pnlPermissions.Enabled = false;

			lblNoPeopleMsg.BackColor = lpPeople.ListView.BackColor;

			_langFathers[_language0] = _languageFather0;
			_langFathers[_language1] = _languageFather1;
			_langFathers[_language2] = _languageFather2;
			_langFathers[_language3] = _languageFather3;
			_langFathers[_language4] = _languageFather4;

			_langMothers[_language0] = _languageMother0;
			_langMothers[_language1] = _languageMother1;
			_langMothers[_language2] = _languageMother2;
			_langMothers[_language3] = _languageMother3;
			_langMothers[_language4] = _languageMother4;

			// Designer keeps messing up the size of pnlBrowser, so we'll force here.
			pnlBrowser.Width = pnlPermissions.Width - pnlBrowser.Left - 10;
			pnlBrowser.Height = pnlPermissions.Height - pnlBrowser.Top - 10;

			tabPeople.ImageList = new ImageList();
			tabPeople.ImageList.Images.Add(Resources.kimidNoPermissionsWarning);

			tblAbout.Paint += SpongeColors.PaintDataEntryBackground;
			pnlPermissions.Paint += SpongeColors.PaintDataEntryBackground;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionsVw"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PeopleVw(SpongeProject _prj) : this()
		{
			_currProj = (_prj ?? MainWnd.CurrentProject);
			lblNoPeopleMsg.Visible = (_currProj.People.Count == 0);
			lpPeople.Items = _currProj.People.ToArray();
		}

		#endregion

		#region ISpongeView interface methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save any changes to the current person's information when the view is
		/// deactivated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void ViewDeactivated()
		{
			base.ViewDeactivated();

			if (_currPerson != null)
				SavePersonFromView(_currPerson);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the name field is valid. If it is, then save any outstanding changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override bool IsOKToLeaveView(bool showMsgWhenNotOK)
		{
			if (!IsNameOK(showMsgWhenNotOK))
				return false;

			if (_currPerson != null)
				SavePersonFromView(_currPerson);

			return base.IsOKToLeaveView(showMsgWhenNotOK);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save any changes to the current person's information when the view is destroyed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);

			if (_currPerson != null)
				SavePersonFromView(_currPerson);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This fixes a paint error in .Net that manifests itself when tab controls are
		/// resized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tabPeople_SizeChanged(object sender, EventArgs e)
		{
			tabPeople.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Validate the name, checking for no name, or whether or not the name already exists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsNameOK(bool showMsgWhenNotOK)
		{
			if (!tblAbout.Enabled)
				return true;

			string msg = null;
			var name = _fullName.Text.Trim();

			if (name == string.Empty)
			{
				msg = LocalizationManager.LocalizeString("PeopleVw.MissingNameMsg",
					"You must enter the person's name.",
					"Message displayed when a person's name is missing in the people view.",
					"Views", LocalizationCategory.GeneralMessage, LocalizationPriority.High);
			}
			else if (_currPerson.FullName != string.Empty)
			{
				var person = _currProj.GetPerson(name);
				if (person != null && person != _currPerson)
				{
					msg = LocalizationManager.LocalizeString("PeopleVw.DuplicateNameMsg",
						"The name {0} already exists.",
						"Message displayed when entering an already existing person's name in the people view.",
						"Views", LocalizationCategory.GeneralMessage, LocalizationPriority.High);

					msg = string.Format(msg, name);
				}
			}
			if (msg == null)
				return true;

			if (showMsgWhenNotOK)
				Utils.MsgBox(msg);

			return false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lpPeople_SelectedItemChanged(object sender, object newItem)
		{
			if (_currPerson == newItem)
				return;

			if (_currPerson != null)
				SavePersonFromView(_currPerson);

			_currPerson = newItem as Person;
			LoadViewFromPerson(_currPerson);
		}

		#region Event handler for adding and deleting a person
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Prepares the view to accept information for a new person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private object lpPeople_NewButtonClicked(object sender)
		{
			if (_currPerson != null)
				SavePersonFromView(_currPerson);

			if (tabPeople.SelectedTab != tpgAbout)
				tabPeople.SelectedTab = tpgAbout;

			_currPerson = Person.CreateFromName(_currProj, string.Empty);
			_currProj.AddPerson(_currPerson);
			ClearView();
			lblNoPeopleMsg.Visible = false;
			lpPeople.AddItem(_currPerson, true, false);

			UpdateDisplay();

			_fullName.SelectAll();
			_fullName.Focus();

			return null;
		}


		private void UpdateDisplay()
		{
			tblAbout.Enabled = pnlPermissions.Enabled = (_currPerson != null);
			_picture.Enabled = _currPerson != null && _currPerson.CanChoosePicture;
			lpPeople.UpdateItem(_currPerson, _fullName.Text.Trim());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If the current person's name is blank it means the user is in middle of entering
		/// information for a new person. In that case, pressing ESC will cancel adding the
		/// new person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape && _currPerson != null && _currPerson.FullName == string.Empty)
			{
				_currProj.People.Remove(_currPerson);
				_currPerson = null;
				lpPeople.DeleteCurrentItem();
				lpPeople.UpdateItem(_currPerson, null);
				CheckIfNoMorePeople();
				_fullName.SelectAll();
				_fullName.Focus();
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fired after the delete button on the list panel is clicked, but before the
		/// selected item is deleted.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool lpPeople_BeforeItemsDeleted(object sender, List<object> itemsToDelete)
		{
			if (itemsToDelete == null || itemsToDelete.Count == 0)
				return false;

			var msg = LocalizationManager.LocalizeString("PeopleVw.DeleteConfirmationMsg",
				"Are you sure you want to delete the selected person and their information?",
				"Question asked when delete button is clicked on the 'About' tab of the People view.",
				"Views", LocalizationCategory.GeneralMessage, LocalizationPriority.High);

			return (Utils.MsgBox(msg, MessageBoxButtons.YesNo) == DialogResult.Yes);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Fired after an item is deleted from the list panel as a result of the user clicking
		/// on the delete button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lpPeople_AfterItemsDeleted(object sender, List<object> itemsToDelete)
		{
			lpPeople.Tag = itemsToDelete;
			webConsent.Navigated += DeleteSelectedPeople;
			webConsent.Navigate(string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the list of people stored in the people's list panel tag property. This
		/// is done in the event the permissions browser is done navigating to nothing
		/// because until that's done, it may have a lock on a permissions file associated
		/// with the person being deleted, which would prevent the deletion. It's sort of
		/// a strange place to do the deletion, I know.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void DeleteSelectedPeople(object sender, WebBrowserNavigatedEventArgs e)
		{
			webConsent.Navigated -= DeleteSelectedPeople;
			var itemsToDelete = lpPeople.Tag as List<object>;

			if (itemsToDelete != null)
			{
				foreach (var obj in itemsToDelete)
					_currProj.DeletePerson(obj.ToString());

				_currPerson = null;
			}

			CheckIfNoMorePeople();
			lpPeople.Focus();
		}



		#endregion

		#region Event handlers for picture box
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the MouseClick event of the _photo control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _picture_MouseClick(object sender, MouseEventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				var caption = LocalizationManager.LocalizeString(
					"PeopleVw.ChangePictureDlgCaption", "Change Picture", "Views");

				dlg.Title = caption;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;
				dlg.Filter = "JPEG Images (*.jpg)|*.jpg|GIF Images (*.gif)|*.gif|" +
					"TIFF Images (*.tif)|*.tif|PNG Images (*.png)|*.png|" +
					"Bitmaps (*.bmp;*.dib)|*.bmp;*.dib|" + Sponge.OFDlgAllFileTypeText + "|*.*";

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					string picFile = _currPerson.CopyPictureFile(dlg.FileName);
					_picture.Load(picFile);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the mouse enter and leave events for the picture.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleMouseEnterLeaveOnPicture(object sender, EventArgs e)
		{
			_picture.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw an indicator over the image when the mouse is over it. It's supposed to
		/// indicate the user may click the photo to change it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _picture_Paint(object sender, PaintEventArgs e)
		{
			if (_picture.ClientRectangle.Contains(_picture.PointToClient(MousePosition)))
			{
				e.Graphics.DrawImageUnscaledAndClipped(Resources.kimidChangePicture,
					_picture.ClientRectangle);
			}
		}

		#endregion

		#region Methods for moving data between the view's fields and a person object.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the specified person from the data in the form's controls. If the person
		/// is null then a new one is created. If the person is not null but the name is
		/// different from the one currently in the form's full name field, then the old
		/// person file is renamed first (unless saveAfterLoad is false). If the name in the
		/// form is blank, then a unique name is assigned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SavePersonFromView(Person person)
		{
			bool nameChanged = true;

			if (person == null)
				person = new Person();
			else
				nameChanged = RenameIfNecessary(person);

			person.ChangeName(_fullName.Text.Trim());
			person.PrimaryLanguage = _language0.Text.Trim();
			person.LearnedLanguageIn = _learnedIn.Text.Trim();
			person.OtherLangauge0 = _language1.Text.Trim();
			person.OtherLangauge1 = _language2.Text.Trim();
			person.OtherLangauge2 = _language3.Text.Trim();
			person.OtherLangauge3 = _language4.Text.Trim();
			person.Education = _education.Text.Trim();
			person.PrimaryOccupation = _primaryOccupation.Text.Trim();
			person.ContactInfo = _contact.Text.Trim();
			person.Notes = _notes.Text.Trim();
			person.Gender = (Gender)Enum.Parse(typeof(Gender), _gender.SelectedItem as string);

			var kvp = _langFathers.FirstOrDefault(x => x.Value.Selected);
			person.FathersLanguage = (kvp.Key == null ? null : kvp.Key.Text.Trim());

			kvp = _langMothers.FirstOrDefault(x => x.Value.Selected);
			person.MothersLanguage = (kvp.Key == null ? null : kvp.Key.Text.Trim());

			person.BirthYear = _birthYear.Text;

			if (person.CanSave)
				person.Save();

			if (nameChanged)
				lpPeople.UpdateItem(person, person.FullName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Before loding a person object from the data in the form, determine whether or
		/// not the name changed, and perform a rename if it has.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool RenameIfNecessary(Person person)
		{
			if (_fullName.Text.Trim() == string.Empty)
				_fullName.Text = _currProj.GetUniquePersonName();

			var prevName = person.FullName ?? string.Empty;
			var newName = _fullName.Text.Trim();
			bool nameChanged = (prevName != newName);

			if (nameChanged && prevName != string.Empty)
			{
				person.Rename(newName);

				// Rebuild the list of permissions files since they
				// will have moved in the person renaming process.
				var currPermissionFile = (lstPermissionFiles.SelectedItem == null ?
					null : lstPermissionFiles.SelectedItem.ToString());

				LoadPermissionsTabFromPerson(person, currPermissionFile);
			}

			return nameChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the form's controls from the data in the specified person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadViewFromPerson(Person person)
		{

			if (person == null)
			{
				ClearView();
				return;
			}

			_fullName.TextChanged -= _fullName_TextChanged;
			_fullName.Text = person.FullName;
			_fullName.TextChanged += _fullName_TextChanged;

			_language0.Text = person.PrimaryLanguage;
			_language1.Text = person.OtherLangauge0;
			_language2.Text = person.OtherLangauge1;
			_language3.Text = person.OtherLangauge2;
			_language4.Text = person.OtherLangauge3;
			_learnedIn.Text = person.LearnedLanguageIn;
			_education.Text = person.Education;
			_primaryOccupation.Text = person.PrimaryOccupation;
			_contact.Text = person.ContactInfo;
			_notes.Text = person.Notes;
			_gender.SelectedItem = person.Gender.ToString();
			_birthYear.Text = person.BirthYear;

			var pair = _langFathers.FirstOrDefault(x => x.Key.Text == person.FathersLanguage);
			if (pair.Value != null)
				pair.Value.Selected = true;
			else
				_languageFather0.Selected = true;

			pair = _langMothers.FirstOrDefault(x => x.Key.Text == person.MothersLanguage);
			if (pair.Value != null)
				pair.Value.Selected = true;
			else
				_languageMother0.Selected = true;

			try
			{
				if (!string.IsNullOrEmpty(person.PictureFile) && File.Exists(person.PictureFile))
				{// Do this instead of using the Load method because Load keeps a lock on the file.
					using (FileStream fs = new FileStream(person.PictureFile, FileMode.Open, FileAccess.Read))
					{
						_picture.Image = System.Drawing.Image.FromStream(fs);
						fs.Close();
					}
				}
				else
				{
					_picture.Image = Resources.kimidNoPhoto;//don't keep showing the previous person
				}

			}
			catch
			{
				_picture.Image = Resources.kimidNoPhoto;
			}

			UpdateDisplay();

			LoadPermissionsTabFromPerson(person, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the permissions tab from the specified person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadPermissionsTabFromPerson(Person person, string fileToSelect)
		{
			lstPermissionFiles.Items.Clear();

			var list = person.PermissionFiles;
			if (list.Length > 0)
			{
				lstPermissionFiles.Items.AddRange((from x in list
												select new PermissionsFile(x)).ToArray());
			}

			if (string.IsNullOrEmpty(fileToSelect))
				ShowPermissionFile(0);
			else
				SelectPermissionFile(fileToSelect);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears all the fields on the form, setting them to their default values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ClearView()
		{
			_fullName.TextChanged -= _fullName_TextChanged;
			_fullName.Text = string.Empty;
			_fullName.TextChanged += _fullName_TextChanged;

			_language0.Text = string.Empty;
			_language1.Text = string.Empty;
			_language2.Text = string.Empty;
			_language3.Text = string.Empty;
			_language4.Text = string.Empty;
			_learnedIn.Text = string.Empty;
			_education.Text = string.Empty;
			_primaryOccupation.Text = string.Empty;
			_contact.Text = string.Empty;
			_notes.Text = string.Empty;
			_gender.SelectedItem = Gender.Male.ToString();
			_birthYear.Text = string.Empty;
			_languageFather0.Selected = true;
			_languageMother0.Selected = true;
			_picture.Image = Resources.kimidNoPhoto;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Checks if all the people have been deleted. If so, the form is updated accordingly.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void CheckIfNoMorePeople()
		{
			if (lpPeople.Items.Count() == 0)
			{
				lblNoPeopleMsg.Visible = true;
				tblAbout.Enabled = false;
				pnlPermissions.Enabled = false;
				ClearView();
				tpgInformedConsent.ImageIndex = -1;
			}
		}

		#endregion

		#region Event handlers for managing the parent language options.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the father selected changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleFatherSelectedChanging(object sender, CancelEventArgs e)
		{
			// Uncomment this if having no father language specified is not allowed.
			//var bpSender = sender as ParentButton;
			//if (bpSender.Selected)
			//{
			//    e.Cancel = true;
			//    return;
			//}

			foreach (var pb in _langFathers.Values)
			{
				if (pb != sender)
				{
					pb.SelectedChanging -= HandleFatherSelectedChanging;
					pb.Selected = false;
					pb.SelectedChanging += HandleFatherSelectedChanging;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the mother selected changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleMotherSelectedChanging(object sender, CancelEventArgs e)
		{
			// Uncomment this if having no mother language specified is not allowed.
			//var bpSender = sender as ParentButton;
			//if (bpSender.Selected)
			//{
			//    e.Cancel = true;
			//    return;
			//}

			foreach (var pb in _langMothers.Values)
			{
				if (pb != sender)
				{
					pb.SelectedChanging -= HandleMotherSelectedChanging;
					pb.Selected = false;
					pb.SelectedChanging += HandleMotherSelectedChanging;
				}
			}
		}

		#endregion

		#region Event handlers for maintaining and supporting language type-ahead list
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure a language text box has the lastest type-ahead list when it gets focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleLanguageNameEnter(object sender, EventArgs e)
		{
			var langNames = new AutoCompleteStringCollection();
			langNames.AddRange(_currProj.LanguageNames.ToArray());
			((TextBox)sender).AutoCompleteCustomSource = langNames;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes sure the language just entered in the text box is added to the project's
		/// list of available langauges.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleLanguageNameLeave(object sender, EventArgs e)
		{
			_currProj.AddLanguageNames(((TextBox)sender).Text.Trim());
		}

		#endregion

		#region Event handlers for the person's name text box
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the name in the peoples list as the user types in the name text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _fullName_TextChanged(object sender, EventArgs e)
		{
//			if (_fullName.Visible && _fullName.Focused)
//				lpPeople.UpdateItem(_currPerson, _fullName.Text.Trim());
//
//            //review: the following (jh fix for SP-41) violates the pattern actually in use here (sorry)
//            // But the the current code combines view and logic.  If
//            // we're to keep logic out of the view code, how can we not change the model so
//            // that it can inform the state of things like, in this case, whether we're ready
//            // to receive a photot yet (we aren't, if we don't name and thus don't have a folder).
//            // Sorry, I didn't meant to mess with the pattern... will write an email.
//
//		    _currPerson.FullName = _fullName.Text.Trim();
//            UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Validate the name, checking for no name, or whether or not the name already exists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleValidatingFullName(object sender, CancelEventArgs e)
		{
			try
			{
				_currPerson.ChangeName(_fullName.Text.Trim());
			}
			catch(Exception error)
			{
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(error.Message);
				e.Cancel = true;
			}
			UpdateDisplay();
		}

		#endregion

		#region Methods for managing permissions files
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Delete the selected permissions file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnDeletePermissionFile_Click(object sender, EventArgs e)
		{
			if (_currPerson == null || _currPerson.FullName == null ||
				lstPermissionFiles.SelectedItem == null)
			{
				return;
			}

			// Perform the delete when the navigation is finished because until then, the
			// browser control may have a handle on the file being displayed, which means
			// it cannot be deleted from the disk.
			webConsent.Navigated += DeleteSelectedPermissionsFile;
			webConsent.Navigate(string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the selected permissions file from the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void DeleteSelectedPermissionsFile(object sender, WebBrowserNavigatedEventArgs e)
		{
			webConsent.Navigated -= DeleteSelectedPermissionsFile;

			// Figure out what file should be selected after deleting the current file.
			int i = lstPermissionFiles.SelectedIndex + 1;
			if (i == lstPermissionFiles.Items.Count)
				i = lstPermissionFiles.SelectedIndex - 1;

			var newFile = (i < 0 ? null : lstPermissionFiles.Items[i].ToString());

			var pf = lstPermissionFiles.SelectedItem as PermissionsFile;
			_currPerson.DeletePermissionsFile(pf.FullPath);
			LoadPermissionsTabFromPerson(_currPerson, newFile);

			// If there are no more permission files, this will force the warning
			// icon to be displayed next to the person's name.
			if (lstPermissionFiles.Items.Count == 0)
				lpPeople.UpdateItem(_currPerson, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Let the user add a new permissions file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnAddPermissionFile_Click(object sender, EventArgs e)
		{
			if (_currPerson == null || _currPerson.FullName == null)
				return;

			using (var dlg = new OpenFileDialog())
			{
				var caption = LocalizationManager.LocalizeString(
					"NewInformedConsentFileDlg.OpenFileDlgCaption",
					"Specify an Informed Consent File", "Dialog Boxes");

				dlg.Title = caption;
				dlg.Filter = Sponge.OFDlgAllFileTypeText + "|*.*";
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;
				if (dlg.ShowDialog(this) != DialogResult.OK)
					return;

				_currPerson.AddPermissionFile(dlg.FileName);
				LoadPermissionsTabFromPerson(_currPerson, Path.GetFileName(dlg.FileName));

				// If we've just added the first permissions file, then force the warning
				// icon to be displayed next to the person's name.
				if (lstPermissionFiles.Items.Count == 1)
					lpPeople.UpdateItem(_currPerson, null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Attempts to find and select the specified permissions file in the list of
		/// permissions files for the current person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void SelectPermissionFile(string permissionFile)
		{
			foreach (object obj in lstPermissionFiles.Items)
			{
				if (obj.ToString() == permissionFile)
				{
					lstPermissionFiles.SelectedItem = obj;
					break;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When there is at least one permissions file and no file is selected, then select
		/// the first one.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tabPeople_Selected(object sender, TabControlEventArgs e)
		{
			if (_currPerson != null)
				SavePersonFromView(_currPerson);

			if (e.TabPage == tpgInformedConsent && lstPermissionFiles.SelectedItem == null)
				ShowPermissionFile(0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Show the selected file in the browser control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lstPermissionFiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			ShowPermissionFile(lstPermissionFiles.SelectedIndex);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the permission file associated with the specified index in the permissions
		/// file list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ShowPermissionFile(int i)
		{
			if (i < 0 || i >= lstPermissionFiles.Items.Count)
			{
				if (webConsent.Url != null && webConsent.Url.IsFile)
				{
					webConsent.Navigate(string.Empty);
					tpgInformedConsent.ImageIndex = 0;
				}
			}
			else
			{
				if (tabPeople.SelectedTab == tpgInformedConsent)
				{
					var file = lstPermissionFiles.Items[i] as PermissionsFile;
					if (file != null && (webConsent.Url == null || webConsent.Url.LocalPath != file.FullPath))
						webConsent.Navigate(file.FullPath);

					if (lstPermissionFiles.SelectedIndex != i)
					{
						lstPermissionFiles.SelectedIndexChanged -= lstPermissionFiles_SelectedIndexChanged;
						lstPermissionFiles.SelectedIndex = i;
						lstPermissionFiles.SelectedIndexChanged += lstPermissionFiles_SelectedIndexChanged;
					}
				}

				tpgInformedConsent.ImageIndex = -1;
			}
		}

		#endregion

		#region PermissionsFile class
		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private class PermissionsFile
		{
			public string FullPath { get; private set; }

			public PermissionsFile(string fullPath)
			{
				FullPath = fullPath;
			}

			public override string ToString()
			{
				return Path.GetFileName(FullPath);
			}
		}

		#endregion

		private void m_editImageMenuItem_Click(object sender, EventArgs e)
		{
			if(_currPerson.PictureFile != null && File.Exists(_currPerson.PictureFile))
			{
				System.Diagnostics.Process.Start(_currPerson.PictureFile);
			}
		}
	}
}
