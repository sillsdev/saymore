using System;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using SayMore.Properties;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationRecorderDlg : SegmenterDlgBase
	{
		private readonly string _normalRecordButtonText;
		private readonly ToolTip _tooltip = new ToolTip();

		/// ------------------------------------------------------------------------------------
		public OralAnnotationRecorderDlg(OralAnnotationRecorderDlgViewModel viewModel)
			: base(viewModel)
		{
			InitializeComponent();

			_normalRecordButtonText = _buttonRecordAnnotation.Text;

			toolStrip1.Items.Clear();

			_toolStripButtons.Items.AddRange(new[] { _buttonRecordAnnotation,
				_buttonListenToAnnotation, _buttonEraseAnnotation });

			Controls.Remove(toolStrip1);
			toolStrip1.Dispose();
			toolStrip1 = null;

			_buttonRecordAnnotation.MouseDown += HandleRecordAnnotationMouseDown;
			_buttonRecordAnnotation.MouseUp += HandleRecordAnnotationMouseUp;
			_buttonListenToAnnotation.Click += delegate { ViewModel.StartAnnotationPlayback(); };

			_buttonEraseAnnotation.Click += delegate
			{
				_waveControl.Stop();
				ViewModel.EraseAnnotation();
				UpdateDisplay();
			};

			_waveControl.Paint += HandleWaveControlPaint;
			_waveControl.MouseMove += HandleWaveControlMouseMove;
			_waveControl.MouseLeave += HandleWaveControlMouseLeave;
		}

		/// ------------------------------------------------------------------------------------
		private OralAnnotationRecorderDlgViewModel ViewModel
		{
			get { return _viewModel as OralAnnotationRecorderDlgViewModel; }
		}

		/// ------------------------------------------------------------------------------------
		protected override FormSettings FormSettings
		{
			get { return Settings.Default.OralAnnotationRecorderDlg; }
			set { Settings.Default.OralAnnotationRecorderDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override float ZoomPercentage
		{
			get { return Settings.Default.ZoomPercentageInAnnotationRecordingDlg; }
			set { Settings.Default.ZoomPercentageInAnnotationRecordingDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int GetHeightOfTableLayoutButtonRow()
		{
			return _buttonListenToOriginal.Height + _buttonListenToOriginal.Margin.Top +
				_buttonListenToOriginal.Margin.Bottom + _buttonListenToAnnotation.Height +
				_buttonListenToAnnotation.Margin.Top + _buttonListenToAnnotation.Margin.Bottom +
				_buttonEraseAnnotation.Height + _buttonEraseAnnotation.Margin.Top +
				_buttonEraseAnnotation.Margin.Bottom + 3;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			ViewModel.CloseAnnotationPlayer();
			ViewModel.CloseAnnotationRecorder();
			base.OnFormClosing(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void StopAllMedia()
		{
			ViewModel.StopAnnotationPlayback();
			base.StopAllMedia();
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateDisplay()
		{
			if (ViewModel.GetIsRecording())
				return;

			_buttonListenToAnnotation.Visible = ViewModel.DoesAnnotationExistForCurrentSegment;
			_buttonListenToAnnotation.Enabled = ViewModel.IsIdle && ViewModel.DoesAnnotationExistForCurrentSegment;

			_buttonRecordAnnotation.Checked = ViewModel.HaveSegmentBoundaries;
			_buttonRecordAnnotation.Visible = !ViewModel.DoesAnnotationExistForCurrentSegment;

			_buttonEraseAnnotation.Visible = ViewModel.DoesAnnotationExistForCurrentSegment;
			_buttonEraseAnnotation.Enabled = (ViewModel.IsIdle && ViewModel.DoesAnnotationExistForCurrentSegment);

			base.UpdateDisplay();
		}

		#region Methods for showing info. about whether or not a segment has a recorded annotation
		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseMove(object sender, MouseEventArgs e)
		{
			var segMouseOver = _waveControl.GetSegmentForX(e.X);

			if (segMouseOver >= 0 && !ViewModel.GetDoesSegmentHaveAnnotationFile(segMouseOver))
			{
				if (_tooltip.GetToolTip(_waveControl) == string.Empty)
				{
					_tooltip.SetToolTip(_waveControl, LocalizationManager.GetString(
						"DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.NoAnnotationToolTipMsg",
						"This segment does not have a recorded annotaton."));
				}

				return;
			}

			if (_tooltip.GetToolTip(_waveControl) != string.Empty)
				_tooltip.SetToolTip(_waveControl, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlMouseLeave(object sender, EventArgs e)
		{
			_tooltip.SetToolTip(_waveControl, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleWaveControlPaint(object sender, PaintEventArgs e)
		{
			var img = Resources.MissingAnnotation;

			int i = 0;
			foreach (var segRect in _waveControl.GetSegmentRectangles())
			{
				if (!segRect.IsEmpty && !ViewModel.GetDoesSegmentHaveAnnotationFile(i++))
				{
					var rc = new Rectangle(new Point(segRect.X + 1, segRect.Y + 1), img.Size);
					if (segRect.Contains(rc))
						e.Graphics.DrawImage(img, rc);
				}
			}
		}

		#endregion

		#region Annotation Listen/Erase/Record button handling
		/// ------------------------------------------------------------------------------------
		private void HandleRecordAnnotationMouseDown(object sender, MouseEventArgs e)
		{
			if (!ViewModel.GetIsRecording() && ViewModel.BeginAnnotationRecording())
			{
				_buttonRecordAnnotation.Text = LocalizationManager.GetString(
					"DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.RecordingButtonText.WhenRecording",
					"Recording...");
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRecordAnnotationMouseUp(object sender, MouseEventArgs e)
		{
			if (!ViewModel.GetIsRecording())
				return;

			if (ViewModel.StopAnnotationRecording())
			{
				_buttonRecordAnnotation.ForeColor = _buttonEraseAnnotation.ForeColor;
				_buttonRecordAnnotation.Text = _normalRecordButtonText;

				if (!ViewModel.IsCurrentSegmentConfirmed)
					_waveControl.SegmentBoundaries = ViewModel.SaveNewSegmentBoundary();
			}
			else
			{
				_buttonRecordAnnotation.ForeColor = Color.Red;
				_buttonRecordAnnotation.Text = LocalizationManager.GetString(
					"DialogBoxes.Transcription.CarefulSpeechAnnotationDlg.RecordingButtonText.WhenRecordingTooShort",
					"Whoops! You need to hold down the SPACE bar or mouse button while talking.");
				_buttonEraseAnnotation.PerformClick();
			}

			UpdateDisplay();
		}

		#endregion

		#region Low level keyboard handling
		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyDown(Keys key)
		{
			if (key != Keys.Space)
				return base.OnLowLevelKeyDown(key);

			HandleRecordAnnotationMouseDown(null, null);
			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyUp(Keys key)
		{
			if (key != Keys.Space)
				return base.OnLowLevelKeyUp(key);

			HandleRecordAnnotationMouseUp(null, null);
			return true;
		}

		#endregion
	}
}
