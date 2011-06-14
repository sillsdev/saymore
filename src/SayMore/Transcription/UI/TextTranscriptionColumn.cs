using System.Diagnostics;
using System.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public class TextTranscriptionColumn : TierColumnBase
	{
		/// ------------------------------------------------------------------------------------
		public TextTranscriptionColumn(ITier tier) : base(tier)
		{
			Debug.Assert(tier.DataType == TierType.Text);
		}

		/// ------------------------------------------------------------------------------------
		protected override void UnsubscribeToGridEvents()
		{
			_grid.CellValueNeeded -= HandleGridCellValueNeeded;
			_grid.CellValuePushed -= HandleGridCellValuePushed;
		}

		/// ------------------------------------------------------------------------------------
		protected override void SubscribeToGridEvents()
		{
			_grid.CellValueNeeded += HandleGridCellValueNeeded;
			_grid.CellValuePushed += HandleGridCellValuePushed;
		}

		/// ------------------------------------------------------------------------------------
		void HandleGridCellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.ColumnIndex == Index)
				((ITextSegment)Tier.GetSegment(e.RowIndex)).SetText(e.Value as string);
		}

		/// ------------------------------------------------------------------------------------
		void HandleGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.ColumnIndex == Index)
				e.Value = ((ITextSegment)Tier.GetSegment(e.RowIndex)).GetText();
		}
	}
}
