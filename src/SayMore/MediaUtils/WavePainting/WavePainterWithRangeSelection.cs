using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using SayMore.Transcription.Model;

namespace SayMore.Media
{
	public class WavePainterWithRangeSelection : WavePainterBasic
	{
		private readonly Dictionary<Color, TimeRange> _selectedRegions = new Dictionary<Color, TimeRange>();

		/// ------------------------------------------------------------------------------------
		public WavePainterWithRangeSelection(Control ctrl, WaveFileReader stream) :
			base(ctrl, stream)
		{
		}

		/// ------------------------------------------------------------------------------------
		public WavePainterWithRangeSelection(Control ctrl, IEnumerable<float> samples, TimeSpan totalTime) :
			base(ctrl, samples, totalTime)
		{
		}

		/// ------------------------------------------------------------------------------------
		public Color DefaultSelectionColor
		{
			// Other possible colors.
			//	Color.FromArgb(100, SystemColors.Highlight)
			//	Color.FromArgb(90, Color.Orange)
			get { return Color.FromArgb(50, Color.CornflowerBlue); }
		}

		/// ------------------------------------------------------------------------------------
		public Color[] GetColorsOfAreaEndingAtTime(TimeSpan time)
		{
			return (from kvp in _selectedRegions
					where kvp.Value.End == time
					select kvp.Key).ToArray();
		}

		/// ------------------------------------------------------------------------------------
		public Color[] GetColorsOfAreaStartingAtTime(TimeSpan time)
		{
			return (from kvp in _selectedRegions
					where kvp.Value.Start == time
					select kvp.Key).ToArray();
		}

		/// ------------------------------------------------------------------------------------
		public TimeRange DefaultSelectedRange
		{
			get
			{
				TimeRange timeRange;
				return (_selectedRegions.TryGetValue(DefaultSelectionColor, out timeRange) ? timeRange : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetSelectedRectangle(Color color)
		{
			TimeRange timeRange;
			if (!_selectedRegions.TryGetValue(color, out timeRange))
				return Rectangle.Empty;

			return GetFullRectangleForTimeRange(timeRange);
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeSpan selStartTime, TimeSpan selEndTime)
		{
			SetSelectionTimes(new TimeRange(selStartTime, selEndTime), DefaultSelectionColor);
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeRange newTimeRange)
		{
			SetSelectionTimes(newTimeRange, DefaultSelectionColor);
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeRange newTimeRange, Color color)
		{
			Debug.Assert(newTimeRange != null);

			if (color == Color.Empty)
				color = DefaultSelectionColor;

			TimeRange previousTimeRange;
			if (_selectedRegions.TryGetValue(color, out previousTimeRange) && newTimeRange == previousTimeRange)
				return;

			InvalidateControl(GetSelectedRectangle(color));
			_selectedRegions[color] = newTimeRange;
			InvalidateControl(GetSelectedRectangle(color));
		}

		/// ------------------------------------------------------------------------------------
		public override void Draw(PaintEventArgs e, Rectangle rc)
		{
			DrawSelectedRegions(e);
			base.Draw(e, rc);
			DrawCursor(e.Graphics, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawSelectedRegions(PaintEventArgs e)
		{
			if (_segmentBoundaries == null)
				return;

			foreach (var kvp in _selectedRegions)
			{
				var regionRect = GetFullRectangleForTimeRange(kvp.Value);
				if (regionRect == Rectangle.Empty || !e.ClipRectangle.IntersectsWith(regionRect))
					continue;

				using (var br = new SolidBrush(kvp.Key))
					e.Graphics.FillRectangle(br, regionRect);
			}
		}
	}
}
