// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: SpongeProjectTests.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.IO;
using System.Xml;
using NUnit.Framework;
using SilUtils;
using System.Linq;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SpongeProjectTests : TestBase
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs before each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void TestSetup()
		{
			base.TestSetup();
			InitProject();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the private Create method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Create()
		{
			VerifyProject(_prj, kTestPrjName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Load method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Load()
		{
			VerifyProject(_prj, kTestPrjName);

			_prj = SpongeProject.Load(_prj.FullFilePath);
			VerifyProject(_prj, kTestPrjName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Initializes method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Initialize()
		{
			VerifyProject(_prj, kTestPrjName);
			Assert.AreEqual(0, _prj.Sessions.Count);

			var path1 = Path.Combine(_prj.SessionsFolder, "bacon");
			var path2 = Path.Combine(_prj.SessionsFolder, "eggs");

			Directory.CreateDirectory(path1);
			Directory.CreateDirectory(path2);

			var writer = XmlWriter.Create(Path.Combine(path1, "bacon.session"));
			writer.WriteStartElement("session");
			writer.WriteEndElement();
			writer.Close();

			writer = XmlWriter.Create(Path.Combine(path2, "eggs.session"));
			writer.WriteStartElement("session");
			writer.WriteEndElement();
			writer.Close();

			ReflectionHelper.CallMethod(_prj, "Initialize", new[] { kTestPrjName, null });

			Assert.AreEqual(2, _prj.Sessions.Count);
			Assert.AreEqual("bacon", _prj.Sessions[0].Id);
			Assert.AreEqual("eggs", _prj.Sessions[1].Id);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the existence of the specified project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void VerifyProject(SpongeProject prj, string expectedPrjName)
		{
			Assert.IsNotNull(prj);
			var expectedPath = Path.Combine(SpongeProject.ProjectsFolder, expectedPrjName);
			Assert.AreEqual(expectedPath, prj.Folder);
			Assert.IsTrue(Directory.Exists(prj.Folder));

			expectedPath = Path.Combine(prj.Folder, "People");
			Assert.AreEqual(expectedPath, prj.PeopleFolder);
			Assert.IsTrue(Directory.Exists(prj.PeopleFolder));

			expectedPath = Path.Combine(prj.Folder, "Sessions");
			Assert.AreEqual(expectedPath, prj.SessionsFolder);
			Assert.IsTrue(Directory.Exists(prj.SessionsFolder));

			expectedPath = Path.Combine(prj.Folder, kTestPrjFileName);
			Assert.AreEqual(expectedPath, prj.FullFilePath);
			Assert.IsTrue(File.Exists(prj.FullFilePath));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the GetUniquePersonName method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetUniquePersonName()
		{
			var name = _prj.GetUniquePersonName();
			Assert.AreEqual("Unknown Name 01", name);

			var person = Person.CreateFromName(_prj, name);
			person.Save();
			Assert.AreEqual("Unknown Name 02", _prj.GetUniquePersonName());
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddSession method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddSession()
		{
			Assert.AreEqual(0, _prj.SessionNames.Length);

			var session = _prj.AddSession(kTestSessionName);
			Assert.IsNotNull(session);
			Assert.AreEqual(1, _prj.Sessions.Count);
			Assert.AreEqual(kTestSessionName, _prj.Sessions[0].Id);

			// Now add a session that already exists.
			Assert.AreEqual(session, _prj.AddSession(kTestSessionName));
			Assert.AreEqual(1, _prj.Sessions.Count);
			Assert.AreEqual(kTestSessionName, _prj.Sessions[0].Id);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Sessions property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Sessions()
		{
			Assert.AreEqual(0, _prj.SessionNames.Length);

			Assert.IsNotNull(_prj.AddSession("Magenta"));
			Assert.IsNotNull(_prj.AddSession("Blue"));
			Assert.IsNotNull(_prj.AddSession("Yellow"));

			// Make sure the list is sorted.
			Assert.AreEqual(3, _prj.SessionNames.Length);
			Assert.AreEqual("Blue", _prj.Sessions[0].Id);
			Assert.AreEqual("Magenta", _prj.Sessions[1].Id);
			Assert.AreEqual("Yellow", _prj.Sessions[2].Id);

			// Make sure a file in the sessions folder doesn't get recognized as a folder.
			File.CreateText(Path.Combine(_prj.SessionsFolder, "junk")).Close();
			Assert.AreEqual(3, _prj.SessionNames.Length);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the RefreshSessionList method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void RefreshSessionList()
		{
			Assert.AreEqual(0, _prj.SessionNames.Length);
			Assert.IsNotNull(_prj.AddSession("lemon"));
			Assert.IsNotNull(_prj.AddSession("lime"));
			Assert.IsNotNull(_prj.AddSession("orange"));
			Assert.AreEqual(3, _prj.SessionNames.Length);

			Directory.CreateDirectory(Path.Combine(_prj.SessionsFolder, "grapefruit"));
			Directory.CreateDirectory(Path.Combine(_prj.SessionsFolder, "guava"));

			_prj.RefreshSessionList();
			Assert.AreEqual(5, _prj.Sessions.Count);
			Assert.AreEqual("grapefruit", _prj.Sessions[0].Id);
			Assert.AreEqual("guava", _prj.Sessions[1].Id);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests the GetSessionFolder method
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetSessionFolder()
		//{
		//    Assert.IsNull(_prj.GetSessionFolder(null));
		//    Assert.IsNull(_prj.GetSessionFolder(string.Empty));
		//    Assert.IsNull(_prj.GetSessionFolder(kTestSessionName));

		//    _prj.AddSession(kTestSessionName);
		//    var path = Path.Combine(_prj.SessionsPath, kTestSessionName);
		//    Assert.AreEqual(path, _prj.GetSessionFolder(kTestSessionName));
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests the GetSessionFiles method
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetSessionFiles()
		//{
		//    Assert.IsNull(_prj.GetSessionFilesNames(null));
		//    Assert.IsNull(_prj.GetSessionFilesNames(string.Empty));

		//    _prj.AddSession(kTestSessionName);
		//    Assert.AreEqual(0, _prj.GetSessionFilesNames(kTestSessionName).Length);

		//    // Create a few files in the session folder. Must use CreateText so we can
		//    // close the files because Create leaves them locked too long.
		//    var path = _prj.GetSessionFolder(kTestSessionName);
		//    File.CreateText(Path.Combine(path, "yak.pdf")).Close();
		//    File.CreateText(Path.Combine(path, "hippo.wav")).Close();
		//    File.CreateText(Path.Combine(path, "meerkat.wma")).Close();

		//    // Get session's files and make sure they're returned sorted.
		//    var files = _prj.GetSessionFilesNames(kTestSessionName);
		//    Assert.AreEqual(3, files.Length);
		//    Assert.AreEqual(files[0], Path.Combine(path, "hippo.wav"));
		//    Assert.AreEqual(files[1], Path.Combine(path, "meerkat.wma"));
		//    Assert.AreEqual(files[2], Path.Combine(path, "yak.pdf"));
		//}

		[Test]
		public void GetPeopleName_NoPeople_Empty()
		{
			Assert.AreEqual(0, _prj.GetPeopleNames().Count());
		}

		[Test]
		public void GetPeopleName_TwoPeople_GivesTwoNames()
		{
			var a = new Person();
			_prj.AddPerson(a);
			a.ChangeName("A");
			a.Save();

			var b = new Person();
			_prj.AddPerson(b);
			b.ChangeName("B");
			b.Save();

			Assert.AreEqual(2, _prj.GetPeopleNames().Count());
			Assert.IsTrue(_prj.GetPeopleNames().Any(p=>p=="A"));
			Assert.IsTrue(_prj.GetPeopleNames().Any(p => p == "B"));
		}
	}

}
