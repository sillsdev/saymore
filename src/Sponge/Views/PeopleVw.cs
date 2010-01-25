using System;
using SIL.Sponge.Model;

namespace SIL.Sponge
{
	public partial class PeopleVw : BaseSplitVw
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="PeopleVw"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public PeopleVw()
		{
			InitializeComponent();

			//silGrid1.RowCount = 16;
			silGrid1.Rows.Add("Full Name", null);
			silGrid1.Rows.Add("Short Name", null);
			silGrid1.Rows.Add("Pseudonym", null);
			silGrid1.Rows.Add("Privacy", null);
			silGrid1.Rows.Add("BirthYear", null);
			silGrid1.Rows.Add("DeathYear", null);
			var ipres = silGrid1.Rows.Add("Primary Residence", null);
			var iores = silGrid1.Rows.Add("Other Residence", null);
			silGrid1.Rows.Add("Language 1", null);
			silGrid1.Rows.Add("Language 2", null);
			silGrid1.Rows.Add("Language 3", null);
			silGrid1.Rows.Add("Primary Occupation", null);
			silGrid1.Rows.Add("Other Occupation", null);
			var ibio = silGrid1.Rows.Add("Biographical Sketch", null);
			var iinfo = silGrid1.Rows.Add("Contact Info.", null);
			var icmnt = silGrid1.Rows.Add("Comment", null);
			silGrid1.AutoResizeColumn(0);


		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This fixes a paint error in .Net that manifests itself when tab controls are
		/// resized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void tabPeople_SizeChanged(object sender, EventArgs e)
		{
			tabPeople.Invalidate();
		}
	}
}
