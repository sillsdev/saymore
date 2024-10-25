using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SayMore.UI.LowLevelControls
{
	public class MultiValueAutoCompleteComboBox : MultiValueComboBox, IMessageFilter
	{
		private string[] _displayedMatches = null;

		#region Constructor and Initialization
		/// ------------------------------------------------------------------------------------
		public MultiValueAutoCompleteComboBox()
		{
			Popup.PopupClosing += delegate { StopPreFilteringMessagesForPopup(); };
		}

		/// ------------------------------------------------------------------------------------
		protected override Control CreateTextControl()
		{
			return new AutoCompleteTextBox();
		}

		/// ------------------------------------------------------------------------------------
		protected override void InitializeTextControl()
		{
			base.InitializeTextControl();
			AutoCompleteTextBox.PopulateAndDisplayList = DisplaySuggestions;
			AutoCompleteTextBox.HideList = ClosePopup;
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
		public override Func<IEnumerable<PickerPopupItem>> JITListAcquisition
		{
			set
			{
				AutoCompleteTextBox.JITListAcquisition = () =>
					from item in value()
					select item.Text;
			}
		}

		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		private AutoCompleteTextBox AutoCompleteTextBox
		{
			get { return _textControl as AutoCompleteTextBox; }
		}

		/// ------------------------------------------------------------------------------------
		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				var values = AutoCompleteTextBox.Values;
				if (values == null)
					return null;
				var source = new AutoCompleteStringCollection();
				source.AddRange(values);
				return source;
			}
			set
			{
				var textBox = AutoCompleteTextBox;

				if (textBox.JITListAcquisition != null || value == null)
					return;
				string[] values = new string[value.Count];
				int i = 0;
				foreach (var item in value)
					values[i++] = item.ToString();
				textBox.Values = values;
			}
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		private void DisplaySuggestions(string[] matches)
		{
			if (!(_textControl is AutoCompleteTextBox) || (_displayedMatches != null && _displayedMatches.SequenceEqual(matches)))
				return;

			var temp = JITListAcquisition;
			_funcJITListAcquisition = () => from name in matches
											orderby name
											select new PickerPopupItem(name, false);

			// Need to pre-filter messages, so that additional typing can be sent to the
			// text box, not the popup.
			Application.AddMessageFilter(this);
			ShowPopup();
			_textControl.Focus();
			_displayedMatches = matches;
			JITListAcquisition = temp;
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleItemCheckChanged(object sender, PickerPopupItem item)
		{
			if (_displayedMatches != null)
			{
				AutoCompleteTextBox.InsertWord(Popup.GetCheckedItemsString());
				ClosePopup();
			}
			else
				base.HandleItemCheckChanged(sender, item);
		}

		/// ------------------------------------------------------------------------------------
		private void ClosePopup()
		{
			//Review: I think this should now be properly handled in OnPopupClosing method in
			// base class.
			//var cancel = new CancelEventArgs();
			//OnValidating(cancel);
			//if (!cancel.Cancel)
			Popup.ClosePopup();
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
			if (!(_textControl is AutoCompleteTextBox) || _displayedMatches == null ||
				Form.ActiveForm != _textControl.FindForm())
			{
				return false;
			}

			var textBox = _textControl as AutoCompleteTextBox;

			switch (m.Msg)
			{
				case MonitorKeyPressDlg.WM_KEYDOWN:
					if ((int)m.WParam == VK_BACK && textBox.SelectionStart > 0)
					{
						textBox.ChangeText(Text.Substring(0, textBox.SelectionStart - 1) +
							Text.Substring(textBox.SelectionStart + SelectionLength),
							textBox.SelectionStart - 1);
						return true;
					}
					if ((int)m.WParam == VK_DELETE && textBox.Text.Length > textBox.SelectionStart)
					{
						textBox.ChangeText(Text.Substring(0, textBox.SelectionStart) +
							Text.Substring(textBox.SelectionStart + 1 + SelectionLength),
							textBox.SelectionStart);
						return true;
					}
					return false;
				case WM_CHAR:
					textBox.ChangeText(Text.Substring(0, textBox.SelectionStart) +
						(char)((int)m.WParam) + Text.Substring(textBox.SelectionStart + SelectionLength),
						textBox.SelectionStart + 1);
					return true;
				default:
					return false;
			}
		}
		#endregion
	}
}
