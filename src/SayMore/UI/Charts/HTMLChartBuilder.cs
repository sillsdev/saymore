using System;
using System.Collections.Generic;
using System.Drawing;
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

		protected readonly StatisticsViewModel _statisticsViewModel;
		protected StringBuilder _htmlText = new StringBuilder();

		/// ------------------------------------------------------------------------------------
		public HTMLChartBuilder(StatisticsViewModel statisticsModel)
		{
			_statisticsViewModel = statisticsModel;
		}

		/// ------------------------------------------------------------------------------------
		public string GetStatisticsCharts()
		{
			_htmlText.Length = 0;

			OpenHtml();

			WriteHtmlHead(string.Format("SayMore statistics for {0}",
				_statisticsViewModel.ProjectName));

			OpenBody();
			WriteOverviewSection();

			var colors = GetStatusColors();
			WriteChartBy("By Genre", "genre", "status", colors);
			//WriteChartBy("By Location", "location", "status", colors);

			CloseBody();
			CloseHtml();

			return _htmlText.ToString();
		}

		/// ------------------------------------------------------------------------------------
		protected IDictionary<string, Color> GetStatusColors()
		{
			var statusColors = new Dictionary<string, Color>();

			foreach (var statusName in Enum.GetNames(typeof(Event.Status)).Where(x => x != Event.Status.Skipped.ToString()))
			{
				statusColors[statusName.Replace('_', ' ')] =
					(Color)Properties.Settings.Default[statusName + "StatusColor"];
			}

			return statusColors;
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteOverviewSection()
		{
			WriteHeading("Overview");
			OpenTable("overview");
			OpenTableHead();

			foreach (var kvp in _statisticsViewModel.GetElementStatisticsPairs())
			{
				OpenTableRow();
				WriteTableRowHead(kvp.Key);
				WriteTableCell("elementcount", kvp.Value);
				CloseTableRow();
			}

			CloseTableHead();
			OpenTableBody();

			foreach (var stats in _statisticsViewModel.GetComponentRoleStatisticsPairs())
			{
				OpenTableRow();
				WriteTableRowHead(string.Format("{0}:", stats.Name));
				WriteTableCell(stats.Length);
				WriteTableCell(stats.Size);
				CloseTableRow();
			}

			CloseTableBody();
			CloseTable();
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteChartBy(string chartHeading, string primaryField,
			string secondaryField, IDictionary<string, Color> colors)
		{
			WriteHeading(chartHeading);
			OpenTable("chart");
			OpenTableBody();

			OpenTableRow();
			WriteTableCell("shimrow", kNonBreakingSpace);
			WriteTableCell("shimrow shimrow-right-col", kNonBreakingSpace);
			CloseTableRow();

			var eventList =
				_statisticsViewModel.EventInformant.GetCategorizedEventsFromDoubleKey(primaryField, secondaryField);

			// Loop through each primary field.
			foreach (var kvp in eventList.OrderBy(x => x.Key.Trim('<', '>')))
				WriteChartEntry(kvp.Key.Trim('<', '>'), kvp.Value, secondaryField, colors);

			WriteLegend(colors);

			CloseTableBody();
			CloseTable();
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteChartEntry(string primaryFieldValue,
			IDictionary<string, IEnumerable<Event>> eventsKeyedBySecondaryField,
			string secondaryField, IDictionary<string, Color> colors)
		{
			OpenTableRow();
			WriteTableCell("rowheading", primaryFieldValue);
			OpenTableCell("colorbar");
			OpenTable();
			OpenTableRow();

			var orderedList = (secondaryField == "status" ?
				eventsKeyedBySecondaryField.OrderBy(x => x.Key, new EventStatusComparer()).Where(x => x.Key != Event.Status.Skipped.ToString()) :
				eventsKeyedBySecondaryField.OrderBy(x => x.Key.Trim('<', '>')));

			var summaryText = WriteColoredBar(
				orderedList.ToDictionary(pair => pair.Key, pair => pair.Value), colors);

			CloseTableRow();
			CloseTable();
			CloseTableCell();
			CloseTableRow();

			OpenTableRow();
			WriteTableCell(string.Empty);
			WriteTableCell("entrysummary", summaryText);
			CloseTableRow();
		}

		/// ------------------------------------------------------------------------------------
		protected string WriteColoredBar(
			IDictionary<string, IEnumerable<Event>> eventsKeyedBySecondaryField,
			IDictionary<string, Color> colors)
		{
			int totalEvents = 0;
			int totalMinutes = 0;

			foreach (var kvp in eventsKeyedBySecondaryField)
			{
				var eventsInSegment = kvp.Value.Count();
				var minutesInSegment = kvp.Value.Sum(x => x.GetTotalMediaDuration().TotalMinutes);

				var roundedMinutesInSegment = (int)Math.Ceiling(minutesInSegment);
				totalMinutes += roundedMinutesInSegment;
				totalEvents += eventsInSegment;

				if (roundedMinutesInSegment > 0)
					WriteColoredBarSegment(kvp.Key, eventsInSegment, roundedMinutesInSegment, colors[kvp.Key]);
			}

			return string.Format("{0} events totaling {1} minutes", totalEvents, totalMinutes);
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteColoredBarSegment(string fieldValue, int eventsInSegment,
			int minutesInSegment, Color clrSegment)
		{
			var segmentWidth = minutesInSegment * kPixelsPerMinute;
			var segmentText = (segmentWidth >= 9 ? minutesInSegment.ToString() : kNonBreakingSpace);
			var tooltipText = string.Format("{0}: {1} events totaling {2} minutes",
				fieldValue, eventsInSegment, minutesInSegment);

			WriteTableCell(null, segmentWidth, clrSegment, tooltipText, segmentText);
		}

		/// ------------------------------------------------------------------------------------
		private void WriteLegend(IDictionary<string, Color> colors)
		{
			OpenTableRow();
			WriteTableCell(string.Empty);
			OpenTableCell("legend");
			OpenTable();
			OpenTableRow();

			foreach (var kvp in colors)
			{
				WriteTableCell(null, 11, kvp.Value, null);
				WriteTableCell("legendtext", kvp.Key.Replace('_', ' '));
			}

			CloseTableRow();
			CloseTable();
			CloseTableCell();
			CloseTableRow();
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenHtml()
		{
			_htmlText.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" " +
				"\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">" +
			"<html xml:lang=\"en\" lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
		}

		/// ------------------------------------------------------------------------------------
		protected void CloseHtml()
		{
			_htmlText.Append("</html>");
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteHtmlHead(string htmlDocTitle)
		{
			_htmlText.AppendFormat(
				"<head>" +
					"<title>{0}</title>" +
					"<meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\"/>" +
					"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />" +
					"<style type=\"text/css\">{1}</style>" +
				"</head>", htmlDocTitle, Properties.Resources.StatisticsViewStyles);
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenBody()
		{
			_htmlText.Append("<body>");
		}

		/// ------------------------------------------------------------------------------------
		protected void CloseBody()
		{
			_htmlText.Append("</body>");
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteHeading(string heading)
		{
			_htmlText.AppendFormat("<h2>{0}</h2>", heading);
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenTable()
		{
			OpenTable(null);
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenTable(string className)
		{
			if (string.IsNullOrEmpty(className))
				_htmlText.Append("<table cellspacing=\"0\" cellpadding=\"0\">");
			else
				_htmlText.AppendFormat("<table class=\"{0}\" cellspacing=\"0\" cellpadding=\"0\">", className);
		}

		/// ------------------------------------------------------------------------------------
		protected void CloseTable()
		{
			_htmlText.Append("</table>");
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenTableHead()
		{
			_htmlText.Append("<thead>");
		}

		/// ------------------------------------------------------------------------------------
		protected void CloseTableHead()
		{
			_htmlText.Append("</thead>");
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenTableRow()
		{
			_htmlText.Append("<tr>");
		}

		/// ------------------------------------------------------------------------------------
		protected void CloseTableRow()
		{
			_htmlText.Append("</tr>");
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteTableRowHead(string content)
		{
			_htmlText.AppendFormat("<th scope=\"row\">{0}</th>", content);
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenTableBody()
		{
			_htmlText.Append("<tbody>");
		}

		/// ------------------------------------------------------------------------------------
		protected void CloseTableBody()
		{
			_htmlText.Append("</tbody>");
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteTableCell(string content)
		{
			WriteTableCell(null, 0, Color.Empty, content);
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteTableCell(string className, string content)
		{
			WriteTableCell(className, 0, Color.Empty, content);
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteTableCell(string className, int width, Color bkgndColor, string content)
		{
			WriteTableCell(className, width, bkgndColor, null, content);
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteTableCell(string className, int width, Color bkgndColor,
			string tooltip, string content)
		{
			OpenTableCell(className, width, bkgndColor, tooltip);
			_htmlText.Append(content);
			CloseTableCell();
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenTableCell()
		{
			OpenTableCell(null, 0, Color.Empty);
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenTableCell(string className)
		{
			OpenTableCell(className, 0, Color.Empty);
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenTableCell(string className, int width, Color bkgndColor)
		{
			OpenTableCell(className, width, bkgndColor, null);
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenTableCell(string className, int width, Color bkgndColor, string tooltip)
		{
			_htmlText.Append("<td");

			if (!string.IsNullOrEmpty(className))
				_htmlText.AppendFormat(" class=\"{0}\"", className);

			if (width > 0 || bkgndColor != Color.Empty)
			{
				var styleInfo = string.Empty;

				if (width > 0)
					styleInfo += string.Format("width: {0}px; ", width);

				if (bkgndColor != Color.Empty)
				{
					var color = (bkgndColor.ToArgb() & 0xFFFFFF).ToString("x6");
					styleInfo += string.Format("background-color: #{0}; ", color);
				}

				_htmlText.AppendFormat(" style=\"{0}\"", styleInfo);
			}

			if (!string.IsNullOrEmpty(tooltip))
				_htmlText.AppendFormat(" title=\"{0}\"", tooltip);

			_htmlText.AppendFormat(">");
		}

		/// ------------------------------------------------------------------------------------
		protected void CloseTableCell()
		{
			_htmlText.Append("</td>");
		}
	}
}
