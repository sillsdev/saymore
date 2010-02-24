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
		public const string PersonFileExtension = "person";

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

		#region static methods for creating a new person object
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a Person object by deserializing the specified file. If that fails, null
		/// is returned or, when there's an exception, it is thrown.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Person CreateFromFile(string fileName)
		{
			Exception e;
			var person = XmlSerializationHelper.DeserializeFromFile<Person>(fileName, out e);
			if (e != null)
			{
				var msg = ExceptionHelper.GetAllExceptionMessages(e);
				Utils.MsgBox(msg);
			}

			return person;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a Person object for the specified person's full name.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static Person CreateFromName(string personName)
		{
			return new Person(personName);
		}

		#endregion

		#region Other static methods and properties
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the path to the folder in which people information files are stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string PeoplesPath { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the people folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static void InitializePeopleFolder(string prjPath)
		{
			PeoplesPath = Path.Combine(prjPath, "People");
			if (!Directory.Exists(PeoplesPath))
				Directory.CreateDirectory(PeoplesPath);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets all the full paths to the people files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string[] PeopleFiles
		{
			get { return Directory.GetFiles(PeoplesPath, "*." + PersonFileExtension); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the first available, unique, unknown name for a person.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public static string UniqueName
		{
			get
			{
				const string fmt = "Unknown Name {0:D2}";
				int i = 1;
				while (true)
				{
					var name = string.Format(fmt, i++);
					var path = Path.Combine(PeoplesPath, name);
					path = Path.ChangeExtension(path, PersonFileExtension);
					if (!File.Exists(path))
						return name;
				}
			}
		}

		#endregion

		#region Construction
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Person"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Person()
		{
			FullName = UniqueName;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Person"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private Person(string personName)
		{
			FullName = personName;
		}

		#endregion

		#region Properties
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
					Utils.MakeSafeFileName(FullName, '_') + "." + PersonFileExtension);
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
			get { return (!string.IsNullOrEmpty(FileName)); }
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
				if (!Directory.Exists(PeoplesPath))
					throw new DirectoryNotFoundException(PeoplesPath);

				var pattern = Path.ChangeExtension(FileName, "*");
				const string validPicExtensions = ".jpg.gif.tif.png.bmp.dib";

				foreach (var file in Directory.GetFiles(PeoplesPath, pattern))
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
		/// giving the copyied file the same name as the person (while keeping the source
		/// file's extension).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string CopyPictureFile(string srcFile)
		{
			if (!File.Exists(srcFile))
				throw new FileNotFoundException(srcFile);

			if (!Directory.Exists(PeoplesPath))
				throw new DirectoryNotFoundException(PeoplesPath);

			var destFile = Path.Combine(PeoplesPath, FileName);
			destFile = Path.ChangeExtension(destFile, Path.GetExtension(srcFile));
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
			if (!Directory.Exists(PeoplesPath))
				throw new DirectoryNotFoundException(PeoplesPath);

			if (CanSave)
			{
				var fullPath = Path.Combine(PeoplesPath, FileName);
				XmlSerializationHelper.SerializeToFile(fullPath, this);
			}
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
				newName = UniqueName;

			// Don't need to do any file renaming if the person hasn't
			// yet been saved to their file.
			if (File.Exists(Path.Combine(PeoplesPath, FileName)))
			{
				var newFileName = Utils.MakeSafeFileName(newName, '_') + "." + PersonFileExtension;
				File.Move(Path.Combine(PeoplesPath, FileName), Path.Combine(PeoplesPath, newFileName));

				var picFile = PictureFile;
				if (picFile != null)
				{
					var picExt = Path.GetExtension(picFile);
					newFileName = Path.ChangeExtension(newFileName, picExt);
					File.Move(picFile, Path.Combine(PeoplesPath, newFileName));
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
				File.Delete(Path.Combine(PeoplesPath, FileName));
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
