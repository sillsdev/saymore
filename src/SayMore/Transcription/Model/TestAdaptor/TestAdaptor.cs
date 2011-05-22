using System;
using System.Collections.Generic;
using System.IO;
using SayMore.Properties;
using System.Linq;

namespace SayMore.Transcription.Model.TestAdaptor
{
	public class TestRepository : ISegmentRepository
	{
		private readonly List<ITier> _tiers = new List<ITier>();
		private readonly List<ISegment> _segments = new List<ISegment>();

		public TestRepository()
		{
			var originalAudio = new TestAudioTier();
			var translation = new TestTextTier();
			var carefulSpeech = new TestAudioTier();
			_tiers.AddRange(new ITier[]{originalAudio,translation,carefulSpeech});

			_segments.Add(new TestSegment(new ICell[] { new TestAudioCell(originalAudio), new TestTextCell(translation, "hello"), new TestAudioCell(carefulSpeech) }));
		}

		public IEnumerable<ISegment> GetAllSegments()
		{
			return _segments;
		}

		public IEnumerable<ITier> GetAllTiers()
		{
			return _tiers;
		}

		public ISegment GetSegment(int i)
		{
			return _segments[i];
		}
	}

	public class TestTextTier : ITier
	{
		public string DisplayName { get; set; }
		public CellDataType DataType { get { return CellDataType.Text; } }
	}

	public class TestAudioTier : ITier
	{
		public string DisplayName {get; set;}
		public CellDataType DataType{get { return CellDataType.Audio; }}
	}

	public class TestSegment : ISegment
	{
		private readonly IEnumerable<ICell> _cells;

		public TestSegment(IEnumerable<ICell> cells)
		{
			_cells = cells;
		}

		public ICell GetCell(ITier tier)
		{
			return _cells.FirstOrDefault(c => c.Tier == tier);
		}
	}


	public class TestAudioCell : IAudioCell
	{
		public TestAudioCell(TestAudioTier tier)
		{
			Tier = tier;
		}
		public ITier Tier
		{
			get;
			private set;
		}
		public CellDataType DataType{get { return CellDataType.Audio; }}

		public byte[] GetAudioClip()
		{
			byte[] buffer = new byte[Resources.Finished.Length];
			Resources.Finished.Read(buffer, 0, buffer.Length);
			return buffer;
		}
	}

	public class TestTextCell: ITextCell
	{
		private string _text;

		public TestTextCell(ITier tier, string text)
		{
			Tier = tier;
			_text = text;
		}

		public CellDataType DataType { get { return CellDataType.Text; } }

		public ITier Tier
		{
			get; private set;
		}

		public string GetText()
		{
			return _text;
		}

		public void SetText(string text)
		{
			_text = text;
		}
	}
}
