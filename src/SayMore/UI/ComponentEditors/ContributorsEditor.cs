using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using Palaso.UI.WindowsForms.ClearShare;
using Palaso.UI.WindowsForms.ClearShare.WinFormsUI;
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

			_contributorsControl = new ContributorsListControl(_model);
			_contributorsControl.Dock = DockStyle.Fill;
			_contributorsControl.ValidatingContributor += HandleValidatingContributor;
			LocalizeColumnHeaders();
			Controls.Add(_contributorsControl);

			file.AfterSave += file_AfterSave;

			SetComponentFile(file);

			if (personInformant != null)
				personInformant.PersonUiIdChanged += HandlePersonsUiIdChanged;
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
			_file.Save();
			if (failureMessage != null)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString("CommonToMultipleViews.ContributorsEditor.TabText", "Contributors");

			if (_contributorsControl != null)
				LocalizeColumnHeaders();

			base.HandleStringsLocalized();
		}

		/// <remarks>SP-874: Localize column headers</remarks>
		private void LocalizeColumnHeaders()
		{
			_contributorsControl.SetColumnHeaderText(0, LocalizationManager.GetString("SessionsView.ContributorsEditor.NameColumnTitle", "Name"));
			_contributorsControl.SetColumnHeaderText(1, LocalizationManager.GetString("SessionsView.ContributorsEditor.RoleColumnTitle", "Role"));
			_contributorsControl.SetColumnHeaderText(2, LocalizationManager.GetString("SessionsView.ContributorsEditor.DateColumnTitle", "Date"));
			_contributorsControl.SetColumnHeaderText(3, LocalizationManager.GetString("SessionsView.ContributorsEditor.CommentColumnTitle", "Comments"));
		}
	}
}
