using System.ComponentModel;
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
		public AudioVideoPlayer(ComponentFile file)
		{
			InitializeComponent();
			Name = "AudioVideoPlayer";

#if !MONO
			InitializeWindowsMediaPlayer(file.PathToAnnotatedFile);
#else
			// Figure out how to host a media player in mono.
#endif
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_wmpPlayer != null)
				{
					_wmpPlayer.Ctlcontrols.stop();
					_wmpPlayer.Dispose();
				}

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
		private void InitializeWindowsMediaPlayer(string fileName)
		{
			_wmpPlayer = new AxWindowsMediaPlayer();
			((ISupportInitialize)(_wmpPlayer)).BeginInit();
			_wmpPlayer.Dock = DockStyle.Fill;
			_wmpPlayer.HandleDestroyed += HandlePlayerHandleDestroyed;
			_wmpPlayer.MediaError += HandleMediaError;
			Controls.Add(_wmpPlayer);
			((ISupportInitialize)(_wmpPlayer)).EndInit();

			_defaultPlayerVolume = _wmpPlayer.settings.volume;
			_wmpPlayer.settings.autoStart = false;
			_wmpPlayer.URL = fileName;

			if (Settings.Default.AudioVideoPlayerVolume >= 0)
				_wmpPlayer.settings.volume = Settings.Default.AudioVideoPlayerVolume;
		}

		/// ------------------------------------------------------------------------------------
		void HandlePlayerHandleDestroyed(object sender, System.EventArgs e)
		{
			if (_wmpPlayer.settings.volume != _defaultPlayerVolume)
				Settings.Default.AudioVideoPlayerVolume = _wmpPlayer.settings.volume;
		}

		/// ------------------------------------------------------------------------------------
		void HandleMediaError(object sender, _WMPOCXEvents_MediaErrorEvent e)
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
			if (!Settings.Default.PauseMediaPlayerWhenTabLoosesFocus)
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
