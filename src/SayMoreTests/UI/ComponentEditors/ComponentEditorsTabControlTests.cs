using System;
using System.Drawing;
using System.Threading;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;
using SayMore.Utilities;

namespace SayMoreTests.UI.ComponentEditors
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// ComponentEditorsTabControl (the native TabControl whose tabs are "Annotations",
	/// "Convert Audio", etc.) froze its Font at construction time, same as ViewTabGroup did,
	/// so its tab headers never picked up the UI scale.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class ComponentEditorsTabControlTests
	{
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleChanged_ScaleIncreased_UpdatesFontLive()
		{
			Settings.Default.UITextScale = 100;

			using (var tabControl = new ComponentEditorsTabControl("key", null,
				       Array.Empty<IEditorProvider>(), Color.White, Color.Black))
			{
				var sizeAt100Percent = tabControl.Font.Size;

				Settings.Default.UITextScale = 200;
				UITextScaler.NotifyScaleChanged();

				Assert.Greater(tabControl.Font.Size, sizeAt100Percent);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Dispose_TabControlDisposed_NoLongerRespondsToScaleChanged()
		{
			var tabControl = new ComponentEditorsTabControl("key", null,
				Array.Empty<IEditorProvider>(), Color.White, Color.Black);
			tabControl.Dispose();

			Assert.DoesNotThrow(() =>
			{
				Settings.Default.UITextScale = 200;
				UITextScaler.NotifyScaleChanged();
			});
		}
	}
}
