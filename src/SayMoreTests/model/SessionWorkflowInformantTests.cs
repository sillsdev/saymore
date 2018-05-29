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
	public sealed class SessionWorkflowInformantTests
	{
		private ElementRepository<Session> _sessionRepo;
		private SessionWorkflowInformant _informant;
		private static IEnumerable<ComponentRole> s_componentRoles;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			s_componentRoles = ApplicationContainer.ComponentRoles;
			_sessionRepo = GetMockedSessionRepo();
			_informant = new SessionWorkflowInformant(_sessionRepo, s_componentRoles);
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
		public static Session GetMockedSession(
			IEnumerable<KeyValuePair<string, string>> metadataFieldsValuesToReturn,
			IEnumerable<ComponentRole> completedStagesToReturn)
		{
			var session = new Mock<Session>();
			session.Setup(e => e.MetaDataFile).Returns(
				GetMockedProjectElementComponentFile(metadataFieldsValuesToReturn));

			foreach (var kvp in metadataFieldsValuesToReturn)
			{
				if (kvp.Key == "id")
				{
					session.Setup(e => e.Id).Returns(kvp.Value);
					break;
				}
			}

			session.Setup(e => e.GetCompletedStages()).Returns(completedStagesToReturn);

			return session.Object;
		}

		/// ------------------------------------------------------------------------------------
		public static ElementRepository<Session> GetMockedSessionRepo()
		{
			var repo = new Mock<ElementRepository<Session>>();
			repo.Setup(x => x.AllItems).Returns(new[]
			{
				GetMockedSession(new[] {
					new KeyValuePair<string, string>("id", "01"),
					new KeyValuePair<string, string>(SessionFileType.kGenreFieldName, "formulaic_discourse"),
					new KeyValuePair<string, string>(SessionFileType.kStatusFieldName, "Incoming") },
					new[] {
						s_componentRoles.ElementAt(0),
						s_componentRoles.ElementAt(1),
					}),

				GetMockedSession(new[] {
					new KeyValuePair<string, string>("id", "02"),
					new KeyValuePair<string, string>(SessionFileType.kGenreFieldName, "formulaic_discourse"),
					new KeyValuePair<string, string>(SessionFileType.kStatusFieldName, "Incoming") },
					new[] {
						s_componentRoles.ElementAt(2),
						s_componentRoles.ElementAt(3)
					}),

				GetMockedSession(new[] {
					new KeyValuePair<string, string>("id", "03"),
					new KeyValuePair<string, string>(SessionFileType.kGenreFieldName, "singing"),
					new KeyValuePair<string, string>(SessionFileType.kStatusFieldName, "In Progress") },
					new[] { s_componentRoles.ElementAt(0) }),

				GetMockedSession(new[] {
					new KeyValuePair<string, string>("id", "04"),
					new KeyValuePair<string, string>(SessionFileType.kGenreFieldName, "singing"),
					new KeyValuePair<string, string>(SessionFileType.kStatusFieldName, "Incoming") },
					new[] { s_componentRoles.ElementAt(1) }),

				GetMockedSession(new[] {
					new KeyValuePair<string, string>("id", "05"),
					new KeyValuePair<string, string>(SessionFileType.kGenreFieldName, "singing"),
					new KeyValuePair<string, string>(SessionFileType.kStatusFieldName, "Finished") },
					new[] { s_componentRoles.ElementAt(0) }),
			});

			return repo.Object;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void NumberOfSessions_ReturnsCount()
		{
			Assert.AreEqual(_sessionRepo.AllItems.Count(), _informant.NumberOfSessions);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSessionsHavingFieldValue_PassDiscourseGenre_ReturnsThem()
		{
			var list = _informant.GetSessionsHavingFieldValue(SessionFileType.kGenreFieldName, "formulaic_discourse");
			Assert.AreEqual(2, list.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSessionsFromListHavingFieldValue_FromSpecifiedList_ReturnsThem()
		{
			var inList = _sessionRepo.AllItems.Where(x => x.Id != "01");
			var outList = SessionWorkflowInformant.GetSessionsFromListHavingFieldValue(inList, SessionFileType.kGenreFieldName, "formulaic_discourse");
			Assert.AreEqual(1, outList.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCategorizedSessionsByField_PassGenre_ReturnsTwoLists()
		{
			var lists = _informant.GetCategorizedSessionsByField(SessionFileType.kGenreFieldName);
			Assert.AreEqual(2, lists.Count);
			Assert.AreEqual(1, (from i in lists where i.Key.ToLower().Contains("formulaic_discourse") select i.Key).Count());
			Assert.AreEqual(1, (from i in lists where i.Key.ToLower().Contains("singing") select i.Key).Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCategorizedSessionsFromListByField_FromSpecifiedList_ReturnsTwoLists()
		{
			var inList = _sessionRepo.AllItems.Where(x => x.Id != "04");
			var lists = SessionWorkflowInformant.GetCategorizedSessionsFromListByField(inList, SessionFileType.kGenreFieldName);
			Assert.AreEqual(2, lists.Count);
			Assert.AreEqual(1, (from i in lists where i.Key.ToLower().Contains("formulaic_discourse") select i.Key).Count());
			Assert.AreEqual(1, (from i in lists where i.Key.ToLower().Contains("singing") select i.Key).Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCategorizedSessionsFromDoubleKey_PassGenreAndStatus_Return()
		{
			var genrelist = _informant.GetCategorizedSessionsFromDoubleKey(SessionFileType.kGenreFieldName, SessionFileType.kStatusFieldName);

			// A list for each genre
			Assert.AreEqual(2, genrelist.Count);

			Assert.AreEqual(1, (from i in genrelist where i.Key.ToLower().Contains("formulaic_discourse") select i.Key).Count());
			Assert.AreEqual(1, (from i in genrelist where i.Key.ToLower().Contains("singing") select i.Key).Count());

			// A list of sessions for each status within each genre
			Assert.AreEqual(1, (from i in genrelist where i.Key.ToLower().Contains("formulaic_discourse") from v in i.Value where v.Key.ToLower().Contains("incoming") select v.Key).Count());
			Assert.AreEqual(1, (from i in genrelist where i.Key.ToLower().Contains("singing") from v in i.Value where v.Key.ToLower().Contains("incoming") select v.Key).Count());
			Assert.AreEqual(1, (from i in genrelist where i.Key.ToLower().Contains("singing") from v in i.Value where v.Key.ToLower().Contains("in progress") select v.Key).Count());
			Assert.AreEqual(1, (from i in genrelist where i.Key.ToLower().Contains("singing") from v in i.Value where v.Key.ToLower().Contains("finished") select v.Key).Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSessionsCategorizedByStage_ReturnsCorrectDictionary()
		{
			var stagesList = _informant.GetSessionsCategorizedByStage();

			Assert.AreEqual(3, stagesList.ElementAt(0).Value.Count());
			Assert.AreEqual(2, stagesList.ElementAt(1).Value.Count());
			Assert.AreEqual(1, stagesList.ElementAt(2).Value.Count());
			Assert.AreEqual(1, stagesList.ElementAt(3).Value.Count());
			Assert.AreEqual(0, stagesList.ElementAt(4).Value.Count());
			Assert.AreEqual(0, stagesList.ElementAt(5).Value.Count());
		}
	}
}