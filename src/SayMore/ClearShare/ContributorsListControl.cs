using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Palaso.Code;
using SayMore.Properties;
using SilUtils;

namespace SayMore.ClearShare
{
	/// ----------------------------------------------------------------------------------------
	public partial class ContributorsListControl : UserControl
	{
		public delegate string ValidatingContributorHandler(ContributorsListControl sender,
			Contribution contribution, CancelEventArgs e);

		public event ValidatingContributorHandler ValidatingContributor;
		public event EventHandler ContributorDeleted;

		private Contribution _preValidatedContribution;
		private readonly ContributorsListControlViewModel _model;

		/// ------------------------------------------------------------------------------------
		public ContributorsListControl()
		{
			InitializeComponent();
		}

		/// ------------------------------------------------------------------------------------
		public ContributorsListControl(ContributorsListControlViewModel model) : this()
		{
			_model = model;
			_model.NewContributionListAvailable += HandleNewContributionListAvailable;
			Initialize();
		}

		/// ------------------------------------------------------------------------------------
		private void Initialize()
		{
			_grid.Font = SystemFonts.IconTitleFont;
			_grid.Columns.Add(SilGrid.CreateTextBoxColumn("name", "Name"));

			var col = SilGrid.CreateDropDownListComboBoxColumn("role",
				_model.OlacRoles.Select(r => r.ToString()));

			col.HeaderText = "Role";
			_grid.Columns.Add(col);

			_grid.Columns.Add(SilGrid.CreateCalendarControlColumn("date", "Date"));
			_grid.Columns.Add(SilGrid.CreateTextBoxColumn("notes", "Notes"));

			_grid.AllowUserToAddRows = true;
			_grid.AllowUserToDeleteRows = true;
			_grid.CurrentRowChanged += HandleGridCurrentRowChanged;
			_grid.RowValidating += HandleGridRowValidating;
			_grid.MouseClick += HandleGridMouseClick;
			_grid.Enter += HandleGridEnter;
			_grid.Leave += HandleGridLeave;

			if (Settings.Default.ContributorsGrid != null)
				Settings.Default.ContributorsGrid.InitializeGrid(_grid);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the extender is currently in design mode.
		/// I have had some problems with the base class' DesignMode property being true
		/// when in design mode. I'm not sure why, but adding a couple more checks fixes the
		/// problem.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private new bool DesignMode
		{
			get
			{
				return (base.DesignMode || GetService(typeof(IDesignerHost)) != null) ||
					(LicenseManager.UsageMode == LicenseUsageMode.Designtime);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (!DesignMode)
				Settings.Default.ContributorsGrid = GridSettings.Create(_grid);

			base.OnHandleDestroyed(e);
		}

		/// ------------------------------------------------------------------------------------
		void HandleNewContributionListAvailable(object sender, EventArgs e)
		{
			Guard.AgainstNull(_model.Contributions, "Contributions");

			// Add one for the new contributor row.
			_grid.RowCount = _model.Contributions.Count() + 1;
			_grid.CurrentCell = _grid[0, 0];
		}

		/// ------------------------------------------------------------------------------------
		private void UpdateDisplay()
		{
			_toolTip.SetToolTip(_buttonDelete, null);

			if (!_model.GetCanDeleteContribution(_grid.CurrentCellAddress.Y))
				_buttonDelete.Enabled = false;
			else
			{
				_buttonDelete.Enabled = true;
				var name = _model.GetContributionValue(_grid.CurrentCellAddress.Y, "name") as string;
				if (!string.IsNullOrEmpty(name))
				{
					var tooltip = string.Format("Delete contributor '{0}'", name);
					_toolTip.SetToolTip(_buttonDelete, tooltip);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleGridCurrentRowChanged(object sender, EventArgs e)
		{
			_preValidatedContribution = _model.GetContributionCopy(_grid.CurrentCellAddress.Y);
			UpdateDisplay();
		}

		/// ------------------------------------------------------------------------------------
		void HandleGridMouseClick(object sender, MouseEventArgs e)
		{
			var hi = _grid.HitTest(e.X, e.Y);
			if (e.Button == MouseButtons.Left && hi.ColumnIndex == -1)
				_grid.CurrentCell = _grid[0, hi.RowIndex];
		}

		/// ------------------------------------------------------------------------------------
		void HandleGridEnter(object sender, EventArgs e)
		{
			_grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
		}

		/// ------------------------------------------------------------------------------------
		void HandleGridLeave(object sender, EventArgs e)
		{
			_grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			if (_grid.CurrentRow != null)
				_grid.CurrentRow.Selected = true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleGridRowValidating(object sender, DataGridViewCellCancelEventArgs e)
		{
			if (ValidatingContributor == null || e.RowIndex == _grid.RowCount - 1)
				return;

			var contribution = _model.Contributions.ElementAt(e.RowIndex);

			// Don't bother doing anything if the contribution didn't change.
			if (contribution.AreContentsEqual(_preValidatedContribution))
				return;

			var args = new CancelEventArgs(e.Cancel);
			var errorMsg = ValidatingContributor(this, contribution, args);
			e.Cancel = args.Cancel;

			if (!string.IsNullOrEmpty(errorMsg))
				Palaso.Reporting.ProblemNotificationDialog.Show(errorMsg);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			var value = _model.GetContributionValue(e.RowIndex, _grid.Columns[e.ColumnIndex].Name);
			if (value != null)
				e.Value = value;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			_model.SetContributionValue(e.RowIndex, _grid.Columns[e.ColumnIndex].Name, e.Value);
		}

		/// ------------------------------------------------------------------------------------
		private void HandleDeleteButtonClicked(object sender, EventArgs e)
		{
			if (!_model.DeleteContribution(_grid.CurrentCellAddress.Y))
				return;

			int rowIndex = _grid.CurrentCellAddress.Y;
			_grid.CurrentRowChanged -= HandleGridCurrentRowChanged;
			_grid.RowCount--;
			_grid.CurrentRowChanged += HandleGridCurrentRowChanged;

			if (rowIndex >= _grid.RowCount - 1 && rowIndex > 0)
				rowIndex--;

			_grid.CurrentCell = _grid[_grid.CurrentCellAddress.X, rowIndex];
			UpdateDisplay();

			if (ContributorDeleted != null)
				ContributorDeleted(this, EventArgs.Empty);
		}
	}
}
