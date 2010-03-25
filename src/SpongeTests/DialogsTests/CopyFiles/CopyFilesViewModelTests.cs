using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using Palaso.TestUtilities;
using SIL.Sponge.Dialogs.NewSessionsFromFiles.CopyFiles;

namespace SpongeTests.DialogsTests.CopyFiles
{
	[TestFixture]
	public sealed class CopyFileViewModelTests
	{
		private TemporaryFolder _sourceFolder;
		private TemporaryFolder _destinationFolder;
		private double[] _megabytesPerFile = new[] { 0.1, 0.2 };
		private List<KeyValuePair<string, string>> _sourceAndDestinationPathPairs;

		[SetUp]
		public void Setup()
		{
			_sourceFolder = new TemporaryFolder("CopyFilesTests-Source");
			_destinationFolder = new TemporaryFolder("CopyFilesTests-Dest");
		}

		[TearDown]
		public void TearDown()
		{
			_sourceFolder.Dispose();
		}


		[Test]
		public void TotalPercentage_NotStarted_Zero()
		{
			var copier = CreateCopier();
			Assert.AreEqual(0,copier.TotalPercentage);
		}
		[Test]
		public void TotalPercentage_Finished_100()
		{
			var copier = CreateCopier();
			copier.Start();

			while (!copier.Finished)
			{
				Thread.Sleep(1000);
			}
			Assert.AreEqual(100, copier.TotalPercentage);
		}

		[Test, ExpectedException(typeof(InvalidOperationException)), Ignore("not yet")]
		public void Constructor_DestinationDirectoryOfSomeFileDoesNotExist_Throws()
		{

		}
		[Test, ExpectedException(typeof(InvalidOperationException)), Ignore("not yet")]
		public void Constructor_DestinationFileAlreadyNotExist_Throws()
		{

		}

		[Test]
		public void Copy_WhenFinished_DestinationFilesExist()
		{
			var copier = CreateCopier();
			copier.Start();
			while(!copier.Finished)
				Thread.Sleep(100);
			foreach (var pair in _sourceAndDestinationPathPairs)
			{
				Assert.IsTrue(File.Exists(pair.Value));
			}
		}

		[Test]
		public void ErrorMessage_ProblemEncountered_NotEmpty()
		{
			var copier = CreateCopier();
			File.WriteAllText(_sourceAndDestinationPathPairs.First().Value, "file in the way");
			copier.Start();
			while (!copier.Finished)
				Thread.Sleep(100);
			Assert.IsTrue(copier.StatusString.ToLower().Contains("failed"));
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Constructor_NoFilesSpecified_Throws()
		{
			new CopyFilesViewModel( new List<KeyValuePair<string, string>>() );

		}

		[Test, Ignore("By Hand UI Test")]
		public void UI_3LargeFiles()
		{
			var c = CreateCopier(100, 3);
			Form f = new Form()
				;
			f.Load += new EventHandler((sender, e) => c.Start());
			bool waiting = true;
//			Application.Idle += new EventHandler((sender, e) =>
//			                                     	{
//														if (waiting)
//															c.Start();
//			                                     		waiting = false;
//			                                     	});

			var view = new CopyFilesView(c);
			f.Controls.Add(view);
			Application.Run(f);
		}



		private CopyFilesViewModel CreateCopier()
		{
			return CreateCopier(0.1, 2);
		}

		private CopyFilesViewModel CreateCopier(double megabytesPerFile, int fileCount)
		{
			_sourceAndDestinationPathPairs = new List<KeyValuePair<string, string>>();
			for (int i = 0; i < fileCount; i++)
			{
				var source = _sourceFolder.GetPathForNewTempFile(false);
				var dest = _destinationFolder.GetPathForNewTempFile(false);
				WriteRandomFile(source, megabytesPerFile);
				_sourceAndDestinationPathPairs.Add(new KeyValuePair<string, string>(source, dest));
			}
			return new CopyFilesViewModel(_sourceAndDestinationPathPairs);
		}

		private void WriteRandomFile(string path, double megabytes)
		{
			var buffer = new byte[1024];
			var random = new Random();
			for (int i = 0; i < 1024; i++)
			{
				buffer[i]=((byte)random.Next(999));
			}

			using(var stream = File.OpenWrite(path))
			{
				for (int i = 0; i < 1024*megabytes; i++)
				{
					stream.Write(buffer,0,1024);
				}
			}
		}
	}

}
