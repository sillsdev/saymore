using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SayMore.Transcription.Model
{
	class ColumnConfigurationList
	{
		public List<ColumnConfiguration> Columns;

	}

	class ColumnConfiguration
	{
		public string TierName { get; set; }
	}
}
