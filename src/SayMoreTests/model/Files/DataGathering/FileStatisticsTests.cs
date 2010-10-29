using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model.Files;

namespace SayMoreTests.Model.Files.DataGathering
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
		//[Ignore("MPLayer isn't quit precise enough for this kind of test.")]
		[Category("SkipOnTeamCity")]
		public void Duration_Audio_Correct()
		{
			using(var folder = new TemporaryFolder("FileStatisticsTests"))
			{
				var recording = CreateRecording(folder.Path);
				var info = new MediaFileInfo(recording);
				Assert.AreEqual(1450, info.Duration.TotalMilliseconds);
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
