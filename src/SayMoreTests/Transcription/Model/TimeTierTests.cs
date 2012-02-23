using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class TimeTierTests
	{
		private TimeTier _tier;
		private TemporaryFolder _tempFolder;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_tempFolder = new TemporaryFolder("TierCollectionTests");
			_tier = new TimeTier("test tier", Path.Combine(_tempFolder.Path, "mediaFile.wav"));
			_tier.AddSegment(10f, 20f);
			_tier.AddSegment(20f, 30f);
			_tier.AddSegment(30f, 40f);

			Assert.AreEqual("test tier", _tier.DisplayName);
			Assert.AreEqual(Path.Combine(_tempFolder.Path, "mediaFile.wav"), _tier.MediaFileName);
			Assert.AreEqual(Path.Combine(_tempFolder.Path, "mediaFile.wav_Annotations"), _tier.SegmentFileFolder);

			Assert.IsInstanceOf<AudioWaveFormColumn>(_tier.GridColumn);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			if (_tempFolder != null)
			{
				_tempFolder.Dispose();
				_tempFolder = null;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Copy_ReturnsDifferntInstance()
		{
			var copy = _tier.Copy();
			Assert.AreEqual(_tier.DisplayName, copy.DisplayName);
			Assert.AreNotSame(_tier, copy);
		}
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Copy_ReturnsDifferentInstanceOfTierWithSameProperties()
		{
			_tier.TierType = TierType.Time;
			_tier.LinguisticType = "blah";

			var copy = _tier.Copy();
			Assert.AreNotSame(_tier, copy);
			Assert.AreEqual(_tier.DisplayName, copy.DisplayName);
			Assert.AreEqual(_tier.TierType, copy.TierType);
			Assert.AreEqual(_tier.Locale, copy.Locale);
			Assert.AreEqual(_tier.LinguisticType, copy.LinguisticType);
			Assert.AreEqual(_tier.GridColumn.GetType(), copy.GridColumn.GetType());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegmentHavingEndBoundary_NoSegmentHasEndBoundary_ReturnsNull()
		{
			Assert.IsNull(_tier.GetSegmentHavingEndBoundary(15f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegmentHavingEndBoundary_SegmentHasEndBoundary_ReturnsSegment()
		{
			Assert.AreSame(_tier.Segments[1], _tier.GetSegmentHavingEndBoundary(30f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegmentEnclosingTime_BoundaryIsNegative_ReturnsNull()
		{
			Assert.IsNull(_tier.GetSegmentEnclosingTime(-1f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegmentEnclosingTime_BoundaryIsAfterAllSegments_ReturnsNull()
		{
			Assert.IsNull(_tier.GetSegmentEnclosingTime(50f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegmentEnclosingTime_IsSameAsBoundary_ReturnsSegmentToLeft()
		{
			var seg = _tier.GetSegmentEnclosingTime(20f);
			Assert.AreEqual(10f, seg.Start);
			Assert.AreEqual(20f, seg.End);
			Assert.AreEqual(_tier.Segments[0], seg);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegmentEnclosingTime_IsBetweenBoundaries_ReturnsCorrectSegment()
		{
			var seg = _tier.GetSegmentEnclosingTime(22.5f);
			Assert.AreEqual(20f, seg.Start);
			Assert.AreEqual(30f, seg.End);
			Assert.AreEqual(_tier.Segments[1], seg);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddSegment_PassTimes_AddsSegmentAndInitializesTimes()
		{
			Assert.AreEqual(3, _tier.Segments.Count);
			_tier.AddSegment(40f, 50f);

			Assert.AreEqual(4, _tier.Segments.Count);
			Assert.AreEqual(40f, _tier.Segments[3].Start);
			Assert.AreEqual(50f, _tier.Segments[3].End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddSegment_ReturnsAddedSegment()
		{
			var seg = _tier.AddSegment(40f, 50f);
			Assert.AreSame(seg, _tier.Segments[3]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_NoSegments_ReturnsFalse()
		{
			_tier.Segments.Clear();
			Segment segment;
			Assert.IsFalse(_tier.TryGetSegment(0, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_SegmentsExistButIndexOutOfRange_ReturnsFalse()
		{
			Segment segment;
			Assert.IsFalse(_tier.TryGetSegment(3, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_SegmentsExistIndexInRange_ReturnsTrue()
		{
			Segment segment;
			Assert.IsTrue(_tier.TryGetSegment(1, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_ByIndex_IndexOutOfRange_ReturnsFalse()
		{
			Assert.AreEqual(3, _tier.Segments.Count);
			Assert.IsFalse(_tier.RemoveSegment(-1));
			Assert.AreEqual(3, _tier.Segments.Count);
			Assert.IsFalse(_tier.RemoveSegment(3));
			Assert.AreEqual(3, _tier.Segments.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_ByIndex_RemoveMiddleSegment_JoinsWithPrecedingSegment()
		{
			Assert.IsTrue(_tier.RemoveSegment(1));
			Assert.AreEqual(2, _tier.Segments.Count);
			Assert.AreEqual(10f, _tier.Segments[0].Start);
			Assert.AreEqual(20f, _tier.Segments[0].End);
			Assert.AreEqual(20f, _tier.Segments[1].Start);
			Assert.AreEqual(40f, _tier.Segments[1].End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_ByIndex_RemoveFirstSegment_JoinsWithNextSegment()
		{
			Assert.IsTrue(_tier.RemoveSegment(0));
			Assert.AreEqual(2, _tier.Segments.Count());
			Assert.AreEqual(10f, _tier.Segments[0].Start);
			Assert.AreEqual(30f, _tier.Segments[0].End);
			Assert.AreEqual(30f, _tier.Segments[1].Start);
			Assert.AreEqual(40f, _tier.Segments[1].End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_ByIndex_RemoveLastSegment_DoesNotAffectPrecedingSegment()
		{
			Assert.IsTrue(_tier.RemoveSegment(2));
			Assert.AreEqual(2, _tier.Segments.Count());
			Assert.AreEqual(10f, _tier.Segments[0].Start);
			Assert.AreEqual(20f, _tier.Segments[0].End);
			Assert.AreEqual(20f, _tier.Segments[1].Start);
			Assert.AreEqual(30f, _tier.Segments[1].End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_ByIndex_RemoveFirstSegment_RemovesAndRenamesSegAnnotationFiles()
		{
			SetupSegmentFileFoldersForRealSegments();

			Assert.IsTrue(_tier.RemoveSegment(0));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Careful.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_30_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_30_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Translation.wav")));
			Assert.AreEqual(4, Directory.GetFiles(_tier.SegmentFileFolder).Length);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_ByIndex_RemoveMiddleSegment_RemovesAndRenamesSegAnnotationFiles()
		{
			SetupSegmentFileFoldersForRealSegments();

			Assert.IsTrue(_tier.RemoveSegment(1));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Careful.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_40_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_40_Translation.wav")));
			Assert.AreEqual(4, Directory.GetFiles(_tier.SegmentFileFolder).Length);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_ByIndex_RemoveLastSegment_RemovesSegAnnotationFiles()
		{
			SetupSegmentFileFoldersForRealSegments();

			Assert.IsTrue(_tier.RemoveSegment(2));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Careful.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Translation.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_BySegment_SegmentDoesNotExist_ReturnsFalse()
		{
			Assert.AreEqual(3, _tier.Segments.Count);
			Assert.IsFalse(_tier.RemoveSegment(new Segment(null, 10f, 19f)));
			Assert.AreEqual(3, _tier.Segments.Count);
			Assert.IsFalse(_tier.RemoveSegment(new Segment(null, 20f, 29.99f)));
			Assert.AreEqual(3, _tier.Segments.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_BySegment_RemoveSecondSegment_JoinsWithPrecedingSegment()
		{
			Assert.IsTrue(_tier.RemoveSegment(new Segment(null, 20f, 30f)));
			Assert.AreEqual(2, _tier.Segments.Count);
			Assert.AreEqual(10f, _tier.Segments[0].Start);
			Assert.AreEqual(20f, _tier.Segments[0].End);
			Assert.AreEqual(20f, _tier.Segments[1].Start);
			Assert.AreEqual(40f, _tier.Segments[1].End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_BySegment_RemoveFirstSegment_JoinsWithNextSegment()
		{
			Assert.IsTrue(_tier.RemoveSegment(new Segment(null, 10f, 20f)));
			Assert.AreEqual(2, _tier.Segments.Count());
			Assert.AreEqual(10f, _tier.Segments[0].Start);
			Assert.AreEqual(30f, _tier.Segments[0].End);
			Assert.AreEqual(30f, _tier.Segments[1].Start);
			Assert.AreEqual(40f, _tier.Segments[1].End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_BySegment_RemoveLastSegment_DoesNotAffectPrecedingSegment()
		{
			Assert.IsTrue(_tier.RemoveSegment(new Segment(null, 30f, 40f)));
			Assert.AreEqual(2, _tier.Segments.Count());
			Assert.AreEqual(10f, _tier.Segments[0].Start);
			Assert.AreEqual(20f, _tier.Segments[0].End);
			Assert.AreEqual(20f, _tier.Segments[1].Start);
			Assert.AreEqual(30f, _tier.Segments[1].End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_BySegment_RemoveFirstSegment_RemovesAndRenamesSegAnnotationFiles()
		{
			SetupSegmentFileFoldersForRealSegments();

			Assert.IsTrue(_tier.RemoveSegment(new Segment(null, 10f, 20f)));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Careful.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_30_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_30_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Translation.wav")));
			Assert.AreEqual(4, Directory.GetFiles(_tier.SegmentFileFolder).Length);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_BySegment_RemoveMiddleSegment_RemovesAndRenamesSegAnnotationFiles()
		{
			SetupSegmentFileFoldersForRealSegments();

			Assert.IsTrue(_tier.RemoveSegment(new Segment(null, 20f, 30f)));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Careful.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_40_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_40_Translation.wav")));
			Assert.AreEqual(4, Directory.GetFiles(_tier.SegmentFileFolder).Length);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_BySegment_RemoveLastSegment_RemovesSegAnnotationFiles()
		{
			SetupSegmentFileFoldersForRealSegments();

			Assert.IsTrue(_tier.RemoveSegment(new Segment(null, 30f, 40f)));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Careful.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Translation.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegmentHavingEndBoundary_NoSegmentForBoundary_ReturnsFalse()
		{
			Assert.IsFalse(_tier.RemoveSegmentHavingEndBoundary(25f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegmentHavingEndBoundary_SegmentHasBoundary_ReturnsTrue()
		{
			Assert.IsTrue(_tier.RemoveSegmentHavingEndBoundary(20f));
			Assert.IsTrue(_tier.RemoveSegmentHavingEndBoundary(40f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIndexOfSegment_SegmentIsNull_ReturnsNegOne()
		{
			Assert.AreEqual(-1, _tier.GetIndexOfSegment(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIndexOfSegment_SegmentDoesNotExist_ReturnsNegOne()
		{
			Assert.AreEqual(-1, _tier.GetIndexOfSegment(new Segment(null, 9.99f, 20f)));
			Assert.AreEqual(-1, _tier.GetIndexOfSegment(new Segment(null, 10f, 19.499f)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIndexOfSegment_SegmentExists_ReturnsIndex()
		{
			Assert.AreEqual(0, _tier.GetIndexOfSegment(new Segment(null, 10f, 20f)));
			Assert.AreEqual(2, _tier.GetIndexOfSegment(new Segment(null, 30f, 40f)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFileNameForCarefulSpeechSegment_PassNullSegment_ThrowsException()
		{
			Assert.Throws<NullReferenceException>(() => TimeTier.ComputeFileNameForCarefulSpeechSegment(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ComputeFileNameForCarefulSpeechSegment_PassGoodSegment_ReturnsCorrectFileName()
		{
			Assert.AreEqual("0_to_4.75_Careful.wav",
				TimeTier.ComputeFileNameForCarefulSpeechSegment(new Segment(null, 0f, 4.75f)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ComputeFileNameForCarefulSpeechSegment_PassGoodStartAndEnd_ReturnsCorrectFileName()
		{
			Assert.AreEqual("3.456_to_10.321_Careful.wav",
				TimeTier.ComputeFileNameForCarefulSpeechSegment(3.456f, 10.321f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ComputeFileNameForOralTranslationSegment_PassNullSegment_ThrowsException()
		{
			Assert.Throws<NullReferenceException>(() => TimeTier.ComputeFileNameForOralTranslationSegment(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ComputeFileNameForOralTranslationSegment_PassGoodSegment_ReturnsCorrectFileName()
		{
			Assert.AreEqual("0_to_4.75_Translation.wav",
				TimeTier.ComputeFileNameForOralTranslationSegment(new Segment(null, 0f, 4.75f)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFileNameForOralTranslationSegment_PassGoodStartAndEnd_ReturnsCorrectFileName()
		{
			Assert.AreEqual("3.456_to_10.321_Translation.wav",
				TimeTier.ComputeFileNameForOralTranslationSegment(3.456f, 10.321f));
		}

		/// ------------------------------------------------------------------------------------
		private void SetupSegmentFileFolders()
		{
			Directory.CreateDirectory(_tier.SegmentFileFolder);

			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "0_to_2_Careful.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Careful.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "4.5_to_5.35_Careful.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "0_to_2_Translation.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Translation.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "4.5_to_5.35_Translation.wav")).Close();

			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "0_to_2_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "4.5_to_5.35_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "0_to_2_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "4.5_to_5.35_Translation.wav")));
		}

		/// ------------------------------------------------------------------------------------
		private void SetupSegmentFileFoldersForRealSegments()
		{
			Directory.CreateDirectory(_tier.SegmentFileFolder);

			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Careful.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Careful.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Careful.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Translation.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Translation.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Translation.wav")).Close();

			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Translation.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RenameAnnotationSegmentFile_CorrectlyRenamesCarefulSpeech()
		{
			SetupSegmentFileFolders();

			_tier.RenameAnnotationSegmentFile(new Segment(null, 2f, 4.5f), 6.234f, 10.587f);
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "6.234_to_10.587_Careful.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RenameAnnotationSegmentFile_CorrectlyRenamesTranslations()
		{
			SetupSegmentFileFolders();

			_tier.RenameAnnotationSegmentFile(new Segment(null, 2f, 4.5f), 6.234f, 10.587f);
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "6.234_to_10.587_Translation.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteAnnotationSegmentFile_DeleteCarerfulSpeech()
		{
			SetupSegmentFileFolders();

			_tier.DeleteAnnotationSegmentFile(new Segment(null, 2f, 4.5f));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Careful.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteAnnotationSegmentFile_DeleteTranslation()
		{
			SetupSegmentFileFolders();

			_tier.DeleteAnnotationSegmentFile(new Segment(null, 6.234f, 10.587f));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Careful.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeSegmentsEndBoundary_FromBoundary_BoundaryNotFound_ReturnsNotSuccess()
		{
			Assert.AreEqual(BoundaryModificationResult.SegmentNotFound, _tier.ChangeSegmentsEndBoundary(12f, 25f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeSegmentsEndBoundary_FromBoundary_BoundaryFound_ReturnsSuccess()
		{
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.ChangeSegmentsEndBoundary(20f, 25f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeSegmentsEndBoundary_WhenSegmentDoesNotExist_ReturnsNotSuccess()
		{
			var segment = new Segment(null, 2.5f, 4.5f);
			Assert.AreEqual(BoundaryModificationResult.SegmentNotFound, _tier.ChangeSegmentsEndBoundary(segment, 25f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeSegmentsEndBoundary_NewBoundaryTooCloseToStart_ReturnsNotSuccess()
		{
			var segment = _tier.Segments[1];
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.ChangeSegmentsEndBoundary(segment, 20.51f));
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _tier.ChangeSegmentsEndBoundary(segment, 20.5f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeSegmentsEndBoundary_NewBoundaryTooCloseToNextSegEnd_ReturnsNotSuccess()
		{
			var segment = _tier.Segments[1];
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.ChangeSegmentsEndBoundary(segment, 39.49f));
			Assert.AreEqual(BoundaryModificationResult.NextSegmentWillBeTooShort, _tier.ChangeSegmentsEndBoundary(segment, 39.5f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeSegmentsEndBoundary_WhenSegmentIsLast_ChangesSegAndReturnsSuccess()
		{
			var segment = _tier.Segments[2];
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.ChangeSegmentsEndBoundary(segment, 35f));
			Assert.AreEqual(35f, segment.End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeSegmentsEndBoundary_WhenSegmentIsNotLast_ChangesSegAndNextSegAndReturnsSuccess()
		{
			var segment = _tier.Segments[1];
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.ChangeSegmentsEndBoundary(segment, 25f));
			Assert.AreEqual(25f, segment.End);
			var nextSegment = _tier.Segments[2];
			Assert.AreEqual(25f, nextSegment.Start);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeSegmentsEndBoundary_WhenSegmentIsLast_RenamesSegAnnotationFiles()
		{
			SetupSegmentFileFoldersForRealSegments();

			var segment = _tier.Segments[2];
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.ChangeSegmentsEndBoundary(segment, 35f));

			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_35_Careful.wav")));

			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_35_Translation.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeSegmentsEndBoundary_WhenSegmentIsNotLast_RenamesSegAnnotationFiles()
		{
			SetupSegmentFileFoldersForRealSegments();

			var segment = _tier.Segments[1];
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.ChangeSegmentsEndBoundary(segment, 25f));

			// Check files for the segment whose end boundary changed
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_25_Careful.wav")));

			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_25_Translation.wav")));

			// Check files for the next segment, whose start boundary changed
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "25_to_40_Careful.wav")));

			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "25_to_40_Translation.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertSegmentBoundary_NewBoundaryTooCloseToPrecedingBoundary_ReturnsNotSuccess()
		{
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _tier.InsertSegmentBoundary(20.4f));
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _tier.InsertSegmentBoundary(20.49f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertSegmentBoundary_NewBoundaryTooCloseToFollowingBoundary_ReturnsNotSuccess()
		{
			Assert.AreEqual(BoundaryModificationResult.NextSegmentWillBeTooShort, _tier.InsertSegmentBoundary(29.5f));
			Assert.AreEqual(BoundaryModificationResult.NextSegmentWillBeTooShort, _tier.InsertSegmentBoundary(29.51f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertSegmentBoundary_WhenNoSegmentsAndNewBoundaryTooSmall_ReturnsNotSuccess()
		{
			_tier.Segments.Clear();
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _tier.InsertSegmentBoundary(0.5f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertSegmentBoundary_WhenNoSegmentsAndNewBoundaryIsValid_CreatesSegmentAndReturnsSuccess()
		{
			_tier.Segments.Clear();
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.InsertSegmentBoundary(0.501f));
			Assert.AreEqual(1, _tier.Segments.Count);
			Assert.AreEqual(0f, _tier.Segments[0].Start);
			Assert.AreEqual(0.501f, _tier.Segments[0].End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertSegmentBoundary_NewBoundaryAfterLastSegment_CreatesSegmentAndReturnsSuccess()
		{
			Assert.AreEqual(3, _tier.Segments.Count);
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.InsertSegmentBoundary(45f));
			Assert.AreEqual(4, _tier.Segments.Count);
			Assert.AreEqual(40f, _tier.Segments[3].Start);
			Assert.AreEqual(45f, _tier.Segments[3].End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertSegmentBoundary_NewBoundaryInsertedInMiddleOfExistingSegment_InsertsSegmentAndReturnsSuccess()
		{
			Assert.AreEqual(3, _tier.Segments.Count);
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.InsertSegmentBoundary(35f));
			Assert.AreEqual(4, _tier.Segments.Count);
			Assert.AreEqual(30f, _tier.Segments[2].Start);
			Assert.AreEqual(35f, _tier.Segments[2].End);
			Assert.AreEqual(35f, _tier.Segments[3].Start);
			Assert.AreEqual(40f, _tier.Segments[3].End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertSegmentBoundary_NewBoundaryInsertedInMiddleOfExistingSegment_RenamesPrecedingSegmentFilesAndReturnsSuccess()
		{
			SetupSegmentFileFoldersForRealSegments();

			Assert.AreEqual(BoundaryModificationResult.Success, _tier.InsertSegmentBoundary(25f));

			// Check files for the segment whose end boundary changed by the insertion
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_25_Careful.wav")));

			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_25_Translation.wav")));

			// Verify the files for the next segment did not change
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Translation.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanBoundaryMoveLeft_MoveWillPutNewBoundaryAtZero_ReturnsFalse()
		{
			Assert.IsFalse(_tier.CanBoundaryMoveLeft(0.5f, 0.5f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanBoundaryMoveLeft_MoveWillYieldNewNegativeBoundary_ReturnsFalse()
		{
			Assert.IsFalse(_tier.CanBoundaryMoveLeft(0.5f, 0.6f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanBoundaryMoveLeft_MoveWillResultInTooShortSegment_ReturnsFalse()
		{
			Assert.IsFalse(_tier.CanBoundaryMoveLeft(12f, 2f));
			Assert.IsFalse(_tier.CanBoundaryMoveLeft(12f, 1.5f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanBoundaryMoveLeft_MoveWillResultInBigEnoughSegment_ReturnsTrue()
		{
			Assert.IsTrue(_tier.CanBoundaryMoveLeft(13f, 2f));
			Assert.IsTrue(_tier.CanBoundaryMoveLeft(12f, 1.49f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanBoundaryMoveRight_MoveWillPutNewBoundaryBeyondLimit_ReturnsFalse()
		{
			Assert.IsFalse(_tier.CanBoundaryMoveRight(40f, 2f, 41f));
			Assert.IsFalse(_tier.CanBoundaryMoveRight(40f, 2f, 41.99f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanBoundaryMoveRight_MoveWillPutNewBoundaryOnLimit_ReturnsTrue()
		{
			Assert.IsTrue(_tier.CanBoundaryMoveRight(40f, 2f, 42f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanBoundaryMoveRight_MoveWillResultInTooShortSegment_ReturnsFalse()
		{
			Assert.IsFalse(_tier.CanBoundaryMoveRight(28f, 2.01f, 100f));
			Assert.IsFalse(_tier.CanBoundaryMoveRight(28f, 1.501f, 100f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanBoundaryMoveRight_MoveWillResultInBigEnoughSegment_ReturnsTrue()
		{
			Assert.IsTrue(_tier.CanBoundaryMoveLeft(28f, 0.5f));
			Assert.IsTrue(_tier.CanBoundaryMoveLeft(28f, 1.5f));
		}
	}
}
