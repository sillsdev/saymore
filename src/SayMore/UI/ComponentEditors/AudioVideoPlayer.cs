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
		AxWindowsMediaPlayer _wmpPlayer;
		TabPage _owningTab;

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
			if (disposing && (components != null))
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
			Controls.Add(_wmpPlayer);
			((ISupportInitialize)(_wmpPlayer)).EndInit();
			_wmpPlayer.settings.autoStart = false;
			_wmpPlayer.URL = fileName;
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
