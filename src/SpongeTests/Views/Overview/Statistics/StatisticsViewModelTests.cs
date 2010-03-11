using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Palaso.TestUtilities;
using SIL.Sponge;
using SIL.Sponge.Model;
using SIL.Sponge.Views.Overview.Statistics;

namespace SpongeTests.Views.Overview.Statistics
{
	[TestFixture]
	public class StatisticsViewModelTests
	{
		[SetUp]
		public void Setup()
		{
			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;
		}

		[Test]
		public void GetPairs_EmptyProject_GivesSomePairs()
		{
			using (var test = new TestProjectWithSessions(0))
			{
				var pairs = new StatisticsViewModel(SpongeProject.Create("test")).GetPairs();
				Assert.Less(0,pairs.Count());
			}
		}

		[Test]
		public void GetOriginalRecordingTime_NoRecording_Zero()
		{
			SessionComponentDefinition originalRecording = SessionComponentDefinition.CreateHardCodedDefinitions().First();

			using (var test = new TestProjectWithSessions(1))
			{
				Assert.AreEqual(new TimeSpan(0),
								new StatisticsViewModel(test.Project).GetRecordingDurations(originalRecording));
			}
		}

		[Test]
		public void GetRecordingDurations_SomeRecording_MoreThanZero()
		{
			using (var test = new TestProjectWithSessions(1))
			{
				SessionComponentDefinition firstRole = SessionComponentDefinition.CreateHardCodedDefinitions().First();
				CreateCanonciallyNamedRecordingInSession(firstRole, test.Project.Sessions[0]);

				Assert.Less(new TimeSpan(0),
							   new StatisticsViewModel(test.Project).GetRecordingDurations(
								   firstRole));
			}
		}

		[Test]
		public void GetRecordingDurations_DistinguishesBetweenRoles()
		{
			using (var test = new TestProjectWithSessions(1))
			{
				SessionComponentDefinition firstRole = SessionComponentDefinition.CreateHardCodedDefinitions().First();
				CreateCanonciallyNamedRecordingInSession(firstRole, test.Project.Sessions[0]);

				SessionComponentDefinition secondRole =
					SessionComponentDefinition.CreateHardCodedDefinitions().ToArray()[1];

				TimeSpan t = new StatisticsViewModel(test.Project).GetRecordingDurations(secondRole);
				Assert.AreEqual(new TimeSpan(0), t, "should not find any files with the second role");
			}
		}

		private void CreateCanonciallyNamedRecordingInSession(SessionComponentDefinition roleDefinition, Session session)
		{
			var path = CreateRecording(session.Folder);
			File.Move(path, roleDefinition.GetCanoncialName(session.Id, path));
		}

		private string CreateRecording(string folder)
		{
			var buf = new byte[Resources.shortSound.Length];
			Resources.shortSound.Read(buf, 0, buf.Length);
			string destination = folder;
			string wavPath = Path.Combine(destination, Directory.GetFiles(destination).Count().ToString()+".wav");
			File.WriteAllBytes(wavPath, buf);
			return wavPath;
		}
	}

	public class TestProjectWithSessions : IDisposable
	{
		private TemporaryFolder _folder;
		public SpongeProject Project { get; private set; }

		public TestProjectWithSessions(int sessionCount)
		{
			 _folder = new TemporaryFolder();
			SpongeProject.ProjectsFolder = _folder.FolderPath;
			Project = SpongeProject.Create("project");
			for (int i = 0; i < sessionCount; i++)
			{
				Session session = Project.AddSession(i.ToString());
				session.Save();
			}
		}

		public void Dispose()
		{
			_folder.Dispose();
		}
	}
}
