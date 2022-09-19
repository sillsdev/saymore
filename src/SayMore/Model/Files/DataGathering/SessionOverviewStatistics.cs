using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SIL.IO;
using FileUtils=SayMore.Media.TempFileUtils;

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
		private readonly IEnumerable<string> _sessionFiles;
		private readonly List<string> _recordingFileExtensions;

		public string Id { get; private set; }
		public TimeSpan PrimaryRecordingDuration { get; private set; }
		public bool IsDescribed { get; private set; }
		public bool HasNationalTranslation { get; private set; }

		/// ------------------------------------------------------------------------------------
		public SessionOverviewStatistics(string sessionFolder)
		{
			_sessionFolder = sessionFolder;

			_sessionFiles = Directory.GetFiles(sessionFolder, "*.*").Select(x => x.ToLowerInvariant());

			_recordingFileExtensions = new List<string>();
			_recordingFileExtensions.AddRange(FileUtils.AudioFileExtensions.Cast<string>());
			_recordingFileExtensions.AddRange(FileUtils.VideoFileExtensions.Cast<string>());
		}

		/// ------------------------------------------------------------------------------------
		public bool GetHasCarefulSpeech()
		{
			var filename = _sessionFiles.FirstOrDefault(x =>
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
			return _sessionFiles.FirstOrDefault(x => x.EndsWith("transcription.txt")) != null;
		}

		/// ------------------------------------------------------------------------------------
		public bool GetHasEnglishTranscription()
		{
			var filename = _sessionFiles.FirstOrDefault(x =>
			{
				var nameonly = Path.GetFileNameWithoutExtension(x);
				return nameonly.EndsWith("english");
			});

			return (filename != null);
		}
	}
}
