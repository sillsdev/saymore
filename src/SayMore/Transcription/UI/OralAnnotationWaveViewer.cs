using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SayMore.Media.Audio;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This class wraps a wave control displays an oral annotation audio file (i.e. multiple
	/// channels containing original recording, careful speech and oral translation).
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationWaveViewer : UserControl
	{
		public EventHandler PlaybackStopped = delegate { };
		public event WaveControlBasic.CursorTimeChangedHandler CursorTimeChanged = delegate { };

		public TimeSpan AudioLength { get; private set; }

		#region Construction and disposal
		/// ------------------------------------------------------------------------------------
		public OralAnnotationWaveViewer()
		{
			DoubleBuffered = true;

			InitializeComponent();
			InitializeWaveControl();

			Application.Idle += HandleInitialWaveDisplay;
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeWaveControl()
		{
			_waveControl.AllowDrawing = false;
			_waveControl.MouseClick += HandleWavePanelMouseClick;
			_waveControl.CursorTimeChanged += (c, time) => CursorTimeChanged(c, time);
			_waveControl.PlaybackStreamProvider = stream =>
			{
				AudioLength = stream.TotalTime;
				return new OralAnnotationPlaybackWaveStream(stream);
			};

			_waveControl.PlaybackStopped += delegate
			{
				if (InvokeRequired)
					Invoke(PlaybackStopped, this, EventArgs.Empty);
				else
					PlaybackStopped(this, EventArgs.Empty);
			};
		}

		/// ------------------------------------------------------------------------------------
		void HandleInitialWaveDisplay(object sender, EventArgs e)
		{
			Application.Idle -= HandleInitialWaveDisplay;
			_waveControl.AllowDrawing = true;
			ArrangeLabels();
			CursorTimeChanged(_waveControl, _waveControl.GetCursorTime());
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
			_waveControl.AllowDrawing = true;
			_waveControl.Initialize(filename);
		}

		/// ------------------------------------------------------------------------------------
		public void CloseAudioStream()
		{
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

			if (rects.Length == 4)
			{
				_labelOriginal.Top = rects[0].Bottom - (int)Math.Ceiling(_labelOriginal.Height / 2f);
				i = 2;
			}
			else
			{
				_labelOriginal.Top = (rects[0].Top + (int)Math.Ceiling(rects[0].Height / 2f)) -
					(int)Math.Ceiling(_labelOriginal.Height / 2f);
				i++;
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
			if (_waveControl.IsPlaying)
				_waveControl.Stop();

			_waveControl.SetCursor(e.X);
		}

		/// ------------------------------------------------------------------------------------
		public void Play()
		{
			_waveControl.Play(_waveControl.GetCursorTime());
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			if (_waveControl.IsPlaying)
				_waveControl.Stop();
		}
	}
}
