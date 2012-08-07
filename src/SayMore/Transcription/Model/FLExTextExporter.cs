using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Localization;
using Palaso.Reporting;
using SayMore.UI;
using SayMore.Utilities;
using SayMore.UI.LowLevelControls;

namespace SayMore.Transcription.Model
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

		/// ------------------------------------------------------------------------------------
		public FLExTextExporter(string outputFilePath, string title,
			TierCollection tierCollection, string wsTranscriptionId, string wsFreeTranslationId)
		{
			_outputFilePath = outputFilePath;
			_title = title;
			_tierCollection = (tierCollection ?? new TierCollection());
			_wsTranscriptionId = wsTranscriptionId;
			_wsFreeTranslationId = wsFreeTranslationId;

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

			for (int i = 0; i < segmentList.Length; i++)
			{
				// _worker will be null during tests.
				if (_worker != null)
					_worker.ReportProgress(i + 1);

				Segment freeTranslationSegment;
				translationTier.TryGetSegment(i, out freeTranslationSegment);
				yield return CreateSingleParagraphElement(segmentList[i].Text,
					(freeTranslationSegment != null ? freeTranslationSegment.Text : null));
			}
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateSingleParagraphElement(string transcription, string freeTranslation)
		{
			var transcriptionElement = CreateSingleWordElement(transcription);
			var phraseElement = new XElement("phrase", transcriptionElement);

			if (freeTranslation != null)
				phraseElement.Add(CreateItemElement(_wsFreeTranslationId, "gls", freeTranslation));

			return new XElement("paragraph", new XElement("phrases", phraseElement));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateSingleWordElement(string text)
		{
			return new XElement("words", new XElement("word",
				CreateItemElement(_wsTranscriptionId, "txt", text)));
		}

		/// ------------------------------------------------------------------------------------
		public XElement CreateItemElement(string langId, string type, string text)
		{
			return new XElement("item", new XAttribute("type", type),
				new XAttribute("lang", langId), text);
		}

		/// ------------------------------------------------------------------------------------
		public static void Save(string outputFilePath, string title, TierCollection tierCollection,
			string wsTranscriptionId, string wsFreeTranslationId)
		{
			var helper = new FLExTextExporter(outputFilePath, title,
				tierCollection, wsTranscriptionId, wsFreeTranslationId);

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
