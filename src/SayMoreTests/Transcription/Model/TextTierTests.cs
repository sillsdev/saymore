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
			Assert.AreEqual(TierType.Text, _tier.DataType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddSegment_AddsSegment()
		{
			Assert.AreEqual(0, _tier.GetAllSegments().Count());
			_tier.AddSegment("C", "carbon");
			Assert.AreEqual(1, _tier.GetAllSegments().Count());
			Assert.AreEqual("C", ((ITextSegment)_tier.GetSegment(0)).Id);
			Assert.AreEqual("carbon", ((ITextSegment)_tier.GetSegment(0)).GetText());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegment_NoSegments_ThrowsException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => _tier.GetSegment(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegment_SegmentsExistButIndexOutOfRange_ThrowsException()
		{
			_tier.AddSegment("CO2", "carbon dioxide");
			Assert.Throws<ArgumentOutOfRangeException>(() => _tier.GetSegment(1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSegment_SegmentsExistIndexOutOfRange_GetsSegment()
		{
			Assert.AreEqual(_tier.AddSegment("F", "fluorine"), _tier.GetSegment(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_NoSegments_ReturnsFalse()
		{
			ISegment segment;
			Assert.IsFalse(_tier.TryGetSegment(0, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_SegmentsExistButIndexOutOfRange_ReturnsFalse()
		{
			_tier.AddSegment("H", "hydrogen");
			ISegment segment;
			Assert.IsFalse(_tier.TryGetSegment(1, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_SegmentsExistIndexInRange_ReturnsTrue()
		{
			_tier.AddSegment("O", "oxygen");
			ISegment segment;
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
	}
}
