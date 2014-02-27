using System;
using System.IO;
using System.Linq;
using System.Drawing;
using L10NSharp;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public class ImageViewerViewModel : IDisposable
	{
		private Image _image;

		public Image Image {
			get { return _image;  }
			private set { _image = value; }
		}

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
			catch (OutOfMemoryException oomex)
			{
				var msg = LocalizationManager.GetString("CommonToMultipleViews.ImageViewer.OpeningPictureFileTooLargeMsg",
					"Could not open that picture. The image size is too large for SayMore to open.");

				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(oomex, msg);
				Image = new Bitmap(100, 100);
			}
			catch (Exception e)
			{
				var msg = LocalizationManager.GetString("CommonToMultipleViews.ImageViewer.OpeningPictureFileErrorMsg",
					"Could not open that picture.");

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

		public void Dispose()
		{
			if (_image != null)
			{
				_image.Dispose();
				_image = null;
			}
		}
	}
}
