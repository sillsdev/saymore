using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Localization;
using SayMore.Model.Files;
using SilTools;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class StatusAndStagesEditor : EditorBase
	{
		public delegate StatusAndStagesEditor Factory(ComponentFile file, string imageKey);
		private readonly IEnumerable<ComponentRole> _componentRoles;

		/// ----------------------------------------------------------------------------------------
		public StatusAndStagesEditor(ComponentFile file, string imageKey,
			IEnumerable<ComponentRole> componentRoles) : base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "StatusAndStages";

			_componentRoles = componentRoles;

			AddStageFields();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			SetFonts();
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

		/// ----------------------------------------------------------------------------------------
		private void AddStageFields()
		{
			int row = _tableLayoutOuter.GetRow(_labelStages) + 1;

			for (int i = row; i < _tableLayoutOuter.RowCount; i++)
				_tableLayoutOuter.RowStyles[i].SizeType = SizeType.AutoSize;

			foreach (var role in _componentRoles)
			{
				AddColorBlockLabel(row, role.Color);
				AddStageCheckBox(row++, role.Name);
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
		private void AddStageCheckBox(int row, string roleName)
		{
			var roleFormat = LocalizationManager.GetString(
				"EventsView.StatusAndStagesEditor.StageNameDisplayFormat", "{0} (on auto-pilot)");

			var stageCheckBox = new CheckBox
			{
				Name = "checkBoxStage_" + roleName,
				Font = Program.DialogFont,
				AutoSize = true,
				Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
				Text = string.Format(roleFormat, roleName),
				Margin = new Padding(10, 4, 0, 2),
				UseVisualStyleBackColor = true
			};

			_tableLayoutOuter.Controls.Add(stageCheckBox, 1, row);
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
	}
}
