using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public class TextAnnotationColumn : TierColumnBase
	{
		/// ------------------------------------------------------------------------------------
		public TextAnnotationColumn(TierBase tier) : base(tier)
		{
			Debug.Assert(tier is TextTier);
			DefaultCellStyle.WrapMode = DataGridViewTriState.True;
			DefaultCellStyle.Font = Program.DialogFont;
			// This seems utterly pointless, but for some reason it fixes SP-566. Without this
			// code, the cell template's style has a font that is not the same as the default
			// column font. By doing this, the new DataGridViewTextBoxCell ends up with a null
			// font in its style. But just setting the style font to null for the existing one
			// makes the program crash. Of course, we have no concrete evidence that SP-566 has
			// anything to do with any of this font stuff.
			CellTemplate = new DataGridViewTextBoxCell();
		}

		/// ------------------------------------------------------------------------------------
		public void HandleProgrammaticValueChange()
		{
			SegmentChangedAction?.Invoke();
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleGridCellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			// REVIEW: The following line has been unchanged since it was originally written in 2011 by David Olson.
			// The second half of it seems to be just a sanity check. I can't think of any situation where it could
			// occur and not be indicative of a serious flaw in the program. I tried a conditional breakpoint to see
			// if I could find a situation in which it would return false, but found none. If it ever did fail, the
			// data the user entered would be lost, but they wouldn't get any feedback to that effect. This seems
			// bad.
			if (e.ColumnIndex != Index || !Tier.TryGetSegment(e.RowIndex, out var segment))
				return;

			segment.Text = e.Value as string;

			base.HandleGridCellValuePushed(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			base.HandleGridCellValueNeeded(sender, e);

			if (e.ColumnIndex == Index)
			{
				e.Value = _grid.GetIgnoreStateForRow(e.RowIndex) ?
					LocalizationManager.GetString("SessionsView.Transcription.TextAnnotationEditor.IgnoredSegmentIndicator", "Ignored") :
					Tier.Segments.ElementAt(e.RowIndex).Text;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ToolStripMenuItem> GetContextMenuCommands()
		{
			var fmt = LocalizationManager.GetString("SessionsView.Transcription.TextAnnotationEditor.CopyColumnTextToClipboardMenuText",
				"Copy {0} to clipboard", "Parameter is column (i.e. tier) name");

			string text = string.Format(fmt, HeaderText);

			yield return new ToolStripMenuItem(text, null, delegate
			{
				var data = Tier.GetTierClipboardData(out var dataFormat);
				Clipboard.SetData(dataFormat, data);
			});

			text = LocalizationManager.GetString("SessionsView.Transcription.TextAnnotationEditor.IgnoredMenuText",
				"&Ignored");

			var ignoredMenuItem = new ToolStripMenuItem(text, null);
			ignoredMenuItem.CheckOnClick = true;

			ignoredMenuItem.Click += (sender, e) =>
			{
				ToolStripMenuItem menu = (ToolStripMenuItem)sender;
				_grid.SetIgnoreStateForCurrentRow(menu.Checked);
			};

			ignoredMenuItem.OwnerChanged += (s, e) =>
			{
				if (ignoredMenuItem.Owner != null && ignoredMenuItem.Owner.IsDropDown)
					((ToolStripDropDown)ignoredMenuItem.Owner).Opened += (s1, e1) =>
						ignoredMenuItem.Checked = _grid.GetIgnoreStateForCurrentRow();
			};

			yield return ignoredMenuItem;
		}

		/// ------------------------------------------------------------------------------------
		public override void InitializeColumnContextMenu()
		{
			// no-op
		}
	}
}