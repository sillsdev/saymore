using System;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.UI.Overview.Statistics;

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
			var nullRole = new ComponentRole(typeof(Session), "someRole", "someRole", ComponentRole.MeasurementTypes.None,
							 p => p.EndsWith("txt"), "$ElementId$_someRole");

			var people = new ElementRepository<Person>(_folder.Combine("people"),"People", null);
			var sessions = new ElementRepository<Session>(_folder.Combine("sessions"),"Sessions", null);

			return new StatisticsViewModel(people, sessions,
				new[]{nullRole},
				new AudioVideoDataGatherer(_folder.Path, new[]{new AudioFileType()}));
		}

		[Test]
		public void GetRecordingDurations_NoMatches_Zero()
		{
			//Mock<ComponentRole> role = new Mock<ComponentRole>();
			//role.Setup(x => x.IsMatch("zzzz")).Returns(false);
			var role = new ComponentRole(typeof (Session), "blah", "blah", ComponentRole.MeasurementTypes.Time,
										 ComponentRole.GetIsAudioVideo, "CantMatchThis");
			Assert.AreEqual(new TimeSpan(0),
							   CreateModel().GetRecordingDurations(role));
		}



		[Test]
		public void GetPairs_EmptyProject_GivesSomePairs()
		{
				var pairs = CreateModel().GetStatisticPairs();
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

		private  void CreateCanonciallyNamedRecordingInSession(ComponentRole roleDefinition, string sessionId)
		{
			var path = CreateRecording(_folder.Path);
			File.Move(path, roleDefinition.GetCanoncialName(sessionId, path));
		}

		private static string CreateRecording(string folder)
		{
			var buf = new byte[Resources.shortSound.Length];
			Resources.shortSound.Read(buf, 0, buf.Length);
			string destination = folder;
			string wavPath = Path.Combine(destination, Directory.GetFiles(destination).Count() + ".wav");
			var f = File.CreateText(wavPath);
			f.Write(buf);
			f.Close();
			return wavPath;
		}
	}


}
