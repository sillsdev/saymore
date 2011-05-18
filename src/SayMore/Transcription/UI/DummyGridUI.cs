using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SayMore.Transcription.Model;

namespace SayMore.Transcription.UI
{
	/* This code is just here to help think out what the model interfaces (iSegRepo, ISegment, etc.) need to provide */
	class DummyGridUI
	{
		private readonly ISegmentRepository _repository;
		private readonly ColumnConfigurationList _columnConfigurationList;
		private Func<ICell, ColumnConfiguration, Control> _controlFactory;

		public DummyGridUI(ISegmentRepository repository, ColumnConfigurationList columnConfigurationList, Func<ICell, ColumnConfiguration, Control> controlFactory)
		{
			_repository = repository;
			_controlFactory = controlFactory;
			_columnConfigurationList = columnConfigurationList;
		}

		void GetControlAt(int x, int y)
		{
			var column = _columnConfigurationList.Columns[x];
			var tier = _repository.GetAllTiers().First(t => t.DisplayName == column.TierName);
			var segment = _repository.GetSegment(y);
			var cell = segment.GetCell(tier);


			//this factory takes the cell (which can tell about the type and tier characteristics) + the columnConfig (which might express preferences like font size or simple/vs. advanced),
			//and gives back a control we can stick in the grid
			var controlForThisCell = _controlFactory(cell, column);
		}
	}
}
