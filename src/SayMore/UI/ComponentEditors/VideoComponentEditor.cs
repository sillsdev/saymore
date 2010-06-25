using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class VideoComponentEditor : EditorBase
	{
		private FieldsValuesGrid _grid;

		/// ------------------------------------------------------------------------------------
		public VideoComponentEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			InitializeGrid();
			Name = "Video File Information";
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeGrid()
		{
			var model = new FieldsValuesGridViewModel(_file,
				GetDefaultFieldIdsToDisplayInGrid(), GetCustomFieldIdsToDisplayInGrid());

			_grid = new FieldsValuesGrid(model);
			_grid.Dock = DockStyle.Top;
			Controls.Add(_grid);
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
			yield return "Resolution";
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
	}
}
