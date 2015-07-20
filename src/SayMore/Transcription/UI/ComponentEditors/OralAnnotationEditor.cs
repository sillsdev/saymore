using System;
using System.IO;
using System.Windows.Forms;
using L10NSharp;
using SIL.Reporting;
using SIL.Windows.Forms.Miscellaneous;
using SayMore.Model.Files;
using SayMore.Media.MPlayer;
using SayMore.UI.ComponentEditors;
using SIL.Windows.Forms;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationEditor : EditorBase
	{
		private bool _isFirstTimeActivated = true;
		private string _fileTooLongMsgDisplayedForFile;

		/// ------------------------------------------------------------------------------------
		public OralAnnotationEditor(ComponentFile file) : base(file, null, "Audio")
		{
			Logger.WriteEvent("OralAnnotationEditor constructor. file = {0}", file);
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

			_buttonStop.Click += delegate { StopPlayback(); };

			_oralAnnotationWaveViewer.PlaybackStopped += PlaybackStopped;

			_oralAnnotationWaveViewer.CursorTimeChanged += HandleCursorTimeChanged;

			_buttonHelp.Click += delegate
			{
				Program.ShowHelpTopic("/Using_Tools/Sessions_tab/Generated_Audio_tab_overview.htm");
			};
		}

		/// ------------------------------------------------------------------------------------
		private void StopPlayback()
		{
			_oralAnnotationWaveViewer.Stop();
			_buttonStop.Enabled = false;
			_buttonPlay.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			_buttonPlay.Enabled = false;

			if (_file != file || _isFirstTimeActivated)
			{
				if (AssociatedComponentFile != null)
				{
					AssociatedComponentFile.PreGenerateOralAnnotationFileAction = null;
					AssociatedComponentFile.PostGenerateOralAnnotationFileAction = null;
				}

				base.SetComponentFile(file);

				var finfo = new FileInfo(_file.PathToAnnotatedFile);
				if (finfo.Exists && finfo.Length > 0)
					LoadFileAndResetUI(); // If it's length is 0, it will get loaded after generating below.

				file.PreDeleteAction = () =>
					_oralAnnotationWaveViewer.CloseAudioStream();
				AssociatedComponentFile.PreGenerateOralAnnotationFileAction = () =>
					_oralAnnotationWaveViewer.CloseAudioStream();
				AssociatedComponentFile.PostGenerateOralAnnotationFileAction = HandleOralAnnotationFileGenerated;
			}

			file.GenerateOralAnnotationFile(this, ComponentFile.GenerateOption.GenerateIfNeeded);

			_buttonHelp.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleOralAnnotationFileGenerated(bool generated)
		{
			if (!generated && _oralAnnotationWaveViewer.WaveControlLoaded)
			{
				_buttonPlay.Enabled = true;
			}
			else if (generated)
			{
				_fileTooLongMsgDisplayedForFile = null;
				try
				{
					LoadFileAndResetUI();
				}
				catch (Exception failure)
				{
					if ((failure is UnauthorizedAccessException || failure is IOException) && generated)
					{
						// Successfully generated, but now some new problem has surfaced.
						ErrorReport.NotifyUserOfProblem(failure, LocalizationManager.GetString(
							"SessionsView.Transcription.GeneratedOralAnnotationView.OralAnnotationFileProblem",
							"Problem occurred attempting to display generated oral annotation file."));
					}
					else
						throw;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		private void LoadFileAndResetUI()
		{
			WaitCursor.Show();
			try
			{
				_oralAnnotationWaveViewer.LoadAnnotationAudioFile(_file.PathToAnnotatedFile);
				_oralAnnotationWaveViewer.ResetWaveControlCursor();
				_buttonPlay.Enabled = true;
				_fileTooLongMsgDisplayedForFile = null;
			}
			catch (ArgumentOutOfRangeException e)
			{
				if (e.StackTrace.Contains("System.IO.FileStream.set_Position") &&
					e.StackTrace.Contains("NAudio.FileFormats.Wav.WaveFileChunkReader.ReadWaveHeader"))
				{
					if (_fileTooLongMsgDisplayedForFile != _file.PathToAnnotatedFile)
					{
						_fileTooLongMsgDisplayedForFile = _file.PathToAnnotatedFile;
						WaitCursor.Hide();
						ErrorReport.NotifyUserOfProblem(e, LocalizationManager.GetString(
							"SessionsView.Transcription.GeneratedOralAnnotationView.OralAnnotationFilePossiblyTooLarge",
							"The generated oral annotation file may be too large to display or play correctly."));
					}
				}
				else
					throw;
			}
			finally
			{
				WaitCursor.Hide();
			}
		}

		/// ------------------------------------------------------------------------------------
		private void PlaybackStopped(object sender, EventArgs eventArgs)
		{
			if (InvokeRequired)
			{
				Invoke(new Action(() => PlaybackStopped(sender, eventArgs)));
				return;
			}
			_buttonStop.Enabled = false;
			_buttonPlay.Enabled = !IsRegeneratingAudioFile;

			if (!Visible)
				_oralAnnotationWaveViewer.ResetWaveControlCursor();
		}

		/// ------------------------------------------------------------------------------------
		private void HandleCursorTimeChanged(Media.Audio.WaveControlBasic ctrl, TimeSpan cursorTime)
		{
			if (InvokeRequired)
			{
				Invoke(new Action(() => HandleCursorTimeChanged(ctrl, cursorTime)));
				return;
			}
			_labelCursorTime.Text = MediaPlayerViewModel.GetTimeDisplay(
			   (float)cursorTime.TotalSeconds, (float)_oralAnnotationWaveViewer.AudioLength.TotalSeconds);
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
			PrepareToDeactivate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnParentTabControlVisibleChanged()
		{
			StopPlayback();
			BeginInvoke(new Action(() => _oralAnnotationWaveViewer.ResetWaveControlCursor()));
		}

		/// ------------------------------------------------------------------------------------
		public override void PrepareToDeactivate()
		{
			StopPlayback();
			base.PrepareToDeactivate();
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
