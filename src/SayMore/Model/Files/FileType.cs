using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Palaso.ClearShare;
using Palaso.Media;
using Palaso.Progress.LogBox;
using Palaso.Reporting;
using SayMore.Model.Fields;
using SayMore.Properties;
using SayMore.Transcription.UI;
using SayMore.UI.ComponentEditors;
using SilTools;

namespace SayMore.Model.Files
{
	#region FileType class
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Each file corresponds to a single kind of fileType. The FileType then tells
	/// us what controls are available for marking up, editing, or viewing that file.
	/// It also tells us which commands to offer in, for example, a context menu.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class FileType
	{
		protected Func<BasicFieldGridEditor.Factory> _basicFieldGridEditorFactoryLazy;
		protected Func<string, bool> _isMatchPredicate;

		protected readonly Dictionary<int, IEnumerable<IEditorProvider>> _editors =
			new Dictionary<int, IEnumerable<IEditorProvider>>();

		public string Name { get; private set; }
		public virtual string TypeDescription { get; protected set; }
		public virtual Image SmallIcon { get; protected set; }
		public virtual string FileSize { get; protected set; }

		/// ------------------------------------------------------------------------------------
		public static FileType Create(string name, string matchForEndOfFileName)
		{
			return new FileType(name, p => p.EndsWith(matchForEndOfFileName));
		}

		/// ------------------------------------------------------------------------------------
		public static FileType Create(string name, string[] matchesForEndOfFileName)
		{
			return new FileType(name, p => matchesForEndOfFileName.Any(p.EndsWith));
		}

		/// ------------------------------------------------------------------------------------
		public FileType(string name, Func<string, bool>isMatchPredicate)
		{
			Name = name;
			_isMatchPredicate = isMatchPredicate;
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsMatch(string path)
		{
			return _isMatchPredicate(path);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string FieldsGridSettingsName
		{
			get { return "UnknownFileFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<IEditorProvider> GetEditorProviders(int hashCode, ComponentFile file)
		{
			IEnumerable<IEditorProvider> editorList;
			if (!_editors.TryGetValue(hashCode, out editorList))
			{
				editorList = new List<IEditorProvider>(GetNewSetOfEditorProviders(file));
				_editors[hashCode] = editorList;
			}
			else
			{
				foreach (var editor in editorList)
					editor.SetComponentFile(file);
			}

			return editorList;
		}

		/// ------------------------------------------------------------------------------------
		protected virtual IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return new DiagnosticsFileInfoControl(file);
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<FileCommand> GetCommands(string filePath)
		{
			yield return new FileCommand("Show in File Explorer...",
				FileCommand.HandleOpenInFileManager_Click, "open");

			yield return new FileCommand("Open in Program Associated with this File ...",
				FileCommand.HandleOpenInApp_Click, "open");
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsForUnknownFileTypes
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsAudioOrVideo
		{
			get { return IsAudio || IsVideo; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsAudio
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool IsVideo
		{
			get { return false; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<FieldDefinition> FactoryFields
		{
			get { return new List<FieldDefinition>(0); }
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Name;
		}

		/// ------------------------------------------------------------------------------------
		public virtual string GetMetaFilePath(string pathToAnnotatedFile)
		{
			return pathToAnnotatedFile + Settings.Default.MetadataFileExtension;
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool GetIsCustomFieldId(string key)
		{
			return !FactoryFields.Any(f => f.Key == key);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool GetIsReadonly(string key)
		{
			var field = FactoryFields.FirstOrDefault(f => f.Key == key);
			return (field != null && field.ReadOnly);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool GetShowInPresetOptions(string key)
		{
			return (!GetIsCustomFieldId(key) && !GetIsReadonly(key));
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ComputedFieldInfo> GetComputedFields()
		{
			return new List<ComputedFieldInfo>(0);
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<DataGridViewColumn> GetFieldsShownInGrid()
		{
			return new List<DataGridViewColumn>(0);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Migrate(ComponentFile file)
		{
		}
	}

	#endregion

	#region PersonFileType class
	/// ----------------------------------------------------------------------------------------
	public class PersonFileType : FileType
	{
		private readonly Func<PersonBasicEditor.Factory> _personBasicEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// <param name="personBasicEditorFactoryLazy">This is to get us around a circular dependency
		/// error in autofac.  NB: when we move to .net 4, this can be replaced by Lazy<Func<PersonBasicEditor.Factory></param>
		/// ------------------------------------------------------------------------------------
		public PersonFileType(Func<PersonBasicEditor.Factory> personBasicEditorFactoryLazy)
			: base("Person", p => p.EndsWith(".person"))
		{
			_personBasicEditorFactoryLazy = personBasicEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override string GetMetaFilePath(string pathToAnnotatedFile)
		{
			return pathToAnnotatedFile; //we are our own metadata file, there is no sidecar
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// These are fields which are always available for files of this type
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FieldDefinition> FactoryFields
		{
			get
			{
				return from key in new []
				{
					"id",
					"primaryLanguage",
					"primaryLanguageLearnedIn",
					"otherLanguage0",
					"otherLanguage1",
					"otherLanguage2",
					"otherLanguage3",
					"fathersLanguage",
					"mothersLanguage",
					"pbOtherLangFather0",
					"pbOtherLangFather1",
					"pbOtherLangFather2",
					"pbOtherLangFather3",
					"pbOtherLangMother0",
					"pbOtherLangMother3",
					"pbOtherLangMother2",
					"pbOtherLangMother1",
					"birthYear",
					"gender",
					"howToContact",
					"education",
					"primaryOccupation",
					"picture",
					"notes"
				}
				select new FieldDefinition(key);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<DataGridViewColumn> GetFieldsShownInGrid()
		{
			var col = SilGrid.CreateTextBoxColumn("Id");
			col.DataPropertyName = "id";
			col.ReadOnly = true;
			col.Frozen = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = SilGrid.CreateImageColumn("Consent");
			col.DataPropertyName = "consent";
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName
		{
			get { return "PersonCustomFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			var text = LocalizationManager.LocalizeString("PersonInfoEditor.PersonTabText", "Person");
			yield return _personBasicEditorFactoryLazy()(file, text, "Person");

			text = LocalizationManager.LocalizeString("PersonInfoEditor.NotesTabText", "Notes");
			yield return new NotesEditor(file, text, "Notes");
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FileCommand> GetCommands(string filePath)
		{
			yield return new FileCommand("Show in File Explorer...",
				FileCommand.HandleOpenInFileManager_Click, "open");
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get { return Resources.PersonFileImage; }
		}
	}

	#endregion

	#region EventFileType class
	/// ----------------------------------------------------------------------------------------
	public class EventFileType : FileType
	{
		private readonly Func<EventBasicEditor.Factory> _eventBasicEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// <param name="eventBasicEditorFactoryLazy">This is to get us around a circular dependency
		/// error in autofac.  NB: when we move to .net 4, this can be replaced by
		/// Lazy<Func<EventBasicEditor.Factory></param>
		/// ------------------------------------------------------------------------------------
		public EventFileType(Func<EventBasicEditor.Factory> eventBasicEditorFactoryLazy)
			: base("Event", p => p.EndsWith(Settings.Default.EventFileExtension))
		{
			_eventBasicEditorFactoryLazy = eventBasicEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName
		{
			get { return "EventCustomFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// These are fields which are always available for files of this type
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FieldDefinition> FactoryFields
		{
			get
			{
				return from key in new[]
				{
					"status",
					"stages",
					"date",
					"synopsis",
					"access",
					"location",
					"setting",
					"situation",
					"genre",
					"participants",
					"title",
					"contributions",
					"notes"
				}
				select new FieldDefinition(key);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<DataGridViewColumn> GetFieldsShownInGrid()
		{
			var col = SilGrid.CreateTextBoxColumn("Id");
			col.DataPropertyName = "id";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			//col.Frozen = true;
			yield return col;

			col = SilGrid.CreateTextBoxColumn("Title");
			col.DataPropertyName = "title";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = SilGrid.CreateImageColumn("Stages");
			col.DataPropertyName = "stages";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = SilGrid.CreateImageColumn("Status");
			col.DataPropertyName = "status";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = SilGrid.CreateTextBoxColumn("Date");
			col.DataPropertyName = "date";
			col.ReadOnly = true;
			col.Visible = false;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = SilGrid.CreateTextBoxColumn("Genre");
			col.DataPropertyName = "genre";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.Visible = false;
			yield return col;

			col = SilGrid.CreateTextBoxColumn("Location");
			col.DataPropertyName = "location";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.Visible = false;
			yield return col;
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			var text = LocalizationManager.LocalizeString("EventInfoEditor.EventTabText", "Event");
			yield return _eventBasicEditorFactoryLazy()(file, text, "Event");

			text = LocalizationManager.LocalizeString("EventInfoEditor.NotesTabText", "Notes");
			yield return new NotesEditor(file, text, "Notes");
		}

		/// ------------------------------------------------------------------------------------
		public override string GetMetaFilePath(string pathToAnnotatedFile)
		{
			return pathToAnnotatedFile; //we are our own metadata file, there is no sidecar
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FileCommand> GetCommands(string filePath)
		{
			yield return new FileCommand("Show in File Explorer...",
				FileCommand.HandleOpenInFileManager_Click, "open");
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get { return Resources.EventFileImage; }
		}
	}

	#endregion

	#region AudioVideoFileTypeBase class
	/// ----------------------------------------------------------------------------------------
	public abstract class AudioVideoFileTypeBase : FileType
	{
		protected readonly Func<ContributorsEditor.Factory> _contributorsEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		protected AudioVideoFileTypeBase(string name, Func<string, bool> isMatchPredicate,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base(name, isMatchPredicate)
		{
			_contributorsEditorFactoryLazy = contributorsEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ComputedFieldInfo> GetComputedFields()
		{
			yield return new ComputedFieldInfo
			{
				Key = "Duration",
				DataItemChooser = (info => info.Duration),
				GetFormatedStatProvider = GetDurationStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Audio_Bit_Rate",
				Suffix = "kbps",
				DataItemChooser = (info => info.AudioBitRate),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Video_Bit_Rate",
				Suffix = "kbps",
				DataItemChooser = (info => info.VideoBitRate),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Sample_Rate",
				Suffix = "Hz",
				DataItemChooser = (info => info.SamplesPerSecond),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Bit_Depth",
				Suffix = "bits",
				DataItemChooser = (info => info.BitDepth == 0 ? null : info.BitDepth.ToString()),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Channels",
				DataItemChooser = (info => info.Channels.ToString()),
				GetFormatedStatProvider = GetChannelsStatistic,
			};

			yield return new ComputedFieldInfo
			{
				Key = "Resolution",
				DataItemChooser = (info => info.Resolution),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Frame_Rate",
				Suffix = "frames/second",
				DataItemChooser = (info => info.FramesPerSecond),
				GetFormatedStatProvider = GetStringStatistic
			};
		}

		/// ------------------------------------------------------------------------------------
		public string GetStringStatistic(MediaFileInfo info,
			Func<MediaFileInfo, object> dataItemChooser, string suffix)
		{
			if (info == null)
				return string.Empty;

			var dataVal = dataItemChooser(info);
			if (dataVal == null)
				return string.Empty;

			return string.Format("{0} {1}", dataVal, suffix).Trim();
		}

		/// ------------------------------------------------------------------------------------
		public string GetChannelsStatistic(MediaFileInfo info,
			Func<MediaFileInfo, object> dataItemChooser, string suffix)
		{
			var channels = GetStringStatistic(info, dataItemChooser, string.Empty);

			switch (channels)
			{
				case "-1": return string.Empty;
				case "0": return string.Empty;
				case "1": return "mono";
				case "2": return "stereo";
			}

			return channels;
		}

		/// ------------------------------------------------------------------------------------
		public string GetDurationStatistic(MediaFileInfo info,
			Func<MediaFileInfo, object> dataItemChooser, string suffix)
		{
			var duration = GetStringStatistic(info, dataItemChooser, string.Empty);

			// Strip off milliseconds.
			int i = duration.LastIndexOf('.');
			return (i < 0 ? duration : duration.Substring(0, i));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Migrates the Recordist and speaker fields to contributions for the specified
		/// component file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void Migrate(ComponentFile file)
		{
			AddContribution(file, "Recordist", "recorder");
			AddContribution(file, "Speaker", "speaker");
			AddContribution(file, "speaker", "speaker");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Migrates the specified field to a contribution, adds it to the list of
		/// contributions already associated with the specified file (or creates a new list
		/// of contributions) and removes the fieldId from the metadata. Finally, the
		/// file's metadata is saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void AddContribution(ComponentFile file, string fieldId, string roleCode)
		{
			var value = file.GetStringValue(fieldId, null);
			if (value == null)
				return;

			var system = new OlacSystem();
			var role = system.GetRoleByCodeOrThrow(roleCode);
			var collection =
				file.GetValue("contributions", new ContributionCollection()) as ContributionCollection;

			var contributor = new Contribution(value, role);
			contributor.Date = file.GetCreateDate();
			collection.Add(contributor);
			string failureMessage;
			file.SetValue("contributions", collection, out failureMessage);
			file.RemoveField(fieldId);
			file.Save();
		}
	}

	#endregion

	#region AudioFileType class
	/// ----------------------------------------------------------------------------------------
	public class AudioFileType : AudioVideoFileTypeBase
	{
		private readonly Func<AudioComponentEditor.Factory> _audioComponentEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		public AudioFileType(
			Func<AudioComponentEditor.Factory> audioComponentEditorFactoryLazy,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base("Audio", p => Settings.Default.AudioFileExtensions.Cast<string>()
				.Any(ext => p.ToLower().EndsWith(ext)), contributorsEditorFactoryLazy)
		{
			_audioComponentEditorFactoryLazy = audioComponentEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsAudio
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// These are fields which are always available for files of this type
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FieldDefinition> FactoryFields
		{
			get { return AudioFields; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is separated out so that video can reuse it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		internal static IEnumerable<FieldDefinition> AudioFields
		{
			get
			{
				foreach (var key in new[] { "Device", "Microphone", "contributions" })
					yield return new FieldDefinition(key);

				foreach (var key in new[] { "Duration", "Channels", "Bit_Depth", "Sample_Rate", "Audio_Bit_Rate"})
					yield return new FieldDefinition(key) { ReadOnly = true };
			}
		}

		/// ------------------------------------------------------------------------------------
		public override bool GetShowInPresetOptions(string key)
		{
			return (base.GetShowInPresetOptions(key) && key != "notes");
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName
		{
			get { return "AudioFileFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			var text = LocalizationManager.LocalizeString("AudioFileInfoEditor.PlaybackTabText", "Audio");
			yield return new AudioVideoPlayer(file, text, "Audio");

			text = LocalizationManager.LocalizeString("AudioFileInfoEditor.SegmentTabText", "Segment");
			yield return new SegmentEditor(file, text, "Segment");

			text = LocalizationManager.LocalizeString("AudioFileInfoEditor.PropertiesTabText", "Properties");
			yield return _audioComponentEditorFactoryLazy()(file, text, null);

			text = LocalizationManager.LocalizeString("AudioFileInfoEditor.Contributors", "Contributors");
			yield return _contributorsEditorFactoryLazy()(file, text, null);

			text = LocalizationManager.LocalizeString("AudioFileInfoEditor.NotesTabText", "Notes");
			yield return new NotesEditor(file, text, "Notes");
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get {return Resources.AudioFileImage;}
		}
	}

	#endregion

	#region VideoFileType class
	/// ----------------------------------------------------------------------------------------
	public class VideoFileType : AudioVideoFileTypeBase
	{
		private readonly Func<VideoComponentEditor.Factory> _videoComponentEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		public VideoFileType(
			Func<VideoComponentEditor.Factory> videoComponentEditorFactoryLazy,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base("Video", p => Settings.Default.VideoFileExtensions.Cast<string>()
				.Any(ext => p.ToLower().EndsWith(ext)), contributorsEditorFactoryLazy)
		{
			_videoComponentEditorFactoryLazy = videoComponentEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsVideo
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// These are fields which are always available for files of this type
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FieldDefinition> FactoryFields
		{
			get
			{
				foreach (var field in AudioFileType.AudioFields)
					yield return field;

				// Add video only fields
				foreach (var key in new[] { "Video_Bit_Rate", "Resolution", "Frame_Rate" })
					yield return new FieldDefinition(key) { ReadOnly = true };
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName
		{
			get { return "VideoFileFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		public override bool GetShowInPresetOptions(string key)
		{
			return (base.GetShowInPresetOptions(key) && key != "notes");
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FileCommand> GetCommands(string filePath)
		{
			// TODO: Localize these strings.

			var commands = base.GetCommands(filePath).ToList();

			if (!File.Exists(filePath.Replace(Path.GetExtension(filePath), ".mp3")))
			{
				commands.Add(null); // Separator
				commands.Add(new FileCommand("Extract Audio to mono MP3 File (low quality)", ExtractMp3Audio, "convert"));
				commands.Add(new FileCommand("Extract Audio Wav File", ExtractWavAudio, "convert"));
			}

			return commands;
		}

		/// ------------------------------------------------------------------------------------
		private void ExtractMp3Audio(string path)
		{
			//var outputPath = path.Replace(Path.GetExtension(path), ".wav");
			var outputPath = path.Replace(Path.GetExtension(path), ".mp3");

			if (!CheckConversionIsPossible(outputPath))
				return;

			Cursor.Current = Cursors.WaitCursor;
			//TODO...provide some progress

			// REVIEW: At some point, we should probably switch to using MPlayer/MEncoder to do this.
			var results = FFmpegRunner.ExtractMp3Audio(path, outputPath, 1 /*mono*/, new NullProgress());
			Cursor.Current = Cursors.Default;

			if (results.ExitCode != 0)
			{
				// TODO: Localize this.
				ErrorReport.NotifyUserOfProblem(
						string.Format("Something didn't work out. FFmpeg said (start reading from the end): {0}{1}{2}",
							Environment.NewLine, Environment.NewLine, results.StandardError));
			}
		}

		/// ------------------------------------------------------------------------------------
		private void ExtractWavAudio(string path)
		{
			var outputPath = path.Replace(Path.GetExtension(path), ".wav");

			if (!CheckConversionIsPossible(outputPath))
				return;

			Cursor.Current = Cursors.WaitCursor;
			//TODO...provide some progress

			// REVIEW: At some point, we should probably switch to using MPlayer/MEncoder to do this.
			var results = FFmpegRunner.ExtractBestQualityWavAudio(path, outputPath, 0 /* whatever*/, new NullProgress());
			Cursor.Current = Cursors.Default;

			if (results.ExitCode != 0)
			{
				// TODO: Localize this.
				ErrorReport.NotifyUserOfProblem(
						string.Format("Something didn't work out. FFmpeg said (start reading from the end): {0}{1}{2}",
							Environment.NewLine, Environment.NewLine, results.StandardError));
			}
		}

		/// ------------------------------------------------------------------------------------
		private bool CheckConversionIsPossible(string outputPath)
		{
			// TODO: Localize these strings.

			if (!MediaInfo.HaveNecessaryComponents)
			{
				MessageBox.Show("SayMore could not find the proper FFmpeg on this computer. FFmpeg is required to do that conversion.");
				return false;
			}

			if (File.Exists(outputPath))
			{
				//todo ask the user (or don't offer this in the first place)
				//File.Delete(outputPath);

				ErrorReport.NotifyUserOfProblem(
					string.Format("Sorry, the file '{0}' already exists.", Path.GetFileName(outputPath)));

				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			// TODO: Localize these strings.

			var text = LocalizationManager.LocalizeString("VideoFileInfoEditor.PlaybackTabText", "Video");
			yield return new AudioVideoPlayer(file, text, "Video");

			text = LocalizationManager.LocalizeString("VideoFileInfoEditor.PropertiesTabText", "Properties");
			yield return _videoComponentEditorFactoryLazy()(file, text, null);

			text = LocalizationManager.LocalizeString("VideoFileInfoEditor.Contributors", "Contributors");
			yield return _contributorsEditorFactoryLazy()(file, text, null);

			text = LocalizationManager.LocalizeString("VideoFileInfoEditor.NotesTabText", "Notes");
			yield return new NotesEditor(file, text, "Notes");
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get { return Resources.VideoFileImage; }
		}
	}

	#endregion

	#region ImageFileType class
	/// ----------------------------------------------------------------------------------------
	public class ImageFileType : FileType
	{
		private readonly Func<ContributorsEditor.Factory> _contributorsEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		public ImageFileType(
			Func<BasicFieldGridEditor.Factory> basicFieldGridEditorFactoryLazy,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base("Image", p => Settings.Default.ImageFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
			_basicFieldGridEditorFactoryLazy = basicFieldGridEditorFactoryLazy;
			_contributorsEditorFactoryLazy = contributorsEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName
		{
			get { return "ImageFileFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			var text = LocalizationManager.LocalizeString("ImageFileInfoEditor.ViewTabText", "Image");
			yield return new ImageViewer(file, text, "Image");

			text = LocalizationManager.LocalizeString("ImageFileInfoEditor.PropertiesTabText", "Properties");
			yield return _basicFieldGridEditorFactoryLazy()(file, text, null);

			text = LocalizationManager.LocalizeString("ImageFileInfoEditor.Contributors", "Contributors");
			yield return _contributorsEditorFactoryLazy()(file, text, null);

			text = LocalizationManager.LocalizeString("ImageFileInfoEditor.NotesTabText", "Notes");
			yield return new NotesEditor(file, text, "Notes");
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get { return Resources.ImageFileImage; }
		}
	}

	#endregion

	#region ComputedFieldInfo class
	/// ----------------------------------------------------------------------------------------
	public class ComputedFieldInfo
	{
		public string Key { get; set; }
		public string Suffix { get; set; }
		public Func<MediaFileInfo, object> DataItemChooser;

		public Func<MediaFileInfo, Func<MediaFileInfo, object>,
			string, string> GetFormatedStatProvider { get; set; }

		/// ------------------------------------------------------------------------------------
		public ComputedFieldInfo()
		{
			Suffix = string.Empty;
		}
	}

	#endregion
}