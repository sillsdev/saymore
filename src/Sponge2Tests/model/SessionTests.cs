using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using Sponge2.Model;


namespace SpongeTests.model
{
	[TestFixture]
	public class SessionTests
	{
		private TemporaryFolder _parentFolder;

		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("sessionTest");
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		private Session CreateSession(string id)
		{
			return Session.CreateAtLocation(_parentFolder.Path, id, ComponentFile.CreateMinimalComponentFileForTests);
		}

		[Test]
		public void GetComponentFiles_AfterCreation_GivesASingleFile()
		{
			var session = CreateSession("xyz");
			Assert.AreEqual(1, session.GetComponentFiles().Count());
		}


		[Test]
		public void GetComponentFiles_SomeFiles_GivesThem()
		{
			var session = CreateSession("xyz");
			File.WriteAllText(_parentFolder.Combine("xyz", "test.txt"), "hello");
			Assert.AreEqual(2, session.GetComponentFiles().Count());
		}
	}
}
