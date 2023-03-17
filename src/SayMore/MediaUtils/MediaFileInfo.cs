using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using SIL.Xml;
using SayMore.Media.MPlayer;
using MediaInfoLib;
using SayMore.Utilities;
using SIL.Reporting;
using static System.String;
using FileInfo = System.IO.FileInfo;
using Resources = SayMore.Properties.Resources;
using FFMpegCore;
using static SIL.IO.FileLocationUtilities;
using static SIL.Media.MediaInfo;

namespace SayMore.Media
{
	#region MediaFileInfo class
	/// ----------------------------------------------------------------------------------------
	[XmlType("mediaFileInfo")]
	public class MediaFileInfo
    {
        public const string kFFprobeName = "FFprobe";
        public const string kFFmpegFolder = "FFmpeg";
        public const string kMediaInfoDll = "MediaInfo.DLL";

        public enum HtmlLabels
        {
			// General labels
            General,
            FilePath,
			FileSize,
			Source,
			// Labels used for format and media streams
            Duration,
            StartTime,
            BitRate,
            Tags,
			// Format labels
            Format,
            FmtName,
            FmtLongName,
            FmtStreamCount,
            FmtProbeScore,
			// Media stream labels
			Id,
            CodecName,
			CodecLongName,
			CodecTag,
			Language,
			Disposition,
			BitDepth,
            // Audio labels
            Audio,
            NumberedAudioStream,
            ChannelCount,
            ChannelLayout,
            SampleRateHz,
            Profile,
			// Video labels
            Video,
            NumberedVideoStream,
            BitsPerRawSample,
            DisplayAspectRatio,
            SampleAspectRatio,
            Width,
            Height,
            FrameRate,
            PixelFormat,
            Rotation,
            AverageFrameRate,
            SubtitleStreamCount,
            ErrorData,
        }

		private Image _fullSizeThumbnail;
		private static readonly string s_templateData;

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
            FFprobeFolder = GetDirectoryDistributedWithApplication(false, kFFmpegFolder);
        }

        /// ------------------------------------------------------------------------------------
		public static MediaFileInfo GetInfo(string mediaFile) => GetInfo(mediaFile, out _);

		/// ------------------------------------------------------------------------------------
		public static MediaFileInfo GetInfo(string mediaFile, out Exception error)
		{
			error = null;

			var finfo = new FileInfo(mediaFile);
			if (!finfo.Exists || finfo.Length == 0)
				return new MediaFileInfo { Audio = new AudioInfo() }; // SP-1007
            
            MediaFileInfo mediaInfo = null;

            var silMediaInfo = SIL.Media.MediaInfo.GetInfo(mediaFile);
			if (silMediaInfo?.AnalysisData != null)
			{
				AudioInfo audio = null;
				var silAudio = silMediaInfo.Audio;
				if (silAudio != null)
				{
					Debug.Assert(silMediaInfo.AnalysisData.PrimaryAudioStream != null);
                    var bitRate = silMediaInfo.AnalysisData.PrimaryAudioStream.BitRate;
                    if (bitRate == 0 && silAudio.BitDepth > 0)
                        bitRate = silAudio.BitDepth * silAudio.SamplesPerSecond * silAudio.ChannelCount;
					audio = new AudioInfo
                    {
						DurationInMilliseconds = (long)silAudio.Duration.TotalMilliseconds,
						BitRate = bitRate,
						BitsPerSample = silAudio.BitDepth,
						Channels = silAudio.ChannelCount,
						Encoding = silAudio.Encoding,
						SamplesPerSecond = silAudio.SamplesPerSecond,
                    };
				}

				VideoInfo video = null;
                var silVideoInfo = silMediaInfo.Video;
				// Unfortunately, just checking for a Video stream is not enough because ffProbe
				// treats an embedded thumbnail jpeg as "video" (which is technically true).
				if (silVideoInfo != null && 
                    silMediaInfo.AnalysisData.PrimaryVideoStream != null &&
                    (silMediaInfo.AnalysisData.PrimaryVideoStream.BitRate > 0 || 
                        silMediaInfo.AnalysisData.PrimaryVideoStream.AvgFrameRate > 0))
				{
					video = new VideoInfo
					{
						DurationInMilliseconds = (long)silVideoInfo.Duration.TotalMilliseconds,
						FramesPerSecond = (float)Math.Round(silMediaInfo.AnalysisData.PrimaryVideoStream.AvgFrameRate, 3),
						AspectRatio = (float)Math.Round((double)silMediaInfo.AnalysisData.PrimaryVideoStream.Width /
                            silMediaInfo.AnalysisData.PrimaryVideoStream.Height, 3),
						BitRate = (int)silMediaInfo.AnalysisData.PrimaryVideoStream.BitRate,
						Height = silMediaInfo.AnalysisData.PrimaryVideoStream.Height,
						Width = silMediaInfo.AnalysisData.PrimaryVideoStream.Width,
						CodecInformation = silMediaInfo.AnalysisData.PrimaryVideoStream.CodecName,
                    };
				}

				mediaInfo = new MediaFileInfo
				{
                    LengthInBytes = finfo.Length,
					Format = silMediaInfo.AnalysisData.Format.FormatLongName,
					Audio = audio,
					Video = video,
					DurationInMilliseconds = (long)silMediaInfo.AnalysisData.Duration.TotalMilliseconds,
                    MediaFilePath = mediaFile,
                };

				// As near as I can tell, SayMore never shows the Format, but for a more complete
				// implementation that has at least as much information as MediaInfo.dll used to
				// supply, I'm adding the codec information.
				if (silMediaInfo.AnalysisData.PrimaryVideoStream != null)
                {
                    mediaInfo.Format += "; " +
                        silMediaInfo.AnalysisData.PrimaryVideoStream.CodecLongName;
                }
            }

            if (mediaInfo == null) // Very unlikely...
            {
                // and even less likely that this is going to do any better. But since this is the
				// way it used to get the media info, seems best to keep it around as a fallback.
                return GetMediaFileInfoUsingMediaInfoDll(mediaFile, ref error);
            }

            if (mediaInfo.Audio == null)
                Logger.WriteEvent("Could not get audio info from SIL.Media. Possibly a video-only media file?");

			return mediaInfo;
		}

#if DEBUG
        /// <summary>
        /// The following was some debugging code written to evaluate the differences between the
        /// information gleaned using MediaInfo.DLL vs. FFprobe. I decided to keep it around in case
        /// at some point I get some want to re-evaluate.
        /// </summary>
        private void CompareMediaInfoResults(MediaFileInfo mediaInfo, MediaFileInfo mediaInfoFromDll)
        {
			if (mediaInfo != null && mediaInfoFromDll != null)
			{
				if (mediaInfoFromDll.BitsPerSample != mediaInfo.BitsPerSample)
					Logger.WriteEvent("BitsPerSample differ");
				if (mediaInfoFromDll.SamplesPerSecond != mediaInfo.SamplesPerSecond)
					Logger.WriteEvent("SamplesPerSecond differ");
				if (Math.Abs(mediaInfoFromDll.DurationInMilliseconds - mediaInfo.DurationInMilliseconds) > 1)
					Logger.WriteEvent("DurationInMilliseconds differ");
				if (mediaInfoFromDll.Channels != mediaInfo.Channels)
					Logger.WriteEvent("Channels differ");
				if (Math.Abs(mediaInfoFromDll.FramesPerSecond - mediaInfo.FramesPerSecond) > 0.01)
					Logger.WriteEvent("FramesPerSecond differ");
				if (mediaInfoFromDll.LengthInBytes != mediaInfo.LengthInBytes)
					Logger.WriteEvent("LengthInBytes differ");
				if (!mediaInfo.Format.Contains(mediaInfoFromDll.Format))
					Logger.WriteEvent("Format differ");
				if (Math.Abs(mediaInfoFromDll.VideoKilobitsPerSecond - mediaInfo.VideoKilobitsPerSecond) > 5)
					Logger.WriteEvent("VideoKilobitsPerSecond differ");
				if (Math.Abs(mediaInfoFromDll.DurationInSeconds - mediaInfo.DurationInSeconds) > 0.00101)
					Logger.WriteEvent("DurationInSeconds differ");
				if (Math.Abs(mediaInfoFromDll.VideoBitRate - mediaInfo.VideoBitRate) > 4700)
					Logger.WriteEvent("VideoBitRate differ");

				if (mediaInfoFromDll.Audio?.SamplesPerSecond != mediaInfo.Audio?.SamplesPerSecond)
					Logger.WriteEvent("Audio SamplesPerSecond differ");
				if (mediaInfoFromDll.Audio?.BitRate != mediaInfo.Audio?.BitRate)
					Logger.WriteEvent("Audio BitRate differ");

				if (mediaInfoFromDll.IsVideo != mediaInfo.IsVideo)
					Logger.WriteEvent("One is video. The other is not!");

				if (mediaInfoFromDll.IsVideo && mediaInfo.IsVideo)
				{
					if (mediaInfoFromDll.Video.AspectRatio != mediaInfo.Video.AspectRatio)
						Logger.WriteEvent("AspectRatio differ");
					if (Math.Abs(mediaInfoFromDll.Video.FramesPerSecond - mediaInfo.Video.FramesPerSecond) > .009)
						Logger.WriteEvent("FramesPerSecond differ");
					if (mediaInfoFromDll.Video.Height != mediaInfo.Video.Height)
						Logger.WriteEvent("Height differ");
					if (mediaInfoFromDll.Video.Width != mediaInfo.Video.Width)
						Logger.WriteEvent("Width differ");
					if (mediaInfoFromDll.Video.PictureSize != mediaInfo.Video.PictureSize)
						Logger.WriteEvent("PictureSize differ");
					if (mediaInfoFromDll.Video.Resolution != mediaInfo.Video.Resolution)
						Logger.WriteEvent("Resolution differ");
					if (mediaInfoFromDll.FullSizedThumbnail?.PhysicalDimension != mediaInfo.FullSizedThumbnail?.PhysicalDimension)
						Logger.WriteEvent("FullSizedThumbnail differ");
				}
			}
		}
#endif

        private static MediaFileInfo GetMediaFileInfoUsingMediaInfoDll(string mediaFile, ref Exception error)
        {
            string xml;
            try
            {
                var info = new MediaInfo();
                if (info.Open(mediaFile) == 0)
                {
                    error = new Exception("Could not read any valid media information from file: " + mediaFile);
                    return null;
                }

                info.Option("Inform", s_templateData);
                xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine +
                    "<mediaFileInfo>" + info.Inform() + "</mediaFileInfo>";
                info.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                error = e;
                return null;
            }

            var mediaInfo = XmlSerializationHelper.DeserializeFromString<MediaFileInfo>(xml, out error);

            if (mediaInfo != null)
            {
                mediaInfo.MediaFilePath = mediaFile;

                if (mediaInfo.Audio == null)
                {
					var msg = $"Could not deserialize audio info to XML from MediaInfo for {mediaFile}:\r\n{xml}";
                    Logger.WriteEvent(msg);
                    error = new Exception(msg);
                }
            }

            return mediaInfo;
        }
        /// ------------------------------------------------------------------------------------
        public static string GetInfoAsHtml(string mediaFile, bool verbose,
            Func<HtmlLabels, string> getLabel, out string source)
        {
            if (!verbose)
            {
                var data = SIL.Media.MediaInfo.GetInfo(mediaFile)?.AnalysisData;
                if (data != null)
                {
                    var htmlBuilder = new StringBuilder("<html>");

                    var trIndent = Empty.PadLeft(4);
                    var tdIndent = trIndent.PadLeft(2);

					void OpenTable()
					{
                        htmlBuilder.AppendLine("  <table width=\"100%\" border=\"0\" cellpadding=\"1\" cellspacing=\"2\" style=\"border:1px solid Navy\" >");
                    }
					
                    void CloseTable()
                    {
                        htmlBuilder.AppendLine("  </table>");
                        htmlBuilder.AppendLine("  <br />");
                    }

                    void AppendRowWithLabel(string label, object value, string format = "i")
                    {
                        htmlBuilder.Append(trIndent);
                        htmlBuilder.AppendLine("<tr>");
                        htmlBuilder.Append(tdIndent);
                        htmlBuilder.AppendFormat("<td{0}><{1}>", format == "h2" ? " width=\"150\"" : "", format);
                        htmlBuilder.Append(HttpUtility.HtmlEncode(label));
                        htmlBuilder.AppendFormat("</{0}></td>", format);
                        htmlBuilder.AppendLine();
                        if (value != null)
                        {
                            htmlBuilder.Append(tdIndent);
                            htmlBuilder.Append("<td>");
                            htmlBuilder.Append(value);
                            htmlBuilder.AppendLine("</td>");
                        } 
                        htmlBuilder.Append(trIndent);
                        htmlBuilder.AppendLine("</tr>");
                    }

                    void AppendRow(HtmlLabels label, object value, string format = "i")
                    {
                        AppendRowWithLabel(getLabel(label), value, format);
                    }

					void AppendStreamInfoRows(MediaStream stream)
					{
                        AppendRow(HtmlLabels.Id, stream.Index);
                        AppendRow(HtmlLabels.CodecName, stream.CodecName);
                        AppendRow(HtmlLabels.CodecLongName, stream.CodecLongName);
                        AppendRow(HtmlLabels.CodecTag, stream.CodecTag);
                        AppendRow(HtmlLabels.Duration, stream.Duration);
                        AppendRow(HtmlLabels.BitRate, stream.BitRate);
					}

                    void AppendStreamDispositionAndTagRows(MediaStream stream)
                    {
                        if (stream.Disposition != null && stream.Disposition.Any())
                        {
                            AppendRow(HtmlLabels.Disposition, null, "h4");
                            foreach (var disposition in stream.Disposition)
                                AppendRowWithLabel(disposition.Key, disposition.Value);
                        }

                        if (stream.Tags != null && stream.Tags.Any())
                        {
                            AppendRow(HtmlLabels.Tags, null, "h4");
                            foreach (var disposition in stream.Tags)
                                AppendRowWithLabel(disposition.Key, disposition.Value);
                        }
                    }

                    htmlBuilder.AppendLine("<head><META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /></head>");
                    htmlBuilder.AppendLine("<body>");
					OpenTable();
					AppendRow(HtmlLabels.General, null, "h2");
					AppendRow(HtmlLabels.FilePath, mediaFile);
					AppendRow(HtmlLabels.FileSize, new FileInfo(mediaFile).Length);

                    AppendRow(HtmlLabels.Format, null, "h3");
					AppendRow(HtmlLabels.FmtName, data.Format.FormatName);
					AppendRow(HtmlLabels.FmtLongName, data.Format.FormatLongName);
                    AppendRow(HtmlLabels.FmtStreamCount, data.Format.StreamCount);
                    AppendRow(HtmlLabels.Duration, data.Format.Duration);
                    AppendRow(HtmlLabels.StartTime, data.Format.StartTime);
                    AppendRow(HtmlLabels.BitRate, data.Format.BitRate);
                    AppendRow(HtmlLabels.FmtProbeScore, data.Format.ProbeScore);
					if (data.SubtitleStreams.Count > 0)
                        AppendRow(HtmlLabels.SubtitleStreamCount, data.SubtitleStreams.Count);
                    if (data.Format.Tags != null)
                    {
                        AppendRow(HtmlLabels.Tags, null, "h4");
                        foreach (var tag in data.Format.Tags)
                            AppendRowWithLabel(tag.Key, tag.Value);
                    }

                    CloseTable();

                    for (var i = 0; i < data.AudioStreams.Count; i++)
                    {
                        var stream = data.AudioStreams[i];
                        OpenTable();
                        if (data.AudioStreams.Count > 1)
                        {
                            AppendRowWithLabel(Format(getLabel(HtmlLabels.NumberedAudioStream),
                                i + 1), null, "h2");
                        }
                        else
                            AppendRow(HtmlLabels.Audio, null, "h2");
                        AppendStreamInfoRows(stream);
                        AppendRow(HtmlLabels.ChannelCount, stream.Channels);
                        AppendRow(HtmlLabels.ChannelLayout, stream.ChannelLayout);
                        AppendRow(HtmlLabels.SampleRateHz, stream.SampleRateHz);
                        if (stream.BitDepth != null)
                            AppendRow(HtmlLabels.BitDepth, stream.BitDepth);
                        AppendRow(HtmlLabels.Language, stream.Language);
                        AppendRow(HtmlLabels.Profile, stream.Profile);
                        AppendStreamDispositionAndTagRows(stream);
                        CloseTable();
                    }

                    for (var i = 0; i < data.VideoStreams.Count; i++)
                    {
                        var stream = data.VideoStreams[i];
                        OpenTable();
                        if (data.VideoStreams.Count > 1)
                        {
                            AppendRowWithLabel(Format(getLabel(HtmlLabels.NumberedVideoStream), i),
                                null, "h2");
                        }
                        else
                            AppendRow(HtmlLabels.Video, null, "h2");
                        AppendStreamInfoRows(stream);
                        AppendRow(HtmlLabels.AverageFrameRate, stream.AverageFrameRate);
                        AppendRow(HtmlLabels.BitsPerRawSample, stream.BitsPerRawSample);
                        AppendRow(HtmlLabels.DisplayAspectRatio, stream.DisplayAspectRatio);
                        AppendRow(HtmlLabels.SampleAspectRatio, stream.SampleAspectRatio);
                        AppendRow(HtmlLabels.Width, stream.Width);
                        AppendRow(HtmlLabels.Height, stream.Height);
                        AppendRow(HtmlLabels.FrameRate, stream.FrameRate);
                        AppendRow(HtmlLabels.PixelFormat, stream.PixelFormat);
                        AppendRow(HtmlLabels.Rotation, stream.Rotation);
                        AppendStreamDispositionAndTagRows(stream);
                        CloseTable();
                    }

                    if (data.ErrorData != null && data.ErrorData.Count > 0)
					{
                        htmlBuilder.Append("  <h2>");
                        htmlBuilder.Append(HttpUtility.HtmlEncode(getLabel(HtmlLabels.ErrorData)));
                        htmlBuilder.AppendLine("  </h2>");
                        foreach (var errorLine in data.ErrorData)
                        {
                            htmlBuilder.Append("  <p>");
                            htmlBuilder.Append(HttpUtility.HtmlEncode(errorLine));
                            htmlBuilder.AppendLine("  </p>");
                        }
					}

                    htmlBuilder.AppendLine("</body>");
                    htmlBuilder.AppendLine("</html>");
                    source = kFFprobeName;
                    return htmlBuilder.ToString();
                }
            }

            source = kMediaInfoDll;
            return GetInfoAsHtmlFromDll(mediaFile, verbose);
        }

        /// ------------------------------------------------------------------------------------
		public static string GetInfoAsHtmlFromDll(string mediaFile, bool verbose)
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
						sb.Insert(i, Format("{0}  <tr>{0}    <td><i>MPlayer path :</i></td>{0}    <td colspan=\"3\">{1}</td>{0}</tr>{0}",
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
		/// not guaranteed to start and end simultaneously).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public float DurationInSeconds
		{
			get
			{
				if (Audio == null)
					return Video?.DurationInSeconds ?? 0;
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
		/// not guaranteed to start and end simultaneously).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TimeSpan Duration
		{
			// ENHANCE: Account for Delay_relative_to_video
			get { return TimeSpan.FromSeconds(DurationInSeconds); }
		}

		/// ------------------------------------------------------------------------------------
		public float FramesPerSecond => Video?.FramesPerSecond ?? 0;

        /// ------------------------------------------------------------------------------------
		public int Channels => Audio?.Channels ?? 0;

        /// ------------------------------------------------------------------------------------
		public int SamplesPerSecond => Audio?.SamplesPerSecond ?? 0;

        /// ------------------------------------------------------------------------------------
		public string AudioEncoding
		{
			get
			{
				// SP-1024: Audio should not normally be null, but if something happens to the
				// media file or the XML file thjat this object loads from, we don't want it
				// to crash. (Calling code seems to be capable of dealing with empty string.)
				if (Audio == null)
					return Empty;

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
		public string Resolution => (Video != null ? Video.Resolution : Empty);

        /// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the bit depth for PCM or WMA audio. Other audio types don't have a
		/// bit depth.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BitsPerSample => Audio?.BitsPerSample ?? 0;

        /// ------------------------------------------------------------------------------------
		public long VideoBitRate => Video?.BitRate ?? 0;

        /// ------------------------------------------------------------------------------------
		public int VideoKilobitsPerSecond => (Video == null) ? 0 : (int)(Video.BitRate / 1000);

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
        public bool IsVideo => Video != null;

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

			public float DurationInSeconds => DurationInMilliseconds / 1000f;

            [XmlElement("bitRateMode")]
			public string BitRateMode { get; set; }

			[XmlElement("bitRate")]
			public long BitRate { get; set; }

			/// ------------------------------------------------------------------------------------
			public int KilobitsPerSecond => (int)(BitRate / 1000);
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

			/// <summary>
			/// Gets the picture size in pixels
			/// </summary>
			public Size PictureSize => new Size(Width, Height);

            /// <summary>
            /// Gets the video resolution as a string __W__ x __H__ (in pixels)
            /// </summary>
			public string Resolution => Width + " x " + Height;
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
