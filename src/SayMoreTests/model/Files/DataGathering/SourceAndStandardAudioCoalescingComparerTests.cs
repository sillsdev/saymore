using System.IO;
using NUnit.Framework;
using SayMore.Media;
using SIL.Reflection;

namespace SayMore.Model.Files.DataGathering
{
	[TestFixture]
	class SourceAndStandardAudioCoalescingComparerTests
	{
		private SourceAndStandardAudioCoalescingComparer m_comparer = new SourceAndStandardAudioCoalescingComparer();

		[Test]
		public void Equals_TotallyDifferentAudioFiles_ReturnsFalse()
		{
			var file1 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file1, "MediaFilePath", "SomePath_Source.mp3");
			
			var file2 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(2300),
				DurationInMilliseconds = 2300,
			};
			ReflectionHelper.SetProperty(file2, "MediaFilePath", "NotThatOne_StandardAudio.WAV");

			// In real life, Equals will not usually get called unless the hash codes are the same.
			Assert.That(m_comparer.GetHashCode(file1), Is.EqualTo(m_comparer.GetHashCode(file2)));
			Assert.IsFalse(m_comparer.Equals(file1, file2));
			Assert.IsFalse(m_comparer.Equals(file2, file1));
		}

		[TestCase("")]
		[TestCase(@"c:\MyPath\")]
		public void Equals_VideoAndAudioFilesWithSameDurationButDifferentNames_ReturnsFalse(string path)
		{
			var file1 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file1, "MediaFilePath", Path.Combine(path, "SomePath_StandardAudio.WAV"));
			
			var file2 = new MediaFileInfo
			{
				DurationInMilliseconds = 5500,
				Video = new TestVideoInfo(5500)
			};
			ReflectionHelper.SetProperty(file2, "MediaFilePath", Path.Combine(path, "NotThatOne_Source.mp4"));

			// In real life, Equals will not usually get called unless the hash codes are the same.
			Assert.That(m_comparer.GetHashCode(file1), Is.EqualTo(m_comparer.GetHashCode(file2)));
			Assert.IsFalse(m_comparer.Equals(file1, file2));
			Assert.IsFalse(m_comparer.Equals(file2, file1));
		}

		[Test]
		public void Equals_AudioFileAndNull_ReturnsFalse()
		{
			var file1 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file1, "MediaFilePath", "SomePath_StandardAudio.WAV");
			
			Assert.IsFalse(m_comparer.Equals(file1, null));
			Assert.IsFalse(m_comparer.Equals(null, file1));
		}

		[Test]
		public void Equals_SameObject_ReturnsTrue()
		{
			var file1 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file1, "MediaFilePath", "SomePath_Source.mp3");

			Assert.IsTrue(m_comparer.Equals(file1, file1));
		}

		[Test]
		public void Equals_IdenticalNameAndDuration_ReturnsTrue()
		{
			var file1 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file1, "MediaFilePath", "SomePath_Source.mp3");
			
			var file2 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file2, "MediaFilePath", "SomePath_Source.mp3");
			// The following is probably impossible in practice, but it at least illustrates
			// that this comparer is not checking all the fields we don't care about.
			file1.LengthInBytes = 7;
			file2.LengthInBytes = 7000;
			file2.Audio.Channels = 6;
			file2.Audio.CodecInformation = "None";

			// In real life, Equals will not usually get called unless the hash codes are the same.
			Assert.That(m_comparer.GetHashCode(file1), Is.EqualTo(m_comparer.GetHashCode(file2)));
			Assert.IsTrue(m_comparer.Equals(file1, file2));
			Assert.IsTrue(m_comparer.Equals(file2, file1));
		}

		[Test]
		public void Equals_IdenticalNameAndDurationWithDifferentPaths_ReturnsFalse()
		{
			var file1 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file1, "MediaFilePath", @"c:\Documents\SayMore\Session2\SomePath_Source.mp3");
			
			var file2 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file2, "MediaFilePath", @"c:\Documents\SayMore\Session3\SomePath_Source.mp3");

			// In real life, Equals will not usually get called unless the hash codes are the same.
			Assert.That(m_comparer.GetHashCode(file1), Is.Not.EqualTo(m_comparer.GetHashCode(file2)));
			Assert.IsFalse(m_comparer.Equals(file1, file2));
			Assert.IsFalse(m_comparer.Equals(file2, file1));
		}

		[Test]
		public void Equals_AudioSourceWithDerivedStandardAudioButDifferentPaths_ReturnsFalse()
		{
			var file1 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file1, "MediaFilePath", @"c:\Documents\SayMore\Session2\SomePath_Source.mp3");
			
			var file2 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file2, "MediaFilePath", @"c:\Documents\SayMore\Session3\SomePath_Source_StandardAudio.wav");

			// In real life, Equals will not usually get called unless the hash codes are the same.
			Assert.That(m_comparer.GetHashCode(file1), Is.Not.EqualTo(m_comparer.GetHashCode(file2)));
			Assert.IsFalse(m_comparer.Equals(file1, file2));
			Assert.IsFalse(m_comparer.Equals(file2, file1));
		}

		[TestCase("")]
		[TestCase(@"c:\MyPath")]
		public void Equals_VideoSourceWithDerivedStandardAudio_ReturnsTrue(string path)
		{
			var file1 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file1, "MediaFilePath", Path.Combine(path, "SomePath_Source.mp3"));
			
			var file2 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file2, "MediaFilePath", Path.Combine(path, "SomePath_Source_StandardAudio.WAV"));

			// In real life, Equals will not usually get called unless the hash codes are the same.
			Assert.That(m_comparer.GetHashCode(file1), Is.EqualTo(m_comparer.GetHashCode(file2)));
			Assert.IsTrue(m_comparer.Equals(file1, file2));
			Assert.IsTrue(m_comparer.Equals(file2, file1));
		}

		[Test]
		public void Equals_AudioSourceWithDerivedStandardAudio_ReturnsTrue()
		{
			
			var file1 = new MediaFileInfo
			{
				DurationInMilliseconds = 5500,
				Video = new TestVideoInfo(5500)
			};
			ReflectionHelper.SetProperty(file1, "MediaFilePath", "SomePath_Source.mp4");

			var file2 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file2, "MediaFilePath", "SomePath_Source_StandardAudio.WAV");

			// In real life, Equals will not usually get called unless the hash codes are the same.
			Assert.That(m_comparer.GetHashCode(file1), Is.EqualTo(m_comparer.GetHashCode(file2)));
			Assert.IsTrue(m_comparer.Equals(file1, file2));
			Assert.IsTrue(m_comparer.Equals(file2, file1));
		}

		[TestCase(1501)]
		[TestCase(-1501)]
		public void Equals_AudioSourceWithDerivedStandardAudioNameButVeryDifferentDuration_ReturnsFalse(int difference)
		{
			// This is a really unlikely scenario, but it is possible (e.g., if the user trimmed one
			// of the files after making the standard audio version). Hard to know how we'd want to
			// deal with it in the statistics, but given how unlikely it is, treating them as not
			// the same file and therefore counting both is probably marginally acceptable.
			var file1 = new MediaFileInfo
			{
				DurationInMilliseconds = 5500 + difference,
				Video = new TestVideoInfo(5500 + difference)
			};
			ReflectionHelper.SetProperty(file1, "MediaFilePath", "SomePath_Source.mp4");

			var file2 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file2, "MediaFilePath", "SomePath_Source_StandardAudio.WAV");

			// In real life, Equals will not usually get called unless the hash codes are the same.
			Assert.That(m_comparer.GetHashCode(file1), Is.EqualTo(m_comparer.GetHashCode(file2)));
			Assert.IsFalse(m_comparer.Equals(file1, file2));
			Assert.IsFalse(m_comparer.Equals(file2, file1));
		}

		[TestCase(-1)]
		[TestCase(1)]
		[TestCase(1500)]
		public void Equals_AudioSourceWithDerivedStandardAudioNameButSlightlyDifferentDuration_ReturnsFalse(int difference)
		{
			// This is a really unlikely scenario, but it is possible (e.g., if the user trimmed one
			// of the files after making the standard audio version). Hard to know how we'd want to
			// deal with it in the statistics, but given how unlikely it is, treating them as not
			// the same file and therefore counting both is probably marginally acceptable.
			var file1 = new MediaFileInfo
			{
				DurationInMilliseconds = 5500 + difference,
				Video = new TestVideoInfo(5500 + difference)
			};
			ReflectionHelper.SetProperty(file1, "MediaFilePath", "SomePath_Source.mp4");

			var file2 = new MediaFileInfo
			{
				Audio = new TestAudioInfo(5500),
				DurationInMilliseconds = 5500,
			};
			ReflectionHelper.SetProperty(file2, "MediaFilePath", "SomePath_Source_StandardAudio.WAV");

			// In real life, Equals will not usually get called unless the hash codes are the same.
			Assert.That(m_comparer.GetHashCode(file1), Is.EqualTo(m_comparer.GetHashCode(file2)));
			Assert.IsTrue(m_comparer.Equals(file1, file2));
			Assert.IsTrue(m_comparer.Equals(file2, file1));
		}

		private class TestAudioInfo : MediaFileInfo.AudioInfo
		{
			internal TestAudioInfo(int duration)
			{
				BitRate = 42;
				BitRateMode = "freaky";
				BitsPerSample = 9;
				Channels = 1;
				CodecInformation = "MP3";
				DurationInMilliseconds = duration;
				Encoding = "WAV";
			}
		}

		private class TestVideoInfo : MediaFileInfo.VideoInfo
		{
			internal TestVideoInfo(int duration)
			{
				FramesPerSecond = 30;
				BitRate = 42;
				BitRateMode = "freaky";
				CodecInformation = "MP4";
				DurationInMilliseconds = duration;
				Encoding = "Yes";
			}
		}
	}
}
