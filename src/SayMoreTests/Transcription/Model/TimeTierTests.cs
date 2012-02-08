using System.Linq;
using NUnit.Framework;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class TimeTierTests
	{
		private TimeTier _tier;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_tier = new TimeTier("test tier", "filename");

			Assert.AreEqual("test tier", _tier.DisplayName);
			Assert.AreEqual("filename", _tier.MediaFileName);
			Assert.IsInstanceOf<AudioWaveFormColumn>(_tier.GridColumn);
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
		public void AddSegment_PassTimes_AddsSegmentAndInitializesTimes()
		{
			Assert.AreEqual(0, _tier.Segments.Count());
			_tier.AddSegment(10f, 20f);
			Assert.AreEqual(1, _tier.Segments.Count());
			Assert.AreEqual(10f, _tier.Segments.ElementAt(0).Start);
			Assert.AreEqual(20f, _tier.Segments.ElementAt(0).End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddSegment_ReturnsAddedSegment()
		{
			var seg = _tier.AddSegment(0f, 1f);
			Assert.AreSame(seg, _tier.Segments.ElementAt(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_NoSegments_ReturnsFalse()
		{
			Segment segment;
			Assert.IsFalse(_tier.TryGetSegment(0, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_SegmentsExistButIndexOutOfRange_ReturnsFalse()
		{
			_tier.AddSegment(0f, 1f);
			Segment segment;
			Assert.IsFalse(_tier.TryGetSegment(1, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_SegmentsExistIndexInRange_ReturnsTrue()
		{
			_tier.AddSegment(0f, 1f);
			Segment segment;
			Assert.IsTrue(_tier.TryGetSegment(0, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_IndexOutOfRange_ReturnsFalse()
		{
			_tier.AddSegment(0f, 1f);
			_tier.AddSegment(2f, 3f);
			_tier.AddSegment(4f, 5f);
			Assert.AreEqual(3, _tier.Segments.Count());
			Assert.IsFalse(_tier.RemoveSegment(-1));
			Assert.AreEqual(3, _tier.Segments.Count());
			Assert.IsFalse(_tier.RemoveSegment(3));
			Assert.AreEqual(3, _tier.Segments.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_Add3SegmentsRemove2nd_RemovesSegment()
		{
			_tier.AddSegment(0f, 1f);
			_tier.AddSegment(2f, 3f);
			_tier.AddSegment(4f, 5f);
			Assert.AreEqual(3, _tier.Segments.Count());
			Assert.IsTrue(_tier.RemoveSegment(1));
			Assert.AreEqual(2, _tier.Segments.Count());
			Assert.AreNotEqual(2f, _tier.Segments.ElementAt(1).Start);
			Assert.AreNotEqual(3f, _tier.Segments.ElementAt(1).End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_RemoveMiddleSegment_JoinsWithPrecedingSegment()
		{
			_tier.AddSegment(0f, 1f);
			_tier.AddSegment(2f, 3f);
			_tier.AddSegment(4f, 5f);
			Assert.IsTrue(_tier.RemoveSegment(1));
			Assert.AreEqual(2, _tier.Segments.Count());
			Assert.AreEqual(0f, _tier.Segments.ElementAt(0).Start);
			Assert.AreEqual(3f, _tier.Segments.ElementAt(0).End);
			Assert.AreEqual(4f, _tier.Segments.ElementAt(1).Start);
			Assert.AreEqual(5f, _tier.Segments.ElementAt(1).End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_RemoveFirstSegment_JoinsWithNextSegment()
		{
			_tier.AddSegment(0f, 1f);
			_tier.AddSegment(2f, 3f);
			_tier.AddSegment(4f, 5f);
			Assert.IsTrue(_tier.RemoveSegment(0));
			Assert.AreEqual(2, _tier.Segments.Count());
			Assert.AreEqual(0f, _tier.Segments.ElementAt(0).Start);
			Assert.AreEqual(3f, _tier.Segments.ElementAt(0).End);
			Assert.AreEqual(4f, _tier.Segments.ElementAt(1).Start);
			Assert.AreEqual(5f, _tier.Segments.ElementAt(1).End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_RemoveLastSegment_DoesNotAffectPrecedingSegment()
		{
			_tier.AddSegment(0f, 1f);
			_tier.AddSegment(2f, 3f);
			_tier.AddSegment(4f, 5f);
			Assert.IsTrue(_tier.RemoveSegment(2));
			Assert.AreEqual(2, _tier.Segments.Count());
			Assert.AreEqual(0f, _tier.Segments.ElementAt(0).Start);
			Assert.AreEqual(1f, _tier.Segments.ElementAt(0).End);
			Assert.AreEqual(2f, _tier.Segments.ElementAt(1).Start);
			Assert.AreEqual(3f, _tier.Segments.ElementAt(1).End);
		}
	}
}
