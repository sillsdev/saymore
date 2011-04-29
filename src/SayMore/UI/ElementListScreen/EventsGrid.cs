using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model;
using SayMore.Properties;
using SilUtils;

namespace SayMore.UI.ElementListScreen
{
	public class EventsGrid : ElementGrid
	{
		public delegate EventsGrid Factory();  //autofac uses this

		private readonly StagesDataProvider _stagesDataProvider;
		private readonly StagesControlToolTip _tooltip;

		/// ------------------------------------------------------------------------------------
		public EventsGrid(StagesDataProvider stagesDataProvider, StagesControlToolTip toolTip)
		{
			_stagesDataProvider = stagesDataProvider;
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
				return _stagesDataProvider.CreateImageForComponentStage(element.GetCompletedStages());

			if (fieldName == "date")
			{
				var date = base.GetValueForField(element, fieldName);
				return (string.IsNullOrEmpty(date as string) ?
					date : DateTime.Parse(date as string).ToShortDateString());
			}

			return base.GetValueForField(element, fieldName);
		}

		/// ------------------------------------------------------------------------------------
		protected override object GetSortValueForField(ProjectElement element, string fieldName)
		{
			if (fieldName == "status")
			{
				var statusString = base.GetValueForField(element, fieldName) as string;
				var status = (Event.Status)Enum.Parse(typeof(Event.Status), statusString.Replace(' ', '_'));
				return (int)status;
			}

			if (fieldName == "stages")
				return _stagesDataProvider.GetCompletedRolesKey(element.GetCompletedStages());

			if (fieldName == "date")
			{
				var dateString = base.GetValueForField(element, fieldName) as string;
				DateTime date = DateTime.MinValue;
				DateTime.TryParse(dateString ?? string.Empty, out date);
				return date;
			}

			return base.GetSortValueForField(element, fieldName) as string;
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<ToolStripMenuItem> GetMenuCommands()
		{
			var cmds = base.GetMenuCommands().ToList();

			cmds.Insert(0, new ToolStripMenuItem("Archive with RAMP (SIL)",
				Resources.RampIcon, (s, e) => ((Event)GetCurrentElement()).CreateArchiveFile()));

			return cmds;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
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
		protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
		{
			base.OnCellMouseLeave(e);

			if (Columns[e.ColumnIndex].DataPropertyName == "stages")
				_tooltip.Hide();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnCellToolTipTextNeeded(DataGridViewCellToolTipTextNeededEventArgs e)
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
