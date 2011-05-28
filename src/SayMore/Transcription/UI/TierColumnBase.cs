using System.Drawing;
using System.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	public class TierColumnBase : DataGridViewTextBoxColumn
	{
		protected ITier _tier;

		/// ------------------------------------------------------------------------------------
		public TierColumnBase(ITier tier)
		{
			base.DefaultCellStyle.ForeColor = SystemColors.WindowText;
			base.DefaultCellStyle.BackColor = SystemColors.Window;
			base.CellTemplate.Style = DefaultCellStyle;

			_tier = tier;
			Name = _tier.DisplayName;
			HeaderText = _tier.DisplayName;
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			_tier = null;
			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public override object Clone()
		{
			var clone = base.Clone() as TierColumnBase;
			clone._tier = _tier;
			return clone;
		}
	}
}
