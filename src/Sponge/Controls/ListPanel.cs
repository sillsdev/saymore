using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SIL.Sponge.Properties;
using SilUtils;

namespace SIL.Sponge.Controls
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
		private Font m_fntDupItem;

		private bool m_monitorSelectedIndexChanges;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ListPanel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ListPanel()
		{
			ReSortWhenItemTextChanges = false;

			InitializeComponent();
			m_fntDupItem = new Font(lvItems.Font, FontStyle.Italic | FontStyle.Bold);
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
				m_fntDupItem.Dispose();
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
			base.OnResize(e);
			hdrList.Width = lvItems.ClientSize.Width - 1;
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
				lvi.Font = m_fntDupItem;
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
			m_fntDupItem.Dispose();
			m_fntDupItem = new Font(lvItems.Font, FontStyle.Italic | FontStyle.Bold);
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
				if (m_monitorSelectedIndexChanges != value)
				{
					if (value)
						lvItems.SelectedIndexChanged += lvItems_SelectedIndexChanged;
					else
						lvItems.SelectedIndexChanged -= lvItems_SelectedIndexChanged;

					m_monitorSelectedIndexChanges = value;
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
			using (var br = new LinearGradientBrush(pnlButtons.ClientRectangle,
				SpongeBar.DefaultSpongeBarColorBegin, SpongeBar.DefaultSpongeBarColorEnd, -30f))
			{
				e.Graphics.FillRectangle(br, pnlButtons.ClientRectangle);
			}

			using (var pen = new Pen(SpongeBar.DefaultSpongeBarColorEnd))
				e.Graphics.DrawLine(pen, 0, 0, pnlButtons.Width, 0);
		}

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
	}
}
