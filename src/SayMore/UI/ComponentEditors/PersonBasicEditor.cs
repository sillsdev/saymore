using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class PersonBasicEditor : EditorBase
	{
		private readonly List<ParentButton> _fatherButtons = new List<ParentButton>();
		private readonly List<ParentButton> _motherButtons = new List<ParentButton>();

		/// ------------------------------------------------------------------------------------
		public PersonBasicEditor(ComponentFile file)
		{
			InitializeComponent();
			Name = "Basic";
			_binder.SetComponentFile(file);

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

			GetParentLanguages();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set values for unbound controls that need special handling.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(System.EventArgs e)
		{
			SetParentLanguages();
			base.OnHandleDestroyed(e);
		}

		#region Methods for handling parents' language
		/// ------------------------------------------------------------------------------------
		private void GetParentLanguages()
		{
			var fathersLanguage = _binder.GetValue("fathersLanguage");
			var pb = _fatherButtons.FirstOrDefault(x => ((TextBox)x.Tag).Text.Trim() == fathersLanguage);
			if (pb != null)
				pb.Selected = true;

			var mothersLanguage = _binder.GetValue("mothersLanguage");
			pb = _motherButtons.FirstOrDefault(x => ((TextBox)x.Tag).Text.Trim() == mothersLanguage);
			if (pb != null)
				pb.Selected = true;
		}

		/// ------------------------------------------------------------------------------------
		private void SetParentLanguages()
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
			var selectedButton = sender as ParentButton;

			foreach (var pb in _fatherButtons.Where(x => x != selectedButton))
			{
				pb.SelectedChanging -= HandleFathersLanguageChanging;
				pb.Selected = false;
				pb.SelectedChanging += HandleFathersLanguageChanging;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMothersLanguageChanging(object sender, CancelEventArgs e)
		{
			var selectedButton = sender as ParentButton;

			foreach (var pb in _motherButtons.Where(x => x != selectedButton))
			{
				pb.SelectedChanging -= HandleMothersLanguageChanging;
				pb.Selected = false;
				pb.SelectedChanging += HandleMothersLanguageChanging;
			}
		}

		#endregion

		#region Methods for handling the person's picture
		/// ------------------------------------------------------------------------------------
		private void HandlePersonPictureMouseClick(object sender, MouseEventArgs e)
		{
			// TODO: Shoe open file dialog to let user pick photo.
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePersonPictureMouseEnterLeave(object sender, System.EventArgs e)
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
	}
}
