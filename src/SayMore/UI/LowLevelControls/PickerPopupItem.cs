using System;
using System.Drawing;
using System.Windows.Forms;

namespace SayMore.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	public class PickerPopupItem : ToolStripButton
	{
		public enum ItemSelectMode
		{
			CheckBox,
			Keyboard,
			Mouse
		}

		private bool _mouseClickSelect;
		static Size s_checkBoxRectangleSize;

		public delegate void CheckChangedHandler(PickerPopupItem sender, ItemSelectMode selectMode);
		public new event CheckChangedHandler CheckedChanged;

		public CheckBox CheckBox { get; private set; }

		/// ------------------------------------------------------------------------------------
		public PickerPopupItem()
		{
			DisplayStyle = ToolStripItemDisplayStyle.Text;
			Margin = new Padding(1, 1, 2, 2);
			Overflow = ToolStripItemOverflow.Never;
			TextAlign = ContentAlignment.MiddleLeft;

			CheckBox = new CheckBox
			{
				BackColor = Color.Magenta,
				AutoSize = true,
				UseVisualStyleBackColor = true,
				Location = new Point(0, -50),
				Tag = this
			};

			CheckBox.CheckedChanged += HandleCheckBoxCheckChanged;
		}

		/// ------------------------------------------------------------------------------------
		public PickerPopupItem(string text, bool chked) : this()
		{
			Text = text;
			Checked = chked;
		}

		/// ------------------------------------------------------------------------------------
		public new bool Checked
		{
			get { return CheckBox.Checked; }
			set
			{
				CheckBox.CheckedChanged -= HandleCheckBoxCheckChanged;
				CheckBox.Checked = value;
				CheckBox.CheckedChanged += HandleCheckBoxCheckChanged;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				CheckBox.CheckedChanged -= HandleCheckBoxCheckChanged;

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnMouseDown(MouseEventArgs e)
		{
			_mouseClickSelect = (e.Button == MouseButtons.Left);
			base.OnMouseDown(e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			Checked = !Checked;
			OnCheckedChanged(_mouseClickSelect ? ItemSelectMode.Mouse : ItemSelectMode.Keyboard);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCheckBoxCheckChanged(object sender, EventArgs e)
		{
			OnCheckedChanged(ItemSelectMode.CheckBox);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnCheckedChanged(ItemSelectMode selectMode)
		{
			_mouseClickSelect = false;

			if (CheckedChanged != null)
				CheckedChanged(this, selectMode);

			if (selectMode != ItemSelectMode.Mouse)
			{
				Parent.Focus();
				Select();
			}
		}

		private Size CheckBoxRectSize
		{
			get
			{
				if (s_checkBoxRectangleSize.Width <= 0)
				{
					// At this point, the AutoSize property is true so we know the entire
					// box part of the check box control is visible. Write it to a bit map.
					using (var bmp = new Bitmap(CheckBox.Width, CheckBox.Height))
					{
						Size checkBoxSize = CheckBox.Size;
						CheckBox.DrawToBitmap(bmp, new Rectangle(new Point(), checkBoxSize));

						// Find the rectangle that is occupied by just the box portion of the check
						// box control by check pixel colors from rigth-to-left and and bottom-to-top.
						s_checkBoxRectangleSize = bmp.Size;
						var clr = CheckBox.BackColor;
						while (bmp.GetPixel(s_checkBoxRectangleSize.Width - 1, 0) == clr)
							s_checkBoxRectangleSize.Width--;

						while (bmp.GetPixel(0, s_checkBoxRectangleSize.Height - 1) == clr)
							s_checkBoxRectangleSize.Height--;
					}
				}

				return s_checkBoxRectangleSize;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// One thing that's difficult with visual styles is to know exactly what the size of
		/// a checkbox is. Setting the AutoSize property to true includes some padding, so
		/// in order to find exactly what size the box itself is, we go through some
		/// gyrations to find it. Then set the AutoSize property to false and the size to
		/// what has been determined.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (CheckBox.BackColor == Color.Transparent)
				return;

			var sz = CheckBoxRectSize;

			// Force the size of the check box control to only accommodate the box portion.
			CheckBox.BackColor = Color.Transparent;
			CheckBox.AutoSize = false;
			CheckBox.Size = new Size(sz.Width, sz.Height);

			// Position the check box accordingly.
			var dy = Bounds.Top + (int)Math.Round((Bounds.Height - CheckBox.Height) / 2f, MidpointRounding.AwayFromZero);
			var dx = (int)Math.Round((CheckBox.Parent.ClientSize.Width - sz.Width) / 2f, MidpointRounding.AwayFromZero);
			CheckBox.Location = new Point(dx, dy);
		}
	}
}
