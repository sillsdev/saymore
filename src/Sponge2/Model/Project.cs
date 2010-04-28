using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Palaso.Code;

namespace Sponge2.Model
{
	/// <summary>
	/// A project corresponds to a single folder (with subfolders) on the disk.
	/// In that folder is a file which persists the settings, then a folder of
	/// people, and another of sessions.
	/// </summary>
	public class Project
	{
		private const string SessionFolderName = "sessions";
		public const string ProjectSettingsFileExtension = "sprj";

		public Session.Factory SessionFactory { get; set; }
		public Func<Session, Session> SessionPropertyInjectionMethod { get; set; }

		/// <summary>
		/// can be used whether the project exists already, or not
		/// </summary>
		public Project(string desiredOrExistingFilePath)
		{
			SettingsFilePath = desiredOrExistingFilePath;
			var projectDirectory = Path.GetDirectoryName(desiredOrExistingFilePath);
			var parentDirectoryPath = Path.GetDirectoryName(projectDirectory);

			if (File.Exists(desiredOrExistingFilePath))
			{
				Load();
			}
			else
			{
				RequireThat.Directory(parentDirectoryPath).Exists();

				if (!Directory.Exists(projectDirectory))
				{
					Directory.CreateDirectory(projectDirectory);
				}
				Save();
			}
			Sessions = new List<Session>();
			People = new List<Person>();
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

			var allSessions = (from folder in SessionNames
					 orderby folder
					 select Session.LoadFromFolder(folder, SessionPropertyInjectionMethod));

			Sessions = (from session in allSessions
						where session != null	// sessions we couldn't load are null
						select session).ToList();
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
			get
			{
				return Path.GetDirectoryName(SettingsFilePath);
			}
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
			Iso639Code = project.Descendants("Iso639Code").First().Value;
		}

		/// <summary>
		/// This is, roughly, the "ethnologue code", taken either from 639-2 (2 letters),
		/// or, more often, 639-3 (3 letters)
		/// </summary>
		public string Iso639Code { get; set; }


		/// <summary>
		/// Note: while the folder name will match the settings file name when it
		/// is first created, it needn't remain that way.
		/// A user can copy the project folder, rename
		/// it "blah (old)", whatever, and this will still work.
		/// </summary>
		[XmlIgnore]
		public string FolderPath
		{
			get
			{
				return Path.GetDirectoryName(SettingsFilePath);
			}
		}

		[XmlIgnore]
		public string SettingsFilePath
		{
			get; set;
		}

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<Session> Sessions { get; private set; }

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public List<Person> People { get; private set; }
	}
}
