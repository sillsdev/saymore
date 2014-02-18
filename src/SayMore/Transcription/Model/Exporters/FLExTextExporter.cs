using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using L10NSharp;
using Palaso.Reporting;
using SayMore.UI;
using SayMore.UI.LowLevelControls;

namespace SayMore.Transcription.Model.Exporters
{
	/// <summary>
	/// Exports FLEX-Text interlinear
	/// </summary>
	public class FLExTextExporter : IProgressViewModel
	{
		public event EventHandler<ProgressFinishedArgs> OnFinished;
		public event EventHandler OnUpdateProgress;
		public event EventHandler OnUpdateStatus;

		private readonly string _wsTranscriptionId;
		private readonly string _wsFreeTranslationId;
		private readonly string _title;
		private readonly TierCollection _tierCollection;
		private readonly string _outputFilePath;
		private BackgroundWorker _worker;
		private readonly int _segmentCount;
		private int _currentSegment;
		private readonly string _mediaFileGuid;
		private readonly string _mediaFilePath;
		private readonly string _sourceFileGuid;
		private readonly string _sourceFilePath;


		/// ------------------------------------------------------------------------------------
		public FLExTextExporter(string outputFilePath, string title,
			TierCollection tierCollection, string wsTranscriptionId, string wsFreeTranslationId,
			string mediaFilePath, string sourceFilePath)
		{
			_outputFilePath = outputFilePath;
			_title = title;
			_tierCollection = (tierCollection ?? new TierCollection());
			_wsTranscriptionId = wsTranscriptionId;
			_wsFreeTranslationId = wsFreeTranslationId;
			_mediaFileGuid = Guid.NewGuid().ToString();
			_mediaFilePath = mediaFilePath;
			if (!string.IsNullOrEmpty(sourceFilePath))
			{
				_sourceFileGuid = Guid.NewGuid().ToString();
				_sourceFilePath = sourceFilePath;
			}
			_currentSegment = 1;

			var textTier = _tierCollection.GetTranscriptionTier();
			if (textTier != null)
				_segmentCount = _tierCollection.GetTranscriptionTier().Segments.Count();
		}

		#region IProgressViewModel implementation
		/// ------------------------------------------------------------------------------------
		public int MaximumProgressValue
		{
			get { return _segmentCount; }
		}

		/// ------------------------------------------------------------------------------------
		public int CurrentProgressValue { get; private set; }

		/// ------------------------------------------------------------------------------------
		public string StatusString { get; private set; }

		/// ------------------------------------------------------------------------------------
		public bool Canceled
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		public void Cancel()
		{
		}

		/// ------------------------------------------------------------------------------------
		public void Start()
		{
			_worker = new BackgroundWorker();
			_worker.WorkerSupportsCancellation = true;
			_worker.WorkerReportsProgress = true;
			_worker.ProgressChanged += HandleWorkerProgressChanged;
			_worker.RunWorkerCompleted += HandleWorkerFinished;
			_worker.DoWork += BuildXml;
			_worker.RunWorkerAsync();
			while (_worker.IsBusy) { Application.DoEvents(); }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		private void BuildXml(object sender, DoWorkEventArgs e)
		{
			if (OnUpdateStatus != null)
			{
				StatusString = LocalizationManager.GetString(
					"SessionsView.Transcription.TextAnnotationEditor.ExportingToFLExInterlinear.ProgressDlg.ProgressMsg",
					"Exporting...");

				OnUpdateStatus(this, EventArgs.Empty);
			}

			GetPopulatedRootElement().Save(_outputFilePath);
		}

		/// ------------------------------------------------------------------------------------
		void HandleWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (OnUpdateProgress != null)
			{
				CurrentProgressValue = e.ProgressPercentage;
				OnUpdateProgress(this, EventArgs.Empty);
			}
		}

		/// ------------------------------------------------------------------------------------
		void HandleWorkerFinished(object sender, RunWorkerCompletedEventArgs e)
		{
			if (OnFinished != null)
			{
				StatusString = LocalizationManager.GetString(
					"SessionsView.Transcription.TextAnnotationEditor.ExportingToFLExInterlinear.ProgressDlg.FinsihedMsg",
					"Finished Exporting");

				OnFinished.Invoke(this, new ProgressFinishedArgs(false, null));
			}
		}

		/// ------------------------------------------------------------------------------------
		public XElement GetPopulatedRootElement()
		{
			var rootElement = CreateRootElement();

			rootElement.Element("interlinear-text").Element("paragraphs").Add(
				CreateParagraphElements());

			rootElement.Element("interlinear-text").Add(CreateLanguagesElement(
				new[] { _wsTranscriptionId, _wsFreeTranslationId }));

			rootElement.Element("interlinear-text").Add(
				new XElement("media-files", new XAttribute("offset-type", ""),
					new XElement("media", new XAttribute("guid", _mediaFileGuid), new XAttribute("location", _mediaFilePath))));

			if (!string.IsNullOrEmpty(_sourceFilePath)) rootElement.Element("interlinear-text").Element("media-files").Add(
				new XElement("media", new XAttribute("guid", _sourceFileGuid), new XAttribute("location", _sourceFilePath)));

			return rootElement;
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateRootElement()
		{
			return new XElement("document", new XElement("interlinear-text",
				CreateItemElement(_wsFreeTranslationId, "title", _title),
				new XElement("paragraphs")));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateLanguagesElement(IEnumerable<string> langIds)
		{
			var element = new XElement("languages");

			if (langIds != null)
			{
				foreach (var id in langIds)
					element.Add(new XElement("language", new XAttribute("lang", id)));
			}

			return element;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<XElement> CreateParagraphElements()
		{
			var transcriptionTier = _tierCollection.GetTranscriptionTier();

			// TODO: This will need refactoring when display name is localizable.
			var translationTier = _tierCollection.GetDependentTextTiers()
				.FirstOrDefault(t => t.DisplayName.ToLower() == TextTier.FreeTranslationTierDisplayName.ToLower());

			var segmentList = transcriptionTier.Segments.ToArray();

			var timeTier = _tierCollection.GetTimeTier();

			var timeSegmentList = timeTier.Segments.ToArray();


			for (int i = 0; i < segmentList.Length; i++)
			{
				// _worker will be null during tests.
				if (_worker != null)
					_worker.ReportProgress(i + 1);

				int startTime = (int)Math.Round(timeSegmentList[i].Start * 1000);
				int endTime = (int)Math.Round(timeSegmentList[i].End * 1000);

				if (translationTier != null)
				{
					Segment freeTranslationSegment;
					translationTier.TryGetSegment(i, out freeTranslationSegment);
					yield return CreateSingleParagraphElement(segmentList[i].Text,
						(freeTranslationSegment != null ? freeTranslationSegment.Text : null),
						startTime.ToString(),
						endTime.ToString()
						);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateSingleParagraphElement(string transcription, string freeTranslation, string start, string end)
		{
			//	var transcriptionElement = CreateSingleWordElement(transcription);
			var phraseElement = new XElement("phrase",
				new XAttribute("begin-time-offset", start), new XAttribute("end-time-offset", end), new XAttribute("media-file", _mediaFileGuid));

			phraseElement.Add(CreateItemElement(_wsFreeTranslationId, "segnum", _currentSegment++.ToString()));
			phraseElement.Add(CreateItemElement(_wsTranscriptionId, "txt", transcription));

			if (freeTranslation != null)
				phraseElement.Add(CreateItemElement(_wsFreeTranslationId, "gls", freeTranslation));

			phraseElement.Add(new XElement("words", null));

			return new XElement("paragraph", new XElement("phrases", phraseElement));
		}

		/// ------------------------------------------------------------------------------------
		//public XElement CreateSingleWordElement(string text)
		//{
		//    return new XElement("words", new XElement("word",
		//        CreateItemElement(_wsTranscriptionId, "txt", text)));
		//}

		/// ------------------------------------------------------------------------------------
		public XElement CreateItemElement(string langId, string type, string text)
		{
			return new XElement("item", new XAttribute("type", type),
				new XAttribute("lang", langId), text);
		}

		/// ------------------------------------------------------------------------------------
		public static void Save(string outputFilePath, string title, TierCollection tierCollection,
			string wsTranscriptionId, string wsFreeTranslationId,
			string mediaFilePath, string sourceFilePath)
		{
			var helper = new FLExTextExporter(outputFilePath, title,
				tierCollection, wsTranscriptionId, wsFreeTranslationId, mediaFilePath, sourceFilePath);

			var caption = LocalizationManager.GetString(
					"SessionsView.Transcription.TextAnnotationEditor.ExportingToFLExInterlinear.ProgressDlg.Caption",
					"Exporting to FLEx Interlinear");

			using (var dlg = new ProgressDlg(helper, caption))
			{
				dlg.StartPosition = FormStartPosition.CenterScreen;
				dlg.ShowDialog();
			}

			UsageReporter.SendNavigationNotice("Export to FieldWorks.");
		}
	}
}
