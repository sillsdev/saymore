using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.UI.LowLevelControls;
using SayMore.Utilities;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public sealed partial class PersonBasicEditor : EditorBase
	{
		public delegate PersonBasicEditor Factory(ComponentFile file, string imageKey);

		private readonly List<ParentButton> _fatherButtons = new List<ParentButton>();
		private readonly List<ParentButton> _motherButtons = new List<ParentButton>();

		private FieldsValuesGrid _gridCustomFields;
		private FieldsValuesGridViewModel _gridViewModel;
		private readonly ImageFileType _imgFileType;

		private bool _loaded;

		// SP-846: Do not save parent languages while setting them
		private bool _loadingLanguages = false;

		/// ------------------------------------------------------------------------------------
		public PersonBasicEditor(ComponentFile file, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer,
			ImageFileType imgFileType)
			: base(file, null, imageKey)
		{
			Logger.WriteEvent("PersonBasicEditor constructor. file = {0}", file);

			InitializeComponent();
			Name = "PersonEditor";

			_imgFileType = imgFileType;

			_fatherButtons.AddRange(new[] {_pbPrimaryLangFather, _pbOtherLangFather0,
				_pbOtherLangFather1, _pbOtherLangFather2, _pbOtherLangFather3 });

			_motherButtons.AddRange(new[] { _pbPrimaryLangMother, _pbOtherLangMother0,
				_pbOtherLangMother1, _pbOtherLangMother2, _pbOtherLangMother3 });

			_pbPrimaryLangFather.Tag = _primaryLanguage;
			_pbPrimaryLangMother.Tag = _primaryLanguage;
			_pbOtherLangFather0.Tag = _otherLanguage0;
			_pbOtherLangMother0.Tag = _otherLanguage0;
			_pbOtherLangFather1.Tag = _otherLanguage1;
			_pbOtherLangMother1.Tag = _otherLanguage1;
			_pbOtherLangFather2.Tag = _otherLanguage2;
			_pbOtherLangMother2.Tag = _otherLanguage2;
			_pbOtherLangFather3.Tag = _otherLanguage3;
			_pbOtherLangMother3.Tag = _otherLanguage3;

			HandleStringsLocalized(null);
			_binder.TranslateBoundValueBeingSaved += HandleBinderTranslateBoundValueBeingSaved;
			_binder.TranslateBoundValueBeingRetrieved += HandleBinderTranslateBoundValueBeingRetrieved;
			_binder.SetComponentFile(file);

			InitializeGrid(autoCompleteProvider, fieldGatherer);

			SetBindingHelper(_binder);
			_autoCompleteHelper.SetAutoCompleteProvider(autoCompleteProvider);

			_id.Enter += delegate { EnsureFirstRowLabelIsVisible(_labelFullName); };
			_birthYear.Enter += delegate { EnsureFirstRowLabelIsVisible(_labelBirthYear); };

			LoadAndValidatePersonInfo();

			_binder.OnDataSaved += _binder_OnDataSaved;
		}

		void _binder_OnDataSaved()
		{
			Program.OnPersonDataChanged();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set values for unbound controls that need special handling.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			// Check that the person still exists.
			if (_file.PathToAnnotatedFile != null)
			{
				var path = Path.GetDirectoryName(_file.PathToAnnotatedFile);
				if (!string.IsNullOrEmpty(path))
				{
					if (Directory.Exists(path))
						SaveParentLanguages();
				}
			}

			_binder.OnDataSaved -= _binder_OnDataSaved;

			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(IMultiListDataProvider autoCompleteProvider, FieldGatherer fieldGatherer)
		{
			_gridViewModel = new CustomFieldsValuesGridViewModel(_file, autoCompleteProvider,
				fieldGatherer);

			_gridCustomFields = new FieldsValuesGrid(_gridViewModel, "PersonBasicEditor._gridCustomFields") { Dock = DockStyle.Top };
			_panelGrid.AutoSize = true;
			_panelGrid.Controls.Add(_gridCustomFields);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			_loaded = false;

			base.SetComponentFile(file);

			if (_gridViewModel != null)
				_gridViewModel.SetComponentFile(file);

			LoadAndValidatePersonInfo();
		}

		/// ------------------------------------------------------------------------------------
		private void LoadAndValidatePersonInfo()
		{
			LoadPersonsPicture();
			LoadParentLanguages();

			ValidateBirthYear();

			_loaded = true;
		}

		/// ------------------------------------------------------------------------------------
		public string PersonFolder
		{
			get { return Path.GetDirectoryName(_file.PathToAnnotatedFile); }
		}

		/// ------------------------------------------------------------------------------------
		public string PictureFileWithoutExt
		{
			get { return Path.GetFileNameWithoutExtension(_file.PathToAnnotatedFile) + "_Photo"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path and file name for the person's picture file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetPictureFile()
		{
			var files = Directory.GetFiles(PersonFolder, PictureFileWithoutExt + ".*");
			var picFiles = files.Where(x => _imgFileType.IsMatch(x)).ToArray();
			return (picFiles.Length == 0 ? null : picFiles[0]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path and file name for the meta file associated with the person's
		/// picture file (i.e. sidecar file).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetPictureMetaFile()
		{
			var picFile = GetPictureFile();
			return (picFile == null ? null : _imgFileType.GetMetaFilePath(picFile));
		}

		#region Methods for handling parents' language
		/// ------------------------------------------------------------------------------------
		private void LoadParentLanguages()
		{
			_loadingLanguages = true;

			// SP-824: Parent language flags not clearing correctly
			foreach (var pb in _fatherButtons)
				pb.Selected = false;

			foreach (var pb in _motherButtons)
				pb.Selected = false;


			// SP-810: Parent language data not saving correctly
			var fathersLanguage = _binder.GetValue("fathersLanguage");
			if (!string.IsNullOrEmpty(fathersLanguage))
			{
				var pb = _fatherButtons.Find(x => ((TextBox)x.Tag).Text.Trim() == fathersLanguage);
				if (pb != null)
					pb.Selected = true;
			}

			var mothersLanguage = _binder.GetValue("mothersLanguage");
			if (!string.IsNullOrEmpty(mothersLanguage))
			{
				var pb = _motherButtons.Find(x => ((TextBox)x.Tag).Text.Trim() == mothersLanguage);
				if (pb != null)
					pb.Selected = true;
			}

			_loadingLanguages = false;
		}

		/// ------------------------------------------------------------------------------------
		private void SaveParentLanguages()
		{
			if (!_loadingLanguages)
				_binder.SetValues(GetParentLanguageKeyValuePairs());
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<KeyValuePair<string, string>> GetParentLanguageKeyValuePairs()
		{
			yield return new KeyValuePair<string, string>("fathersLanguage",
				GetParentLanguageName(_fatherButtons.SingleOrDefault(x => x.Selected)));
			yield return new KeyValuePair<string, string>("mothersLanguage",
				GetParentLanguageName(_motherButtons.SingleOrDefault(x => x.Selected)));
		}

		/// ------------------------------------------------------------------------------------
		private string GetParentLanguageName(ParentButton pb)
		{
			return pb == null ? null : ((TextBox)pb.Tag).Text.Trim();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFathersLanguageChanging(object sender, CancelEventArgs e)
		{
			HandleParentLanguageChange(_fatherButtons, sender as ParentButton,
				HandleFathersLanguageChanging);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMothersLanguageChanging(object sender, CancelEventArgs e)
		{
			HandleParentLanguageChange(_motherButtons, sender as ParentButton,
				HandleMothersLanguageChanging);
		}

		// SP-810:  Parent language data not saving correctly
		void HandleParentLanguageSelectedChanged(object sender, EventArgs e)
		{
			if (_loaded)
				SaveParentLanguages();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLanguageValidated(object sender, EventArgs e)
		{
			HandleParentLanguageSelectedChanged(_fatherButtons.Single(pb => pb.Tag == sender), e);
			HandleParentLanguageSelectedChanged(_motherButtons.Single(pb => pb.Tag == sender), e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleParentLanguageChange(IEnumerable<ParentButton> buttons,
			ParentButton selectedButton, CancelEventHandler changeHandler)
		{
			foreach (var pb in buttons.Where(x => x != selectedButton))
			{
				pb.SelectedChanged -= HandleParentLanguageSelectedChanged;
				pb.SelectedChanging -= changeHandler;
				pb.Selected = false;
				pb.SelectedChanged += HandleParentLanguageSelectedChanged;
				pb.SelectedChanging += changeHandler;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleParentLanguageButtonMouseEnter(object sender, EventArgs e)
		{
			var pb = sender as ParentButton;

			if (pb != null)
			{
				if (pb.ParentType == ParentType.Father)
				{
					var tipSelected = LocalizationManager.GetString("PeopleView.MetadataEditor.FatherSelectorToolTip.WhenSelected",
						"Indicates this is the father's primary language");

					var tipNotSelected =
						LocalizationManager.GetString("PeopleView.MetadataEditor.FatherSelectorToolTip.WhenNotSelected",
							"Click to indicate this is the father's primary language");

					_tooltip.SetToolTip(pb, pb.Selected ? tipSelected : tipNotSelected);
				}
				else
				{
					var tipSelected = LocalizationManager.GetString("PeopleView.MetadataEditor.MotherSelectorToolTip.WhenSelected",
						"Indicates this is the mother's primary language");

					var tipNotSelected =
						LocalizationManager.GetString("PeopleView.MetadataEditor.MotherSelectorToolTip.WhenNotSelected",
							"Click to indicate this is the mother's primary language");

					_tooltip.SetToolTip(pb, pb.Selected ? tipSelected : tipNotSelected);
				}
			}
		}

		#endregion

		#region Methods for handling the person's picture
		/// ------------------------------------------------------------------------------------
		private void HandlePersonPictureMouseClick(object sender, MouseEventArgs args)
		{
			var previouslyActiveControl = ActiveControl;
			_personsPicture.Focus();
			using (var dlg = new OpenFileDialog())
			{
				var caption = LocalizationManager.GetString("PeopleView.MetadataEditor.ChangePictureDlgCaption", "Change Picture");

				var imageFileTypes = LocalizationManager.GetString("PeopleView.MetadataEditor.ImageFileTypes",
					"JPEG Images (*.jpg)|*.jpg|GIF Images (*.gif)|*.gif|TIFF Images (*.tif)|*.tif|PNG Images (*.png)|*.png|Bitmaps (*.bmp;*.dib)|*.bmp;*.dib|All Files (*.*)|*.*");

				dlg.Title = caption;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;
				dlg.Filter = imageFileTypes;

				if (dlg.ShowDialog(this) == DialogResult.OK)
					ChangePersonsPicture(dlg.FileName);
			}
			if (previouslyActiveControl != null)
				previouslyActiveControl.Focus();
		}

		/// ------------------------------------------------------------------------------------
		private void ChangePersonsPicture(string fileName)
		{
			Program.SuspendBackgroundProcesses();
			Exception error = null;

			var oldPicMetaFile = GetPictureMetaFile();
			var oldPicFile = GetPictureFile();
			var newPicFile = PictureFileWithoutExt + Path.GetExtension(fileName);
			newPicFile = Path.Combine(PersonFolder, newPicFile);

			if (oldPicFile != null)
			{
				try
				{
					// Delete the old picture.
					File.Delete(oldPicFile);
				}
				catch (Exception e)
				{
					error = e;
				}
			}

			if (error == null && oldPicMetaFile != null && File.Exists(oldPicMetaFile))
			{
				try
				{
					// Rename the previous picture's meta file according to the new picture file name.
					var newPicMetaFile = _imgFileType.GetMetaFilePath(newPicFile);
					File.Move(oldPicMetaFile, newPicMetaFile);
				}
				catch (Exception e)
				{
					error = e;
				}
			}

			if (error == null)
			{
				try
				{
					// Copy the new picture file to the person's folder.
					File.Copy(fileName, newPicFile, true);
					LoadPersonsPicture();
				}
				catch (Exception e)
				{
					error = e;
				}
			}

			if (error == null)
			{
				if (ComponentFileListRefreshAction != null)
					ComponentFileListRefreshAction(null, null);
			}
			else
			{
				var msg = LocalizationManager.GetString(
					"PeopleView.MetadataEditor.ErrorChangingPersonsPhotoMsg",
					"There was an error changing the person's photo.");

				SIL.Reporting.ErrorReport.NotifyUserOfProblem(error, msg);
			}

			Program.ResumeBackgroundProcesses(true);
		}

		/// ------------------------------------------------------------------------------------
		private void LoadPersonsPicture()
		{
			if (_personsPicture == null)
				return;

			Program.SuspendAudioVideoBackgroundProcesses();

			try
			{
				var picFile = GetPictureFile();

				if (picFile == null)
					_personsPicture.Image = ResourceImageCache.kimidNoPhoto;
				else
				{
					// Do this instead of using the Load method because Load keeps a lock on the file.
					using (var fs = new FileStream(picFile, FileMode.Open, FileAccess.Read))
					{
						_personsPicture.Image = Image.FromStream(fs);
						fs.Close();
					}
				}
			}
			catch (Exception e)
			{
				var msg = LocalizationManager.GetString("PeopleView.MetadataEditor.ErrorLoadingPersonsPhotoMsg",
					"There was an error loading the person's photo.");

				SIL.Reporting.ErrorReport.NotifyUserOfProblem(e, msg);
			}

			Program.ResumeAudioVideoBackgroundProcesses(true);
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePersonPictureMouseEnterLeave(object sender, EventArgs e)
		{
			_personsPicture.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePersonPicturePaint(object sender, PaintEventArgs e)
		{
			if (!_personsPicture.ClientRectangle.Contains(_personsPicture.PointToClient(MousePosition)))
				return;

			var img = ResourceImageCache.kimidChangePicture;
			var rc = _personsPicture.ClientRectangle;

			if (rc.Width > rc.Height)
			{
				rc.Width = rc.Height;
				rc.X = (_personsPicture.ClientRectangle.Width - rc.Width) / 2;
			}
			else if (rc.Height > rc.Width)
			{
				rc.Height = rc.Width;
				rc.Y = (_personsPicture.ClientRectangle.Height - rc.Height) / 2;
			}

			e.Graphics.DrawImage(img, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePictureDragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;

			var picFile = GetPictureFileFromDragData(e.Data);
			if (picFile != null)
			{
				e.Effect = DragDropEffects.Copy;
				_personsPicture.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePictureDragLeave(object sender, EventArgs e)
		{
			_personsPicture.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePictureDragDrop(object sender, DragEventArgs e)
		{
			var picFile = GetPictureFileFromDragData(e.Data);
			if (picFile != null)
				ChangePersonsPicture(picFile);
		}

		/// ------------------------------------------------------------------------------------
		private string GetPictureFileFromDragData(IDataObject data)
		{
			if (!data.GetFormats().Contains(DataFormats.FileDrop))
				return null;

			var list = data.GetData(DataFormats.FileDrop) as string[];
			if (list == null || list.Length != 1)
				return null;

			return (_imgFileType.IsMatch(list[0]) ? list[0] : null);
		}

		#endregion

		#region Methods for handling localized gender names
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text and gender names in case they were localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized(ILocalizationManager lm)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
			{
				TabText = LocalizationManager.GetString("PeopleView.MetadataEditor.TabText",
					"Person");

				if (_gender != null)
				{
					int i = _gender.SelectedIndex;
					_gender.Items.Clear();
					_gender.Items.Add(LocalizationManager.GetString(
						"PeopleView.MetadataEditor.GenderSelector.Male", "Male"));
					_gender.Items.Add(LocalizationManager.GetString(
						"PeopleView.MetadataEditor.GenderSelector.Female", "Female"));
					_gender.SelectedIndex = i;
				}
			}

			base.HandleStringsLocalized(lm);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Instead of letting the binding helper set the gender combo box value from the
		/// value in the file (which will be the English text for male or female), we'll
		/// intercept the process since the text in the gender combo box may have been
		/// localized to non English text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleBinderTranslateBoundValueBeingRetrieved(object sender,
			TranslateBoundValueBeingRetrievedArgs args)
		{
			if (args.BoundControl == _gender)
			{
				// Because of a former bug (SP-847), gender metadata was saved as localized
				// string instead of English, so when retrieving, recognize those versions of the
				// values for "Male" as well.
				string valueFromFile = args.ValueFromFile.Normalize(NormalizationForm.FormD);
				_gender.SelectedIndex = (valueFromFile == "Male" ||
					valueFromFile == "Macho" ||
					valueFromFile == "Mâle".Normalize(NormalizationForm.FormD) ||
					valueFromFile == "Мужской".Normalize(NormalizationForm.FormD) ||
					valueFromFile == "男性".Normalize(NormalizationForm.FormD) ? 0 : 1);
				args.Handled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When the binding helper gets to writing field values to the metadata file, we need
		/// to make sure the English values for male and female are written to the file, not
		/// the localized values for male and female (which is what is in the gender combo box).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleBinderTranslateBoundValueBeingSaved(object sender,
			TranslateBoundValueBeingSavedArgs args)
		{
			if (args.BoundControl == _gender)
				args.NewValue = (_gender.SelectedIndex == 0 ? "Male" : "Female");
		}

		#endregion

		#region Painting methods
		/// ------------------------------------------------------------------------------------
		private void HandleTableLayoutPaint(object sender, PaintEventArgs e)
		{
			DrawUnderlineBelowLabel(e.Graphics, _labelPrimaryLanguage);
			DrawUnderlineBelowLabel(e.Graphics, _labelOtherLanguages);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawUnderlineBelowLabel(Graphics g, Control lbl)
		{
			var col = _tableLayout.GetColumn(lbl);
			var span = _tableLayout.GetColumnSpan(lbl);

			var widths = _tableLayout.GetColumnWidths();

			var lineWidth = 0;
			for (int i = col; i < col + span; i++)
				lineWidth += widths[i];

			lineWidth -= lbl.Margin.Right;
			lineWidth -= _pbPrimaryLangMother.Margin.Right;

			using (var pen = new Pen(SystemColors.ControlDark, 1))
			{
				pen.EndCap = System.Drawing.Drawing2D.LineCap.Square;
				var pt1 = new Point(lbl.Left, lbl.Bottom + 1);
				var pt2 = new Point(lbl.Left + lineWidth, lbl.Bottom + 1);
				g.DrawLine(pen, pt1, pt2);
			}
		}

		#endregion

		private void _birthYear_Validating(object sender, CancelEventArgs e)
		{
			ValidateBirthYear();
		}

		private void ValidateBirthYear()
		{
			_birthYear.ForeColor = _birthYear.Text.IsValidBirthYear() ? _id.ForeColor : Color.Red;
		}
	}
}
