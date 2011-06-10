using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Localization;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.UI.ComponentEditors;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class SegmentEditor : EditorBase
	{
		private SegmentEditorGrid _grid;

		/// ------------------------------------------------------------------------------------
		public SegmentEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Segments";

			_grid = new SegmentEditorGrid();
			_grid.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			_grid.Margin = new Padding(0);
			_grid.VirtualMode = true;
			_grid.FullRowFocusRectangleColor = _grid.DefaultCellStyle.SelectionBackColor;
			_grid.DefaultCellStyle.SelectionBackColor = ColorHelper.CalculateColor(Color.White,
				_grid.DefaultCellStyle.SelectionBackColor, 140);
			_grid.DefaultCellStyle.SelectionForeColor = _grid.DefaultCellStyle.ForeColor;
			_grid.EditMode = DataGridViewEditMode.EditOnEnter;

			_tableLayout.Controls.Add(_grid, 0, 1);
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
			using (var dlg = new OpenFileDialog())
			{
				var caption = LocalizationManager.LocalizeString(
					"SegmentEditor.LoadSegmentFileDlgCaption", "Select Segment File");

				dlg.Title = caption;
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;

				// TODO: Add ELAN .eaf files.

				dlg.Filter = "Audacity Label File (*.txt)|*.txt|All Files (*.*)|*.*";

				if (dlg.ShowDialog(this) == DialogResult.OK)
					LoadTiersFromLabelFile(GetTimePositionPairsFromFile(dlg.FileName));
			}
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<KeyValuePair<float, float>> GetTimePositionPairsFromFile(string labelFile)
		{
			foreach (var ln in File.ReadAllLines(labelFile))
			{
				var pieces = ln.Split('\t');
				if (pieces.Length < 2)
					continue;

				float start;
				if (!float.TryParse(pieces[0], out start))
					continue;

				float stop;
				if (!float.TryParse(pieces[1], out stop))
					continue;

				yield return new KeyValuePair<float, float>(start, stop - start);
			}
		}

		/// ------------------------------------------------------------------------------------
		private void LoadTiersFromLabelFile(IEnumerable<KeyValuePair<float, float>> timeSegments)
		{
			int rowCount = 0;

			var otTier = new OriginalTranscriptionTier(_file.PathToAnnotatedFile);
			_grid.Columns.Add(new AudioWaveFormColumn(otTier));

			var ttTier = new TextTranscriptionTier();
			_grid.Columns.Add(new TextTranscriptionColumn(ttTier));

			foreach (var kvp in timeSegments)
			{
				otTier.AddSegment(kvp.Key, kvp.Value);
				ttTier.AddSegment(string.Empty);
				rowCount++;
			}

			_grid.RowCount = rowCount;
		}
	}

	//public class TierRepo : ITierRepository
	//{
	//    private List<ITier> _tiers;

	//    public TierRepo(string originalFileName)
	//    {
	//        _tiers = new List<ITier> { new OriginalTranscriptionTier(originalFileName), new TranscriptionTier() };
	//    }

	//    public IEnumerable<ITier> GetAllTiers()
	//    {
	//        return _tiers;
	//    }

	//    public ITier GetTier(int i)
	//    {
	//        return _tiers[i];
	//    }
	//}

	public class OriginalTranscriptionTier : ITier
	{
		private List<IMediaSegment> _segments = new List<IMediaSegment>();
		private string _filename;

		public OriginalTranscriptionTier(string filename)
		{
			DisplayName = "Original";
			_filename = filename;
		}

		public string DisplayName { get; private set; }

		public TierType DataType
		{
			get { return TierType.Audio; }
		}

		public void AddSegment(float start, float length)
		{
			_segments.Add(new AudioSegment(this, _filename, start, length));
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

	public class TextTranscriptionTier : ITier
	{
		private List<ITextSegment> _segments = new List<ITextSegment>();

		public TextTranscriptionTier()
		{
			DisplayName = "Transcription";

			//_segments = new List<ITextSegment>
			//{
			//    new TextSegment(this, "blah one"),
			//    new TextSegment(this, "blah two"),
			//    new TextSegment(this, "blah three"),
			//    new TextSegment(this, "blah four"),
			//    new TextSegment(this, "blah five"),
			//};
		}

		public string DisplayName { get; private set; }

		public TierType DataType
		{
			get { return TierType.Text; }
		}

		public void AddSegment(string text)
		{
			_segments.Add(new TextSegment(this, text));
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
