using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Localization;
using Palaso.Code;
using Palaso.Reporting;
using SayMore.Model.Fields;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.UI.Utilities;
using SilUtils;

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
#if !MONO
		public const uint SHGFI_DISPLAYNAME = 0x00000200;
		public const uint SHGFI_TYPENAME = 0x400;
		public const uint SHGFI_EXETYPE = 0x2000;
		public const uint SHGFI_ICON = 0x100;
		public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
		public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint
			dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
#endif
		#endregion

		//autofac uses this, so that callers only need to know the path, not all the dependencies
		public delegate ComponentFile Factory(string pathToAnnotatedFile);

		public delegate void ValueChangedHandler(ComponentFile file, string oldValue, string newValue);
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
		public FileType FileType { get; protected set; }
		public string FileTypeDescription { get; protected set; }
		public string FileSize { get; protected set; }
		public Image SmallIcon { get; protected set; }
		public string DateModified { get; protected set; }

		protected string _metaDataPath;

		/// ------------------------------------------------------------------------------------
		public ComponentFile(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes,
			IEnumerable<ComponentRole> componentRoles,
			FileSerializer fileSerializer,
			IProvideAudioVideoFileStatistics statisticsProvider,
			PresetGatherer presetProvider, FieldUpdater fieldUpdater)
		{
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

			_metaDataPath = FileType.GetMetaFilePath(pathToAnnotatedFile);

			RootElementName = "MetaData";

			if (File.Exists(_metaDataPath))
				Load();

			InitializeFileInfo();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// used only by ProjectElementComponentFile
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected ComponentFile(string filePath, FileType fileType, string rootElementName,
			FileSerializer fileSerializer, FieldUpdater fieldUpdater)
		{
			FileType = fileType;
			_fileSerializer = fileSerializer;
			_metaDataPath = filePath;
			_fieldUpdater = fieldUpdater;
			MetaDataFieldValues = new List<FieldInstance>();
			RootElementName = rootElementName;
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

			LoadFileSizeAndDateModified();

			// Initialize file's icon and description.
			Bitmap icon;
			string fileDesc;
			GetSmallIconAndFileType(PathToAnnotatedFile, out icon, out fileDesc);
			SmallIcon = FileType.SmallIcon ?? icon;
			FileTypeDescription = (FileType is UnknownFileType ? fileDesc : FileType.Name);
		}

		/// ------------------------------------------------------------------------------------
		public string DurationString
		{
			get
			{
				if (_statisticsProvider == null)
					return string.Empty;

				var stats = _statisticsProvider.GetFileData(PathToAnnotatedFile);
				if (stats == null || stats.Duration == default(TimeSpan))
				{
					return string.Empty;
				}

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
					computedFieldInfo.DataSetChooser, computedFieldInfo.DataItemChooser,
					computedFieldInfo.Suffix);
			}

			// Get the value from the metadata file.
			var	field = MetaDataFieldValues.FirstOrDefault(v => v.FieldId == key);
			var savedValue = (field == null ? defaultValue : field.Value);

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
					SetValue(key, computedValue, out failureMessage);
					if (failureMessage != null)
						ErrorReport.NotifyUserOfProblem(failureMessage);

					Save();
					return computedValue;
				}
			}

			return savedValue;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the value for persisting, and returns the same value, potentially modified
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual string SetValue(string key, string newValue, out string failureMessage)
		{
			return SetValue(new FieldInstance(key, newValue), out failureMessage);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the value for persisting, and returns the same value, potentially modified
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual string SetValue(FieldInstance newFieldInstance, out string failureMessage)
		{
			failureMessage = null;

			newFieldInstance.Value = (newFieldInstance.Value ?? string.Empty).Trim();

			var oldFieldValue =
				MetaDataFieldValues.Find(v => v.FieldId == newFieldInstance.FieldId);

			if (oldFieldValue == newFieldInstance)
				return newFieldInstance.Value;

			string oldValue = null;

			if (oldFieldValue == null)
				MetaDataFieldValues.Add(newFieldInstance);
			else
			{
				oldValue = oldFieldValue.Value;
				oldFieldValue.Copy(newFieldInstance);
			}

			LoadFileSizeAndDateModified();
			InvokeMetadataValueChanged(oldValue, newFieldInstance.Value);
			return newFieldInstance.Value; //overrides may do more
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
		public void Save()
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
			_fileSerializer.Load(MetaDataFieldValues, _metaDataPath, RootElementName);
		}

		/// ------------------------------------------------------------------------------------
		public virtual string TryChangeChangeId(string newId, out string failureMessage)
		{
			throw new NotImplementedException();
		}

		/// ------------------------------------------------------------------------------------
		protected void InvokeIdChanged(string oldId, string newId)
		{
			if (IdChanged != null)
				IdChanged(this, oldId, newId);
		}

		/// ------------------------------------------------------------------------------------
		protected void InvokeMetadataValueChanged(string oldValue, string newValue)
		{
			if (MetadataValueChanged != null)
				MetadataValueChanged(this, oldValue, newValue);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// What part(s) does this file play in the workflow of the event/person?
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<ComponentRole> GetAssignedRoles()
		{
			return from r in _componentRoles
			   where r.IsMatch(PathToAnnotatedFile)
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
		/// <summary>
		/// The roles various people have played in creating/editing this file.
		/// </summary>
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
		public static ComponentFile CreateMinimalComponentFileForTests(string path)
		{
			return new ComponentFile(path, new FileType[] { new UnknownFileType(null) },
				new ComponentRole[] { }, new FileSerializer(), null, null, null);
		}

		/// ------------------------------------------------------------------------------------
		public static ComponentFile CreateMinimalComponentFileForTests(string path, FileType fileType)
		{
			return new ComponentFile(path, new[] { fileType },
				new ComponentRole[] { }, new FileSerializer(), null, null, null);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ToolStripItem> GetMenuCommands(Action refreshAction)
		{
			foreach (FileCommand cmd in FileType.GetCommands(PathToAnnotatedFile))
			{
				FileCommand cmd1 = cmd;// needed to avoid "access to modified closure". I.e., avoid executing the wrong command.
				if (cmd1 == null)
					yield return new ToolStripSeparator();
				else
				{
					yield return new ToolStripMenuItem(cmd.EnglishLabel, null, (sender, args) =>
					{
						cmd1.Action(PathToAnnotatedFile);
						if (refreshAction != null)
							refreshAction();
					}) { Tag = cmd1.MenuId };
				}
			}

			bool needSeparator = true;

			// commands which assign to roles
			foreach (var role in _componentRoles)
			{
				if (role.IsPotential(PathToAnnotatedFile))
				{
					if (needSeparator)
					{
						needSeparator = false;
						yield return new ToolStripSeparator();
					}

					string label = string.Format("Rename For {0}", role.Name);
					ComponentRole role1 = role;
					yield return new ToolStripMenuItem(label, null, (sender, args) =>
					{
						AssignRole(role1);
						if (refreshAction != null)
							refreshAction();
					}) { Tag = "rename" };
				}
		   }
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

					Utils.MsgBox(string.Format(msg, Application.ProductName, newPath));
					return;
				}

				if (renameMetaFile && File.Exists(newMetaPath))
				{
					var msg = LocalizationManager.LocalizeString("ComponentFile.CannotRenameMetadataFileMsg",
						"{0} could not rename the metadata file to '{1}' because there is already a file with that name.");

					Utils.MsgBox(string.Format(msg, Application.ProductName, newMetaPath));
					return;
				}

				File.Move(PathToAnnotatedFile, newPath);
				if (renameMetaFile)
					File.Move(_metaDataPath, newMetaPath);

				PathToAnnotatedFile = newPath;
			}
			catch (Exception e)
			{
				ErrorReport.ReportNonFatalException(e);
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
		private static string GetDisplayableFileSize(long fileSize)
		{
			if (fileSize < 1000)
				return string.Format("{0} B", fileSize);

			if (fileSize < Math.Pow(1024, 2))
			{
				var size = (int)Math.Round(fileSize / (decimal)1024, 2, MidpointRounding.AwayFromZero);
				if (size < 1)
					size = 1;

				return string.Format("{0} KB", size.ToString("###"));
			}

			double dblSize;
			if (fileSize < Math.Pow(1024, 3))
			{
				dblSize = Math.Round(fileSize / Math.Pow(1024, 2), 2, MidpointRounding.AwayFromZero);
				return string.Format("{0} MB", dblSize.ToString("###.##"));
			}

			dblSize = Math.Round(fileSize / Math.Pow(1024, 3), 2, MidpointRounding.AwayFromZero);
			return string.Format("{0} GB", dblSize.ToString("###,###.##"));
		}

		/// ------------------------------------------------------------------------------------
		private static void GetSmallIconAndFileType(string fullFilePath, out Bitmap smallIcon,
			out string fileType)
		{
#if !MONO
			SHFILEINFO shinfo = new SHFILEINFO();
			SHGetFileInfo(fullFilePath, 0, ref
				shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_TYPENAME |
				SHGFI_SMALLICON | SHGFI_ICON | SHGFI_DISPLAYNAME);

			// This should only be zero during tests.
			smallIcon = (shinfo.hIcon == IntPtr.Zero ?
				null : Icon.FromHandle(shinfo.hIcon).ToBitmap());

			fileType = shinfo.szTypeName;
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
				SetValue(pair.Key, pair.Value, out failureStringMessage);

				if (!string.IsNullOrEmpty(failureStringMessage))
					ErrorReport.NotifyUserOfProblem(failureStringMessage);
			}

			Save();
		}
	}
}