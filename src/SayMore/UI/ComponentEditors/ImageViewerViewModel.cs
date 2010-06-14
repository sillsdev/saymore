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
			// Do this instead of using the Load method because Load keeps a lock on the file.
			using (var fs = new FileStream(pathToImage, FileMode.Open, FileAccess.Read))
			{
				Image = Image.FromStream(fs);
				fs.Close();
			}

			ClickZoomPercentages = clickZoomPercentages;
		}

		/// ------------------------------------------------------------------------------------
		public int GetPercentOfImageSizeToFitSize(int maxPercent, int minPercent, Size areaToFit)
		{
			for (int pct = maxPercent; pct >= minPercent; pct -= 5)
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
			return new Size((int)(Image.Width * scaleFactor), (int)(Image.Height * scaleFactor));
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
