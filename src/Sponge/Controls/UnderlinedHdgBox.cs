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
		private Color _lineColor = SystemColors.ControlDark;
		private int _lineThickness = 1;
		private readonly Label _lblHeading;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="UnderlinedHdgBox"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public UnderlinedHdgBox()
		{
			_lblHeading = new Label();
			_lblHeading.AutoEllipsis = true;
			_lblHeading.AutoSize = false;
			_lblHeading.Dock = DockStyle.Top;
			Controls.Add(_lblHeading);
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
			get { return _lineColor; }
			set
			{
				_lineColor = value;
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
			get { return _lineThickness; }
			set
			{
				_lineThickness = value;
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
				_lblHeading.Font = value;

				using (var g = CreateGraphics())
				{
					_lblHeading.Height = TextRenderer.MeasureText(g, "X",
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
				_lblHeading.Text = value;
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
				_lblHeading.ForeColor = value;
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
				_lblHeading.BackColor = value;
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
				var dx = _lblHeading.Bottom + 3 + (LineThickness / 2);
				pen.EndCap = System.Drawing.Drawing2D.LineCap.Square;
				var pt1 = new Point(0, dx);
				var pt2 = new Point(Width, dx);
				e.Graphics.DrawLine(pen, pt1, pt2);
			}
		}
	}
}
