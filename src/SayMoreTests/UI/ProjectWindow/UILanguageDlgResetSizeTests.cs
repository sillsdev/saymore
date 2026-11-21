using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.UI.ProjectWindow;

namespace SayMoreTests.UI.ProjectWindow
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Diagnostic: after scaling up and then clicking Reset back to 100%, do _lblTextScale,
	/// _buttonOK, and _buttonCancel actually return to their original font size / bounding box?
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class UILanguageDlgResetSizeTests
	{
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		private static T GetPrivateField<T>(object instance, string fieldName)
		{
			var field = typeof(UILanguageDlg).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(field, $"Field '{fieldName}' not found.");
			return (T)field.GetValue(instance);
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ClickReset_AfterScalingUp_RestoresOriginalFontSizeAndControlSize()
		{
			Settings.Default.UITextScale = 100;

			using (var dlg = new UILanguageDlg())
			{
				dlg.Show();

				var lblTextScale = GetPrivateField<Label>(dlg, "_lblTextScale");
				var buttonOK = GetPrivateField<Button>(dlg, "_buttonOK");
				var buttonCancel = GetPrivateField<Button>(dlg, "_buttonCancel");
				var numUpDownScale = GetPrivateField<NumericUpDown>(dlg, "_numUpDownScale");
				var btnReset = GetPrivateField<Button>(dlg, "_btnReset");

				var originalLabelFontSize = lblTextScale.Font.Size;
				var originalOKFontSize = buttonOK.Font.Size;
				var originalCancelFontSize = buttonCancel.Font.Size;
				var originalOKSize = buttonOK.Size;
				var originalCancelSize = buttonCancel.Size;
				var originalLabelSize = lblTextScale.Size;

				numUpDownScale.Value = 180;
				Assert.Greater(buttonOK.Font.Size, originalOKFontSize, "Sanity check: scaling up should have grown the OK button's font.");

				btnReset.PerformClick();

				Assert.Multiple(() =>
				{
					Assert.AreEqual(100, numUpDownScale.Value, "Reset should bring the spinner back to 100 for English.");
					Assert.AreEqual(originalLabelFontSize, lblTextScale.Font.Size, "_lblTextScale font size");
					Assert.AreEqual(originalOKFontSize, buttonOK.Font.Size, "_buttonOK font size");
					Assert.AreEqual(originalCancelFontSize, buttonCancel.Font.Size, "_buttonCancel font size");
					Assert.AreEqual(originalLabelSize, lblTextScale.Size, "_lblTextScale control size");
					Assert.AreEqual(originalOKSize, buttonOK.Size, "_buttonOK control size");
					Assert.AreEqual(originalCancelSize, buttonCancel.Size, "_buttonCancel control size");
				});
			}
		}
	}
}
