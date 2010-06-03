using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;

namespace SayMoreTests.Model
{
	/// <summary>
	/// Notice, we're using Sessions as the concrete class here, but all the functionality
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
			return new Session(_parentFolder.Path, "xyz", MakeComponent, new FileSerializer());
		}

		private ComponentFile MakeComponent(string pathtoannotatedfile)
		{
			return null;
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
		public void TryChangeIdAndSave_FolderPathUpdated()
		{
			var session = CreateSession();
			SaveAndChangeIdShouldSucceed(session);
			Assert.AreEqual("newId", Path.GetFileNameWithoutExtension(session.FolderPath));
		}

		[Test]
		public void TryChangeIdAndSave_SettingsFilePathUpdated()
		{
			var session = CreateSession();
			SaveAndChangeIdShouldSucceed(session);
			Assert.AreEqual("newId", Path.GetFileNameWithoutExtension(session.SettingsFilePath));
		}

		[Test]
		public void TryChangeIdAndSave_FolderRenamed()
		{
			var session = CreateSession();
			SaveAndChangeIdShouldSucceed(session);
			Assert.IsTrue(Directory.Exists(_parentFolder.Combine("newId")));
			Assert.IsFalse(Directory.Exists(_parentFolder.Combine("xyz")));
		}

		[Test]
		public void TryChangeIdAndSave_ElementMetaDataFileRenamed()
		{
			var session = CreateSession();
			SaveAndChangeIdShouldSucceed(session);
			Assert.IsTrue(File.Exists(session.SettingsFilePath));
			Assert.AreEqual(1, Directory.GetFiles(session.FolderPath).Count(), "was an old file left behind instead of renamed?");
		}

		[Test]
		public void TryChangeIdAndSave_HasFilesWithOldName_RenamesFiles()
		{
			var session = CreateSession();
			File.CreateText(Path.Combine(session.FolderPath, "xyz_original.wav")).Close();
			SaveAndChangeIdShouldSucceed(session);
			Assert.IsTrue(File.Exists(Path.Combine(session.FolderPath, "newId_original.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(session.FolderPath, "xyz_original.wav")));
		}

		[Test]
		public void TryChangeIdAndSave_NewIdIsEmptyString_ReturnsFalse()
		{
			var session = CreateSession();
			SaveAndChangeIdShouldFail(session, "");
		}

		[Test]
		public void TryChangeIdAndSave_NewIdIsInvalidFolderName_ReturnsFalse()
		{
			var session = CreateSession();
			SaveAndChangeIdShouldFail(session, "chan*ge");
		}


		[Test]
		public void TryChangeIdAndSave_NewIdAlreadyInUse_ReturnsFalse()
		{
			var session = CreateSession();
			Directory.CreateDirectory(_parentFolder.Combine("red"));
			SaveAndChangeIdShouldFail(session, "red");
		}
	}
}
