using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Localization;
using Palaso.Code;
using Palaso.Reporting;
using Palaso.UI.WindowsForms.FileSystem;
using SayMore.Model.Fields;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.UI.ElementListScreen;
using SayMore.UI.Archiving;

namespace SayMore.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Both events and people are made up of a number of files: an xml file we help them
	/// edit (i.e. .event or .person), plus any number of other files (videos, texts, images,
	/// etc.). Each of these is represented by an object of this class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ComponentFile
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

		private static readonly Dictionary<string, string> s_fileTypes = new Dictionary<string, string>();
		private static readonly Dictionary<string, Bitmap> s_smallFileIcons = new Dictionary<string, Bitmap>();

		//autofac uses this, so that callers only need to know the path, not all the dependencies
		public delegate ComponentFile Factory(ProjectElement parentElement, string pathToAnnotatedFile);

		public delegate void ValueChangedHandler(ComponentFile file, string fieldId, object oldValue, object newValue);
		public event ValueChangedHandler IdChanged;
		public event ValueChangedHandler MetadataValueChanged;

		protected IEnumerable<ComponentRole> _componentRoles;
		protected FileSerializer _fileSerializer;
		private readonly IProvideAudioVideoFileStatistics _statisticsProvider;
		private readonly PresetGatherer _presetProvider;
		private readonly FieldUpdater _fieldUpdater;

		public string RootElementName { get; protected set; }
		public string PathToAnnotatedFile { get; protected set; }
		public List<FieldInstance> MetaDataFieldValues { get; protected set; }
		public EafFile TranscriptionFile { get; protected set; }
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

		protected string _metaDataPath;
		protected ProjectElement _parentElement;

		/// ------------------------------------------------------------------------------------
		public ComponentFile(ProjectElement parentElement, string pathToAnnotatedFile,
			IEnumerable<FileType> fileTypes, IEnumerable<ComponentRole> componentRoles,
			FileSerializer fileSerializer, IProvideAudioVideoFileStatistics statisticsProvider,
			PresetGatherer presetProvider, FieldUpdater fieldUpdater)
		{
			_parentElement = parentElement;
			PathToAnnotatedFile = pathToAnnotatedFile;
			_componentRoles = componentRoles;
			_fileSerializer = fileSerializer;
			_statisticsProvider = statisticsProvider;
			_presetProvider = presetProvider;
			_fieldUpdater = fieldUpdater;

			DetermineFileType(pathToAnnotatedFile, fileTypes);

			// we musn't do anything to remove the existing extension, as that is needed
			// to keep, say, foo.wav and foo.txt separate. Instead, we just append ".meta"
			//_metaDataPath = ComputeMetaDataPath(pathToAnnotatedFile);

			MetaDataFieldValues = new List<FieldInstance>();

			Guard.AgainstNull(FileType, "At runtime (maybe not in tests) FileType should go to a type intended for unknowns");

			_metaDataPath = FileType.GetMetaFilePath(pathToAnnotatedFile);

			RootElementName = "MetaData";

			if (File.Exists(_metaDataPath))
				Load();

			InitializeFileInfo();
		}

		[Obsolete("For Mocking Only")]
		public ComponentFile(){}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// used only by ProjectElementComponentFile
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected ComponentFile(ProjectElement parentElement, string filePath,
			FileType fileType, string rootElementName,
			FileSerializer fileSerializer, FieldUpdater fieldUpdater)
		{
			RootElementName = rootElementName;
			_parentElement = parentElement;
			FileType = fileType;
			_fileSerializer = fileSerializer;
			_metaDataPath = filePath;
			_fieldUpdater = fieldUpdater;
			MetaDataFieldValues = new List<FieldInstance>();
			_componentRoles = new ComponentRole[] {}; //no roles for person or event
			InitializeFileInfo();
		}

		/// ------------------------------------------------------------------------------------
		protected void DetermineFileType(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes)
		{
			FileType = (fileTypes.FirstOrDefault(t => t.IsMatch(pathToAnnotatedFile)) ??
				fileTypes.FirstOrDefault(t => t.IsForUnknownFileTypes));
		}

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

			if (GetCanHaveTranscriptionFile())
				TranscriptionFile = new EafFile(GetPathToTranscriptionFile(), PathToAnnotatedFile);
		}

		/// ------------------------------------------------------------------------------------
		public DateTime GetCreateDate()
		{
			var fi = new FileInfo(PathToAnnotatedFile);
			return fi.CreationTime;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the component file can have transcriptions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual bool GetCanHaveTranscriptionFile()
		{
			return (FileType.IsAudioOrVideo &&
				".mp3;.wav".Contains(Path.GetExtension(PathToAnnotatedFile)));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the folder where transcriptions are stored for the
		/// component file. If the file type of the component file is not one that can have
		/// transcriptions, then null is returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetTranscriptionFolderName()
		{
			if (!GetCanHaveTranscriptionFile())
				return null;

			return PathToAnnotatedFile + "_transcription";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the transcription file for the component file. If the file
		/// type of the component file is not one that can have transcriptions, then null is
		/// returned.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string GetPathToTranscriptionFile()
		{
			if (!GetCanHaveTranscriptionFile())
				return null;

			var filename = Path.GetFileName(PathToAnnotatedFile);
			return Path.Combine(GetTranscriptionFolderName(), filename + ".eaf");
		}

		/// ------------------------------------------------------------------------------------
		public virtual string DurationString
		{
			get
			{
				if (_statisticsProvider == null)
					return string.Empty;

				var stats = _statisticsProvider.GetFileData(PathToAnnotatedFile);
				if (stats == null || stats.Duration == default(TimeSpan))
					return GetStringValue("Duration", string.Empty);

				//trim off the milliseconds so it doesn't get too geeky
				return new TimeSpan(stats.Duration.Hours,
					stats.Duration.Minutes,
					stats.Duration.Seconds).ToString();
			}
		}

		/// ------------------------------------------------------------------------------------
		protected void LoadFileSizeAndDateModified()
		{
			// Initialize file's display size. File should only not exist during tests.
			var fi = new FileInfo(PathToAnnotatedFile);
			FileSize = (fi.Exists ? GetDisplayableFileSize(fi.Length) : "0 KB");
			DateModified = (fi.Exists ? fi.LastWriteTime.ToString() : string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string GetStringValue(string key, string defaultValue)
		{
			string computedValue = null;

			var computedFieldInfo =
				FileType.GetComputedFields().FirstOrDefault(computedField => computedField.Key == key);

			if (computedFieldInfo != null && _statisticsProvider != null)
			{
				// Get the computed value (if there is one).
				computedValue = computedFieldInfo.GetFormatedStatProvider(
					_statisticsProvider.GetFileData(PathToAnnotatedFile),
					computedFieldInfo.DataItemChooser, computedFieldInfo.Suffix);
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
					string failureMessage;
					SetStringValue(key, computedValue, out failureMessage);
					if (failureMessage != null)
						ErrorReport.NotifyUserOfProblem(failureMessage);

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
		public virtual string SetStringValue(string key, string newValue, out string failureMessage)
		{
			return SetStringValue(new FieldInstance(key, newValue), out failureMessage);
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
					return newValue;

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
		public virtual string SetStringValue(FieldInstance newFieldInstance, out string failureMessage)
		{
			failureMessage = null;

			newFieldInstance.Value = (newFieldInstance.ValueAsString ?? string.Empty).Trim();

			var oldFieldValue =
				MetaDataFieldValues.Find(v => v.FieldId == newFieldInstance.FieldId);

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

			if (_fieldUpdater != null)
				_fieldUpdater.RenameField(this, oldId, newId);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void RemoveField(string idToRemove)
		{
			var existingValue = MetaDataFieldValues.Find(f => f.FieldId == idToRemove);
			if (existingValue != null)
				MetaDataFieldValues.Remove(existingValue);

			if (_fieldUpdater != null)
				_fieldUpdater.DeleteField(this, idToRemove);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Save()
		{
			Save(_metaDataPath);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Save(string path)
		{
			_metaDataPath = path;
			_fileSerializer.Save(MetaDataFieldValues, _metaDataPath, RootElementName);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Load()
		{
			_fileSerializer.CreateIfMissing(_metaDataPath, RootElementName);
			_fileSerializer.Load(/*TODO this.Work, */ MetaDataFieldValues, _metaDataPath, RootElementName);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string TryChangeChangeId(string newId, out string failureMessage)
		{
			throw new NotImplementedException();
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
		/// What part(s) does this file play in the workflow of the event/person?
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ComponentRole> GetAssignedRoles()
		{
			return  GetAssignedRoles(null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// What part(s) does this file play in the workflow of the event/person?
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ComponentRole> GetAssignedRoles(Type elementType)
		{
			return from r in _componentRoles
				   where r.IsMatch(PathToAnnotatedFile) && (elementType == null || elementType == r.RelevantElementType)
				   select r;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Judging by the path (and maybe contents of the file itself), what
		/// parts might this file conceivably play in the workflow of the event/person?
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
				new ComponentRole[] { }, new FileSerializer(null), null, null, null);
		}

		/// ------------------------------------------------------------------------------------
		public static ComponentFile CreateMinimalComponentFileForTests(string path, FileType fileType)
		{
			return new ComponentFile(null, path, new[] { fileType },
				new ComponentRole[] { }, new FileSerializer(null), null, null, null);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ToolStripItem> GetMenuCommands(Action refreshAction)
		{
			foreach (var cmd in FileType.GetCommands(PathToAnnotatedFile))
			{
				var cmd1 = cmd;// needed to avoid "access to modified closure". I.e., avoid executing the wrong command.
				if (cmd1 == null)
					yield return new ToolStripSeparator();
				else
				{
					yield return new ToolStripMenuItem(cmd.EnglishLabel, null, (sender, args) =>
					{
						if (PreFileCommandAction != null)
							PreFileCommandAction();

						cmd1.Action(PathToAnnotatedFile);

						if (refreshAction != null)
							refreshAction();

						if (PostFileCommandAction != null)
							PostFileCommandAction();

					}) { Tag = cmd1.MenuId };
				}
			}

			bool needSeparator = true;

			// commands which assign to roles
			foreach (var role in GetRelevantComponentRoles().Where(role => role.IsPotential(PathToAnnotatedFile)))
			{
				if (needSeparator)
				{
					needSeparator = false;
					yield return new ToolStripSeparator();
				}

				string label = string.Format("Rename For {0}", role.Name);
				var role1 = role;
				var toolStripMenuItem = new ToolStripMenuItem(label, null, (sender, args) =>
				{
					if (PreRenameAction != null)
						PreRenameAction();

					AssignRole(role1);

					if (refreshAction != null)
						refreshAction();

					if (PostRenameAction != null)
						PostRenameAction();

				}) { Tag = "rename" };

				//disable if the file is already named appropriately for this role
				toolStripMenuItem.Enabled = !role.IsMatch(PathToAnnotatedFile);
				yield return toolStripMenuItem;
			}

			if (!(this is ProjectElementComponentFile))
			{
				yield return new ToolStripMenuItem("Custom Rename...", null, (sender, args) =>
				{
					if (PreRenameAction != null)
						PreRenameAction();

					DoCustomRename();

					if (refreshAction != null)
						refreshAction();

					if (PostRenameAction != null)
						PostRenameAction();

				}) { Tag = "rename" };
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ComponentRole> GetRelevantComponentRoles()
		{
			return (_parentElement == null ? _componentRoles :
				_componentRoles.Where(r => r.RelevantElementType == _parentElement.GetType()));
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
		private void RenameAnnotatedFile(string newPath)
		{
			try
			{
				// some types don't have a separate sidecar file (e.g. Person, Event)
				var newMetaPath = FileType.GetMetaFilePath(newPath);
				var renameMetaFile = (newMetaPath != newPath && File.Exists(_metaDataPath));

				if (File.Exists(newPath))
				{
					var msg = LocalizationManager.LocalizeString("ComponentFile.CannotRenameMsg",
						"{0} could not rename the file to '{1}' because there is already a file with that name.");

					ErrorReport.NotifyUserOfProblem(msg, Application.ProductName, newPath);
					return;
				}

				if (renameMetaFile && File.Exists(newMetaPath))
				{
					var msg = LocalizationManager.LocalizeString("ComponentFile.CannotRenameMetadataFileMsg",
						"{0} could not rename the meta data file to '{1}' because there is already a file with that name.");

					ErrorReport.NotifyUserOfProblem(msg, Application.ProductName, newMetaPath);
					return;
				}

				File.Move(PathToAnnotatedFile, newPath);
				if (renameMetaFile)
					File.Move(_metaDataPath, newMetaPath);

				PathToAnnotatedFile = newPath;
			}
			catch (Exception e)
			{
				ErrorReport.NotifyUserOfProblem(e, "Sorry, SayMore could not rename that file because something else (perhaps another part of SayMore) is reading it. Please try again later.");
			}
		}

		/// ------------------------------------------------------------------------------------
		public void DoCustomRename()
		{
			using (var dlg = new CustomComponentFileRenamingDialog(_parentElement.Id, PathToAnnotatedFile))
			{
				if (dlg.ShowDialog() == DialogResult.OK)
					RenameAnnotatedFile(dlg.NewFilePath);
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
		public static bool GetIsValidComponentFile(string fileName)
		{
			fileName = fileName.ToLower();
			var badEndings = Settings.Default.ComponentFileEndingsNotAllowed.Cast<string>();
			return !badEndings.Any(x => fileName.EndsWith(x.ToLower()));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the size of the event file in a displayable form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetDisplayableFileSize(long fileSize)
		{
			return GetDisplayableFileSize(fileSize, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the size of the event file in a displayable form.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string GetDisplayableFileSize(long fileSize, bool abbreviateFileSizeUnits)
		{
			var fmtBytes = "{0} " + (abbreviateFileSizeUnits ? "B" : "Bytes");
			var fmtKilobytes = "{0} " + (abbreviateFileSizeUnits ? "KB" : "Kilobytes");
			var fmtMegabytes = "{0} " + (abbreviateFileSizeUnits ? "MB" : "Megabytes");
			var fmtGigabytes = "{0} " + (abbreviateFileSizeUnits ? "GB" : "Gigabytes");

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
			if (s_fileTypes.TryGetValue(ext, out fileType))
			{
				smallIcon = s_smallFileIcons[ext];
				return;
			}

			SHFILEINFO shinfo = new SHFILEINFO();
			try
			{
				SHGetFileInfo(fullFilePath, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_TYPENAME |
						SHGFI_SMALLICON | SHGFI_ICON | SHGFI_DISPLAYNAME);
			}
			catch (Exception)
			{ }

			// This should only be zero during tests.
			if (shinfo.hIcon != IntPtr.Zero)
			{
				var icon = Icon.FromHandle(shinfo.hIcon);
				smallIcon = icon.ToBitmap();
				DestroyIcon(shinfo.hIcon);
			}

			fileType = shinfo.szTypeName;

			s_fileTypes[ext] = fileType;
			s_smallFileIcons[ext] = smallIcon;
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
			Guard.AgainstNull(_presetProvider, "PresetProvider");
			return _presetProvider.GetPresets();
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
			{
				string failureStringMessage;
				SetStringValue(pair.Key, pair.Value, out failureStringMessage);

				if (!string.IsNullOrEmpty(failureStringMessage))
					ErrorReport.NotifyUserOfProblem(failureStringMessage);
			}

			Save();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Waits for the lock on a file to be released. The method will give up after waiting
		/// for 10 seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WaitForFileRelease(string filePath)
		{
			WaitForFileRelease(filePath, false);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Waits for the lock on a file to be released. The method will give up after waiting
		/// for 10 seconds.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void WaitForFileRelease(string filePath, bool fileOpenedByThisProcess)
		{
			var timeout = DateTime.Now.AddSeconds(10);

			// Now wait until the process lets go of the file.
			while (true)
			{
				try
				{
					if (fileOpenedByThisProcess)
						Application.DoEvents();
					else
						Thread.Sleep(100);

					if (!IsFileLocked(filePath) || DateTime.Now >= timeout)
						return;
				}
				catch
				{
					if (DateTime.Now >= timeout)
						return;
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public static bool IsFileLocked(string filePath)
		{
			if (filePath == null || !File.Exists(filePath))
				return false;

			try
			{
				File.OpenWrite(filePath).Close();
				return false;
			}
			catch
			{
				return true;
			}
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

			if (askForConfirmation && !ConfirmRecycleDialog.JustConfirm(Path.GetFileName(path)))
				return false;

			if (file.PreDeleteAction != null)
				file.PreDeleteAction();

			if (!ConfirmRecycleDialog.Recycle(path))
				return false;

			var metaPath = path + ".meta";
			if (File.Exists(metaPath))
				ConfirmRecycleDialog.Recycle(metaPath);

			return true;
		}
	}
}