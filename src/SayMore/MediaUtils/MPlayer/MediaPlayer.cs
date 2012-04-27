using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SayMore.Utilities.LowLevelControls;
using SilTools;

namespace SayMore.Media.MPlayer
{
	public partial class MediaPlayer : UserControl
	{
		private MediaPlayerViewModel _viewModel;

		#region Construction and disposal
		/// ------------------------------------------------------------------------------------
		public MediaPlayer()
		{
			InitializeComponent();
			DoubleBuffered = true;

			_toolbarButtons.Renderer = new NoToolStripBorderRenderer();
			CreateHandle();
			SetupVolumePopup();
		}

		/// ------------------------------------------------------------------------------------
		public MediaPlayer(MediaPlayerViewModel viewModel) : this()
		{
			SetViewModel(viewModel);
		}

		/// ------------------------------------------------------------------------------------
		public void SetViewModel(MediaPlayerViewModel viewModel)
		{
			if (_viewModel != null)
			{
				_viewModel.MediaQueued -= HandleMediaQueued;
				_viewModel.PlaybackStarted -= HandleMediaPlayStarted;
			}

			_viewModel = viewModel;
			_viewModel.MediaQueued += HandleMediaQueued;
			_viewModel.PlaybackStarted += HandleMediaPlayStarted;
			_viewModel.PlaybackPaused = delegate { Invoke((Action)HandlePlaybackPausedResumed); };
			_viewModel.PlaybackResumed = delegate { Invoke((Action)HandlePlaybackPausedResumed); };
			_viewModel.PlaybackPositionChanged = delegate(float pos) { Invoke((Action<float>)(HandlePlaybackPositionChanged), pos); };

			UpdateButtons();
			_volumePopup.VolumeLevel = _viewModel.Volume;

			_videoPanel.SetPlayerViewModel(_viewModel);
		}

		/// ------------------------------------------------------------------------------------
		private void SetupVolumePopup()
		{
			_videoPanel.Controls.Remove(_volumePopup);
			_volumePopup.VolumeLevel = 100;
			_volumePopup.Width = _buttonVolume.Width;

			var host = new ToolStripControlHost(_volumePopup);
			host.Padding = Padding.Empty;
			host.Margin = Padding.Empty;
			host.AutoSize = false;
			host.Size = _volumePopup.Size;

			var dropDown = new ToolStripDropDown();
			dropDown.Padding = Padding.Empty;
			dropDown.AutoSize = false;
			dropDown.LayoutStyle = ToolStripLayoutStyle.Table;
			dropDown.Size = _volumePopup.Size;
			dropDown.Items.Add(host);
			dropDown.Opened += HandleVolumeDropDownOpened;

			_buttonVolume.DropDown = dropDown;
			_volumePopup.OwningDropDown = dropDown;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the extender is currently in design mode.
		/// I have had some problems with the base class' DesignMode property being true
		/// when in design mode. I'm not sure why, but adding a couple more checks fixes the
		/// problem.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private new bool DesignMode
		{
			get
			{
				return (base.DesignMode || GetService(typeof(IDesignerHost)) != null) ||
					(LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			//_reportPlayerOutput = null;
			_viewModel.ShutdownMPlayerProcess();
			base.OnHandleDestroyed(e);
		}

		#endregion

		#region Display update methods
		/// ------------------------------------------------------------------------------------
		private void UpdateButtons()
		{
			if (DesignMode)
				return;

			_buttonPause.Visible = _viewModel.IsPauseButtonVisible;
			_buttonPlay.Visible = _viewModel.IsPlayButtonVisible;
			_buttonStop.Enabled = _viewModel.IsStopEnabled;
			_buttonVolume.Image = (_viewModel.IsVolumeMuted ?
				Properties.Resources.MuteVolume : Properties.Resources.Volume);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateTimeDisplay(float position)
		{
			_labelTime.Text = _viewModel.GetTimeDisplay(position);
		}

		#endregion

		#region GUI Control event handlers
		/// ------------------------------------------------------------------------------------
		private void HandleButtonPlayClick(object sender, EventArgs e)
		{
			if (ModifierKeys == (Keys.Shift | Keys.Control | Keys.Alt))
				_viewModel.ShowPlayerOutputLog(true);
			else
			{
				_viewModel.Play();
				UpdateButtons();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonPauseClick(object sender, EventArgs e)
		{
			_viewModel.Pause();
			UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonStopClick(object sender, EventArgs e)
		{
			_viewModel.Stop();
			UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleVolumeButtonClick(object sender, EventArgs e)
		{
			_viewModel.ToggleVolumeMute();
			UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		void HandleVolumePopupValueChanged(object sender, EventArgs e)
		{
			if (_viewModel == null)
				return;

			if (_viewModel.IsVolumeMuted)
			{
				_viewModel.ToggleVolumeMute();
				UpdateButtons();
			}

			_viewModel.SetVolume(((VolumePopup)sender).VolumeLevel);
		}

		///// ------------------------------------------------------------------------------------
		//private void HandleSliderTimeBeforeUserMovingThumb(object sender, float newValue)
		//{
		//    if (!_viewModel.IsPaused)
		//        HandleButtonPauseClick(null, null);

		//    _viewModel.PlaybackPositionChanged -= HandlePlaybackPositionChanged;
		//    _sliderTime.ValueChanged += HandleSliderTimeValueChanged;
		//}

		/// ------------------------------------------------------------------------------------
		private void HandleSliderTimeAfterUserMovingThumb(object sender, EventArgs e)
		{
			_sliderTime.ValueChanged -= HandleSliderTimeValueChanged;
			_viewModel.Seek(_sliderTime.Value);
			_viewModel.PlaybackPositionChanged += HandlePlaybackPositionChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// I found that when the VolumePopup is hosted in a ToolStripControlHost, then the
		/// tooltip doesn't get displayed when the mouse is over the thumb until the thumb
		/// begins to move. Setting focus to the VolumePopup does the trick.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		void HandleVolumeDropDownOpened(object sender, EventArgs e)
		{
			_volumePopup.SetVolumeLevelWithoutEvents(_viewModel.Volume);
			_volumePopup.Focus();
		}

		/// ------------------------------------------------------------------------------------
		void HandleSliderTimeValueChanged(object sender, EventArgs e)
		{
			UpdateTimeDisplay(_sliderTime.Value);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			var rc = ClientRectangle;
			rc.Height -= _videoPanel.Height;
			rc.Y = _videoPanel.Bottom;

			var clr1 = Color.DimGray;
			var clr2 = Color.FromArgb(50, 50, 50);

			using (var br = new LinearGradientBrush(rc, clr1, clr2, 90))
				e.Graphics.FillRectangle(br, rc);
		}

		#endregion

		#region View model's event handlers
		/// ------------------------------------------------------------------------------------
		void HandlePlaybackPausedResumed()
		{
			UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaQueued(object sender, EventArgs e)
		{
			Invoke((Action)(() => _sliderTime.SetValueWithoutEvent(0)));
			Invoke((Action)(() => _sliderTime.Maximum = _viewModel.GetTotalMediaDuration()));
			Invoke((Action)(() => _sliderTime.Enabled = true));
			Invoke((Action)(() => UpdateTimeDisplay(0)));
			Invoke((Action)UpdateButtons);
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackPositionChanged(float position)
		{
			if (!_sliderTime.UserIsMoving)
			{
				UpdateTimeDisplay(position);
				_sliderTime.SetValueWithoutEvent(position);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlayStarted(object sender, EventArgs e)
		{
			Invoke((Action)UpdateButtons);
		}

		#endregion
	}
}
