using System;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;
using SayMoreTests.UI.Utilities;

namespace SayMoreTests.Transcription.UI
{
	[TestFixture]
	public class SegmenterDlgBaseViewModelTests
	{
		private SegmenterDlgBaseViewModel _model;
		private string _tempAudioFile;
		private Mock<ComponentFile> _componentFile;
		private TimeTier _timeTier;
		private TextTier _textTier;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;

			_tempAudioFile = MPlayerMediaInfoTests.GetLongerTestAudioFile();
			_timeTier = new TimeTier(_tempAudioFile);
			_timeTier.AddSegment(0f, 10f);
			_timeTier.AddSegment(10f, 20f);
			_timeTier.AddSegment(20f, 30f);

			_textTier = new TextTier("Junk");
			_textTier.AddSegment("one");
			_textTier.AddSegment("two");
			_textTier.AddSegment("three");

			var annotationFile = new Mock<AnnotationComponentFile>();
			annotationFile.Setup(a => a.Tiers).Returns(new TierCollection { _timeTier, _textTier });

			_componentFile = new Mock<ComponentFile>();
			_componentFile.Setup(f => f.PathToAnnotatedFile).Returns(_tempAudioFile);
			_componentFile.Setup(f => f.GetAnnotationFile()).Returns(annotationFile.Object);

			_model = new SegmenterDlgBaseViewModel(_componentFile.Object);
			Directory.CreateDirectory(_model.OralAnnotationsFolder);

			Assert.IsNotNull(_model.OrigWaveStream);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			try { Directory.Delete(_model.OralAnnotationsFolder, true); }
			catch { }

			if (_model != null)
				_model.Dispose();

			try { File.Delete(_tempAudioFile); }
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Construction_CreatesOriginalAudioStream()
		{
			Assert.IsNotNull(_model.OrigWaveStream);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Construction_SetsTimeTier()
		{
			Assert.IsNotNull(_model.TimeTier);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Construction_CreatesTierCopies()
		{
			Assert.AreEqual(2, _model.Tiers.Count);
			Assert.AreNotSame(_timeTier, _model.Tiers[0]);
			Assert.AreNotSame(_textTier, _model.Tiers[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CopyOralAnnotationsToTempLocation_CopiesFilesAndReturnsPath()
		{
			File.OpenWrite(Path.Combine(_model.OralAnnotationsFolder, "one.wav")).Close();
			File.OpenWrite(Path.Combine(_model.OralAnnotationsFolder, "two.wav")).Close();

			var destinationPath = _model.CopyOralAnnotationsToTempLocation();

			Assert.IsTrue(File.Exists(Path.Combine(destinationPath, "one.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(destinationPath, "two.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CopyOralAnnotationsToTempLocation_CreatesDestinationEvenWhenNoAnnotationFilesToCopy()
		{
			Directory.Delete(_model.OralAnnotationsFolder, true);
			Assert.IsTrue(Directory.Exists(_model.CopyOralAnnotationsToTempLocation()));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SaveNewOralAnnoationsInPermanentLocation_CopiesFiles()
		{
			Directory.Delete(_model.OralAnnotationsFolder, true);
			var destinationPath = _model.CopyOralAnnotationsToTempLocation();

			File.OpenWrite(Path.Combine(destinationPath, "one.wav")).Close();
			File.OpenWrite(Path.Combine(destinationPath, "two.wav")).Close();

			Assert.IsFalse(File.Exists(Path.Combine(_model.OralAnnotationsFolder, "one.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(_model.OralAnnotationsFolder, "two.wav")));

			_model.SaveNewOralAnnoationsInPermanentLocation();

			Assert.IsTrue(File.Exists(Path.Combine(_model.OralAnnotationsFolder, "one.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_model.OralAnnotationsFolder, "two.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegmentEndBoundaries_ReturnsCorrectBoundaries()
		{
			var list = _model.GetSegmentEndBoundaries().ToArray();
			Assert.AreEqual(3, list.Length);
			Assert.AreEqual(TimeSpan.FromSeconds(10), list[0]);
			Assert.AreEqual(TimeSpan.FromSeconds(20), list[1]);
			Assert.AreEqual(TimeSpan.FromSeconds(30), list[2]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetEndOfLastSegment_ReturnsCorrectTime()
		{
			Assert.AreEqual(TimeSpan.FromSeconds(30), _model.GetEndOfLastSegment());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegmentCount_ReturnsCorrectSegmentCount()
		{
			Assert.AreEqual(3, _model.GetSegmentCount());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsSegmentLongEnough_ProposedEndIsNegative_ReturnsFalse()
		{
			Assert.IsFalse(_model.GetIsSegmentLongEnough(TimeSpan.FromSeconds(20).Negate()));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsSegmentLongEnough_ProposedEndTooCloseToPrecedingBoundary_ReturnsFalse()
		{
			var minSize = Settings.Default.MinimumAnnotationSegmentLengthInMilliseconds / 1000f;
			Assert.IsFalse(_model.GetIsSegmentLongEnough(TimeSpan.FromSeconds(20 + minSize / 2)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsSegmentLongEnough_ProposedEndYieldsMinAllowed_ReturnsTrue()
		{
			var minSize = Settings.Default.MinimumAnnotationSegmentLengthInMilliseconds / 1000f;
			Assert.IsTrue(_model.GetIsSegmentLongEnough(TimeSpan.FromSeconds(20 + minSize)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsSegmentLongEnough_ProposedEndInRange_ReturnsTrue()
		{
			Assert.IsTrue(_model.GetIsSegmentLongEnough(TimeSpan.FromSeconds(25)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SegmentBoundaryMoved_OldSameAsNew_ReturnsFalse()
		{
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.IsFalse(_model.SegmentBoundaryMoved(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3)));
			Assert.IsFalse(_model.SegmentBoundariesChanged);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SegmentBoundaryMoved_NewIsValid_ReturnsTrue()
		{
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.IsTrue(_model.SegmentBoundaryMoved(TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(24)));
			Assert.IsTrue(_model.SegmentBoundariesChanged);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteBoundary_BoundaryDoesNotExist_ReturnsFalse()
		{
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.IsFalse(_model.DeleteBoundary(TimeSpan.FromSeconds(20).Negate()));
			Assert.IsFalse(_model.DeleteBoundary(TimeSpan.FromSeconds(200)));
			Assert.IsFalse(_model.DeleteBoundary(TimeSpan.FromSeconds(24)));
			Assert.IsFalse(_model.SegmentBoundariesChanged);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteBoundary_BoundaryExists_ReturnsTrue()
		{
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.IsTrue(_model.DeleteBoundary(TimeSpan.FromSeconds(20)));
			Assert.IsTrue(_model.SegmentBoundariesChanged);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteBoundary_BoundaryExists_RemovesSegmentFromAllTiers()
		{
			var deletedTimeSeg = _model.Tiers[0].Segments[1];
			var deletedTextSeg = _model.Tiers[1].Segments[1];

			Assert.AreEqual(3, _model.Tiers[0].Segments.Count);
			Assert.AreEqual(3, _model.Tiers[1].Segments.Count);

			Assert.IsTrue(_model.DeleteBoundary(TimeSpan.FromSeconds(20)));

			Assert.AreEqual(2, _model.Tiers[0].Segments.Count);
			Assert.AreEqual(2, _model.Tiers[1].Segments.Count);
			Assert.IsFalse(_model.Tiers[0].Segments.Contains(deletedTimeSeg));
			Assert.IsFalse(_model.Tiers[1].Segments.Contains(deletedTextSeg));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanMoveBoundary_BoundaryNegative_ReturnsFalse()
		{
			Assert.IsFalse(_model.CanMoveBoundary(TimeSpan.FromSeconds(20).Negate(), 2000));
			Assert.IsFalse(_model.CanMoveBoundary(TimeSpan.FromSeconds(20).Negate(), -2000));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanMoveBoundary_MovedBoundaryWouldBePastEndOfAudio_ReturnsFalse()
		{
			var boundary = _model.OrigWaveStream.TotalTime - TimeSpan.FromSeconds(3);
			Assert.IsFalse(_model.CanMoveBoundary(boundary, 3500));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanMoveBoundary_MovedBoundaryIsValid_ReturnsTrue()
		{
			var boundary = _model.OrigWaveStream.TotalTime - TimeSpan.FromSeconds(3);
			Assert.IsTrue(_model.CanMoveBoundary(boundary, 2500));
			Assert.IsTrue(_model.CanMoveBoundary(boundary, -2500));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMissingTextSegmentsForTimeSegments_TiersHaveSameNumberSegments_DoesNothing()
		{
			Assert.AreEqual(_timeTier.Segments.Count, _textTier.Segments.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMissingTextSegmentsForTimeSegments_TextTierHasFewerSegments_AddsSegments()
		{
			_model.Tiers[1].Segments.RemoveAt(1);
			Assert.Greater(_model.Tiers[0].Segments.Count, _model.Tiers[1].Segments.Count);

			_model.CreateMissingTextSegmentsToMatchTimeSegmentCount();
			Assert.AreEqual(_model.Tiers[0].Segments.Count, _model.Tiers[1].Segments.Count);
		}
	}
}
