using System;
using System.IO;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore.Media;
using SayMore.Media.FFmpeg;
using SayMore.UI;
using SayMoreTests.MediaUtils;

namespace SayMoreTests.UI
{
	[TestFixture]
	public class ConvertMediaDlgViewModelTests
	{
		private TemporaryFolder _tempFolder;
		private string _testAudioFileName;
		private ConvertMediaDlgViewModel _model;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_tempFolder = new TemporaryFolder("ConvertMediaDlgViewModelTests");
			_testAudioFileName = MediaFileInfoTests.GetLongerTestAudioFile();
			var destFile = Path.Combine(_tempFolder.Path, "test.wav");
			File.Move(_testAudioFileName, destFile);
			_testAudioFileName = destFile;

			_model = new ConvertMediaDlgViewModel(_testAudioFileName,
				ConvertMediaDlg.GetFactoryExtractToMp3AudioConversionName());
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_tempFolder.Dispose();
			_tempFolder = null;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNewOutputFileName_OutputFileDoesNotExist_ReturnedNameDiffersOnlyByExtension()
		{
			Assert.AreEqual("test.mp3", _model.GetNewOutputFileName(true));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNewOutputFileName_NoSelectedConversionSpecified_ReturnsNull()
		{
			_model.SelectedConversion = null;
			Assert.IsNull(_model.GetNewOutputFileName(true));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetNewOutputFileName_OutputFileExists_ReturnsNameWithNumberedSuffix()
		{
			File.OpenWrite(_tempFolder.Combine("test.mp3")).Close();
			Assert.AreEqual("test_01.mp3", _model.GetNewOutputFileName(true));

			File.OpenWrite(_tempFolder.Combine("test_01.mp3")).Close();
			Assert.AreEqual("test_02.mp3", _model.GetNewOutputFileName(true));

			File.OpenWrite(_tempFolder.Combine("test_02.mp3")).Close();
			Assert.AreEqual("test_03.mp3", _model.GetNewOutputFileName(true));

			File.Delete(_tempFolder.Combine("test_01.mp3"));
			Assert.AreEqual("test_01.mp3", _model.GetNewOutputFileName(true));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildCommandLine_NoCommandLineSwitches_ReturnsSimpleCommandLine()
		{
			_model.SelectedConversion = FFmpegConversionInfo.CreateForTest("mp3", "switches");
			var outputFile =  _tempFolder.Combine("test.mp3");

			Assert.AreEqual("-i \"" + _testAudioFileName + "\" switches \"" + outputFile + "\"",
				_model.BuildCommandLine(outputFile));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void BuildCommandLine_CommandLineIncludesAudioBitRateReplacementMarkers_ReturnsRealRate()
		{
			_model.SelectedConversion = FFmpegConversionInfo.CreateForTest("mp3", "-ab {ab}");
			var outputFile = _tempFolder.Combine("test.mp3");

			var mediaInfo = MediaFileInfo.GetInfo(_testAudioFileName);
			Assert.AreEqual("-i \"" + _testAudioFileName + "\" -ab " + mediaInfo.Audio.BitRate + " \"" + outputFile + "\"",
				_model.BuildCommandLine(outputFile));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnCI")]
		public void BeginConversion_WhenOutputFileNotSpecified_CreatesInferredOutputFile()
		{
			_model.BeginConversion(null);
			Assert.IsTrue(File.Exists(_tempFolder.Combine("test.mp3")));

			_model.BeginConversion(null);
			Assert.IsTrue(File.Exists(_tempFolder.Combine("test_01.mp3")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnCI")]
		public void BeginConversion_WhenOutputFileIsSpecified_CreatesSpecifiedOutputFile()
		{
			_model.BeginConversion(null, _tempFolder.Combine("blah.mp3"));
			Assert.IsTrue(File.Exists(_tempFolder.Combine("blah.mp3")));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnCI")]
		public void BeginConversion_WithReportingAction_ReportingActionCalledWithCommandLine()
		{
			var goodCallBack = false;
			_model.BeginConversion((t, s) =>
			{
				if (!goodCallBack)
					goodCallBack = s.StartsWith("Command Line");
			});

			Assert.IsTrue(goodCallBack, "Callback not called with command line.");
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnCI")]
		public void BeginConversion_WithReportingAction_ReportsProgressingTime()
		{
			var currentTime = default(TimeSpan);
			var timesIncreased = true;
			_model.BeginConversion((t, s) =>
			{
				if (t < currentTime && timesIncreased)
					timesIncreased = false;
				else
					currentTime = t;
			});

			Assert.IsTrue(timesIncreased && currentTime > default(TimeSpan),
				"Times did not increase during conversion.");
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeOfProgress_NullInputAndNoPreviousTime_ReturnsDefaultTime()
		{
			Assert.AreEqual(default(TimeSpan), _model.GetTimeOfProgress(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeOfProgress_InvalidInputAndNoPreviousTime_ReturnsDefaultTime()
		{
			Assert.AreEqual(default(TimeSpan), _model.GetTimeOfProgress("blah"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeOfProgress_NullInputWhenHavePreviousTime_ReturnsPreviousTime()
		{
			_model.GetTimeOfProgress("frame= time=00:00:59.30");
			Assert.AreEqual(TimeSpan.Parse("00:00:59.30"), _model.GetTimeOfProgress(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeOfProgress_InvalidInputWhenHavePreviousTime_ReturnsPreviousTime()
		{
			_model.GetTimeOfProgress("size= time=00:00:59.30");
			Assert.AreEqual(TimeSpan.Parse("00:00:59.30"), _model.GetTimeOfProgress(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeOfProgress_ValidInputStartsWithFrame_ReturnsCorrectTime()
		{
			Assert.AreEqual(TimeSpan.Parse("00:01:22.10"), _model.GetTimeOfProgress("frame= time=00:01:22.10"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeOfProgress_ValidInputStartsWithSize_ReturnsCorrectTime()
		{
			Assert.AreEqual(TimeSpan.Parse("00:01:33.10"), _model.GetTimeOfProgress("size= time=00:01:33.10"));
		}

	}
}
