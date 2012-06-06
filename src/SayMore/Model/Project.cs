using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using SayMore.Properties;
using SilTools;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A project corresponds to a single folder (with subfolders) on the disk.
	/// In that folder is a file which persists the settings, then a folder of
	/// people, and another of sessions.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Project
	{
		private const string SessionFolderName = "Sessions";

		public delegate Project Factory(string desiredOrExistingFilePath);
		//public delegate Project FactoryForNew(string parentDirectory, int x, string projectName);

		public Session.Factory SessionFactory { get; set; }
		public Func<Session, Session> SessionPropertyInjectionMethod { get; set; }

		public string Name { get; protected set; }

		public Font TranscriptionFont { get; set; }
		public Font FreeTranslationFont { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// can be used whether the project exists already, or not
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Project(string desiredOrExistingSettingsFilePath)
		{
			SettingsFilePath = desiredOrExistingSettingsFilePath;
			Name = Path.GetFileNameWithoutExtension(desiredOrExistingSettingsFilePath);
			var projectDirectory = Path.GetDirectoryName(desiredOrExistingSettingsFilePath);
			var parentDirectoryPath = Path.GetDirectoryName(projectDirectory);

			if (File.Exists(desiredOrExistingSettingsFilePath))
			{
				RenameEventsToSessions(projectDirectory);
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

			if (TranscriptionFont == null)
				TranscriptionFont = Program.DialogFont;

			if (FreeTranslationFont == null)
				FreeTranslationFont = Program.DialogFont;
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
		/// Renames the project's Events folder to Sessions; rename's all its session files
		/// to have "session" extensions rather than "event" extensions; renames the Event
		/// tags in those files to "Session".
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RenameEventsToSessions(string projectDirectory)
		{
			var eventFolder = Directory.GetDirectories(projectDirectory,
				"Events", SearchOption.TopDirectoryOnly).FirstOrDefault();

			if (string.IsNullOrEmpty(eventFolder))
				return;

			var oldFolder = Path.Combine(projectDirectory, "Events");
			var newFolder = Path.Combine(projectDirectory, "Sessions");

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

			foreach (var file in Directory.GetFiles(newFolder, "*.event", SearchOption.AllDirectories))
				RenameEventFileToSessionFile(file);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Renames a single project's session file to an event file, including modifying the
		/// Session tags inside the file.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void RenameEventFileToSessionFile(string oldFile)
		{
			// TODO: Should probably put some error checking in here. Although,
			// I'm not sure what I would do with a failure along the way.
			var evnt = XElement.Load(oldFile);
			var session = new XElement("Session", evnt.Nodes());
			var newFile = Path.ChangeExtension(oldFile, "session");
			session.Save(newFile);
			File.Delete(oldFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the sessions for the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void InitializeSessions()
		{
			if (!Directory.Exists(SessionsFolder))
				Directory.CreateDirectory(SessionsFolder);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the list of sorted session folders (including their full path) in the project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string[] SessionNames
		{
			get
			{
				return (from x in Directory.GetDirectories(SessionsFolder)
						orderby x
						select x).ToArray();
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the folder in which the project's session folders are stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string SessionsFolder
		{
			get { return Path.Combine(ProjectFolder, SessionFolderName); }
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
			var project = new XElement("Project");
			project.Add(new XElement("Iso639Code", Iso639Code));

			if (TranscriptionFont != Program.DialogFont)
				project.Add(new XElement("transcriptionFont", FontHelper.FontToString(TranscriptionFont)));

			if (FreeTranslationFont != Program.DialogFont)
				project.Add(new XElement("freeTranslationFont", FontHelper.FontToString(FreeTranslationFont)));

			project.Save(SettingsFilePath);
		}

		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			var project = XElement.Load(SettingsFilePath);
			var elements = project.Descendants("Iso639Code").ToArray();

			if (elements.Length == 0)
				elements = project.Descendants("IsoCode").ToArray(); //old value when we were called "Sponge"

			Iso639Code = elements.First().Value;

			elements = project.Descendants("transcriptionFont").ToArray();
			if (elements.Length > 0)
				TranscriptionFont = FontHelper.MakeFont(elements.First().Value);

			elements = project.Descendants("freeTranslationFont").ToArray();
			if (elements.Length > 0)
				FreeTranslationFont = FontHelper.MakeFont(elements.First().Value);
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
