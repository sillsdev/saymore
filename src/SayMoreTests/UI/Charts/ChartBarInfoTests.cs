using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using Moq;
using SayMore.Model;
using SayMore.Utilities.Charts;

namespace SayMoreTests.UI.Charts
{
	[TestFixture]
	public class ChartBarAndBarSegmentInfoTests
	{
		private List<Event> _events;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_events = new List<Event>();

			var evnt = new Mock<Event>();
			evnt.Setup(e => e.Id).Returns("bacon");
			evnt.Setup(e => e.GetTotalMediaDuration()).Returns(new TimeSpan(0, 22, 30));
			_events.Add(evnt.Object);

			evnt = new Mock<Event>();
			evnt.Setup(e => e.Id).Returns("eggs");
			evnt.Setup(e => e.GetTotalMediaDuration()).Returns(new TimeSpan(1, 05, 20));
			_events.Add(evnt.Object);

			evnt = new Mock<Event>();
			evnt.Setup(e => e.Id).Returns("cheese");
			evnt.Setup(e => e.GetTotalMediaDuration()).Returns(new TimeSpan(0, 7, 5));
			_events.Add(evnt.Object);
		}

		/// ------------------------------------------------------------------------------------
		private ChartBarSegmentInfo CreateBasicBarSegment(string fieldName, string fieldValue)
		{
			return new ChartBarSegmentInfo(fieldName, fieldValue, _events, Color.Empty, Color.Empty);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChartBarSegmentInfo_Construct_CalculatesCorrectTotalTime()
		{
			var seg = CreateBasicBarSegment("fieldname", "fieldvalue");
			Assert.AreEqual(95, seg.TotalTime);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChartBarSegmentInfo_NullFieldValue_ThrowsException()
		{
			Assert.Throws<NullReferenceException>(() => CreateBasicBarSegment("fieldname", null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CompareTo_OtherIsNull_ReturnsGreaterThanOne()
		{
			var seg = CreateBasicBarSegment(null, string.Empty);
			Assert.IsTrue(seg.CompareTo(null) > 0);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CompareTo_UsingSortMethod_SwapsOrder()
		{
			var list = new List<ChartBarSegmentInfo>
			{
				CreateBasicBarSegment("1", "1 This Comes After"),
				CreateBasicBarSegment("0", "0 This Comes Before")
			};

			list.Sort();

			Assert.AreEqual("0", list[0].FieldName);
			Assert.AreEqual("1", list[1].FieldName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CompareTo_WhenFieldNameIsStatus_SortsByEnumOrder()
		{
			var list = new List<ChartBarSegmentInfo>
			{
				CreateBasicBarSegment("status", Event.Status.Finished.ToString()),
				CreateBasicBarSegment("status", Event.Status.In_Progress.ToString()),
				CreateBasicBarSegment("status", Event.Status.Incoming.ToString()),
			};

			list.Sort();

			Assert.AreEqual(Event.Status.Incoming.ToString(), list[0].FieldValue);
			Assert.AreEqual(Event.Status.In_Progress.ToString(), list[1].FieldValue);
			Assert.AreEqual(Event.Status.Finished.ToString(), list[2].FieldValue);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CompareTo_WhenFieldNameIsStatusAndValuesContainSpaces_SortsByEnumOrder()
		{
			var list = new List<ChartBarSegmentInfo>
			{
				CreateBasicBarSegment("status", Event.Status.Finished.ToString()),
				CreateBasicBarSegment("status", Event.Status.Incoming.ToString()),
				CreateBasicBarSegment("status", "In Progress"),
			};

			list.Sort();

			Assert.AreEqual(Event.Status.Incoming.ToString(), list[0].FieldValue);
			Assert.AreEqual("In Progress", list[1].FieldValue);
			Assert.AreEqual(Event.Status.Finished.ToString(), list[2].FieldValue);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChartBarInfo_ConstructWithStatusSecondaryField_ThrowsOutSkippedStatuses()
		{
			var segs = new Dictionary<string, IEnumerable<Event>>();
			segs[Event.Status.Incoming.ToString()] = _events;
			segs[Event.Status.Skipped.ToString()] = _events;
			segs[Event.Status.In_Progress.ToString()] = _events;

			var colors = segs.ToDictionary(kvp => kvp.Key, kvp => Color.Empty);
			var barInfo = new ChartBarInfo("Narrative", "status", segs, colors, colors);

			Assert.AreEqual(2, barInfo.Segments.Count());
			Assert.AreEqual(Event.Status.Incoming.ToString(), barInfo.Segments.ElementAt(0).FieldValue);
			Assert.AreEqual(Event.Status.In_Progress.ToString(), barInfo.Segments.ElementAt(1).FieldValue);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChartBarInfo_Construct_CalculatesCorrectTotalTimeAndEvents()
		{
			var segs = new Dictionary<string, IEnumerable<Event>>();
			segs["0"] = _events;
			segs["1"] = _events;

			var colors = segs.ToDictionary(kvp => kvp.Key, kvp => Color.Empty);

			var barInfo = new ChartBarInfo("Narrative", string.Empty, segs, colors, colors);
			Assert.AreEqual(190, barInfo.TotalTime);
			Assert.AreEqual(6, barInfo.TotalEvents);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChartBarInfo_Construct__CalculatesBarSegmentSizesCorrectly()
		{
			var segs = new Dictionary<string, IEnumerable<Event>>();
			segs["0"] = _events;
			segs["1"] = _events.Where(e => e.Id != "eggs");
			segs["2"] = _events.Where(e => e.Id != "bacon");

			var colors = segs.ToDictionary(kvp => kvp.Key, kvp => Color.Empty);
			var barInfo = new ChartBarInfo("Narrative", string.Empty, segs, colors, colors);

			// Total time is 198 minutes.

			// First segment is 95 minutes so takes up 95/198 of the bar size.
			Assert.AreEqual(48, barInfo.Segments.ElementAt(0).SegmentSize);

			// Second segment is 30 minutes so takes up 30/198 of the bar size.
			Assert.AreEqual(15, barInfo.Segments.ElementAt(1).SegmentSize);

			// Third segment is 73 minutes so takes up 73/198 of the bar size.
			Assert.AreEqual(37, barInfo.Segments.ElementAt(2).SegmentSize);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CalculateBarSizes_CalculatesBarSizesCorrectly()
		{
			var segs1 = new Dictionary<string, IEnumerable<Event>>();
			segs1["0"] = _events;

			var segs2 = new Dictionary<string, IEnumerable<Event>>();
			segs2["1"] = _events.Where(e => e.Id != "eggs");

			var colors1 = segs1.ToDictionary(kvp => kvp.Key, kvp => Color.Empty);
			var colors2 = segs2.ToDictionary(kvp => kvp.Key, kvp => Color.Empty);

			var barInfo1 = new ChartBarInfo("Narrative", string.Empty, segs1, colors1, colors1);
			var barInfo2 = new ChartBarInfo("Singing", string.Empty, segs2, colors2, colors2);

			ChartBarInfo.CalculateBarSizes(new List<ChartBarInfo> { barInfo1, barInfo2 });

			Assert.AreEqual(100, barInfo1.BarSize);
			Assert.AreEqual(32, barInfo2.BarSize);
		}
	}
}
