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
		public const string kNonBreakingSpace = "&nbsp;";
		protected const int kPixelsPerMinute = 20;

		protected readonly StatisticsViewModel _statsViewModel;
		protected StringBuilder _htmlText = new StringBuilder(6000);

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
			var eventsByStage = _statsViewModel.EventInformant.GetEventsCategorizedByStage()
				.Where(r => r.Key.Id != "consent");

			var barInfoList = (eventsByStage.Select(
				x => new ChartBarInfo(x.Key.Name, x.Value, x.Key.Color, x.Key.TextColor))).ToList();

			ChartBarInfo.CalculateBarSizes(barInfoList);
			WriteChartForList("By Stages", barInfoList, null, false);
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

			var barInfoList = (from x in outerList
							  select new ChartBarInfo(x.Key, secondaryField, x.Value, colors, textColors))
							  .OrderBy(bi => bi.FieldValue).ToList();

			ChartBarInfo.CalculateBarSizes(barInfoList);
			WriteChartForList(chartHeading, barInfoList, colors, true);
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteChartForList(string chartHeading,
			IEnumerable<ChartBarInfo> barInfoList,
			IDictionary<string, Color> legendColors, bool writeLegend)
		{
			WriteHeading(chartHeading);
			OpenTable("chart");
			OpenTableBody();

			OpenTableRow("shimrow");
			WriteTableCell(kNonBreakingSpace);
			WriteTableCell(kNonBreakingSpace);
			CloseTableRow();

			foreach (var bi in barInfoList)
				WriteChartEntry(bi);

			WriteLegend(legendColors, writeLegend);

			CloseTableBody();
			CloseTable();
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteChartEntry(ChartBarInfo barInfo)
		{
			OpenTableRow();
			WriteTableCell("rowheading", barInfo.FieldValue);
			OpenTableCell("colorbar");
			OpenTable(null, barInfo.BarSize);
			OpenTableRow();

			var summaryText = WriteBar(barInfo);

			CloseTableRow();
			CloseTable();
			CloseTableCell();
			CloseTableRow();

			OpenTableRow();
			WriteTableCell("entrysummaryrowheading", string.Empty);
			WriteTableCell("entrysummary", summaryText);
			CloseTableRow();
		}

		/// ------------------------------------------------------------------------------------
		protected string WriteBar(ChartBarInfo barInfo)
		{
			foreach (var seg in barInfo.Segments)
			{
				if (seg.TotalTime > 0)
					WriteBarSegment(seg);
			}

			return string.Format("{0} events totaling {1} minutes",
				barInfo.TotalEvents, barInfo.TotalTime);
		}

		/// ------------------------------------------------------------------------------------
		public void WriteBarSegment(ChartBarSegmentInfo barSegInfo)
		{
			// Only segments total time if it's size is 5% or more of the total width of the table.
			var segmentText = (barSegInfo.SegmentSize >= 5 ?
				barSegInfo.TotalTime.ToString() : kNonBreakingSpace);

			var fmt = (string.IsNullOrEmpty(barSegInfo.FieldValue) ?
				"{0}{1} events totaling {2} minutes" : "{0}: {1} events totaling {2} minutes");

			var tooltipText = string.Format(fmt, barSegInfo.FieldValue,
				barSegInfo.Events.Count(), barSegInfo.TotalTime);

			WriteTableCell(null, barSegInfo.SegmentSize, barSegInfo.BackColor,
				barSegInfo.TextColor, tooltipText, segmentText);
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
					WriteTableCell("legendblock", 0, kvp.Value, null);
					WriteTableCell("legendtext", kvp.Key.Replace('_', ' '));
				}

				CloseTableRow();
				CloseTable();
			}

			CloseTableCell();
			CloseTableRow();
		}

		#region Methods for writing HTML tags
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
			OpenTable(null, 0);
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTable(string className)
		{
			OpenTable(className, 0);
		}

		/// ------------------------------------------------------------------------------------
		public void OpenTable(string className, int width)
		{
			_htmlText.Append("<table cellspacing=\"0\" cellpadding=\"0\"");

			className = (className ?? string.Empty);

			if (className.Trim() != string.Empty)
				_htmlText.AppendFormat(" class=\"{0}\"", className);

			if (width > 0)
				_htmlText.AppendFormat(" width={0}%", width);

			_htmlText.Append(">");
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
		public void OpenTableRow(string className)
		{
			_htmlText.Append("<tr");

			className = (className ?? string.Empty);
			if (className.Trim() != string.Empty)
				_htmlText.AppendFormat(" class=\"{0}\"", className);

			_htmlText.Append(">");
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
					styleInfo += string.Format("width: {0}%; ", width);

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

		#endregion
	}
}
