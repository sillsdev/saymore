namespace SayMore.Transcription.Model
{
	public interface ISegment
	{
		ICell GetCell(ITier tier);
	}
}