
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using Palaso.Extensions;
using Palaso.UI.WindowsForms.ClearShare;
using Palaso.UI.WindowsForms.Widgets.BetterGrid;
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
			using (BackgroundWorker backgroundWorker = new BackgroundWorker())
			{
				backgroundWorker.DoWork += backgroundWorker_DoWork;
				backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
				backgroundWorker.RunWorkerAsync();
			}
		}

		void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			LoadAssociatedSessionsAndFiles();
		}

		void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
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
				Name = "PersonContributionGrid"
			};

			// look for saved settings
			var widths = Array.ConvertAll(Settings.Default.PersonContributionColumns.Split(new[] {','}), int.Parse).ToList();
			while (widths.Count < 4)
				widths.Add(200);

			_grid.Columns.Add(new DataGridViewTextBoxColumn { Width = widths[0], SortMode = DataGridViewColumnSortMode.NotSortable, ReadOnly = true, HeaderText = LocalizationManager.GetString("PeopleView.ContributionEditor.NameColumnTitle", "Name") });
			_grid.Columns.Add(new DataGridViewTextBoxColumn { Width = widths[1], SortMode = DataGridViewColumnSortMode.NotSortable, ReadOnly = true, HeaderText = LocalizationManager.GetString("PeopleView.ContributionEditor.RoleColumnTitle", "Role") });
			_grid.Columns.Add(new DataGridViewTextBoxColumn { Width = widths[2], SortMode = DataGridViewColumnSortMode.NotSortable, ReadOnly = true, HeaderText = LocalizationManager.GetString("PeopleView.ContributionEditor.DateColumnTitle", "Date") });
			_grid.Columns.Add(new DataGridViewTextBoxColumn { Width = widths[3], SortMode = DataGridViewColumnSortMode.NotSortable, ReadOnly = true, HeaderText = LocalizationManager.GetString("PeopleView.ContributionEditor.CommentColumnTitle", "Comments") });
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

					var rowid = _grid.AddRow(new object[] { sessionDescription, sessionRole, sessionDate, string.Empty });
					_grid.Rows[rowid].Cells[0].Style.Font = boldFont;
				}

				// get the files for this session
				var files = Directory.GetFiles(session.FolderPath, "*" + Settings.Default.MetadataFileExtension);
				var searchForId = "<name>" + _personId + "</name>";
				var searchForCode = (string.IsNullOrEmpty(_personCode) ? null : "<name>" + _personCode + "</name>");

				foreach (var file in files)
				{
					var fileContents = File.ReadAllText(file);

					// look for this person
					SearchInFile(file, fileContents, searchForId);
					if (searchForCode != null)
						SearchInFile(file, fileContents, searchForCode);
				}
			}
		}

		private void SearchInFile(string fileName, string fileContents, string searchFor)
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
				var fname = Path.GetFileName(fileName.Substring(0, fileName.Length - Settings.Default.MetadataFileExtension.Length));

				_grid.AddRow(new object[] { Path.GetFileName(fname), role, date, note });

				// look again
				pos = fileContents.IndexOf(searchFor, StringComparison.InvariantCultureIgnoreCase);
			}
		}

		private string FormatParseDate(string dateString)
		{
			if (string.IsNullOrEmpty(dateString)) return string.Empty;

			// older saymore date problem due to saving localized date string rather than ISO8601
			var date = dateString.IsISO8601Date()
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

		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString("PeopleView.ContributionEditor.TabText", "Contributions");
			base.HandleStringsLocalized();
		}

		public override void SetComponentFile(ComponentFile file)
		{
			RememberPersonId(file);
			GetDataInBackground();
		}

		private void RememberPersonId(ComponentFile file)
		{
			_personId = file.FileName.Substring(0, file.FileName.Length - Settings.Default.PersonFileExtension.Length);
			_personCode = file.GetStringValue("code", null);
		}
	}
}
