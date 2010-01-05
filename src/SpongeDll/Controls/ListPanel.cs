using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SIL.Sponge
{
	public partial class ListPanel : UserControl
	{
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
		/// Gets or sets the default text of a new item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string NewItemText { get; set; }

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
		/// Gets the list of items.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ListView.ListViewItemCollection Items
		{
			get { return lvItems.Items; }
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
		/// Make sure the single list view column is a tiny bit narrower than the list view
		/// control. This will prevent the list view's horizontal scrollbar from becoming
		/// visible.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			hdrList.Width = lvItems.ClientSize.Width - 1;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Add a new item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (lvItems.SelectedItems.Count == 0)
				return;

			// REVIEW: do we want to support deleting more than one items at a time?
			int i = lvItems.SelectedIndices[0];

			lvItems.Items.Remove(lvItems.SelectedItems[0]);
			if (lvItems.Items.Count == 0)
				return;

			if (i == lvItems.Items.Count)
				i--;

			lvItems.Items[i].Selected = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Delete the selected item.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnNew_Click(object sender, EventArgs e)
		{
			var item = lvItems.Items.Add(NewItemText ?? "New Item");
			lvItems.SelectedItems.Clear();
			item.Selected = true;
			item.BeginEdit();
		}
	}
}
