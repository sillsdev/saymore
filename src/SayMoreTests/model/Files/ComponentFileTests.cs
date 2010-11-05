using System;
using System.Drawing;
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

		private string ParentFolderName
		{
			get { return Path.GetFileName(_parentFolder.Path); }
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GetFileType_IsText_GivesTextFileType()
		{
			var f = CreateComponentFile("abc.txt");
			Assert.AreEqual("Text", f.FileType.Name);
		}

		private ComponentFile CreateComponentFile(string fileName)
		{
			File.WriteAllText(_parentFolder.Combine(fileName), @"hello");

			var cf = new ComponentFile(_parentFolder.Combine(fileName),
				new[] {FileType.Create("Text", ".txt"), new UnknownFileType(null) },
				new ComponentRole[] { }, new FileSerializer(), null, null, null);

			cf.Save(); //creates the meta file path
			return cf;
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GetFileType_UnknownType_UnknownFileType()
		{
			var f = CreateComponentFile("abc.zzz");
			Assert.AreEqual("Unknown", f.FileType.Name);
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GetStringValue_FieldMissing_ReturnsSpecifiedDefault()
		{
			var f = CreateComponentFile("abc.zzz");
			Assert.AreEqual("hello", f.GetStringValue("notThere", "hello"));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GetStringValue_FieldIsThere_ReturnsCorrectValue()
		{
			var f = CreateComponentFile("abc.zzz");
			SetValue(f, "color", "red");
			Assert.AreEqual("red", f.GetStringValue("color", "blue"));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void RenameId_FieldIsThere_Succeeds()
		{
			var f = CreateComponentFile("abc.zzz");
			SetValue(f, "height", "25");
			SetValue(f, "width", "50");

			f.RenameId("width", "girth");
			Assert.IsNull(f.GetStringValue("width", null));
			Assert.AreEqual("50", f.GetStringValue("girth", null));
			Assert.AreEqual("25", f.GetStringValue("height", null));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void RenameId_FieldIsThere_OldIdReturnsNothing()
		{
			var f = CreateComponentFile("abc.zzz");
			SetValue(f, "height", "25");
			SetValue(f, "width", "50");

			f.RenameId("width", "girth");
			Assert.IsNull(f.GetStringValue("width", null));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void RenameId_FieldIsThere_NewIdReturnsOldValue()
		{
			var f = CreateComponentFile("abc.zzz");
			SetValue(f, "height", "25");
			SetValue(f, "width", "50");

			f.RenameId("width", "girth");
			Assert.AreEqual("50", f.GetStringValue("girth", null));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void RenameId_FieldMissing_ReturnsFalse()
		{
			var f = CreateComponentFile("abc.zzz");
			SetValue(f, "height", "25");

			f.RenameId("width", "girth");
			Assert.IsNull(f.GetStringValue("width", null));
			Assert.IsNull(f.GetStringValue("girth", null));
			Assert.AreEqual("25", f.GetStringValue("height", null));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void RenameId_FieldMissing_NewIdReturnsNothing()
		{
			var f = CreateComponentFile("abc.zzz");
			SetValue(f, "height", "25");
			f.RenameId("width", "girth");
			Assert.IsNull(f.GetStringValue("width", null));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void RemoveField_FieldIsThere_RemovesIt()
		{
			var f = CreateComponentFile("abc.zzz");
			SetValue(f, "height", "25");
			SetValue(f, "width", "50");
			f.RemoveField("width");
			Assert.AreEqual("25", f.GetStringValue("height", null));
			Assert.IsNull(f.GetStringValue("width", null));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void RemoveField_FieldIsThere_OldIdReturnsNothing()
		{
			var f = CreateComponentFile("abc.zzz");
			SetValue(f, "height", "25");
			SetValue(f, "width", "50");
			f.RemoveField("width");
			Assert.IsNull(f.GetStringValue("width", null));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void RemoveField_FieldMissing_DoesNothing()
		{
			var f = CreateComponentFile("abc.zzz");
			SetValue(f, "height", "25");
			f.RemoveField("width");
			Assert.AreEqual("25", f.GetStringValue("height", null));
			Assert.IsNull(f.GetStringValue("width", null));
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void SetValue_ChangingValue_NewValueOverwritesOld()
		{
			var f = CreateComponentFile("abc.zzz");
			SetValue(f, "color", "red");
			SetValue(f, "color", "green");
			Assert.AreEqual("green", f.GetStringValue("color", "blue"));
		}

		private ComponentFile CreateComponentFileWithRoleChoices(string path)
		{
			var componentRoles = new[]
			{
				new ComponentRole(typeof(Event), "translation", "translation",
					ComponentRole.MeasurementTypes.None, p => p.EndsWith("txt"),
					"$ElementId$_Original", Color.Magenta),

				new ComponentRole(typeof(Event), "transcriptionN", "Written Translation",
					ComponentRole.MeasurementTypes.Words, (p => Path.GetExtension(p).ToLower() == ".txt"),
					"$ElementId$_Translation-N", Color.Magenta),

				new ComponentRole(typeof(Event), "original", "Original Recording",
					ComponentRole.MeasurementTypes.Time, ComponentRole.GetIsAudioVideo,
					"$ElementId$_Original", Color.Magenta),

				new ComponentRole(typeof(Person), "consent", "Informed Consent",
					ComponentRole.MeasurementTypes.None, (p => p.Contains("_Consent.")),
					"$ElementId$_Consent", Color.Magenta)
			};

			return new ComponentFile(path,
				new[] { FileType.Create("Text", ".txt"), },
				componentRoles, new FileSerializer(), null, null, null);
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GetAssignedRoles_NoRoles_ReturnsEmptyEnumerator()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc.txt");
			Assert.AreEqual(0, f.GetAssignedRoles().Count());
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GetAssignedRoles_HasOneRoles_ReturnsThem()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc_Original.txt");
			Assert.AreEqual(1, f.GetAssignedRoles().Count());
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GetAssignedRoles_ForConsentAndEventType_ReturnsEmptyEnumerator()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc_Consent.txt");
			Assert.AreEqual(0, f.GetAssignedRoles(typeof(Event)).Count());
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void GetAssignedRoles_ForConsentAndPersonType_ReturnsThem()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc_Consent.txt");
			Assert.AreEqual(1, f.GetAssignedRoles(typeof(Person)).Count());
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void IdentifyAsRole_FileRenamed()
		{
			ComponentFile f = CreateComponentFile("abc.txt");
			var role = new ComponentRole(typeof (Event), "someRole", "someRole", ComponentRole.MeasurementTypes.None,
										 p => p.EndsWith("txt"), "$ElementId$_someRole", Color.Magenta);
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
