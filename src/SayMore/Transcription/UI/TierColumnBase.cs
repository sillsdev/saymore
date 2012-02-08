using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public class TierColumnBase : DataGridViewColumn
	{
		protected TextAnnotationEditorGrid _grid;

		public Action SegmentChangedAction;
		public TierBase Tier { get; private set; }

		/// ------------------------------------------------------------------------------------
		public TierColumnBase(DataGridViewCell cellTemplate, TierBase tier)
			: base(cellTemplate)
		{
			DefaultCellStyle.ForeColor = SystemColors.WindowText;
			DefaultCellStyle.BackColor = SystemColors.Window;
			CellTemplate.Style = DefaultCellStyle;
			SetTier(tier);
		}

		/// ------------------------------------------------------------------------------------
		public TierColumnBase(TierBase tier) : this(new DataGridViewTextBoxCell(), tier)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override void Dispose(bool disposing)
		{
			Tier = null;

			if (_grid != null)
				UnsubscribeToGridEvents();

			base.Dispose(disposing);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnDataGridViewChanged()
		{
			base.OnDataGridViewChanged();

			if (_grid != null)
				UnsubscribeToGridEvents();

			_grid = DataGridView as TextAnnotationEditorGrid;

			if (_grid != null)
				SubscribeToGridEvents();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void UnsubscribeToGridEvents()
		{
			_grid.CellValueNeeded -= HandleGridCellValueNeeded;
			_grid.CellValuePushed -= HandleGridCellValuePushed;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void SubscribeToGridEvents()
		{
			_grid.CellValueNeeded += HandleGridCellValueNeeded;
			_grid.CellValuePushed += HandleGridCellValuePushed;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleGridCellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			if (SegmentChangedAction != null)
				SegmentChangedAction();
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void HandleGridCellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
		}

		/// ------------------------------------------------------------------------------------
		public virtual void SetTier(TierBase tier)
		{
			Tier = tier;
			Name = Tier.DisplayName.Replace(" ", string.Empty);
			HeaderText = Tier.DisplayName;
		}

		/// ------------------------------------------------------------------------------------
		public override object Clone()
		{
			var clone = base.Clone() as TierColumnBase;
			clone.Tier = Tier;
			return clone;
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ToolStripMenuItem> GetContextMenuCommands()
		{
			return new ToolStripMenuItem[] { };
		}
	}
}
