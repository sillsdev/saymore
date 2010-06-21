using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Palaso.Code;
using Palaso.Reporting;
using SayMore.Model.Fields;
using SayMore.Model.Files.DataGathering;
using SayMore.Properties;
using SayMore.UI.Utilities;

namespace SayMore.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Both sessions and people are made up of a number of files: an xml file we help them
	/// edit (i.e. .session or .person), plus any number of other files (videos, texts, images,
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

		public string PathToAnnotatedFile { get; protected set; }
		protected IEnumerable<ComponentRole> _componentRoles;
		protected FileSerializer _fileSerializer;
		private readonly IProvideAudioVideoFileStatistics _statisticsProvider;
		private readonly PresetGatherer _presetProvider;
		protected string _rootElementName;

		public List<FieldValue> MetaDataFieldValues { get; set; }
		public List<FieldValue> Fields { get; private set; }
		public FileType FileType { get; private set; }
		public string FileTypeDescription { get; private set; }
		public string FileSize { get; private set; }
		public Image SmallIcon { get; private set; }
		public string DateModified { get; private set; }

		protected string _metaDataPath;

		/// ------------------------------------------------------------------------------------
		public ComponentFile(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes,
							IEnumerable<ComponentRole> componentRoles,
							FileSerializer fileSerializer,
							IProvideAudioVideoFileStatistics statisticsProvider,
							PresetGatherer presetProvider)
		{
			PathToAnnotatedFile = pathToAnnotatedFile;
			_componentRoles = componentRoles;
			_fileSerializer = fileSerializer;
			_statisticsProvider = statisticsProvider;
			_presetProvider = presetProvider;

			DetermineFileType(pathToAnnotatedFile, fileTypes);

			// we musn't do anything to remove the existing extension, as that is needed
			// to keep, say, foo.wav and foo.txt separate. Instead, we just append ".meta"
			//_metaDataPath = ComputeMetaDataPath(pathToAnnotatedFile);

			MetaDataFieldValues = new List<FieldValue>();

			_metaDataPath = FileType.GetMetaFilePath(pathToAnnotatedFile);

			_rootElementName = "MetaData";

			if (File.Exists(_metaDataPath))
				Load();

			InitializeFileInfo();
		}

		/// ------------------------------------------------------------------------------------
		public string DurationString
		{
			get
			{
				if (_statisticsProvider == null)
					return string.Empty;

				var stats = _statisticsProvider.GetFileData(PathToAnnotatedFile);
				return (stats == null || stats.Duration == default(TimeSpan) ?
					string.Empty : stats.Duration.ToString());
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// used only by ProjectElementComponentFile
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected ComponentFile(string filePath, FileType fileType,
			FileSerializer fileSerializer, string rootElementName)
		{
			FileType = fileType;
			_fileSerializer = fileSerializer;
			_metaDataPath = filePath;
			MetaDataFieldValues = new List<FieldValue>();
			_rootElementName = rootElementName;
			_componentRoles = new ComponentRole[] {}; //no roles for person or session
			InitializeFileInfo();
		}

		/// ------------------------------------------------------------------------------------
		protected void DetermineFileType(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes)
		{
			FileType = (fileTypes.FirstOrDefault(t => t.IsMatch(pathToAnnotatedFile)) ??
				new UnknownFileType());
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
			var field = MetaDataFieldValues.FirstOrDefault(v => v.FieldDefinitionKey == key);
			return (field == null ? defaultValue : field.Value);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the value for persisting, and returns the same value, potentially modified
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public virtual string SetValue(string key, string newValue, out string failureMessage)
		{
			failureMessage = null;
			string oldValue = null;
			newValue = (newValue == null ? string.Empty : newValue.Trim());

			var field = MetaDataFieldValues.FirstOrDefault(v => v.FieldDefinitionKey == key);
			if (field != null && field.Value == newValue)
				return newValue;

			if (field == null)
				MetaDataFieldValues.Add(new FieldValue(key, "string", newValue));
			else
			{
				oldValue = field.Value;
				field.Value = newValue;
			}

			LoadFileSizeAndDateModified();
			InvokeMetadataValueChanged(oldValue, newValue);
			return newValue; //overrides may do more
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
			_fileSerializer.Save(MetaDataFieldValues, _metaDataPath, _rootElementName);
		}

		/// ------------------------------------------------------------------------------------
		public virtual void Load()
		{
			_fileSerializer.CreateIfMissing(_metaDataPath, _rootElementName);
			_fileSerializer.Load(MetaDataFieldValues, _metaDataPath, _rootElementName);
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
		/// What part(s) does this file play in the workflow of the session/person?
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
			return new ComponentFile(path, new FileType[] {}, new ComponentRole[]{}, new FileSerializer(), null, null);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ToolStripItem> GetContextMenuItems(Action refreshAction)
		{
			foreach (FileCommand cmd in FileType.Commands)
			{
				FileCommand cmd1 = cmd;// needed to avoid "access to modified closure". I.e., avoid executing the wrong command.
				yield return new ToolStripMenuItem(cmd.EnglishLabel, null, (sender, args) => cmd1.Action(PathToAnnotatedFile));
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
						refreshAction();
					});
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
				File.Move(PathToAnnotatedFile, newPath);
				var newMetaPath = FileType.GetMetaFilePath(newPath);

				//enhance: if somethine goes wrong from here down,
				//this would leave us with one file renamed, but not the other.
				if (newMetaPath != newPath //some types don't have a separate sidecar file (e.g. Person, Session)
					&& File.Exists(_metaDataPath))
				{
					File.Move(_metaDataPath, newMetaPath);
				}

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
		/// Sets the size of the session file in a displayable form.
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
		}
	}
}