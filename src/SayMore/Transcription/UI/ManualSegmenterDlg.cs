using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using Palaso.Reporting;
using SayMore.AudioUtils;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI.LowLevelControls;
using SayMore.UI.MediaPlayer;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class ManualSegmenterDlg : MonitorKeyPressDlg
	{
		private readonly string _pathToAnnotationsFolder;
		private readonly string _normalRecordButtonText;
		private readonly string _mediaFileName;
		private bool _haveSegmentBoundaries;
		private int _currentSegment;
		private TimeSpan _playbackStartPosition;
		private TimeSpan _playbackEndPosition;
		private WaveStream _origWaveStream;
		private AudioPlayer _annotationPlayer;
		private AudioRecorder _annotationRecorder;
		private OralAnnotationType _annotationType;

		private bool _isIdle = true;

		private List<KeyValuePair<TimeSpan, TimeSpan>> _segments =
			new List<KeyValuePair<TimeSpan, TimeSpan>>();

		/// ------------------------------------------------------------------------------------
		public ManualSegmenterDlg()
		{
			InitializeComponent();

			DoubleBuffered = true;
			_normalRecordButtonText = _buttonRecordAnnotation.Text;
			_comboBoxZoom.Text = _comboBoxZoom.Items[0] as string;
			_comboBoxZoom.Font = SystemFonts.IconTitleFont;
			_labelZoom.Font = SystemFonts.IconTitleFont;
			_labelTimeDisplay.Font = SystemFonts.IconTitleFont;
			_labelRecordingType.Font = new Font(SystemFonts.IconTitleFont.FontFamily,
				11f, FontStyle.Bold, GraphicsUnit.Point);

			//_labelRecordingFormat.Font = SystemFonts.IconTitleFont;

			//var bestFormat = WaveFileUtils.GetDefaultWaveFormat(1);

			//_labelRecordingFormat.Text = string.Format(_labelRecordingFormat.Text,
			//    bestFormat.BitsPerSample, bestFormat.SampleRate);

			if (Settings.Default.OralAnnotationDlg == null)
			{
				StartPosition = FormStartPosition.CenterScreen;
				Settings.Default.OralAnnotationDlg = FormSettings.Create(this);
			}
		}

		/// ------------------------------------------------------------------------------------
		public ManualSegmenterDlg(ComponentFile file) : this()
		{
			_mediaFileName = file.PathToAnnotatedFile;
			_playbackStartPosition = TimeSpan.Zero;
			_playbackEndPosition = TimeSpan.Zero;
			_origWaveStream = new WaveFileReader(_mediaFileName);
			InitializeWaveControl();
			InitializeSegments(file);

			_pathToAnnotationsFolder = _mediaFileName + Settings.Default.OralAnnotationsFolderAffix;
			SetCurrentSegmentNumber(0);

			_labelTimeDisplay.Text =
				MediaPlayerViewModel.GetTimeDisplay(0f, (float)_origWaveStream.TotalTime.TotalSeconds);
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeWaveControl()
		{
			_waveControl.Initialize(_origWaveStream);
			_waveControl.ShadePlaybackAreaDuringPlayback = true;
			_waveControl.PlaybackStarted = () =>
			{
				_isIdle = false;
				UpdateDisplay();

				_waveControl.PlaybackUpdate = (position, totalTime) =>
				{
					_labelTimeDisplay.Text = MediaPlayerViewModel.GetTimeDisplay(
						(float) position.TotalSeconds, (float) totalTime.TotalSeconds);
				};
			};

			_waveControl.Stopped = (start, end) =>
			{
				_isIdle = true;

				if (!_haveSegmentBoundaries)
				{
					_haveSegmentBoundaries = true;
					_playbackEndPosition = end;
					_waveControl.ShadePlaybackAreaDuringPlayback = false;
				}

				_waveControl.PlaybackUpdate = null;
				_waveControl.SetCursor(_playbackEndPosition);
				UpdateDisplay();
			};
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeSegments(ComponentFile file)
		{
			if (file.GetAnnotationFile() == null)
				return;

			var toTier = file.GetAnnotationFile().Tiers.FirstOrDefault(t => t is TimeOrderTier);
			if (toTier == null)
				return;

			foreach (var segment in toTier.GetAllSegments())
			{
				var toseg = segment as ITimeOrderSegment;
				var kvp = new KeyValuePair<TimeSpan, TimeSpan>(TimeSpan.FromSeconds(toseg.Start), TimeSpan.FromSeconds(toseg.Stop));
				_segments.Add(kvp);
			}

			_waveControl.SegmentBoundaries = _segments.Select(s => s.Value);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		private string ProgramAreaForUsageReporting
		{
			get { return "ManualAnnotations/Oral/" + _annotationType; }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsRecordingTooShort
		{
			get { return (_annotationRecorder.RecordedTime <= TimeSpan.FromMilliseconds(500)); }
		}

		/// ------------------------------------------------------------------------------------
		public bool DoesAnnotationExist
		{
			get { return (File.Exists(GetPathToCurrentAnnotationFile())); }
		}

		/// ------------------------------------------------------------------------------------
		public bool SegmentBoundariesChanged { get; private set; }

		/// ------------------------------------------------------------------------------------
		public bool AnnotationRecordingsChanged { get; private set; }

		#endregion

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_waveControl.Stop();
			CloseAnnotationPlayer();
			CloseAnnotationRecorder();

			if (_origWaveStream != null)
			{
				_origWaveStream.Close();
				_origWaveStream.Dispose();
				_origWaveStream = null;
			}

			DialogResult = (_segments.Count > 0 && (SegmentBoundariesChanged ||
				AnnotationRecordingsChanged) ? DialogResult.OK : DialogResult.Cancel);

			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCloseClick(object sender, EventArgs e)
		{
			Close();
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetSegments()
		{
			return _segments.Select(b => b.Value.TotalSeconds.ToString());
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnShown(EventArgs e)
		{
			Settings.Default.OralAnnotationDlg.InitializeForm(this);
			base.OnShown(e);
		}
		/// ------------------------------------------------------------------------------------
		public string GetPathToAnnotationFileForSegment(int segmentNumber)
		{
			if (segmentNumber == _segments.Count)
				return string.Empty;

			var segment = _segments[segmentNumber];

			return GetPathToAnnotationFileForSegment(segment.Key, segment.Value);
		}

		/// ------------------------------------------------------------------------------------
		public string GetPathToAnnotationFileForSegment(TimeSpan start, TimeSpan end)
		{
			var affix = (_annotationType == OralAnnotationType.Careful ?
				Settings.Default.OralAnnotationCarefulSegmentFileAffix :
				Settings.Default.OralAnnotationTranslationSegmentFileAffix);

			return Path.Combine(_pathToAnnotationsFolder,
				string.Format(Settings.Default.OralAnnotationSegmentFileFormat,
				(float)start.TotalSeconds, (float)end.TotalSeconds, affix));
		}

		/// ------------------------------------------------------------------------------------
		public string GetPathToCurrentAnnotationFile()
		{
			return GetPathToAnnotationFileForSegment(_currentSegment);
		}

		/// ------------------------------------------------------------------------------------
		public void SetCurrentSegmentNumber(int segmentNumber)
		{
			_waveControl.Stop();
			CloseAnnotationRecorder();

			if (_annotationPlayer != null && _annotationPlayer.PlaybackState == PlaybackState.Playing)
				_annotationPlayer.Stop();

			_currentSegment = segmentNumber;
			_waveControl.ShadePlaybackAreaDuringPlayback = (segmentNumber == _segments.Count);
			_haveSegmentBoundaries = (segmentNumber < _segments.Count);
			_annotationRecorder = new AudioRecorder(WaveFileUtils.GetDefaultWaveFormat(1));
			_annotationRecorder.RecordingStarted += delegate
			{
				_isIdle = false;
				UpdateDisplay();
			};

			_annotationRecorder.Stopped += delegate
			{
				_isIdle = true;
				UpdateDisplay();
			};

			//_annotationRecorder.SetRecordingLevelChangeControl(MicLevelChangeControl);
			//_annotationRecorder.SetRecordingLevelDisplayControl(MicLevelDisplayControl);


			if (segmentNumber == _segments.Count)
			{
				_waveControl.ClearSelectedRegion();
				_playbackEndPosition = TimeSpan.Zero;
				_playbackStartPosition = (_segments.Count == 0 ?
					TimeSpan.Zero : _segments[segmentNumber - 1].Value);
			}
			else
			{
				_playbackStartPosition = _segments[segmentNumber].Key;
				_playbackEndPosition = _segments[segmentNumber].Value;
				_waveControl.SetSelectionTimes(_playbackStartPosition, _playbackEndPosition);
			}

			_waveControl.SetCursor(_playbackStartPosition);
			InitializeAnnotationPlayer();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeAnnotationPlayer()
		{
			CloseAnnotationPlayer();

			var filename = GetPathToCurrentAnnotationFile();
			if (File.Exists(filename))
			{
				var fi = new FileInfo(filename);
				if (fi.Length == 0)
				{
					fi.Delete();
					return;
				}

				_annotationPlayer = new AudioPlayer();
				_annotationPlayer.LoadFile(filename);
				_annotationPlayer.PlaybackStarted += delegate
				{
					_isIdle = false;
					UpdateDisplay();
				};

				_annotationPlayer.Stopped += delegate
				{
					_isIdle = true;
					UpdateDisplay();
				};
			}
		}

		/// ------------------------------------------------------------------------------------
		private void CloseAnnotationPlayer()
		{
			if (_annotationPlayer != null)
				_annotationPlayer.Dispose();

			_annotationPlayer = null;
		}

		/// ------------------------------------------------------------------------------------
		private void CloseAnnotationRecorder()
		{
			if (_annotationRecorder != null)
				_annotationRecorder.Dispose();

			_annotationRecorder = null;
		}

		#region Low level keyboard handling
		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyDown(Keys key)
		{
			if (key == Keys.ControlKey)
			{
				HandleListenToOriginalMouseDown(null, null);
				return true;
			}

			if (!_haveSegmentBoundaries || _waveControl.IsPlaying)
				return false;

			switch (key)
			{
				case Keys.Right:
					AdjustSegmentEndBoundary(Settings.Default.MillisecondsToAdvanceInManualSegmentation);
					return true;

				case Keys.Left:
					AdjustSegmentEndBoundary(-Settings.Default.MillisecondsToBackupInManualSegmentation);
					return true;

				case Keys.Return:
					MarkSegmentBoundaries();
					return true;

				case Keys.Escape:
					SetCurrentSegmentNumber(_segments.Count);
					return true;

				case Keys.Space:
					HandleRecordAnnotationMouseDown(null, null);
					return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyUp(Keys key)
		{
			if (key == Keys.ControlKey)
			{
				_waveControl.Stop();
				return true;
			}

			if (key == Keys.Right || key == Keys.Left)
			{
				ReplaySegmentPortionAfterAdjustingEndBoundary();
				return true;
			}

			if (key == Keys.Space)
			{
				HandleRecordAnnotationMouseUp(null, null);
				return true;
			}

			return false;
		}

		#endregion

		#region Methods for adjusting/saving/playing within segment boundaries
		/// ------------------------------------------------------------------------------------
		private void MarkSegmentBoundaries()
		{
			if (_currentSegment == _segments.Count)
			{
				var kvp = new KeyValuePair<TimeSpan, TimeSpan>(_playbackStartPosition, _playbackEndPosition);
				_segments.Add(kvp);
				SegmentBoundariesChanged = true;
			}

			_waveControl.SegmentBoundaries = _segments.Select(s => s.Value);
			SetCurrentSegmentNumber(_segments.Count);
		}

		/// ------------------------------------------------------------------------------------
		private void AdjustSegmentEndBoundary(int milliseconds)
		{
			var timeAdjustment = TimeSpan.FromMilliseconds(Math.Abs(milliseconds));
			var newEndPosition = _playbackEndPosition + (milliseconds < 0 ? -timeAdjustment : timeAdjustment);

			if (newEndPosition <= _playbackStartPosition)
				return;

			_playbackEndPosition = (newEndPosition >= _origWaveStream.TotalTime ?
				_origWaveStream.TotalTime : newEndPosition);

			_waveControl.SetSelectionTimes(_playbackStartPosition, _playbackEndPosition);
		}

		/// ------------------------------------------------------------------------------------
		private void ReplaySegmentPortionAfterAdjustingEndBoundary()
		{
			if (!_isIdle || !_haveSegmentBoundaries)
				return;

			var tempPlaybackStartPosition = _playbackEndPosition -
				TimeSpan.FromMilliseconds(Settings.Default.MillisecondsToRePlayAfterAdjustingManualSegmentBoundary);

			if (tempPlaybackStartPosition < _playbackStartPosition)
				tempPlaybackStartPosition = _playbackStartPosition;

			_waveControl.Play(tempPlaybackStartPosition, _playbackEndPosition);
		}

		#endregion

		#region Listen/Erase/Record button handling
		/// ------------------------------------------------------------------------------------
		private void HandleListenToOriginalMouseDown(object sender, MouseEventArgs e)
		{
			if (_isIdle)
				_waveControl.Play(_playbackStartPosition, _playbackEndPosition);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleListenToOriginalMouseUp(object sender, MouseEventArgs e)
		{
			_waveControl.Stop();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordAnnotationMouseDown(object sender, MouseEventArgs e)
		{
			if (!_isIdle || DoesAnnotationExist || !_haveSegmentBoundaries)
				return;

			_buttonRecordAnnotation.Text = "Recording...";
			var path = GetPathToAnnotationFileForSegment(_playbackStartPosition, _playbackEndPosition);
			_annotationRecorder.BeginRecording(path);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordAnnotationMouseUp(object sender, MouseEventArgs e)
		{
			if (!_buttonRecordAnnotation.Enabled)
				return;

			_annotationRecorder.Stop();

			if (!IsRecordingTooShort)
			{
				_buttonRecordAnnotation.ForeColor = _buttonEraseAnnotation.ForeColor;
				_buttonRecordAnnotation.Text = _normalRecordButtonText;
				MarkSegmentBoundaries();
				AnnotationRecordingsChanged = true;
			}
			else
			{
				_buttonRecordAnnotation.ForeColor = Color.Red;
				_buttonRecordAnnotation.Text = "Whoops. You need to hold down the SPACE bar or mouse button while talking.";
				HandleEraseAnnotationClick(null, null);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackAnnotationClick(object sender, EventArgs e)
		{
			if (_isIdle && DoesAnnotationExist)
			{
				_annotationPlayer.Play();
				UsageReporter.SendNavigationNotice(ProgramAreaForUsageReporting + "/PlayAnnotation");
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleEraseAnnotationClick(object sender, EventArgs e)
		{
			_waveControl.Stop();
			CloseAnnotationPlayer();
			var path = GetPathToCurrentAnnotationFile();
			ComponentFile.WaitForFileRelease(path);

			try
			{
				if (File.Exists(path))
					File.Delete(path);

				InitializeAnnotationPlayer();
				UsageReporter.SendNavigationNotice(ProgramAreaForUsageReporting + "/EraseAnnotation");
			}
			catch (Exception error)
			{
				ErrorReport.NotifyUserOfProblem(error, "Could not remove that annotation. If this problem persists, try restarting your computer.");
			}

			UpdateDisplay();
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseClick(object sender, MouseEventArgs e)
		{
			if (_segments.Count == 0)
				return;

			var time = _waveControl.GetTimeFromX(e.X);

			for (int i = 0; i < _segments.Count; i++)
			{
				if (time >= _segments[i].Key && time <= _segments[i].Value)
				{
					SetCurrentSegmentNumber(i);
					return;
				}
			}

			SetCurrentSegmentNumber(_segments.Count);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_buttonListenToOriginal.Checked = _waveControl.IsPlaying || !_haveSegmentBoundaries;

			_buttonRecordAnnotation.Checked = _haveSegmentBoundaries;
			_buttonRecordAnnotation.Visible = (!DoesAnnotationExist || GetIsRecording());
			_buttonRecordAnnotation.Enabled = _haveSegmentBoundaries && (_isIdle || GetIsRecording());

			_buttonListenToAnnotation.Enabled = _isIdle && DoesAnnotationExist;
			_buttonListenToAnnotation.Visible = (DoesAnnotationExist && !GetIsRecording());

			_buttonEraseAnnotation.Enabled = _isIdle && DoesAnnotationExist;
			_buttonEraseAnnotation.Visible = (DoesAnnotationExist && !GetIsRecording());

			_labelTimeDisplay.Text = MediaPlayerViewModel.GetTimeDisplay(
				(float)_waveControl.GetCursorTime().TotalSeconds,
				(float)_origWaveStream.TotalTime.TotalSeconds);
		}

		/// ------------------------------------------------------------------------------------
		private bool GetIsRecording()
		{
			return (_annotationRecorder != null &&
				_annotationRecorder.RecordingState == RecordingState.Recording);
		}

		#region Methods for handling zoom
		/// ------------------------------------------------------------------------------------
		private void HandleZoomComboValidating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SetZoom();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleZoomSelectedIndexChanged(object sender, EventArgs e)
		{
			SetZoom();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleZoomKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
				SetZoom();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void SetZoom()
		{
			var text = _comboBoxZoom.Text.Replace("%", string.Empty).Trim();
			float newValue;
			if (float.TryParse(text, out newValue))
				_waveControl.ZoomPercentage = newValue;

			_comboBoxZoom.Text = string.Format("{0}%", _waveControl.ZoomPercentage);
		}

		#endregion
	}
}
