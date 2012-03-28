using System.Drawing;
using SilTools;

namespace SayMore.Transcription.UI
{
	public static class StandardAudioButtons
	{
		public static Image PlayButtonImage { get; private set; }
		public static Image StopButtonImage { get; private set; }
		public static Image HotPlayButtonImage { get; private set; }
		public static Image HotStopButtonImage { get; private set; }

		/// ------------------------------------------------------------------------------------
		static StandardAudioButtons()
		{
			PlayButtonImage = CreateMediaControlImage(Properties.Resources.PlaySegment);
			StopButtonImage = CreateMediaControlImage(Properties.Resources.StopSegment);
			HotPlayButtonImage = PaintingHelper.MakeHotImage(PlayButtonImage);
			HotStopButtonImage = PaintingHelper.MakeHotImage(StopButtonImage);
		}

		/// ------------------------------------------------------------------------------------
		private static Image CreateMediaControlImage(Image img)
		{
			var bmp = new Bitmap(img.Width + 6, img.Height + 6);

			using (var br = new SolidBrush(Color.FromArgb(255, Color.White)))
			using (var g = Graphics.FromImage(bmp))
			{
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				g.FillEllipse(br, 0, 0, img.Width + 5, img.Width + 5);
				g.DrawImage(img, 3, 3, img.Width, img.Height);
			}

			return bmp;
		}
	}
}
