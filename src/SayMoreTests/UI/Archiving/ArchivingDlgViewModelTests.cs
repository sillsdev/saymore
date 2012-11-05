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
		private Mock<Session> _session;
		private Mock<Person> _person;
		private Mock<PersonInformant> _personInformant;
		private ArchivingDlgViewModel _helper;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			ErrorReport.IsOkToInteractWithUser = false;

			_tmpFolder = new TemporaryFolder("ArchiveHelperTestFolder");

			CreateSessionAndPersonFolders();

			SetupMocks();

			_helper = new ArchivingDlgViewModel(_session.Object, _personInformant.Object);
		}

		/// ------------------------------------------------------------------------------------
		private void CreateSessionAndPersonFolders()
		{
			// Create a session
			var folder = Path.Combine(_tmpFolder.Path, "Sessions");
			Directory.CreateDirectory(folder);
			folder = Path.Combine(folder, "ddo-session");
			Directory.CreateDirectory(folder);
			File.CreateText(Path.Combine(folder, "ddo.session")).Close();
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
			metaFile.Setup(m => m.GetStringValue("title", null)).Returns("StupidSession");

			_session = new Mock<Session>();
			_session.Setup(e => e.FolderPath).Returns(Path.Combine(Path.Combine(_tmpFolder.Path, "Sessions"), "ddo-session"));
			_session.Setup(e => e.GetAllParticipants()).Returns(new[] { "ddo-person" });
			_session.Setup(e => e.Id).Returns("ddo");
			_session.Setup(e => e.MetaDataFile).Returns(metaFile.Object);

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

			try { Directory.Delete(Path.Combine(Path.GetTempPath(), "ddo-session"), true); }
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
		public void GetFilesToArchive_GetsCorrectSessionFiles()
		{
			var files = _helper.GetFilesToArchive();
			Assert.AreEqual(4, files[string.Empty].Count());

			var list = files[string.Empty].Select(f => Path.GetFileName(f)).ToList();
			Assert.Contains("ddo.session", list);
			Assert.Contains("ddo.mpg", list);
			Assert.Contains("ddo.mp3", list);
			Assert.Contains("ddo.pdf", list);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void GetFilesToArchive_ParticipantFileDoNotExist_DoesNotCrash()
		{
			_session.Setup(e => e.GetAllParticipants()).Returns(new[] { "ddo-person", "non-existant-person" });
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
			var metsPath = _helper.CreateMetsFile();
			Assert.IsNotNull(metsPath);
			Assert.IsTrue(File.Exists(metsPath));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void CreateRampPackageWithSessionArchiveAndMetsFile_CreatesRampPackage()
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
		public void GetSourceFilesForMetsData_ListContainsOnlySessionMetaFile_ReturnsCorrectMetsData()
		{
			var fileLists = new Dictionary<string, IEnumerable<string>>();
			fileLists[string.Empty] = new[] { "blah.session" };

			var expected = "\" \":\"blah.session\",\"description\":\"SayMore Session Metadata (XML)\",\"relationship\":\"source\"";
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
		public void GetSourceFilesForMetsData_ListContainsGenericSessionFile_ReturnsCorrectMetsData()
		{
			var fileLists = new Dictionary<string, IEnumerable<string>>();
			fileLists[string.Empty] = new[] { "blah.wav" };

			var expected = "\" \":\"blah.wav\",\"description\":\"SayMore Session File\",\"relationship\":\"source\"";
			Assert.AreEqual(expected, _helper.GetSourceFilesForMetsData(fileLists).ElementAt(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSourceFilesForMetsData_ListContainsGenericPersonFile_ReturnsCorrectMetsData()
		{
			var fileLists = new Dictionary<string, IEnumerable<string>>();
			fileLists["Carmen"] = new[] { "Carmen_blah.wav" };

			var expected = "\" \":\"__Contributors__Carmen_blah.wav\",\"description\":\"SayMore Contributor File\",\"relationship\":\"source\"";
			Assert.AreEqual(expected, _helper.GetSourceFilesForMetsData(fileLists).ElementAt(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetSourceFilesForMetsData_ListMultipleFiles_ReturnsCorrectMetsData()
		{
			var fileLists = new Dictionary<string, IEnumerable<string>>();
			fileLists[string.Empty] = new[] { "blah.session", "really cool.wav" };
			fileLists["person id"] = new[] { "person id_blah.person", "person id_baa.mpg", "person id_baa.mpg.meta" };

			Assert.AreEqual("\" \":\"blah.session\",\"description\":\"SayMore Session Metadata (XML)\",\"relationship\":\"source\"",
				_helper.GetSourceFilesForMetsData(fileLists).ElementAt(0));

			Assert.AreEqual("\" \":\"really+cool.wav\",\"description\":\"SayMore Session File\",\"relationship\":\"source\"",
				_helper.GetSourceFilesForMetsData(fileLists).ElementAt(1));

			Assert.AreEqual("\" \":\"__Contributors__person+id_blah.person\",\"description\":\"SayMore Contributor Metadata (XML)\",\"relationship\":\"source\"",
				_helper.GetSourceFilesForMetsData(fileLists).ElementAt(2));

			Assert.AreEqual("\" \":\"__Contributors__person+id_baa.mpg\",\"description\":\"SayMore Contributor File\",\"relationship\":\"source\"",
				_helper.GetSourceFilesForMetsData(fileLists).ElementAt(3));

			Assert.AreEqual("\" \":\"__Contributors__person+id_baa#mpg.meta\",\"description\":\"SayMore File Metadata (XML)\",\"relationship\":\"source\"",
				_helper.GetSourceFilesForMetsData(fileLists).ElementAt(4));
		}
	}
}
