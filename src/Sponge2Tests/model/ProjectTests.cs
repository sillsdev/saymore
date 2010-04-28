using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Palaso.TestUtilities;
using Sponge2.Model;


namespace SpongeTests.model
{
	[TestFixture]
	public class ProjectTests
	{
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

		[Test, ExpectedException(typeof(ArgumentException))]
		public void CreateAtLocation_FolderAlreadyExists_Throws()
		{
			using (var parent = new Palaso.TestUtilities.TemporaryFolder("parent"))
			using (new Palaso.TestUtilities.TemporaryFolder(parent, "child"))
			{
				Project.CreateAtLocation(parent.Path, "child", TODO);
			}
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void CreateAtLocation_ParentFolderDoesNotExist_Throws()
		{
			using (var parent = new Palaso.TestUtilities.TemporaryFolder("parent"))
			{
				Project.CreateAtLocation(parent.Path+"YY", "child", TODO);
			}
		}

		[Test]
		public void CreateAtLocation_EverythingOk_ReturnsProject()
		{
			using (var parent = new Palaso.TestUtilities.TemporaryFolder("parent"))
			{
				Assert.IsNotNull(Project.CreateAtLocation(parent.Path, "foo", TODO));
			}
		}

		[Test]
		public void CreateAtLocation_EverythingOk_CreatesFolderAndSettingsFile()
		{
			using (var parent = new Palaso.TestUtilities.TemporaryFolder("parent"))
			{
				Project.CreateAtLocation(parent.Path, "foo", TODO);
				Assert.IsTrue(File.Exists(parent.Combine("foo", "foo."+Project.ProjectSettingsFileExtension)));
			}
		}
		 */
	}

}
