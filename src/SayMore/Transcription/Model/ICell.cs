namespace SayMore.Transcription.Model
{
	public enum CellDataType { Text, Audio, Video } ;

	public interface ICell
	{
		ITier Tier { get; }
		CellDataType DataType { get; }
	}

	interface IAudioCell : ICell
	{
		//type here is just a guess for now
		byte[] GetAudioClip();
	}

	interface ITextCell : ICell
	{
		string GetText();
		/// <summary>
		/// nb: this is not a property because it may trigger non-trivial things, like saving to disk
		/// </summary>
		void SetText(string text);
	}

}