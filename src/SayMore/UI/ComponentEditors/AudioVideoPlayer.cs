using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using AxWMPLib;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class AudioVideoPlayer : EditorBase
	{
		private AxWindowsMediaPlayer _wmpPlayer;
		private TabPage _owningTab;
		private int _defaultPlayerVolume;

		/// ------------------------------------------------------------------------------------
		public AudioVideoPlayer(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "AudioVideoPlayer";
			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_wmpPlayer != null && !_wmpPlayer.IsDisposed)
					_wmpPlayer.Dispose();

				if (_owningTab != null)
				{
					_owningTab.Enter -= HandleOwningTabPageEnter;
					_owningTab.Leave -= HandleOwningTabPageLeave;
				}

				if (components != null)
					components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);
			Name = "AudioVideoPlayer:" + Path.GetFileName(file.PathToAnnotatedFile);

#if !MONO
			InitializeWindowsMediaPlayer(file.PathToAnnotatedFile);
#else
			// Figure out how to host a media player in mono.
#endif
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeWindowsMediaPlayer(string mediaFile)
		{
			DisposeOfExistingPlayer();

			_wmpPlayer = new AxWindowsMediaPlayer();
			((ISupportInitialize)(_wmpPlayer)).BeginInit();
			_wmpPlayer.Dock = DockStyle.Fill;
			_wmpPlayer.HandleDestroyed += HandlePlayerHandleDestroyed;
			_wmpPlayer.MediaError += HandleMediaError;
			Controls.Add(_wmpPlayer);
			((ISupportInitialize)(_wmpPlayer)).EndInit();

			_defaultPlayerVolume = _wmpPlayer.settings.volume;
			_wmpPlayer.settings.autoStart = Settings.Default.AutoPlayMediaPlayerWhenSelectingMediaFile;
			_wmpPlayer.URL = mediaFile;

			if (Settings.Default.AudioVideoPlayerVolume >= 0)
				_wmpPlayer.settings.volume = Settings.Default.AudioVideoPlayerVolume;
		}

		/// ------------------------------------------------------------------------------------
		public override void GoDormant()
		{
			DisposeOfExistingPlayer();
		}

		/// ------------------------------------------------------------------------------------
		private void DisposeOfExistingPlayer()
		{
			if (_wmpPlayer != null)
			{
				_wmpPlayer.Ctlcontrols.stop();
				_wmpPlayer.close();
				Controls.Remove(_wmpPlayer);

				// Using the player's close method is supposed to release all the resources
				// used by the player for the current media file it has open, but I found
				// that there is a sufficient lag between a close and when all the resources
				// are finally released. E.g. after playing and closing, the folder that
				// contains the media file still has a lock on it several seconds later and
				// if the user tries to rename his session (which renames the folder)
				// during that time, an exception is thrown. Disposing works better.
				_wmpPlayer.Dispose();
				_wmpPlayer = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlayerHandleDestroyed(object sender, System.EventArgs e)
		{
			if (_wmpPlayer.settings.volume != _defaultPlayerVolume)
				Settings.Default.AudioVideoPlayerVolume = _wmpPlayer.settings.volume;
		}

		/// ------------------------------------------------------------------------------------
		private static void HandleMediaError(object sender, _WMPOCXEvents_MediaErrorEvent e)
		{
			Palaso.Reporting.ErrorReport.NotifyUserOfProblem("Media error: " + e.pMediaObject);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnParentChanged(System.EventArgs e)
		{
			base.OnParentChanged(e);

			if (_owningTab != null)
			{
				_owningTab.Enter -= HandleOwningTabPageEnter;
				_owningTab.Leave -= HandleOwningTabPageLeave;
			}

			_owningTab = Parent as TabPage;

			if (_owningTab != null)
			{
				_owningTab.Enter += HandleOwningTabPageEnter;
				_owningTab.Leave += HandleOwningTabPageLeave;
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleOwningTabPageEnter(object sender, System.EventArgs e)
		{
			if (!Settings.Default.PauseMediaPlayerWhenTabLoosesFocus || _wmpPlayer == null)
				return;

			if ((_wmpPlayer.Tag as string) == "PausedWhenLostFocus")
			{
				_wmpPlayer.Tag = null;
				_wmpPlayer.Ctlcontrols.play();
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleOwningTabPageLeave(object sender, System.EventArgs e)
		{
			if (!Settings.Default.PauseMediaPlayerWhenTabLoosesFocus)
				return;

			if (_wmpPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
			{
				_wmpPlayer.Tag = "PausedWhenLostFocus";
				_wmpPlayer.Ctlcontrols.pause();
			}
		}
	}
}
