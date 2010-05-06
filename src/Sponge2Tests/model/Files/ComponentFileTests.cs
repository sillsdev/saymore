using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using Sponge2.Model.Files;

namespace Sponge2Tests.model.Files
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

		private ComponentFile CreateComponentFile(string path)
		{
			return new ComponentFile(path,
				new FileType[]{FileType.Create("Text", ".txt"), },
				new ComponentRole[]{},
				new FileSerializer());
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

		private IEnumerable<ComponentRole> MakeComponentRoles()
		{
			yield return new ComponentRole("original", "Original Recording", ComponentRole.MeasurementTypes.Time, ComponentRole.GetIsAudioVideo, "$SessionId$_Original");
			yield return new ComponentRole("carefulSpeech", "Careful Speech", ComponentRole.MeasurementTypes.Time, ComponentRole.GetIsAudioVideo, "$SessionId$_Careful");
			yield return new ComponentRole("oralTranslation", "Oral Translation", ComponentRole.MeasurementTypes.Time, ComponentRole.GetIsAudioVideo, "$SessionId$_OralTranslation");
			yield return new ComponentRole("transcription", "Transcription", ComponentRole.MeasurementTypes.Words, (p => Path.GetExtension(p).ToLower() == ".txt"), "$SessionId$_Transcription");
			yield return new ComponentRole("transcriptionN", "Written Translation", ComponentRole.MeasurementTypes.Words, (p => Path.GetExtension(p).ToLower() == ".txt"), "$SessionId$_Translation-N");
		}

		private ComponentFile CreateComponentFileWithRoleChoices(string path)
		{
			var componentRoles = new []
									{
										new ComponentRole("translation", "translation", ComponentRole.MeasurementTypes.None, p => p.EndsWith("txt"), "$SessionId$_Original"),
										new ComponentRole("transcriptionN", "Written Translation", ComponentRole.MeasurementTypes.Words, (p => Path.GetExtension(p).ToLower() == ".txt"), "$SessionId$_Translation-N"),
										new ComponentRole("original", "Original Recording", ComponentRole.MeasurementTypes.Time, ComponentRole.GetIsAudioVideo, "$SessionId$_Original")
									};

			return new ComponentFile(path,
				new FileType[] { FileType.Create("Text", ".txt"), },
				componentRoles,
				new FileSerializer());
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
