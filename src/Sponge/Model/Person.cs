using System;
using System.IO;
using System.Xml.Serialization;
using Palaso.Reporting;
using SIL.Sponge.Properties;
using SilUtils;

namespace SIL.Sponge.Model
{
	#region Enumerations
	public enum Privacy
	{
		Public,
		Private,
		Unknown
	}

	public enum ParentType
	{
		Father,
		Mother
	}

	public enum Gender
	{
		Male,
		Female
	}

	#endregion

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates information about a person (i.e. contributor)
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlRoot("person")]
	public class Person
	{
		#region Serialized/Deserialized Properties
		[XmlElement("fullName")]
		public string FullName { get; set; }

		[XmlElement("primaryLanguage")]
		public string PrimaryLanguage { get; set; }

		[XmlElement("learnedLanguageIn")]
		public string LearnedLanguageIn { get; set; }

		[XmlElement("fathersLanguage")]
		public string FathersLanguage { get; set; }

		[XmlElement("mothersLanguage")]
		public string MothersLanguage { get; set; }

		[XmlElement("birthYear")]
		public string BirthYear { get; set; }

		[XmlElement("gender")]
		public Gender Gender { get; set; }

		[XmlElement("otherLangauge0")]
		public string OtherLangauge0 { get; set; }

		[XmlElement("otherLangauge1")]
		public string OtherLangauge1 { get; set; }

		[XmlElement("otherLangauge2")]
		public string OtherLangauge2 { get; set; }

		[XmlElement("otherLangauge3")]
		public string OtherLangauge3 { get; set; }

		[XmlElement("primaryOccupation")]
		public string PrimaryOccupation { get; set; }

		[XmlElement("education")]
		public string Education { get; set; }

		[XmlElement("contactInfo")]
		public string ContactInfo { get; set; }

		[XmlElement("notes")]
		public string Notes { get; set; }

		#endregion

		#region Creation and Construction methods
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a Person object by deserializing the specified file. The file can be just
		/// the name of the file, without the path, or the full path specification. If that
		/// fails, null is returned or, when there's an exception, it is thrown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Person Load(SpongeProject prj, string pathName)
		{
			var fileName = Path.GetFileName(pathName);
			var folder = fileName;
			if (folder.EndsWith("." + Sponge.PersonFileExtension))
				folder = fileName.Remove(folder.Length - (Sponge.PersonFileExtension.Length + 1));
			else
				fileName += ("." + Sponge.PersonFileExtension);

			folder = Path.Combine(prj.PeopleFolder, folder);
			fileName = Path.Combine(folder, fileName);

			Exception e;
			var person = XmlSerializationHelper.DeserializeFromFile<Person>(fileName, out e);
			if (e != null)
			{
				var msg = ExceptionHelper.GetAllExceptionMessages(e);
				Utils.MsgBox(msg);
			}

			person.Project = prj;
			return person;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a Person object for the specified person's full name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Person CreateFromName(SpongeProject prj, string personName)
		{
			return new Person(prj, personName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Person"/> class. This should only
		/// be used for deserialization/serialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Person()
		{
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Person"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Person(SpongeProject prj, string personName)
		{
			Project = prj;
			FullName = personName;
		}

		#endregion

		#region Properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the person's owning project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public SpongeProject Project { get; /*private*/ set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the person's name in a form that's safe to be used as a file or folder
		/// name. (i.e. all invalid path characters are replaced with an underscore.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string SafeName
		{
			get
			{
				return (string.IsNullOrEmpty(FullName) ? null :
					Utils.MakeSafeFileName(FullName, Settings.Default.SafeNameReplacementChar));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the person file (without its path).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FileName
		{
			get
			{
				return (string.IsNullOrEmpty(SafeName) ? null :
					SafeName + "." + Sponge.PersonFileExtension);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path of the folder in which the person's file, picture file and
		/// permissions are stored. (e.g. ...\People\Eggbert)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string Folder
		{
			get
			{
				return (Project == null || SafeName == null ? null :
					Path.Combine(Project.PeopleFolder, SafeName));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path (including filename and extension) of the person's file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FullFilePath
		{
			get { return (Folder == null || FileName == null ? null : Path.Combine(Folder, FileName)); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the person's permissions folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string PermissionsFolder
		{
			get { return (Folder == null ? null : Path.Combine(Folder, Sponge.PermissionsFolderName)); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the image resource used when the person has no permissions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string ImageKey
		{
			get
			{
				// If the full name is null or blank, it likely means the person hasn't
				// been saved to their file yet, so in that case, it's a bit premature
				// to return an image key.
				return (HasPermissions || string.IsNullOrEmpty(FullName) ?
					null : "kimidNoPermissionsWarning");
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the person file can be saved (i.e. has
		/// the full name been specified).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool CanSave
		{
			get
			{
				return (!string.IsNullOrEmpty(Project.PeopleFolder) &&
					Directory.Exists(Project.PeopleFolder) && !string.IsNullOrEmpty(FileName));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not the person has any associated permissions.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public bool HasPermissions
		{
			get { return (PermissionFiles.Length > 0); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a list of the person's permission files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string[] PermissionFiles
		{
			get
			{
				return (PermissionsFolder == null || !Directory.Exists(PermissionsFolder) ?
					new string[] { } : Directory.GetFiles(PermissionsFolder, "*.*"));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the person's picture file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string PictureFile
		{
			get
			{
				var pattern = Path.ChangeExtension(FileName, "*");
				const string validPicExtensions = ".jpg.gif.tif.png.bmp.dib";

				foreach (var file in Directory.GetFiles(Folder, pattern))
				{
					if (validPicExtensions.Contains(Path.GetExtension(file).ToLower()))
						return file;
				}

				return null;
			}
		}

		public bool CanChoosePicture
		{
			get { return Folder != null; }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the picture file, by copying the specifed srcFile to the peoples folder and
		/// giving the copied file the same name as the person (while keeping the source
		/// file's extension).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CopyPictureFile(string srcFile)
		{
			if (srcFile == null)
				throw new NullReferenceException("srcFile");

			if (!File.Exists(srcFile))
				throw new FileNotFoundException(srcFile);

			EnsureFolderExists();

			var destFile = Path.ChangeExtension(FullFilePath, Path.GetExtension(srcFile));
			File.Copy(srcFile, destFile, true);
			return destFile;
		}

		private void EnsureFolderExists()
		{
			if (!Directory.Exists(Folder))
				Directory.CreateDirectory(Folder);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Adds the specified permissions file to the person's list of permissions files.
		/// (This will copy the specified file to the person's permissions file folder.)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddPermissionFile(string srcFile)
		{
			if (srcFile == null)
				throw new NullReferenceException("srcFile");

			if (!File.Exists(srcFile))
				throw new FileNotFoundException(srcFile);

			if (!Directory.Exists(Folder))
				throw new DirectoryNotFoundException(Folder);

			if (Path.GetDirectoryName(srcFile) == PermissionsFolder)
				return;

			if (!Directory.Exists(PermissionsFolder))
				Directory.CreateDirectory(PermissionsFolder);

			var destFile = Path.GetFileName(srcFile);
			destFile = Path.Combine(PermissionsFolder, destFile);
			File.Copy(srcFile, destFile, true);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the specified permissions file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void DeletePermissionsFile(string file)
		{
			if (file == null)
				throw new NullReferenceException("file");

			if (!File.Exists(file))
				throw new FileNotFoundException(file);

			if (!Directory.Exists(Folder))
				throw new DirectoryNotFoundException(Folder);

			File.Delete(file);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves this instance of the person info to its file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			FullName = (FullName ?? Project.GetUniquePersonName()).Trim();
			if (FullName == string.Empty)
				FullName = Project.GetUniquePersonName();

			EnsureFolderExists();

			if (CanSave)
				XmlSerializationHelper.SerializeToFile(FullFilePath, this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Rename's the person to the new name. This involves setting the person's name and,
		/// if the person's file and picture file exits, renaming those files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Rename(string newName)
		{
			// REVIEW: Do we need to check for the new name already exisiting, or is it
			// enough that the UI never lets that happen?

			if (string.IsNullOrEmpty(newName))
				newName = Project.GetUniquePersonName();

			var picFile = PictureFile;
			var newSafeName = Utils.MakeSafeFileName(newName, Settings.Default.SafeNameReplacementChar);
			var newFolder = Path.Combine(Project.PeopleFolder, newSafeName);

			// Rename person's folder.
			if (Directory.Exists(Folder))
				Directory.Move(Folder, newFolder);

			// Rename person's file.
			var srcFile = Path.Combine(newFolder, FileName);
			var dstFile = Path.Combine(newFolder, newSafeName + "." + Sponge.PersonFileExtension);
			if (File.Exists(srcFile))
				File.Move(srcFile, dstFile);

			// Rename person's picture file.
			if (picFile != null)
			{
				picFile = Path.GetFileName(picFile);
				var picExt = Path.GetExtension(picFile);
				srcFile = Path.Combine(newFolder, picFile);
				dstFile = Path.Combine(newFolder, newSafeName + picExt);
				File.Move(srcFile, dstFile);
			}

			FullName = newName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the person's folder and, of course, everything in it.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Delete()
		{
			try
			{
				Directory.Delete(Folder, true);
			}
			catch { }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return FullName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Changes the name.
		/// </summary>
		/// <param name="newName">The new name.</param>
		/// <exception cref="ApplicationException">Throws if it can't make the change.</exception>
		/// ------------------------------------------------------------------------------------
		public void ChangeName(string newName)
		{
			newName = newName.Trim();
			if (FullName == newName)
			{
				return;
			}
//
//            if(FullName == null && newName == string.Empty)
//            {
//                return;//ok, we're no worse
//            }
			if (newName == string.Empty)
			{
				throw new ApplicationException("Name cannot be empty.");
			}

			if (!string.IsNullOrEmpty(FullName) && !string.IsNullOrEmpty(Folder))
			{
				var parent = Directory.GetParent(Folder).FullName;
				string newFolderPath = Path.Combine(parent, newName);
				if (Directory.Exists(newFolderPath))
				{
					throw new ApplicationException("There is already someone with that name.");
				}

				MoveAndRenameFiles(newName);
				Directory.Move(Folder, newFolderPath);
			}

			FullName = newName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Moves the and rename files.
		/// </summary>
		/// <param name="newName">The new name.</param>
		/// ------------------------------------------------------------------------------------
		private void MoveAndRenameFiles(string newName)
		{
			foreach (var file in Directory.GetFiles(Folder))
			{
				var name = Path.GetFileName(file);
				if (!name.ToLower().StartsWith(FullName.ToLower()))
					return;

				//todo: do a case-insensitive replacement
				//todo... this could over-replace
				try
				{
					File.Move(file, Path.Combine(Folder, name.Replace(FullName, newName)));
				}
				catch (ArgumentException error)
				{
					throw new ApplicationException("There is a problem with the name: " + Environment.NewLine + error.Message);
				}
			}
		}
	}
}
