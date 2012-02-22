using System;
using System.Linq;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;

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
			Assert.IsInstanceOf<TextAnnotationColumn>(_tier.GridColumn);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddSegment_PassText_AddsSegmentAndInitializesText()
		{
			Assert.AreEqual(0, _tier.Segments.Count());
			_tier.AddSegment("carbon");
			Assert.AreEqual(1, _tier.Segments.Count());
			Assert.AreEqual("carbon", _tier.Segments.ElementAt(0).Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddSegment_ReturnsAddedSegment()
		{
			var seg = _tier.AddSegment("C");
			Assert.AreSame(seg, _tier.Segments.ElementAt(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddSegment_PassNullText_ReturnsSegmentWithEmptyText()
		{
			Assert.IsEmpty(_tier.AddSegment(null).Text);
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
			_tier.AddSegment("hydrogen");
			Segment segment;
			Assert.IsFalse(_tier.TryGetSegment(1, out segment));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void TryGetSegment_SegmentsExistIndexInRange_ReturnsTrue()
		{
			_tier.AddSegment("oxygen");
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
			_tier.AddSegment("oxygen");
			_tier.AddSegment("hydrogen");
			_tier.AddSegment("carbon");

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
			_tier.AddSegment(null);
			_tier.AddSegment(string.Empty);
			Assert.IsFalse(_tier.GetIsComplete());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsComplete_SomeButNotAllSegmentsAreEmpty_ReturnsFalse()
		{
			_tier.AddSegment("oxygen");
			_tier.AddSegment(string.Empty);
			Assert.IsFalse(_tier.GetIsComplete());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIsComplete_AllSegmentsContainText_ReturnsTrue()
		{
			_tier.AddSegment("oxygen");
			_tier.AddSegment("hydrogen");
			Assert.IsTrue(_tier.GetIsComplete());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Copy_ReturnsDifferentInstanceOfTierWithSameProperties()
		{
			_tier.TierType = TierType.Transcription;
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
		public void JoinSements_NoSegments_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => _tier.JoinSements(2, 3));
			Assert.Throws<ArgumentOutOfRangeException>(() => _tier.JoinSements(0, 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void JoinSements_FromIndexOutOfRange_ThrowsArgumentOutOfRangeException()
		{
			_tier.AddSegment("1");
			Assert.Throws<ArgumentOutOfRangeException>(() => _tier.JoinSements(1, 0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void JoinSements_IndexDifferenceMoreThanOne_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>(() => _tier.JoinSements(0, 2));
			Assert.Throws<ArgumentException>(() => _tier.JoinSements(1, 3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void JoinSements_ToIndexOutOfRange_ThrowsArgumentOutOfRangeException()
		{
			_tier.AddSegment("1");
			Assert.Throws<ArgumentOutOfRangeException>(() => _tier.JoinSements(0, 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void JoinSements_NullSegments_JoinedTextIsEmpty()
		{
			_tier.AddSegment(null);
			_tier.AddSegment(null);
			_tier.AddSegment(null);
			_tier.JoinSements(1, 2);
			Assert.AreEqual(string.Empty, _tier.Segments.ElementAt(2).Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void JoinSements_JoinLowerToHigher_JoinedTextIsLowerPlusHigher()
		{
			_tier.AddSegment("1");
			_tier.AddSegment("2");
			_tier.AddSegment("3");
			_tier.JoinSements(0, 1);
			Assert.AreEqual("1 2", _tier.Segments.ElementAt(1).Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void JoinSements_JoinLowerEmptyToHigherNotEmpty_JoinedTextIsHigher()
		{
			_tier.AddSegment(null);
			_tier.AddSegment("2");
			_tier.AddSegment("3");
			_tier.JoinSements(0, 1);
			Assert.AreEqual("2", _tier.Segments.ElementAt(1).Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void JoinSements_JoinHigherToLower_JoinedTextIsLowerPlusHigher()
		{
			_tier.AddSegment("1");
			_tier.AddSegment("2");
			_tier.AddSegment("3");
			_tier.JoinSements(1, 2);
			Assert.AreEqual("2 3", _tier.Segments.ElementAt(2).Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void JoinSements_JoinHigherEmptyToLowerNotEmpty_JoinedTextIsLower()
		{
			_tier.AddSegment("1");
			_tier.AddSegment("2");
			_tier.AddSegment(null);
			_tier.JoinSements(1, 2);
			Assert.AreEqual("2", _tier.Segments.ElementAt(2).Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_IndexOutOfRange_ReturnsFalse()
		{
			_tier.AddSegment("1");
			_tier.AddSegment("2");
			_tier.AddSegment("3");
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
			_tier.AddSegment("1");
			_tier.AddSegment("2");
			_tier.AddSegment("3");
			Assert.AreEqual(3, _tier.Segments.Count());
			Assert.IsTrue(_tier.RemoveSegment(1));
			Assert.AreEqual(2, _tier.Segments.Count());
			Assert.IsNull(_tier.Segments.FirstOrDefault(s => s.Text == "2"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_RemoveMiddleSegment_JoinsWithNextSegment()
		{
			_tier.AddSegment("1");
			_tier.AddSegment("2");
			_tier.AddSegment("3");
			Assert.IsTrue(_tier.RemoveSegment(1));
			Assert.AreEqual("1", _tier.Segments.ElementAt(0).Text);
			Assert.AreEqual("2 3", _tier.Segments.ElementAt(1).Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_RemoveFirstSegment_JoinsWithNextSegment()
		{
			_tier.AddSegment("1");
			_tier.AddSegment("2");
			_tier.AddSegment("3");
			Assert.IsTrue(_tier.RemoveSegment(0));
			Assert.AreEqual("1 2", _tier.Segments.ElementAt(0).Text);
			Assert.AreEqual("3", _tier.Segments.ElementAt(1).Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_RemoveLastSegment_JoinsWithPrecedingSegment()
		{
			_tier.AddSegment("1");
			_tier.AddSegment("2");
			_tier.AddSegment("3");
			Assert.IsTrue(_tier.RemoveSegment(2));
			Assert.AreEqual("1", _tier.Segments.ElementAt(0).Text);
			Assert.AreEqual("2 3", _tier.Segments.ElementAt(1).Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_With2SegmentsRemoveFirst_JoinsThem()
		{
			_tier.AddSegment("1");
			_tier.AddSegment("2");
			Assert.IsTrue(_tier.RemoveSegment(0));
			Assert.AreEqual("1 2", _tier.Segments.ElementAt(0).Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_With2SegmentsRemoveSecond_JoinsThem()
		{
			_tier.AddSegment("1");
			_tier.AddSegment("2");
			Assert.IsTrue(_tier.RemoveSegment(1));
			Assert.AreEqual("1 2", _tier.Segments.ElementAt(0).Text);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveSegment_With1Segments_RemovesIt()
		{
			_tier.AddSegment("1");
			Assert.IsTrue(_tier.RemoveSegment(0));
			Assert.AreEqual(0, _tier.Segments.Count);
		}
	}
}
