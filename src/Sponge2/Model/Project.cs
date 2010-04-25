using System;
using System.IO;
using System.Xml.Serialization;
using Palaso.Code;
using SilUtils;

namespace Sponge2.Model
{
	/// <summary>
	/// A project corresponds to a single folder (with subfolders) on the disk.
	/// In that folder is a file which persists the settings, then a folder of
	/// people, and another of sessions.
	/// </summary>
	[XmlType("Project")]
	public class Project
	{
		public const string ProjectSettingsFileExtension = "sprj";

		/// <summary>
		/// Existing project factory method
		/// </summary>
		public static Project FromSettingsFilePath(string settingsFilePath)
		{
			if(!File.Exists(settingsFilePath))
			{
				throw new FileNotFoundException("Could not find the file.", settingsFilePath);
			}
			Exception e;
			var project = XmlSerializationHelper.DeserializeFromFile<Project>(settingsFilePath, out e);
			if (e != null)
			{
				throw e;
			}
			if (project == null) //TODO: what does this  XmlSerializationHelper actually do for us?
						// can it be fixed to not return null with no exception,as it now does?
			{
				throw new ApplicationException("Could not load the project");
			}
			project.Initialize(settingsFilePath);
			return project;
		}


		/// <summary>
		/// New project Factory Method
		/// </summary>
		public static Project CreateAtLocation(string parentDirectoryPath, string projectName)
		{
			var project = new Project();
			var projectDirectory = Path.Combine(parentDirectoryPath, projectName);
			RequireThat.Directory(parentDirectoryPath).Exists();
			RequireThat.Directory(projectDirectory).DoesNotExist();
			Directory.CreateDirectory(projectDirectory);
			project.Initialize(Path.Combine(projectDirectory,projectName+"."+ProjectSettingsFileExtension));
			project.Save();
			return project;
		}

		public void Initialize(string settingsFilePath)
		{
			SettingsFilePath = settingsFilePath;
		}


		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			XmlSerializationHelper.SerializeToFile(SettingsFilePath, this);
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

	}
}
