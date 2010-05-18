using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SayMore.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A special text box that only shows its border when the mouse hovers over it or when
	/// the control has focus and is only editable when the control has focus.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class VisuallyDynamicTextBox : UserControl
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="VisuallyDynamicTextBox"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public VisuallyDynamicTextBox()
		{
			UnfocusedBorderColor = Color.Transparent;

			FocusedBorderColor = (
				VisualStyleInformation.IsSupportedByOS && VisualStyleInformation.IsEnabledByUser ?
				VisualStyleInformation.TextControlBorder : SystemColors.ControlDarkDark);

			UnfocusedBackColor = SystemColors.Window;
			FocusedBackColor = SystemColors.Window;

			InitializeComponent();
			_txtBox.BackColor = UnfocusedBackColor;
			SetStyle(ControlStyles.UseTextForAccessibility, true);
			SizeChanged += HandleSizeChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the color of the border when the mouse is not hovering over the
		/// control and when the control does not have focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color UnfocusedBorderColor { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the color of the border when hovering over the control or when it
		/// has focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color FocusedBorderColor { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the color of the background when the mouse is not hovering
		/// over the control and when the control does not have focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color UnfocusedBackColor { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the color of the background when the mouse is hovering over the
		/// control and when the control does not have focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color FocusedBackColor { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the text associated with the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
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
		public new Color BackColor
		{
			get { return _txtBox.BackColor; }
			set
			{
				if (value != Color.Transparent)
					_txtBox.BackColor = value;
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
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool Multiline
		{
			get { return _txtBox.Multiline; }
			set { _txtBox.Multiline = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ScrollBars ScrollBars
		{
			get { return _txtBox.ScrollBars; }
			set { _txtBox.ScrollBars = value; }
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

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"/> event.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//protected override void OnMouseEnter(EventArgs e)
		//{
		//    base.OnMouseEnter(e);
		//    ShowVisualCue();
		//}

		///// ------------------------------------------------------------------------------------
		//protected override void OnMouseLeave(EventArgs e)
		//{
		//    base.OnMouseLeave(e);
		//    HideVisualCue();
		//}

		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			_txtBox.Focus();
		}

		/// ------------------------------------------------------------------------------------
		private void _txtBox_MouseEnter(object sender, EventArgs e)
		{
			ShowVisualCue();
		}

		/// ------------------------------------------------------------------------------------
		private void _txtBox_Enter(object sender, EventArgs e)
		{
			ShowVisualCue();
			_txtBox.SelectAll();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remove border when control does not have focus.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _txtBox_MouseLeave(object sender, EventArgs e)
		{
			if (!_txtBox.Focused)
				HideVisualCue();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Remove the border when the mouse is not over the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void _txtBox_Leave(object sender, EventArgs e)
		{
			if (!ClientRectangle.Contains(PointToClient(MousePosition)))
				HideVisualCue();
		}

		/// ------------------------------------------------------------------------------------
		private void ShowVisualCue()
		{
			if (base.BackColor != FocusedBorderColor)
				base.BackColor = FocusedBorderColor;

			if (_txtBox.BackColor != FocusedBackColor)
				_txtBox.BackColor = FocusedBackColor;
		}

		/// ------------------------------------------------------------------------------------
		private void HideVisualCue()
		{
			if (base.BackColor != UnfocusedBorderColor)
				base.BackColor = UnfocusedBorderColor;

			if (_txtBox.BackColor != UnfocusedBackColor)
				_txtBox.BackColor = UnfocusedBackColor;
		}

		/// ------------------------------------------------------------------------------------
		private void _txtBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter && _txtBox.Multiline)
				return;

			var move = 0;

			switch (e.KeyCode)
			{
				case Keys.Up: move = (ShouldUpMoveToPreviousControl() ? -1 : 0); break;
				case Keys.Left: move = (ShouldLeftMoveToPreviousControl() ? -1 : 0); break;
				case Keys.Down: move = (ShouldDownMoveToNextControl() ? 1 : 0); break;
				case Keys.Right: move = (ShouldRightMoveToNextControl() ? 1 : 0); break;
				case Keys.Enter: move = (ShouldEnterMoveToNextControl() ? 1 : 0); break;
			}

			if (move != 0)
			{
				Parent.SelectNextControl(this, move > 0, true, true, true);
				e.SuppressKeyPress = true;
				e.Handled = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		private bool ShouldUpMoveToPreviousControl()
		{
			if (!_txtBox.Multiline)
				return true;

			if (_txtBox.SelectionLength > 0)
				return false;

			var currLineIndex = _txtBox.GetLineFromCharIndex(_txtBox.SelectionStart + _txtBox.SelectionLength);
			return (currLineIndex == 0);
		}

		/// ------------------------------------------------------------------------------------
		private bool ShouldLeftMoveToPreviousControl()
		{
			return (_txtBox.SelectionLength == 0 && _txtBox.SelectionStart == 0);
		}

		/// ------------------------------------------------------------------------------------
		private bool ShouldDownMoveToNextControl()
		{
			if (!_txtBox.Multiline || _txtBox.Lines.Length == 0)
				return true;

			if (_txtBox.SelectionLength > 0)
				return false;

			var currLineIndex = _txtBox.GetLineFromCharIndex(_txtBox.SelectionStart + _txtBox.SelectionLength);
			return (currLineIndex == _txtBox.Lines.Length - 1);
		}

		/// ------------------------------------------------------------------------------------
		private bool ShouldEnterMoveToNextControl()
		{
			if (_txtBox.Multiline)
				return false;

			return ShouldDownMoveToNextControl();
		}

		/// ------------------------------------------------------------------------------------
		private bool ShouldRightMoveToNextControl()
		{
			return (_txtBox.SelectionLength == 0 && _txtBox.SelectionStart == _txtBox.Text.Length);
		}
	}
}
