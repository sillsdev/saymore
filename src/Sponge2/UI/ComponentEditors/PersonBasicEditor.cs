using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Sponge2.Model.Files;
using Sponge2.Properties;
using Sponge2.UI.LowLevelControls;

namespace Sponge2.UI.ComponentEditors
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

			_motherButtons.AddRange(new[] { _pbPimaryLangMother, _pbOtherLangMother0,
				_pbOtherLangMother1, _pbOtherLangMother2, _pbOtherLangMother3 });
		}

		/// ------------------------------------------------------------------------------------
		private void HandleFathersLanguageChanging(object sender, CancelEventArgs e)
		{
			// TODO: figure out how this will work with the binder.

			var selectedButton = sender as ParentButton;

			foreach (var pb in _fatherButtons)
			{
				if (pb != selectedButton)
				{
					pb.SelectedChanging -= HandleFathersLanguageChanging;
					pb.Selected = false;
					pb.SelectedChanging += HandleFathersLanguageChanging;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMothersLanguageChanging(object sender, CancelEventArgs e)
		{
			// TODO: figure out how this will work with the binder.

			var selectedButton = sender as ParentButton;

			foreach (var pb in _motherButtons)
			{
				if (pb != selectedButton)
				{
					pb.SelectedChanging -= HandleMothersLanguageChanging;
					pb.Selected = false;
					pb.SelectedChanging += HandleMothersLanguageChanging;
				}
			}

		}

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
