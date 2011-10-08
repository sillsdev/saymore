using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public class TextAnnotationColumn : TierColumnBase
	{
		/// ------------------------------------------------------------------------------------
		public TextAnnotationColumn(ITier tier) : base(tier)
		{
			Debug.Assert(tier.DataType == TierType.Text);
			DefaultCellStyle.WrapMode = DataGridViewTriState.True;
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleGridCellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			ISegment segment;

			if (e.ColumnIndex != Index || !Tier.TryGetSegment(e.RowIndex, out segment))
				return;

			((ITextSegment)segment).SetText(e.Value as string);

			base.HandleGridCellValuePushed(sender, e);
		}

		/// ------------------------------------------------------------------------------------
		protected override void HandleGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			base.HandleGridCellValueNeeded(sender, e);

			if (e.ColumnIndex == Index)
				e.Value = ((ITextSegment)Tier.GetSegment(e.RowIndex)).GetText();
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ToolStripMenuItem> GetContextMenuCommands()
		{
			var fmt = Program.GetString("EventsView.Transcription.TextAnnotationEditor.CopyColumnTextToClipboardMenuText",
				"Copy {0} to clipboard", "Parameter is column (i.e. tier) name");

			string text = string.Format(fmt, HeaderText);

			yield return new ToolStripMenuItem(text, null, delegate
			{
				string dataFormat;
				var data = Tier.GetTierClipboardData(out dataFormat);
				Clipboard.SetData(dataFormat, data);
			});
		}
	}
}
