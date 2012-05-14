using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using Palaso.Reporting;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.UI.Utilities;

namespace SayMoreTests.Utilities
{
	[TestFixture]
	public class ArchivingDlgViewModelTests
	{
		private TemporaryFolder _tmpFolder;
		private Mock<Event> _event;
		private Mock<Person> _person;
		private Mock<PersonInformant> _personInformant;
		private ArchivingDlgViewModel _helper;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			ErrorReport.IsOkToInteractWithUser = false;

			_tmpFolder = new TemporaryFolder("ArchiveHelperTestFolder");

			CreateEventAndPersonFolders();

			SetupMocks();

			_helper = new ArchivingDlgViewModel(_event.Object, _personInformant.Object);
		}

		/// ------------------------------------------------------------------------------------
		private void CreateEventAndPersonFolders()
		{
			// Create an event
			var folder = Path.Combine(_tmpFolder.Path, "Events");
			Directory.CreateDirectory(folder);
			folder = Path.Combine(folder, "ddo-event");
			Directory.CreateDirectory(folder);
			File.CreateText(Path.Combine(folder, "ddo.event")).Close();
			File.CreateText(Path.Combine(folder, "ddo.mpg")).Close();
			File.CreateText(Path.Combine(folder, "ddo.mp3")).Close();
			File.CreateText(Path.Combine(folder, "ddo.pdf")).Close();

			// Create a person
			folder = Path.Combine(_tmpFolder.Path, "People");
			Directory.CreateDirectory(folder);
			folder = Path.Combine(folder, "ddo-person");
			Directory.CreateDirectory(folder);
			File.CreateText(Path.Combine(folder, "ddoPic.jpg")).Close();
			File.CreateText(Path.Combine(folder, "ddoVoice.wav")).Close();
		}

		/// ------------------------------------------------------------------------------------
		private void SetupMocks()
		{
			var metaFile = new Mock<ProjectElementComponentFile>();
			metaFile.Setup(m => m.GetStringValue("title", null)).Returns("StupidEvent");

			_event = new Mock<Event>();
			_event.Setup(e => e.FolderPath).Returns(Path.Combine(Path.Combine(_tmpFolder.Path, "Events"), "ddo-event"));
			_event.Setup(e => e.GetAllParticipants()).Returns(new[] { "ddo-person" });
			_event.Setup(e => e.Id).Returns("ddo");
			_event.Setup(e => e.MetaDataFile).Returns(metaFile.Object);

			_person = new Mock<Person>();
			_person.Setup(p => p.FolderPath).Returns(Path.Combine(Path.Combine(_tmpFolder.Path, "People"), "ddo-person"));
			_person.Setup(p => p.Id).Returns("ddo-person");

			_personInformant = new Mock<PersonInformant>();
			_personInformant.Setup(i => i.GetPersonByName("ddo-person")).Returns(_person.Object);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_tmpFolder.Dispose();
			_helper.CleanUp();

			try { Directory.Delete(Path.Combine(Path.GetTempPath(), "ddo-event"), true); }
			catch { }

			try { File.Delete(_helper.RampPackagePath); }
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void GetFilesToArchive_GetsCorrectListSize()
		{
			var files = _helper.GetFilesToArchive();
			Assert.AreEqual(2, files.Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void GetFilesToArchive_GetsCorrectEventFiles()
		{
			var files = _helper.GetFilesToArchive();
			Assert.AreEqual(4, files[string.Empty].Count());

			var list = files[string.Empty].Select(f => Path.GetFileName(f)).ToList();
			Assert.Contains("ddo.event", list);
			Assert.Contains("ddo.mpg", list);
			Assert.Contains("ddo.mp3", list);
			Assert.Contains("ddo.pdf", list);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void GetFilesToArchive_ParticipantFileDoNotExist_DoesNotCrash()
		{
			_event.Setup(e => e.GetAllParticipants()).Returns(new[] { "ddo-person", "non-existant-person" });
			_helper.GetFilesToArchive();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void GetFilesToArchive_GetsCorrectPersonFiles()
		{
			var files = _helper.GetFilesToArchive();
			Assert.AreEqual(2, files["ddo-person"].Count());
			var list = files["ddo-person"].Select(f => Path.GetFileName(f)).ToList();
			Assert.Contains("ddoPic.jpg", list);
			Assert.Contains("ddoVoice.wav", list);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateMetsFile_CreatesFile()
		{
			Assert.IsTrue(_helper.CreateMetsFile());
			Assert.IsTrue(File.Exists(Path.Combine(Path.GetTempPath(), "mets.xml")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void CreateRampPackageWithEventArchiveAndMetsFile_CreatesRampPackage()
		{
			int dummy;
			_helper.Initialize(out dummy, null);
			_helper.CreateMetsFile();
			Assert.IsTrue(_helper.CreateRampPackage());
			Assert.IsTrue(File.Exists(_helper.RampPackagePath));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRecordingExtent_NullList_ReturnsNull()
		{
			Assert.IsNull(_helper.GetRecordingExtent(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRecordingExtent_EmptyList_ReturnsNull()
		{
			Assert.IsNull(_helper.GetRecordingExtent(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRecordingExtent_ValidTimesInList_ReturnsTotalTimeAsString()
		{
			var times = new[] { "00:50:10", "03:20:03", "00:04:27" };
			Assert.AreEqual("04:14:40", _helper.GetRecordingExtent(times));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMode_NullList_ReturnsNull()
		{
			Assert.IsNull(_helper.GetMode(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMode_EmptyList_ReturnsNull()
		{
			Assert.IsNull(_helper.GetMode(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMode_SingleTypeInList_ReturnsCorrectMetsList()
		{
			Assert.AreEqual("\"dc.type.mode\":[\"Video\"]", _helper.GetMode(new[] { "blah.mpg" }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMode_MultipleTypesInList_ReturnsCorrectMetsList()
		{
			var mode =_helper.GetMode(new[] { "blah.mp3", "blah.doc", "blah.mov" });
			Assert.AreEqual("\"dc.type.mode\":[\"Speech\",\"Text\",\"Video\"]", mode);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetMode_ListContainsMultiplesOfOneType_ReturnsOnlyOneTypeInList()
		{
			var mode = _helper.GetMode(new[] { "blah.mp3", "blah.wma", "blah.wav" });
			Assert.AreEqual("\"dc.type.mode\":[\"Speech\"]", mode);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSourceFilesForMetsData_ListContainsOnlyEventMetaFile_ReturnsCorrectMetsData()
		{
			var fileLists = new Dictionary<string, IEnumerable<string>>();
			fileLists[string.Empty] = new[] { "blah.event" };

			var expected = "\" \":\"blah.event\",\"description\":\"SayMore Event Metadata (XML)\",\"relationship\":\"source\"";
			Assert.AreEqual(expected, _helper.GetSourceFilesForMetsData(fileLists).ElementAt(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSourceFilesForMetsData_ListContainsOnlyPersonMetaFile_ReturnsCorrectMetsData()
		{
			var fileLists = new Dictionary<string, IEnumerable<string>>();
			fileLists[string.Empty] = new[] { "blah.person" };

			var expected = "\" \":\"blah.person\",\"description\":\"SayMore Contributor Metadata (XML)\",\"relationship\":\"source\"";
			Assert.AreEqual(expected, _helper.GetSourceFilesForMetsData(fileLists).ElementAt(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSourceFilesForMetsData_ListContainsOnlyMetaFile_ReturnsCorrectMetsData()
		{
			var fileLists = new Dictionary<string, IEnumerable<string>>();
			fileLists[string.Empty] = new[] { "blah.meta" };

			var expected = "\" \":\"blah.meta\",\"description\":\"SayMore File Metadata (XML)\",\"relationship\":\"source\"";
			Assert.AreEqual(expected, _helper.GetSourceFilesForMetsData(fileLists).ElementAt(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSourceFilesForMetsData_ListContainsGenericEventFile_ReturnsCorrectMetsData()
		{
			var fileLists = new Dictionary<string, IEnumerable<string>>();
			fileLists[string.Empty] = new[] { "blah.wav" };

			var expected = "\" \":\"blah.wav\",\"description\":\"SayMore Event File\",\"relationship\":\"source\"";
			Assert.AreEqual(expected, _helper.GetSourceFilesForMetsData(fileLists).ElementAt(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSourceFilesForMetsData_ListContainsGenericPersonFile_ReturnsCorrectMetsData()
		{
			var fileLists = new Dictionary<string, IEnumerable<string>>();
			fileLists["person id"] = new[] { "blah.wav" };

			var expected = "\" \":\"Contributors/person id/blah.wav\",\"description\":\"SayMore Contributor File\",\"relationship\":\"source\"";
			Assert.AreEqual(expected, _helper.GetSourceFilesForMetsData(fileLists).ElementAt(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSourceFilesForMetsData_ListMultipleFiles_ReturnsCorrectMetsData()
		{
			var fileLists = new Dictionary<string, IEnumerable<string>>();
			fileLists[string.Empty] = new[] { "blah.event", "baa.wav" };
			fileLists["person id"] = new[] { "blah.person", "baa.mpg", "baa.mpg.meta" };

			Assert.AreEqual("\" \":\"blah.event\",\"description\":\"SayMore Event Metadata (XML)\",\"relationship\":\"source\"",
				_helper.GetSourceFilesForMetsData(fileLists).ElementAt(0));

			Assert.AreEqual("\" \":\"baa.wav\",\"description\":\"SayMore Event File\",\"relationship\":\"source\"",
				_helper.GetSourceFilesForMetsData(fileLists).ElementAt(1));

			Assert.AreEqual("\" \":\"Contributors/person id/blah.person\",\"description\":\"SayMore Contributor Metadata (XML)\",\"relationship\":\"source\"",
				_helper.GetSourceFilesForMetsData(fileLists).ElementAt(2));

			Assert.AreEqual("\" \":\"Contributors/person id/baa.mpg\",\"description\":\"SayMore Contributor File\",\"relationship\":\"source\"",
				_helper.GetSourceFilesForMetsData(fileLists).ElementAt(3));

			Assert.AreEqual("\" \":\"Contributors/person id/baa.mpg.meta\",\"description\":\"SayMore File Metadata (XML)\",\"relationship\":\"source\"",
				_helper.GetSourceFilesForMetsData(fileLists).ElementAt(4));
		}
	}
}
