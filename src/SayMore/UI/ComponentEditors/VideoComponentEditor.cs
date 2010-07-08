using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class VideoComponentEditor : MediaComponentEditor
	{
		public delegate VideoComponentEditor Factory(ComponentFile file, string tabText, string imageKey);

		/// ------------------------------------------------------------------------------------
		public VideoComponentEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, tabText, imageKey, autoCompleteProvider, fieldGatherer)
		{
			Name = "Video File Information";
		}
	}
}
