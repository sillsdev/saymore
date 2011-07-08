using System;
using System.Drawing;
using System.Windows.Forms;
using Palaso.Media;
using SilTools.Controls;

namespace SayMore.UI.MediaPlayer
{
	public class VideoPanel : SilPanel
	{
		private readonly Panel _panelPlayingSurface;
		private MediaPlayerViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		public VideoPanel()
		{
			_panelPlayingSurface = new Panel();
			_panelPlayingSurface.BackgroundImageLayout = ImageLayout.Center;
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
			}

			_viewModel = viewModel;
			_viewModel.MediaQueued += HandleMediaQueued;
			_viewModel.PlaybackStarted += HandleMediaPlayStarted;

			AdjustVideoSurfaceSize();
		}

		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				base.BackColor = value;
				_panelPlayingSurface.BackColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			AdjustVideoSurfaceSize();
		}

		/// ------------------------------------------------------------------------------------
		public int VideoWindowHandle
		{
			get { return _panelPlayingSurface.Handle.ToInt32(); }
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
			var usableRatio = (float)ClientSize.Width / ClientSize.Height;
			var videoRatio = (float)_viewModel.MediaInfo.PictureSize.Width / _viewModel.MediaInfo.PictureSize.Height;

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
			_panelPlayingSurface.BackgroundImage = _viewModel.MediaInfo.FullSizedThumbnail;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaQueued(object sender, EventArgs e)
		{
			Invoke((Action)AdjustVideoSurfaceSize);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMediaPlayStarted(object sender, EventArgs e)
		{
			Invoke((Action)(() => _panelPlayingSurface.BackgroundImage = null));
		}
	}
}
