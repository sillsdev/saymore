using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Palaso.Reporting;
using SilUtils;

namespace Sponge2.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Both sessions and people are made up of a number of files: an xml file we help them edit
	/// (i.e. .session or .person), plus any number of other files (videos, texts, images, etc.).
	/// Each of these is represented by an object of this class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[XmlType("componentFile")]
	public class ComponentFile
	{
		//autofac uses this, so that callers only need to know the path, not all the dependencies
		public delegate ComponentFile Factory(string path);

		// John: is it necessary to lug around the whole list? Do you see a problem setting
		// the component file's type during contstruction and not every time it's requested?
		private readonly IEnumerable<FileType> _fileTypes;

		[XmlIgnore]
		public string Path { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads into a ComponentFile object the information from a standoff markup file
		/// associated with the specified path.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static ComponentFile Load(string path)
		{
			var standoffFile = GetStandoffFile(path);

			// Future: Perform migration transforms here on standoffFilie when needed.

			Exception e = null;

			ComponentFile componentFile = (!File.Exists(standoffFile) ? new ComponentFile() :
				XmlSerializationHelper.DeserializeFromFile<ComponentFile>(standoffFile, out e));

			// Review: should this be fatal?
			if (e != null)
				ErrorReport.ReportFatalException(e);

			// Review: will DI provide the file types later?
			componentFile.Path = path;
			return componentFile;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Constructs the full path to the (stand-off markup) data file for the session file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static string GetStandoffFile(string fileName)
		{
			return System.IO.Path.ChangeExtension(fileName, "smd");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Used mainly for serialization.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ComponentFile()
		{
		}

		/// ------------------------------------------------------------------------------------
		public ComponentFile(string path, IEnumerable<FileType> fileTypes)
		{
			_fileTypes = fileTypes;
			Path = path;
		}

		/// ------------------------------------------------------------------------------------
		public bool Save()
		{
			var standoffFile = GetStandoffFile(Path);
			Exception e = null;
			bool result = XmlSerializationHelper.SerializeToFile(standoffFile, this, out e);

			if (e != null)
				ErrorReport.ReportNonFatalException(e);

			return result;
		}

		/// ------------------------------------------------------------------------------------
		/// John: do you have a philosophy behind making this a method rather than a property?
		/// Or, is it arbitrary?
		///
		public FileType GetFileType()
		{
			var fileType = _fileTypes.FirstOrDefault(t => t.IsMatch(Path));
			return fileType ?? new UnknownFileType();
		}

#if notyet
		/// <summary>
		/// What part(s) does this file play in the workflow of the session/person?
		/// </summary>
		public IEnumerable<ComponentRole> GetRoles()
		{
			return new ComponentRole[] {};
		}

		/// <summary>
		/// The roles various people have played in creating/editing this file.
		/// </summary>
		public List<Contribution> Contributions
		{
			get; private set;
		}

		/// <summary>
		/// The metadata we have associated with this file.
		/// </summary>
		public List<FieldValue> MetaDataValues
		{
			get; private set;
		}
#endif

		/// ------------------------------------------------------------------------------------
		public static ComponentFile CreateMinimalComponentFileForTests(string path)
		{
			return new ComponentFile(path, new FileType[]{});
		}
	}
}