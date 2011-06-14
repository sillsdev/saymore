using System.Drawing;
using System.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public class TierColumnBase : DataGridViewTextBoxColumn
	{
		protected DataGridView _grid;

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
			UnsubscribeToGridEvents();
			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnDataGridViewChanged()
		{
			base.OnDataGridViewChanged();

			if (_grid != null)
				UnsubscribeToGridEvents();

			_grid = DataGridView;

			if (_grid != null)
				SubscribeToGridEvents();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void UnsubscribeToGridEvents()
		{
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void SubscribeToGridEvents()
		{
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetTier(ITier tier)
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
