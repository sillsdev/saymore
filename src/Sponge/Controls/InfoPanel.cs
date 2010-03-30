using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Palaso.Code;
using SIL.Sponge.Model;
using SIL.Sponge.Model.MetaData;
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
		private readonly List<LabeledTextBox> _fieldControls = new List<LabeledTextBox>();
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
				foreach (var control in _fieldControls)
					control.BackColor = value;

				Invalidate();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the panel with the specified labels and values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void LoadData(IEnumerable<SessionFileField> fields)
		{
			Utils.SetWindowRedraw(this, false);
			ClearInfo();
			if (fields != null)
			{
				using (Graphics g = CreateGraphics())
				{
					int maxLblWidth = 0;
					foreach (var obj in fields)
					{
						var info = obj as IInfoPanelField;
						if (info != null && !string.IsNullOrEmpty(info.DisplayText))
						{
							var control = new LabeledTextBox(info.DisplayText);
							control.Visible = false;
							control.Name = info.Name;
							control.Value = info.Value;
							control.Font = Font;
							control.BackColor = LabeledTextBoxBackgroundColor;
							var dx = TextRenderer.MeasureText(g, info.DisplayText, control.Font).Width;
							maxLblWidth = Math.Max(maxLblWidth, dx);
							Controls.Add(control);
							_fieldControls.Add(control);
						}
					}

					foreach (var control in _fieldControls)
					{
						control.InnerLabel.Width = maxLblWidth;
						control.Width = maxLblWidth + 150;
					}
				}
			}

			Utils.SetWindowRedraw(this, true);
			ArrangeFields();
		}



		//stupid dotnet combo box control requires something like this
		public class PresetWrapper
		{
			public Dictionary<string, string> Dictionary { get; set; }
			private readonly string label;

			public PresetWrapper(string label, Dictionary<string, string> dictionary)
			{
				Dictionary = dictionary;
				this.label = label;
			}

			public override string ToString()
			{
				return label;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Calculates how many columns are necessary to display all the fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private int CalcColumnCount()
		{
			if (_fieldControls.Count == 0)
				return 0;

			int workingHeight = ClientSize.Height - picIcon.Top;
			int heightNeeded = _fieldControls.Count * _fieldControls[0].Height;
			return (int)Math.Ceiling((decimal)heightNeeded / workingHeight);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Arranges the fields.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void ArrangeFields()
		{
			if (_fieldControls.Count == 0)
				return;

			Utils.SetWindowRedraw(this, false);

			const int dxGutter = 4;
			int cols = CalcColumnCount();
			int workingWidth = ClientSize.Width - picIcon.Right - 5;
			int width = (workingWidth / cols) - (dxGutter * (cols - 1));

			int dx = picIcon.Right + 5;
			int dy = picIcon.Bottom;

			foreach (var fieldControl in _fieldControls)
			{
				fieldControl.Visible = true;
				fieldControl.Location = new Point(dx, dy);
				fieldControl.Width = width;
				dy += fieldControl.Height;

				if (dy + fieldControl.Height > ClientSize.Height)
				{
					dx += fieldControl.Width + dxGutter;
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
		public void Save(IEnumerable<SessionFileField> fields)
		{
			foreach (var field in fields)
			{
				var control = Controls[field.Name] as LabeledTextBox;
				if (control != null)
					field.Value = control.Value;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Clears the info. labels and values.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void ClearInfo()
		{
			foreach (var ltb in _fieldControls)
			{
				Controls.Remove(ltb);
				ltb.Dispose();
			}

			_fieldControls.Clear();
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

		public PresetProvider PresetProvider { get; set; }

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

		private void UsePreset(IDictionary<string, string> preset)
		{
			//just take whatever presets which have names matching fields we're showing
			foreach (var fieldControl in _fieldControls)
			{
				string value;
				if(preset.TryGetValue(fieldControl.Name, out value))
				{
					fieldControl.Value = value;
				}
			}
		}

		private void OnPresetButtonClick(object sender, EventArgs e)
		{
			_presetMenu.Items.Clear();
			Guard.AgainstNull(PresetProvider, "PresetProvider");
			if (PresetProvider != null)
			{
				foreach (KeyValuePair<string, Dictionary<string, string>> pair in PresetProvider.GetSuggestions())
				{
					KeyValuePair<string, Dictionary<string, string>> valuePair = pair;
					_presetMenu.Items.Add(pair.Key, null, (obj, send) => UsePreset(valuePair.Value));
				}
				var pt = _presetMenuButton.PointToScreen(new Point(0, _presetMenuButton.Height));
				_presetMenu.Show(pt);
			}
		}
	}
}
