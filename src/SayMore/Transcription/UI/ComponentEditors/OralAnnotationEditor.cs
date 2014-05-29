using System;
using System.IO;
using System.Windows.Forms;
using L10NSharp;
using Palaso.Reporting;
using Palaso.UI.WindowsForms.Miscellaneous;
using SayMore.Model.Files;
using SayMore.Media.MPlayer;
using SayMore.UI.ComponentEditors;
using Palaso.UI.WindowsForms;

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
			InitializeComponent();
			Name = "OralAnnotations";
			_toolStrip.Renderer = new NoToolStripBorderRenderer();
			_labelCursorTime.Text = string.Empty;

			_buttonRegenerate.Click += HandleRegenerateFileButtonClick;

			_buttonPlay.Click += delegate
			{
				_buttonStop.Enabled = true;
				_buttonPlay.Enabled = false;
			};

			_buttonStop.Click += delegate { StopPlayback(); };

			_buttonHelp.Click += delegate
			{
				Program.ShowHelpTopic("/Using_Tools/Sessions_tab/Generated_Audio_tab_overview.htm");
			};
		}

		/// ------------------------------------------------------------------------------------
		private void StopPlayback()
		{
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

				AssociatedComponentFile.PostGenerateOralAnnotationFileAction = HandleOralAnnotationFileGenerated;
			}

			file.GenerateOralAnnotationFile(this, ComponentFile.GenerateOption.GenerateIfNeeded);

			_buttonHelp.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		private void HandleOralAnnotationFileGenerated(bool generated)
		{
			if (!generated)
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
			_buttonHelp.Enabled = false;
			_buttonPlay.Enabled = false;
			_buttonStop.Enabled = false;
			_buttonRegenerate.Enabled = false;

			_file.GenerateOralAnnotationFile(this, ComponentFile.GenerateOption.RegenerateNow);
			SetComponentFile(_file);

			_buttonRegenerate.Enabled = true;
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
