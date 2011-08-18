using System.Collections.Generic;
using System.IO;
using System.Linq;
using SayMore.Properties;

namespace SayMore.Model.Files.DataGathering
{
	/// ----------------------------------------------------------------------------------------
	public interface IProvideAudioVideoFileStatistics
	{
		MediaFileInfo GetFileData(string filePath);
	}

	/// ----------------------------------------------------------------------------------------
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
			path = path.ToLower();

			var isOralAnnotationSegmentFile =
				Path.GetDirectoryName(path).EndsWith(Settings.Default.OralAnnotationsFolderAffix.ToLower());

			// Don't collect info. on oral annotation files.
			if (!isOralAnnotationSegmentFile &&
				!path.EndsWith(Settings.Default.OralAnnotationGeneratedFileAffix.ToLower()))
			{
				base.CollectDataForFile(path);
			}
		}
	}
}