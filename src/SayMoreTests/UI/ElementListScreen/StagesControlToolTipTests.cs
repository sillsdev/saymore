using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using L10NSharp;
using NUnit.Framework;
using SayMore;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ElementListScreen;

namespace SayMoreTests.UI.ElementListScreen
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class StagesControlToolTipTests
	{
		private ComponentRole _role;
		private StagesDataProvider _dataProvider;

		/// ------------------------------------------------------------------------------------
		[OneTimeSetUp]
		public void FixtureSetup()
		{
			LocalizationManager.StrictInitializationMode = false;
		}

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void SetUp()
		{
			_role = new ComponentRole(typeof(object), ComponentRole.kSourceComponentRoleId,
				() => "Source", ComponentRole.MeasurementTypes.None, path => true,
				"$ElementId$_Source", Color.White, Color.Black);
			_dataProvider = new StagesDataProvider(new[] { _role });
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		private static Label GetRoleLabel(StagesControlToolTip tooltip)
		{
			var tableLayout = (TableLayoutPanel)tooltip.Controls.Find("_tableLayout", true)[0];
			return tableLayout.Controls.OfType<Label>().First(l => l.Text == "Source:");
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Show_ScaleNotAt100Percent_SizesTooltipFromAlreadyScaledContent()
		{
			Settings.Default.UITextScale = 300;

			using (var tooltip = new StagesControlToolTip(new[] { _role }, _dataProvider))
			{
				tooltip.Show(new Point(0, 0), new ComponentRole[0]);

				var roleLabel = GetRoleLabel(tooltip);
				Assert.Greater(roleLabel.Font.Size, Program.DialogFont.Size);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Show_ScaleNotAt100Percent_SizesTooltipWideEnoughForScaledLabel()
		{
			Settings.Default.UITextScale = 100;
			int unscaledWidth;
			using (var unscaledReference = new StagesControlToolTip(new[] { _role }, _dataProvider))
			{
				unscaledReference.Show(new Point(0, 0), new ComponentRole[0]);
				unscaledWidth = unscaledReference.Width;
			}

			Settings.Default.UITextScale = 300;
			using (var tooltip = new StagesControlToolTip(new[] { _role }, _dataProvider))
			{
				tooltip.Show(new Point(0, 0), new ComponentRole[0]);

				Assert.Greater(tooltip.Width, unscaledWidth);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Show_CalledAgainAfterScaleChangedWithToolTipAlreadyLoaded_RescalesNewlyCreatedLabels()
		{
			Settings.Default.UITextScale = 100;

			using (var tooltip = new StagesControlToolTip(new[] { _role }, _dataProvider))
			{
				// First Show() is what fires the one-time Form.Load event.
				tooltip.Show(new Point(0, 0), new ComponentRole[0]);
				tooltip.Hide();

				Settings.Default.UITextScale = 300;
				tooltip.Show(new Point(0, 0), new ComponentRole[0]);

				var roleLabel = GetRoleLabel(tooltip);
				Assert.Greater(roleLabel.Font.Size, Program.DialogFont.Size);
			}
		}
	}
}
