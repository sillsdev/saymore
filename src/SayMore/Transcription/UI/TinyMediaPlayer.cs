using System;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Transcription.Model;
using SayMore.UI.MediaPlayer;
using SilTools;

namespace SayMore.Transcription.UI
{
	public partial class TinyMediaPlayer : UserControl
	{
		protected bool _mediaFileNeedsLoading = true;
		protected MediaPlayerViewModel _model;
		protected bool _showButtons;

		/// ------------------------------------------------------------------------------------
		public TinyMediaPlayer()
		{
			InitializeComponent();
			BackColor = SystemColors.Window;
			ForeColor = SystemColors.WindowText;
			SetStyle(ControlStyles.Selectable, false);
			DoubleBuffered = true;
			Font = FontHelper.MakeFont(SystemFonts.IconTitleFont, 7f);

			_buttonPlay.Click += delegate { Play(); };
			_buttonStop.Click += delegate { Stop(); };

			_model = new MediaPlayerViewModel();
			_model.SetVolume(100);
			_model.PlaybackEnded = () => Invoke((Action)HandlePlaybackStopped);
			_model.PlaybackPositionChanged = (pos) => Invoke((Action<Rectangle>)(Invalidate), WaveFormRectangle);

			ShowButtons = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				if (_model != null)
					_model.ShutdownMPlayerProcess();

				_model = null;
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public IMediaSegment Segment { get; private set; }

		/// ------------------------------------------------------------------------------------
		public void LoadSegment(IMediaSegment segment)
		{
			if (segment == Segment)
				return;

			Segment = segment;
			_mediaFileNeedsLoading = true;
			Invalidate(WaveFormRectangle);
		}

		/// ------------------------------------------------------------------------------------
		protected Rectangle WaveFormRectangle
		{
			get
			{
				var rc = ClientRectangle;
				rc.Width -= (_buttonPlay.Width + Padding.Right);
				return rc;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool ShowButtons
		{
			get { return _showButtons; }
			set
			{
				_showButtons = value;
				UpdateButtons();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Play()
		{
			if (_model.HasPlaybackStarted)
			{
				Stop();
				return;
			}

			if (_mediaFileNeedsLoading)
			{
				_model.LoadFile(Segment.MediaFile, Segment.MediaStart, Segment.MediaLength);
				_mediaFileNeedsLoading = false;
			}

			_model.Play();
			UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			_model.Stop();
			UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		protected void UpdateButtons()
		{
			_buttonPlay.Visible = (ShowButtons && _model.IsPlayButtonVisible);
			_buttonStop.Visible = (ShowButtons && !_model.IsPlayButtonVisible);
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackStopped()
		{
			Invalidate(WaveFormRectangle);
			UpdateButtons();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			Parent.Focus();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var rc = WaveFormRectangle;

			if (_mediaFileNeedsLoading)
				Draw(e.Graphics, Segment.MediaStart, Segment.MediaLength, rc, ForeColor, BackColor);
			else
			{
				Draw(e.Graphics, _model.GetTimeDisplay(), rc, ForeColor, BackColor);

				var pixelsPerSec = rc.Width / Segment.MediaLength;
				var dx = (int)Math.Round(pixelsPerSec * (_model.CurrentPosition - Segment.MediaStart),
					MidpointRounding.AwayFromZero);

				if (dx > 0)
				{
					using (var pen = new Pen(ForeColor))
						e.Graphics.DrawLine(pen, rc.Left + dx, rc.Top, rc.Left + dx, rc.Bottom);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Draw(Graphics g, float startPosition, float length, Rectangle rc,
			Color foreColor, Color backColor)
		{
			var text = _model.GetTimeDisplay(startPosition, startPosition + length);
			Draw(g, text, rc, foreColor, backColor);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void Draw(Graphics g, string text, Rectangle rc, Color foreColor, Color backColor)
		{
			using (var br = new SolidBrush(backColor))
				g.FillRectangle(br, rc);

			const TextFormatFlags flags = TextFormatFlags.WordEllipsis |
				TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.Left;

			TextRenderer.DrawText(g, text, Font, rc, foreColor, backColor, flags);
		}
	}
}
