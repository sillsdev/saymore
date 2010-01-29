using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SIL.Sponge.Controls
{
	public partial class ListPanel : UserControl
	{
		public delegate void SelectedItemChangedHandler(object sender, string newItem);
		public event SelectedItemChangedHandler SelectedItemChanged;

		public delegate bool DeleteButtonClickHandler(object sender, List<string> itemsToDelete);
		public event DeleteButtonClickHandler DeleteButtonClicked;

		public delegate string NewButtonClickedHandler(object sender);
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
		public string[] Items
		{
			get
			{
				if (lvItems.Items.Count == 0)
					return new string[] { };

				var items = new List<string>(lvItems.Items.Count);
				foreach (ListViewItem item in lvItems.Items)
					items.Add(item.Text);

				return items.ToArray();
			}
			set
			{
				lvItems.Items.Clear();
				if (value != null)
				{
					foreach (string text in value)
						lvItems.Items.Add(text);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the current item in the list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CurrentItem
		{
			get { return (lvItems.FocusedItem == null ? null : lvItems.FocusedItem.Text); }
			set
			{
				if (value != null)
				{
					lvItems.FocusedItem = lvItems.FindItemWithText(value);
					lvItems.SelectedIndexChanged -= lvItems_SelectedIndexChanged;
					lvItems.SelectedItems.Clear();
					lvItems.SelectedIndexChanged += lvItems_SelectedIndexChanged;
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
			if (lvItems.SelectedItems.Count == 0 || DeleteButtonClicked == null)
				return;

			// Create a list with the text of each selected item.
			var itemsToDelete = new List<string>(lvItems.SelectedItems.Count);
			foreach (ListViewItem item in lvItems.SelectedItems)
				itemsToDelete.Add(item.Text);

			// Call delegates.
			if (!DeleteButtonClicked(this, itemsToDelete))
				return;

			var currText =  lvItems.FocusedItem.Text;
			var currIndex = lvItems.FocusedItem.Index;

			lvItems.SelectedIndexChanged -= lvItems_SelectedIndexChanged;

			// Remove the selected item. (This list could have been modified by a
			// delegate.)
			foreach (string text in itemsToDelete)
			{
				var item = lvItems.FindItemWithText(text);
				if (item != null)
					lvItems.Items.Remove(item);
			}

			lvItems.SelectedIndexChanged += lvItems_SelectedIndexChanged;

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
				lvItems.Items.Add(newItem);
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
					lvItems.FocusedItem.Text : null);
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
				SpongeBar.DefaultSpongeBarColorBegin, SpongeBar.DefaultSpongeBarColorEnd, -30))
			{
				e.Graphics.FillRectangle(br, pnlButtons.ClientRectangle);
			}

			using (var pen = new Pen(SpongeBar.DefaultSpongeBarColorEnd))
				e.Graphics.DrawLine(pen, 0, 0, pnlButtons.Width, 0);
		}
	}
}
