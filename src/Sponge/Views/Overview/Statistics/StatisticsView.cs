using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SIL.Sponge.Views.Overview.Statistics;

namespace SIL.Sponge.Views
{
	public partial class StatisticsView : UserControl
	{
		private readonly StatisticsViewModel _model;

		Font _headerFont = new Font(SystemFonts.MessageBoxFont.FontFamily, 12, FontStyle.Bold);

		public StatisticsView(StatisticsViewModel model)
		{
			_model = model;
			InitializeComponent();
		}

		private void StatisticsView_Load(object sender, EventArgs e)
		{
			UpdateDisplay();
		}

		private void UpdateDisplay()
		{
			_table.Controls.Clear();
			_table.RowCount = 0;
			_table.RowStyles.Clear();

			foreach(KeyValuePair<string,string>  pair in _model.GetPairs())
			{
				AddRow(pair.Key, pair.Value);
			}
		}

		private void AddRow(string label, string amount)
		{
			_table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			_table.RowCount++;
			_table.Controls.Add(new Label() { Text = label, Font = _headerFont, Width = TextRenderer.MeasureText(label, _headerFont).Width+10 }, 0, _table.RowCount);
			_table.Controls.Add(new Label() { Text = amount, Font = _headerFont }, 1, _table.RowCount);
		}

		private void OnRefreshButtonClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			UpdateDisplay();
		}
	}
}
