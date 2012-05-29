using System;
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
		protected override void HandlePlaybackTypeMenuItemClicked(object sender, EventArgs e)
		{
			base.HandlePlaybackTypeMenuItemClicked(sender, e);
			Settings.Default.TranscriptionPlaybackType = (int)PlaybackType;
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<ToolStripMenuItem> GetPlaybackOptionMenus()
		{
			var menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = OralAnnotationType.Source,
				Checked = (PlaybackType == OralAnnotationType.Source),
			};

			menuItem.Text = LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.TranscriptionColumnPlaybackMenuOptions.Source",
				"Playback Source Recording", null, menuItem);

			yield return menuItem;

			menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = OralAnnotationType.Careful,
				Checked = (PlaybackType == OralAnnotationType.Careful),
			};

			menuItem.Text = LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.TranscriptionColumnPlaybackMenuOptions.CarefulSpeech",
				"Playback Careful Speech Recording", null, menuItem);

			yield return menuItem;

			menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = (OralAnnotationType.Source | OralAnnotationType.Careful),
				Checked = (PlaybackType == (OralAnnotationType.Source | OralAnnotationType.Careful)),
			};

			menuItem.Text = LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.TranscriptionColumnPlaybackMenuOptions.Both",
				"Playback Both Recordings", null, menuItem);

			yield return menuItem;
		}
	}
}
