using System.Drawing;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.UI.ComponentEditors;

namespace SayMoreTests.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class ImageViewerViewModelTests
	{
		private TemporaryFolder _tmpfolder;
		ImageViewerViewModel _model;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_tmpfolder = new TemporaryFolder("projectTest");

			var image = new Bitmap(100, 200);
			var imageFile = _tmpfolder.Combine("junk.jpg");
			image.Save(imageFile, System.Drawing.Imaging.ImageFormat.Jpeg);

			_model = new ImageViewerViewModel(imageFile, new[] { 5, 10, 15, 20 });
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_tmpfolder.Dispose();
			_tmpfolder = null;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextClickPercent_MaxReference_ReturnsMin()
		{
			Assert.That(_model.GetNextClickPercent(20), Is.EqualTo(5));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextClickPercent_MinReference_ReturnsNext()
		{
			Assert.That(_model.GetNextClickPercent(5), Is.EqualTo(10));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextClickPercent_ArbitraryReference_ReturnsNext()
		{
			Assert.That(_model.GetNextClickPercent(13), Is.EqualTo(15));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetScaledSize_50Pct_ReturnsHalfSize()
		{
			Assert.That(_model.GetScaledSize(50), Is.EqualTo(new Size(50, 100)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetScaledSize_75Pct_ReturnsThreeFourthsSize()
		{
			Assert.That(_model.GetScaledSize(75), Is.EqualTo(new Size(75, 150)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetScaledSize_200Pct_ReturnsDoubleSize()
		{
			Assert.That(_model.GetScaledSize(200), Is.EqualTo(new Size(200, 400)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPercentOfImageSizeToFitSize_AreaSameSizeAsImage_Returns100pct()
		{
			var result = _model.GetPercentOfImageSizeToFitSize(1000, 0, new Size(100, 200));
			Assert.That(result, Is.EqualTo(100));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPercentOfImageSizeToFitSize_WhenMaxRestricted_Returns100pct()
		{
			var result = _model.GetPercentOfImageSizeToFitSize(100, 0, new Size(300, 500));
			Assert.That(result, Is.EqualTo(100));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPercentOfImageSizeToFitSize_WhenMaxVeryBig_Returns200pct()
		{
			var result = _model.GetPercentOfImageSizeToFitSize(5000, 0, new Size(200, 400));
			Assert.That(result, Is.EqualTo(200));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPercentOfImageSizeToFitSize_WhenAreaHasDiffProportionVert_Returns200pct()
		{
			var result = _model.GetPercentOfImageSizeToFitSize(500, 0, new Size(200, 1000));
			Assert.That(result, Is.EqualTo(200));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPercentOfImageSizeToFitSize_WhenAreaHasDiffProportionHorz_Returns200pct()
		{
			var result = _model.GetPercentOfImageSizeToFitSize(500, 0, new Size(500, 400));
			Assert.That(result, Is.EqualTo(200));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPercentOfImageSizeToFitSize_WhenAreaSmallerThanImage_Returns50pct()
		{
			var result = _model.GetPercentOfImageSizeToFitSize(500, 0, new Size(50, 100));
			Assert.That(result, Is.EqualTo(50));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPercentOfImageSizeToFitSize_WhenAreaHasDiffProportionVert_Returns50pct()
		{
			var result = _model.GetPercentOfImageSizeToFitSize(500, 0, new Size(100, 100));
			Assert.That(result, Is.EqualTo(50));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPercentOfImageSizeToFitSize_WhenAreaHasDiffProportionHorz_Returns50pct()
		{
			var result = _model.GetPercentOfImageSizeToFitSize(500, 0, new Size(50, 200));
			Assert.That(result, Is.EqualTo(50));
		}
	}
}
