using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DesktopAnalytics;
using L10NSharp;
using SIL.Code;
using SIL.Reporting;
using SIL.Windows.Forms.FileSystem;
using SayMore.Media.Audio;
using SayMore.Model.Fields;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Transcription.UI;
using SayMore.UI.ElementListScreen;
using SayMore.Utilities;
using System.ComponentModel;
using SayMore.Media;

namespace SayMore.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Both sessions and people are made up of a number of files: an xml file we help them
	/// edit (i.e. .session or .person), plus any number of other files (videos, texts, images,
	/// etc.). Each of these is represented by an object of this class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ComponentFile : IDisposable
	{
		#region Windows API stuff
#if !__MonoCS__
		public const uint SHGFI_DISPLAYNAME = 0x00000200;
		public const uint SHGFI_TYPENAME = 0x400;
		public const uint SHGFI_EXETYPE = 0x2000;
		public const uint SHGFI_ICON = 0x100;
		public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
		public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint
			dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		extern static bool DestroyIcon(IntPtr handle);
#endif
		#endregion

		#region enum GenerateOption
		public enum GenerateOption
		{
			RegenerateNow,
			ClearAndRegenerateOnDemand,
			GenerateIfNeeded,
		}
		#endregion

		private static readonly Dictionary<string, string> s_fileTypes = new Dictionary<string, string>();
		private static readonly Dictionary<string, Bitmap> s_smallFileIcons = new Dictionary<string, Bitmap>();

		public delegate void ValueChangedHandler(ComponentFile file, string fieldId, object oldValue, object newValue);
		public event ValueChangedHandler IdChanged;
		public event ValueChangedHandler MetadataValueChanged;

		public delegate void RenamingHandler(ComponentFile sender, CancelEventArgs e);
		public event RenamingHandler StartingRename;

		public event EventHandler BeforeSave;
		public event EventHandler AfterSave;

		private AnnotationComponentFile _annotationFile;

		protected IEnumerable<ComponentRole> _componentRoles;
		private XmlFileSerializer _xmlFileSerializer;
		private IProvideAudioVideoFileStatistics _statisticsProvider;
		private PresetGatherer _presetProvider;
		private FieldUpdater _fieldUpdater;

		public ProjectElement ParentElement { get; protected set; }
		public string RootElementName { get; }
		public virtual string PathToAnnotatedFile { get; protected set; }
		public List<FieldInstance> MetaDataFieldValues { get; protected set; }
		public FileType FileType { get; protected set; }
		public string FileTypeDescription { get; protected set; }
		public string FileSize { get; protected set; }
		public Image SmallIcon { get; protected set; }
		public string DateModified { get; protected set; }
		public Action PreFileCommandAction;
		public Action PostFileCommandAction;
		public Action PreRenameAction;
		public Action PostRenameAction;
		public Action PreDeleteAction;
		public Action PreGenerateOralAnnotationFileAction;
		public Action<bool> PostGenerateOralAnnotationFileAction;

		protected string _metaDataPath;

		/// ------------------------------------------------------------------------------------
		public ComponentFile(ProjectElement parentElement,
			string pathToAnnotatedFile,
			IEnumerable<FileType> fileTypes,
			IEnumerable<ComponentRole> componentRoles,
			XmlFileSerializer xmlFileSerializer,
			IProvideAudioVideoFileStatistics statisticsProvider,
			PresetGatherer presetProvider,
			FieldUpdater fieldUpdater)
		{
			ParentElement = parentElement;
			SetPathToFile(pathToAnnotatedFile);
			_componentRoles = componentRoles;
			_xmlFileSerializer = xmlFileSerializer;
			_statisticsProvider = statisticsProvider;
			_presetProvider = presetProvider;
			_fieldUpdater = fieldUpdater;

			DetermineFileType(pathToAnnotatedFile, fileTypes);

			// We mustn't do anything to remove the existing extension, as that is needed
			// to keep, say, foo.wav and foo.txt separate. Instead, we just append ".meta"
			//_metaDataPath = ComputeMetaDataPath(pathToAnnotatedFile);

			MetaDataFieldValues = new List<FieldInstance>();

			Guard.AgainstNull(FileType, "At runtime (maybe not in tests) FileType should go to a type intended for unknowns");

			_metaDataPath = FileType.GetMetaFilePath(pathToAnnotatedFile);

			RootElementName = "MetaData";

			if (File.Exists(_metaDataPath))
				LoadNow();

			InitializeFileInfo();
		}

		[Obsolete("For Mocking Only")]
		public ComponentFile(FileType fileType)
		{
			_componentRoles = ApplicationContainer.ComponentRoles;
			FileType = fileType;
			MetaDataFieldValues = new List<FieldInstance>();
		}

		public void LoadNow()
		{
			Load();
		}

		private void SetPathToFile(string pathToAnnotatedFile)
		{
			PathToAnnotatedFile = pathToAnnotatedFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// used only by ProjectElementComponentFile and AnnotationComponentFile
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected ComponentFile(ProjectElement parentElement, string filePath,
			FileType fileType, string rootElementName,
			XmlFileSerializer xmlFileSerializer, FieldUpdater fieldUpdater)
		{
			RootElementName = rootElementName;
			ParentElement = parentElement;
			//The annotated file is the same as the annotation file; there isn't a pair of files for session/person
			SetPathToFile(filePath);
			FileType = fileType;
			_xmlFileSerializer = xmlFileSerializer;
			_metaDataPath = filePath;
			_fieldUpdater = fieldUpdater;
			MetaDataFieldValues = new List<FieldInstance>();
			_componentRoles = new ComponentRole[] {}; //no roles for person or event
			InitializeFileInfo();
		}

		/// ------------------------------------------------------------------------------------
		protected void DetermineFileType(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes)
		{
			var fTypes = fileTypes.ToArray();

			FileType =
				fTypes.FirstOrDefault(t => t.IsMatch(pathToAnnotatedFile)) ??
				fTypes.Single(t => t.IsForUnknownFileTypes);

			// SP-2245: There is unlikely any reason why a user would do this intentionally, but
			// they *can* and one apparently did by accident, wich led to a crash. Who knows what
			// else could go wrong, so let's at least log the situation to make it easier to debug
			// future problems.
			if (IsPersonOrSessionFileLocatedInWrongFolder)
			{
				Logger.WriteEvent($"WARNING: {FileType.Name} file ({pathToAnnotatedFile}) located " +
					$"inside {ParentElement.RootElementName} {ParentElement.Id}!");
			}
		}

		private bool IsPersonOrSessionFileLocatedInWrongFolder => ParentElement != null &&
			((FileType is PersonFileType && ParentElement.RootElementName == Session.kRootElement) ||
				(FileType is SessionFileType && ParentElement.RootElementName == Person.kRootElement));

		/// ------------------------------------------------------------------------------------
		protected void InitializeFileInfo()
		{
			if (PathToAnnotatedFile == null)
				return;

			FileType.Migrate(this);

			LoadFileSizeAndDateModified();

			// Initialize file's icon and description.
			Bitmap icon;
			string fileDesc;
			GetSmallIconAndFileType(PathToAnnotatedFile, out icon, out fileDesc);
			SmallIcon = FileType.SmallIcon ?? icon;
			FileTypeDescription = (FileType is UnknownFileType ? fileDesc : FileType.Name);
		}

		/// ------------------------------------------------------------------------------------
		/// <remarks>Virtual to support overload in mock for testing</remarks>
		/// ------------------------------------------------------------------------------------
		public virtual DateTime GetCreateDate()
		{
			var fi = new FileInfo(PathToAnnotatedFile);
			return fi.CreationTime;
		}

		#region Methods related to a file's annotation files
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the component file can have transcriptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual bool GetCanHaveAnnotationFile()
		{
			return (FileType.IsAudioOrVideo);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool GetNeedsConvertingToStandardAudio()
		{
			return (GetCanHaveAnnotationFile() &&
				!AudioUtils.GetIsFileStandardPcm(PathToAnnotatedFile));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path of to the component file's annotation file, even if the file
		/// doesn't exist. If the component file is not of a type that can have an annotation
		/// file, then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetSuggestedPathToAnnotationFile()
		{
			if (!GetCanHaveAnnotationFile())
				return null;

			var template = "{0}" + AnnotationFileHelper.kAnnotationsEafFileSuffix;
			return string.Format(template, PathToAnnotatedFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to what the component file's standard audio file is or should
		/// be. If the component file is not of a type that can have an annotation file, then
		/// null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual string GetSuggestedPathToStandardAudioFile()
		{
			return !GetCanHaveAnnotationFile() ? null :
				AudioVideoFileTypeBase.ComputeStandardPcmAudioFilePath(PathToAnnotatedFile);
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool HasSubordinateFiles
		{
			get { return GetDoesHaveAnnotationFile(); }
		}

		/// ------------------------------------------------------------------------------------
		public bool GetDoesHaveAnnotationFile()
		{
			return (GetCanHaveAnnotationFile() && GetAnnotationFile() != null);
		}

		/// ------------------------------------------------------------------------------------
		public virtual AnnotationComponentFile GetAnnotationFile()
		{
			return (_annotationFile != null && File.Exists(_annotationFile.PathToAnnotatedFile) ?
				_annotationFile : null);
		}

		/// ------------------------------------------------------------------------------------
		public void SetAnnotationFile(AnnotationComponentFile annotationFile)
		{
			_annotationFile = (annotationFile != null &&
				File.Exists(annotationFile.PathToAnnotatedFile) ? annotationFile : null);
		}
		#endregion

		#region Public properties
		public XmlFileSerializer XmlFileSerializer { get { return _xmlFileSerializer; } }
		public IProvideAudioVideoFileStatistics StatisticsProvider { get { return _statisticsProvider; } }
		public PresetGatherer PresetProvider { get { return _presetProvider; } }
		public FieldUpdater FieldUpdater { get { return _fieldUpdater; } }

		/// ------------------------------------------------------------------------------------
		public virtual int DisplayIndentLevel
		{
			get { return 0; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual MediaFileInfo GetMediaFileInfoOrNull() => StatisticsProvider?.GetFileData(PathToAnnotatedFile);

		/// ------------------------------------------------------------------------------------
		public virtual TimeSpan DurationSeconds
		{
			get
			{
				if (StatisticsProvider == null)
					return TimeSpan.Zero;

				var stats = GetMediaFileInfoOrNull();

				return GetDurationSeconds(stats);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// This method is exposed as a convenience so a caller that has already called
		/// GetMediaFileInfo, does not need to re-get it when accessing the DurationSeconds
		/// property. The MediaFileInfo object supplied here is expected be the information from
		/// GetMediaFileInfo.
		/// ------------------------------------------------------------------------------------
		internal TimeSpan GetDurationSeconds(MediaFileInfo stats)
		{
			if (stats == null || stats.Duration == default)
			{
				string duration = GetStringValue("Duration", string.Empty);
				if (duration == "Not Generated")
					return TimeSpan.Zero;
				return string.IsNullOrEmpty(duration) ? TimeSpan.Zero : TimeSpan.Parse(duration);
			}

			//trim off the milliseconds so it doesn't get too geeky
			return TimeSpan.FromSeconds((int)stats.Duration.TotalSeconds);
		}

		public string DurationString
		{
			get
			{
				return FileType.IsAudioOrVideo ? DurationSeconds.ToString() : string.Empty;
			}
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		public virtual void Refresh()
		{
			LoadFileSizeAndDateModified();
		}

		/// ------------------------------------------------------------------------------------
		protected void LoadFileSizeAndDateModified()
		{
			// Initialize file's display size. File should only not exist during tests.
			FileInfo fi = null;
			try
			{
				fi = new FileInfo(PathToAnnotatedFile);
			}
			catch (Exception e)
			{
				if (e is PathTooLongException || e is ArgumentException)
				{
					ErrorReport.ReportNonFatalExceptionWithMessage(e,
						LocalizationManager.GetString("CommonToMultipleViews.FileList.CannotRenameFileErrorMsg",
						"{0} could not load the file: {1}"),
						Application.ProductName, PathToAnnotatedFile);
					return;
				}
				throw;
			}
			FileSize = (fi.Exists ? GetDisplayableFileSize(fi.Length) : "0 KB");

			// display the file time using the current culture, same as windows explorer
			DateModified = (fi.Exists ? fi.LastWriteTime.ToShortDateString() + " " + fi.LastWriteTime.ToShortTimeString() : string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string GetStringValue(string key, string defaultValue)
		{
			if (key == "id" && IsPersonOrSessionFileLocatedInWrongFolder)
				return Path.GetFileNameWithoutExtension(PathToAnnotatedFile);

			string computedValue = null;

			var computedFieldInfo =
				FileType.GetComputedFields().FirstOrDefault(computedField => computedField.Key == key);

			if (computedFieldInfo != null && StatisticsProvider != null)
			{
				var mediaFileInfo = StatisticsProvider.GetFileData(PathToAnnotatedFile);
				if (mediaFileInfo != null)
				{
					if (mediaFileInfo.Audio == null &&
						PathToAnnotatedFile.EndsWith(Settings.Default.OralAnnotationGeneratedFileSuffix))
						return "Not Generated";
					// Get the computed value (if there is one).
					computedValue = computedFieldInfo.GetFormattedStatProvider(
						mediaFileInfo, computedFieldInfo.DataItemChooser, computedFieldInfo.Suffix);
				}
			}

			// Get the value from the metadata file.
			var	field = MetaDataFieldValues.FirstOrDefault(v => v.FieldId == key);
			var savedValue = (field == null ? defaultValue : field.ValueAsString);

			if (!string.IsNullOrEmpty(computedValue))
			{
				// If the computed value is different from the value found in the metadata
				// file, then save the computed value to the metadata file.
				if (computedValue != savedValue)
				{
					// REVIEW: We probably don't want to save the formatted value to the
					// metadata file, which is what we're doing here. In the future we'll
					// probably want to change things to save the raw computed value.
					SetStringValue(key, computedValue);
					Save();
					return computedValue;
				}
			}

			return savedValue;
		}

		/// ------------------------------------------------------------------------------------
		public virtual object GetValue(string key, object defaultValue)
		{
			var field = MetaDataFieldValues.FirstOrDefault(v => v.FieldId == key);
			return (field == null ? defaultValue : field.Value);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the value for persisting, and returns the same value, potentially modified
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual string SetStringValue(string key, string newValue)
		{
			return SetStringValue(new FieldInstance(key, newValue));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the value for persisting, and returns the same value, potentially modified
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual object SetValue(string key, object newValue, out string failureMessage)
		{
			failureMessage = null;

			object oldFieldValue = null;
			var oldFieldInstance = MetaDataFieldValues.Find(v => v.FieldId == key);

			if (oldFieldInstance == null)
			{
				if (newValue == null)
					return null;

				MetaDataFieldValues.Add(new FieldInstance(key, newValue));
			}
			else if (oldFieldInstance.Value.Equals(newValue))
				return newValue;
			else
				oldFieldValue = oldFieldInstance.Value;

			LoadFileSizeAndDateModified();
			OnMetadataValueChanged(key, oldFieldValue, newValue);

			if (oldFieldInstance != null)
				oldFieldInstance.Value = newValue;

			return newValue;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the value for persisting, and returns the same value, potentially modified
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual string SetStringValue(FieldInstance newFieldInstance)
		{
			newFieldInstance.Value = (newFieldInstance.ValueAsString ?? string.Empty).Trim();
			var oldFieldValue = MetaDataFieldValues.Find(v => v.FieldId == newFieldInstance.FieldId);

			if (oldFieldValue == newFieldInstance)
				return newFieldInstance.ValueAsString;

			string oldValue = null;

			if (oldFieldValue == null)
				MetaDataFieldValues.Add(newFieldInstance);
			else
			{
				oldValue = oldFieldValue.ValueAsString;
				oldFieldValue.Copy(newFieldInstance);
			}

			LoadFileSizeAndDateModified();
			OnMetadataValueChanged(newFieldInstance.FieldId, oldValue, newFieldInstance.ValueAsString);
			return newFieldInstance.ValueAsString; //overrides may do more
		}

		/// ------------------------------------------------------------------------------------
		public virtual void RenameId(string oldId, string newId)
		{
			var fieldValue = MetaDataFieldValues.Find(v => v.FieldId == oldId);
			if (fieldValue != null)
				fieldValue.FieldId = newId;

			if (FieldUpdater != null)
				FieldUpdater.RenameField(this, oldId, newId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>Removes the given (custom) field from this file's meta-data and also
		/// updates all other component files of this type if possible.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual void RemoveField(string idToRemove)
		{
			var existingValue = MetaDataFieldValues.Find(f => f.FieldId == idToRemove);
			if (existingValue != null)
				MetaDataFieldValues.Remove(existingValue);

			if (FieldUpdater != null)
				FieldUpdater.DeleteField(this, idToRemove);
		}

		/// ------------------------------------------------------------------------------------
		// ReSharper disable once VirtualMemberNeverOverriden.Global - Do not remove "virtual" - Mocked in tests
		public virtual void Save()
		{
			Save(_metaDataPath);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Save(string path)
		{
			_metaDataPath = path;
			OnBeforeSave(this);
			XmlFileSerializer.Save(MetaDataFieldValues, _metaDataPath, RootElementName);
			OnAfterSave(this);
		}

		/// ------------------------------------------------------------------------------------
		protected void OnBeforeSave(object sender)
		{
			if (BeforeSave != null)
				BeforeSave(sender, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnAfterSave(object sender)
		{
			if (AfterSave != null)
				AfterSave(sender, EventArgs.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Load()
		{
			XmlFileSerializer.CreateIfMissing(_metaDataPath, RootElementName);
			XmlFileSerializer.Load(/*TODO this.Work, */ MetaDataFieldValues,
				_metaDataPath, RootElementName, FileType);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string TryChangeChangeId(string newId, out string failureMessage)
		{
			throw new NotImplementedException($"Class {GetType()} does not know how to change the ID ({newId}).");
		}

		/// ------------------------------------------------------------------------------------
		protected void OnIdChanged(string fieldId, string oldId, string newId)
		{
			if (IdChanged != null)
				IdChanged(this, fieldId, oldId, newId);
		}

		/// ------------------------------------------------------------------------------------
		protected void OnMetadataValueChanged(string fieldId, object oldValue, object newValue)
		{
			if (MetadataValueChanged != null)
				MetadataValueChanged(this, fieldId, oldValue, newValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// What part(s) does this file play in the workflow of the session/person?
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ComponentRole> GetAssignedRoles()
		{
			return  GetAssignedRoles(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// What part(s) does this file play in the workflow of the session/person?
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<ComponentRole> GetAssignedRoles(ProjectElement element)
		{
			//if (_componentRoles)
			return from r in _componentRoles
				   where (r.IsMatch(PathToAnnotatedFile) && (element == null || r.RelevantElementType.IsInstanceOfType(element)))
				   select r;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// What part(s) does this file's annotation file play in the workflow of the session/
		/// person?
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<ComponentRole> GetAssignedRolesFromAnnotationFile()
		{
			if (GetDoesHaveAnnotationFile())
				return GetAnnotationFile().GetAssignedRoles();
			return new ComponentRole[0];
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Judging by the path (and maybe contents of the file itself), what
		/// parts might this file conceivably play in the workflow of the session/person?
		/// This is used to offer the user choices of assigning roles.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ComponentRole> GetPotentialRoles()
		{
			return from r in _componentRoles
				   where r.IsPotential(PathToAnnotatedFile)
				   select r;
		}

#if notyet
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The roles various people have played in creating/editing this file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<Contribution> Contributions
		{
			get; private set;
		}
#endif

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the component file name without its path. WARNING: THIS NAME IS HARD-CODED
		/// IN THE UI GRID
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual string FileName
		{
			get { return Path.GetFileName(PathToAnnotatedFile); }
		}

		/// ------------------------------------------------------------------------------------
		public static ComponentFile CreateMinimalComponentFileForTests(ProjectElement parentElement, string path)
		{
			return new ComponentFile(parentElement, path, new FileType[] { new UnknownFileType(null, null) },
				new ComponentRole[] { }, new XmlFileSerializer(null), null, null, null);
		}

		/// ------------------------------------------------------------------------------------
		public static ComponentFile CreateMinimalComponentFileForTests(string path, FileType fileType)
		{
			return new ComponentFile(null, path, new[] { fileType },
				new ComponentRole[] { }, new XmlFileSerializer(null), null, null, null);
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ToolStripItem> GetMenuCommands(Action<string> refreshAction)
		{
			return GetFileTypeMenuCommands(refreshAction);
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ToolStripItem> GetFileTypeMenuCommands(Action<string> refreshAction)
		{
			foreach (var cmd in FileType.GetCommands(PathToAnnotatedFile))
			{
				var cmd1 = cmd;// needed to avoid "access to modified closure". I.e., avoid executing the wrong command.
				if (cmd1 == null)
					yield return new ToolStripSeparator();
				else
				{
					yield return new ToolStripMenuItem(cmd.EnglishLabel, null, (s, e) =>
					{
						if (PreFileCommandAction != null)
							PreFileCommandAction();

						cmd1.Action(PathToAnnotatedFile);

						if (refreshAction != null)
							refreshAction(PathToAnnotatedFile);

						if (PostFileCommandAction != null)
							PostFileCommandAction();

					}) { Tag = cmd1.MenuId };
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool CanBeRenamedForRole
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual bool CanBeCustomRenamed
		{
			get { return true; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ComponentRole> GetRelevantComponentRoles()
		{
			return (ParentElement == null ? _componentRoles :
				_componentRoles.Where(r => r.RelevantElementType == ParentElement.GetType()));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Rename the file so that it is clear (visually and programatically) that this file
		/// plays this role.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AssignRole(ComponentRole role)
		{
			var nameOfParentFolderWhichIsAlsoElementId =
				Path.GetFileName(Path.GetDirectoryName(PathToAnnotatedFile));

			var newPath = role.GetCanoncialName(nameOfParentFolderWhichIsAlsoElementId,
				PathToAnnotatedFile);

			RenameAnnotatedFile(newPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// If user clicks OK and recordings were made or changed, the full path to the EAF
		/// file is returned. Otherwise null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string RecordAnnotations(Form frm, AudioRecordingType annotationType)
		{
			try
			{
				Program.SuspendBackgroundProcesses();

				using (var viewModel = OralAnnotationRecorderDlgViewModel.Create(this, annotationType))
				using (var dlg = OralAnnotationRecorderBaseDlg.Create(viewModel, annotationType))
				{
					if (dlg.ShowDialog(frm) != DialogResult.OK || !viewModel.WereChangesMade)
					{
						viewModel.DiscardChanges();
						return null;
					}

					Analytics.Track("Changes made using Oral Annotation Recorder");

					var eafFileName = viewModel.Tiers.Save(PathToAnnotatedFile);
					GenerateOralAnnotationFile(viewModel.Tiers, frm, GenerateOption.ClearAndRegenerateOnDemand);
					return eafFileName;
				}
			}
			finally
			{
				Program.ResumeBackgroundProcesses(true);
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual void GenerateOralAnnotationFile(Control parentOfProgressPopup, GenerateOption option)
		{
			GenerateOralAnnotationFile(GetAnnotationFile().Tiers, parentOfProgressPopup, option);
		}

		/// ------------------------------------------------------------------------------------
		public bool GenerateOralAnnotationFile(TierCollection tiers, Control parentOfProgressPopup, GenerateOption option)
		{
			bool generated = false;
			// subclass OralAnnotationComponentFile will handle the case of JIT generation
			if (option != GenerateOption.GenerateIfNeeded)
			{
				if (PreGenerateOralAnnotationFileAction != null)
					PreGenerateOralAnnotationFileAction();

				generated = OralAnnotationFileGenerator.Generate(tiers, parentOfProgressPopup,
					option == GenerateOption.ClearAndRegenerateOnDemand);
			}

			if (PostGenerateOralAnnotationFileAction != null)
				PostGenerateOralAnnotationFileAction(generated);

			return generated;
		}

		public bool IsOkayToRename 
		{
			get
			{
				if (StartingRename != null)
				{
					var args = new CancelEventArgs();
					StartingRename(this, args);
					return !args.Cancel;
				}
				return true;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Rename(Action<string> refreshAction)
		{
			using (var dlg = new ComponentFileRenamingDialog(this, _componentRoles))
			{
				if (dlg.ShowDialog() != DialogResult.OK)
					return;

				if (PreRenameAction != null)
					PreRenameAction();

				var newRole = dlg.GetNewRoleOfFile();
				if (newRole != null)
					AssignRole(newRole);
				else
					RenameAnnotatedFile(dlg.NewFilePath);

				if (refreshAction != null)
					refreshAction(PathToAnnotatedFile);

				if (PostRenameAction != null)
					PostRenameAction();
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual void RenameAnnotatedFile(string newPath)
		{
			try
			{
				// some types don't have a separate sidecar file (e.g. Person, Session)
				var newMetaPath = FileType.GetMetaFilePath(newPath);
				var renameMetaFile = (newMetaPath != newPath && File.Exists(_metaDataPath));

				if (File.Exists(newPath))
				{
					var msg = LocalizationManager.GetString("CommonToMultipleViews.FileList.CannotRenameFileErrorMsg",
						"{0} could not rename the file to '{1}' because there is already a file with that name.");

					ErrorReport.NotifyUserOfProblem(msg, Application.ProductName, newPath);
					return;
				}

				if (renameMetaFile && File.Exists(newMetaPath))
				{
					var msg = LocalizationManager.GetString("CommonToMultipleViews.FileList.CannotRenameMetadataFileErrorMsg",
						"{0} could not rename the meta data file to '{1}' because there is already a file with that name.");

					ErrorReport.NotifyUserOfProblem(msg, Application.ProductName, newMetaPath);
					return;
				}

				File.Move(PathToAnnotatedFile, newPath);
				if (renameMetaFile)
				{
					File.Move(_metaDataPath, newMetaPath);
					_metaDataPath = newMetaPath;
				}

				PathToAnnotatedFile = newPath;

				if (_annotationFile != null)
					_annotationFile.RenameAnnotatedFile(GetSuggestedPathToAnnotationFile());
			}
			catch (PathTooLongException pathTooLong)
			{
				throw new PathTooLongException(pathTooLong.Message + Environment.NewLine + newPath, pathTooLong);
			}
			catch (Exception e)
			{
				var msg = LocalizationManager.GetString("CommonToMultipleViews.FileList.CannotRenameFileGenericErrorMsg",
					"Sorry, SayMore could not rename that file because something else (perhaps another part of SayMore) is reading it. Please try again later.");

				ErrorReport.NotifyUserOfProblem(e, msg);
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual void HandleDoubleClick()
		{
			FileCommand.HandleOpenInApp_Click(PathToAnnotatedFile);
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return PathToAnnotatedFile;
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			if (_annotationFile != null)
				_annotationFile.Dispose();
			_annotationFile = null;

			_xmlFileSerializer = null;
			_statisticsProvider = null;
			_presetProvider = null;
			_fieldUpdater = null;
			MetaDataFieldValues = null;
			ParentElement = null;
		}

		/// ------------------------------------------------------------------------------------
		public static bool GetIsValidComponentFile(string fileName)
		{
			fileName = fileName.ToLower();
			var badEndings = Settings.Default.ComponentFileEndingsNotAllowed.Cast<string>();
			return !badEndings.Any(x => fileName.EndsWith(x.ToLower()));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the size of the session file in a displayable form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetDisplayableFileSize(long fileSize)
		{
			return GetDisplayableFileSize(fileSize, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the size of the session file in a displayable form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetDisplayableFileSize(long fileSize, bool abbreviateFileSizeUnits)
		{
			var fmtBytes = (abbreviateFileSizeUnits ?
				LocalizationManager.GetString("CommonToMultipleViews.FileList.FileSizeBytesAbbreviation", "{0} B") :
				LocalizationManager.GetString("CommonToMultipleViews.FileList.FileSizeBytes", "{0} Bytes"));

			var fmtKilobytes = (abbreviateFileSizeUnits ?
				LocalizationManager.GetString("CommonToMultipleViews.FileList.FileSizeKilobytesAbbreviation", "{0} KB") :
				LocalizationManager.GetString("CommonToMultipleViews.FileList.FileSizeKilobytes", "{0} Kilobytes"));

			var fmtMegabytes = (abbreviateFileSizeUnits ?
				LocalizationManager.GetString("CommonToMultipleViews.FileList.FileSizeMegabytesAbbreviation", "{0} MB") :
				LocalizationManager.GetString("CommonToMultipleViews.FileList.FileSizeMegabytes", "{0} Megabytes"));

			var fmtGigabytes = (abbreviateFileSizeUnits ?
				LocalizationManager.GetString("CommonToMultipleViews.FileList.FileSizeGigabytesAbbreviation", "{0} GB") :
				LocalizationManager.GetString("CommonToMultipleViews.FileList.FileSizeGigabytes", "{0} Gigabytes"));

			if (fileSize < 1000)
				return string.Format(fmtBytes, fileSize);

			if (fileSize < Math.Pow(1024, 2))
			{
				var size = (int)Math.Round(fileSize / (decimal)1024, 2, MidpointRounding.AwayFromZero);
				if (size < 1)
					size = 1;

				return string.Format(fmtKilobytes, size.ToString("###"));
			}

			double dblSize;
			if (fileSize < Math.Pow(1024, 3))
			{
				dblSize = Math.Round(fileSize / Math.Pow(1024, 2), 2, MidpointRounding.AwayFromZero);
				return string.Format(fmtMegabytes, dblSize.ToString("###.##"));
			}

			dblSize = Math.Round(fileSize / Math.Pow(1024, 3), 2, MidpointRounding.AwayFromZero);
			return string.Format(fmtGigabytes, dblSize.ToString("###,###.##"));
		}

		/// ------------------------------------------------------------------------------------
		private static void GetSmallIconAndFileType(string fullFilePath, out Bitmap smallIcon,
			out string fileType)
		{
			smallIcon = null;
			fileType = null;

#if !__MonoCS__
			var ext = Path.GetExtension(fullFilePath);
			if (ext == null)
				return;

			ext = ext.ToLowerInvariant();

			lock (s_fileTypes)
			{
				if (s_fileTypes.TryGetValue(ext, out fileType))
				{
					smallIcon = s_smallFileIcons[ext];
					return;
				}

				uint shGetFileInfoFlags = SHGFI_TYPENAME | SHGFI_DISPLAYNAME;

				if (Settings.Default.LoadComponentFileIcons)
				{
					shGetFileInfoFlags |= SHGFI_SMALLICON | SHGFI_ICON;
					Logger.WriteEvent("Getting icon and file type for file {0} (type: {1})", fullFilePath, ext);
				}
				else
				{
					Logger.WriteEvent("Getting file type for file {0} (type: {1})", fullFilePath, ext);
				}

				var shinfo = new SHFILEINFO();
				try
				{
					SHGetFileInfo(fullFilePath, 0, ref shinfo, (uint) Marshal.SizeOf(shinfo), shGetFileInfoFlags);
				}
				catch (Exception e)
				{
					Logger.WriteEvent("Error calling SHGetFileInfo with path: {0}\r\nException: {1}", fullFilePath, e);
					if (e is OutOfMemoryException)
						throw;
				}

				// This should only be zero during tests.
				if (Settings.Default.LoadComponentFileIcons && shinfo.hIcon != IntPtr.Zero)
				{
					var icon = Icon.FromHandle(shinfo.hIcon);
					smallIcon = icon.ToBitmap();
					DestroyIcon(shinfo.hIcon);
				}

				fileType = shinfo.szTypeName;

				s_fileTypes[ext] = fileType;
				s_smallFileIcons[ext] = smallIcon;
			}
#else
			// REVIEW: Figure out a better way to get this in Mono.
			Icon icon = Icon.ExtractAssociatedIcon(fullFilePath);
			var largeIcons = new ImageList();
			largeIcons.Images.Add(icon);
			var bmSmall = new Bitmap(16, 16);

			using (var g = Graphics.FromImage(bmSmall))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.DrawImage(LargeIcon, new Rectangle(0, 0, 16, 16),
					new Rectangle(new Point(0, 0), LargeIcon.Size), GraphicsUnit.Pixel);
			}

			smallIcon = bmSmall;
			// TODO: Figure out how to get FileType in Mono.
#endif
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<KeyValuePair<string, Dictionary<string, string>>> GetPresetChoices()
		{
			Guard.AgainstNull(PresetProvider, "PresetProvider");
			return PresetProvider.GetPresets();
		}

		/// ------------------------------------------------------------------------------------
		//public IEnumerable<Dictionary<string, string>> GetPresetChoices()
		//{
		//    Guard.AgainstNull(_presetProvider, "PresetProvider");
		//    return _presetProvider.GetPresets();
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Set our values to those of the preset
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void UsePreset(IDictionary<string, string> preset)
		{
			foreach (KeyValuePair<string, string> pair in preset)
				SetStringValue(pair.Key, pair.Value);

			Save();
		}

		/// ------------------------------------------------------------------------------------
		public static bool MoveToRecycleBin(ComponentFile file)
		{
			return MoveToRecycleBin(file, true);
		}

		/// ------------------------------------------------------------------------------------
		public static bool MoveToRecycleBin(ComponentFile file, bool askForConfirmation)
		{
			var path = file.PathToAnnotatedFile;

			if (!File.Exists(path))
				return false;

			var uiFileName = Path.GetFileName(path);
			if (file.HasSubordinateFiles)
				uiFileName = String.Format(LocalizationManager.GetString(
					"CommonToMultipleViews.FileList.DeleteSubordinateFilesFormat",
					"{0} and subordinate files",
					"Used to format a string that will indicate that subordinate files (i.e., annotation files)" +
					" will also be moved to the recycle bin. Parameter is the name (without path) of the main file being deleted."), uiFileName);
			if (askForConfirmation && !ConfirmRecycleDialog.JustConfirm(uiFileName, file.HasSubordinateFiles, ApplicationContainer.kSayMoreLocalizationId))
				return false;

			return file.Delete();
		}

		/// ------------------------------------------------------------------------------------
		protected internal virtual bool Delete()
		{
			var annotationFile = GetAnnotationFile();

			if (PreDeleteAction != null)
				PreDeleteAction();

			var path = PathToAnnotatedFile;

			// Delete the underlying component file.
			if (!ConfirmRecycleDialog.Recycle(path))
				return false;

			// Delete the file's metadata file.
			var metaPath = path + Settings.Default.MetadataFileExtension;
			if (File.Exists(metaPath))
				ConfirmRecycleDialog.Recycle(metaPath);

			if (annotationFile != null)
				annotationFile.Delete();

			return true;
		}
	}
}