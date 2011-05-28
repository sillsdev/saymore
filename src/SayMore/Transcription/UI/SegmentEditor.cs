using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.UI.ComponentEditors;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class SegmentEditor : EditorBase
	{
		private SilGrid _grid;

		/// ------------------------------------------------------------------------------------
		public SegmentEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Segments";

			_grid = new SilGrid();
			_grid.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			_grid.Margin = new Padding(0);
			_grid.VirtualMode = true;
			_grid.FullRowFocusRectangleColor = _grid.DefaultCellStyle.SelectionBackColor;
			_grid.DefaultCellStyle.SelectionBackColor = ColorHelper.CalculateColor(Color.White,
				_grid.DefaultCellStyle.SelectionBackColor, 140);
			_grid.DefaultCellStyle.SelectionForeColor = _grid.DefaultCellStyle.ForeColor;
			_grid.EditMode = DataGridViewEditMode.EditOnEnter;

			int rowCount = 0;

			foreach (var tier in (new TierRepo(file.PathToAnnotatedFile)).GetAllTiers())
			{
				if (tier.DataType == TierType.Audio)
					_grid.Columns.Add(new AudioWaveFormColumn(tier));
				else if (tier.DataType == TierType.Text)
					_grid.Columns.Add(new TranscriptionColumn(tier));

				rowCount = Math.Max(rowCount, tier.GetAllSegments().Count());
			}

			_tableLayout.Controls.Add(_grid, 0, 1);

			_grid.RowCount = rowCount;
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			if (!file.GetCanHaveSegmentFile())
				return;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsOKSToShow
		{
			get { return _file.GetCanHaveSegmentFile(); }
		}

		/// ------------------------------------------------------------------------------------
		private void HandleLoadSegmentFileClick(object sender, EventArgs e)
		{

		}
	}

	public class TierRepo : ITierRepository
	{
		private List<ITier> _tiers;

		public TierRepo(string originalFileName)
		{
			_tiers = new List<ITier> { new OriginalTranscriptionTier(originalFileName), new TranscriptionTier() };
		}

		public IEnumerable<ITier> GetAllTiers()
		{
			return _tiers;
		}

		public ITier GetTier(int i)
		{
			return _tiers[i];
		}
	}

	public class OriginalTranscriptionTier : ITier
	{
		private IEnumerable<IMediaSegment> _segments;

		public OriginalTranscriptionTier(string filename)
		{
			DisplayName = "Original";

			_segments = new List<IMediaSegment>
			{
				new AudioSegment(this, filename, 0f, 3.0f),
				new AudioSegment(this, filename, 3.0f, 5.5f),
				new AudioSegment(this, filename, 5.5f, 6.2f),
				new AudioSegment(this, filename, 10f, 3.8f),
				new AudioSegment(this, filename, 93.7f, 5.7f),
			};
		}

		public string DisplayName { get; private set; }

		public TierType DataType
		{
			get { return TierType.Audio; }
		}

		public IEnumerable<ISegment> GetAllSegments()
		{
			return _segments.Cast<ISegment>();
		}

		public ISegment GetSegment(int index)
		{
			return _segments.ElementAt(index);
		}
	}

	public class TranscriptionTier : ITier
	{
		private IEnumerable<ITextSegment> _segments;

		public TranscriptionTier()
		{
			DisplayName = "Transcription";

			_segments = new List<ITextSegment>
			{
				new TextSegment(this, "blah one"),
				new TextSegment(this, "blah two"),
				new TextSegment(this, "blah three"),
				new TextSegment(this, "blah four"),
				new TextSegment(this, "blah five"),
			};
		}

		public string DisplayName { get; private set; }

		public TierType DataType
		{
			get { return TierType.Text; }
		}

		public IEnumerable<ISegment> GetAllSegments()
		{
			return _segments.Cast<ISegment>();
		}

		public ISegment GetSegment(int index)
		{
			return _segments.ElementAt(index);
		}
	}

	public class TextSegment : ITextSegment
	{
		string _text;

		public TextSegment(ITier tier, string text)
		{
			Tier = tier;
			SetText(text);
		}

		public string GetText()
		{
			return _text;
		}

		public void SetText(string text)
		{
			_text = text;
		}

		public ITier Tier { get; private set; }
	}

	public class AudioSegment : IMediaSegment
	{
		public string MediaFile { get; private set; }
		public float MediaStart { get; private set; }
		public float MediaLength { get; private set; }
		public ITier Tier { get; private set; }

		public AudioSegment(ITier tier, string filename, float start, float length)
		{
			Tier = tier;
			MediaFile = filename;
			MediaStart = start;
			MediaLength = length;
		}
	}
}
