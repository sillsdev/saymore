using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model.Fields;
using SayMore.Model.Files;

namespace SayMoreTests.model.Files
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class FieldUpdaterTests
	{
		private TemporaryFolder _rootFolder;
		private ComponentFile _imgFile1;
		private ComponentFile _imgFile2;
		private ComponentFile _audioFile1;
		private ComponentFile _audioFile2;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_rootFolder = new TemporaryFolder("~FieldUpdaterTests~");
			var italiansFolder = _rootFolder.Combine("~Italians~");
			Directory.CreateDirectory(italiansFolder);

			var imgFileType = new ImageFileType();
			var audioFileType = new AudioFileType(() => null);

			var path = _rootFolder.Combine("PierreAugusteRenoir.jpg");
			_imgFile1 = SetupData(path, imgFileType, "Claude", "Monet", "1840", "1840");

			path = Path.Combine(italiansFolder, "SistineChapel.jpg");
			_imgFile2 = SetupData(path, imgFileType, "Michelangelo", "di Lodovico Buonarroti Simoni", "1475", "1564");

			path = _rootFolder.Combine("OntheTerrace.mp3");
			_audioFile1 = SetupData(path, audioFileType, "Pierre-Auguste", "Renoir", "1841", "1919");

			path = Path.Combine(italiansFolder, "TheLastSupper.mp3");
			_audioFile2 = SetupData(path, audioFileType, "Leonardo", "da Vinci", "1452", "1519");
		}

		/// ------------------------------------------------------------------------------------
		private static ComponentFile SetupData(string annotatedFilePath, FileType fileType,
			string first, string last, string birth, string death)
		{
			var file = ComponentFile.CreateMinimalComponentFileForTests(annotatedFilePath, fileType);
			File.CreateText(annotatedFilePath).Close();

			file.MetaDataFieldValues.Clear();
			file.MetaDataFieldValues.Add(new FieldValue("firstName", "string", first));
			file.MetaDataFieldValues.Add(new FieldValue("lastName", "string", last));

			var fav = new FieldValue("born", "string", birth);
			file.MetaDataFieldValues.Add(fav);

			fav = new FieldValue("died", "string", death);
			file.MetaDataFieldValues.Insert(0, fav);

			file.Save();
			return file;
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_rootFolder.Dispose();
			_rootFolder = null;
		}

		#region Renaming tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void RenameFields_DoesNotUpdateOtherFileTypes()
		{
			var list = new List<KeyValuePair<string, string>>();

			list.Add(new KeyValuePair<string, string>("born", "WasBirthed"));
			list.Add(new KeyValuePair<string, string>("died", "PassedOn"));

			var updater = new FieldUpdater(_rootFolder.Path);
			updater.RenameFields(_audioFile1, list);

			_imgFile1.Load();
			Assert.That(_imgFile1.MetaDataFieldValues.Find(x => x.FieldId == "born"), Is.Not.Null);
			Assert.That(_imgFile1.MetaDataFieldValues.Find(x => x.FieldId == "died"), Is.Not.Null);
			Assert.That(_imgFile1.MetaDataFieldValues.Find(x => x.FieldId == "WasBirthed"), Is.Null);
			Assert.That(_imgFile1.MetaDataFieldValues.Find(x => x.FieldId == "PassedOn"), Is.Null);

			_imgFile2.Load();
			Assert.That(_imgFile2.MetaDataFieldValues.Find(x => x.FieldId == "born"), Is.Not.Null);
			Assert.That(_imgFile2.MetaDataFieldValues.Find(x => x.FieldId == "died"), Is.Not.Null);
			Assert.That(_imgFile2.MetaDataFieldValues.Find(x => x.FieldId == "WasBirthed"), Is.Null);
			Assert.That(_imgFile2.MetaDataFieldValues.Find(x => x.FieldId == "PassedOn"), Is.Null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the updater doesn't bother renaming fields in the ComponentFile passed
		/// to the renaming method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void RenameFields_DoesNotUpdateSelf()
		{
			var list = new List<KeyValuePair<string, string>>();

			list.Add(new KeyValuePair<string, string>("born", "WasBirthed"));
			list.Add(new KeyValuePair<string, string>("died", "PassedOn"));

			var updater = new FieldUpdater(_rootFolder.Path);
			updater.RenameFields(_audioFile1, list);

			_audioFile1.Load();
			Assert.That(_audioFile1.MetaDataFieldValues.Find(x => x.FieldId == "born"), Is.Not.Null);
			Assert.That(_audioFile1.MetaDataFieldValues.Find(x => x.FieldId == "died"), Is.Not.Null);
			Assert.That(_audioFile1.MetaDataFieldValues.Find(x => x.FieldId == "WasBirthed"), Is.Null);
			Assert.That(_audioFile1.MetaDataFieldValues.Find(x => x.FieldId == "PassedOn"), Is.Null);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RenameFields_DoesUpdates()
		{
			var list = new List<KeyValuePair<string, string>>();

			list.Add(new KeyValuePair<string, string>("born", "WasBirthed"));
			list.Add(new KeyValuePair<string, string>("died", "PassedOn"));

			var updater = new FieldUpdater(_rootFolder.Path);
			updater.RenameFields(_audioFile1, list);

			// See RenameFields_DoesNotUpdateSelf for why _file2 isn't checked.

			_audioFile2.Load();
			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId == "born"), Is.Null);
			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId == "died"), Is.Null);
			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId == "WasBirthed"), Is.Not.Null);
			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId == "PassedOn"), Is.Not.Null);
		}

		#endregion

		#region Deleting tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteFields_DoesNotUpdateOtherFileTypes()
		{
			var list = new List<string> { "born", "died" };
			var updater = new FieldUpdater(_rootFolder.Path);
			updater.DeleteFields(_audioFile1, list);

			_imgFile1.Load();
			Assert.That(_imgFile1.MetaDataFieldValues.Find(x => x.FieldId == "born"), Is.Not.Null);
			Assert.That(_imgFile1.MetaDataFieldValues.Find(x => x.FieldId == "died"), Is.Not.Null);

			_imgFile2.Load();
			Assert.That(_imgFile2.MetaDataFieldValues.Find(x => x.FieldId == "born"), Is.Not.Null);
			Assert.That(_imgFile2.MetaDataFieldValues.Find(x => x.FieldId == "died"), Is.Not.Null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the updater doesn't bother deleting fields in the ComponentFile passed
		/// to the delete method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteFields_DoesNotUpdateSelf()
		{
			var list = new List<string> { "born", "died" };
			var updater = new FieldUpdater(_rootFolder.Path);
			updater.DeleteFields(_audioFile1, list);

			_audioFile1.Load();
			Assert.That(_audioFile1.MetaDataFieldValues.Find(x => x.FieldId == "born"), Is.Not.Null);
			Assert.That(_audioFile1.MetaDataFieldValues.Find(x => x.FieldId == "died"), Is.Not.Null);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteFields_DoesUpdates()
		{
			var list = new List<string> { "born", "died" };
			var updater = new FieldUpdater(_rootFolder.Path);
			updater.DeleteFields(_audioFile1, list);

			// See DeleteFields_DoesNotUpdateSelf for why _file2 isn't checked.

			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId == "born"), Is.Not.Null);
			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId == "died"), Is.Not.Null);
			_audioFile2.Load();
			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId == "born"), Is.Null);
			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId == "died"), Is.Null);
		}

		#endregion

		#region Adding tests
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFields_DoesNotUpdateOtherFileTypes()
		{
			var list = new List<string> { "ShoeSize", "Height" };
			var updater = new FieldUpdater(_rootFolder.Path);
			updater.AddFields(_audioFile1, list);

			_imgFile1.Load();
			Assert.That(_imgFile1.MetaDataFieldValues.Find(x => x.FieldId.StartsWith("Shoe")), Is.Null);
			Assert.That(_imgFile1.MetaDataFieldValues.Find(x => x.FieldId.StartsWith("Height")), Is.Null);

			_imgFile2.Load();
			Assert.That(_imgFile2.MetaDataFieldValues.Find(x => x.FieldId.StartsWith("Shoe")), Is.Null);
			Assert.That(_imgFile2.MetaDataFieldValues.Find(x => x.FieldId.StartsWith("Height")), Is.Null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the updater doesn't bother adding fields in the ComponentFile passed
		/// to the add method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFields_DoesNotUpdateSelf()
		{
			var list = new List<string> { "ShoeSize", "Height" };
			var updater = new FieldUpdater(_rootFolder.Path);
			updater.AddFields(_audioFile1, list);

			_audioFile1.Load();
			Assert.That(_audioFile1.MetaDataFieldValues.Find(x => x.FieldId.StartsWith("Shoe")), Is.Null);
			Assert.That(_audioFile1.MetaDataFieldValues.Find(x => x.FieldId.StartsWith("Height")), Is.Null);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFields_DoesUpdates()
		{
			var list = new List<string> { "ShoeSize", "Height" };
			var updater = new FieldUpdater(_rootFolder.Path);
			updater.AddFields(_audioFile1, list);

			// See AddFields_DoesNotUpdateSelf for why _file2 isn't checked.

			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId.StartsWith("Shoe")), Is.Null);
			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId.StartsWith("Height")), Is.Null);
			_audioFile2.Load();
			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId.StartsWith("Shoe")), Is.Not.Null);
			Assert.That(_audioFile2.MetaDataFieldValues.Find(x => x.FieldId.StartsWith("Height")), Is.Not.Null);
		}

		#endregion
	}
}
