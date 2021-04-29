using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using SayMore.Media;

namespace SayMore.Model.Files.DataGathering
{
	/// ------------------------------------------------------------------------------------
	/// <summary>
	/// From a single original artifact, we may create derivative files (e.g., both a video
	/// and an audio-only extract. When we go to compute statistics on the project, we only
	/// want to count the duration of the original file. But there is no sure way to know
	/// when looking at a single file (i.e., its name) whether it is a derived file, an
	/// original source file, or a source file that has no derived counterpart. This custom
	/// comparer makes it possible to consider two files equal if one is the derived version
	/// of the other, so that only one of them gets counted. Note that the determination of
	/// "equality" in this sense involves a bit of "fuzziness" because we really only want
	/// to consider them equal if they contain the same audio data. In many/most cases, we
	/// can guess that they are the same based on their duration (if their names are
	/// appropriately similar), but unfortunately a converted file can vary slightly in
	/// duration (due to trimming, rounding errors, and/or videos whose audio tracks do not
	/// correspond exactly to their video tracks).
	/// </summary>
	/// ------------------------------------------------------------------------------------
	class SourceAndStandardAudioCoalescingComparer : IEqualityComparer<MediaFileInfo>
	{
		private Regex m_regexStandardAudio = new Regex(Properties.Settings.Default.StandardAudioFileSuffix + "$",
			RegexOptions.Compiled | RegexOptions.IgnoreCase);
	
		public bool Equals(MediaFileInfo x, MediaFileInfo y)
		{
			if (ReferenceEquals(x, y))
				return true;
			if (ReferenceEquals(x, null))
				return false;
			if (ReferenceEquals(y, null))
				return false;
			if (x.GetType() != y.GetType())
				return false;
			if (Math.Abs(x.DurationInMilliseconds - y.DurationInMilliseconds) > 1500) // Somewhat arbitrary fudge factor
				return false;
			if (x.MediaFilePath.Equals(y.MediaFilePath))
				return true;
			var matchX = m_regexStandardAudio.Match(x.MediaFilePath);
			if (matchX.Success)
				return y.MediaFilePath.StartsWith(x.MediaFilePath.Substring(0, matchX.Index));
			var matchY = m_regexStandardAudio.Match(y.MediaFilePath);
			return matchY.Success && x.MediaFilePath.StartsWith(y.MediaFilePath.Substring(0, matchY.Index));
		}

		public int GetHashCode(MediaFileInfo obj)
		{
			unchecked
			{
				var directory = obj.MediaFilePath == null ? null : Path.GetDirectoryName(obj.MediaFilePath);
				// We'd prefer to include the DurationInMilliseconds in the hashcode, but unfortunately a
				// converted file can vary slightly in duration.
				return directory != null ? directory.GetHashCode() : 0;
			}
		}
	}
}
