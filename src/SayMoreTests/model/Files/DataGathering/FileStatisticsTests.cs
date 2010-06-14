using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model.Files.DataGathering;

namespace SayMoreTests.model.Files.DataGathering
{
	[TestFixture]
	public class FileStatisticsTests
	{
		[SetUp]
		public void Setup()
		{
			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void Duration_Audio_Correct()
		{
			using(var folder = new TemporaryFolder("FileStatisticsTests"))
			{
				var recording = CreateRecording(folder.Path);
				var stats = new AudioVideoFileStatistics(recording);
				Assert.AreEqual(new TimeSpan(0,0,0,1), stats.Duration);
			}
		}


		private static string CreateRecording(string folder)
		{
			var buf = new byte[Resources.shortSound.Length];
			Resources.shortSound.Read(buf, 0, buf.Length);
			string destination = folder;
			string wavPath = Path.Combine(destination, Directory.GetFiles(destination).Count() + ".wav");
			var f = File.Create(wavPath);
			f.Write(buf, 0, buf.Length);
			f.Close();
			return wavPath;
		}
	}
}
