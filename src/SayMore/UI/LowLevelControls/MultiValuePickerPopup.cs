using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SayMore.Model.Fields;

namespace SayMore.UI.LowLevelControls
{
	public partial class MultiValuePickerPopup : PopupControl
	{
		public delegate void ItemCheckChangedHandler(object sender, PickerPopupItem item);
		public event ItemCheckChangedHandler ItemCheckChanged;

		private readonly Timer _timer;
		private readonly ToolTip _tooltip;
		private bool _showPromptTextBox;
		private readonly HashSet<PickerPopupItem> _items = new HashSet<PickerPopupItem>();

		/// ------------------------------------------------------------------------------------
		public MultiValuePickerPopup()
		{
			MaxItemsDisplayed = 8;

			InitializeComponent();

			// Will paint over the toolstrip's bottom border (thereby erasing it).
			_toolStripItems.Renderer.RenderToolStripBorder += ((sender, e) =>
			{
				var rc = e.AffectedBounds;
				rc.Height = 2;
				rc.Y = _toolStripItems.Height - 2;
				using (var br = new SolidBrush(BackColor))
					e.Graphics.FillRectangle(br, rc);
			});

			_panelTextBox.Font = _toolStripItems.Font;
			_timer = new Timer { Interval = 750 };
			_timer.Tick += HandleTimerTick;
			_tooltip = new ToolTip();
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the number of items displayed at a time.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DefaultValue(8)]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int MaxItemsDisplayed { get; set; }

		/// ------------------------------------------------------------------------------------
		[DefaultValue(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public bool ShowPromptTextBox
		{
			get { return _showPromptTextBox; }
			set
			{
				_showPromptTextBox = value;
				_panelTextBox.Visible = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		public IEnumerable<PickerPopupItem> AllItems
		{
			get { return _items.ToArray(); }
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
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
		public void Clear()
		{
			for (int i = _items.Count - 1; i >= 0; i--)
				_items.ElementAt(i).Dispose();

			_items.Clear();
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

		#region Methods for showing, closing, loading and sizing popup
		/// ------------------------------------------------------------------------------------
		public override void ShowPopup(Point pt)
		{
			ShowPopup(pt, _items);
		}

		/// ------------------------------------------------------------------------------------
		public void ShowPopup(Point pt, IEnumerable<PickerPopupItem> pickerList)
		{
			_panelTextBox.Visible = _showPromptTextBox;
			LoadValues(pickerList);
			base.ShowPopup(pt);
			_toolStripItems.Focus();
		}

		/// ------------------------------------------------------------------------------------
		public void LoadValues(IEnumerable<PickerPopupItem> pickerList)
		{
			_panelItems.AutoScrollPosition = new Point(0, 0);
			_panelCheckboxes.Controls.Clear();
			_toolStripItems.Items.Clear();
			_toolStripItems.Items.AddRange(pickerList.ToArray());

			foreach (var item in pickerList)
			{
				_panelCheckboxes.Controls.Add(item.CheckBox);
				_tooltip.SetToolTip(item.CheckBox, item.ToolTipText);
				item.MouseEnter += HandleItemMouseEnter;
				item.MouseLeave += HandleItemMouseLeave;
				item.CheckBox.MouseEnter += HandleItemMouseEnter;
				item.CheckBox.MouseLeave += HandleItemMouseLeave;
			}

			SetSizesOfControls();
		}

		/// ------------------------------------------------------------------------------------
		private void SetSizesOfControls()
		{
			_panelTextBox.Height = (_panelTextBox.Padding.Top * 2) + _textBoxPrompt.Height;
			var sz = _toolStripItems.PreferredSize;
			_panelCheckboxes.Height = _toolStripItems.Height = sz.Height;

			if (_toolStripItems.Items.Count > 0)
			{
				var itemHeight = _toolStripItems.Items[0].Height +
					_toolStripItems.Items[0].Margin.Top + _toolStripItems.Items[0].Margin.Bottom;

				sz.Height = Math.Min(sz.Height, MaxItemsDisplayed * itemHeight);
			}

			if (ShowPromptTextBox)
				sz.Height += _panelTextBox.Height;

			sz.Height += 4;			// account for borders.
			sz.Width = Width;
			Size = sz;
		}

		#endregion

		#region Methods for handling checked items in delimited strings.
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
			if (builder.Length > 2)
				builder.Length -= 2;

			return builder.ToString();
		}

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

			foreach (var text in FieldInstance.GetMultipleValuesFromText(items))
				CheckItem(text);
		}

		#endregion

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		private void HandleItemDisposed(object sender, EventArgs e)
		{
			var item = sender as PickerPopupItem;
			item.CheckedChanged -= HandleItemCheckedChanged;
			item.Disposed -= HandleItemDisposed;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleItemCheckedChanged(PickerPopupItem item,
			PickerPopupItem.ItemSelectMode selectMode)
		{
			if (ItemCheckChanged != null)
				ItemCheckChanged(this, item);

			if (selectMode == PickerPopupItem.ItemSelectMode.Mouse)
				ClosePopup();
		}

		/// ------------------------------------------------------------------------------------
		void HandleItemMouseEnter(object sender, EventArgs e)
		{
			_timer.Tag = (sender is CheckBox ? ((CheckBox)sender).Tag : sender);
			_timer.Start();
		}

		/// ------------------------------------------------------------------------------------
		void HandleItemMouseLeave(object sender, EventArgs e)
		{
			_timer.Stop();
			_timer.Tag = null;
		}

		/// ------------------------------------------------------------------------------------
		void HandleTimerTick(object sender, EventArgs e)
		{
			_timer.Stop();
			var item = _timer.Tag as ToolStripItem;

			// By now, if the item the user is hovering over is not fully visible,
			// this will ensure that it gets bumped down or up a bit so it is.
			var scrollPosY = _panelItems.AutoScrollPosition.Y;
			var rc = item.Bounds;
			rc.Height += (item.Margin.Top + item.Margin.Bottom);
			var topEdge = -(item.Bounds.Top - item.Margin.Top);
			var bottomEdge = rc.Bottom + scrollPosY;

			if (topEdge > scrollPosY)
				_panelItems.AutoScrollPosition = new Point(0, -topEdge);
			else if (bottomEdge > _panelItems.ClientSize.Height)
			{
				var dy = bottomEdge - _panelItems.ClientSize.Height;
				_panelItems.AutoScrollPosition = new Point(0, -scrollPosY + dy);
			}
		}

		#endregion

		#region Painting handlers
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw a line between check boxes and toolbar buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleCheckBoxPanelPaint(object sender, PaintEventArgs e)
		{
			var sz = _panelCheckboxes.ClientSize;

			using (var pen = new Pen(Color.FromArgb(50, Color.Black)))
				e.Graphics.DrawLine(pen, sz.Width - 1, 2, sz.Width - 1, sz.Height - 3);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Draw a line between check boxes and prompt text box.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleTextBoxPanelPaint(object sender, PaintEventArgs e)
		{
			var sz = _panelTextBox.ClientSize;

			using (var pen = new Pen(Color.FromArgb(50, Color.Black)))
				e.Graphics.DrawLine(pen, 2, sz.Height - 1, sz.Width - 3, sz.Height - 1);
		}

		#endregion
	}
}
