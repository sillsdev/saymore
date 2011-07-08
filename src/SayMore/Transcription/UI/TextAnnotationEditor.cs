using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;

namespace SayMore.Transcription.UI
{
	/// ----------------------------------------------------------------------------------------
	public partial class TextAnnotationEditor : EditorBase
	{
		public delegate TextAnnotationEditor Factory(ComponentFile file, string tabText, string imageKey);

		private readonly TextAnnotationEditorGrid _grid;
		private FileSystemWatcher _watcher;

		/// ------------------------------------------------------------------------------------
		public TextAnnotationEditor(ComponentFile file, string tabText, string imageKey)
			: base(file, tabText, imageKey)
		{
			InitializeComponent();
			Name = "Annotations";

			_comboPlaybackSpeed.Font = SystemFonts.IconTitleFont;

			_grid = new TextAnnotationEditorGrid();
			_grid.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			_grid.Margin = new Padding(0, 8, 0, 0);
			_tableLayout.Controls.Add(_grid, 0, 1);
			_tableLayout.SetColumnSpan(_grid, 2);

			// TODO: Internationalize
			_comboPlaybackSpeed.Items.AddRange(new[] {"100% (Normal)", "90%", "80%", "70%",
				"60%", "50%", "40%", "30%", "20%", "10%"});

			SetSpeedPercentageString(Settings.Default.AnnotationEditorPlaybackSpeed);
			_comboPlaybackSpeed.SelectedValueChanged += HandlePlaybackSpeedValueChanged;
			SetComponentFile(file);
		}

		/// ------------------------------------------------------------------------------------
		public override void SetComponentFile(ComponentFile file)
		{
			base.SetComponentFile(file);

			file.Load();
			_grid.Load(file as AnnotationComponentFile);
			SetupWatchingForFileChanges();
		}

		/// ------------------------------------------------------------------------------------
		private void HandlePlaybackSpeedValueChanged(object sender, EventArgs e)
		{
			int percentage = GetSpeedPercentageFromText(_comboPlaybackSpeed.SelectedItem as string);
			Settings.Default.AnnotationEditorPlaybackSpeed = percentage;
			_grid.SetPlaybackSpeed(percentage);
		}

		/// ------------------------------------------------------------------------------------
		private int GetSpeedPercentageFromText(string text)
		{
			text = text ?? string.Empty;
			text = text.Replace("%", string.Empty).Trim();
			int percentage;
			return (int.TryParse(text, out percentage) ? percentage : 100);
		}

		/// ------------------------------------------------------------------------------------
		private void SetSpeedPercentageString(int percentage)
		{
			var text = (percentage == 0 || percentage == 100 ?
				_comboPlaybackSpeed.Items[0] as string : string.Format("{0}%", percentage));

			int i = _comboPlaybackSpeed.FindStringExact(text);
			_comboPlaybackSpeed.SelectedIndex = (i >= 0 ? i : 0);
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnEditorAndChildrenLostFocus()
		{
			base.OnEditorAndChildrenLostFocus();
			_grid.Stop();
			_grid.Invalidate();
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnFormLostFocus()
		{
			base.OnFormLostFocus();
			OnEditorAndChildrenLostFocus();
		}

		#region Methods for tracking changes to the EAF file outside of SayMore
		/// ------------------------------------------------------------------------------------
		void SetupWatchingForFileChanges()
		{
			_watcher = new FileSystemWatcher(
				Path.GetDirectoryName(_file.PathToAnnotatedFile),
				Path.GetFileName(_file.PathToAnnotatedFile));

			_watcher.IncludeSubdirectories = false;
			_watcher.EnableRaisingEvents = true;
			_watcher.Changed += HandleAnnotationFileChanged;

			((AnnotationComponentFile)_file).PreSaveAction = () =>
			{
				if (_watcher != null)
					_watcher.EnableRaisingEvents = false;
			};

			((AnnotationComponentFile)_file).PostSaveAction = () =>
			{
				if (_watcher != null)
					_watcher.EnableRaisingEvents = true;
			};
		}

		/// ------------------------------------------------------------------------------------
		void HandleAnnotationFileChanged(object sender, FileSystemEventArgs e)
		{
			Invoke(new EventHandler((s, args) =>
			{
				_file.Load();
				_grid.Load(_file as AnnotationComponentFile);
			}));
		}

		/// ------------------------------------------------------------------------------------
		public override void Deactivate()
		{
			((AnnotationComponentFile)_file).PostSaveAction = null;
			((AnnotationComponentFile)_file).PostSaveAction = null;

			if (_watcher != null)
			{
				_watcher.Changed -= HandleAnnotationFileChanged;
				_watcher.Dispose();
				_watcher = null;
			}
		}

		#endregion
	}
}
