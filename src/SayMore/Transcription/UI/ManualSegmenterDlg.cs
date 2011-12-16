using System.Drawing;
using System.Windows.Forms;
using Localization;
using SayMore.Properties;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class ManualSegmenterDlg : SegmenterDlgBase
	{
		private readonly string _origAddSegBoundaryButtonText;

		/// ------------------------------------------------------------------------------------
		public ManualSegmenterDlg(SegmenterDlgBaseViewModel viewModel) : base(viewModel)
		{
			InitializeComponent();

			toolStrip1.Items.Clear();

			_toolStripButtons.Items.Add(_buttonAddSegmentBoundary);

			Controls.Remove(toolStrip1);
			toolStrip1.Dispose();
			toolStrip1 = null;

			_origAddSegBoundaryButtonText = _buttonAddSegmentBoundary.Text;
			_buttonAddSegmentBoundary.Click += delegate
			{
				if (_viewModel.IsSegmentLongEnough)
					_waveControl.SegmentBoundaries = _viewModel.SaveNewSegmentBoundary();
				else
				{
					_buttonAddSegmentBoundary.ForeColor = Color.Red;
					_buttonAddSegmentBoundary.Text = LocalizationManager.GetString(
						"DialogBoxes.Transcription.ManualSegmenterDlg._buttonAddSegmentBoundary.WhenSegmentTooShort",
						"Whoops! The segment will be too short (press ESC to reset)");
				}
			};
		}

		/// ------------------------------------------------------------------------------------
		protected override FormSettings FormSettings
		{
			get { return Settings.Default.ManualSegmenterDlg; }
			set { Settings.Default.ManualSegmenterDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override float ZoomPercentage
		{
			get { return Settings.Default.ZoomPercentageInManualSegmenterDlg; }
			set { Settings.Default.ZoomPercentageInManualSegmenterDlg = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override int GetHeightOfTableLayoutButtonRow()
		{
			return _buttonListenToOriginal.Height + _buttonListenToOriginal.Margin.Top +
				_buttonListenToOriginal.Margin.Bottom + _buttonAddSegmentBoundary.Height +
				_buttonAddSegmentBoundary.Margin.Top + _buttonAddSegmentBoundary.Margin.Bottom + 3;
		}

		/// ------------------------------------------------------------------------------------
		protected override void UpdateDisplay()
		{
			_buttonAddSegmentBoundary.ForeColor = _buttonListenToOriginal.ForeColor;
			_buttonAddSegmentBoundary.Text = _origAddSegBoundaryButtonText;
			_buttonAddSegmentBoundary.Enabled = _viewModel.HaveUnconfirmedSegmentBoundariesBeenEstablished;
			base.UpdateDisplay();
		}

		#region Low level keyboard handling
		/// ------------------------------------------------------------------------------------
		protected override bool OnLowLevelKeyUp(Keys key)
		{
			if (key != Keys.Enter || !_viewModel.IsIdle)
				return base.OnLowLevelKeyUp(key);

			_buttonAddSegmentBoundary.PerformClick();
			return true;
		}

		#endregion
	}
}
