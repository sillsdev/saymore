using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using SilTools;

namespace SayMore.UI.LowLevelControls
{
	public partial class MultiValueComboBox : MultiValueDropDownBox
	{
		protected readonly int _borderWidth;
		private bool _selectAllTextOnMouseDown;

		#region Constructor and Initialization
		/// ------------------------------------------------------------------------------------
		public MultiValueComboBox()
		{
			_borderWidth = (_textControl.Size.Width - _textControl.ClientSize.Width) / 2;
			var borderHeight = (_textControl.Size.Height - _textControl.ClientSize.Height) / 2;

			Padding = new Padding(_borderWidth, borderHeight, _borderWidth, borderHeight);

			_textControl.Validating += (s, e) => OnValidating(e);

			CausesValidation = true;

			_textControl.MouseDown += delegate
			{
				if (_selectAllTextOnMouseDown)
				{
					_selectAllTextOnMouseDown = false;
					TextBox.SelectAll();
				}
			};
		}

		/// ------------------------------------------------------------------------------------
		protected override Control CreateTextControl()
		{
			return new TextBox();
		}

		/// ------------------------------------------------------------------------------------
		protected override void InitializeTextControl()
		{
			base.InitializeTextControl();
			TextBox.BorderStyle = BorderStyle.None;
			_textControl.Leave += HandleTextBoxLeave;
		}
		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TextBox TextBox
		{
			get { return _textControl as TextBox; }
		}

		/// ------------------------------------------------------------------------------------
		public AutoCompleteMode AutoCompleteMode
		{
			get { return TextBox.AutoCompleteMode; }
			set { TextBox.AutoCompleteMode = value; }
		}

		/// ------------------------------------------------------------------------------------
		public AutoCompleteSource AutoCompleteSource
		{
			get { return TextBox.AutoCompleteSource; }
			set { TextBox.AutoCompleteSource = value; }
		}

		/// ------------------------------------------------------------------------------------
		[DefaultValue(false)]
		protected override bool ReadOnly
		{
			get { return TextBox.ReadOnly; }
			set { TextBox.ReadOnly = value; }
		}

		/// ------------------------------------------------------------------------------------
		public int SelectionStart
		{
			get { return TextBox.SelectionStart; }
			set { TextBox.SelectionStart = value; }
		}

		/// ------------------------------------------------------------------------------------
		public int SelectionLength
		{
			get { return TextBox.SelectionLength; }
			set { TextBox.SelectionLength = value; }
		}

		#endregion

		#region public methods
		/// ------------------------------------------------------------------------------------
		public void SelectAll()
		{
			TextBox.SelectAll();
		}
		#endregion

		#region Event handlers and overrides
		/// ------------------------------------------------------------------------------------
		private void HandleTextBoxLeave(object sender, EventArgs e)
		{
			TextBox.SelectionStart = 0;
			TextBox.SelectionLength = 0;
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleItemCheckChanged(object sender, PickerPopupItem item)
		{
			base.HandleItemCheckChanged(sender, item);
			TextBox.SelectAll();
		}
		#endregion
	}
}
