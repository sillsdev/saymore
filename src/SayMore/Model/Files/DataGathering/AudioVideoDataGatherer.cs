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
		protected override bool GetDoIncludeFile(string path)
		{
			// Don't collect info. on oral annotation segment files.
			var isOralAnnotationSegmentFile =
				Path.GetDirectoryName(path.ToLower()).EndsWith(Settings.Default.OralAnnotationsFolderAffix.ToLower());

			return (!isOralAnnotationSegmentFile && base.GetDoIncludeFile(path));
		}
	}
}