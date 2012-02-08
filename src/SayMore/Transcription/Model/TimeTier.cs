using Localization;
using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	/// ----------------------------------------------------------------------------------------
	public class TimeTier : TierBase
	{
		public string MediaFileName { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public TimeTier(string filename) :
			this(LocalizationManager.GetString("EventsView.Transcription.TierNames.OriginalRecording", "Original"), filename)
		{
		}

		/// ------------------------------------------------------------------------------------
		public TimeTier(string displayName, string filename) :
			base(displayName, tier => new AudioWaveFormColumn(tier))
		{
			MediaFileName = filename;
		}

		/// ------------------------------------------------------------------------------------
		protected override TierBase GetNewTierInstance()
		{
			return new TimeTier(DisplayName, MediaFileName);
		}

		/// ------------------------------------------------------------------------------------
		public Segment AddSegment(float start, float stop)
		{
			var segment = new Segment(this, start, stop);
			_segments.Add(segment);
			return segment;
		}

		/// ------------------------------------------------------------------------------------
		public override bool RemoveSegment(int index)
		{
			if (_segments.Count > 1 && index >= 0 && index < _segments.Count - 1)
			{
				var segToRemove = _segments[index];

				if (index == 0)
				{
					var nextSeg = _segments[index + 1];
					nextSeg.Start = segToRemove.Start;
				}
				else
				{
					var prevSeg = _segments[index - 1];
					prevSeg.End = segToRemove.End;
				}
			}

			return base.RemoveSegment(index);
		}
	}
}
