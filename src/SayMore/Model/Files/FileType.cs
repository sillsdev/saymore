using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Palaso.Progress;
using Palaso.UI.WindowsForms.ClearShare;
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

		public string Name { get; protected set; }
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
		public virtual string GetShowInFileExplorerMenuText()
		{
			return LocalizationManager.GetString("CommonToMultipleViews.FileList.Open.ShowInFileExplorerMenuText",
				"Show in File Explorer...");
		}

		/// ------------------------------------------------------------------------------------
		public virtual string GetOpenInAssociatedProgramMenuText()
		{
			return LocalizationManager.GetString("CommonToMultipleViews.FileList.Open.OpenInAssociatedProgramMenuText",
				"Open in Program Associated with this File ...");
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<IEditorProvider> GetEditorProviders(int hashCode, ComponentFile file)
		{
			List<IEditorProvider> editorList;
			IEnumerable<IEditorProvider> editors;

			if (_editors.TryGetValue(hashCode, out editors))
			{
				editorList = editors.ToList();
				foreach (var editor in editorList)
					editor.SetComponentFile(file);
			}
			else
			{
				editorList = new List<IEditorProvider>(GetNewSetOfEditorProviders(file));
				_editors[hashCode] = editorList;
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
			yield return new FileCommand(GetShowInFileExplorerMenuText(),
				FileCommand.HandleOpenInFileManager_Click, "open");

			yield return new FileCommand(GetOpenInAssociatedProgramMenuText(),
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
		/// <param name="personBasicEditorFactoryLazy">This is to get us around a circular
		/// dependency error in autofac.  NB: when we move to .net 4, this can be replaced by
		/// Lazy<Func<PersonBasicEditor.Factory></param>
		/// ------------------------------------------------------------------------------------
		public PersonFileType(Func<PersonBasicEditor.Factory> personBasicEditorFactoryLazy)
			: base("Person", p => p.ToLower().EndsWith(".person"))
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
			var col = SilGrid.CreateTextBoxColumn("id");
			col.HeaderText = "_L10N_:PeopleView.PeopleList.ColumnHeadings.Id!Id";
			col.DataPropertyName = "id";
			col.ReadOnly = true;
			col.Frozen = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = SilGrid.CreateImageColumn("consent");
			col.HeaderText = "_L10N_:PeopleView.PeopleList.ColumnHeadings.Consent!Consent";
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
			yield return _personBasicEditorFactoryLazy()(file, "Person");
			yield return new NotesEditor(file);
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FileCommand> GetCommands(string filePath)
		{
			yield return new FileCommand(GetShowInFileExplorerMenuText(),
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
			: base("Event", p => p.ToLower().EndsWith(Settings.Default.EventFileExtension.ToLower()))
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
			var col = SilGrid.CreateTextBoxColumn("id");
			col.HeaderText = "_L10N_:EventsView.EventsList.ColumnHeadings.Id!Id";
			col.DataPropertyName = "id";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = SilGrid.CreateTextBoxColumn("title");
			col.HeaderText = "_L10N_:EventsView.EventsList.ColumnHeadings.Title!Title";
			col.DataPropertyName = "title";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = SilGrid.CreateImageColumn("stages");
			col.HeaderText = "_L10N_:EventsView.EventsList.ColumnHeadings.Stages!Stages";
			col.DataPropertyName = "stages";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = SilGrid.CreateImageColumn("status");
			col.HeaderText = "_L10N_:EventsView.EventsList.ColumnHeadings.Status!Status";
			col.DataPropertyName = "status";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = SilGrid.CreateTextBoxColumn("date");
			col.HeaderText = "_L10N_:EventsView.EventsList.ColumnHeadings.Date!Date";
			col.DataPropertyName = "date";
			col.ReadOnly = true;
			col.Visible = false;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = SilGrid.CreateTextBoxColumn("genre");
			col.HeaderText = "_L10N_:EventsView.EventsList.ColumnHeadings.Genre!Genre";
			col.DataPropertyName = "genre";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.Visible = false;
			yield return col;

			col = SilGrid.CreateTextBoxColumn("location");
			col.HeaderText = "_L10N_:EventsView.EventsList.ColumnHeadings.Location!Location";
			col.DataPropertyName = "location";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.Visible = false;
			yield return col;
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return _eventBasicEditorFactoryLazy()(file, "Event");
			yield return new NotesEditor(file);
		}

		/// ------------------------------------------------------------------------------------
		public override string GetMetaFilePath(string pathToAnnotatedFile)
		{
			return pathToAnnotatedFile; //we are our own metadata file, there is no sidecar
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FileCommand> GetCommands(string filePath)
		{
			yield return new FileCommand(GetShowInFileExplorerMenuText(),
				FileCommand.HandleOpenInFileManager_Click, "open");
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon
		{
			get { return Resources.EventFileImage; }
		}
	}

	#endregion

	#region FileTypeWithContributors class
	/// ----------------------------------------------------------------------------------------
	public abstract class FileTypeWithContributors : FileType
	{
		protected readonly Func<ContributorsEditor.Factory> _contributorsEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		protected FileTypeWithContributors(string name, Func<string, bool> isMatchPredicate,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base(name, isMatchPredicate)
		{
			_contributorsEditorFactoryLazy = contributorsEditorFactoryLazy;
		}
	}

	#endregion

	#region AnnotationFileType class
	/// ----------------------------------------------------------------------------------------
	public class AnnotationFileType : FileTypeWithContributors
	{
		/// ------------------------------------------------------------------------------------
		public AnnotationFileType(Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base("Annotations",
				f => f.ToLower().EndsWith(Settings.Default.AnnotationFileExtension.ToLower()),
				contributorsEditorFactoryLazy)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return new TextAnnotationEditor(file, "Annotation");
			//yield return _contributorsEditorFactoryLazy()(file, null);
			//yield return new NotesEditor(file);
		}

		/// ------------------------------------------------------------------------------------
		public override string GetMetaFilePath(string pathToAnnotatedFile)
		{
			return pathToAnnotatedFile; //we are our own metadata file, there is no sidecar
		}
	}

	#endregion

	#region OralAnnotationFileType class
	/// ----------------------------------------------------------------------------------------
	public class OralAnnotationFileType : AudioFileType
	{
		/// ------------------------------------------------------------------------------------
		public OralAnnotationFileType(
			Func<AudioComponentEditor.Factory> audioComponentEditorFactoryLazy,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy) :
				base(audioComponentEditorFactoryLazy, contributorsEditorFactoryLazy)
		{
			Name = "OralAnnotations";
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsMatch(string path)
		{
			return path.ToLower().EndsWith(Settings.Default.OralAnnotationGeneratedFileSuffix.ToLower());
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return new OralAnnotationEditor(file);
			yield return _audioComponentEditorFactoryLazy()(file, null);
			//yield return _contributorsEditorFactoryLazy()(file, null);
			//yield return new NotesEditor(file);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// These are fields which are always available for files of this type
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FieldDefinition> FactoryFields
		{
			get { return GetBaseAudioFields(); }
		}
	}

	#endregion

	#region AudioVideoFileTypeBase class
	/// ----------------------------------------------------------------------------------------
	public abstract class AudioVideoFileTypeBase : FileTypeWithContributors
	{
		/// ------------------------------------------------------------------------------------
		protected AudioVideoFileTypeBase(string name, Func<string, bool> isMatchPredicate,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base(name, isMatchPredicate, contributorsEditorFactoryLazy)
		{
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return new StartAnnotatingEditor(file);
			yield return new ConvertToStandardAudioEditor(file);
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ComputedFieldInfo> GetComputedFields()
		{
			// TODO: Figure out how to localize the suffixes so english is saved in
			// the metadata file but the user sees them in their UI language.

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
				//Suffix = Program.Get____String("Model.Files.AudioVideoFileType.AudioBitRateSuffix", "kbps"),
				DataItemChooser = (info => info.AudioBitRate),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Video_Bit_Rate",
				Suffix = "kbps",
				//Suffix = Program.Get____String("Model.Files.AudioVideoFileType.VideoBitRateSuffix", "kbps"),
				DataItemChooser = (info => info.VideoBitRate),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Sample_Rate",
				Suffix = "Hz",
				//Suffix = Program.Get____String("Model.Files.AudioVideoFileType.SampleRateSuffix", "Hz"),
				DataItemChooser = (info => info.SamplesPerSecond),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Bit_Depth",
				Suffix = "bits",
				//Suffix = Program.Get____String("Model.Files.AudioVideoFileType.BitDepthSuffix", "bits"),
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
				//Suffix = Program.Get____String("Model.Files.AudioVideoFileType.FrameRateSuffix", "frames/second"),
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

			// TODO: Figure out how to localize these so english is saved in the
			// metadata file but the user sees them in their UI language.

			switch (channels)
			{
				case "-1": return string.Empty;
				case "0": return string.Empty;
				case "1": return "mono";
				case "2": return "stereo";
				//case "1": return Program.Get____String("Model.Files.AudioVideoFileType.MonoLabel", "mono");
				//case "2": return Program.Get____String("Model.Files.AudioVideoFileType.StereoLabel", "stereo");
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
		protected readonly Func<AudioComponentEditor.Factory> _audioComponentEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		public AudioFileType(
			Func<AudioComponentEditor.Factory> audioComponentEditorFactoryLazy,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base("Audio",
				p => Settings.Default.AudioFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext.ToLower())),
				contributorsEditorFactoryLazy)
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
		internal static IEnumerable<FieldDefinition> GetBaseAudioFields()
		{
			yield return new FieldDefinition("Duration") { ReadOnly = true };
			yield return new FieldDefinition("Channels") { ReadOnly = true };
			yield return new FieldDefinition("Bit_Depth") { ReadOnly = true };
			yield return new FieldDefinition("Sample_Rate") { ReadOnly = true };
			yield return new FieldDefinition("Audio_Bit_Rate") { ReadOnly = true };
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
				yield return new FieldDefinition("Device");
				yield return new FieldDefinition("Microphone");
				yield return new FieldDefinition("contributions");

				foreach (var fld in GetBaseAudioFields())
					yield return fld;
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
			yield return new AudioVideoPlayer(file, "Audio");
			yield return _audioComponentEditorFactoryLazy()(file, null);
			yield return _contributorsEditorFactoryLazy()(file, null);
			yield return new NotesEditor(file);

			foreach (var editor in base.GetNewSetOfEditorProviders(file))
				yield return editor;
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
				.Any(ext => p.ToLower().EndsWith(ext.ToLower())), contributorsEditorFactoryLazy)
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
				yield return new FieldDefinition("Video_Bit_Rate") { ReadOnly = true };
				yield return new FieldDefinition("Resolution") { ReadOnly = true };
				yield return new FieldDefinition("Frame_Rate") { ReadOnly = true };
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

				var menuText = LocalizationManager.GetString("CommonToMultipleViews.FileList.Convert.ExtractMp3AudioMenuText",
					"Extract Audio to mono MP3 File (low quality)");

				commands.Add(new FileCommand(menuText, ExtractMp3Audio, "convert"));

				menuText = LocalizationManager.GetString("CommonToMultipleViews.FileList.Convert.ExtractWavAudioMenuText",
					"Extract Audio to Wave File");

				commands.Add(new FileCommand(menuText, ExtractWavAudio, "convert"));
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

			WaitCursor.Show();
			//TODO...provide some progress

			// REVIEW: At some point, we should probably switch to using MPlayer/MEncoder to do this.
			var results = FFmpegRunner.ExtractMp3Audio(path, outputPath, 1 /*mono*/, new NullProgress());
			WaitCursor.Hide();

			if (results.ExitCode != 0)
				ErrorReport.NotifyUserOfProblem(GetFFmpegConversionError(), results.StandardError);
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

			// Using ExtractBestQualityWavAudio does not ensure pcm will be the audio format
			// extracted, so call ExtractPcmAudio instead. The only problem is that the bits
			// per sample must be specified. TODO: use ffmpeg to get the info from the video
			// file and look for the audio's bits per sample. then use that to pass to
			// ExtractPcmAudio.

			var results = FFmpegRunner.ExtractPcmAudio(path, outputPath, 16, 0, 0, new NullProgress());
			//var results = FFmpegRunner.ExtractBestQualityWavAudio(path, outputPath, 0 /* whatever*/, new NullProgress());
			Cursor.Current = Cursors.Default;

			if (results.ExitCode != 0)
				ErrorReport.NotifyUserOfProblem(GetFFmpegConversionError(), results.StandardError);
		}

		/// ------------------------------------------------------------------------------------
		private string GetFFmpegConversionError()
		{
			return LocalizationManager.GetString("CommonToMultipleViews.FileList.Convert.FailureMsg",
				"Something didn't work out. FFmpeg said (start reading from the end): {0}\n\n");

		}
		/// ------------------------------------------------------------------------------------
		private bool CheckConversionIsPossible(string outputPath)
		{
			if (!MediaInfo.HaveNecessaryComponents)
			{
				var msg = LocalizationManager.GetString("CommonToMultipleViews.FileList.Convert.FFmpegMissingErrorMsg",
					"SayMore could not find the proper FFmpeg on this computer. FFmpeg is required to do that conversion.");

				ErrorReport.NotifyUserOfProblem(msg);
				return false;
			}

			if (File.Exists(outputPath))
			{
				//todo ask the user (or don't offer this in the first place)
				//File.Delete(outputPath);

				var msg = LocalizationManager.GetString(
					"CommonToMultipleViews.FileList.Convert.FileAlreadyExistsDuringConversionErrorMsg",
					"Sorry, the file '{0}' already exists.");

				ErrorReport.NotifyUserOfProblem(msg, Path.GetFileName(outputPath));
				return false;
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return new AudioVideoPlayer(file, "Video");
			yield return _videoComponentEditorFactoryLazy()(file, null);
			yield return _contributorsEditorFactoryLazy()(file, null);
			yield return new NotesEditor(file);

			foreach (var editor in base.GetNewSetOfEditorProviders(file))
				yield return editor;
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
			: base("Image", p => Settings.Default.ImageFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext.ToLower())))
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
			yield return new ImageViewer(file);
			yield return _basicFieldGridEditorFactoryLazy()(file, null);
			yield return _contributorsEditorFactoryLazy()(file, null);
			yield return new NotesEditor(file);
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