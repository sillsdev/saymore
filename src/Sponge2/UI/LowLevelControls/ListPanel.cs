using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Sponge2.Properties;
using SilUtils;
using Sponge2.UI.Utilities;

namespace Sponge2.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Control encapsulating a heading, list view and 'New'/'Delete' buttons.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ListPanel : UserControl
	{
		public delegate void SelectedItemChangedHandler(object sender, object newItem);
		public event SelectedItemChangedHandler SelectedItemChanged;

		public delegate bool BeforeItemsDeletedHandler(object sender, List<object> itemsToDelete);
		public event BeforeItemsDeletedHandler BeforeItemsDeleted;

		public delegate void AfterItemsDeletedHandler(object sender, List<object> itemsToDelete);
		public event AfterItemsDeletedHandler AfterItemsDeleted;

		public delegate object NewButtonClickedHandler(object sender);
		public event NewButtonClickedHandler NewButtonClicked;

		public delegate void ItemAddedHandler(object sender, object itemBeingAdded);
		public event ItemAddedHandler BeforeItemAdded;
		public event ItemAddedHandler AfterItemAdded;

		// This font is used to indicate the text of the current item is a duplicate.
		private Font _fntDupItem;

		private bool _monitorSelectedIndexChanges;
		private ListViewItem _prevFocusedItem;
		private readonly List<Button> _buttons = new List<Button>();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ListPanel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ListPanel()
		{
			ReSortWhenItemTextChanges = false;

			InitializeComponent();

			if (DesignMode)
				return;

			_headerLabel.Font = SystemFonts.IconTitleFont;
			_itemsListView.Font = SystemFonts.IconTitleFont;
			_fntDupItem = new Font(_itemsListView.Font, FontStyle.Italic | FontStyle.Bold);
			_itemsListView.ListViewItemSorter = new ListSorter();
			MonitorSelectedIndexChanges = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				_fntDupItem.Dispose();
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the heading text above the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get { return _headerLabel.Text; }
			set { _headerLabel.Text = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ListView ListView
		{
			get { return _itemsListView; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public object[] Items
		{
			get
			{
				if (_itemsListView.Items.Count == 0)
					return new object[] { };

				var items = new List<object>(_itemsListView.Items.Count);
				foreach (ListViewItem item in _itemsListView.Items)
					items.Add(item.Tag);

				return items.ToArray();
			}
			set
			{
				var currItem = CurrentItem;
				_itemsListView.Items.Clear();
				if (value != null && value.Length > 0)
				{
					var list = new List<object>(value);
					list.Sort((x, y) => (x.ToString() ?? string.Empty).CompareTo(y.ToString() ?? string.Empty));
					foreach (object obj in list)
					{
						var newLvItem = _itemsListView.Items.Add(obj.ToString());
						newLvItem.Tag = obj;
						newLvItem.Name = obj.ToString();
						SetImage(newLvItem);
					}

					CurrentItem = (currItem ?? list[0]);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the image for the specified list view item based on the object in its Tag
		/// property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void SetImage(ListViewItem lvi)
		{
			if (lvi.Tag == null || lvi.Tag.GetType().GetProperty("ImageKey") == null)
				return;

			var imgKey = ReflectionHelper.GetProperty(lvi.Tag, "ImageKey") as string;
			if (imgKey == null)
			{
				lvi.ImageKey = imgKey;
				return;
			}

			var img = Resources.ResourceManager.GetObject(imgKey) as Bitmap;
			if (img == null)
				return;

			if (lvi.ListView.SmallImageList == null)
				lvi.ListView.SmallImageList = new ImageList();

			var imgList = lvi.ListView.SmallImageList;
			if (!imgList.Images.ContainsKey(imgKey))
				imgList.Images.Add(imgKey, img);

			lvi.ImageKey = imgKey;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the current item in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public object CurrentItem
		{
			get { return (_itemsListView.FocusedItem == null ? null : _itemsListView.FocusedItem.Tag); }
			set { SelectItem(value, true); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether or not the list will be Re-sorted when
		/// the UpdateItem is called and the text for the item being updated has changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool ReSortWhenItemTextChanges { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Selects the item with the specified text.
		/// </summary>
		/// <param name="item">The item to be selected. This can be the text of the item,
		/// the list view item, or the underlying object associated with a list view item.
		/// </param>
		/// <param name="generateSelChgEvent">if true, the SelectedItemChanged event is fired.
		/// otherwise the event is not fired.</param>
		/// ------------------------------------------------------------------------------------
		public void SelectItem(object item, bool generateSelChgEvent)
		{
			if (item == null)
				return;

			MonitorSelectedIndexChanges = false;

			if (item is string)
				_itemsListView.FocusedItem = _itemsListView.FindItemWithText(item as string);
			else if (item is ListViewItem)
				_itemsListView.FocusedItem = (item as ListViewItem);
			else
				_itemsListView.FocusedItem = FindListViewItem(item);

			_itemsListView.SelectedItems.Clear();

			if (_itemsListView.FocusedItem != null)
				_itemsListView.FocusedItem.Selected = true;

			if (generateSelChgEvent && SelectedItemChanged != null)
			{
				SelectedItemChanged(this, _itemsListView.FocusedItem != null ?
					_itemsListView.FocusedItem.Tag : null);
			}

			MonitorSelectedIndexChanges = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tries to find the list view item associated with the specified object.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private ListViewItem FindListViewItem(object item)
		{
			foreach (ListViewItem lvi in _itemsListView.Items)
			{
				if (lvi.Tag == item)
					return lvi;
			}

			return null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gives focus to the list view portion of the list panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new void Focus()
		{
			_itemsListView.Focus();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the displayed text for the specified item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UpdateItem(object itemToRefresh, string newText)
		{
			if (itemToRefresh == null)
				return;

			var lvi = FindListViewItem(itemToRefresh);
			if (lvi == null)
				return;

			lvi.Text = (newText ?? itemToRefresh.ToString());
			SetImage(lvi);

			if (ReSortWhenItemTextChanges)
			{
				int index = InsertIndex(lvi.Text);
				if (index - 1 != lvi.Index)
					ReSort();
			}

			// Check if the item before or after the current item has the same text.
			var i = lvi.Index;
			if ((i > 0 && lvi.Text == _itemsListView.Items[i - 1].Text) ||
				(i < _itemsListView.Items.Count - 1 && lvi.Text == _itemsListView.Items[i + 1].Text))
			{
				// Change the look of the current item if its text is duplicated.
				lvi.ForeColor = Color.Red;
				lvi.BackColor = Color.PapayaWhip;
				lvi.Font = _fntDupItem;
				_itemsListView.HideSelection = true;
			}
			else
			{
				// Restore the look of the current item to its default.
				lvi.ForeColor = _itemsListView.ForeColor;
				lvi.BackColor = _itemsListView.BackColor;
				lvi.Font = _itemsListView.Font;
				_itemsListView.HideSelection = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines the preferred index at which to insert the specified string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int InsertIndex(string text)
		{
			for (int i = 0; i < _itemsListView.Items.Count; i++)
			{
				if (_itemsListView.Items[i].Text.CompareTo(text) > 0)
					return i;
			}

			return _itemsListView.Items.Count;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Recreate the font for duplicated items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvItems_FontChanged(object sender, EventArgs e)
		{
			if (_fntDupItem != null)
			{
				_fntDupItem.Dispose();
				_fntDupItem = new Font(_itemsListView.Font, FontStyle.Italic | FontStyle.Bold);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Resorts the list and makes the item with the specified text the focused item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ReSort()
		{
			MonitorSelectedIndexChanges = false;
			var selectedItem = _itemsListView.FocusedItem;
			_itemsListView.BeginUpdate();
			_itemsListView.Sort();
			_itemsListView.EndUpdate();
			MonitorSelectedIndexChanges = true;
			SelectItem(selectedItem, false);
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshTextOfCurrentItem(bool resortAfterRefresh)
		{
			if (CurrentItem != null)
			{
				_itemsListView.FocusedItem.Text = _itemsListView.FocusedItem.Tag.ToString();

				if (resortAfterRefresh)
					ReSort();
			}
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshTextOfAllItems(bool resortAfterRefresh)
		{
			foreach (ListViewItem item in _itemsListView.Items)
				item.Text = item.Tag.ToString();

			if (resortAfterRefresh)
				ReSort();
		}

		/// ------------------------------------------------------------------------------------
		public bool IsItemInList(object item)
		{
			return (item == null ? false : IsItemStringInList(item.ToString()));
		}

		/// ------------------------------------------------------------------------------------
		public bool IsItemStringInList(string item)
		{
			return (_itemsListView.Items.Find(item, true).Length > 0);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the focused item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void DeleteCurrentItem()
		{
			if (_itemsListView.FocusedItem == null)
				return;

			var newIndex = _itemsListView.FocusedItem.Index;

			MonitorSelectedIndexChanges = false;
			_itemsListView.Items.RemoveAt(newIndex);
			MonitorSelectedIndexChanges = true;

			if (newIndex >= _itemsListView.Items.Count)
				newIndex = _itemsListView.Items.Count - 1;

			if (_itemsListView.Items.Count > 0)
				CurrentItem = _itemsListView.Items[newIndex];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Call delete handler delegates and remove the selected items if the delegate
		/// returns true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (_itemsListView.SelectedItems.Count == 0)
				return;

			// Create a list containing each selected item.
			var itemsToDelete = new List<object>(_itemsListView.SelectedItems.Count);
			foreach (ListViewItem item in _itemsListView.SelectedItems)
				itemsToDelete.Add(item.Tag);

			if (itemsToDelete.Count == 0)
				return;

			if (BeforeItemsDeleted != null && !BeforeItemsDeleted(this, itemsToDelete))
				return;

			var currText = _itemsListView.FocusedItem.Text;
			var currIndex = _itemsListView.FocusedItem.Index;

			MonitorSelectedIndexChanges = false;

			// Remove the selected item. (This list could have been modified by a
			// delegate.)
			foreach (object obj in itemsToDelete)
			{
				var item = _itemsListView.FindItemWithText(obj.ToString());
				if (item != null)
					_itemsListView.Items.Remove(item);
			}

			MonitorSelectedIndexChanges = true;

			// Call delegates.
			if (AfterItemsDeleted != null)
				AfterItemsDeleted(this, itemsToDelete);

			// Check if the currently focused item is the same as the one that was
			// focused before removing anything from the list.
			if (_itemsListView.FocusedItem != null && currText == _itemsListView.FocusedItem.Text)
				return;

			// Try to restore the focus to the item that had it before removing items.
			var newItem = _itemsListView.FindItemWithText(currText);
			if (newItem != null)
			{
				_itemsListView.FocusedItem = newItem;
				return;
			}

			// By now, we've failed to restore the focused item, so try to give focus
			// to the item having the same index as that of the focused item before
			// any items were removed from the list.
			if (currIndex >= _itemsListView.Items.Count)
				currIndex = _itemsListView.Items.Count - 1;

			if (_itemsListView.Items.Count > 0)
				_itemsListView.Items[currIndex].Selected = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Call the new item handler delegate and add the item returned from the delegate.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnNew_Click(object sender, EventArgs e)
		{
			if (NewButtonClicked != null)
				AddItem(NewButtonClicked(this), true, true);
		}

		/// ------------------------------------------------------------------------------------
		public void AddRange(IEnumerable<object> items)
		{
			foreach (object item in items)
				AddItem(item, false, false, false);

			ReSort();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an item to the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddItem(object item, bool selectAfterAdd, bool generateSelChgEvent)
		{
			AddItem(item, selectAfterAdd, generateSelChgEvent, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds an item to the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddItem(object item, bool selectAfterAdd, bool generateSelChgEvent,
			bool reSortAfterAdd)
		{
			// REVIEW: Is it OK to do nothing if the item already exists
			// or should we throw an error or something like that?
			if (item != null && (item.ToString() == string.Empty || !IsItemInList(item)))
			{
				if (BeforeItemAdded != null)
					BeforeItemAdded(this, item);

				_itemsListView.Items.Add(item.ToString()).Tag = item;

				if (reSortAfterAdd)
					ReSort();

				if (selectAfterAdd)
					SelectItem(item, generateSelChgEvent);

				if (AfterItemAdded != null)
					AfterItemAdded(this, item);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets a value indicating whether the list view's SelectedIndexChanged
		/// event should be subscribed to. The setter will make sure that the event is never
		/// subscribed to more than once at a time.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private bool MonitorSelectedIndexChanges
		{
			set
			{
				if (_monitorSelectedIndexChanges != value)
				{
					if (value)
						_itemsListView.SelectedIndexChanged += HandleSelectedIndexChanged;
					else
						_itemsListView.SelectedIndexChanged -= HandleSelectedIndexChanged;

					_monitorSelectedIndexChanges = value;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the SelectedIndexChanged event of the lvItems control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleSelectedIndexChanged(object sender, EventArgs e)
		{
			if (_itemsListView.SelectedIndices.Count == 0)
				return;

			if (SelectedItemChanged != null && _prevFocusedItem != _itemsListView.FocusedItem)
			{
				SelectedItemChanged(this, _itemsListView.FocusedItem != null ?
					_itemsListView.FocusedItem.Tag : null);
			}

			_prevFocusedItem = _itemsListView.FocusedItem;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make it pretty behind the buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleButtonPanelPaint(object sender, PaintEventArgs e)
		{
			SpongeColors.PaintDataEntryBackground(e.Graphics,
				_buttonsFlowLayoutPanel.ClientRectangle, BorderSides.Top);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Inserts the specified button in the panel of buttons at the bottom of the control.
		/// The button will be inserted at the specified index, where zero is before the New
		/// button, 1 is between the New and Delete button and so forth.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InsertButton(int index, Button btn)
		{
			if (_buttons.Count == 0)
			{
				_buttons.Add(_newButton);
				_buttons.Add(_deleteButton);
			}

			btn.Height = _newButton.Height;
			btn.Margin = _newButton.Margin;

			if (index < 0)
				_buttons.Insert(0, btn);
			else if (index >= _buttons.Count)
				_buttons.Add(btn);
			else
				_buttons.Insert(index, btn);

			_buttonsFlowLayoutPanel.Controls.Clear();

			foreach (var b in _buttons)
				_buttonsFlowLayoutPanel.Controls.Add(b);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make sure the single list view column is a tiny bit narrower than the list view
		/// control. This will prevent the list view's horizontal scrollbar from becoming
		/// visible.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			hdrList.Width = _itemsListView.ClientSize.Width - 1;
		}

		#region ListSorter class
		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private class ListSorter : IComparer
		{
			public int Compare(object x, object y)
			{
				return string.Compare(((ListViewItem)x).Text, ((ListViewItem)y).Text);
			}
		}

		#endregion
	}
}
