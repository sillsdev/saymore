using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SayMore.Model.Fields;

namespace SayMore.UI.LowLevelControls
{
	public class MultiValuePickerPopupViewModel
	{
		public MultiValuePickerPopup Popup { get; private set; }

		private readonly HashSet<PickerPopupItem> _items = new HashSet<PickerPopupItem>();

		/// ------------------------------------------------------------------------------------
		public MultiValuePickerPopupViewModel()
		{
			Popup = new MultiValuePickerPopup();
		}

		/// ------------------------------------------------------------------------------------
		public void ShowPopup(Point pt)
		{
			Popup.ShowPopup(pt, AllItems);
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public IEnumerable<PickerPopupItem> AllItems
		{
			get { return _items.ToArray(); }
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<PickerPopupItem> CheckedItems
		{
			get { return _items.Where(x => x.Checked); }
		}

		#endregion

		#region Methods for adding values, and getting and setting values' checked state.
		/// ------------------------------------------------------------------------------------
		public void Add(string text)
		{
			if (_items.FirstOrDefault(x => x.Text == text) == null)
				Add(new PickerPopupItem { Text = text });
		}

		/// ------------------------------------------------------------------------------------
		public void Add(PickerPopupItem item)
		{
			if (!_items.Contains(item))
			{
				item.CheckedChanged += HandleItemCheckedChanged;
				item.Disposed += HandleItemDisposed;
				_items.Add(item);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void AddRange(IEnumerable<PickerPopupItem> items)
		{
			foreach (var item in items)
				Add(item);
		}

		/// ------------------------------------------------------------------------------------
		public bool IsItemChecked(string text)
		{
			return (_items.FirstOrDefault(x => x.Text == text && x.Checked) != null);
		}

		/// ------------------------------------------------------------------------------------
		public void CheckItem(string text)
		{
			var item = _items.FirstOrDefault(x => x.Text == text);
			if (item != null && !item.Checked)
				item.Checked = true;
		}

		/// ------------------------------------------------------------------------------------
		public void UnCheckItem(string text)
		{
			var item = _items.FirstOrDefault(x => x.Text == text);
			if (item != null && item.Checked)
				item.Checked = false;
		}

		#endregion

		#region Methods for handling checked values in delimited strings.
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a string containing all the checked items's text delimited by the default
		/// delimiter character.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetCheckedItemsString()
		{
			if (_items.Count == 0)
				return string.Empty;

			var builder = new StringBuilder();

			foreach (var item in CheckedItems.OrderBy(x => x.Text))
				builder.AppendFormat("{0}{1} ", item.Text, FieldInstance.kDefaultMultiValueDelimiter);

			// Knock off the last delimiter and space.
			builder.Length -= 2;

			return builder.ToString();
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets a string containing all the checked items' text delimited by the specified
		///// delimiter character.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public string GetCheckedItemsString(char delimiter)
		//{
		//    if (_items.Count == 0)
		//        return string.Empty;

		//    var builder = new StringBuilder();

		//    foreach (var item in CheckedItems.OrderBy(x => x.Text))
		//        builder.AppendFormat("{0}{1} ", item.Text, delimiter);

		//    // Knock off the last delimiter and space.
		//    builder.Length -= 2;

		//    return builder.ToString();
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the checked items from the specified string delimited by the default
		/// delimiter character. All other items in the list are made unchecked.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void SetCheckedItemsFromDelimitedString(string items)
		{
			foreach (var item in _items)
				item.Checked = false;

			foreach (var text in FieldInstance.GetValuesFromText(items))
				CheckItem(text);
		}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Sets the checked items from the specified string delimited by the specified
		///// delimiter character. All other items in the list are made unchecked.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public void SetCheckedItemsFromDelimitedString(string items, char delimiter)
		//{
		//    foreach (var item in _items)
		//        item.Checked = false;

		//    var list = items.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

		//    foreach (var text in (from val in list
		//                          where val.Trim() != string.Empty
		//                          select val.Trim()))
		//    {
		//        CheckItem(text);
		//    }
		//}

		#endregion

		#region Event handlers for picker items.
		/// ------------------------------------------------------------------------------------
		private void HandleItemDisposed(object sender, EventArgs e)
		{
			var item = sender as PickerPopupItem;
			item.CheckedChanged -= HandleItemCheckedChanged;
			item.Disposed -= HandleItemDisposed;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleItemCheckedChanged(PickerPopupItem item, bool checkWasOnCheckBox)
		{
			if (!checkWasOnCheckBox)
				Popup.ClosePopup();
		}

		#endregion
	}
}
