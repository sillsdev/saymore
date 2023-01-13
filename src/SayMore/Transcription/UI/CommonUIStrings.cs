using L10NSharp;

namespace SayMore.Transcription.UI
{
    internal static class CommonUIStrings
    {
        internal static string TranscriptionTierDisplayName =>
            LocalizationManager.GetString("SessionsView.Transcription.TierDisplayNames.Transcription", "Transcription");

        public static string TranslationTierDisplayName =>
            LocalizationManager.GetString("SessionsView.Transcription.TierDisplayNames.FreeTranslation", "Free Translation");

        public static string StartAnnotatingTabText => LocalizationManager.GetString(
            "SessionsView.Transcription.StartAnnotatingTab.TabText", "Start Annotating");
    }
}
