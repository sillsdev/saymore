using System.IO;
using System.Threading;
using NUnit.Framework;
using Palaso.TestUtilities;
using Moq;
using SayMore.Model;
using SayMore.Utilities.EventRecording;

namespace SayMoreTests.UI
{
	[TestFixture]
	public class EventRecorderDlgViewModelTests
	{
		EventRecorderDlgViewModel _model;
		TemporaryFolder _tempFolder;
		Mock<Event> _event;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void SetUp()
		{
			_model = new EventRecorderDlgViewModel();
			_model.Recorder.BeginMonitoring();
			_tempFolder = new TemporaryFolder("EventRecorderDlgViewModelTests");
			Assert.IsFalse(_model.IsRecording);
			Assert.IsFalse(_model.IsPlaying);
			Assert.IsFalse(_model.CanPlay);
			Assert.IsTrue(_model.CanRecordNow);

			_event = new Mock<Event>();
			_event.Setup(e => e.FolderPath).Returns(_tempFolder.Path);
			_event.Setup(e => e.Id).Returns("Tom");
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
		public void BeginRecording_AfterStopingPlayBack_PutsIntoRecordingState()
		{
			_model.BeginRecording();
			Thread.Sleep(200);
			_model.Stop();
			_model.BeginPlayback();
			_model.Stop();
			_model.BeginRecording();
			Assert.IsTrue(_model.IsRecording);
			Assert.IsFalse(_model.IsPlaying);
			Assert.IsFalse(_model.CanPlay);
			Assert.IsFalse(_model.CanRecordNow);
		}

		[Test]
		[Category("SkipOnTeamCity")]
		public void MoveRecordingToEventFolder_MovesFile()
		{
			_model.BeginRecording();
			Thread.Sleep(200);
			_model.Stop();
			Assert.IsFalse(File.Exists(Path.Combine(_tempFolder.Path, "Tom.wav")));
			_model.MoveRecordingToEventFolder(_event.Object);
			Assert.IsTrue(File.Exists(Path.Combine(_tempFolder.Path, "Tom.wav")));
		}
	}
}
