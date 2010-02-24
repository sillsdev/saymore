using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
using SIL.Sponge.Controls;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;
using SilUtils;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// UI Class for managing the people associated with a project.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class PeopleVw : BaseSplitVw
	{
		private readonly SpongeProject m_currProj;
		private Person m_currPerson;
		private List<TextBox> m_txtLanguages = new List<TextBox>(5);

		private Dictionary<TextBox, ParentButton> m_langFathers =
			new Dictionary<TextBox, ParentButton>(5);

		private Dictionary<TextBox, ParentButton> m_langMothers =
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
			m_gender.SelectedIndex = 0;
			tblLayout.Enabled = false;
			HideSaveCancel();

			m_txtLanguages.Add(m_language0);
			m_txtLanguages.Add(m_language1);
			m_txtLanguages.Add(m_language2);
			m_txtLanguages.Add(m_language3);
			m_txtLanguages.Add(m_language4);

			m_langFathers[m_language0] = m_languageFather0;
			m_langFathers[m_language1] = m_languageFather1;
			m_langFathers[m_language2] = m_languageFather2;
			m_langFathers[m_language3] = m_languageFather3;
			m_langFathers[m_language4] = m_languageFather4;

			m_langMothers[m_language0] = m_languageMother0;
			m_langMothers[m_language1] = m_languageMother1;
			m_langMothers[m_language2] = m_languageMother2;
			m_langMothers[m_language3] = m_languageMother3;
			m_langMothers[m_language4] = m_languageMother4;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="SessionsVw"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PeopleVw(SpongeProject m_prj) : this()
		{
			m_currProj = (m_prj ?? MainWnd.CurrentProject);
			lpPeople.Items = m_currProj.People.ToArray();
			SetAutoCompleteLanguageList();
		}

		private void SetAutoCompleteLanguageList()
		{
			var langNames = new AutoCompleteStringCollection();
			langNames.AddRange(m_currProj.LanguageNames.ToArray());

			m_language0.AutoCompleteCustomSource = langNames;
			m_language1.AutoCompleteCustomSource = langNames;
			m_language2.AutoCompleteCustomSource = langNames;
			m_language3.AutoCompleteCustomSource = langNames;
			m_language4.AutoCompleteCustomSource = langNames;
		}

		#endregion

		#region Methods for showing and hiding the Save and Cancel buttons
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Shows the save and cancel buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ShowSaveCancel()
		{
			Utils.SetWindowRedraw(pnlRightSide, false);
			btnSave.Visible = true;
			btnSave.Enabled = false;
			btnCancel.Visible = true;
			m_notes.Height = btnSave.Top - m_notes.Top - 7;
			Utils.SetWindowRedraw(pnlRightSide, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Hides the save and cancel buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HideSaveCancel()
		{
			Utils.SetWindowRedraw(pnlRightSide, false);
			btnSave.Visible = false;
			btnCancel.Visible = false;
			m_notes.Height = btnSave.Bottom - m_notes.Top;
			Utils.SetWindowRedraw(pnlRightSide, true);
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

			if (m_currPerson != null)
				LoadPersonFromForm(m_currPerson, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Save any changes to the current person's information when the view is
		/// deactivated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void ViewDeactivated()
		{
			base.ViewDeactivated();

			if (m_currPerson != null)
				LoadPersonFromForm(m_currPerson, true);
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
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lpPeople_SelectedItemChanged(object sender, object newItem)
		{
			if (m_currPerson != null)
				LoadPersonFromForm(m_currPerson, true);

			m_currPerson = newItem as Person;
			LoadFormFromPerson(m_currPerson);
		}

		#region Event methods for deleting a person
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
			if (itemsToDelete != null)
			{
				foreach (var obj in itemsToDelete)
					m_currProj.DeletePerson(obj.ToString());

				m_currPerson = null;
			}

			lpPeople.Focus();
		}

		#endregion

		#region Event methods for adding a new person
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Prepares the view to accept information for a new person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private object lpPeople_NewButtonClicked(object sender)
		{
			if (m_currPerson != null)
				LoadPersonFromForm(m_currPerson, true);

			ShowSaveCancel();
			lpPeople.PushFocusedItem(true);
			lpPeople.Enabled = false;
			tblLayout.Enabled = true;

			m_currPerson = new Person();
			LoadFormFromPerson(m_currPerson);
			m_fullName.Focus();
			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines when to enable/disable the save button when the user is in the process
		/// of adding a new name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_fullName_TextChanged(object sender, EventArgs e)
		{
			if (btnSave.Visible)
			{
				// Enable the button if there's text in the name field
				// and it doesn't match a name that already exists.
				var name = m_fullName.Text.Trim();
				btnSave.Enabled = (name != string.Empty &&
					m_currProj.People.FirstOrDefault(x => x.FullName == name) == null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the new person, adds it to the people list and makes it the current person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnSave_Click(object sender, EventArgs e)
		{
			HideSaveCancel();
			LoadPersonFromForm(m_currPerson, false);

			if (!m_currProj.AddPerson(m_currPerson, true))
				lpPeople.PopFocusedItem(true);
			else
			{
				lpPeople.PopFocusedItem(false);
				lpPeople.SelectedItemChanged -= lpPeople_SelectedItemChanged;
				lpPeople.Items = m_currProj.People.ToArray();
				lpPeople.SelectedItemChanged += lpPeople_SelectedItemChanged;
				lpPeople.CurrentItem = m_currPerson;
			}

			lpPeople.Enabled = true;
			lpPeople.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Cancels adding a new person by restoring the person that was current before the
		/// New button was pressed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnCancel_Click(object sender, EventArgs e)
		{
			HideSaveCancel();
			m_currPerson = null;
			lpPeople.PopFocusedItem(true);
			lpPeople.Enabled = true;
			lpPeople.Focus();
		}

		#endregion

		#region Event handlers for picture box
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the MouseClick event of the m_photo control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_picture_MouseClick(object sender, MouseEventArgs e)
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
					string picFile = m_currPerson.SetPictureFile(m_currProj.PeoplesPath, dlg.FileName);
					m_picture.Load(picFile);
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
			m_picture.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw an indicator over the image when the mouse is over it. It's supposed to
		/// indicate the user may click the photo to change it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_picture_Paint(object sender, PaintEventArgs e)
		{
			if (m_picture.ClientRectangle.Contains(m_picture.PointToClient(MousePosition)))
			{
				e.Graphics.DrawImageUnscaledAndClipped(Resources.kimidChangePicture,
					m_picture.ClientRectangle);
			}
		}

		#endregion

		#region Methods for moving data between the view's fields and a person object.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the specified person from the data in the form's controls. If the person
		/// is null then a new one is created. If the person is not null but the name is
		/// different from the one currently in the form's full name field, then the old
		/// person file is deleted first (unless saveAfterLoad is false).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadPersonFromForm(Person person, bool saveAfterLoad)
		{
			if (person == null)
				person = new Person();
			else
			{
				// REVIEW: How should we handle the fact that a name could be edited
				// so that it matches a name of a different person?
				if (saveAfterLoad && person.FullName != m_fullName.Text.Trim())
				{
					try
					{
						File.Delete(Path.Combine(m_currProj.PeoplesPath, person.FileName));
					}
					catch { }
				}
			}

			person.FullName = m_fullName.Text.Trim();
			person.PrimaryLanguage = m_language0.Text.Trim();
			person.LearnedLanguageIn = m_learnedIn.Text.Trim();
			person.OtherLangauge0 = m_language1.Text.Trim();
			person.OtherLangauge1 = m_language2.Text.Trim();
			person.OtherLangauge2 = m_language3.Text.Trim();
			person.OtherLangauge3 = m_language4.Text.Trim();
			person.Education = m_education.Text.Trim();
			person.PrimaryOccupation = m_primaryOccupation.Text.Trim();
			person.ContactInfo = m_contact.Text.Trim();
			person.Notes = m_notes.Text.Trim();
			person.Gender = (Gender)Enum.Parse(typeof(Gender), m_gender.SelectedItem as string);

			var kvp = m_langFathers.FirstOrDefault(x => x.Value.Selected);
			person.FathersLanguage = (kvp.Key == null ? null : kvp.Key.Text.Trim());

			kvp = m_langMothers.FirstOrDefault(x => x.Value.Selected);
			person.MothersLanguage = (kvp.Key == null ? null : kvp.Key.Text.Trim());

			int year;
			if (int.TryParse(m_birthYear.Text, out year))
				person.BirthYear = year;

			if (saveAfterLoad && person.CanSave)
				person.Save(m_currProj.PeoplesPath);

			lpPeople.RefreshItemTexts();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the form's controls from the data in the specified person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadFormFromPerson(Person person)
		{
			tblLayout.Enabled = (person != null);

			if (person == null)
				person = new Person();

			m_fullName.Text = person.FullName;
			m_language0.Text = person.PrimaryLanguage;
			m_language1.Text = person.OtherLangauge0;
			m_language2.Text = person.OtherLangauge1;
			m_language3.Text = person.OtherLangauge2;
			m_language4.Text = person.OtherLangauge3;
			m_learnedIn.Text = person.LearnedLanguageIn;
			m_education.Text = person.Education;
			m_primaryOccupation.Text = person.PrimaryOccupation;
			m_contact.Text = person.ContactInfo;
			m_notes.Text = person.Notes;
			m_gender.SelectedItem = person.Gender.ToString();
			m_birthYear.Text = (person.BirthYear > 0 ?
				person.BirthYear.ToString() : string.Empty);

			var kvp = m_langFathers.FirstOrDefault(x => x.Key.Text == person.FathersLanguage);
			if (kvp.Value != null)
				kvp.Value.Selected = true;
			else
				m_languageFather0.Selected = true;

			kvp = m_langMothers.FirstOrDefault(x => x.Key.Text == person.MothersLanguage);
			if (kvp.Value != null)
				kvp.Value.Selected = true;
			else
				m_languageMother0.Selected = true;

			try
			{
				m_picture.Load(person.GetPictureFile(m_currProj.PeoplesPath));
			}
			catch
			{
				m_picture.Image = Resources.kimidNoPhoto;
			}
		}

		#endregion

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

			foreach (var pb in m_langFathers.Values)
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

			foreach (var pb in m_langMothers.Values)
			{
				if (pb != sender)
				{
					pb.SelectedChanging -= HandleMotherSelectedChanging;
					pb.Selected = false;
					pb.SelectedChanging += HandleMotherSelectedChanging;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the language name validated.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleLanguageNameValidated(object sender, EventArgs e)
		{
//			m_currProj.AddLanguageNames(((TextBox)sender).Text.Trim());
//			SetAutoCompleteLanguageList();
		}
	}
}
