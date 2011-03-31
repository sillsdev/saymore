using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SayMore.UI.LowLevelControls;

namespace SayMore.UI.MediaPlayer
{
	public partial class MediaPlayer : UserControl
	{
		private delegate void ReportPlayerOutputHandler(string data);
		private ReportPlayerOutputHandler _reportPlayerOutput;
		private MediaPlayerViewModel _viewModel;

		#region Construction and disposal
		/// ------------------------------------------------------------------------------------
		public MediaPlayer()
		{
			InitializeComponent();
			DoubleBuffered = true;

			_panelVideoSurface.BackgroundImageLayout = ImageLayout.Center;
			_panelVideoSurface.BackColor = _panelContainer.BackColor;
			_toolbarButtons.Renderer.RenderToolStripBorder += HandleButtonBarBorderPainting;
			CreateHandle();
			_reportPlayerOutput = HandleReportingPlayerOutput;
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
			_viewModel = viewModel;

			_viewModel.Initialize(_panelVideoSurface.Handle.ToInt32(),
				HandleOutputDataReceived, HandleErrorDataReceived);

			_viewModel.MediaQueued += HandleMediaQueued;
			_viewModel.PlaybackPaused += HandlePlaybackPausedResumed;
			_viewModel.PlaybackResumed += HandlePlaybackPausedResumed;
			_viewModel.PlaybackEnded += HandleMediaPlayEnded;
			_viewModel.PlaybackStarted += HandleMediaPlayStarted;
			_viewModel.PlaybackPositionChanged += HandlePlaybackPositionChanged;

			UpdateButtons();
			_volumePopup.VolumeLevel = _viewModel.Volume;
		}

		/// ------------------------------------------------------------------------------------
		private void SetupVolumePopup()
		{
			_panelContainer.Controls.Remove(_volumePopup);
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
			_reportPlayerOutput = null;
			_viewModel.ShutdownMPlayerProcess();
			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			AdjustVideoSurfaceSize();
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will adjust the inner panel (which is the one in which video is
		/// displayed) so it's size matches the aspect ration of the video and it's location
		/// is centered. MPlayer, by default, will keep the aspect ration for most video output
		/// devices. However, for the sake of Windows 7, we're using direct3d. For some
		/// reason, MPlayer doesn't keep the aspect ration when using that video output
		/// device. So, we'll do it ourselves.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustVideoSurfaceSize()
		{
			if (_viewModel == null || _viewModel.MediaInfo == null ||
				_viewModel.MediaInfo.PictureSize.Width == 0 || _viewModel.MediaInfo.PictureSize.Height == 0)
			{
				return;
			}

			var rc = Rectangle.Empty;
			var usableRatio = (float)_panelContainer.Width / _panelContainer.Height;
			var videoRatio = (float)_viewModel.MediaInfo.PictureSize.Width / _viewModel.MediaInfo.PictureSize.Height;

			if (usableRatio < videoRatio)
			{
				rc.Width = _panelContainer.ClientSize.Width;
				rc.Height = (int)(rc.Width / videoRatio);
			}
			else
			{
				rc.Height = _panelContainer.ClientSize.Height;
				rc.Width = (int)(rc.Height * videoRatio);
			}

			rc.X = (_panelContainer.ClientSize.Width - rc.Width) / 2;
			rc.Y = (_panelContainer.ClientSize.Height - rc.Height) / 2;

			_panelVideoSurface.Bounds = rc;
			_panelVideoSurface.BackgroundImage = _viewModel.MediaInfo.FullSizedThumbnail;
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

		/// ------------------------------------------------------------------------------------
		private void HandleSliderTimeBeforeUserMovingThumb(object sender, float newValue)
		{
			if (!_viewModel.IsPaused)
				HandleButtonPauseClick(null, null);

			_viewModel.PlaybackPositionChanged -= HandlePlaybackPositionChanged;
			_sliderTime.ValueChanged += HandleSliderTimeValueChanged;
		}

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
			rc.Height -= _panelContainer.Height;
			rc.Y = _panelContainer.Bottom;

			var clr1 = Color.DimGray;
			var clr2 = Color.FromArgb(50, 50, 50);

			using (var br = new LinearGradientBrush(rc, clr1, clr2, 90))
				e.Graphics.FillRectangle(br, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonBarBorderPainting(object sender, ToolStripRenderEventArgs e)
		{
			var rc = e.AffectedBounds;
			rc.Height = 2;
			rc.Y = _toolbarButtons.Height - 2;

			using (var br = new SolidBrush(Color.FromArgb(50, 50, 50)))
				e.Graphics.FillRectangle(br, rc);
		}

		#endregion

		#region View model's event handlers
		/// ------------------------------------------------------------------------------------
		void HandlePlaybackPausedResumed(object sender, EventArgs e)
		{
			UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaQueued(object sender, EventArgs e)
		{
			_sliderTime.SetValueWithoutEvent(0);
			_sliderTime.Maximum = _viewModel.GetTotalMediaDuration();
			_sliderTime.Enabled = true;

			AdjustVideoSurfaceSize();

			UpdateTimeDisplay(0);
			UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackPositionChanged(object sender, float position)
		{
			if (!_sliderTime.UserIsMoving)
			{
				UpdateTimeDisplay(position);
				_sliderTime.SetValueWithoutEvent(position);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlayEnded(object sender, EventArgs e)
		{
			//_sliderTime.Enabled = false;
			//UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlayStarted(object sender, EventArgs e)
		{
			_panelVideoSurface.BackgroundImage = null;
			UpdateButtons();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public void HandleReportingPlayerOutput(string data)
		{
			_viewModel.HandlePlayerOutput(data);
		}

		#region Delegate methods executed in worker thread
		/// ------------------------------------------------------------------------------------
		void HandleErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (_reportPlayerOutput != null && e.Data != null)
				Invoke(_reportPlayerOutput, e.Data);
		}

		/// ------------------------------------------------------------------------------------
		void HandleOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (_reportPlayerOutput != null && e.Data != null)
				Invoke(_reportPlayerOutput, e.Data);
		}

		#endregion
	}
}
