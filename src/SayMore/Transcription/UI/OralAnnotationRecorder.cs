using System;
using System.Drawing;
using System.Windows.Forms;
using SilTools;
using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	public partial class OralAnnotationRecorder : UserControl, IMessageFilter
	{
		private const int WM_KEYDOWN = 0x100;
		private const int WM_KEYUP = 0x101;

		private readonly string _segmentCountFormatString;
		//private readonly string _micLevelFormatString;
		private OralAnnotationRecorderViewModel _viewModel;
		private bool _recordingButtonDown;

		/// ------------------------------------------------------------------------------------
		public OralAnnotationRecorder()
		{
			InitializeComponent();

			_segmentCountFormatString = _labelSegmentNumber.Text;
			_labelSegmentNumber.Font = SystemFonts.IconTitleFont;
			//_micLevelFormatString = _labelMicLevel.Text;
			//_labelMicLevel.Font = SystemFonts.IconTitleFont;
			_buttonPlayOriginal.Font = SystemFonts.IconTitleFont;
			_buttonRecord.Font = SystemFonts.IconTitleFont;
			_buttonPlayAnnotation.Font = SystemFonts.IconTitleFont;
			_buttonEraseAnnotation.Font = SystemFonts.IconTitleFont;

			_buttonPlayOriginal.Click += delegate { UpdateDisplay(); };
			_buttonPlayAnnotation.Click += delegate { UpdateDisplay(); };
			_buttonRecord.Click += delegate { UpdateDisplay(); };

			_trackBarSegment.ValueChanged += HandleSegmentTrackBarValueChanged;
			_trackBarMicLevel.ValueChanged += delegate { UpdateDisplay(); };

			Application.AddMessageFilter(this);
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(OralAnnotationRecorderViewModel viewModel)
		{
			_viewModel = viewModel;
			_viewModel.MicLevelChangeControl = _trackBarMicLevel;
			_viewModel.MicLevelDisplayControl = _panelMicorphoneLevel;
			_viewModel.PlaybackEnded += HandlePlaybackEnded;

			_buttonPlayOriginal.Initialize(" Playing... (press 'O' to stop)",
				_viewModel.PlayOriginalRecording, _viewModel.Stop);

			_buttonPlayAnnotation.Initialize(" Playing... (press 'A' to stop)",
				_viewModel.PlayAnnotation, _viewModel.Stop);

			_buttonRecord.Initialize(" Recording... (release SPACE to stop)",
				_viewModel.BeginRecording, HandleRecordingStopped);

			_trackBarSegment.Minimum = 1;
			_trackBarSegment.Maximum = _viewModel.SegmentCount;
			_trackBarSegment.Value = _viewModel.CurrentSegmentNumber + 1;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			Application.RemoveMessageFilter(this);

			if (_viewModel != null)
			{
				_viewModel.Stop();
				_viewModel.PlaybackEnded -= HandlePlaybackEnded;
				_viewModel.Dispose();
			}

			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg != WM_KEYDOWN && m.Msg != WM_KEYUP)
				return false;

			if (m.Msg == WM_KEYUP && (Keys)m.WParam != Keys.Space)
				return false;

			switch ((Keys)m.WParam)
			{
				case Keys.Right: MoveToNextSegment(); break;

				case Keys.Left:
					if (_trackBarSegment.Value > _trackBarSegment.Minimum)
						_trackBarSegment.Value--;
					break;

				case Keys.O:
					if (_buttonPlayOriginal.Enabled)
						_buttonPlayOriginal.PerformClick();
					break;

				case Keys.A:
					if (_buttonPlayAnnotation.Visible)
						_buttonPlayAnnotation.PerformClick();
					break;

				case Keys.Space:
					if (_buttonRecord.Visible)
					{
						if (m.Msg == WM_KEYDOWN && !_recordingButtonDown)
						{
							_recordingButtonDown = true;
							_buttonRecord.InvokeStartAction();
							UpdateDisplay();
						}
						else if (m.Msg == WM_KEYUP)
						{
							_recordingButtonDown = false;
							_buttonRecord.InvokeStopAction();
							UpdateDisplay();
						}
					}
				break;

				case Keys.Enter:
					break;

				default:
					return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackEnded(object sender, EventArgs e)
		{
			if (!IsDisposed)
				Invoke((Action)UpdateDisplay);
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			Utils.SetWindowRedraw(this, false);

			_trackBarSegment.Enabled = !_viewModel.IsRecording;

			_labelSegmentNumber.Text = string.Format(_segmentCountFormatString,
				_trackBarSegment.Value, _viewModel.SegmentCount);

			//_labelMicLevel.Text = string.Format(_micLevelFormatString,_trackBarMicLevel.Value);

			var state = _viewModel.GetState();
			_buttonPlayOriginal.SetStateProperties(state == OralAnnotationRecorderViewModel.State.PlayingOriginal);
			_buttonPlayAnnotation.SetStateProperties(state == OralAnnotationRecorderViewModel.State.PlayingAnnotation);
			_buttonRecord.SetStateProperties(state == OralAnnotationRecorderViewModel.State.Recording);

			_buttonPlayOriginal.Enabled = (state != OralAnnotationRecorderViewModel.State.Recording);
			_buttonPlayAnnotation.Visible = _viewModel.ShouldListenToAnnotationButtonBeVisible;
			_buttonRecord.Visible = _viewModel.ShouldRecordButtonBeVisible;
			//_buttonEraseAnnotation.Enabled = _viewModel.ShouldEraseAnnotationButtonBeEnabled;
			_buttonEraseAnnotation.Visible = _viewModel.ShouldEraseAnnotationButtonBeVisible;

			Utils.SetWindowRedraw(this, true);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordingStopped()
		{
			_viewModel.Stop();
			UpdateDisplay();
			MoveToNextSegment();
		}

		/// ------------------------------------------------------------------------------------
		private void MoveToNextSegment()
		{
			if (_trackBarSegment.Value < _trackBarSegment.Maximum)
				_trackBarSegment.Value++;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleEraseButtonClick(object sender, EventArgs e)
		{
			_viewModel.EraseAnnotation();
			UpdateDisplay();
			_buttonPlayOriginal.Focus();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSegmentTrackBarValueChanged(object sender, EventArgs e)
		{
			if (_buttonPlayOriginal.HasActionStarted)
				_buttonPlayOriginal.PerformClick();
			else if (_buttonPlayAnnotation.HasActionStarted)
				_buttonPlayAnnotation.PerformClick();

			if (_viewModel.SetCurrentSegmentNumber(_trackBarSegment.Value - 1))
				_buttonPlayOriginal.PerformClick();
			else
				UpdateDisplay();
		}
	}

	#region StartStopButton class
	/// ----------------------------------------------------------------------------------------
	public class StartStopButton : NicerButton
	{
		private Image _startImage;
		private string _startText;
		private string _stopText;
		private Action _startAction;
		private Action _stopAction;

		public bool HasActionStarted { get; private set; }

		/// ------------------------------------------------------------------------------------
		public void Initialize(string stopText, Action startAction, Action stopAction)
		{
			ShowFocusRectangle = false;

			_startAction = startAction;
			_stopAction = stopAction;
			_stopText = stopText;
			_startText = Text;
			_startImage = Image;
		}

		/// ------------------------------------------------------------------------------------
		public void SetStateProperties(bool setStopProps)
		{
			Text = (setStopProps ? _stopText : _startText);
			Image = (setStopProps ? Properties.Resources.RecordStop : _startImage);
			HasActionStarted = setStopProps;
			BackColor = (HasActionStarted ? Color.CornflowerBlue : Color.Transparent);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnClick(EventArgs e)
		{
			if (HasActionStarted)
				InvokeStopAction();
			else
				InvokeStartAction();

			base.OnClick(e);
		}

		/// ------------------------------------------------------------------------------------
		public void InvokeStartAction()
		{
			if (_startAction != null)
				_startAction();
		}

		/// ------------------------------------------------------------------------------------
		public void InvokeStopAction()
		{
			if (HasActionStarted && _stopAction != null)
				_stopAction();
		}
	}

	#endregion
}
