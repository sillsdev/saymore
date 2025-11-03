using System;
using System.Drawing;
using System.IO;
using Moq;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using System.Linq;
using System.Collections.Generic;
using SayMore.Utilities;
using SIL.Core.ClearShare;
using SIL.IO;
using SayMore.UI.ComponentEditors;

namespace SayMoreTests.Model
{
	/// <summary>
	/// NOTE: MUCH OF THE BEHAVIOR IS IN THE BASE CLASS, AND TESTED THERE
	/// </summary>
	[TestFixture]
	public class SessionTests
	{
		private TemporaryFolder _parentFolder;
		private Mock<ProjectElementComponentFile> _mockedSessionMetaDataFile;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("sessionTest");
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
			_mockedSessionMetaDataFile = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>Helper method to create a session for testing.</summary>
		/// <param name="participants">List of strings. Each string can be a simple string with
		/// just a person's name or a forward-slash--delimited string where the first part is the
		/// person's name, the second part is the role (typically an official OLAC role code
		/// (lowercase) or name (title case)), and the third part (optional) is a comment about
		/// the person's contribution.</param>
		/// <param name="project">The project (optional) to be used to set default session
		/// metadata values</param>
		/// <param name="additionalSetup">Additional setup to be done for the mocked
		/// <see cref="ProjectElementComponentFile"/> representing this session.</param>
		/// ------------------------------------------------------------------------------------
		private Session CreateSession(IReadOnlyList<string> participants,
			Project project = null, Action<Mock<ProjectElementComponentFile>> additionalSetup = null)
		{
			ProjectElementComponentFile ProjElemComponentFileFactory(ProjectElement parentElement,
				FileType fileType, XmlFileSerializer fileSerializer, string rootElementName)
			{
				var file = new Mock<ProjectElementComponentFile>();
				file.Setup(f => f.Save());
				if (participants != null)
				{
					var contributions = new Mock<ContributionCollection>(MockBehavior.Strict);
					foreach (var p in participants)
					{
						var parts = p.Split('/');
						var role = parts.Length == 1 || parts[1] == string.Empty ? 
							Session.OlacSystem.GetRoleByCodeOrThrow("participant") :
							Session.GetRoleFromOlacList(parts[1]);
						var contrib = new Contribution(parts[0], role);
						if (parts.Length > 2)
							contrib.Comments = parts[2];
						contributions.Object.Add(contrib);
					}

					file.Setup(f => f.GetValue(SessionFileType.kContributionsFieldName, null))
						.Returns(contributions.Object);
				}

				if (additionalSetup != null)
				{
					additionalSetup(file);
					_mockedSessionMetaDataFile = file;
				}

				return file.Object;
			}

			ComponentFile ComponentFileFactory(ProjectElement parentElement, string path)
			{
				var file = new Mock<ComponentFile>();
				//person.Setup(p => p.GetInformedConsentComponentFile()).Returns((ComponentFile)null);
				file.Setup(p => p.Save());
				return file.Object;
			}

			var personInformant = new Mock<PersonInformant>();

			if (participants != null)
			{
				foreach (var participant in participants)
				{
					personInformant.Setup(i => i.GetHasInformedConsent(participant)).Returns(participant.Contains("Consent"));
				}
			}

			var componentRoles = new List<ComponentRole>
			{
				new ComponentRole(null, ComponentRole.kConsentComponentRoleId, null,
				ComponentRole.MeasurementTypes.None, null, null, Color.Empty, Color.Empty),
				new ComponentRole(null, ComponentRole.kSourceComponentRoleId, null,
				ComponentRole.MeasurementTypes.Time, FileSystemUtils.GetIsAudioVideo,
				ComponentRole.kElementIdToken + ComponentRole.kFileSuffixSeparator + "Source", Color.Empty, Color.Empty)
			};

			return new Session(_parentFolder.Path, "dummyId", null,
				new SessionFileType(new Lazy<Func<SessionBasicEditor.Factory>>(() => null),
				new Lazy<Func<StatusAndStagesEditor.Factory>>(() => null),
				new Lazy<Func<ContributorsEditor.Factory>>(() => null)), ComponentFileFactory,
				new XmlFileSerializer(null), ProjElemComponentFileFactory, componentRoles,
				personInformant.Object, project);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Constructor_NewSession_ProjectLocationInfoCopiedToSessionAndGenreSetToUnknown()
		{
			var project = new Project(_parentFolder.Combine("foo", "foo." + Project.ProjectSettingsFileExtension), null, null)
			{
				Country = "Fred",
				Continent = "Ocean Floor",
				Region = "Sideways",
				Location = "Upstairs"
			};

			using (CreateSession(new string[] {}, project,
				       file =>
				       {
					       file.Setup(f => f.GetCreateDate()).Returns(DateTime.Now.AddSeconds(-57));
					       file.Setup(f => f.GetStringValue(SessionFileType.kCountryFieldName, null)).Returns((string) null);
					       file.Setup(f => f.GetStringValue(SessionFileType.kContinentFieldName, null)).Returns((string) null);
					       file.Setup(f => f.GetStringValue(SessionFileType.kRegionFieldName, null)).Returns((string) null);
					       file.Setup(f => f.GetStringValue(SessionFileType.kAddressFieldName, null)).Returns((string) null);
					       file.Setup(f => f.GetStringValue(SessionFileType.kGenreFieldName, null)).Returns((string) null);
					       file.Setup(f => f.Save(Path.Combine(_parentFolder.Path, "dummyId", "dummyId.session")));
				       }))
			{
				_mockedSessionMetaDataFile.Verify(f => f.TrySetStringValue(SessionFileType.kCountryFieldName, "Fred"));
				_mockedSessionMetaDataFile.Verify(f => f.TrySetStringValue(SessionFileType.kContinentFieldName, "Ocean Floor"));
				_mockedSessionMetaDataFile.Verify(f => f.TrySetStringValue(SessionFileType.kRegionFieldName, "Sideways"));
				_mockedSessionMetaDataFile.Verify(f => f.TrySetStringValue(SessionFileType.kAddressFieldName, "Upstairs"));
				_mockedSessionMetaDataFile.Verify(f => f.TrySetStringValue(SessionFileType.kGenreFieldName, GenreDefinition.UnknownType.Name));
			}
		}


		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllParticipants_NoParticipantsListed_NoneReturned()
		{
			using (var session = CreateSession(new string[] { }))
				Assert.AreEqual(0, session.GetAllParticipants().Count());

			using (var session = CreateSession(null))
				Assert.AreEqual(0, session.GetAllParticipants().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllParticipants_SomeParticipantsListed_ReturnsTheirNames()
		{
			using (var session = CreateSession(new[] { "mo", "curly" }))
			{
				var names = session.GetAllParticipants().ToList();
				Assert.Contains("mo", names);
				Assert.Contains("curly", names);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllContributionsFor_NullPerson_Throws()
		{
			List<SessionContribution> result = null;
			using (var session = CreateSession(new[] { "Moe" }))
			{
				Assert.That(() => { result = session.GetAllContributionsFor(null).ToList(); },
					Throws.ArgumentNullException);
			}

			Assert.That(result, Is.Null);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllContributionsFor_NoContribs_ReturnsEmpty()
		{
			using (var session = CreateSession(Array.Empty<string>()))
			{
				Assert.That(session.GetAllContributionsFor("Moe"), Is.Empty);
				Assert.That(session.GetAllContributionsFor("Kermit"), Is.Empty);
			}

			using (var session = CreateSession(null))
			{
				Assert.That(session.GetAllContributionsFor("Moe"), Is.Empty);
				Assert.That(session.GetAllContributionsFor("Kermit"), Is.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllContributionsFor_OnlySessionLevelParticipantsListed_ReturnsTheirContributions()
		{
			using (var session = CreateSession(new[] { "Moe", "Curly/Data Inputter/Bad typist", "Joe/Recorder", "Joe/Consultant" }))
			{
				var contribution = session.GetAllContributionsFor("Moe").Single();
				ValidateSessionLevelDetails(contribution, session);
				Assert.That(contribution.Contribution.ContributorName, Is.EqualTo("Moe"));
				Assert.That(contribution.Contribution.Role.Name, Is.EqualTo("Participant"));
				Assert.That(contribution.Contribution.Comments, Is.Null);

				contribution = session.GetAllContributionsFor("Curly").Single();
				ValidateSessionLevelDetails(contribution, session);
				Assert.That(contribution.Contribution.ContributorName, Is.EqualTo("Curly"));
				Assert.That(contribution.Contribution.Role.Name, Is.EqualTo("Data Inputter"));
				Assert.That(contribution.Contribution.Comments, Is.EqualTo("Bad typist"));

				var contributions = session.GetAllContributionsFor("Joe").ToList();
				Assert.That(contributions.Count, Is.EqualTo(2));
				foreach (var contribByJoe in contributions)
				{
					ValidateSessionLevelDetails(contribByJoe, session);
					Assert.That(contribByJoe.Contribution.ContributorName, Is.EqualTo("Joe"));
					Assert.That(contribByJoe.Contribution.Comments, Is.Null);
				}
				Assert.That(contributions.Select(c => c.Contribution.Role.Name),
					Is.EquivalentTo(new [] { "Recorder", "Consultant" }));

				Assert.That(session.GetAllContributionsFor("Kermit"), Is.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllContributions_BothSessionLevelAndFileLevelParticipantsListed_ReturnsTheirContributions()
		{
			const string frogMetadata = @"
				<MetaData>
					<Channels type=""string"">mono</Channels>
					<contributions>
						<contributor>
							<name>Moe</name>
							<role>developer</role>
							<date>2024-07-04</date>
							<notes>Show host</notes>
						</contributor>
						<contributor>
							<name>Curly</name>
							<role>hopper</role>
						</contributor>
					</contributions>
				</MetaData>";

			const string soupMetadata = @"
				<MetaData>
					<contributions>
						<contributor>
							<name>Curly</name>
							<role>data_inputter</role>
						</contributor>
						<contributor>
							<name>Curly</name>
							<role>translator</role>
						</contributor>
					</contributions>
				</MetaData>";

			using (var session = CreateSession(new[] { "Moe", "Curly/consultant" }))
			{
				var metaFile1 = Path.Combine(session.FolderPath, "frog.meta");
				using (var writer = new StreamWriter(metaFile1))
					writer.Write(frogMetadata);
				Assert.That(metaFile1, Does.Exist);
				try
				{
					var metaFile2 = Path.Combine(session.FolderPath, "soup.meta");
					using (var writer = new StreamWriter(metaFile2))
						writer.Write(soupMetadata);

					Assert.That(metaFile2, Does.Exist);
					try
					{
						var contributions = session.GetAllContributionsFor("Moe").ToList();
						Assert.That(contributions.Count, Is.EqualTo(2));
						Assert.That(contributions.Select(c => c.Contribution.ContributorName),
							Is.All.EqualTo("Moe"));
						ValidateSessionLevelDetails(
							contributions.Single(c => c.SpecificFileName == null), session);
						var fileContrib = contributions.Single(c => c.SpecificFileName != null);
						Assert.That(fileContrib.SpecificFileName, Is.EqualTo(metaFile1));
						ValidateSessionLevelDetails(fileContrib, session, true);
						Assert.That(fileContrib.Contribution.Role.Name, Is.EqualTo("Developer"));
						Assert.That(fileContrib.Contribution.Comments, Is.EqualTo("Show host"));
						Assert.That(fileContrib.Contribution.Date, Is.EqualTo(new DateTime(2024, 7, 4)));

						contributions = session.GetAllContributionsFor("Curly").ToList();
						Assert.That(contributions.Count, Is.EqualTo(4));
						Assert.That(contributions.Select(c => c.Contribution.ContributorName),
							Is.All.EqualTo("Curly"));
						var sessionContrib = contributions.Single(c => c.SpecificFileName == null);
						ValidateSessionLevelDetails(sessionContrib, session);
						Assert.That(sessionContrib.Contribution.Role.Name, Is.EqualTo("Consultant"));
						fileContrib = contributions.Single(c => c.SpecificFileName == metaFile1);
						ValidateSessionLevelDetails(fileContrib, session, true);
						Assert.That(fileContrib.Contribution.Role.Name, Is.EqualTo("hopper"));
						Assert.That(fileContrib.Contribution.Comments, Is.Null.Or.Empty);
						contributions = contributions.Where(c => c.SpecificFileName == metaFile2).ToList();
						Assert.That(contributions.Count, Is.EqualTo(2));
						Assert.That(contributions.Select(c => c.Contribution.Role.Name),
							Is.EquivalentTo(new [] { "Data Inputter", "Translator" }));
					}
					finally
					{
						RobustFile.Delete(metaFile2);
					}
				}
				finally
				{
					RobustFile.Delete(metaFile1);
				}
			}
		}


		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCompletedStages_NoParticipantsListed_NoConsent()
		{
			using (var session = CreateSession(new string[] { }))
			{
				var stages = session.GetCompletedStages();
				Assert.IsFalse(stages.Any(s => s.Id == ComponentRole.kConsentComponentRoleId));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCompletedStages_ParticipantsListedButNotFound_NoConsent()
		{
			using (var session = CreateSession(new[] { "you", "me" }))
			{
				var stages = session.GetCompletedStages();
				Assert.IsFalse(stages.Any(s => s.Id == ComponentRole.kConsentComponentRoleId));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCompletedStages_TwoParticipantsFoundOneLacksConsent_NoConsent()
		{
			using (var session = CreateSession(new[] { "oneWithConsent", "none" }))
			{
				var stages = session.GetCompletedStages();
				Assert.IsFalse(stages.Any(s => s.Id == ComponentRole.kConsentComponentRoleId));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCompletedStages_TwoParticipantsFoundBothHaveConsent_ResultIncludesConsent()
		{
			using (var session = CreateSession(new[] { "oneWithConsent", "anotherWithConsent" }))
			{
				var stages = session.GetCompletedStages();
				Assert.IsTrue(stages.Any(s => s.Id == ComponentRole.kConsentComponentRoleId));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnCI")]
		public void GetValidFilesToCopy_TwoMediaFilesToBeCopiedToSessionWithNoExistingSource_FirstOneRenamedAsSource()
		{
			using (var session = CreateSession(new string[] { }))
			{
				const string srcFile1 = @"c:\wherever\whatever.mov";
				const string srcFile2 = @"c:\wherever\whatever.mp3";
				var list = session.GetValidFilesToCopy(
					new[] { srcFile1, srcFile2 }).ToArray();

				Assert.That(list.Length, Is.EqualTo(2));

				var file1 = list.SingleOrDefault(kvp => kvp.Key == srcFile1);
				Assert.IsNotNull(file1);
				Assert.AreEqual(Path.Combine(session.FolderPath, "whatever_Source.mov"), file1.Value);
				var file2 = list.SingleOrDefault(kvp => kvp.Key == srcFile2);
				Assert.IsNotNull(file2);
				Assert.AreEqual(Path.Combine(session.FolderPath, "whatever.mp3"), file2.Value);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnCI")]
		public void GetValidFilesToCopy_TwoMediaFilesToBeCopiedToSessionWithSourceMarkedComplete_FirstOneRenamedAsSource()
		{
			using (var session = CreateSession(new string[] { }))
			{
				const string srcFile1 = @"c:\wherever\whatever.mov";
				const string srcFile2 = @"c:\wherever\whatever.mp3";
				session.StageCompletedControlValues[ComponentRole.kSourceComponentRoleId] = StageCompleteType.Complete;
				var list = session.GetValidFilesToCopy(
					new[] { srcFile1, srcFile2 }).ToArray();

				Assert.That(list.Length, Is.EqualTo(2));

				var file1 = list.SingleOrDefault(kvp => kvp.Key == srcFile1);
				Assert.IsNotNull(file1);
				Assert.AreEqual(Path.Combine(session.FolderPath, Path.GetFileName(srcFile1)), file1.Value);
				var file2 = list.SingleOrDefault(kvp => kvp.Key == srcFile2);
				Assert.IsNotNull(file2);
				Assert.AreEqual(Path.Combine(session.FolderPath, Path.GetFileName(srcFile2)), file2.Value);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalDurationOfSourceMedia_SessionFileIsOnlyComponentFile_ReturnsZero()
		{
			using (var session = CreateSession(new string[] { }))
			{
				Assert.AreEqual(TimeSpan.Zero, session.GetTotalDurationOfSourceMedia());
			}
		}

		
		private static void ValidateSessionLevelDetails(SessionContribution contribution,
			Session session, bool specificFile = false)
		{
			Assert.That(contribution.SessionId, Is.EqualTo(session.Id));
			Assert.That(contribution.SessionTitle, Is.EqualTo(session.Title));
			Assert.That(contribution.SessionDate, Is.EqualTo(session.SessionDate));
			if (!specificFile)
				Assert.That(contribution.SpecificFileName, Is.Null);
		}
	}
}
