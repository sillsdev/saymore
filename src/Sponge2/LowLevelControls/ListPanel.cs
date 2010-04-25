using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Sponge2.Properties;
using Sponge2.Utilities;
using SilUtils;

namespace Sponge2.Controls
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

		// This font is used to indicate the text of the current item is a duplicate.
		private Font _fntDupItem;

		private bool _monitorSelectedIndexChanges;
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
			_fntDupItem = new Font(lvItems.Font, FontStyle.Italic | FontStyle.Bold);
			lvItems.ListViewItemSorter = new ListSorter();
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
			get { return hlblItems.Text; }
			set { hlblItems.Text = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list view.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ListView ListView
		{
			get { return lvItems; }
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
				if (lvItems.Items.Count == 0)
					return new object[] { };

				var items = new List<object>(lvItems.Items.Count);
				foreach (ListViewItem item in lvItems.Items)
					items.Add(item.Tag);

				return items.ToArray();
			}
			set
			{
				var currItem = CurrentItem;
				lvItems.Items.Clear();
				if (value != null && value.Length > 0)
				{
					var list = new List<object>(value);
					list.Sort((x, y) => (x.ToString() ?? string.Empty).CompareTo(y.ToString() ?? string.Empty));
					foreach (object obj in list)
					{
						var newLvItem = lvItems.Items.Add(obj.ToString());
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
			get { return (lvItems.FocusedItem == null ? null : lvItems.FocusedItem.Tag); }
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
				lvItems.FocusedItem = lvItems.FindItemWithText(item as string);
			else if (item is ListViewItem)
				lvItems.FocusedItem = (item as ListViewItem);
			else
				lvItems.FocusedItem = FindListViewItem(item);

			lvItems.SelectedItems.Clear();

			if (lvItems.FocusedItem != null)
				lvItems.FocusedItem.Selected = true;

			if (generateSelChgEvent && SelectedItemChanged != null)
			{
				SelectedItemChanged(this, lvItems.FocusedItem != null ?
					lvItems.FocusedItem.Tag : null);
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
			foreach (ListViewItem lvi in lvItems.Items)
			{
				if (lvi.Tag == item)
					return lvi;
			}

			return null;
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
			//HACK (jh): the vertical scroll bar refuses to show.  Some code I don't see is messing with the anchoring
			// and perhaps the size of the list view.  Argghhhh.  Hence the next few, desperate lines

			lvItems.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
			base.OnResize(e);
			lvItems.Height = this.Height - (pnlButtons.Height + 50);
			hdrList.Width = lvItems.ClientSize.Width - 1;
			lvItems.Width = Width - 9;


			pnlButtons.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gives focus to the list view portion of the list panel.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public new void Focus()
		{
			lvItems.Focus();
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
			if ((i > 0 && lvi.Text == lvItems.Items[i - 1].Text) ||
				(i < lvItems.Items.Count - 1 && lvi.Text == lvItems.Items[i + 1].Text))
			{
				// Change the look of the current item if it's text is duplicated.
				lvi.ForeColor = Color.Red;
				lvi.BackColor = Color.PapayaWhip;
				lvi.Font = _fntDupItem;
				lvItems.HideSelection = true;
			}
			else
			{
				// Restore the look of the current item to its default.
				lvi.ForeColor = lvItems.ForeColor;
				lvi.BackColor = lvItems.BackColor;
				lvi.Font = lvItems.Font;
				lvItems.HideSelection = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines the preferred index at which to insert the specified string.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int InsertIndex(string text)
		{
			for (int i = 0; i < lvItems.Items.Count; i++)
			{
				if (lvItems.Items[i].Text.CompareTo(text) > 0)
					return i;
			}

			return lvItems.Items.Count;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Recreate the font for duplicated items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvItems_FontChanged(object sender, EventArgs e)
		{
			_fntDupItem.Dispose();
			_fntDupItem = new Font(lvItems.Font, FontStyle.Italic | FontStyle.Bold);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Resorts the list and makes the item with the specified text the focused item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ReSort()
		{
			MonitorSelectedIndexChanges = false;
			var selectedItem = lvItems.FocusedItem;
			lvItems.BeginUpdate();
			lvItems.Sort();
			lvItems.EndUpdate();
			MonitorSelectedIndexChanges = true;
			SelectItem(selectedItem, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the focused item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void DeleteCurrentItem()
		{
			if (lvItems.FocusedItem == null)
				return;

			var newIndex = lvItems.FocusedItem.Index;

			MonitorSelectedIndexChanges = false;
			lvItems.Items.RemoveAt(newIndex);
			MonitorSelectedIndexChanges = true;

			if (newIndex >= lvItems.Items.Count)
				newIndex = lvItems.Items.Count - 1;

			if (lvItems.Items.Count > 0)
				CurrentItem = lvItems.Items[newIndex];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Call delete handler delegates and remove the selected items if the delegate
		/// returns true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (lvItems.SelectedItems.Count == 0)
				return;

			// Create a list containing each selected item.
			var itemsToDelete = new List<object>(lvItems.SelectedItems.Count);
			foreach (ListViewItem item in lvItems.SelectedItems)
				itemsToDelete.Add(item.Tag);

			if (itemsToDelete.Count == 0)
				return;

			if (BeforeItemsDeleted != null && !BeforeItemsDeleted(this, itemsToDelete))
				return;

			var currText = lvItems.FocusedItem.Text;
			var currIndex = lvItems.FocusedItem.Index;

			MonitorSelectedIndexChanges = false;

			// Remove the selected item. (This list could have been modified by a
			// delegate.)
			foreach (object obj in itemsToDelete)
			{
				var item = lvItems.FindItemWithText(obj.ToString());
				if (item != null)
					lvItems.Items.Remove(item);
			}

			MonitorSelectedIndexChanges = true;

			// Call delegates.
			if (AfterItemsDeleted != null)
				AfterItemsDeleted(this, itemsToDelete);

			// Check if the currently focused item is the same as the one that was
			// focused before removing anything from the list.
			if (lvItems.FocusedItem != null && currText == lvItems.FocusedItem.Text)
				return;

			// Try to restore the focus to the item that had it before removing items.
			var newItem = lvItems.FindItemWithText(currText);
			if (newItem != null)
			{
				lvItems.FocusedItem = newItem;
				return;
			}

			// By now, we've failed to restore the focused item, so try to give focus
			// to the item having the same index as that of the focused item before
			// any items were removed from the list.
			if (currIndex >= lvItems.Items.Count)
				currIndex = lvItems.Items.Count - 1;

			if (lvItems.Items.Count > 0)
				lvItems.Items[currIndex].Selected = true;
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
		/// <summary>
		/// Adds an item to the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddItem(object item, bool selectAfterAdd, bool generateSelChgEvent)
		{
			// REVIEW: Is it OK to do nothing if the item already exists
			// or should we throw an error or something like that?
			if (item != null && (item.ToString() == string.Empty ||
				lvItems.Items.Find(item.ToString(), true).Length == 0))
			{
				lvItems.Items.Add(item.ToString()).Tag = item;
				ReSort();

				if (selectAfterAdd)
					SelectItem(item, generateSelChgEvent);
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
						lvItems.SelectedIndexChanged += lvItems_SelectedIndexChanged;
					else
						lvItems.SelectedIndexChanged -= lvItems_SelectedIndexChanged;

					_monitorSelectedIndexChanges = value;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the SelectedIndexChanged event of the lvItems control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SelectedItemChanged != null)
			{
				SelectedItemChanged(this, lvItems.FocusedItem != null ?
					lvItems.FocusedItem.Tag : null);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make it pretty behind the buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void pnlButtons_Paint(object sender, PaintEventArgs e)
		{
			SpongeColors.PaintDataEntryBackground(e.Graphics, pnlButtons.ClientRectangle, BorderSides.Top);
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
				_buttons.Add(btnNew);
				_buttons.Add(btnDelete);
			}

			btn.Height = btnNew.Height;

			if (index < 0)
				_buttons.Insert(0, btn);
			else if (index >= _buttons.Count)
				_buttons.Add(btn);
			else
				_buttons.Insert(index, btn);

			foreach (var b in _buttons)
				b.Anchor = AnchorStyles.Top | AnchorStyles.Left;

			pnlButtons.Controls.Add(btn);
			HandleButtonPanelClientSizeChanged(null, null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Arrange the buttons in the button panels into rows, from left to right, and wrap
		/// around when a button will extend beyond the right edge of the panel. Then adjust
		/// the height of the panel to accomodate the stack of buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleButtonPanelClientSizeChanged(object sender, EventArgs e)
		{
			Utils.SetWindowRedraw(this, false);

			const int horizMargin = 5;
			const int vertMargin = 5;

			int x = horizMargin;
			int y = vertMargin;

			foreach (var btn in _buttons)
			{
				if (x + btn.Width > (pnlButtons.ClientSize.Width - horizMargin))
				{
					x = horizMargin;
					y += btn.Height + vertMargin;
				}

				var pt = new Point(x, y);
				if (btn.Location != pt)
					btn.Location = pt;

				x += (btn.Width + horizMargin);
			}

			pnlButtons.ClientSizeChanged -= HandleButtonPanelClientSizeChanged;
			pnlButtons.Height = _buttons[_buttons.Count - 1].Bottom + vertMargin;
			pnlButtons.Top = ClientSize.Height - pnlButtons.Height;
			pnlButtons.ClientSizeChanged += HandleButtonPanelClientSizeChanged;

			Utils.SetWindowRedraw(this, true);
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
