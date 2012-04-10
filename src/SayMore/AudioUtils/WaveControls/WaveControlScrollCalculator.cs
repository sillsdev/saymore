using System;
using System.Diagnostics;
using SayMore.Transcription.Model;

namespace SayMore.Media
{
	/// ----------------------------------------------------------------------------------------
	public class WaveControlScrollCalculator
	{
		private readonly TimeRange _timeRange;
		private readonly int _leftMarginPercent;
		private readonly int _rightMarginPercent;
		private readonly WaveControlBasic _waveControl;
		private readonly bool _scrollToCenter;
		private int _cachedScrollPosAtWhichEntireRangeIsVisible = int.MinValue;
		private int _cachedWaveControlClientWidth;

		/// ------------------------------------------------------------------------------------
		public WaveControlScrollCalculator(WaveControlBasic waveControl)
			: this(waveControl, new TimeRange(TimeSpan.Zero, waveControl.WaveStream.TotalTime), false)
		{
		}

		/// ------------------------------------------------------------------------------------
		public WaveControlScrollCalculator(WaveControlBasic waveControl, TimeRange timeRange,
			bool scrollToCenter) : this(waveControl, timeRange, scrollToCenter, 5, 95)
		{
		}

		/// ------------------------------------------------------------------------------------
		public WaveControlScrollCalculator(WaveControlBasic waveControl, TimeRange timeRange,
			bool scrollToCenter, int leftMarginPct, int rightMarginPct)
		{
			Debug.Assert(!TimeRange.IsNullOrZeroLength(timeRange));

			_waveControl = waveControl;
			_cachedWaveControlClientWidth = _waveControl.ClientSize.Width;
			_timeRange = timeRange;
			_scrollToCenter = scrollToCenter;
			_leftMarginPercent = leftMarginPct;
			_rightMarginPercent = rightMarginPct;
		}

		/// ------------------------------------------------------------------------------------
		private bool HasRange
		{
			get { return _timeRange.DurationSeconds > 0; }
		}

		/// ------------------------------------------------------------------------------------
		public int ComputeTargetScrollOffset(int x)
		{
			int minX = (int)(_waveControl.ClientSize.Width * _leftMarginPercent / 100f);
			int maxX = (int)(_waveControl.ClientSize.Width * _rightMarginPercent / 100f);

			if (HasRange && ComputeCachedScrollPosAtWhichEntireRangeIsVisible(minX, maxX))
				return _cachedScrollPosAtWhichEntireRangeIsVisible;

			// Can't fit the whole range into the visible width. Just try to get x into view.
			if (x >= minX && x <= maxX)
				return -_waveControl.AutoScrollPosition.X; // Already in view.

			if (_scrollToCenter)
				return -_waveControl.AutoScrollPosition.X + x - (_waveControl.ClientRectangle.Width / 2);

			if (x < minX)
				return -_waveControl.AutoScrollPosition.X + x - minX;

			return Math.Max(-_waveControl.AutoScrollPosition.X + x - maxX,
				_waveControl.Painter.VirtualWidth - _waveControl.ClientRectangle.Width);
		}

		/// ------------------------------------------------------------------------------------
		private bool ComputeCachedScrollPosAtWhichEntireRangeIsVisible(int minX, int maxX)
		{
			if (_cachedWaveControlClientWidth != _waveControl.ClientSize.Width)
			{
				_cachedWaveControlClientWidth = _waveControl.ClientSize.Width;
				_cachedScrollPosAtWhichEntireRangeIsVisible = int.MinValue;
			}

			if (_cachedScrollPosAtWhichEntireRangeIsVisible == -_waveControl.AutoScrollPosition.X)
				return true;

			var startX = _waveControl.Painter.ConvertTimeToXCoordinate(_timeRange.Start);
			var endX = _waveControl.Painter.ConvertTimeToXCoordinate(_timeRange.End);

			if (endX - startX >= _waveControl.ClientSize.Width)
				return false;

			if (startX >= 0 && endX <= _waveControl.ClientRectangle.Right)
			{
				// Whole range is already visible. Don't scroll at all.
				_cachedScrollPosAtWhichEntireRangeIsVisible = -_waveControl.AutoScrollPosition.X;
			}
			else if (endX - startX <= maxX - minX)
			{
				// Whole range will fit within the margins, so scroll it into place

				if (startX < minX)
				{
					// The start of the range we want to see is scrolled beyond the left edge of the
					// viewable area.
					_cachedScrollPosAtWhichEntireRangeIsVisible = -_waveControl.AutoScrollPosition.X - (minX - startX);
				}
				else
				{
					// The end of the range we want to see is scrolled beyond the right edge of the
					// viewable area.
					_cachedScrollPosAtWhichEntireRangeIsVisible =
					-_waveControl.AutoScrollPosition.X + (endX - maxX);
				}
			}

			// It's too big to fit in the margins, but it fits in the window, so let it hang over the margins.
			else if (startX < 0)
			{
				// The start of the range we want to see is scrolled beyond the left edge of the
				// viewable area.
				_cachedScrollPosAtWhichEntireRangeIsVisible = -_waveControl.AutoScrollPosition.X - startX;
			}
			else
			{
				// The end of the range we want to see is scrolled beyond the right edge of the
				// viewable area.
				_cachedScrollPosAtWhichEntireRangeIsVisible =
				-_waveControl.AutoScrollPosition.X + (endX - _waveControl.ClientRectangle.Width);
			}

			return true;
		}
	}
}
