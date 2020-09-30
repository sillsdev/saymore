using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;
using SayMoreTests.Model.Files;

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
			var tempMediaPath = MediaFileInfoTests.GetLongerTestAudioFile();
			_tempFolder = new TemporaryFolder("TierCollectionTests");
			var mediaFile = Path.Combine(_tempFolder.Path, "mediaFile.wav");
			File.Move(tempMediaPath, mediaFile);
			_tier = new TimeTier("test tier", mediaFile);
			_tier.AddSegment(10f, 20f);
			_tier.AddSegment(20f, 30f);
			_tier.AddSegment(30f, 40f);

			Assert.AreEqual("test tier", _tier.Id);
			Assert.AreEqual(string.Empty, _tier.DisplayName);
			Assert.AreEqual(mediaFile, _tier.MediaFileName);
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
			AnnotationSegment segment;
			Assert.IsFalse(_tier.TryGetSegment(0, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_SegmentsExistButIndexOutOfRange_ReturnsFalse()
		{
			AnnotationSegment segment;
			Assert.IsFalse(_tier.TryGetSegment(3, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_SegmentsExistIndexInRange_ReturnsTrue()
		{
			AnnotationSegment segment;
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
			CreateAnnotationFilesForSegmentsCreatedInSetup();

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
			CreateAnnotationFilesForSegmentsCreatedInSetup();

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
			CreateAnnotationFilesForSegmentsCreatedInSetup();

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
			Assert.IsFalse(_tier.RemoveSegment(new AnnotationSegment(null, 10f, 19f)));
			Assert.AreEqual(3, _tier.Segments.Count);
			Assert.IsFalse(_tier.RemoveSegment(new AnnotationSegment(null, 20f, 29.99f)));
			Assert.AreEqual(3, _tier.Segments.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_BySegment_RemoveSecondSegment_JoinsWithPrecedingSegment()
		{
			Assert.IsTrue(_tier.RemoveSegment(new AnnotationSegment(null, 20f, 30f)));
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
			Assert.IsTrue(_tier.RemoveSegment(new AnnotationSegment(null, 10f, 20f)));
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
			Assert.IsTrue(_tier.RemoveSegment(new AnnotationSegment(null, 30f, 40f)));
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
			CreateAnnotationFilesForSegmentsCreatedInSetup();

			Assert.IsTrue(_tier.RemoveSegment(new AnnotationSegment(null, 10f, 20f)));
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
			CreateAnnotationFilesForSegmentsCreatedInSetup();

			Assert.IsTrue(_tier.RemoveSegment(new AnnotationSegment(null, 20f, 30f)));
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
			CreateAnnotationFilesForSegmentsCreatedInSetup();

			Assert.IsTrue(_tier.RemoveSegment(new AnnotationSegment(null, 30f, 40f)));
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
			Assert.AreEqual(-1, _tier.GetIndexOfSegment(new AnnotationSegment(null, 9.99f, 20f)));
			Assert.AreEqual(-1, _tier.GetIndexOfSegment(new AnnotationSegment(null, 10f, 19.499f)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIndexOfSegment_SegmentExists_ReturnsIndex()
		{
			Assert.AreEqual(0, _tier.GetIndexOfSegment(new AnnotationSegment(null, 10f, 20f)));
			Assert.AreEqual(2, _tier.GetIndexOfSegment(new AnnotationSegment(null, 30f, 40f)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFileNameForCarefulSpeechSegment_PassNullSegment_ThrowsException()
		{
			Assert.Throws<NullReferenceException>(() => TimeTier.ComputeFileNameForCarefulSpeechSegment(null as AnnotationSegment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToCarefulSpeechFile_PassNullSegment_ThrowsException()
		{
			Assert.Throws<NullReferenceException>(() => _tier.GetFullPathToCarefulSpeechFile(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToCarefulSpeechFile_PassGoodSegment_ReturnsCorrectPath()
		{
			var filepath = _tier.GetFullPathToCarefulSpeechFile(new AnnotationSegment(null, 3f, 5f));
			Assert.AreEqual(_tier.SegmentFileFolder, Path.GetDirectoryName(filepath));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToOralTranslationFile_PassNullSegment_ThrowsException()
		{
			Assert.Throws<NullReferenceException>(() => _tier.GetFullPathToOralTranslationFile(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullPathToOralTranslationFile_PassGoodSegment_ReturnsCorrectPath()
		{
			var filepath = _tier.GetFullPathToOralTranslationFile(new AnnotationSegment(null, 3f, 5f));
			Assert.AreEqual(_tier.SegmentFileFolder, Path.GetDirectoryName(filepath));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ComputeFileNameForCarefulSpeechSegment_PassGoodSegment_ReturnsCorrectFileName()
		{
			Assert.AreEqual("0_to_4.75_Careful.wav",
				TimeTier.ComputeFileNameForCarefulSpeechSegment(new AnnotationSegment(null, 0f, 4.75f)));
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
			Assert.Throws<NullReferenceException>(() => TimeTier.ComputeFileNameForOralTranslationSegment(null as AnnotationSegment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ComputeFileNameForOralTranslationSegment_PassGoodSegment_ReturnsCorrectFileName()
		{
			Assert.AreEqual("0_to_4.75_Translation.wav",
				TimeTier.ComputeFileNameForOralTranslationSegment(new AnnotationSegment(null, 0f, 4.75f)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFileNameForOralTranslationSegment_PassGoodStartAndEnd_ReturnsCorrectFileName()
		{
			Assert.AreEqual("3.456_to_10.321_Translation.wav",
				TimeTier.ComputeFileNameForOralTranslationSegment(3.456f, 10.321f));
		}

		#region GetTotalAnnotatedTime tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalAnnotatedTime_NoSegments_ReturnsZero()
		{
			_tier.Segments.Clear();
			Assert.AreEqual(TimeSpan.Zero, _tier.GetTotalAnnotatedTime(OralAnnotationType.CarefulSpeech));
			Assert.AreEqual(TimeSpan.Zero, _tier.GetTotalAnnotatedTime(OralAnnotationType.Translation));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalAnnotatedTime_NoAnnotations_ReturnsZero()
		{
			Assert.AreEqual(TimeSpan.Zero, _tier.GetTotalAnnotatedTime(OralAnnotationType.CarefulSpeech));
			Assert.AreEqual(TimeSpan.Zero, _tier.GetTotalAnnotatedTime(OralAnnotationType.Translation));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalAnnotatedTime_SomeSegmentsHaveAnnotations_ReturnsTotalAnnotatedTime()
		{
			Directory.CreateDirectory(_tier.SegmentFileFolder);
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "10_to_20_Careful.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Careful.wav")).Close();
			File.OpenWrite(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Translation.wav")).Close();
			Assert.AreEqual(TimeSpan.FromSeconds(20), _tier.GetTotalAnnotatedTime(OralAnnotationType.CarefulSpeech));
			Assert.AreEqual(TimeSpan.FromSeconds(10), _tier.GetTotalAnnotatedTime(OralAnnotationType.Translation));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalAnnotatedTime_NotFullySegmentedButAllSegmentsHaveAnnotations_ReturnsTotalAnnotatedTime()
		{
			CreateAnnotationFilesForSegmentsCreatedInSetup();
			Assert.AreEqual(TimeSpan.FromSeconds(30), _tier.GetTotalAnnotatedTime(OralAnnotationType.CarefulSpeech));
			Assert.AreEqual(TimeSpan.FromSeconds(30), _tier.GetTotalAnnotatedTime(OralAnnotationType.Translation));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalAnnotatedTime_FullyAnnotated_ReturnsLengthOfMediaFile()
		{
			CreateAnnotationFilesForSegmentsCreatedInSetup();
			var totalTime = (float)_tier.TotalTime.TotalSeconds;
			CreateAndAnnotateSegment(0, 10f);
			CreateAndAnnotateSegment(40f, totalTime);

			Assert.AreEqual(_tier.TotalTime, _tier.GetTotalAnnotatedTime(OralAnnotationType.CarefulSpeech));
			Assert.AreEqual(_tier.TotalTime, _tier.GetTotalAnnotatedTime(OralAnnotationType.Translation));
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		private void CreateAndAnnotateSegment(float startTime, float endTime)
		{
			CreateAndAnnotateSegment(_tier, startTime, endTime);
		}

		/// ------------------------------------------------------------------------------------
		internal static void CreateAndAnnotateSegment(TimeTier tier, float startTime, float endTime)
		{
			var segment = tier.AddSegment(startTime, endTime);
			File.OpenWrite(Path.Combine(tier.SegmentFileFolder, tier.GetFullPathToCarefulSpeechFile(segment))).Close();
			File.OpenWrite(Path.Combine(tier.SegmentFileFolder, tier.GetFullPathToOralTranslationFile(segment))).Close();
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
		private void CreateAnnotationFilesForSegmentsCreatedInSetup()
		{
			CreateAnnotationFiles(_tier, "10_to_20_Careful.wav", "20_to_30_Careful.wav", "30_to_40_Careful.wav",
				"10_to_20_Translation.wav", "20_to_30_Translation.wav", "30_to_40_Translation.wav");
		}

		/// ------------------------------------------------------------------------------------
		internal static void CreateAnnotationFiles(TimeTier tier, params string[] files)
		{
			Directory.CreateDirectory(tier.SegmentFileFolder);

			foreach (var file in files)
				File.OpenWrite(Path.Combine(tier.SegmentFileFolder, file)).Close();

			foreach (var file in files)
				Assert.IsTrue(File.Exists(Path.Combine(tier.SegmentFileFolder, file)));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RenameAnnotationSegmentFile_CorrectlyRenamesCarefulSpeech()
		{
			SetupSegmentFileFolders();

			_tier.RenameAnnotationSegmentFile(new AnnotationSegment(null, 2f, 4.5f), 6.234f, 10.587f);
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "6.234_to_10.587_Careful.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RenameAnnotationSegmentFile_CorrectlyRenamesTranslations()
		{
			SetupSegmentFileFolders();

			_tier.RenameAnnotationSegmentFile(new AnnotationSegment(null, 2f, 4.5f), 6.234f, 10.587f);
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Translation.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "6.234_to_10.587_Translation.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RenameAnnotationSegmentFile_AnnotationFileDoesNotExist_BackupOralAnnotationSegmentFileActionNotCalled()
		{
			SetupSegmentFileFolders();

			bool backupCalled = false;
			bool deleteFlag = true;
			_tier.BackupOralAnnotationSegmentFileAction = (f, g) => { backupCalled = true; deleteFlag = g; };
			_tier.RenameAnnotationSegmentFile(new AnnotationSegment(null, 3f, 4.5f), 6.234f, 10.587f);
			Assert.IsFalse(backupCalled);
			Assert.IsTrue(deleteFlag);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RenameAnnotationSegmentFile_AnnotationFileExists_BackupOralAnnotationSegmentFileActionCalled()
		{
			SetupSegmentFileFolders();

			bool backupCalled = false;
			bool deleteFlag = true;
			_tier.BackupOralAnnotationSegmentFileAction = (f, g) => { backupCalled = true; deleteFlag = g; };
			_tier.RenameAnnotationSegmentFile(new AnnotationSegment(null, 2f, 4.5f), 6.234f, 10.587f);
			Assert.IsTrue(backupCalled);
			Assert.IsFalse(deleteFlag);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteAnnotationSegmentFile_DeleteCarefulSpeech()
		{
			SetupSegmentFileFolders();

			_tier.DeleteAnnotationSegmentFile(new AnnotationSegment(null, 2f, 4.5f));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Careful.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteAnnotationSegmentFile_DeleteTranslation()
		{
			SetupSegmentFileFolders();

			_tier.DeleteAnnotationSegmentFile(new AnnotationSegment(null, 6.234f, 10.587f));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "2_to_4.5_Careful.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteAnnotationSegmentFile_AnnotationFileDoesNotExist_BackupOralAnnotationSegmentFileActionNotCalled()
		{
			SetupSegmentFileFolders();

			bool backupCalled = false;
			bool deleteFlag = false;
			_tier.BackupOralAnnotationSegmentFileAction = (f, g) => { backupCalled = true; deleteFlag = g; };
			_tier.DeleteAnnotationSegmentFile(new AnnotationSegment(null, 3f, 4.5f));
			Assert.IsFalse(backupCalled);
			Assert.IsFalse(deleteFlag);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteAnnotationSegmentFile_AnnotationFileExists_BackupOralAnnotationSegmentFileActionCalled()
		{
			SetupSegmentFileFolders();

			bool backupCalled = false;
			bool deleteFlag = false;
			_tier.BackupOralAnnotationSegmentFileAction = (f, g) => { backupCalled = true; deleteFlag = g; };
			_tier.DeleteAnnotationSegmentFile(new AnnotationSegment(null, 2f, 4.5f));
			Assert.IsTrue(backupCalled);
			Assert.IsTrue(deleteFlag);
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
			var segment = new AnnotationSegment(null, 2.5f, 4.5f);
			Assert.AreEqual(BoundaryModificationResult.SegmentNotFound, _tier.ChangeSegmentsEndBoundary(segment, 25f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeSegmentsEndBoundary_NewBoundaryTooCloseToStart_ReturnsNotSuccess()
		{
			var segment = _tier.Segments[1];
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.ChangeSegmentsEndBoundary(segment,
				20f + SayMore.Properties.Settings.Default.MinimumSegmentLengthInMilliseconds / 1000f));
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _tier.ChangeSegmentsEndBoundary(segment,
				19.99f + SayMore.Properties.Settings.Default.MinimumSegmentLengthInMilliseconds / 1000f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChangeSegmentsEndBoundary_NewBoundaryTooCloseToNextSegEnd_ReturnsNotSuccess()
		{
			var segment = _tier.Segments[1];
			var endOfNextSegment = _tier.Segments[2].End;

			/*
			 * We are adding and subtracting 0.01 seconds to mitigate floating-point errors
			 * that can cause the tests to fail when they should indeed pass.
			 *
			 * Example: 40 - 39.15f = 0.8499985 (instead of 0.850)
			 */
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.ChangeSegmentsEndBoundary(segment,
				(endOfNextSegment - 0.01f) - SayMore.Properties.Settings.Default.MinimumSegmentLengthInMilliseconds / 1000f));
			Assert.AreEqual(BoundaryModificationResult.NextSegmentWillBeTooShort, _tier.ChangeSegmentsEndBoundary(segment,
				(endOfNextSegment + 0.01f)  - SayMore.Properties.Settings.Default.MinimumSegmentLengthInMilliseconds / 1000f));
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
			CreateAnnotationFilesForSegmentsCreatedInSetup();

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
			CreateAnnotationFilesForSegmentsCreatedInSetup();

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
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _tier.InsertSegmentBoundary(20.45f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertSegmentBoundary_NewBoundaryTooCloseToFollowingBoundary_ReturnsNotSuccess()
		{
			Assert.AreEqual(BoundaryModificationResult.NextSegmentWillBeTooShort, _tier.InsertSegmentBoundary(29.541f));
			Assert.AreEqual(BoundaryModificationResult.NextSegmentWillBeTooShort, _tier.InsertSegmentBoundary(29.55f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertSegmentBoundary_WhenNoSegmentsAndNewBoundaryTooSmall_ReturnsNotSuccess()
		{
			_tier.Segments.Clear();
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _tier.InsertSegmentBoundary(0.4599f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertSegmentBoundary_WhenNoSegmentsAndNewBoundaryIsValid_CreatesSegmentAndReturnsSuccess()
		{
			_tier.Segments.Clear();
			var end = SayMore.Properties.Settings.Default.MinimumSegmentLengthInMilliseconds / 1000f;
			Assert.AreEqual(BoundaryModificationResult.Success, _tier.InsertSegmentBoundary(end));
			Assert.AreEqual(1, _tier.Segments.Count);
			Assert.AreEqual(0f, _tier.Segments[0].Start);
			Assert.AreEqual(end, _tier.Segments[0].End);
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
		public void InsertSegmentBoundary_NewBoundaryInsertedInMiddleOfExistingSegmentDeletionsAllowed_ReturnsSuccess()
		{
			CreateAnnotationFilesForSegmentsCreatedInSetup();

			Assert.AreEqual(BoundaryModificationResult.Success, _tier.InsertSegmentBoundary(25f, (s, b1, b2) => true));

			// The time tier is no longer responsible for the deleting/renaming annotation files,
			// so the pre-existing files should still be present.
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Careful.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_25_Careful.wav")));

			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Translation.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_25_Translation.wav")));

			// Verify the files for the next segment did not change
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Careful.wav")));
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "30_to_40_Translation.wav")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertSegmentBoundary_NewBoundaryInsertedInMiddleOfExistingSegmentDisallowed_ReturnsBlockedByOralAnnotations()
		{
			CreateAnnotationFilesForSegmentsCreatedInSetup();

			Assert.AreEqual(BoundaryModificationResult.BlockedByOralAnnotations, _tier.InsertSegmentBoundary(25f, (s, b1, b2) => false));

			// Check files for the segment whose end boundary changed by the insertion
			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Careful.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_25_Careful.wav")));

			Assert.IsTrue(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_30_Translation.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(_tier.SegmentFileFolder, "20_to_25_Translation.wav")));

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
			Assert.IsFalse(_tier.CanBoundaryMoveLeft(12f, 2.0f));
			Assert.IsFalse(_tier.CanBoundaryMoveLeft(12f, 1.541f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanBoundaryMoveLeft_MoveWillResultInBigEnoughSegment_ReturnsTrue()
		{
			Assert.IsTrue(_tier.CanBoundaryMoveLeft(13f, 2f));
			Assert.IsTrue(_tier.CanBoundaryMoveLeft(12f, .99f - SayMore.Properties.Settings.Default.MinimumSegmentLengthInMilliseconds / 1000f));
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
			Assert.IsFalse(_tier.CanBoundaryMoveRight(28f, 1.541f, 100f));
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
