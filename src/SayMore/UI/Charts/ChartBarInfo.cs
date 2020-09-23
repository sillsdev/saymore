using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
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
		public TimeSpan TotalTimeSpan  { get; protected set; }
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
						where (secondaryFieldName != SessionFileType.kStatusFieldName || kvp.Key != Session.GetLocalizedStatus(Session.Status.Skipped))
						select new ChartBarSegmentInfo(secondaryFieldName, kvp.Key, kvp.Value,
							backColors[kvp.Key], textColors[kvp.Key])).OrderBy(kvp => kvp).ToList();

			try
			{
				TotalSessions = Segments.Sum(s => s.GetSessionCount());
				TotalTimeSpan = new TimeSpan(Segments.Sum(s => s.TotalTimeSpan.Ticks));

				foreach (var seg in Segments)
				{
					seg.SegmentSize = (int)Math.Round((Math.Ceiling(seg.TotalTimeSpan.TotalMinutes) / Math.Ceiling(TotalTimeSpan.TotalMinutes)) * 100,
						MidpointRounding.AwayFromZero);
				}
			}
			catch (InvalidOperationException)
			{
				// SP-854: This can happen if the the list is still loading, "Collection was modified; enumeration operation may not execute."
				// Let the other thread continue and try again.
				Application.DoEvents();
				Thread.Sleep(0);
			}
		}

		/// ----------------------------------------------------------------------------------------
		public static void CalculateBarSizes(IEnumerable<ChartBarInfo> barInfoList)
		{
			var barInfo = barInfoList.ToArray();
			var largestBarTime = (barInfo.Length == 0 ? 0 : barInfo.Max(bi => Math.Ceiling(bi.TotalTimeSpan.TotalMinutes)));

			foreach (var bi in barInfo)
			{
				bi.BarSize = (int)Math.Round(((Math.Ceiling(bi.TotalTimeSpan.TotalMinutes) / largestBarTime) * 100),
					MidpointRounding.AwayFromZero);
			}
		}
	}

	/// ----------------------------------------------------------------------------------------
	public class ChartBarSegmentInfo : IComparable<ChartBarSegmentInfo>
	{
		public class CollectionModifiedException : InvalidOperationException
		{
			public CollectionModifiedException(InvalidOperationException innerException)
				: base(innerException.Message, innerException)
			{
			}
		}

		private readonly IEnumerable<Session> m_sessions;

		public string FieldName { get; protected set; }
		public string FieldValue { get; protected set; }
		public Color BackColor { get; protected set; }
		public Color TextColor { get; protected set; }
		public TimeSpan TotalTimeSpan  { get; protected set; }
		public int SegmentSize { get; set; } // This is a percentage of the full bar's width.
		public override string ToString() { return FieldValue; }

		/// ------------------------------------------------------------------------------------
		public ChartBarSegmentInfo(string fieldName, string fieldValue,
			IEnumerable<Session> sessions, Color backColor, Color textColor)
		{
			FieldName = fieldName;
			FieldValue = fieldValue.Trim('<', '>');
			m_sessions = sessions;
			BackColor = backColor;
			TextColor = textColor;

			try
			{
				var minutesInSegment = GetMinutesInSegment();
				TotalTimeSpan = TimeSpan.FromMinutes(minutesInSegment);
			}
			catch (InvalidOperationException)
			{
				// SP-854: This can happen if the the list is still loading, "Collection was modified; enumeration operation may not execute."
				// Let the other thread continue and try again.
				Application.DoEvents();
				Thread.Sleep(0);
			}
		}

		/// ------------------------------------------------------------------------------------
		private double GetMinutesInSegment()
		{
			return m_sessions.Sum(x => x.GetTotalMediaDuration().TotalMinutes);
		}

		/// ------------------------------------------------------------------------------------
		public int GetSessionCount()
		{
			try
			{
				return m_sessions.Count();
			}
			catch (InvalidOperationException e)
			{
				// SP-1020: The Sessions collection has probably changed on another (probably UI) thread.
				// Hard to guarantee that the particular exception we caught was this one because the message
				// could be in a different language or change in a future version of Windows/.Net.
				// But for DEBUG purposes, let's do a sanity check.
				Debug.Assert(e.Message.StartsWith("Collection was modified"));
				throw new CollectionModifiedException(e);
			}
		}

		/// ------------------------------------------------------------------------------------
		public int CompareTo(ChartBarSegmentInfo other)
		{
			if (other == null)
				return 1;

			if (FieldName != SessionFileType.kStatusFieldName)
				return String.Compare(FieldValue, other.FieldValue, StringComparison.InvariantCulture);

			return new SessionStatusComparer().Compare(FieldValue, other.FieldValue);
		}
	}
}
