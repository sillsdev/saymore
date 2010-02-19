using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SIL.Localize.LocalizationUtils;
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
					m_picture.Load(dlg.FileName);
					m_picture.Tag = dlg.FileName;
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
			person.PrimaryLanguage = m_primaryLanguage.Text.Trim();
			person.PrimaryLanguageParent = m_primaryLanguageParent.Value;
			person.LearnedLanguageIn = m_learnedIn.Text.Trim();
			person.OtherLangauge0 = m_otherLanguage0.Text.Trim();
			person.OtherLangauge1 = m_otherLanguage1.Text.Trim();
			person.OtherLangauge2 = m_otherLanguage2.Text.Trim();
			person.OtherLangauge3 = m_otherLanguage3.Text.Trim();
			person.OtherLangaugeParent0 = m_otherLanguageParentGender0.Value;
			person.OtherLangaugeParent1 = m_otherLanguageParentGender1.Value;
			person.OtherLangaugeParent2 = m_otherLanguageParentGender2.Value;
			person.OtherLangaugeParent3 = m_otherLanguageParentGender3.Value;
			person.Education = m_education.Text.Trim();
			person.PrimaryOccupation = m_primaryOccupation.Text.Trim();
			person.ContactInfo = m_contact.Text.Trim();
			person.Notes = m_notes.Text.Trim();
			person.Gender = (Gender)Enum.Parse(typeof(Gender), m_gender.SelectedItem as string);
			person.PictureFile = m_picture.Tag as string;

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
			m_primaryLanguage.Text = person.PrimaryLanguage;
			m_primaryLanguageParent.Value = person.PrimaryLanguageParent;
			m_learnedIn.Text = person.LearnedLanguageIn;
			m_otherLanguage0.Text = person.OtherLangauge0;
			m_otherLanguage1.Text = person.OtherLangauge1;
			m_otherLanguage2.Text = person.OtherLangauge2;
			m_otherLanguage3.Text = person.OtherLangauge3;
			m_otherLanguageParentGender0.Value = person.OtherLangaugeParent0;
			m_otherLanguageParentGender1.Value = person.OtherLangaugeParent1;
			m_otherLanguageParentGender2.Value = person.OtherLangaugeParent2;
			m_otherLanguageParentGender3.Value = person.OtherLangaugeParent3;
			m_education.Text = person.Education;
			m_primaryOccupation.Text = person.PrimaryOccupation;
			m_contact.Text = person.ContactInfo;
			m_notes.Text = person.Notes;
			m_gender.SelectedItem = person.Gender.ToString();
			m_birthYear.Text = (person.BirthYear > 0 ?
				person.BirthYear.ToString() : string.Empty);

			try
			{
				m_picture.Load(person.PictureFile);
				m_picture.Tag = person.PictureFile;
			}
			catch
			{
				m_picture.Image = Resources.kimidNoPhoto;
				m_picture.Tag = null;
			}
		}

		#endregion
	}
}
