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

namespace AutoComplete
{
	public class AutoCompleteTextBox : TextBox
	{
		public Action<string[]> PopulateAndDisplayList;
		public Action HideList;
		private ListBox _listBox;
		private bool _isAdded;
		private String _formerValue = String.Empty;
		public string[] Values { get; set; }
		private string[] _separators;

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

		public AutoCompleteTextBox()
		{
			Separator = "; ";
			PopulateAndDisplayList = PopulateAndDisplayListBox;
			HideList = ResetListBox;
			InitializeComponent();
			ResetListBox();
		}

		private void InitializeComponent()
		{
			_listBox = new ListBox();
			KeyDown += this_KeyDown;
			KeyUp += this_KeyUp;
		}

		private void ShowListBox()
		{
			if (!_isAdded)
			{
				Parent.Controls.Add(_listBox);
				_listBox.Left = Left;
				_listBox.Top = Top + Height;
				_isAdded = true;
			}
			_listBox.Visible = true;
		}

		private void ResetListBox()
		{
			_listBox.Items.Clear();
			_listBox.Visible = false;
		}

		private void this_KeyUp(object sender, KeyEventArgs e)
		{
			UpdateListBox();
		}

		private void this_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Tab:
					{
						if (_listBox.Visible)
						{
							InsertWord((String)_listBox.SelectedItem);
							ResetListBox();
							_formerValue = Text;
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

		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Tab:
					return true;
				default:
					return base.IsInputKey(keyData);
			}
		}

		private void UpdateListBox()
		{
			if (Text != _formerValue)
			{
				_formerValue = Text;
				String word = GetWord();

				if (word.Length > 0)
				{
					String[] matches = Array.FindAll(Values,
						x => (x.StartsWith(word) && !SelectedValues.Contains(x)));
					if (matches.Length > 0)
					{
						PopulateAndDisplayList(matches);
						return;
					}
				}
			ResetListBox();
			}
		}

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

		public TextRange RangeOfCurrentItem
		{
			get
			{
				int pos = SelectionStart;

				int posStart = -1;
				int lengthOfPrecedingSeparator = 0;
				foreach (string s in _separators)
				{
					var i = Text.LastIndexOf(s, (pos < 1) ? 0 : pos - 1);
					if (i > posStart || (i == posStart && s.Length > lengthOfPrecedingSeparator))
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

		private String GetWord()
		{
			var currentItem = RangeOfCurrentItem;
			return Text.Substring(currentItem.Start, currentItem.End - currentItem.Start).Trim();
		}

		public void InsertWord(String newItem)
		{
			var currentItem = RangeOfCurrentItem;
			String firstPart = Text.Substring(0, currentItem.Start) + newItem;

			Text = firstPart + Text.Substring(currentItem.End, Text.Length - currentItem.End);
			SelectionStart = firstPart.Length;
		}

		public List<String> SelectedValues
		{
			get
			{
				String[] result = Text.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
				return new List<String>(result);
			}
		}
	}

	public struct TextRange
	{
		public int Start;
		public int End;
	}
}
