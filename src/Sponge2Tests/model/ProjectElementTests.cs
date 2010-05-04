using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using Sponge2.Model;
using Sponge2.Model.Files;

namespace Sponge2Tests.model
{
	[TestFixture]
	public class ProjectElementTests
	{
		private TemporaryFolder _parentFolder;

		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("ProjectElementTest");
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		[Test]
		public void Save_NewlyCreated_CreatesMetaDataFile()
		{
			Person person = CreatePerson();
			person.Save();
			Assert.IsTrue(File.Exists(_parentFolder.Combine("xyz", "xyz.person")));
			Assert.AreEqual(1, Directory.GetFiles(_parentFolder.Combine("xyz")).Length);
		}

		private Person CreatePerson()
		{
			return new Person(_parentFolder.Path, "xyz", MakeComponent, new FileSerializer());
		}


		public string SetValue(Person person, string key, string value)
		{
			string failureMessage;
			var suceeded = person.MetaDataFile.SetValue("color", "red", out failureMessage);
			if (!string.IsNullOrEmpty(failureMessage))
			{
				throw new ApplicationException(failureMessage);
			}
			return suceeded;
		}

		[Test]
		public void Load_AfterSave_PreservesId()
		{
			Person person = CreatePerson();
			SetValue(person, "color", "red");
			person.Save();
			Person person2 = CreatePerson();
			person2.Load();
			Assert.AreEqual("red", person2.MetaDataFile.GetStringValue("color", "blue"));
			Assert.AreEqual(1, Directory.GetFiles(_parentFolder.Combine("xyz")).Length);
		}

		[Test]
		public void GetComponentFiles_AfterCreation_GivesASingleFile()
		{
			var person = CreatePerson();
			IEnumerable<ComponentFile> componentFiles = person.GetComponentFiles();
			Assert.AreEqual(1, componentFiles.Count());
			Assert.AreEqual("xyz.person", componentFiles.First().FileName);

		}


		[Test]
		public void GetComponentFiles_SomeFiles_GivesThem()
		{
			var person = CreatePerson();
			File.WriteAllText(_parentFolder.Combine("xyz", "test.txt"), @"hello");
			Assert.AreEqual(2, person.GetComponentFiles().Count());
		}


		private ComponentFile MakeComponent(string pathtoannotatedfile)
		{
			return null;
		}
	}


}
