using NUnit.Framework;
using SayMore.UI.LowLevelControls;

namespace SayMoreTests.UI.LowLevelControls
{
	[TestFixture]
	public class PromptTextBoxTests
	{
		[Test]
		public void GetPromptText_Set_IsReEvaluatedEachTimePromptIsShown_NotCachedFromWhenItWasSet()
		{
			// A PromptTextBox used for a localized placeholder (e.g. ListPanel's search box)
			// needs to reflect a UI language change made while the program is running, the
			// next time the prompt is (re)shown - not whatever string GetPromptText returned
			// when it was first assigned.
			var box = new PromptTextBox();
			var promptText = "English prompt";
			box.GetPromptText = () => promptText;

			// Showing the prompt (empty text, not focused) reads GetPromptText for the first time.
			box.Text = "";
			Assert.AreEqual("English prompt", GetPromptLabelText(box));

			// Simulate a UI language change happening while the box isn't being shown/hidden.
			promptText = "Other-language prompt";

			// The next time the prompt is (re)shown - e.g. the user tabs into then back out of
			// the box - it should pick up the new value, not the one captured when GetPromptText
			// was assigned.
			box.Text = "typing";
			box.Text = "";
			Assert.AreEqual("Other-language prompt", GetPromptLabelText(box));
		}

		[Test]
		public void RefreshPromptDisplay_NotFocusedAndEmpty_ShowsPrompt()
		{
			// A pre-existing WinForms quirk (disabling a parent container that holds focus
			// bypasses the normal Leave event) can leave this control's own Focused state
			// desynced from its placeholder's visibility - unfocused, but still hidden as if
			// it were focused. RefreshPromptDisplay lets an external caller force a recheck.
			var box = new PromptTextBox();
			box.GetPromptText = () => "Filter";
			var label = GetPromptLabel(box);
			label.Visible = false;

			box.RefreshPromptDisplay();

			Assert.IsTrue(label.Visible);
			Assert.AreEqual("Filter", label.Text);
		}

		private static System.Windows.Forms.Label GetPromptLabel(PromptTextBox box)
		{
			var labelField = typeof(PromptTextBox).GetField("_labelPrompt",
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			return (System.Windows.Forms.Label)labelField.GetValue(box);
		}

		private static string GetPromptLabelText(PromptTextBox box)
		{
			return GetPromptLabel(box).Text;
		}
	}
}
