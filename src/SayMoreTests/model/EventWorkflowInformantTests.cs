using System.Linq;
using Moq;
using NUnit.Framework;
using SayMore.Model;
using SayMore.Model.Files;

namespace SayMoreTests.model
{
	[TestFixture]
	public sealed class EventWorkflowInformantTests
	{
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEventsByStatus_NoEvents_ReturnsEmptyList()
		//{
		//    var repo = new Mock<ElementRepository<Event>>();
		//    repo.Setup(x => x.AllItems).Returns(new Event[] { });
		//    var informant = new EventWorkflowInformant(repo.Object);
		//    var events = informant.GetEventsByStatus(Event.Status.Skipped);
		//    Assert.AreEqual(0, events.Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEventsByStatus_NoEventsHaveStatus_ReturnsEmptyList()
		//{
		//    var e1 = new Mock<Event>();
		//    e1.Setup(e => e.GetStatus()).Returns(Event.Status.In_Progress);

		//    var e2 = new Mock<Event>();
		//    e2.Setup(e => e.GetStatus()).Returns(Event.Status.Finished);

		//    var repo = new Mock<ElementRepository<Event>>();
		//    repo.Setup(x => x.AllItems).Returns(new[] { e1.Object, e2.Object });
		//    var informant = new EventWorkflowInformant(repo.Object);

		//    var events = informant.GetEventsByStatus(Event.Status.Skipped);
		//    Assert.AreEqual(0, events.Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEventsByStatus_EventsHaveStatus_ReturnsThem()
		//{
		//    var e1 = new Mock<Event>();
		//    e1.Setup(e => e.GetStatus()).Returns(Event.Status.Finished);

		//    var e2 = new Mock<Event>();
		//    e2.Setup(e => e.GetStatus()).Returns(Event.Status.In_Progress);

		//    var e3 = new Mock<Event>();
		//    e3.Setup(e => e.GetStatus()).Returns(Event.Status.Finished);

		//    var repo = new Mock<ElementRepository<Event>>();
		//    repo.Setup(x => x.AllItems).Returns(new[] { e1.Object, e2.Object, e3.Object });
		//    var informant = new EventWorkflowInformant(repo.Object);

		//    var events = informant.GetEventsByStatus(Event.Status.Finished);
		//    Assert.AreEqual(2, events.Count());
		//    Assert.IsTrue(events.Contains(e1.Object));
		//    Assert.IsTrue(events.Contains(e3.Object));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEventsForEachStatus_NoEvents_ReturnsEmptyList()
		//{
		//    var repo = new Mock<ElementRepository<Event>>();
		//    repo.Setup(x => x.AllItems).Returns(new Event[] { });
		//    var informant = new EventWorkflowInformant(repo.Object);
		//    var events = informant.GetEventsForEachStatus();
		//    Assert.AreEqual(0, events.Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEventsForEachStatus_TwoDistinctStatuses_ReturnsTwoLists()
		//{
		//    var e1 = new Mock<Event>();
		//    e1.Setup(e => e.GetStatus()).Returns(Event.Status.Finished);
		//    e1.Setup(e => e.Id).Returns("David");

		//    var e2 = new Mock<Event>();
		//    e2.Setup(e => e.GetStatus()).Returns(Event.Status.In_Progress);
		//    e2.Setup(e => e.Id).Returns("John");

		//    var e3 = new Mock<Event>();
		//    e3.Setup(e => e.GetStatus()).Returns(Event.Status.Finished);
		//    e3.Setup(e => e.Id).Returns("Zeke");

		//    var repo = new Mock<ElementRepository<Event>>();
		//    repo.Setup(x => x.AllItems).Returns(new[] { e1.Object, e2.Object, e3.Object });
		//    var informant = new EventWorkflowInformant(repo.Object);

		//    var lists = informant.GetEventsForEachStatus();
		//    Assert.AreEqual(2, lists.Count());
		//    Assert.AreEqual(2, lists[Event.Status.Finished].Count());
		//    Assert.AreEqual(1, lists[Event.Status.In_Progress].Count());

		//    Assert.AreEqual(e1.Object, lists[Event.Status.Finished].ElementAt(0));
		//    Assert.AreEqual(e3.Object, lists[Event.Status.Finished].ElementAt(1));
		//    Assert.AreEqual(e2.Object, lists[Event.Status.In_Progress].ElementAt(0));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEventsByGenre_NoEvents_ReturnsEmptyList()
		//{
		//    var repo = new Mock<ElementRepository<Event>>();
		//    repo.Setup(x => x.AllItems).Returns(new Event[] { });
		//    var informant = new EventWorkflowInformant(repo.Object);
		//    var events = informant.GetEventsByGenre("discourse");
		//    Assert.AreEqual(0, events.Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEventsByGenre_NoEventsHaveGenre_ReturnsEmptyList()
		//{
		//    var m1 = new Mock<ProjectElementComponentFile>();
		//    m1.Setup(m => m.GetStringValue("genre", null)).Returns("discourse");

		//    var m2 = new Mock<ProjectElementComponentFile>();
		//    m2.Setup(m => m.GetStringValue("genre", null)).Returns("singing");

		//    var e1 = new Mock<Event>();
		//    e1.Setup(e => e.MetaDataFile).Returns(m1.Object);

		//    var e2 = new Mock<Event>();
		//    e2.Setup(e => e.MetaDataFile).Returns(m2.Object);

		//    var repo = new Mock<ElementRepository<Event>>();
		//    repo.Setup(x => x.AllItems).Returns(new[] { e1.Object, e2.Object });

		//    var informant = new EventWorkflowInformant(repo.Object);
		//    var events = informant.GetEventsByGenre("oratory");
		//    Assert.AreEqual(0, events.Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEventsByGenre_EventsHaveGenre_ReturnsThem()
		//{
		//    var m1 = new Mock<ProjectElementComponentFile>();
		//    m1.Setup(m => m.GetStringValue("genre", null)).Returns("discourse");

		//    var m2 = new Mock<ProjectElementComponentFile>();
		//    m2.Setup(m => m.GetStringValue("genre", null)).Returns("singing");

		//    var m3 = new Mock<ProjectElementComponentFile>();
		//    m3.Setup(m => m.GetStringValue("genre", null)).Returns("discourse");

		//    var e1 = new Mock<Event>();
		//    e1.Setup(e => e.MetaDataFile).Returns(m1.Object);

		//    var e2 = new Mock<Event>();
		//    e2.Setup(e => e.MetaDataFile).Returns(m2.Object);

		//    var e3 = new Mock<Event>();
		//    e3.Setup(e => e.MetaDataFile).Returns(m3.Object);

		//    var repo = new Mock<ElementRepository<Event>>();
		//    repo.Setup(x => x.AllItems).Returns(new[] { e1.Object, e2.Object, e3.Object });

		//    var informant = new EventWorkflowInformant(repo.Object);
		//    var events = informant.GetEventsByGenre("discourse");
		//    Assert.AreEqual(2, events.Count());
		//    Assert.IsTrue(events.Contains(e1.Object));
		//    Assert.IsTrue(events.Contains(e3.Object));
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEventsForEachGenre_NoEvents_ReturnsEmptyList()
		//{
		//    var repo = new Mock<ElementRepository<Event>>();
		//    repo.Setup(x => x.AllItems).Returns(new Event[] { });
		//    var informant = new EventWorkflowInformant(repo.Object);
		//    var events = informant.GetEventsForEachGenre();
		//    Assert.AreEqual(0, events.Count());
		//}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetEventsForEachGenre_TwoDistinctGenres_ReturnsTwoLists()
		//{
		//    var m1 = new Mock<ProjectElementComponentFile>();
		//    m1.Setup(m => m.GetStringValue("genre", null)).Returns("discourse");

		//    var m2 = new Mock<ProjectElementComponentFile>();
		//    m2.Setup(m => m.GetStringValue("genre", null)).Returns("singing");

		//    var m3 = new Mock<ProjectElementComponentFile>();
		//    m3.Setup(m => m.GetStringValue("genre", null)).Returns("discourse");

		//    var e1 = new Mock<Event>();
		//    e1.Setup(e => e.MetaDataFile).Returns(m1.Object);
		//    e1.Setup(e => e.Id).Returns("David");

		//    var e2 = new Mock<Event>();
		//    e2.Setup(e => e.MetaDataFile).Returns(m2.Object);
		//    e2.Setup(e => e.Id).Returns("John");

		//    var e3 = new Mock<Event>();
		//    e3.Setup(e => e.MetaDataFile).Returns(m3.Object);
		//    e3.Setup(e => e.Id).Returns("Zeke");

		//    var repo = new Mock<ElementRepository<Event>>();
		//    repo.Setup(x => x.AllItems).Returns(new[] { e1.Object, e2.Object, e3.Object });

		//    var informant = new EventWorkflowInformant(repo.Object);
		//    var lists = informant.GetEventsForEachGenre();

		//    Assert.AreEqual(2, lists.Count());
		//    Assert.AreEqual(2, lists["discourse"].Count());
		//    Assert.AreEqual(1, lists["singing"].Count());

		//    Assert.AreEqual(e1.Object, lists["discourse"].ElementAt(0));
		//    Assert.AreEqual(e3.Object, lists["discourse"].ElementAt(1));
		//    Assert.AreEqual(e2.Object, lists["singing"].ElementAt(0));
		//}
	}
}