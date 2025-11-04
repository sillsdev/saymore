using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;
using SayMoreTests.MediaUtils;

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

		/// ------------------------------------------------------------------------------------
		[TestCase(32, 36, 6, 0, 0)]
		[TestCase(3, 44, 8, 2, 0)]
		[TestCase(32, 36, 6, 0, 1)]
		[TestCase(3, 44, 8, 2, 12)]
		public void GetButtonRectangleForSegment_SingleButtonFits_PositionedWithNormalLeftAndBottomMargin(
			int buttonWidth, int buttonHeight, int bottomMargin, int extraRoom, int rcLeft)
		{
			var rect = new Rectangle(rcLeft, 0, buttonWidth + SegmenterDlgBase.kNormalHorizontalMargin * 2 + extraRoom, buttonHeight + bottomMargin + extraRoom);
			var result = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {new Size(buttonWidth, buttonHeight)}, rect.Right, 0);
			Assert.AreEqual(rcLeft + SegmenterDlgBase.kNormalHorizontalMargin, result.Left);
			Assert.AreEqual(extraRoom, result.Top);
			Assert.AreEqual(buttonHeight, result.Height);
			Assert.AreEqual(buttonWidth, result.Width);
		}
		
		/// ------------------------------------------------------------------------------------
		[TestCase(32, 36, 6, 0, 0)]
		[TestCase(3, 44, 8, 2, 0)]
		[TestCase(32, 36, 6, 0, 1)]
		[TestCase(3, 44, 8, 2, 12)]
		public void GetButtonRectangleForSegment_SingleButtonFitsUsingReducedMargins_PositionedWithReducedLeftMargin(
			int buttonWidth, int buttonHeight, int bottomMargin, int extraRoom, int rcLeft)
		{
			var rect = new Rectangle(rcLeft, 0, buttonWidth + SegmenterDlgBase.kMinimalHorizontalMargin * 2 + extraRoom, buttonHeight + bottomMargin + extraRoom);
			var result = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {new Size(buttonWidth, buttonHeight)}, rect.Right, 0);
			Assert.AreEqual(rcLeft + SegmenterDlgBase.kMinimalHorizontalMargin, result.Left);
			Assert.AreEqual(extraRoom, result.Top);
			Assert.AreEqual(buttonHeight, result.Height);
			Assert.AreEqual(buttonWidth, result.Width);
		}

		/// ------------------------------------------------------------------------------------
		[TestCase(32, 36, 6)]
		[TestCase(3, 44, 8)]
		public void GetButtonRectangleForSegment_SingleButtonTooWide_EmptyRectangle(
			int buttonWidth, int buttonHeight, int bottomMargin)
		{
			// Note: Current design should prevent this.
			var rect = new Rectangle(0, 0, buttonWidth + SegmenterDlgBase.kMinimalHorizontalMargin * 2 - 1, buttonHeight + bottomMargin);
			Assert.AreEqual(Rectangle.Empty,
				SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {new Size(buttonWidth, buttonHeight)}, 1000, 0));
		}

		// Note: We could add tests for various cases where the (combined) button height(s) are too
		// tall to fit, but our current design prevents that, s we'd need to decide on a desired
		// behavior.
		
		/// ------------------------------------------------------------------------------------
		[TestCase(32, -6, 6)] // 38 pixels visible in segment.
		[TestCase(32, -6, 5)] // 37 pixels visible in segment.
		[TestCase(32, -6, 3)] // 35 pixels visible in segment.
		[TestCase(32, -6, 1)] // 33 pixels visible in segment.
		[TestCase(32, -6, 0)] // 32 pixels visible in segment.
		public void GetButtonRectangleForSegment_SingleButtonFitsWithinVisibleRectangleUsingNormalRightMargin_ButtonAlignedToRightEdgeOfSegmentWithMargin(
			int buttonWidth, int boundingRectangleLeft, int extraWidth)
		{
			const int bottomMargin = 6;
			const int buttonHeight = 34;
		
			var rect = new Rectangle(boundingRectangleLeft, 0, buttonWidth + SegmenterDlgBase.kNormalHorizontalMargin * 2 + extraWidth, buttonHeight + bottomMargin);
			var result = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {new Size(buttonWidth, buttonHeight)}, rect.Right, 0);
			Assert.AreEqual(0, result.Top);
			Assert.AreEqual(buttonHeight, result.Height);
			Assert.AreEqual(buttonWidth, result.Width);
			Assert.AreEqual(rect.Right - 6, result.Right);
		}
		
		/// ------------------------------------------------------------------------------------
		[TestCase(32, -6, 6)] // 35 pixels visible in segment.
		[TestCase(32, -6, 8)] // 37 pixels visible in segment.
		public void GetButtonRectangleForSegment_SingleButtonFitsWithinVisibleRectangleUsingReducedRightMargin_ButtonAlignedToRightEdgeOfSegmentWithNarrowMargin(
			int buttonWidth, int boundingRectangleLeft, int extraWidth)
		{
			const int bottomMargin = 6;
			const int buttonHeight = 34;
		
			var rect = new Rectangle(boundingRectangleLeft, 0, buttonWidth + SegmenterDlgBase.kMinimalHorizontalMargin + extraWidth, buttonHeight + bottomMargin);
			var result = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {new Size(buttonWidth, buttonHeight)}, rect.Right, 0);
			Assert.AreEqual(0, result.Top);
			Assert.AreEqual(buttonHeight, result.Height);
			Assert.AreEqual(buttonWidth, result.Width);
			Assert.AreEqual(rect.Right - SegmenterDlgBase.kMinimalHorizontalMargin, result.Right);
		}

		/// ------------------------------------------------------------------------------------
		[TestCase(32, -6, 12)] // 52 pixels visible in segment (only slightly scrolled). Entire button visible with normal margins
		[TestCase(32, -6, 6)] // 44 pixels visible in segment (only slightly scrolled). Entire button visible with normal margins
		[TestCase(32, -26, 26)] // 44 pixels visible in segment. Entire button visible with normal margins
		public void GetButtonRectangleForSegment_SingleButtonFitsEasilyWithNormalMarginsInRectangleWithNegativeLeft_LeftMarginSetRelativeToVisibleEdge(
			int buttonWidth, int boundingRectangleLeft, int extraRoom)
		{
			const int bottomMargin = 6;
			const int buttonHeight = 34;
		
			var rect = new Rectangle(boundingRectangleLeft, 0, buttonWidth + SegmenterDlgBase.kNormalHorizontalMargin * 2 + extraRoom, buttonHeight + bottomMargin);
			var result = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {new Size(buttonWidth, buttonHeight)}, rect.Right, 0);
			Assert.AreEqual(0, result.Top);
			Assert.AreEqual(buttonHeight, result.Height);
			Assert.AreEqual(buttonWidth, result.Width);
			Assert.AreEqual(SegmenterDlgBase.kNormalHorizontalMargin, result.Left);
		}

		/// ------------------------------------------------------------------------------------
		[TestCase(32, -8, 0, ExpectedResult = 1)] // 36 pixels visible in segment. Entire button visible.
		[TestCase(10, -11, 2, ExpectedResult = 0)] // 13 pixels visible in segment. Entire button visible, no visible left margin
		[TestCase(10, -12, 2, ExpectedResult = -1)] // 12 pixels visible in segment. Button clipped by 1 pixel on left to allow for minimim right margin
		[TestCase(20, -28, 12, ExpectedResult = -10)] // 16 pixels visible in segment. 20-10 = 10 pixels of button visible. (Don't bother with reduced margins.)
		public int GetButtonRectangleForSegment_SingleButtonFitsInRectangleThatIsScrolledTooFarLeftToAllowFullLeftMargin_ReducedOrClippedLeftPosition(
			int buttonWidth, int boundingRectangleLeft, int extraWidth)
		{
			const int bottomMargin = 6;
			const int buttonHeight = 34;
		
			var rect = new Rectangle(boundingRectangleLeft, 0, buttonWidth + SegmenterDlgBase.kNormalHorizontalMargin * 2 + extraWidth, buttonHeight + bottomMargin);
			var result = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {new Size(buttonWidth, buttonHeight)}, rect.Right, 0);
			Assert.AreEqual(0, result.Top);
			Assert.AreEqual(buttonHeight, result.Height);
			Assert.AreEqual(buttonWidth, result.Width);
			return result.Left;
		}

		/// ------------------------------------------------------------------------------------
		[TestCase(32, -44, 0)]
		[TestCase(32, -38, 0)]
		[TestCase(10, -24, 2)]
		[TestCase(10, -34, 6)]
		public void GetButtonRectangleForSegment_SegmentScrolledTooFarLeftToAllowButtonToDisplay_NonVisibleRectangle(
			int buttonWidth, int boundingRectangleLeft, int extraWidth)
		{
			const int bottomMargin = 6;
			const int buttonHeight = 34;
		
			var rect = new Rectangle(boundingRectangleLeft, 0, buttonWidth + SegmenterDlgBase.kNormalHorizontalMargin * 2 + extraWidth, buttonHeight + bottomMargin);
			Assert.IsTrue(SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {new Size(buttonWidth, buttonHeight)}, rect.Right, 0).Right <= 0);
		}

		/// ------------------------------------------------------------------------------------
		[TestCase(32, 36, 6, 0)]
		[TestCase(3, 44, 8, 2)]
		public void GetButtonRectangleForSegment_TwoButtonsOfSameSizeFitOnSameRowUsingNormalMarginsUnscrolled_PositionedWithNormalMargins(
			int buttonWidth, int buttonHeight, int bottomMargin, int extraWidth)
		{
			var rect = new Rectangle(0, 0, buttonWidth * 2 + SegmenterDlgBase.kNormalHorizontalMargin * 2 + SegmenterDlgBase.kButtonSpacing + extraWidth, buttonHeight + bottomMargin);
			var buttonSize = new Size(buttonWidth, buttonHeight);
			var resultB0 = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {buttonSize, buttonSize}, rect.Right, 0);
			Assert.AreEqual(SegmenterDlgBase.kNormalHorizontalMargin, resultB0.Left);
			Assert.AreEqual(0, resultB0.Top);
			Assert.AreEqual(buttonHeight, resultB0.Height);
			Assert.AreEqual(buttonWidth, resultB0.Width);
			var resultB1 = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {buttonSize, buttonSize}, rect.Right, 1);
			Assert.AreEqual(SegmenterDlgBase.kNormalHorizontalMargin + buttonWidth + SegmenterDlgBase.kButtonSpacing + extraWidth, resultB1.Left);
			Assert.AreEqual(0, resultB1.Top);
			Assert.AreEqual(buttonHeight, resultB1.Height);
			Assert.AreEqual(buttonWidth, resultB1.Width);
		}
		
		/// ------------------------------------------------------------------------------------
		[TestCase(32, 36, 6, 0)]
		[TestCase(3, 44, 8, 2)]
		public void GetButtonRectangleForSegment_TwoButtonsOfSameSizeFitOnSameRowUsingReducedMargins_PositionedWithReducedLeftMargin(
			int buttonWidth, int buttonHeight, int bottomMargin, int extraWidth)
		{
			var rect = new Rectangle(0, 0, buttonWidth * 2 + SegmenterDlgBase.kMinimalHorizontalMargin * 2 + SegmenterDlgBase.kButtonSpacing + extraWidth, buttonHeight + bottomMargin);
			var buttonSize = new Size(buttonWidth, buttonHeight);
			var resultB0 = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {buttonSize, buttonSize}, rect.Right, 0);
			Assert.AreEqual(SegmenterDlgBase.kMinimalHorizontalMargin, resultB0.Left);
			Assert.AreEqual(0, resultB0.Top);
			Assert.AreEqual(buttonHeight, resultB0.Height);
			Assert.AreEqual(buttonWidth, resultB0.Width);
			var resultB1 = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, new[] {buttonSize, buttonSize}, rect.Right, 1);
			Assert.AreEqual(SegmenterDlgBase.kMinimalHorizontalMargin + buttonWidth + SegmenterDlgBase.kButtonSpacing + extraWidth, resultB1.Left);
			Assert.AreEqual(0, resultB1.Top);
			Assert.AreEqual(buttonHeight, resultB1.Height);
			Assert.AreEqual(buttonWidth, resultB1.Width);
		}
		
		/// ------------------------------------------------------------------------------------
		[TestCase(32, 36, 40, 42)] // Wider and taller
		[TestCase(13, 44, 80, 44)] // Wider but same height
		[TestCase(23, 20, 24, 19)] // Wider but shorter
		[TestCase(32, 36, 20, 42)] // Narrower and taller
		[TestCase(13, 44, 10, 44)] // Narrower but same height
		[TestCase(23, 20, 21, 19)] // Narrower but shorter
		public void GetButtonRectangleForSegment_TwoButtonsOfDifferentSizeFitExactlyOnSameRowUsingNormalMargins_PositionedWithNormalLeftMargin(
			int firstButtonWidth, int firstButtonHeight, int secondButtonWidth, int secondButtonHeight)
		{
			const int bottomMargin = 6;

			var rect = new Rectangle(0, 0, firstButtonWidth + secondButtonWidth + SegmenterDlgBase.kNormalHorizontalMargin * 2 + SegmenterDlgBase.kButtonSpacing, 100);
			var buttonSizes = new[] {new Size(firstButtonWidth, firstButtonHeight), new Size(secondButtonWidth, secondButtonHeight)};
			var resultB0 = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, buttonSizes, rect.Right, 0);
			Assert.AreEqual(SegmenterDlgBase.kNormalHorizontalMargin, resultB0.Left);
			Assert.AreEqual(94, resultB0.Bottom);
			Assert.AreEqual(firstButtonHeight, resultB0.Height);
			Assert.AreEqual(firstButtonWidth, resultB0.Width);
			var resultB1 = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, buttonSizes, rect.Right, 1);
			Assert.AreEqual(SegmenterDlgBase.kNormalHorizontalMargin + firstButtonWidth + SegmenterDlgBase.kButtonSpacing, resultB1.Left);
			Assert.AreEqual(94, resultB1.Bottom);
			Assert.AreEqual(secondButtonHeight, resultB1.Height);
			Assert.AreEqual(secondButtonWidth, resultB1.Width);
		}
		
		/// ------------------------------------------------------------------------------------
		[TestCase(100, -42)]
		[TestCase(100, 42)]
		[TestCase(100, 98)]
		[TestCase(100, 100)]
		public void GetButtonRectangleForSegment_TwoButtonsFitOnWideRowScrolledPartiallyOffScreen_PositionedWithinVisibleArea(
			int extraWidth, int x)
		{
			const int bottomMargin = 6;
			const int buttonSize = 32;

			var buttonDimensions = new Size(buttonSize, buttonSize);
			var rect = new Rectangle(x, 0, buttonSize * 2 + SegmenterDlgBase.kNormalHorizontalMargin * 2 + SegmenterDlgBase.kButtonSpacing + extraWidth, buttonSize + bottomMargin);
			var buttonSizes = new[] {buttonDimensions, buttonDimensions};
			var boundingControlRightEdge = rect.Width;
			var resultB0 = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, buttonSizes, boundingControlRightEdge, 0);
			Assert.AreEqual(Math.Max(0, x) + SegmenterDlgBase.kNormalHorizontalMargin, resultB0.Left);
			Assert.AreEqual(buttonSize, resultB0.Bottom);
			Assert.AreEqual(buttonSize, resultB0.Height);
			Assert.AreEqual(buttonSize, resultB0.Width);
			var resultB1 = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, buttonSizes, boundingControlRightEdge, 1);
			Assert.AreEqual(Math.Min(boundingControlRightEdge, rect.Right) - SegmenterDlgBase.kNormalHorizontalMargin, resultB1.Right);
			Assert.AreEqual(buttonSize, resultB1.Bottom);
			Assert.AreEqual(buttonSize, resultB1.Height);
			Assert.AreEqual(buttonSize, resultB1.Width);
		}
		
		/// ------------------------------------------------------------------------------------
		[TestCase(10, -42, 0)]
		[TestCase(10, 42, 6)]
		public void GetButtonRectangleForSegment_TwoButtonsFitStackedButNotSideBySideBecauseSegmentIsPartiallyOffScreen_PositionedStacked(
			int extraWidth, int x, int y)
		{
			const int bottomMargin = 6;
			const int buttonSize = 32;

			var buttonDimensions = new Size(buttonSize, buttonSize);
			var rect = new Rectangle(x, y, buttonSize * 2 + SegmenterDlgBase.kNormalHorizontalMargin * 2 + SegmenterDlgBase.kButtonSpacing + extraWidth,
				buttonSize * 2 + bottomMargin + SegmenterDlgBase.kButtonSpacing);
			var buttonSizes = new[] {buttonDimensions, buttonDimensions};
			var boundingControlRightEdge = rect.Width;
			var expectedLeftEdgeOfButtons = Math.Max(0, x) + SegmenterDlgBase.kNormalHorizontalMargin;
			var resultB0 = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, buttonSizes, boundingControlRightEdge, 0);
			Assert.AreEqual(expectedLeftEdgeOfButtons, resultB0.Left);
			Assert.AreEqual(y, resultB0.Top);
			Assert.AreEqual(buttonSize, resultB0.Height);
			Assert.AreEqual(buttonSize, resultB0.Width);
			var resultB1 = SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, buttonSizes, boundingControlRightEdge, 1);
			Assert.AreEqual(expectedLeftEdgeOfButtons, resultB1.Left);
			Assert.AreEqual(y + buttonSize + SegmenterDlgBase.kButtonSpacing, resultB1.Top);
			Assert.AreEqual(buttonSize, resultB1.Height);
			Assert.AreEqual(buttonSize, resultB1.Width);
		}
		
		/// ------------------------------------------------------------------------------------
		/// This is basically for fun (to get full test coverage) because I kind of over-
		/// designed this method. Currently in SayMore we only ever have two buttons, one left-
		/// aligned and the other right-aligned. But if we ever want to change that, now it will
		/// be easy.
		/// ------------------------------------------------------------------------------------
		[TestCase(32, 36, 40, 0)] // All right-aligned
		[TestCase(13, 44, 80, 1)]
		[TestCase(23, 20, 24, 2)] // All left-aligned
		public void GetButtonRectangleForSegment_ThreeButtonsOfDifferentSizeFitOnSameRowUsingNormalMargins_PositionedAccordingToAlignmentSpecs(
			int firstButtonWidth, int secondButtonWidth, int thirdButtonWidth, int indexOfFirstRightAlignedButton)
		{
			const int bottomMargin = 6;

			var rect = new Rectangle(0, 0, 400, 50);
			var buttonSizes = new[] {new Size(firstButtonWidth, 33), new Size(secondButtonWidth, 33), new Size(thirdButtonWidth, 33)};
			var results = new[]
			{
				SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, buttonSizes, rect.Right, 0, indexOfFirstRightAlignedButton),
				SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, buttonSizes, rect.Right, 1, indexOfFirstRightAlignedButton),
				SegmenterDlgBase.GetButtonRectangleForSegment(rect, bottomMargin, buttonSizes, rect.Right, 2, indexOfFirstRightAlignedButton)
			};
			var runningLeftPos = SegmenterDlgBase.kNormalHorizontalMargin;
			for (int i = 0; i < 3; i++)
			{
				var rc = results[i];
				Assert.AreEqual(44, rc.Bottom, $"Bottom of button {i} incorrect.");
				Assert.AreEqual(33, rc.Height, $"Height of button {i} incorrect.");
				Assert.AreEqual(buttonSizes[i].Width, rc.Width, $"Width of button {i} incorrect.");
				if (i < indexOfFirstRightAlignedButton)
				{
					Assert.AreEqual(runningLeftPos, rc.Left, $"Horizontal position of button {i} incorrect - should have aligned left.");
					runningLeftPos += rc.Width + SegmenterDlgBase.kButtonSpacing;
				}
			}

			var runningRightPos = rect.Right - SegmenterDlgBase.kNormalHorizontalMargin;;
			for (int i = 2; i < indexOfFirstRightAlignedButton; i--)
			{
				var rc = results[i];
				Assert.AreEqual(runningRightPos, rc.Right, $"Horizontal position of button {i} incorrect - should have aligned right.");
				runningRightPos -= rc.Width + SegmenterDlgBase.kButtonSpacing;
			}

			Assert.IsTrue(runningLeftPos - SegmenterDlgBase.kButtonSpacing <= runningRightPos,
				"Buttons should not have collided in the middle!");
		}
	}
}
