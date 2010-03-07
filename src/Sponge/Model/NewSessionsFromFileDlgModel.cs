using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		public NewSessionFile(string fullFilePath) : base(fullFilePath)
		{
			Selected = true;
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class NewSessionsFromFileDlgModel
	{
		private string m_selectedFolder;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NewSessionsFromFileDlgModel"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public NewSessionsFromFileDlgModel()
		{
			Files = new List<NewSessionFile>();
			SelectedFolder = Settings.Default.NewSessionsFromFilesLastFolder;
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
		/// Gets or sets the selected folder from which new sessions will be created.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string SelectedFolder
		{
			get { return m_selectedFolder; }
			set
			{
				m_selectedFolder = value;
				LoadFilesFromFolder(value);
				Settings.Default.NewSessionsFromFilesLastFolder = value;
				Settings.Default.Save();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Loads the list of files from the audio and video files found in the specified
		/// folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void LoadFilesFromFolder(string folder)
		{
			Files.Clear();

			if (folder == null || !Directory.Exists(folder))
				return;

			var validExtensions = (Settings.Default.AudioFileExtensions +
				Settings.Default.VideoFileExtensions).ToLower();

			foreach (var file in Directory.GetFiles(folder))
			{
				if (validExtensions.Contains(Path.GetExtension(file.ToLower())))
					Files.Add(new NewSessionFile(file));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Refreshes the file list by rereading the files from the selected folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void Refresh()
		{
			LoadFilesFromFolder(SelectedFolder);
		}
	}
}