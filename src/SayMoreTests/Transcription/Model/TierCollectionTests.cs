using System.IO;
using System.Linq;
using NUnit.Framework;
using SayMore.Transcription.Model;
using SayMoreTests.UI.Utilities;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class TierCollectionTests
	{
		private TierCollection _collection;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_collection = new TierCollection("annotatedBlah");
			_collection.Clear();
			_collection.Add(new TimeTier("timeTier", "timeTierFilename"));
			_collection.Add(new TextTier(TextTier.ElanTranscriptionTierId));
			_collection.Add(new TextTier(TextTier.ElanTranslationTierId));
			_collection.Add(new TextTier("otherTextTier"));

			((TimeTier)_collection[0]).AddSegment(10f, 20f);
			((TimeTier)_collection[0]).AddSegment(20f, 30f);
			((TimeTier)_collection[0]).AddSegment(30f, 40f);

			((TextTier)_collection[1]).AddSegment("trans1");
			((TextTier)_collection[1]).AddSegment("trans2");
			((TextTier)_collection[1]).AddSegment("trans3");

			((TextTier)_collection[2]).AddSegment("free1");
			((TextTier)_collection[2]).AddSegment("free2");
			((TextTier)_collection[2]).AddSegment(null);

			((TextTier)_collection[3]).AddSegment("other1");
			((TextTier)_collection[3]).AddSegment(null);
			((TextTier)_collection[3]).AddSegment(null);

			Assert.AreEqual("annotatedBlah", _collection.AnnotatedMediaFile);
			Assert.AreEqual(4, _collection.Count);
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
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _collection.InsertTierSegment(10.5f));
			Assert.AreEqual(BoundaryModificationResult.SegmentWillBeTooShort, _collection.InsertTierSegment(30.4f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertTierSegment_BoundaryIsTooCloseToNext_ReturnsNotSuccess()
		{
			Assert.AreEqual(BoundaryModificationResult.NextSegmentWillBeTooShort, _collection.InsertTierSegment(19.5f));
			Assert.AreEqual(BoundaryModificationResult.NextSegmentWillBeTooShort, _collection.InsertTierSegment(29.5f));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void InsertTierSegment_BoundaryIsBeyonodLast_InsertsAndReturnsSuccess()
		{
			var segs = _collection.GetTimeTier().Segments;
			Assert.AreEqual(3, segs.Count);

			Assert.AreEqual(BoundaryModificationResult.Success, _collection.InsertTierSegment(40.501f));
			Assert.AreEqual(4, segs.Count);
			Assert.AreEqual(40f, segs[3].Start);
			Assert.AreEqual(40.501f, segs[3].End);

			Assert.AreEqual(BoundaryModificationResult.Success, _collection.InsertTierSegment(50f));
			Assert.AreEqual(5, _collection[0].Segments.Count);
			Assert.AreEqual(40.501f, segs[4].Start);
			Assert.AreEqual(50f, segs[4].End);
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
			Assert.AreEqual("timeTier", tier.DisplayName);
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
			var mediafile = MPlayerMediaInfoTests.GetShortTestAudioFile();
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
		private void CheckTier(TierBase expected, TierBase actual)
		{
			Assert.AreEqual(expected.DisplayName, actual.DisplayName);

			var expectedSegs = expected.Segments.ToArray();
			var actualSegs = actual.Segments.ToArray();
			Assert.AreEqual(expectedSegs.Length, actualSegs.Length);

			for (int i = 0; i < expectedSegs.Length; i++)
			{
				Assert.AreEqual(expectedSegs[i].Start, actualSegs[i].Start);
				Assert.AreEqual(expectedSegs[i].End, actualSegs[i].End);
				Assert.AreEqual(expectedSegs[i].Text, actualSegs[i].Text);
			}
		}
	}
}