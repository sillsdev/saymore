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

		private FileType _fileType;
		private readonly FileSerializer _fileSerializer;
		private string _rootElementName;
		private string _fileNameToAdvertise;

		/// ------------------------------------------------------------------------------------
		public ComponentFile(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes,
							 FileSerializer fileSerializer)
		{
			_fileSerializer = fileSerializer;

			//we musn't do anything to remove the existing extension, as that is needed to keep, say,
			// foo.wav and foo.txt separate. Instead, we just append ".meta"
			MetaDataPath = pathToAnnotatedFile + ".meta";
			_fileNameToAdvertise = Path.GetFileName(pathToAnnotatedFile);
			_rootElementName = "MetaData";

			MetaDataFieldValues = new List<FieldValue>();

			SetFileType(pathToAnnotatedFile, fileTypes);
		}

		/// <summary>
		/// This constructor is for files which are not annotating something else (e.g. person and session)
		/// </summary>
		public ComponentFile(string filePath, FileType fileType,
							 FileSerializer fileSerializer, string rootElementName)
		{
			_fileType = fileType;
			_fileNameToAdvertise = Path.GetFileName(filePath);
			_fileSerializer = fileSerializer;
			MetaDataPath = filePath;
			MetaDataFieldValues = new List<FieldValue>();
			_rootElementName = rootElementName;
		}

		/// ------------------------------------------------------------------------------------
		private void SetFileType(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes)
		{
			_fileType = fileTypes.FirstOrDefault(t => t.IsMatch(pathToAnnotatedFile));
			_fileType = _fileType ?? new UnknownFileType();
		}

		public List<FieldValue> MetaDataFieldValues { get; set; }

		public string GetStringValue(string key, string defaultValue)
		{
			var field =MetaDataFieldValues.FirstOrDefault(v => v.FieldDefinitionKey == key);
			if(field == null)
			{
				return defaultValue;
			}
			return field.Value;
		}

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
			_fileSerializer.Save(MetaDataFieldValues, MetaDataPath, _rootElementName);
		}

		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			_fileSerializer.CreateIfMissing(MetaDataPath, _rootElementName);
			_fileSerializer.Load(MetaDataFieldValues, MetaDataPath, _rootElementName);
		}

		private string MetaDataPath
		{
			get;  set;
		}

		public FileType FileType
		{
			get { return _fileType; }
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
		public static ComponentFile CreateMinimalComponentFileForTests(string path)
		{
			return new ComponentFile(path, new FileType[] {}, new FileSerializer());
		}

		public List<FieldValue> Fields { get; private set; }


		/// <summary>
		/// WARNING: THIS NAME IS HARD-CODED IN THE UI GRID
		/// </summary>
		public string FileName
		{
			get { return _fileNameToAdvertise; }
		}
	}
}