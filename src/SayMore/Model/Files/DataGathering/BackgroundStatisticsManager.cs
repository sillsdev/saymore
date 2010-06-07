namespace SayMore.Model.Files.DataGathering
{
	public interface IProvideFileStatistics
	{
		FileStatistics GetFileData(string filePath);
	}

	public class BackgroundStatisticsManager :
		BackgroundFileProcessor<FileStatistics>, IProvideFileStatistics
	{
		public BackgroundStatisticsManager(string rootDirectoryPath) :
			base(rootDirectoryPath, path=>new FileStatistics(path))
		{
		}
	}
}