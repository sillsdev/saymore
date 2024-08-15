using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using SIL.Extensions;
using SIL.Windows.Forms.ClearShare;
using SIL.Windows.Forms.Widgets.BetterGrid;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.UI.ComponentEditors
{
	public partial class PersonContributionEditor : EditorBase
	{
		public delegate PersonContributionEditor Factory(ComponentFile file, string imageKey);

		private BetterGrid _grid;
		private string _personId;
		private string _personCode;

		private static readonly OlacSystem OlacSystem = new OlacSystem();

		public PersonContributionEditor(ComponentFile file, string imageKey)
			: base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "PersonRoles";
			RememberPersonId(file);

			InitializeControls();

			// do this to set the Associated Sessions the first time because the project might not be loaded yet
			GetDataInBackground();
		}

		private void GetDataInBackground()
		{
			using (var backgroundWorker = new BackgroundWorker())
			{
				backgroundWorker.DoWork += WaitForProjectToBeSet;
				backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
				backgroundWorker.RunWorkerAsync();
			}
		}

		void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			LoadAssociatedSessionsAndFiles();
		}

		private void WaitForProjectToBeSet(object sender, DoWorkEventArgs e)
		{
			var count = 0;
			while ((Program.CurrentProject == null) && (count < 50))
			{
				Thread.Sleep(100);
				count++;
			}
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
			LoadAssociatedSessionsAndFiles();
		}

		void _grid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			// save column widths (min width to save is 50)
			var widths = (from DataGridViewColumn col in _grid.Columns select (col.Width < 50) ? 50 : col.Width).ToList();

			Settings.Default.PersonContributionColumns = string.Join(",", widths);
		}

		private void LoadAssociatedSessionsAndFiles()
		{
			var project = Program.CurrentProject;
			var boldFont = new Font(_grid.Font.FontFamily, _grid.Font.Size, FontStyle.Bold);

			if (project == null) return;

			_grid.Rows.Clear();

			foreach (var session in project.GetAllSessions())
			{
				var person = session.GetAllPersonsInSession().FirstOrDefault(p => p.Id == _personId);

				if (person != null)
				{
					// add Participant row for this session
					var sessionDescription = session.Id + " " + session.Title;
					var sessionRole = LocalizationManager.GetString("PeopleView.ContributionEditor.RoleParticipant", "Participant");
					var sessionDate = FormatParseDate(session.MetaDataFile.GetStringValue("date", string.Empty));

					var iRow = _grid.AddRow(new object[] { sessionDescription, sessionRole, sessionDate, string.Empty });
					_grid.Rows[iRow].Cells[0].Style.Font = boldFont;
				}

				// get the metadata files for this session
				var files = Directory.GetFiles(session.FolderPath, "*" + Settings.Default.MetadataFileExtension);
				var searchForId = "<name>" + _personId + "</name>";
				var searchForCode = (string.IsNullOrEmpty(_personCode) ? null : "<name>" + _personCode + "</name>");

				foreach (var file in files)
				{
					var fileContents = File.ReadAllText(file);

					// look for this person
					AddGridRowsForFileSpecificContributions(file, fileContents, searchForId);
					if (searchForCode != null)
						AddGridRowsForFileSpecificContributions(file, fileContents, searchForCode);
				}
			}
		}

		private void AddGridRowsForFileSpecificContributions(string fileName, string fileContents, string searchFor)
		{
			var pos = fileContents.IndexOf(searchFor, StringComparison.InvariantCultureIgnoreCase);
			while (pos > 0)
			{
				// remove everything before this person
				fileContents = fileContents.Substring(pos + searchFor.Length);

				// get the end of this contributor
				var testString = fileContents.Substring(0, fileContents.IndexOf("</contributor>", StringComparison.InvariantCultureIgnoreCase));

				var role = GetRoleFromOlacList(GetValueFromXmlString(testString, "role"));
				var date = FormatParseDate(GetValueFromXmlString(testString, "date"));
				var note = GetValueFromXmlString(testString, "notes");
				var filename = Path.GetFileName(fileName.Substring(0, fileName.Length - Settings.Default.MetadataFileExtension.Length));

				_grid.AddRow(new object[] { Path.GetFileName(filename), role, date, note });

				// look again
				pos = fileContents.IndexOf(searchFor, StringComparison.InvariantCultureIgnoreCase);
			}
		}

		private string FormatParseDate(string dateString)
		{
			if (string.IsNullOrEmpty(dateString)) return string.Empty;

			// older SayMore date problem due to saving localized date string rather than ISO8601
			var date = DateTimeExtensions.IsISO8601Date(dateString)
				? DateTime.Parse(dateString)
				: DateTimeExtensions.ParseDateTimePermissivelyWithException(dateString);

			return date == DateTime.MinValue ? string.Empty : date.ToShortDateString();
		}

		private static string GetRoleFromOlacList(string savedRole)
		{
			Role role;
			if (OlacSystem.TryGetRoleByCode(savedRole, out role))
				return role.Name;

			if (OlacSystem.TryGetRoleByName(savedRole, out role))
				return role.Name;

			return savedRole;
		}

		private static string GetValueFromXmlString(string xmlString, string valueName)
		{
			var pattern = string.Format("<{0}>(.*)</{0}>", valueName);
			var match = Regex.Match(xmlString, pattern);

			return match.Success ? match.Groups[1].Value : string.Empty;
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
			GetDataInBackground();
		}

		private void RememberPersonId(ComponentFile file)
		{
			_personId = file.FileName.Substring(0, file.FileName.Length - Settings.Default.PersonFileExtension.Length);
			_personCode = file.GetStringValue(PersonFileType.kCode, null);
		}
	}
}
