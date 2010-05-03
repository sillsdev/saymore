using NUnit.Framework;
using Palaso.TestUtilities;
using Sponge2.Model;
using Sponge2.Model.Files;

namespace Sponge2Tests.model
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
			return new ComponentFile(path, new FileType[]{FileType.Create("Text", ".txt"), }, new FileSerializer());
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
			f.SetValue("color", "red");
			Assert.AreEqual("red", f.GetStringValue("color", "blue"));
		}

		[Test]
		public void SetValue_ChangingValue_NewValueOverwritesOld()
		{
			ComponentFile f = CreateComponentFile("abc.zzz");
			f.SetValue("color", "red");
			f.SetValue("color", "green");
			Assert.AreEqual("green", f.GetStringValue("color", "blue"));
		}
	}
}
