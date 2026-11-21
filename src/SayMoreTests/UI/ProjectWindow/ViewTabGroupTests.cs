using System.Drawing;
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
		public void ScaleUIFont_HostContainsViewTabGroup_WidensTabToFitLargerText()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel())
			using (var group = new ViewTabGroup())
			{
				host.Controls.Add(group);
				var view = new Panel { Text = "Sessions" };
				var tab = group.AddTab(view);
				var widthAt100Percent = tab.Width;

				Settings.Default.UITextScale = 300;
				host.ScaleUIFont();

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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// AddTab adds the hosted view itself as a direct child of ViewTabGroup (see ViewTab's
		/// constructor), not just the tab header -- this is where the actual Sessions/People/
		/// Project screen content lives. ViewTabGroup's own ApplyScale must therefore also scale
		/// each hosted view, not just the tab strip, since being dispatched via IUiScalable means
		/// the generic recursive pass that used to reach ViewTabGroup's own Controls (including
		/// each view) no longer does.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_HostContainsViewTabGroup_ScalesHostedViewContent()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel())
			using (var group = new ViewTabGroup())
			using (var view = new Panel())
			using (var label = new Label { Text = "Hello", Font = new Font(FontFamily.GenericSansSerif, 10f) })
			{
				view.Controls.Add(label);
				host.Controls.Add(group);
				group.AddTab(view);
				var originalSize = label.Font.Size;

				Settings.Default.UITextScale = 200;
				host.ScaleUIFont();

				Assert.Greater(label.Font.Size, originalSize,
					"ViewTabGroup.ApplyScale must also scale the content of each hosted view, " +
					"not just the tab strip -- this content used to be reached by the generic " +
					"recursive pass before ViewTabGroup was dispatched via IUiScalable.");
			}
		}
	}
}
