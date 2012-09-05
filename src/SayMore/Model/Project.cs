using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Palaso.Reporting;
using SayMore.Properties;
using SayMore.Transcription.Model;
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
	public class Project : IAutoSegmenterSettings
	{
		private const string SessionFolderName = "Sessions";

		public delegate Project Factory(string desiredOrExistingFilePath);
		//public delegate Project FactoryForNew(string parentDirectory, int x, string projectName);

		public Session.Factory SessionFactory { get; set; }
		public Func<Session, Session> SessionPropertyInjectionMethod { get; set; }

		public string Name { get; protected set; }

		public Font TranscriptionFont { get; set; }
		public Font FreeTranslationFont { get; set; }

		public int AutoSegmenterMinimumSegmentLengthInMilliseconds { get; set; }
		public int AutoSegmenterMaximumSegmentLengthInMilliseconds { get; set; }
		public int AutoSegmenterPreferrerdPauseLengthInMilliseconds { get; set; }
		public double AutoSegmenterOptimumLengthClampingFactor { get; set; }

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

			if (AutoSegmenterMinimumSegmentLengthInMilliseconds < Settings.Default.MinimumSegmentLengthInMilliseconds ||
				AutoSegmenterMaximumSegmentLengthInMilliseconds <= 0 ||
				AutoSegmenterMinimumSegmentLengthInMilliseconds >= AutoSegmenterMaximumSegmentLengthInMilliseconds ||
				AutoSegmenterPreferrerdPauseLengthInMilliseconds <= 0 ||
				AutoSegmenterPreferrerdPauseLengthInMilliseconds > AutoSegmenterMaximumSegmentLengthInMilliseconds ||
				AutoSegmenterOptimumLengthClampingFactor <= 0)
			{
				var saveNeeded = AutoSegmenterMinimumSegmentLengthInMilliseconds != 0 || AutoSegmenterMaximumSegmentLengthInMilliseconds != 0 ||
					AutoSegmenterPreferrerdPauseLengthInMilliseconds != 0 || !AutoSegmenterOptimumLengthClampingFactor.Equals(0);

				AutoSegmenterMinimumSegmentLengthInMilliseconds = Settings.Default.DefaultAutoSegmenterMinimumSegmentLengthInMilliseconds;
				AutoSegmenterMaximumSegmentLengthInMilliseconds = Settings.Default.DefaultAutoSegmenterMaximumSegmentLengthInMilliseconds;
				AutoSegmenterPreferrerdPauseLengthInMilliseconds = Settings.Default.DefaultAutoSegmenterPreferrerdPauseLengthInMilliseconds;
				AutoSegmenterOptimumLengthClampingFactor = Settings.Default.DefaultAutoSegmenterOptimumLengthClampingFactor;
				if (saveNeeded)
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

			if (Directory.Exists(newFolder))
			{
				if (Directory.EnumerateFiles(newFolder).Any() || Directory.EnumerateDirectories(newFolder).Any())
				{
					var backupSessionsFolder = newFolder + " BAK";
					Directory.Move(newFolder, backupSessionsFolder);
					ErrorReport.NotifyUserOfProblem("In order to upgrade this project, SayMore renamed Events to Sessions. Because a Sessions" +
						"folder already existed, SayMore renamed it to Sessions BAK." + Environment.NewLine +
						"Project path: " + projectDirectory + Environment.NewLine + Environment.NewLine +
						"We recommend you request technical support to decide what to do with the contents of the Sessions BAK folder.");
				}
				else
					Directory.Delete(newFolder);
			}

			//try
			//{
			Directory.Move(oldFolder, newFolder);
			//}
			//catch (Exception)
			//{
			//    // TODO: should probably be more informative and give the user
			//    // a chance to "unlock" the folder and retry.
			//    //Palaso.Reporting.ErrorReport.ReportFatalException(e);
			//    throw;  //by rethrowing, we allow the higher levels to do what they are supposed to, which is to
			//    //say "sorry, couldn't open that." If we have more info to give here, we could do that via a non-fatal error.
			//}

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

			if (AutoSegmenterMinimumSegmentLengthInMilliseconds != Settings.Default.DefaultAutoSegmenterMinimumSegmentLengthInMilliseconds ||
				AutoSegmenterMaximumSegmentLengthInMilliseconds != Settings.Default.DefaultAutoSegmenterMaximumSegmentLengthInMilliseconds ||
				AutoSegmenterPreferrerdPauseLengthInMilliseconds != Settings.Default.DefaultAutoSegmenterPreferrerdPauseLengthInMilliseconds ||
				!AutoSegmenterOptimumLengthClampingFactor.Equals(Settings.Default.DefaultAutoSegmenterOptimumLengthClampingFactor))
			{
				var autoSegmenterSettings = new XElement("AutoSegmentersettings");
				project.Add(autoSegmenterSettings);
				autoSegmenterSettings.Add(new XAttribute("minSegmentLength", AutoSegmenterMinimumSegmentLengthInMilliseconds));
				autoSegmenterSettings.Add(new XAttribute("maxSegmentLength", AutoSegmenterMaximumSegmentLengthInMilliseconds));
				autoSegmenterSettings.Add(new XAttribute("preferrerdPauseLength", AutoSegmenterPreferrerdPauseLengthInMilliseconds));
				autoSegmenterSettings.Add(new XAttribute("optimumLengthClampingFactor", AutoSegmenterOptimumLengthClampingFactor));
			}

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

			var autoSegmenterSettings = project.Element("AutoSegmentersettings");
			if (autoSegmenterSettings != null)
			{
				AutoSegmenterMinimumSegmentLengthInMilliseconds = GetIntAttributeValue(autoSegmenterSettings,
					"minSegmentLength");
				AutoSegmenterMaximumSegmentLengthInMilliseconds = GetIntAttributeValue(autoSegmenterSettings,
					"maxSegmentLength");
				AutoSegmenterPreferrerdPauseLengthInMilliseconds = GetIntAttributeValue(autoSegmenterSettings,
					"preferrerdPauseLength");
				AutoSegmenterOptimumLengthClampingFactor = GetDoubleAttributeValue(autoSegmenterSettings,
					"optimumLengthClampingFactor");
			}
		}

		/// ------------------------------------------------------------------------------------
		private int GetIntAttributeValue(XElement project, string attribName)
		{
			var attrib = project.Attribute(attribName);
			int val;
			return (attrib != null && Int32.TryParse(attrib.Value, out val)) ? val : default(int);
		}

		/// ------------------------------------------------------------------------------------
		private double GetDoubleAttributeValue(XElement project, string attribName)
		{
			var attrib = project.Attribute(attribName);
			double val;
			return (attrib != null && Double.TryParse(attrib.Value, out val)) ? val : default(double);
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
		/// Gets the SayMore project settings file extension (without the leading period)
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

		/// ------------------------------------------------------------------------------------
		public static string[] GetAllProjectSettingsFiles(string path)
		{
			return Directory.GetFiles(path, "*." + ProjectSettingsFileExtension, SearchOption.AllDirectories);
		}
	}
}
