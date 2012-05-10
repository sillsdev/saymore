using System;
using System.IO;
using System.Threading;
using Moq;
using NUnit.Framework;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;
using SayMoreTests.Model.Files;

namespace SayMoreTests.Transcription.UI
{
	[TestFixture]
	public class OralAnnotationRecorderDlgViewModelTests
	{
		private OralAnnotationRecorderDlgViewModel _model;
		private string _tempAudioFile;
		private Mock<ComponentFile> _componentFile;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_tempAudioFile = MediaFileInfoTests.GetLongerTestAudioFile();
			var tier = new TimeTier(_tempAudioFile);
			tier.AddSegment(0f, 5f);
			tier.AddSegment(5f, 10f);
			tier.AddSegment(15f, 20f);
			tier.AddSegment(25f, 30f);

			var annotationFile = new Mock<AnnotationComponentFile>();
			annotationFile.Setup(a => a.Tiers).Returns(new TierCollection { tier });

			_componentFile = new Mock<ComponentFile>();
			_componentFile.Setup(f => f.PathToAnnotatedFile).Returns(_tempAudioFile);
			_componentFile.Setup(f => f.GetAnnotationFile()).Returns(annotationFile.Object);

			_model = OralAnnotationRecorderDlgViewModel.Create(_componentFile.Object, OralAnnotationType.Careful);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			if (Directory.Exists(_model.OralAnnotationsFolder))
				Directory.Delete(_model.OralAnnotationsFolder, true);

			if (_model != null)
				_model.Dispose();

			File.Delete(_tempAudioFile);
		}

		#region Helper methods
		/// ------------------------------------------------------------------------------------
		private void CreateModelAndAnnotationFileForSegment(float start, float end)
		{
			CreateModelAndAnnotationFileForType(OralAnnotationType.Careful,
				OralAnnotationType.Careful, start, end);
		}

		/// ------------------------------------------------------------------------------------
		private void CreateModelAndAnnotationFileForType(OralAnnotationType modelType,
			OralAnnotationType fileType)
		{
			CreateModelAndAnnotationFileForType(modelType, fileType, 25f, 30f);
// REVIEW			_model.SelectSegmentFromTime(TimeSpan.FromSeconds(30f));
		}

		/// ------------------------------------------------------------------------------------
		private void CreateModelAndAnnotationFileForType(OralAnnotationType modelType,
			OralAnnotationType fileType, float start, float end)
		{
			if (Directory.Exists(_model.OralAnnotationsFolder))
				Directory.Delete(_model.OralAnnotationsFolder, true);

			_model.Dispose();
			_model = OralAnnotationRecorderDlgViewModel.Create(_componentFile.Object, modelType);

			Directory.CreateDirectory(_model.OralAnnotationsFolder);

			if (fileType == OralAnnotationType.Careful)
			{
				File.OpenWrite(Path.Combine(_model.OralAnnotationsFolder,
					TimeTier.ComputeFileNameForCarefulSpeechSegment(start, end))).Close();
			}
			else
			{
				File.OpenWrite(Path.Combine(_model.OralAnnotationsFolder,
					TimeTier.ComputeFileNameForOralTranslationSegment(start, end))).Close();
			}
		}

		#endregion

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetIsSegmentLongEnough_ProposedEndIsNegativeButCurrentSegment_ReturnsTrue()
		//{
		//    // The only difference between GetIsSegmentLongEnough in the
		//    // OralAnnotationRecorderViewModel and it's base class version occurs when the
		//    // OralAnnotationRecorderViewModel's CurrentSegment is not null. Therefore,
		//    // that condition is all that's tested in this fixture since there are already
		//    // tests for the base class version.

		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(10f).Negate());
		//    Assert.IsFalse(_model.GetIsSegmentLongEnough(TimeSpan.FromSeconds(1).Negate()));
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(10f));
		//    Assert.IsTrue(_model.GetIsSegmentLongEnough(TimeSpan.FromSeconds(1).Negate()));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetIsSegmentLongEnough_ProposedEndTooCloseToPrecedingBoundary_ReturnsTrue()
		//{
		//    // The only difference between GetIsSegmentLongEnough in the
		//    // OralAnnotationRecorderViewModel and it's base class version occurs when the
		//    // OralAnnotationRecorderViewModel's CurrentSegment is not null. Therefore,
		//    // that condition is all that's tested in this fixture since there are already
		//    // tests for the base class version.

		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(10f).Negate());
		//    Assert.IsFalse(_model.GetIsSegmentLongEnough(TimeSpan.FromSeconds(1).Negate()));
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(10f));
		//    Assert.IsTrue(_model.GetIsSegmentLongEnough(TimeSpan.FromSeconds(1).Negate()));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetIsSegmentLongEnough_ProposedEndYieldsMinAllowed_ReturnsTrue()
		//{
		//    // The only difference between GetIsSegmentLongEnough in the
		//    // OralAnnotationRecorderViewModel and it's base class version occurs when the
		//    // OralAnnotationRecorderViewModel's CurrentSegment is not null. Therefore,
		//    // that condition is all that's tested in this fixture since there are already
		//    // tests for the base class version.

		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(10f));
		//    Assert.IsTrue(_model.GetIsSegmentLongEnough(TimeSpan.FromSeconds(10000)));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetIsSegmentLongEnough_ProposedEndInRange_ReturnsTrue()
		//{
		//    // The only difference between GetIsSegmentLongEnough in the
		//    // OralAnnotationRecorderViewModel and it's base class version occurs when the
		//    // OralAnnotationRecorderViewModel's CurrentSegment is not null. Therefore,
		//    // that condition is all that's tested in this fixture since there are already
		//    // tests for the base class version.

		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(10f));
		//    Assert.IsTrue(_model.GetIsSegmentLongEnough(TimeSpan.FromSeconds(10000)));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetStartOfCurrentSegment_CurrentSegmentIsNull_ReturnsEndOfLastSegment()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(1f).Negate());
		//    Assert.AreEqual(TimeSpan.FromSeconds(30f), _model.GetStartOfCurrentSegment());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetStartOfCurrentSegment_CurrentSegmentIsNotNull_ReturnsStartOfCurrSegment()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(20f));
		//    Assert.AreEqual(TimeSpan.FromSeconds(15f), _model.GetStartOfCurrentSegment());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEndOfCurrentSegment_CurrentSegmentIsNull_ReturnsZero()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(1f).Negate());
		//    Assert.AreEqual(TimeSpan.Zero, _model.GetEndOfCurrentSegment());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEndOfCurrentSegment_CurrentSegmentIsNotNull_ReturnsEndOfCurrSegment()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(20f));
		//    Assert.AreEqual(TimeSpan.FromSeconds(20f), _model.GetEndOfCurrentSegment());
		//}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesHaveSegments_SegmentsDoNotExist_ReturnsFalse()
		{
			_model.Tiers[0].Segments.Clear();
			Assert.IsFalse(_model.GetDoesHaveSegments());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesHaveSegments_SegmentsExist_ReturnsTrue()
		{
			Assert.IsTrue(_model.GetDoesHaveSegments());
		}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void SelectSegmentFromTime_PassNegativeTime_MakesCurrentSegmentNull()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(1f).Negate());
		//    Assert.IsNull(_model.HighlightedSegment);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void SelectSegmentFromTime_PassNonBoundaryTime_MakesCurrentSegmentNull()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(12f));
		//    Assert.IsNull(_model.HighlightedSegment);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void SelectSegmentFromTime_PassBoundaryTime_SetsCurrentSegment()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(20f));
		//    Assert.IsNotNull(_model.HighlightedSegment);
		//    Assert.AreEqual(_model.Tiers[0].Segments[2], _model.HighlightedSegment);
		//    Assert.AreEqual(15f, _model.HighlightedSegment.Start);
		//    Assert.AreEqual(20f, _model.HighlightedSegment.End);
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetTimeWherePlaybackShouldStart_CurrentSegmentIsNull_ReturnsEndOfLastSegment()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(1f).Negate());
		//    Assert.AreEqual(TimeSpan.FromSeconds(30f),
		//        _model.GetTimeWherePlaybackShouldStart(TimeSpan.FromSeconds(3.4f)));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetTimeWherePlaybackShouldStart_CurrentSegmentStartsBeforeProposed_ReturnsProposedTime()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(20f));
		//    Assert.AreEqual(TimeSpan.FromSeconds(24f),
		//        _model.GetTimeWherePlaybackShouldStart(TimeSpan.FromSeconds(24f)));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetTimeWherePlaybackShouldStart_CurrentSegmentStartsAfterProposed_ReturnsStartOfCurrentSegment()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(20f));
		//    Assert.AreEqual(TimeSpan.FromSeconds(15f),
		//        _model.GetTimeWherePlaybackShouldStart(TimeSpan.FromSeconds(10f)));
		//}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToAnnotationFileForSegment_ReturnsPathWithCorrectFolder()
		{
			// This test only checks for the correct folder because testing for
			// correct file name is done in the TimeTier tests.

			var tier = new TimeTier(_model.OralAnnotationsFolder.Replace("_Annotations", string.Empty));

			Assert.AreEqual(_model.OralAnnotationsFolder, Path.GetDirectoryName(
				_model.GetFullPathToAnnotationFileForSegment(new Segment(tier, 1f, 2f))));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathOfAnnotationFileForTimeRange_ReturnsPathWithCorrectFolder()
		{
			// This test only checks for the correct folder because testing for
			// correct file name is done in the TimeTier tests.

			Assert.AreEqual(_model.OralAnnotationsFolder, Path.GetDirectoryName(
				_model.GetFullPathOfAnnotationFileForTimeRange(new TimeRange(0, 0))));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesSegmentHaveAnnotationFile_FilesDoNotExist_ReturnsFalse()
		{
			Assert.IsFalse(_model.GetDoesSegmentHaveAnnotationFile(2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesSegmentHaveAnnotationFile_TypeIsTranslation_CarefulExistsTranslationDoesNot_ReturnsFalse()
		{
			CreateModelAndAnnotationFileForType(OralAnnotationType.Translation, OralAnnotationType.Careful);
			Assert.IsFalse(_model.GetDoesSegmentHaveAnnotationFile(3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesSegmentHaveAnnotationFile_TypeIsCareful_TranslationExistsCarefulDoesNot_ReturnsFalse()
		{
			CreateModelAndAnnotationFileForType(OralAnnotationType.Careful, OralAnnotationType.Translation);
			Assert.IsFalse(_model.GetDoesSegmentHaveAnnotationFile(3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesSegmentHaveAnnotationFile_TypeIsCareful_CarefulExists_ReturnsTrue()
		{
			CreateModelAndAnnotationFileForType(OralAnnotationType.Careful, OralAnnotationType.Careful);
			Assert.IsTrue(_model.GetDoesSegmentHaveAnnotationFile(3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoesSegmentHaveAnnotationFile_TypeIsTranslation_TranslationExists_ReturnsTrue()
		{
			CreateModelAndAnnotationFileForType(OralAnnotationType.Translation, OralAnnotationType.Translation);
			Assert.IsTrue(_model.GetDoesSegmentHaveAnnotationFile(3));
		}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetDoesCurrentSegmentHaveAnnotationFile_CurrentSegmentIsNull_ReturnsFalse()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(1f).Negate());
		//    Assert.IsFalse(_model.GetDoesCurrentSegmentHaveAnnotationFile());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetDoesCurrentSegmentHaveAnnotationFile_FilesDoNotExist_ReturnsFalse()
		//{
		//    _model.SelectSegmentFromTime(TimeSpan.FromSeconds(20f));
		//    Assert.IsNotNull(_model.HighlightedSegment);
		//    Assert.IsFalse(_model.GetDoesCurrentSegmentHaveAnnotationFile());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetDoesCurrentSegmentHaveAnnotationFile_TypeIsTranslation_CarefulExistsTranslationDoesNot_ReturnsFalse()
		//{
		//    CreateModelAndAnnotationFileForType(OralAnnotationType.Translation, OralAnnotationType.Careful);
		//    Assert.IsFalse(_model.GetDoesCurrentSegmentHaveAnnotationFile());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetDoesCurrentSegmentHaveAnnotationFile_TypeIsCareful_TranslationExistsCarefulDoesNot_ReturnsFalse()
		//{
		//    CreateModelAndAnnotationFileForType(OralAnnotationType.Careful, OralAnnotationType.Translation);
		//    Assert.IsFalse(_model.GetDoesCurrentSegmentHaveAnnotationFile());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetDoesCurrentSegmentHaveAnnotationFile_TypeIsCareful_CarefulExists_ReturnsTrue()
		//{
		//    CreateModelAndAnnotationFileForType(OralAnnotationType.Careful, OralAnnotationType.Careful);
		//    Assert.IsTrue(_model.GetDoesCurrentSegmentHaveAnnotationFile());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetDoesCurrentSegmentHaveAnnotationFile_TypeIsTranslation_TranslationExists_ReturnsTrue()
		//{
		//    CreateModelAndAnnotationFileForType(OralAnnotationType.Translation, OralAnnotationType.Translation);
		//    Assert.IsTrue(_model.GetDoesCurrentSegmentHaveAnnotationFile());
		//}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetNextUnannotatedSegment_FollowingUnannotatedSegmentExists_MakesCurrentAndReturnsTrue()
		{
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.AreEqual(_model.TimeTier.Segments[1], _model.CurrentUnannotatedSegment);
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.AreEqual(_model.TimeTier.Segments[2], _model.CurrentUnannotatedSegment);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetNextUnannotatedSegment_FollowingSegmentsHaveAnnotations_ReturnsTrueAndFallsBackToFirstUnannotatedSegment()
		{
			CreateModelAndAnnotationFileForSegment(25f, 30f);
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.AreEqual(_model.TimeTier.Segments[2], _model.CurrentUnannotatedSegment);
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.AreEqual(_model.TimeTier.Segments[0], _model.CurrentUnannotatedSegment);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetNextUnannotatedSegment_CurrentAndFollowingSegmentsHaveAnnotations_ReturnsTrueAndFallsBackToFirstUnannotatedSegment()
		{
			CreateModelAndAnnotationFileForSegment(25f, 30f);
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.AreEqual(_model.TimeTier.Segments[2], _model.CurrentUnannotatedSegment);
			CreateModelAndAnnotationFileForSegment(15f, 20f);
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.AreEqual(_model.TimeTier.Segments[0], _model.CurrentUnannotatedSegment);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetNextUnannotatedSegment_NonAdjacentFollowingSegmentDoesNotHaveAnnotations_MakesCurrentAndReturnsTrue()
		{
			CreateModelAndAnnotationFileForSegment(15f, 20f);
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.AreEqual(_model.TimeTier.Segments[1], _model.CurrentUnannotatedSegment);
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.AreEqual(_model.TimeTier.Segments[3], _model.CurrentUnannotatedSegment);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetNextUnannotatedSegment_CurrentSegmentIsLast_ReturnsTrueAndFallsBackToFirstUnannotatedSegment()
		{
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.AreEqual(_model.TimeTier.Segments[3], _model.CurrentUnannotatedSegment);
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.AreEqual(_model.TimeTier.Segments[0], _model.CurrentUnannotatedSegment);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetNextUnannotatedSegment_NoCurrentSegment_ReturnsTrueAndSelectsFirstUnannotatedSegment()
		{
			Assert.IsNull(_model.CurrentUnannotatedSegment);
			Assert.IsTrue(_model.SetNextUnannotatedSegment());
			Assert.AreEqual(_model.TimeTier.Segments[0], _model.CurrentUnannotatedSegment);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsFullySegmented_LastSegmentEndsAtEOF_ReturnsTrue()
		{
			_model.TimeTier.Segments[3].End = (float)_model.OrigWaveStream.TotalTime.TotalSeconds;
			Assert.IsTrue(_model.IsFullySegmented);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsFullySegmented_LastSegmentEndsBeforeEOF_ReturnsFalse()
		{
			Assert.IsFalse(_model.IsFullySegmented);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Ignore("Can't figure out how to record during tests")]
		public void Undo_AnnotationHasBeenAddedToExistingLastSegment_RemovesAnnotation()
		{
			var startingSegmentCount = _model.GetSegmentCount();
			var startingLastSegmentBoundary = _model.GetEndOfLastSegment();
			var segmentTimeRange = _model.GetSegment(0).TimeRange.Copy();
			_model.InitializeAnnotationRecorder(null, t => { });
			_model.BeginAnnotationRecording(segmentTimeRange);
			var endTime = DateTime.Now.AddMilliseconds(SayMore.Properties.Settings.Default.MinimumSegmentLengthInMilliseconds);
			while (endTime < DateTime.Now)
				System.Windows.Forms.Application.DoEvents();
			//Thread.Sleep(SayMore.Properties.Settings.Default.MinimumSegmentLengthInMilliseconds);
			_model.StopAnnotationRecording(segmentTimeRange);
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.IsTrue(_model.GetDoesSegmentHaveAnnotationFile(0));
			_model.Undo();
			Assert.AreEqual(startingSegmentCount, _model.GetSegmentCount());
			Assert.AreEqual(startingLastSegmentBoundary, _model.GetEndOfLastSegment());
			Assert.IsFalse(_model.WereChangesMade);
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.IsFalse(_model.GetDoesSegmentHaveAnnotationFile(0));
			Assert.AreEqual(null, _model.TimeRangeForUndo);
		}
	}
}
