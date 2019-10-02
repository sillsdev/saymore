using NUnit.Framework;
using SayMore.UI.LowLevelControls;
using SIL.Reporting;

namespace SayMoreTests.UI.LowLevelControls
{
	[TestFixture]
	public class SliderTests
	{
		[Test]
		public void SetValueWithoutEvent_ValueInRange_NoNonFatalErrorReported()
		{
			ErrorReport.IsOkToInteractWithUser = false;

			using (var rpt = new ErrorReport.NoNonFatalErrorReportExpected())
			{
				using (var slider = new Slider())
				{
					var result = slider.SetValueWithoutEvent(50);
					Assert.IsTrue(result);
					Assert.IsNull(rpt.Message);
				}
			}
		}

		[Test]
		public void SetValueWithoutEvent_ValueNegative_NonFatalErrorReported()
		{
			ErrorReport.IsOkToInteractWithUser = false;

			using (var rpt = new ErrorReport.NonFatalErrorReportExpected())
			{
				using (var slider = new Slider())
				{
					var result = slider.SetValueWithoutEvent(-2);
					Assert.IsFalse(result);
					Assert.AreEqual("Attempted to set slider to a negative value: -2.", rpt.Message);
				}
			}
		}

		[Test]
		public void SetValueWithoutEvent_ValueGreaterThanMaximum_NonFatalErrorReported()
		{
			ErrorReport.IsOkToInteractWithUser = false;

			using (var rpt = new ErrorReport.NonFatalErrorReportExpected())
			{
				using (var slider = new Slider())
				{
					var result = slider.SetValueWithoutEvent(1000);
					Assert.IsFalse(result);
					Assert.AreEqual("Attempted to set slider to a value (1000) which is greater than the maximum (100).", rpt.Message);
				}
			}
		}
	}
}
