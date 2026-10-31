using NUnit.Framework;
using SayMore;
using SayMore.Properties;
using SayMore.Utilities;

namespace SayMoreTests
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class ProgramScaledDialogFontTests
	{
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
			UITextScaler.NotifyScaleChanged();
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ScaledDialogFont_ScaleChangesAfterFirstAccess_ReflectsNewScale()
		{
			Settings.Default.UITextScale = 100;
			var baselineSize = Program.ScaledDialogFont.Size;

			Settings.Default.UITextScale = 300;
			UITextScaler.NotifyScaleChanged();

			Assert.Greater(Program.ScaledDialogFont.Size, baselineSize);
		}
	}
}
