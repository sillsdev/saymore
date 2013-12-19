using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using Palaso.IO;
using Palaso.UI.WindowsForms.ClearShare;
using Palaso.UI.WindowsForms.Widgets.BetterGrid;
using SayMore.Media;
using SayMore.Media.Audio;
using SayMore.Model.Fields;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;
using SayMore.UI;
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
		public virtual bool CanBeConverted
		{
			get { return false; }
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
			get
			{
				yield return new FieldDefinition("notes");
			}
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
			return (!GetIsCustomFieldId(key) && !GetIsReadonly(key) && key != "contributions");
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
				foreach (var fieldDefinition in base.FactoryFields)
					yield return fieldDefinition;

				foreach (var fieldId in new[]
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
				})
					yield return new FieldDefinition(fieldId);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<DataGridViewColumn> GetFieldsShownInGrid()
		{
			var col = BetterGrid.CreateTextBoxColumn("id");
			col.HeaderText = "_L10N_:PeopleView.PeopleList.ColumnHeadings.Id!Id";
			col.DataPropertyName = "id";
			col.ReadOnly = true;
			col.Frozen = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateImageColumn("consent");
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

	#region SessionFileType class
	/// ----------------------------------------------------------------------------------------
	public class SessionFileType : FileType
	{
		public const string kStageFieldPrefix = "stage_";

		public const string kStatusFieldName = "status";
		public const string kStagesFieldName = "stages";
		public const string kDateFieldName = "date";
		public const string kSynopsisFieldName = "synopsis";
		public const string kAccessFieldName = "access";
		public const string kLocationFieldName = "location";
		public const string kGenreFieldName = "genre";
		public const string kParticipantsFieldName = "participants";
		public const string kTitleFieldName = "title";

		// additional fields
		public const string kCountryFieldName = XmlFileSerializer.kAdditionalFieldIdPrefix + "Location_Country";
		public const string kContinentFieldName = XmlFileSerializer.kAdditionalFieldIdPrefix + "Location_Continent";
		public const string kRegionFieldName = XmlFileSerializer.kAdditionalFieldIdPrefix + "Location_Region";
		public const string kAddressFieldName = XmlFileSerializer.kAdditionalFieldIdPrefix + "Location_Address";
		public const string kSubGenreFieldName = XmlFileSerializer.kAdditionalFieldIdPrefix + "Sub-Genre";
		public const string kInteractivityFieldName = XmlFileSerializer.kAdditionalFieldIdPrefix + "Interactivity";
		public const string kPlanningTypeFieldName = XmlFileSerializer.kAdditionalFieldIdPrefix + "Planning_Type";
		public const string kInvolvementFieldName = XmlFileSerializer.kAdditionalFieldIdPrefix + "Involvement";
		public const string kSocialContextFieldName = XmlFileSerializer.kAdditionalFieldIdPrefix + "Social_Context";
		public const string kTaskFieldName = XmlFileSerializer.kAdditionalFieldIdPrefix + "Task";

		private readonly Func<SessionBasicEditor.Factory> _sessionBasicEditorFactoryLazy;
		private readonly Func<StatusAndStagesEditor.Factory> _statusAndStagesEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// <param name="sessionBasicEditorFactoryLazy">This is to get us around a circular
		/// dependency error in autofac. NB: when we move to .net 4, this can be replaced by
		/// Lazy<Func<SessionBasicEditor.Factory></param>
		/// ------------------------------------------------------------------------------------
		public SessionFileType(Func<SessionBasicEditor.Factory> sessionBasicEditorFactoryLazy,
			Func<StatusAndStagesEditor.Factory> statusAndStagesEditorFactoryLazy)
			: base("Session", p => p.ToLower().EndsWith(Settings.Default.SessionFileExtension.ToLower()))
		{
			_sessionBasicEditorFactoryLazy = sessionBasicEditorFactoryLazy;
			_statusAndStagesEditorFactoryLazy = statusAndStagesEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName
		{
			get { return "SessionCustomFieldsGrid"; }
		}

		/// ------------------------------------------------------------------------------------
		public override bool GetIsCustomFieldId(string key)
		{
			if (key.StartsWith(kStageFieldPrefix))
			{
				var role = key.Substring(kStageFieldPrefix.Length);
				if (ApplicationContainer.ComponentRoles.Any(cr => cr.Id == role))
					return false;
			}

			return base.GetIsCustomFieldId(key);
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
				foreach (var fieldDefinition in base.FactoryFields)
					yield return fieldDefinition;

				foreach (var fieldId in new[]
				{
					kStatusFieldName,
					kStagesFieldName,
					kDateFieldName,
					kSynopsisFieldName,
					kAccessFieldName,
					kLocationFieldName,
					"setting",
					"situation",
					kGenreFieldName,
					kParticipantsFieldName,
					kTitleFieldName,
					"contributions",
					kInteractivityFieldName,
					kInvolvementFieldName,
					kCountryFieldName,
					kContinentFieldName,
					kRegionFieldName,
					kAddressFieldName,
					kPlanningTypeFieldName,
					kSubGenreFieldName,
					kSocialContextFieldName,
					kTaskFieldName
				})
					yield return new FieldDefinition(fieldId);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<DataGridViewColumn> GetFieldsShownInGrid()
		{
			var col = BetterGrid.CreateTextBoxColumn("id");
			col.HeaderText = "_L10N_:SessionsView.SessionsList.ColumnHeadings.Id!Id";
			col.DataPropertyName = "id";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateTextBoxColumn(kTitleFieldName);
			col.HeaderText = "_L10N_:SessionsView.SessionsList.ColumnHeadings.Title!Title";
			col.DataPropertyName = kTitleFieldName;
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateImageColumn(kStagesFieldName);
			col.HeaderText = "_L10N_:SessionsView.SessionsList.ColumnHeadings.Stages!Stages";
			col.DataPropertyName = kStagesFieldName;
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateImageColumn(kStatusFieldName);
			col.HeaderText = "_L10N_:SessionsView.SessionsList.ColumnHeadings.Status!Status";
			col.DataPropertyName = kStatusFieldName;
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateTextBoxColumn(kDateFieldName);
			col.HeaderText = "_L10N_:SessionsView.SessionsList.ColumnHeadings.Date!Date";
			col.DataPropertyName = kDateFieldName;
			col.ReadOnly = true;
			col.Visible = false;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateTextBoxColumn(kGenreFieldName);
			col.HeaderText = "_L10N_:SessionsView.SessionsList.ColumnHeadings.Genre!Genre";
			col.DataPropertyName = kGenreFieldName;
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.Visible = false;
			yield return col;

			col = BetterGrid.CreateTextBoxColumn(kLocationFieldName);
			col.HeaderText = "_L10N_:SessionsView.SessionsList.ColumnHeadings.Location!Location";
			col.DataPropertyName = kLocationFieldName;
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.Visible = false;
			yield return col;
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return _sessionBasicEditorFactoryLazy()(file, "Session");
			yield return _statusAndStagesEditorFactoryLazy()(file, "StatusAndStages");
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
			get { return Resources.SessionFileImage; }
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
		protected readonly Func<TextAnnotationEditor.Factory> _textAnnotationEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		public AnnotationFileType(Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy,
			Func<TextAnnotationEditor.Factory> textAnnotationEditorFactoryLazy)
			: base("Annotations", GetIsAnAnnotationFile, contributorsEditorFactoryLazy)
		{
			_textAnnotationEditorFactoryLazy = textAnnotationEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsAnAnnotationFile(string path)
		{
			var annotationSuffix = AnnotationFileHelper.kAnnotationsEafFileSuffix;

			return (path.ToLower().EndsWith(annotationSuffix) &&
				File.Exists(path.ToLower().Replace(annotationSuffix, string.Empty)));
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return _textAnnotationEditorFactoryLazy()(file, "Annotation");
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

	#region AnnotationFileWithMisingMediaFileType class
	/// ----------------------------------------------------------------------------------------
	public class AnnotationFileWithMisingMediaFileType : FileType
	{
		/// ------------------------------------------------------------------------------------
		public AnnotationFileWithMisingMediaFileType()
			: base("AnnotationsWithMissingMedia", GetIsAnAnnotationFileWithMissingMedia)
		{
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsAnAnnotationFileWithMissingMedia(string path)
		{
			var annotationSuffix = AnnotationFileHelper.kAnnotationsEafFileSuffix;

			return (path.ToLower().EndsWith(annotationSuffix) &&
				!File.Exists(path.ToLower().Replace(annotationSuffix, string.Empty)));
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return new MissingMediaFileEditor(file, "/Concepts/ELAN.htm");
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
			Func<Project> project,
			Func<AudioComponentEditor.Factory> audioComponentEditorFactoryLazy,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy) :
			base(project(), audioComponentEditorFactoryLazy, contributorsEditorFactoryLazy)
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
		private readonly Project _project;

		/// ------------------------------------------------------------------------------------
		protected AudioVideoFileTypeBase(string name, Project project, Func<string, bool> isMatchPredicate,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base(name, isMatchPredicate, contributorsEditorFactoryLazy)
		{
			_project = project;
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return new StartAnnotatingEditor(file, _project);
			yield return new ConvertToStandardAudioEditor(file);
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FieldDefinition> FactoryFields
		{
			get
			{
				foreach (var fieldDef in base.FactoryFields)
					yield return fieldDef;

				yield return new FieldDefinition("Device");
				yield return new FieldDefinition("Microphone");
				yield return new FieldDefinition("contributions");
			}
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
				DataItemChooser = (info => info.Audio.KilobitsPerSecond),
				GetFormatedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Video_Bit_Rate",
				Suffix = "kbps",
				//Suffix = Program.Get____String("Model.Files.AudioVideoFileType.VideoBitRateSuffix", "kbps"),
				DataItemChooser = (info => info.VideoKilobitsPerSecond),
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
				DataItemChooser = (info => info.BitsPerSample == 0 ? null : info.BitsPerSample.ToString()),
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
			MigrateFieldToContribution(file, XmlFileSerializer.kCustomFieldIdPrefix + "Recordist", "recorder");
			MigrateFieldToContribution(file, XmlFileSerializer.kCustomFieldIdPrefix + "Speaker", "speaker");
			MigrateFieldToContribution(file, XmlFileSerializer.kCustomFieldIdPrefix + "speaker", "speaker");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Migrates the specified field to a contribution, adds it to the list of
		/// contributions already associated with the specified file (or creates a new list
		/// of contributions) and removes the fieldId from the metadata. Finally, the
		/// file's metadata is saved.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void MigrateFieldToContribution(ComponentFile file, string fieldId, string roleCode)
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

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FileCommand> GetCommands(string filePath)
		{
			var commands = base.GetCommands(filePath).ToList();

			if (commands.Count > 0)
				commands.Add(null); // Separator

			var menuText = LocalizationManager.GetString(
				"CommonToMultipleViews.FileList.Convert.ConvertMenuText",
				"Convert...");

			commands.Add(new FileCommand(menuText,
				path => ConvertMediaDlg.Show(path, null), "convert"));

			return commands;
		}

		/// ------------------------------------------------------------------------------------
		public override bool CanBeConverted
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		public static string ComputeStandardPcmAudioFilePath(string path)
		{
			if (path.EndsWith(Settings.Default.StandardAudioFileSuffix))
				return path;

			var pcmPath = Path.GetFileNameWithoutExtension(path);
			if (pcmPath.EndsWith(Path.GetFileNameWithoutExtension(Settings.Default.StandardAudioFileSuffix)))
				return Path.Combine(Path.GetDirectoryName(path), pcmPath + ".wav");

			pcmPath += Settings.Default.StandardAudioFileSuffix;
			return Path.Combine(Path.GetDirectoryName(path), pcmPath);
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsStandardPcmAudioFile(string path)
		{
			return path.EndsWith(Settings.Default.StandardAudioFileSuffix) ||
				AudioUtils.GetIsFileStandardPcm(path);
		}
	}

	#endregion

	#region AudioFileType class
	/// ----------------------------------------------------------------------------------------
	public class AudioFileType : AudioVideoFileTypeBase
	{
		protected readonly Func<AudioComponentEditor.Factory> _audioComponentEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		public AudioFileType(Project project,
			Func<AudioComponentEditor.Factory> audioComponentEditorFactoryLazy,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base("Audio", project,
				p => FileUtils.AudioFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext.ToLower())),
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
			get { return base.FactoryFields.Union(GetBaseAudioFields()); }
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
		public VideoFileType(Project project,
			Func<VideoComponentEditor.Factory> videoComponentEditorFactoryLazy,
			Func<ContributorsEditor.Factory> contributorsEditorFactoryLazy)
			: base("Video", project, p => FileUtils.VideoFileExtensions.Cast<string>()
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
				foreach (var field in base.FactoryFields)
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
			: base("Image", p => FileUtils.ImageFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext.ToLower())))
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