using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;

namespace SayMore.Media
{
	public class WavePainterWithRangeSelection : WavePainterBasic
	{
		public TimeSpan SelectedRegionStartTime { get; private set; }
		public TimeSpan SelectedRegionEndTime { get; private set; }

		private Tuple<int, int> _previousSelectedRegion;

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
		private Rectangle PreviousSelectedRectangle
		{
			get
			{
				return (_previousSelectedRegion == null ? Rectangle.Empty:
					new Rectangle(_previousSelectedRegion.Item1, 0,
						_previousSelectedRegion.Item2, Control.ClientSize.Height));
			}
		}
		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeSpan selStartTime, TimeSpan selEndTime)
		{
			if (selStartTime == SelectedRegionStartTime && selEndTime == SelectedRegionEndTime)
				return;

			Control.Invalidate(PreviousSelectedRectangle);
			SelectedRegionStartTime = selStartTime;
			SelectedRegionEndTime = selEndTime;

			var endX = ConvertTimeToXCoordinate(selEndTime);
			var startX = ConvertTimeToXCoordinate(selStartTime);

			_previousSelectedRegion = new Tuple<int, int>(startX, endX - startX);
			Control.Invalidate(PreviousSelectedRectangle);
		}

		/// ------------------------------------------------------------------------------------
		public override void Draw(PaintEventArgs e, Rectangle rc)
		{
			base.Draw(e, rc);
			DrawSelectedRegion(e.Graphics);
			DrawCursor(e.Graphics, rc);
		}

		/// ------------------------------------------------------------------------------------
		private void DrawSelectedRegion(Graphics g)
		{
			if (_segmentBoundaries == null || SelectedRegionStartTime >= TimeSpan.Zero)
			{
				var regionRect = GetRectangleForTimeRange(SelectedRegionStartTime, SelectedRegionEndTime);
				if (regionRect == Rectangle.Empty)
					return;

//				using (var br = new SolidBrush(Color.FromArgb(100, SystemColors.Highlight)))
				using (var br = new SolidBrush(Color.FromArgb(90, Color.Orange)))
					g.FillRectangle(br, regionRect);

				_previousSelectedRegion = new Tuple<int,int>(regionRect.X, regionRect.Width);
			}
		}
	}
}
