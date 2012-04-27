using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.Utilities.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class AudioComponentEditor : MediaComponentEditor
	{
		public delegate AudioComponentEditor Factory(ComponentFile file, string imageKey);

		/// ------------------------------------------------------------------------------------
		public AudioComponentEditor(ComponentFile file, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, null, imageKey, autoCompleteProvider, fieldGatherer)
		{
			Name = "Audio File Information";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = GetPropertiesTabText();
			base.HandleStringsLocalized();
		}
	}
}
