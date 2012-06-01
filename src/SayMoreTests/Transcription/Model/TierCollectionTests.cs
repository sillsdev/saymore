using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Transcription.Model;
using SayMoreTests.Model.Files;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class TierCollectionTests
	{
		private TierCollection _collection;
		private TemporaryFolder _tempFolder;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			var tempMediaPath = MediaFileInfoTests.GetLongerTestAudioFile();
			_tempFolder = new TemporaryFolder("TierCollectionTests");
			var mediaFile = Path.Combine(_tempFolder.Path, "mediaFile.wav");
			File.Move(tempMediaPath, mediaFile);

			_collection = new TierCollection(mediaFile);
			_collection.Clear();
			var timeTier = new TimeTier("timeTier", mediaFile);
			_collection.Add(timeTier);

			var transcriptionTier = new TextTier(TextTier.ElanTranscriptionTierId);
			_collection.Add(transcriptionTier);

			var translationTier = new TextTier(TextTier.ElanTranslationTierId);
			_collection.Add(translationTier);

			var otherTextTier = new TextTier("otherTextTier");
			_collection.Add(otherTextTier);

			timeTier.AddSegment(10f, 20f);
			timeTier.AddSegment(20f, 30f);
			timeTier.AddSegment(30f, 40f);

			transcriptionTier.AddSegment("trans1");
			transcriptionTier.AddSegment("trans2");
			transcriptionTier.AddSegment("trans3");

			translationTier.AddSegment("free1");
			translationTier.AddSegment("free2");
			translationTier.AddSegment(null);

			otherTextTier.AddSegment("other1");
			otherTextTier.AddSegment(null);
			otherTextTier.AddSegment(null);

			Assert.AreEqual(mediaFile, _collection.AnnotatedMediaFile);
			Assert.AreEqual(4, _collection.Count);
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
		public void Copy_CreatesCopiesOfEachTier()
		{
			var copy = _collection.Copy();
			Assert.AreEqual(4, copy.Count);
			Assert.AreNotSame(copy, _collection);
			Assert.AreNotSame(copy[0], _collection[0]);
			Assert.AreNotSame(copy[1], _collection[1]);
			Assert.AreNotSame(copy[2], _collection[2]);
			Assert.AreNotSame(copy[3], _collection[3]);

			Assert.AreEqual(copy[0].DisplayName, _collection[0].DisplayName);
			Assert.AreEqual(copy[1].DisplayName, _collection[1].DisplayName);
			Assert.AreEqual(copy[2].DisplayName, _collection[2].DisplayName);
			Assert.AreEqual(copy[3].DisplayName, _collection[3].DisplayName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Copy_ReturnsDifferentInstanceWithSameProperties()
		{
			var copy = _collection.Copy();
			Assert.AreNotSame(_collection, copy);
			Assert.AreEqual(_collection.AnnotatedMediaFile, copy.AnnotatedMediaFile);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoTimeSegmentsExist_WhenSegmentsDoNotExist_ReturnsFalse()
		{
			_collection.GetTimeTier().Segments.Clear();
			Assert.IsFalse(_collection.GetDoTimeSegmentsExist());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDoTimeSegmentsExist_WhenSegmentsExist_ReturnsTrue()
		{
			Assert.IsTrue(_collection.GetDoTimeSegmentsExist());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveTierSegments_IndexOutOfRangeLow_ReturnFalse()
		{
			Assert.IsFalse(_collection.RemoveTierSegments(-1));
			_collection.Clear();
			Assert.IsFalse(_collection.RemoveTierSegments(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveTierSegments_IndexOutOfRangeHigh_ReturnFalse()
		{
			Assert.IsFalse(_collection.RemoveTierSegments(3));
			_collection.Clear();
			Assert.IsFalse(_collection.RemoveTierSegments(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveTierSegments_IndexInRange_ReturnTrue()
		{
			Assert.AreEqual(3, _collection[0].Segments.Count);
			Assert.AreEqual(3, _collection[1].Segments.Count);
			Assert.AreEqual(3, _collection[2].Segments.Count);

			Assert.IsTrue(_collection.RemoveTierSegments(2));
			Assert.AreEqual(2, _collection[0].Segments.Count);
			Assert.AreEqual(2, _collection[1].Segments.Count);
			Assert.AreEqual(2, _collection[2].Segments.Count);

			Assert.IsTrue(_collection.RemoveTierSegments(0));
			Assert.AreEqual(1, _collection[0].Segments.Count);
			Assert.AreEqual(1, _collection[1].Segments.Count);
			Assert.AreEqual(1, _collection[2].Segments.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertTierSegment_BoundaryNegative_ReturnsNotSuccess()
		{
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _collection.InsertTierSegment(-1f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertTierSegment_BoundaryIsSameAsExisting_ReturnsNotSuccess()
		{
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _collection.InsertTierSegment(10f));
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _collection.InsertTierSegment(20f));
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _collection.InsertTierSegment(30f));
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _collection.InsertTierSegment(40f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertTierSegment_BoundaryIsTooCloseToPrevious_ReturnsNotSuccess()
		{
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _collection.InsertTierSegment(10.49f));
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _collection.InsertTierSegment(30.4f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertTierSegment_BoundaryIsTooCloseToNext_ReturnsNotSuccess()
		{
			Assert.AreEqual(BoundaryModificationResult.NextSegmentWillBeTooShort, _collection.InsertTierSegment(19.501f));
			Assert.AreEqual(BoundaryModificationResult.NextSegmentWillBeTooShort, _collection.InsertTierSegment(29.501f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertTierSegment_BoundaryIsBeyondLast_InsertsAndReturnsSuccess()
		{
			var segs = _collection.GetTimeTier().Segments;
			Assert.AreEqual(3, segs.Count);

			var endOfSeg3 = 40.001f + SayMore.Properties.Settings.Default.MinimumSegmentLengthInMilliseconds / 1000f;
			Assert.AreEqual(BoundaryModificationResult.Success, _collection.InsertTierSegment(endOfSeg3));
			Assert.AreEqual(4, segs.Count);
			Assert.IsTrue(segs[3].StartsAt(40f));
			Assert.IsTrue(segs[3].EndsAt(endOfSeg3));

			Assert.AreEqual(BoundaryModificationResult.Success, _collection.InsertTierSegment(50f));
			Assert.AreEqual(5, _collection[0].Segments.Count);
			Assert.IsTrue(segs[4].StartsAt(endOfSeg3));
			Assert.IsTrue(segs[4].EndsAt(50f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertTierSegment_BoundaryIsBetweenExisting_InsertsAndReturnsSuccess()
		{
			var segs = _collection.GetTimeTier().Segments;
			Assert.AreEqual(3, segs.Count);
			Assert.AreEqual(20f, segs[1].Start);
			Assert.AreEqual(30f, segs[1].End);

			Assert.AreEqual(BoundaryModificationResult.Success, _collection.InsertTierSegment(25f));
			Assert.AreEqual(4, segs.Count);
			Assert.AreEqual(20f, segs[1].Start);
			Assert.AreEqual(25f, segs[1].End);
			Assert.AreEqual(25f, segs[2].Start);
			Assert.AreEqual(30f, segs[2].End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertTierSegment_BoundaryIsBetweenExisting_InsertsInTextTiers()
		{
			Assert.AreEqual(3, _collection[0].Segments.Count);
			Assert.AreEqual(3, _collection[1].Segments.Count);
			Assert.AreEqual(3, _collection[2].Segments.Count);

			_collection[1].Segments[0].Text = "1a";
			_collection[1].Segments[1].Text = "2a";
			_collection[1].Segments[2].Text = "3a";
			_collection[2].Segments[0].Text = "1b";
			_collection[2].Segments[1].Text = "2b";
			_collection[2].Segments[2].Text = "3b";

			Assert.AreEqual(BoundaryModificationResult.Success, _collection.InsertTierSegment(25f));

			Assert.AreEqual(4, _collection[0].Segments.Count);
			Assert.AreEqual(4, _collection[1].Segments.Count);
			Assert.AreEqual(4, _collection[2].Segments.Count);

			Assert.AreEqual("1a", _collection[1].Segments[0].Text);
			Assert.IsEmpty(_collection[1].Segments[1].Text);
			Assert.AreEqual("2a", _collection[1].Segments[2].Text);

			Assert.AreEqual("1b", _collection[2].Segments[0].Text);
			Assert.IsEmpty(_collection[2].Segments[1].Text);
			Assert.AreEqual("2b", _collection[2].Segments[2].Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeTier_TierExists_ReturnsIt()
		{
			var tier = _collection.GetTimeTier();
			Assert.AreEqual(string.Empty, tier.DisplayName);
			Assert.AreEqual("timeTier", tier.Id);
			Assert.AreEqual(TierType.Time, tier.TierType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeTier_TierDoesNotExist_ReturnsNull()
		{
			_collection.RemoveAt(0);
			Assert.IsNull(_collection.GetTimeTier());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTranscriptionTier_TierExists_ReturnsIt()
		{
			var tier = _collection.GetTranscriptionTier();
			Assert.AreEqual(TextTier.ElanTranscriptionTierId, tier.DisplayName);
			Assert.AreEqual(TierType.Transcription, tier.TierType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTranscriptionTier_TierDoesNotExist_ReturnsNull()
		{
			_collection.RemoveAt(1);
			Assert.IsNull(_collection.GetTranscriptionTier());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFreeTranslationTier_TierExists_ReturnsIt()
		{
			var tier = _collection.GetFreeTranslationTier();
			Assert.AreEqual(TextTier.ElanTranslationTierId, tier.Id);
			Assert.AreEqual(TierType.FreeTranslation, tier.TierType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFreeTranslationTier_TierDoesNotExist_ReturnsNull()
		{
			_collection.RemoveAt(2);
			Assert.IsNull(_collection.GetFreeTranslationTier());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetUserDefinedTextTiers_TiersExists_ReturnsThem()
		{
			var tiers = _collection.GetUserDefinedTextTiers().ToArray();
			Assert.AreEqual(1, tiers.Length);
			Assert.AreEqual("otherTextTier", tiers[0].DisplayName);
			Assert.AreEqual(TierType.Other, tiers[0].TierType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetUserDefinedTextTiers_TiersDoNotExist_ReturnsEmptyList()
		{
			_collection.RemoveAt(3);
			Assert.IsEmpty(_collection.GetUserDefinedTextTiers().ToArray());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetDependentTiers_TiersExists_ReturnsThem()
		{
			var tiers = _collection.GetDependentTextTiers().ToArray();
			Assert.AreEqual(2, tiers.Length);
			Assert.AreEqual(TextTier.ElanTranslationTierId, tiers[0].Id);
			Assert.AreEqual("otherTextTier", tiers[1].DisplayName);
			Assert.AreEqual(TierType.FreeTranslation, tiers[0].TierType);
			Assert.AreEqual(TierType.Other, tiers[1].TierType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Save_CanReadSavedFile()
		{
			var mediafile = MediaFileInfoTests.GetShortTestAudioFile();
			var expectedEafFile = mediafile + ".annotations.eaf";

			try
			{
				var savedEafFile = _collection.Save(mediafile);
				Assert.AreEqual(expectedEafFile, savedEafFile);
				var tiers = AnnotationFileHelper.Load(savedEafFile).GetTierCollection();
				Assert.AreEqual(4, tiers.Count);

				for (int i = 0; i < tiers.Count; i++)
					CheckTier(_collection[i], tiers[i]);
			}
			catch { }
			finally
			{
				try { File.Delete(mediafile); } catch { }
				try { File.Delete(expectedEafFile); } catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddTextTierWithEmptySegments_AddsTier()
		{
			_collection.AddTextTierWithEmptySegments("tb");
			Assert.IsNotNull(_collection.FirstOrDefault(t => t.Id == "tb"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddTextTierWithEmptySegments_NewTierHasCorrectNumberOfEmptySegments()
		{
			_collection.AddTextTierWithEmptySegments("tb");
			var newTier = _collection.FirstOrDefault(t => t.Id == "tb");
			Assert.AreEqual(_collection.GetTimeTier().Segments.Count, newTier.Segments.Count);
			Assert.IsTrue(newTier.Segments.All(s => s.Text == string.Empty));
		}

		#region GetTotalAnnotatedTime tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalAnnotatedTime_NoSegments_ReturnsZero()
		{
			_collection.GetTranscriptionTier().Segments.Clear();
			_collection.GetFreeTranslationTier().Segments.Clear();
			_collection.GetTimeTier().Segments.Clear();
			Assert.AreEqual(TimeSpan.Zero, _collection.GetTotalAnnotatedTime(TextAnnotationType.Transcription));
			Assert.AreEqual(TimeSpan.Zero, _collection.GetTotalAnnotatedTime(TextAnnotationType.Translation));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalAnnotatedTime_NoAnnotations_ReturnsZero()
		{
			foreach (var segment in _collection.GetTranscriptionTier().Segments)
				segment.Text = null;
			foreach (var segment in _collection.GetFreeTranslationTier().Segments)
				segment.Text = null;
			Assert.AreEqual(TimeSpan.Zero, _collection.GetTotalAnnotatedTime(TextAnnotationType.Transcription));
			Assert.AreEqual(TimeSpan.Zero, _collection.GetTotalAnnotatedTime(TextAnnotationType.Translation));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalAnnotatedTime_SomeSegmentsHaveAnnotations_ReturnsTotalAnnotatedTime()
		{
			_collection.GetTranscriptionTier().Segments[1].Text = string.Empty;
			Assert.AreEqual(TimeSpan.FromSeconds(20), _collection.GetTotalAnnotatedTime(TextAnnotationType.Transcription));
			Assert.AreEqual(TimeSpan.FromSeconds(20), _collection.GetTotalAnnotatedTime(TextAnnotationType.Translation));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalAnnotatedTime_NotFullyAnnotatedButAllSegmentsHaveAnnotations_ReturnsTotalAnnotatedTime()
		{
			_collection.GetFreeTranslationTier().Segments[2].Text = "Now I'm not empty anymore";
			Assert.AreEqual(TimeSpan.FromSeconds(30), _collection.GetTotalAnnotatedTime(TextAnnotationType.Transcription));
			Assert.AreEqual(TimeSpan.FromSeconds(30), _collection.GetTotalAnnotatedTime(TextAnnotationType.Translation));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalAnnotatedTime_FullyAnnotated_ReturnsLengthOfMediaFile()
		{
			_collection.GetTimeTier().Segments.Insert(0, new Segment(_collection.GetTimeTier(), 0, 10));
			_collection.GetTimeTier().AddSegment(40, (float)_collection.GetTimeTier().TotalTime.TotalSeconds);

			var transcriptionTier = _collection.GetTranscriptionTier();
			transcriptionTier.Segments.Insert(0, new Segment(transcriptionTier, "0"));
			transcriptionTier.AddSegment("4");

			var translationTier = _collection.GetFreeTranslationTier();
			translationTier.Segments.Insert(0, new Segment(translationTier, "translation of 0"));
			translationTier.Segments[3].Text = "Now I'm not empty anymore";
			translationTier.AddSegment("translation of 4");

			Assert.AreEqual(_collection.GetTimeTier().TotalTime, _collection.GetTotalAnnotatedTime(TextAnnotationType.Transcription));
			Assert.AreEqual(_collection.GetTimeTier().TotalTime, _collection.GetTotalAnnotatedTime(TextAnnotationType.Translation));
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		public static void CheckTier(TierBase expected, TierBase actual)
		{
			Assert.AreEqual(expected.DisplayName, actual.DisplayName);

			Assert.AreEqual(expected.TierType, actual.TierType);
			Assert.AreEqual(expected.Id, actual.Id);

			var expectedSegs = expected.Segments;
			var actualSegs = actual.Segments;
			Assert.AreEqual(expectedSegs.Count, actualSegs.Count);
			for (int i = 0; i < expectedSegs.Count; i++)
			{
				Assert.AreEqual(expectedSegs[i].Start, actualSegs[i].Start);
				Assert.AreEqual(expectedSegs[i].End, actualSegs[i].End);
				Assert.AreEqual(expectedSegs[i].Text, actualSegs[i].Text);
			}
		}
	}
}