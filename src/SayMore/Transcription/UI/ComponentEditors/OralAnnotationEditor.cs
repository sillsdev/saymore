using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Localization;
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
				_buttonPlay.Enabled = true;
			};

			_oralAnnotationWaveViewer.CursorTimeChanged += (c, time) =>
				_labelCursorTime.Text = MediaPlayerViewModel.GetTimeDisplay(
					(float)time.TotalSeconds, (float)_oralAnnotationWaveViewer.AudioLength.TotalSeconds);

			_buttonHelp.Click += delegate
			{
				Program.ShowHelpTopic("/Using_Tools/Events_tab/Generated_Audio_tab_overview.htm");
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
			_oralAnnotationWaveViewer.LoadAnnotationAudioFile(file.PathToAnnotatedFile);
			_buttonHelp.Enabled = _buttonPlay.Enabled = true;

			AssociatedComponentFile.PreGenerateOralAnnotationFileAction = () =>
				_oralAnnotationWaveViewer.CloseAudioStream();

			AssociatedComponentFile.PostGenerateOralAnnotationFileAction = () =>
				_oralAnnotationWaveViewer.LoadAnnotationAudioFile(file.PathToAnnotatedFile);
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
			_buttonHelp.Enabled = false;
			_buttonPlay.Enabled = false;
			_buttonStop.Enabled = false;
			_buttonRegenerate.Enabled = false;

			_oralAnnotationWaveViewer.CloseAudioStream();

			var oralAnnotationfile = (OralAnnotationComponentFile)_file;
			var eafFile = oralAnnotationfile.AssociatedComponentFile.GetAnnotationFile();
			var tier = (TimeTier)eafFile.Tiers.FirstOrDefault(t => t is TimeTier);
			oralAnnotationfile.AssociatedComponentFile.GenerateOralAnnotationFile(tier, this);
			SetComponentFile(_file);
			_oralAnnotationWaveViewer.Invalidate(true);

			_buttonRegenerate.Enabled = true;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = LocalizationManager.GetString("EventsView.Transcription.GeneratedOralAnnotationView.TabText", "Generated Audio");
			base.HandleStringsLocalized();
		}
	}
}
