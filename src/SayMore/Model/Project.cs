using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using SayMore.Properties;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A project corresponds to a single folder (with subfolders) on the disk.
	/// In that folder is a file which persists the settings, then a folder of
	/// people, and another of events.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Project
	{
		private const string EventFolderName = "Events";

		public delegate Project Factory(string desiredOrExistingFilePath);
		//public delegate Project FactoryForNew(string parentDirectory, int x, string projectName);

		public Event.Factory EventFactory { get; set; }
		public Func<Event, Event> EventPropertyInjectionMethod { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// can be used whether the project exists already, or not
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Project(string desiredOrExistingSettingsFilePath)
		{
			SettingsFilePath = desiredOrExistingSettingsFilePath;
			var projectDirectory = Path.GetDirectoryName(desiredOrExistingSettingsFilePath);
			var parentDirectoryPath = Path.GetDirectoryName(projectDirectory);

			if (File.Exists(desiredOrExistingSettingsFilePath))
			{
				RenameSessionsToEvents(projectDirectory);
				Load();
			}
			else
			{
				if (!Directory.Exists(parentDirectoryPath))
					Directory.CreateDirectory(parentDirectoryPath);

				if (!Directory.Exists(projectDirectory))
					Directory.CreateDirectory(projectDirectory);

				Save();
			}
		}

//		public Project(string parentDirectory,  string projectName)
//		{
//			RequireThat.Directory(parentDirectory).Exists();
//
//			var projectDirectory = Path.Combine(parentDirectory, projectName);
//			RequireThat.Directory(projectDirectory).DoesNotExist();
//			Directory.CreateDirectory(projectDirectory);
//
//			SettingsFilePath = Path.Combine(projectDirectory, projectName + "." + ProjectSettingsFileExtension);
//			RequireThat.File(SettingsFilePath).DoesNotExist();
//			Save();
//		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Renames the project's Sessions folder to Events; rename's all its session files
		/// to have "event" extensions rather than "session" extensions; renames the Session
		/// tags in those files to "Event".
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RenameSessionsToEvents(string projectDirectory)
		{
			var sessionFolder = Directory.GetDirectories(projectDirectory,
				"Sessions", SearchOption.TopDirectoryOnly).FirstOrDefault();

			if (string.IsNullOrEmpty(sessionFolder))
				return;

			var oldFolder = Path.Combine(projectDirectory, "Sessions");
			var newFolder = Path.Combine(projectDirectory, "Events");

			try
			{
				Directory.Move(oldFolder, newFolder);
			}
			catch (Exception)
			{
				// TODO: should probably be more informative and give the user
				// a chance to "unlock" the folder and retry.
				//Palaso.Reporting.ErrorReport.ReportFatalException(e);
				throw;  //by rethrowing, we allow the higher levels to do what they are supposed to, which is to
				//say "sorry, couldn't open that." If we have more info to give here, we could do that via a non-fatal error.
			}

			var sessionFiles = Directory.GetFiles(newFolder, "*.session", SearchOption.AllDirectories);
			foreach (var file in sessionFiles)
				RenameSessionFileToEventFile(file);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Renames a single project's session file to an event file, including modifying the
		/// Session tags inside the file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RenameSessionFileToEventFile(string oldFile)
		{
			// TODO: Should probably put some error checking in here. Although,
			// I'm not sure what I would do with a failure along the way.
			var session = XElement.Load(oldFile);
			var evnt = new XElement("Event", session.Nodes());
			var newFile = Path.ChangeExtension(oldFile, "event");
			evnt.Save(newFile);
			File.Delete(oldFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the events for the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InitializeEvents()
		{
			if (!Directory.Exists(EventsFolder))
				Directory.CreateDirectory(EventsFolder);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of sorted event folders (including their full path) in the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string[] EventNames
		{
			get
			{
				return (from x in Directory.GetDirectories(EventsFolder)
						orderby x
						select x).ToArray();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the folder in which the project's event folders are stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string EventsFolder
		{
			get { return Path.Combine(ProjectFolder, EventFolderName); }
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		protected string ProjectFolder
		{
			get { return Path.GetDirectoryName(SettingsFilePath); }
		}

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			XElement project = new XElement("Project");
			project.Add(new XElement("Iso639Code", Iso639Code));
			project.Save(SettingsFilePath);
		}

		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			XElement project = XElement.Load(SettingsFilePath);
			var elements = project.Descendants("Iso639Code");
			if(elements.Count()==0)
			{
				elements = project.Descendants("IsoCode"); //old value when we were called "Sponge"
			}
			Iso639Code = elements.First().Value;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// This is, roughly, the "ethnologue code", taken either from 639-2 (2 letters),
		/// or, more often, 639-3 (3 letters)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public string Iso639Code { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Note: while the folder name will match the settings file name when it is first
		/// created, it needn't remain that way. A user can copy the project folder, rename
		/// it "blah (old)", whatever, and this will still work.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FolderPath
		{
			get { return Path.GetDirectoryName(SettingsFilePath); }
		}

		[XmlIgnore]
		public string SettingsFilePath { get; set; }

		/// ------------------------------------------------------------------------------------
		public static string ProjectSettingsFileExtension
		{
			get { return Settings.Default.ProjectFileExtension.TrimStart('.'); }
		}

		/// ------------------------------------------------------------------------------------
		public static string ComputePathToSettings(string parentFolderPath, string newProjectName)
		{
			var p = Path.Combine(parentFolderPath, newProjectName);
			return Path.Combine(p, newProjectName + "." + ProjectSettingsFileExtension);
		}
	}
}
