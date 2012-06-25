using System;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.UI.Overview.Statistics;
using SayMore.Utilities;

namespace SayMoreTests.UI.Overview.Statistics
{
	[TestFixture]
	public class StatisticsViewModelTests
	{
		private TemporaryFolder _folder;
		[SetUp]
		public void Setup()
		{
			_folder = new TemporaryFolder("StatisticsViewModelTests");
			Directory.CreateDirectory(_folder.Combine("people"));
			Directory.CreateDirectory(_folder.Combine("sessions"));
			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;
		}

		[TearDown]
		public void	TearDown()
		{
			_folder.Dispose();
		}

		private StatisticsViewModel CreateModel()
		{
			var nullRole = new ComponentRole(typeof(Session), "someRole", "someRole",
				ComponentRole.MeasurementTypes.None,
				p => p.EndsWith("txt"), "$ElementId$_someRole", Color.Magenta, Color.Black);

			var personInformer = new PersonInformant(
				new ElementRepository<Person>(_folder.Combine("people"), "People", null, null), null);

			var sessionInformer = new SessionWorkflowInformant(
				new ElementRepository<Session>(_folder.Combine("sessions"), "Sessions", null, null),
				new[] { nullRole });

			return new StatisticsViewModel(null, personInformer, sessionInformer, new[] { nullRole },
				new AudioVideoDataGatherer(_folder.Path, new[] { new AudioFileType(null, () => null, null) }));
		}

		[Test]
		public void GetRecordingDurations_NoMatches_Zero()
		{
			//Mock<ComponentRole> role = new Mock<ComponentRole>();
			//role.Setup(x => x.IsMatch("zzzz")).Returns(false);
			var role = new ComponentRole(typeof (Session), "blah", "blah", ComponentRole.MeasurementTypes.Time,
										 FileSystemUtils.GetIsAudioVideo, "CantMatchThis", Color.Magenta, Color.Black);
			Assert.AreEqual(new TimeSpan(0),
							   CreateModel().GetRecordingDurations(role));
		}

		[Test]
		public void GetElementStatisticsPairs_EmptyProject_GivesSomePairs()
		{
			var pairs = CreateModel().GetElementStatisticsPairs();
			Assert.Less(0, pairs.Count());
		}
	/*
		 [Test]
		[Ignore("Getting file durations using Microsoft.DirectX.AudioVideoPlayback doesn't work in tests.")]
		public void GetRecordingDurations_SomeRecording_MoreThanZero()
		{
			using (var test = new TestProjectWithSessions(1))
			{
				ComponentRole firstRole = ComponentRole.CreateHardCodedDefinitions().First();
				CreateCanonciallyNamedRecordingInSession(firstRole, test.Project.Sessions[0]);

				SpongeProject project = test.Project;
				Assert.Less(new TimeSpan(0),
					CreateModel(project).GetRecordingDurations(firstRole));
			}
		}
*/

		/*
		 this is bogus at the moment because we can't get durations from tests yet
		 *
		 [Test]
		public void GetRecordingDurations_DistinguishesBetweenRoles()
		{
			ComponentRole firstRole = new ComponentRole(typeof (Session), "matchAnything", "matchAnything",
														ComponentRole.MeasurementTypes.Time,
														(x => true), "$ElementId$_someRole");

			CreateCanonciallyNamedRecordingInSession(firstRole, "SomeSessionId");


			TimeSpan t = CreateModel().GetRecordingDurations(firstRole);
			 Assert.AreNotEqual(new TimeSpan(0), t, "should find one file with the first role");

			ComponentRole secondRole = new ComponentRole(typeof (Session), "blah", "blah", ComponentRole.MeasurementTypes.Time,
														 (x => true), "CantMatchThis");

			TimeSpan t = CreateModel().GetRecordingDurations(secondRole);
			Assert.AreEqual(new TimeSpan(0), t, "should not find any files with the second role");

		}
	*/

		//private void CreateCanonciallyNamedRecordingInEvent(ComponentRole roleDefinition, string sessionId)
		//{
		//    var path = MediaFileInfoTests.CreateRecording(_folder.Path);
		//    File.Move(path, roleDefinition.GetCanoncialName(sessionId, path));
		//}
	}
}
