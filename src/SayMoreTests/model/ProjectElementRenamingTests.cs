using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;

namespace SayMoreTests.Model
{
	/// <summary>
	/// Notice, we're using Events as the concrete class here, but all the functionality
	/// is on its superclass, ProjectElement, so we're effectively testing renaming People, too.
	/// </summary>
	[TestFixture]
	public class ProjectRenamingElementTests
	{
		private TemporaryFolder _parentFolder;

		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("ProjectRenamingElementTest");
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		private Session CreateSession()
		{
			return new Session(_parentFolder.Path, "xyz", null, new SessionFileType(() => null, () => null),
				(parentElement, path) => null, new XmlFileSerializer(null), (w, x, y, z) =>
			{
				return new ProjectElementComponentFile(w, x, y, z,
					FieldUpdater.CreateMinimalFieldUpdaterForTests(null));

			}, null, null, null);
		}

		private void SaveAndChangeIdShouldSucceed(Session session)
		{
			session.Save();
			string failureMessage;
			Assert.IsTrue(session.TryChangeIdAndSave("newId", out failureMessage));
			session.Save();
		}

		private string SaveAndChangeIdShouldFail(Session session, string newId)
		{
			session.Save();
			string failureMessage;
			Assert.IsFalse(session.TryChangeIdAndSave(newId, out failureMessage));
			return failureMessage;
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_FolderPathUpdated()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.AreEqual("newId", Path.GetFileNameWithoutExtension(newEvent.FolderPath));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_SettingsFilePathUpdated()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.AreEqual("newId", Path.GetFileNameWithoutExtension(newEvent.SettingsFilePath));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_FolderRenamed()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.IsTrue(Directory.Exists(_parentFolder.Combine("newId")));
			Assert.IsFalse(Directory.Exists(_parentFolder.Combine("xyz")));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_ElementMetaDataFileRenamed()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.IsTrue(File.Exists(newEvent.SettingsFilePath));
			Assert.AreEqual(1, Directory.GetFiles(newEvent.FolderPath).Count(), "was an old file left behind instead of renamed?");
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_HasFilesWithOldName_RenamesFiles()
		{
			var newEvent = CreateSession();
			File.CreateText(Path.Combine(newEvent.FolderPath, "xyz_source.wav")).Close();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.IsTrue(File.Exists(Path.Combine(newEvent.FolderPath, "newId_source.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(newEvent.FolderPath, "xyz_source.wav")));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_NewIdIsEmptyString_ReturnsFalse()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldFail(newEvent, "");
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_NewIdIsInvalidFolderName_ReturnsFalse()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldFail(newEvent, "chan*ge");
		}


		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_NewIdAlreadyInUse_ReturnsFalse()
		{
			var newEvent = CreateSession();
			Directory.CreateDirectory(_parentFolder.Combine("red"));
			SaveAndChangeIdShouldFail(newEvent, "red");
		}
	}
}
