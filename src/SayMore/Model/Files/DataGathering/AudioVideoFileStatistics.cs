using System;
using System.IO;
using System.Threading;
using Microsoft.DirectX;
using Palaso.Media;

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
		public MediaInfo MediaInfo { get; private set; }
		public string Path { get; private set; }
		public long LengthInBytes { get; private set; }
		public TimeSpan Duration
		{
			//review: we're just using the audio for both audio and video
			get { return MediaInfo.Audio.Duration; }
		}

		/// ------------------------------------------------------------------------------------
		public AudioVideoFileStatistics(string path)
		{
			Path = path;
			LengthInBytes = new FileInfo(path).Length;
			MediaInfo = MediaInfo.GetInfo(path);
		}
	}
}