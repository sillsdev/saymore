using System.Drawing;
using System.Reflection;
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
		public void ScaleChanged_TabPageAlreadyCreated_RescalesEditorControlFontLive()
		{
			Settings.Default.UITextScale = 100;

			using (new ComponentEditorTabPage(_provider.Object, Color.Black))
			{
				var sizeAt100Percent = _label.Font.Size;

				Settings.Default.UITextScale = 300;
				UITextScaler.NotifyScaleChanged();

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
		/// SetProvider is public, so nothing stops a future caller from reassigning a tab page's
		/// provider after construction. Unlike its own TabTextChanged subscription (unsubscribed
		/// defensively if EditorProvider is already set), the original code unconditionally
		/// re-subscribed to the static ScaleChanged event on every call, so a second call left
		/// two registrations where Dispose only ever removes one. The dangling registration would
		/// then keep firing HandleScaleChanged against a disposed tab page on every future scale
		/// change for the lifetime of the process. Asserted by counting ScaleChanged's actual
		/// subscriber delta, since a disposed control's Font setter doesn't reliably throw, so
		/// Assert.DoesNotThrow can't tell a dangling subscription from a correctly removed one.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void Dispose_SetProviderCalledTwiceBeforeDispose_FullyUnsubscribesFromScaleChanged()
		{
			Settings.Default.UITextScale = 100;
			var baselineSubscriberCount = GetScaleChangedSubscriberCount();

			var tabPage = new ComponentEditorTabPage(_provider.Object, Color.Black);
			tabPage.SetProvider(_provider.Object);
			tabPage.Dispose();

			Assert.AreEqual(baselineSubscriberCount, GetScaleChangedSubscriberCount(),
				"Dispose should leave no dangling ScaleChanged subscriptions, however many times SetProvider was called.");
		}

		/// ------------------------------------------------------------------------------------
		private static int GetScaleChangedSubscriberCount()
		{
			var field = typeof(UITextScaler).GetField("ScaleChanged",
				BindingFlags.NonPublic | BindingFlags.Static);
			var del = (System.Delegate)field.GetValue(null);
			return del?.GetInvocationList().Length ?? 0;
		}
	}
}
