using System.ComponentModel;
using System.Windows.Forms;
using SayMore.ClearShare;
using SayMore.Model.Files;

namespace SayMore.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	public partial class ContributorsEditor : EditorBase
	{
		protected ContributorsListControl _contributorsControl;
		protected ContributorsListControlViewModel _model;

		/// ------------------------------------------------------------------------------------
		public ContributorsEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Contributors";

			_model = new ContributorsListControlViewModel();

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
			_model.SetWorkFromXML(file.GetStringValue("work", null));
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
			SaveWork();
			return null;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleContributorDeleted(object sender, System.EventArgs e)
		{
			SaveWork();
		}

		/// ------------------------------------------------------------------------------------
		public void SaveWork()
		{
			var xml = _model.GetXMLFromWork();

			if (!string.IsNullOrEmpty(xml))
			{
				string failureMessage;
				_file.SetValue("work", xml, out failureMessage);
				_file.Save();
				if (failureMessage != null)
					Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
			}
		}
	}
}
