using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Palaso.Reporting;
using SilUtils;
using Sponge2.Persistence;

namespace Sponge2.Model
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
		private FileSerializer _fileSerializer;
		private const string RootElementName = "MetaData";

		/// ------------------------------------------------------------------------------------
		public ComponentFile(string pathToAnnotatedFile, IEnumerable<FileType> fileTypes,
							 FileSerializer fileSerializer)
		{
			_fileSerializer = fileSerializer;
			PathToAnnotatedFile = pathToAnnotatedFile;

			SetFileType(fileTypes);
		}

		/// ------------------------------------------------------------------------------------
		private void SetFileType(IEnumerable<FileType> fileTypes)
		{
			_fileType = fileTypes.FirstOrDefault(t => t.IsMatch(PathToAnnotatedFile));
			_fileType = _fileType ?? new UnknownFileType();
		}

		public string PathToAnnotatedFile { get; private set; }
		public List<FieldValue> MetaDataFieldValues { get; set; }

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			_fileSerializer.Save(MetaDataFieldValues, MetaDataPath, RootElementName);
		}

		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			_fileSerializer.Load(MetaDataFieldValues, MetaDataPath, RootElementName);
		}

		protected string MetaDataPath
		{
			get { return System.IO.Path.ChangeExtension(PathToAnnotatedFile, "smd"); }
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

		public string FileName
		{
			get { return Path.GetFileName(PathToAnnotatedFile); }
		}

		/// ------------------------------------------------------------------------------------
		public static ComponentFile CreateMinimalComponentFileForTests(string path)
		{
			return new ComponentFile(path, new FileType[] {}, new FileSerializer());
		}

		public List<FieldValue> Fields { get; private set; }

	}
}