using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Palaso.Code;
using Palaso.Reporting;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.Model
{
	/// <summary>
	/// A project is made of sessions and people, each of which subclass from this simple class.
	/// Here, we call those things "ProjectElemements"
	/// </summary>
	public abstract class ProjectElement
	{
		/// <summary>
		/// This lets us make componentFile instances without knowing all the inputs they need
		/// </summary>
		private readonly ComponentFile.Factory _componentFileFactory;
		private readonly FileSerializer _fileSerializer;
		private string _id;

		public string Id { get { return _id; } }
		public ProjectElementComponentFile MetaDataFile { get; private set; }
		public abstract string RootElementName { get; }
		protected internal string ParentFolderPath { get; set; }
		protected abstract string ExtensionWithoutPeriod { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Use this for creating new or existing elements
		/// </summary>
		/// <param name="parentElementFolder">E.g. "c:/MyProject/Sessions"</param>
		/// <param name="id">e.g. "ETR007"</param>
		/// <param name="componentFileFactory"></param>
		/// <param name="fileSerializer">used to load/save</param>
		/// <param name="fileType"></param>
		/// ------------------------------------------------------------------------------------
		protected ProjectElement(string parentElementFolder, string id,
			ComponentFile.Factory componentFileFactory, FileSerializer fileSerializer,
			FileType fileType)
		{
			_componentFileFactory = componentFileFactory;
			_fileSerializer = fileSerializer;
			RequireThat.Directory(parentElementFolder).Exists();

			ParentFolderPath = parentElementFolder;
			_id = id;

			MetaDataFile = new ProjectElementComponentFile(this, fileType,
				_fileSerializer, RootElementName);

			if (File.Exists(SettingsFilePath))
				Load();
			else
			{
				Directory.CreateDirectory(FolderPath);
				Save();
			}
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ComponentFile> GetComponentFiles()
		{
			// John: Should we cache this?
			// Ansr: if it proves slow, but then we have to complicate things to keep it up to date.

			//this is the actual person or session data
			yield return MetaDataFile;

			//these are the other files we find in the folder
			var otherFiles = from x in Directory.GetFiles(FolderPath, "*.*")
							 where (
								 !x.EndsWith("." + ExtensionWithoutPeriod) &&
								 !x.EndsWith(Settings.Default.MetadataFileExtension) &&
								 !x.ToLower().EndsWith("thumbs.db"))
							 orderby x
							 select _componentFileFactory(x);

			foreach (var file in otherFiles)
			{
				yield return file;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string FolderPath
		{
			get
			{
				return Path.Combine(ParentFolderPath, Id);
			}
		}

		/// ------------------------------------------------------------------------------------
		public string SettingsFilePath
		{
			get
			{
				return Path.Combine(FolderPath, Id + "." + ExtensionWithoutPeriod);
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			MetaDataFile.Save(SettingsFilePath);
		}

		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			MetaDataFile.Load();
		}

		/// ------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Id;
		}

		/// ------------------------------------------------------------------------------------
		public bool AddComponentFile(string fileToAdd)
		{
			return AddComponentFiles(new[] { fileToAdd });
		}

		/// ------------------------------------------------------------------------------------
		public bool AddComponentFiles(string[] filesToAdd)
		{
			filesToAdd = RemoveInvalidFilesFromProspectiveFilesToAdd(filesToAdd).ToArray();
			if (filesToAdd.Length == 0)
				return false;

			foreach (var srcFile in filesToAdd)
			{
				try
				{
					var destFile = Path.Combine(FolderPath, Path.GetFileName(srcFile));
					File.Copy(srcFile, destFile);
				}
				catch (Exception e)
				{
					ErrorReport.ReportNonFatalException(e);
				}
			}

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> RemoveInvalidFilesFromProspectiveFilesToAdd(string[] filesBeingAdded)
		{
			if (filesBeingAdded == null)
				filesBeingAdded = new string[] { };

			foreach (var prospectiveFile in filesBeingAdded.Where(ComponentFile.GetIsValidComponentFile))
			{
				if (!File.Exists(Path.Combine(FolderPath, Path.GetFileName(prospectiveFile))))
					yield return prospectiveFile;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// The reason this is separate from the Id property is: 1) You're not supposed to do
		/// anything non-trivial in property accessors (like renaming folders) and 2) It may
		/// fail, and needs a way to indicate that to the caller.
		///
		/// NB: at the moment, all the change is done immediately, so a Save() is needed to
		/// keep things consistent. We could imagine just making the change pending until
		/// the next Save.
		/// </summary>
		/// <returns>true if the change was possible and occurred</returns>
		/// ------------------------------------------------------------------------------------
		public bool TryChangeIdAndSave(string newId, out string failureMessage)
		{
			failureMessage = null;
			Save();
			newId = newId.Trim();
			if (_id == newId)
			{
				return true;
			}

			if (newId == string.Empty)
			{
				failureMessage = "You must specify a session id.";
				return false;
			}

			var parent =  Directory.GetParent(FolderPath).FullName;
			string newFolderPath = Path.Combine(parent, newId);
			if (Directory.Exists(newFolderPath))
			{
				failureMessage = string.Format(
					"Could not rename from {0} to {1} because there is already a session by that name.", Id, newId);

				return false;
			}

			//var previousFileWatchingStatus = Project.EnableFileWatching;
			try
			{
				//Project.EnableFileWatching = false;
				//todo... need a way to make this all one big all or nothing transaction.  As it is, some things can be
				//renamed and then we run into a snag, and we're left in a bad, inconsistent state.

				// for now, at least check for the very common situation where the rename of the
				// directory itself will fail, and find that out *before* we do the file renamings
				try
				{
					Directory.Move(FolderPath, FolderPath + "Renaming");
					Directory.Move(FolderPath + "Renaming", FolderPath);
				}
				catch
				{
					failureMessage = "Something is holding onto that folder or a file in it, so it cannot be renamed. You can try restarting this program, or restarting the computer.";
					return false;
				}

				foreach (var file in Directory.GetFiles(FolderPath))
				{
					var name = Path.GetFileName(file);
					if (name.ToLower().StartsWith(Id.ToLower()))// to be conservative, let's only trigger if it starts with the id
					{
						//todo: do a case-insensitive replacement
						//todo... this could over-replace
						File.Move(file, Path.Combine(FolderPath, name.Replace(Id, newId)));
					}
				}

				//Project.EnableFileWatching = previousFileWatchingStatus;
				Directory.Move(FolderPath, newFolderPath);
			}
			catch (Exception e)
			{
				failureMessage = ExceptionHelper.GetAllExceptionMessages(e);
				return false;
			}
			//finally
			//{
			//    Project.EnableFileWatching = previousFileWatchingStatus;
			//}

			_id = newId;
			Save();

			return true;
		}
	}
}
