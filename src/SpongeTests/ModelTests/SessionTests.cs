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
// File: SessionTests.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Palaso.TestUtilities;
using SIL.Sponge;
using SIL.Sponge.Model;

namespace SpongeTests.ModelTests
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests for the Session class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SessionTests : TestBase
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
		/// Tests the Create method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Create()
		{
			var session = Session.Create(m_prj, "Ferrari");
			Assert.AreEqual("Ferrari", session.Id);
			Assert.AreEqual(m_prj, session.Project);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Load method, passing the full path to the session file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Load_FullPath()
		{
			Session.Create(m_prj, "Jaguar").Save();
			var file = Path.Combine(m_prj.SessionsFolder, "Jaguar");
			file = Path.Combine(file, "Jaguar.session");
			string msg;
			var session = Session.Load(m_prj, file, out msg);

			Assert.AreEqual("Jaguar", session.Id);
			Assert.AreEqual(m_prj, session.Project);
			Assert.AreEqual(file, session.FullFilePath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Load method, passing only the name of a session and letting the Load
		/// method infer from it, the full path to the session file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Load_NameOnly()
		{
			Session.Create(m_prj, "Maserati").Save();

			string msg;
			var session = Session.Load(m_prj, "Maserati", out msg);
			Assert.AreEqual("Maserati", session.Id);
			Assert.AreEqual(m_prj, session.Project);

			var file = Path.Combine(m_prj.SessionsFolder, "Maserati");
			file = Path.Combine(file, "Maserati.session");
			Assert.AreEqual(file, session.FullFilePath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that setting the Date property will also set the SerializedDate property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Date()
		{
			var session = Session.Create(m_prj, "Lotus");
			Assert.IsNull(session.SerializedDate);
			var dt = new DateTime(1963, 4, 19);
			session.Date = dt;
			Assert.IsNotNull(session.SerializedDate);
			dt = DateTime.Parse(session.SerializedDate);
			Assert.AreEqual("19-Apr-1963", dt.ToString("dd-MMM-yyyy"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the FileName property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FileName()
		{
			var session = Session.Create(m_prj, "Lamborghini");
			Assert.AreEqual("Lamborghini.session", session.FileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Folder property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Folder()
		{
			var session = Session.Create(m_prj, "Alfa Romeo");
			var path = Path.Combine(m_prj.SessionsFolder, "Alfa Romeo");
			Assert.AreEqual(path, session.Folder);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the FullFilePath property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FullFilePath()
		{
			var session = Session.Create(m_prj, "Aston Martin");
			var path = Path.Combine(m_prj.SessionsFolder, "Aston Martin");
			Assert.AreEqual(Path.Combine(path, "Aston Martin.session"), session.FullFilePath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Files property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Files()
		{
			var session = Session.Create(m_prj, "Lancia");
			session.Save();

			var file1 = Path.Combine(session.Folder, "Mustang.pdf");
			var file2 = Path.Combine(session.Folder, "Camero.jpg");
			File.CreateText(file1).Close();
			File.CreateText(file2).Close();

			var files = new List<string>(session.Files);
			Assert.AreEqual(2, files.Count);
			Assert.IsTrue(files.Contains(file1));
			Assert.IsTrue(files.Contains(file2));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddFiles method when the session's folder is missing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFiles_WhenSessionFolderMissing()
		{
			var session = Session.Create(m_prj, "wallace");
			Assert.IsFalse(session.AddFiles(new[] { string.Empty }));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the AddFiles method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFiles()
		{
			var session = Session.Create(m_prj, "gromit");
			session.Save();

			using(var file1 = new TempFile("wrong.junk"))
			using (var file2 = new TempFile("trousers.junk"))
			{
				Assert.IsTrue(session.AddFiles(new[] { file1.Path, file2.Path }));
				Assert.IsTrue(File.Exists(Path.Combine(session.Folder, Path.GetFileName(file1.Path))));
				Assert.IsTrue(File.Exists(Path.Combine(session.Folder, Path.GetFileName(file1.Path))));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Save method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Save()
		{
			var session = Session.Create(m_prj, "A Grand Day Out!");
			Assert.IsFalse(Directory.Exists(session.Folder));
			session.Save();
			Assert.IsTrue(Directory.Exists(session.Folder));
			Assert.IsTrue(File.Exists(session.FullFilePath));

			string msg;
			session = Session.Load(m_prj, "A Grand Day Out!", out msg);
			Assert.IsNotNull(session);
			Assert.IsNull(msg);
		}

		[Test]
		public void ChangeIdAndSave_HadSameIdBefore_ReturnsTrue()
		{
			var session = Session.Create(m_prj, "ETR001");
			session.Save();
			Assert.IsTrue(session.ChangeIdAndSave("ETR001"));
			Assert.AreEqual("ETR001", session.Id);
			Assert.AreEqual("ETR001", Path.GetFileName(session.Folder));
			Assert.AreEqual("ETR001", Path.GetFileNameWithoutExtension(session.FullFilePath));
		}

		[Test]
		public void ChangeIdAndSave_NewIdIsEmptyString_ReturnsFalse()
		{
			var session = Session.Create(m_prj, "ETR001");
			session.Save();
			Assert.IsFalse(session.ChangeIdAndSave(""));
			Assert.AreEqual("ETR001", session.Id);
		}

		[Test]
		public void ChangeIdAndSave_NewIdIsInvalidFolderName_ReturnsFalse()
		{
			var session = Session.Create(m_prj, "ETR001");
			session.Save();
			Assert.IsFalse(session.ChangeIdAndSave("chan\\ge"));
			Assert.AreEqual("ETR001", session.Id);
		}

		[Test]
		public void ChangeIdAndSave_ShouldChange_IdAndFolderAndFileChange()
		{
			var session = Session.Create(m_prj, "ETR001");
			session.Save();
			string ETR001Path = session.FullFilePath;
			Assert.IsTrue(session.ChangeIdAndSave("change"));
			Assert.AreEqual("change", session.Id);
			Assert.AreEqual("change", Path.GetFileName(session.Folder));
			Assert.AreEqual("change", Path.GetFileNameWithoutExtension(session.FullFilePath));
			Assert.IsFalse(Directory.Exists(ETR001Path));
			Assert.IsFalse(File.Exists(Path.Combine(session.Folder, "ETR001."+Sponge.SessionFileExtension)));
		}

		[Test]
		public void ChangeIdAndSave_HasFilesWithOldName_RenamesFiles()
		{
			var session = Session.Create(m_prj, "ETR001");
			session.Save();
			File.WriteAllText(Path.Combine(session.Folder,"ETR001_original.wav"),"");
			session.ChangeIdAndSave("change");
			Assert.IsTrue(File.Exists(Path.Combine(session.Folder, "change_original.wav")));
			Assert.IsFalse(File.Exists(Path.Combine(session.Folder, "ETR001_original.wav")));
		}
	}
}