using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SIL.Sponge.Controls
{
	public partial class LabeledTextBox : UserControl
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="LabeledTextBox"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public LabeledTextBox()
		{
			InitializeComponent();
			SizeChanged += HandleSizeChanged;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="LabeledTextBox"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public LabeledTextBox(string labelText) : this()
		{
			_label.Text = labelText;
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
				_label.Font = value;
				_txtBox.Font = value;
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
				_label.BackColor = value;
				if (value != Color.Transparent)
					_txtBox.BackColor = value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the inner label.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Label InnerLabel
		{
			get { return _label; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the text associated with the control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public HoverCueTextBox InnerTextBox
		{
			get { return _txtBox; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Ensures the height of the user control is always the height of the text box plus
		/// the upper and lower padding.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandleSizeChanged(object sender, EventArgs e)
		{
			SizeChanged -= HandleSizeChanged;
			_txtBox.SizeChanged -= HandleSizeChanged;

			Height = _txtBox.Height + Padding.Top + Padding.Bottom;

			SizeChanged += HandleSizeChanged;
			_txtBox.SizeChanged += HandleSizeChanged;
		}
	}
}
