using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIL.Sponge.Model;

namespace SIL.Sponge.Views.Overview.Statistics
{
	public class StatisticsViewModel
	{
		private readonly SpongeProject _project;

		public StatisticsViewModel(SpongeProject project)
		{
			_project = project;
		}

		public IEnumerable<KeyValuePair<string,string>> GetPairs()
		{
			yield return new KeyValuePair<string, string>("Sessions", _project.Sessions.Count.ToString());
			yield return new KeyValuePair<string, string>("People", _project.People.Count.ToString());
			yield return new KeyValuePair<string, string>("", "");

			//TODO: this will need to be some kind of background operation

			foreach (var definition in SessionComponentDefinition.CreateHardCodedDefinitions().Where(def=>def.MeasurementType == SessionComponentDefinition.MeasurementTypes.Time))
			{
				 yield return new KeyValuePair<string, string>(definition.Name,
					 GetRecordingDurations(definition).ToString());
			}
		}

		public TimeSpan GetRecordingDurations(SessionComponentDefinition  definition)
		{
			var total = new TimeSpan(0);
			foreach (var session in _project.Sessions)
				total += session.GetDurationOfMatchingFile(definition.MatchFilter);

			return total;
		}
	}
}
