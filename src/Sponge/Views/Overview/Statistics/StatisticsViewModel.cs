using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SIL.Sponge.Model;

namespace SIL.Sponge.Views.Overview.Statistics
{
	public class StatisticsViewModel :IDisposable
	{
		private readonly SpongeProject _project;
		private BackgroundStatisticsMananager _backgroundStatisticsGather;

		public StatisticsViewModel(SpongeProject project, BackgroundStatisticsMananager backgroundStatisticsMananager)
		{
			_project = project;
			_backgroundStatisticsGather = backgroundStatisticsMananager;
			_backgroundStatisticsGather.NewStatistics += new EventHandler(OnNewStatistics);

		}

		public string Status
		{
			get{ return _backgroundStatisticsGather.Status;}
		}

		public bool UIUpdateNeeded { get; set; }

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

			foreach (var definition in SessionComponentDefinition.CreateHardCodedDefinitions().Where(def => def.MeasurementType == SessionComponentDefinition.MeasurementTypes.Time))
			{
				int megabytes = GetMegabytes(definition);
				string value;
				if(megabytes> 1000)//review: is a gigabyte 1000, or 1024 megabytes?
				{
					value = ((double)megabytes/1000.0).ToString("N",CultureInfo.InvariantCulture)+ " Gigabytes";
				}
				else if(megabytes == 0)
				{
					value = "---";
				}
				else
				{
					value = megabytes.ToString("###",CultureInfo.InvariantCulture)+ " Megabytes";
				}
				yield return new KeyValuePair<string, string>(definition.Name, value);

			}
		}

		private int GetMegabytes(SessionComponentDefinition definition)
		{
			long bytes=0;
			foreach(FileStatistics stat in _backgroundStatisticsGather.GetAllStatistics())
			{
				if(definition.MatchFilter(stat.Path))
				{
					bytes += stat.LengthInBytes;
				}
			}
			return (int)((float) bytes/(float) (1024*1024));
		}

		public TimeSpan GetRecordingDurations(SessionComponentDefinition  definition)
		{
			var total = new TimeSpan(0);
			foreach (FileStatistics stat in _backgroundStatisticsGather.GetAllStatistics())
			{
				if (definition.MatchFilter(stat.Path))
				{
					total += stat.Duration;
				}
			}

			return total;
		}

		public void Refresh()
		{
			_backgroundStatisticsGather.Restart();
		}

		public void Dispose()
		{
			_backgroundStatisticsGather.NewStatistics -= new EventHandler(OnNewStatistics);
		}

		void OnNewStatistics(object sender, EventArgs e)
		{
			UIUpdateNeeded = true;
		}
	}
}
