using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.UI.ProjectWindow;
using SayMore.Utilities;

namespace SayMoreTests.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// ViewTabGroup (the custom Project/Sessions/People tab strip) manually sizes each tab to
	/// fit its text (AutoSize is deliberately off), using a separate TabFont field rather than
	/// relying on the generic recursive UI scaling to resize things for it. Confirms it
	/// re-measures tab width when the scale setting changes.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class ViewTabGroupTests
	{
		/// ------------------------------------------------------------------------------------
		[OneTimeSetUp]
		public void FixtureSetup()
		{
			LocalizationManager.StrictInitializationMode = false;
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
			UITextScaler.NotifyScaleChanged();
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleChanged_ScaleIncreased_WidensTabToFitLargerText()
		{
			Settings.Default.UITextScale = 100;

			using (var group = new ViewTabGroup())
			{
				var view = new Panel { Text = "Sessions" };
				var tab = group.AddTab(view);
				var widthAt100Percent = tab.Width;

				Settings.Default.UITextScale = 300;
				UITextScaler.NotifyScaleChanged();

				Assert.Greater(tab.Width, widthAt100Percent);
			}
		}
	}
}
