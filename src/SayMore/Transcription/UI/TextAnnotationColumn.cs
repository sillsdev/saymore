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
			Name = "transcriptionColumn";
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
	}
}
