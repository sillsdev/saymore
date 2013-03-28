using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using Palaso.Reporting;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.Media.MPlayer;
using SayMore.UI.ComponentEditors;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationEditor : EditorBase
	{
		private bool _isFirstTimeActivated = true;

		/// ------------------------------------------------------------------------------------
		public OralAnnotationEditor(ComponentFile file) : base(file, null, "Audio")
		{
			InitializeComponent();
			Name = "OralAnnotations";
			_toolStrip.Renderer = new NoToolStripBorderRenderer();
			_labelCursorTime.Text = string.Empty;

			_oralAnnotationWaveViewer.Dock = DockStyle.Fill;
			_oralAnnotationWaveViewer.ZoomPercentage = 300;

			_buttonRegenerate.Click += HandleRegenerateFileButtonClick;

			_buttonPlay.Click += delegate
			{
				_oralAnnotationWaveViewer.Play();
				_buttonStop.Enabled = true;
				_buttonPlay.Enabled = false;
			};

			_buttonStop.Click += delegate
			{
				_oralAnnotationWaveViewer.Stop();
				_buttonStop.Enabled = false;
				_buttonPlay.Enabled = true;
			};

			_oralAnnotationWaveViewer.PlaybackStopped += delegate
			{
				_buttonStop.Enabled = false;
				_buttonPlay.Enabled = !IsRegeneratingAudioFile;
			};

			_oralAnnotationWaveViewer.CursorTimeChanged += (c, time) =>
				_labelCursorTime.Text = MediaPlayerViewModel.GetTimeDisplay(
					(float)time.TotalSeconds, (float)_oralAnnotationWaveViewer.AudioLength.TotalSeconds);

			_buttonHelp.Click += delegate
			{
				Program.ShowHelpTopic("/Using_Tools/Sessions_tab/Generated_Audio_tab_overview.htm");
			};
		}

		/// ------------------------------------------------------------------------------------
		private ComponentFile AssociatedComponentFile
		{
			get
			{
				return _file == null ? null :
					((OralAnnotationComponentFile)_file).AssociatedComponentFile;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			if (AssociatedComponentFile != null)
			{
				AssociatedComponentFile.PreGenerateOralAnnotationFileAction = null;
				AssociatedComponentFile.PostGenerateOralAnnotationFileAction = null;
			}

			base.SetComponentFile(file);

			_buttonPlay.Enabled = false;

			file.PreDeleteAction = () =>
				_oralAnnotationWaveViewer.CloseAudioStream();
			AssociatedComponentFile.PreGenerateOralAnnotationFileAction = () =>
				_oralAnnotationWaveViewer.CloseAudioStream();
			AssociatedComponentFile.PostGenerateOralAnnotationFileAction = HandleOralAnnotationFileGenerated;
			file.GenerateOralAnnotationFile(this, ComponentFile.GenerateOption.GenerateIfNeeded);

			_buttonHelp.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleOralAnnotationFileGenerated(bool generated)
		{
			var finfo = new FileInfo(_file.PathToAnnotatedFile);
			if (finfo.Exists && finfo.Length > 0)
			{
				try
				{
					_oralAnnotationWaveViewer.LoadAnnotationAudioFile(_file.PathToAnnotatedFile);
					_buttonPlay.Enabled = true;
				}
				catch (Exception failure)
				{
					if (failure is UnauthorizedAccessException || failure is IOException)
					{
						if (generated) // Successfully generated, but now some new problem has surfaced.
						{
							ErrorReport.NotifyUserOfProblem(failure, LocalizationManager.GetString(
								"SessionsView.Transcription.GeneratedOralAnnotationView.OralAnnotationFileProblem",
								"Problem occurred attempting to display generated oral annotation file."));
						}
					}
					else
						throw;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public override bool ComponentFileDeletionInitiated(ComponentFile file)
		{
			if (base.ComponentFileDeletionInitiated(file))
			{
				_oralAnnotationWaveViewer.CloseAudioStream();
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public override void Activated()
		{
			base.Activated();

			if (!_isFirstTimeActivated)
				return;

			SetComponentFile(_file);
			_isFirstTimeActivated = false;
			_labelCursorTime.Font = Program.DialogFont;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormLostFocus()
		{
			base.OnFormLostFocus();
			OnEditorAndChildrenLostFocus();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEditorAndChildrenLostFocus()
		{
			base.OnEditorAndChildrenLostFocus();
			_oralAnnotationWaveViewer.Stop();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleRegenerateFileButtonClick(object sender, EventArgs e)
		{
			_oralAnnotationWaveViewer.Stop();

			_buttonHelp.Enabled = false;
			_buttonPlay.Enabled = false;
			_buttonStop.Enabled = false;
			_buttonRegenerate.Enabled = false;

			_oralAnnotationWaveViewer.CloseAudioStream();

			_file.GenerateOralAnnotationFile(this, ComponentFile.GenerateOption.RegenerateNow);
			SetComponentFile(_file);
			_oralAnnotationWaveViewer.Invalidate(true);

			_buttonRegenerate.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		private bool IsRegeneratingAudioFile
		{
			get { return !_buttonRegenerate.Enabled; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString("SessionsView.Transcription.GeneratedOralAnnotationView.TabText", "Generated Audio");
			base.HandleStringsLocalized();
		}
	}
}
