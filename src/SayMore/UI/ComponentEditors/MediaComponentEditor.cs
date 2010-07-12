using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class MediaComponentEditor : EditorBase
	{
		protected FieldsValuesGrid _grid;
		protected FieldsValuesGridViewModel _gridViewModel;

		/// ------------------------------------------------------------------------------------
		public MediaComponentEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			InitializeGrid(autoCompleteProvider, fieldGatherer);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void InitializeGrid(AutoCompleteValueGatherer autoCompleteProvider,
			FieldGatherer fieldGatherer)
		{
			_gridViewModel = new FieldsValuesGridViewModel(_file, autoCompleteProvider,
				fieldGatherer, key => key != "notes");

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
		protected virtual void HandlePresetMenuButtonClick(object sender, EventArgs e)
		{
			_presetMenu.Items.Clear();

			foreach (KeyValuePair<string, Dictionary<string, string>> pair in _file.GetPresetChoices())
			{
				// Copy to avoid the dreaded "access to modified closure"
				KeyValuePair<string, Dictionary<string, string>> valuePair = pair;
				_presetMenu.Items.Add(pair.Key, null, (obj, sendr) => UsePreset(valuePair.Value));
			}

			var pt = _presetMenuButton.PointToScreen(new Point(0, _presetMenuButton.Height));
			_presetMenu.Show(pt);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void UsePreset(IDictionary<string, string> preset)
		{
			_file.UsePreset(preset);
			_gridViewModel.SetComponentFile(_file);
			_grid.Invalidate();
		}
	}
}
