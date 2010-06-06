using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;

namespace SayMoreTests.Model.Files
{
	[TestFixture]
	public class ComponentFileTests
	{
		private TemporaryFolder _parentFolder;

		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("fileTypeTest");
		}

		private string ParentFolderName {
			get
			{
				return Path.GetFileName(_parentFolder.Path);
			}
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}


		[Test]
		public void GetFileType_IsText_GivesTextFileType()
		{
			ComponentFile f = CreateComponentFile("abc.txt");
			Assert.AreEqual("Text",f.FileType.Name);
		}

		private ComponentFile CreateComponentFile(string fileName)
		{
			File.WriteAllText(_parentFolder.Combine(fileName), @"hello");
			var cf= new ComponentFile(_parentFolder.Combine(fileName),
				new FileType[]{FileType.Create("Text", ".txt"), },
				new ComponentRole[]{},
				new FileSerializer(),
				null);

			cf.Save();//creates the meta file path
			return cf;
		}

		[Test]
		public void GetFileType_UnknownType_UnknownFileType()
		{
			ComponentFile f = CreateComponentFile("abc.zzz");
			Assert.AreEqual("Unknown", f.FileType.Name);
		}

		[Test]
		public void GetStringValue_FieldMissing_ReturnsSpecifiedDefault()
		{
			ComponentFile f = CreateComponentFile("abc.zzz");

			Assert.AreEqual("hello", f.GetStringValue("notThere", "hello"));
		}

		[Test]
		public void GetStringValue_FieldIsThere_ReturnsCorrectValue()
		{
			ComponentFile f = CreateComponentFile("abc.zzz");
			SetValue(f, "color", "red");
			Assert.AreEqual("red", f.GetStringValue("color", "blue"));
		}

		[Test]
		public void SetValue_ChangingValue_NewValueOverwritesOld()
		{
			ComponentFile f = CreateComponentFile("abc.zzz");
			SetValue(f, "color", "red");
			SetValue(f, "color", "green");
			Assert.AreEqual("green", f.GetStringValue("color", "blue"));
		}


		private ComponentFile CreateComponentFileWithRoleChoices(string path)
		{
			var componentRoles = new []
									{
										new ComponentRole(typeof(Session),"translation", "translation", ComponentRole.MeasurementTypes.None, p => p.EndsWith("txt"), "$ElementId$_Original"),
										new ComponentRole(typeof(Session),"transcriptionN", "Written Translation", ComponentRole.MeasurementTypes.Words, (p => Path.GetExtension(p).ToLower() == ".txt"), "$ElementId$_Translation-N"),
										new ComponentRole(typeof(Session),"original", "Original Recording", ComponentRole.MeasurementTypes.Time, ComponentRole.GetIsAudioVideo, "$ElementId$_Original")
									};

			return new ComponentFile(path,
				new FileType[] { FileType.Create("Text", ".txt"), },
				componentRoles,
				new FileSerializer(), null);
		}


		[Test]
		public void GetAssignedRoles_NoRoles_ReturnsEmptyEnumerator()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc.txt");
			Assert.AreEqual(0, f.GetAssignedRoles().Count());
		}

		[Test]
		public void GetAssignedRoles_HasTwoRoles_ReturnsThem()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc.txt");
			Assert.AreEqual(0, f.GetAssignedRoles().Count());
		}

		[Test]
		public void IdentifyAsRole_FileRenamed()
		{
			ComponentFile f = CreateComponentFile("abc.txt");
			var role = new ComponentRole(typeof (Session), "someRole", "someRole", ComponentRole.MeasurementTypes.None,
										 p => p.EndsWith("txt"), "$ElementId$_someRole");
			f.AssignRole(role);
			Assert.AreEqual(ParentFolderName + "_someRole.txt", Path.GetFileName(f.PathToAnnotatedFile));
			Assert.IsTrue(File.Exists(f.PathToAnnotatedFile));
		}

		public string SetValue(ComponentFile file, string key, string value)
		{
			string failureMessage;
			var suceeded = file.SetValue(key, value, out failureMessage);
			if (!string.IsNullOrEmpty(failureMessage))
			{
				throw new ApplicationException(failureMessage);
			}
			return suceeded;
		}
	}
}
