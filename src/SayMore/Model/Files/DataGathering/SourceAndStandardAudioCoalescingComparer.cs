using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using SayMore.Media;

namespace SayMore.Model.Files.DataGathering
{
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
