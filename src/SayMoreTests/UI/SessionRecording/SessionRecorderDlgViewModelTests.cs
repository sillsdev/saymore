using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using Palaso.TestUtilities;
using Moq;
using SayMore.Model;
using SayMore.UI.SessionRecording;

namespace SayMoreTests.UI
{
	[TestFixture]
	public class SessionRecorderDlgViewModelTests
	{
		SessionRecorderDlgViewModel _model;
		TemporaryFolder _tempFolder;
		Mock<Session> _session;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void SetUp()
		{
			_model = new SessionRecorderDlgViewModel();
			_model.Recorder.BeginMonitoring();
			_tempFolder = new TemporaryFolder("SessionRecorderDlgViewModelTests");
			Assert.IsFalse(_model.IsRecording);
			Assert.IsFalse(_model.IsPlaying);
			Assert.IsFalse(_model.CanPlay);
			Assert.IsTrue(_model.CanRecordNow);

			_session = new Mock<Session>();
			_session.Setup(e => e.FolderPath).Returns(_tempFolder.Path);
			_session.Setup(e => e.Id).Returns("Tom");
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_model.Dispose();
			_model = null;

			_tempFolder.Dispose();
			_tempFolder = null;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void BeginRecording_AfterConstruction_PutsIntoRecordingState()
		{
			_model.BeginRecording();
			Assert.IsTrue(_model.IsRecording);
			Assert.IsFalse(_model.IsPlaying);
			Assert.IsFalse(_model.CanPlay);
			Assert.IsFalse(_model.CanRecordNow);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void BeginRecording_AfterStoppingPlayBack_PutsIntoRecordingState()
		{
			_model.BeginRecording();
			Wait(200);
			_model.Stop();
			Wait(220);
			_model.BeginPlayback();
			_model.Stop();
			Wait(50);
			_model.BeginRecording();
			Assert.IsTrue(_model.IsRecording);
			Assert.IsFalse(_model.IsPlaying);
			Assert.IsFalse(_model.CanPlay);
			Assert.IsFalse(_model.CanRecordNow);
		}

		/// ------------------------------------------------------------------------------------
		private static void Wait(double howLong)
		{
			var stop = DateTime.Now.AddMilliseconds(howLong);
			do Application.DoEvents();
			while (stop > DateTime.Now);
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void MoveRecordingToSessionFolder_MovesFile()
		{
			_model.BeginRecording();
			Thread.Sleep(200);
			_model.Stop();
			Assert.IsFalse(File.Exists(Path.Combine(_tempFolder.Path, "Tom.wav")));
			_model.MoveRecordingToSessionFolder(_session.Object);
			Assert.IsTrue(File.Exists(Path.Combine(_tempFolder.Path, "Tom.wav")));
		}
	}
}
