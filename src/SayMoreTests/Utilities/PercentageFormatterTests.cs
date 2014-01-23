using System.Globalization;
using NUnit.Framework;
using SayMore.Utilities;

namespace SayMoreTests.Utilities
{
	[TestFixture]
	public class PercentageFormatterTests
	{
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Format_Integer_FormatsAccordingToSpecifiedPattern()
		{
			var pctFormatter = new PercentageFormatter("{0}%", new NumberFormatInfo());
			Assert.AreEqual("55%", pctFormatter.Format(55));

			pctFormatter = new PercentageFormatter("{0} %", new NumberFormatInfo());
			Assert.AreEqual("55 %", pctFormatter.Format(55));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Format_OneThousand_FormatsWithoutComma()
		{
			var pctFormatter = new PercentageFormatter("{0}%", new NumberFormatInfo());
			Assert.AreEqual("1000%", pctFormatter.Format(1000));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Format_Double_FormatsAccordingToSpecifiedPattern()
		{
			var pctFormatter = new PercentageFormatter("{0}%", new NumberFormatInfo());
			Assert.AreEqual("55%", pctFormatter.Format(.552));

			pctFormatter = new PercentageFormatter("{0} %", new NumberFormatInfo());
			Assert.AreEqual("55 %", pctFormatter.Format(.552));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Format_ValidNumberWithPercentSign_ParsesAndFormatsCorrectly()
		{
			var pctFormatter = new PercentageFormatter("{0}%", new NumberFormatInfo());
			double value;
			Assert.AreEqual("55%", pctFormatter.Format("55 %", out value));
			Assert.AreEqual(55, value);
			Assert.AreEqual("55%", pctFormatter.Format("55.2%", out value));
			Assert.AreEqual(55.2, value);
			Assert.AreEqual("51%", pctFormatter.Format("% 51", out value));
			Assert.AreEqual(51, value);

			pctFormatter = new PercentageFormatter("{0} %", new NumberFormatInfo());
			Assert.AreEqual("56 %", pctFormatter.Format("55.9 %", out value));
			Assert.AreEqual(55.9, value);
			Assert.AreEqual("55 %", pctFormatter.Format("55%", out value));
			Assert.AreEqual(55, value);
			Assert.AreEqual("54 %", pctFormatter.Format("%53.9", out value));
			Assert.AreEqual(53.9, value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Format_ValidNumberWithoutPercentSign_ParsesAndFormatsCorrectly()
		{
			var pctFormatter = new PercentageFormatter("{0}%", new NumberFormatInfo());
			double value;
			Assert.AreEqual("55%", pctFormatter.Format("55", out value));
			Assert.AreEqual(55, value);
			Assert.AreEqual("56%", pctFormatter.Format("55.6", out value));
			Assert.AreEqual(55.6, value);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Format_InvalidNumbers_ReturnsNull()
		{
			var pctFormatter = new PercentageFormatter("{0}%", new NumberFormatInfo());
			double value;
			Assert.IsNull(pctFormatter.Format("55.A%", out value));
			Assert.AreEqual(0, value);
			Assert.IsNull(pctFormatter.Format("ABC45", out value));
			Assert.AreEqual(0, value);
		}
	}
}
