using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SilUtils;

namespace SayMore.UI.LowLevelControls
{
	public partial class MultiValueComboBox : UserControl
	{
		public event CancelEventHandler DropDownOpening;

		public MultiValuePickerPopup Popup { get; private set; }

		/// ------------------------------------------------------------------------------------
		public MultiValueComboBox()
		{
			Popup = new MultiValuePickerPopup();
			Popup.PopupOpening += OnDropDownOpening;
			Popup.PopupClosing += OnDropDownClosing;
			Popup.ItemCheckChanged += HandleItemCheckChanged;

			base.Font = SystemFonts.IconTitleFont;
			InitializeComponent();
			Font = base.Font;

			var borderWidth = (_textBox.Size.Width - _textBox.ClientSize.Width) / 2;
			var borderHeight = (_textBox.Size.Height - _textBox.ClientSize.Height) / 2;
			_textBox.BorderStyle = BorderStyle.None;
			Padding = new Padding(borderWidth, borderHeight, borderWidth, borderHeight);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		[DefaultValue(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool ShowPromptTextBoxInDropDown
		{
			get { return Popup.ShowPromptTextBox; }
			set { Popup.ShowPromptTextBox = value; }
		}

		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public TextBox TextBox
		{
			get { return _textBox; }
		}

		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get { return _textBox.Text; }
			set { _textBox.Text = value; }
		}

		/// ------------------------------------------------------------------------------------
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				_textBox.Font = value;
				Height = 1;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get { return _textBox.BackColor; }
			set { _textBox.BackColor = value; }
		}

		/// ------------------------------------------------------------------------------------
		public override Color ForeColor
		{
			get { return _textBox.ForeColor; }
			set { _textBox.ForeColor = value; }
		}

		#endregion

		#region Overrides and painting methods
		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			_textBox.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Force the height of the control to be the same height as the text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			using (var cbo = new ComboBox())
			{
				cbo.Font = Font;
				height = cbo.PreferredHeight;
			}

			_textBox.Top = (height - _textBox.Height) / 2;
			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			if (!PaintingHelper.CanPaintVisualStyle())
				ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle);
			else
			{
				var renderer = new VisualStyleRenderer(GetVisualStyleTextBox());
				renderer.DrawBackground(e.Graphics, ClientRectangle);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonPaint(object sender, PaintEventArgs e)
		{
			var rc = _panelButton.ClientRectangle;
			var element = GetVisualStyleComboButton();

			if (!PaintingHelper.CanPaintVisualStyle())
			{
				var state = ButtonState.Normal;
				if (element == VisualStyleElement.ComboBox.DropDownButton.Pressed)
					state = ButtonState.Pushed;

				if (!Enabled)
					state |= ButtonState.Inactive;

				ControlPaint.DrawComboButton(e.Graphics, rc, state);
				return;
			}

			if (element != VisualStyleElement.ComboBox.DropDownButton.Normal)
			{
				var renderer = new VisualStyleRenderer(element);
				renderer.DrawBackground(e.Graphics, rc);
			}
			else
			{
				var x = (int)Math.Round((rc.Width - 7) / 2f, MidpointRounding.AwayFromZero);
				var y = (int)Math.Round((rc.Height - 4) / 2f, MidpointRounding.AwayFromZero);
				e.Graphics.DrawLine(SystemPens.WindowText, x, y, x + 6, y++);
				e.Graphics.DrawLine(SystemPens.WindowText, x + 1, y, x + 5, y++);
				e.Graphics.DrawLine(SystemPens.WindowText, x + 2, y, x + 4, y);
				e.Graphics.DrawLine(SystemPens.WindowText, x + 3, y, x + 3, y + 1);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the correct visual style text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private VisualStyleElement GetVisualStyleTextBox()
		{
			var element = VisualStyleElement.TextBox.TextEdit.Normal;

			if (!Enabled)
				element = VisualStyleElement.TextBox.TextEdit.Disabled;
			else if (_textBox.Focused)
				element = VisualStyleElement.TextBox.TextEdit.Focused;
			else if (ClientRectangle.Contains(PointToClient(MousePosition)))
				element = VisualStyleElement.TextBox.TextEdit.Hot;

			return element;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the correct visual style combo button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private VisualStyleElement GetVisualStyleComboButton()
		{
			var element = VisualStyleElement.ComboBox.DropDownButton.Normal;

			if (!Enabled)
				element = VisualStyleElement.ComboBox.DropDownButton.Disabled;
			else if (_panelButton.ClientRectangle.Contains(_panelButton.PointToClient(MousePosition)))
			{
				element = (MouseButtons == MouseButtons.Left ?
					VisualStyleElement.ComboBox.DropDownButton.Pressed :
					VisualStyleElement.ComboBox.DropDownButton.Hot);
			}

			return element;
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		private void HandleTextBoxEnter(object sender, EventArgs e)
		{
			_textBox.SelectAll();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonMouseEnterLeave(object sender, EventArgs e)
		{
			_panelButton.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonMouseDownUp(object sender, MouseEventArgs e)
		{
			_panelButton.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		private void OnMouseClickOnDropDownButton(object sender, MouseEventArgs e)
		{
			if (!_textBox.Focused)
				_textBox.Focus();

			Popup.Width = Width;
			var pt = PointToScreen(new Point(0, Height));
			Popup.ShowPopup(pt);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnDropDownOpening(object sender, CancelEventArgs e)
		{
			if (!e.Cancel && DropDownOpening != null)
				DropDownOpening(this, e);

			if (!e.Cancel)
			{
				_textBox.HideSelection = false;
				Popup.SetCheckedItemsFromDelimitedString(Text);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnDropDownClosing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			if (!e.Cancel)
				_textBox.HideSelection = true;
		}

		/// ------------------------------------------------------------------------------------
		void HandleItemCheckChanged(object sender, PickerPopupItem item)
		{
			Text = Popup.GetCheckedItemsString();
			_textBox.SelectAll();
		}

		#endregion
	}
}
