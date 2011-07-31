using System;
using System.Drawing;
using System.Windows.Forms;
using SilTools;
using SilTools.Controls;

namespace SayMore.Transcription.UI
{
	public partial class OralAnnotationRecorder : UserControl
	{
		private readonly string _segmentCountFormatString;
		private readonly string _micLevelFormatString;
		private OralAnnotationRecorderViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		public OralAnnotationRecorder()
		{
			InitializeComponent();

			_segmentCountFormatString = _labelSegmentNumber.Text;
			_labelSegmentNumber.Font = SystemFonts.IconTitleFont;
			_micLevelFormatString = _labelMicLevel.Text;
			_labelMicLevel.Font = SystemFonts.IconTitleFont;
			_buttonPlayOriginal.Font = SystemFonts.IconTitleFont;
			_buttonRecord.Font = SystemFonts.IconTitleFont;
			_buttonPlayAnnotation.Font = SystemFonts.IconTitleFont;
			_buttonEraseAnnotation.Font = SystemFonts.IconTitleFont;

			_buttonPlayOriginal.Click += delegate { UpdateDisplay(); };
			_buttonPlayAnnotation.Click += delegate { UpdateDisplay(); };
			_buttonRecord.Click += delegate { UpdateDisplay(); };

			_trackBarSegment.ValueChanged += HandleSegmentTrackBarValueChanged;
			_trackBarMicLevel.ValueChanged += delegate { UpdateDisplay(); };
		}

		/// ------------------------------------------------------------------------------------
		public void Initialize(OralAnnotationRecorderViewModel viewModel)
		{
			_viewModel = viewModel;
			_viewModel.MicLevelChangeControl = _trackBarMicLevel;
			_viewModel.MicLevelDisplayControl = _panelMicorphoneLevel;
			_viewModel.PlaybackEnded += delegate { Invoke((Action)UpdateDisplay); };

			_buttonPlayOriginal.Initialize(" Stop Listening (press 'O')",
				_viewModel.PlayOriginalRecording, _viewModel.Stop);

			_buttonPlayAnnotation.Initialize(" Stop Listening (press 'A')",
				_viewModel.PlayAnnotation, _viewModel.Stop);

			_buttonRecord.Initialize(" Stop Recording (press 'SPACE')",
				_viewModel.BeginRecording, _viewModel.Stop);

			_trackBarSegment.Minimum = 1;
			_trackBarSegment.Maximum = _viewModel.SegmentCount;
			_trackBarSegment.Value = _viewModel.CurrentSegmentNumber + 1;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (_viewModel != null)
				_viewModel.Dispose();

			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (!_trackBarSegment.Focused)
			{
				if (keyData == Keys.Right && _trackBarSegment.Value < _trackBarSegment.Maximum)
				{
					_trackBarSegment.Value++;
					return true;
				}

				if (keyData == Keys.Left && _trackBarSegment.Value > _trackBarSegment.Minimum)
				{
					_trackBarSegment.Value--;
					return true;
				}
			}

			if (keyData == Keys.O && _buttonPlayOriginal.Enabled)
				_buttonPlayOriginal.PerformClick();
			else if (keyData == Keys.A && _buttonPlayAnnotation.Visible)
				_buttonPlayAnnotation.PerformClick();
			else if (keyData == Keys.Space)
			{
				if (_buttonRecord.Visible)
					_buttonRecord.PerformClick();
			}
			else
				return base.ProcessDialogKey(keyData);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			Utils.SetWindowRedraw(this, false);

			_trackBarSegment.Enabled = !_viewModel.IsRecording;

			_labelSegmentNumber.Text = string.Format(_segmentCountFormatString,
				_trackBarSegment.Value, _viewModel.SegmentCount);

			_labelMicLevel.Text = string.Format(_micLevelFormatString,_trackBarMicLevel.Value);

			var state = _viewModel.GetState();
			_buttonPlayOriginal.SetStateProperties(state == OralAnnotationRecorderViewModel.State.PlayingOriginal);
			_buttonPlayAnnotation.SetStateProperties(state == OralAnnotationRecorderViewModel.State.PlayingAnnotation);
			_buttonRecord.SetStateProperties(state == OralAnnotationRecorderViewModel.State.Recording);

			_buttonPlayOriginal.Enabled = (state != OralAnnotationRecorderViewModel.State.Recording);
			_buttonPlayAnnotation.Visible = _viewModel.ShouldListenToAnnotationButtonBeVisible;
			_buttonRecord.Visible = _viewModel.ShouldRecordButtonBeVisible;
			_buttonEraseAnnotation.Enabled = _viewModel.ShouldEraseAnnotationButtonBeEnabled;
			//_buttonEraseAnnotation.Visible = _viewModel.ShouldEraseAnnotationButtonBeVisible;

			Utils.SetWindowRedraw(this, true);
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
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnClick(EventArgs e)
		{
			if (HasActionStarted)
			{
				if (_stopAction != null)
					_stopAction();
			}
			else if (_startAction != null)
				_startAction();

			base.OnClick(e);
		}
	}

	#endregion
}
