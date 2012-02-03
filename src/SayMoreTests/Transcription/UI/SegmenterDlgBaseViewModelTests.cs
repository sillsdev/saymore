using System;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using SayMore.Model.Files;
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
		private TimeOrderTier _timeTier;
		private TextTier _textTier;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_tempAudioFile = MPlayerMediaInfoTests.GetLongerTestAudioFile();
			_timeTier = new TimeOrderTier(_tempAudioFile);
			_timeTier.AddSegment(0f, 10f);
			_timeTier.AddSegment(10f, 20f);
			_timeTier.AddSegment(20f, 30f);

			_textTier = new TextTier("Junk");
			_textTier.AddSegment("1", "one");
			_textTier.AddSegment("2", "two");
			_textTier.AddSegment("3", "three");

			var annotationFile = new Mock<AnnotationComponentFile>();
			annotationFile.Setup(a => a.Tiers).Returns(new ITier[] { _textTier, _timeTier });

			_componentFile = new Mock<ComponentFile>();
			_componentFile.Setup(f => f.PathToAnnotatedFile).Returns(_tempAudioFile);
			_componentFile.Setup(f => f.GetAnnotationFile()).Returns(annotationFile.Object);

			_model = new SegmenterDlgBaseViewModel(_componentFile.Object);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			if (_model != null)
				_model.Dispose();

			File.Delete(_tempAudioFile);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Construction_CreatesOriginalAudioStream()
		{
			Assert.IsNotNull(_model.OrigWaveStream);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Construction_CreatesTierCopies()
		{
			Assert.AreEqual(2, _model.Tiers.Count);
			Assert.AreNotSame(_textTier, _model.Tiers[0]);
			Assert.AreNotSame(_timeTier, _model.Tiers[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InitializeSegments_PassNonTimeOrderTier_ReturnsEmptyBoundaryList()
		{
			var list = _model.InitializeSegments(new[] { new TextTier("junk") });
			Assert.False(list.Any());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InitializeSegments_PassTimeOrderTier_ReturnsCorrectBoundariesInList()
		{
			var list = _model.InitializeSegments(new[] { _timeTier }).ToArray();
			Assert.AreEqual(3, list.Length);

			Assert.AreEqual(TimeSpan.Zero, list[0].start);
			Assert.AreEqual(TimeSpan.FromSeconds(10), list[0].end);

			Assert.AreEqual(TimeSpan.FromSeconds(10), list[1].start);
			Assert.AreEqual(TimeSpan.FromSeconds(20), list[1].end);

			Assert.AreEqual(TimeSpan.FromSeconds(20), list[2].start);
			Assert.AreEqual(TimeSpan.FromSeconds(30), list[2].end);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTierForTimeSegments_ReturnsCorrectTier()
		{
			Assert.IsInstanceOf<TimeOrderTier>(_model.GetTierForTimeSegments());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegmentBoundaries_ReturnsCorrectBoundaries()
		{
			var list = _model.GetSegmentBoundaries().ToArray();
			Assert.AreEqual(3, list.Length);
			Assert.AreEqual(TimeSpan.FromSeconds(10), list[0]);
			Assert.AreEqual(TimeSpan.FromSeconds(20), list[1]);
			Assert.AreEqual(TimeSpan.FromSeconds(30), list[2]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegments_ReturnsCorrectBoundariesAsStrings()
		{
			var list = _model.GetSegments().ToArray();
			Assert.AreEqual(3, list.Length);
			Assert.AreEqual("10", list[0]);
			Assert.AreEqual("20", list[1]);
			Assert.AreEqual("30", list[2]);
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
		public void GetPreviousBoundary_PassFirstBoundary_ReturnsZero()
		{
			Assert.AreEqual(TimeSpan.Zero, _model.GetPreviousBoundary(TimeSpan.FromSeconds(10)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPreviousBoundary_PassNegativeReferenceBoundary_ReturnsZero()
		{
			Assert.AreEqual(TimeSpan.Zero, _model.GetPreviousBoundary(TimeSpan.FromSeconds(10).Negate()));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPreviousBoundary_PassNonExistentReferenceBoundary_ReturnsZero()
		{
			Assert.AreEqual(TimeSpan.Zero, _model.GetPreviousBoundary(TimeSpan.FromSeconds(45)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPreviousBoundary_PassExistingReferenceBoundary_ReturnsPreviousBoundary()
		{
			Assert.AreEqual(TimeSpan.FromSeconds(10), _model.GetPreviousBoundary(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextBoundary_PassLastBoundary_ReturnsZero()
		{
			Assert.AreEqual(TimeSpan.Zero, _model.GetNextBoundary(TimeSpan.FromSeconds(30)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextBoundary_PassNegativeReferenceBoundary_ReturnsZero()
		{
			Assert.AreEqual(TimeSpan.Zero, _model.GetNextBoundary(TimeSpan.FromSeconds(10).Negate()));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextBoundary_PassNonExistentReferenceBoundary_ReturnsZero()
		{
			Assert.AreEqual(TimeSpan.Zero, _model.GetNextBoundary(TimeSpan.FromSeconds(45)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNextBoundary_PassExistingReferenceBoundary_ReturnsNextBoundary()
		{
			Assert.AreEqual(TimeSpan.FromSeconds(30), _model.GetNextBoundary(TimeSpan.FromSeconds(20)));
		}

		// Write tests for these methods.
		//MoveExistingSegmentBoundary
		//SegmentBoundaryMoved
		//ChangeSegmentsEndBoundary
		//DeleteBoundary
	}
}
