using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SayMore.AudioUtils
{
	public class WavePainterWithRangeSelection : WavePainterBasic
	{
		public TimeSpan SelectedRegionStartTime { get; private set; }
		public TimeSpan SelectedRegionEndTime { get; private set; }

		private Rectangle _previousSelectedRegionRectangle;

		/// ------------------------------------------------------------------------------------
		public WavePainterWithRangeSelection(Control ctrl, IEnumerable<float> samples, TimeSpan totalTime) :
			base(ctrl, samples, totalTime)
		{
		}

		/// ------------------------------------------------------------------------------------
		public void SetSelectionTimes(TimeSpan selStartTime, TimeSpan selEndTime)
		{
			if (_previousSelectedRegionRectangle != Rectangle.Empty)
				Control.Invalidate(_previousSelectedRegionRectangle);

			SelectedRegionStartTime = selStartTime;
			SelectedRegionEndTime = selEndTime;

			var endX = ConvertTimeToXCoordinate(selEndTime);
			var startX = ConvertTimeToXCoordinate(selStartTime);

			_previousSelectedRegionRectangle = new Rectangle(startX, 0,
				endX - startX, Control.ClientSize.Height);

			Control.Invalidate(_previousSelectedRegionRectangle);
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

				_previousSelectedRegionRectangle = regionRect;
			}
		}
	}
}
