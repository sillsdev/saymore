// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: ImageViewerViewModel.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using SayMore.Properties;
using SilUtils;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ImageViewerViewModel
	{
		public Image Image { get; private set; }
		public int[] ClickZoomPercentages { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ImageViewerViewModel(string pathToImage)
		{
			// Do this instead of using the Load method because Load keeps a lock on the file.
			using (var fs = new FileStream(pathToImage, FileMode.Open, FileAccess.Read))
			{
				Image = Image.FromStream(fs);
				fs.Close();
			}

			ClickZoomPercentages = PortableSettingsProvider.GetIntArrayFromString(
				Settings.Default.ImageViewerClickImageZoomPercentages);
		}

		/// ------------------------------------------------------------------------------------
		public int GetPercentOfImageSizeToFitSize(int maxPercent, int minPercent,
			int stepValue, Size areaToFit)
		{
			for (int pct = maxPercent; pct >= minPercent; pct -= stepValue)
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
