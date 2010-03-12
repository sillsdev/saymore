using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SIL.Sponge.Utilities;
using SilUtils;

namespace SIL.Sponge.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public partial class InfoPanel : UserControl
	{
		public event EventHandler MoreActionButtonClicked;
		private readonly List<LabeledTextBox> _fields = new List<LabeledTextBox>();
		private Color _labeledTextBoxBackgroundColor = SystemColors.Control;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="InfoPanel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public InfoPanel()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the create params.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x00000020;//WS_EX_TRANSPARENT
				return cp;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Color BackColor
		{
			get { return base.BackColor; }
			set
			{
				base.BackColor = value;
				LabeledTextBoxBackgroundColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the background color of the labeled text boxes when the mouse is not
		/// over them.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Color LabeledTextBoxBackgroundColor
		{
			get { return _labeledTextBoxBackgroundColor; }
			set
			{
				_labeledTextBoxBackgroundColor = value;
				foreach (var ltb in _fields)
					ltb.BackColor = value;

				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the panel with the specified labels and values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Initialize(IEnumerable fldInfo)
		{
			Utils.SetWindowRedraw(this, false);
			ClearInfo();

			if (fldInfo != null)
			{
				using (Graphics g = CreateGraphics())
				{
					int maxLblWidth = 0;
					foreach (var obj in fldInfo)
					{
						var info = obj as IInfoPanelField;
						if (info != null)
						{
							var ltb = new LabeledTextBox(info.DisplayText);
							ltb.Visible = false;
							ltb.Name = info.FieldName;
							ltb.InnerTextBox.Text = info.Value;
							ltb.Font = Font;
							ltb.BackColor = LabeledTextBoxBackgroundColor;
							var dx = TextRenderer.MeasureText(g, info.DisplayText, ltb.Font).Width;
							maxLblWidth = Math.Max(maxLblWidth, dx);
							Controls.Add(ltb);
							_fields.Add(ltb);
						}
					}

					foreach (var ltb in _fields)
					{
						ltb.InnerLabel.Width = maxLblWidth;
						ltb.Width = maxLblWidth + 150;
					}
				}
			}

			Utils.SetWindowRedraw(this, true);
			ArrangeFields();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculates how many columns are necessary to display all the fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int CalcColumnCount()
		{
			if (_fields.Count == 0)
				return 0;

			int workingHeight = ClientSize.Height - picIcon.Top;
			int heightNeeded = _fields.Count * _fields[0].Height;
			return (int)Math.Ceiling((decimal)heightNeeded / workingHeight);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Arranges the fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ArrangeFields()
		{
			if (_fields.Count == 0)
				return;

			Utils.SetWindowRedraw(this, false);

			const int dxGutter = 4;
			int cols = CalcColumnCount();
			int workingWidth = ClientSize.Width - picIcon.Right - 5;
			int width = (workingWidth / cols) - (dxGutter * (cols - 1));

			int dx = picIcon.Right + 5;
			int dy = picIcon.Top;

			foreach (var ltb in _fields)
			{
				ltb.Visible = true;
				ltb.Location = new Point(dx, dy);
				ltb.Width = width;
				dy += ltb.Height;

				if (dy + ltb.Height > ClientSize.Height)
				{
					dx += ltb.Width + dxGutter;
					dy = picIcon.Top;
				}
			}

			Utils.SetWindowRedraw(this, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Rearrange the fields when the control's size changes.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			ArrangeFields();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves the values in the panel's fields to the specified list of IInfoPanelField
		/// objects.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(IEnumerable fldInfo)
		{
			foreach (var obj in fldInfo)
			{
				var info = obj as IInfoPanelField;
				if (info != null)
				{
					var ltb = Controls[info.FieldName] as LabeledTextBox;
					if (ltb != null)
						info.Value = ltb.InnerTextBox.Text;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears the info. labels and values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ClearInfo()
		{
			foreach (var ltb in _fields)
			{
				Controls.Remove(ltb);
				ltb.Dispose();
			}

			_fields.Clear();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the font of the text displayed by the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override Font Font
		{
			get { return base.Font; }
			set
			{
				base.Font = value;
				lblFile.Font = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the icon.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Image Icon
		{
			get { return picIcon.Image; }
			set { picIcon.Image = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FileName
		{
			get { return lblFile.Text; }
			set { lblFile.Text = value; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Handles the Click event of the btnMoreAction button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void btnMoreAction_Click(object sender, EventArgs e)
		{
			if (MoreActionButtonClicked != null)
				MoreActionButtonClicked(sender, e);
		}
	}
}
