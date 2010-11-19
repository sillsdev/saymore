using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Palaso.Code;
using SayMore.UI.ComponentEditors;

namespace SayMore.ClearShare
{
	public partial class ContributorsListControl : UserControl
	{
		public ContributorsListControl()
		{
			InitializeComponent();

			_grid.Columns.Add("Role", "Role");
			_grid.Columns.Add("Person", "Person");
			_grid.Columns.Add("Date", "Date");
			_grid.Columns.Add("Notes", "Notes");
		}

		public DummyContributionsRepository Repository { get; set; }

		private void ContributorsListControl_Load(object sender, EventArgs e)
		{
			Guard.AgainstNull(Repository,"Repository");

			foreach (Contribution contribution in Repository.AllItems)
			{

			}
		}
	}
}
