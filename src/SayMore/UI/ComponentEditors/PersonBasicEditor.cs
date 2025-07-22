using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using L10NSharp;
using SayMore.Model;
using SIL.Reporting;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.UI.ElementListScreen;
using SayMore.UI.LowLevelControls;
using SayMore.Utilities;
using SIL.Extensions;
using SIL.IO;
using SIL.Windows.Forms.Extensions;
using static System.IO.Path;
using static System.String;
using static System.StringComparison;
using static SayMore.UI.LowLevelControls.ParentType;
using static SIL.Windows.Forms.Extensions.ControlExtensions.ErrorHandlingAction;

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
		private bool _loadingLanguages;
		private bool _suppressWsDlgOnNextEnter;
		private bool _suppressWsDlgForChanges;
		private TextBox _currentLanguageTextBox;
		private readonly HashSet<string> _suppressValidation = new HashSet<string>();
		private readonly Dictionary<TextBox, Color> _languageFieldsDefaultForeColor;

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

			_languageFieldsDefaultForeColor = new Dictionary<TextBox, Color>
				{
					{ _primaryLanguage, _primaryLanguage.ForeColor},
					{ _otherLanguage0, _otherLanguage0.ForeColor},
					{ _otherLanguage1, _otherLanguage1.ForeColor},
					{ _otherLanguage2, _otherLanguage2.ForeColor},
					{ _otherLanguage3, _otherLanguage3.ForeColor}
				};

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

			foreach (var textBox in _languageFieldsDefaultForeColor.Keys)
				textBox.Validating += HandleValidatingLanguage;

			NotifyWhenProjectIsSet();
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
				var path = GetDirectoryName(_file.PathToAnnotatedFile);
				if (!IsNullOrEmpty(path))
				{
					if (Directory.Exists(path))
						SaveParentLanguages();
				}
			}

			_binder.OnDataSaved -= _binder_OnDataSaved;
			foreach (var textBox in _languageFieldsDefaultForeColor.Keys)
				textBox.Validating -= HandleValidatingLanguage;

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

		protected override void SetWorkingLanguageFont(Font font)
		{
			this.SafeInvoke(() =>
				{
					if (!font.Equals(_primaryLanguageLearnedIn.Font))
					{
						_primaryLanguageLearnedIn.Font = font;
						_howToContact.Font = font;
						_education.Font = font;
						_primaryOccupation.Font = font;
					}
				}, $"{GetType().Name}.{nameof(SetWorkingLanguageFont)}", IgnoreAll);
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
			_suppressWsDlgOnNextEnter = false;
		}

		/// ------------------------------------------------------------------------------------
		private PersonListScreen OwningPersonListScreen 
		{
			get
			{
				Control ctrl = Parent;
				while (ctrl != null)
				{
					if (ctrl is PersonListScreen retVal)
						return retVal;
					ctrl = ctrl.Parent;
				}
				return null;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string PersonFolder => GetDirectoryName(_file.PathToAnnotatedFile);

		/// ------------------------------------------------------------------------------------
		public string PictureFileWithoutExt => 
			GetFileNameWithoutExtension(_file.PathToAnnotatedFile) + "_Photo";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path and file name for the person's picture file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetPictureFile()
		{
			var files = Directory.GetFiles(PersonFolder, PictureFileWithoutExt + ".*");
			var picFiles = files.Where(x => _imgFileType.IsMatch(x)).ToArray();
			return picFiles.Length == 0 ? null : picFiles[0];
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
			return picFile == null ? null : _imgFileType.GetMetaFilePath(picFile);
		}

		#region Methods for handling languages
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
			var fathersLanguage = _binder.GetValue(PersonFileType.kFathersLanguage);
			if (!IsNullOrEmpty(fathersLanguage))
			{
				var pb = _fatherButtons.Find(x => ((TextBox)x.Tag).Text.Trim() == fathersLanguage);
				if (pb != null)
					pb.Selected = true;
			}

			var mothersLanguage = _binder.GetValue(PersonFileType.kMothersLanguage);
			if (!IsNullOrEmpty(mothersLanguage))
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
			yield return new KeyValuePair<string, string>(PersonFileType.kFathersLanguage,
				GetParentLanguageName(_fatherButtons.SingleOrDefault(x => x.Selected)));
			yield return new KeyValuePair<string, string>(PersonFileType.kMothersLanguage,
				GetParentLanguageName(_motherButtons.SingleOrDefault(x => x.Selected)));
		}

		/// ------------------------------------------------------------------------------------
		private static string GetParentLanguageName(ParentButton pb)
		{
			return ((TextBox)pb?.Tag)?.Text.Trim();
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
		private void HandleParentLanguageSelectedChanged(object sender, EventArgs e)
		{
			if (_loaded)
				SaveParentLanguages();
		}

		private enum ShowWsDlg
		{
			Unconditionally,
			IfIllFormedAndValuePresentOrPrimaryLanguage,
			IfIllFormed
		}

		private void HandleLanguageFieldEnter(object sender, EventArgs e)
		{
			var textBox = (TextBox)sender;

			var showAsNeeded = !_suppressWsDlgOnNextEnter ||
				_currentLanguageTextBox != textBox;
			
			// We need to set these BEFORE potentially showing the Language Lookup dialog;
			// Otherwise, we can end up re-opening it when the user makes a valid selection.
			_currentLanguageTextBox = textBox;
			_suppressWsDlgOnNextEnter = true;

			if (!showAsNeeded)
				return;

			ShowWritingSystemDlgIfNeeded(
				ShowWsDlg.IfIllFormedAndValuePresentOrPrimaryLanguage, textBox);
		}

		private void HandleLanguageFieldLeave(object sender, EventArgs e)
		{
			if (ContainsFocus)
				_currentLanguageTextBox = null;
		}

		private void HandleLanguageFieldChange(object sender, EventArgs e)
		{
			var textBox = (TextBox)sender;

			// Since the value has now (probably) changed, clear any previous validation error.
			textBox.ForeColor = _languageFieldsDefaultForeColor[textBox];
			_tooltip.SetToolTip(textBox, null);

			if (!_loaded)
				return;

			// Now we want to try to do our best not to annoy the user. If we take one of these
			// early returns, we can always catch problems later during validation.

			if (textBox.Text == Empty || textBox.Text.Contains(":", Ordinal) ||
			    textBox.AutoCompleteCustomSource?.OfType<string>()
				    .Any(a => a.StartsWith(textBox.Text, OrdinalIgnoreCase)) == true)
				return;

			if (_currentLanguageTextBox == textBox && _suppressWsDlgForChanges)
				return; // User previously canceled out of the language lookup while editing.

			ShowWritingSystemDlgIfNeeded(ShowWsDlg.IfIllFormed, textBox);
		}

		private void HandleLanguageFieldDoubleClick(object sender, MouseEventArgs e)
		{
			ShowWritingSystemDlgIfNeeded(ShowWsDlg.Unconditionally, (TextBox)sender);
		}

		private void ShowWritingSystemDlgIfNeeded(ShowWsDlg show, TextBox textBox)
		{
			// Don't pop this up if the Enter/Change event fires as part of program start-up.
			if (Program.CurrentProject == null || !(FindForm()?.Visible ?? false))
				return;

			var isPrimary = textBox == _primaryLanguage;
			var currentValue = textBox.Text;
			bool shouldShowDlg;
			switch (show)
			{
				case ShowWsDlg.Unconditionally:
					shouldShowDlg = true;
					break;
				case ShowWsDlg.IfIllFormedAndValuePresentOrPrimaryLanguage:
					shouldShowDlg = (currentValue != Empty || isPrimary) &&
						!LanguageHelper.IsWellFormedTwoPartLanguageSpecification(currentValue);
					break;
				case ShowWsDlg.IfIllFormed:
					shouldShowDlg = !LanguageHelper.IsWellFormedTwoPartLanguageSpecification(currentValue);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(show), show, null);
			}
			if (shouldShowDlg)
			{
				var languageSpecifier = WritingSystemDlg.LookUpLanguage(FindForm(), currentValue,
					_binder.ComponentFile.ParentElement.UiId, isPrimary,
					GetParentButtonForTextBox(Father, textBox).Selected,
					GetParentButtonForTextBox(Mother, textBox).Selected);
				// If the user cancels out or selects the "Unknown" language, we don't want
				// to hassle them anymore as long as they stay in this text box.
				if (languageSpecifier == null || languageSpecifier.Code == "qaa")
					_suppressWsDlgForChanges = true;
				var newValue = languageSpecifier?.ToString();
				if (newValue != null && textBox.Text != newValue)
					textBox.Text = newValue;
				_suppressWsDlgOnNextEnter = true;
			}
		}

		private ParentButton GetParentButtonForTextBox(ParentType parent, TextBox sender)
		{
			return (parent == Father ? _fatherButtons : _motherButtons)
				.Single(pb => pb.Tag == sender);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLanguageValidated(object sender, EventArgs e)
		{
			var textBox = (TextBox)sender;
			HandleParentLanguageSelectedChanged(GetParentButtonForTextBox(Father, textBox), e);
			HandleParentLanguageSelectedChanged(GetParentButtonForTextBox(Mother, textBox), e);
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
			if (sender is ParentButton pb)
			{
				if (pb.ParentType == Father)
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


		/// ------------------------------------------------------------------------------------
		public override bool IsOKToLeaveEditor
		{
			get
			{
				if (!base.IsOKToLeaveEditor)
					return false;

				return _currentLanguageTextBox == null ||
					!IsDuplicateLanguage(_currentLanguageTextBox);
			}
		}

		/// ------------------------------------------------------------------------------------
		private bool IsDuplicateLanguage(TextBox textBox)
		{
			var currentValue = textBox.Text;
			if (currentValue == Empty)
				return false;

			var langSpec = LanguageHelper.GetTwoPartLanguageSpecification(currentValue);
			var code = langSpec != null && langSpec.IsValid ? langSpec.Code : null;

			var dupTextBox = _languageFieldsDefaultForeColor.Keys.Where(tb => tb != textBox)
				.FirstOrDefault(tb => tb.Text == currentValue || (code != null &&
					LanguageHelper.GetTwoPartLanguageSpecification(tb.Text)?.Code == code));
			if (dupTextBox != null)
			{
				if (textBox == _primaryLanguage)
				{
					// Primary is probably the one we want to keep, so rather than treating it
					// as a validation error, just clear the other one.
					dupTextBox.Text = Empty;
				}
				else
				{
					textBox.ForeColor = Color.Red;
					var tooltipText = dupTextBox == _primaryLanguage ?
						LocalizationManager.GetString("PeopleView.MetadataEditor.DupLanguage.Primary",
							"This language is already specified as the primary language for this person.") :
						LocalizationManager.GetString("PeopleView.MetadataEditor.DupLanguage.Other",
							"This language is already specified as another language for this person.");
					_tooltip.Show(tooltipText, textBox, 4000);

					return true;
				}
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleValidatingLanguage(object sender, CancelEventArgs e)
		{
			var textBox = (TextBox)sender;
			var currentValue = textBox.Text;
			if (currentValue == Empty)
				return;

			// First, prevent duplicates.
			e.Cancel = IsDuplicateLanguage(textBox);

			// Next, deal with the possibility of a suboptimal language code.

			if (!(OwningPersonListScreen as Control ?? this).ContainsFocus)
			{
				// We don't want to display this when the BindingHelper is validating all children
				// because the user likely didn't even enter these fields or change their values and,
				// in any case, will have no idea which field we're even referring to in the message.
				return;
			}

			_suppressWsDlgOnNextEnter = false;
			_suppressWsDlgForChanges = false;

			var suppressKey = $"{_id.Text}/{textBox.Text}";
			// We don't want to badger the user. If they tell us they are not interested in fixing
			// this, we remember that decision and stop hassling them.
			if (_suppressValidation.Contains(suppressKey))
				return;
			
			if (LanguageHelper.IsValidBCP47SayMoreLanguageSpecification(currentValue))
				return;

			var msgFmtPart1 = textBox == _primaryLanguage ?
				LocalizationManager.GetString(
					"PeopleView.MetadataEditor.InvalidBcp47Code.Primary",
					"The primary language entered does not appear to specify a valid code according " +
					"to {0}.",
					"Param: \"BCP-47\" (literal name of language tag standard") :
				LocalizationManager.GetString(
					"PeopleView.MetadataEditor.InvalidBcp47Code.Other",
					"The language entered does not appear to specify a valid code according " +
					"to {0}.",
					"Param: \"BCP-47\" (literal name of language tag standard");

			var msgPart1 = Format(msgFmtPart1, "BCP-47");

			if (MessageBox.Show(this, Format(LocalizationManager.GetString(
					    "PeopleView.MetadataEditor.InvalidBcp47Code.ExplanationAndQuestion",
					    "{0} Although {1} allows you to enter anything you want in this " +
					    "field (e.g., just a language name or abbreviation), for archiving " +
					    "purposes, a recognizable code is generally preferable. Would you like " +
					    "to try to look up the intended language?",
					    "Param 0: The first part of the message (from " +
					    "PeopleView.MetadataEditor.InvalidBcp47Code.Primary or " +
					    "PeopleView.MetadataEditor.InvalidBcp47Code.Other); " +
					    "Param 1: \"SayMore\" (product name)"),
					    msgPart1, Program.ProductName),
				    ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				ShowWritingSystemDlgIfNeeded(ShowWsDlg.Unconditionally, textBox);
				// We don't set e.Cancel to true because the user either:
				// 1) chose a new value in the dialog (in which case it is valid unless it's a
				// duplicate (in which case I think it will get re-validated and caught);
				// 2) cancelled out of the dlg, in which case they've presumably decided to
				// live with the suboptimal value.
			}

			// If the user chose to ignore this problem or opened the language lookup dialog and
			// then decided to cancel and not deal with it, we don't want to hassle them about
			// this particular language for this particular person again during this session.
			// (They can always decide to come back and fix it on their own if they want to.)
			// We could probably just add the key unconditionally because if they did change
			// select a valid value in the dialog, it's very unlikely they would ever come back
			// and reset it to the old invalid value. But there is a chance they could.
			if (currentValue == textBox.Text)
			{
				_suppressValidation.Add(suppressKey);
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
			var newPicFile = PictureFileWithoutExt + GetExtension(fileName);
			newPicFile = Combine(PersonFolder, newPicFile);

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
					RobustFile.Copy(fileName, newPicFile, true);
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

				ErrorReport.NotifyUserOfProblem(error, msg);
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

				ErrorReport.NotifyUserOfProblem(e, msg);
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
		/// localized to non-English text.
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
