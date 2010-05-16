using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SilUtils;

namespace SayMore.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A special text box that only shows its border when the mouse hovers over it or when
	/// the control has focus and is only editable when the control has focus.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class HoverCueTextBox : UserControl
	{
		private bool _showHoverBorder;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="HoverCueTextBox"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public HoverCueTextBox()
		{
			DynamicBorder = true;
			HoverBorderColor = (PaintingHelper.CanPaintVisualStyle() ?
				VisualStyleInformation.TextControlBorder : SystemColors.ControlDarkDark);

			InitializeComponent();
			_txtBox.BackColor = BackColor;
			SetStyle(ControlStyles.UseTextForAccessibility, true);
			SizeChanged += HandleSizeChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the color of the border when hovering over the control (or when it
		/// has focus).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color HoverBorderColor { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the control's border color changes
		/// as the mouse enters and leaves the control (or when focus is gained and lost).
		/// When this value is false, the border is always showing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool DynamicBorder { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the text associated with the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get { return _txtBox.Text; }
			set { _txtBox.Text = value; }
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
				_txtBox.Font = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the background color of the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				base.BackColor = value;

				if (value != Color.Transparent)
					_txtBox.BackColor = value;

				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color ForeColor
		{
			get { return _txtBox.ForeColor; }
			set { _txtBox.ForeColor = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the inner text box control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TextBox InnerTextBox
		{
			get { return _txtBox; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Ensures the height of the user control is always the height of the text box plus
		/// the upper and lower padding.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleSizeChanged(object sender, EventArgs e)
		{
			SizeChanged -= HandleSizeChanged;
			Height = _txtBox.Height + Padding.Top + Padding.Bottom;
			SizeChanged += HandleSizeChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			_txtBox_MouseEnter(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			_txtBox_MouseLeave(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Enter"/> event.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			_txtBox.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the MouseEnter event of the _txtBox control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _txtBox_MouseEnter(object sender, EventArgs e)
		{
			_txtBox_Enter(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Give control a border.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _txtBox_Enter(object sender, EventArgs e)
		{
			_showHoverBorder = true;
			Invalidate();
			_txtBox.BackColor = SystemColors.Window;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remove border when control does not have focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _txtBox_MouseLeave(object sender, EventArgs e)
		{
			if (!_txtBox.Focused)
			{
				_showHoverBorder = false;
				_txtBox.BackColor =
					(BackColor == Color.Transparent ? SystemColors.Control : BackColor);

				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remove the border when the mouse is not over the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _txtBox_Leave(object sender, EventArgs e)
		{
			_txtBox.SelectionStart = 0;

			if (!ClientRectangle.Contains(PointToClient(MousePosition)))
			{
				if (BackColor != Color.Transparent)
					_txtBox.BackColor = BackColor;

				_showHoverBorder = false;
				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Paints the border around the text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			using (Pen pen = new Pen(_showHoverBorder || !DynamicBorder ? HoverBorderColor : BackColor))
			{
				Rectangle rc = ClientRectangle;
				rc.Height--;
				rc.Width--;
				e.Graphics.DrawRectangle(pen, rc);
			}
		}
	}
}
