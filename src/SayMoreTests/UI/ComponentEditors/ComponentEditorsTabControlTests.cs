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
		public void ScaleUIFont_HostContainsTabControl_UpdatesFontLive()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel())
			using (var tabControl = new ComponentEditorsTabControl("key", null,
				       Array.Empty<IEditorProvider>(), Color.White, Color.Black))
			{
				host.Controls.Add(tabControl);
				var sizeAt100Percent = tabControl.Font.Size;

				Settings.Default.UITextScale = 200;
				// ComponentEditorsTabControl.ApplyScale reads Program.ScaledDialogFont, whose
				// cache is only invalidated by the ScaleChanged event -- NotifyScaleChanged is
				// what a real settings change always fires alongside a live form's own
				// ScaleUIFont() call (see SayMoreFormBase.HandleScaleChanged).
				UITextScaler.NotifyScaleChanged();
				host.ScaleUIFont();

				Assert.Greater(tabControl.Font.Size, sizeAt100Percent);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// EditorProvider.Control is reached entirely via ComponentEditorsTabControl's own
		/// ApplyScale -- TabControl isn't otherwise matched by the generic recursive switch, so
		/// this is the only path that scales a hosted editor's Control.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_HostContainsTabControlWithVisibleEditor_ScalesEditorControlFont()
		{
			Settings.Default.UITextScale = 100;

			var editorControl = new Label { Text = "Hello", Font = new Font(FontFamily.GenericSansSerif, 10f) };
			var provider = new Mock<IEditorProvider>();
			provider.SetupGet(p => p.Control).Returns(editorControl);
			provider.SetupGet(p => p.TabText).Returns("Test Editor");
			provider.SetupGet(p => p.ImageKey).Returns((string)null);
			provider.SetupGet(p => p.IsOKToShow).Returns(true);

			using (var host = new Panel())
			using (var tabControl = new ComponentEditorsTabControl("key", null,
				       new[] { provider.Object }, Color.White, Color.Black))
			{
				host.Controls.Add(tabControl);
				var originalSize = editorControl.Font.Size;

				Settings.Default.UITextScale = 200;
				host.ScaleUIFont();

				Assert.Greater(editorControl.Font.Size, originalSize);
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
