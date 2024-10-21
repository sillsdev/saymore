using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using SayMore.Model;
using SIL.Windows.Forms.Widgets.BetterGrid;
using SayMore.Model.Files;
using DateTime = System.DateTime;
using Settings = SayMore.Properties.Settings;

namespace SayMore.UI.ComponentEditors
{
	public partial class PersonContributionEditor : EditorBase
	{
		public delegate PersonContributionEditor Factory(ComponentFile file, string imageKey);

		private BetterGrid _grid;
		private string _personId;
		private string _personCode;

		public PersonContributionEditor(ComponentFile file, string imageKey)
			: base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "PersonRoles";
			RememberPersonId(file);

			InitializeControls();

			// do this to set the Associated Sessions the first time because the project might not be loaded yet
			NotifyWhenProjectIsSet();
		}
				
		protected override void OnCurrentProjectSet()
		{
			base.OnCurrentProjectSet();
			LoadAssociatedSessionsAndFiles();
		}

		private void InitializeControls()
		{
			_grid = new BetterGrid
			{
				Dock = DockStyle.Top,
				RowHeadersVisible = false,
				AllowUserToOrderColumns = false,
				AllowUserToResizeRows = true,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
				Name = "PersonContributionGrid",
				BorderStyle = BorderStyle.None,
				ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
				ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
			};

			// look for saved settings
			var widths = Array.ConvertAll(Settings.Default.PersonContributionColumns.Split(','), int.Parse).ToList();
			while (widths.Count < 4)
				widths.Add(200);

			// set the localizable column header text
			string[] headerText =
			{
				@"_L10N_:PeopleView.ContributionEditor.NameColumnTitle!Name",
				@"_L10N_:PeopleView.ContributionEditor.RoleColumnTitle!Role",
				@"_L10N_:PeopleView.ContributionEditor.DateColumnTitle!Date",
				@"_L10N_:PeopleView.ContributionEditor.CommentColumnTitle!Comments"
			};

			for (var i = 0; i < headerText.Length; i++)
				_grid.Columns.Add(new DataGridViewTextBoxColumn { Width = widths[i], SortMode = DataGridViewColumnSortMode.Automatic, ReadOnly = true, HeaderText = headerText[i] });

			// set column header height to match the other grids
			_grid.AutoResizeColumnHeadersHeight();
			_grid.ColumnHeadersHeight += 8;

			// make it localizable
			L10NSharpExtender locExtender = new L10NSharpExtender();
			locExtender.LocalizationManagerId = "SayMore";
			locExtender.SetLocalizingId(_grid, "ContributionsEditorGrid");

			locExtender.EndInit();

			Controls.Add(_grid);

			_grid.ColumnWidthChanged += _grid_ColumnWidthChanged;

			Program.PersonDataChanged += Program_PersonDataChanged;
			Disposed += PersonContributionEditor_Disposed;
		}

		void PersonContributionEditor_Disposed(object sender, EventArgs e)
		{
			Program.PersonDataChanged -= Program_PersonDataChanged;
		}

		void Program_PersonDataChanged()
			{
			//GetDataInBackground();
					LoadAssociatedSessionsAndFiles();
			}

		private void _grid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			// save column widths (min width to save is 50)
			var widths = (from DataGridViewColumn col in _grid.Columns select (col.Width < 50) ? 50 : col.Width).ToList();

			Settings.Default.PersonContributionColumns = string.Join(",", widths);
		}

		private void LoadAssociatedSessionsAndFiles()
		{
			var project = Program.CurrentProject;
			if (project == null)
				return;

			var boldFont = new Font(_grid.Font.FontFamily, _grid.Font.Size, FontStyle.Bold);

			_grid.Rows.Clear();

			foreach (var session in project.GetAllSessions())
			{
				foreach (var contrib in session.GetAllContributionsFor(_personId, _personCode))
				{
					var iRow = _grid.AddRow(GetContribRowData(contrib));

					if (contrib.SpecificFileName == null)
						_grid.Rows[iRow].Cells[0].Style.Font = boldFont;
				}
			}
		}

		private object[] GetContribRowData(SessionContribution contrib)
		{
			var description = contrib.SpecificFileName != null ?
				System.IO.Path.GetFileNameWithoutExtension(contrib.SpecificFileName) :
				contrib.SessionId + " " + contrib.SessionTitle;

			var date = contrib.Contribution.Date;
			if (date == DateTime.MinValue)
			{
				if (contrib.SpecificFileName == null)
					date = contrib.SessionDate;
				else
				{
					try
					{
						date = new System.IO.FileInfo(contrib.SpecificFileName).CreationTime;
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
				}
			}
			var formattedDate = date == DateTime.MinValue ? string.Empty : date.ToShortDateString();

			var role = contrib.Contribution.Role;
			var localizedRole = role.Name ==  "Participant" ?
				LocalizationManager.GetString("PeopleView.ContributionEditor.RoleParticipant", "Participant") :
				LocalizationManager.GetDynamicString("SayMore",
					$"PeopleView.ContributionEditor.{role.Code}", role.Name);

			return new object[] { description, localizedRole, formattedDate, contrib.Contribution.Comments };
		}

		protected override void HandleStringsLocalized(ILocalizationManager lm)
		{
			if (lm == null || lm.Id == ApplicationContainer.kSayMoreLocalizationId)
			{
				TabText = LocalizationManager.GetString(
					"PeopleView.ContributionEditor.TabText", "Contributions");
			}

			base.HandleStringsLocalized(lm);
		}

		public override void SetComponentFile(ComponentFile file)
		{
			RememberPersonId(file);
			LoadAssociatedSessionsAndFiles();
		}

		private void RememberPersonId(ComponentFile file)
		{
			_personId = file.FileName.Substring(0, file.FileName.Length - Settings.Default.PersonFileExtension.Length);
			_personCode = file.GetStringValue(PersonFileType.kCode, null);
		}
	}
}
