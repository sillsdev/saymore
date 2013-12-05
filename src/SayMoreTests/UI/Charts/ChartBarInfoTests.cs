using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using Moq;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.UI.Charts;

namespace SayMoreTests.UI.Charts
{
	[TestFixture]
	public class ChartBarAndBarSegmentInfoTests
	{
		private List<Session> _sessions;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_sessions = new List<Session>();

			var session = new Mock<Session>();
			session.Setup(e => e.Id).Returns("bacon");
			session.Setup(e => e.GetTotalMediaDuration()).Returns(new TimeSpan(0, 22, 30));
			_sessions.Add(session.Object);

			session = new Mock<Session>();
			session.Setup(e => e.Id).Returns("eggs");
			session.Setup(e => e.GetTotalMediaDuration()).Returns(new TimeSpan(1, 05, 20));
			_sessions.Add(session.Object);

			session = new Mock<Session>();
			session.Setup(e => e.Id).Returns("cheese");
			session.Setup(e => e.GetTotalMediaDuration()).Returns(new TimeSpan(0, 7, 5));
			_sessions.Add(session.Object);
		}

		/// ------------------------------------------------------------------------------------
		private ChartBarSegmentInfo CreateBasicBarSegment(string fieldName, string fieldValue)
		{
			return new ChartBarSegmentInfo(fieldName, fieldValue, _sessions, Color.Empty, Color.Empty);
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
				CreateBasicBarSegment(SessionFileType.kStatusFieldName, Session.Status.Finished.ToString()),
				CreateBasicBarSegment(SessionFileType.kStatusFieldName, Session.Status.In_Progress.ToString()),
				CreateBasicBarSegment(SessionFileType.kStatusFieldName, Session.Status.Incoming.ToString()),
			};

			list.Sort();

			Assert.AreEqual(Session.Status.Incoming.ToString(), list[0].FieldValue);
			Assert.AreEqual(Session.Status.In_Progress.ToString(), list[1].FieldValue);
			Assert.AreEqual(Session.Status.Finished.ToString(), list[2].FieldValue);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CompareTo_WhenFieldNameIsStatusAndValuesContainSpaces_SortsByEnumOrder()
		{
			var list = new List<ChartBarSegmentInfo>
			{
				CreateBasicBarSegment(SessionFileType.kStatusFieldName, Session.Status.Finished.ToString()),
				CreateBasicBarSegment(SessionFileType.kStatusFieldName, Session.Status.Incoming.ToString()),
				CreateBasicBarSegment(SessionFileType.kStatusFieldName, "In Progress"),
			};

			list.Sort();

			Assert.AreEqual(Session.Status.Incoming.ToString(), list[0].FieldValue);
			Assert.AreEqual("In Progress", list[1].FieldValue);
			Assert.AreEqual(Session.Status.Finished.ToString(), list[2].FieldValue);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChartBarInfo_ConstructWithStatusSecondaryField_ThrowsOutSkippedStatuses()
		{
			var segs = new Dictionary<string, IEnumerable<Session>>();
			segs[Session.Status.Incoming.ToString()] = _sessions;
			segs[Session.Status.Skipped.ToString()] = _sessions;
			segs[Session.Status.In_Progress.ToString()] = _sessions;

			var colors = segs.ToDictionary(kvp => kvp.Key, kvp => Color.Empty);
			var barInfo = new ChartBarInfo("Narrative", SessionFileType.kStatusFieldName, segs, colors, colors);

			Assert.AreEqual(2, barInfo.Segments.Count());
			Assert.AreEqual(Session.Status.Incoming.ToString(), barInfo.Segments.ElementAt(0).FieldValue);
			Assert.AreEqual(Session.Status.In_Progress.ToString(), barInfo.Segments.ElementAt(1).FieldValue);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChartBarInfo_Construct_CalculatesCorrectTotalTimeAndSessions()
		{
			var segs = new Dictionary<string, IEnumerable<Session>>();
			segs["0"] = _sessions;
			segs["1"] = _sessions;

			var colors = segs.ToDictionary(kvp => kvp.Key, kvp => Color.Empty);

			var barInfo = new ChartBarInfo("Narrative", string.Empty, segs, colors, colors);
			Assert.AreEqual(190, barInfo.TotalTime);
			Assert.AreEqual(6, barInfo.TotalSessions);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ChartBarInfo_Construct__CalculatesBarSegmentSizesCorrectly()
		{
			var segs = new Dictionary<string, IEnumerable<Session>>();
			segs["0"] = _sessions;
			segs["1"] = _sessions.Where(e => e.Id != "eggs");
			segs["2"] = _sessions.Where(e => e.Id != "bacon");

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
			var segs1 = new Dictionary<string, IEnumerable<Session>>();
			segs1["0"] = _sessions;

			var segs2 = new Dictionary<string, IEnumerable<Session>>();
			segs2["1"] = _sessions.Where(e => e.Id != "eggs");

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
