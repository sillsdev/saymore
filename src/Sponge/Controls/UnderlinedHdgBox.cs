using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Sponge.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A container control that contains a heading label with a line below it, separating the
	/// heading from what's below it.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
	public class UnderlinedHdgBox : UserControl
	{
		private Color m_lineColor = SystemColors.ControlDark;
		private int m_lineThickness = 1;
		private Label m_lblHeading;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="UnderlinedHdgBox"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UnderlinedHdgBox()
		{
			m_lblHeading = new Label();
			m_lblHeading.AutoEllipsis = true;
			m_lblHeading.AutoSize = false;
			m_lblHeading.Dock = DockStyle.Top;
			Controls.Add(m_lblHeading);
			BackColor = Color.Transparent;

			Font = SystemFonts.MenuFont;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the color of the line below the heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color LineColor
		{
			get { return m_lineColor; }
			set
			{
				m_lineColor = value;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the thickness in pixels of the line below the heading.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int LineThickness
		{
			get { return m_lineThickness; }
			set
			{
				m_lineThickness = value;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the font of the text displayed by the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				m_lblHeading.Font = value;

				using (var g = CreateGraphics())
				{
					m_lblHeading.Height = TextRenderer.MeasureText(g, "X",
						value, new Size(int.MaxValue, int.MaxValue)).Height;
				}

				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the heading text associated with this control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get { return base.Text; }
			set
			{
				base.Text = value;
				m_lblHeading.Text = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the foreground color for the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set
			{
				base.ForeColor = value;
				m_lblHeading.ForeColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				base.BackColor = value;
				m_lblHeading.BackColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the line.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			using (Pen pen = new Pen(LineColor, LineThickness))
			{
				var dx = m_lblHeading.Bottom + 3 + (LineThickness / 2);
				pen.EndCap = System.Drawing.Drawing2D.LineCap.Square;
				var pt1 = new Point(0, dx);
				var pt2 = new Point(Width, dx);
				e.Graphics.DrawLine(pen, pt1, pt2);
			}
		}
	}
}
