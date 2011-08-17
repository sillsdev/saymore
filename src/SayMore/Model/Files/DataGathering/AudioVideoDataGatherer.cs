using System.Collections.Generic;
using System.Linq;
using SayMore.Properties;
using SayMore.Transcription.UI;

namespace SayMore.Model.Files.DataGathering
{
	/// ------------------------------------------------------------------------------------
	public interface IProvideAudioVideoFileStatistics
	{
		MediaFileInfo GetFileData(string filePath);
	}

	public class AudioVideoDataGatherer :
		BackgroundFileProcessor<MediaFileInfo>, IProvideAudioVideoFileStatistics
	{
		/// ------------------------------------------------------------------------------------
		public AudioVideoDataGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes) :
			base(rootDirectoryPath, allFileTypes.Where(t => t.IsAudioOrVideo),
				path => new MediaFileInfo(path))
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override void CollectDataForFile(string path)
		{
			// Don't collect info. on oral annotation files.
			if (path.EndsWith(Settings.Default.OralAnnotationGeneratedFileAffix) ||
				path.EndsWith("_" + OralAnnotationType.Translation) ||
				path.EndsWith("_" + OralAnnotationType.Careful))
			{
				return;
			}

			base.CollectDataForFile(path);
		}
	}
}