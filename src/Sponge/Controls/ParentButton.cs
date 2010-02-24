using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SIL.Sponge.Model;
using SIL.Sponge.Properties;

namespace SIL.Sponge.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Control that acts like two radio buttons but uses an image of a male and female.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ParentButton : UserControl
	{
		public event CancelEventHandler SelectedChanging;

		private ParentType m_parentType = ParentType.Father;
		private bool m_selected;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ParentButton"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ParentButton()
		{
			BackColor = Color.Transparent;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the value of the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ParentType ParentType
		{
			get { return m_parentType; }
			set
			{
				m_parentType = value;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ParentButton"/> is selected.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Selected
		{
			get { return m_selected; }
			set
			{
				if (m_selected == value)
					return;

				if (SelectedChanging != null)
				{
					var args = new CancelEventArgs();
					SelectedChanging(this, args);
					if (args.Cancel)
						return;
				}

				m_selected = value;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether the control can respond to user interaction.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new bool Enabled
		{
			get { return base.Enabled; }
			set
			{
				base.Enabled = true;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the current image.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Image Image
		{
			get
			{
				if (m_parentType == ParentType.Father)
				{
					return (m_selected && Enabled ?
						Resources.kimidMale_Selected : Resources.kimidMale_NotSelected);
				}

				return (m_selected && Enabled ?
					Resources.kimidFemale_Selected : Resources.kimidFemale_NotSelected);
			}
		}

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.KeyPress"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (Enabled && e.KeyChar == (char)Keys.Space)
				Selected = !Selected;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Click"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			if (Enabled)
				Selected = !Selected;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Enter"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Leave"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			var highlight = ClientRectangle.Contains(PointToClient(MousePosition));

			if (highlight)
			{
				using (var br = new SolidBrush(ProfessionalColors.ButtonSelectedHighlight))
					e.Graphics.FillRectangle(br, ClientRectangle);
			}

			var img = Image;
			var x = (img.Width > ClientSize.Width ? 0 : (ClientSize.Width - img.Width) / 2);
			var y = (img.Height > ClientSize.Height ? 0 : (ClientSize.Height - img.Height) / 2);
			var rc = new Rectangle(x, y, img.Width, img.Height);

			e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImage(img, rc);

			if (highlight || Focused)
			{
				using (var pen = new Pen(ProfessionalColors.ButtonSelectedHighlightBorder))
				{
					rc = ClientRectangle;
					rc.Height--;
					rc.Width--;
					e.Graphics.DrawRectangle(pen, rc);
				}
			}
		}

		#endregion
	}
}
