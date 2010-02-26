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
using NUnit.Framework;

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
		/// Tests the CreateFromName method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateFromName()
		{
			var person = Person.CreateFromName(m_prj, "Dudley Doright");
			Assert.AreEqual("Dudley Doright", person.FullName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the FileName property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FileName()
		{
			var person = Person.CreateFromName(m_prj, "Mozart");
			Assert.AreEqual("Mozart.person", person.FileName);
			Assert.AreEqual(Path.Combine(m_prj.PeopleFolder, "Mozart.person"), person.FullPath);

			person = Person.CreateFromName(m_prj, "J:S/Bach");
			Assert.AreEqual("J_S_Bach.person", person.FileName);
			Assert.AreEqual(Path.Combine(m_prj.PeopleFolder, "J_S_Bach.person"), person.FullPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the FullPath property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void FullPath()
		{
			var person = Person.CreateFromName(m_prj, "Mozart");
			Assert.AreEqual(Path.Combine(m_prj.PeopleFolder, "Mozart.person"), person.FullPath);

			person = Person.CreateFromName(m_prj, "J:S/Bach");
			Assert.AreEqual(Path.Combine(m_prj.PeopleFolder, "J_S_Bach.person"), person.FullPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CanSave property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CanSave()
		{
			var person = Person.CreateFromName(m_prj, null);
			Assert.IsFalse(person.CanSave);
			person = Person.CreateFromName(m_prj, string.Empty);
			Assert.IsFalse(person.CanSave);
			person = Person.CreateFromName(m_prj, "Fogerty");
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
			var person = Person.CreateFromName(m_prj, "D.Fogerty");
			Assert.IsNull(person.PictureFile);

			foreach (var ext in new[] { "jpg", "gif", "tif", "png", "bmp", "dib" })
			{
				person = Person.CreateFromName(m_prj, "K_Loggins");
				var path = Path.ChangeExtension(person.FullPath, ext);
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
			var person = Person.CreateFromName(m_prj, "P.Collins");
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
			var person = Person.CreateFromName(m_prj, "Flock of Seagulls");
			person.CopyPictureFile(null);
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
				var person = Person.CreateFromName(m_prj, "Beethoven");
				string srcPicFile = null;

				try
				{
					srcPicFile = Path.Combine(m_prj.PeopleFolder, "~SpongPic~");
					srcPicFile = Path.ChangeExtension(srcPicFile, ext);
					File.CreateText(srcPicFile).Close();

					person.CopyPictureFile(srcPicFile);

					var expected = Path.Combine(m_prj.PeopleFolder, "Beethoven");
					expected = Path.ChangeExtension(expected, ext);
					Assert.IsTrue(File.Exists(expected));
					Assert.IsTrue(File.Exists(srcPicFile));
					File.Delete(Path.Combine(m_prj.PeopleFolder, "Beethoven.person"));
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
			var picFile = Path.Combine(m_prj.PeopleFolder, "Chumbawamba.jpg");
			File.CreateText(picFile).Close();
			var person = Person.CreateFromName(m_prj, "Chumbawamba");
			person.Save();

			Assert.IsTrue(File.Exists(picFile));
			Assert.IsTrue(File.Exists(person.FullPath));

			person.Rename("B:Joel");

			Assert.AreEqual("B_Joel.person", person.FileName);
			Assert.IsFalse(File.Exists(picFile));
			Assert.IsFalse(File.Exists(Path.Combine(m_prj.PeopleFolder, "Chumbawamba.person")));
			Assert.IsTrue(File.Exists(person.FullPath));
			Assert.IsTrue(File.Exists(Path.Combine(m_prj.PeopleFolder, "B_Joel.jpg")));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Delete method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Delete()
		{
			var person = Person.CreateFromName(m_prj, "U2");
			person.Save();
			var picFile = Path.Combine(m_prj.PeopleFolder, "U2.tif");
			File.CreateText(picFile).Close();

			var personFile = Path.Combine(m_prj.PeopleFolder, "U2.person");

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
			var person = Person.CreateFromName(m_prj, @"\Sweet");
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
			Assert.IsTrue(File.Exists(person.FullPath));

			person = Person.CreateFromFile(m_prj, person.FullPath);
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the CreateFromFile method when the path of the file name is invalid or
		/// incomplete.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CreateFromFile_WithIncompletePath()
		{
			var person = Person.CreateFromName(m_prj, @"Smash?Mouth");
			person.Education = "BS, Whitworth College";
			person.Save();
			Assert.IsTrue(File.Exists(person.FullPath));

			person = Person.CreateFromFile(m_prj, @"junk\preceding\filename\Smash_Mouth.person");
			Assert.IsNotNull(person);
			Assert.AreEqual("BS, Whitworth College", person.Education);
		}
	}
}
