using System.Drawing;
using NUnit.Framework;
using SayMore.UI.Charts;

namespace SayMoreTests.UI.Charts
{
	[TestFixture]
	public class HTMLChartBuilderTests
	{
		private HTMLChartBuilder _builder;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_builder = new HTMLChartBuilder(null);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenBody_WritesCorrectTag()
		{
			_builder.OpenBody();
			Assert.AreEqual("<body>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseBody_WritesCorrectTag()
		{
			_builder.CloseBody();
			Assert.AreEqual("</body>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteHeading_PassNull_WritesNothingBetweeOpenAndClosedTags()
		{
			_builder.WriteHeading(null);
			Assert.AreEqual("<h2></h2>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteHeading_PassSomething_WritesItBetweenOpenAndClosedTags()
		{
			_builder.WriteHeading("something");
			Assert.AreEqual("<h2>something</h2>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTable_NoArguments_WritesCorrectTag()
		{
			_builder.OpenTable();
			Assert.AreEqual("<table cellspacing=\"0\" cellpadding=\"0\">", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTable_PassNull_DoesNotWriteClassAttribute()
		{
			_builder.OpenTable(null);
			Assert.AreEqual("<table cellspacing=\"0\" cellpadding=\"0\">", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTable_PassSpaces_DoesNotWriteClassAttribute()
		{
			_builder.OpenTable(" ");
			Assert.AreEqual("<table cellspacing=\"0\" cellpadding=\"0\">", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTable_PassClassName_WritesClassAttribute()
		{
			_builder.OpenTable("classname");
			Assert.AreEqual("<table cellspacing=\"0\" cellpadding=\"0\" class=\"classname\">", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseTable_WritesCorrectTag()
		{
			_builder.CloseTable();
			Assert.AreEqual("</table>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableHead_WritesCorrectTag()
		{
			_builder.OpenTableHead();
			Assert.AreEqual("<thead>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseTableHead_WritesCorrectTag()
		{
			_builder.CloseTableHead();
			Assert.AreEqual("</thead>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableRow_WritesCorrectTag()
		{
			_builder.OpenTableRow();
			Assert.AreEqual("<tr>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableRow_ClassIsNull_DoesNotWriteClass()
		{
			_builder.OpenTableRow(null);
			Assert.AreEqual("<tr>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableRow_ClassIsSpace_DoesNotWriteClass()
		{
			_builder.OpenTableRow(" ");
			Assert.AreEqual("<tr>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableRow_ClassIsSomething_WritesIt()
		{
			_builder.OpenTableRow("something");
			Assert.AreEqual("<tr class=\"something\">", _builder.HtmlContent);
		}




		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseTableRow_WritesCorrectTag()
		{
			_builder.CloseTableRow();
			Assert.AreEqual("</tr>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableRowHead_PassNull_WritesNothingBetweeOpenAndClosedTags()
		{
			_builder.WriteTableRowHead(null);
			Assert.AreEqual("<th scope=\"row\"></th>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableRowHead_PassSpace_WritesNothingBetweeOpenAndClosedTags()
		{
			_builder.WriteTableRowHead(" ");
			Assert.AreEqual("<th scope=\"row\"></th>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableRowHead_PassSomething_WritesItBetweeOpenAndClosedTags()
		{
			_builder.WriteTableRowHead("something");
			Assert.AreEqual("<th scope=\"row\">something</th>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableBody_WritesCorrectTag()
		{
			_builder.OpenTableBody();
			Assert.AreEqual("<tbody>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseTableBody_WritesCorrectTag()
		{
			_builder.CloseTableBody();
			Assert.AreEqual("</tbody>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ContentIsNull_WritesNothingBetweeOpenAndClosedTags()
		{
			_builder.WriteTableCell(null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ContentIsSpace_WritesNothingBetweeOpenAndClosedTags()
		{
			_builder.WriteTableCell(" ");
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ContentIsSomething_WritesItBetweeOpenAndClosedTags()
		{
			_builder.WriteTableCell("something");
			Assert.AreEqual("<td>something</td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ClassIsNull_DoesNotWriteClass()
		{
			_builder.WriteTableCell(null, null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ClassIsSpace_DoesNotWriteClass()
		{
			_builder.WriteTableCell(" ", null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_ClassIsSomething_WritesIt()
		{
			_builder.WriteTableCell("something", null);
			Assert.AreEqual("<td class=\"something\"></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_WidthIsZero_DoesNotWriteWidth()
		{
			_builder.WriteTableCell(null, 0, Color.Empty, null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_WidthIsGreaterThanZero_WritesWidth()
		{
			_builder.WriteTableCell(null, 45, Color.Empty, null);
			Assert.AreEqual("<td style=\"width: 45%;\"></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_BkgColorIsEmpty_DoesNotWriteBkgColor()
		{
			_builder.WriteTableCell(null, 0, Color.Empty, null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_BkgColorIsBlack_WritesBkgColor()
		{
			_builder.WriteTableCell(null, 0, Color.Black, null);
			Assert.AreEqual("<td style=\"background-color: #000000;\"></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_TextColorIsEmpty_DoesNotWriteTextColor()
		{
			_builder.WriteTableCell(null, 0, Color.Empty, Color.Empty, null, null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_TextColorIsBlack_WritesTextColor()
		{
			_builder.WriteTableCell(null, 0, Color.Empty, Color.Black, null, null);
			Assert.AreEqual("<td style=\"color: #000000;\"></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_TooltipIsNull_DoesNotWriteTooltip()
		{
			_builder.WriteTableCell(null, 0, Color.Empty, null, null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_TooltipIsSpace_DoesNotWriteTooltip()
		{
			_builder.WriteTableCell(null, 0, Color.Empty, " ", null);
			Assert.AreEqual("<td></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_TooltipIsSomething_WritesIt()
		{
			_builder.WriteTableCell(null, 0, Color.Empty, "something", null);
			Assert.AreEqual("<td title=\"something\"></td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void WriteTableCell_PassSomethingOfEverything_WritesThemAll()
		{
			_builder.WriteTableCell("classname", 47, Color.Black, Color.Black, "tooltip", "content");
			Assert.AreEqual("<td class=\"classname\" style=\"width: 47%; " +
				"background-color: #000000; color: #000000;\" title=\"tooltip\">content</td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableCell_WritesCorrectTag()
		{
			_builder.OpenTableCell();
			Assert.AreEqual("<td>", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void OpenTableCell_PassClass_WritesClass()
		{
			_builder.OpenTableCell("classname");
			Assert.AreEqual("<td class=\"classname\">", _builder.HtmlContent);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CloseTableCell_WritesCorrectTag()
		{
			_builder.CloseTableCell();
			Assert.AreEqual("</td>", _builder.HtmlContent);
		}
	}
}
