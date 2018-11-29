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
using SIL.Archiving.Generic;
using SIL.Media.Naudio;
using SIL.Windows.Forms.ClearShare;
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
			_session.Participants = new[] { "ddo-person", "non-existant-person" };

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

			_session.MediaFiles = new[] { sourceMediaFile1.Object, sourceMediaFile2.Object, sourceMediaFile3.Object };
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

			_session.MediaFiles = new[] { sourceMediaFile1.Object, sourceMediaFile2.Object, sourceMediaFile3.Object };
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
			var actor = ArchivingHelper.InitializeActor(model.Object, person.Object, DateTime.MinValue, "Particpant");
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
			var actor = ArchivingHelper.InitializeActor(model.Object, person.Object, new DateTime(2018, 1, 1), "Participant");
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
			ArchivingHelper.Project = null;
			var returnValue = ArchivingHelper.GetOneLanguage("qba");
			Assert.IsNull(returnValue);
		}

		[Test]
		public void GetOneLanguage_UnDefinedName_ReturnsNull()
		{
			ArchivingHelper.Project = null;
			var returnValue = ArchivingHelper.GetOneLanguage("Phony Language");
			Assert.IsNull(returnValue);
		}

		[Test]
		public void GetOneLanguage_UnknownLanguageCodeOrName_ReturnsNull()
		{
			// Putting a language on the project because at one time this was a default value for GetOneLanguage.
			// But even if the project has a language, and unknown language is unknown.
			var project = new Mock<Project>(MockBehavior.Strict, Path.Combine(Path.GetTempPath(), "foo", "foo." + Project.ProjectSettingsFileExtension), null, null);
			// tru:Turoyo is now covered by our ISO language code data.
			project.Object.VernacularISO3CodeAndName = "tru:Turoyo";
			ArchivingHelper.Project = project.Object;
			var returnValue = ArchivingHelper.GetOneLanguage("tru");
			Assert.That(returnValue, Is.Not.Null);
			returnValue = ArchivingHelper.GetOneLanguage("Turoyo");
			Assert.That(returnValue, Is.Not.Null);
			// So try a fake language that really should be unknown.
			var project2 = new Mock<Project>(MockBehavior.Strict, Path.Combine(Path.GetTempPath(), "foo", "foo." + Project.ProjectSettingsFileExtension), null, null);
			project2.Object.VernacularISO3CodeAndName = "qqq:Phoniness";
			ArchivingHelper.Project = project2.Object;
			returnValue = ArchivingHelper.GetOneLanguage("qqq");
			Assert.That(returnValue, Is.Null);
			returnValue = ArchivingHelper.GetOneLanguage("Phoniness");
			Assert.That(returnValue, Is.Null);
		}

		/// <see cref="en.wikipedia.org/wiki/List_of_ISO_639-2_codes"/>
		[Test]
		[Category("SkipOnTeamCity")]
		public void GetOneLanguage_PrivateUseIso_ReturnsNull()
		{
			ArchivingHelper.Project = null;
			// The first code in the private area is actually defined for us.
			var returnValue = ArchivingHelper.GetOneLanguage("qaa");
			Assert.IsNotNull(returnValue);
			Assert.AreEqual("Unlisted Language", returnValue.EnglishName);
			Assert.AreEqual(returnValue.Iso3Code, "qaa");
			// But the second and following codes are undefined.
			returnValue = ArchivingHelper.GetOneLanguage("qab");
			Assert.IsNull(returnValue);
		}

		/// <see cref="en.wikipedia.org/wiki/List_of_ISO_639-2_codes"/>
		[Test]
		public void GetOneLanguage_MissingIso_ReturnsNull()
		{
			ArchivingHelper.Project = null;
			var returnValue = ArchivingHelper.GetOneLanguage("qzz");
			Assert.IsNull(returnValue);
		}

		/// <see cref="en.wikipedia.org/wiki/List_of_ISO_639-2_codes"/>
		[Test]
		public void GetOneLanguage_UndeterminedIso_ReturnsNull()
		{
			ArchivingHelper.Project = null;
			var returnValue = ArchivingHelper.GetOneLanguage("und");
			Assert.IsNull(returnValue);
		}

		[Test]
		public void AnalysisLanguage_DefaultCase_ReturnsEnglishCode()
		{
			Assert.AreEqual("eng: English", ArchivingHelper.AnalysisLanguage());
		}

		[Test]
		public void AddIMDISession_RecordingEquipment_EquipmentInKeysOfModelIMDISession()
		{
			var sourceMediaFile1 = IMDIMediaFileSetup();
			sourceMediaFile1.Object.MetaDataFieldValues.Add(new FieldInstance("Device", "Computer"));
			sourceMediaFile1.Object.MetaDataFieldValues.Add(new FieldInstance("Microphone", "Zoom"));
			var model = AddIMDISessionTestSetup(out var imdiSession, sourceMediaFile1.Object);
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
			const string sampleEthnicGroup = "Ewondo";
			_personMetaFile.Setup(m => m.GetStringValue("ethnicGroup", It.IsAny<string>())).Returns(sampleEthnicGroup);
			var model = AddIMDISessionTestSetup(out var imdiSession);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var actor = imdiSession.Object.MDGroup.Actors.Actor.FirstOrDefault();
			Assert.AreEqual(sampleEthnicGroup, actor.EthnicGroup);
		}

		public void AddIMDISession_AddSituation_AddSituationAsContentKey()
		{
			const string sampleSituation = "village hut recording";
			_session.MetaFile.Setup(f => f.GetStringValue("situation", It.IsAny<string>())).Returns(sampleSituation);
			var model = AddIMDISessionTestSetup(out var imdiSession);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var situationValue = (from k in imdiSession.Object.MDGroup.Content.Keys.Key
				where k.Name == "Situation"		// The keyword in the saymore session is lowercase but in Imdi we chose uppercase
				select k.Value).FirstOrDefault();
			Assert.AreEqual(sampleSituation, situationValue);
		}

		[Test]
		public void AddIMDISession_SessionCustomField_AddCustomKeyValueAsContentKeyValue()
		{
			const string sampleKey = "SampleKey";
			const string sampleValue = "My Sample Value";
			_session.MetaFile.Setup(f => f.GetCustomFields()).Returns(new[] {new FieldInstance(XmlFileSerializer.kCustomFieldIdPrefix + sampleKey, sampleValue)});
			var model = AddIMDISessionTestSetup(out var imdiSession);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from k in imdiSession.Object.MDGroup.Content.Keys.Key
				where k.Name == sampleKey
				select k.Value).FirstOrDefault();
			Assert.AreEqual(sampleValue, val);
		}

		[Test]
		public void AddIMDISession_SessionElarTopicKeyword_AddSingleTopicMultipleKeywords()
		{
			const string sampleTopic = "ELAR Topic";
			const string sampleTopicValue = "My Topic";
			const string sampleKeyword = "ELAR Keyword";
			const string sampleKeywordValue = "Village, Uncolonized";
			_session.MetaFile.Setup(f => f.GetCustomFields()).Returns(new[]
			{
				new FieldInstance(XmlFileSerializer.kCustomFieldIdPrefix + sampleTopic, sampleTopicValue),
				new FieldInstance(XmlFileSerializer.kCustomFieldIdPrefix + sampleKeyword, sampleKeywordValue)
			});
			var model = AddIMDISessionTestSetup(out var imdiSession);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from k in imdiSession.Object.MDGroup.Content.Keys.Key
				where k.Name == "Topic"
				select k.Value).FirstOrDefault();
			Assert.AreEqual(sampleTopicValue, val);
			var val1 = (from k in imdiSession.Object.MDGroup.Content.Keys.Key
				where k.Name == "Keyword"
				select k.Value).FirstOrDefault();
			Assert.AreEqual("Village", val1);
			var val2 = (from k in imdiSession.Object.MDGroup.Content.Keys.Key
				where k.Value == "Uncolonized"
				select k.Name).FirstOrDefault();
			Assert.AreEqual("Keyword", val2);
		}

		[Test]
		public void AddIMDISession_PersonCustomField_AddCustomKeyValueAsPersonKeyValue()
		{
			const string sampleKey = "SampleKey";
			const string sampleValue = "My Sample Value";
			_personMetaFile.Setup(f => f.GetCustomFields()).Returns(new[] { new FieldInstance(XmlFileSerializer.kCustomFieldIdPrefix + sampleKey, sampleValue) });
			var model = AddIMDISessionTestSetup(out var imdiSession);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from a in imdiSession.Object.MDGroup.Actors.Actor
				from k in a.Keys.Key
				where k.Name == sampleKey
				select k.Value).FirstOrDefault();
			Assert.AreEqual(sampleValue, val);
		}

		[Test]
		public void AddIMDISession_AddNotes_AddContentDescription()
		{
			const string sampleNote = "Recording was made externally";
			_session.MetaFile.Setup(f => f.GetStringValue("notes", It.IsAny<string>())).Returns(sampleNote);
			var model = AddIMDISessionTestSetup(out var imdiSession);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from d in imdiSession.Object.MDGroup.Content.Description
				select d.Value).FirstOrDefault();
			Assert.AreEqual(sampleNote, val);
		}

		[Test]
		public void AddIMDISession_ContentLanguage_AddTruToContentLanguagesWithContentDescription()
		{
			const string sampleLanguageCodeAndName = "tru:Turoyo";
			var project = new Mock<Project>(MockBehavior.Strict, Path.Combine(Path.GetTempPath(), "foo", "foo." + Project.ProjectSettingsFileExtension), null, null);
			project.Object.VernacularISO3CodeAndName = sampleLanguageCodeAndName;
			project.Object.AnalysisISO3CodeAndName = "eng:English";
			var model = AddIMDISessionTestSetup(out var imdiSession);
			ArchivingHelper.Project = project.Object;
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from c in imdiSession.Object.MDGroup.Content.Languages.Language
				from n in c.Name
				from d in c.Description
				where d.Value.Contains("Content")
				select n.Value).FirstOrDefault();
			Assert.IsTrue(sampleLanguageCodeAndName.Contains(val));
			ArchivingHelper.Project = null;
		}

		[Test]
		public void AddIMDISession_WorkingLanguage_AddFraToContentLanguagesWithWorkingDescription()
		{
			const string sampleLanguageCodeAndName = "fra:French";
			var project = new Mock<Project>(MockBehavior.Strict, Path.Combine(Path.GetTempPath(), "foo", "foo." + Project.ProjectSettingsFileExtension), null, null);
			project.Object.VernacularISO3CodeAndName = "tru:Turoyo";
			project.Object.AnalysisISO3CodeAndName = sampleLanguageCodeAndName;
			var model = AddIMDISessionTestSetup(out var imdiSession);
			ArchivingHelper.Project = project.Object;
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from c in imdiSession.Object.MDGroup.Content.Languages.Language
				from n in c.Name
				from d in c.Description
				where d.Value.Contains("Working")
				select n.Value).FirstOrDefault();
			Assert.IsTrue(sampleLanguageCodeAndName.Contains(val));
			ArchivingHelper.Project = null;
		}

		[Test]
		public void AddIMDISession_PersonNotes_AddPersonDescription()
		{
			const string sampleValue = "I met him in college";
			_personMetaFile.Object.MetaDataFieldValues.Add(new FieldInstance("notes", sampleValue));
			var model = AddIMDISessionTestSetup(out var imdiSession);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from a in imdiSession.Object.MDGroup.Actors.Actor
				from d in a.Description
				select d.Value).FirstOrDefault();
			Assert.AreEqual(sampleValue, val);
		}

		[Test]
		public void AddIMDISession_AddResourceNotes_AddAsResourceDescription()
		{
			const string sampleNote = "A personal history";
			var sourceMediaFile1 = IMDIMediaFileSetup();
			sourceMediaFile1.Object.MetaDataFieldValues.Add(new FieldInstance("notes", sampleNote));
			var model = AddIMDISessionTestSetup(out var imdiSession, sourceMediaFile1.Object);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from f in imdiSession.Object.Resources.MediaFile
				from d in f.Description
				where d.Value != null
				select d.Value).FirstOrDefault();
			Assert.AreEqual(sampleNote, val);
		}

		[Test]
		public void AddIMDISession_PersonContact_AddActorContactAddress()
		{
			const string sampleValue = "house next to flag pole";
			_personMetaFile.Setup(m => m.GetStringValue("howToContact", It.IsAny<string>())).Returns(sampleValue);
			var model = AddIMDISessionTestSetup(out var imdiSession);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from a in imdiSession.Object.MDGroup.Actors.Actor
				where a.Contact != null
				select a.Contact.Address).FirstOrDefault();
			Assert.AreEqual(sampleValue, val);
		}

		[Test]
		public void AddIMDISession_ExportSessionContributors_SessionContributorsIncluded()
		{
			const string samplePerson = "Jane Doe";
			var contributionCollection = new Mock<ContributionCollection>(MockBehavior.Strict);
			contributionCollection.Object.Add(new Contribution(samplePerson, new Role("rdr", "Reader", null)));
			var sourceMediaFile1 = IMDIMediaFileSetup();
			sourceMediaFile1.Object.MetaDataFieldValues.Add(new FieldInstance("contributions", contributionCollection.Object));
			_session.MetaFile.Setup(m => m.GetStringValue(SessionFileType.kParticipantsFieldName, It.IsAny<string>())).Returns("ddo-person (Editor)");
			_session.MetaFile.Object.MetaDataFieldValues.Add(new FieldInstance("participants", "ddo-person (Editor)"));
			var sessionContributions = new Mock<ContributionCollection>(MockBehavior.Strict);
			sessionContributions.Object.Add(new Contribution("ddo-person", new Role("edt", "Editor", null)));
			_session.MetaFile.Setup(m => m.GetValue(SessionFileType.kContributionsFieldName, null))
				.Returns(sessionContributions.Object);
			var model = AddIMDISessionTestSetup(out var imdiSession, sourceMediaFile1.Object);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from a in imdiSession.Object.MDGroup.Actors.Actor
				from n in a.Name
				where n == samplePerson
				select n).FirstOrDefault();
			Assert.AreEqual(samplePerson, val);
			var actor = imdiSession.Object.MDGroup.Actors.Actor.FirstOrDefault(a => a.FullName.Contains("ddo-person"));
			Assert.That(actor.Role.Value, Is.EqualTo("Editor"));
		}

		[Test]
		public void AddIMDISession_MediaDuration_IMDIMediaFileDurationSet()
		{
			const string sampleDuration = "06:03:00";
			var sourceMediaFile1 = IMDIMediaFileSetup();
			sourceMediaFile1.Object.MetaDataFieldValues.Add(new FieldInstance("Duration", sampleDuration));
			var model = AddIMDISessionTestSetup(out var imdiSession, sourceMediaFile1.Object);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from f in imdiSession.Object.Resources.MediaFile
				where f.TimePosition.End != null
				where f.TimePosition.End != "Unspecified"
				select f.TimePosition.End).FirstOrDefault();
			Assert.AreEqual(sampleDuration, val);
		}

		[Test]
		public void AddIMDISession_PersonCode_AddActorHasSameCode()
		{
			const string sampleCode = "123";
			_personMetaFile.Setup(m => m.GetStringValue(PersonFileType.kCode, It.IsAny<string>())).Returns(sampleCode);
			var model = AddIMDISessionTestSetup(out var imdiSession);
			ArchivingHelper.AddIMDISession(_session, model.Object);
			var val = (from a in imdiSession.Object.MDGroup.Actors.Actor
				where !string.IsNullOrEmpty(a.Code)
				select a.Code).FirstOrDefault();
			Assert.AreEqual(sampleCode, val);
		}

		private Mock<IMDIArchivingDlgViewModel> AddIMDISessionTestSetup(out Mock<SIL.Archiving.IMDI.Schema.Session> imdiSession, ComponentFile mediaFile = null)
		{
			var model = new Mock<IMDIArchivingDlgViewModel>(MockBehavior.Strict, "SayMore", "ddo", "ddo-session", "whatever",
				false, null, @"C:\my_imdi_folder");
			imdiSession = new Mock<SIL.Archiving.IMDI.Schema.Session>(MockBehavior.Strict);
			imdiSession.Object.Name = "ddo";
			model.Setup(m => m.AddSession(_session.Id)).Returns(imdiSession.Object);
			_session.MediaFiles = mediaFile == null ? new ComponentFile[0] : new[] { mediaFile };
			// We have to use real objects here because ArchivingPackage.FundingProject is not virtual.
			// We typically need an ArchivingPackage with a FundingProject because Session.AddProject()
			// uses FundingProject.Name.
			var ap = new IMDIPackage(false, "");
			model.Setup(m => m.ArchivingPackage).Returns(ap);
			ap.FundingProject = new ArchivingProject();
			return model;
		}

		private Mock<ComponentFile> IMDIMediaFileSetup()
		{
			var fileType = new Mock<AudioFileType>(null, null, null);
			fileType.Setup(t => t.IsAudioOrVideo).Returns(true);
			var sourceMediaFile1 = new Mock<ComponentFile>(MockBehavior.Strict, fileType.Object);
			sourceMediaFile1.Setup(f => f.PathToAnnotatedFile).Returns(_mp3FullName);
			return sourceMediaFile1;
		}
		#endregion
	}

	public class DummySession : Session
	{
		public string[] Participants;
		public readonly Mock<ProjectElementComponentFile> MetaFile = new Mock<ProjectElementComponentFile>();
		public ComponentFile[] MediaFiles;

		public DummySession(string parentFolder, string name, PersonInformant personInformant, params string[] actors) : base(parentFolder, name + "-session", null, new SessionFileType(() => null, () => null, () => null),
				MakeComponent, new XmlFileSerializer(null), (w, x, y, z) =>
					new ProjectElementComponentFile(w, x, y, z, FieldUpdater.CreateMinimalFieldUpdaterForTests(null)),
					ApplicationContainer.ComponentRoles, personInformant, null)
		{
			if (actors == null || actors.Length == 0)
				Participants = new[] {"ddo-person"};
			else
				Participants = actors;
			MetaFile.Setup(m => m.GetStringValue(SessionFileType.kTitleFieldName, It.IsAny<string>())).Returns(name);
			MetaFile.Setup(m => m.GetCustomFields()).Returns(new List<FieldInstance>());
		}

		public override IEnumerable<string> GetAllParticipants()
		{
			return Participants;
		}

		public override ProjectElementComponentFile MetaDataFile
		{
			get { return MetaFile.Object; }
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

					foreach (ComponentFile componentFile in MediaFiles)
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
