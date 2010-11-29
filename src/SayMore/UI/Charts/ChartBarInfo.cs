using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SayMore.Model;
using SayMore.Properties;

namespace SayMore.UI.Charts
{
	/// ----------------------------------------------------------------------------------------
	public class ChartBarInfo
	{
		public string FieldValue { get; set; }
		public IEnumerable<ChartBarSegmentInfo> Segments { get; set; }
		public int TotalEvents { get; protected set; }
		public int TotalTime { get; protected set; }
		public int BarSize { get; protected set; } // This is a percentage of the total table width.
		public override string ToString() { return FieldValue; }

		/// ------------------------------------------------------------------------------------
		public ChartBarInfo(string fieldValue, IEnumerable<Event> eventList, Color clrBack, Color clrText)
		{
			var segmentList = new Dictionary<string, IEnumerable<Event>>();
			segmentList[fieldValue] = eventList;

			var backColors = new Dictionary<string, Color>();
			backColors[fieldValue] = clrBack;

			var textColors = new Dictionary<string, Color>();
			textColors[fieldValue] = clrText;

			Initialize(fieldValue, string.Empty, segmentList, backColors, textColors);
		}

		/// ------------------------------------------------------------------------------------
		public ChartBarInfo(string fieldValue, string secondaryFieldName,
			IDictionary<string, IEnumerable<Event>> segmentList,
			IDictionary<string, Color> backColors, IDictionary<string, Color> textColors)
		{
			Initialize(fieldValue, secondaryFieldName, segmentList, backColors, textColors);
		}

		/// ----------------------------------------------------------------------------------------
		protected void Initialize(string fieldValue, string secondaryFieldName,
			IDictionary<string, IEnumerable<Event>> segmentList,
			IDictionary<string, Color> backColors, IDictionary<string, Color> textColors)
		{
			FieldValue = fieldValue.Trim('<', '>');
			if (!Settings.Default.AllowStatisticsChartLabelsToWrap)
				FieldValue = FieldValue.Replace(" ", HTMLChartBuilder.kNonBreakingSpace);

			Segments = (from kvp in segmentList
						where (secondaryFieldName != "status" || kvp.Key != Event.Status.Skipped.ToString())
						select new ChartBarSegmentInfo(secondaryFieldName, kvp.Key, kvp.Value,
							backColors[kvp.Key], textColors[kvp.Key])).OrderBy(kvp => kvp).ToList();

			TotalEvents = Segments.Sum(s => s.Events.Count());
			TotalTime = Segments.Sum(s => s.TotalTime);

			foreach (var seg in Segments)
			{
				seg.SegmentSize = (int)Math.Round(((seg.TotalTime / (float)TotalTime) * 100),
					MidpointRounding.AwayFromZero);
			}
		}

		/// ----------------------------------------------------------------------------------------
		public static void CalculateBarSizes(IEnumerable<ChartBarInfo> barInfoList)
		{
			var largestBarTime = barInfoList.Max(bi => bi.TotalTime);

			foreach (var bi in barInfoList)
			{
				bi.BarSize = (int)Math.Round(((bi.TotalTime / (float)largestBarTime) * 100),
					MidpointRounding.AwayFromZero);
			}
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class ChartBarSegmentInfo : IComparable<ChartBarSegmentInfo>
	{
		public string FieldName { get; protected set; }
		public string FieldValue { get; protected set; }
		public IEnumerable<Event> Events { get; protected set; }
		public Color BackColor { get; protected set; }
		public Color TextColor { get; protected set; }
		public int TotalTime { get; protected set; }
		public int SegmentSize { get; set; } // This is a percentage of the full bar's width.
		public override string ToString() { return FieldValue; }

		/// ------------------------------------------------------------------------------------
		public ChartBarSegmentInfo(string fieldName, string fieldValue,
			IEnumerable<Event> events, Color backColor, Color textColor)
		{
			FieldName = fieldName;
			FieldValue = fieldValue.Trim('<', '>');
			Events = events;
			BackColor = backColor;
			TextColor = textColor;

			var minutesInSegment = Events.Sum(x => x.GetTotalMediaDuration().TotalMinutes);
			TotalTime = (int)Math.Ceiling(minutesInSegment);
		}

		/// ------------------------------------------------------------------------------------
		public int CompareTo(ChartBarSegmentInfo other)
		{
			if (other == null)
				return 1;

			if (FieldName != "status")
				return FieldValue.CompareTo(other.FieldValue);

			var sx = FieldValue.Replace(' ', '_');
			var sy = other.FieldValue.Replace(' ', '_');

			if (!Enum.GetNames(typeof(Event.Status)).Contains(sx))
				return 1;

			if (!Enum.GetNames(typeof(Event.Status)).Contains(sy))
				return -1;

			return (int)Enum.Parse(typeof(Event.Status), sx) -
				(int)Enum.Parse(typeof(Event.Status), sy);
		}
	}
}
