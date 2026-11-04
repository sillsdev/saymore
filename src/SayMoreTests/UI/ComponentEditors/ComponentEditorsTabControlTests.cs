using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Moq;
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
		/// <summary>
		/// EditorProviders are long-lived, cached instances (owned by FileType's own editor
		/// cache) reused across element selections -- MakeAppropriateEditorsVisible rebuilds its
		/// TabPages whenever the set of IsOKToShow providers changes (e.g. selecting a session
		/// whose audio needs conversion, then one that's already standard format), but the
		/// editor Controls inside those TabPages must survive the rebuild, since the next
		/// selection may need to display the exact same Control instance again.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void MakeAppropriateEditorsVisible_AnotherEditorsIsOKToShowChanges_DoesNotDisposeStillVisibleEditorControl()
		{
			var stillVisibleControl = new Panel();
			var stillVisibleProvider = new Mock<IEditorProvider>();
			stillVisibleProvider.SetupGet(p => p.Control).Returns(stillVisibleControl);
			stillVisibleProvider.SetupGet(p => p.TabText).Returns("Annotations");
			stillVisibleProvider.SetupGet(p => p.ImageKey).Returns((string)null);
			stillVisibleProvider.SetupGet(p => p.IsOKToShow).Returns(true);

			var toggledIsOkToShow = true;
			var toggledProvider = new Mock<IEditorProvider>();
			toggledProvider.SetupGet(p => p.Control).Returns(new Panel());
			toggledProvider.SetupGet(p => p.TabText).Returns("Convert Audio");
			toggledProvider.SetupGet(p => p.ImageKey).Returns((string)null);
			toggledProvider.SetupGet(p => p.IsOKToShow).Returns(() => toggledIsOkToShow);

			using (var tabControl = new ComponentEditorsTabControl("key", null,
				       new[] { stillVisibleProvider.Object, toggledProvider.Object }, Color.White, Color.Black))
			{
				// Constructor already called MakeAppropriateEditorsVisible once with both visible.

				toggledIsOkToShow = false;
				tabControl.MakeAppropriateEditorsVisible();

				Assert.IsFalse(stillVisibleControl.IsDisposed,
					"A still-visible editor's Control must survive a TabPage rebuild triggered by " +
					"another editor's IsOKToShow flipping.");

				toggledIsOkToShow = true;
				Assert.DoesNotThrow(() => tabControl.MakeAppropriateEditorsVisible());

				Assert.IsFalse(stillVisibleControl.IsDisposed,
					"The still-visible editor's Control must also survive a second rebuild.");
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
