using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model.Files;

namespace SayMoreTests.Model.Files
{
	[TestFixture]
	public class FileTypeTests
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
		public void IsMatch_BasedOnFactoryUsingExtension_ShouldBeAMatch()
		{
			var t = FileType.Create("text", "txt");
			Assert.IsTrue(t.IsMatch(_parentFolder.Combine("blah.txt")));
		}
		[Test]
		public void IsMatch_BasedOnFactoryUsingExtension_ShouldNotBeAMatch()
		{
			var t = FileType.Create("text", "txt");
			Assert.IsFalse(t.IsMatch(_parentFolder.Combine("blah.foo")));
		}
		[Test]
		public void IsMatch_BasedOnFactoryUsingMultipleExtension_ShouldBeAMatch()
		{
			var t = FileType.Create("text", new string[]{"txt","db"});
			Assert.IsTrue(t.IsMatch(_parentFolder.Combine("blah.db")));
		}


	}
}
