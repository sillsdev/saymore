using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Transcription.UI;

namespace SayMoreTests.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// _labelListenHint/_labelRecordHint/_labelErrorInfo share three equal Percent rows in
	/// _tableLayoutButtons, whose own total Height is fixed regardless of UI text scale -- this
	/// tests the pure height-calculation helper directly, without constructing the whole dialog
	/// (which needs a real ViewModel/ComponentFile).
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class OralAnnotationRecorderBaseDlgTests
	{
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void GetRequiredTableLayoutButtonsHeight_HintLabelScaledUpPastUnscaledHeight_GrowsToFitTallestHint()
		{
			const int videoHelpHeight = 10;
			const int unscaledHeight = 30;

			using (var host = new Panel())
			using (var listenHint = new Label { AutoSize = true, Font = new Font("Segoe UI", 8.25f), Text = "Listen hint" })
			using (var recordHint = new Label { AutoSize = true, Font = new Font("Segoe UI", 24f), Text = "Record hint" })
			using (var errorInfo = new Label { AutoSize = true, Font = new Font("Segoe UI", 8.25f), Text = "Error info" })
			{
				host.Controls.Add(listenHint);
				host.Controls.Add(recordHint);
				host.Controls.Add(errorInfo);

				var result = OralAnnotationRecorderBaseDlg.GetRequiredTableLayoutButtonsHeight(
					new[] { listenHint, recordHint, errorInfo }, videoHelpHeight, unscaledHeight);

				var expectedTallest = recordHint.GetPreferredSize(Size.Empty).Height;
				Assert.That(result, Is.EqualTo(expectedTallest + videoHelpHeight));
				Assert.That(result, Is.GreaterThan(unscaledHeight));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void GetRequiredTableLayoutButtonsHeight_HintLabelsFitWithinUnscaledHeight_ReturnsUnscaledHeightUnchanged()
		{
			const int videoHelpHeight = 10;
			const int unscaledHeight = 1000;

			using (var host = new Panel())
			using (var listenHint = new Label { AutoSize = true, Font = new Font("Segoe UI", 8.25f), Text = "Listen hint" })
			using (var recordHint = new Label { AutoSize = true, Font = new Font("Segoe UI", 8.25f), Text = "Record hint" })
			using (var errorInfo = new Label { AutoSize = true, Font = new Font("Segoe UI", 8.25f), Text = "Error info" })
			{
				host.Controls.Add(listenHint);
				host.Controls.Add(recordHint);
				host.Controls.Add(errorInfo);

				var result = OralAnnotationRecorderBaseDlg.GetRequiredTableLayoutButtonsHeight(
					new[] { listenHint, recordHint, errorInfo }, videoHelpHeight, unscaledHeight);

				Assert.That(result, Is.EqualTo(unscaledHeight));
			}
		}
	}
}
