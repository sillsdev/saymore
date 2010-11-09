using System.Collections.Generic;
using System.Linq;
using SayMore.UI.Utilities;

namespace SayMore.Model.Files.DataGathering
{
	public interface IProvideAudioVideoFileStatistics
	{
		MediaFileInfo GetFileData(string filePath);
	}

	public class AudioVideoDataGatherer :
		BackgroundFileProcessor<MediaFileInfo>, IProvideAudioVideoFileStatistics
	{
		public AudioVideoDataGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes) :
			base(rootDirectoryPath,
				from t in allFileTypes where t.IsAudioOrVideo select t,
				path => new MediaFileInfo(path))
		{
		}

		/// ------------------------------------------------------------------------------------
		public override void Start()
		{
			// This will force the gathering of A/V data to be done at least once, before
			// the program is fully up and running. That way fields are available to views
			// right away.
			ProcessAllFiles();
			_restartRequested = false;
			base.Start();
		}
	}
}