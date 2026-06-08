using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using L10NSharp;
using SIL.IO;
using SIL.Core.ClearShare;
using SIL.Windows.Forms.Widgets.BetterGrid;
using SayMore.Media;
using SayMore.Media.Audio;
using SayMore.Model.Fields;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;
using SayMore.UI;
using SayMore.UI.ComponentEditors;
using static System.String;

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
		public virtual string RootElementName => null;
		public virtual Image SmallIcon => null;
		public virtual string FileSize => null;

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
		public virtual string FieldsGridSettingsName => "UnknownFileFieldsGrid";

		/// ------------------------------------------------------------------------------------
		public virtual bool CanBeConverted => false;

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

			if (_editors.TryGetValue(hashCode, out var editors))
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
		public virtual bool IsForUnknownFileTypes => false;

		/// ------------------------------------------------------------------------------------
		public virtual bool IsAudioOrVideo => IsAudio || IsVideo;

		/// ------------------------------------------------------------------------------------
		public virtual bool IsAudio => false;

		/// ------------------------------------------------------------------------------------
		public virtual bool IsVideo => false;

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
			return (!GetIsCustomFieldId(key) && !GetIsReadonly(key) && key != SessionFileType.kContributionsFieldName);
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
		private readonly Lazy<Func<PersonBasicEditor.Factory>> _personBasicEditorFactoryLazy;
		private readonly Lazy<Func<PersonContributionEditor.Factory>> _personContributionEditorFactoryLazy;

		public const string kCode = "code";
		public const string kGender = "gender";
		public const string kEducation = "education";
		public const string kPrimaryOccupation = "primaryOccupation";
		public const string kPrimaryLanguage = "primaryLanguage";
		public const string kMothersLanguage = "mothersLanguage";
		public const string kFathersLanguage = "fathersLanguage";

		public static string GetOtherLanguageKey(int i)
		{
			Debug.Assert(i >= 0 && i < 4);
			return $"otherLanguage{i}";
		}

		public override string RootElementName => Name;

		/// ------------------------------------------------------------------------------------
		/// <param name="personBasicEditorFactoryLazy">This is to get us around a circular
		/// dependency error in autofac.</param>
		/// <param name="personRoleEditorFactoryLazy"></param>
		/// ------------------------------------------------------------------------------------
		public PersonFileType(Lazy<Func<PersonBasicEditor.Factory>> personBasicEditorFactoryLazy,
			Lazy<Func<PersonContributionEditor.Factory>> personRoleEditorFactoryLazy)
			: base("Person", p => p.ToLower().EndsWith(Settings.Default.PersonFileExtension.ToLower()))
		{
			_personBasicEditorFactoryLazy = personBasicEditorFactoryLazy;
			_personContributionEditorFactoryLazy = personRoleEditorFactoryLazy;
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
					kCode,
					"nickName",
					kPrimaryLanguage,
					"primaryLanguageLearnedIn",
					GetOtherLanguageKey(0),
					GetOtherLanguageKey(1),
					GetOtherLanguageKey(2),
					GetOtherLanguageKey(3),
					kFathersLanguage,
					kMothersLanguage,
					"pbOtherLangFather0",
					"pbOtherLangFather1",
					"pbOtherLangFather2",
					"pbOtherLangFather3",
					"pbOtherLangMother0",
					"pbOtherLangMother3",
					"pbOtherLangMother2",
					"pbOtherLangMother1",
					"birthYear",
					kGender,
					"howToContact",
					kEducation,
					kPrimaryOccupation,
					"picture",
					"privacyProtection",
					"birthYear",
					"ethnicGroup"
				})
					yield return new FieldDefinition(fieldId);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<DataGridViewColumn> GetFieldsShownInGrid()
		{
			var col = BetterGrid.CreateTextBoxColumn("id");
			col.HeaderText = @"_L10N_:PeopleView.PeopleList.ColumnHeadings.Person!Person";
			col.DataPropertyName = "display name";
			col.ReadOnly = true;
			col.Frozen = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateImageColumn("consent");
			col.HeaderText = @"_L10N_:PeopleView.PeopleList.ColumnHeadings.Consent!Consent";
			col.DataPropertyName = "consent";
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName => "PersonCustomFieldsGrid";

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return _personBasicEditorFactoryLazy.Value()(file, "Person");
			yield return _personContributionEditorFactoryLazy.Value()(file, "Session");
			yield return new NotesEditor(file);
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FileCommand> GetCommands(string filePath)
		{
			yield return new FileCommand(GetShowInFileExplorerMenuText(),
				FileCommand.HandleOpenInFileManager_Click, "open");
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon => ResourceImageCache.PersonFileImage;
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
		public const string kContributionsFieldName = "contributions";

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

		private readonly Lazy<Func<SessionBasicEditor.Factory>> _sessionBasicEditorFactoryLazy;
		private readonly Lazy<Func<StatusAndStagesEditor.Factory>> _statusAndStagesEditorFactoryLazy;
		private readonly Lazy<Func<ContributorsEditor.Factory>> _sessionContributorEditorFactoryLazy;

		public override string RootElementName => Name;

		/// <summary>
		/// Initializes a new instance of the <see cref="SessionFileType"/> class with the specified
		/// factories for creating session-related editors.
		/// </summary>
		/// <param name="sessionBasicEditorFactoryLazy">
		/// A lazy factory for creating instances of <see cref="SessionBasicEditor"/>. This helps
		/// resolve circular dependency issues in Autofac.
		/// </param>
		/// <param name="statusAndStagesEditorFactoryLazy">
		/// A lazy factory for creating instances of <see cref="StatusAndStagesEditor"/>.
		/// </param>
		/// <param name="sessionContributorEditorFactoryLazy">
		/// A lazy factory for creating instances of <see cref="ContributorsEditor"/>.
		/// </param>
		public SessionFileType(Lazy<Func<SessionBasicEditor.Factory>> sessionBasicEditorFactoryLazy,
			Lazy<Func<StatusAndStagesEditor.Factory>> statusAndStagesEditorFactoryLazy,
			Lazy<Func<ContributorsEditor.Factory>> sessionContributorEditorFactoryLazy)
			: base("Session", p => p.ToLower().EndsWith(Settings.Default.SessionFileExtension.ToLower()))
		{
			_sessionBasicEditorFactoryLazy = sessionBasicEditorFactoryLazy;
			_statusAndStagesEditorFactoryLazy = statusAndStagesEditorFactoryLazy;
			_sessionContributorEditorFactoryLazy = sessionContributorEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName => "SessionCustomFieldsGrid";

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
					kContributionsFieldName,
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
			col.HeaderText = @"_L10N_:SessionsView.SessionsList.ColumnHeadings.Id!Id";
			col.DataPropertyName = "id";
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateTextBoxColumn(kTitleFieldName);
			col.HeaderText = @"_L10N_:SessionsView.SessionsList.ColumnHeadings.Title!Title";
			col.DataPropertyName = kTitleFieldName;
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateImageColumn(kStagesFieldName);
			col.HeaderText = @"_L10N_:SessionsView.SessionsList.ColumnHeadings.Stages!Stages";
			col.DataPropertyName = kStagesFieldName;
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateImageColumn(kStatusFieldName);
			col.HeaderText = @"_L10N_:SessionsView.SessionsList.ColumnHeadings.Status!Status";
			col.DataPropertyName = kStatusFieldName;
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateTextBoxColumn(kDateFieldName);
			col.HeaderText = @"_L10N_:SessionsView.SessionsList.ColumnHeadings.Date!Date";
			col.DataPropertyName = kDateFieldName;
			col.ReadOnly = true;
			col.Visible = false;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			yield return col;

			col = BetterGrid.CreateTextBoxColumn(kGenreFieldName);
			col.HeaderText = @"_L10N_:SessionsView.SessionsList.ColumnHeadings.Genre!Genre";
			col.DataPropertyName = kGenreFieldName;
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.Visible = false;
			yield return col;

			col = BetterGrid.CreateTextBoxColumn(kLocationFieldName);
			col.HeaderText = @"_L10N_:SessionsView.SessionsList.ColumnHeadings.Location!Location";
			col.DataPropertyName = kLocationFieldName;
			col.ReadOnly = true;
			col.SortMode = DataGridViewColumnSortMode.Programmatic;
			col.Visible = false;
			yield return col;
		}

		/// ------------------------------------------------------------------------------------
		public override void Migrate(ComponentFile file)
		{
			base.Migrate(file);

			var accessCode = file.GetStringValue(kAccessFieldName, Empty);

			// "Insite users" has been changed to "REAP users"
			if (accessCode == "Insite users")
			{
				accessCode = "REAP users";
				file.SetStringValue(kAccessFieldName, accessCode);
				file.Save();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return _sessionBasicEditorFactoryLazy.Value()(file, "Session");
			yield return _statusAndStagesEditorFactoryLazy.Value()(file, "StatusAndStages");
			yield return _sessionContributorEditorFactoryLazy.Value()(file, "Contributor");
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
		public override Image SmallIcon => ResourceImageCache.SessionFileImage;
	}

	#endregion

	#region FileTypeWithContributors class
	/// ----------------------------------------------------------------------------------------
	public abstract class FileTypeWithContributors : FileType
	{
		protected Lazy<Func<ContributorsEditor.Factory>> ContributorsEditorFactoryLazy { get; }

		/// ------------------------------------------------------------------------------------
		protected FileTypeWithContributors(string name, Func<string, bool> isMatchPredicate,
			Lazy<Func<ContributorsEditor.Factory>> contributorsEditorFactoryLazy)
			: base(name, isMatchPredicate)
		{
			ContributorsEditorFactoryLazy = contributorsEditorFactoryLazy;
		}
	}

	#endregion

	#region AnnotationFileType class
	/// ----------------------------------------------------------------------------------------
	public class AnnotationFileType : FileTypeWithContributors
	{
		private readonly Lazy<Func<TextAnnotationEditor.Factory>> _textAnnotationEditorFactoryLazy;

		/// ------------------------------------------------------------------------------------
		public AnnotationFileType(Lazy<Func<ContributorsEditor.Factory>> contributorsEditorFactoryLazy,
			Lazy<Func<TextAnnotationEditor.Factory>> textAnnotationEditorFactoryLazy)
			: base("Annotations", GetIsAnAnnotationFile, contributorsEditorFactoryLazy)
		{
			_textAnnotationEditorFactoryLazy = textAnnotationEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsAnAnnotationFile(string path)
		{
			var annotationSuffix = AnnotationFileHelper.kAnnotationsEafFileSuffix;

			return (path.ToLower().EndsWith(annotationSuffix) &&
				File.Exists(path.ToLower().Replace(annotationSuffix, Empty)));
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return _textAnnotationEditorFactoryLazy.Value()(file, "Annotation");
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

	#region AnnotationFileWithMissingMediaFileType class
	/// ----------------------------------------------------------------------------------------
	public class AnnotationFileWithMissingMediaFileType : FileType
	{
		/// ------------------------------------------------------------------------------------
		public AnnotationFileWithMissingMediaFileType()
			: base("AnnotationsWithMissingMedia", GetIsAnAnnotationFileWithMissingMedia)
		{
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsAnAnnotationFileWithMissingMedia(string path)
		{
			var annotationSuffix = AnnotationFileHelper.kAnnotationsEafFileSuffix;

			return (path.ToLower().EndsWith(annotationSuffix) &&
				!File.Exists(path.ToLower().Replace(annotationSuffix, Empty)));
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
			Lazy<Func<AudioComponentEditor.Factory>> audioComponentEditorFactoryLazy,
			Lazy<Func<ContributorsEditor.Factory>> contributorsEditorFactoryLazy) :
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
			yield return AudioComponentEditorFactoryLazy.Value()(file, null);
			//yield return _contributorsEditorFactoryLazy()(file, null);
			//yield return new NotesEditor(file);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// These are fields which are always available for files of this type
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FieldDefinition> FactoryFields => GetBaseAudioFields();
	}

	#endregion

	#region AudioVideoFileTypeBase class
	/// ----------------------------------------------------------------------------------------
	public abstract class AudioVideoFileTypeBase : FileTypeWithContributors
	{
		private readonly Project _project;

		/// ------------------------------------------------------------------------------------
		protected AudioVideoFileTypeBase(string name, Project project, Func<string, bool> isMatchPredicate,
			Lazy<Func<ContributorsEditor.Factory>> contributorsEditorFactoryLazy)
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
				yield return new FieldDefinition(SessionFileType.kContributionsFieldName);
			}
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ComputedFieldInfo> GetComputedFields()
		{
			// TODO: Figure out how to localize the suffixes so English is saved in
			// the metadata file but the user sees them in their UI language.

			yield return new ComputedFieldInfo
			{
				Key = "Duration",
				DataItemChooser = (info => info.Duration),
				GetFormattedStatProvider = GetDurationStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Audio_Bit_Rate",
				Suffix = "kbps",
				//Suffix = Program.Get____String("Model.Files.AudioVideoFileType.AudioBitRateSuffix", "kbps"),
				DataItemChooser = (info => info.Audio?.KilobitsPerSecond ?? 0),
				GetFormattedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Video_Bit_Rate",
				Suffix = "kbps",
				//Suffix = Program.Get____String("Model.Files.AudioVideoFileType.VideoBitRateSuffix", "kbps"),
				DataItemChooser = (info => info.VideoKilobitsPerSecond),
				GetFormattedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Sample_Rate",
				Suffix = "Hz",
				//Suffix = Program.Get____String("Model.Files.AudioVideoFileType.SampleRateSuffix", "Hz"),
				DataItemChooser = (info => info.SamplesPerSecond),
				GetFormattedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Bit_Depth",
				Suffix = "bits",
				//Suffix = Program.Get____String("Model.Files.AudioVideoFileType.BitDepthSuffix", "bits"),
				DataItemChooser = (info => info.BitsPerSample == 0 ? null : info.BitsPerSample.ToString(CultureInfo.InvariantCulture)),
				GetFormattedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Channels",
				DataItemChooser = (info => info.Channels.ToString(CultureInfo.InvariantCulture)),
				GetFormattedStatProvider = GetChannelsStatistic,
			};

			yield return new ComputedFieldInfo
			{
				Key = "Resolution",
				DataItemChooser = (info => info.Resolution),
				GetFormattedStatProvider = GetStringStatistic
			};

			yield return new ComputedFieldInfo
			{
				Key = "Frame_Rate",
				Suffix = "frames/second",
				//Suffix = Program.Get____String("Model.Files.AudioVideoFileType.FrameRateSuffix", "frames/second"),
				DataItemChooser = (info => info.FramesPerSecond),
				GetFormattedStatProvider = GetStringStatistic
			};
		}

		/// ------------------------------------------------------------------------------------
		public string GetStringStatistic(MediaFileInfo info,
			Func<MediaFileInfo, object> dataItemChooser, string suffix)
		{
			if (info == null)
				return Empty;

			var dataVal = dataItemChooser(info);
			return dataVal == null ? Empty : $"{dataVal} {suffix}".Trim();
		}

		/// ------------------------------------------------------------------------------------
		public string GetChannelsStatistic(MediaFileInfo info,
			Func<MediaFileInfo, object> dataItemChooser, string suffix)
		{
			var channels = GetStringStatistic(info, dataItemChooser, Empty);

			// TODO: Figure out how to localize these so English is saved in the
			// metadata file but the user sees them in their UI language.

			switch (channels)
			{
				case "-1": return Empty;
				case "0": return Empty;
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
			var duration = GetStringStatistic(info, dataItemChooser, Empty);

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
				file.GetValue(SessionFileType.kContributionsFieldName, new ContributionCollection()) as ContributionCollection;

			var contributor = new Contribution(value, role) { Date = file.GetCreateDate() };
			if (collection != null)
			{
				collection.Add(contributor);
				file.SetValue(SessionFileType.kContributionsFieldName, collection, out _);
			}
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
		public override bool CanBeConverted => true;

		/// ------------------------------------------------------------------------------------
		public static string ComputeStandardPcmAudioFilePath(string path)
		{
			if (path.EndsWith(Settings.Default.StandardAudioFileSuffix))
				return path;

			var dirName = Path.GetDirectoryName(path);
			if (dirName == null) return path;

			var pcmPath = Path.GetFileNameWithoutExtension(path);
			var testPath = Path.GetFileNameWithoutExtension(Settings.Default.StandardAudioFileSuffix);

			if (testPath != Empty)
			{
				if (pcmPath.EndsWith(testPath))
					return Path.Combine(dirName, pcmPath + ".wav");
			}

			pcmPath += Settings.Default.StandardAudioFileSuffix;
			return Path.Combine(dirName, pcmPath);
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
		protected Lazy<Func<AudioComponentEditor.Factory>> AudioComponentEditorFactoryLazy { get; }

		/// ------------------------------------------------------------------------------------
		public AudioFileType(Project project,
			Lazy<Func<AudioComponentEditor.Factory>> audioComponentEditorFactoryLazy,
			Lazy<Func<ContributorsEditor.Factory>> contributorsEditorFactoryLazy)
			: base("Audio", project,
				p => FileUtils.AudioFileExtensions.Cast<string>().Any(ext => p.ToLower().EndsWith(ext.ToLower())),
				contributorsEditorFactoryLazy)
		{
			AudioComponentEditorFactoryLazy = audioComponentEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsAudio => true;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// These are fields which are always available for files of this type
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FieldDefinition> FactoryFields => 
			base.FactoryFields.Union(GetBaseAudioFields());

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
		public override string FieldsGridSettingsName => "AudioFileFieldsGrid";

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return new AudioVideoPlayer(file, "Audio");
			yield return AudioComponentEditorFactoryLazy.Value()(file, null);
			yield return ContributorsEditorFactoryLazy.Value()(file, null);
			yield return new NotesEditor(file);

			foreach (var editor in base.GetNewSetOfEditorProviders(file))
				yield return editor;
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon => ResourceImageCache.AudioFileImage;
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
			Lazy<Func<ContributorsEditor.Factory>> contributorsEditorFactoryLazy)
			: base("Video", project, p => FileUtils.VideoFileExtensions.Cast<string>()
				.Any(ext => p.ToLower().EndsWith(ext.ToLower())), contributorsEditorFactoryLazy)
		{
			_videoComponentEditorFactoryLazy = videoComponentEditorFactoryLazy;
		}

		/// ------------------------------------------------------------------------------------
		public override bool IsVideo => true;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// These are fields which are always available for files of this type
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override IEnumerable<FieldDefinition> FactoryFields
		{
			get
			{
				foreach (var fieldDef in base.FactoryFields)
					yield return fieldDef;

				// Add video only fields
				yield return new FieldDefinition("Video_Bit_Rate") { ReadOnly = true };
				yield return new FieldDefinition("Resolution") { ReadOnly = true };
				yield return new FieldDefinition("Frame_Rate") { ReadOnly = true };
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string FieldsGridSettingsName => "VideoFileFieldsGrid";

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
			yield return ContributorsEditorFactoryLazy.Value()(file, null);
			yield return new NotesEditor(file);

			foreach (var editor in base.GetNewSetOfEditorProviders(file))
				yield return editor;
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon => ResourceImageCache.VideoFileImage;
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
		public override string FieldsGridSettingsName => "ImageFileFieldsGrid";

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<IEditorProvider> GetNewSetOfEditorProviders(ComponentFile file)
		{
			yield return new ImageViewer(file);
			yield return _basicFieldGridEditorFactoryLazy()(file, null);
			yield return _contributorsEditorFactoryLazy()(file, null);
			yield return new NotesEditor(file);
		}

		/// ------------------------------------------------------------------------------------
		public override Image SmallIcon => ResourceImageCache.ImageFileImage;
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
			string, string> GetFormattedStatProvider { get; set; }

		/// ------------------------------------------------------------------------------------
		public ComputedFieldInfo()
		{
			Suffix = Empty;
		}
	}

	#endregion
}