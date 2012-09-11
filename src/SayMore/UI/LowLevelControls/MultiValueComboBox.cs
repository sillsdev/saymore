using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SilTools;

namespace SayMore.UI.LowLevelControls
{
	public partial class MultiValueComboBox : UserControl, IMessageFilter
	{
		private Func<IEnumerable<PickerPopupItem>> _funcJITListAcquisition;
		public event CancelEventHandler DropDownOpening;
		public new event KeyEventHandler KeyDown;

		public MultiValuePickerPopup Popup { get; private set; }

		protected readonly int _borderWidth;
		private bool _selectAllTextOnMouseDown;
		private string[] _displayedMatches = null;

		#region Constructor and Initialization
		/// ------------------------------------------------------------------------------------
		public MultiValueComboBox()
		{
			Popup = new MultiValuePickerPopup();
			Popup.PopupOpening += OnDropDownOpening;
			Popup.PopupClosing += delegate { StopPreFilteringMessagesForPopup(); };
			Popup.ItemCheckChanged += HandleItemCheckChanged;

			Font = Program.DialogFont;
			InitializeComponent();
			_textBox.PopulateAndDisplayList = DisplaySuggestions;
			_textBox.HideList = ClosePopup;
			_textBox.Font = Font;
			Height = 1;

			_borderWidth = (_textBox.Size.Width - _textBox.ClientSize.Width) / 2;
			var borderHeight = (_textBox.Size.Height - _textBox.ClientSize.Height) / 2;
			_textBox.BorderStyle = BorderStyle.None;
			_panelButton.Width = SystemInformation.VerticalScrollBarWidth;

			Padding = new Padding(_borderWidth, borderHeight, _borderWidth, borderHeight);

			_textBox.Validating += (s, e) => OnValidating(e);

			CausesValidation = true;

			_textBox.MouseDown += delegate
			{
				if (_selectAllTextOnMouseDown)
				{
					_selectAllTextOnMouseDown = false;
					_textBox.SelectAll();
				}
			};
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			Parent.Disposed += delegate { StopPreFilteringMessagesForPopup(); };
		}
		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		public Func<IEnumerable<PickerPopupItem>> JITListAcquisition
		{
			get { return _funcJITListAcquisition; }
			set
			{
				_funcJITListAcquisition = value;
				_textBox.JITListAcquisition = () => from item in value()
													select item.Text;
			}
		}

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
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = _textBox.BackColor = value; }
		}

		/// ------------------------------------------------------------------------------------
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = _textBox.ForeColor = value; }
		}

		/// ------------------------------------------------------------------------------------
		public AutoCompleteMode AutoCompleteMode
		{
			get { return _textBox.AutoCompleteMode; }
			set { _textBox.AutoCompleteMode = value; }
		}

		/// ------------------------------------------------------------------------------------
		public AutoCompleteSource AutoCompleteSource
		{
			get { return _textBox.AutoCompleteSource; }
			set { _textBox.AutoCompleteSource = value; }
		}

		/// ------------------------------------------------------------------------------------
		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				var values = _textBox.Values;
				if (values == null)
					return null;
				var source = new AutoCompleteStringCollection();
					source.AddRange(values);
				return source;
			}
			set
			{
				if (_textBox.JITListAcquisition != null || value == null)
					return;
				string[] values = new string[value.Count];
				int i = 0;
				foreach (var item in value)
					values[i++] = item.ToString();
				_textBox.Values = values;
			}
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(false)]
		public bool ReadOnly
		{
			get { return _textBox.ReadOnly; }
			set { _textBox.ReadOnly = value; }
		}

		/// ------------------------------------------------------------------------------------
		public int SelectionStart
		{
			get { return _textBox.SelectionStart; }
			set { _textBox.SelectionStart = value; }
		}

		/// ------------------------------------------------------------------------------------
		public int SelectionLength
		{
			get { return _textBox.SelectionLength; }
			set { _textBox.SelectionLength = value; }
		}

		#endregion

		#region public methods
		/// ------------------------------------------------------------------------------------
		public void SelectAll()
		{
			_textBox.SelectAll();
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
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			Popup.Clear();
			Popup.AddRange(JITListAcquisition());
			Popup.SetCheckedItemsFromDelimitedString(Text);
			Text = Popup.GetCheckedItemsString();
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
		private void DisplaySuggestions(string[] matches)
		{
			if (_displayedMatches != null && _displayedMatches.SequenceEqual(matches))
				return;

			var temp = JITListAcquisition;
			_funcJITListAcquisition = () => from name in matches
									   orderby name
									   select new PickerPopupItem(name, false);

			// Need to pre-filter messages, so that additional typing can be sent to the
			// text box, not the popup.
			Application.AddMessageFilter(this);
			ShowPopup();
			_textBox.Focus();
			_displayedMatches = matches;
			JITListAcquisition = temp;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleButtonSizeChanged(object sender, EventArgs e)
		{
			_textBox.Width = ClientSize.Width - ((_borderWidth * 2) + _panelButton.Width + 3);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTextBoxLeave(object sender, EventArgs e)
		{
			_textBox.SelectionStart = 0;
			_textBox.SelectionLength = 0;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleMouseClickOnDropDownButton(object sender, MouseEventArgs e)
		{
			ShowPopup();
		}

		/// ------------------------------------------------------------------------------------
		private void ShowPopup()
		{
			if (JITListAcquisition != null)
			{
				Popup.Clear();
				Popup.AddRange(JITListAcquisition());
			}

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
				Popup.SetCheckedItemsFromDelimitedString(Text);
		}

		/// ------------------------------------------------------------------------------------
		void HandleItemCheckChanged(object sender, PickerPopupItem item)
		{
			if (_displayedMatches != null)
			{
				_textBox.InsertWord(Popup.GetCheckedItemsString());
				ClosePopup();
			}
			else
			{
				Text = Popup.GetCheckedItemsString();
				_textBox.SelectAll();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ClosePopup()
		{
			var cancel = new CancelEventArgs();
			OnValidating(cancel);
			if (!cancel.Cancel)
				Popup.ClosePopup();
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

			if (!_textBox.ReadOnly)
				return;

			e.Handled = (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left ||
				e.KeyCode == Keys.Down || e.KeyCode == Keys.Up);
		}

		/// ------------------------------------------------------------------------------------
		private void StopPreFilteringMessagesForPopup()
		{
			if (_displayedMatches != null)
			{
				Application.RemoveMessageFilter(this);
				_displayedMatches = null;
			}
		}
		#endregion

		#region Implementation of IMessageFilter
		const int WM_CHAR = 0x102;
		const int VK_BACK = 0x8;
		const int VK_DELETE = 0x2e;
		/// ------------------------------------------------------------------------------------
		/// <summary>This is a bit of a hack to get around the problem that the control in the
		/// popup window gets all the keyboard input, so the user can't keep typing in the text
		/// box when the list is showing.</summary>
		/// ------------------------------------------------------------------------------------
		public bool PreFilterMessage(ref Message m)
		{
			if (_displayedMatches == null || Form.ActiveForm != _textBox.FindForm())
				return false;

			switch (m.Msg)
			{
				case MonitorKeyPressDlg.WM_KEYDOWN:
					if ((int)m.WParam == VK_BACK && _textBox.SelectionStart > 0)
					{
						_textBox.ChangeText(Text.Substring(0, _textBox.SelectionStart - 1) +
							Text.Substring(_textBox.SelectionStart + SelectionLength),
							_textBox.SelectionStart - 1);
						return true;
					}
					if ((int)m.WParam == VK_DELETE && _textBox.Text.Length > _textBox.SelectionStart)
					{
						_textBox.ChangeText(Text.Substring(0, _textBox.SelectionStart) +
							Text.Substring(_textBox.SelectionStart + 1 + SelectionLength),
							_textBox.SelectionStart);
						return true;
					}
					return false;
				case WM_CHAR:
					_textBox.ChangeText(Text.Substring(0, _textBox.SelectionStart) +
						(char)((int)m.WParam) + Text.Substring(_textBox.SelectionStart + SelectionLength),
						_textBox.SelectionStart + 1);
					return true;
				default:
					return false;
			}
		}
		#endregion
	}
}
