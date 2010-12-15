using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class EventBasicEditor : EditorBase
	{
		public delegate EventBasicEditor Factory(ComponentFile file, string tabText, string imageKey);

		private FieldsValuesGrid _gridCustomFields;
		private FieldsValuesGridViewModel _gridViewModel;
		private readonly PersonInformant _personInformant;

		/// ------------------------------------------------------------------------------------
		public EventBasicEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer,
			PersonInformant personInformant)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "EventEditor";

			_personInformant = personInformant;
			InitializeGrid(autoCompleteProvider, fieldGatherer);
			_status.Items.AddRange(Event.GetStatusNames().ToArray());

			if (GenreDefinition.FactoryGenreDefinitions != null)
			{
				//add the ones in use, factory or otherwise
				var valueLists = autoCompleteProvider.GetValueLists(false);
				IEnumerable<string> list;
				if (valueLists.TryGetValue("genre", out list))
				{
					_genre.Items.AddRange(list.ToArray());
					_genre.Items.Add("-----");
				}

				// Add the rest of the factory defaults
				_genre.Items.AddRange(GenreDefinition.FactoryGenreDefinitions.ToArray());
			}

			SetBindingHelper(_binder);
			_autoCompleteHelper.SetAutoCompleteProvider(autoCompleteProvider);
			_participants.JITListAcquisition += HandleParticipantJustInTimeListAcquisition;

			var sampleStatusImage = Properties.Resources.StatusFinished;
			if (_status.ItemHeight < sampleStatusImage.Height)
				_status.ItemHeight = sampleStatusImage.Height;
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<PickerPopupItem> HandleParticipantJustInTimeListAcquisition(object sender)
		{
			return from name in _personInformant.GetPeopleNamesFromRepository()
				orderby name
				select new PickerPopupItem { Text = name, ToolTipText = null };
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(IMultiListDataProvider autoCompleteProvider,
			FieldGatherer fieldGatherer)
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
			base.SetComponentFile(file);

			if (_gridViewModel != null)
				_gridViewModel.SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleIdEnter(object sender, EventArgs e)
		{
			// Makes sure the id's label is also visible when the id field gains focus.
			AutoScrollPosition = new Point(0, 0);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStatusDrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();

			var text = (e.Index < 0 ? string.Empty : _status.Items[e.Index] as string);
			var img = (Image)Properties.Resources.ResourceManager.GetObject("Status" + text.Replace(' ', '_'));
			int dy = (int)Math.Round((e.Bounds.Height - img.Height) / 2f, MidpointRounding.AwayFromZero);

			// Draw image
			var rc = e.Bounds;
			rc.Width = img.Width;
			rc.Y += dy;
			rc.Height = img.Height;
			e.Graphics.DrawImage(img, rc);

			// Draw text
			rc = e.Bounds;
			rc.X += (img.Width + 3);
			rc.Width -= (img.Width + 3);
			TextRenderer.DrawText(e.Graphics, text, e.Font, rc, e.ForeColor,
				TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis);
		}
	}
}
