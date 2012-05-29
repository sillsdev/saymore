using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Palaso.Reporting;
using SayMore.Model;
using SayMore.Properties;
using Palaso.Extensions;
using SilTools;

namespace SayMore.UI.ElementListScreen
{
	public class SessionsGrid : ElementGrid
	{
		public delegate SessionsGrid Factory();  //autofac uses this

		private readonly StagesDataProvider _stagesDataProvider;
		private readonly StagesControlToolTip _tooltip;

		/// ------------------------------------------------------------------------------------
		public SessionsGrid(StagesDataProvider stagesDataProvider, StagesControlToolTip toolTip)
		{
			_stagesDataProvider = stagesDataProvider;
			_tooltip = toolTip;
		}

		/// ------------------------------------------------------------------------------------
		public override GridSettings GridSettings
		{
			get { return Settings.Default.SessionsListGrid; }
			set { Settings.Default.SessionsListGrid = value; }
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
				return date;
			}

			return base.GetValueForField(element, fieldName);
		}

		/// ------------------------------------------------------------------------------------
		protected override object GetSortValueForField(ProjectElement element, string fieldName)
		{
			if (fieldName == "status")
			{
				var statusString = base.GetValueForField(element, fieldName) as string;
				var status = (Session.Status)Enum.Parse(typeof(Session.Status), statusString.Replace(' ', '_'));
				return (int)status;
			}

			if (fieldName == "stages")
				return _stagesDataProvider.GetCompletedRolesKey(element.GetCompletedStages());

			if (fieldName == "date")
			{
				var dateString = base.GetValueForField(element, fieldName) as string;

				try
				{
					//we parse it and then generate it because we're trying to migrate old, locale-specific dates to ISO8601 dates
					return DateTimeExtensions.ParseDateTimePermissivelyWithException(dateString).ToISO8601DateOnlyString();
				}
				catch (Exception e)
				{
#if DEBUG
					ErrorReport.NotifyUserOfProblem(e, "only seeing because your'e in DEBUG mode");
#endif
					return DateTime.MinValue.ToISO8601DateOnlyString();
				}
			}

			return base.GetSortValueForField(element, fieldName) as string;
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ToolStripMenuItem> GetMenuCommands()
		{
			var cmds = base.GetMenuCommands().ToList();

			var menu = new ToolStripMenuItem(string.Empty,
				Resources.RampIcon, (s, e) => ((Session)GetCurrentElement()).CreateArchiveFile());

			menu.Text = LocalizationManager.GetString("SessionsView.SessionsList.RampArchiveMenuText",
				"Archive with RAMP (SIL)...", null, menu);

			cmds.Insert(0, menu);

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
				var statusText = Session.GetLocalizedStatus(value as string);

				e.ToolTipText = string.Format(
					LocalizationManager.GetString("SessionsView.SessionStatus.TooltipFormatText", "Status: {0}"), statusText);
			}

			base.OnCellToolTipTextNeeded(e);
		}
	}
}
