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
				Tag = OralAnnotationType.Source,
				Checked = (PlaybackType == OralAnnotationType.Source),
			};

			menuItem.Text = LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.Source",
				"Playback Source Recording", null, menuItem);

			yield return menuItem;

			menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = OralAnnotationType.Translation,
				Checked = (PlaybackType == OralAnnotationType.Translation),
			};

			menuItem.Text = LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.OralTranslation",
				"Playback Oral Translation Recording", null, menuItem);

			yield return menuItem;

			menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = (OralAnnotationType.Source | OralAnnotationType.Translation),
				Checked = (PlaybackType == (OralAnnotationType.Source | OralAnnotationType.Translation)),
			};

			menuItem.Text = LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.Both",
				"Playback Both Recordings", null, menuItem);

			yield return menuItem;
		}
	}
}
