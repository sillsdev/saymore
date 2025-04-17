using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Utilities;
using SIL.TestUtilities;

namespace SayMoreTests.Utilities
{
	[TestFixture]
	public class FileSystemUtilsTests
	{
		[Test]
		public void RobustDelete_NotLocked_Deletes_NoException()
		{
			using (var tempFolder = new TemporaryFolder("RobustDelete01"))
			{
				var fileName = Path.Combine(tempFolder.Path, "not_locked.txt");

				FileAssert.DoesNotExist(fileName);

				File.WriteAllText(fileName, @"temp file");
				FileAssert.Exists(fileName);
				
				Assert.DoesNotThrow(() => FileSystemUtils.RobustDelete(fileName));
				FileAssert.DoesNotExist(fileName);
			}
		}

		[Test]
		public void RobustDelete_Locked_NoDelete_ThrowsException()
		{
			using (var tempFolder = new TemporaryFolder("RobustDelete02"))
			{
				var fileName = Path.Combine(tempFolder.Path, "not_locked.txt");

				FileAssert.DoesNotExist(fileName);

				File.WriteAllText(fileName, @"temp file");
				FileAssert.Exists(fileName);

				using (new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
				{
					var expectedFileToCheck = fileName;
					FileSyncHelper.SyncClient OnFileChecked(string filePath)
					{
						if (expectedFileToCheck != filePath)
							Assert.Fail("Unexpected file checked: " + filePath);
						expectedFileToCheck = "never again";
						return FileSyncHelper.SyncClient.None;
					}

					FileSyncHelper.TestOverrideFileChecked += OnFileChecked;

					try
					{
						Assert.Throws<IOException>(() => FileSystemUtils.RobustDelete(fileName));
					}
					finally
					{
						FileSyncHelper.TestOverrideFileChecked -= OnFileChecked;
						Assert.That(expectedFileToCheck, Is.EqualTo("never again"));
					}
					FileAssert.Exists(fileName);
				}
			}
		}

		[Test]
		public void RobustDelete_TemporaryLocked_Deletes_NoException()
		{
			using (var tempFolder = new TemporaryFolder("RobustDelete03"))
			{
				var fileName = Path.Combine(tempFolder.Path, "not_locked.txt");

				FileAssert.DoesNotExist(fileName);

				var worker = new BackgroundWorker();
				worker.DoWork += delegate
				{
					File.WriteAllText(fileName, @"temp file");
					using (new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
					{
						Thread.Sleep(1700);
					}
				};
				worker.RunWorkerAsync();

				while (!File.Exists(fileName))
					Application.DoEvents();

				Assert.DoesNotThrow(() => FileSystemUtils.RobustDelete(fileName));
				FileAssert.DoesNotExist(fileName);
			}
		}

        [TestCase(@"myfile.nam")]
        [TestCase(@"noext")]
        [TestCase(@"c:\mypath.ext")]
        [TestCase(@"c:\mypath.ext\hispath\yourpath.yes")]
        [TestCase(@"the\good.one")]
        public void IsValidShortFileNamePath_Valid_ReturnsTrue(string path)
        {
            Assert.That(FileSystemUtils.IsValidShortFileNamePath(path), Is.True);
        }

        [TestCase(@"mybigfile.nam")]
        [TestCase(@"myfile.bigext")]
        [TestCase(@"my file.txt")]
        [TestCase(@"what€is.ths")]
        [TestCase(@"the:bad.one")]
        [TestCase(@"the/bad.one")]
        [TestCase(@"the?bad.one")]
        [TestCase(@"the.bad.one")]
        public void IsValidShortFileNamePath_Invalid_ReturnsFalse(string path)
        {
            Assert.That(FileSystemUtils.IsValidShortFileNamePath(path), Is.False);
        }
	}
}
