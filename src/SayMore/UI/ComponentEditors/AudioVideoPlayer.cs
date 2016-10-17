using System;
using System.IO;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Media.MPlayer;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class AudioVideoPlayer : EditorBase
	{
		private TabPage _owningTab;
		private bool _playerPausedWhenTabChanged;
		private readonly MediaPlayerViewModel _mediaPlayerViewModel;
		private readonly MediaPlayer _mediaPlayer;

		/// ------------------------------------------------------------------------------------
		public AudioVideoPlayer(ComponentFile file, string imageKey) : base(file, null, imageKey)
		{
			Logger.WriteEvent("AudioVideoPlayer constructor. file = {0}; imageKey = {1}", file, imageKey);
			InitializeComponent();
			Name = "AudioVideoPlayer";

			_mediaPlayerViewModel = new MediaPlayerViewModel();

			_mediaPlayer = new MediaPlayer(_mediaPlayerViewModel);
			_mediaPlayer.Dock = DockStyle.Fill;
			Controls.Add(_mediaPlayer);

			FinishInitializing(file);
		}

		private void FinishInitializing(ComponentFile file)
		{
			SetComponentFile(file);

			// SP-831: tab is being localized before the file has been set in the base class
			HandleStringsLocalized();
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
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			// SP-831: tab is being localized before the file has been set in the base class
			if (_file == null) return;

			TabText = (_file.FileType.IsVideo ?
				LocalizationManager.GetString("CommonToMultipleViews.MediaPlayer.TabText-Video", "Video") :
				LocalizationManager.GetString("CommonToMultipleViews.MediaPlayer.TabText-Audio", "Audio"));

			base.HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);
			Name = "AudioVideoPlayer:" + Path.GetFileName(file.PathToAnnotatedFile);

			file.PreDeleteAction = (() => _mediaPlayerViewModel.Stop(true));
			file.PreFileCommandAction = (() => _mediaPlayerViewModel.Stop(true));
			file.PostFileCommandAction = (() => LoadAnnotationFile(file));
			file.PreRenameAction = (() => _mediaPlayerViewModel.Stop(true));
			file.PostRenameAction = (() => LoadAnnotationFile(file));

			_playerPausedWhenTabChanged = false;
			LoadAnnotationFile(file);

			if (Settings.Default.MediaPlayerVolume >= 0)
				_mediaPlayerViewModel.SetVolume(Settings.Default.MediaPlayerVolume);

			_mediaPlayerViewModel.VolumeChanged = delegate { Invoke((Action)HandleMediaPlayerVolumeChanged); };

			UseWaitCursor = false;
			Cursor = Cursors.Default;
		}

		/// ------------------------------------------------------------------------------------
		private void LoadAnnotationFile(ComponentFile file)
		{
			try
			{
				_mediaPlayerViewModel.LoadFile(file.PathToAnnotatedFile);
			}
			catch (Exception e)
			{
				{
					if (InvokeRequired)
						Invoke((Action)(() => ErrorReport.NotifyUserOfProblem(e.Message)));
					else
						ErrorReport.NotifyUserOfProblem(e.Message);
				};
			}
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
		//    SIL.Reporting.ErrorReport.NotifyUserOfProblem("Media error: " + e.pMediaObject);
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
