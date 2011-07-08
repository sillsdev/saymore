using System.Linq;
using System.Text;
using System.Windows.Forms;
using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class TextTier : TierBase
	{
		/// ------------------------------------------------------------------------------------
		public TextTier(string displayName) : base(displayName)
		{
			DataType = TierType.Text;
			GridColumn = new TextAnnotationColumn(this);
		}

		/// ------------------------------------------------------------------------------------
		public TextSegment AddSegment(string id)
		{
			return AddSegment(id, string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public TextSegment AddSegment(string id, string text)
		{
			var segment = new TextSegment(this, id, text);
			_segments.Add(segment);
			return segment;
		}

		/// ------------------------------------------------------------------------------------
		public override object GetTierClipboardData(out string dataFormat)
		{
			dataFormat = DataFormats.UnicodeText;
			var bldr = new StringBuilder();
			foreach (var seg in GetAllSegments().Cast<ITextSegment>())
				bldr.AppendLine(seg.GetText());

			return bldr.ToString().TrimEnd();
		}
	}
}
