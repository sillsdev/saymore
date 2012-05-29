using NUnit.Framework;
using SayMore.Transcription.Model;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class SegmentTests
	{
		private Segment _segment;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			var tier = new TextTier("tier");
			_segment = new Segment(tier, "segText");

			Assert.AreEqual(tier, _segment.Tier);
			Assert.AreEqual("segText", _segment.Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Copy_CreatesDifferentObject()
		{
			Assert.AreNotSame(_segment, _segment.Copy(new TextTier("newTier")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Copy_CreatesCopyWithSameTextAndId()
		{
			var copy = _segment.Copy(new TextTier("newTier"));
			Assert.AreEqual(_segment.Text, copy.Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Copy_SetStartAndEndInSource_CreatesCopyWithSameStartAndEnd()
		{
			_segment.Start = 123f;
			_segment.End = 789f;

			var copy = _segment.Copy(new TextTier("newTier"));
			Assert.AreEqual(_segment.Start, copy.Start);
			Assert.AreEqual(_segment.End, copy.End);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetLength_StartAndEndNotSet_ReturnsZero()
		{
			Assert.AreEqual(0f, _segment.GetLength());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetLength_EndGreaterThanStart_ReturnsDifference()
		{
			_segment.Start = 6f;
			_segment.End = 10f;
			Assert.AreEqual(4f, _segment.GetLength());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetLength_EndLessThanStart_ReturnsZero()
		{
			_segment.Start = 10f;
			_segment.End = 6f;
			Assert.AreEqual(0f, _segment.GetLength());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetLength_PassDecPlaceRoundingValue_ReturnsRoundedValue()
		{
			_segment.Start = 1.4444f;
			_segment.End = 5.8888f;
			Assert.AreEqual(4.44f, _segment.GetLength(2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ToString_StartAndEndNotSet_ReturnsText()
		{
			Assert.AreEqual(_segment.Text, _segment.ToString());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ToString_StartAndEndSet_ReturnsText()
		{
			_segment.Start = 6f;
			_segment.End = 10f;
			Assert.IsTrue(_segment.ToString().Contains("6.0"));
			Assert.IsTrue(_segment.ToString().Contains("10.0"));
		}
	}
}
