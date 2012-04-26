using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Palaso.IO;
using SayMore.Media.UI;
using SayMore.Properties;
using SayMore.UI.Utilities;
using SilTools;

namespace SayMore.Model.Files
{
	#region MediaFileInfo class
	/// ----------------------------------------------------------------------------------------
	public class MediaFileInfo
	{
		private Image _fullSizeThumbnail;

		public string MediaFilePath { get; private set; }
		public AudioInfo Audio { get; private set; }
		public VideoInfo Video { get; private set; }
		public long LengthInBytes { get; private set; }

		#region Constructor and initialization helper methods
		/// ------------------------------------------------------------------------------------
		private MediaFileInfo(string path, XElement xmlInfo)
		{
			MediaFilePath = path;
			LengthInBytes = new FileInfo(path).Length;
			FileSystemUtils.WaitForFileRelease(path);

			Audio = new AudioInfo(GetTrack(xmlInfo, "Audio"));

			var videoTrack = GetTrack(xmlInfo, "Video");
			if (videoTrack != null)
				Video = new VideoInfo(videoTrack);
		}

		/// ------------------------------------------------------------------------------------
		private static XElement GetTrack(XElement xmlInfo, string trackName)
		{
			var fileElement = xmlInfo.Element("File");
			return fileElement.Elements("track").FirstOrDefault(t =>
				{
					var typeAttrib = t.Attribute("type");
					return typeAttrib != null && typeAttrib.Value == trackName;
				});
		}

		/// ------------------------------------------------------------------------------------
		public static string MediaInfoProgramPath
		{
			get { return FileLocator.GetFileDistributedWithApplication("MediaInfo", "MediaInfo.exe"); }
		}

		/// ------------------------------------------------------------------------------------
		public static MediaFileInfo GetInfo(string mediaFile)
		{
			var templatePath = Path.Combine(Path.GetTempPath(), "mediaInfoTemplate.xml");
			File.WriteAllText(templatePath, Resources.mediaFileInfoOutputTemplate);
			var prs = new ExternalProcess(MediaInfoProgramPath);

			try
			{
				mediaFile = mediaFile.Replace('\\', '/');
				prs.StartInfo.Arguments = string.Format("--inform=\"{0}\" \"{1}\"", templatePath, mediaFile);

				if (!prs.StartProcess())
					return null;

				return XmlSerializationHelper.DeserializeFromString<MediaFileInfo>(prs.StandardOutput.ReadToEnd());

				//var xmlInfo = XElement.Load(prs.StandardOutput);
			}
			finally
			{
				prs.WaitForExit();
				prs.Close();

				if (File.Exists(templatePath))
					File.Delete(templatePath);
			}

			// SayMore is not interested in media with no audio track.
			//return GetTrack(xmlInfo, "Audio") == null ? null : new MediaFileInfo(mediaFile, xmlInfo);
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
		public float DurationSeconds
		{
			get { return (float)Duration.TotalSeconds; }
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
			get { return (Video == null || Audio.Duration > Video.Duration) ? Audio.Duration : Video.Duration; }
		}

		/// ------------------------------------------------------------------------------------
		public float FramesPerSecond
		{
			get { return (Video != null ? Video.FramesPerSecond : 0); }
		}

		/// ------------------------------------------------------------------------------------
		public int Channels
		{
			get { return Audio.ChannelCount; }
		}

		/// ------------------------------------------------------------------------------------
		public int SamplesPerSecond
		{
			get { return Audio.SamplesPerSecond; }
		}

		/// ------------------------------------------------------------------------------------
		public string AudioCodec
		{
			get { return Audio.Encoding; }
		}

		/// ------------------------------------------------------------------------------------
		public string VideoCodec
		{
			get { return (Video != null ? Video.Encoding : string.Empty); }
		}

		/// ------------------------------------------------------------------------------------
		public string Resolution
		{
			get { return (Video != null ? Video.Resolution : string.Empty); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the bit depth for PCM audio. Other audio types don't have a bit depth.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BitDepth
		{
			get { return Audio.BitDepth; }
		}

		/// ------------------------------------------------------------------------------------
		public int AudioBitRate
		{
			get { return Audio.BitRate; }
		}

		/// ------------------------------------------------------------------------------------
		public int VideoBitRate
		{
			get { return (Video == null) ? 0 : Video.BitRate; }
		}

		/// ------------------------------------------------------------------------------------
		public Image FullSizedThumbnail
		{
			get
			{
				if (_fullSizeThumbnail == null)
				{
					int seconds = Math.Min(8, Video.Duration.Seconds / 2);
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
			private readonly TimeSpan _duration;
			private readonly int _bitRate;
			public string Encoding { get; private set; } // Format
			public string EncodingDescription { get; private set; } // Codec_ID_Info

			/// ------------------------------------------------------------------------------------
			protected TrackInfo(XElement xmlTrackInfo)
			{
				_duration = GetDuration(xmlTrackInfo);
				_bitRate = GetRate(xmlTrackInfo, "Bit");
				ExtractEncoding(xmlTrackInfo);
			}

			/// ------------------------------------------------------------------------------------
			public TimeSpan Duration
			{
				get { return _duration; }
			}

			/// ------------------------------------------------------------------------------------
			public int BitRate // Bit_rate * 1000
			{
				get { return _bitRate; }
			}

			/// ------------------------------------------------------------------------------------
			private TimeSpan GetDuration(XElement xmlTrackInfo)
			{
				return GetTime(xmlTrackInfo, "Duration");
			}

			/// ------------------------------------------------------------------------------------
			protected TimeSpan GetTime(XElement xmlTrackInfo, string elementName)
			{
				var duration = xmlTrackInfo.Element(elementName);
				if (duration == null)
					return default(TimeSpan);

				// ENHANCE: deal with videos longer than a day.
				// ENHANCE: allow for a leading negative sign, in order to process the "Delay_relative_to_video" element.
				var match = Regex.Match(duration.Value, @"=((?<hours>\d+)h)? ((?<minutes>\d+)mn)? ((?<seconds>\d+)s)? ((?<milliseconds>\d+)ms)?");
				if (!match.Success)
					return default(TimeSpan);

				var hours = GetIntFromMatchedGroup(match, "hours");
				var minutes = GetIntFromMatchedGroup(match, "minutes");
				var seconds = GetIntFromMatchedGroup(match, "seconds");
				var milliseconds = GetIntFromMatchedGroup(match, "milliseconds");

				return new TimeSpan(0, hours, minutes, seconds, milliseconds);
			}

			/// ------------------------------------------------------------------------------------
			protected int GetRate(XElement xmlTrackInfo, string rateType)
			{
				return (int)(GetFloatValue(xmlTrackInfo, rateType + "_rate") * 1000);
			}

			/// ------------------------------------------------------------------------------------
			private void ExtractEncoding(XElement xmlTrackInfo)
			{
				XElement format = xmlTrackInfo.Element("Format");
				if (format != null)
					Encoding = format.Value;

				XElement encodingDesc = xmlTrackInfo.Element("Codec_ID_Info");
				if (encodingDesc != null)
					EncodingDescription = encodingDesc.Value;
			}

			/// ------------------------------------------------------------------------------------
			/// <summary>
			/// Gets the integer value of the specified element.
			/// </summary>
			/// ------------------------------------------------------------------------------------
			internal static int GetIntValue(XElement track, string elementName)
			{
				int val = default(int);
				var element = track.Element(elementName);
				if (element != null)
				{
					var match = Regex.Match(element.Value, @"\d+");
					if (match.Groups.Count == 1)
						int.TryParse(match.Groups[0].Value, out val);
				}
				return val;
			}

			/// ------------------------------------------------------------------------------------
			/// <summary>
			/// Gets the double value of the specified element.
			/// </summary>
			/// ------------------------------------------------------------------------------------
			internal static float GetFloatValue(XElement track, string elementName)
			{
				float val = default(float);
				var element = track.Element(elementName);
				if (element != null)
				{
					var match = Regex.Match(element.Value, @"=\d*.?\d*");
					if (match.Groups.Count == 1)
						float.TryParse(match.Groups[0].Value, out val);
				}
				return val;
			}

			/// ------------------------------------------------------------------------------------
			private static int GetIntFromMatchedGroup(Match match, string groupName)
			{
				int val;
				return !int.TryParse(match.Result("${" + groupName + "}"), out val) ? default(int) : val;
			}
		}

		/// ------------------------------------------------------------------------------------
		public class AudioInfo : TrackInfo
		{
			private readonly int _channelCount;
			private readonly int _samplesPerSecond;
			private readonly int _bitDepth;
			//private readonly TimeSpan _delay;

			/// ------------------------------------------------------------------------------------
			internal AudioInfo(XElement xmlAudioTrackInfo) : base(xmlAudioTrackInfo)
			{
				_bitDepth = GetIntValue(xmlAudioTrackInfo, "Bit_depth");
				_samplesPerSecond = GetRate(xmlAudioTrackInfo, "Sampling");
				_channelCount = GetIntValue(xmlAudioTrackInfo, "Channel_s_");
				//_delay = GetTime(xmlAudioTrackInfo, "Delay_relative_to_video");
			}

			/// ------------------------------------------------------------------------------------
			public int ChannelCount
			{
				get { return _channelCount; }
			}

			/// ------------------------------------------------------------------------------------
			public int SamplesPerSecond // Sampling_rate * 1000
			{
				get { return _samplesPerSecond; }
			}

			/// ------------------------------------------------------------------------------------
			public int BitDepth
			{
				get { return _bitDepth; }
			}
		}

		/// ------------------------------------------------------------------------------------
		public class VideoInfo : TrackInfo
		{
			private readonly float _framesPerSecond;
			private readonly Size _size;
			private readonly string _standard;

			/// ------------------------------------------------------------------------------------
			internal VideoInfo(XElement xmlInfo) : base(xmlInfo)
			{
				_framesPerSecond = GetFloatValue(xmlInfo, "Frame_rate");
				_size = new Size(GetIntValue(xmlInfo, "Width"), GetIntValue(xmlInfo, "Height"));
				XElement standard = xmlInfo.Element("Standard");
				if (standard != null)
					_standard = standard.Value;
			}

			/// ------------------------------------------------------------------------------------
			public float FramesPerSecond // Frame_rate
			{
				get { return _framesPerSecond; }
			}

			/// ------------------------------------------------------------------------------------
			public Size PictureSize // in pixels
			{
				get { return _size; }
			}

			/// ------------------------------------------------------------------------------------
			public float AspectRatio
			{
				get { return ((float)PictureSize.Width) / PictureSize.Height; }
			}

			/// ------------------------------------------------------------------------------------
			public string Standard // NTSC or PAL
			{
				get { return _standard; }
			}

			/// ------------------------------------------------------------------------------------
			public string Resolution
			{
				get { return PictureSize.Width + " x " + PictureSize.Height; }
			}
		}
	}

	#endregion

	#region MediaFileInfo class if MPlayer is used (Commented out)
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
	//            Palaso.Reporting.ErrorReport.NotifyUserOfProblem("Gathering audio/video " +
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
