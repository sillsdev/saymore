using System.Drawing;
using NUnit.Framework;
using SayMore.UI.Charts;
using SayMore.UI.Overview.Statistics;

namespace SayMoreTests.UI.Charts
{
	[TestFixture]
	public class HTMLChartBuilderTests
	{
		private HTMLChartBuilderTestWrapper _builder;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_builder = new HTMLChartBuilderTestWrapper(null);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenBody_WritesCorrectTag()
		{
			_builder.CallOpenBody();
			Assert.AreEqual("<body>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseBody_WritesCorrectTag()
		{
			_builder.CallCloseBody();
			Assert.AreEqual("</body>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteHeading_PassNull_WritesNothingBetweeOpenAndClosedTags()
		{
			_builder.CallWriteHeading(null);
			Assert.AreEqual("<h2></h2>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteHeading_PassSomething_WritesItBetweenOpenAndClosedTags()
		{
			_builder.CallWriteHeading("something");
			Assert.AreEqual("<h2>something</h2>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTable_NoArguments_WritesCorrectTag()
		{
			_builder.CallOpenTable();
			Assert.AreEqual("<table cellspacing=\"0\" cellpadding=\"0\">", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTable_PassNull_DoesNotWriteClassAttribute()
		{
			_builder.CallOpenTable(null);
			Assert.AreEqual("<table cellspacing=\"0\" cellpadding=\"0\">", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTable_PassSpaces_DoesNotWriteClassAttribute()
		{
			_builder.CallOpenTable(" ");
			Assert.AreEqual("<table cellspacing=\"0\" cellpadding=\"0\">", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTable_PassClassName_WritesClassAttribute()
		{
			_builder.CallOpenTable("classname");
			Assert.AreEqual("<table cellspacing=\"0\" cellpadding=\"0\" class=\"classname\">", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseTable_WritesCorrectTag()
		{
			_builder.CallCloseTable();
			Assert.AreEqual("</table>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableHead_WritesCorrectTag()
		{
			_builder.CallOpenTableHead();
			Assert.AreEqual("<thead>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseTableHead_WritesCorrectTag()
		{
			_builder.CallCloseTableHead();
			Assert.AreEqual("</thead>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableRow_WritesCorrectTag()
		{
			_builder.CallOpenTableRow();
			Assert.AreEqual("<tr>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableRow_ClassIsNull_DoesNotWriteClass()
		{
			_builder.CallOpenTableRow(null);
			Assert.AreEqual("<tr>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableRow_ClassIsSpace_DoesNotWriteClass()
		{
			_builder.CallOpenTableRow(" ");
			Assert.AreEqual("<tr>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableRow_ClassIsSomething_WritesIt()
		{
			_builder.CallOpenTableRow("something");
			Assert.AreEqual("<tr class=\"something\">", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseTableRow_WritesCorrectTag()
		{
			_builder.CallCloseTableRow();
			Assert.AreEqual("</tr>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableRowHead_PassNull_WritesNothingBetweeOpenAndClosedTags()
		{
			_builder.CallWriteTableRowHead(null);
			Assert.AreEqual("<th scope=\"row\"></th>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableRowHead_PassSpace_WritesNothingBetweeOpenAndClosedTags()
		{
			_builder.CallWriteTableRowHead(" ");
			Assert.AreEqual("<th scope=\"row\"></th>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableRowHead_PassSomething_WritesItBetweeOpenAndClosedTags()
		{
			_builder.CallWriteTableRowHead("something");
			Assert.AreEqual("<th scope=\"row\">something</th>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableBody_WritesCorrectTag()
		{
			_builder.CallOpenTableBody();
			Assert.AreEqual("<tbody>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseTableBody_WritesCorrectTag()
		{
			_builder.CallCloseTableBody();
			Assert.AreEqual("</tbody>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ContentIsNull_WritesNothingBetweeOpenAndClosedTags()
		{
			_builder.CallWriteTableCell(null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ContentIsSpace_WritesNothingBetweeOpenAndClosedTags()
		{
			_builder.CallWriteTableCell(" ");
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ContentIsSomething_WritesItBetweeOpenAndClosedTags()
		{
			_builder.CallWriteTableCell("something");
			Assert.AreEqual("<td>something</td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ClassIsNull_DoesNotWriteClass()
		{
			_builder.CallWriteTableCell(null, null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ClassIsSpace_DoesNotWriteClass()
		{
			_builder.CallWriteTableCell(" ", null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ClassIsSomething_WritesIt()
		{
			_builder.CallWriteTableCell("something", null);
			Assert.AreEqual("<td class=\"something\"></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_WidthIsZero_DoesNotWriteWidth()
		{
			_builder.CallWriteTableCell(null, 0, Color.Empty, null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_WidthIsGreaterThanZero_WritesWidth()
		{
			_builder.CallWriteTableCell(null, 45, Color.Empty, null);
			Assert.AreEqual("<td style=\"width: 45%;\"></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_BkgColorIsEmpty_DoesNotWriteBkgColor()
		{
			_builder.CallWriteTableCell(null, 0, Color.Empty, null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_BkgColorIsBlack_WritesBkgColor()
		{
			_builder.CallWriteTableCell(null, 0, Color.Black, null);
			Assert.AreEqual("<td style=\"background-color: #000000;\"></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_TextColorIsEmpty_DoesNotWriteTextColor()
		{
			_builder.CallWriteTableCell(null, 0, Color.Empty, Color.Empty, null, null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_TextColorIsBlack_WritesTextColor()
		{
			_builder.CallWriteTableCell(null, 0, Color.Empty, Color.Black, null, null);
			Assert.AreEqual("<td style=\"color: #000000;\"></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_TooltipIsNull_DoesNotWriteTooltip()
		{
			_builder.CallWriteTableCell(null, 0, Color.Empty, null, null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_TooltipIsSpace_DoesNotWriteTooltip()
		{
			_builder.CallWriteTableCell(null, 0, Color.Empty, " ", null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_TooltipIsSomething_WritesIt()
		{
			_builder.CallWriteTableCell(null, 0, Color.Empty, "something", null);
			Assert.AreEqual("<td title=\"something\"></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_PassSomethingOfEverything_WritesThemAll()
		{
			_builder.CallWriteTableCell("classname", 47, Color.Black, Color.Black, "tooltip", "content");
			Assert.AreEqual("<td class=\"classname\" style=\"width: 47%; " +
				"background-color: #000000; color: #000000;\" title=\"tooltip\">content</td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableCell_WritesCorrectTag()
		{
			_builder.CallOpenTableCell();
			Assert.AreEqual("<td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableCell_PassClass_WritesClass()
		{
			_builder.CallOpenTableCell("classname");
			Assert.AreEqual("<td class=\"classname\">", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseTableCell_WritesCorrectTag()
		{
			_builder.CallCloseTableCell();
			Assert.AreEqual("</td>", _builder.HtmlContent);
		}
	}

	public class HTMLChartBuilderTestWrapper : HTMLChartBuilder
	{
		public HTMLChartBuilderTestWrapper(StatisticsViewModel statsViewModel) : base(statsViewModel)
		{
		}

		public void CallOpenBody()
		{
			lock (_htmlText)
				base.OpenBody();
		}

		public void CallCloseBody()
		{
			lock (_htmlText)
				base.CloseBody();
		}

		public void CallWriteHeading(string heading)
		{
			lock (_htmlText)
				base.WriteHeading(heading);
		}

		public void CallOpenTable()
		{
			lock (_htmlText)
				base.OpenTable();
		}

		public void CallOpenTable(string className)
		{
			lock (_htmlText)
				base.OpenTable(className);
		}

		public void CallCloseTableCell()
		{
			lock (_htmlText)
				base.CloseTableCell();
		}

		public void CallCloseTable()
		{
			lock (_htmlText)
				base.CloseTable();
		}

		public void CallOpenTableHead()
		{
			lock (_htmlText)
				base.OpenTableHead();
		}

		public void CallCloseTableHead()
		{
			lock (_htmlText)
				base.CloseTableHead();
		}

		public void CallOpenTableRow()
		{
			lock (_htmlText)
				base.OpenTableRow();
		}

		public void CallOpenTableRow(string className)
		{
			lock (_htmlText)
				base.OpenTableRow(className);
		}

		public void CallCloseTableRow()
		{
			lock (_htmlText)
				base.CloseTableRow();
		}

		public void CallWriteTableRowHead(string content)
		{
			lock (_htmlText)
				base.WriteTableRowHead(content);
		}

		public void CallOpenTableBody()
		{
			lock (_htmlText)
				base.OpenTableBody();
		}

		public void CallCloseTableBody()
		{
			lock (_htmlText)
				base.CloseTableBody();
		}

		public void CallWriteTableCell(string content)
		{
			lock (_htmlText)
				base.WriteTableCell(content);
		}

		public void CallWriteTableCell(string className, string content)
		{
			lock (_htmlText)
				base.WriteTableCell(className, content);
		}

		public void CallWriteTableCell(string className, int width, Color bkgndColor, string content)
		{
			lock (_htmlText)
				base.WriteTableCell(className, width, bkgndColor, content);
		}

		public void CallWriteTableCell(string className, int width, Color bkgndColor,
			string tooltip, string content)
		{
			lock (_htmlText)
				base.WriteTableCell(className, width, bkgndColor, tooltip, content);
		}

		public void CallWriteTableCell(string className, int width, Color bkgndColor,
			Color textColor, string tooltip, string content)
		{
			lock (_htmlText)
				base.WriteTableCell(className, width, bkgndColor, textColor, tooltip, content);
		}

		public void CallOpenTableCell()
		{
			lock (_htmlText)
				base.OpenTableCell(null);
		}

		public void CallOpenTableCell(string className)
		{
			lock (_htmlText)
				base.OpenTableCell(className);
		}
	}
}
