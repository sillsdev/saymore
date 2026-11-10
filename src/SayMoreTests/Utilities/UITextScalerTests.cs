using System;
using System.Drawing;
using System.Runtime.InteropServices;
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
		[Test]
		public void GetScaledWorkingLanguageFont_200Percent_DoublesSize()
		{
			using (var baseline = new Font("Times New Roman", 14f, FontStyle.Italic))
			using (var scaled = UITextScaler.GetScaledWorkingLanguageFont(baseline, 200))
			{
				Assert.AreEqual(28f, scaled.Size, 0.01f);
				Assert.AreEqual(baseline.FontFamily, scaled.FontFamily,
					"Scaling must change only the working-language font's size, never its family.");
				Assert.AreEqual(baseline.Style, scaled.Style);
				Assert.AreEqual(baseline.Unit, scaled.Unit);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetScaledWorkingLanguageFont_PercentAboveMaximum_ClampsToMaximum()
		{
			using (var baseline = new Font("Times New Roman", 10f))
			using (var scaled = UITextScaler.GetScaledWorkingLanguageFont(baseline, 10000))
			{
				Assert.AreEqual(10f * UITextScaler.kMaxScalePercent / 100f, scaled.Size, 0.01f);
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetScaledWorkingLanguageFont_NullBaseline_ReturnsNull()
		{
			Assert.IsNull(UITextScaler.GetScaledWorkingLanguageFont(null, 150));
			Assert.IsNull(UITextScaler.GetScaledWorkingLanguageFont(null));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Text boxes and FieldsValuesGrid cells showing working-language content all call this
		/// same overload, so for a given baseline and scale they all render with one shared Font.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetScaledWorkingLanguageFont_CurrentScale_SharedUntilScaleChanges()
		{
			using (var baseline = new Font("Times New Roman", 14f))
			{
				Settings.Default.UITextScale = 100;
				var first = UITextScaler.GetScaledWorkingLanguageFont(baseline);
				Assert.AreSame(first, UITextScaler.GetScaledWorkingLanguageFont(baseline));
				Assert.AreEqual(14f, first.Size, 0.01f);

				Settings.Default.UITextScale = 150;
				var afterScaleChange = UITextScaler.GetScaledWorkingLanguageFont(baseline);
				Assert.AreEqual(21f, afterScaleChange.Size, 0.01f);
				Assert.AreEqual(baseline.FontFamily, afterScaleChange.FontFamily);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// A control whose font is owned by other code (e.g. a working-language text box, whose
		/// editor assigns it an already-scaled working-language font) must keep that font through
		/// a scaling pass; rescaling it again would compound the scale factor.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ControlExcludedFromUIFontScaling_KeepsItsFont()
		{
			using (var host = new Panel())
			using (var excluded = new Label { Font = new Font("Times New Roman", 18f) })
			{
				host.Controls.Add(excluded);
				UITextScaler.ExcludeFromUIFontScaling(excluded);
				Settings.Default.UITextScale = 200;

				host.ScaleUIFont();
				Assert.AreEqual(18f, excluded.Font.Size, 0.01f, "Reached through an ancestor's walk");

				excluded.ScaleUIFont();
				Assert.AreEqual(18f, excluded.Font.Size, 0.01f, "Scaled directly");
			}
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
		/// A control's baseline font can be declared in a unit other than Point (e.g. World); a
		/// staleness check that only compares FontFamily/Style would silently treat a Unit-only
		/// change as "still the same baseline" and return the stale original.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void GetOriginalFont_ControlsCurrentFontUnitDiffersFromCachedOriginal_RecachesCurrentFont()
		{
			using (var control = new Label { Font = new Font(FontFamily.GenericSansSerif, 9f, FontStyle.Regular, GraphicsUnit.Point) })
			{
				UITextScaler.GetOriginalFont(control);

				control.Font = new Font(FontFamily.GenericSansSerif, 9f, FontStyle.Regular, GraphicsUnit.World);

				var recached = UITextScaler.GetOriginalFont(control);
				Assert.AreEqual(GraphicsUnit.World, recached.Unit);
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
		/// A ComboBox anchored Right (stretching to fill its TableLayoutPanel cell, e.g.
		/// ProjectMetadataScreen's Country/Continent fields) tracks whatever width the panel
		/// computes for that cell. When the cell's own column is sized by SizeType.Percent and a
		/// neighboring SizeType.AutoSize column holds a label that grows during the same scaling
		/// pass, the Percent column's share of the available width shrinks -- taking the
		/// Right-anchored combo's rendered width down with it, even though its Font just grew.
		/// Without a scaled MinimumSize floor, the combo can end up narrower after scaling than
		/// before, cramming its (now larger) selected text into a smaller box instead of growing
		/// to fit it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_RightAnchoredComboInPercentColumnSqueezedByGrowingAutoSizeLabel_DoesNotShrinkBelowScaledWidth()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel { Size = new Size(900, 200) })
			{
				var panel = BuildCountryLikeRow(out var combo);
				host.Controls.Add(panel);
				host.CreateControl();
				panel.PerformLayout();

				var widthAt100 = combo.Width;

				Settings.Default.UITextScale = 200;
				host.ScaleUIFont();

				Assert.GreaterOrEqual(combo.Width, widthAt100,
					"A Right-anchored ComboBox in a Percent-sized TableLayoutPanel column must not " +
					"end up narrower after scaling, even though a neighboring AutoSize column's " +
					"label grows at the same time and would otherwise squeeze the Percent column's " +
					"share of the available width.");
			}

			Settings.Default.UITextScale = 100;
		}

		/// ------------------------------------------------------------------------------------
		private static TableLayoutPanel BuildCountryLikeRow(out ComboBox combo)
		{
			var labelFont = new Font(FontFamily.GenericSansSerif, 9.75f, FontStyle.Bold);
			var comboFont = new Font(FontFamily.GenericSansSerif, 12f);

			var label = new Label
			{
				AutoSize = true,
				Text = "Some Fairly Long Field Label",
				Font = labelFont
			};

			combo = new ComboBox
			{
				Font = comboFont,
				Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
				Width = 300
			};

			var panel = new TableLayoutPanel
			{
				Dock = DockStyle.Top,
				ColumnCount = 2,
				RowCount = 1,
				Width = 900
			};
			panel.ColumnStyles.Add(new ColumnStyle());
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
			panel.RowStyles.Add(new RowStyle());
			panel.Controls.Add(label, 0, 0);
			panel.Controls.Add(combo, 1, 0);
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
		/// <summary>
		/// A ToolStripItem with its own Designer-set styling (e.g. SegmenterDlgBase's Bold
		/// "Undo" menu item) must keep that styling through scaling, not have it replaced by an
		/// ancestor-derived font that strips the Bold -- the same class of bug already fixed for
		/// DataGridViewColumn header fonts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ToolStripItemHasExplicitBoldFont_PreservesBoldAndScalesSize()
		{
			Settings.Default.UITextScale = 200;

			using (var host = new Panel())
			using (var menuStrip = new MenuStrip())
			{
				var item = new ToolStripMenuItem("Undo") { Font = new Font(menuStrip.Font, FontStyle.Bold) };
				menuStrip.Items.Add(item);
				host.Controls.Add(menuStrip);
				var originalSize = item.Font.Size;

				host.ScaleUIFont();

				Assert.IsTrue(item.Font.Bold, "Scaling must not strip a tool strip item's own explicit Bold styling.");
				Assert.Greater(item.Font.Size, originalSize);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Guards against a redundant scaling pass compounding the scale factor on top of an
		/// already-scaled tool strip item font, or losing the Bold styling on a second pass.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ToolStripItemCalledTwiceWithSameScale_DoesNotCompoundSizeAndKeepsBold()
		{
			Settings.Default.UITextScale = 200;

			using (var host = new Panel())
			using (var menuStrip = new MenuStrip())
			{
				var item = new ToolStripMenuItem("Undo") { Font = new Font(menuStrip.Font, FontStyle.Bold) };
				menuStrip.Items.Add(item);
				host.Controls.Add(menuStrip);

				host.ScaleUIFont();
				var sizeAfterFirstPass = item.Font.Size;

				host.ScaleUIFont();

				Assert.AreEqual(sizeAfterFirstPass, item.Font.Size, 0.01f);
				Assert.IsTrue(item.Font.Bold);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// An item with no explicit Font of its own ambiently reads its owner strip's current
		/// Font. The strip's own Font is scaled before its items are processed, so caching an
		/// unstyled item's "original" after that point would wrongly capture the already-scaled
		/// strip font, compounding the scale on every subsequent pass -- including a pass back
		/// down to 100%, which must still fully recover the original size.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ToolStripItemHasNoExplicitFont_DoesNotCompoundAcrossRepeatedScaleChanges()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel())
			using (var menuStrip = new MenuStrip())
			{
				var item = new ToolStripMenuItem("Open");
				menuStrip.Items.Add(item);
				host.Controls.Add(menuStrip);
				var originalSize = item.Font.Size;

				Settings.Default.UITextScale = 200;
				host.ScaleUIFont();

				Assert.AreEqual(originalSize * 2f, item.Font.Size, 0.1f,
					"An unstyled item must scale by exactly the configured factor, not compound with the strip's own already-scaled font.");

				Settings.Default.UITextScale = 100;
				host.ScaleUIFont();

				Assert.AreEqual(originalSize, item.Font.Size, 0.1f,
					"Returning to 100% must fully recover the original size, not stay corrupted from a bad cached baseline.");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Same ambient-tracking hazard as the sibling-item case above, but for a submenu item,
		/// which ambiently reads its parent item's Font rather than the strip's.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_UnstyledSubmenuItem_DoesNotCompound()
		{
			Settings.Default.UITextScale = 100;

			using (var host = new Panel())
			using (var menuStrip = new MenuStrip())
			{
				var topItem = new ToolStripMenuItem("File");
				var subItem = new ToolStripMenuItem("Open");
				topItem.DropDownItems.Add(subItem);
				menuStrip.Items.Add(topItem);
				host.Controls.Add(menuStrip);
				var originalSize = subItem.Font.Size;

				Settings.Default.UITextScale = 200;
				host.ScaleUIFont();

				Assert.AreEqual(originalSize * 2f, subItem.Font.Size, 0.1f,
					"A submenu item must scale by exactly the configured factor, not compound through its parent item's already-scaled font.");
			}
		}

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

		/// ------------------------------------------------------------------------------------
		private class FakeScalablePanel : Panel, IUiScalable
		{
			public int ApplyScaleCallCount { get; private set; }
			public void ApplyScale(float scale) => ApplyScaleCallCount++;
		}

		/// ------------------------------------------------------------------------------------
		private class FakeScalableDataGridView : DataGridView, IUiScalable
		{
			public int ApplyScaleCallCount { get; private set; }
			public void ApplyScale(float scale) => ApplyScaleCallCount++;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// ViewTabGroup is a Panel that also implements IUiScalable. The switch in ApplyFontCore
		/// matches case patterns in textual order -- the first match wins, so the IUiScalable
		/// case must come before the generic Panel case or a Panel-derived IUiScalable would be
		/// handled by the wrong one and ApplyScale would never be called.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ChildIsPanelThatImplementsIUiScalable_DispatchesToApplyScaleNotGenericPanelCase()
		{
			Settings.Default.UITextScale = 200;

			using (var host = new Panel())
			using (var child = new FakeScalablePanel())
			{
				host.Controls.Add(child);

				host.ScaleUIFont();

				Assert.AreEqual(1, child.ApplyScaleCallCount,
					"A Panel-derived IUiScalable must be dispatched via its own ApplyScale, not the generic Panel case.");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// FieldsValuesGrid is a DataGridView that also implements IUiScalable. Same ordering
		/// requirement as the Panel case above, but for the DataGridView case.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ChildIsDataGridViewThatImplementsIUiScalable_DispatchesToApplyScaleNotGenericDataGridViewCase()
		{
			Settings.Default.UITextScale = 200;

			using (var host = new Panel())
			using (var child = new FakeScalableDataGridView())
			{
				host.Controls.Add(child);

				host.ScaleUIFont();

				Assert.AreEqual(1, child.ApplyScaleCallCount,
					"A DataGridView-derived IUiScalable must be dispatched via its own ApplyScale, not the generic DataGridView case.");
			}
		}

		[DllImport("user32.dll")]
		private static extern bool IsWindowVisible(IntPtr hWnd);

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sending WM_SETREDRAW(TRUE) to a hidden window is a documented Win32 quirk that can
		/// make the window visible again as a side effect, even though WinForms' own Visible
		/// property still (wrongly) reports false -- e.g. StagesControlToolTip, a long-lived
		/// hidden form reused between hovers, would reappear as a stale "ghost" the next time
		/// any open dialog's scale changes. ScaleUIFont must not suppress/restore repaint on a
		/// control that isn't actually visible on screen.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test, Apartment(ApartmentState.STA)]
		public void ScaleUIFont_ControlIsHidden_DoesNotMakeNativeWindowVisible()
		{
			Settings.Default.UITextScale = 200;

			using (var form = new Form { Visible = false })
			{
				var handle = form.Handle; // Forces native handle creation while still hidden.

				form.ScaleUIFont();

				Assert.IsFalse(IsWindowVisible(handle),
					"Scaling a hidden control must not make its native window visible.");
			}
		}
	}
}
