using System.Collections.Generic;
using System.Linq;

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
			base(rootDirectoryPath, allFileTypes.Where(t => t.IsAudioOrVideo),
				path => new MediaFileInfo(path))
		{
		}
	}
}