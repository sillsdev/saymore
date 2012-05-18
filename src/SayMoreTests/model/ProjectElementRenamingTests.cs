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

		private Event CreateEvent()
		{
			return new Event(_parentFolder.Path, "xyz", null, new EventFileType(() => null, () => null),
				(parentElement, path) => null, new FileSerializer(null), (w, x, y, z) =>
			{
				return new ProjectElementComponentFile(w, x, y, z,
					FieldUpdater.CreateMinimalFieldUpdaterForTests(null));

			}, null,null);
		}

		private void SaveAndChangeIdShouldSucceed(Event evnt)
		{
			evnt.Save();
			string failureMessage;
			Assert.IsTrue(evnt.TryChangeIdAndSave("newId", out failureMessage));
			evnt.Save();
		}

		private string SaveAndChangeIdShouldFail(Event evnt, string newId)
		{
			evnt.Save();
			string failureMessage;
			Assert.IsFalse(evnt.TryChangeIdAndSave(newId, out failureMessage));
			return failureMessage;
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_FolderPathUpdated()
		{
			var newEvent = CreateEvent();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.AreEqual("newId", Path.GetFileNameWithoutExtension(newEvent.FolderPath));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_SettingsFilePathUpdated()
		{
			var newEvent = CreateEvent();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.AreEqual("newId", Path.GetFileNameWithoutExtension(newEvent.SettingsFilePath));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_FolderRenamed()
		{
			var newEvent = CreateEvent();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.IsTrue(Directory.Exists(_parentFolder.Combine("newId")));
			Assert.IsFalse(Directory.Exists(_parentFolder.Combine("xyz")));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_ElementMetaDataFileRenamed()
		{
			var newEvent = CreateEvent();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.IsTrue(File.Exists(newEvent.SettingsFilePath));
			Assert.AreEqual(1, Directory.GetFiles(newEvent.FolderPath).Count(), "was an old file left behind instead of renamed?");
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_HasFilesWithOldName_RenamesFiles()
		{
			var newEvent = CreateEvent();
			File.CreateText(Path.Combine(newEvent.FolderPath, "xyz_original.wav")).Close();
			SaveAndChangeIdShouldSucceed(newEvent);
			Assert.IsTrue(File.Exists(Path.Combine(newEvent.FolderPath, "newId_original.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(newEvent.FolderPath, "xyz_original.wav")));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_NewIdIsEmptyString_ReturnsFalse()
		{
			var newEvent = CreateEvent();
			SaveAndChangeIdShouldFail(newEvent, "");
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_NewIdIsInvalidFolderName_ReturnsFalse()
		{
			var newEvent = CreateEvent();
			SaveAndChangeIdShouldFail(newEvent, "chan*ge");
		}


		[Test]
		[Category("SkipOnTeamCity")]
		public void TryChangeIdAndSave_NewIdAlreadyInUse_ReturnsFalse()
		{
			var newEvent = CreateEvent();
			Directory.CreateDirectory(_parentFolder.Combine("red"));
			SaveAndChangeIdShouldFail(newEvent, "red");
		}
	}
}
