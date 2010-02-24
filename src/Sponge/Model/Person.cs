using System;
using System.IO;
using System.Xml.Serialization;
using Palaso.Reporting;
using SilUtils;
using System.Drawing;

namespace SIL.Sponge.Model
{
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

		#region Construction
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="Person"/> class.
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

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Gets the file's path (i.e. full path without file name).
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[XmlIgnore]
		//public string FilePath
		//{
		//    get { return Path.GetDirectoryName(m_fileName); }
		//}

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

		#endregion

		public string GetPictureFile(string folderPath)
		{
			if (!Directory.Exists(folderPath))
				throw new DirectoryNotFoundException(folderPath);

			var pattern = Path.ChangeExtension(FileName, "*");
			const string validPicExtensions = ".jpg.gif.tif.png.bmp.dib";

			foreach (var file in Directory.GetFiles(folderPath, pattern))
			{
				if (validPicExtensions.Contains(Path.GetExtension(file)))
					return file;
			}

			return null;
		}

		public string SetPictureFile(string destPath, string srcFile)
		{
			if (!File.Exists(srcFile))
				throw new FileNotFoundException(srcFile);

			if (!Directory.Exists(destPath))
				throw new DirectoryNotFoundException(destPath);

			var destFile = Path.Combine(destPath, FileName);
			destFile = Path.ChangeExtension(destFile, Path.GetExtension(srcFile));
			File.Copy(srcFile, destFile, true);
			return destFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Saves this instance of the person info to it's file in the specified folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Save(string folderPath)
		{
			if (!Directory.Exists(folderPath))
				throw new DirectoryNotFoundException(folderPath);

			if (CanSave)
			{
				folderPath = Path.Combine(folderPath, FileName);
				XmlSerializationHelper.SerializeToFile(folderPath, this);
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
