using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SayMore.Model.Files.DataGathering
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A SessionOverviewStatistics is created for a single session. It then provides
	/// information to be used on an overview screen.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SessionOverviewStatistics
	{
		private string _sessionFolder;

		public string Id { get; private set; }
		public TimeSpan PrimaryRecordingDuration { get; private set; }
		public bool IsDescribed { get; private set; }
		public bool HasCarfulSpeech { get; private set; }
		public bool HasNationalTranslation { get; private set; }
		public bool HasVernacularTranscription { get; private set; }

		public SessionOverviewStatistics(string sessionFolder)
		{
			_sessionFolder = sessionFolder;



		}
	}
}
