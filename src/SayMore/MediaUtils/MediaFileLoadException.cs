using System;

namespace SayMore.MediaUtils
{
	internal class MediaFileLoadException : Exception
	{
		public string Filename { get; }

		public MediaFileLoadException(string mediaFile, Exception innerException) :
			base(innerException.Message, innerException)
		{
			Filename = mediaFile;
		}
	}
}