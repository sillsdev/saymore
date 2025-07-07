using System;
using System.Drawing;
using System.Windows.Forms;
using SIL.Windows.Forms.Extensions;
using SIL.Windows.Forms.Widgets;
using static SIL.Windows.Forms.Extensions.ControlExtensions.ErrorHandlingAction;


namespace SayMore.Media.MPlayer
{
	public class VideoPanel : EnhancedPanel
	{
		private readonly Panel _panelPlayingSurface;
		private MediaPlayerViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		public VideoPanel()
		{
			_panelPlayingSurface = new Panel();
			BackColor = Color.Black;
			Controls.Add(_panelPlayingSurface);
		}

		/// ------------------------------------------------------------------------------------
		public void SetPlayerViewModel(MediaPlayerViewModel viewModel)
		{
			if (_viewModel != null)
			{
				_viewModel.MediaQueued -= HandleMediaQueued;
				_viewModel.PlaybackStarted -= HandleMediaPlayStarted;
				_viewModel.PlaybackEnded -= HandleMediaPlaybackEnded;
			}

			_viewModel = viewModel;
			_viewModel.VideoWindowHandle = VideoWindowHandle;
			_viewModel.MediaQueued += HandleMediaQueued;
			_viewModel.PlaybackStarted += HandleMediaPlayStarted;
			_viewModel.PlaybackEnded += HandleMediaPlaybackEnded;

			AdjustVideoSurfaceSize();
		}

		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get => base.BackColor;
			set
			{
				base.BackColor = value;
				_panelPlayingSurface.BackColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public int VideoWindowHandle => _panelPlayingSurface.Handle.ToInt32();

		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			AdjustVideoSurfaceSize();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This method will adjust the inner panel (which is the one in which video is
		/// displayed) so it's size matches the aspect ratio of the video and it's location
		/// is centered. MPlayer, by default, will keep the aspect ratio for most video output
		/// devices. However, for the sake of Windows 7, we're using direct3d. For some
		/// reason, MPlayer doesn't keep the aspect ratio when using that video output
		/// device. So, we'll do it ourselves.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void AdjustVideoSurfaceSize()
		{
			_panelPlayingSurface.BackgroundImage = null;

			if (_viewModel?.MediaInfo == null || !_viewModel.MediaInfo.IsVideo ||
			    _viewModel.MediaInfo.Video.PictureSize.Width == 0 || _viewModel.MediaInfo.Video.PictureSize.Height == 0)
			{
				return;
			}

			this.SetWindowRedraw(false);
			var rc = Rectangle.Empty;
			var usableRatio = (float)ClientSize.Width / ClientSize.Height;
			var videoRatio = _viewModel.MediaInfo.Video.AspectRatio;

			if (usableRatio < videoRatio)
			{
				rc.Width = ClientSize.Width;
				rc.Height = (int)(rc.Width / videoRatio);
			}
			else
			{
				rc.Height = ClientSize.Height;
				rc.Width = (int)(rc.Height * videoRatio);
			}

			rc.X = (ClientSize.Width - rc.Width) / 2;
			rc.Y = (ClientSize.Height - rc.Height) / 2;

			_panelPlayingSurface.Bounds = rc;

			if (!_viewModel.Loop && !_viewModel.HasPlaybackStarted)
				ShowVideoThumbnailNow();

			this.SetWindowRedraw(true);
		}

		/// ------------------------------------------------------------------------------------
		public void ShowVideoThumbnailNow()
		{
			if (_viewModel?.MediaInfo?.IsVideo != true)
				return;

			var img = _viewModel.GetVideoThumbnail();
			var rc = _panelPlayingSurface.ClientRectangle;

			if (img != null && (rc.Width > img.Width || rc.Height > img.Height))
				_panelPlayingSurface.BackgroundImageLayout = ImageLayout.Center;
			else
				_panelPlayingSurface.BackgroundImageLayout = ImageLayout.Stretch;

			_panelPlayingSurface.BackgroundImage = img;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaQueued(object sender, EventArgs e)
		{
			InvokeUIUpdate(AdjustVideoSurfaceSize, nameof(HandleMediaQueued));
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlayStarted(object sender, EventArgs e)
		{
			InvokeUIUpdate(() => _panelPlayingSurface.BackgroundImage = null,
				nameof(HandleMediaPlayStarted));
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlaybackEnded(object sender, bool endedBecauseEOF)
		{
			if (_viewModel.Loop && endedBecauseEOF)
				return;

			InvokeUIUpdate(ShowVideoThumbnailNow, nameof(HandleMediaPlaybackEnded));
		}

		private void InvokeUIUpdate(Action updateAction, string methodName)
		{
			this.SafeInvoke(updateAction, $"{GetType().Name}.{methodName}", IgnoreIfDisposed);
		}
	}
}
