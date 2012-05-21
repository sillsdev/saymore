using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
using SayMore.Model;
using SayMore.Model.Files;
using SilTools;
using Color = System.Drawing.Color;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class StatusAndStagesEditor : EditorBase
	{
		public delegate StatusAndStagesEditor Factory(ComponentFile file, string imageKey);
		private List<RadioButton> _statusRadioButtons;
		private List<CheckBox> _stageCheckBoxes;
		private ComponentRole[] _componentRoles;

		/// ----------------------------------------------------------------------------------------
		public StatusAndStagesEditor(ComponentFile file, string imageKey,
			IEnumerable<ComponentRole> componentRoles) : base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "StatusAndStages";

			_componentRoles = componentRoles.ToArray();

			AddStatusFields();
			AddStageFields();
			_tableLayoutOuter.ColumnStyles[0].SizeType = SizeType.AutoSize;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			SetFonts();
			UpdateDisplay();
		}

		/// ----------------------------------------------------------------------------------------
		private void SetFonts()
		{
			_labelReadAboutStatus.Font = Program.DialogFont;
			_labelReadAboutStages.Font = Program.DialogFont;
			_labelStatusHint.Font = Program.DialogFont;
			_labelStagesHint.Font = Program.DialogFont;

			_labelStatus.Font = FontHelper.MakeFont(Program.DialogFont,
				Program.DialogFont.SizeInPoints + 1, FontStyle.Bold);

			_labelStages.Font = _labelStatus.Font;
		}

		#region Methods for adding status and stage controls to the table layout
		/// ------------------------------------------------------------------------------------
		private void AddStatusFields()
		{
			_statusRadioButtons = new List<RadioButton>(Enum.GetValues(typeof(Event.Status)).Length);

			int row = _tableLayoutOuter.GetRow(_labelStatus) + 1;
			int lastRow = _tableLayoutOuter.GetRow(_labelStages) - 1;

			for (int i = row; i <= lastRow; i++)
				_tableLayoutOuter.RowStyles[i].SizeType = SizeType.AutoSize;

			foreach (var status in Enum.GetValues(typeof(Event.Status)).Cast<Event.Status>())
			{
				AddStatusImage(row, status.ToString());
				AddStatusRadioButton(row++, status);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void AddStatusImage(int row, string statusName)
		{
			var statusPicBox = new PictureBox
			{
				Name = "picture_" + statusName,
				Anchor = AnchorStyles.Left,
				Image = (Image)Properties.Resources.ResourceManager.GetObject("Status" + statusName),
				Margin = new Padding(5, 0, 0, 0),
				SizeMode = PictureBoxSizeMode.AutoSize,
			};

			if (row == _tableLayoutOuter.GetRow(_labelStages))
				_tableLayoutOuter.RowStyles.Insert(row, new RowStyle(SizeType.AutoSize));

			_tableLayoutOuter.Controls.Add(statusPicBox, 0, row);
		}

		/// ------------------------------------------------------------------------------------
		private void AddStatusRadioButton(int row, Event.Status status)
		{
			var statusRadioButton = new RadioButton
			{
				Name = "radioButton_" + status,
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				AutoSize = true,
				Margin = new Padding(10, 1, 0, 2),
				Text = Event.GetLocalizedStatus(status.ToString()),
				UseVisualStyleBackColor = true,
				Tag = status,
			};

			statusRadioButton.CheckedChanged += (s, e) =>
			{
				var radioButton = (RadioButton)s;
				if (radioButton.Checked)
					SaveFieldValue("status", radioButton.Tag.ToString());
			};

			_tableLayoutOuter.Controls.Add(statusRadioButton, 1, row);
			_statusRadioButtons.Add(statusRadioButton);
		}

		/// ----------------------------------------------------------------------------------------
		private void AddStageFields()
		{
			int row = _tableLayoutOuter.GetRow(_labelStages) + 1;

			for (int i = row; i < _tableLayoutOuter.RowCount; i++)
				_tableLayoutOuter.RowStyles[i].SizeType = SizeType.AutoSize;

			_stageCheckBoxes = new List<CheckBox>(_componentRoles.Length);

			foreach (var role in _componentRoles)
			{
				AddColorBlockLabel(row, role.Color);
				AddStageCheckBox(row++, role);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void AddColorBlockLabel(int row, Color roleColor)
		{
			var colorBlock = new Label
			{
				AutoSize = false,
				Anchor = AnchorStyles.Top,
				Size = new Size(10, 14),
				Margin = new Padding(0, 4, 0, 2),
				BackColor = roleColor
			};

			colorBlock.Paint += HandleStagesColorBlockPaint;

			if (row == _tableLayoutOuter.RowCount)
				_tableLayoutOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			_tableLayoutOuter.Controls.Add(colorBlock, 0, row);
		}

		/// ------------------------------------------------------------------------------------
		private void AddStageCheckBox(int row, ComponentRole role)
		{
			var stageCheckBox = new CheckBox
			{
				Name = "checkBoxStage_" + role.Name,
				Font = Program.DialogFont,
				AutoSize = true,
				Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
				Text = role.Name,
				Margin = new Padding(10, 4, 0, 2),
				UseVisualStyleBackColor = true,
				Tag = role,
			};

			stageCheckBox.CheckedChanged += HandleStageCheckBoxCheckChanged;
			_tableLayoutOuter.Controls.Add(stageCheckBox, 1, row);
			_stageCheckBoxes.Add(stageCheckBox);
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			Utils.SetWindowRedraw(this, false);

			var status = _file.GetValue("status", Event.Status.Incoming.ToString()) as string;

			foreach (var radioButton in _statusRadioButtons.Where(r => r.Tag.ToString() == status))
				radioButton.Checked = true;

			var roleFormat = LocalizationManager.GetString(
				"EventsView.StatusAndStagesEditor.StageNameDisplayFormat", "{0} (on auto-pilot)");

			foreach (var checkBox in _stageCheckBoxes)
			{
				checkBox.CheckedChanged -= HandleStageCheckBoxCheckChanged;
				var role = checkBox.Tag as ComponentRole;
				var completeType = _file.ParentElement.StageCompletedControlValues[role.Id];

				if (completeType == StageCompleteType.Auto)
				{
					checkBox.Text = string.Format(roleFormat, role.Name);
					checkBox.ForeColor = Color.DimGray;
					checkBox.CheckState = CheckState.Indeterminate;
				}
				else
				{
					checkBox.Text = role.Name;
					checkBox.ForeColor = ForeColor;
					checkBox.CheckState = (completeType == StageCompleteType.Complete ?
						CheckState.Checked : CheckState.Unchecked);
				}

				checkBox.CheckedChanged += HandleStageCheckBoxCheckChanged;
			}

			Utils.SetWindowRedraw(this, true);
		}

		/// ------------------------------------------------------------------------------------
		private void SaveFieldValue(string fieldName, string value)
		{
			string failureMessage;
			_file.SetValue(fieldName, value, out failureMessage);

			if (failureMessage == null)
				_file.Save();
			else
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
		}

		#region Event handlers
		/// ------------------------------------------------------------------------------------
		void HandleStageCheckBoxCheckChanged(object sender, EventArgs e)
		{
			var roleId = ((ComponentRole)((CheckBox)sender).Tag).Id;

			switch (_file.ParentElement.StageCompletedControlValues[roleId])
			{
				case StageCompleteType.Auto:
					_file.ParentElement.StageCompletedControlValues[roleId] = StageCompleteType.NotComplete;
					break;
				case StageCompleteType.NotComplete:
					_file.ParentElement.StageCompletedControlValues[roleId] = StageCompleteType.Complete;
					break;
				default:
					_file.ParentElement.StageCompletedControlValues[roleId] = StageCompleteType.Auto;
					break;
			}

			SaveFieldValue("stage_" + roleId, _file.ParentElement.StageCompletedControlValues[roleId].ToString());
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleStagesColorBlockPaint(object sender, PaintEventArgs e)
		{
			var rc = ((Label)sender).ClientRectangle;
			rc.Width--;
			rc.Height--;
			e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, rc);
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString("EventsView.StatusAndStagesEditor.TabText", "Status && Stages");
			base.HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTableLayoutOuterPaint(object sender, PaintEventArgs e)
		{
			using (var pen = new Pen(Properties.Settings.Default.EventEditorsBorderColor))
			{
				foreach (var label in new[] { _labelStatus, _labelStages })
				{
					var y = label.Top + (label.Height / 2) + 1;
					e.Graphics.DrawLine(pen, label.Right + 3, y, _tableLayoutOuter.ClientRectangle.Right, y);
				}
			}
		}

		#endregion
	}
}
