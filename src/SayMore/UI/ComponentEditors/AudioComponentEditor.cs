using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class AudioComponentEditor : MediaComponentEditor
	{
		public delegate AudioComponentEditor Factory(ComponentFile file, string tabText, string imageKey);

		/// ------------------------------------------------------------------------------------
		public AudioComponentEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, tabText, imageKey, autoCompleteProvider, fieldGatherer)
		{
			Name = "Audio File Information";
		}
	}
}
