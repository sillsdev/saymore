using System.Collections.Generic;
using System.Windows.Forms;
using Localization;
using SayMore.Properties;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public class TranslationAnnotationColumn : TextAnnotationColumnWithMenu
	{
		/// ------------------------------------------------------------------------------------
		public TranslationAnnotationColumn(TierBase tier) : base(tier)
		{
			PlaybackType = (OralAnnotationType)Settings.Default.TranslationPlaybackType;
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<ToolStripMenuItem> GetPlaybackOptionMenus()
		{
			yield return new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = OralAnnotationType.Original,
				Checked = (PlaybackType == OralAnnotationType.Original),
				Text = LocalizationManager.GetString(
					"EventsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.Original",
					"Playback Original Recording"),
			};

			yield return new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = OralAnnotationType.Translation,
				Checked = (PlaybackType == OralAnnotationType.Translation),
				Text = LocalizationManager.GetString(
					"EventsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.OralTranslation",
					"Playback Oral Translation Recording"),
			};

			yield return new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = (OralAnnotationType.Original & OralAnnotationType.Translation),
				Checked = (PlaybackType == (OralAnnotationType.Original & OralAnnotationType.Translation)),
				Text = LocalizationManager.GetString(
					"EventsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.Both",
					"Playback Both Recordings"),
			};
		}
	}
}
