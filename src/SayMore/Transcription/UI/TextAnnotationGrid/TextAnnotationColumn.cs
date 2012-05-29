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
				e.Value = Tier.Segments.ElementAt(e.RowIndex).Text;
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
		}
	}
}
