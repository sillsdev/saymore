using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
				Image = null;
				const double max = 2048;

				// Do this instead of using the Image.Load method because Load keeps a lock on the file.

				/***************************************************************************************
				 * SP-910: Out of memory error with larger images.
				 *
				 * The WinForms Bitmap apparently requires a contiguous block of free video memory to
				 * load an image. Larger images (number of pixels) consistently cause out of memory
				 * errors.  The WPF BitmapDecoder does not appear to have this limitation.
				 *
				 * This implementation uses the WPF BitmapDecoder to load the image and determine its
				 * size.  If either of the dimensions is greater than 2048, the image is shrunk before
				 * being converted to a WinForms Bitmap so it can be displayed.
				 *
				 ***************************************************************************************/

				using (var fs = new FileStream(pathToImage, FileMode.Open, FileAccess.Read))
				{
					var bd = BitmapDecoder.Create(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None);
					BitmapSource src = bd.Frames[0];

					// check the size
					if ((src.PixelHeight > max) || (src.PixelWidth > max))
					{
						// resize
						double scale;
						if (src.PixelHeight > src.PixelWidth)
							scale = max / src.PixelHeight;
						else
							scale = max / src.PixelWidth;

						TransformedBitmap tbm = new TransformedBitmap(src, new ScaleTransform(scale, scale));
						src = BitmapFrame.Create(tbm);
					}

					// convert from WPF bitmap to WinForms bitmap
					var be = new BmpBitmapEncoder();
					be.Frames.Add(src as BitmapFrame);

					using (var ms = new MemoryStream())
					{
						be.Save(ms);
						Image = new Bitmap(ms);
					}

					fs.Close();
				}
			}
			catch (OutOfMemoryException oomex)
			{
				var msg = LocalizationManager.GetString("CommonToMultipleViews.ImageViewer.OpeningPictureFileTooLargeMsg",
					"Could not open that picture. The image size is too large for SayMore to open.");
				SIL.Reporting.ErrorReport.NotifyUserOfProblem(oomex, msg);
			}
			catch (Exception e)
			{
				/***************************************************************************************
				 * While debugging, showing the Palaso error report dialog causes an error in the
				 * EnhancedPanel user control, which causes it to show the red X box.  No exception is
				 * being thrown.  The only way to get rid of the red X box is to restart the entire
				 * program.
				 ***************************************************************************************/
				var msg = LocalizationManager.GetString("CommonToMultipleViews.ImageViewer.OpeningPictureFileErrorMsg",
					"Could not open that picture.");
				SIL.Reporting.ErrorReport.NotifyUserOfProblem(e, msg);
			}
			finally
			{
				if (Image == null) Image = new Bitmap(100, 100);
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
