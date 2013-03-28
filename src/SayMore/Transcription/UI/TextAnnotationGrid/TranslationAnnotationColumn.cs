using System;
using System.Collections.Generic;
using System.Windows.Forms;
using L10NSharp;
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
			PlaybackType = (AudioRecordingType)Settings.Default.TranslationPlaybackType;
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
				Tag = AudioRecordingType.Source,
				Checked = (PlaybackType == AudioRecordingType.Source),
			};

			menuItem.Text = LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.Source",
				"Playback Source Recording", null, menuItem);

			yield return menuItem;

			menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = AudioRecordingType.Translation,
				Checked = (PlaybackType == AudioRecordingType.Translation),
			};

			menuItem.Text = LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.OralTranslation",
				"Playback Oral Translation Recording", null, menuItem);

			yield return menuItem;

			menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = (AudioRecordingType.Source | AudioRecordingType.Translation),
				Checked = (PlaybackType == (AudioRecordingType.Source | AudioRecordingType.Translation)),
			};

			menuItem.Text = LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.FreeTranslationColumnPlaybackMenuOptions.Both",
				"Playback Both Recordings", null, menuItem);

			yield return menuItem;
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetFont(System.Drawing.Font newFont)
		{
			base.SetFont(newFont);
			_grid.TranscriptionFont = newFont;
		}
	}
}
