using System;
using System.Drawing;
using System.Windows.Forms;

namespace SayMore.UI.Overview.Statistics
{
	public partial class StatisticsView : UserControl
	{
		private readonly StatisticsViewModel _model;

		/// ------------------------------------------------------------------------------------
		public StatisticsView(StatisticsViewModel model)
		{
			_model = model;
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_model.UIUpdateNeeded = false;
			_webBrowser.DocumentText = _model.HTMLString;
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePrintButtonClicked(object sender, EventArgs e)
		{
			// Show print dialog
		}

		/// ------------------------------------------------------------------------------------
		private void HandleSaveButtonClicked(object sender, EventArgs e)
		{
			// Show save file dialog
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRefreshButtonClicked(object sender, EventArgs e)
		{
			_model.Refresh();
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTimerTick(object sender, EventArgs e)
		{
			_labelStatus.Text = _model.Status;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRefreshTimerTick(object sender, EventArgs e)
		{
			if (_model.UIUpdateNeeded)
				UpdateDisplay();
		}
	}
}
