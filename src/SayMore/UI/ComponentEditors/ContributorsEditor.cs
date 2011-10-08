using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Palaso.ClearShare;
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
			AutoCompleteValueGatherer autoCompleteProvider) : base(file, null, imageKey)
		{
			InitializeComponent();
			Name = "Contributors";

			_model = new ContributorsListControlViewModel(autoCompleteProvider, SaveContributors);

			_contributorsControl = new ContributorsListControl(_model);
			_contributorsControl.Dock = DockStyle.Fill;
			_contributorsControl.ValidatingContributor += HandleValidatingContributor;
			_contributorsControl.ContributorDeleted += HandleContributorDeleted;
			Controls.Add(_contributorsControl);


			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
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
		public override bool IsOKSToLeaveEditor
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
						Program.GetString("CommonToMultipleViews.ContributorsEditor.MissingContributorNameMsg", "Enter a name."));
				}

				if (contribution.Role == null)
				{
					return new KeyValuePair<string, string>("role",
						Program.GetString("CommonToMultipleViews.ContributorsEditor.MissingContributorRoleMsg", "Choose a role."));
				}
			}

			return new KeyValuePair<string, string>();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleContributorDeleted(object sender, System.EventArgs e)
		{
			SaveContributors();
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
			TabText = Program.GetString("CommonToMultipleViews.ContributorsEditor.TabText", "Contributors");
			base.HandleStringsLocalized();
		}
	}
}
