using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SIL.Sponge.Properties;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates all the file information displayed in the NewSessionsFromFileDlg.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NewSessionFile : SessionFileBase
	{
		public bool Selected { get; set; }
		public NewSessionFile(string fullFilePath) : base(fullFilePath) { }
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NewSessionsFromFileModel
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NewSessionsFromFileModel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFileModel()
		{
			Files = new List<NewSessionFile>();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of potential session files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public List<NewSessionFile> Files { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating whether or not any files are selected from which a
		/// session will be created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public bool AnyFilesSelected
		{
			get { return Files.Any(x => x.Selected); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a value indicating how many files are selected from which sessions will be
		/// created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public int NumberOfSelectedFiles
		{
			get { return Files.Count(x => x.Selected); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads a the specified potential session file into the files list.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void AddFile(string fullFilePath)
		{
			if (File.Exists(fullFilePath) &&
				Files.FirstOrDefault(x => x.FullFilePath == fullFilePath) == null)
			{
				Files.Add(new NewSessionFile(fullFilePath));
			}
		}
	}
}