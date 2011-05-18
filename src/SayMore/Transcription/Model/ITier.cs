namespace SayMore.Transcription.Model
{
	public interface ITier
	{
		string DisplayName { get; }
		CellDataType DataType { get; }
	}
}