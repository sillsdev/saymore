using System.Collections.Generic;
using System.IO;
using System.Linq;
using SayMore.Properties;
using SayMore.Transcription.UI;

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
		private readonly string _translationFileAffix;
		private readonly string _CarefulFileAffix;

		/// ------------------------------------------------------------------------------------
		public AudioVideoDataGatherer(string rootDirectoryPath, IEnumerable<FileType> allFileTypes) :
			base(rootDirectoryPath, allFileTypes.Where(t => t.IsAudioOrVideo),
				path => new MediaFileInfo(path))
		{
			_translationFileAffix = "_" + OralAnnotationType.Translation.ToString().ToLower() + ".wav";
			_CarefulFileAffix = "_" + OralAnnotationType.Careful.ToString().ToLower() + ".wav";
		}

		/// ------------------------------------------------------------------------------------
		protected override void CollectDataForFile(string path)
		{
			path = path.ToLower();

			var isOralAnnotationSegmentFile =
				Path.GetDirectoryName(path).EndsWith(Settings.Default.OralAnnotationsFolderAffix.ToLower()) &&
				path.EndsWith(_translationFileAffix) &&
				path.EndsWith(_CarefulFileAffix);

			// Don't collect info. on oral annotation files.
			if (!isOralAnnotationSegmentFile &&
				!path.EndsWith(Settings.Default.OralAnnotationGeneratedFileAffix.ToLower()))
			{
				base.CollectDataForFile(path);
			}
		}
	}
}