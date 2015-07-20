using System;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;
using SIL.IO;
using SIL.Xml;
using SayMore.Media.MPlayer;
using SayMore.Model.Files;
using SayMore.Properties;
using MediaInfoLib;
using SayMore.Utilities;

namespace SayMore.Media
{
	#region MediaFileInfo class
	/// ----------------------------------------------------------------------------------------
	[XmlType("mediaFileInfo")]
	public class MediaFileInfo
	{
		private Image _fullSizeThumbnail;
		private static string s_templateData;

		[XmlIgnore]
		public string MediaFilePath { get; private set; }

		[XmlElement("audio")]
		public AudioInfo Audio { get; set; }

		[XmlElement("video")]
		public VideoInfo Video { get; set; }

		[XmlElement("fileSize")]
		public long LengthInBytes { get; set; }

		[XmlElement("duration")]
		public float DurationInMilliseconds { get; set; }

		[XmlElement("format")]
		public string Format { get; set; }

		#region Constructor and initialization helper methods
		/// ------------------------------------------------------------------------------------
		static MediaFileInfo()
		{
			s_templateData = Resources.mediaFileInfoOutputTemplate.Replace(Environment.NewLine + "<", "<");
		}

		/// ------------------------------------------------------------------------------------
		public static string MediaInfoProgramPath
		{
			get { return FileLocator.GetFileDistributedWithApplication("MediaInfo", "MediaInfo.exe"); }
		}

		/// ------------------------------------------------------------------------------------
		public static MediaFileInfo GetInfo(string mediaFile)
		{
			var finfo = new FileInfo(mediaFile);
			if (!finfo.Exists || finfo.Length == 0)
			{
				var emptyMediaFileInfo = new MediaFileInfo();
				emptyMediaFileInfo.Audio = new AudioInfo(); // SP-1007
				return emptyMediaFileInfo;
			}
			var info = new MediaInfo();
			if (info.Open(mediaFile) == 0)
				return null;
			info.Option("Inform", s_templateData);
			string output = info.Inform();
			info.Close();
			Exception error;
			var mediaInfo = XmlSerializationHelper.DeserializeFromString<MediaFileInfo>(output, out error);

			if (mediaInfo == null || mediaInfo.Audio == null)
				return null;

			mediaInfo.MediaFilePath = mediaFile;
			return mediaInfo;
		}

		/// ------------------------------------------------------------------------------------
		public static string GetInfoAsHtml(string mediaFile, bool verbose)
		{
			var info = new MediaInfo();
			info.Open(mediaFile);

			info.Option("Complete", verbose ? "1" : "0");
			info.Option("Inform", "HTML");
			string output = info.Inform();
			info.Close();
			// SP-985: The following makes it easier for end-users to get the 8.3, which may be needed
			// to diagnose MPlayer issues.
			if (verbose)
			{
				var i = output.IndexOf(mediaFile, StringComparison.Ordinal);
				if (i > 0)
				{
					i = output.IndexOf("</tr>", i, StringComparison.OrdinalIgnoreCase);
					if (i > 0)
					{
						i += "</tr>".Length;
						var sb = new StringBuilder(output);
						sb.Insert(i, String.Format("{0}  <tr>{0}    <td><i>MPlayer path :</i></td>{0}    <td colspan=\"3\">{1}</td>{0}</tr>{0}",
							Environment.NewLine,
							FileSystemUtils.GetShortName(mediaFile).Replace('\\', '/')));
						output = sb.ToString();
					}
				}
			}
			return output;
		}

		#endregion

		#region Public properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the total duration in seconds. For audio files, the duration is always the
		/// duration of the audio. For video files, it is the total duration, counting from the
		/// start of the first track to the end of the last track (audio and video tracks are
		/// not guaranteed to start and end simultaneouly).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float DurationInSeconds
		{
			get
			{
				if (Audio == null)
					return Video == null ? 0 : Video.DurationInSeconds;
				if (Video == null)
					return Audio.DurationInSeconds;
				return Audio.DurationInSeconds > Video.DurationInSeconds ?
					Audio.DurationInSeconds : Video.DurationInSeconds;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the total duration. For audio files, the duration is always the
		/// duration of the audio. For video files, it is the total duration, counting from the
		/// start of the first track to the end of the last track (audio and video tracks are
		/// not guaranteed to start and end simultaneouly).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TimeSpan Duration
		{
			// ENHANCE: Account for Delay_relative_to_video
			get { return TimeSpan.FromSeconds(DurationInSeconds); }
		}

		/// ------------------------------------------------------------------------------------
		public float FramesPerSecond
		{
			get { return (Video != null ? Video.FramesPerSecond : 0); }
		}

		/// ------------------------------------------------------------------------------------
		public int Channels
		{
			get { return Audio.Channels; }
		}

		/// ------------------------------------------------------------------------------------
		public int SamplesPerSecond
		{
			get { return Audio.SamplesPerSecond; }
		}

		/// ------------------------------------------------------------------------------------
		public string AudioEncoding
		{
			get
			{
				if (Audio.Encoding == "MPEG Audio")
				{
					if (Audio.EncodingProfile == "Layer 3")
						return "MP3";

					if (Audio.EncodingProfile == "Layer 2")
						return "MP2";
				}

				return Audio.Encoding;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string Resolution
		{
			get { return (Video != null ? Video.Resolution : string.Empty); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the bit depth for PCM or WMA audio. Other audio types don't have a
		/// bit depth.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BitsPerSample
		{
			get { return Audio.BitsPerSample; }
		}

		/// ------------------------------------------------------------------------------------
		public int VideoBitRate
		{
			get { return (Video == null) ? 0 : Video.BitRate; }
		}

		/// ------------------------------------------------------------------------------------
		public int VideoKilobitsPerSecond
		{
			get { return (Video == null) ? 0 : Video.BitRate / 1000; }
		}

		/// ------------------------------------------------------------------------------------
		public Image FullSizedThumbnail
		{
			get
			{
				if (_fullSizeThumbnail == null && Video != null)
				{
					int seconds = (int)Math.Min(8, Video.DurationInSeconds / 2);
					_fullSizeThumbnail = MPlayerHelper.GetImageFromVideo(MediaFilePath, seconds);
				}

				return _fullSizeThumbnail;
			}
		}

		/// ------------------------------------------------------------------------------------
		public bool IsVideo
		{
			get { return Video != null; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public class TrackInfo
		{
			[XmlElement("format")]
			public string Encoding { get; set; } // Format

			[XmlElement("formatInfo")]
			public string EncodingDescription { get; set; }

			[XmlElement("formatVersion")]
			public string EncodingVersion { get; set; }

			[XmlElement("formatProfile")]
			public string EncodingProfile { get; set; }

			[XmlElement("formatCompression")]
			public string EncodingCompression { get; set; }

			[XmlElement("formatCommercialInfo")]
			public string EncodingCommercialDescription { get; set; }

			[XmlElement("internetMediaType")]
			public string InternetMediaType { get; set; }

			[XmlElement("codecInfo")]
			public string CodecInformation { get; set; }

			[XmlElement("duration")]
			public long DurationInMilliseconds { get; set; }

			public float DurationInSeconds
			{
				get { return DurationInMilliseconds / 1000f; }
			}

			[XmlElement("bitRateMode")]
			public string BitRateMode { get; set; }

			[XmlElement("bitRate")]
			public int BitRate { get; set; }

			/// ------------------------------------------------------------------------------------
			public int KilobitsPerSecond
			{
				get { return BitRate / 1000; }
			}
		}

		/// ------------------------------------------------------------------------------------
		[XmlType("audio")]
		public class AudioInfo : TrackInfo
		{
			[XmlElement("sampleRate")]
			public int SamplesPerSecond { get; set; }

			[XmlElement("channels")]
			public int Channels { get; set; }

			[XmlElement("bitDepth")]
			public int BitsPerSample { get; set; }
		}

		/// ------------------------------------------------------------------------------------
		[XmlType("video")]
		public class VideoInfo : TrackInfo
		{
			[XmlElement("width")]
			public int Width { get; set; }

			[XmlElement("height")]
			public int Height { get; set; }

			[XmlElement("resolution")]
			public string ResolutionString { get; set; }

			[XmlElement("frameRate")]
			public float FramesPerSecond { get; set; }

			[XmlElement("displayAspectRatio")]
			public float AspectRatio { get; set; }

			[XmlElement("standard")]
			public string Standard  { get; set; }  // NTSC or PAL

			/// ------------------------------------------------------------------------------------
			public Size PictureSize // in pixels
			{
				get { return new Size(Width, Height); }
			}

			/// ------------------------------------------------------------------------------------
			public string Resolution
			{
				get { return Width + " x " + Height; }
			}
		}
	}

	#endregion

	#region MediaFileInfo class if MPlayer is used
	///// ----------------------------------------------------------------------------------------
	///// <summary>
	///// Class that, using MPlayer, gets the information about a specific audio or video file.
	///// </summary>
	///// ----------------------------------------------------------------------------------------
	//public class MediaFileInfo
	//{
	//    private int _videoResolutionX;
	//    private int _videoResolutionY;

	//    public TimeSpan Duration { get; private set; }
	//    public float FramesPerSecond { get; private set; }
	//    public int Channels { get; private set; }
	//    public int SamplesPerSecond { get; private set; }
	//    public int AudioBitRate { get; private set; }
	//    public int VideoBitRate { get; private set; }
	//    public string AudioCodec { get; private set; }

	//    /// ------------------------------------------------------------------------------------
	//    public MediaFileInfo(string mediaFilePath)
	//    {
	//        MediaFilePath = mediaFilePath;
	//        LengthInBytes = new FileInfo(mediaFilePath).Length;
	//        LoadMPlayer(mediaFilePath);
	//    }

	//    public string MediaFilePath { get; private set; }
	//    public long LengthInBytes { get; private set; }

	//    /// ------------------------------------------------------------------------------------
	//    public string Resolution
	//    {
	//        get { return string.Format("{0}x{1}", _videoResolutionX, _videoResolutionY); }
	//    }

	//    /// ------------------------------------------------------------------------------------
	//    /// <summary>
	//    /// Returns the bit depth for PCM audio. Other audio types don't have a bit depth.
	//    /// </summary>
	//    /// ------------------------------------------------------------------------------------
	//    public int BitDepth
	//    {
	//        get
	//        {
	//            if (AudioCodec.ToLower() != "pcm")
	//                return 0;

	//            return AudioBitRate / (SamplesPerSecond * Channels);
	//        }
	//    }

	//    /// ------------------------------------------------------------------------------------
	//    private void LoadMPlayer(string path)
	//    {
	//        var prs = new Process();
	//        prs.StartInfo.CreateNoWindow = true;
	//        prs.StartInfo.UseShellExecute = false;
	//        prs.StartInfo.RedirectStandardOutput = true;
	//        prs.StartInfo.FileName = MPlayerHelper.MPlayerPath;

	//        path = path.Replace('\\', '/');
	//        prs.StartInfo.Arguments = string.Format("-msglevel all=6 -identify " +
	//            "-nofontconfig -frames 0 -ao null -vc null -vo null \"{0}\"", path);

	//        if (prs.Start())
	//        {
	//            prs.PriorityClass = ProcessPriorityClass.RealTime;
	//            prs.WaitForExit(1500);
	//            string line;
	//            while ((line = prs.StandardOutput.ReadLine()) != null)
	//                ParseMPlayerOutputLine(line);

	//            prs.Close();
	//        }
	//        else
	//        {
	//            prs = null;
	//            SIL.Reporting.ErrorReport.NotifyUserOfProblem("Gathering audio/video " +
	//                "statistics failed. Please verify that MPlayer is installed in the folder '{0}'.",
	//                Path.GetDirectoryName(path));
	//        }
	//    }

	//    /// ------------------------------------------------------------------------------------
	//    private void ParseMPlayerOutputLine(string data)
	//    {
	//        if (data == null)
	//            return;

	//        if (data.StartsWith("ID_VIDEO_FPS="))
	//        {
	//            FramesPerSecond = (float)Math.Round(
	//                double.Parse(data.Substring(13)), 3, MidpointRounding.AwayFromZero);
	//        }
	//        else if (data.StartsWith("ID_LENGTH="))
	//        {
	//            Duration = TimeSpan.FromSeconds(float.Parse(data.Substring(10)));
	//        }
	//        else if (data.StartsWith("ID_VIDEO_WIDTH="))
	//        {
	//            _videoResolutionX = int.Parse(data.Substring(15));
	//        }
	//        else if (data.StartsWith("ID_VIDEO_HEIGHT="))
	//        {
	//            _videoResolutionY = int.Parse(data.Substring(16));
	//        }
	//        else if (data.StartsWith("Channels: "))
	//        {
	//            // This is more accurate than ID_AUDIO_NCH but only get's
	//            // output from mplayer when the output is pretty verbose.
	//            Channels = int.Parse(data.Substring(10, 1));
	//        }
	//        //else if (data.StartsWith("ID_AUDIO_NCH="))
	//        //{
	//        //	  // This doesn't always report the correct number of channels.
	//        //    Channels = int.Parse(data.Substring(13));
	//        //}
	//        else if (data.StartsWith("ID_AUDIO_RATE="))
	//        {
	//            SamplesPerSecond = int.Parse(data.Substring(14));
	//        }
	//        else if (data.StartsWith("ID_AUDIO_BITRATE="))
	//        {
	//            AudioBitRate = int.Parse(data.Substring(17));
	//        }
	//        else if (data.StartsWith("ID_VIDEO_BITRATE="))
	//        {
	//            VideoBitRate = int.Parse(data.Substring(17));
	//        }
	//        else if (data.StartsWith("ID_AUDIO_CODEC="))
	//        {
	//            AudioCodec = data.Substring(15);
	//        }
	//    }
	//}

	#endregion
}
