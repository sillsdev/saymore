using System.Collections.Generic;
using System.Linq;

namespace SayMore.Model.Files.DataGathering
{
	public interface IProvideAudioVideoFileStatistics
	{
		AudioVideoFileStatistics GetFileData(string filePath);
	}

	public class AudioVideoDataGatherer :
		BackgroundFileProcessor<AudioVideoFileStatistics>, IProvideAudioVideoFileStatistics
	{
		public AudioVideoDataGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes) :
			base(rootDirectoryPath,
				from t in allFileTypes where t.IsAudioOrVideo select t,
				path => new AudioVideoFileStatistics(path))
		{
		}
	}
}