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
		public void SetValueWithoutEvent_ValueNegative_ValueSetToZero()
		{
			using (var slider = new Slider())
			{
				slider.SetValueWithoutEvent(-2);
				Assert.AreEqual(0, slider.Value);
			}
		}

		[Test]
		public void SetValueWithoutEvent_ValueGreaterThanMaximum_ValueSetToMaximum()
		{
			using (var slider = new Slider())
			{
				slider.SetValueWithoutEvent(1000);
				Assert.AreEqual(100, slider.Value);
			}
		}
	}
}
