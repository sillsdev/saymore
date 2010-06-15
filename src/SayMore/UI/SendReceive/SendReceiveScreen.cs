using System.Windows.Forms;
using SayMore.Properties;
using SayMore.UI.ProjectWindow;

namespace SayMore.UI.SendReceive
{
	/// ----------------------------------------------------------------------------------------
	public class SendReceiveScreen : UserControl, ISayMoreView
	{
		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get { return "Send/Receive"; }
			set { }
		}

		#region ISayMoreView Members
		/// ------------------------------------------------------------------------------------
		public void ViewActivated(bool firstTime)
		{
		}

		/// ------------------------------------------------------------------------------------
		public void ViewDeactivated()
		{
		}

		/// ------------------------------------------------------------------------------------
		public bool IsOKToLeaveView(bool showMsgWhenNotOK)
		{
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public System.Drawing.Image Image
		{
			get { return Resources.SendReceive; }
		}

		#endregion
	}
}
