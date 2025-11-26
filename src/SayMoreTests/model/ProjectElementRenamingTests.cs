using System;
using System.IO;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.UI.ComponentEditors;

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
			return new Session(_parentFolder.Path, "xyz", null,
				new SessionFileType(new Lazy<Func<SessionBasicEditor.Factory>>(() => null),
					new Lazy<Func<StatusAndStagesEditor.Factory>>(() => null),
					new Lazy<Func<ContributorsEditor.Factory>>(() => null)),
				(parentElement, path) => null, new XmlFileSerializer(null), (w, x, y, z) =>
					new ProjectElementComponentFile(w, x, y, z,
					FieldUpdater.CreateMinimalFieldUpdaterForTests(null)), null, null, null);
		}

		private void SaveAndChangeIdShouldSucceed(Session session)
		{
			session.Save();
			Assert.IsTrue(session.TryChangeIdAndSave("newId", out _));
			session.Save();
		}

		private static string SaveAndChangeIdShouldFail(Session session, string newId)
		{
			session.Save();
			Assert.IsFalse(session.TryChangeIdAndSave(newId, out var failureMessage));
			return failureMessage;
		}

		[Test]
		[Category("SkipOnCI")]
		public void TryChangeIdAndSave_FolderPathUpdated()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.AreEqual("newId", Path.GetFileNameWithoutExtension(newEvent.FolderPath));
		}

		[Test]
		[Category("SkipOnCI")]
		public void TryChangeIdAndSave_SettingsFilePathUpdated()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.AreEqual("newId", Path.GetFileNameWithoutExtension(newEvent.SettingsFilePath));
		}

		[Test]
		[Category("SkipOnCI")]
		public void TryChangeIdAndSave_FolderRenamed()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.IsTrue(Directory.Exists(_parentFolder.Combine("newId")));
			Assert.IsFalse(Directory.Exists(_parentFolder.Combine("xyz")));
		}

		[Test]
		[Category("SkipOnCI")]
		public void TryChangeIdAndSave_ElementMetaDataFileRenamed()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.IsTrue(File.Exists(newEvent.SettingsFilePath));
			Assert.That(Directory.GetFiles(newEvent.FolderPath).Length, Is.EqualTo(1),
				"was an old file left behind instead of renamed?");
		}

		[Test]
		[Category("SkipOnCI")]
		public void TryChangeIdAndSave_HasFilesWithOldName_RenamesFiles()
		{
			var newEvent = CreateSession();
			File.CreateText(Path.Combine(newEvent.FolderPath, "xyz_source.wav")).Close();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.IsTrue(File.Exists(Path.Combine(newEvent.FolderPath, "newId_source.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(newEvent.FolderPath, "xyz_source.wav")));
		}

		[Test]
		[Category("SkipOnCI")]
		public void TryChangeIdAndSave_NewIdIsEmptyString_ReturnsFalse()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldFail(newEvent, "");
		}

		[Test]
		[Category("SkipOnCI")]
		public void TryChangeIdAndSave_NewIdIsInvalidFolderName_ReturnsFalse()
		{
			var newEvent = CreateSession();
			SaveAndChangeIdShouldFail(newEvent, "chan*ge");
		}


		[Test]
		[Category("SkipOnCI")]
		public void TryChangeIdAndSave_NewIdAlreadyInUse_ReturnsFalse()
		{
			var newEvent = CreateSession();
			Directory.CreateDirectory(_parentFolder.Combine("red"));
			SaveAndChangeIdShouldFail(newEvent, "red");
		}
	}
}
