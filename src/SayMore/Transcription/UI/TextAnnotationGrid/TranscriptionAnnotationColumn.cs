using System;
using System.Collections.Generic;
using System.Windows.Forms;
using L10NSharp;
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
			PlaybackType = (AudioRecordingType)Settings.Default.TranscriptionPlaybackType;
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
				Tag = AudioRecordingType.Source,
				Checked = (PlaybackType == AudioRecordingType.Source),
			};

			menuItem.Text = LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.TranscriptionColumnPlaybackMenuOptions.Source",
				"Playback Source Recording", null, menuItem);

			yield return menuItem;

			menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = AudioRecordingType.Careful,
				Checked = (PlaybackType == AudioRecordingType.Careful),
			};

			menuItem.Text = LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.TranscriptionColumnPlaybackMenuOptions.CarefulSpeech",
				"Playback Careful Speech Recording", null, menuItem);

			yield return menuItem;

			menuItem = new ToolStripMenuItem(null, null, HandlePlaybackTypeMenuItemClicked)
			{
				Tag = (AudioRecordingType.Source | AudioRecordingType.Careful),
				Checked = (PlaybackType == (AudioRecordingType.Source | AudioRecordingType.Careful)),
			};

			menuItem.Text = LocalizationManager.GetString(
				"SessionsView.Transcription.TextAnnotationEditor.TranscriptionColumnPlaybackMenuOptions.Both",
				"Playback Both Recordings", null, menuItem);

			yield return menuItem;
		}

		/// ------------------------------------------------------------------------------------
		protected override void SetFont(System.Drawing.Font newFont)
		{
			base.SetFont(newFont);
			lock (this)
			{
				if (_grid != null)
					_grid.TranscriptionFont = newFont;
			}
		}
	}
}
