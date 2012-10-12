using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.Reporting;
using Palaso.TestUtilities;
using Moq;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.UI.ElementListScreen;
using SayMore.UI.NewSessionsFromFiles;

namespace SayMoreTests.UI.NewEventsFromFiles
{
	[TestFixture]
	public class NewEventsFromFileDlgViewModelTests
	{
		private TemporaryFolder _tmpFolder;
		private string _srcFolder;
		private string _sessionsFolder;
		private ElementListViewModel<Session> _sessionPresentationModel;
		private NewSessionsFromFileDlgViewModel _model;

		// TODO: Need tests for CreateSingleSession() and some other stuff.

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_tmpFolder = new TemporaryFolder("NewSessionsFromFilesTests");

			_srcFolder = _tmpFolder.Combine("source");
			_sessionsFolder = _tmpFolder.Combine("Sessions");

			Directory.CreateDirectory(_srcFolder);

			var repo = new Mock<ElementRepository<Session>>();
			repo.Setup(e => e.PathToFolder).Returns(_sessionsFolder);

			_sessionPresentationModel = new ElementListViewModel<Session>(repo.Object);
			_model = new NewSessionsFromFileDlgViewModel(_sessionPresentationModel, CreateNewComponentFile);
		}

		/// ------------------------------------------------------------------------------------
		public static NewComponentFile CreateNewComponentFile(string path)
		{
			return new NewComponentFile(path,
				new[] { FileType.Create("Text", ".txt"), new UnknownFileType(null, null) },
				new ComponentRole[] { }, new FileSerializer(null), null, null, null);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_tmpFolder.Dispose();
			_tmpFolder = null;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllSourceAndDestinationPairs_HaveFiles_ReturnsThem()
		{
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\coke.wav"));
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\pepsi.wmv"));
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\drpepper.mpg"));

			var pairs = _model.GetAllSourceAndDestinationPairs().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			Assert.AreEqual(Path.Combine(_sessionsFolder, @"coke\coke_Source.wav"), pairs[@"c:\newSessionSrc\coke.wav"]);
			Assert.AreEqual(Path.Combine(_sessionsFolder, @"pepsi\pepsi_Source.wmv"), pairs[@"c:\newSessionSrc\pepsi.wmv"]);
			Assert.AreEqual(Path.Combine(_sessionsFolder, @"drpepper\drpepper_Source.mpg"), pairs[@"c:\newSessionSrc\drpepper.mpg"]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllSourceAndDestinationPairs_NoFiles_ReturnsEmptyList()
		{
			Assert.AreEqual(0, _model.GetAllSourceAndDestinationPairs().Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullFilePath_IndexNegative_ReturnsEmptyString()
		{
			Assert.AreEqual(string.Empty, _model.GetFullFilePath(-1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullFilePath_IndexTooBig_ReturnsEmptyString()
		{
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\coke.wav"));
			Assert.AreEqual(string.Empty, _model.GetFullFilePath(1));
		}


		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetFullFilePath_IndexInRange_ReturnsCorrectPath()
		{
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\pepsi.wmv"));
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\drpepper.mpg"));
			Assert.AreEqual(@"c:\newSessionSrc\pepsi.wmv", _model.GetFullFilePath(0));
			Assert.AreEqual(@"c:\newSessionSrc\drpepper.mpg", _model.GetFullFilePath(1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ToggleFilesSelectedState_IndexNegative_DoesNotCrash()
		{
			_model.ToggleFilesSelectedState(-1);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ToggleFilesSelectedState_IndexTooBig_DoesNotCrash()
		{
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\drpepper.mpg"));
			_model.ToggleFilesSelectedState(1);
		}


		/// ------------------------------------------------------------------------------------
		[Test]
		public void ToggleFilesSelectedState_IndexInRange_TogglesState()
		{
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\drpepper.mpg"));
			_model.Files[0].Selected = false;
			_model.ToggleFilesSelectedState(0);
			Assert.IsTrue(_model.Files[0].Selected);
			_model.ToggleFilesSelectedState(0);
			Assert.IsFalse(_model.Files[0].Selected);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SelectAllFiles_Select_SelectsAll()
		{
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\pepsi.wmv"));
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\drpepper.mpg"));
			_model.Files[0].Selected = false;
			_model.Files[1].Selected = false;

			_model.SelectAllFiles(true);
			Assert.IsTrue(_model.Files[0].Selected);
			Assert.IsTrue(_model.Files[1].Selected);

			_model.Files[1].Selected = false;

			_model.SelectAllFiles(false);
			Assert.IsFalse(_model.Files[0].Selected);
			Assert.IsFalse(_model.Files[1].Selected);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetUniqueSourceAndDestinationPairs_NoDestExists_ReturnsAllPairs()
		{
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\coke.wav"));
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\pepsi.wmv"));
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\drpepper.mpg"));

			var pairs = _model.GetUniqueSourceAndDestinationPairs().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			Assert.AreEqual(Path.Combine(_sessionsFolder, @"coke\coke_Source.wav"), pairs[@"c:\newSessionSrc\coke.wav"]);
			Assert.AreEqual(Path.Combine(_sessionsFolder, @"pepsi\pepsi_Source.wmv"), pairs[@"c:\newSessionSrc\pepsi.wmv"]);
			Assert.AreEqual(Path.Combine(_sessionsFolder, @"drpepper\drpepper_Source.mpg"), pairs[@"c:\newSessionSrc\drpepper.mpg"]);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetUniqueSourceAndDestinationPairs_SomeDestExists_ReturnsOnlyPairsForNonExistant()
		{
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\coke.wav"));
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\pepsi.wmv"));
			_model.Files.Add(CreateNewComponentFile(@"c:\newSessionSrc\drpepper.mpg"));

			Directory.CreateDirectory(Path.Combine(_sessionsFolder, "coke"));
			Directory.CreateDirectory(Path.Combine(_sessionsFolder, "drpepper"));
			File.CreateText(Path.Combine(_sessionsFolder, @"coke\coke.wav")).Close();
			File.CreateText(Path.Combine(_sessionsFolder, @"drpepper\drpepper_Source.mpg")).Close();

			ErrorReport.IsOkToInteractWithUser = false;

			using (new ErrorReport.NonFatalErrorReportExpected())
			{
				var pairs = _model.GetUniqueSourceAndDestinationPairs().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
				Assert.AreEqual(2, pairs.Count);
				Assert.AreEqual(Path.Combine(_sessionsFolder, @"coke\coke_Source.wav"), pairs[@"c:\newSessionSrc\coke.wav"]);
				Assert.AreEqual(Path.Combine(_sessionsFolder, @"pepsi\pepsi_Source.wmv"), pairs[@"c:\newSessionSrc\pepsi.wmv"]);
			}
		}
	}
}
