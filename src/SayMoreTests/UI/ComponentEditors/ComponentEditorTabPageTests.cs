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
	[TestFixture]
	[NonParallelizable]
	public class ComponentEditorTabPageTests
	{
		private Mock<IEditorProvider> _provider;
		private Panel _editorControl;
		private Label _label;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void SetUp()
		{
			_label = new Label { Text = "Hello", Font = new Font(FontFamily.GenericSansSerif, 10f) };
			_editorControl = new Panel();
			_editorControl.Controls.Add(_label);

			_provider = new Mock<IEditorProvider>();
			_provider.SetupGet(p => p.Control).Returns(_editorControl);
			_provider.SetupGet(p => p.TabText).Returns("Test Editor");
			_provider.SetupGet(p => p.ImageKey).Returns((string)null);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void SetProvider_ScaleNotAt100Percent_ScalesEditorControlFont()
		{
			Settings.Default.UITextScale = 200;
			var origSize = _label.Font.Size;

			using (new ComponentEditorTabPage(_provider.Object, Color.Black))
			{
				Assert.Greater(_label.Font.Size, origSize);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_HostContainsAlreadyCreatedTabPage_RescalesEditorControlFontLive()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new TabControl())
			using (var tabPage = new ComponentEditorTabPage(_provider.Object, Color.Black))
			{
				host.Controls.Add(tabPage);
				var sizeAt100Percent = _label.Font.Size;

				Settings.Default.UITextScale = 300;
				host.ScaleUIFont();

				Assert.Greater(_label.Font.Size, sizeAt100Percent);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Dispose_TabPageDisposed_NoLongerRespondsToScaleChanged()
		{
			Settings.Default.UITextScale = 100;

			var tabPage = new ComponentEditorTabPage(_provider.Object, Color.Black);
			tabPage.Dispose();

			Assert.DoesNotThrow(() =>
			{
				Settings.Default.UITextScale = 300;
				UITextScaler.NotifyScaleChanged();
			});
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// EditorProvider.Control is a long-lived instance owned by FileType's own editor cache,
		/// reused across element selections -- not owned by this TabPage. Disposing the TabPage
		/// must not cascade Dispose() onto it, or the next selection that needs to show it again
		/// would throw ObjectDisposedException when re-adding it to a new TabPage.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Dispose_PageDisposed_DoesNotDisposeEditorControl()
		{
			var tabPage = new ComponentEditorTabPage(_provider.Object, Color.Black);

			tabPage.Dispose();

			Assert.IsFalse(_editorControl.IsDisposed,
				"Disposing the TabPage must not dispose the long-lived editor Control it hosted.");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// TabPage derives from Panel, so it's already reached by the generic recursive switch's
		/// Panel case whenever its own ancestor is traversable -- confirms that removing this
		/// class's own direct UITextScaler.ScaleChanged subscription doesn't lose that reach.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_HostContainsTabPage_ScalesEditorControlFont()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new TabControl())
			using (var tabPage = new ComponentEditorTabPage(_provider.Object, Color.Black))
			{
				host.Controls.Add(tabPage);
				var originalSize = _label.Font.Size;

				Settings.Default.UITextScale = 200;
				host.ScaleUIFont();

				Assert.Greater(_label.Font.Size, originalSize);
			}
		}

	}
}
