using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Localization;
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
		public void HandleProgramaticValueChange()
		{
			if (SegmentChangedAction != null)
				SegmentChangedAction();
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleGridCellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			Segment segment;

			if (e.ColumnIndex != Index || !Tier.TryGetSegment(e.RowIndex, out segment))
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
				string dataFormat;
				var data = Tier.GetTierClipboardData(out dataFormat);
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