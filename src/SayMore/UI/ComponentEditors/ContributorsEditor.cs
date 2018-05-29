using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using L10NSharp.UI;
using SIL.Windows.Forms.ClearShare;
using SIL.Windows.Forms.ClearShare.WinFormsUI;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;


namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class ContributorsEditor : EditorBase
	{
		public delegate ContributorsEditor Factory(ComponentFile file, string imageKey);

		protected ContributorsListControl _contributorsControl;
		protected ContributorsListControlViewModel _model;

		/// ------------------------------------------------------------------------------------
		public ContributorsEditor(ComponentFile file, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider, PersonInformant personInformant) :
			base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "Contributors";

			_model = new ContributorsListControlViewModel(autoCompleteProvider, SaveContributors);

			// ReSharper disable once UseObjectOrCollectionInitializer
			_contributorsControl = new ContributorsListControl(_model);
			_contributorsControl.Dock = DockStyle.Fill;
			_contributorsControl.ValidatingContributor += HandleValidatingContributor;

			InitializeGrid();

			InsertLinkLabelBack(imageKey);

			Controls.Add(_contributorsControl);

			file.AfterSave += file_AfterSave;

			SetComponentFile(file);

			if (personInformant != null)
				personInformant.PersonUiIdChanged += HandlePersonsUiIdChanged;
		}

		private void InsertLinkLabelBack(string imageKey)
		{
			if (imageKey != null)
			{
				LinkLabel linkBack = new LinkLabel();
				linkBack.BackColor = Color.Transparent;
				linkBack.ForeColor = Color.Black;
				linkBack.LinkColor = Color.Black;
				linkBack.DisabledLinkColor = Color.Black;
				linkBack.TextAlign = ContentAlignment.TopRight;
				linkBack.Dock = DockStyle.Right;
				linkBack.Text = "\u003C";
				linkBack.Name = "linkBack";
				linkBack.Font = new Font("Segoe UI Symbol", 16);
				linkBack.LinkBehavior = LinkBehavior.NeverUnderline;
				linkBack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleLinkClick);
				Controls.Add(linkBack);
			}
		}

		private void HandleLinkClick(object sender, LinkLabelLinkClickedEventArgs e)
		{
			var frm = FindForm();
			if (frm == null)
				return;

			var tabPages = ((ElementListScreen.ElementListScreen<Session>)frm.ActiveControl).SelectedComponentEditorsTabControl.TabPages;
			foreach (TabPage tab in tabPages)
			{
				if (tab.ImageKey != @"Session") continue;
				((ElementListScreen.ElementListScreen<Session>)frm.ActiveControl).SelectedComponentEditorsTabControl.SelectedTab = tab;
				tab.Focus();
				break;
			}
		}

		/// <remarks>SP-874: Not able to open L10NSharp with Alt-Shift-click</remarks>
		private void InitializeGrid()
		{
			// misc. column header settings
			_contributorsControl.Grid.BorderStyle = BorderStyle.None;
			_contributorsControl.Grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			_contributorsControl.Grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

			// set the localizable column header text
			string[] headerText =
			{
				@"_L10N_:SessionsView.ContributorsEditor.NameColumnTitle!Name",
				@"_L10N_:SessionsView.ContributorsEditor.RoleColumnTitle!Role",
				@"_L10N_:SessionsView.ContributorsEditor.DateColumnTitle!Date",
				@"_L10N_:SessionsView.ContributorsEditor.CommentColumnTitle!Comments"
			};

			for (var i = 0; i < headerText.Length; i++)
			{
				_contributorsControl.SetColumnHeaderText(i, headerText[i]);

				// this is needed to match the visual behavior of the other grids
				_contributorsControl.Grid.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
			}

			// set column header height to match the other grids
			_contributorsControl.Grid.AutoResizeColumnHeadersHeight();
			_contributorsControl.Grid.ColumnHeadersHeight += 8;

			// make it localizable
			L10NSharpExtender locExtender = new L10NSharpExtender();
			locExtender.LocalizationManagerId = "SayMore";
			_contributorsControl.SetLocalizationExtender(locExtender);

			locExtender.EndInit();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// We get this message from the person informant when a person's UI ID has changed.
		/// When that happens, we just need to update the Text in the participant control. No
		/// change is needed (or desirable) in the underlying metadata.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePersonsUiIdChanged(object sender, ElementIdChangedArgs e)
		{
			var contribs = _model.Contributions;
			foreach (var c in contribs.Where(c => c.ContributorName == e.OldId))
				c.ContributorName = e.NewId;

			_model.SetContributionList(contribs);
		}

		void file_AfterSave(object sender, System.EventArgs e)
		{
			Program.OnPersonDataChanged();
		}

		/// ------------------------------------------------------------------------------------
		public override sealed void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);
			_model.SetContributionList(file.GetValue("contributions", null) as ContributionCollection);
		}

		/// ------------------------------------------------------------------------------------
		private KeyValuePair<string, string> HandleValidatingContributor(ContributorsListControl sender,
			Contribution contribution, CancelEventArgs e)
		{
			var kvp = CheckIfContributorIsValid(contribution);
			e.Cancel = !string.IsNullOrEmpty(kvp.Key);

			if (!e.Cancel)
				SaveContributors();

			return kvp;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsOKToLeaveEditor
		{
			get
			{
				var contribution = _contributorsControl.GetCurrentContribution();

				if (contribution == null ||
					(contribution.IsEmpty && _contributorsControl.InNewContributionRow))
				{
					return true;
				}

				return string.IsNullOrEmpty(CheckIfContributorIsValid(contribution).Key);
			}
		}

		/// ------------------------------------------------------------------------------------
		private static KeyValuePair<string, string> CheckIfContributorIsValid(Contribution contribution)
		{
			if (contribution != null)
			{
				if (string.IsNullOrEmpty(contribution.ContributorName))
				{
					return new KeyValuePair<string, string>("name",
						LocalizationManager.GetString("CommonToMultipleViews.ContributorsEditor.MissingContributorNameMsg", "Enter a name."));
				}

				if (contribution.Role == null)
				{
					return new KeyValuePair<string, string>("role",
						LocalizationManager.GetString("CommonToMultipleViews.ContributorsEditor.MissingContributorRoleMsg", "Choose a role."));
				}
			}

			return new KeyValuePair<string, string>();
		}

		/// ------------------------------------------------------------------------------------
		private void SaveContributors()
		{
			string failureMessage;
			_file.SetValue("contributions", _model.Contributions, out failureMessage);
			_file.SetValue(SessionFileType.kParticipantsFieldName, GetParticipants(), out failureMessage);
			_file.Save();

			var frm = FindForm();
			if (frm == null)
				return;

			//Set the people list whenever changes happen in Contributors list
			foreach (var editor in Program.GetControlsOfType<SessionBasicEditor>(Program.ProjectWindow))
				editor.SetPeople(GetParticipants());

			if (failureMessage != null)
				SIL.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
		}

		/// --------------------------------------------------------------------------------------
		/// Get the participants list from the Sessions contributions
		/// --------------------------------------------------------------------------------------
		private string GetParticipants()
		{
			string participants = string.Empty;
			foreach (Contribution contributor in _model.Contributions)
			{
				participants += contributor.ContributorName + " (" + contributor.Role.Name + "); ";
			}

			return participants;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString("CommonToMultipleViews.ContributorsEditor.TabText", "Contributors");
			base.HandleStringsLocalized();
		}
	}
}
