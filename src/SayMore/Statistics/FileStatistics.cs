using System;
using System.IO;
using System.Threading;
using SayMore.Model.Files;

namespace SayMore.Statistics
{
	public class FileStatistics
	{
		public long LengthInBytes;
		public string Path;
		public TimeSpan Duration;

		public FileStatistics(string path)
		{
			Path = path;
			LengthInBytes = new FileInfo(path).Length;
			Duration = GetDuration();
		}

		private TimeSpan GetDuration()
		{
			// TODO: What should we do if DirectX throws an exception (e.g. the path is really
			// not a valid audio or video stream)? For now, just ignore exceptions.
			try
			{
				if (ComponentRole.GetIsAudio(Path))
				{
					using (var audio = new Microsoft.DirectX.AudioVideoPlayback.Audio(Path))
					{
						return TimeSpan.FromSeconds((int) audio.Duration);
					}
				}
				else if (ComponentRole.GetIsVideo(Path))
				{
					using (var video = new Microsoft.DirectX.AudioVideoPlayback.Video(Path))
					{
						return TimeSpan.FromSeconds((int) video.Duration);
					}

				}
			}
			catch(ThreadAbortException)
			{
				//fine, just return
			}
			catch(Exception e)
			{
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e,"Could not get duration of "+Path);
			}


			return new TimeSpan();
		}

	}
}