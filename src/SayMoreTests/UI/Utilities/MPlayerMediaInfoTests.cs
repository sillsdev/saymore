using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SayMore.UI.Archiving;

namespace SayMoreTests.UI.Utilities
{
	[TestFixture]
	public class MPlayerMediaInfoTests
	{
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
		public static string GetTestAudioFile()
		{
			return GetMediaResourceFile("SayMoreTests.Resources.shortSound.wav", "wav");
		}

		/// ------------------------------------------------------------------------------------
		private static string GetMediaResourceFile(string resource, string mediaFileExtension)
		{
			var path = Path.GetTempFileName();
			var mediaFilePath = Path.ChangeExtension(path, mediaFileExtension);
			File.Move(path, mediaFilePath);

			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream(resource))
			{
				var buffer = new byte[stream.Length];
				for (int i = 0; i < buffer.Length; i++)
					buffer[i] = (byte)stream.ReadByte();

				File.WriteAllBytes(mediaFilePath, buffer);
				stream.Close();
			}

			return mediaFilePath;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void MPlayerMediaInfo_CreateAudio_ContainsCorrectInfo()
		{
			var tmpfile = GetTestAudioFile();

			try
			{
				var minfo = new MPlayerMediaInfo(tmpfile);
				Assert.IsFalse(minfo.IsVideo);
				Assert.AreEqual(tmpfile, minfo.FileName);
				Assert.AreEqual(1.45f, minfo.Duration);
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
				var minfo = new MPlayerMediaInfo(tmpfile);
				Assert.IsTrue(minfo.IsVideo);
				Assert.AreEqual(tmpfile, minfo.FileName);
				Assert.AreEqual(5.49f, (float)Math.Round(minfo.StartTime, 2));
				Assert.AreEqual(4.01f, (float)Math.Round(minfo.Duration, 2));
				Assert.AreEqual(new Size(320, 240), minfo.PictureSize);
				Assert.IsNotNull(minfo.FullSizedThumbnail);
			}
			finally
			{
				File.Delete(tmpfile);
			}
		}
	}
}
