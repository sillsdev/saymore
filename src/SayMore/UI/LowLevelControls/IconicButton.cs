using System;
using System.Drawing;
using System.Windows.Forms;

namespace SayMore.UI.LowLevelControls
{
	#region PopupDialogButton class
	/// ----------------------------------------------------------------------------------------
	public class IconicButton : Panel
	{
		protected Image m_imgHot;
		protected Image m_imageNormal;

		/// ------------------------------------------------------------------------------------
		public IconicButton()
		{
			DoubleBuffered = true;
			BackColor = Color.Transparent;
			Size = new Size(22, 22);
			BackgroundImageLayout = ImageLayout.Center;

			// Use the background image instead of the Image property because when
			// using the Image property there is a lot of painting flicker as the
			// Image changes from the mouse moving over the button.
			BackgroundImage = ImageNormal;
		}

		/// ------------------------------------------------------------------------------------
		public virtual Image ImageHot
		{
			get { return (m_imgHot ?? BackgroundImage); }
			set { m_imgHot = value; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual Image ImageNormal
		{
			get { return (m_imageNormal ?? BackgroundImage); }
			set
			{
				m_imageNormal = value;
				if (!IsHandleCreated)
					BackgroundImage = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			BackgroundImage = ImageHot;
			base.OnMouseEnter(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			BackgroundImage = ImageNormal;
			base.OnMouseLeave(e);
		}
	}

	#endregion
}
