using System;
using System.Linq;
using System.Collections.Generic;
using L10NSharp;
using SayMore.Media;
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
		private readonly AudioVideoDataGatherer _backgroundStatisticsGatherer;
		protected HTMLChartBuilder _chartBuilder;

		public PersonInformant PersonInformant { get; protected set; }
		public SessionWorkflowInformant SessionInformant { get; protected set; }
		public string ProjectName { get; protected set; }
		public string ProjectPath { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public StatisticsViewModel(Project project, PersonInformant personInformant,
			SessionWorkflowInformant sessionInformant, IEnumerable<ComponentRole> componentRoles,
			AudioVideoDataGatherer backgroundStatisticsManager)
		{
			ProjectName = project?.Name ?? string.Empty;
			ProjectPath = project?.FolderPath ?? string.Empty;
			PersonInformant = personInformant;
			SessionInformant = sessionInformant;
			_componentRoles = componentRoles;
			_backgroundStatisticsGatherer = backgroundStatisticsManager;
			_backgroundStatisticsGatherer.FinishedProcessingAllFiles += HandleFinishedGatheringStatisticsForAllFiles;
			_backgroundStatisticsGatherer.NewDataAvailable += HandleNewStatistics;

			project?.TrackStatistics(this);

			_chartBuilder = new HTMLChartBuilder(this);
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			_backgroundStatisticsGatherer.NewDataAvailable -= HandleNewStatistics;
			_backgroundStatisticsGatherer.FinishedProcessingAllFiles -= HandleFinishedGatheringStatisticsForAllFiles;
		}

		/// ------------------------------------------------------------------------------------
		public string Status => _backgroundStatisticsGatherer.Status;

		/// ------------------------------------------------------------------------------------
		public bool IsDataUpToDate => _backgroundStatisticsGatherer.DataUpToDate;

		/// ------------------------------------------------------------------------------------
		public bool IsBusy => _backgroundStatisticsGatherer.Busy;

		/// ------------------------------------------------------------------------------------
		public string HTMLString => _chartBuilder.GetStatisticsCharts();

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

				yield return new ComponentRoleStatistics
				{
					Name = role.Name,
					Length = GetRecordingDurations(role),
					Size = bytes
				};
			}
		}

		/// ------------------------------------------------------------------------------------
		private long GetTotalComponentRoleFileSizes(ComponentRole role)
		{
			// do not include the generated oral annotation file in the calculations
			return GetFilteredFileData(role).Sum(info => info.LengthInBytes);
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetRecordingDurations(ComponentRole role)
		{
			// do not include the generated oral annotation file in the calculations
			var total = GetFilteredFileData(role).Aggregate(TimeSpan.Zero, (current, info) => current + info.Duration);

			// Trim off the milliseconds so it doesn't get too geeky
			return new TimeSpan(total.Hours, total.Minutes, total.Seconds);
		}

		private IEnumerable<MediaFileInfo> GetFilteredFileData(ComponentRole role)
		{
			var comparer = new SourceAndStandardAudioCoalescingComparer();
			// SP-2171: i.MediaFilePath will be empty if the file is zero length (see MediaFileInfo.GetInfo()). This happens often with the generated oral annotation file.
			return _backgroundStatisticsGatherer.GetAllFileData()
				.Where(i => !string.IsNullOrEmpty(i.MediaFilePath) && !i.MediaFilePath.EndsWith(Settings.Default.OralAnnotationGeneratedFileSuffix) &&
					role.IsMatch(i.MediaFilePath))
				.Distinct(comparer);
		}

		/// ------------------------------------------------------------------------------------
		public void Refresh()
		{
			_backgroundStatisticsGatherer.Restart();
		}

		/// ------------------------------------------------------------------------------------
		void HandleNewStatistics(object sender, EventArgs e)
		{
			NewStatisticsAvailable?.Invoke(this, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		void HandleFinishedGatheringStatisticsForAllFiles(object sender, EventArgs e)
		{
			FinishedGatheringStatisticsForAllFiles?.Invoke(this, EventArgs.Empty);
		}
	}

	public class ComponentRoleStatistics
	{
		public string Name { get; set; }
		public TimeSpan Length { get; set; }
		public long Size { get; set; }
	}
}