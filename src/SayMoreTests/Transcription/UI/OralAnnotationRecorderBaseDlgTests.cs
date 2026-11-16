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
			var constrainedSize = new Size(300, 1000);

			using (var host = new Panel())
			using (var listenHint = new Label { AutoSize = true, Font = new Font("Segoe UI", 8.25f), Text = "Listen hint" })
			using (var recordHint = new Label { AutoSize = true, Font = new Font("Segoe UI", 24f), Text = "Record hint" })
			using (var errorInfo = new Label { AutoSize = true, Font = new Font("Segoe UI", 8.25f), Text = "Error info" })
			{
				host.Controls.Add(listenHint);
				host.Controls.Add(recordHint);
				host.Controls.Add(errorInfo);

				var result = OralAnnotationRecorderBaseDlg.GetRequiredTableLayoutButtonsHeight(
					new[] { listenHint, recordHint, errorInfo }, constrainedSize, videoHelpHeight,
					unscaledHeight);

				Assert.That(result, Is.EqualTo(
					recordHint.GetPreferredSize(constrainedSize).Height + recordHint.Margin.Vertical +
					videoHelpHeight));
				Assert.That(result, Is.GreaterThan(unscaledHeight));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// GetPreferredSize reports a label's own content size only; TableLayoutPanel still
		/// reserves that label's Margin around it inside the row, so a row sized to exactly
		/// GetPreferredSize's height clamps the label's real assigned Height down by
		/// Margin.Vertical -- confirmed against the real dialog, where this silently dropped a
		/// wrapped hint's last line. This pins that the returned height includes each label's
		/// own Margin.Vertical, not just its bare GetPreferredSize.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void GetRequiredTableLayoutButtonsHeight_LabelHasVerticalMargin_IncludesMarginInRequiredHeight()
		{
			const int videoHelpHeight = 10;
			const int unscaledHeight = 10;
			var constrainedSize = new Size(300, 1000);

			using (var host = new Panel())
			using (var listenHint = new Label
			{
				AutoSize = true,
				Font = new Font("Segoe UI", 8.25f),
				Text = "Listen hint",
				Margin = new Padding(3, 2, 3, 2)
			})
			{
				host.Controls.Add(listenHint);

				var result = OralAnnotationRecorderBaseDlg.GetRequiredTableLayoutButtonsHeight(
					new[] { listenHint }, constrainedSize, videoHelpHeight, unscaledHeight);

				Assert.That(result, Is.EqualTo(
					listenHint.GetPreferredSize(constrainedSize).Height + listenHint.Margin.Vertical +
					videoHelpHeight));
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void GetRequiredTableLayoutButtonsHeight_HintLabelsFitWithinUnscaledHeight_ReturnsUnscaledHeightUnchanged()
		{
			const int videoHelpHeight = 10;
			const int unscaledHeight = 1000;
			var constrainedSize = new Size(300, 1000);

			using (var host = new Panel())
			using (var listenHint = new Label { AutoSize = true, Font = new Font("Segoe UI", 8.25f), Text = "Listen hint" })
			using (var recordHint = new Label { AutoSize = true, Font = new Font("Segoe UI", 8.25f), Text = "Record hint" })
			using (var errorInfo = new Label { AutoSize = true, Font = new Font("Segoe UI", 8.25f), Text = "Error info" })
			{
				host.Controls.Add(listenHint);
				host.Controls.Add(recordHint);
				host.Controls.Add(errorInfo);

				var result = OralAnnotationRecorderBaseDlg.GetRequiredTableLayoutButtonsHeight(
					new[] { listenHint, recordHint, errorInfo }, constrainedSize, videoHelpHeight,
					unscaledHeight);

				Assert.That(result, Is.EqualTo(unscaledHeight));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The whole reason this reads GetPreferredSize instead of each label's current Height:
		/// a label hosted directly in a TableLayoutPanel's Percent-type cell can have its
		/// actual Height clamped down by that cell when the cell doesn't yet have enough room
		/// -- confirmed against the real dialog, where a 36pt label's Height read back as 9px.
		/// Reading Height in that situation is circular (it already reflects the too-small
		/// constraint this method exists to correct), so this reproduces the real clamping
		/// mechanism (a too-small Percent cell), not just an artificially-set MaximumSize, and
		/// pins GetPreferredSize's insensitivity to it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void GetRequiredTableLayoutButtonsHeight_LabelsCurrentHeightIsClampedSmall_StillReturnsTrueNeededHeight()
		{
			const int videoHelpHeight = 10;
			const int unscaledHeight = 10;
			var constrainedSize = new Size(300, 1000);

			using (var table = new TableLayoutPanel { ColumnCount = 1, RowCount = 1, Width = 300, Height = 9 })
			using (var listenHint = new Label { AutoSize = true, Font = new Font("Segoe UI", 36f), Text = "Listen hint" })
			{
				table.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
				table.Controls.Add(listenHint, 0, 0);
				table.CreateControl();

				Assume.That(listenHint.Height, Is.LessThan(20),
					"This test's premise requires a too-small Percent cell to have already " +
					"clamped the label's current Height down, the same way _tableLayoutButtons' " +
					"row does before this method ever runs.");

				var result = OralAnnotationRecorderBaseDlg.GetRequiredTableLayoutButtonsHeight(
					new[] { listenHint }, constrainedSize, videoHelpHeight, unscaledHeight);

				Assert.That(result, Is.GreaterThan(40),
					"A 36pt label needs well over 40px for even one line; GetPreferredSize " +
					"must report that real need, not the label's currently-clamped Height.");
			}
		}
	}
}
