using System;
using System.Linq;
using System.Collections.Generic;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.UI.Charts;

namespace SayMore.UI.Overview.Statistics
{
	public class StatisticsViewModel : IDisposable
	{
		private readonly IEnumerable<ComponentRole> _componentRoles;
		private readonly AudioVideoDataGatherer _backgroundStatisticsGather;
		protected HTMLChartBuilder _chartBuilder;

		public PersonInformant PersonInformant { get; protected set; }
		public EventWorkflowInformant EventInformant { get; protected set; }
		public bool UIUpdateNeeded { get; set; }

		/// ------------------------------------------------------------------------------------
		public StatisticsViewModel(PersonInformant personInformant,
			EventWorkflowInformant eventInformant, IEnumerable<ComponentRole> componentRoles,
			AudioVideoDataGatherer backgroundStatisticsMananager)
		{
			PersonInformant = personInformant;
			EventInformant = eventInformant;
			_componentRoles = componentRoles;
			_backgroundStatisticsGather = backgroundStatisticsMananager;
			_backgroundStatisticsGather.NewDataAvailable += OnNewStatistics;
			_chartBuilder = new HTMLChartBuilder(this);
		}

		/// ------------------------------------------------------------------------------------
		public string Status
		{
			get { return _backgroundStatisticsGather.Status; }
		}

		/// ------------------------------------------------------------------------------------
		public string HTMLString
		{
			get { return _chartBuilder.GetChart(); }
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<KeyValuePair<string, string>> GetElementStatisticsPairs()
		{
			yield return new KeyValuePair<string, string>("Events:", EventInformant.NumberOfEvents.ToString());
			yield return new KeyValuePair<string, string>("People:", PersonInformant.NumberOfPeople.ToString());
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ComponentRoleStatistics> GetComponentRoleStatisticsPairs()
		{
			foreach (var role in _componentRoles.Where(def => def.MeasurementType == ComponentRole.MeasurementTypes.Time))
			{
				long bytes = GetTotalComponentRoleFileSizes(role);
				var size = (bytes == 0 ? "---" : ComponentFile.GetDisplayableFileSize(bytes));

				yield return new ComponentRoleStatistics
				{
					Name = role.Name,
					Length = GetRecordingDurations(role).ToString(),
					Size = size
				};
			}
		}

		/// ------------------------------------------------------------------------------------
		private long GetTotalComponentRoleFileSizes(ComponentRole role)
		{
			long bytes = 0;
			foreach (MediaFileInfo info in _backgroundStatisticsGather.GetAllFileData())
			{
				if (role.IsMatch(info.MediaFilePath))
					bytes += info.LengthInBytes;
			}

			return bytes;
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetRecordingDurations(ComponentRole role)
		{
			var total = new TimeSpan(0);
			foreach (MediaFileInfo info in _backgroundStatisticsGather.GetAllFileData())
			{
				if (role.IsMatch(info.MediaFilePath))
					total += info.Duration;
			}

			// Trim off the milliseconds so it doesn't get too geeky
			return new TimeSpan(total.Hours, total.Minutes, total.Seconds);
		}

		/// ------------------------------------------------------------------------------------
		public void Refresh()
		{
			_backgroundStatisticsGather.Restart();
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			_backgroundStatisticsGather.NewDataAvailable -= OnNewStatistics;
		}

		/// ------------------------------------------------------------------------------------
		void OnNewStatistics(object sender, EventArgs e)
		{
			UIUpdateNeeded = true;
		}
	}

	public class ComponentRoleStatistics
	{
		public string Name { get; set; }
		public string Length { get; set; }
		public string Size { get; set; }
	}
}