using System.IO;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore.Model.Files;
using SayMoreTests.MediaUtils;

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
			var t = FileType.Create("text", new[]{"txt","db"});
			Assert.IsTrue(t.IsMatch(_parentFolder.Combine("blah.db")));
		}

		[Test]
		public void ComputeStandardPcmAudioFilePath_SourceHasStandardAudioSuffixAndExt_ReturnsOriginal()
		{
			Assert.AreEqual(@"c:\blah\dumb" + SayMore.Properties.Settings.Default.StandardAudioFileSuffix,
				AudioVideoFileTypeBase.ComputeStandardPcmAudioFilePath(@"c:\blah\dumb" + SayMore.Properties.Settings.Default.StandardAudioFileSuffix));
		}

		[Test]
		public void ComputeStandardPcmAudioFilePath_SourceHasStandardAudioSuffixButNotExt_DoesNotDuplicateSuffix()
		{
			Assert.AreEqual(@"c:\blah\dumb" + SayMore.Properties.Settings.Default.StandardAudioFileSuffix,
				AudioVideoFileTypeBase.ComputeStandardPcmAudioFilePath(@"c:\blah\dumb_StandardAudio.mpg"));
		}

		[Test]
		public void ComputeStandardPcmAudioFilePath_SourceDoesNotHaveStandardAudioSuffix_ReturnsCorrectPath()
		{
			Assert.AreEqual(@"c:\blah\dumb" + SayMore.Properties.Settings.Default.StandardAudioFileSuffix,
				AudioVideoFileTypeBase.ComputeStandardPcmAudioFilePath(@"c:\blah\dumb.mp3"));
		}

		[Test]
		public void GetIsStandardPcmAudioFile_SourceHasStandardAudioSuffixAndExt_ReturnsTrue()
		{
			Assert.IsTrue(AudioVideoFileTypeBase.GetIsStandardPcmAudioFile(@"c:\blah\dumb" +
				SayMore.Properties.Settings.Default.StandardAudioFileSuffix));
		}

		[Test]
		public void GetIsStandardPcmAudioFile_SourceHasStandardAudioSuffixButNotExt_ReturnsFalse()
		{
			using (var tempFile = new TempFileFromFolder(_parentFolder, "blah_StandardAudio.mp3", ""))
				Assert.IsFalse(AudioVideoFileTypeBase.GetIsStandardPcmAudioFile(tempFile.Path));
		}

		[Test]
		public void GetIsStandardPcmAudioFile_SourceIsAlreadyPcm_ReturnsTrue()
		{
			var audioFile = MediaFileInfoTests.GetShortTestAudioFile();

			try
			{
				Assert.IsTrue(AudioVideoFileTypeBase.GetIsStandardPcmAudioFile(audioFile));
			}
			finally
			{
				if (audioFile != null && File.Exists(audioFile))
					File.Delete(audioFile);
			}
		}
	}
}
