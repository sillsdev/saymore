using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI;
using SayMore.UI.ComponentEditors;
using SayMore.UI.MediaPlayer;
using SilTools;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class OralAnnotationEditor : EditorBase
	{
		public delegate OralAnnotationEditor Factory(ComponentFile file, string tabText, string imageKey);

		private bool _isFirstTimeActivated = true;

		/// ------------------------------------------------------------------------------------
		public OralAnnotationEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "OralAnnotations";
			_toolStrip.Renderer = new NoToolStripBorderRenderer();

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
				dlg.StartPosition = FormStartPosition.CenterScreen;
				dlg.Show();
				Application.DoEvents();
				base.SetComponentFile(file);
				_oralAnnotationWaveViewer.LoadAnnotationAudioFile(file.PathToAnnotatedFile);
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
	}
}
