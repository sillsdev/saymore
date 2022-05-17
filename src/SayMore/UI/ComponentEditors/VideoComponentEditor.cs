using L10NSharp;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class VideoComponentEditor : MediaComponentEditor
	{
		public delegate VideoComponentEditor Factory(ComponentFile file, string imageKey);

		/// ------------------------------------------------------------------------------------
		public VideoComponentEditor(ComponentFile file, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, FieldGatherer fieldGatherer)
			: base(file, null, imageKey, autoCompleteProvider, fieldGatherer)
		{
			Name = "Video File Information";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized(ILocalizationManager lm)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
				TabText = GetPropertiesTabText();
			base.HandleStringsLocalized(lm);
		}
	}
}
