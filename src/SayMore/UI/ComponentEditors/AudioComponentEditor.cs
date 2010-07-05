using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class AudioComponentEditor : EditorBase
	{
		public delegate AudioComponentEditor Factory(ComponentFile file, string tabText, string imageKey);

		private FieldsValuesGrid _grid;
		private FieldsValuesGridViewModel _gridViewModel;

		/// ------------------------------------------------------------------------------------
		public AudioComponentEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Audio File Information";
			InitializeGrid(autoCompleteProvider, fieldGatherer);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid(AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
		{
			_gridViewModel = new FieldsValuesGridViewModel(_file, autoCompleteProvider, fieldGatherer);
			_grid = new FieldsValuesGrid(_gridViewModel);
			_grid.Dock = DockStyle.Fill;
			_tableLayout.Controls.Add(_grid, 0, 1);
			_grid.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);
			_gridViewModel.SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePresetMenuButtonClick(object sender, EventArgs e)
		{
			_presetMenu.Items.Clear();
			foreach (KeyValuePair<string, Dictionary<string, string>> pair in _file.GetPresetChoices())
			{
				// Copy to avoid the dreaded "access to modified closure"
				KeyValuePair<string, Dictionary<string, string>> valuePair = pair;
				_presetMenu.Items.Add(pair.Key, null, (obj, send) => UsePreset(valuePair.Value));
			}

			var pt = _presetMenuButton.PointToScreen(new Point(0, _presetMenuButton.Height));
			_presetMenu.Show(pt);
		}

		/// ------------------------------------------------------------------------------------
		private void UsePreset(IDictionary<string, string> preset)
		{
			_file.UsePreset(preset);

			// TODO: update the fields in the grid.
			//_binder.UpdateFieldsFromFile();
		}

		//private
	}
}
