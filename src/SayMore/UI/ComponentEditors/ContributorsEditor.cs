using System.ComponentModel;
using System.Windows.Forms;
using SayMore.ClearShare;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class ContributorsEditor : EditorBase
	{
		public delegate ContributorsEditor Factory(ComponentFile file, string tabText, string imageKey);

		protected ContributorsListControl _contributorsControl;
		protected ContributorsListControlViewModel _model;

		/// ------------------------------------------------------------------------------------
		public ContributorsEditor(ComponentFile file, string tabText, string imageKey,
			AutoCompleteValueGatherer autoCompleteProvider)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Contributors";

			_model = new ContributorsListControlViewModel(autoCompleteProvider);

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
		private string HandleValidatingContributor(ContributorsListControl sender,
			Contribution contribution, CancelEventArgs e)
		{
			e.Cancel = true;

			if (string.IsNullOrEmpty(contribution.ContributorName) && contribution.Role == null)
				return "Please enter a contributor's name and role.";

			if (string.IsNullOrEmpty(contribution.ContributorName))
				return "Please enter a contributor's name.";

			if (contribution.Role == null)
				return "Please choose a contributor's role.";

			e.Cancel = false;
			SaveContributors();
			return null;
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
	}
}
