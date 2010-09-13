using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Localization;
using Palaso.CommandLineProcessing;
using Palaso.Media;
using Palaso.Reporting;
using SayMore.Model.Fields;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.UI.ComponentEditors;

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
			return new FileType(name, p => matchesForEndOfFileName.Any(ext => p.EndsWith(ext)));
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
				return from key in
					new []{
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

			//_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));
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
				return from key in
						   new[]{
								 "date",
								 "synopsis",
								 "access",
								 "location",
								 "setting",
								 "situation",
								 "genre",
								 "participants",
								 "title",
								 "notes"
						}
					   select new FieldDefinition(key);
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			var text = LocalizationManager.LocalizeString("EventInfoEditor.EventTabText", "Event");
			yield return _eventBasicEditorFactoryLazy()(file, text, "Event");

			text = LocalizationManager.LocalizeString("EventInfoEditor.NotesTabText", "Notes");
			yield return new NotesEditor(file, text, "Notes");

			//_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));
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
		/// ------------------------------------------------------------------------------------
		protected AudioVideoFileTypeBase(string name, Func<string, bool> isMatchPredicate)
			: base(name, isMatchPredicate)
		{
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ComputedFieldInfo> GetComputedFields()
		{
			yield return new ComputedFieldInfo
			{
				Key = "Duration",
				DataSetChooser = (info => info.Audio),
				DataItemChooser = (audio => ((MediaInfo.AudioInfo)audio).Duration),
				GetFormatedStatProvider = GetDurationStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Sample_Rate",
				Suffix = "Hz",
				DataSetChooser = (info => info.Audio),
				DataItemChooser = (audio => ((MediaInfo.AudioInfo)audio).SamplesPerSecond),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Bit_Depth",
				Suffix = "bits",
				DataSetChooser = (info => info.Audio),
				DataItemChooser = (audio => ((MediaInfo.AudioInfo)audio).BitDepth),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Channels",
				DataSetChooser = (info => info.Audio),
				DataItemChooser = (audio => ((MediaInfo.AudioInfo)audio).ChannelCount.ToString()),
				GetFormatedStatProvider = GetChannelsStatistic,
			};

			yield return new ComputedFieldInfo
			{
				Key = "Resolution",
				DataSetChooser = (info => info.Video),
				DataItemChooser = (video => ((MediaInfo.VideoInfo)video).Resolution),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Frame_Rate",
				Suffix = "frames/second",
				DataSetChooser = (info => info.Video),
				DataItemChooser = (video => ((MediaInfo.VideoInfo)video).FramesPerSecond),
				GetFormatedStatProvider = GetStringStatistic
			};
		}

		/// ------------------------------------------------------------------------------------
		public string GetStringStatistic(AudioVideoFileStatistics stats,
			Func<MediaInfo, object> dataSetChooser, Func<object, object> dataItemChooser,
			string suffix)
		{
			if (stats == null || stats.MediaInfo == null || dataSetChooser(stats.MediaInfo) == null)
				return string.Empty;

			return string.Format("{0} {1}", dataItemChooser(dataSetChooser(stats.MediaInfo)), suffix).Trim();
		}

		/// ------------------------------------------------------------------------------------
		public string GetChannelsStatistic(AudioVideoFileStatistics stats,
			Func<MediaInfo, object> dataSetChooser, Func<object, object> dataItemChooser,
			string suffix)
		{
			var channels = GetStringStatistic(stats, dataSetChooser, dataItemChooser, string.Empty);

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
		public string GetDurationStatistic(AudioVideoFileStatistics stats,
			Func<MediaInfo, object> dataSetChooser, Func<object, object> dataItemChooser,
			string suffix)
		{
			var duration = GetStringStatistic(stats, dataSetChooser, dataItemChooser, string.Empty);

			// Strip off milliseconds.
			int i = duration.LastIndexOf('.');
			return (i < 0 ? duration : duration.Substring(0, i));
		}
	}

	#endregion

	#region AudioFileType class
	/// ----------------------------------------------------------------------------------------
	public class AudioFileType : AudioVideoFileTypeBase
	{
		private readonly Func<AudioComponentEditor.Factory> _audioComponentEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		public AudioFileType(Func<AudioComponentEditor.Factory> audioComponentEditorFactoryLazy)
			: base("Audio",
			p => Settings.Default.AudioFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
			_audioComponentEditorFactoryLazy = audioComponentEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsAudioOrVideo
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
			get {return AudioFields;}
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
				foreach (var key in new[] { "Recordist", "Device", "Microphone" })
					yield return new FieldDefinition(key);

				foreach (var key in new[] { "Duration", "Channels", "Bit_Depth", "Sample_Rate" })
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

			text = LocalizationManager.LocalizeString("AudioFileInfoEditor.PropertiesTabText", "Properties");
			yield return _audioComponentEditorFactoryLazy()(file, text, null);

			text = LocalizationManager.LocalizeString("AudioFileInfoEditor.NotesTabText", "Notes");
			yield return new NotesEditor(file, text, "Notes");

			//_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));
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
		public VideoFileType(Func<VideoComponentEditor.Factory> videoComponentEditorFactoryLazy)
			: base("Video",
				p => Settings.Default.VideoFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
			_videoComponentEditorFactoryLazy = videoComponentEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsAudioOrVideo
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
				foreach (var key in new[] { "Resolution", "Frame_Rate" })
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
			var commands = base.GetCommands(filePath).ToList();

			if (!File.Exists(filePath.Replace(Path.GetExtension(filePath), ".mp3")))
			{
				commands.Add(null); // Separator
				commands.Add(new FileCommand("Extract Audio to MP3 File", ExtractMp3Audio, "convert"));
			}

			return commands;
		}

		/// ------------------------------------------------------------------------------------
		private void ExtractMp3Audio(string path)
		{
			if (!MediaInfo.HaveNecessaryComponents)
			{
				MessageBox.Show("SayMore could not find the proper FFmpeg on this computer. FFmpeg is required to do that conversion.");
			}

			var outputPath = path.Replace(Path.GetExtension(path), ".mp3");

			if (File.Exists(outputPath))
			{
				//todo ask the user (or don't offer this in the first place)
				//File.Delete(outputPath);

				ErrorReport.NotifyUserOfProblem(
					string.Format("Sorry, the file '{0}' already exists.", Path.GetFileName(outputPath)));

				return;
			}

			Cursor.Current = Cursors.WaitCursor;
			//TODO...provide some progress
			var results = FFmpegRunner.ExtractMp3Audio(path, outputPath, new NullProgress());
			Cursor.Current = Cursors.Default;

			if (results.ExitCode != 0)
			{
				ErrorReport.NotifyUserOfProblem(
						string.Format("Something didn't work out. FFmpeg said (start reading from the end): {0}{1}{2}",
							Environment.NewLine, Environment.NewLine, results.StandardError));

				return;
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			var text = LocalizationManager.LocalizeString("VideoFileInfoEditor.PlaybackTabText", "Video");
			yield return new AudioVideoPlayer(file, text, "Video");

			text = LocalizationManager.LocalizeString("VideoFileInfoEditor.PropertiesTabText", "Properties");
			yield return _videoComponentEditorFactoryLazy()(file, text, null);

			text = LocalizationManager.LocalizeString("VideoFileInfoEditor.NotesTabText", "Notes");
			yield return new NotesEditor(file, text, "Notes");

			//_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));

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
		/// ------------------------------------------------------------------------------------
		public ImageFileType(Func<BasicFieldGridEditor.Factory> basicFieldGridEditorFactoryLazy)
			: base("Image",
			p => Settings.Default.ImageFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext)))
		{
			_basicFieldGridEditorFactoryLazy = basicFieldGridEditorFactoryLazy;
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

			text = LocalizationManager.LocalizeString("ImageFileInfoEditor.NotesTabText", "Notes");
			yield return new NotesEditor(file, text, "Notes");

			//_editors.Add(new ContributorsEditor(file, "Contributors", "Contributors"));
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
		public Func<MediaInfo, object> DataSetChooser;
		public Func<object, object> DataItemChooser;

		public Func<AudioVideoFileStatistics, Func<MediaInfo, object>,
			Func<object, object>, string, string> GetFormatedStatProvider { get; set; }

		/// ------------------------------------------------------------------------------------
		public ComputedFieldInfo()
		{
			Suffix = string.Empty;
		}
	}

	#endregion
}