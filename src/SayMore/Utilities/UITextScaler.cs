using SIL.Reporting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using SayMore.UI.ProjectWindow;

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

		public static Font GetOriginalFont(Control control)
		{
			if (s_origFonts.TryGetValue(control, out var font))
			{
				// Ensure that the font family and style are still the same; otherwise, reset.
				// Mismatches here can happen, for example, when the UI language dialog is closed
				// with OK but the scale percentage didn't actually change (NotifyScaleChanged 
				// still fires, re-triggering a scaling pass over controls that were already
				// scaled), or when a control's baseline font family genuinely differs from an
				// ancestor's (e.g. a mixed-family Designer file). In either case, the control's
				// current font is simply the new baseline to track.
				if (control.Font.FontFamily.Equals(font.FontFamily) &&
					control.Font.Style == font.Style)
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
			var scaledFont = control.GetScaledFont(out var scale);
			if (scaledFont == null)
				return;

			try
			{
				control.ApplyFontRecursively(scaledFont, scale);
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
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
				switch (child)
				{
					// ViewTab maintains its own already-correctly-scaled font (see
					// ViewTabGroup.HandleScaleChanged/MakeScaledTabFont); since it's a Label, it
					// would otherwise also be reached by the Label case below, which would treat
					// that already-scaled font as a fresh 100% baseline and compound on top of it.
					case ViewTab _:
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
							ApplyFontToToolStripItems(child.ContextMenuStrip.Items, newFont);
						break;
					case ComboBox combo:
						child.ApplyFontCore(newFont, scale);

						// A ComboBox's own rendered height always tracks its Font correctly (the
						// OS enforces PreferredHeight regardless of AutoSize), but when one sits
						// in a TableLayoutPanel row with SizeType.AutoSize, the panel's row-height
						// calculation only measures that correctly if AutoSize is also true here;
						// otherwise the row is left too short, clipping the taller combo box after
						// scaling. This has no effect on width, since Dock/Anchor still govern that.
						combo.AutoSize = true;

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
						if (combo.DropDownStyle == ComboBoxStyle.DropDownList)
						{
							// Apply font to the items in the dropdown list
							foreach (var item in combo.Items)
							{
								if (item is Control itemControl)
									itemControl.Font = newFont;
							}
						}

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
						// The strip's own Font (e.g. MenuStrip's) governs its layout/height
						// independent of its items' fonts; leaving it unscaled can leave the
						// strip too short to properly display the now-larger item text.
						var origStripFont = GetOriginalFont(toolStrip);
						toolStrip.Font = new Font(origStripFont.FontFamily,
							origStripFont.Size * scale, origStripFont.Style, origStripFont.Unit);
						ApplyFontToToolStripItems(toolStrip.Items, newFont);
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

		private static void ApplyFontToToolStripItems(ToolStripItemCollection items, Font newFont)
		{
			foreach (ToolStripItem item in items)
			{
				item.Font = newFont;

				if (item is ToolStripDropDownItem dropDownItem && dropDownItem.HasDropDownItems)
					ApplyFontToToolStripItems(dropDownItem.DropDownItems, newFont);
			}
		}

		internal static void ShowScaledContextMenuStrip(this ContextMenuStrip contextMenu, Point location)
		{
			ApplyFontToToolStripItems(contextMenu.Items, contextMenu.GetScaledFont(out _));
			contextMenu.Show(location);
		}
	}
}
