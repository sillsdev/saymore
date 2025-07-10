using System;
using System.Windows.Forms;
using SIL.Windows.Forms.Reporting;

namespace SayMore.UI.LowLevelControls
{
	public class MonitorKeyPressDlg : Form, IMessageFilter
	{
		public const int WM_KEYDOWN = 0x100;
		private const int WM_KEYUP = 0x101;

		/// ------------------------------------------------------------------------------------
		public MonitorKeyPressDlg()
		{
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Application.AddMessageFilter(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			// This should have happened when the form closed, but in case it crashed, we
			// do it now to prevent having keystrokes get directed to a defunct window.
			if (disposing)
				Application.RemoveMessageFilter(this);

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			Application.RemoveMessageFilter(this);
			base.OnFormClosed(e);
		}

		/// ------------------------------------------------------------------------------------
		public bool PreFilterMessage(ref Message m)
		{
			if (ActiveForm is ExceptionReportingDialog)
				return false;

			if (m.Msg == WM_KEYDOWN)
				return OnLowLevelKeyDown((Keys)m.WParam);

			if (m.Msg == WM_KEYUP)
				return OnLowLevelKeyUp((Keys)m.WParam);

			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnLowLevelKeyDown(Keys key)
		{
			return false;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual bool OnLowLevelKeyUp(Keys key)
		{
			return false;
		}
	}
}
