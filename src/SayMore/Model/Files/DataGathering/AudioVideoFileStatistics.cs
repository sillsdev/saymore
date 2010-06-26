using System;
using System.IO;
using System.Threading;
using Microsoft.DirectX;

namespace SayMore.Model.Files.DataGathering
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A AudioVideoFileStatistics is created for a single file.  It then provides information
	/// about the file, such as the duration, if it is a media file.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AudioVideoFileStatistics
	{
		public string Path { get; private set; }
		public long LengthInBytes { get; private set; }
		public TimeSpan Duration { get; private set; }

		/// ------------------------------------------------------------------------------------
		public AudioVideoFileStatistics(string path)
		{
			Path = path;
			LengthInBytes = new FileInfo(path).Length;
			Duration = GetDuration();
		}

		/// ------------------------------------------------------------------------------------
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
						return TimeSpan.FromSeconds((int)audio.Duration);
					}
				}

				if (ComponentRole.GetIsVideo(Path))
				{
					using (var video = new Microsoft.DirectX.AudioVideoPlayback.Video(Path))
					{
						return TimeSpan.FromSeconds((int)video.Duration);
					}

				}
			}
			catch (ThreadAbortException)
			{
				//fine, just return
			}
/*	remove temporarily so we can build on TeamCity... then find out if the SDK is needed or replace
			with ffmpeg
			catch (DirectXException e)
			{
				if (e.ErrorString != "VFW_E_UNSUPPORTED_STREAM")
					Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e, "Could not get duration of " + Path);
			}
*/			catch (Exception e)
			{
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e, "Could not get duration of " + Path);
			}

			return new TimeSpan();
		}
	}
}