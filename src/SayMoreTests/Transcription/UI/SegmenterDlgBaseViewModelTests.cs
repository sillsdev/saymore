using System;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;
using SayMoreTests.Model.Files;

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
			SIL.Reporting.ErrorReport.IsOkToInteractWithUser = false;

			_tempAudioFile = MediaFileInfoTests.GetLongerTestAudioFile();
			_timeTier = new TimeTier(_tempAudioFile);
			_timeTier.AddSegment(0f, 10f);
			_timeTier.AddSegment(10f, 20f);
			_timeTier.AddSegment(20f, 30f);

			_textTier = new TextTier(TextTier.ElanTranscriptionTierId);

			var annotationFile = new Mock<AnnotationComponentFile>();
			annotationFile.Setup(a => a.Tiers).Returns(new TierCollection { _timeTier, _textTier });

			_componentFile = new Mock<ComponentFile>(null);
			_componentFile.Setup(f => f.PathToAnnotatedFile).Returns(_tempAudioFile);
			_componentFile.Setup(f => f.GetAnnotationFile()).Returns(annotationFile.Object);

			CreateNewModel();
			Directory.CreateDirectory(_model.OralAnnotationsFolder);

			Assert.IsNotNull(_model.OrigWaveStream);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			try { Directory.Delete(_model.OralAnnotationsFolder, true); }
			catch { }

			_componentFile.Setup(f => f.PathToAnnotatedFile).Returns("");

			if (_model != null)
				_model.Dispose();

			try { File.Delete(_tempAudioFile); }
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		private void AddTextSegmentsForAllTimeSegments()
		{
			((TextTier)_model.Tiers[1]).AddSegment("one");
			((TextTier)_model.Tiers[1]).AddSegment("two");
			((TextTier)_model.Tiers[1]).AddSegment("three");
		}

		/// ------------------------------------------------------------------------------------
		private void CreateAnnotationFile(AudioRecordingType fileType, float start, float end)
		{
			if (!Directory.Exists(_model.OralAnnotationsFolder))
				Directory.CreateDirectory(_model.OralAnnotationsFolder);

			if (fileType == AudioRecordingType.Careful)
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

		/// ------------------------------------------------------------------------------------
		private void CreateNewModel()
		{
			if (_model != null)
				_model.Dispose();

			_model = new SegmenterDlgBaseViewModel(_componentFile.Object);
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
		public void BackupOralAnnotationSegmentFile_DestFileAlreadyExists_MakesNewBackupVersion()
		{
			var srcFile = Path.Combine(_model.OralAnnotationsFolder, "one.wav");
			var dstFile = Path.Combine(_model.TempOralAnnotationsFolder, "one.wav");

			Directory.CreateDirectory(_model.TempOralAnnotationsFolder);

			var stream = File.OpenWrite(srcFile);
			stream.WriteByte(0);
			stream.Close();

			stream = File.OpenWrite(dstFile);
			stream.WriteByte(0xFF);
			stream.Close();

			_model.BackupOralAnnotationSegmentFile(srcFile, false);

			try
			{
				stream = File.OpenRead(dstFile);
				Assert.AreEqual(0xFF, stream.ReadByte());
			}
			finally
			{
				stream.Close();
			}

			try
			{
				stream = File.OpenRead(dstFile +  ".b1");
				Assert.AreEqual(0, stream.ReadByte());
			}
			finally
			{
				stream.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void BackupOralAnnotationSegmentFile_SrcFilesExistDestFilesDoNotAlreadyExist_CopiesThem()
		{
			var srcFile1 = Path.Combine(_model.OralAnnotationsFolder, "one_Careful.wav");
			var srcFile2 = Path.Combine(_model.OralAnnotationsFolder, "one_Translation.wav");

			File.OpenWrite(srcFile1).Close();
			File.OpenWrite(srcFile2).Close();
			CreateNewModel();

			Assert.IsFalse(File.Exists(Path.Combine(_model.TempOralAnnotationsFolder, "one_Careful.wav")));
			_model.BackupOralAnnotationSegmentFile(srcFile1, false);
			Assert.IsTrue(File.Exists(Path.Combine(_model.TempOralAnnotationsFolder, "one_Careful.wav")));

			Assert.IsFalse(File.Exists(Path.Combine(_model.TempOralAnnotationsFolder, "one_Translation.wav")));
			_model.BackupOralAnnotationSegmentFile(srcFile2, false);
			Assert.IsTrue(File.Exists(Path.Combine(_model.TempOralAnnotationsFolder, "one_Translation.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetListOfOralAnnotationSegmentFilesBeforeChanges_NoFilesExist_ReturnsEmptyList()
		{
			Assert.IsEmpty(_model.GetListOfOralAnnotationSegmentFilesBeforeChanges().ToArray());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetListOfOralAnnotationSegmentFilesBeforeChanges_CarefulSpeechFileExists_ReturnsIt()
		{
			var srcFile = Path.Combine(_model.OralAnnotationsFolder, "one_Careful.wav");
			File.OpenWrite(srcFile).Close();

			var list = _model.GetListOfOralAnnotationSegmentFilesBeforeChanges().ToArray();
			Assert.AreEqual(1, list.Length);
			Assert.AreEqual(srcFile, list[0]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetListOfOralAnnotationSegmentFilesBeforeChanges_BothTypesOfFilesExists_ReturnsThem()
		{
			var srcFile1 = Path.Combine(_model.OralAnnotationsFolder, "one_Careful.wav");
			var srcFile2 = Path.Combine(_model.OralAnnotationsFolder, "one_Translation.wav");

			File.OpenWrite(srcFile1).Close();
			File.OpenWrite(srcFile2).Close();

			var list = _model.GetListOfOralAnnotationSegmentFilesBeforeChanges().ToArray();
			Assert.AreEqual(2, list.Length);
			Assert.AreEqual(srcFile1, list[0]);
			Assert.AreEqual(srcFile2, list[1]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiscardRecordedAnnotations_NoAnnotationFolder_DoesNotCrash()
		{
			Directory.Delete(_model.OralAnnotationsFolder, true);
			_model.DiscardRecordedAnnotations();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiscardRecordedAnnotations_NoFilesToDiscardWhenNoNewRecordings_DoesNothing()
		{
			var srcFile = Path.Combine(_model.OralAnnotationsFolder, "one_Careful.wav");
			File.OpenWrite(srcFile).Close();
			CreateNewModel();

			Assert.AreEqual(1, Directory.GetFiles(_model.OralAnnotationsFolder, "*.*").Count());
			_model.DiscardRecordedAnnotations();
			Assert.AreEqual(1, Directory.GetFiles(_model.OralAnnotationsFolder, "*.*").Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DiscardRecordedAnnotations_NewRecordingExists_DeletesIt()
		{
			var srcFile = Path.Combine(_model.OralAnnotationsFolder, "one_Careful.wav");
			File.OpenWrite(srcFile).Close();
			CreateNewModel();

			var newRecording = Path.Combine(_model.OralAnnotationsFolder, "two_Careful.wav");
			File.OpenWrite(newRecording).Close();

			Assert.AreEqual(2, Directory.GetFiles(_model.OralAnnotationsFolder, "*.*").Count());
			_model.DiscardRecordedAnnotations();
			Assert.AreEqual(1, Directory.GetFiles(_model.OralAnnotationsFolder, "*.*").Count());
			Assert.IsFalse(File.Exists(newRecording));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RestoreOriginalRecordedAnnotations_NoFilesToRestore_DoesNotThrowException()
		{
			var srcFile = Path.Combine(_model.OralAnnotationsFolder, "one_Careful.wav");
			File.OpenWrite(srcFile).Close();
			CreateNewModel();
			_model.RestoreOriginalRecordedAnnotations();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RestoreOriginalRecordedAnnotations_HasFilesToRestore_RestoresThem()
		{
			var file1 = Path.Combine(_model.OralAnnotationsFolder, "one_Careful.wav");
			var file2 = Path.Combine(_model.OralAnnotationsFolder, "one_Translation.wav");
			File.OpenWrite(file1).Close();
			File.OpenWrite(file2).Close();
			CreateNewModel();
			_model.BackupOralAnnotationSegmentFile(file1, true);
			_model.BackupOralAnnotationSegmentFile(file2, true);

			Assert.IsFalse(File.Exists(file1));
			Assert.IsFalse(File.Exists(file2));

			_model.RestoreOriginalRecordedAnnotations();

			Assert.IsTrue(File.Exists(file1));
			Assert.IsTrue(File.Exists(file2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RestoreOriginalRecordedAnnotations_HasMultipleVersionsOfFile_RestoresOriginalVersion()
		{
			var srcFile = Path.Combine(_model.OralAnnotationsFolder, "one_Careful.wav");
			var stream = File.OpenWrite(srcFile);
			stream.WriteByte(0xAA);
			stream.Close();

			CreateNewModel();

			_model.BackupOralAnnotationSegmentFile(srcFile, true); // back up original
			Assert.IsFalse(File.Exists(srcFile));

			stream = File.OpenWrite(srcFile);
			stream.WriteByte(0);
			stream.Close();

			Assert.IsTrue(File.Exists(srcFile));

			_model.BackupOralAnnotationSegmentFile(srcFile, true); // back up 2nd version
			Assert.IsFalse(File.Exists(srcFile));

			stream = File.OpenWrite(srcFile);
			stream.WriteByte(0xFF);
			stream.Close();

			_model.BackupOralAnnotationSegmentFile(srcFile, false); // back up 3rd version

			Assert.IsTrue(File.Exists(srcFile));

			_model.RestoreOriginalRecordedAnnotations();

			Assert.IsTrue(File.Exists(srcFile));

			try
			{
				stream = File.OpenRead(srcFile);
				Assert.AreEqual(0xAA, stream.ReadByte());
			}
			finally
			{
				stream.Close();
			}
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
			var minSize = Settings.Default.MinimumSegmentLengthInMilliseconds / 1000f;
			Assert.IsFalse(_model.GetIsSegmentLongEnough(TimeSpan.FromSeconds(20 + minSize / 2)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsSegmentLongEnough_ProposedEndYieldsMinAllowed_ReturnsTrue()
		{
			var minSize = Settings.Default.MinimumSegmentLengthInMilliseconds / 1000f;
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
			AddTextSegmentsForAllTimeSegments();

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
		public void DeleteBoundary_BetweenUnignoredAndIgnoredSegments_ResultsInUnignoredSegment()
		{
			AddTextSegmentsForAllTimeSegments();
			_model.Tiers.MarkSegmentAsIgnored(1);

			Assert.IsTrue(_model.DeleteBoundary(TimeSpan.FromSeconds(10)));

			Assert.AreEqual(2, _model.Tiers[0].Segments.Count);
			Assert.AreEqual(2, _model.Tiers[1].Segments.Count);
			Assert.False(_model.GetIsSegmentIgnored(0));
			Assert.AreEqual(0, _model.GetIgnoredSegmentRanges().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteBoundary_BetweenIgnoredAndUnignoredSegments_ResultsInUnignoredSegment()
		{
			AddTextSegmentsForAllTimeSegments();
			_model.Tiers.MarkSegmentAsIgnored(0);

			Assert.IsTrue(_model.DeleteBoundary(TimeSpan.FromSeconds(10)));

			Assert.AreEqual(2, _model.Tiers[0].Segments.Count);
			Assert.AreEqual(2, _model.Tiers[1].Segments.Count);
			Assert.False(_model.GetIsSegmentIgnored(0));
			Assert.AreEqual(0, _model.GetIgnoredSegmentRanges().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteBoundary_BetweenIgnoredSegments_ResultsInIgnoredSegment()
		{
			AddTextSegmentsForAllTimeSegments();
			_model.Tiers.MarkSegmentAsIgnored(0);
			_model.Tiers.MarkSegmentAsIgnored(1);

			Assert.IsTrue(_model.DeleteBoundary(TimeSpan.FromSeconds(10)));

			Assert.AreEqual(2, _model.Tiers[0].Segments.Count);
			Assert.AreEqual(2, _model.Tiers[1].Segments.Count);
			Assert.IsTrue(_model.GetIsSegmentIgnored(0));
			var ignoredRanges = _model.GetIgnoredSegmentRanges();
			Assert.AreEqual(1, ignoredRanges.Count());
			Assert.AreEqual(0, ignoredRanges.First().StartSeconds);
			Assert.AreEqual(20, ignoredRanges.First().EndSeconds);
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
		public void IsBoundaryPermanent_NoSegmentsHaveAnnotations_ReturnsFalse()
		{
			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_NonAdjacentSegmentHasOralAnnotation_ReturnsFalse()
		{
			CreateAnnotationFile(AudioRecordingType.Careful, 0, 10);

			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_NonAdjacentSegmentHasIgnoredTextAnnotation_ReturnsFalse()
		{
			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);
			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);
			((TextTier)_model.Tiers[1]).AddSegment("three");

			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(10)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_MiddleSegmentBeforeBoundaryHasCarefulSpeechOralAnnotation_ReturnsTrue()
		{
			CreateAnnotationFile(AudioRecordingType.Careful, 10, 20);

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_InitialSegmentBeforeBoundaryHasTranslationOralAnnotation_ReturnsTrue()
		{
			CreateAnnotationFile(AudioRecordingType.Translation, 0, 10);

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(10)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentAfterBoundaryHasCarefulSpeechOralAnnotation_ReturnsTrue()
		{
			CreateAnnotationFile(AudioRecordingType.Careful, 20, 30);

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentBeforeBoundaryHasTranslationOralAnnotation_ReturnsTrue()
		{
			CreateAnnotationFile(AudioRecordingType.Translation, 20, 30);

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(30)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_MiddleSegmentBeforeBoundaryHasNonEmptyFirstTextTier_ReturnsTrue()
		{
			_model.Tiers.PreventSegmentBoundaryMovingWhereTextAnnotationsAreAdjacent = true;

			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);
			((TextTier)_model.Tiers[1]).AddSegment("two");

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentBeforeBoundaryHasNonEmptyFirstTextTier_ReturnsFalse()
		{
			_model.Tiers.PreventSegmentBoundaryMovingWhereTextAnnotationsAreAdjacent = true;

			((TextTier)_model.Tiers[1]).AddSegment("ignore this");
			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);

			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(30)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_InitialSegmentBeforeBoundaryHasNonEmptyFirstTextTier_ReturnsTrue()
		{
			_model.Tiers.PreventSegmentBoundaryMovingWhereTextAnnotationsAreAdjacent = true;

			((TextTier)_model.Tiers[1]).AddSegment("one");

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(10)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentAfterBoundarHasNonEmptySecondTextTier_ReturnsTrue()
		{
			_model.Tiers.PreventSegmentBoundaryMovingWhereTextAnnotationsAreAdjacent = true;

			_model.Tiers.AddTextTierWithEmptySegments("Garbled Speech");
			_model.Tiers[2].Segments[2].Text = "Dude, I'm not empty";

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentBeforeBoundaryHasNonEmptyFirstTextTier_ReturnsTrue()
		{
			_model.Tiers.PreventSegmentBoundaryMovingWhereTextAnnotationsAreAdjacent = true;

			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);
			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);
			((TextTier)_model.Tiers[1]).AddSegment("three");

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(30)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_BoundaryBeyondEndOfLastSegment_ReturnsFalse()
		{
			_model.Tiers.PreventSegmentBoundaryMovingWhereTextAnnotationsAreAdjacent = true;

			AddTextSegmentsForAllTimeSegments();

			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(40)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_MiddleSegmentBeforeBoundaryHasIgnoredNonEmptyFirstTextTier_ReturnsFalse()
		{
			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);
			((TextTier)_model.Tiers[1]).AddSegment("two");

			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_InitialSegmentBeforeBoundaryHasIgnoredNonEmptyFirstTextTier_ReturnsFalse()
		{
			((TextTier)_model.Tiers[1]).AddSegment("one");

			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(10)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentAfterBoundarHasIgnoredNonEmptySecondTextTier_ReturnsFalse()
		{
			_model.Tiers.AddTextTierWithEmptySegments("Garbled Speech");
			_model.Tiers[2].Segments[2].Text = "Dude, I'm not empty";

			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentBeforeBoundaryHasIgnoredNonEmptyFirstTextTier_ReturnsFalse()
		{
			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);
			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);
			((TextTier)_model.Tiers[1]).AddSegment("three");

			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(30)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_NoSegments_ReturnsFalse()
		{
			_model.TimeTier.Segments.Clear();

			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(23)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMissingTextSegmentsForTimeSegments_TiersHaveSameNumberSegments_DoesNothing()
		{
			AddTextSegmentsForAllTimeSegments();

			Assert.AreEqual(_timeTier.Segments.Count, _model.Tiers[1].Segments.Count);
			_model.CreateMissingTextSegmentsToMatchTimeSegmentCount();
			Assert.AreEqual(_timeTier.Segments.Count, _model.Tiers[1].Segments.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMissingTextSegmentsForTimeSegments_TextTierHasFewerSegments_AddsSegments()
		{
			AddTextSegmentsForAllTimeSegments();

			_model.Tiers[1].Segments.RemoveAt(1);
			Assert.Greater(_model.Tiers[0].Segments.Count, _model.Tiers[1].Segments.Count);

			_model.CreateMissingTextSegmentsToMatchTimeSegmentCount();
			Assert.AreEqual(_model.Tiers[0].Segments.Count, _model.Tiers[1].Segments.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertNewBoundary_SplitIgnoredSegment_BothSegmentsAreIgnored()
		{
			AddTextSegmentsForAllTimeSegments();
			var endOfInitialIgnoredSegment = TimeSpan.FromSeconds(40);
			_model.AddIgnoredSegment(endOfInitialIgnoredSegment);
			var startingSegmentCount = _model.GetSegmentCount();
			Assert.IsTrue(_model.GetIsSegmentIgnored(startingSegmentCount - 1));

			var endOfInsertedSegment = TimeSpan.FromSeconds(35);
			_model.InsertNewBoundary(endOfInsertedSegment);

			Assert.AreEqual(startingSegmentCount + 1, _model.GetSegmentCount());
			Assert.AreEqual(endOfInitialIgnoredSegment, _model.GetEndOfLastSegment());
			Assert.IsTrue(_model.GetIsSegmentIgnored(startingSegmentCount));
			Assert.IsTrue(_model.GetIsSegmentIgnored(startingSegmentCount - 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Undo_NoChangesHaveBeenMade_InvalidOperation()
		{
			var startingSegmentCount = _model.GetSegmentCount();
			var startingLastSegmentBoundary = _model.GetEndOfLastSegment();
			Assert.AreEqual(null, _model.TimeRangeForUndo);
			Assert.IsFalse(_model.WereChangesMade);
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.Throws(typeof(InvalidOperationException), _model.Undo);
			Assert.AreEqual(startingSegmentCount, _model.GetSegmentCount());
			Assert.AreEqual(startingLastSegmentBoundary, _model.GetEndOfLastSegment());
			Assert.IsFalse(_model.WereChangesMade);
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.AreEqual(null, _model.TimeRangeForUndo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Undo_LastSegmentBoundaryHasBeenChanged_ResetsLastSegmentBoundary()
		{
			var startingSegmentCount = _model.GetSegmentCount();
			var startingLastSegmentBoundary = _model.GetEndOfLastSegment();
			var newEnd = TimeSpan.FromSeconds(40);
			_model.SegmentBoundaryMoved(TimeSpan.FromSeconds(30), newEnd);
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.AreEqual(newEnd, _model.GetEndOfLastSegment());
			Assert.AreEqual(new TimeRange(20, 40), _model.TimeRangeForUndo);
			_model.Undo();
			Assert.AreEqual(startingSegmentCount, _model.GetSegmentCount());
			Assert.AreEqual(startingLastSegmentBoundary, _model.GetEndOfLastSegment());
			Assert.IsFalse(_model.WereChangesMade);
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.AreEqual(null, _model.TimeRangeForUndo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Undo_SegmentHasBeenAdded_RemovesLastAddedSegment()
		{
			AddTextSegmentsForAllTimeSegments();
			var startingSegmentCount = _model.GetSegmentCount();
			var startingLastSegmentBoundary = _model.GetEndOfLastSegment();
			var newEnd = TimeSpan.FromSeconds(40);
			_model.InsertNewBoundary(newEnd);
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.AreEqual(newEnd, _model.GetEndOfLastSegment());
			Assert.AreEqual(new TimeRange(30, 40), _model.TimeRangeForUndo);
			_model.Undo();
			Assert.AreEqual(startingSegmentCount, _model.GetSegmentCount());
			Assert.AreEqual(startingLastSegmentBoundary, _model.GetEndOfLastSegment());
			Assert.IsFalse(_model.WereChangesMade);
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.AreEqual(null, _model.TimeRangeForUndo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Undo_SegmentHasBeenDeleted_UndoNotImplemented()
		{
			AddTextSegmentsForAllTimeSegments();
			var startingSegmentCount = _model.GetSegmentCount();
			_model.DeleteBoundary(TimeSpan.FromSeconds(30));
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.AreEqual(TimeSpan.FromSeconds(20), _model.GetEndOfLastSegment());
			Assert.IsNull(_model.TimeRangeForUndo);
			Assert.Throws(typeof(NotImplementedException), _model.Undo);
			Assert.AreEqual(startingSegmentCount - 1, _model.GetSegmentCount());
			Assert.AreEqual(TimeSpan.FromSeconds(20), _model.GetEndOfLastSegment());
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.IsNull(_model.TimeRangeForUndo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Undo_SegmentHasBeenAddedAndThenChanged_RemovesLastAddedSegment()
		{
			AddTextSegmentsForAllTimeSegments();
			var startingSegmentCount = _model.GetSegmentCount();
			var startingLastSegmentBoundary = _model.GetEndOfLastSegment();
			var endWhenAdded = TimeSpan.FromSeconds(40);
			var newEnd = TimeSpan.FromSeconds(38);
			_model.InsertNewBoundary(endWhenAdded);
			_model.SegmentBoundaryMoved(endWhenAdded, newEnd);
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.AreEqual(newEnd, _model.GetEndOfLastSegment());
			Assert.AreEqual(new TimeRange(30, 38), _model.TimeRangeForUndo);
			_model.Undo();
			Assert.AreEqual(startingSegmentCount, _model.GetSegmentCount());
			Assert.AreEqual(startingLastSegmentBoundary, _model.GetEndOfLastSegment());
			Assert.IsFalse(_model.WereChangesMade);
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.AreEqual(null, _model.TimeRangeForUndo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Undo_MultipleSegmentsHaveBeenAdded_RemovesSegmentsInReverseOrder()
		{
			AddTextSegmentsForAllTimeSegments();
			var startingSegmentCount = _model.GetSegmentCount();
			var startingLastSegmentBoundary = _model.GetEndOfLastSegment();
			var newEnd1 = TimeSpan.FromSeconds(40);
			_model.InsertNewBoundary(newEnd1);
			var newEnd2 = TimeSpan.FromSeconds(50);
			_model.InsertNewBoundary(newEnd2);
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.AreEqual(newEnd2, _model.GetEndOfLastSegment());
			Assert.AreEqual(new TimeRange(40, 50), _model.TimeRangeForUndo);
			_model.Undo();
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.AreEqual(newEnd1, _model.GetEndOfLastSegment());
			Assert.AreEqual(new TimeRange(30, 40), _model.TimeRangeForUndo);
			_model.Undo();
			Assert.AreEqual(startingSegmentCount, _model.GetSegmentCount());
			Assert.AreEqual(startingLastSegmentBoundary, _model.GetEndOfLastSegment());
			Assert.IsFalse(_model.WereChangesMade);
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.AreEqual(null, _model.TimeRangeForUndo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Undo_SegmentHasBeenDeletedAndReAdded_RemovesAddedSegment()
		{
			AddTextSegmentsForAllTimeSegments();
			var startingSegmentCount = _model.GetSegmentCount();
			var end = TimeSpan.FromSeconds(30);
			_model.DeleteBoundary(end);
			_model.InsertNewBoundary(end);
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.AreEqual(end, _model.GetEndOfLastSegment());
			Assert.AreEqual(new TimeRange(20, 30), _model.TimeRangeForUndo);
			_model.Undo();
			Assert.AreEqual(startingSegmentCount - 1, _model.GetSegmentCount());
			Assert.AreEqual(TimeSpan.FromSeconds(20), _model.GetEndOfLastSegment());
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.IsNull(_model.TimeRangeForUndo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Undo_SegmentBoundaryHasBeenMovedAndFollowingSegmentAdded_RemovesAddedSegmentAndThenRestoresOriginalBoundary()
		{
			AddTextSegmentsForAllTimeSegments();
			var startingSegmentCount = _model.GetSegmentCount();
			var startingLastSegmentBoundary = _model.GetEndOfLastSegment();
			var newEnd = TimeSpan.FromSeconds(40);
			_model.SegmentBoundaryMoved(startingLastSegmentBoundary, newEnd);
			var addedEnd = TimeSpan.FromSeconds(50);
			_model.InsertNewBoundary(addedEnd);
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.AreEqual(addedEnd, _model.GetEndOfLastSegment());
			Assert.AreEqual(new TimeRange(40, 50), _model.TimeRangeForUndo);
			_model.Undo();
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.AreEqual(newEnd, _model.GetEndOfLastSegment());
			Assert.AreEqual(new TimeRange(20, 40), _model.TimeRangeForUndo);
			_model.Undo();
			Assert.AreEqual(startingSegmentCount, _model.GetSegmentCount());
			Assert.AreEqual(startingLastSegmentBoundary, _model.GetEndOfLastSegment());
			Assert.IsFalse(_model.WereChangesMade);
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.AreEqual(null, _model.TimeRangeForUndo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Undo_IgnoredSegmentAdded_RemovesAddedSegment()
		{
			AddTextSegmentsForAllTimeSegments();
			var startingSegmentCount = _model.GetSegmentCount();
			_model.Tiers.GetTranscriptionTier().Segments[startingSegmentCount - 1].Text = "last segment";
			var startingLastSegmentBoundary = _model.GetEndOfLastSegment();
			var newEnd = TimeSpan.FromSeconds(40);
			_model.AddIgnoredSegment(newEnd);
			Assert.IsTrue(_model.WereChangesMade);
			Assert.IsTrue(_model.SegmentBoundariesChanged);
			Assert.AreEqual(newEnd, _model.GetEndOfLastSegment());
			Assert.IsTrue(_model.GetIsSegmentIgnored(startingSegmentCount));
			Assert.AreEqual(new TimeRange(30, 40), _model.TimeRangeForUndo);
			_model.Undo();
			Assert.AreEqual(startingSegmentCount, _model.GetSegmentCount());
			Assert.AreEqual(startingLastSegmentBoundary, _model.GetEndOfLastSegment());
			Assert.IsFalse(_model.WereChangesMade);
			Assert.IsFalse(_model.SegmentBoundariesChanged);
			Assert.AreEqual(null, _model.TimeRangeForUndo);
			Assert.AreEqual("last segment", _model.Tiers.GetTranscriptionTier().Segments[startingSegmentCount - 1].Text);
			Assert.IsFalse(_model.GetIsSegmentIgnored(startingSegmentCount - 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFinalSegmentIfAlmostComplete_NoSegments_DoesNothing()
		{
			_model.TimeTier.Segments.Clear();
			Assert.AreEqual(0, _model.GetSegmentCount());
			_model.AddFinalSegmentIfAlmostComplete();
			Assert.AreEqual(0, _model.GetSegmentCount());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFinalSegmentIfAlmostComplete_SixSecondsRemaining_DoesNothing()
		{
			AddTextSegmentsForAllTimeSegments();
			_model.InsertNewBoundary(_model.OrigWaveStream.TotalTime.Add(TimeSpan.FromSeconds(-6)));
			var initialTimeRangeForUndo = _model.TimeRangeForUndo;

			var startingSegmentCount = _model.GetSegmentCount();
			_model.AddFinalSegmentIfAlmostComplete();
			Assert.AreEqual(startingSegmentCount, _model.GetSegmentCount());
			Assert.IsFalse(_model.GetIsSegmentIgnored(startingSegmentCount));
			Assert.AreEqual(initialTimeRangeForUndo, _model.TimeRangeForUndo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFinalSegmentIfAlmostComplete_FiveSecondsRemaining_AddsFinalSegment()
		{
			AddTextSegmentsForAllTimeSegments();
			_model.InsertNewBoundary(_model.OrigWaveStream.TotalTime.Add(TimeSpan.FromSeconds(-5)));
			var initialTimeRangeForUndo = _model.TimeRangeForUndo;

			var startingSegmentCount = _model.GetSegmentCount();
			_model.AddFinalSegmentIfAlmostComplete();
			Assert.AreEqual(startingSegmentCount + 1, _model.GetSegmentCount());
			Assert.AreEqual(_model.OrigWaveStream.TotalTime, _model.GetEndOfLastSegment());
			Assert.IsTrue(_model.GetIsSegmentIgnored(startingSegmentCount));
			Assert.AreNotEqual(initialTimeRangeForUndo, _model.TimeRangeForUndo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFinalSegmentIfAlmostComplete_LessThanMinimumSegmentLengthRemaining_ExtendsFinalSegment()
		{
			AddTextSegmentsForAllTimeSegments();
			_model.InsertNewBoundary(_model.OrigWaveStream.TotalTime.Add(TimeSpan.FromSeconds(
				-(Settings.Default.MinimumSegmentLengthInMilliseconds - 1) / 1000f)));
			var initialTimeRangeForUndo = _model.TimeRangeForUndo;

			var startingSegmentCount = _model.GetSegmentCount();
			_model.AddFinalSegmentIfAlmostComplete();
			Assert.AreEqual(startingSegmentCount, _model.GetSegmentCount());
			Assert.AreEqual(_model.OrigWaveStream.TotalTime, _model.GetEndOfLastSegment());
			Assert.IsFalse(_model.GetIsSegmentIgnored(startingSegmentCount));
			Assert.AreNotEqual(initialTimeRangeForUndo, _model.TimeRangeForUndo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFinalSegmentIfAlmostComplete_CompletelySegmented_DoesNothing()
		{
			AddTextSegmentsForAllTimeSegments();
			_model.InsertNewBoundary(_model.OrigWaveStream.TotalTime);
			var initialTimeRangeForUndo = _model.TimeRangeForUndo;

			var startingSegmentCount = _model.GetSegmentCount();
			_model.AddFinalSegmentIfAlmostComplete();
			Assert.AreEqual(startingSegmentCount, _model.GetSegmentCount());
			Assert.IsFalse(_model.GetIsSegmentIgnored(startingSegmentCount));
			Assert.AreEqual(initialTimeRangeForUndo, _model.TimeRangeForUndo);
		}
	}
}
