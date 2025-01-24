using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using SayMore.Model;
using SayMore.Model.Files;
using DateTime = System.DateTime;
using Settings = SayMore.Properties.Settings;

namespace SayMore.UI.ComponentEditors
{
	public partial class PersonContributionEditor : EditorBase
	{
		public delegate PersonContributionEditor Factory(ComponentFile file, string imageKey);

		private CancellationTokenSource _cancellationTokenSource;
		private string _personId;
		private string _personCode;

		public PersonContributionEditor(ComponentFile file, string imageKey)
			: base(file, null, imageKey)
		{
			InitializeComponent();
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
			// look for saved settings
			var widths = Array.ConvertAll(Settings.Default.PersonContributionColumns.Split(','), int.Parse).ToList();
			while (widths.Count < 4)
				widths.Add(200);

			for (var i = 0; i < _grid.Columns.Count; i++)
				_grid.Columns[i].Width = widths[i];

			// set column header height to match the other grids
			_grid.AutoResizeColumnHeadersHeight();
			_grid.ColumnHeadersHeight += 8;

			Program.PersonDataChanged += Program_PersonDataChanged;
		}

		private void Program_PersonDataChanged()
		{
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

			_cancellationTokenSource = new CancellationTokenSource();
			var cancelTok = _cancellationTokenSource.Token;
			foreach (var session in project.GetAllSessions(cancelTok))
			{
				if (cancelTok.IsCancellationRequested)
					break;

				foreach (var contrib in session.GetAllContributionsFor(_personId, _personCode))
				{
					var iRow = _grid.AddRow(GetContribRowData(contrib));

					if (contrib.SpecificFileName == null)
						_grid.Rows[iRow].Cells[0].Style.Font = boldFont;
				}
			}

			_cancellationTokenSource.Dispose();
			_cancellationTokenSource = null;
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
			_cancellationTokenSource?.Cancel();
			RememberPersonId(file);
			LoadAssociatedSessionsAndFiles();
		}

		private void RememberPersonId(ComponentFile file)
		{
			_personId = file.FileName.Substring(0,
				file.FileName.Length - Settings.Default.PersonFileExtension.Length);
			_personCode = file.GetStringValue(PersonFileType.kCode, null);
		}
	}
}
