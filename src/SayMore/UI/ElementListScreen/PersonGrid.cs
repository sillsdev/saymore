using System.Linq;
using System.Windows.Forms;
using SayMore.Model;
using SayMore.Properties;
using SilTools;

namespace SayMore.UI.ElementListScreen
{
	public class PersonGrid : ElementGrid
	{
		public delegate PersonGrid Factory();  //autofac uses this

		/// ------------------------------------------------------------------------------------
		public override GridSettings GridSettings
		{
			get { return Settings.Default.PersonListGrid; }
			set { Settings.Default.PersonListGrid = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override object GetValueForField(ProjectElement element, string fieldName)
		{
			if (fieldName != "consent")
				return base.GetValueForField(element, fieldName);

			return ((Person)element).GetInformedConsentImage();
		}

		/// ------------------------------------------------------------------------------------
		protected override object GetSortValueForField(ProjectElement element, string fieldName)
		{
			return (fieldName == "consent" ?
				((Person)element).GetInformedConsentSortKey() :
				base.GetSortValueForField(element, fieldName));
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
		{
			base.OnCellMouseEnter(e);

			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
				Columns[e.ColumnIndex].DataPropertyName == "consent")
			{
				var element = _items.ElementAt(e.RowIndex);
				this[e.ColumnIndex, e.RowIndex].ToolTipText =
					((Person)element).GetToolTipForInformedConsentType();
			}
		}
	}
}
