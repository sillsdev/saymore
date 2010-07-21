using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.UI.LowLevelControls;
using SIL.Localization;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class PersonBasicEditor : EditorBase
	{
		public delegate PersonBasicEditor Factory(ComponentFile file, string tabText, string imageKey);

		private readonly List<ParentButton> _fatherButtons = new List<ParentButton>();
		private readonly List<ParentButton> _motherButtons = new List<ParentButton>();

		private FieldsValuesGrid _gridCustomFields;
		private FieldsValuesGridViewModel _gridViewModel;

		/// ------------------------------------------------------------------------------------
		public PersonBasicEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "PersonEditor";

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

			_binder.SetComponentFile(file);
			InitializeGrid(autoCompleteProvider, fieldGatherer);
			SetBindingHelper(_binder);
			_autoCompleteHelper.SetAutoCompleteProvider(autoCompleteProvider);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(IMultiListDataProvider autoCompleteProvider, FieldGatherer fieldGatherer)
		{
			_gridViewModel = new FieldsValuesGridViewModel(_file, autoCompleteProvider,
				fieldGatherer, key => _file.FileType.GetIsCustomFieldId(key));

			_gridCustomFields = new FieldsValuesGrid(_gridViewModel);
			_gridCustomFields.Dock = DockStyle.Top;
			_panelGrid.AutoSize = true;
			_panelGrid.Controls.Add(_gridCustomFields);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			if (_file != null && File.Exists(_file.PathToAnnotatedFile) &&
				file.PathToAnnotatedFile != _file.PathToAnnotatedFile)
			{
				SaveParentLanguages();
			}

			base.SetComponentFile(file);

			if (_gridViewModel != null)
				_gridViewModel.SetComponentFile(file);

			LoadPersonsPhoto();
			LoadParentLanguages();
		}

		/// ------------------------------------------------------------------------------------
		private string PersonFolder
		{
			get { return Path.GetDirectoryName(_file.PathToAnnotatedFile); }
		}

		/// ------------------------------------------------------------------------------------
		private string PhotoFileWithoutExt
		{
			get { return Path.GetFileNameWithoutExtension(_file.PathToAnnotatedFile) + "_Photo"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set values for unbound controls that need special handling.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			// Check that the person still exists.
			if (Directory.Exists(Path.GetDirectoryName(_file.PathToAnnotatedFile)))
				SaveParentLanguages();

			base.OnHandleDestroyed(e);
		}

		#region Methods for handling parents' language
		/// ------------------------------------------------------------------------------------
		private void LoadParentLanguages()
		{
			var fathersLanguage = _binder.GetValue("fathersLanguage");
			var pb = _fatherButtons.Find(x => ((TextBox)x.Tag).Text.Trim() == fathersLanguage);
			if (pb != null)
				pb.Selected = true;

			var mothersLanguage = _binder.GetValue("mothersLanguage");
			pb = _motherButtons.Find(x => ((TextBox)x.Tag).Text.Trim() == mothersLanguage);
			if (pb != null)
				pb.Selected = true;
		}

		/// ------------------------------------------------------------------------------------
		private void SaveParentLanguages()
		{
			var pb = _fatherButtons.SingleOrDefault(x => x.Selected);
			if (pb != null)
				_binder.SetValue("fathersLanguage", ((TextBox)pb.Tag).Text.Trim());

			pb = _motherButtons.SingleOrDefault(x => x.Selected);
			if (pb != null)
				_binder.SetValue("mothersLanguage", ((TextBox)pb.Tag).Text.Trim());
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

		/// ------------------------------------------------------------------------------------
		private static void HandleParentLanguageChange(IEnumerable<ParentButton> buttons,
			ParentButton selectedButton, CancelEventHandler changeHandler)
		{
			foreach (var pb in buttons.Where(x => x != selectedButton))
			{
				pb.SelectedChanging -= changeHandler;
				pb.Selected = false;
				pb.SelectedChanging += changeHandler;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleParentLanguageButtonMouseEnter(object sender, EventArgs e)
		{
			var pb = sender as ParentButton;

			if (pb.ParentType == ParentType.Father)
			{
				_tooltip.SetToolTip(pb, pb.Selected ?
					"Indicates this is the father's primary language" :
					"Click to indicate this is the father's primary language");
			}
			else
			{
				_tooltip.SetToolTip(pb, pb.Selected ?
					"Indicates this is the mother's primary language" :
					"Click to indicate this is the mother's primary language");
			}
		}

		#endregion

		#region Methods for handling the person's picture
		/// ------------------------------------------------------------------------------------
		private void HandlePersonPictureMouseClick(object sender, MouseEventArgs args)
		{
			using (var dlg = new OpenFileDialog())
			{
				var caption = LocalizationManager.LocalizeString(
					"PeopleView.ChangePictureDlgCaption", "Change Picture", "Views");

				dlg.Title = caption;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;
				dlg.Filter = "JPEG Images (*.jpg)|*.jpg|GIF Images (*.gif)|*.gif|" +
					"TIFF Images (*.tif)|*.tif|PNG Images (*.png)|*.png|" +
					"Bitmaps (*.bmp;*.dib)|*.bmp;*.dib|All Files (*.*)|*.*";

				if (dlg.ShowDialog(this) == DialogResult.OK)
				{
					var dest = PhotoFileWithoutExt + Path.GetExtension(dlg.FileName);
					dest = Path.Combine(PersonFolder, dest);

					try
					{
						File.Copy(dlg.FileName, dest, true);
						LoadPersonsPhoto();
					}
					catch (Exception e)
					{
						var msg = LocalizationManager.LocalizeString(
							"PersonView.ErrorChangingPersonsPhotoMsg",
							"There was an error changing the person's photo.");

						Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e, msg);
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void LoadPersonsPhoto()
		{
			if (_picture == null)
				return;

			try
			{
				var photoFiles = Directory.GetFiles(PersonFolder, PhotoFileWithoutExt + ".*");

				if (photoFiles.Length == 0)
					_picture.Image = Resources.kimidNoPhoto;
				else
				{
					// Do this instead of using the Load method because Load keeps a lock on the file.
					using (var fs = new FileStream(photoFiles[0], FileMode.Open, FileAccess.Read))
					{
						_picture.Image = Image.FromStream(fs);
						fs.Close();
					}
				}
			}
			catch (Exception e)
			{
				var msg = LocalizationManager.LocalizeString("PersonView.ErrorLoadingPersonsPhotoMsg",
					"There was an error loading the person's photo.");

				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e, msg);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePersonPictureMouseEnterLeave(object sender, EventArgs e)
		{
			_picture.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePersonPicturePaint(object sender, PaintEventArgs e)
		{
			if (_picture.ClientRectangle.Contains(_picture.PointToClient(MousePosition)))
			{
				e.Graphics.DrawImageUnscaledAndClipped(Resources.kimidChangePicture,
					_picture.ClientRectangle);
			}
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void HandleIdEnter(object sender, EventArgs e)
		{
			// Makes sure the id's label is also visible when the id field gains focus.
			AutoScrollPosition = new Point(0, 0);
		}

		/// ------------------------------------------------------------------------------------
		private void _tblLayoutOuter_Paint(object sender, PaintEventArgs e)
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

			using (Pen pen = new Pen(SystemColors.ControlDark, 1))
			{
				pen.EndCap = System.Drawing.Drawing2D.LineCap.Square;
				var pt1 = new Point(lbl.Left, lbl.Bottom + 1);
				var pt2 = new Point(lbl.Left + lineWidth, lbl.Bottom + 1);
				g.DrawLine(pen, pt1, pt2);
			}
		}
	}
}
