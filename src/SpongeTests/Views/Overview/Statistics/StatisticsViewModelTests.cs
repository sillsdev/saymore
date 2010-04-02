using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SIL.Sponge.Model;
using SIL.Sponge.Views.Overview.Statistics;
using SilUtils;

namespace SIL.Sponge.Views
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
			using (new TestProjectWithSessions(0))
			{
				SpongeProject project = SpongeProject.Create("test");
				var pairs = CreateModel(project).GetPairs();
				Assert.Less(0, pairs.Count());
			}
		}

		private StatisticsViewModel CreateModel(SpongeProject project)
		{
			return new StatisticsViewModel(project, new BackgroundStatisticsMananager(project.SessionsFolder));
		}

		[Test]
		public void GetOriginalRecordingTime_NoRecording_Zero()
		{
			SessionComponentDefinition originalRecording = SessionComponentDefinition.CreateHardCodedDefinitions().First();

			using (var test = new TestProjectWithSessions(1))
			{
				SpongeProject project = test.Project;
				Assert.AreEqual(new TimeSpan(0),
							   CreateModel(project).GetRecordingDurations(originalRecording));
			}
		}

		[Test]
		[Ignore("Getting file durations using Microsoft.DirectX.AudioVideoPlayback doesn't work in tests.")]
		public void GetRecordingDurations_SomeRecording_MoreThanZero()
		{
			using (var test = new TestProjectWithSessions(1))
			{
				SessionComponentDefinition firstRole = SessionComponentDefinition.CreateHardCodedDefinitions().First();
				CreateCanonciallyNamedRecordingInSession(firstRole, test.Project.Sessions[0]);

				SpongeProject project = test.Project;
				Assert.Less(new TimeSpan(0),
					CreateModel(project).GetRecordingDurations(firstRole));
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

				SpongeProject project = test.Project;
				TimeSpan t = CreateModel(project).GetRecordingDurations(secondRole);
				Assert.AreEqual(new TimeSpan(0), t, "should not find any files with the second role");
			}
		}

		private static void CreateCanonciallyNamedRecordingInSession(SessionComponentDefinition roleDefinition, Session session)
		{
			var path = CreateRecording(session.Folder);
			File.Move(path, roleDefinition.GetCanoncialName(session.Id, path));
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

	public class TestProjectWithSessions : IDisposable
	{
		private TemporaryFolder _testAppFolder;
		public SpongeProject Project { get; private set; }

		public TestProjectWithSessions(int sessionCount)
		{
			 _testAppFolder = new TemporaryFolder("~~SpongeStatViewModelTestFolder~~");
			 ReflectionHelper.SetField(typeof(Sponge), "s_mainAppFldr", _testAppFolder.FolderPath);

			Project = SpongeProject.Create("statVwModelTestPrj");
			for (int i = 0; i < sessionCount; i++)
			{
				Session session = Project.AddSession(i.ToString());
				session.Save();
			}
		}

		public void Dispose()
		{
			_testAppFolder.Dispose();
		}
	}
}
