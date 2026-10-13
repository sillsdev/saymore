using SIL.Reporting;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SayMore.Utilities
{
	internal static class UITextScaler
	{
		private static readonly ConditionalWeakTable<Control, Font> s_origFonts = new ConditionalWeakTable<Control, Font>();

		public static Font GetOriginalFont(Control control)
		{
			if (s_origFonts.TryGetValue(control, out var font))
			{
				// Ensure that the font family and style are still the same; otherwise, reset
				if (control.Font.FontFamily.Equals(font.FontFamily) &&
					control.Font.Style == font.Style)
				{
					return font;
				}

				Debug.Fail("Not sure, but I think this should never happen");
				s_origFonts.Remove(control);
			}

			s_origFonts.Add(control, control.Font);
				
			return control.Font;
		}

		private static Font GetScaledFont(this Control control, out float origSize, out float scale)
		{
			scale = Properties.Settings.Default.UITextScale / 100f;

			if (Math.Abs(scale - 1) < 0.001)
			{
				origSize = control.Font.Size; // Could just return 0; should be ignored.
				return null;
			}

			var font = control.Font;
			origSize = GetOriginalFont(control).Size;

			try
			{
				return new Font(font.FontFamily, origSize * scale, font.Style);
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
				return null;
			}
		}

		public static void ScaleUIFont(this Control control)
		{
			var scaledFont = control.GetScaledFont(out var origSize, out var scale);
			if (scaledFont == null)
				return;

			try
			{
				control.ApplyFontRecursively(scaledFont, origSize, scale);
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
			}
		}

		public static void ApplyFontRecursively(this Control control, Font newFont, float origSize, float scale)
		{
			if (!scale.Equals(1f) && control is Form formScale)
			{
				// Prevent auto-scaling from interfering
				formScale.AutoScaleMode = AutoScaleMode.None;
			}
			
			if (!string.IsNullOrEmpty(control.Text))
			{
				var origFont = GetOriginalFont(control);
				var ctrlFont = control.Font;
				if (origFont.FontFamily.Equals(newFont.FontFamily) &&
				    Math.Abs(origSize - origFont.Size) < 0.001 &&
				    origFont.Style == newFont.Style)
				{
					control.Font = newFont;
				}
				else
				{
					// Occasionally, the control's font family may not match the new font's family.
					// This should be rare and will probably usually be a one-off situation, so we
					// will recurse using the new font from the parent.
					control.Font = new Font(ctrlFont.FontFamily, origFont.Size * scale, ctrlFont.Style);
				}
			}

			foreach (Control child in control.Controls)
			{
				switch (child)
				{
					// List of types we explicitly support
					case Label _:
					case Button _:
					case CheckBox _:
					case RadioButton _:
					case GroupBox _:
					case Panel _:
					case UserControl _:
						child.ApplyFontRecursively(newFont, origSize, scale);
						if (control.ContextMenuStrip != null)
							ApplyFontToToolStripItems(control.ContextMenuStrip.Items, newFont);
						break;
					case ComboBox combo:
						child.ApplyFontRecursively(newFont, origSize, scale);
						int baselineDropDownWidth = -1;
						if (combo.Tag == null)
							combo.Tag = baselineDropDownWidth = combo.DropDownWidth;
						else if (combo.Tag is int baselineWidth && baselineWidth > combo.Width)
							baselineDropDownWidth = baselineWidth;
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
						foreach (DataGridViewColumn col in dgv.Columns)
							col.HeaderCell.Style.Font = newFont;
						// REVIEW: recurse to apply to child controls within DataGridView (rare)?
						break;
					case ToolStrip toolStrip: // This also includes ToolStripDropDown
						ApplyFontToToolStripItems(toolStrip.Items, newFont);
						break;
				}
			}

			if (scale.Equals(1f) && control is Form formNormal)
			{
				// Use normal OS-based scaling
				formNormal.AutoScaleMode = AutoScaleMode.Font;
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
			ApplyFontToToolStripItems(contextMenu.Items, contextMenu.GetScaledFont(out _, out _));
			contextMenu.Show(location);
		}
	}
}
