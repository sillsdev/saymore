using System;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using Palaso.TestUtilities;
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

			var annotationFile = new Mock<AnnotationComponentFile>();
			annotationFile.Setup(a => a.Tiers).Returns(new TierCollection { _timeTier, _textTier });

			_componentFile = new Mock<ComponentFile>();
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
		private void CreateAnnotationFile(OralAnnotationType fileType, float start, float end)
		{
			if (!Directory.Exists(_model.OralAnnotationsFolder))
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
		public void BackupOralAnnotationSegmentFile_DestFileAlreadyExists_DoesNotCopy()
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

			_model.BackupOralAnnotationSegmentFile(srcFile);

			try
			{
				stream = File.OpenRead(dstFile);
				Assert.AreEqual(0xFF, stream.ReadByte());
			}
			finally
			{
				stream.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void BackupOralAnnotationSegmentFile_SrcFileDidNotAlreadyExists_DoesNotCopy()
		{
			var srcFile = Path.Combine(_model.OralAnnotationsFolder, "one.wav");
			var dstFile = Path.Combine(_model.TempOralAnnotationsFolder, "one.wav");

			Directory.CreateDirectory(_model.TempOralAnnotationsFolder);

			Assert.IsFalse(File.Exists(dstFile));
			_model.BackupOralAnnotationSegmentFile(srcFile);
			Assert.IsFalse(File.Exists(dstFile));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void BackupOralAnnotationSegmentFile_SrcFileExistsDestFileDoesNotAlreadyExists_CopiesThem()
		{
			var srcFile1 = Path.Combine(_model.OralAnnotationsFolder, "one_Careful.wav");
			var srcFile2 = Path.Combine(_model.OralAnnotationsFolder, "one_Translation.wav");

			File.OpenWrite(srcFile1).Close();
			File.OpenWrite(srcFile2).Close();
			CreateNewModel();

			Assert.IsFalse(File.Exists(Path.Combine(_model.TempOralAnnotationsFolder, "one_Careful.wav")));
			_model.BackupOralAnnotationSegmentFile(srcFile1);
			Assert.IsTrue(File.Exists(Path.Combine(_model.TempOralAnnotationsFolder, "one_Careful.wav")));

			Assert.IsFalse(File.Exists(Path.Combine(_model.TempOralAnnotationsFolder, "one_Translation.wav")));
			_model.BackupOralAnnotationSegmentFile(srcFile2);
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
			_model.BackupOralAnnotationSegmentFile(file1);
			_model.BackupOralAnnotationSegmentFile(file2);
			File.Delete(file1);
			File.Delete(file2);

			Assert.IsFalse(File.Exists(file1));
			Assert.IsFalse(File.Exists(file2));

			_model.RestoreOriginalRecordedAnnotations();

			Assert.IsTrue(File.Exists(file1));
			Assert.IsTrue(File.Exists(file2));
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
			CreateAnnotationFile(OralAnnotationType.Careful, 0, 10);

			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_NonAdjacentSegmentHasTextAnnotation_ReturnsFalse()
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
			CreateAnnotationFile(OralAnnotationType.Careful, 10, 20);

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_InitialSegmentBeforeBoundaryHasTranslationOralAnnotation_ReturnsTrue()
		{
			CreateAnnotationFile(OralAnnotationType.Translation, 0, 10);

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(10)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentAfterBoundaryHasCarefulSpeechOralAnnotation_ReturnsTrue()
		{
			CreateAnnotationFile(OralAnnotationType.Careful, 20, 30);

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentBeforeBoundaryHasTranslationOralAnnotation_ReturnsTrue()
		{
			CreateAnnotationFile(OralAnnotationType.Translation, 20, 30);

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(30)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_MiddleSegmentBeforeBoundaryHasNonEmptyFirstTextTier_ReturnsTrue()
		{
			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);
			((TextTier)_model.Tiers[1]).AddSegment("two");

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentBeforeBoundaryHasNonEmptyFirstTextTier_ReturnsFalse()
		{
			((TextTier)_model.Tiers[1]).AddSegment("ignore this");
			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);

			Assert.IsFalse(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(30)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_InitialSegmentBeforeBoundaryHasNonEmptyFirstTextTier_ReturnsTrue()
		{
			((TextTier)_model.Tiers[1]).AddSegment("one");

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(10)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentAfterBoundarHasNonEmptySecondTextTier_ReturnsTrue()
		{
			_model.Tiers.AddTextTierWithEmptySegments("Garbled Speech");
			_model.Tiers[2].Segments[2].Text = "Dude, I'm not empty";

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(20)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsBoundaryPermanent_FinalSegmentBeforeBoundaryHasNonEmptyFirstTextTier_ReturnsTrue()
		{
			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);
			((TextTier)_model.Tiers[1]).AddSegment(string.Empty);
			((TextTier)_model.Tiers[1]).AddSegment("three");

			Assert.IsTrue(_model.IsBoundaryPermanent(TimeSpan.FromSeconds(30)));
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
	}
}
