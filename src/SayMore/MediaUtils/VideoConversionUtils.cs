using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SayMore.MediaUtils
{
	public class VideoConversionUtils
	{
		// ffmpeg -i {0} -vb 1000k -vcodec mpeg4 -ab 320k -acodec aac -strict -2  {1}
		// ffmpeg -i {0} -vb 1000k -vcodec mpeg4 -ab 320k -acodec libfaac {1}

	//-f s16le -acodec pcm_s16le
	}
}
