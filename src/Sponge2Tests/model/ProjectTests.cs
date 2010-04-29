using System;
using System.IO;
using NUnit.Framework;
using Palaso.TestUtilities;
using Sponge2.Model;

namespace Sponge2Tests.model
{
	[TestFixture]
	public class ProjectTests
	{
		private TemporaryFolder _parentFolder;

		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("projectTest");
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}


		[Test]
		public void Load_AfterSave_IsoPreserved()
		{
			using (var parent = new Palaso.TestUtilities.TemporaryFolder("parent"))
			{
				string settingsPath = parent.Combine("foo." + Project.ProjectSettingsFileExtension);
				var project = new Project(settingsPath);
				project.Iso639Code = "abc";
				project.Save();

				var project2 = new Project(settingsPath);
				Assert.AreEqual("abc",project2.Iso639Code);
			}
		}

		/*
		[Test, ExpectedException(typeof(FileNotFoundException))]
		public void FromSettingsFilePath_FileNotFound_Throws()
		{
			Project.FromSettingsFilePath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Templates),
													  "foo." + Project.ProjectSettingsFileExtension));
		}

		[Test]
		public void FromSettingsFilePath_FileIsFound_GivesProject()
		{
			using (var parent = new Palaso.TestUtilities.TemporaryFolder("parent"))
			{
				var project = Project.CreateAtLocation(parent.Path, "foo", TODO);
				Assert.IsNotNull(Project.FromSettingsFilePath(project.SettingsFilePath));

			}
		}

		[Test]
		public void FromSettingsFilePath_SettingsPathNameDiffersFromDirectory_OK()
		{
			using (var parent = new Palaso.TestUtilities.TemporaryFolder("parent"))
			{
				var originalProject = Project.CreateAtLocation(parent.Path, "foo", TODO);

				//rename the settings, but not the directory, and reload
				string changedSettingsPath = originalProject.SettingsFilePath.Replace("foo." + Project.ProjectSettingsFileExtension, "baa." + Project.ProjectSettingsFileExtension);
				File.Move(originalProject.SettingsFilePath, changedSettingsPath);
				Project loadedProject = Project.FromSettingsFilePath(changedSettingsPath);
				Assert.IsNotNull(loadedProject);
				Assert.AreEqual(originalProject.FolderPath, loadedProject.FolderPath);
				Assert.AreEqual(changedSettingsPath, loadedProject.SettingsFilePath);
			}
		}

		[Test]
		public void Persistance_WritesAndReadsIso()
		{
			using (var parent = new Palaso.TestUtilities.TemporaryFolder("parent"))
			{
				var project = Project.CreateAtLocation(parent.Path, "foo", TODO);
				project.Iso639Code = "abc";
				project.Save();
				var sameProject = Project.FromSettingsFilePath(project.SettingsFilePath);
				Assert.AreEqual("abc", sameProject.Iso639Code);
			}
		}
		*/


		[Test, ExpectedException(typeof(ArgumentException))]
		public void Constructor_ParentFolderDoesNotExist_Throws()
		{
			Assert.IsTrue(File.Exists(_parentFolder.Combine("NotThere", "foo", "foo." + Project.ProjectSettingsFileExtension)));
		}

		[Test]
		public void Constructor_EverythingOk_CreatesFolderAndSettingsFile()
		{
				CreateProject(_parentFolder);
				Assert.IsTrue(File.Exists(_parentFolder.Combine("foo", "foo."+Project.ProjectSettingsFileExtension)));
		}

		private void CreateProject(TemporaryFolder parent)
		{
			new Project(parent.Combine("foo","foo." + Project.ProjectSettingsFileExtension));
		}
	}

}
