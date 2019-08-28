using System;
using System.Linq;
using System.Collections.Generic;
using L10NSharp;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.UI.Charts;

namespace SayMore.UI.Overview.Statistics
{
	public class StatisticsViewModel : IDisposable
	{
		public event EventHandler NewStatisticsAvailable;
		public event EventHandler FinishedGatheringStatisticsForAllFiles;

		private readonly IEnumerable<ComponentRole> _componentRoles;
		private readonly AudioVideoDataGatherer _backgroundStatisticsGather;
		protected HTMLChartBuilder _chartBuilder;

		public PersonInformant PersonInformant { get; protected set; }
		public SessionWorkflowInformant SessionInformant { get; protected set; }
		public string ProjectName { get; protected set; }
		public string ProjectPath { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public StatisticsViewModel(Project project, PersonInformant personInformant,
			SessionWorkflowInformant sessionInformant, IEnumerable<ComponentRole> componentRoles,
			AudioVideoDataGatherer backgroundStatisticsMananager)
		{
			ProjectName = (project == null ? string.Empty : project.Name);
			ProjectPath = (project == null ? string.Empty : project.FolderPath);
			PersonInformant = personInformant;
			SessionInformant = sessionInformant;
			_componentRoles = componentRoles;
			_backgroundStatisticsGather = backgroundStatisticsMananager;
			_backgroundStatisticsGather.NewDataAvailable += HandleNewStatistics;
			_backgroundStatisticsGather.FinishedProcessingAllFiles += HandleFinishedGatheringStatisticsForAllFiles;

			_chartBuilder = new HTMLChartBuilder(this);
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			_backgroundStatisticsGather.NewDataAvailable -= HandleNewStatistics;
			_backgroundStatisticsGather.FinishedProcessingAllFiles -= HandleFinishedGatheringStatisticsForAllFiles;
		}

		/// ------------------------------------------------------------------------------------
		public string Status
		{
			get { return _backgroundStatisticsGather.Status; }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsDataUpToDate
		{
			get { return _backgroundStatisticsGather.DataUpToDate; }
		}

		/// ------------------------------------------------------------------------------------
		public bool IsBusy
		{
			get { return _backgroundStatisticsGather.Busy; }
		}

		/// ------------------------------------------------------------------------------------
		public string HTMLString
		{
			get { return _chartBuilder.GetStatisticsCharts(); }
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<KeyValuePair<string, string>> GetElementStatisticsPairs()
		{
			var sessionsLabel = LocalizationManager.GetString("ProgressView.SessionsLabel", "Sessions:");
			var peopleLabel = LocalizationManager.GetString("ProgressView.PeopleLabel", "People:");

			yield return new KeyValuePair<string, string>(sessionsLabel, SessionInformant.NumberOfSessions.ToString());
			yield return new KeyValuePair<string, string>(peopleLabel, PersonInformant.NumberOfPeople.ToString());
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ComponentRoleStatistics> GetComponentRoleStatisticsPairs()
		{
			foreach (var role in _componentRoles.Where(def => def.MeasurementType == ComponentRole.MeasurementTypes.Time))
			{
				long bytes = GetTotalComponentRoleFileSizes(role);
				var size = (bytes == 0 ? "---" : ComponentFile.GetDisplayableFileSize(bytes, false));

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
			// do not include the generated oral annotation file in the calculations
			return _backgroundStatisticsGather.GetAllFileData()
				.Where(i => !i.MediaFilePath.EndsWith(Settings.Default.OralAnnotationGeneratedFileSuffix))
				.Where(info => role.IsMatch(info.MediaFilePath))
				.Sum(info => info.LengthInBytes);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetRecordingDurations(ComponentRole role)
		{
			// do not include the generated oral annotation file in the calculations
			var total = _backgroundStatisticsGather.GetAllFileData()
				.Where(i => !i.MediaFilePath.EndsWith(Settings.Default.OralAnnotationGeneratedFileSuffix))
				.Where(info => role.IsMatch(info.MediaFilePath))
				.Aggregate(TimeSpan.Zero, (current, info) => current + info.Duration);

			// Trim off the milliseconds so it doesn't get too geeky
			return new TimeSpan(total.Hours, total.Minutes, total.Seconds);
		}

		/// ------------------------------------------------------------------------------------
		public void Refresh()
		{
			_backgroundStatisticsGather.Restart();
		}

		/// ------------------------------------------------------------------------------------
		void HandleNewStatistics(object sender, EventArgs e)
		{
			if (NewStatisticsAvailable != null)
				NewStatisticsAvailable(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		void HandleFinishedGatheringStatisticsForAllFiles(object sender, EventArgs e)
		{
			if (FinishedGatheringStatisticsForAllFiles != null)
				FinishedGatheringStatisticsForAllFiles(this, EventArgs.Empty);
		}
	}

	public class ComponentRoleStatistics
	{
		public string Name { get; set; }
		public string Length { get; set; }
		public string Size { get; set; }
	}
}