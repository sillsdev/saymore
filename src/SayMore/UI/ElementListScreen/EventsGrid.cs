using System;
using System.Linq;
using SayMore.Model;
using SayMore.Properties;
using SilUtils;

namespace SayMore.UI.ElementListScreen
{
	public class EventsGrid : ElementGrid
	{
		public delegate EventsGrid Factory();  //autofac uses this

		private readonly StagesImageMaker _stagesImageMaker;
		private readonly StagesControlToolTip _tooltip;

		/// ------------------------------------------------------------------------------------
		public EventsGrid(StagesImageMaker stagesImageMaker, StagesControlToolTip toolTip)
		{
			_stagesImageMaker = stagesImageMaker;
			_tooltip = toolTip;
		}

		/// ------------------------------------------------------------------------------------
		public override GridSettings GridSettings
		{
			get { return Settings.Default.EventsListGrid; }
			set { Settings.Default.EventsListGrid = value; }
		}

		/// ------------------------------------------------------------------------------------
		protected override object GetValueForField(ProjectElement element, string fieldName)
		{
			if (fieldName == "status")
			{
				var value = base.GetValueForField(element, fieldName);
				return Resources.ResourceManager.GetObject("Status" + ((string)value).Replace(' ', '_'));
			}

			if (fieldName == "stages")
				return _stagesImageMaker.CreateImageForComponentStage(element.GetCompletedStages());

			if (fieldName == "date")
			{
				var date = base.GetValueForField(element, fieldName);
				return (string.IsNullOrEmpty(date as string) ?
					date : DateTime.Parse(date as string).ToShortDateString());
			}

			return base.GetValueForField(element, fieldName);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseEnter(System.Windows.Forms.DataGridViewCellEventArgs e)
		{
			base.OnCellMouseEnter(e);

			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
				Columns[e.ColumnIndex].DataPropertyName == "stages")
			{
				var element = _items.ElementAt(e.RowIndex);
				var pt = MousePosition;
				pt.Offset(5, 5);
				_tooltip.Show(pt, element.GetCompletedStages());
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseLeave(System.Windows.Forms.DataGridViewCellEventArgs e)
		{
			base.OnCellMouseLeave(e);

			if (Columns[e.ColumnIndex].DataPropertyName == "stages")
				_tooltip.Hide();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellToolTipTextNeeded(System.Windows.Forms.DataGridViewCellToolTipTextNeededEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
				Columns[e.ColumnIndex].DataPropertyName == "status")
			{
				var value = base.GetValueForField(_items.ElementAt(e.RowIndex), "status");
				e.ToolTipText = string.Format("Status: {0}", value);
			}

			base.OnCellToolTipTextNeeded(e);
		}
	}
}
