using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.UI;
using SayMore.Utilities;

namespace SayMoreTests.UI
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class SayMoreFormBaseTests
	{
		private class TestForm : SayMoreFormBase
		{
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Load_ScaleNotAt100Percent_ScalesChildControlFont()
		{
			Settings.Default.UITextScale = 200;

			using (var form = new TestForm())
			{
				var label = new Label { Text = "Hello", Font = new Font(FontFamily.GenericSansSerif, 10f) };
				form.Controls.Add(label);
				var origSize = label.Font.Size;

				form.Show();

				Assert.Greater(label.Font.Size, origSize);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleChanged_FormAlreadyLoaded_RescalesChildControlFontLive()
		{
			Settings.Default.UITextScale = 100;

			using (var form = new TestForm())
			{
				var label = new Label { Text = "Hello", Font = new Font(FontFamily.GenericSansSerif, 10f) };
				form.Controls.Add(label);
				form.Show();
				var sizeAt100Percent = label.Font.Size;

				Settings.Default.UITextScale = 300;
				UITextScaler.NotifyScaleChanged();

				Assert.Greater(label.Font.Size, sizeAt100Percent);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Dispose_FormDisposed_NoLongerRespondsToScaleChanged()
		{
			Settings.Default.UITextScale = 100;

			var form = new TestForm();
			var label = new Label { Text = "Hello", Font = new Font(FontFamily.GenericSansSerif, 10f) };
			form.Controls.Add(label);
			form.Show();

			form.Dispose();

			Assert.DoesNotThrow(() =>
			{
				Settings.Default.UITextScale = 300;
				UITextScaler.NotifyScaleChanged();
			});
		}
	}
}
