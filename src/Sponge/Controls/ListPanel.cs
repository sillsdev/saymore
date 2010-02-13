using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="ListPanel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ListPanel()
		{
			InitializeComponent();
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
					list.Sort((x, y) => x.ToString().CompareTo(y.ToString()));
					foreach (object obj in list)
						lvItems.Items.Add(obj.ToString()).Tag = obj;

					CurrentItem = (currItem ?? list[0]);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the current item in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public object CurrentItem
		{
			get { return (lvItems.FocusedItem == null ? null : lvItems.FocusedItem.Tag); }
			set
			{
				if (value != null)
				{
					lvItems.FocusedItem = lvItems.FindItemWithText(value.ToString());
					lvItems.SelectedIndexChanged -= lvItems_SelectedIndexChanged;
					lvItems.SelectedItems.Clear();
					lvItems.SelectedIndexChanged += lvItems_SelectedIndexChanged;

					if (lvItems.FocusedItem != null)
						lvItems.FocusedItem.Selected = true;
				}
			}
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

			lvItems.SelectedIndexChanged -= lvItems_SelectedIndexChanged;

			// Remove the selected item. (This list could have been modified by a
			// delegate.)
			foreach (object obj in itemsToDelete)
			{
				var item = lvItems.FindItemWithText(obj.ToString());
				if (item != null)
					lvItems.Items.Remove(item);
			}

			lvItems.SelectedIndexChanged += lvItems_SelectedIndexChanged;

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
			if (NewButtonClicked == null)
				return;

			var newItem = NewButtonClicked(this);
			if (newItem != null)
			{
				// TODO: Insert item in sorted order.
				lvItems.SelectedIndexChanged -= lvItems_SelectedIndexChanged;
				lvItems.Items.Add(newItem.ToString()).Tag = newItem;
				lvItems.SelectedItems.Clear();
				lvItems.SelectedIndexChanged += lvItems_SelectedIndexChanged;
				CurrentItem = newItem;
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
	}
}
