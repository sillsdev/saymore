using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.UI;
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
		private class DiagnosticForm : SayMoreFormBase { }

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A ViewTab is still a Label, so the generic recursive UI scaling pass (triggered once
		/// at the owning Form's Load) reaches it too -- but it's already been correctly scaled
		/// by ViewTabGroup's own mechanism at construction time. Without an exclusion, the
		/// generic pass treats that already-scaled font as a fresh 100% baseline and scales it
		/// again on top, compounding the result.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ViewTabAlreadyScaledByViewTabGroup_DoesNotCompound()
		{
			Settings.Default.UITextScale = 130;

			using (var form = new DiagnosticForm())
			using (var tabGroup = new ViewTabGroup())
			using (var view = new UserControl { Text = "Project" })
			{
				form.Controls.Add(tabGroup);
				var tab = tabGroup.AddTab(view);

				// ViewTabGroup's own constructor already set this correctly from Settings.
				var sizeBeforeGenericScaling = tab.Font.Size;

				form.Show();
				try
				{
					Assert.AreEqual(sizeBeforeGenericScaling, tab.Font.Size, 0.01f,
						"The generic scaling pass must not compound on top of ViewTabGroup's own already-correct tab font.");
				}
				finally
				{
					form.Close();
				}
			}
		}
	}
}
