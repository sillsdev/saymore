using System;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore.Media;

namespace SayMoreTests.Model.Files
{
	[TestFixture]
	public class MediaFileInfoTests
	{
		[SetUp]
		public void Setup()
		{
			SIL.Reporting.ErrorReport.IsOkToInteractWithUser = false;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates video file in the temp. folder. It's up to the caller to delete the file
		/// when finished.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetTestVideoFile()
		{
			return GetMediaResourceFile("SayMoreTests.Resources.shortVideo.wmv", "wmv");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates audio file in the temp. folder. It's up to the caller to delete the file
		/// when finished.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetShortTestAudioFile()
		{
			return GetMediaResourceFile(Resources.shortSound, "wav");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates audio file in the temp. folder. It's up to the caller to delete the file
		/// when finished.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetLongerTestAudioFile()
		{
			return GetMediaResourceFile(Resources.longerSound,"wav");
		}

		/// ------------------------------------------------------------------------------------
		private static string GetMediaResourceFile(string resource, string mediaFileExtension)
		{
			var assembly = Assembly.GetExecutingAssembly();
			return GetMediaResourceFile(assembly.GetManifestResourceStream(resource), mediaFileExtension);
		}

		/// ------------------------------------------------------------------------------------
		private static string GetMediaResourceFile(Stream stream, string mediaFileExtension)
		{
			string path, mediaFilePath;
			do
			{
				path = Path.GetTempFileName();
				mediaFilePath = Path.ChangeExtension(path, mediaFileExtension);
			} while (File.Exists(mediaFilePath));
			File.Move(path, mediaFilePath);

			var buffer = new byte[stream.Length];
			stream.Read(buffer, 0, buffer.Length);
			stream.Close();
			stream.Dispose();

			using (var outStream = File.OpenWrite(mediaFilePath))
			{
				outStream.Write(buffer, 0, buffer.Length);
				outStream.Close();
			}

			return mediaFilePath;
		}

		/// ------------------------------------------------------------------------------------
		public static string CreateRecording(string folder)
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

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void Duration_Audio_Correct()
		{
			using (var folder = new TemporaryFolder("FileStatisticsTests"))
			{
				var recording = CreateRecording(folder.Path);
				var info = MediaFileInfo.GetInfo(recording);
				Assert.AreEqual(1446d, info.Duration.TotalMilliseconds);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void MPlayerMediaInfo_CreateAudio_ContainsCorrectInfo()
		{
			var tmpfile = GetShortTestAudioFile();

			try
			{
				var minfo = MediaFileInfo.GetInfo(tmpfile);
				Assert.IsFalse(minfo.IsVideo);
				Assert.AreEqual(tmpfile, minfo.MediaFilePath);
				Assert.AreEqual(1.446d, minfo.Duration.TotalSeconds);
				Assert.IsNull(minfo.FullSizedThumbnail);
			}
			finally
			{
				File.Delete(tmpfile);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void MPlayerMediaInfo_CreateVideo_ContainsCorrectInfo()
		{
			var tmpfile = GetTestVideoFile();

			try
			{
				var minfo = MediaFileInfo.GetInfo(tmpfile);
				Assert.IsTrue(minfo.IsVideo);
				Assert.AreEqual(tmpfile, minfo.MediaFilePath);
// REVIEW: What to do?				Assert.AreEqual(5.49f, (float)Math.Round(minfo.StartTime, 2));
				Assert.AreEqual(9.5f, (float)Math.Round(minfo.DurationInSeconds, 2));
				Assert.AreEqual(new Size(320, 240), minfo.Video.PictureSize);
				Assert.IsNotNull(minfo.FullSizedThumbnail);
			}
			finally
			{
				File.Delete(tmpfile);
			}
		}
	}
}