using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using L10NSharp;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.UI.Overview.Statistics;
using SIL.Reporting;

namespace SayMore.UI.Charts
{
	/// ----------------------------------------------------------------------------------------
	public class HTMLChartBuilder
	{
		public const string kNonBreakingSpace = "&nbsp;";

		private readonly StatisticsViewModel _statsViewModel;
		protected readonly StringBuilder _htmlText = new StringBuilder(6000);

		/// ------------------------------------------------------------------------------------
		public HTMLChartBuilder(StatisticsViewModel statsViewModel)
		{
			_statsViewModel = statsViewModel;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is used only for tests.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string HtmlContent
		{
			get
			{
				lock (_htmlText)
					return _htmlText.ToString();
			}
		}

		/// ------------------------------------------------------------------------------------
		public string GetStatisticsCharts()
		{
			lock (_htmlText)
			{
				string fallbackHtml = _htmlText.ToString();
				try
				{
					_htmlText.Length = 0;

					OpenHtml();

					var text = LocalizationManager.GetString("ProgressView.HeadingText", "SayMore statistics for {0}",
						"Parameter is project name.");
					WriteHtmlHead(string.Format(text, _statsViewModel.ProjectName));

					OpenBody();
					WriteOverviewSection();
					WriteStageChart();

					var backColors = GetStatusSegmentColors();
					var textColors = backColors.ToDictionary(kvp => kvp.Key, kvp => Color.Empty);
					text = LocalizationManager.GetString("ProgressView.ByGenreHeadingText", "By Genre");
					WriteChartByFieldPair(text, SessionFileType.kGenreFieldName, SessionFileType.kStatusFieldName, backColors,
						textColors);

					CloseBody();
					CloseHtml();

					return _htmlText.ToString();
				}
				catch (ChartBarSegmentInfo.CollectionModifiedException e)
				{
					// SP-1020: It's possible that the Sessions collection has changed on another (probably UI) thread. Because
					// of the number of layers involved and the overall poor design, it's virtually impossible to ensure that
					// the sessions collection is properly locked to prevent this. But since a change in the number of sessions
					// should cause a new update to the HTML anyway, we'll just return our previous value and let the next one
					// try again.
					Logger.WriteEvent("Exception in HTMLChartBuilder.GetStatisticsCharts:\r\n{0}", e.ToString());
					DesktopAnalytics.Analytics.ReportException(e);
					return fallbackHtml;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void WriteStageChart()
		{
			var sessionsByStage = _statsViewModel.SessionInformant.GetSessionsCategorizedByStage()
				.Where(r => r.Key.Id != ComponentRole.kConsentComponentRoleId);

			var barInfoList = (sessionsByStage.Select(
				x => new ChartBarInfo(x.Key.Name, x.Value, x.Key.Color, x.Key.TextColor))).ToList();

			ChartBarInfo.CalculateBarSizes(barInfoList);
			var text = LocalizationManager.GetString("ProgressView.ByStagesHeadingText", "Completed Stages");
			WriteChartForList(text, barInfoList, null, false);
		}

		/// ------------------------------------------------------------------------------------
		private IDictionary<string, Color> GetStatusSegmentColors()
		{
			var statusColors = new Dictionary<string, Color>();

			foreach (var statusName in Enum.GetNames(typeof(Session.Status)).Where(x => x != Session.Status.Skipped.ToString()))
			{
				statusColors[Session.GetLocalizedStatus(statusName)] =
					(Color)Properties.Settings.Default[statusName + "StatusColor"];
			}

			return statusColors;
		}

		/// ------------------------------------------------------------------------------------
		private void WriteOverviewSection()
		{
			var text = LocalizationManager.GetString("ProgressView.OverviewHeadingText", "Overview");
			WriteHeading(text);
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
		private void WriteChartByFieldPair(string chartHeading, string primaryField,
			string secondaryField, IDictionary<string, Color> colors, IDictionary<string, Color> textColors)
		{
			var outerList =
				_statsViewModel.SessionInformant.GetCategorizedSessionsFromDoubleKey(primaryField, secondaryField);

			var barInfoList = (from x in outerList
							  select new ChartBarInfo(x.Key, secondaryField, x.Value, colors, textColors))
							  .OrderBy(bi => bi.FieldValue).ToList();

			ChartBarInfo.CalculateBarSizes(barInfoList);
			WriteChartForList(chartHeading, barInfoList, colors, true);
		}

		/// ------------------------------------------------------------------------------------
		private void WriteChartForList(string chartHeading,
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
		private void WriteChartEntry(ChartBarInfo barInfo)
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
		private string WriteBar(ChartBarInfo barInfo)
		{
			foreach (var seg in barInfo.Segments)
			{
				if (seg.TotalTimeSpan.Ticks > 0)
					WriteBarSegment(seg);
			}

			var text = LocalizationManager.GetString("ProgressView.SummaryTotalsTextForOneBar", "{0} sessions totaling {1}");
			return string.Format(text, barInfo.TotalSessions, barInfo.TotalTimeSpan.ToString());
		}

		/// ------------------------------------------------------------------------------------
		private void WriteBarSegment(ChartBarSegmentInfo barSegInfo)
		{
			// Only segments total time if it's size is 5% or more of the total width of the table.
			var segmentText = (barSegInfo.SegmentSize >= 5 ?
				Math.Ceiling(barSegInfo.TotalTimeSpan.TotalMinutes).ToString(CultureInfo.InvariantCulture) : kNonBreakingSpace);

			var fmt = (string.IsNullOrEmpty(barSegInfo.FieldValue) ?
				LocalizationManager.GetString("ProgressView.SummaryTotalsTextForSegment1", "{0}{1} sessions totaling {2}") :
				LocalizationManager.GetString("ProgressView.SummaryTotalsTextForSegment2", "{0}: {1} sessions totaling {2}"));

			var tooltipText = string.Format(fmt, barSegInfo.FieldValue,
				barSegInfo.GetSessionCount(), barSegInfo.TotalTimeSpan.ToString());

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
					WriteTableCell("legendtext", kvp.Key);
				}

				CloseTableRow();
				CloseTable();
			}

			CloseTableCell();
			CloseTableRow();
		}

		#region Methods for writing HTML tags
		/// ------------------------------------------------------------------------------------
		public static string XMLDocTypeInfo
		{
			get
			{
				return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" " +
					"\"https://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">" +
					"<html xml:lang=\"en\" lang=\"en\" xmlns=\"https://www.w3.org/1999/xhtml\">";
			}
		}

		/// ------------------------------------------------------------------------------------
		private void OpenHtml()
		{
			_htmlText.Append(XMLDocTypeInfo);
		}

		/// ------------------------------------------------------------------------------------
		private void CloseHtml()
		{
			_htmlText.Append("</html>");
		}

		/// ------------------------------------------------------------------------------------
		private void WriteHtmlHead(string htmlDocTitle)
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
			lock (_htmlText)
			{
				_htmlText.Append("<body>");
			}
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
			OpenTable(null, 0);
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenTable(string className)
		{
			OpenTable(className, 0);
		}

		/// ------------------------------------------------------------------------------------
		private void OpenTable(string className, int width)
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
		protected void OpenTableRow(string className)
		{
			_htmlText.Append("<tr");

			className = (className ?? string.Empty);
			if (className.Trim() != string.Empty)
				_htmlText.AppendFormat(" class=\"{0}\"", className);

			_htmlText.Append(">");
		}

		/// ------------------------------------------------------------------------------------
		protected void CloseTableRow()
		{
			_htmlText.Append("</tr>");
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteTableRowHead(string content)
		{
			content = (content ?? string.Empty);
			_htmlText.AppendFormat("<th scope=\"row\">{0}</th>", content.Trim());
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
			WriteTableCell(className, width, bkgndColor, Color.Empty, tooltip, content);
		}

		/// ------------------------------------------------------------------------------------
		protected void WriteTableCell(string className, int width, Color bkgndColor,
			Color textColor, string tooltip, string content)
		{
			content = (content ?? string.Empty);
			OpenTableCell(className, width, bkgndColor, textColor, tooltip);
			_htmlText.Append(content.Trim());
			CloseTableCell();
		}

		/// ------------------------------------------------------------------------------------
		protected void OpenTableCell(string className)
		{
			OpenTableCell(className, 0, Color.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private void OpenTableCell(string className, int width, Color bkgndColor)
		{
			OpenTableCell(className, width, bkgndColor, null);
		}

		/// ------------------------------------------------------------------------------------
		private void OpenTableCell(string className, int width, Color bkgndColor, string tooltip)
		{
			OpenTableCell(className, width, bkgndColor, Color.Empty, tooltip);
		}

		/// ------------------------------------------------------------------------------------
		private void OpenTableCell(string className, int width, Color bkgndColor,
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
		protected void CloseTableCell()
		{
			_htmlText.Append("</td>");
		}
		#endregion
	}
}
