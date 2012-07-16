//--------------------------------------------------------------------------------
// This custom textbox control was developed by Peter Holpar and downloaded from
// CodePlex. It is licensed under Microsoft Public License (Ms-PL). Details here:
// http://autocompletetexboxcs.codeplex.com/license
// Significant modifications made by Tom Bogle to adapt it for use in SayMore.
//--------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace SayMore.UI.LowLevelControls
{
	public class AutoCompleteTextBox : TextBox
	{
		public Action<string[]> PopulateAndDisplayList;
		public Action HideList;
		private ListBox _listBox;
		private String _formerValue = String.Empty;
		public Func<IEnumerable<string>> JITListAcquisition;
		private string[] _values;
		private string[] _separators;
		private bool _updatingTextProgrammatically;

		#region Construction & initialization
		/// ------------------------------------------------------------------------------------
		public AutoCompleteTextBox()
		{
			Separator = "; ";
			PopulateAndDisplayList = PopulateAndDisplayListBox;
			HideList = ResetListBox;
		}
		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		public string[] Values
		{
			get { return _values; }
			set
			{
				if (JITListAcquisition != null)
					throw new InvalidOperationException("Values should not be set directly if using a just-in-time list.");
				_values = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string Separator
		{
			get { return _separators[0]; }
			set
			{
				var trimmedSeparator = value.Trim();
				_separators = trimmedSeparator != value && trimmedSeparator.Length > 0 ?
					new[] {value, trimmedSeparator} : new[] {value};
			}
		}

		/// ------------------------------------------------------------------------------------
		public List<String> SelectedValues
		{
			get
			{
				String[] result = Text.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
				return new List<String>(result);
			}
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		private void ShowListBox()
		{
			if (_listBox == null)
			{
				_listBox = new ListBox();
				Parent.Controls.Add(_listBox);
				_listBox.Left = Left;
				_listBox.Top = Top + Height;
			}
			_listBox.Visible = true;
			Focus();
		}

		/// ------------------------------------------------------------------------------------
		private void ResetListBox()
		{
			if (_listBox == null)
				return;
			_listBox.Items.Clear();
			_listBox.Visible = false;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			UpdateListBox();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (_listBox == null)
				return;

			switch (e.KeyCode)
			{
				case Keys.Tab:
					{
						if (_listBox.Visible)
						{
							var item = (String)_listBox.SelectedItem;
							InsertWord(item);
							ResetListBox();
							_formerValue = item;
						}
						break;
					}
				case Keys.Down:
					{
						if ((_listBox.Visible) && (_listBox.SelectedIndex < _listBox.Items.Count - 1))
						{
							_listBox.SelectedIndex++;
						}
						break;
					}
				case Keys.Up:
					{
						if ((_listBox.Visible) && (_listBox.SelectedIndex > 0))
						{
							_listBox.SelectedIndex--;
						}
						break;
					}
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override bool IsInputKey(Keys keyData)
		{
			if (_listBox != null && keyData == Keys.Tab)
				return true;
			return base.IsInputKey(keyData);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			if (JITListAcquisition != null)
				_values = JITListAcquisition().ToArray();
		}

		/// ------------------------------------------------------------------------------------
		public void ChangeText(string newText, int newSelectionPosition)
		{
			// When we change the Text programmatically, the previous selection is lost,
			// making the AutoCompleteTextBox lose track of which item is "current". We
			// don't want the list to update until we've reset the selection.
			_updatingTextProgrammatically = true;

			Text = newText;
			SelectionStart = newSelectionPosition;

			_updatingTextProgrammatically = false;

			UpdateListBox();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateListBox()
		{
			if (_updatingTextProgrammatically || _values == null)
				return;

			String word = GetWord();
			if (word == _formerValue)
				return;
			_formerValue = word;

			if (word.Length > 0)
			{
				String[] matches = Array.FindAll(_values,
					x => (x.StartsWith(word, StringComparison.OrdinalIgnoreCase) && !SelectedValues.Contains(x)));
				if (matches.Length > 0)
				{
					PopulateAndDisplayList(matches);
					return;
				}
			}
			HideList();
		}

		/// ------------------------------------------------------------------------------------
		private void PopulateAndDisplayListBox(string[] matches)
		{
			ShowListBox();
			Array.ForEach(matches, x => _listBox.Items.Add(x));
			_listBox.SelectedIndex = 0;
			_listBox.Height = 0;
			_listBox.Width = 0;
			Focus();
			using (Graphics graphics = _listBox.CreateGraphics())
			{
				for (int i = 0; i < _listBox.Items.Count; i++)
				{
					_listBox.Height += _listBox.GetItemHeight(i);
					int itemWidth = (int)graphics.MeasureString(((String)_listBox.Items[i]), _listBox.Font).Width + 5;
					_listBox.Width = (_listBox.Width < itemWidth) ? itemWidth : _listBox.Width;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public TextRange RangeOfCurrentItem
		{
			get
			{
				int pos = SelectionStart;

				int posStart = 0;
				int lengthOfPrecedingSeparator = 0;
				foreach (string s in _separators)
				{
					var i = Text.LastIndexOf(s, (pos < 1) ? 0 : pos - 1);
					if (i > posStart)
					{
						posStart = i;
						lengthOfPrecedingSeparator = s.Length;
					}
				}

				int posEnd = _separators.Select(s => Text.IndexOf(s, pos)).Min();
				return new TextRange
				{
					Start = (posStart == -1) ? 0 : posStart + lengthOfPrecedingSeparator,
					End = (posEnd == -1) ? Text.Length : posEnd
				};
			}
		}

		/// ------------------------------------------------------------------------------------
		private String GetWord()
		{
			var currentItem = RangeOfCurrentItem;
			return Text.Substring(currentItem.Start, currentItem.End - currentItem.Start).Trim();
		}

		/// ------------------------------------------------------------------------------------
		public void InsertWord(String newItem)
		{
			var currentItem = RangeOfCurrentItem;
			String firstPart = Text.Substring(0, currentItem.Start) + newItem;

			Text = firstPart + Text.Substring(currentItem.End, Text.Length - currentItem.End);
			SelectionStart = firstPart.Length;
		}
	}

	/// ------------------------------------------------------------------------------------
	public struct TextRange
	{
		public int Start;
		public int End;
	}
}
