using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.Properties;
using SayMore.UI;
using SayMore.Utilities;

namespace SayMoreTests.Utilities
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	[NonParallelizable]
	public class UITextScalerTests
	{
		private class DiagnosticForm : SayMoreFormBase { }

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void NotifyScaleChanged_SubscriberRegistered_SubscriberIsInvoked()
		{
			var wasCalled = false;
			EventHandler handler = (s, e) => wasCalled = true;

			UITextScaler.ScaleChanged += handler;
			try
			{
				UITextScaler.NotifyScaleChanged();
				Assert.IsTrue(wasCalled);
			}
			finally
			{
				UITextScaler.ScaleChanged -= handler;
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void NotifyScaleChanged_NoSubscribers_DoesNotThrow()
		{
			Assert.DoesNotThrow(() => UITextScaler.NotifyScaleChanged());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void NotifyScaleChanged_SubscriberUnregistered_SubscriberIsNotInvoked()
		{
			var wasCalled = false;
			EventHandler handler = (s, e) => wasCalled = true;

			UITextScaler.ScaleChanged += handler;
			UITextScaler.ScaleChanged -= handler;

			UITextScaler.NotifyScaleChanged();

			Assert.IsFalse(wasCalled);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_ValueWithinRange_ReturnsSameValue()
		{
			Assert.AreEqual(150, UITextScaler.ClampScalePercent(150));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_ValueZero_ReturnsMinimum()
		{
			Assert.AreEqual(UITextScaler.kMinScalePercent, UITextScaler.ClampScalePercent(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_NegativeValue_ReturnsMinimum()
		{
			Assert.AreEqual(UITextScaler.kMinScalePercent, UITextScaler.ClampScalePercent(-50));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_ValueBelowMinimum_ReturnsMinimum()
		{
			Assert.AreEqual(UITextScaler.kMinScalePercent,
				UITextScaler.ClampScalePercent(UITextScaler.kMinScalePercent - 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_ValueAboveMaximum_ReturnsMaximum()
		{
			Assert.AreEqual(UITextScaler.kMaxScalePercent,
				UITextScaler.ClampScalePercent(UITextScaler.kMaxScalePercent + 1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void ClampScalePercent_RidiculouslyLargeValue_ReturnsMaximum()
		{
			Assert.AreEqual(UITextScaler.kMaxScalePercent, UITextScaler.ClampScalePercent(int.MaxValue));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRecommendedScalePercent_Tibetan_ReturnsLargerThanDefault()
		{
			var result = UITextScaler.GetRecommendedScalePercent("bo");
			Assert.IsTrue(result.HasValue);
			Assert.Greater(result.Value, 100);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRecommendedScalePercent_LanguageWithoutRecommendation_ReturnsNull()
		{
			Assert.IsNull(UITextScaler.GetRecommendedScalePercent("en"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetRecommendedScalePercent_NullLanguageCode_ReturnsNull()
		{
			Assert.IsNull(UITextScaler.GetRecommendedScalePercent(null));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Guards against a Debug.Fail -- which pops a blocking dialog, even under an automated
		/// NUnit run -- when a control's cached original font no longer matches its current font
		/// family/style on a redundant scaling pass (e.g. a scale-changed notification firing
		/// even though the percentage didn't actually change, or a mixed-family Designer
		/// hierarchy). The fix: treat a mismatch as the control's font legitimately changing out
		/// from under us, not a bug to flag.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void GetOriginalFont_ControlsCurrentFontFamilyDiffersFromCachedOriginal_DoesNotThrowAndRecachesCurrentFont()
		{
			using (var control = new Label { Font = new Font(FontFamily.GenericSansSerif, 9f) })
			{
				var cached = UITextScaler.GetOriginalFont(control);
				Assert.AreEqual(FontFamily.GenericSansSerif, cached.FontFamily);

				control.Font = new Font(FontFamily.GenericSerif, 12f);

				Font recached = null;
				Assert.DoesNotThrow(() => recached = UITextScaler.GetOriginalFont(control));
				Assert.AreEqual(FontFamily.GenericSerif, recached.FontFamily);

				// A third call with the font unchanged should now hit the fast (cached) path.
				var thirdCall = UITextScaler.GetOriginalFont(control);
				Assert.AreEqual(FontFamily.GenericSerif, thirdCall.FontFamily);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Guards against reconstructing a font from its family, size, and style while
		/// discarding its GraphicsUnit. A control's baseline font can be declared in a unit
		/// other than Point (e.g. World, as the 3-argument Font constructor's default would
		/// silently convert to); losing that unit re-expresses the same numeric size in a
		/// different unit, inflating or shrinking the rendered text even at a neutral 100% scale.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ControlsBaselineFontUsesWorldUnits_PreservesUnitEvenAtNeutralScale()
		{
			Settings.Default.UITextScale = 100;

			using (var button = new Button
			{
				Text = "New",
				Font = new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.World)
			})
			{
				button.ScaleUIFont();

				Assert.AreEqual(GraphicsUnit.World, button.Font.Unit,
					"Reconstructing the font must preserve the original GraphicsUnit, not silently convert it to Point.");
				Assert.AreEqual(11f, button.Font.Size, 0.01f,
					"At 100% scale the numeric size (in its original unit) should be unchanged.");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A control's Text is a mutable, independent property -- a Label with empty Text now
		/// can have text set into it later (e.g. a status message, or a field populated after a
		/// slower load). Skipping the Font assignment while Text happens to be empty would leave
		/// that control's Font un-set (ambient) only until the next time its own Font is ever
		/// explicitly assigned for any reason.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ControlHasEmptyText_FontIsStillScaled()
		{
			Settings.Default.UITextScale = 200;

			using (var label = new Label { Text = "", Font = new Font(FontFamily.GenericSansSerif, 9f) })
			{
				var originalSize = label.Font.Size;

				label.ScaleUIFont();

				Assert.Greater(label.Font.Size, originalSize);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The recursive scaling pass only recurses into a hardcoded allowlist of container
		/// types. SplitContainer isn't a Panel subclass, so without explicit handling, content
		/// inside its Panel1/Panel2 (e.g. a left-hand sidebar) would be silently skipped even
		/// though the SplitContainer itself is reached.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_LabelInsideSplitContainerPanel1_ScalesLabelFont()
		{
			Settings.Default.UITextScale = 200;

			using (var host = new Panel())
			using (var split = new SplitContainer())
			{
				var label = new Label { Text = "Sidebar", Font = new Font(FontFamily.GenericSansSerif, 10f) };
				split.Panel1.Controls.Add(label);
				host.Controls.Add(split);
				var origSize = label.Font.Size;

				host.ScaleUIFont();

				Assert.Greater(label.Font.Size, origSize);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A Control with both AutoSize=true and an explicit MinimumSize (common for buttons
		/// throughout the app) grows correctly as its font grows, but once its preferred size
		/// has exceeded MinimumSize, shrinking the font back down doesn't reliably shrink the
		/// control back down on its own -- a known WinForms quirk. Handling this centrally, in
		/// the shared recursive scaling pass, means every scaled Form/UserControl in the app
		/// gets the fix automatically, rather than needing it repeated per-dialog.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ButtonWithAutoSizeAndMinimumSizeScaledUpThenBackDown_ShrinksBackToOriginalSize()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel())
			{
				var button = new Button
				{
					AutoSize = true,
					AutoSizeMode = AutoSizeMode.GrowAndShrink,
					MinimumSize = new Size(117, 26),
					Text = "New From Recording...",
					Font = new Font(FontFamily.GenericSansSerif, 8.25f)
				};
				host.Controls.Add(button);

				var originalSize = button.Size;

				Settings.Default.UITextScale = 180;
				host.ScaleUIFont();
				Assert.Greater(button.Height, originalSize.Height, "Sanity check: scaling up should have grown the button.");

				Settings.Default.UITextScale = 100;
				host.ScaleUIFont();

				Assert.AreEqual(originalSize, button.Size);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A ComboBox docked in the last row of an AutoSize TableLayoutPanel can end up with its
		/// row too short to contain it after scaling, even though the ComboBox's own rendered
		/// height is always correct (the OS enforces PreferredHeight regardless of AutoSize). The
		/// cause: WinForms' TableLayoutPanel under-measures an AutoSize row containing a ComboBox,
		/// because ComboBox.AutoSize defaults to false and the panel's row-AutoSize calculation
		/// doesn't fall back to GetPreferredSize() for non-AutoSize children the way it does for
		/// e.g. Labels -- a pre-existing WinForms quirk, not something the scaling pass introduces.
		/// Scaling isn't required to trigger it: a single TableLayoutPanel and a ComboBox given a
		/// large font directly reproduces it just as well; only the parent's row-height
		/// bookkeeping is wrong.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ComboBoxDockedInAutoSizeTableLayoutPanelRow_RowGrowsToFitAfterScaling()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel { Size = new Size(400, 400) })
			{
				var panel = BuildAccessLikeRow(out var combo);
				host.Controls.Add(panel);
				host.CreateControl();
				panel.PerformLayout();

				Settings.Default.UITextScale = 180;
				host.ScaleUIFont();

				Assert.GreaterOrEqual(panel.Height, combo.Bottom,
					"The combo box's own row/panel should be tall enough to fully contain it after scaling, not clip it.");
			}
		}

		/// ------------------------------------------------------------------------------------
		private static TableLayoutPanel BuildAccessLikeRow(out ComboBox combo)
		{
			var font = new Font(FontFamily.GenericSansSerif, 8.25f);
			combo = new ComboBox { Dock = DockStyle.Top, Font = font, Text = "Not Restricted" };
			var label = new Label { AutoSize = true, Text = "Access", Font = font };

			var panel = new TableLayoutPanel
			{
				Dock = DockStyle.Top,
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				ColumnCount = 1,
				RowCount = 2
			};
			panel.RowStyles.Add(new RowStyle());
			panel.RowStyles.Add(new RowStyle());
			panel.Controls.Add(label, 0, 0);
			panel.Controls.Add(combo, 0, 1);
			return panel;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A DataGridView column with its own explicit HeaderCell.Style.Font (e.g. an
		/// intentionally bold header) must keep that styling through a scaling pass, not have it
		/// replaced by an ancestor-derived font that would strip the Bold and any other distinct
		/// styling.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ColumnHasExplicitBoldHeaderFont_PreservesBoldAndScalesSize()
		{
			Settings.Default.UITextScale = 200;

			using (var host = new Panel())
			using (var grid = new DataGridView())
			{
				var col = new DataGridViewTextBoxColumn { HeaderText = "Transcription" };
				col.HeaderCell.Style.Font = new Font(grid.Font, FontStyle.Bold);
				grid.Columns.Add(col);
				host.Controls.Add(grid);
				var originalSize = col.HeaderCell.Style.Font.Size;

				host.ScaleUIFont();

				Assert.IsTrue(col.HeaderCell.Style.Font.Bold,
					"Scaling must not strip a column's own explicit Bold header styling.");
				Assert.Greater(col.HeaderCell.Style.Font.Size, originalSize);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Guards against a redundant scaling pass (e.g. a scale-changed notification firing
		/// even though the percentage didn't actually change) compounding the scale factor on
		/// top of an already-scaled header font, or losing the Bold styling on a second pass.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_CalledTwiceWithSameScale_DoesNotCompoundSizeAndKeepsBold()
		{
			Settings.Default.UITextScale = 200;

			using (var host = new Panel())
			using (var grid = new DataGridView())
			{
				var col = new DataGridViewTextBoxColumn { HeaderText = "Transcription" };
				col.HeaderCell.Style.Font = new Font(grid.Font, FontStyle.Bold);
				grid.Columns.Add(col);
				host.Controls.Add(grid);

				host.ScaleUIFont();
				var sizeAfterFirstPass = col.HeaderCell.Style.Font.Size;

				host.ScaleUIFont();

				Assert.AreEqual(sizeAfterFirstPass, col.HeaderCell.Style.Font.Size, 0.01f);
				Assert.IsTrue(col.HeaderCell.Style.Font.Bold);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A control that never gets its own Font explicitly set (e.g. ListPanel's buttons
		/// panel and New button) ambient-tracks its parent's Font instead of holding its own
		/// value. Caching such a control's "original" font the first time it's touched is only
		/// correct if that happens before any ancestor in the same pass has already been
		/// rescaled; otherwise it picks up the ancestor's already-scaled Font as its own
		/// baseline, and the scale factor compounds once per ambient level.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ButtonAmbientlyInheritsFontThroughMultipleUnsetLevels_DoesNotCompoundScale()
		{
			Settings.Default.UITextScale = 100;

			using (var outer = new Panel
			{
				Font = new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.World)
			})
			using (var flow = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink })
			using (var button = new Button
			{
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				MinimumSize = new Size(75, 26),
				Text = "&New"
			})
			{
				flow.Controls.Add(button);
				outer.Controls.Add(flow);
				outer.CreateControl();

				Settings.Default.UITextScale = 130;
				outer.ScaleUIFont();

				Assert.AreEqual(outer.Font.Size, button.Font.Size, 0.01f,
					"A control several levels of unset Font below the scaled root should end up " +
					"at the same scaled size as the root, not compounded once per ambient level.");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A DataGridView's own Font must be scaled too, not just its column headers: an
		/// unstyled cell/column inherits through it, and code that paints directly onto the grid
		/// (e.g. a drag-and-drop hint message) reads it directly, since that code has no
		/// per-cell style of its own to pull from.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_DataGridView_ScalesGridsOwnFont()
		{
			Settings.Default.UITextScale = 200;

			using (var host = new Panel())
			using (var grid = new DataGridView())
			{
				host.Controls.Add(grid);
				var originalSize = grid.Font.Size;

				host.ScaleUIFont();

				Assert.Greater(grid.Font.Size, originalSize);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A control added to an already-loaded Form's tree well after that Form's own
		/// one-time Load-triggered scaling pass -- e.g. a view a screen swaps into a
		/// placeholder panel only once the user navigates to it, such as ProjectScreen's project
		/// sub-views -- should get scaled at the point it's swapped in.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_CalledOnControlSwappedIntoAlreadyLoadedForm_FontIsScaled()
		{
			Settings.Default.UITextScale = 200;

			using (var form = new DiagnosticForm())
			using (var panel2 = new Panel { Dock = DockStyle.Fill })
			using (var label = new Label { AutoSize = true, Text = "Caption",
				Font = new Font(FontFamily.GenericSansSerif, 8.25f) })
			{
				form.Controls.Add(panel2);
				var originalSize = label.Font.Size;

				form.Show();
				try
				{
					// Simulates a screen swapping a new sub-view into an already-loaded Form,
					// the way ProjectScreen.ShowControl does.
					panel2.Controls.Add(label);
					label.ScaleUIFont();

					Assert.Greater(label.Font.Size, originalSize);
				}
				finally
				{
					form.Close();
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A ToolStrip's (e.g. MenuStrip's) own Font must be scaled too, not just its items'
		/// fonts: it governs the strip's own layout/height independent of the items' fonts, so
		/// an unscaled strip can end up too short to properly display the now-larger item text.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ToolStrip_ScalesStripsOwnFont()
		{
			Settings.Default.UITextScale = 200;

			using (var host = new Panel())
			using (var menuStrip = new MenuStrip())
			{
				host.Controls.Add(menuStrip);
				var originalSize = menuStrip.Font.Size;

				host.ScaleUIFont();

				Assert.Greater(menuStrip.Font.Size, originalSize);
			}
		}
	}
}
