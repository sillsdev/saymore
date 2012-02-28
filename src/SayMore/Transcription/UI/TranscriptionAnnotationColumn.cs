using System.Collections.Generic;
using System.Windows.Forms;
using Localization;
using SayMore.Properties;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public class TranscriptionAnnotationColumn : TextAnnotationColumnWithMenu
	{
		/// ------------------------------------------------------------------------------------
		public TranscriptionAnnotationColumn(TierBase tier) : base(tier)
		{
			PlaybackType = (OralAnnotationType)Settings.Default.TranscriptionPlaybackType;
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<ToolStripMenuItem> GetPlaybackOptionMenus()
		{
			yield return new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = OralAnnotationType.Original,
				Checked = (PlaybackType == OralAnnotationType.Original),
				Text = LocalizationManager.GetString(
					"EventsView.Transcription.TextAnnotationEditor.TranscriptionColumnPlaybackMenuOptions.Original",
					"Playback Original Recording"),
			};

			yield return new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = OralAnnotationType.Careful,
				Checked = (PlaybackType == OralAnnotationType.Careful),
				Text = LocalizationManager.GetString(
					"EventsView.Transcription.TextAnnotationEditor.TranscriptionColumnPlaybackMenuOptions.CarefulSpeech",
					"Playback Careful Speech Recording"),
			};

			yield return new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = (OralAnnotationType.Original & OralAnnotationType.Careful),
				Checked = (PlaybackType == (OralAnnotationType.Original & OralAnnotationType.Careful)),
				Text = LocalizationManager.GetString(
					"EventsView.Transcription.TextAnnotationEditor.TranscriptionColumnPlaybackMenuOptions.Both",
					"Playback Both Recordings"),
			};
		}
	}
}
