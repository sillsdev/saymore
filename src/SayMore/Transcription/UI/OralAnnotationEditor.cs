using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Transcription.Model;
using SayMore.UI;
using SayMore.UI.ComponentEditors;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationEditor : EditorBase
	{
		//public delegate OralAnnotationEditor Factory(ComponentFile file, string imageKey);

		private bool _isFirstTimeActivated = true;

		/// ------------------------------------------------------------------------------------
		public OralAnnotationEditor(ComponentFile file) : base(file, null, "Audio")
		{
			InitializeComponent();
			Name = "OralAnnotations";
			_toolStrip.Renderer = new NoToolStripBorderRenderer();

			_oralAnnotationWaveViewer.Dock = DockStyle.Fill;
			_tableLayoutError.Dock = DockStyle.Fill;
			_labelError.Font = new Font(SystemFonts.IconTitleFont.FontFamily, 11, FontStyle.Bold);
			_textBoxError.Font = SystemFonts.IconTitleFont;
			_pictureBoxError.Image = SystemIcons.Exclamation.ToBitmap();

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

			//_buttonHelp.Click += delegate
			//{
			//    Program.ShowHelpTopic("/Using_Tools/Events_tab/Create_Annotation_File_overview.htm");
			//};
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			using (var dlg = new LoadingDlg())
			{
				dlg.Show(this);
				_tableLayoutError.Visible = false;
				_oralAnnotationWaveViewer.Visible = true;
				_buttonHelp.Enabled = _buttonPlay.Enabled = _buttonStop.Enabled = false;

				try
				{
					base.SetComponentFile(file);
					_oralAnnotationWaveViewer.LoadAnnotationAudioFile(file.PathToAnnotatedFile);
					_buttonHelp.Enabled = _buttonPlay.Enabled = true;
				}
				catch (Exception e)
				{
					_tableLayoutError.Visible = true;
					_oralAnnotationWaveViewer.Visible = false;
					_textBoxError.Text = Palaso.Reporting.ExceptionHelper.GetAllExceptionMessages(e);
					_textBoxError.Text = _textBoxError.Text.Replace("\n", "\r\n");
				}

				dlg.Close();
			}
		}

		/// ------------------------------------------------------------------------------------
		public override void Activated()
		{
			base.Activated();

			if (!_isFirstTimeActivated)
				return;

			SetComponentFile(_file);
			_oralAnnotationWaveViewer.Invalidate(true);
			_isFirstTimeActivated = false;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsOKSToLeaveEditor
		{
			get { return !_oralAnnotationWaveViewer.IsBusyLoading && base.IsOKSToLeaveEditor; }
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
			var oralAnnotationfile = (OralAnnotationComponentFile)_file;
			var textAnnotationFile = oralAnnotationfile.AssociatedComponentFile.GetAnnotationFile();
			var tier = (TimeOrderTier)textAnnotationFile.Tiers.FirstOrDefault(t => t is TimeOrderTier);
			OralAnnotationFileGenerator.Generate(tier, this);
			SetComponentFile(_file);
			_oralAnnotationWaveViewer.Invalidate(true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Update the tab text in case it was localized.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override void HandleStringsLocalized()
		{
			TabText = Program.GetString("EventsView.Transcription.GeneratedOralAnnotationView.TabText", "Generated Audio");
			base.HandleStringsLocalized();
		}
	}
}
