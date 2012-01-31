using Localization;
using SayMore.Transcription.UI;

namespace SayMore.Transcription.Model
{
	public class TimeOrderTier : TierBase
	{
		public string MediaFileName { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public TimeOrderTier(string filename) :
			this(LocalizationManager.GetString("EventsView.Transcription.TierNames.OriginalRecording", "Original"), filename)
		{
		}

		/// ------------------------------------------------------------------------------------
		public TimeOrderTier(string displayName, string filename) : base(displayName)
		{
			DataType = TierType.TimeOrder;
			MediaFileName = filename;
			GridColumn = new AudioWaveFormColumn(this);
		}

		/// ------------------------------------------------------------------------------------
		protected override TierBase GetNewTierInstance()
		{
			return new TimeOrderTier(DisplayName, MediaFileName);
		}

		/// ------------------------------------------------------------------------------------
		public TimeOrderSegment AddSegment(float start, float stop)
		{
			var segment = new TimeOrderSegment(this, start, stop);
			_segments.Add(segment);
			return segment;
		}

		/// ------------------------------------------------------------------------------------
		public override bool RemoveSegment(int index)
		{
			if (_segments.Count > 1 && index >= 0 && index < _segments.Count - 1)
			{
				var segToRemove = GetSegment(index) as ITimeOrderSegment;

				if (index == 0)
				{
					var nextSeg = GetSegment(index + 1) as ITimeOrderSegment;
					nextSeg.Start = segToRemove.Start;
				}
				else
				{
					var prevSeg = GetSegment(index - 1) as ITimeOrderSegment;
					prevSeg.Stop = segToRemove.Stop;
				}
			}

			return base.RemoveSegment(index);
		}
	}
}
