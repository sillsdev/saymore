using System.Collections.Generic;
using System.Drawing;
using Palaso.Code;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{

	/// ----------------------------------------------------------------------------------------
	public partial class AudioComponentEditor : EditorBase
	{
		private readonly PresetGatherer _presetProvider;

		public delegate AudioComponentEditor Factory(ComponentFile file);

		/// ------------------------------------------------------------------------------------
		public AudioComponentEditor(ComponentFile file, PresetGatherer presetProvider)
		{
			_presetProvider = presetProvider;
			InitializeComponent();
			Name = "Audio File Information";
			_binder.SetComponentFile(file);

		}


		private void _presetMenuButton_Click(object sender, System.EventArgs e)
		{
			_presetMenu.Items.Clear();
			Guard.AgainstNull(_presetProvider, "PresetProvider");
			if (_presetProvider != null)
			{
				foreach (KeyValuePair<string, Dictionary<string, string>> pair in _presetProvider.GetSuggestions())
				{
					KeyValuePair<string, Dictionary<string, string>> valuePair = pair;
					_presetMenu.Items.Add(pair.Key, null, (obj, send) => UsePreset(valuePair.Value));
				}
				var pt = _presetMenuButton.PointToScreen(new Point(0, _presetMenuButton.Height));
				_presetMenu.Show(pt);
			}
		}


		private void UsePreset(IDictionary<string, string> preset)
		{
			//todo: add something to the binder, probably, to make use of this preset
		}
	}
}
