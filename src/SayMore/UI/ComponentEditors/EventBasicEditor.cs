using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
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
		public delegate EventBasicEditor Factory(ComponentFile file, string imageKey);

		private FieldsValuesGrid _gridCustomFields;
		private FieldsValuesGridViewModel _gridViewModel;
		private readonly PersonInformant _personInformant;

		/// ------------------------------------------------------------------------------------
		public EventBasicEditor(ComponentFile file, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer,
			PersonInformant personInformant)
			: base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "EventEditor";

			_personInformant = personInformant;
			InitializeGrid(autoCompleteProvider, fieldGatherer);

			_status.Items.AddRange(Enum.GetNames(typeof(Event.Status))
				.Select(x => x.ToString().Replace('_', ' ')).ToArray());

			//LoadGenreList(autoCompleteProvider, null);
			autoCompleteProvider.NewDataAvailable += LoadGenreList;

			SetBindingHelper(_binder);
			_autoCompleteHelper.SetAutoCompleteProvider(autoCompleteProvider);
			_participants.JITListAcquisition += HandleParticipantJustInTimeListAcquisition;

			var sampleStatusImage = Properties.Resources.StatusFinished;
			if (_status.ItemHeight < sampleStatusImage.Height)
				_status.ItemHeight = sampleStatusImage.Height;

			_id.Enter += delegate { EnsureFirstRowLabelIsVisible(_labelId); };
			_date.Enter += delegate { EnsureFirstRowLabelIsVisible(_labelDate); };
			_status.Enter += delegate { EnsureFirstRowLabelIsVisible(_labelStatus); };
		}

		/// ------------------------------------------------------------------------------------
		private void LoadGenreList(object sender, EventArgs e)
		{
			List<string> genreList = new List<string>();

			var autoCompleteProvider = sender as AutoCompleteValueGatherer;

			if (autoCompleteProvider != null)
			{
				// Add the genres in use, factory or not.
				var valueLists = autoCompleteProvider.GetValueLists(false);
				IEnumerable<string> list = null;
				if (valueLists.TryGetValue("genre", out list))
					genreList.AddRange(list.ToArray());
			}

			// Add the rest of the factory defaults
			if (GenreDefinition.FactoryGenreDefinitions != null)
			{
				if (genreList.Count > 0)
					genreList.Add("-----");

				genreList.AddRange(GenreDefinition.FactoryGenreDefinitions.Select(gd => gd.Name).ToArray());
			}

			if (genreList.Count == 0)
				return;

			// Do this because we've gotten here when the auto-complete helper has new data available.
			_genre.Invoke((MethodInvoker)delegate
			{
				_genre.Items.Clear();
				_genre.Items.AddRange(genreList.ToArray());
			});
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
		private void HandleStatusDrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();

			var enumText = (e.Index < 0 ? string.Empty : _status.Items[e.Index] as string).Replace(' ', '_');
			var img = (Image)Properties.Resources.ResourceManager.GetObject("Status" + enumText);
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
			TextRenderer.DrawText(e.Graphics, Event.GetLocalizedStatus(enumText), e.Font,
				rc, e.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString("EventsView.MetadataEditor.TabText", "Event");
			base.HandleStringsLocalized();
		}
	}
}
