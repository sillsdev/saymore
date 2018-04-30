using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using SIL.Reporting;
using SIL.TestUtilities;
using SIL.Archiving.IMDI;
using SayMore;
using SayMore.Model;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SIL.Archiving;
using SayMore.Properties;
using Session = SayMore.Model.Session;

namespace SayMoreTests.Utilities
{
	[TestFixture]
	public class SessionArchivingTests
	{
		private string _dummyProjectName;
		private TemporaryFolder _tmpFolder;
		private DummySession _session;
		private Mock<Person> _person;
		private Mock<PersonInformant> _personInformant;
		private readonly Mock<ProjectElementComponentFile> _personMetaFile = new Mock<ProjectElementComponentFile>();
		private readonly Mock<ProjectElementComponentFile> _fileMetaFile = new Mock<ProjectElementComponentFile>();
		private string _mp3FullName = String.Empty;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			ErrorReport.IsOkToInteractWithUser = false;

			_dummyProjectName = "ArchiveHelperTestFolder";
			_tmpFolder = new TemporaryFolder(_dummyProjectName);

			CreateSessionAndMockedPerson();
		}

		/// ------------------------------------------------------------------------------------
		private void CreateSessionAndMockedPerson()
		{
			// Create a person
			var folder = Path.Combine(_tmpFolder.Path, Person.kFolderName);
			Directory.CreateDirectory(folder);
			folder = Path.Combine(folder, "ddo-person");
			Directory.CreateDirectory(folder);
			File.CreateText(Path.Combine(folder, "ddo-person.person")).Close();
			File.CreateText(Path.Combine(folder, "ddoPic.jpg")).Close();
			File.CreateText(Path.Combine(folder, "ddoVoice.wav")).Close();

			_person = new Mock<Person>();
			_person.Setup(p => p.FolderPath).Returns(Path.Combine(Path.Combine(_tmpFolder.Path, Person.kFolderName), "ddo-person"));
			_person.Setup(p => p.Id).Returns("ddo-person");
			_personMetaFile.Setup(m => m.GetStringValue(PersonFileType.kCode, It.IsAny<string>())).Returns("1");
			_personMetaFile.Setup(m => m.GetStringValue("privacyProtection", It.IsAny<string>())).Returns("false");
			_personMetaFile.Setup(m => m.GetStringValue("birthYear", It.IsAny<string>())).Returns("2000");
			_person.Setup(p => p.MetaDataFile).Returns(_personMetaFile.Object);


			_personInformant = new Mock<PersonInformant>();
			_personInformant.Setup(i => i.GetPersonByNameOrCode("ddo-person")).Returns(_person.Object);

			// Create a session
			var parentFolder = Path.Combine(_tmpFolder.Path, Session.kFolderName);
			Directory.CreateDirectory(parentFolder);
			folder = Path.Combine(parentFolder, "ddo-session");
			Directory.CreateDirectory(folder);
			File.CreateText(Path.Combine(folder, "ddo.session")).Close();
			File.CreateText(Path.Combine(folder, "ddo.mpg")).Close();
			_mp3FullName = Path.Combine(folder, "ddo.mp3");
			File.CreateText(_mp3FullName).Close();
			File.CreateText(Path.Combine(folder, "ddo.pdf")).Close();
			_session = new DummySession(parentFolder, "ddo", _personInformant.Object);

			// create a project file
			var projFileName = _dummyProjectName + Settings.Default.ProjectFileExtension;
			File.CreateText(Path.Combine(_tmpFolder.Path, projFileName)).Close();
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_tmpFolder.Dispose();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void SetFilesToArchive_RAMP_GetsCorrectListSizeIncludingSessionFile()
		{
			var model = new Mock<RampArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo",
				"ddo-session", "whatever", null, new Func<string, string, string>((a, b) =>a));
			model.Setup(s => s.AddFileGroup(string.Empty, It.Is<IEnumerable<string>>(e => e.Count() == 4), "Adding Files for Session 'ddo'"));
			model.Setup(s => s.AddFileGroup("ddo-person", It.Is<IEnumerable<string>>(e => e.Count() == 3), "Adding Files for Contributor 'ddo-person'"));
			_session.SetFilesToArchive(model.Object);
			model.VerifyAll();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void SetFilesToArchive_IMDI_GetsCorrectListSizeExcludingSessionFile()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo",
				"ddo-session", "whatever", false, null, @"C:\my_imdi_folder");
			model.Setup(s => s.AddFileGroup(string.Empty, It.Is<IEnumerable<string>>(e => e.Count() == 3), "Adding Files for Session 'ddo'"));
			model.Setup(s => s.AddFileGroup("ddo-person", It.Is<IEnumerable<string>>(e => e.Count() == 2), "Adding Files for Contributor 'ddo-person'"));
			_session.SetFilesToArchive(model.Object);
			model.VerifyAll();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void SetFilesToArchive_GetsCorrectSessionAndPersonFiles()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo", "ddo-session", "whatever", false, null, @"C:\my_imdi_folder");

			model.Setup(s => s.AddFileGroup(string.Empty,
				It.Is<IEnumerable<string>>(e => e.Select(Path.GetFileName).Union(new [] {"ddo.session", "ddo.mpg", "ddo.mp3", "ddo.pdf"}).Count() == 4),
				"Adding Files for Session 'ddo'"));

			model.Setup(s => s.AddFileGroup("ddo-person",
				It.Is<IEnumerable<string>>(e => e.Select(Path.GetFileName).Union(new[] { "ddoPic.jpg", "ddoVoice.wav" }).Count() == 2),
				"Adding Files for Contributor 'ddo-person'"));

			_session.SetFilesToArchive(model.Object);
			model.VerifyAll();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void SetFilesToArchive_ParticipantFileDoesNotExist_DoesNotCrash()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo", "ddo-session", "whatever", false, null, @"C:\my_imdi_folder");
			_session._participants = new[] { "ddo-person", "non-existant-person" };

			model.Setup(s => s.AddFileGroup(string.Empty,
				It.Is<IEnumerable<string>>(e => e.Select(Path.GetFileName).Union(new [] {"ddo.session", "ddo.mpg", "ddo.mp3", "ddo.pdf"}).Count() == 4),
				"Adding Files for Session 'ddo'"));

			model.Setup(s => s.AddFileGroup("ddo-person",
				It.Is<IEnumerable<string>>(e => e.Select(Path.GetFileName).Union(new[] { "ddoPic.jpg", "ddoVoice.wav" }).Count() == 2),
				"Adding Files for Contributor 'ddo-person'"));

			_session.SetFilesToArchive(model.Object);
			model.VerifyAll();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalDurationOfSourceMedia_ThreeSourceMediaFiles_ReturnsTotalTime()
		{
			var sourceRoleArray = new[] { ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kSourceComponentRoleId) };

			var sourceMediaFile1 = new Mock<ComponentFile>(null);
			sourceMediaFile1.Setup(f => f.DurationSeconds).Returns(new TimeSpan(0, 50, 10));
			sourceMediaFile1.Setup(f => f.GetAssignedRoles()).Returns(sourceRoleArray);
			var sourceMediaFile2 = new Mock<ComponentFile>(null);
			sourceMediaFile2.Setup(f => f.DurationSeconds).Returns(new TimeSpan(3, 20, 3));
			sourceMediaFile2.Setup(f => f.GetAssignedRoles()).Returns(sourceRoleArray);
			var sourceMediaFile3 = new Mock<ComponentFile>(null);
			sourceMediaFile3.Setup(f => f.DurationSeconds).Returns(new TimeSpan(0, 4, 27));
			sourceMediaFile3.Setup(f => f.GetAssignedRoles()).Returns(sourceRoleArray);

			_session._mediaFiles = new[] { sourceMediaFile1.Object, sourceMediaFile2.Object, sourceMediaFile3.Object };
			Assert.AreEqual(new TimeSpan(4, 14, 40), _session.GetTotalDurationOfSourceMedia());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTotalDurationOfSourceMedia_TwoSourceMediaFilesAndOneNonSourceMediaFile_ReturnsTotalTime()
		{
			var sourceRoleArray = new[] { ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kSourceComponentRoleId) };

			var sourceMediaFile1 = new Mock<ComponentFile>(null);
			sourceMediaFile1.Setup(f => f.DurationSeconds).Returns(new TimeSpan(0, 50, 10));
			sourceMediaFile1.Setup(f => f.GetAssignedRoles()).Returns(sourceRoleArray);
			var sourceMediaFile2 = new Mock<ComponentFile>(null);
			sourceMediaFile2.Setup(f => f.DurationSeconds).Returns(new TimeSpan(3, 20, 3));
			sourceMediaFile2.Setup(f => f.GetAssignedRoles()).Returns(new[]
				{
					ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kConsentComponentRoleId),
					ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kSourceComponentRoleId)
				});
			var sourceMediaFile3 = new Mock<ComponentFile>(null);
			sourceMediaFile3.Setup(f => f.DurationSeconds).Returns(new TimeSpan(0, 4, 27));
			sourceMediaFile3.Setup(f => f.GetAssignedRoles()).Returns(new[]
				{
					ApplicationContainer.ComponentRoles.First(r => r.Id == ComponentRole.kConsentComponentRoleId)
				});

			_session._mediaFiles = new[] { sourceMediaFile1.Object, sourceMediaFile2.Object, sourceMediaFile3.Object };
			Assert.AreEqual(new TimeSpan(4, 10, 13), _session.GetTotalDurationOfSourceMedia());
		}

		#region IMDI Archiving Tests

		[Test]
		public void GetProjectName_ReturnsCorrectProjectName()
		{
			var projname = _session.GetProjectName();
			Assert.AreEqual(_dummyProjectName, projname);
		}

		[Test]
		public void InitializeActor_NoBirthYear_ExpectUnspecifiedAge()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo", "ddo-session", "whatever",
				false, null, @"C:\my_imdi_folder");
			var person = new Mock<Person>();
			person.Setup(p => p.MetaDataFile.GetStringValue("privacyProtection", "false")).Returns("false");
			person.Setup(p => p.MetaDataFile.GetStringValue("birthYear", string.Empty)).Returns(string.Empty);
			var actor = ArchivingHelper.InitializeActor(model.Object, person.Object, DateTime.MinValue);
			Assert.AreEqual("Unspecified", actor.Age);
			model.VerifyAll();
		}

		[Test]
		public void InitializeActor_AgeEqualBirthYearMinusSessionYear_Expect68()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo", "ddo-session", "whatever",
				false, null, @"C:\my_imdi_folder");
			var person = new Mock<Person>();
			person.Setup(p => p.MetaDataFile.GetStringValue("privacyProtection", "false")).Returns("false");
			person.Setup(p => p.MetaDataFile.GetStringValue("birthYear", string.Empty)).Returns("1950");
			var actor = ArchivingHelper.InitializeActor(model.Object, person.Object, new DateTime(2018, 1, 1));
			Assert.AreEqual("68", actor.Age);
			person.VerifyAll();
			model.VerifyAll();
		}

		[Test]
		public void GetOneLanguage_DefinedIso_ReturnsCodeAndName()
		{
			var returnValue = ArchivingHelper.GetOneLanguage("eng");
			Assert.AreEqual("eng", returnValue.Iso3Code);
			Assert.AreEqual("English", returnValue.LanguageName);
			Assert.AreEqual("English", returnValue.EnglishName);
		}

		[Test]
		public void GetOneLanguage_DefinedName_ReturnsCodeAndName()
		{
			var returnValue = ArchivingHelper.GetOneLanguage("English");
			Assert.AreEqual("eng", returnValue.Iso3Code);
			Assert.AreEqual("English", returnValue.LanguageName);
			Assert.AreEqual("English", returnValue.EnglishName);
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GetOneLanguage_UndefinedIso_ReturnsNull()
		{
			ArchivingHelper._defaultLanguage = null;
			var returnValue = ArchivingHelper.GetOneLanguage("tru");
			Assert.IsNull(returnValue);
		}

		[Test]
		public void GetOneLanguage_UnDefinedName_ReturnsNull()
		{
			ArchivingHelper._defaultLanguage = null;
			var returnValue = ArchivingHelper.GetOneLanguage("Turoyo");
			Assert.IsNull(returnValue);
		}

		[Test]
		public void GetOneLanguage_DefaultIso_ReturnsCodeAndName()
		{
			ArchivingHelper._defaultLanguage = new ArchivingLanguage("tru", "Turoyo", "Turoyo");
			var returnValue = ArchivingHelper.GetOneLanguage("tru");
			Assert.AreEqual("tru", returnValue.Iso3Code);
			Assert.AreEqual("Turoyo", returnValue.LanguageName);
			Assert.AreEqual("Turoyo", returnValue.EnglishName);
		}

		[Test]
		public void GetOneLanguage_DefaultName_ReturnsCodeAndName()
		{
			ArchivingHelper._defaultLanguage = new ArchivingLanguage("tru", "Turoyo", "Turoyo");
			var returnValue = ArchivingHelper.GetOneLanguage("Turoyo");
			Assert.AreEqual("tru", returnValue.Iso3Code);
			Assert.AreEqual("Turoyo", returnValue.LanguageName);
			Assert.AreEqual("Turoyo", returnValue.EnglishName);
		}

		/// <see cref="en.wikipedia.org/wiki/List_of_ISO_639-2_codes"/>
		[Test]
		[Category("SkipOnTeamCity")]
		public void GetOneLanguage_PrivateUseIso_ReturnsNull()
		{
			ArchivingHelper._defaultLanguage = null;
			var returnValue = ArchivingHelper.GetOneLanguage("qaa");
			Assert.IsNull(returnValue);
		}

		/// <see cref="en.wikipedia.org/wiki/List_of_ISO_639-2_codes"/>
		[Test]
		public void GetOneLanguage_MissingIso_ReturnsNull()
		{
			ArchivingHelper._defaultLanguage = null;
			var returnValue = ArchivingHelper.GetOneLanguage("mis");
			Assert.IsNull(returnValue);
		}

		/// <see cref="en.wikipedia.org/wiki/List_of_ISO_639-2_codes"/>
		[Test]
		public void GetOneLanguage_UndeterminedIso_ReturnsNull()
		{
			ArchivingHelper._defaultLanguage = null;
			var returnValue = ArchivingHelper.GetOneLanguage("und");
			Assert.IsNull(returnValue);
		}

		[Test]
		public void AddIMDISession_RecordingEquipment_EquipmentInKeysOfModelIMDISession()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo", "ddo-session", "whatever",
				false, null, @"C:\my_imdi_folder");
			var imdiSession = new Mock<SIL.Archiving.IMDI.Schema.Session>(MockBehavior.Strict);
			imdiSession.Object.Name = "ddo";
			model.Setup(m => m.AddSession(_session.Id)).Returns(imdiSession.Object);
			var fileType = new Mock<FileType>(MockBehavior.Strict, "mp3", null);
			fileType.Setup(t => t.IsAudioOrVideo).Returns(true);
			var sourceMediaFile1 = new Mock<ComponentFile>(MockBehavior.Strict, fileType.Object);
			sourceMediaFile1.Setup(f => f.PathToAnnotatedFile).Returns(_mp3FullName);
			sourceMediaFile1.Object.MetaDataFieldValues.Add(new FieldInstance("Device", "Computer"));
			sourceMediaFile1.Object.MetaDataFieldValues.Add(new FieldInstance("Microphone", "Zoom"));
			_session._mediaFiles = new [] {sourceMediaFile1.Object};
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var eqCount = (from f in imdiSession.Object.Resources.MediaFile
				from k in f.Keys.Key
				where k.Name == "RecordingEquipment"
				select k.Name).Count();
			Assert.AreEqual(2, eqCount);
		}

		[Test]
		public void AddIMDISession_SetEthnicGroup_PersonHasEthnicGroup()
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo", "ddo-session", "whatever",
				false, null, @"C:\my_imdi_folder");
			var imdiSession = new Mock<SIL.Archiving.IMDI.Schema.Session>(MockBehavior.Strict);
			imdiSession.Object.Name = "ddo";
			model.Setup(m => m.AddSession(_session.Id)).Returns(imdiSession.Object);
			_session._mediaFiles = new ComponentFile[0];
			const string SampleEthnicGroup = "Ewondo";
			_personMetaFile.Setup(m => m.GetStringValue("ethnicGroup", It.IsAny<string>())).Returns(SampleEthnicGroup);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var actor = imdiSession.Object.MDGroup.Actors.Actor.FirstOrDefault();
			Assert.AreEqual(SampleEthnicGroup, actor.EthnicGroup);
		}
		#endregion
	}

	public class DummySession : Session
	{
		public string[] _participants;
		private readonly Mock<ProjectElementComponentFile> _metaFile = new Mock<ProjectElementComponentFile>();
		public ComponentFile[] _mediaFiles;

		public DummySession(string parentFolder, string name, PersonInformant personInformant, params string [] actors) : base(parentFolder, name + "-session", null, new SessionFileType(() => null, () => null),
				MakeComponent, new XmlFileSerializer(null), (w, x, y, z) =>
					new ProjectElementComponentFile(w, x, y, z, FieldUpdater.CreateMinimalFieldUpdaterForTests(null)),
					ApplicationContainer.ComponentRoles, personInformant, null)
		{
			if (actors == null || actors.Length == 0)
				_participants = new[] {"ddo-person"};
			else
				_participants = actors;
			_metaFile.Setup(m => m.GetStringValue(SessionFileType.kTitleFieldName, It.IsAny<string>())).Returns(name);
			foreach (var key in new[] {
				SessionFileType.kGenreFieldName,
				SessionFileType.kSubGenreFieldName,
				SessionFileType.kAccessFieldName,
				SessionFileType.kInteractivityFieldName,
				SessionFileType.kInvolvementFieldName,
				SessionFileType.kPlanningTypeFieldName,
				SessionFileType.kSocialContextFieldName,
				SessionFileType.kTaskFieldName
				})
			{
				_metaFile.Setup(m => m.GetStringValue(key, It.IsAny<string>())).Returns("x");
			}
			_metaFile.Setup(m => m.GetCustomFields()).Returns(new List<FieldInstance>());
		}

		public override IEnumerable<string> GetAllParticipants()
		{
			return _participants;
		}

		public override ProjectElementComponentFile MetaDataFile
		{
			get { return _metaFile.Object; }
		}

		public override ComponentFile[] GetComponentFiles()
		{
			lock (this)
			{
				// Return a copy of the list to guard against changes
				// on another thread (i.e., from the FileSystemWatcher)
				if (_componentFiles == null)
				{
					_componentFiles = new HashSet<ComponentFile>();

					// This is the actual person or session data
					_componentFiles.Add(MetaDataFile);

					foreach (ComponentFile componentFile in _mediaFiles)
						_componentFiles.Add(componentFile);
				}
				return _componentFiles.ToArray();
			}
		}

		private static ComponentFile MakeComponent(ProjectElement parentElement, string pathtoannotatedfile)
		{
			return ComponentFile.CreateMinimalComponentFileForTests(parentElement, pathtoannotatedfile);
		}
	}


}
