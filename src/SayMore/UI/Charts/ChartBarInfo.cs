using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.UI.Charts
{
	/// ----------------------------------------------------------------------------------------
	public class ChartBarInfo
	{
		public string FieldValue { get; set; }
		public IEnumerable<ChartBarSegmentInfo> Segments { get; set; }
		public int TotalSessions { get; protected set; }
		public int TotalTime { get; protected set; }
		public int BarSize { get; protected set; } // This is a percentage of the total table width.
		public override string ToString() { return FieldValue; }

		/// ------------------------------------------------------------------------------------
		public ChartBarInfo(string fieldValue, IEnumerable<Session> sessionList, Color clrBack, Color clrText)
		{
			var segmentList = new Dictionary<string, IEnumerable<Session>>();
			segmentList[fieldValue] = sessionList;

			var backColors = new Dictionary<string, Color>();
			backColors[fieldValue] = clrBack;

			var textColors = new Dictionary<string, Color>();
			textColors[fieldValue] = clrText;

			Initialize(fieldValue, string.Empty, segmentList, backColors, textColors);
		}

		/// ------------------------------------------------------------------------------------
		public ChartBarInfo(string fieldValue, string secondaryFieldName,
			IDictionary<string, IEnumerable<Session>> segmentList,
			IDictionary<string, Color> backColors, IDictionary<string, Color> textColors)
		{
			Initialize(fieldValue, secondaryFieldName, segmentList, backColors, textColors);
		}

		/// ----------------------------------------------------------------------------------------
		protected void Initialize(string fieldValue, string secondaryFieldName,
			IDictionary<string, IEnumerable<Session>> segmentList,
			IDictionary<string, Color> backColors, IDictionary<string, Color> textColors)
		{
			FieldValue = fieldValue.Trim('<', '>');
			if (!Settings.Default.AllowStatisticsChartLabelsToWrap)
				FieldValue = FieldValue.Replace(" ", HTMLChartBuilder.kNonBreakingSpace);

			Segments = (from kvp in segmentList
						where (secondaryFieldName != SessionFileType.kStatusFieldName || kvp.Key != Session.Status.Skipped.ToString())
						select new ChartBarSegmentInfo(secondaryFieldName, kvp.Key, kvp.Value,
							backColors[kvp.Key], textColors[kvp.Key])).OrderBy(kvp => kvp).ToList();

			TotalSessions = Segments.Sum(s => s.Sessions.Count());
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
			var barInfo = barInfoList.ToArray();
			var largestBarTime = (barInfo.Length == 0 ? 0 : barInfo.Max(bi => bi.TotalTime));

			foreach (var bi in barInfo)
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
		public IEnumerable<Session> Sessions { get; protected set; }
		public Color BackColor { get; protected set; }
		public Color TextColor { get; protected set; }
		public int TotalTime { get; protected set; }
		public int SegmentSize { get; set; } // This is a percentage of the full bar's width.
		public override string ToString() { return FieldValue; }

		/// ------------------------------------------------------------------------------------
		public ChartBarSegmentInfo(string fieldName, string fieldValue,
			IEnumerable<Session> sessions, Color backColor, Color textColor)
		{
			FieldName = fieldName;
			FieldValue = fieldValue.Trim('<', '>');
			Sessions = sessions;
			BackColor = backColor;
			TextColor = textColor;

			var minutesInSegment = Sessions.Sum(x => x.GetTotalMediaDuration().TotalMinutes);
			TotalTime = (int)Math.Ceiling(minutesInSegment);
		}

		/// ------------------------------------------------------------------------------------
		public int CompareTo(ChartBarSegmentInfo other)
		{
			if (other == null)
				return 1;

			if (FieldName != SessionFileType.kStatusFieldName)
				return FieldValue.CompareTo(other.FieldValue);

			return new SessionStatusComparer().Compare(FieldValue, other.FieldValue);
		}
	}
}
