using System.IO;
using System.Linq;
using Palaso.Media;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.UI.MediaPlayer;

namespace SayMore.Transcription.UI
{
	public class OralAnnotationRecorderViewModel
	{
		public enum State
		{
			Idle,
			PlayingOriginal,
			PlayingAnnotation,
			Recording
		}

		public event MediaPlayerViewModel.PlaybackEndedEventHandler PlaybackEnded;

		public ISimpleAudioSession Recorder { get; private set; }
		public int CurrentSegmentNumber { get; private set; }

		private readonly MediaPlayerViewModel _origPlayerViewModel;
		private MediaPlayerViewModel _annotationPlayerViewModel;
		private readonly ITimeOrderSegment[] _segments;
		private readonly string _pathToAnnotationsFolder;
		public string _annotationFileAffix = "_Careful.wav";
		public string _originalRecordingPath;

		/// ------------------------------------------------------------------------------------
		public OralAnnotationRecorderViewModel(TimeOrderTier tier)
		{
			_origPlayerViewModel = new MediaPlayerViewModel();

			_origPlayerViewModel.PlaybackEnded += delegate(object sender, bool EndedBecauseEOF)
			{
				if (PlaybackEnded != null)
					PlaybackEnded(_origPlayerViewModel.MediaFile, EndedBecauseEOF);
			};

			_originalRecordingPath = tier.MediaFileName;
			_segments = tier.GetAllSegments().Cast<ITimeOrderSegment>().ToArray();
			_pathToAnnotationsFolder = tier.MediaFileName + "_Annotations";
			SetCurrentSegmentNumber(0);
		}

		/// ------------------------------------------------------------------------------------
		public int SegmentCount
		{
			get { return _segments.Length; }
		}

		/// ------------------------------------------------------------------------------------
		public string GetPathToCurrentAnnotationFile()
		{
			var segment = _segments[CurrentSegmentNumber];

			return Path.Combine(_pathToAnnotationsFolder,
				segment.Start + "_to_" + segment.Stop + _annotationFileAffix);
		}

		/// ------------------------------------------------------------------------------------
		public bool SetCurrentSegmentNumber(int segmentNumber)
		{
			Stop();
			bool incremented = (CurrentSegmentNumber < segmentNumber);
			CurrentSegmentNumber = segmentNumber;
			Recorder = AudioFactory.AudioSession(GetPathToCurrentAnnotationFile());
			var segment = _segments[CurrentSegmentNumber];
			_origPlayerViewModel.LoadFile(_originalRecordingPath, segment.Start, segment.GetLength());
			InitializeAnnotationPlayerModel();

			return incremented;
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeAnnotationPlayerModel()
		{
			var filename = GetPathToCurrentAnnotationFile();
			if (!File.Exists(filename))
				_annotationPlayerViewModel = null;
			else
			{
				_annotationPlayerViewModel = new MediaPlayerViewModel();
				_annotationPlayerViewModel.LoadFile(filename);
				_annotationPlayerViewModel.PlaybackEnded += delegate(object sender, bool EndedBecauseEOF)
				{
					if (PlaybackEnded != null)
						PlaybackEnded(_annotationPlayerViewModel.MediaFile, EndedBecauseEOF);
				};
			}
		}

		/// ------------------------------------------------------------------------------------
		public State GetState()
		{
			if (Recorder != null && Recorder.IsRecording)
				return State.Recording;

			if (_origPlayerViewModel.HasPlaybackStarted)
				return State.PlayingOriginal;

			if (_annotationPlayerViewModel != null && _annotationPlayerViewModel.HasPlaybackStarted)
				return State.PlayingAnnotation;

			return State.Idle;
		}

		/// ------------------------------------------------------------------------------------
		public void EraseAnnotation()
		{
			Stop();
			var filename = GetPathToCurrentAnnotationFile();
			ComponentFile.WaitForFileRelease(filename);
			File.Delete(filename);
			InitializeAnnotationPlayerModel();
		}

		/// ------------------------------------------------------------------------------------
		public void BeginRecording()
		{
			Stop();
			Recorder.StartRecording();
		}

		/// ------------------------------------------------------------------------------------
		public void PlayAnnotation()
		{
			Stop();
			_annotationPlayerViewModel.Play();
		}

		/// ------------------------------------------------------------------------------------
		public void PlayOriginalRecording()
		{
			Stop();
			_origPlayerViewModel.Play();
		}

		/// ------------------------------------------------------------------------------------
		public void Stop()
		{
			if (Recorder != null && Recorder.IsRecording)
				SaveRecoding();

			if (_annotationPlayerViewModel != null && _annotationPlayerViewModel.HasPlaybackStarted)
				_annotationPlayerViewModel.Stop();

			if (_origPlayerViewModel != null && _origPlayerViewModel.HasPlaybackStarted)
				_origPlayerViewModel.Stop();
		}

		/// ------------------------------------------------------------------------------------
		private void SaveRecoding()
		{
			if (!Directory.Exists(_pathToAnnotationsFolder))
				Directory.CreateDirectory(_pathToAnnotationsFolder);

			Recorder.StopRecordingAndSaveAsWav();
			InitializeAnnotationPlayerModel();
		}

		#region Properties for enabling, showing and hiding player buttons.
		/// ------------------------------------------------------------------------------------
		public bool IsRecording
		{
			get { return Recorder.IsRecording; }
		}

		/// ------------------------------------------------------------------------------------
		public bool ShouldRecordButtonBeVisible
		{
			get { return !File.Exists(GetPathToCurrentAnnotationFile()); }
		}

		/// ------------------------------------------------------------------------------------
		public bool ShouldStopButtonBeVisible
		{
			get
			{
				return Recorder.CanStop || Recorder.IsRecording ||
					_origPlayerViewModel.HasPlaybackStarted ||
					(_annotationPlayerViewModel != null &&_annotationPlayerViewModel.HasPlaybackStarted);
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool ShouldListenToAnnotationButtonBeVisible
		{
			get { return File.Exists(GetPathToCurrentAnnotationFile()); }
		}

		///// ------------------------------------------------------------------------------------
		//public bool ShouldEraseAnnotationButtonBeVisible
		//{
		//    get { return File.Exists(GetPathToCurrentAnnotationFile()); }
		//}

		/// ------------------------------------------------------------------------------------
		public bool ShouldEraseAnnotationButtonBeEnabled
		{
			get
			{
				return File.Exists(GetPathToCurrentAnnotationFile()) &&
					!ShouldStopButtonBeVisible;
			}
		}

		#endregion
	}
}
