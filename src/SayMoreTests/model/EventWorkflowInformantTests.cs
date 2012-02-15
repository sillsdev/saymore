using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SayMore;
using SayMore.Model;
using SayMore.Model.Files;

namespace SayMoreTests.Model
{
	[TestFixture]
	public sealed class EventWorkflowInformantTests
	{
		private ElementRepository<Event> _eventRepo;
		private EventWorkflowInformant _informant;
		private static IEnumerable<ComponentRole> s_componentRoles;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			s_componentRoles = ApplicationContainer.ComponentRoles;
			_eventRepo = GetMockedEventRepo();
			_informant = new EventWorkflowInformant(_eventRepo, s_componentRoles);
		}

		/// ------------------------------------------------------------------------------------
		public static ProjectElementComponentFile GetMockedProjectElementComponentFile(
			IEnumerable<KeyValuePair<string, string>> fieldsAndValues)
		{
			var file = new Mock<ProjectElementComponentFile>();

			foreach (var kvp in fieldsAndValues)
				file.Setup(m => m.GetStringValue(kvp.Key, null)).Returns(kvp.Value);

			return file.Object;
		}

		/// ------------------------------------------------------------------------------------
		public static Event GetMockedEvent(
			IEnumerable<KeyValuePair<string, string>> metadataFieldsValuesToReturn,
			IEnumerable<ComponentRole> completedStagesToReturn)
		{
			var evnt = new Mock<Event>();
			evnt.Setup(e => e.MetaDataFile).Returns(
				GetMockedProjectElementComponentFile(metadataFieldsValuesToReturn));

			foreach (var kvp in metadataFieldsValuesToReturn)
			{
				if (kvp.Key == "id")
				{
					evnt.Setup(e => e.Id).Returns(kvp.Value);
					break;
				}
			}

			evnt.Setup(e => e.GetCompletedStages()).Returns(completedStagesToReturn);

			return evnt.Object;
		}

		/// ------------------------------------------------------------------------------------
		public static ElementRepository<Event> GetMockedEventRepo()
		{
			var repo = new Mock<ElementRepository<Event>>();
			repo.Setup(x => x.AllItems).Returns(new[]
			{
				GetMockedEvent(new[] {
					new KeyValuePair<string, string>("id", "01"),
					new KeyValuePair<string, string>("genre", "discourse"),
					new KeyValuePair<string, string>("status", "Incoming") },
					new[] {
						s_componentRoles.ElementAt(0),
						s_componentRoles.ElementAt(1),
					}),

				GetMockedEvent(new[] {
					new KeyValuePair<string, string>("id", "02"),
					new KeyValuePair<string, string>("genre", "discourse"),
					new KeyValuePair<string, string>("status", "Incoming") },
					new[] {
						s_componentRoles.ElementAt(2),
						s_componentRoles.ElementAt(3)
					}),

				GetMockedEvent(new[] {
					new KeyValuePair<string, string>("id", "03"),
					new KeyValuePair<string, string>("genre", "singing"),
					new KeyValuePair<string, string>("status", "In Progress") },
					new[] { s_componentRoles.ElementAt(0) }),

				GetMockedEvent(new[] {
					new KeyValuePair<string, string>("id", "04"),
					new KeyValuePair<string, string>("genre", "singing"),
					new KeyValuePair<string, string>("status", "Incoming") },
					new[] { s_componentRoles.ElementAt(1) }),

				GetMockedEvent(new[] {
					new KeyValuePair<string, string>("id", "05"),
					new KeyValuePair<string, string>("genre", "singing"),
					new KeyValuePair<string, string>("status", "Finished") },
					new[] { s_componentRoles.ElementAt(0) }),
			});

			return repo.Object;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void NumberOfEvents_ReturnsCount()
		{
			Assert.AreEqual(_eventRepo.AllItems.Count(), _informant.NumberOfEvents);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetEventsHavingFieldValue_PassDiscourseGenre_ReturnsThem()
		{
			var list = _informant.GetEventsHavingFieldValue("genre", "discourse");
			Assert.AreEqual(2, list.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetEventsFromListHavingFieldValue_FromSpecifiedList_ReturnsThem()
		{
			var inList = _eventRepo.AllItems.Where(x => x.Id != "01");
			var outList = EventWorkflowInformant.GetEventsFromListHavingFieldValue(inList, "genre", "discourse");
			Assert.AreEqual(1, outList.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCategorizedEventsByField_PassGenre_ReturnsTwoLists()
		{
			var lists = _informant.GetCategorizedEventsByField("genre");
			Assert.AreEqual(2, lists.Count);
			Assert.AreEqual(2, lists["discourse"].Count());
			Assert.AreEqual(3, lists["singing"].Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCategorizedEventsFromListByField_FromSpecifiedList_ReturnsTwoLists()
		{
			var inList = _eventRepo.AllItems.Where(x => x.Id != "04");
			var lists = EventWorkflowInformant.GetCategorizedEventsFromListByField(inList, "genre");
			Assert.AreEqual(2, lists.Count);
			Assert.AreEqual(2, lists["discourse"].Count());
			Assert.AreEqual(2, lists["singing"].Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCategorizedEventsFromDoubleKey_PassGenreAndStatus_Return()
		{
			var genrelist = _informant.GetCategorizedEventsFromDoubleKey("genre", "status");

			// A list for each genre
			Assert.AreEqual(2, genrelist.Count);

			Assert.AreEqual(1, genrelist["discourse"].Count);
			Assert.AreEqual(3, genrelist["singing"].Count);

			// A list of events for each status within each genre
			Assert.AreEqual(2, genrelist["discourse"]["Incoming"].Count());
			Assert.AreEqual(1, genrelist["singing"]["Incoming"].Count());
			Assert.AreEqual(1, genrelist["singing"]["In Progress"].Count());
			Assert.AreEqual(1, genrelist["singing"]["Finished"].Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetEventsCategorizedByStage_ReturnsCorrectDictionary()
		{
			var stagesList = _informant.GetEventsCategorizedByStage();

			Assert.AreEqual(3, stagesList.ElementAt(0).Value.Count());
			Assert.AreEqual(2, stagesList.ElementAt(1).Value.Count());
			Assert.AreEqual(1, stagesList.ElementAt(2).Value.Count());
			Assert.AreEqual(1, stagesList.ElementAt(3).Value.Count());
			Assert.AreEqual(0, stagesList.ElementAt(4).Value.Count());
			Assert.AreEqual(0, stagesList.ElementAt(5).Value.Count());
		}
	}
}