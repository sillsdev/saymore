using SIL.Reporting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using SIL.Windows.Forms.Extensions;

namespace SayMore.Utilities
{
	internal static class UITextScaler
	{
		// UILanguageDlg's constructor overwrites _numUpDownScale's Designer-configured
		// Minimum/Maximum with these at construction time, so these are the actual source
		// of truth even though the Designer file also sets the values.
		public const int kMinScalePercent = 70;
		public const int kMaxScalePercent = 200;

		// Scripts whose vertical glyph stacking (e.g. Tibetan's vowel signs, prefixes, and
		// subjoined consonants) makes them look disproportionately small/cramped at the same
		// point size as Latin text. Keyed by L10NCultureInfo.Name (e.g. "bo" for Tibetan).
		private static readonly Dictionary<string, int> s_recommendedScalePercentByLanguage =
			new Dictionary<string, int> { ["bo"] = 130 };

		private static readonly ConditionalWeakTable<Control, Font> s_origFonts = new ConditionalWeakTable<Control, Font>();

		private static readonly ConditionalWeakTable<ComboBox, StrongBox<int>> s_baselineDropDownWidths =
			new ConditionalWeakTable<ComboBox, StrongBox<int>>();

		private static readonly ConditionalWeakTable<DataGridViewColumn, Font> s_origColumnHeaderFonts =
			new ConditionalWeakTable<DataGridViewColumn, Font>();

		private static readonly ConditionalWeakTable<ToolStripItem, Font> s_origToolStripItemFonts =
			new ConditionalWeakTable<ToolStripItem, Font>();

		private static readonly ConditionalWeakTable<Control, object> s_controlsExcludedFromUIFontScaling =
			new ConditionalWeakTable<Control, object>();

		private static readonly object s_scaledWorkingLanguageFontLock = new object();
		private static Font s_scaledWorkingLanguageFontBaseline;
		private static int s_scaledWorkingLanguageFontPercent;
		private static Font s_scaledWorkingLanguageFont;

		public static event EventHandler ScaleChanged;

		public static void NotifyScaleChanged()
		{
			ScaleChanged?.Invoke(null, EventArgs.Empty);
		}

		/// <summary>
		/// Clamps a text-scale percentage to a range that can never produce a non-positive or
		/// otherwise invalid font size. Defends against bad values reaching here from anywhere
		/// a scale percentage could end up out of range (e.g. a user actively typing into the
		/// scale spinner before its value is fully parsed/validated, or a corrupted settings
		/// file), not just from the control that currently happens to be the source of truth.
		/// </summary>
		public static int ClampScalePercent(int percent)
		{
			return Math.Max(kMinScalePercent, Math.Min(kMaxScalePercent, percent));
		}

		/// <summary>
		/// Returns a recommended scale percentage for the given UI language code, or null if
		/// the default (100%, i.e. no special recommendation) is fine.
		/// </summary>
		public static int? GetRecommendedScalePercent(string languageCode)
		{
			return languageCode != null &&
				s_recommendedScalePercentByLanguage.TryGetValue(languageCode, out var percent)
				? percent
				: (int?)null;
		}

		/// <summary>
		/// Returns the working-language font to display at the given UI text scale: the
		/// project's baseline working-language font with only its size scaled. Family, style,
		/// and unit always stay exactly as the project defines them, because working-language
		/// content must render in the font chosen for that language regardless of the UI
		/// language's font.
		/// </summary>
		public static Font GetScaledWorkingLanguageFont(Font baseline, int scalePercent)
		{
			if (baseline == null)
				return null;

			return new Font(baseline.FontFamily,
				baseline.Size * ClampScalePercent(scalePercent) / 100f,
				baseline.Style, baseline.Unit);
		}

		/// <summary>
		/// Returns the working-language font for the given baseline at the current
		/// <c>Settings.Default.UITextScale</c>. Every control that shows
		/// working-language content (editor text boxes and FieldsValuesGrid cells alike) gets
		/// its font from here, so they all render at the same size. The result is cached and
		/// shared until the baseline or the scale changes; a superseded Font is left for the
		/// garbage collector rather than disposed, since controls may still be displaying it
		/// until they are given the new one.
		/// </summary>
		public static Font GetScaledWorkingLanguageFont(Font baseline)
		{
			if (baseline == null)
				return null;

			var scalePercent = ClampScalePercent(Properties.Settings.Default.UITextScale);
			lock (s_scaledWorkingLanguageFontLock)
			{
				if (s_scaledWorkingLanguageFont == null ||
					!ReferenceEquals(s_scaledWorkingLanguageFontBaseline, baseline) ||
					s_scaledWorkingLanguageFontPercent != scalePercent)
				{
					Font scaled;
					try
					{
						scaled = GetScaledWorkingLanguageFont(baseline, scalePercent);
					}
					catch (Exception e)
					{
						ErrorReport.ReportNonFatalException(e);
						scaled = baseline;
					}

					s_scaledWorkingLanguageFontBaseline = baseline;
					s_scaledWorkingLanguageFontPercent = scalePercent;
					s_scaledWorkingLanguageFont = scaled;
				}

				return s_scaledWorkingLanguageFont;
			}
		}

		/// <summary>
		/// Marks a control whose font is assigned and kept at the current scale by its own
		/// code (e.g. a working-language text box, which its editor gives an already-scaled
		/// working-language font). Scaling passes leave such a control's font untouched:
		/// rescaling an already-scaled font would compound the scale factor, and its cached
		/// "original" font would not be the working-language baseline anyway.
		/// </summary>
		public static void ExcludeFromUIFontScaling(Control control)
		{
			if (!s_controlsExcludedFromUIFontScaling.TryGetValue(control, out _))
				s_controlsExcludedFromUIFontScaling.Add(control, new object());
		}

		private static bool IsExcludedFromUIFontScaling(Control control) =>
			s_controlsExcludedFromUIFontScaling.TryGetValue(control, out _);

		public static Font GetOriginalFont(Control control)
		{
			if (s_origFonts.TryGetValue(control, out var font))
			{
				// Ensure the font hasn't changed out from under us; otherwise, reset. Mismatches
				// here can happen, for example, when the UI language dialog is closed with OK
				// but the scale percentage didn't actually change (NotifyScaleChanged still
				// fires, re-triggering a scaling pass over controls that were already scaled),
				// or when a control's baseline font genuinely differs from an ancestor's (e.g.
				// a mixed-family Designer file). In either case, the control's current font is
				// simply the new baseline to track.
				//
				// Deliberately not using SIL.Windows.Forms.FontHelper.AreFontsSame here: it also
				// compares Size, which this check must not do. A control several levels below an
				// ambiently-scaled ancestor reads that ancestor's Font -- already resized to the
				// new scale -- partway through the very same recursive pass that's in the middle
				// of resizing it (the ancestor's own ApplyFontCore call already ran by the time
				// this descendant's is reached), so its Size legitimately differs from its cached
				// original even though nothing about its baseline actually changed. Comparing
				// Size here would misread that as a changed baseline and re-cache the
				// already-scaled font as a fresh one, compounding the scale with every ambient
				// level (confirmed by ScaleUIFont_ButtonAmbientlyInheritsFontThroughMultipleUnsetLevels_DoesNotCompoundScale).
				if (control.Font.FontFamily.Equals(font.FontFamily) &&
					control.Font.Style == font.Style &&
					control.Font.Unit == font.Unit)
				{
					return font;
				}

				s_origFonts.Remove(control);
			}

			s_origFonts.Add(control, control.Font);
				
			return control.Font;
		}

		private static Font GetScaledFont(this Control control, out float scale)
		{
			scale = ClampScalePercent(Properties.Settings.Default.UITextScale) / 100f;

			// NOTE: Deliberately not short-circuiting when scale is ~1 (e.g. the UI language
			// setting changed and the saved UITextScale happens to be 100%). A control may
			// already have been scaled away from 1.0 by an earlier call and now needs to be
			// put back, so "target scale is 1" does not mean "there's nothing to do here."
			var origFont = GetOriginalFont(control);

			try
			{
				return new Font(origFont.FontFamily, origFont.Size * scale, origFont.Style, origFont.Unit);
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
				return null;
			}
		}

		public static void ScaleUIFont(this Control control)
		{
			if (IsExcludedFromUIFontScaling(control))
				return;

			var scaledFont = control.GetScaledFont(out var scale);
			if (scaledFont == null)
				return;

			// Suppresses actual window repainting (not just the layout engine) while the
			// recursive pass below touches many independently-laid-out nested containers, so a
			// correction made after the control is already visible (e.g. SayMoreFormBase's
			// Form.Shown safety net, or a live scale change while a dialog is open) applies as
			// one batched repaint instead of a visible per-control cascade. Only done for a
			// control that's actually visible on screen: sending WM_SETREDRAW to a hidden window
			// is a documented Win32 quirk that can make it visible again as a side effect (e.g.
			// StagesControlToolTip, a long-lived hidden form reused between hovers, would
			// reappear as a stale "ghost" the next time any open dialog's scale changes).
			var suppressRepaint = control.IsHandleCreated && control.Visible;
			if (suppressRepaint)
				control.SetWindowRedraw(false);
			try
			{
				control.ApplyFontRecursively(scaledFont, scale);
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
			}
			finally
			{
				if (suppressRepaint)
					control.SetWindowRedraw(true);
			}
		}

		public static void ApplyFontRecursively(this Control control, Font newFont, float scale)
		{
			// A control that has never had its own Font explicitly set ambient-tracks its
			// parent's Font, so reading it as that control's "original" is only valid before
			// any ancestor in this same pass has already been rescaled. Caching every
			// descendant's original font up front, against the pre-scale tree, prevents an
			// ambient-tracking control several levels deep from picking up an ancestor's
			// already-scaled Font as its own baseline and having the scale factor compound
			// once per ambient level (e.g. scale^3 for a three-level chain).
			CacheOriginalFontsRecursively(control);
			control.ApplyFontCore(newFont, scale);
		}

		private static void CacheOriginalFontsRecursively(Control control)
		{
			GetOriginalFont(control);

			foreach (Control child in control.Controls)
				CacheOriginalFontsRecursively(child);
		}

		private static void ApplyFontCore(this Control control, Font newFont, float scale)
		{
			if (!scale.Equals(1f) && control is Form formScale)
			{
				// Prevent auto-scaling from interfering
				formScale.AutoScaleMode = AutoScaleMode.None;
			}
			
			// Always rebuilt from this control's own cached original -- family, style, and
			// unit -- rather than reusing newFont (computed from some ancestor's original). A
			// descendant's baseline font can legitimately differ from an ancestor's; reusing
			// newFont could silently apply the wrong family, re-express the same numeric size in
			// the wrong unit, etc.
			var origFont = GetOriginalFont(control);
			control.Font = new Font(origFont.FontFamily, origFont.Size * scale, origFont.Style, origFont.Unit);

			ForceResizeIfAutoSizeWithMinimumSize(control);

			foreach (Control child in control.Controls)
			{
				if (IsExcludedFromUIFontScaling(child))
					continue;

				switch (child)
				{
					// Image and BackgroundImage are intentionally left unscaled by this walk:
					// resizing a raster image to match a font-scale factor produces visible
					// pixelation. PictureBox and other image-only controls are not matched here
					// for that reason; a control that needs more room for a fixed-size image
					// alongside its scaled text (e.g. SessionRecorderDlg's recording
					// ToolStripButtons) grows its own Size/container to fit both instead.
					//
					// Checked first: a composite control's own ApplyScale takes full
					// responsibility for its own state and descendants (e.g. ViewTabGroup's
					// TabFont, FieldsValuesGrid's _factoryFieldFont), so this must be matched
					// before any case below that would otherwise also structurally match it
					// (ViewTabGroup is a Panel; FieldsValuesGrid is a DataGridView) and handle
					// it with the wrong, generic logic instead.
					case IUiScalable scalable:
						scalable.ApplyScale(scale);
						break;
					// List of types we explicitly support
					case Label _:
					case Button _:
					case CheckBox _:
					case RadioButton _:
					case GroupBox _:
					case Panel _:
					case UserControl _:
						child.ApplyFontCore(newFont, scale);
						if (child.ContextMenuStrip != null)
						{
							CacheOriginalToolStripItemFontsRecursively(child.ContextMenuStrip.Items);
							ApplyFontToToolStripItems(child.ContextMenuStrip.Items,
								GetOriginalFont(child.ContextMenuStrip), scale);
						}
						break;
					case ComboBox combo:
						child.ApplyFontCore(newFont, scale);

						// A ComboBox's own rendered height always tracks its Font correctly (the
						// OS enforces PreferredHeight regardless of AutoSize), but when one sits
						// in a TableLayoutPanel row with SizeType.AutoSize, the panel's row-height
						// calculation only measures that correctly if AutoSize is also true here;
						// otherwise the row is left too short, clipping the taller combo box after
						// scaling.
						combo.AutoSize = true;

						// A ComboBox anchored Right (stretching horizontally to fill its
						// TableLayoutPanel cell, e.g. ProjectMetadataScreen's Country/Continent
						// fields) tracks whatever width the panel computes for that cell -- which
						// isn't self-determined by the combo at all. In a column sized by
						// SizeType.Percent, that computed width shrinks as neighboring
						// SizeType.AutoSize columns (e.g. bold field labels) grow during the very
						// same scaling pass, so the cell -- and the combo stretching to fill it --
						// can end up narrower than its own (correctly scaled, and now larger) Font
						// needs, cramping the displayed text. Above 100%, MinimumSize.Width is set
						// to the width the combo's own content needs at its current scaled Font
						// (see GetComboContentWidth), giving the layout engine a floor it honors
						// whenever the row/column has any room to spare, so the text keeps fitting
						// as a growing sibling column claims its share. A column squeezed so tight
						// that even this floor can't be honored is a container-width limit outside
						// what a per-combo floor can address. At or below 100%, the floor is
						// cleared: the layout-determined width is correct as-is there, and the
						// combo stays free to follow its container as the window is resized.
						if ((combo.Anchor & AnchorStyles.Right) != 0 || combo.Dock != DockStyle.None)
						{
							var floorWidth = scale > 1f ? GetComboContentWidth(combo) : 0;
							if (combo.MinimumSize.Width != floorWidth)
							{
								combo.MinimumSize = new Size(floorWidth, combo.MinimumSize.Height);
								ForceResizeIfAutoSizeWithMinimumSize(combo);
							}
						}

						int baselineDropDownWidth;
						if (!s_baselineDropDownWidths.TryGetValue(combo, out var baselineWidthBox))
						{
							baselineDropDownWidth = combo.DropDownWidth;
							s_baselineDropDownWidths.Add(combo, new StrongBox<int>(baselineDropDownWidth));
						}
						else if (baselineWidthBox.Value > combo.Width)
							baselineDropDownWidth = baselineWidthBox.Value;
						else
							baselineDropDownWidth = -1;
						if (baselineDropDownWidth > 1) // sanity check
							combo.DropDownWidth = (int)Math.Ceiling(baselineDropDownWidth * scale);

						// A ComboBox's dropdown items are plain objects rendered via ToString() or
						// owner-draw painting (DrawItem); WinForms never hosts them as child
						// Controls, so there's nothing here for a font-scaling pass to reach directly.

						break;
					case DataGridView dgv:
						// The grid's own Font cascades to any column/cell that doesn't set its
						// own style, and is also what code that paints directly onto the grid
						// (e.g. a drag-and-drop hint message) reads, since it has no per-cell
						// style of its own to pull from -- so it needs rescaling here too, not
						// just the column headers below.
						var origGridFont = GetOriginalFont(dgv);
						dgv.Font = new Font(origGridFont.FontFamily, origGridFont.Size * scale,
							origGridFont.Style, origGridFont.Unit);

						foreach (DataGridViewColumn col in dgv.Columns)
						{
							if (!s_origColumnHeaderFonts.TryGetValue(col, out var origHeaderFont))
							{
								// HeaderCell.Style.Font is null unless the column explicitly set
								// its own (e.g. TextAnnotationColumnWithMenu's intentionally bold
								// header); fall back to the grid's own cached original font so an
								// unstyled header still scales consistently with the rest of the
								// grid, instead of losing a column's own distinct styling to the
								// ancestor-derived newFont every time this case is reached.
								origHeaderFont = col.HeaderCell.Style.Font ?? origGridFont;
								s_origColumnHeaderFonts.Add(col, origHeaderFont);
							}

							col.HeaderCell.Style.Font = new Font(origHeaderFont.FontFamily,
								origHeaderFont.Size * scale, origHeaderFont.Style, origHeaderFont.Unit);
						}
						// REVIEW: recurse to apply to child controls within DataGridView (rare)?
						break;
					// REVIEW: no ListView/TreeView case. The one current usage
					// (FileLoadErrorsReportDlg.m_listFileErrors) shows untranslated exception
					// text, so scaling its font is questionable even though scaling its size for
					// visual consistency might be worth doing; see the REVIEW comment there.
					case ToolStrip toolStrip: // This also includes ToolStripDropDown
						// Every item's original font must be cached before the strip's own Font
						// is mutated below: an item with no explicit Font of its own ambiently
						// reads its owner strip's CURRENT Font, so caching after the mutation
						// would capture the already-scaled strip font as the item's "original"
						// and compound the scale on every pass from then on -- the same ambient-
						// tracking hazard CacheOriginalFontsRecursively exists to prevent for
						// ordinary Controls.
						CacheOriginalToolStripItemFontsRecursively(toolStrip.Items);

						// The strip's own Font (e.g. MenuStrip's) governs its layout/height
						// independent of its items' fonts; leaving it unscaled can leave the
						// strip too short to properly display the now-larger item text.
						var origStripFont = GetOriginalFont(toolStrip);
						toolStrip.Font = new Font(origStripFont.FontFamily,
							origStripFont.Size * scale, origStripFont.Style, origStripFont.Unit);
						ApplyFontToToolStripItems(toolStrip.Items, origStripFont, scale);
						break;
					case SplitContainer split:
						// SplitContainer doesn't derive from Panel, so its panels (e.g. a
						// left-hand sidebar) would otherwise be silently skipped.
						split.Panel1.ApplyFontCore(newFont, scale);
						split.Panel2.ApplyFontCore(newFont, scale);
						break;
				}
			}

			if (scale.Equals(1f) && control is Form formNormal)
			{
				// Use normal OS-based scaling
				formNormal.AutoScaleMode = AutoScaleMode.Font;
			}
		}

		/// <summary>
		/// Room a ComboBox needs beyond its measured text: its dropdown arrow and borders. Same
		/// allowance ProjectAccessScreen.SizeProtocolsComboBox and
		/// ProjectMetadataScreen.SizeContinentComboBox use when sizing a combo to its items.
		/// </summary>
		private const int kComboBoxChromeWidth = 30;

		/// <summary>
		/// The width a ComboBox needs to show its widest item (or, for a combo with no items,
		/// its current text) in full at its current Font, including its own chrome. Returns 0
		/// when there is no text to measure at all, so an empty combo gets no floor.
		/// </summary>
		private static int GetComboContentWidth(ComboBox combo)
		{
			var maxTextWidth = 0;
			if (combo.Items.Count > 0)
			{
				foreach (var item in combo.Items)
				{
					maxTextWidth = Math.Max(maxTextWidth,
						TextRenderer.MeasureText(combo.GetItemText(item), combo.Font).Width);
				}
			}
			else if (!string.IsNullOrEmpty(combo.Text))
				maxTextWidth = TextRenderer.MeasureText(combo.Text, combo.Font).Width;

			return maxTextWidth > 0 ? maxTextWidth + kComboBoxChromeWidth : 0;
		}

		/// <summary>
		/// A WinForms quirk: a Control with both AutoSize=true and an explicit MinimumSize
		/// (common for buttons throughout the app, e.g. SessionsListScreen's "New From
		/// Recording..." button) grows correctly as its font grows, but once its preferred
		/// size has exceeded MinimumSize, shrinking the font back down doesn't reliably shrink
		/// the control back down on its own -- its cached preferred size seems to get stuck.
		/// Toggling AutoSize off and back on forces WinForms to recompute it from scratch.
		/// </summary>
		private static void ForceResizeIfAutoSizeWithMinimumSize(Control control)
		{
			if (control.AutoSize && control.MinimumSize != Size.Empty)
			{
				control.AutoSize = false;
				control.AutoSize = true;
			}
		}

		/// <summary>
		/// Caches every item's (and, recursively, every submenu item's) current Font as its
		/// original baseline, before anything in the same pass has a chance to mutate the Font
		/// any of them would ambiently read. ToolStripItem.Font is never null -- an item with no
		/// explicit Font of its own returns its owner strip's current Font -- so this must run
		/// before the owner strip's own Font is changed, or an unstyled item would cache the
		/// already-scaled strip font as its "original" and compound the scale on every pass from
		/// then on, including back down to 100%.
		/// </summary>
		private static void CacheOriginalToolStripItemFontsRecursively(ToolStripItemCollection items)
		{
			foreach (ToolStripItem item in items)
			{
				if (!s_origToolStripItemFonts.TryGetValue(item, out _))
					s_origToolStripItemFonts.Add(item, item.Font);

				if (item is ToolStripDropDownItem dropDownItem && dropDownItem.HasDropDownItems)
					CacheOriginalToolStripItemFontsRecursively(dropDownItem.DropDownItems);
			}
		}

		/// <summary>
		/// Requires CacheOriginalToolStripItemFontsRecursively to have already run over the same
		/// items (and their submenu items) in this pass, before anything mutated a Font any of
		/// them could ambiently read.
		/// </summary>
		private static void ApplyFontToToolStripItems(ToolStripItemCollection items, Font ancestorOrigFont, float scale)
		{
			foreach (ToolStripItem item in items)
			{
				if (!s_origToolStripItemFonts.TryGetValue(item, out var origItemFont))
					origItemFont = ancestorOrigFont; // Defensive: should already be cached.

				item.Font = new Font(origItemFont.FontFamily, origItemFont.Size * scale,
					origItemFont.Style, origItemFont.Unit);

				if (item is ToolStripDropDownItem dropDownItem && dropDownItem.HasDropDownItems)
					ApplyFontToToolStripItems(dropDownItem.DropDownItems, ancestorOrigFont, scale);
			}
		}

		internal static void ShowScaledContextMenuStrip(this ContextMenuStrip contextMenu, Point location)
		{
			contextMenu.GetScaledFont(out var scale);
			CacheOriginalToolStripItemFontsRecursively(contextMenu.Items);
			ApplyFontToToolStripItems(contextMenu.Items, contextMenu.Font, scale);
			contextMenu.Show(location);
		}
	}
}
