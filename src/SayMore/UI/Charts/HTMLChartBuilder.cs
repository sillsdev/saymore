using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SayMore.Model;
using SayMore.UI.Overview.Statistics;

namespace SayMore.UI.Charts
{
	/// ----------------------------------------------------------------------------------------
	public class HTMLChartBuilder
	{
		protected const int kPixelsPerMinute = 3;
		protected const string kNonBreakingSpace = "&nbsp;";

		protected const string kTitleToken = "$title$";
		protected const string kOverviewHeadingToken = "$overviewheading$";
		protected const string kElementsOverviewRowsToken = "<!-- $elementoverviewrows$ -->";
		protected const string kComponentRolesOverviewRowsToken = "<!-- $componentroleoverviewrows$ -->";
		protected const string kChartTablesToken = "<!--$charttables$-->";

		protected const string kElementRow =
			"<tr><th scope=\"row\">{0}</th><td class=\"elementcount\">{1}</td></tr>";

		protected const string kComponentRoleInfoRow =
			"<tr><th scope=\"row\" class=\"componentrow\">{0}</th><td>{1}</td><td>{2}</td></tr>";

		protected const string kChartTable =
			"<h2>{0}</h2>" +									// Chart heading
			"<table cellspacing=\"0\" cellpadding=\"0\">" +
				"<tfoot>" +
					"<tr>" +
						"<th></th>" +
						"<td>{1}</td>" +						// Legend
					"</tr>" +
				"</tfoot>" +
				"<tbody>{2}</tbody>" +							// Chart rows.
			"</table>";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parameter 0 = the barcolor number that will match one of the barcolor styles.
		/// When segments represent an enumerated value (e.g. event status), use the int
		/// value of the enumeration. Parameter 1 = the width in pixels of the bar's segment;
		/// parameter 2 = the text to show in a tooltip when the user hovers over the bar's
		/// segment; parameter 3 = the number that goes in the bar segment that represents
		/// how long the bar is. (If parameter 3 is small enough that displaying its text will
		/// make the segment longer than what's specified in parameter 1, then do not show the
		/// number, but insert a non-breaking space instead.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected const string kBarSegment =
			"<div class=\"barcolor{0}\" style=\"width: {1}px\" title=\"{2}\">{3}</div>";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Parameter 0 = label along y-axis (e.g. genre name); parameter 1 = a number of bar
		/// segments (see kBarSegment above); parameter 2 = summary information for the entry
		/// (e.g. 10 events, 33 minutes).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected const string kChartEntry =
			"<tr class=\"colorbar\">" +
				"<th scope=\"rowgroup\" rowspan=\"2\">{0}</th>" +
				"<td>{1}</td>" +
			"</tr>" +
			"<tr class=\"detail\">" +
				"<td>{2}</td>" +
			"</tr>";

		protected readonly StatisticsViewModel _statisticsViewModel;

		/// ------------------------------------------------------------------------------------
		public HTMLChartBuilder(StatisticsViewModel statisticsModel)
		{
			_statisticsViewModel = statisticsModel;
		}

		/// ------------------------------------------------------------------------------------
		public string GetChart()
		{
			var html = new StringBuilder(Properties.Resources.ChartTemplate);
			html = html.Replace(kTitleToken, "SayMore Statistics");
			html = html.Replace(kOverviewHeadingToken, "Overview");
			html = html.Replace(kElementsOverviewRowsToken, GetElementsOverviewSection());
			html = html.Replace(kComponentRolesOverviewRowsToken, GetComponentRolesOverviewSection());

			var tables = new StringBuilder();
			tables.Append(GetChartBy("By Genre", "genre", "status"));
			tables.Append(GetChartBy("By Location", "location", "status"));

			html = html.Replace(kChartTablesToken, tables.ToString());
			return html.ToString();
		}

		/// ------------------------------------------------------------------------------------
		protected string GetElementsOverviewSection()
		{
			var rows = new StringBuilder();

			foreach (var kvp in _statisticsViewModel.GetElementStatisticsPairs())
				rows.AppendFormat(kElementRow, kvp.Key, kvp.Value);

			return rows.ToString();
		}

		/// ------------------------------------------------------------------------------------
		protected string GetComponentRolesOverviewSection()
		{
			var rows = new StringBuilder();

			foreach (var stats in _statisticsViewModel.GetComponentRoleStatisticsPairs())
				rows.AppendFormat(kComponentRoleInfoRow, stats.Name, stats.Length, stats.Size);

			return rows.ToString();
		}

		/// ------------------------------------------------------------------------------------
		protected string GetChartBy(string chartHeading, string primaryField, string secondaryField)
		{
			var eventGenreList =
				_statisticsViewModel.EventInformant.GetCategorizedEventsFromDoubleKey(primaryField, secondaryField);

			var rows = new StringBuilder();
			var colorNumberGenerator = (secondaryField == "status" ?
				TranslateEventStatusToBarColorNumber : (Func<string, int, int>)null);

			// Loop through each primary field.
			foreach (var kvp in eventGenreList.OrderBy(x => x.Key.Trim('<', '>')))
			{
				var eventsHavingPrimaryFieldValue = (secondaryField == "status" ?
					kvp.Value.OrderBy(x => x.Key, new EventStatusComparer()) :
					kvp.Value.OrderBy(x => x.Key.Trim('<', '>')));

				rows.Append(GetChartEntry(kvp.Key.Trim('<', '>'),
					eventsHavingPrimaryFieldValue.ToDictionary(x => x.Key, x => x.Value),
					colorNumberGenerator));
			}

			return string.Format(kChartTable, chartHeading, "legend goes here", rows);
		}

		/// ------------------------------------------------------------------------------------
		protected string GetChartEntry(string primaryFieldValue,
			IDictionary<string, IEnumerable<Event>> eventsHavingPrimaryFieldValue,
			Func<string, int, int> colorNumberGenerator)
		{
			int totalEvents = 0;
			int totalMinutes = 0;
			int colorNumber = 0;
			var barSegments = new StringBuilder();

			foreach (var kvp in eventsHavingPrimaryFieldValue)
			{
				int minutesInSegment = 0;

				foreach (var evnt in kvp.Value)
				{
					minutesInSegment += evnt.GetTotalMediaDuration().Minutes;
					totalEvents++;
				}

				if (minutesInSegment > 0)
				{
					if (colorNumberGenerator != null)
						colorNumber = colorNumberGenerator(kvp.Key, colorNumber);

					barSegments.Append(GetBarSegment(kvp.Key, kvp.Value.Count(), minutesInSegment, colorNumber));
					totalMinutes += minutesInSegment;
				}

				colorNumber++;
			}

			var details = string.Format("{0} events totalling {1} minutes", totalEvents, totalMinutes);
			return string.Format(kChartEntry, primaryFieldValue, barSegments, details);
		}

		/// ------------------------------------------------------------------------------------
		protected string GetBarSegment(string fieldValue, int eventCount, int minutesInSegment, int colorNumber)
		{
			var segmentWidth = minutesInSegment * kPixelsPerMinute;
			var segmentText = (segmentWidth >= 9 ? minutesInSegment.ToString() : kNonBreakingSpace);

			var tooltipText = string.Format("{0}: {1} events totalling {2} minutes",
				fieldValue, eventCount, minutesInSegment);

			return string.Format(kBarSegment, colorNumber, segmentWidth, tooltipText, segmentText);
		}

		/// ------------------------------------------------------------------------------------
		protected IDictionary<Event.Status, TimeSpan> GetTimesForEachStatus(IEnumerable<Event> eventList)
		{
			// Initialize a dictionary of event status'
			var timesByStatus = new Dictionary<Event.Status, TimeSpan>();
			foreach (Event.Status status in Enum.GetValues(typeof(Event.Status)))
				timesByStatus[status] = new TimeSpan();

			foreach (var evnt in eventList)
				timesByStatus[evnt.GetStatus()] += evnt.GetTotalMediaDuration();

			timesByStatus.Remove(Event.Status.Skipped);
			return timesByStatus;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual int TranslateEventStatusToBarColorNumber(string status, int defaultValue)
		{
			status = status.Replace(' ', '_');

			return (Enum.GetNames(typeof(Event.Status)).Contains(status) ?
				(int)Enum.Parse(typeof(Event.Status), status) : defaultValue);
		}
	}
}
