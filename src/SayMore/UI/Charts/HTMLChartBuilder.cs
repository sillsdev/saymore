using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SayMore.Model;

namespace SayMore.UI.Charts
{
	/// ----------------------------------------------------------------------------------------
	public class HTMLChartBuilder
	{
		protected const int kPixelsPerMinute = 3;
		protected const string kTitleToken = "$title$";
		protected const string kChartHeadingToken = "$chartheading$";
		protected const string kLegendToken = "$legend$";
		protected const string kRowsToken = "<!--$chartrows$-->";

		protected const string kNonBreakingSpace = "&nbsp;";

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
		protected const string kFullEntry =
			"<tr class=\"colorbar\">" +
				"<th scope=\"rowgroup\" rowspan=\"2\">{0}</th>" +
				"<td>{1}</td>" +
			"</tr>" +
			"<tr class=\"detail\">" +
				"<td>{2}</td>" +
			"</tr>";

		protected EventWorkflowInformant _informant;
		protected string _htmlText;

		/// ------------------------------------------------------------------------------------
		public HTMLChartBuilder(EventWorkflowInformant informant)
		{
			_informant = informant;

			_htmlText = Properties.Resources.ChartTemplate;
		}

		/// ------------------------------------------------------------------------------------
		public string GetGenreChart()
		{
			var html = _htmlText.Replace(kTitleToken, "Genre Chart");
			html = html.Replace(kChartHeadingToken, "By Genre");

			var eventGenreList = _informant.GetEventsForEachGenre();
			var rows = new StringBuilder();

			// Loop through each genre.
			foreach (var kvp in eventGenreList)
			{
				int totalMinutes;
				var barSegments = GetBarSegments(kvp.Value, out totalMinutes);
				var details = string.Format("{0} events totalling {1} minutes", kvp.Value.Count(), totalMinutes);
				rows.AppendFormat(kFullEntry, kvp.Key.Trim('<', '>'), barSegments, details);
			}

			return html.Replace(kRowsToken, rows.ToString());
		}

		/// ------------------------------------------------------------------------------------
		protected string GetBarSegments(IEnumerable<Event> eventList, out int totalMinutes)
		{
			totalMinutes = 0;
			var barSegments = new StringBuilder();

			foreach (var kvp in GetTimesForEachStatus(eventList))
			{
				var minutes = kvp.Value.Minutes;
				if (minutes == 0)
					continue;

				totalMinutes += minutes;
				var segmentWidth = minutes * kPixelsPerMinute;
				var segmentText = (segmentWidth >= 9 ? minutes.ToString() : kNonBreakingSpace);
				barSegments.AppendFormat(kBarSegment, (int)kvp.Key, segmentWidth, "tooltip", segmentText);
			}

			return barSegments.ToString();
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
	}
}
