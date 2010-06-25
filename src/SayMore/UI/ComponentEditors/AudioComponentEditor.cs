using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SilUtils;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class AudioComponentEditor : EditorBase
	{
		public delegate AudioComponentEditor Factory(ComponentFile file, string tabText, string imageKey);

		private FieldsValuesGrid _grid;
		private FieldUpdater _customFieldUpdater;

		/// ------------------------------------------------------------------------------------
		public AudioComponentEditor(ComponentFile file, string tabText, string imageKey,
			FieldUpdater customFieldUpdater) : base(file, tabText, imageKey)
		{
			InitializeComponent();
			InitializeGrid();
			Name = "Audio File Information";
			SetBindingHelper(_binder);

			_customFieldUpdater = customFieldUpdater;
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid()
		{
			var model = new FieldsValuesGridViewModel(_file,
				GetDefaultFieldIdsToDisplayInGrid(), GetCustomFieldIdsToDisplayInGrid());

			_grid = new FieldsValuesGrid(model);
			_grid.Dock = DockStyle.Top;
			_tableLayout.Controls.Add(_grid, 0, 1);
			_grid.BringToFront();
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<string> GetAllDefaultFieldIds()
		{
			yield return "Recordist";
			yield return "Device";
			yield return "Microphone";
			yield return "Channel";
			yield return "Bit_Depth";
			yield return "Sample_Rate";
			yield return "Analog_Gain";
			yield return "notes";
		}

		/// ------------------------------------------------------------------------------------
		private IEnumerable<string> GetDefaultFieldIdsToDisplayInGrid()
		{
			// Show all but the notes field in the grid.
			return from id in GetAllDefaultFieldIds()
				   where id != "notes"
				   select id;
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
			_binder.UpdateFieldsFromFile();
		}

		//private
	}
}
