using System;
using System.IO;
using System.Xml.Serialization;
using Palaso.Reporting;
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
		public int BirthYear { get; set; }

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
		public static Person CreateFromFile(SpongeProject prj, string fileName)
		{
			if (!Path.IsPathRooted(fileName))
			{
				fileName = Path.GetFileName(fileName);
				fileName = Path.Combine(prj.PeopleFolder, fileName);
			}

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
		public SpongeProject Project { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the name of the file (without its path).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FileName
		{
			get
			{
				return (string.IsNullOrEmpty(FullName) ? null :
					Utils.MakeSafeFileName(FullName, '_') + "." + Sponge.PersonFileExtension);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path (including filename) of the person's file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FullPath
		{
			get { return (Project == null ? null : Path.Combine(Project.PeopleFolder, FileName)); }
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
		/// Gets the full path to the person's picture file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string PictureFile
		{
			get
			{
				if (!Directory.Exists(Project.PeopleFolder))
					throw new DirectoryNotFoundException(Project.PeopleFolder);

				var pattern = Path.ChangeExtension(FileName, "*");
				const string validPicExtensions = ".jpg.gif.tif.png.bmp.dib";

				foreach (var file in Directory.GetFiles(Project.PeopleFolder, pattern))
				{
					if (validPicExtensions.Contains(Path.GetExtension(file).ToLower()))
						return file;
				}

				return null;
			}
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

			if (!Directory.Exists(Project.PeopleFolder))
				throw new DirectoryNotFoundException(Project.PeopleFolder);

			var destFile = Path.ChangeExtension(FullPath, Path.GetExtension(srcFile));
			File.Copy(srcFile, destFile, true);
			return destFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves this instance of the person info to it's file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			FullName = (FullName ?? Project.GetUniquePersonName()).Trim();
			if (FullName == string.Empty)
				FullName = Project.GetUniquePersonName();

			if (CanSave)
				XmlSerializationHelper.SerializeToFile(FullPath, this);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Rename's the person to the new name. This involves setting the person's name and,
		/// if the person's file and picture file exits, renaming those files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Rename(string newName)
		{
			if (string.IsNullOrEmpty(newName))
				newName = Project.GetUniquePersonName();

			// Don't need to do any file renaming if the person hasn't
			// yet been saved to their file.
			if (File.Exists(FullPath))
			{
				var newFileName = Utils.MakeSafeFileName(newName, '_') + "." + Sponge.PersonFileExtension;
				File.Move(FullPath, Path.Combine(Project.PeopleFolder, newFileName));

				var picFile = PictureFile;
				if (picFile != null)
				{
					var picExt = Path.GetExtension(picFile);
					newFileName = Path.ChangeExtension(newFileName, picExt);
					File.Move(picFile, Path.Combine(Project.PeopleFolder, newFileName));
				}
			}

			FullName = newName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Deletes the person's file and, optionally, the person's picture file too.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Delete(bool deletePictureFileToo)
		{
			try
			{
				File.Delete(FullPath);
			}
			catch { }

			if (deletePictureFileToo)
			{
				try
				{
					File.Delete(PictureFile);
				}
				catch { }
			}
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
	}
}
