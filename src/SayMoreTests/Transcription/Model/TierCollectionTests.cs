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
			_collection.Add(new TextTier(TextTier.TranscriptionTierName) { TierType = TierType.Transcription });
			_collection.Add(new TextTier(TextTier.ElanFreeTranslationTierName) { TierType = TierType.FreeTranslation });
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
			Assert.AreEqual(TextTier.TranscriptionTierName, tier.DisplayName);
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
			Assert.AreEqual(TextTier.ElanFreeTranslationTierName, tier.DisplayName);
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
			Assert.AreEqual(TextTier.ElanFreeTranslationTierName, tiers[0].DisplayName);
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