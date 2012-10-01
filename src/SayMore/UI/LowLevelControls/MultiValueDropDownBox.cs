using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SilTools;

namespace SayMore.UI.LowLevelControls
{
	public partial class MultiValueDropDownBox : UserControl
	{
		protected Func<IEnumerable<PickerPopupItem>> _funcJITListAcquisition;
		public event CancelEventHandler DropDownOpening;
		public new event KeyEventHandler KeyDown;

		public MultiValuePickerPopup Popup { get; private set; }

		private bool _popupShowing = false;

		protected Control _textControl;

		#region Constructor and Initialization
		/// ------------------------------------------------------------------------------------
		public MultiValueDropDownBox()
		{
			Popup = new MultiValuePickerPopup();
			Popup.PopupOpening += OnDropDownOpening;
			Popup.ItemCheckChanged += HandleItemCheckChanged;

			Font = Program.DialogFont;
			_textControl = CreateTextControl();
			InitializeComponent();
			InitializeTextControl();
			_panelButton.Width = SystemInformation.VerticalScrollBarWidth;

			_panelButton.MouseEnter += delegate { _panelButton.Invalidate(); };
			_panelButton.MouseLeave += delegate { _panelButton.Invalidate(); };
		}

		/// ------------------------------------------------------------------------------------
		protected virtual Control CreateTextControl()
		{
			return new Label();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void InitializeTextControl()
		{
			Controls.Add(_textControl);

			_textControl.Font = Font;
			_textControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			_textControl.Name = "_textControl";
			_textControl.TabIndex = 0;
			_textControl.KeyDown += HandleTextBoxKeyDown;
			_textControl.MouseClick += HandleMouseClickOnDropDownButton;
		}
		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		public virtual Func<IEnumerable<PickerPopupItem>> JITListAcquisition
		{
			get { return _funcJITListAcquisition; }
			set { _funcJITListAcquisition = value; }
		}

		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Control TextControl
		{
			get { return _textControl; }
		}

		/// ------------------------------------------------------------------------------------
		public override string Text
		{
			get { return _textControl.Text; }
			set { _textControl.Text = value; }
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(false)]
		protected virtual bool ReadOnly
		{
			get { return true; }
			set { }
		}

		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = _textControl.BackColor = value; }
		}

		/// ------------------------------------------------------------------------------------
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = _textControl.ForeColor = value; }
		}
		#endregion

		#region Overrides and painting methods
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			if (_textControl == null)
				return;
			_textControl.Height = Height - 4;
			_textControl.Location = new Point(_textControl.Left, 3);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Force the height of the control to be the same height as a normal combo box for the
		/// current font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			using (var cbo = new ComboBox())
			{
				cbo.Font = Font;
				height = cbo.PreferredHeight;
			}

			//_textControl.Top = (height - _textControl.Height) / 2;
			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			_textControl.Focus();
			_textControl.Paint += HandleTextBoxPaint;
			_textControl.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			Popup.Clear();
			Popup.AddRange(JITListAcquisition());
			Popup.SetCheckedItemsFromDelimitedString(Text);
			Text = Popup.GetCheckedItemsString();
			_textControl.Paint -= HandleTextBoxPaint;
			_textControl.Invalidate();
			_popupShowing = false;
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
		private void HandleTextBoxPaint(object sender, PaintEventArgs e)
		{
			TextRenderer.DrawText(e.Graphics, TextControl.Text, TextControl.Font,
				_textControl.ClientRectangle, SystemColors.HighlightText,
				SystemColors.Highlight, TextFormatFlags.Left);
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
			else if (_textControl.Focused)
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
			if (!Enabled)
				return VisualStyleElement.ComboBox.DropDownButton.Disabled;

			if (_panelButton.ClientRectangle.Contains(_panelButton.PointToClient(MousePosition)))
			{
				return (MouseButtons == MouseButtons.Left ?
					VisualStyleElement.ComboBox.DropDownButton.Pressed :
					VisualStyleElement.ComboBox.DropDownButton.Hot);
			}

			return VisualStyleElement.ComboBox.DropDownButton.Normal;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			HandleButtonSizeChanged(null, null);
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		private void HandleButtonSizeChanged(object sender, EventArgs e)
		{
			_textControl.Width = ClientSize.Width - (_panelButton.Width + 3);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMouseClickOnDropDownButton(object sender, MouseEventArgs e)
		{
			if (_popupShowing)
				_popupShowing = false;
			else
				ShowPopup();
		}

		/// ------------------------------------------------------------------------------------
		protected void ShowPopup()
		{
			if (JITListAcquisition != null)
			{
				Popup.Clear();
				Popup.AddRange(JITListAcquisition());
			}

			Popup.Width = Width;
			var pt = PointToScreen(new Point(0, Height));
			Popup.ShowPopup(pt);
			_popupShowing = true;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnDropDownOpening(object sender, CancelEventArgs e)
		{
			if (!e.Cancel && DropDownOpening != null)
				DropDownOpening(this, e);

			if (!e.Cancel)
			{
				Popup.SetCheckedItemsFromDelimitedString(Text);
			//	_textControl.Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleItemCheckChanged(object sender, PickerPopupItem item)
		{
			Text = Popup.GetCheckedItemsString();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTextBoxKeyDown(object sender, KeyEventArgs e)
		{
			if (KeyDown != null)
				KeyDown(this, e);

			if (e.Handled)
				return;

			if (e.Alt && e.KeyCode == Keys.Down)
				ShowPopup();

			if (!ReadOnly)
				return;

			e.Handled = (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left ||
				e.KeyCode == Keys.Down || e.KeyCode == Keys.Up);
		}
		#endregion
	}
}
