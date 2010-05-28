using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Palaso.Reporting;
using SayMore.Model.Fields;
using SayMore.Properties;

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
		//autofac uses this, so that callers only need to know the path, not all the dependencies
		public delegate ComponentFile Factory(string pathToAnnotatedFile);

		/// <summary>
		/// Things like list views should hook into this event
		/// </summary>
		public event EventHandler UiShouldRefresh;

		public string PathToAnnotatedFile { get; protected set; }
		private readonly IEnumerable<ComponentRole> _componentRoles;
		private readonly FileSerializer _fileSerializer;
		private string _rootElementName;

		public List<FieldValue> MetaDataFieldValues { get; set; }
		public List<FieldValue> Fields { get; private set; }
		public FileType FileType { get; private set; }

		private string _metaDataPath;

		/// ------------------------------------------------------------------------------------
		public ComponentFile(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes,
							IEnumerable<ComponentRole> componentRoles,
							FileSerializer fileSerializer)
		{
			PathToAnnotatedFile = pathToAnnotatedFile;
			_componentRoles = componentRoles;
			_fileSerializer = fileSerializer;

			// we musn't do anything to remove the existing extension, as that is needed
			// to keep, say, foo.wav and foo.txt separate. Instead, we just append ".meta"
			_metaDataPath = ComputeMetaDataPath(pathToAnnotatedFile);

			_rootElementName = "MetaData";

			MetaDataFieldValues = new List<FieldValue>();

			DetermineFileType(pathToAnnotatedFile, fileTypes);

			if (File.Exists(_metaDataPath))
				Load();
		}

		/// ------------------------------------------------------------------------------------
		private static string ComputeMetaDataPath(string pathToAnnotatedFile)
		{
			return pathToAnnotatedFile + Settings.Default.MetadataFileExtension;
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
			_componentRoles = new ComponentRole[] {};//no roles for person or session
		}

		/// ------------------------------------------------------------------------------------
		private void DetermineFileType(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes)
		{
			FileType = (fileTypes.FirstOrDefault(t => t.IsMatch(pathToAnnotatedFile)) ??
				new UnknownFileType());
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
		public virtual string SetValue(string key, string value, out string failureMessage)
		{
			failureMessage = null;
			var field = MetaDataFieldValues.FirstOrDefault(v => v.FieldDefinitionKey == key);
			if (field == null)
			{
				MetaDataFieldValues.Add(new FieldValue(key, "string", value.Trim()));
			}
			else
			{
				field.Value = value;
			}

			return value; //overrides may do more
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			Save(_metaDataPath);
		}

		/// ------------------------------------------------------------------------------------
		public void Save(string path)
		{
			_metaDataPath = path;
			_fileSerializer.Save(MetaDataFieldValues, _metaDataPath, _rootElementName);
		}

		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			_fileSerializer.CreateIfMissing(_metaDataPath, _rootElementName);
			_fileSerializer.Load(MetaDataFieldValues, _metaDataPath, _rootElementName);
		}

		/// ------------------------------------------------------------------------------------
		protected void InvokeUiShouldRefresh()
		{
			if (UiShouldRefresh != null)
			{
				UiShouldRefresh(this, null);
			}
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
		public Image SmallIcon
		{
			get { return FileType.SmallIcon; }
		}

		/// ------------------------------------------------------------------------------------
		public string FileSize
		{
			get { return FileType.FileSize; }
		}

		/// ------------------------------------------------------------------------------------
		public static ComponentFile CreateMinimalComponentFileForTests(string path)
		{
			return new ComponentFile(path, new FileType[] {}, new ComponentRole[]{}, new FileSerializer());
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
				var newMetaPath = ComputeMetaDataPath(newPath);
				//enhance: if somethine goes wrong from here down,
				//this would leave us with one file renamed, but not the other.
				if (File.Exists(_metaDataPath))
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
	}
}