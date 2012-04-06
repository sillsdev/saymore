using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

			return GetSelectedRectangle(timeRange);
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetSelectedRectangle(TimeRange timeRange)
		{
			var endX = ConvertTimeToXCoordinate(timeRange.End);
			var startX = ConvertTimeToXCoordinate(timeRange.Start);
			return new Rectangle(startX, 0, endX - startX, Control.ClientSize.Height);
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
			base.Draw(e, rc);
			DrawSelectedRegions(e);
			DrawCursor(e.Graphics, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawSelectedRegions(PaintEventArgs e)
		{
			if (_segmentBoundaries == null)
				return;

			foreach (var kvp in _selectedRegions)
			{
				var regionRect = GetSelectedRectangle(kvp.Value);
				if (regionRect == Rectangle.Empty || !e.ClipRectangle.IntersectsWith(regionRect))
					continue;

				using (var br = new SolidBrush(kvp.Key))
					e.Graphics.FillRectangle(br, regionRect);
			}
		}
	}
}
