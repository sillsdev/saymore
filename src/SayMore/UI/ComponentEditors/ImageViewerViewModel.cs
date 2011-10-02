using System;
using System.IO;
using System.Linq;
using System.Drawing;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public class ImageViewerViewModel
	{
		public Image Image { get; private set; }
		public int[] ClickZoomPercentages { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ImageViewerViewModel(string pathToImage, int[] clickZoomPercentages)
		{
			try
			{
				// Do this instead of using the Load method because Load keeps a lock on the file.
				using (var fs = new FileStream(pathToImage, FileMode.Open, FileAccess.Read))
				{
					Image = Image.FromStream(fs);
					fs.Close();
				}

			}
			catch (Exception e)
			{
				var msg = Program.GetString("UI.ImageViewer.OpeningPictureFileErrorMsg", "Could not open that picture");
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e, msg);
				Image = new Bitmap(100, 100);
			}

			ClickZoomPercentages = clickZoomPercentages;
		}

		/// ------------------------------------------------------------------------------------
		public int GetPercentOfImageSizeToFitSize(int maxPercent, int minPercent, Size areaToFit)
		{
			for (int pct = maxPercent; pct >= minPercent; pct--)
			{
				var sz = GetScaledSize(pct);
				if (sz.Width <= areaToFit.Width && sz.Height <= areaToFit.Height)
					return pct;
			}

			return minPercent;
		}

		/// ------------------------------------------------------------------------------------
		public Size GetScaledSize(int percent)
		{
			var scaleFactor = percent / 100f;
			return new Size((int)Math.Ceiling(Image.Width * scaleFactor),
				(int)Math.Ceiling(Image.Height * scaleFactor));
		}

		/// ------------------------------------------------------------------------------------
		public int GetNextClickPercent(int referencePercent)
		{
			return (referencePercent >= ClickZoomPercentages.Max() ?
				ClickZoomPercentages.Min() :
				ClickZoomPercentages.First(x => x > referencePercent));
		}
	}
}
