using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SayMore.Properties;

namespace SayMore.Model.Files.DataGathering
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A EventOverviewStatistics is created for a single event. It then provides
	/// information to be used on an overview screen.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class EventOverviewStatistics
	{
		private string _eventFolder;
		private readonly IEnumerable<string> _eventFiles;
		private readonly List<string> _recordingFileExtensions;

		public string Id { get; private set; }
		public TimeSpan PrimaryRecordingDuration { get; private set; }
		public bool IsDescribed { get; private set; }
		public bool HasNationalTranslation { get; private set; }

		/// ------------------------------------------------------------------------------------
		public EventOverviewStatistics(string eventFolder)
		{
			_eventFolder = eventFolder;

			_eventFiles = Directory.GetFiles(eventFolder, "*.*").Select(x => x.ToLowerInvariant());

			_recordingFileExtensions = new List<string>();
			_recordingFileExtensions.AddRange(Settings.Default.AudioFileExtensions.Cast<string>());
			_recordingFileExtensions.AddRange(Settings.Default.VideoFileExtensions.Cast<string>());
		}

		/// ------------------------------------------------------------------------------------
		public bool GetHasCarefulSpeech()
		{
			var filename = _eventFiles.FirstOrDefault(x =>
			{
				var ext = Path.GetExtension(x);
				if (!_recordingFileExtensions.Contains(ext))
					return false;

				var nameonly = Path.GetFileNameWithoutExtension(x);
				return nameonly.EndsWith("carefulspeech");
			});

			return (filename != null);
		}

		/// ------------------------------------------------------------------------------------
		public bool GetHasVernacularTranscription()
		{
			return _eventFiles.FirstOrDefault(x => x.EndsWith("transcription.txt")) != null;
		}

		/// ------------------------------------------------------------------------------------
		public bool GetHasEnglishTranscription()
		{
			var filename = _eventFiles.FirstOrDefault(x =>
			{
				var nameonly = Path.GetFileNameWithoutExtension(x);
				return nameonly.EndsWith("english");
			});

			return (filename != null);
		}
	}
}
