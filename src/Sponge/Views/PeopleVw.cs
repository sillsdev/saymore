using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
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

		private readonly Dictionary<TextBox, ParentButton> m_langFathers =
			new Dictionary<TextBox, ParentButton>(5);

		private readonly Dictionary<TextBox, ParentButton> m_langMothers =
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

			lblNoPeopleMsg.BackColor = lpPeople.ListView.BackColor;

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
			lblNoPeopleMsg.Visible = (m_currProj.People.Count == 0);
			lpPeople.Items = m_currProj.People.ToArray();
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

			if (m_currPerson != null)
				LoadPersonFromForm(m_currPerson, true);
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

			if (m_currPerson != null)
				LoadPersonFromForm(m_currPerson, true);

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
		/// Validate the name, checking for no name, or whether or not the name already exists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool IsNameOK(bool showMsgWhenNotOK)
		{
			if (!tblLayout.Enabled)
				return true;

			string msg = null;
			var name = m_fullName.Text.Trim();

			if (name == string.Empty)
			{
				msg = LocalizationManager.LocalizeString("PeopleVw.MissingNameMsg",
					"You must enter the person's name.",
					"Message displayed when a person's name is missing in the people view.",
					"Views", LocalizationCategory.GeneralMessage, LocalizationPriority.High);
			}
			else if (m_currPerson.FullName != string.Empty)
			{
				var person = m_currProj.GetPerson(name);
				if (person != null && person != m_currPerson)
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
			if (m_currPerson == newItem)
				return;

			if (m_currPerson != null)
				LoadPersonFromForm(m_currPerson, true);

			m_currPerson = newItem as Person;
			LoadFormFromPerson(m_currPerson);
		}

		#region Event handler for adding and deleting a person
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Prepares the view to accept information for a new person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private object lpPeople_NewButtonClicked(object sender)
		{
			if (m_currPerson != null)
				LoadPersonFromForm(m_currPerson, true);

			m_currPerson = Person.CreateFromName(m_currProj, string.Empty);
			m_currProj.AddPerson(m_currPerson);
			ClearForm();
			lblNoPeopleMsg.Visible = false;
			lpPeople.AddItem(m_currPerson, true, false);

			tblLayout.Enabled = true;
			m_fullName.SelectAll();
			m_fullName.Focus();

			return null;
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
			if (keyData == Keys.Escape && m_currPerson != null && m_currPerson.FullName == string.Empty)
			{
				m_currProj.People.Remove(m_currPerson);
				m_currPerson = null;
				lpPeople.DeleteCurrentItem();
				lpPeople.UpdateItemText(m_currPerson, null);
				m_fullName.SelectAll();
				m_fullName.Focus();
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
			if (itemsToDelete != null)
			{
				foreach (var obj in itemsToDelete)
					m_currProj.DeletePerson(obj.ToString());

				m_currPerson = null;
			}

			lblNoPeopleMsg.Visible = (lpPeople.Items.Count() == 0);
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
					string picFile = m_currPerson.CopyPictureFile(dlg.FileName);
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
		/// person file is renamed first (unless saveAfterLoad is false). If the name in the
		/// form is blank, then a unique name is assigned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadPersonFromForm(Person person, bool saveAfterLoad)
		{
			if (person == null)
				person = new Person();
			else
			{
				if (m_fullName.Text.Trim() == string.Empty)
					m_fullName.Text = m_currProj.GetUniquePersonName();

				if (saveAfterLoad && person.FullName != m_fullName.Text.Trim())
					person.Rename(m_fullName.Text.Trim());
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
				person.Save();

			lpPeople.UpdateItemText(person, person.FullName);
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
			{
				ClearForm();
				return;
			}

			m_fullName.TextChanged -= m_fullName_TextChanged;
			m_fullName.Text = person.FullName;
			m_fullName.TextChanged += m_fullName_TextChanged;

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
				m_picture.Load(person.PictureFile);
			}
			catch
			{
				m_picture.Image = Resources.kimidNoPhoto;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears all the fields on the form, setting them to their default values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ClearForm()
		{
			m_fullName.TextChanged -= m_fullName_TextChanged;
			m_fullName.Text = string.Empty;
			m_fullName.TextChanged += m_fullName_TextChanged;

			m_language0.Text = string.Empty;
			m_language1.Text = string.Empty;
			m_language2.Text = string.Empty;
			m_language3.Text = string.Empty;
			m_language4.Text = string.Empty;
			m_learnedIn.Text = string.Empty;
			m_education.Text = string.Empty;
			m_primaryOccupation.Text = string.Empty;
			m_contact.Text = string.Empty;
			m_notes.Text = string.Empty;
			m_gender.SelectedItem = Gender.Male.ToString();
			m_birthYear.Text = string.Empty;
			m_languageFather0.Selected = true;
			m_languageMother0.Selected = true;
			m_picture.Image = Resources.kimidNoPhoto;
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
			langNames.AddRange(m_currProj.LanguageNames.ToArray());
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
			m_currProj.AddLanguageNames(((TextBox)sender).Text.Trim());
		}

		#endregion

		#region Event handlers for the person's name text box
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Turn on the text changing event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_fullName_Enter(object sender, EventArgs e)
		{
			m_fullName.TextChanged += m_fullName_TextChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Turn off the text changing event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_fullName_Leave(object sender, EventArgs e)
		{
			m_fullName.TextChanged -= m_fullName_TextChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the name in the peoples list as the user types in the name text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_fullName_TextChanged(object sender, EventArgs e)
		{
			lpPeople.UpdateItemText(m_currPerson, m_fullName.Text.Trim());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Validate the name, checking for no name, or whether or not the name already exists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void m_fullName_Validating(object sender, CancelEventArgs e)
		{
			if (!IsNameOK(true))
			{
				m_fullName.SelectAll();
				e.Cancel = true;
			}
		}

		#endregion
	}
}
