using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using L10NSharp;
using SIL.Windows.Forms;
using SIL.Windows.Forms.Extensions;
using SayMore.Model;
using SayMore.Model.Files;
using Color = System.Drawing.Color;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class StatusAndStagesEditor : EditorBase
	{
		public delegate StatusAndStagesEditor Factory(ComponentFile file, string imageKey);
		private List<RadioButton> _statusRadioButtons;
		private List<StageCheckBox> _stageCheckBoxes;
		private readonly ComponentRole[] _componentRoles;

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

			_buttonReadAboutStatus.Click += (s, e) =>
				Program.ShowHelpTopic("/Using_Tools/Sessions_tab/Status_Stages_tab/Select_session_status.htm");

			_buttonReadAboutStages.Click += (s, e) =>
				Program.ShowHelpTopic("/Using_Tools/Sessions_tab/Status_Stages_tab/Select_session_stages.htm");
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
			_statusRadioButtons = new List<RadioButton>(Enum.GetValues(typeof(Session.Status)).Length);

			int row = _tableLayoutOuter.GetRow(_labelStatus) + 1;
			int lastRow = _tableLayoutOuter.GetRow(_labelStages) - 1;

			for (int i = row; i <= lastRow; i++)
				_tableLayoutOuter.RowStyles[i].SizeType = SizeType.AutoSize;

			int tabIndex = 0;

			foreach (var status in Enum.GetValues(typeof(Session.Status)).Cast<Session.Status>())
			{
				var picBox = AddStatusImage(row, status);
				var radioButton = AddStatusRadioButton(row++, tabIndex++, status);

				if (radioButton.Height > picBox.Height)
					AdjustTopMarginForStatusControls(radioButton, picBox);
				else if (picBox.Height > radioButton.Height)
					AdjustTopMarginForStatusControls(picBox, radioButton);
			}

			_tableLayoutOuter.SetRow(_labelReadAboutStatus, row - 1);
			_tableLayoutOuter.SetRow(_buttonReadAboutStatus, row - 1);
			_buttonReadAboutStatus.TabIndex = tabIndex;
		}

		/// ------------------------------------------------------------------------------------
		private void AdjustTopMarginForStatusControls(Control tallCtrl, Control shortCtrl)
		{
			int y = (int)Math.Round((tallCtrl.Height - shortCtrl.Height) / 2d, MidpointRounding.AwayFromZero);
			shortCtrl.Margin = new Padding(shortCtrl.Margin.Left,
				tallCtrl.Margin.Top + y, shortCtrl.Margin.Right, shortCtrl.Margin.Bottom);
		}

		/// ------------------------------------------------------------------------------------
		private PictureBox AddStatusImage(int row, Session.Status status)
		{
			var statusPicBox = new PictureBox
			{
				Name = "picture_" + status,
				Anchor = AnchorStyles.Top | AnchorStyles.Left,
				Image = ResourceImageCache.GetBitmap("Status" + status),
				Margin = new Padding(5, 4, 0, 2),
				SizeMode = PictureBoxSizeMode.AutoSize,
			};

			statusPicBox.Size = statusPicBox.Image.Size;

			var toolTip = GetStatusToolTip(status);
			if (toolTip != null)
				_toolTip.SetToolTip(statusPicBox, toolTip);

			statusPicBox.MouseEnter += delegate
			{
				_toolTip.ToolTipTitle = Session.GetLocalizedStatus(status.ToString());
			};

			if (row == _tableLayoutOuter.GetRow(_labelStages))
				_tableLayoutOuter.RowStyles.Insert(row, new RowStyle(SizeType.AutoSize));

			_tableLayoutOuter.Controls.Add(statusPicBox, 0, row);
			return statusPicBox;
		}

		/// ------------------------------------------------------------------------------------
		private RadioButton AddStatusRadioButton(int row, int tabIndex, Session.Status status)
		{
			var statusRadioButton = new RadioButton
			{
				Name = "radioButton_" + status,
				Anchor = AnchorStyles.Top | AnchorStyles.Left,
				AutoEllipsis = true,
				AutoSize = true,
				Margin = new Padding(10, 4, 0, 2),
				Text = Session.GetLocalizedStatus(status.ToString()),
				Font = Program.DialogFont,
				UseVisualStyleBackColor = true,
				Tag = status,
				TabIndex = tabIndex,
			};

			var toolTip = GetStatusToolTip(status);
			if (toolTip != null)
				_toolTip.SetToolTip(statusRadioButton, toolTip);

			statusRadioButton.MouseEnter += delegate
			{
				_toolTip.ToolTipTitle = statusRadioButton.Text;
			};

			statusRadioButton.CheckedChanged += HandleStatusRadioButtonCheckedChanged;

			_tableLayoutOuter.Controls.Add(statusRadioButton, 1, row);
			_statusRadioButtons.Add(statusRadioButton);
			return statusRadioButton;
		}

		/// ----------------------------------------------------------------------------------------
		private string GetStatusToolTip(Session.Status status)
		{
			if (status == Session.Status.Incoming)
			{
				return LocalizationManager.GetString(
					"SessionsView.StatusAndStagesEditor.StatusToolTips.Incoming",
					"I am gathering the recording and meta\ndata and may or may not take it further.");
			}

			if (status == Session.Status.In_Progress)
			{
				return LocalizationManager.GetString(
					"SessionsView.StatusAndStagesEditor.StatusToolTips.In_Progress",
					"I'm working on it.");
			}

			if (status == Session.Status.Finished)
			{
				return LocalizationManager.GetString(
					"SessionsView.StatusAndStagesEditor.StatusToolTips.Finished",
					"I'm done working on it.");
			}

			if (status == Session.Status.Skipped)
			{
				return LocalizationManager.GetString(
					"SessionsView.StatusAndStagesEditor.StatusToolTips.Skipped",
					"I've decided to not develop\nthis session at this time.");
			}

			return null;
		}

		/// ----------------------------------------------------------------------------------------
		private void AddStageFields()
		{
			int row = _tableLayoutOuter.GetRow(_labelStages) + 1;

			for (int i = row; i < _tableLayoutOuter.RowCount; i++)
				_tableLayoutOuter.RowStyles[i].SizeType = SizeType.AutoSize;

			_stageCheckBoxes = new List<StageCheckBox>(_componentRoles.Length);

			int tabIndex = _buttonReadAboutStatus.TabIndex + 1;

			foreach (var role in _componentRoles)
			{
				AddColorBlockLabel(row, role.Color);
				AddStageCheckBox(row++, tabIndex++, role);
			}

			_tableLayoutOuter.SetRow(_labelReadAboutStages, row - 1);
			_tableLayoutOuter.SetRow(_buttonReadAboutStages, row - 1);
		}

		/// ------------------------------------------------------------------------------------
		private void AddColorBlockLabel(int row, Color roleColor)
		{
			var colorBlock = new Label
			{
				AutoSize = false,
				Anchor = AnchorStyles.Top,
				Size = new Size(10, 14),
				Margin = new Padding(0, 5, 0, 2),
				BackColor = roleColor
			};

			colorBlock.Paint += HandleStagesColorBlockPaint;

			if (row == _tableLayoutOuter.RowCount)
				_tableLayoutOuter.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			_tableLayoutOuter.Controls.Add(colorBlock, 0, row);
		}

		/// ------------------------------------------------------------------------------------
		private void AddStageCheckBox(int row, int tabIndex, ComponentRole role)
		{
			var stageCheckBox = new StageCheckBox(role) { TabIndex = tabIndex };

			stageCheckBox.CheckedChanged += HandleStageCheckBoxCheckChanged;

			stageCheckBox.IsRoleCompleteProvider = r1 =>
				_file.ParentElement.GetCompletedStages(false).Any(r2 => r2.Id == r1.Id);

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
			this.SetWindowRedraw(false);

			var status = _file.GetValue(SessionFileType.kStatusFieldName, Session.Status.Incoming.ToString()) as string;

			var radioButton = _statusRadioButtons.FirstOrDefault(r => r.Tag.ToString() == status);
			if (radioButton != null)
			{
				radioButton.CheckedChanged -= HandleStatusRadioButtonCheckedChanged;
				radioButton.Checked = true;
				radioButton.CheckedChanged += HandleStatusRadioButtonCheckedChanged;
			}

			foreach (var checkBox in _stageCheckBoxes)
			{
				checkBox.CheckedChanged -= HandleStageCheckBoxCheckChanged;
				checkBox.Update(false, _file.ParentElement.StageCompletedControlValues);
				checkBox.CheckedChanged += HandleStageCheckBoxCheckChanged;
			}

			this.SetWindowRedraw(true);
		}

		/// ------------------------------------------------------------------------------------
		private void SaveFieldValue(string fieldName, string value)
		{
			string failureMessage;
			_file.SetValue(fieldName, value, out failureMessage);

			if (failureMessage == null)
				_file.Save();
			else
				SIL.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
		}

		#region Event handlers
		/// ----------------------------------------------------------------------------------------
		private void HandleStatusRadioButtonCheckedChanged(object s, EventArgs e)
		{
			var radioButton = (RadioButton)s;
			if (radioButton.Checked)
				SaveFieldValue(SessionFileType.kStatusFieldName, radioButton.Tag.ToString());
		}

		/// ------------------------------------------------------------------------------------
		void HandleStageCheckBoxCheckChanged(object sender, EventArgs e)
		{
			var checkBox = sender as StageCheckBox;

			checkBox.CheckedChanged -= HandleStageCheckBoxCheckChanged;
			var newCompleteType = checkBox.Update(true, _file.ParentElement.StageCompletedControlValues);
			checkBox.CheckedChanged += HandleStageCheckBoxCheckChanged;

			SaveFieldValue(SessionFileType.kStageFieldPrefix + checkBox.Role.Id, newCompleteType);
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
			TabText = LocalizationManager.GetString("SessionsView.StatusAndStagesEditor.TabText", "Status && Stages");
			if (_statusRadioButtons != null)
			{
				foreach (var radioButton in _statusRadioButtons.Where(b => b.Tag is Session.Status))
				{
					var status = (Session.Status)radioButton.Tag;
					radioButton.Text = Session.GetLocalizedStatus(status.ToString());
					var toolTip = GetStatusToolTip(status);
					if (toolTip != null)
						_toolTip.SetToolTip(radioButton, toolTip);
				}
			}

			base.HandleStringsLocalized();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleTableLayoutOuterPaint(object sender, PaintEventArgs e)
		{
			using (var pen = new Pen(Properties.Settings.Default.SessionEditorsBorderColor))
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

	#region StageCheckBox class
	/// ----------------------------------------------------------------------------------------
	public class StageCheckBox : CheckBox
	{
		public Func<ComponentRole, bool> IsRoleCompleteProvider;
		public ComponentRole Role { get; private set; }

		private StageCompleteType _completeType;

		/// ------------------------------------------------------------------------------------
		public StageCheckBox(ComponentRole role)
		{
			Name = "checkBoxStage_" + role.Name;
			Font = Program.DialogFont;
			AutoSize = true;
			Anchor = AnchorStyles.Left;
			Text = role.Name;
			Margin = new Padding(10, 4, 0, 2);
			UseVisualStyleBackColor = true;
			Role = role;

			DoubleBuffered = true;
		}

		/// ------------------------------------------------------------------------------------
		public string Update(bool toggleValue,
			IDictionary<string, StageCompleteType> stageCompletedControlValues)
		{
			var isRoleAutoCompleted = IsRoleCompleteProvider(Role);
			_completeType = stageCompletedControlValues[Role.Id];

			if (toggleValue)
			{
				switch (_completeType)
				{
					case StageCompleteType.Auto:
						_completeType = (isRoleAutoCompleted ? StageCompleteType.NotComplete : StageCompleteType.Complete);
						break;

					case StageCompleteType.NotComplete:
						_completeType = (isRoleAutoCompleted ? StageCompleteType.Complete : StageCompleteType.Auto);
						break;

					default:
						_completeType = (isRoleAutoCompleted ? StageCompleteType.Auto : StageCompleteType.NotComplete);
						break;
				}
			}

			var roleFormat = LocalizationManager.GetString(
				"SessionsView.StatusAndStagesEditor.StageNameDisplayFormat", "{0} (on auto-pilot)");

			if (_completeType == StageCompleteType.Auto)
			{
				Text = string.Format(roleFormat, Role.Name);
				CheckState = CheckState.Indeterminate;
			}
			else
			{
				Text = Role.Name;
				CheckState = (_completeType == StageCompleteType.Complete ?
					CheckState.Checked : CheckState.Unchecked);
			}

			stageCompletedControlValues[Role.Id] = _completeType;
			return _completeType.ToString();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnPaint(PaintEventArgs e)
		{
			using (var br = new SolidBrush(BackColor))
				e.Graphics.FillRectangle(br, ClientRectangle);

			var textRect = GetTextRectangle(e.Graphics);

			// I was having some trouble getting the DrawCheckBox to honor the ForeColor property
			// under curtain circumstances and I never could sort out why. Therefore, I draw the
			// text as a separate operations in which I can specify the text color. It may
			// actually be better since if I were to set the ForeColor property here, I would get
			// a recursive call to OnPaint.

			CheckBoxRenderer.DrawCheckBox(e.Graphics, GetCheckBoxLocation(e.Graphics),
				textRect, string.Empty, Font, Focused, GetCheckState());

			var clr = (ClientRectangle.Contains(PointToClient(MousePosition)) || _completeType != StageCompleteType.Auto ?
				SystemColors.ControlText : Color.DimGray);

			TextRenderer.DrawText(e.Graphics, Text, Font, textRect, clr,
				TextFormatFlags.Left |TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
		}

		/// ------------------------------------------------------------------------------------
		private CheckBoxState GetCheckState()
		{
			if (CheckState == CheckState.Indeterminate)
				return (IsRoleCompleteProvider(Role) ? CheckBoxState.CheckedDisabled : CheckBoxState.UncheckedDisabled);

			var hot = ClientRectangle.Contains(PointToClient(MousePosition));
			var buttonDown = (MouseButtons == MouseButtons.Left && hot);

			if (CheckState == CheckState.Checked)
			{
				if (buttonDown)
					return CheckBoxState.CheckedPressed;

				return (hot ? CheckBoxState.CheckedHot : CheckBoxState.CheckedNormal);
			}

			if (buttonDown)
				return CheckBoxState.UncheckedPressed;

			return (hot ? CheckBoxState.UncheckedHot : CheckBoxState.UncheckedNormal);
		}

		/// ------------------------------------------------------------------------------------
		private Rectangle GetTextRectangle(Graphics g)
		{
			var checkBoxWidth = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal).Width;

			return new Rectangle(ClientRectangle.X + checkBoxWidth + 3, ClientRectangle.Y,
				ClientRectangle.Width - checkBoxWidth - 3, ClientRectangle.Height);
		}

		/// ------------------------------------------------------------------------------------
		private Point GetCheckBoxLocation(Graphics g)
		{
			var checkBoxHeight = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal).Height;
			var y = (int)Math.Round((ClientSize.Height - checkBoxHeight) / 2d, MidpointRounding.AwayFromZero);
			return new Point(0, y);
		}
	}

	#endregion
}
