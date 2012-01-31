using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SayMore.UI.MediaPlayer;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class wraps a wave control that's supposed to be used for a three channel audio
	/// transcription file (i.e. original recording, careful speech and oral translation).
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationWaveViewer : UserControl
	{
		public EventHandler PlaybackStopped;
		private readonly MediaPlayerViewModel _playerViewModel;

		#region Construction and disposal
		/// ------------------------------------------------------------------------------------
		public OralAnnotationWaveViewer()
		{
			DoubleBuffered = true;

			InitializeComponent();

			_labelOriginal.Font = new Font(SystemFonts.IconTitleFont, FontStyle.Bold);
			_labelCareful.Font = _labelOriginal.Font;
			_labelTranslation.Font = _labelOriginal.Font;

			_playerViewModel = new MediaPlayerViewModel();
			_playerViewModel.SetVolume(100);

			_playerViewModel.PlaybackEnded += delegate
			{
				if (PlaybackStopped != null)
				{
					if (InvokeRequired)
						Invoke(PlaybackStopped, this, EventArgs.Empty);
					else
						PlaybackStopped(this, EventArgs.Empty);
				}
			};

			_waveControl.AllowDrawing = false;
			_waveControl.MouseClick += HandleWavePanelMouseClick;
			Application.Idle += HandleInitialWaveDisplay;
		}

		/// ------------------------------------------------------------------------------------
		void HandleInitialWaveDisplay(object sender, EventArgs e)
		{
			Application.Idle -= HandleInitialWaveDisplay;
			_waveControl.AllowDrawing = true;
			ArrangeLabels();
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public void LoadAnnotationAudioFile(string filename)
		{
			_playerViewModel.LoadFile(filename);
			_waveControl.Initialize(filename);
		}

		/// ------------------------------------------------------------------------------------
		public void CloseAudioStream()
		{
			_playerViewModel.ShutdownMPlayerProcess();
			_waveControl.AutoScrollPosition = new Point(0, AutoScrollPosition.Y);
			_waveControl.SetCursor(0);
			_waveControl.CloseStream();
		}

		/// ------------------------------------------------------------------------------------
		public float ZoomPercentage
		{
			get { return _waveControl.ZoomPercentage; }
			set { _waveControl.ZoomPercentage = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			ArrangeLabels();
		}

		/// ------------------------------------------------------------------------------------
		private void ArrangeLabels()
		{
			var rects = _waveControl.GetChannelDisplayRectangles().ToArray();

			if (rects.Length == 0)
				return;

			int i = 0;

			if (rects.Length == 3)
			{
				_labelOriginal.Top = (rects[0].Top + (int)Math.Ceiling(rects[0].Height / 2f)) -
					(int)Math.Ceiling(_labelOriginal.Height / 2f);
			}
			else
			{
				_labelOriginal.Top = rects[0].Bottom - (int)Math.Ceiling(_labelOriginal.Height / 2f);
				i = 2;
			}

			_labelCareful.Top = (rects[i].Top + (int)Math.Ceiling(rects[i++].Height / 2f)) -
				(int)Math.Ceiling(_labelCareful.Height / 2f);

			_labelTranslation.Top = (rects[i].Top + (int)Math.Ceiling(rects[i].Height / 2f)) -
				(int)Math.Ceiling(_labelTranslation.Height / 2f);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLabelPanelPaint(object sender, PaintEventArgs e)
		{
			var dx = _panelLabels.Width - 1;

			using (var pen = new Pen(VisualStyleInformation.TextControlBorder))
				e.Graphics.DrawLine(pen, dx, 0, dx, _tableLayout.Height);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWavePanelMouseClick(object sender, MouseEventArgs e)
		{
			if (_playerViewModel.HasPlaybackStarted)
				_playerViewModel.Stop();

			_waveControl.SetCursor(e.X);
		}

		/// ------------------------------------------------------------------------------------
		public void Play()
		{
			//_playerViewModel.SetSpeed(Settings.Default.AnnotationEditorPlaybackSpeedIndex);
			_playerViewModel.PlaybackStartPosition = (float)_waveControl.GetCursorTime().TotalSeconds;
			_playerViewModel.PlaybackPositionChanged = pos => Invoke((Action<TimeSpan>)(_waveControl.SetCursor), TimeSpan.FromSeconds(pos));
			_playerViewModel.Play(true);
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			if (_playerViewModel.HasPlaybackStarted)
				_playerViewModel.Stop();
		}
	}
}
