using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sponge2.Model.Files
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Both sessions and people are made up of a number of files: an xml file we help them edit
	/// (i.e. .session or .person), plus any number of other files (videos, texts, images, etc.).
	/// Each of these is represented by an object of this class.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ComponentFile
	{
		//autofac uses this, so that callers only need to know the path, not all the dependencies
		public delegate ComponentFile Factory(string pathToAnnotatedFile);

		private readonly FileSerializer _fileSerializer;
		private string _rootElementName;
		private string _fileNameToAdvertise;

		public List<FieldValue> MetaDataFieldValues { get; set; }
		public List<FieldValue> Fields { get; private set; }
		public FileType FileType { get; private set; }

		private string _metaDataPath;

		/// ------------------------------------------------------------------------------------
		public ComponentFile(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes,
							 FileSerializer fileSerializer)
		{
			_fileSerializer = fileSerializer;

			// we musn't do anything to remove the existing extension, as that is needed
			// to keep, say, foo.wav and foo.txt separate. Instead, we just append ".meta"
			_metaDataPath = pathToAnnotatedFile + ".meta";
			_fileNameToAdvertise = Path.GetFileName(pathToAnnotatedFile);
			_rootElementName = "MetaData";

			MetaDataFieldValues = new List<FieldValue>();

			SetFileType(pathToAnnotatedFile, fileTypes);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This constructor is for files which are not annotating something else (e.g. person
		/// and session)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ComponentFile(string filePath, FileType fileType,
							 FileSerializer fileSerializer, string rootElementName)
		{
			FileType = fileType;
			_fileNameToAdvertise = Path.GetFileName(filePath);
			_fileSerializer = fileSerializer;
			_metaDataPath = filePath;
			MetaDataFieldValues = new List<FieldValue>();
			_rootElementName = rootElementName;
		}

		/// ------------------------------------------------------------------------------------
		private void SetFileType(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes)
		{
			FileType = (fileTypes.FirstOrDefault(t => t.IsMatch(pathToAnnotatedFile)) ??
				new UnknownFileType());
		}

		/// ------------------------------------------------------------------------------------
		public string GetStringValue(string key, string defaultValue)
		{
			var field = MetaDataFieldValues.FirstOrDefault(v => v.FieldDefinitionKey == key);
			return (field == null ? defaultValue : field.Value);
		}

		/// ------------------------------------------------------------------------------------
		public void SetValue(string key, string value)
		{
			var field = MetaDataFieldValues.FirstOrDefault(v => v.FieldDefinitionKey == key);
			if (field == null)
			{
				MetaDataFieldValues.Add(new FieldValue(key, "string", value.Trim()));
			}
			else
			{
				field.Value = value;
			}
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

#endif

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// WARNING: THIS NAME IS HARD-CODED IN THE UI GRID
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string FileName
		{
			get { return _fileNameToAdvertise; }
		}

		/// ------------------------------------------------------------------------------------
		public static ComponentFile CreateMinimalComponentFileForTests(string path)
		{
			return new ComponentFile(path, new FileType[] {}, new FileSerializer());
		}
	}
}