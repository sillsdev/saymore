using System;
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
		protected override void HandlePlaybackTypeMenuItemClicked(object sender, EventArgs e)
		{
			base.HandlePlaybackTypeMenuItemClicked(sender, e);
			Settings.Default.TranslationPlaybackType = (int)PlaybackType;
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<ToolStripMenuItem> GetPlaybackOptionMenus()
		{
			var menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = OralAnnotationType.Original,
				Checked = (PlaybackType == OralAnnotationType.Original),
			};

			menuItem.Text = LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.Original",
				"Playback Original Recording", null, menuItem);

			yield return menuItem;

			menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = OralAnnotationType.Translation,
				Checked = (PlaybackType == OralAnnotationType.Translation),
			};

			menuItem.Text = LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.OralTranslation",
				"Playback Oral Translation Recording", null, menuItem);

			yield return menuItem;

			menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = (OralAnnotationType.Original | OralAnnotationType.Translation),
				Checked = (PlaybackType == (OralAnnotationType.Original | OralAnnotationType.Translation)),
			};

			menuItem.Text = LocalizationManager.GetString(
				"EventsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.Both",
				"Playback Both Recordings", null, menuItem);

			yield return menuItem;
		}
	}
}
