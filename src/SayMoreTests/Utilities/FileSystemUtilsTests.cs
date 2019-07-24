﻿using System.ComponentModel;
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
					Assert.Throws<IOException>(() => FileSystemUtils.RobustDelete(fileName));
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
	}
}
