using System;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SIL.Windows.Forms;
using SayMore.Utilities;

namespace SayMore.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Control encapsulating a heading, list view and 'New'/'Delete' buttons.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class ListPanel : UserControl
	{
		public event EventHandler NewButtonClicked;
		public event EventHandler DeleteButtonClicked;

		private readonly List<Button> _buttons = new List<Button>();

		public Color ButtonPanelBackColor1 { get; set; }
		public Color ButtonPanelBackColor2 { get; set; }
		public Color ButtonPanelTopBorderColor { get; set; }

		public Color HeaderPanelBackColor1 { get; set; }
		public Color HeaderPanelBackColor2 { get; set; }
		public Color HeaderPanelBottomBorderColor { get; set; }

		protected Control _listControl;
		protected bool _showColumnChooserButton;

		/// ------------------------------------------------------------------------------------
		public ListPanel()
		{
			ButtonPanelBackColor1 = SystemColors.Control;
			ButtonPanelBackColor2 = SystemColors.Control;
			ButtonPanelTopBorderColor = SystemColors.ControlDark;

			HeaderPanelBackColor1 = SystemColors.Control;
			HeaderPanelBackColor2 = SystemColors.Control;
			HeaderPanelBottomBorderColor = SystemColors.ControlDark;

			InitializeComponent();

			_headerLabel.MouseDown += (sender, args) => base.OnMouseDown(args);

			if (DesignMode)
				return;

			_headerLabel.Font = FontHelper.MakeFont(Program.DialogFont, FontStyle.Bold);
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
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Control ListControl
		{
			get { return _listControl; }
			set
			{
				if (_listControl != null)
					_outerPanel.Controls.Remove(_listControl);

				_listControl = value;
				_listControl.Dock = DockStyle.Fill;
				_outerPanel.Controls.Add(_listControl);
				_listControl.BringToFront();
				_listControl.TabIndex = 0;

				InitializeColumnChooser();
			}
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(false)]
		public bool ShowColumnChooserButton
		{
			get { return _showColumnChooserButton; }
			set
			{
				_showColumnChooserButton = value;
				InitializeColumnChooser();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void InitializeColumnChooser()
		{
			if (DesignMode)
			{
				_buttonColChooser.Visible = _showColumnChooserButton;
				return;
			}

			_buttonColChooser.Visible = (_showColumnChooserButton && _listControl is DataGridView);
			_buttonColChooser.Grid = _listControl as DataGridView;
		}

		/// ------------------------------------------------------------------------------------
		public new string Name
		{
			get { return base.Name; }
			set
			{
				var prevName = base.Name;
				base.Name = value;

				if (!string.IsNullOrEmpty(prevName))
				{
					//_buttonDelete.Name = _buttonDelete.Name.Replace(prevName + "_", string.Empty);
					_buttonNew.Name = _buttonNew.Name.Replace(prevName + "_", string.Empty);
				}

				if (!string.IsNullOrEmpty(value))
				{
					// Setting these names are for the sake of testing.
					//_buttonDelete.Name = string.Format("{0}_{1}", value, _buttonDelete.Name);
					_buttonNew.Name = string.Format("{0}_{1}", value, _buttonNew.Name);
				}
			}
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
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Button NewButton
		{
			get { return _buttonNew; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Call delete handler delegates and remove the selected items if the delegate
		/// returns true.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleDeleteButtonClick(object sender, EventArgs e)
		{
			if (DeleteButtonClicked != null)
				DeleteButtonClicked(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Call the new item handler delegate and add the item returned from the delegate.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleNewButtonClick(object sender, EventArgs e)
		{
			if (NewButtonClicked != null)
				NewButtonClicked(this, e);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make it pretty behind the buttons.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleButtonPanelPaint(object sender, PaintEventArgs e)
		{
			var rc = _buttonsFlowLayoutPanel.ClientRectangle;

			using (var br = new LinearGradientBrush(rc, ButtonPanelBackColor1,
				ButtonPanelBackColor2, 135))
			{
				e.Graphics.FillRectangle(br, rc);
			}

			AppColors.PaintBorder(e.Graphics, ButtonPanelTopBorderColor, rc, BorderSides.Top);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleHeaderPanelPaint(object sender, PaintEventArgs e)
		{
			var rc = _headerLabel.ClientRectangle;

			using (var br = new LinearGradientBrush(rc, HeaderPanelBackColor1,
				HeaderPanelBackColor2, 135))
			{
				e.Graphics.FillRectangle(br, rc);
			}

			TextRenderer.DrawText(e.Graphics, _headerLabel.Text, _headerLabel.Font, rc,
				_headerLabel.ForeColor, TextFormatFlags.VerticalCenter);

			AppColors.PaintBorder(e.Graphics, HeaderPanelBottomBorderColor, rc, BorderSides.Bottom);
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
				_buttons.Add(_buttonNew);

			btn.Height = _buttonNew.Height;
			btn.Margin = _buttonNew.Margin;

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
	}
}
