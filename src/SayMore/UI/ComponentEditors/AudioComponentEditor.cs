using System.Collections.Generic;
using System.Drawing;
using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class AudioComponentEditor : EditorBase
	{
		private readonly ComponentFile _file;

	//	public delegate AudioComponentEditor Factory(ComponentFile file);

		/// ------------------------------------------------------------------------------------
		public AudioComponentEditor(ComponentFile file)
		{
			_file = file;
			InitializeComponent();
			Name = "Audio File Information";
			_binder.SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePresetMenuButtonClick(object sender, System.EventArgs e)
		{
			_presetMenu.Items.Clear();
			foreach (KeyValuePair<string, Dictionary<string, string>> pair in _file.GetPresetChoices())
			{
				//copy to avoid the dreadd "access to modified closure"
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
	}
}
