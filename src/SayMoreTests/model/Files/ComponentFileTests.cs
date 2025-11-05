using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.Utilities;
using SayMoreTests.Transcription.Model;

namespace SayMoreTests.Model.Files
{
	[TestFixture]
	public class ComponentFileTests
	{
		private TemporaryFolder _parentFolder;
		private List<FileType> _fileTypes;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("componentFileTest");
			_fileTypes = new List<FileType> { new AnnotationFileType(null, null) };
		}

		/// ------------------------------------------------------------------------------------
		private string ParentFolderName => Path.GetFileName(_parentFolder.Path);

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnCI")]
		public void GetFileType_IsText_GivesTextFileType()
		{
			var f = CreateComponentFile("abc.txt");
			Assert.AreEqual("Text", f.FileType.Name);
		}

		/// ------------------------------------------------------------------------------------
		private ComponentFile CreateComponentFile(string fileName)
		{
			return CreateComponentFile(null, fileName);
		}

		/// ------------------------------------------------------------------------------------
		public ComponentFile CreateComponentFile(ProjectElement parentElement, string fileName)
		{
			return CreateComponentFile(_parentFolder, parentElement, fileName);
		}

		/// ------------------------------------------------------------------------------------
		public static ComponentFile CreateComponentFile(TemporaryFolder parentFolder,
			ProjectElement parentElement, string fileName)
		{
			File.WriteAllText(parentFolder.Combine(fileName), @"hello");

			var cf = new ComponentFile(parentElement, parentFolder.Combine(fileName),
				new[] { FileType.Create("Text", ".txt"), new UnknownFileType(null, null) },
				new ComponentRole[] { }, new XmlFileSerializer(null), null, null, null);

			cf.Save(); //creates the meta file path

			Assert.IsTrue(File.Exists(parentFolder.Combine(fileName)));
			Assert.IsTrue(File.Exists(parentFolder.Combine(fileName + ".meta")));

			return cf;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnCI")]
		public void MoveToRecycleBin_MoveBothFileAndMetaFile_MovesFiles()
		{
			var f = CreateComponentFile("abc.zzz");
			Assert.IsTrue(ComponentFile.MoveToRecycleBin(f, false));
			Assert.IsFalse(File.Exists(_parentFolder.Combine("abc.zzz")));
			Assert.IsFalse(File.Exists(_parentFolder.Combine("abc.zzz.meta")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnCI")]
		public void MoveToRecycleBin_MissingComponentFile_Fails()
		{
			var f = CreateComponentFile("abc.zzz");
			File.Delete(_parentFolder.Combine("abc.zzz"));
			Assert.IsFalse(File.Exists(_parentFolder.Combine("abc.zzz")));
			Assert.IsFalse(ComponentFile.MoveToRecycleBin(f, false));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnCI")]
		public void MoveToRecycleBin_MissingMetadataFile_FailsButReturnsTrue()
		{
			var f = CreateComponentFile("abc.zzz");
			File.Delete(_parentFolder.Combine("abc.zzz.meta"));
			Assert.IsFalse(File.Exists(_parentFolder.Combine("abc.zzz.meta")));
			Assert.IsTrue(ComponentFile.MoveToRecycleBin(f, false));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnCI")]
		public void GetFileType_UnknownType_UnknownFileType()
		{
			var f = CreateComponentFile("abc.zzz");
			Assert.AreEqual("Unknown", f.FileType.Name);
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetStringValue_FieldMissing_ReturnsSpecifiedDefault()
		{
			var f = CreateComponentFile("abc.zzz");
			Assert.AreEqual("hello", f.GetStringValue("notThere", "hello"));
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetStringValue_FieldIsThere_ReturnsCorrectValue()
		{
			var f = CreateComponentFile("abc.zzz");
			SetStringValue(f, "color", "red");
			Assert.AreEqual("red", f.GetStringValue("color", "blue"));
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetValue_FieldMissing_ReturnsSpecifiedDefault()
		{
			var f = CreateComponentFile("abc.zzz");
			Assert.AreEqual(DateTime.MinValue, f.GetValue("notThere", DateTime.MinValue));
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetValue_FieldIsThere_ReturnsCorrectValue()
		{
			var f = CreateComponentFile("abc.zzz");
			SetValue(f, "wwII", new DateTime(1941, 12, 7));
			Assert.AreEqual(new DateTime(1941, 12, 7), f.GetValue("wwII", null));
		}

		[Test]
		[Category("SkipOnCI")]
		public void RenameId_FieldIsThere_Succeeds()
		{
			var f = CreateComponentFile("abc.zzz");
			SetStringValue(f, "height", "25");
			SetStringValue(f, "width", "50");

			f.RenameId("width", "girth");
			Assert.IsNull(f.GetStringValue("width", null));
			Assert.AreEqual("50", f.GetStringValue("girth", null));
			Assert.AreEqual("25", f.GetStringValue("height", null));
		}

		[Test]
		[Category("SkipOnCI")]
		public void RenameId_FieldIsThere_OldIdReturnsNothing()
		{
			var f = CreateComponentFile("abc.zzz");
			SetStringValue(f, "height", "25");
			SetStringValue(f, "width", "50");

			f.RenameId("width", "girth");
			Assert.IsNull(f.GetStringValue("width", null));
		}

		[Test]
		[Category("SkipOnCI")]
		public void RenameId_FieldIsThere_NewIdReturnsOldValue()
		{
			var f = CreateComponentFile("abc.zzz");
			SetStringValue(f, "height", "25");
			SetStringValue(f, "width", "50");

			f.RenameId("width", "girth");
			Assert.AreEqual("50", f.GetStringValue("girth", null));
		}

		[Test]
		[Category("SkipOnCI")]
		public void RenameId_FieldMissing_ReturnsFalse()
		{
			var f = CreateComponentFile("abc.zzz");
			SetStringValue(f, "height", "25");

			f.RenameId("width", "girth");
			Assert.IsNull(f.GetStringValue("width", null));
			Assert.IsNull(f.GetStringValue("girth", null));
			Assert.AreEqual("25", f.GetStringValue("height", null));
		}

		[Test]
		[Category("SkipOnCI")]
		public void RenameId_FieldMissing_NewIdReturnsNothing()
		{
			var f = CreateComponentFile("abc.zzz");
			SetStringValue(f, "height", "25");
			f.RenameId("width", "girth");
			Assert.IsNull(f.GetStringValue("width", null));
		}

		[Test]
		[Category("SkipOnCI")]
		public void RemoveField_FieldIsThere_RemovesIt()
		{
			var f = CreateComponentFile("abc.zzz");
			SetStringValue(f, "height", "25");
			SetStringValue(f, "width", "50");
			f.RemoveField("width");
			Assert.AreEqual("25", f.GetStringValue("height", null));
			Assert.IsNull(f.GetStringValue("width", null));
		}

		[Test]
		[Category("SkipOnCI")]
		public void RemoveField_FieldIsThere_OldIdReturnsNothing()
		{
			var f = CreateComponentFile("abc.zzz");
			SetStringValue(f, "height", "25");
			SetStringValue(f, "width", "50");
			f.RemoveField("width");
			Assert.IsNull(f.GetStringValue("width", null));
		}

		[Test]
		[Category("SkipOnCI")]
		public void RemoveField_FieldMissing_DoesNothing()
		{
			var f = CreateComponentFile("abc.zzz");
			SetStringValue(f, "height", "25");
			f.RemoveField("width");
			Assert.AreEqual("25", f.GetStringValue("height", null));
			Assert.IsNull(f.GetStringValue("width", null));
		}

		[Test]
		[Category("SkipOnCI")]
		public void SetValue_ChangingValue_NewValueOverwritesOld()
		{
			var f = CreateComponentFile("abc.zzz");
			SetStringValue(f, "color", "red");
			SetStringValue(f, "color", "green");
			Assert.AreEqual("green", f.GetStringValue("color", "blue"));
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetCanHaveAnnotationFile_IsNotMediaFile_ReturnsFalse()
		{
			var f = CreateComponentFile("abc.zzz");
			Assert.IsFalse(f.GetCanHaveAnnotationFile());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetCanHaveAnnotationFile_IsWaveFile_ReturnsTrue()
		{
			var f = CreateAudioComponentFile("abc.wav");
			Assert.IsTrue(f.GetCanHaveAnnotationFile());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetCanHaveAnnotationFile_IsMp3File_ReturnsTrue()
		{
			var f = CreateAudioComponentFile("abc.mp3");
			Assert.IsTrue(f.GetCanHaveAnnotationFile());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetSuggestedPathToAnnotationFile_IsNotMediaFile_ReturnsNull()
		{
			var f = CreateComponentFile("abc.zzz");
			Assert.IsNull(f.GetSuggestedPathToAnnotationFile());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetSuggestedPathToAnnotationFile_FileIsWave_ReturnsFolderPath()
		{
			var f = CreateAudioComponentFile("abc.wav");
			var expected = Path.GetDirectoryName(f.PathToAnnotatedFile);
			expected = Path.Combine(expected, "abc.wav.annotations.eaf");
			Assert.AreEqual(expected, f.GetSuggestedPathToAnnotationFile());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetSuggestedPathToAnnotationFile_FileIsMp3_ReturnsFolderPath()
		{
			var f = CreateAudioComponentFile("abc.mp3");
			var expected = Path.GetDirectoryName(f.PathToAnnotatedFile);
			expected = Path.Combine(expected, "abc.mp3.annotations.eaf");
			Assert.AreEqual(expected, f.GetSuggestedPathToAnnotationFile());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetDoesHaveAnnotationFile_IsNotMediaFile_ReturnsFalse()
		{
			Assert.IsFalse(CreateComponentFile("abc.zzz").GetDoesHaveAnnotationFile());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetDoesHaveAnnotationFile_IsMediaFileButDoesNotHaveEafFile_ReturnsFalse()
		{
			var file = CreateAudioComponentFile("abc.mp3");
			File.Delete(file.GetSuggestedPathToAnnotationFile());
			Assert.IsFalse(file.GetDoesHaveAnnotationFile());

			file = CreateAudioComponentFile("abc.wav");
			File.Delete(file.GetSuggestedPathToAnnotationFile());
			Assert.IsFalse(file.GetDoesHaveAnnotationFile());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetDoesHaveAnnotationFile_IsMediaFileAndHasEafFile_ReturnsTrue()
		{
			Assert.IsTrue(CreateAudioComponentFile("abc.mp3").GetDoesHaveAnnotationFile());
			Assert.IsTrue(CreateAudioComponentFile("abc.wav").GetDoesHaveAnnotationFile());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetSuggestedPathToOralAnnotationFile_FileCanBeAnnotated_ReturnsCorrectPath()
		{
			Assert.AreEqual("abc.mp3.oralAnnotations.wav", Path.GetFileName(
				CreateAudioComponentFile("abc.mp3").GetAnnotationFile().GetSuggestedPathToOralAnnotationFile()));

			Assert.AreEqual("abc.wav.oralAnnotations.wav", Path.GetFileName(
				CreateAudioComponentFile("abc.wav").GetAnnotationFile().GetSuggestedPathToOralAnnotationFile()));

			Assert.AreEqual("abc.wma.oralAnnotations.wav", Path.GetFileName(
				CreateAudioComponentFile("abc.wma").GetAnnotationFile().GetSuggestedPathToOralAnnotationFile()));

			Assert.AreEqual("abc.mov.oralAnnotations.wav", Path.GetFileName(
				CreateVideoComponentFile("abc.mov").GetAnnotationFile().GetSuggestedPathToOralAnnotationFile()));

			Assert.AreEqual("abc.mpg.oralAnnotations.wav", Path.GetFileName(
				CreateVideoComponentFile("abc.mpg").GetAnnotationFile().GetSuggestedPathToOralAnnotationFile()));

			Assert.AreEqual("abc.wmv.oralAnnotations.wav", Path.GetFileName(
				CreateVideoComponentFile("abc.wmv").GetAnnotationFile().GetSuggestedPathToOralAnnotationFile()));
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetSuggestedPathToAnnotationFile_FileCannotBeAnnotated_ReturnsNull()
		{
			var file = CreateAudioComponentFile("abc.pdf");
			Assert.IsNull(file.GetSuggestedPathToAnnotationFile());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetSuggestedPathToAnnotationFile_FileCanBeAnnotated_ReturnsCorrectPath()
		{
			Assert.AreEqual("abc.mp3.annotations.eaf", Path.GetFileName(
				CreateAudioComponentFile("abc.mp3").GetSuggestedPathToAnnotationFile()));

			Assert.AreEqual("abc.wav.annotations.eaf", Path.GetFileName(
				CreateAudioComponentFile("abc.wav").GetSuggestedPathToAnnotationFile()));

			Assert.AreEqual("abc.wma.annotations.eaf", Path.GetFileName(
				CreateAudioComponentFile("abc.wma").GetSuggestedPathToAnnotationFile()));

			Assert.AreEqual("abc.mov.annotations.eaf", Path.GetFileName(
				CreateVideoComponentFile("abc.mov").GetSuggestedPathToAnnotationFile()));

			Assert.AreEqual("abc.mpg.annotations.eaf", Path.GetFileName(
				CreateVideoComponentFile("abc.mpg").GetSuggestedPathToAnnotationFile()));

			Assert.AreEqual("abc.wmv.annotations.eaf", Path.GetFileName(
				CreateVideoComponentFile("abc.wmv").GetSuggestedPathToAnnotationFile()));
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetSuggestedPathToStandardAudioFile_FileCannotBeAnnotated_ReturnsNull()
		{
			var file = CreateAudioComponentFile("abc.pdf");
			Assert.IsNull(file.GetSuggestedPathToStandardAudioFile());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetSuggestedPathToStandardAudioFile_FileCanBeAnnotated_ReturnsCorrectPath()
		{
			foreach (var ext in new[] { "mp3", "wav", "wma", "flac" })
			{
				Assert.AreEqual("abc" + SayMore.Properties.Settings.Default.StandardAudioFileSuffix, Path.GetFileName(
					CreateAudioComponentFile("abc." + ext).GetSuggestedPathToStandardAudioFile()));
			}

			foreach (var ext in new[] { "mov", "mpg", "wmv" })
			{
				Assert.AreEqual("abc" + SayMore.Properties.Settings.Default.StandardAudioFileSuffix, Path.GetFileName(
					CreateVideoComponentFile("abc." + ext).GetSuggestedPathToStandardAudioFile()));
			}
		}

		private ComponentFile CreateAudioComponentFile(string filename)
		{
			var file = new ComponentFile(null, _parentFolder.Combine(filename),
				new FileType[] { new AudioFileType(null, null, null), new UnknownFileType(null, null) },
				new ComponentRole[] { }, new XmlFileSerializer(null), null, null, null);

			var annotationPath = Path.Combine(_parentFolder.Path,
				AnnotationFileHelper.ComputeEafFileNameFromOralAnnotationFile(filename));
			AnnotationFileHelperTests.CreateTestEaf(annotationPath);
			var annotationFile = new AnnotationComponentFile(null, annotationPath,
				file, _fileTypes, null);

			file.SetAnnotationFile(annotationFile);
			return file;
		}

		private ComponentFile CreateVideoComponentFile(string filename)
		{
			var file = new ComponentFile(null, _parentFolder.Combine(filename),
				new FileType[] { new VideoFileType(null, null, null), new UnknownFileType(null, null) },
				new ComponentRole[] { }, new XmlFileSerializer(null), null, null, null);

			var annotationPath = Path.Combine(_parentFolder.Path,
				AnnotationFileHelper.ComputeEafFileNameFromOralAnnotationFile(filename));
			AnnotationFileHelperTests.CreateTestEaf(annotationPath);
			var annotationFile = new AnnotationComponentFile(null, annotationPath,
				file, _fileTypes, null);

			file.SetAnnotationFile(annotationFile);
			return file;
		}

		private static ComponentFile CreateComponentFileWithRoleChoices(string path)
		{
			return CreateComponentFileWithRoleChoices(null, path);
		}

		private static ComponentFile CreateComponentFileWithRoleChoices(ProjectElement parentElement, string path)
		{
			var componentRoles = new[]
			{
				new ComponentRole(typeof(Session), ComponentRole.kOralTranslationComponentRoleId, "translation",
					ComponentRole.MeasurementTypes.None, p => p.EndsWith("txt"),
					"$ElementId$_Translation", Color.Magenta, Color.Black),

				new ComponentRole(typeof(Session), ComponentRole.kFreeTranslationComponentRoleId, "Written Translation",
					ComponentRole.MeasurementTypes.Words, (p => Path.GetExtension(p).ToLower() == ".txt"),
					"$ElementId$_Transcription", Color.Magenta, Color.Black),

				new ComponentRole(typeof(Session), ComponentRole.kSourceComponentRoleId, "Source Recording",
					ComponentRole.MeasurementTypes.Time, FileSystemUtils.GetIsAudioVideo,
					"$ElementId$_Source", Color.Magenta, Color.Black),

				new ComponentRole(typeof(Person), ComponentRole.kConsentComponentRoleId, "Informed Consent",
					ComponentRole.MeasurementTypes.None, (p => p.Contains("_Consent.")),
					"$ElementId$_Consent", Color.Magenta, Color.Black),

				new ComponentRole(typeof(Person), ComponentRole.kCarefulSpeechComponentRoleId, "Careful Speech",
					ComponentRole.MeasurementTypes.None, (p => p.Contains("_Careful.")),
					"$ElementId$_Consent", Color.Magenta, Color.Black)
			};

			return new ComponentFile(parentElement, path, new[] { FileType.Create("Text", ".txt"), FileType.Create("Audio", ".avi"), },
				componentRoles, new XmlFileSerializer(null), null, null, null);
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetRelevantComponentRoles_ForEvent_ReturnsOnlySessionRoles()
		{
			using (var folder = new TemporaryFolder("TestGetRelevantComponentRoles"))
			{
				var session = ProjectElementTests.CreateSession(folder.Path, "stupidSession");
				var f = CreateComponentFileWithRoleChoices(session, "abc.txt");
				foreach (var role in f.GetRelevantComponentRoles())
					Assert.AreEqual(typeof(Session), role.RelevantElementType);
			}
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetRelevantComponentRoles_ForPerson_ReturnsOnlyPersonRoles()
		{
			using (var folder = new TemporaryFolder("TestGetRelevantComponentRoles"))
			{
				var session = ProjectElementTests.CreatePerson(folder.Path, "stupidPerson");
				var f = CreateComponentFileWithRoleChoices(session, "abc.txt");
				foreach (var role in f.GetRelevantComponentRoles())
					Assert.AreEqual(typeof(Person), role.RelevantElementType);
			}
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetAssignedRoles_NoRoles_ReturnsEmptyEnumerator()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc.txt");
			Assert.AreEqual(0, f.GetAssignedRoles().Count());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetAssignedRoles_HasOneRoles_ReturnsThem()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc_Source.avi");
			Assert.AreEqual(1, f.GetAssignedRoles().Count());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetAssignedRoles_ForConsentAndSessionType_ReturnsEmptyEnumerator()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc_Consent.txt");
			Assert.AreEqual(0, f.GetAssignedRoles(new Session()).Count());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetAssignedRoles_ForConsentAndPersonType_ReturnsThem()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc_Consent.txt");
			Assert.AreEqual(1, f.GetAssignedRoles(new Person()).Count());
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetAssignedRoles_ForTranslation_ReturnCorrectOne()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc_Translation.txt");
			Assert.AreEqual("translation", f.GetAssignedRoles(new Session()).First().Name);
		}

		[Test]
		[Category("SkipOnCI")]
		public void GetAssignedRoles_HasTranslationWIthLanguageTag_ReturnTranslation()
		{
			ComponentFile f = CreateComponentFileWithRoleChoices("abc_Translation-xyz.txt");
			Assert.AreEqual("translation", f.GetAssignedRoles(new Session()).First().Name);
		}

		[Test]
		[Category("SkipOnCI")]
		public void IdentifyAsRole_FileRenamed()
		{
			var f = CreateComponentFile("abc.txt");
			var role = new ComponentRole(typeof (Session), "someRole", "someRole", ComponentRole.MeasurementTypes.None,
										 p => p.EndsWith("txt"), "$ElementId$_someRole", Color.Magenta, Color.Black);
			f.AssignRole(role);
			Assert.AreEqual(ParentFolderName + "_someRole.txt", Path.GetFileName(f.PathToAnnotatedFile));
			Assert.IsTrue(File.Exists(f.PathToAnnotatedFile));
		}

		public string SetStringValue(ComponentFile file, string key, string value)
		{
			return file.SetStringValue(key, value);
		}

		public object SetValue(ComponentFile file, string key, object value)
		{
			var succeeded = file.SetValue(key, value, out var failureMessage);

			if (!string.IsNullOrEmpty(failureMessage))
				throw new ApplicationException(failureMessage);

			return succeeded;
		}
	}
}
