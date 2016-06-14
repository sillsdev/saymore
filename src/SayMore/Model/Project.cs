using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using DesktopAnalytics;
using L10NSharp;
using SIL.Extensions;
using SIL.Reporting;
using SIL.Windows.Forms;
using SayMore.UI.ComponentEditors;
using SayMore.UI.Overview;
using SIL.Archiving;
using SIL.Archiving.Generic;
using SIL.Archiving.IMDI;
using SayMore.Properties;
using SayMore.Transcription.Model;
using SayMore.Model.Files;
using SayMore.UI;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A project corresponds to a single folder (with subfolders) on the disk.
	/// In that folder is a file which persists the settings, then a folder of
	/// people, and another of sessions.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Project : IAutoSegmenterSettings, IIMDIArchivable, IDisposable
	{
		private ElementRepository<Session>.Factory _sessionsRepoFactory;
		private readonly SessionFileType _sessionFileType;
		private string _accessProtocol;
		private bool _accessProtocolChanged;

		public delegate Project Factory(string desiredOrExistingFilePath);

		public string Name { get; protected set; }

		public Font TranscriptionFont { get; set; }
		private bool _needToDisposeTranscriptionFont;
		public Font FreeTranslationFont { get; set; }
		private bool _needToDisposeFreeTranslationFont;

		public int AutoSegmenterMinimumSegmentLengthInMilliseconds { get; set; }
		public int AutoSegmenterMaximumSegmentLengthInMilliseconds { get; set; }
		public int AutoSegmenterPreferrerdPauseLengthInMilliseconds { get; set; }
		public double AutoSegmenterOptimumLengthClampingFactor { get; set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// can be used whether the project exists already, or not
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Project(string desiredOrExistingSettingsFilePath,
			ElementRepository<Session>.Factory sessionsRepoFactory, SessionFileType sessionFileType)
		{
			_sessionsRepoFactory = sessionsRepoFactory;
			_sessionFileType = sessionFileType;
			SettingsFilePath = desiredOrExistingSettingsFilePath;
			Name = Path.GetFileNameWithoutExtension(desiredOrExistingSettingsFilePath);
			var projectDirectory = Path.GetDirectoryName(desiredOrExistingSettingsFilePath);
			var saveNeeded = false;

			if (File.Exists(desiredOrExistingSettingsFilePath))
			{
				RenameEventsToSessions(projectDirectory);
				Load();
			}
			else
			{
				var parentDirectoryPath = Path.GetDirectoryName(projectDirectory);
				if (parentDirectoryPath != null)
				{
					if (!Directory.Exists(parentDirectoryPath))
						Directory.CreateDirectory(parentDirectoryPath);

					if (!Directory.Exists(projectDirectory))
						Directory.CreateDirectory(projectDirectory);
				}

				Title = Name;

				saveNeeded = true;
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
				saveNeeded = AutoSegmenterMinimumSegmentLengthInMilliseconds != 0 || AutoSegmenterMaximumSegmentLengthInMilliseconds != 0 ||
					AutoSegmenterPreferrerdPauseLengthInMilliseconds != 0 || !AutoSegmenterOptimumLengthClampingFactor.Equals(0) || saveNeeded;

				AutoSegmenterMinimumSegmentLengthInMilliseconds = Settings.Default.DefaultAutoSegmenterMinimumSegmentLengthInMilliseconds;
				AutoSegmenterMaximumSegmentLengthInMilliseconds = Settings.Default.DefaultAutoSegmenterMaximumSegmentLengthInMilliseconds;
				AutoSegmenterPreferrerdPauseLengthInMilliseconds = Settings.Default.DefaultAutoSegmenterPreferrerdPauseLengthInMilliseconds;
				AutoSegmenterOptimumLengthClampingFactor = Settings.Default.DefaultAutoSegmenterOptimumLengthClampingFactor;
			}

			if (saveNeeded)
				Save();
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			_sessionsRepoFactory = null;
			if (_needToDisposeTranscriptionFont)
				TranscriptionFont.Dispose();
			TranscriptionFont = null;
			if (_needToDisposeFreeTranslationFont)
				FreeTranslationFont.Dispose();
			FreeTranslationFont = null;
		}

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
			var newFolder = Path.Combine(projectDirectory, Session.kFolderName);

			if (Directory.Exists(newFolder))
			{
				if (Directory.EnumerateFiles(newFolder).Any() || Directory.EnumerateDirectories(newFolder).Any())
				{
					var backupSessionsFolder = newFolder + " BAK";
					Directory.Move(newFolder, backupSessionsFolder);
					ErrorReport.NotifyUserOfProblem("In order to upgrade this project, SayMore renamed Events to " + Session.kFolderName +
						". Because a " + Session.kFolderName +
						"folder already existed, SayMore renamed it to " + Path.GetDirectoryName(backupSessionsFolder) + "." + Environment.NewLine +
						"Project path: " + projectDirectory + Environment.NewLine + Environment.NewLine +
						"We recommend you request technical support to decide what to do with the contents of the folder: " + backupSessionsFolder);
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
			//    //SIL.Reporting.ErrorReport.ReportFatalException(e);
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
			var newFile = Path.ChangeExtension(oldFile, Settings.Default.SessionFileExtension);
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
		/// Gets the full path to the folder in which the project's session folders are stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string SessionsFolder
		{
			get { return Path.Combine(ProjectFolder, Session.kFolderName); }
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
			project.Add(new XElement("Iso639Code", Iso639Code.NullTrim()));

			project.Add(!TranscriptionFont.Equals(Program.DialogFont)
				? new XElement("transcriptionFont", FontHelper.FontToString(TranscriptionFont))
				: new XElement("transcriptionFont", null));

			project.Add(!FreeTranslationFont.Equals(Program.DialogFont)
				? new XElement("freeTranslationFont", FontHelper.FontToString(FreeTranslationFont))
				: new XElement("freeTranslationFont", null));

			var autoSegmenterSettings = new XElement("AutoSegmentersettings");
			project.Add(autoSegmenterSettings);

			if (AutoSegmenterMinimumSegmentLengthInMilliseconds != Settings.Default.DefaultAutoSegmenterMinimumSegmentLengthInMilliseconds ||
				AutoSegmenterMaximumSegmentLengthInMilliseconds != Settings.Default.DefaultAutoSegmenterMaximumSegmentLengthInMilliseconds ||
				AutoSegmenterPreferrerdPauseLengthInMilliseconds != Settings.Default.DefaultAutoSegmenterPreferrerdPauseLengthInMilliseconds ||
				!AutoSegmenterOptimumLengthClampingFactor.Equals(Settings.Default.DefaultAutoSegmenterOptimumLengthClampingFactor))
			{
				autoSegmenterSettings.Add(new XAttribute("minSegmentLength", AutoSegmenterMinimumSegmentLengthInMilliseconds));
				autoSegmenterSettings.Add(new XAttribute("maxSegmentLength", AutoSegmenterMaximumSegmentLengthInMilliseconds));
				autoSegmenterSettings.Add(new XAttribute("preferrerdPauseLength", AutoSegmenterPreferrerdPauseLengthInMilliseconds));
				autoSegmenterSettings.Add(new XAttribute("optimumLengthClampingFactor", AutoSegmenterOptimumLengthClampingFactor));
			}
			else
			{
				autoSegmenterSettings.Add(new XAttribute("minSegmentLength", "0"));
				autoSegmenterSettings.Add(new XAttribute("maxSegmentLength", "0"));
				autoSegmenterSettings.Add(new XAttribute("preferrerdPauseLength", "0"));
				autoSegmenterSettings.Add(new XAttribute("optimumLengthClampingFactor", "0"));
			}

			// metadata for archiving
			project.Add(new XElement("Title", Title.NullTrim()));
			project.Add(new XElement("FundingProjectTitle", FundingProjectTitle.NullTrim()));
			project.Add(new XElement("ProjectDescription", ProjectDescription.NullTrim()));
			project.Add(new XElement("VernacularISO3CodeAndName", VernacularISO3CodeAndName.NullTrim()));
			project.Add(new XElement("Location", Location.NullTrim()));
			project.Add(new XElement("Region", Region.NullTrim()));
			project.Add(new XElement("Country", Country.NullTrim() ?? "Unspecified"));
			project.Add(new XElement("Continent", Continent.NullTrim() ?? "Unspecified"));
			project.Add(new XElement("ContactPerson", ContactPerson.NullTrim()));
			project.Add(new XElement("AccessProtocol", AccessProtocol.NullTrim()));
			project.Add(new XElement("DateAvailable", DateAvailable.NullTrim()));
			project.Add(new XElement("RightsHolder", RightsHolder.NullTrim()));
			project.Add(new XElement("Depositor", Depositor.NullTrim()));
			project.Add(new XElement("IMDIOutputDirectory", IMDIOutputDirectory.NullTrim()));

			int retryCount = 1;
			Exception error;
			do
			{
				try
				{
					error = null;
					project.Save(SettingsFilePath);
					break;
				}
				catch (Exception e)
				{
					error = e;
					if (retryCount-- == 0)
						break;
					Thread.Sleep(250);
				}
			} while (true);

			if (error != null)
			{
				ErrorReport.NotifyUserOfProblem(error,
					LocalizationManager.GetString("MainWindow.ProblemSavingSayMoreProject",
						"There was a problem saving the SayMore project:\r\n\r\n{0}"), SettingsFilePath);
			}

			if (_accessProtocolChanged)
			{
				foreach (var editor in Program.GetControlsOfType<SessionBasicEditor>(Program.ProjectWindow))
					editor.SetAccessProtocol();

				_accessProtocolChanged = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			// SP-791: Invalid URI: The hostname could not be parsed.
			Uri settingsUri;
			if (!Uri.TryCreate(SettingsFilePath, UriKind.Absolute, out settingsUri))
			{
				var msg = LocalizationManager.GetString("DialogBoxes.LoadProject.InvalidPath", "SayMore is not able to open the project file. \"{0}\" is not a valid path.");
				ErrorReport.ReportNonFatalMessageWithStackTrace(msg, SettingsFilePath);

				// allow the user to select a different project
				var prs = new Process();
				prs.StartInfo.FileName = Application.ExecutablePath;
				prs.StartInfo.Arguments = "-nl";
				prs.Start();

				Environment.Exit(0);
			}

			var project = XElement.Load(SettingsFilePath);

			var settingValue = GetStringSettingValue(project, "Iso639Code", null);

			if (string.IsNullOrEmpty(settingValue))
				settingValue = GetStringSettingValue(project, "IsoCode", null); //old value when we were called "Sponge"

			if (!string.IsNullOrEmpty(settingValue))
				Iso639Code = settingValue;

			settingValue = GetStringSettingValue(project, "transcriptionFont", null);
			if (!string.IsNullOrEmpty(settingValue))
			{
				TranscriptionFont = FontHelper.MakeFont(settingValue);
				_needToDisposeTranscriptionFont = true;
			}
			settingValue = GetStringSettingValue(project, "freeTranslationFont", null);
			if (!string.IsNullOrEmpty(settingValue))
			{
				FreeTranslationFont = FontHelper.MakeFont(settingValue);
				_needToDisposeFreeTranslationFont = true;
			}
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

			Title = GetStringSettingValue(project, "Title", string.Empty);
			FundingProjectTitle = GetStringSettingValue(project, "FundingProjectTitle", string.Empty);
			ProjectDescription = GetStringSettingValue(project, "ProjectDescription", string.Empty);
			VernacularISO3CodeAndName = GetStringSettingValue(project, "VernacularISO3CodeAndName", string.Empty);
			Location = GetStringSettingValue(project, "Location", string.Empty);
			Region = GetStringSettingValue(project, "Region", string.Empty);
			Country = GetStringSettingValue(project, "Country", "Unspecified");
			Continent = GetStringSettingValue(project, "Continent", "Unspecified");
			ContactPerson = GetStringSettingValue(project, "ContactPerson", string.Empty);
			AccessProtocol = GetStringSettingValue(project, "AccessProtocol", string.Empty);
			_accessProtocolChanged = false;

			DateAvailable = GetStringSettingValue(project, "DateAvailable", string.Empty);
			RightsHolder = GetStringSettingValue(project, "RightsHolder", string.Empty);
			Depositor = GetStringSettingValue(project, "Depositor", string.Empty);

			IMDIOutputDirectory = GetStringSettingValue(project, "IMDIOutputDirectory", string.Empty);
		}

		/// ------------------------------------------------------------------------------------
		private string GetStringSettingValue(XElement project, string elementName, string defaultValue)
		{
			var element = project.Element(elementName);
			return element == null ? defaultValue : element.Value;
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

		/// ------------------------------------------------------------------------------------
		public List<XmlException> FileLoadErrors
		{
			get { return _sessionsRepoFactory(Path.GetDirectoryName(SettingsFilePath), Session.kFolderName, _sessionFileType).FileLoadErrors; }
		}

		internal IEnumerable<Session> GetAllSessions()
		{
			ElementRepository<Session> sessionRepo = _sessionsRepoFactory(Path.GetDirectoryName(SettingsFilePath), Session.kFolderName, _sessionFileType);
			sessionRepo.RefreshItemList();

			return sessionRepo.AllItems;
		}

		#region Archiving
		/// ------------------------------------------------------------------------------------
		public string ArchiveInfoDetails
		{
			get
			{
				return LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ProjectArchivingInfoDetails",
					"The archive corpus will include all required files and data related to this project.",
					"This sentence is inserted as a parameter in DialogBoxes.ArchivingDlg.IMDIOverviewText");
			}
		}

		/// ------------------------------------------------------------------------------------
		public string Title { get; set; }

		/// ------------------------------------------------------------------------------------
		public string FundingProjectTitle { get; set; }

		/// ------------------------------------------------------------------------------------
		public string ProjectDescription { get; set; }

		/// ------------------------------------------------------------------------------------
		public string VernacularISO3CodeAndName { get; set; }

		/// ------------------------------------------------------------------------------------
		public string Location { get; set; }

		/// ------------------------------------------------------------------------------------
		public string Region { get; set; }

		/// ------------------------------------------------------------------------------------
		public string Country { get; set; }

		/// ------------------------------------------------------------------------------------
		public string Continent { get; set; }

		/// ------------------------------------------------------------------------------------
		public string ContactPerson { get; set; }

		/// ------------------------------------------------------------------------------------
		public string DateAvailable { get; set; }

		/// ------------------------------------------------------------------------------------
		public string RightsHolder { get; set; }

		/// ------------------------------------------------------------------------------------
		public string Depositor { get; set; }

		/// ------------------------------------------------------------------------------------
		public string IMDIOutputDirectory { get; set; }

		/// ------------------------------------------------------------------------------------
		public string AccessProtocol
		{
			get { return _accessProtocol; }
			set
			{
				if (value == _accessProtocol) return;
				_accessProtocol = value;
				_accessProtocolChanged = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string Id
		{
			get { return Name; }
		}

		/// ------------------------------------------------------------------------------------
		public void InitializeModel(IMDIArchivingDlgViewModel model)
		{
			//Set project metadata here.
			model.OverrideDisplayInitialSummary = fileLists => DisplayInitialArchiveSummary(fileLists, model);
			ArchivingHelper.SetIMDIMetadataToArchive(this, model);
		}

		/// ------------------------------------------------------------------------------------
		private void DisplayInitialArchiveSummary(IDictionary<string, Tuple<IEnumerable<string>, string>> fileLists, ArchivingDlgViewModel model)
		{
			foreach (var message in model.AdditionalMessages)
				model.DisplayMessage(message.Key + "\n", message.Value);

			if (fileLists.Count > 1)
			{
				model.DisplayMessage(LocalizationManager.GetString("DialogBoxes.ArchivingDlg.PrearchivingStatusMsg1",
					"The following session and contributor files will be added to your archive."), ArchivingDlgViewModel.MessageType.Normal);
			}
			else
			{
				model.DisplayMessage(LocalizationManager.GetString("DialogBoxes.ArchivingDlg.NoContributorsForSessionMsg",
					"There are no contributors for this session."), ArchivingDlgViewModel.MessageType.Warning);

				model.DisplayMessage(LocalizationManager.GetString("DialogBoxes.ArchivingDlg.PrearchivingStatusMsg2",
					"The following session files will be added to your archive."), ArchivingDlgViewModel.MessageType.Progress);
			}

			var fmt = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ArchivingProgressMsg", "     {0}: {1}",
				"The first parameter is 'Session' or 'Contributor'. The second parameter is the session or contributor name.");

			foreach (var kvp in fileLists)
			{
				var element = (kvp.Key.StartsWith("\n") ?
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ContributorElementName", "Contributor") :
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.SessionElementName", "Session"));

				model.DisplayMessage(string.Format(fmt, element, (kvp.Key.StartsWith("\n") ? kvp.Key.Substring(1) : kvp.Key)),
					ArchivingDlgViewModel.MessageType.Progress);

				foreach (var file in kvp.Value.Item1)
					model.DisplayMessage(Path.GetFileName(file), ArchivingDlgViewModel.MessageType.Bullet);
			}
		}

		/// ------------------------------------------------------------------------------------
		internal void ArchiveProjectUsingIMDI(Form parentForm)
		{
			Analytics.Track("Archive Project using IMDI");

			ArchivingHelper.ArchiveUsingIMDI(this);
		}

		/// ------------------------------------------------------------------------------------
		public void SetFilesToArchive(ArchivingDlgViewModel model)
		{
			Dictionary<string, HashSet<string>> contributorFiles = new Dictionary<string, HashSet<string>>();
			Type archiveType = model.GetType();
			foreach (var session in GetAllSessions())
			{
				model.AddFileGroup(session.Id, session.GetSessionFilesToArchive(archiveType), session.AddingSessionFilesProgressMsg);
				foreach (var person in session.GetParticipantFilesToArchive(model.GetType()))
				{
					if (!contributorFiles.ContainsKey(person.Key))
						contributorFiles.Add(person.Key, new HashSet<string>());

					foreach (var file in person.Value)
						contributorFiles[person.Key].Add(file);
				}

				IArchivingSession s = model.AddSession(session.Id);
				s.Location.Address = session.MetaDataFile.GetStringValue(SessionFileType.kLocationFieldName, string.Empty);
				s.Title = session.Title;
			}

			// project description documents
			var docsPath = Path.Combine(FolderPath, ProjectDescriptionDocsScreen.kFolderName);
			if (Directory.Exists(docsPath))
			{
				var files = Directory.GetFiles(docsPath, "*.*", SearchOption.TopDirectoryOnly);

				// the directory exists and contains files
				if (files.Length > 0)
				{
					model.AddFileGroup(ProjectDescriptionDocsScreen.kArchiveSessionName, files,
						LocalizationManager.GetString("DialogBoxes.ArchivingDlg.AddingProjectDescriptionDocumentsToIMDIArchiveProgressMsg",
							"Adding Project Description Documents..."));
				}
			}

			// other project documents
			docsPath = Path.Combine(FolderPath, ProjectOtherDocsScreen.kFolderName);
			if (Directory.Exists(docsPath))
			{
				var files = Directory.GetFiles(docsPath, "*.*", SearchOption.TopDirectoryOnly);

				// the directory exists and contains files
				if (files.Length > 0)
				{
					model.AddFileGroup(ProjectOtherDocsScreen.kArchiveSessionName, files,
						LocalizationManager.GetString("DialogBoxes.ArchivingDlg.AddingOtherProjectDocumentsToIMDIArchiveProgressMsg",
							"Adding Other Project Documents..."));
				}
			}

			foreach (var key in contributorFiles.Keys)
			{
				model.AddFileGroup("\n" + key, contributorFiles[key],
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.AddingContributorFilesToIMDIArchiveProgressMsg",
					"Adding Files for Contributors..."));
			}
		}
		#endregion
	}
}
