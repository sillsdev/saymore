using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Palaso.Media;
using SayMore.UI.Archiving;

namespace SayMore.Model.Files
{
	#region MediaFileInfo class
	/// ----------------------------------------------------------------------------------------
	public class MediaFileInfo
	{
		private readonly MediaInfo _mediaInfo;

		/// ------------------------------------------------------------------------------------
		public MediaFileInfo(string mediaFilePath)
		{
			MediaFilePath = mediaFilePath;
			LengthInBytes = new FileInfo(mediaFilePath).Length;
			_mediaInfo = MediaInfo.GetInfo(mediaFilePath);
			ComponentFile.WaitForFileRelease(mediaFilePath);
		}

		#region Properties
		public string MediaFilePath { get; private set; }
		public long LengthInBytes { get; private set; }

		/// ------------------------------------------------------------------------------------
		public TimeSpan Duration
		{
			get { return _mediaInfo.Audio.Duration; }
		}

		/// ------------------------------------------------------------------------------------
		public int FramesPerSecond
		{
			get { return (_mediaInfo.Video != null ? _mediaInfo.Video.FramesPerSecond : 0); }
		}

		/// ------------------------------------------------------------------------------------
		public int Channels
		{
			get { return _mediaInfo.Audio.ChannelCount; }
		}

		/// ------------------------------------------------------------------------------------
		public int SamplesPerSecond
		{
			get { return _mediaInfo.Audio.SamplesPerSecond; }
		}

		/// ------------------------------------------------------------------------------------
		public string AudioCodec
		{
			get { return _mediaInfo.Audio.Encoding; }
		}

		/// ------------------------------------------------------------------------------------
		public string VideoCodec
		{
			get { return (_mediaInfo.Video != null ? _mediaInfo.Video.Encoding : string.Empty); }
		}

		/// ------------------------------------------------------------------------------------
		public string Resolution
		{
//			get { return string.Format("{0}x{1}", _videoResolutionX, _videoResolutionY); }
			get { return (_mediaInfo.Video != null ? _mediaInfo.Video.Resolution : string.Empty); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the bit depth for PCM audio. Other audio types don't have a bit depth.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int BitDepth
		{
			get
			{
				//if (AudioCodec.ToLower() != "pcm")
				//    return 0;

				//return AudioBitRate / (SamplesPerSecond * Channels);

				return _mediaInfo.Audio.BitDepth;
			}
		}

		/// ------------------------------------------------------------------------------------
		public int AudioBitRate
		{
			get { return GetBitRateValue("Audio"); }
		}

		/// ------------------------------------------------------------------------------------
		public int VideoBitRate
		{
			get { return GetBitRateValue("Video"); }
		}

		/// ------------------------------------------------------------------------------------
		private int GetBitRateValue(string mediaType)
		{
			int bitRate = 0;

			// I'm sure there's a expression that can extract just the numeric kb/s value
			// but I've spent several hours trying to learn enough regex. to figure it out,
			// but without success.
			var expression = string.Format("{0}:\\s.+(?=\\skb/s)", mediaType);
			var match = Regex.Match(_mediaInfo.RawData, expression);
			if (match.Success)
			{
				var value = match.Value.Substring(match.Value.LastIndexOf(' '));
				int.TryParse(value, out bitRate);
			}

			return bitRate;
		}

		#endregion
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
