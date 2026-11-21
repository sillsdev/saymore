using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.UI.ProjectWindow;
using SayMore.Utilities;

namespace SayMoreTests.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Reproduces a real crash: clicking OK unconditionally fired UITextScaler.NotifyScaleChanged,
	/// even when the scale percentage hadn't actually changed -- re-triggering a pointless
	/// re-scale pass over controls already correctly scaled, which could hit a font-family-
	/// mismatch edge case in UITextScaler.GetOriginalFont (now fixed separately, but the
	/// redundant notification itself was never needed: at the point OnFormClosing runs, the UI
	/// language hasn't even been switched yet -- the caller only does that after ShowDialog
	/// returns -- so this notification can only ever be about the scale value).
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class UILanguageDlgFormClosingTests
	{
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		private static void InvokeOnFormClosing(UILanguageDlg dlg)
		{
			var method = typeof(UILanguageDlg).GetMethod("OnFormClosing",
				BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(method, "OnFormClosing method not found.");
			method.Invoke(dlg, new object[] { new FormClosingEventArgs(CloseReason.None, false) });
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void OnFormClosing_OKWithScaleUnchanged_DoesNotNotifyScaleChanged()
		{
			Settings.Default.UITextScale = 130;

			using (var dlg = new UILanguageDlg())
			{
				dlg.DialogResult = DialogResult.OK;

				var wasCalled = false;
				EventHandler handler = (s, e) => wasCalled = true;
				UITextScaler.ScaleChanged += handler;
				try
				{
					InvokeOnFormClosing(dlg);
					Assert.IsFalse(wasCalled,
						"The scale value never changed, so no re-scale notification was needed.");
				}
				finally
				{
					UITextScaler.ScaleChanged -= handler;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void OnFormClosing_OKWithScaleChanged_NotifiesScaleChanged()
		{
			Settings.Default.UITextScale = 130;

			using (var dlg = new UILanguageDlg())
			{
				var numUpDown = (NumericUpDown)typeof(UILanguageDlg).GetField("_numUpDownScale",
					BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dlg);
				numUpDown.Value = 150;
				dlg.DialogResult = DialogResult.OK;

				var wasCalled = false;
				EventHandler handler = (s, e) => wasCalled = true;
				UITextScaler.ScaleChanged += handler;
				try
				{
					InvokeOnFormClosing(dlg);
					Assert.IsTrue(wasCalled,
						"The scale value changed from 130 to 150, so a re-scale notification was needed.");
				}
				finally
				{
					UITextScaler.ScaleChanged -= handler;
				}
			}
		}
	}
}
