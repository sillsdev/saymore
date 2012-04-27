using System.IO;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Utilities.Utilities;

namespace SayMoreTests.UI.Utilities
{
	[TestFixture]
	public class FileSystemUtilsTests
	{
		private TemporaryFolder _parentFolder;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("FileSystemUtilsTests");
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		[Test]
		public void IsFileLocked_FilePathIsNull_ReturnsFalse()
		{
			Assert.IsFalse(FileSystemUtils.IsFileLocked(null));
		}

		[Test]
		public void IsFileLocked_FileDoesntExist_ReturnsFalse()
		{
			Assert.IsFalse(FileSystemUtils.IsFileLocked(@"c:\blahblah.blah"));
		}

		[Test]
		public void IsFileLocked_FileExistsAndIsNotLocked_ReturnsFalse()
		{
			using (var file = new TempFileFromFolder(_parentFolder))
				Assert.IsFalse(FileSystemUtils.IsFileLocked(file.Path));
		}

		[Test]
		public void IsFileLocked_FileExistsAndIsLocked_ReturnsTrue()
		{
			using (var file = new TempFileFromFolder(_parentFolder))
			{
				var stream = File.OpenWrite(file.Path);
				try
				{
					Assert.IsTrue(FileSystemUtils.IsFileLocked(file.Path));
				}
				finally
				{
					stream.Close();
				}
			}
		}
	}
}
