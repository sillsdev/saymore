using System;
using System.IO;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.MediaPlayer;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class AudioVideoPlayer : EditorBase
	{
		private TabPage _owningTab;
		private bool _playerPausedWhenTabChanged;
		private readonly MediaPlayerViewModel _mediaPlayerViewModel;
		private readonly MediaPlayer.MediaPlayer _mediaPlayer;

		/// ------------------------------------------------------------------------------------
		public AudioVideoPlayer(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "AudioVideoPlayer";

			_mediaPlayerViewModel = new MediaPlayerViewModel();
			_mediaPlayer = new MediaPlayer.MediaPlayer(_mediaPlayerViewModel);
			_mediaPlayer.Dock = DockStyle.Fill;
			Controls.Add(_mediaPlayer);

			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
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

			file.PreDeleteAction = (() => _mediaPlayerViewModel.Stop(true));
			file.PreFileCommandAction = (() => _mediaPlayerViewModel.Stop(true));
			file.PostFileCommandAction = (() => _mediaPlayerViewModel.LoadFile(file.PathToAnnotatedFile));
			file.PreRenameAction = (() => _mediaPlayerViewModel.Stop(true));
			file.PostRenameAction = (() => _mediaPlayerViewModel.LoadFile(file.PathToAnnotatedFile));

			_playerPausedWhenTabChanged = false;
			_mediaPlayerViewModel.LoadFile(file.PathToAnnotatedFile);

			if (Settings.Default.MediaPlayerVolume >= 0)
				_mediaPlayerViewModel.SetVolume(Settings.Default.MediaPlayerVolume);

			_mediaPlayerViewModel.VolumeChanged = delegate { Invoke((Action)HandleMediaPlayerVolumeChanged); };
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlayerVolumeChanged()
		{
			Settings.Default.MediaPlayerVolume = _mediaPlayerViewModel.Volume;
		}

		/// ------------------------------------------------------------------------------------
		public override void Deactivated()
		{
			_mediaPlayerViewModel.VolumeChanged = null;
			_mediaPlayerViewModel.ShutdownMPlayerProcess();
		}

		///// ------------------------------------------------------------------------------------
		//private static void HandleMediaError(object sender, _WMPOCXEvents_MediaErrorEvent e)
		//{
		//    Palaso.Reporting.ErrorReport.NotifyUserOfProblem("Media error: " + e.pMediaObject);
		//}

		/// ------------------------------------------------------------------------------------
		protected override void OnParentChanged(EventArgs e)
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
		void HandleOwningTabPageEnter(object sender, EventArgs e)
		{
			if (Settings.Default.PauseMediaPlayerWhenTabLoosesFocus && _playerPausedWhenTabChanged)
			{
				_mediaPlayerViewModel.Play();
				_playerPausedWhenTabChanged = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleOwningTabPageLeave(object sender, EventArgs e)
		{
			if (Settings.Default.PauseMediaPlayerWhenTabLoosesFocus && !_mediaPlayerViewModel.IsPaused)
			{
				_mediaPlayerViewModel.Pause();
				_playerPausedWhenTabChanged = true;
			}
		}
	}
}
