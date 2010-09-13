using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMore.UI.Overview.Statistics
{
	public class StatisticsViewModel :IDisposable
	{
		private readonly ElementRepository<Person> _people;
		private readonly ElementRepository<Event> _events;
		private readonly IEnumerable<ComponentRole> _componentRoles;
		private AudioVideoDataGatherer _backgroundStatisticsGather;

		public StatisticsViewModel(ElementRepository<Person> people,
									ElementRepository<Event> events,
									IEnumerable<ComponentRole> componentRoles,
									AudioVideoDataGatherer backgroundStatisticsMananager)
		{
			_people = people;
			_events = events;
			_componentRoles = componentRoles;
			_backgroundStatisticsGather = backgroundStatisticsMananager;
			_backgroundStatisticsGather.NewDataAvailable += OnNewStatistics;

		}

		public string Status
		{
			get{ return _backgroundStatisticsGather.Status;}
		}

		public bool UIUpdateNeeded { get; set; }

		public IEnumerable<KeyValuePair<string,string>> GetStatisticPairs()
		{
			yield return new KeyValuePair<string, string>("Events", _events.AllItems.Count().ToString());
			yield return new KeyValuePair<string, string>("People", _people.AllItems.Count().ToString());
			yield return new KeyValuePair<string, string>("", "");

			foreach (var role in _componentRoles.Where(def => def.MeasurementType == ComponentRole.MeasurementTypes.Time))
			{
				 yield return new KeyValuePair<string, string>(role.Name,
					 GetRecordingDurations(role).ToString());
			}

			foreach (var role in _componentRoles.Where(def => def.MeasurementType == ComponentRole.MeasurementTypes.Time))
			{
				int megabytes = GetMegabytes(role);
				string value;
				if (megabytes > 1000) //review: is a gigabyte 1000, or 1024 megabytes?
				{
					value = (megabytes / 1000.0).ToString("N", CultureInfo.InvariantCulture) + " Gigabytes";
				}
				else if (megabytes == 0)
				{
					value = "---";
				}
				else
				{
					value = megabytes.ToString("###", CultureInfo.InvariantCulture) + " Megabytes";
				}

				yield return new KeyValuePair<string, string>(role.Name, value);
			}
		}

		private int GetMegabytes(ComponentRole role)
		{
			long bytes = 0;
			foreach(AudioVideoFileStatistics stat in _backgroundStatisticsGather.GetAllFileData())
			{
				if (role.IsMatch(stat.Path))
				{
					bytes += stat.LengthInBytes;
				}
			}
			return (int)((float)bytes / (1024 * 1024));
		}

		public TimeSpan GetRecordingDurations(ComponentRole role)
		{
			var total = new TimeSpan(0);
			foreach (AudioVideoFileStatistics stat in _backgroundStatisticsGather.GetAllFileData())
			{
				if (role.IsMatch(stat.Path))
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
			_backgroundStatisticsGather.NewDataAvailable -= OnNewStatistics;
		}

		void OnNewStatistics(object sender, EventArgs e)
		{
			UIUpdateNeeded = true;
		}
	}
}
