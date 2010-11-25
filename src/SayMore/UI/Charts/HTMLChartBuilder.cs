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
		protected const string kNonBreakingSpace = "&nbsp;";
		protected const int kPixelsPerMinute = 3;

		protected readonly StatisticsViewModel _statsViewModel;
		protected StringBuilder _htmlText = new StringBuilder();

		/// ------------------------------------------------------------------------------------
		public HTMLChartBuilder(StatisticsViewModel statsViewModel)
		{
			_statsViewModel = statsViewModel;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is used mainly for tests.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string HtmlContent
		{
			get { return _htmlText.ToString(); }
		}

		/// ------------------------------------------------------------------------------------
		public string GetStatisticsCharts()
		{
			_htmlText.Length = 0;

			OpenHtml();

			WriteHtmlHead(string.Format("SayMore statistics for {0}",
				_statsViewModel.ProjectName));

			OpenBody();
			WriteOverviewSection();
			WriteStageChart();

			var backColors = GetStatusSegmentColors();
			var textColors = backColors.ToDictionary(kvp => kvp.Key, kvp => Color.Empty);
			WriteChartByFieldPair("By Genre", "genre", "status", backColors, textColors);

			CloseBody();
			CloseHtml();

			return _htmlText.ToString();
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteStageChart()
		{
			var eventsByStage = _statsViewModel.EventInformant.GetEventsCategorizedByStage();
			var backColors = eventsByStage.Keys.ToDictionary(r => r.Name, r => r.Color);
			var textColors = eventsByStage.Keys.ToDictionary(r => r.Name, r => r.TextColor);

			// Put this list in a form the chart writing methods can handle.
			var stagesList = eventsByStage.OrderBy(s => s.Key.Name).ToDictionary(
					kvp => kvp.Key.Name,
					kvp => (new[]
					{
						new ColorBarSegmentInfo
						{
							FieldName = "stage",
							FieldValue = kvp.Key.Name,
							Events = kvp.Value,
							BackColor = backColors[kvp.Key.Name],
							TextColor = textColors[kvp.Key.Name]
						}
					}) as IEnumerable<ColorBarSegmentInfo>);

			WriteChartForList("By Stages", stagesList, null, false);
		}

		/// ------------------------------------------------------------------------------------
		protected IDictionary<string, Color> GetStatusSegmentColors()
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

			foreach (var kvp in _statsViewModel.GetElementStatisticsPairs())
			{
				OpenTableRow();
				WriteTableRowHead(kvp.Key);
				WriteTableCell("elementcount", kvp.Value);
				CloseTableRow();
			}

			CloseTableHead();
			OpenTableBody();

			foreach (var stats in _statsViewModel.GetComponentRoleStatisticsPairs())
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
		protected void WriteChartByFieldPair(string chartHeading, string primaryField,
			string secondaryField, IDictionary<string, Color> colors, IDictionary<string, Color> textColors)
		{
			var outerList =
				_statsViewModel.EventInformant.GetCategorizedEventsFromDoubleKey(primaryField, secondaryField);

			var segmentList = (outerList.OrderBy(f => f.Key.Trim('<', '>'))).ToDictionary(
				kvp => kvp.Key.Trim('<', '>'),
				kvp => kvp.Value.Where(x => (secondaryField != "status" || x.Key != Event.Status.Skipped.ToString()))
					.Select(x => new ColorBarSegmentInfo
					{
						FieldName = secondaryField,
						FieldValue = x.Key.Trim('<', '>'),
						Events = x.Value,
						BackColor = colors[x.Key],
						TextColor = textColors[x.Key]
					})
					.OrderBy(x => x) as IEnumerable<ColorBarSegmentInfo>
				);

			WriteChartForList(chartHeading, segmentList, colors, true);
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteChartForList(string chartHeading,
			IDictionary<string, IEnumerable<ColorBarSegmentInfo>> outerList,
			IDictionary<string, Color> legendColors, bool writeLegend)
		{
			WriteHeading(chartHeading);
			OpenTable("chart");
			OpenTableBody();

			OpenTableRow();
			WriteTableCell("shimrow", kNonBreakingSpace);
			WriteTableCell("shimrow shimrow-right-col", kNonBreakingSpace);
			CloseTableRow();

			foreach (var kvp in outerList)
				WriteChartEntry(kvp.Key, kvp.Value);

			WriteLegend(legendColors, writeLegend);

			CloseTableBody();
			CloseTable();
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteChartEntry(string primaryFieldValue,
			IEnumerable<ColorBarSegmentInfo> barSegmentInfo)
		{
			OpenTableRow();
			WriteTableCell("rowheading", primaryFieldValue);
			OpenTableCell("colorbar");
			OpenTable();
			OpenTableRow();

			var summaryText = WriteColoredBar(barSegmentInfo);

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
		protected string WriteColoredBar(IEnumerable<ColorBarSegmentInfo> barSegmentInfo)
		{
			int totalEvents = 0;
			int totalMinutes = 0;

			foreach (var seg in barSegmentInfo)
			{
				var eventsInSegment = seg.Events.Count();
				var minutesInSegment = seg.Events.Sum(x => x.GetTotalMediaDuration().TotalMinutes);

				var roundedMinutesInSegment = (int)Math.Ceiling(minutesInSegment);
				totalMinutes += roundedMinutesInSegment;
				totalEvents += eventsInSegment;

				if (roundedMinutesInSegment > 0)
				{
					WriteColoredBarSegment(seg.FieldValue, eventsInSegment,
						roundedMinutesInSegment, seg.BackColor, seg.TextColor);
				}
			}

			return string.Format("{0} events totaling {1} minutes", totalEvents, totalMinutes);
		}

		/// ------------------------------------------------------------------------------------
		public void WriteColoredBarSegment(string fieldValue, int eventsInSegment,
			int minutesInSegment, Color clrSegment, Color clrText)
		{
			var segmentWidth = minutesInSegment * kPixelsPerMinute;
			var segmentText = (segmentWidth >= 9 ? minutesInSegment.ToString() : kNonBreakingSpace);

			var fmt = (string.IsNullOrEmpty(fieldValue) ?
				"{0}{1} events totaling {2} minutes" : "{0}: {1} events totaling {2} minutes");

			var tooltipText = string.Format(fmt, fieldValue, eventsInSegment, minutesInSegment);

			WriteTableCell(null, segmentWidth, clrSegment, clrText, tooltipText, segmentText);
		}

		/// ------------------------------------------------------------------------------------
		private void WriteLegend(IDictionary<string, Color> colors, bool drawColorBlocks)
		{
			OpenTableRow();
			WriteTableCell(string.Empty);
			OpenTableCell("legend");

			if (drawColorBlocks)
			{
				OpenTable();
				OpenTableRow();

				foreach (var kvp in colors)
				{
					WriteTableCell(null, 11, kvp.Value, null);
					WriteTableCell("legendtext", kvp.Key.Replace('_', ' '));
				}

				CloseTableRow();
				CloseTable();
			}

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
		public void CloseHtml()
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
		public void OpenBody()
		{
			_htmlText.Append("<body>");
		}

		/// ------------------------------------------------------------------------------------
		public void CloseBody()
		{
			_htmlText.Append("</body>");
		}

		/// ------------------------------------------------------------------------------------
		public void WriteHeading(string heading)
		{
			_htmlText.AppendFormat("<h2>{0}</h2>", heading);
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTable()
		{
			OpenTable(null);
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTable(string className)
		{
			className = (className ?? string.Empty);

			if (className.Trim() == string.Empty)
				_htmlText.Append("<table cellspacing=\"0\" cellpadding=\"0\">");
			else
				_htmlText.AppendFormat("<table class=\"{0}\" cellspacing=\"0\" cellpadding=\"0\">", className);
		}

		/// ------------------------------------------------------------------------------------
		public void CloseTable()
		{
			_htmlText.Append("</table>");
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTableHead()
		{
			_htmlText.Append("<thead>");
		}

		/// ------------------------------------------------------------------------------------
		public void CloseTableHead()
		{
			_htmlText.Append("</thead>");
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTableRow()
		{
			_htmlText.Append("<tr>");
		}

		/// ------------------------------------------------------------------------------------
		public void CloseTableRow()
		{
			_htmlText.Append("</tr>");
		}

		/// ------------------------------------------------------------------------------------
		public void WriteTableRowHead(string content)
		{
			content = (content ?? string.Empty);
			_htmlText.AppendFormat("<th scope=\"row\">{0}</th>", content.Trim());
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTableBody()
		{
			_htmlText.Append("<tbody>");
		}

		/// ------------------------------------------------------------------------------------
		public void CloseTableBody()
		{
			_htmlText.Append("</tbody>");
		}

		/// ------------------------------------------------------------------------------------
		public void WriteTableCell(string content)
		{
			WriteTableCell(null, 0, Color.Empty, content);
		}

		/// ------------------------------------------------------------------------------------
		public void WriteTableCell(string className, string content)
		{
			WriteTableCell(className, 0, Color.Empty, content);
		}

		/// ------------------------------------------------------------------------------------
		public void WriteTableCell(string className, int width, Color bkgndColor, string content)
		{
			WriteTableCell(className, width, bkgndColor, null, content);
		}

		/// ------------------------------------------------------------------------------------
		public void WriteTableCell(string className, int width, Color bkgndColor,
			string tooltip, string content)
		{
			WriteTableCell(className, width, bkgndColor, Color.Empty, tooltip, content);
		}

		/// ------------------------------------------------------------------------------------
		public void WriteTableCell(string className, int width, Color bkgndColor,
			Color textColor, string tooltip, string content)
		{
			content = (content ?? string.Empty);
			OpenTableCell(className, width, bkgndColor, textColor, tooltip);
			_htmlText.Append(content.Trim());
			CloseTableCell();
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTableCell()
		{
			OpenTableCell(null, 0, Color.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTableCell(string className)
		{
			OpenTableCell(className, 0, Color.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTableCell(string className, int width, Color bkgndColor)
		{
			OpenTableCell(className, width, bkgndColor, null);
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTableCell(string className, int width, Color bkgndColor, string tooltip)
		{
			OpenTableCell(className, width, bkgndColor, Color.Empty, tooltip);
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTableCell(string className, int width, Color bkgndColor,
			Color textColor, string tooltip)
		{
			className = (className ?? string.Empty);
			tooltip = (tooltip ?? string.Empty);

			_htmlText.Append("<td");

			if (className.Trim() != string.Empty)
				_htmlText.AppendFormat(" class=\"{0}\"", className);

			if (width > 0 || bkgndColor != Color.Empty || textColor != Color.Empty)
			{
				var styleInfo = string.Empty;

				if (width > 0)
					styleInfo += string.Format("width: {0}px; ", width);

				if (bkgndColor != Color.Empty)
				{
					var color = (bkgndColor.ToArgb() & 0xFFFFFF).ToString("x6");
					styleInfo += string.Format("background-color: #{0}; ", color);
				}

				if (textColor != Color.Empty)
				{
					var color = (textColor.ToArgb() & 0xFFFFFF).ToString("x6");
					styleInfo += string.Format("color: #{0}; ", color);
				}

				_htmlText.AppendFormat(" style=\"{0}\"", styleInfo.Trim());
			}

			if (tooltip.Trim() != string.Empty)
				_htmlText.AppendFormat(" title=\"{0}\"", tooltip);

			_htmlText.AppendFormat(">");
		}

		/// ------------------------------------------------------------------------------------
		public void CloseTableCell()
		{
			_htmlText.Append("</td>");
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class ColorBarSegmentInfo : IComparable<ColorBarSegmentInfo>
	{
		public string FieldName { get; set; }
		public string FieldValue { get; set; }
		public IEnumerable<Event> Events { get; set; }
		public Color BackColor { get; set; }
		public Color TextColor { get; set; }
		public override string ToString() { return FieldValue; }

		/// ------------------------------------------------------------------------------------
		public int CompareTo(ColorBarSegmentInfo other)
		{
			if (FieldValue == null)
				return -1;

			if (other == null || other.FieldValue == null)
				return 1;

			if (FieldName != "status")
				return FieldValue.CompareTo(other.FieldValue);

			var sx = FieldValue.Replace(' ', '_');
			var sy = other.FieldValue.Replace(' ', '_');

			if (!Enum.GetNames(typeof(Event.Status)).Contains(sx))
				return 1;

			if (!Enum.GetNames(typeof(Event.Status)).Contains(sy))
				return -1;

			return (int)Enum.Parse(typeof(Event.Status), sx) -
				(int)Enum.Parse(typeof(Event.Status), sy);
		}
	}
}
