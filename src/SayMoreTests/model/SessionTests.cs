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
		private Session CreateSession(IEnumerable<string> participants,
			Project project = null, Action<Mock<ProjectElementComponentFile>> additionalSetup = null)
		{
			ProjectElementComponentFile.Factory factory = (parentElement, fileType, fileSerializer, rootElementName) =>
				{
					var file = new Mock<ProjectElementComponentFile>();
					file.Setup(f => f.Save());
					//file.Setup(
					//	f => f.GetStringValue(SessionFileType.kParticipantsFieldName, string.Empty)).
					//	Returns(participants.Count() > 0 ? participants.Aggregate((a, b) => a + ";" + b) : string.Empty
					//	);
					var contributions = new Mock<ContributionCollection>(MockBehavior.Strict);
					foreach (var p in participants)
					{
						var parts = p.Split('/');
						var role = (parts.Length == 1) ? new Role("par", "Participant", null) :
							new Role(parts[1], parts[2], null);
						var contrib = new Contribution(parts[0], role);
						if (parts.Length > 3)
							contrib.Comments = parts[3];
						contributions.Object.Add(contrib);
					}

					file.Setup(f => f.GetValue(SessionFileType.kContributionsFieldName, null)).Returns(contributions.Object);

					if (additionalSetup != null)
					{
						additionalSetup(file);
						_mockedSessionMetaDataFile = file;
					}

					return file.Object;
				};

			Func<ProjectElement, string, ComponentFile> componentFactory = (parentElement, path) =>
				{
					var file = new Mock<ComponentFile>();
					//person.Setup(p => p.GetInformedConsentComponentFile()).Returns((ComponentFile)null);
					file.Setup(p => p.Save());
					return file.Object;
				};

			var personInformant = new Mock<PersonInformant>();
			foreach (var participant in participants)
			{
				personInformant.Setup(i => i.GetHasInformedConsent(participant)).Returns(participant.Contains("Consent"));
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
					new Lazy<Func<ContributorsEditor.Factory>>(() => null)), componentFactory,
				new XmlFileSerializer(null), factory, componentRoles, personInformant.Object, project);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Constructor_NewSession_ProjectLocationInfoCopiedToSessionAndGenreSetToUnknown()
		{
			var project = new Project(_parentFolder.Combine("foo", "foo." + Project.ProjectSettingsFileExtension), null, null);
			project.Country = "Fred";
			project.Continent = "Ocean Floor";
			project.Region = "Sideways";
			project.Location = "Upstairs";

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
		[Category("SkipOnTeamCity")]
		public void GetFilesToCopy_TwoMediaFilesToBeCopiedToSessionWithNoExistingSource_FirstOneRenamedAsSource()
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
		[Category("SkipOnTeamCity")]
		public void GetFilesToCopy_TwoMediaFilesToBeCopiedToSessionWithSourceMarkedComplete_FirstOneRenamedAsSource()
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
	}
}
