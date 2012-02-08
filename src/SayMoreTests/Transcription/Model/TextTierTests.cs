using System;
using System.Linq;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Transcription.Model;

namespace SayMoreTests.Transcription.Model
{
	[TestFixture]
	public class TextTierTests
	{
		private TextTier _tier;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_tier = new TextTier("test tier");

			Assert.AreEqual("test tier", _tier.DisplayName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddSegment_AddsSegment()
		{
			Assert.AreEqual(0, _tier.Segments.Count());
			_tier.AddSegment("C", "carbon");
			Assert.AreEqual(1, _tier.Segments.Count());
			Assert.AreEqual("C", _tier.Segments.ElementAt(0) .Id);
			Assert.AreEqual("carbon", _tier.Segments.ElementAt(0).Text);
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
			_tier.AddSegment("H", "hydrogen");
			Segment segment;
			Assert.IsFalse(_tier.TryGetSegment(1, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_SegmentsExistIndexInRange_ReturnsTrue()
		{
			_tier.AddSegment("O", "oxygen");
			Segment segment;
			Assert.IsTrue(_tier.TryGetSegment(0, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTierClipboardData_NoSegments_ReturnsEmptyString()
		{
			string dataFormat;
			Assert.IsEmpty(_tier.GetTierClipboardData(out dataFormat) as string);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTierClipboardData_HasSegments_ReturnsCorrectData()
		{
			_tier.AddSegment("O", "oxygen");
			_tier.AddSegment("H", "hydrogen");
			_tier.AddSegment("C", "carbon");

			string dataFormat;
			var data = _tier.GetTierClipboardData(out dataFormat) as string;
			Assert.AreEqual(DataFormats.UnicodeText, dataFormat);

			var expected = string.Format("oxygen{0}hydrogen{1}carbon", Environment.NewLine, Environment.NewLine);
			Assert.AreEqual(expected, data);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsComplete_NoSegments_ReturnsFalse()
		{
			Assert.IsFalse(_tier.GetIsComplete());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsComplete_AllSegmentsAreEmpty_ReturnsFalse()
		{
			_tier.AddSegment("O", null);
			_tier.AddSegment("H", string.Empty);
			Assert.IsFalse(_tier.GetIsComplete());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsComplete_SomeButNotAllSegmentsAreEmpty_ReturnsFalse()
		{
			_tier.AddSegment("O", "oxygen");
			_tier.AddSegment("H", string.Empty);
			Assert.IsFalse(_tier.GetIsComplete());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsComplete_AllSegmentsContainText_ReturnsTrue()
		{
			_tier.AddSegment("O", "oxygen");
			_tier.AddSegment("H", "hydrogen");
			Assert.IsTrue(_tier.GetIsComplete());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Copy_ReturnsDifferentInstanceOfTierWithSameDisplayName()
		{
			var copy = _tier.Copy();
			Assert.AreNotSame(_tier, copy);
			Assert.AreEqual(_tier.DisplayName, copy.DisplayName);
		}
	}
}
