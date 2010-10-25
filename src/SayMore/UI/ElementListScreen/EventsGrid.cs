using System;
using System.Linq;
using SayMore.Model;

namespace SayMore.UI.ElementListScreen
{
	public class EventsGrid : ElementGrid
	{
		private readonly EventComponentToolTip _tooltip = new EventComponentToolTip();

		/// ------------------------------------------------------------------------------------
		protected override object GetValueForField(ProjectElement element, string fieldName)
		{
			if (fieldName != "completed")
				return base.GetValueForField(element, fieldName);

			var completedComponents = element.MetaDataFile.GetStringValue(fieldName, string.Empty);
			return Event.GetImageForComponentStage(GetComponentStageFromString(completedComponents));
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseEnter(System.Windows.Forms.DataGridViewCellEventArgs e)
		{
			base.OnCellMouseEnter(e);

			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
				Columns[e.ColumnIndex].DataPropertyName == "completed")
			{
				var element = _items.ElementAt(e.RowIndex);
				var text = element.MetaDataFile.GetStringValue("completed", string.Empty);
				var pt = MousePosition;
				pt.Offset(5, 5);
				_tooltip.Show(pt, GetComponentStageFromString(text));
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseLeave(System.Windows.Forms.DataGridViewCellEventArgs e)
		{
			base.OnCellMouseLeave(e);

			if (Columns[e.ColumnIndex].DataPropertyName == "completed")
				_tooltip.Hide();
		}

		/// ------------------------------------------------------------------------------------
		private static Event.ComponentStage GetComponentStageFromString(string text)
		{
			return (string.IsNullOrEmpty(text) ? Event.ComponentStage.None :
				(Event.ComponentStage)Enum.Parse(typeof(Event.ComponentStage), text));
		}
	}
}
