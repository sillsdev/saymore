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
// File: PersonTests.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SilUtils;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests for the Person class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PersonTests : TestBase
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
		/// Tests the UniqueName property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void UniqueName()
		{
			var name = Person.UniqueName;
			Assert.AreEqual("Unknown Name 01", name);

			var person = Person.CreateFromName(name);
			person.Save();
			Assert.AreEqual("Unknown Name 02", Person.UniqueName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CreateFromName method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateFromName()
		{
			var person = Person.CreateFromName("Dudley Doright");
			Assert.AreEqual("Dudley Doright", person.FullName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the InitializePeopleFolder method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void InitializePeopleFolder()
		{
			var fakePrjPath = Path.Combine(Path.GetTempPath(), "~fakeSpongePath~");

			try
			{
				Person.InitializePeopleFolder(fakePrjPath);
				var expected = Path.Combine(fakePrjPath, Sponge.PeopleFolderName);
				Assert.AreEqual(expected, Person.PeoplesPath);
				Assert.IsTrue(Person.PeoplesPath.StartsWith(fakePrjPath));
				Assert.IsTrue(Directory.Exists(expected));
			}
			finally
			{
				try
				{
					Directory.Delete(fakePrjPath, true);
				}
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PeopleFiles property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PeopleFiles()
		{
			var person1 = Person.CreateFromName("Rocky");
			person1.Save();
			var person2 = Person.CreateFromName("Bullwinkle");
			person2.Save();
			var person3 = Person.CreateFromName("George Jetson");
			person3.Save();

			var files = Person.PeopleFiles.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
			Assert.AreEqual(3, files.Count);
			Assert.IsTrue(files.Contains("Rocky"));
			Assert.IsTrue(files.Contains("Bullwinkle"));
			Assert.IsTrue(files.Contains("George Jetson"));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the FileName property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FileName()
		{
			var person = Person.CreateFromName("Mozart");
			Assert.AreEqual("Mozart.person", person.FileName);

			person = Person.CreateFromName("J:S/Bach");
			Assert.AreEqual("J_S_Bach.person", person.FileName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CanSave property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanSave()
		{
			var person = Person.CreateFromName(null);
			Assert.IsFalse(person.CanSave);
			person = Person.CreateFromName(string.Empty);
			Assert.IsFalse(person.CanSave);
			person = Person.CreateFromName("Fogerty");
			Assert.IsTrue(person.CanSave);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the PictureFile property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PictureFile()
		{
			var person = Person.CreateFromName("D.Fogerty");
			Assert.IsNull(person.PictureFile);

			foreach (var ext in new[] { "jpg", "gif", "tif", "png", "bmp", "dib" })
			{
				person = Person.CreateFromName("K_Loggins");
				var path = Path.ChangeExtension(Path.Combine(Person.PeoplesPath, "K_Loggins"), ext);
				File.CreateText(path).Close();
				Assert.AreEqual(path, person.PictureFile);
				File.Delete(path);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CopyPictureFile method when the picture file does not exist.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void CopyPictureFile_BadFile()
		{
			var person = Person.CreateFromName("P.Collins");
			person.CopyPictureFile("invalid.jpg");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CopyPictureFile method when the picture file is null.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(NullReferenceException))]
		public void CopyPictureFile_NullFile()
		{
			var person = Person.CreateFromName("Flock of Seagulls");
			person.CopyPictureFile(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CopyPictureFile method when the PeoplesPath does not exist.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		[ExpectedException(typeof(DirectoryNotFoundException))]
		public void CopyPictureFile_BadPeoplesPath()
		{
			var person = Person.CreateFromName("Beethoven");
			ReflectionHelper.SetProperty(typeof(Person), "PeoplesPath", null);
			string tmpPicFile = null;

			try
			{
				tmpPicFile = Path.GetTempFileName();
				File.Move(tmpPicFile, Path.ChangeExtension(tmpPicFile, "jpg"));
				tmpPicFile = Path.ChangeExtension(tmpPicFile, "jpg");

				person.CopyPictureFile(tmpPicFile);
			}
			finally
			{
				File.Delete(tmpPicFile);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CopyPictureFile method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CopyPictureFile()
		{
			foreach (var ext in new[] { "jpg", "gif", "tif", "png", "bmp", "dib" })
			{
				var person = Person.CreateFromName("Beethoven");
				string srcPicFile = null;

				try
				{
					srcPicFile = Path.Combine(Person.PeoplesPath, "~SpongPic~");
					srcPicFile = Path.ChangeExtension(srcPicFile, ext);
					File.CreateText(srcPicFile).Close();

					person.CopyPictureFile(srcPicFile);

					var expected = Path.Combine(Person.PeoplesPath, "Beethoven");
					expected = Path.ChangeExtension(expected, ext);
					Assert.IsTrue(File.Exists(expected));
					Assert.IsTrue(File.Exists(srcPicFile));
					File.Delete(Path.Combine(Person.PeoplesPath, "Beethoven.person"));
				}
				finally
				{
					File.Delete(srcPicFile);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Rename method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Rename()
		{
			var picFile = Path.Combine(Person.PeoplesPath, "Chumbawamba.jpg");
			File.CreateText(picFile).Close();
			var person = Person.CreateFromName("Chumbawamba");
			person.Save();

			Assert.IsTrue(File.Exists(picFile));
			Assert.IsTrue(File.Exists(Path.Combine(Person.PeoplesPath, "Chumbawamba.person")));

			person.Rename("B:Joel");

			Assert.IsFalse(File.Exists(picFile));
			Assert.IsFalse(File.Exists(Path.Combine(Person.PeoplesPath, "Chumbawamba.person")));
			Assert.IsTrue(File.Exists(Path.Combine(Person.PeoplesPath, "B_Joel.person")));
			Assert.IsTrue(File.Exists(Path.Combine(Person.PeoplesPath, "B_Joel.jpg")));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Delete method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Delete()
		{
			var person = Person.CreateFromName("U2");
			person.Save();
			var picFile = Path.Combine(Person.PeoplesPath, "U2.tif");
			File.CreateText(picFile).Close();

			var personFile = Path.Combine(Person.PeoplesPath, "U2.person");

			Assert.IsTrue(File.Exists(personFile));
			Assert.IsTrue(File.Exists(picFile));

			person.Delete(false);
			Assert.IsFalse(File.Exists(personFile));
			Assert.IsTrue(File.Exists(picFile));

			person.Save();
			Assert.IsTrue(File.Exists(personFile));
			Assert.IsTrue(File.Exists(picFile));

			person.Delete(true);
			Assert.IsFalse(File.Exists(personFile));
			Assert.IsFalse(File.Exists(picFile));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CreateFromFile method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateFromFile()
		{
			var person = Person.CreateFromName(@"\Sweet");
			person.PrimaryLanguage = "English";
			person.OtherLangauge0 = "Farsi";
			person.OtherLangauge1 = "Lithuanian";
			person.Notes = "It's a beautiful day";
			person.LearnedLanguageIn = "Cradle";
			person.BirthYear = 1812;
			person.ContactInfo = "Around the corner";
			person.Education = "PhD";
			person.FathersLanguage = "Farsi";
			person.MothersLanguage = "Lithuanian";
			person.Gender = Gender.Female;
			person.PrimaryOccupation = "Bus Driver";

			person.Save();
			var path = Path.Combine(Person.PeoplesPath, person.FileName);
			Assert.IsTrue(File.Exists(path));

			person = Person.CreateFromFile(path);
			Assert.AreEqual(@"\Sweet", person.FullName);
			Assert.AreEqual("English", person.PrimaryLanguage);
			Assert.AreEqual("Farsi", person.OtherLangauge0);
			Assert.AreEqual("Lithuanian", person.OtherLangauge1);
			Assert.AreEqual("It's a beautiful day", person.Notes);
			Assert.AreEqual("Cradle", person.LearnedLanguageIn);
			Assert.AreEqual(1812, person.BirthYear);
			Assert.AreEqual("Around the corner", person.ContactInfo);
			Assert.AreEqual("PhD", person.Education);
			Assert.AreEqual("Farsi", person.FathersLanguage);
			Assert.AreEqual("Lithuanian", person.MothersLanguage);
			Assert.AreEqual(Gender.Female, person.Gender);
			Assert.AreEqual("Bus Driver", person.PrimaryOccupation);
			Assert.IsNull(person.OtherLangauge2);
			Assert.IsNull(person.OtherLangauge3);
		}
	}
}
