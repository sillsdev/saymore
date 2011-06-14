using System.Drawing;
using System.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	public class TierColumnBase : DataGridViewTextBoxColumn
	{
		public ITier Tier { get; private set; }

		/// ------------------------------------------------------------------------------------
		public TierColumnBase()
		{
			DefaultCellStyle.ForeColor = SystemColors.WindowText;
			DefaultCellStyle.BackColor = SystemColors.Window;
			CellTemplate.Style = DefaultCellStyle;
		}

		/// ------------------------------------------------------------------------------------
		public TierColumnBase(ITier tier)
		{
			SetTier(tier);
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			Tier = null;
			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		public void SetTier(ITier tier)
		{
			Tier = tier;
			Name = Tier.DisplayName;
			HeaderText = Tier.DisplayName;
		}

		/// ------------------------------------------------------------------------------------
		public override object Clone()
		{
			var clone = base.Clone() as TierColumnBase;
			clone.Tier = Tier;
			return clone;
		}
	}
}
