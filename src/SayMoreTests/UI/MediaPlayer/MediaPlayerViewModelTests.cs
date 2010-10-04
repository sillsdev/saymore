using System;
using System.Drawing;
using System.IO;
using System.Text;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.UI.MediaPlayer;

namespace SayMoreTests.UI.MediaPlayer
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class MediaPlayerViewModelTests
	{
		MediaPlayerViewModel _model;
		MemoryStream _stream;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_model = new MediaPlayerViewModel();
			_stream = new MemoryStream();
			_model.SetStdInForTest(new StreamWriter(_stream));
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_stream.Close();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void MakeTimeString_WithHHMMSSms_VerifyCorrect()
		{
			var ts = new TimeSpan(0, 2, 32, 54, 430);
			var seconds = float.Parse(ts.TotalSeconds.ToString());
			Assert.AreEqual("02:32:54.4", MediaPlayerViewModel.MakeTimeString(seconds));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void MakeTimeString_WithMMSSms_VerifyCorrect()
		{
			var ts = new TimeSpan(0, 0, 16, 23, 560);
			var seconds = float.Parse(ts.TotalSeconds.ToString());
			Assert.AreEqual("16:23.6", MediaPlayerViewModel.MakeTimeString(seconds));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void MakeTimeString_WithSSms_VerifyCorrect()
		{
			var ts = new TimeSpan(0, 0, 0, 47, 368);
			var seconds = float.Parse(ts.TotalSeconds.ToString());
			Assert.AreEqual("47.4", MediaPlayerViewModel.MakeTimeString(seconds));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetTimeDisplayTest()
		{
			var length = (float)(new TimeSpan(23, 59, 59).TotalSeconds);
			_model.HandlePlayerOutput("ID_LENGTH=" + length);

			var ts = new TimeSpan(0, 2, 32, 54, 430);
			var seconds = float.Parse(ts.TotalSeconds.ToString());
			Assert.AreEqual("02:32:54.4 / 23:59:59.0", _model.GetTimeDisplay(seconds));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void HandlePlayerOutput_SendEOF_GetsPlaybackEndedEvent()
		{
			bool eventCalled = false;
			_model.PlaybackEnded += ((sender, e) => eventCalled = true );
			_model.HandlePlayerOutput("EOF code:");
			Assert.IsTrue(eventCalled);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void HandlePlayerOutput_SendMediaLength_SetsProperty()
		{
			Assert.AreEqual(0f, _model.MediaFileLength);
			_model.HandlePlayerOutput("ID_LENGTH=172.8");
			Assert.AreEqual(172.8f, _model.MediaFileLength);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void HandlePlayerOutput_SendVideoFormat_SetsProperty()
		{
			Assert.IsFalse(_model.IsForVideo);
			_model.HandlePlayerOutput("ID_VIDEO_FORMAT");
			Assert.IsTrue(_model.IsForVideo);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void HandlePlayerOutput_SendPause_GetsPausedEvent()
		{
			bool eventCalled = false;
			_model.PlaybackPaused += ((sender, e) => eventCalled = true);
			_model.HandlePlayerOutput("ID_PAUSED");
			Assert.IsTrue(eventCalled);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void HandlePlayerOutput_SendVideoWidth_SetsProperty()
		{
			Assert.AreEqual(new Size(0, 0), _model.VideoPictureSize);
			_model.HandlePlayerOutput("ID_VIDEO_WIDTH=320");
			Assert.AreEqual(new Size(320, 0), _model.VideoPictureSize);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void HandlePlayerOutput_SendVideoHeight_SetsProperty()
		{
			Assert.AreEqual(new Size(0, 0), _model.VideoPictureSize);
			_model.HandlePlayerOutput("ID_VIDEO_HEIGHT=240");
			Assert.AreEqual(new Size(0, 240), _model.VideoPictureSize);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void HandlePlayerOutput_SendAllMediaInfoForVideo_GetsMediaQueuedEvent()
		{
			_model.HandlePlayerOutput("ID_VIDEO_FORMAT");
			bool eventCalled = false;
			_model.MediaQueued += ((sender, e) => eventCalled = true);
			_model.HandlePlayerOutput("ID_LENGTH=172.8");
			Assert.IsFalse(eventCalled);
			_model.HandlePlayerOutput("ID_VIDEO_WIDTH=320");
			Assert.IsFalse(eventCalled);
			_model.HandlePlayerOutput("ID_VIDEO_HEIGHT=240");
			Assert.IsTrue(eventCalled);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void HandlePlayerOutput_SendAllMediaInfoForAudio_GetsMediaQueuedEvent()
		{
			bool eventCalled = false;
			_model.MediaQueued += ((sender, e) => eventCalled = true);
			_model.HandlePlayerOutput("ID_LENGTH=172.8");
			Assert.IsTrue(eventCalled);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void HandlePlayerOutput_SendPositionChangeWhenPaused_GetsPlaybackResumedEvent()
		{
			_model.HandlePlayerOutput("ID_PAUSED");
			Assert.IsTrue(_model.IsPaused);

			bool eventCalled = false;
			_model.PlaybackResumed += ((sender, e) => eventCalled = true);
			_model.HandlePlayerOutput("A:  24.8");
			Assert.IsTrue(eventCalled);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void HandlePlayerOutput_SendPositionChange_GetsPositionChangedEvent()
		{
			bool eventCalled = false;
			_model.PlaybackPositionChanged += ((sender, e) => eventCalled = true);
			_model.HandlePlayerOutput("A:  24.8");
			Assert.IsTrue(eventCalled);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ToggleVolumeMute_Call_TogglesProperty()
		{
			Assert.IsFalse(_model.IsVolumeMuted);
			_model.ToggleVolumeMute();
			Assert.IsTrue(_model.IsVolumeMuted);
			_model.ToggleVolumeMute();
			Assert.IsFalse(_model.IsVolumeMuted);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetVolume_SetTooLow_Ignores()
		{
			var vol = _model.Volume;
			bool eventCalled = false;
			_model.VolumeChanged += ((sender, e) => eventCalled = true);
			_model.SetVolume(-1f);
			Assert.IsFalse(eventCalled);
			Assert.AreEqual(vol, _model.Volume);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetVolume_SetTooHigh_Ignores()
		{
			var vol = _model.Volume;
			bool eventCalled = false;
			_model.VolumeChanged += ((sender, e) => eventCalled = true);
			_model.SetVolume(101f);
			Assert.IsFalse(eventCalled);
			Assert.AreEqual(vol, _model.Volume);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetVolume_ChangeValue_GetsEvent()
		{
			var vol = _model.Volume;
			bool eventCalled = false;
			_model.VolumeChanged += ((sender, e) => eventCalled = true);
			_model.SetVolume(vol + 10);
			Assert.IsTrue(eventCalled);
			Assert.AreEqual(vol + 10, _model.Volume);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetVolume_ChangeValue_ChangesValue()
		{
			var vol = _model.Volume;
			_model.SetVolume(vol + 10);
			Assert.AreEqual(vol + 10, _model.Volume);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetVolume_ChangeValue_WritesToInputStream()
		{
			var vol = _model.Volume + 10;
			_model.SetVolume(vol);

			var expected = string.Format("volume {0} 1 \r\n", vol);
			Assert.AreEqual(expected, Encoding.ASCII.GetString(_stream.ToArray()));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Pause_Call_WritesToInputStream()
		{
			_model.Pause();
			VerifyStreamContent("pause \r\n");
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Stop_Call_WritesToInputStream()
		{
			_model.Stop();
			VerifyStreamContent("stop \r\n");
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Seek_Call_WritesToInputStream()
		{
			_model.Seek(32.6f);
			VerifyStreamContent("seek 32.6 2\r\n");
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Play_CallWhenPaused_WritesToInputStream()
		{
			_model.HandlePlayerOutput("ID_PAUSED");
			_model.Play();
			VerifyStreamContent(string.Format("pause \r\nvolume {0} 1 \r\n", _model.Volume));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Play_CallWhenNotPaused_DoesNotWriteToInputStream()
		{
			_model.Play();
			VerifyStreamContent(string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadFile_CallPlayImmediately_WritesToInputStream()
		{
			var vol = _model.Volume;

			using (var file = new TempFile())
			{
				_model.LoadFile(file.Path, true);
				var path = file.Path.Replace('\\', '/');
				VerifyStreamContent(string.Format(
					"loadfile \"{0}\" \r\nvolume 0 \r\nvolume {1} 1 \r\n", path, vol));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void LoadFile_CallDoNotPlayImmediately_WritesToInputStream()
		{
			using (var file = new TempFile())
			{
				_model.LoadFile(file.Path, false);
				var path = file.Path.Replace('\\', '/');
				VerifyStreamContent(string.Format("loadfile \"{0}\" \r\nvolume 0 \r\npause \r\n", path));
			}
		}

		/// ------------------------------------------------------------------------------------
		private void VerifyStreamContent(string expected)
		{
			Assert.AreEqual(expected, Encoding.ASCII.GetString(_stream.ToArray()));
		}
	}
}
