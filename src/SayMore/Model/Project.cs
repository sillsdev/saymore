using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
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
using SayMore.Utilities;
using SIL.Core.ClearShare;
using SIL.IO;
using static System.IO.Path;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A project corresponds to a single folder (with subfolders) on the disk.
	/// In that folder is a file which persists the settings, then a folder of
	/// people, and another of sessions.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Project : IAutoSegmenterSettings, IRAMPArchivable, IDisposable
	{
		// Fixing the misspelling and incorrect capitalization of these two settings will prevent
		// older versions of SayMore from reading the corrected settings. But since they can
		// only be changed manually (they have never been exposed in the UI), there is only a
		// very slim chance that this would ever affect anyone.
		private const string kAutoSegmenterSettings = "AutoSegmenterSettings";
		private const string kMisspelledAutoSegmenterSettings = "AutoSegmentersettings";
		private const string kMinSegmentLength = "minSegmentLength";
		private const string kMaxSegmentLength = "maxSegmentLength";
		private const string kMisspelledPreferredPauseLength = "preferrerdPauseLength";
		private const string kPreferredPauseLength = "preferredPauseLength";
		private const string kOptimumLengthClampingFactor = "optimumLengthClampingFactor";
		private const string kTranscriptionFont = "transcriptionFont";
		private const string kFreeTranslationFont = "freeTranslationFont";
		private const string kWorkingLanguageFont = "workingLanguageFont";

		private ElementRepository<Session>.Factory _sessionsRepoFactory;
		private readonly SessionFileType _sessionFileType;
		private string _accessProtocol;
		private bool _accessProtocolChanged;
		private Font _freeTranslationFont;
		private bool _needToDisposeFreeTranslationFont;
		private Font _workingLanguageFont;
		private bool _needToDisposeWorkingLanguageFont;

		public delegate Project Factory(string desiredOrExistingFilePath);

		public string Name { get; }

		public Font TranscriptionFont { get; set; }
		private bool _needToDisposeTranscriptionFont;

		public Font FreeTranslationFont
		{
			get => _freeTranslationFont ?? _workingLanguageFont;
			set => _freeTranslationFont = value;
		}

		public Font WorkingLanguageFont
		{
			get => _workingLanguageFont ?? _freeTranslationFont;
			set => _workingLanguageFont = value;
		}

		public int AutoSegmenterMinimumSegmentLengthInMilliseconds { get; set; }
		public int AutoSegmenterMaximumSegmentLengthInMilliseconds { get; set; }
		public int AutoSegmenterPreferredPauseLengthInMilliseconds { get; set; }
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
			Name = GetFileNameWithoutExtension(desiredOrExistingSettingsFilePath);
			var projectDirectory = GetDirectoryName(desiredOrExistingSettingsFilePath);
			if (projectDirectory == null)
				throw new ArgumentException("Invalid project path specified", nameof(desiredOrExistingSettingsFilePath));
			var saveNeeded = false;

			if (File.Exists(desiredOrExistingSettingsFilePath))
			{
				RenameEventsToSessions(projectDirectory);
				Load();
			}
			else
			{
				Directory.CreateDirectory(projectDirectory);
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
				AutoSegmenterPreferredPauseLengthInMilliseconds <= 0 ||
				AutoSegmenterPreferredPauseLengthInMilliseconds > AutoSegmenterMaximumSegmentLengthInMilliseconds ||
				AutoSegmenterOptimumLengthClampingFactor <= 0)
			{
				saveNeeded = AutoSegmenterMinimumSegmentLengthInMilliseconds != 0 || AutoSegmenterMaximumSegmentLengthInMilliseconds != 0 ||
					AutoSegmenterPreferredPauseLengthInMilliseconds != 0 || !AutoSegmenterOptimumLengthClampingFactor.Equals(0) || saveNeeded;

				AutoSegmenterMinimumSegmentLengthInMilliseconds = Settings.Default.DefaultAutoSegmenterMinimumSegmentLengthInMilliseconds;
				AutoSegmenterMaximumSegmentLengthInMilliseconds = Settings.Default.DefaultAutoSegmenterMaximumSegmentLengthInMilliseconds;
				AutoSegmenterPreferredPauseLengthInMilliseconds = Settings.Default.DefaultAutoSegmenterPreferrerdPauseLengthInMilliseconds;
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
			if (_needToDisposeWorkingLanguageFont)
				WorkingLanguageFont.Dispose();
			WorkingLanguageFont = null;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Renames the project's Events folder to Sessions; renames all its session files
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

			var oldFolder = Combine(projectDirectory, "Events");
			var newFolder = Combine(projectDirectory, Session.kFolderName);

			if (Directory.Exists(newFolder))
			{
				if (Directory.EnumerateFiles(newFolder).Any() || Directory.EnumerateDirectories(newFolder).Any())
				{
					var backupSessionsFolder = newFolder + " BAK";
					Directory.Move(newFolder, backupSessionsFolder);
					ErrorReport.NotifyUserOfProblem("In order to upgrade this project, SayMore renamed Events to " + Session.kFolderName +
						". Because a " + Session.kFolderName +
						"folder already existed, SayMore renamed it to " + GetDirectoryName(backupSessionsFolder) + "." + Environment.NewLine +
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
			var newFile = ChangeExtension(oldFile, Settings.Default.SessionFileExtension);
			session.Save(newFile);
			File.Delete(oldFile);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path to the folder in which the project's session folders are stored.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string SessionsFolder => Combine(ProjectFolder, Session.kFolderName);

		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		protected string ProjectFolder => GetDirectoryName(SettingsFilePath);

		/// ------------------------------------------------------------------------------------
		public void Save()
		{
			var project = new XElement("Project");

			project.Add(new XElement(kTranscriptionFont,
				TranscriptionFont.Equals(Program.DialogFont) ? null : 
					FontHelper.FontToString(TranscriptionFont)));

			project.Add(new XElement(kFreeTranslationFont, 
				Program.DialogFont.Equals(_freeTranslationFont) ? null :
					FontHelper.FontToString(_freeTranslationFont)));

			project.Add(new XElement(kWorkingLanguageFont, 
				_workingLanguageFont == null || _workingLanguageFont.Equals(_freeTranslationFont) ? null :
					FontHelper.FontToString(_workingLanguageFont)));

			var autoSegmenterSettings = new XElement(kAutoSegmenterSettings);
			project.Add(autoSegmenterSettings);

			if (AutoSegmenterMinimumSegmentLengthInMilliseconds != Settings.Default.DefaultAutoSegmenterMinimumSegmentLengthInMilliseconds ||
				AutoSegmenterMaximumSegmentLengthInMilliseconds != Settings.Default.DefaultAutoSegmenterMaximumSegmentLengthInMilliseconds ||
				AutoSegmenterPreferredPauseLengthInMilliseconds != Settings.Default.DefaultAutoSegmenterPreferrerdPauseLengthInMilliseconds ||
				!AutoSegmenterOptimumLengthClampingFactor.Equals(Settings.Default.DefaultAutoSegmenterOptimumLengthClampingFactor))
			{
				autoSegmenterSettings.Add(new XAttribute(kMinSegmentLength, AutoSegmenterMinimumSegmentLengthInMilliseconds));
				autoSegmenterSettings.Add(new XAttribute(kMaxSegmentLength, AutoSegmenterMaximumSegmentLengthInMilliseconds));
				autoSegmenterSettings.Add(new XAttribute(kPreferredPauseLength, AutoSegmenterPreferredPauseLengthInMilliseconds));
				autoSegmenterSettings.Add(new XAttribute(kOptimumLengthClampingFactor, AutoSegmenterOptimumLengthClampingFactor));
			}
			else
			{
				autoSegmenterSettings.Add(new XAttribute(kMinSegmentLength, "0"));
				autoSegmenterSettings.Add(new XAttribute(kMaxSegmentLength, "0"));
				autoSegmenterSettings.Add(new XAttribute(kPreferredPauseLength, "0"));
				autoSegmenterSettings.Add(new XAttribute(kOptimumLengthClampingFactor, "0"));
			}

			// metadata for archiving
			project.Add(new XElement("Title", Title.NullTrim()));
			project.Add(new XElement("FundingProjectTitle", FundingProjectTitle.NullTrim()));
			project.Add(new XElement("ProjectDescription", ProjectDescription.NullTrim()));
			project.Add(new XElement("VernacularISO3CodeAndName", VernacularISO3CodeAndName.NullTrim()));
			project.Add(new XElement("AnalysisISO3CodeAndName", AnalysisISO3CodeAndName.NullTrim()));
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

			foreach (var editor in Program.GetControlsOfType<EditorBase>(Program.ProjectWindow))
			{
				if (_accessProtocolChanged && editor is SessionBasicEditor sessionEditor)
					sessionEditor.SetAccessProtocol();
				editor.SetWorkingLanguageFont();
			}
			_accessProtocolChanged = false;
		}

		/// ------------------------------------------------------------------------------------
		public string GetFileDescription(string key, string file)
		{
			var description = (key == string.Empty ? "SayMore Session File" : "SayMore Contributor File");

			if (file.ToLower().EndsWith(Settings.Default.SessionFileExtension))
				description = "SayMore Session Metadata (XML)";
			else if (file.ToLower().EndsWith(Settings.Default.PersonFileExtension))
				description = "SayMore Contributor Metadata (XML)";
			else if (file.ToLower().EndsWith(Settings.Default.MetadataFileExtension))
				description = "SayMore File Metadata (XML)";

			return description;
		}

		/// ------------------------------------------------------------------------------------
		public void SetAdditionalMetsData(RampArchivingDlgViewModel model)
		{
			foreach (var session in GetAllSessions(CancellationToken.None))
			{
				model.SetScholarlyWorkType(ScholarlyWorkType.PrimaryData);
				model.SetDomains(SilDomain.Ling_LanguageDocumentation);

				var value = session.MetaDataFile.GetStringValue(SessionFileType.kDateFieldName, null);
				if (!string.IsNullOrEmpty(value))
					model.SetCreationDate(value);

				// Return the session's note as the abstract portion of the package's description.
				value = session.MetaDataFile.GetStringValue(SessionFileType.kSynopsisFieldName, null);
				if (!string.IsNullOrEmpty(value))
					model.SetAbstract(value, string.Empty);

				// Set contributors
				var contribsVal = session.MetaDataFile.GetValue(SessionFileType.kContributionsFieldName, null);
				if (contribsVal is ContributionCollection contributions && contributions.Count > 0)
					model.SetContributors(contributions);

				// Return total duration of source audio/video recordings.
				TimeSpan totalDuration = session.GetTotalDurationOfSourceMedia();
				if (totalDuration.Ticks > 0)
					model.SetAudioVideoExtent($"Total Length of Source Recordings: {totalDuration}");

				//First session details are enough for "Archive RAMP (SIL)..." from Project menu
				break;
			}
		}


		/// ------------------------------------------------------------------------------------
		public void Load()
		{
			// SP-791: Invalid URI: The hostname could not be parsed.
			if (!Uri.TryCreate(SettingsFilePath, UriKind.Absolute, out _))
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

			var settingValue = GetStringSettingValue(project, kTranscriptionFont, null);
			if (!string.IsNullOrEmpty(settingValue))
			{
				TranscriptionFont = RobustFontHelper.MakeFont(settingValue, e =>
					HandleFontCreationError(LocalizationManager.GetString("Project.TranscriptionFontDescription", "Transcription",
						"Used as a parameter in Project.FontCreationError"), settingValue, e));
				_needToDisposeTranscriptionFont = TranscriptionFont != null;
			}
			settingValue = GetStringSettingValue(project, kFreeTranslationFont, null);
			if (!string.IsNullOrEmpty(settingValue))
			{
				FreeTranslationFont = RobustFontHelper.MakeFont(settingValue, e =>
					HandleFontCreationError(LocalizationManager.GetString("Project.FreeTranslationFontDescription", "Free Translation",
						"Used as a parameter in Project.FontCreationError"), settingValue, e));
				_needToDisposeFreeTranslationFont = FreeTranslationFont != null;
			}
			var workingLanguageFontSettingValue = GetStringSettingValue(project, kWorkingLanguageFont, null);
			if (!string.IsNullOrEmpty(workingLanguageFontSettingValue) && workingLanguageFontSettingValue != settingValue)
			{
				settingValue = workingLanguageFontSettingValue;
				WorkingLanguageFont = RobustFontHelper.MakeFont(settingValue, e =>
					HandleFontCreationError(LocalizationManager.GetString("Project.WorkingLanguageFontDescription", "Working Language",
						"Used as a parameter in Project.HandleFontCreationError"), settingValue, e));
				_needToDisposeWorkingLanguageFont = WorkingLanguageFont != null;
			}
			var autoSegmenterSettings = project.Element(kAutoSegmenterSettings) ?? project.Element(kMisspelledAutoSegmenterSettings);
			if (autoSegmenterSettings != null)
			{
				AutoSegmenterMinimumSegmentLengthInMilliseconds = GetIntAttributeValue(autoSegmenterSettings,
					kMinSegmentLength);
				AutoSegmenterMaximumSegmentLengthInMilliseconds = GetIntAttributeValue(autoSegmenterSettings,
					kMaxSegmentLength);
				AutoSegmenterPreferredPauseLengthInMilliseconds = GetIntAttributeValue(autoSegmenterSettings,
					kPreferredPauseLength, kMisspelledPreferredPauseLength);
				AutoSegmenterOptimumLengthClampingFactor = GetDoubleAttributeValue(autoSegmenterSettings,
					kOptimumLengthClampingFactor);
			}

			Title = GetStringSettingValue(project, "Title", string.Empty);
			FundingProjectTitle = GetStringSettingValue(project, "FundingProjectTitle", string.Empty);
			ProjectDescription = GetStringSettingValue(project, "ProjectDescription", string.Empty);
			VernacularISO3CodeAndName = GetStringSettingValue(project, "VernacularISO3CodeAndName", string.Empty);
			AnalysisISO3CodeAndName = GetStringSettingValue(project, "AnalysisISO3CodeAndName", ArchivingHelper.FallbackAnalysisLanguage);
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

		private void HandleFontCreationError(string description, string settingValue, Exception error)
		{
			string msg = GetFontErrorMessage(description, settingValue);
			if (error == null)
			{
				ErrorReport.ReportNonFatalMessageWithStackTrace(msg + Environment.NewLine +
					LocalizationManager.GetString("Project.FontCreationFallback",
						"A font with the specified font family was successfully created using a " +
						"fallback size. Please examine the font settings and choose a suitable " +
						"font. If after doing so, you continue to see this warning, please send " +
						"in this report."));
			}
			else
			{
				ErrorReport.ReportNonFatalExceptionWithMessage(error, msg);
			}
		}

		private static string GetFontErrorMessage(string description, string settingValue)
		{
			return string.Format(LocalizationManager.GetString("Project.FontCreationError",
					"An error occurred trying to create the {0} font from settings: {1}",
					"Param 0: name of setting or description of font being created;" +
					"Param 1: settings string used to attempt to create the font (This is essentially human-readable, " +
					"but it will not be localized, since it is a string the computer interprets.)"),
				description, settingValue);
		}

		/// ------------------------------------------------------------------------------------
		private string GetStringSettingValue(XElement project, string elementName, string defaultValue)
		{
			var element = project.Element(elementName);
			return element == null ? defaultValue : element.Value;
		}

		/// ------------------------------------------------------------------------------------
		private int GetIntAttributeValue(XElement project, string attribName, string fallbackAttribName = null)
		{
			var attrib = project.Attribute(attribName);
			if (attrib == null && fallbackAttribName != null)
				attrib = project.Attribute(fallbackAttribName);
			return (attrib != null && Int32.TryParse(attrib.Value, out var val)) ? val : default;
		}

		/// ------------------------------------------------------------------------------------
		private double GetDoubleAttributeValue(XElement project, string attribName)
		{
			var attrib = project.Attribute(attribName);
			return (attrib != null && Double.TryParse(attrib.Value, out var val)) ? val : default;
		}

		/// ------------------------------------------------------------------------------------
		public void CustomFilenameNormalization(string key, string file, StringBuilder bldr)
		{
			if (key != string.Empty)
				bldr.Insert(0, "__Contributors__");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Note: while the folder name will match the settings file name when it is first
		/// created, it needn't remain that way. A user can copy the project folder, rename
		/// it "blah (old)", whatever, and this will still work.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[XmlIgnore]
		public string FolderPath => GetDirectoryName(SettingsFilePath);

		[XmlIgnore]
		public string SettingsFilePath { get; set; }

		/// ------------------------------------------------------------------------------------
		/// Gets the SayMore project settings file extension (without the leading period)
		/// ------------------------------------------------------------------------------------
		public static string ProjectSettingsFileExtension => 
			Settings.Default.ProjectFileExtension.TrimStart('.');

		/// ------------------------------------------------------------------------------------
		public static string ComputePathToSettings(string parentFolderPath, string newProjectName)
		{
			var p = Combine(parentFolderPath, newProjectName);
			return Combine(p, newProjectName + "." + ProjectSettingsFileExtension);
		}

		/// ------------------------------------------------------------------------------------
		public static string[] GetAllProjectSettingsFiles(string path)
		{
			return Directory.GetFiles(path, "*." + ProjectSettingsFileExtension, SearchOption.AllDirectories);
		}

		/// ------------------------------------------------------------------------------------
		public List<XmlException> FileLoadErrors => _sessionsRepoFactory(
			GetDirectoryName(SettingsFilePath), Session.kFolderName, _sessionFileType).FileLoadErrors;

		internal IEnumerable<Session> GetAllSessions(CancellationToken cancellationToken)
		{
			var sessionRepo = _sessionsRepoFactory(GetDirectoryName(SettingsFilePath),
				Session.kFolderName, _sessionFileType);
			sessionRepo.RefreshItemList(cancellationToken);

			return sessionRepo.AllItems;
		}

		#region Archiving
		/// ------------------------------------------------------------------------------------
		public string ArchiveInfoDetails =>
			LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ProjectArchivingInfoDetails",
				"The archive corpus will include all required files and data related to this project.",
				"This sentence is inserted as a parameter in DialogBoxes.ArchivingDlg.IMDIOverviewText");

		/// ------------------------------------------------------------------------------------
		public string Title { get; set; }

		/// ------------------------------------------------------------------------------------
		public string FundingProjectTitle { get; set; }

		/// ------------------------------------------------------------------------------------
		public string ProjectDescription { get; set; }

		/// ------------------------------------------------------------------------------------
		public string VernacularISO3CodeAndName { get; set; }

		/// ------------------------------------------------------------------------------------
		public string AnalysisISO3CodeAndName { get; set; }

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
			get => _accessProtocol;
			set
			{
				if (value == _accessProtocol) return;
				_accessProtocol = value;
				_accessProtocolChanged = true;
			}
		}

		/// ------------------------------------------------------------------------------------
		public string Id => Name;

		/// ------------------------------------------------------------------------------------
		public void InitializeModel(ArchivingDlgViewModel model)
		{
			//Set project metadata here.
			model.OverrideDisplayInitialSummary = (fileLists, cancellationToken) =>
				DisplayInitialArchiveSummary(fileLists, model, cancellationToken);
		}

		/// ------------------------------------------------------------------------------------
		public void DisplayInitialArchiveSummary(
			IDictionary<string, Tuple<IEnumerable<string>, string>> fileLists,
			ArchivingDlgViewModel model, CancellationToken cancellationToken)
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
				if (cancellationToken.IsCancellationRequested)
					throw new OperationCanceledException();
				var element = (kvp.Key.StartsWith("\n") || kvp.Key.Length > 0 ?
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ContributorElementName", "Contributor") :
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.SessionElementName", "Sessions"));

				model.DisplayMessage(string.Format(fmt, element, (kvp.Key.StartsWith("\n") || kvp.Key.Length > 0 ? kvp.Key.Substring(1) : Title)),
					ArchivingDlgViewModel.MessageType.Progress);

				foreach (var file in kvp.Value.Item1)
					model.DisplayMessage(GetFileName(file), ArchivingDlgViewModel.MessageType.Bullet);
			}
		}

		/// ------------------------------------------------------------------------------------
		internal void ArchiveProjectUsingIMDI(Form parentForm)
		{
			Analytics.Track("Archive Project using IMDI");

			ArchivingHelper.ArchiveUsingIMDI(this, parentForm);
		}

		/// ------------------------------------------------------------------------------------
		internal void ArchiveProjectUsingRAMP(Form parentForm)
		{
			Analytics.Track("Archive Project using RAMP");
			ArchivingHelper.ArchiveUsingRAMP(this, parentForm);
		}

		/// ------------------------------------------------------------------------------------
		public void SetFilesToArchive(ArchivingDlgViewModel model,
			CancellationToken cancellationToken)
		{
			if (model is RampArchivingDlgViewModel)
			{
				Dictionary<string, HashSet<string>> contributorFiles = new Dictionary<string, HashSet<string>>();
				model.AddFileGroup(string.Empty, GetSessionFilesToArchive(model.GetType(), cancellationToken), AddingSessionFilesProgressMsg);
				var fmt = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.AddingContributorFilesProgressMsg", "Adding Files for Contributor '{0}'");
				var participants = new HashSet<string>();
				foreach (var session in GetAllSessions(cancellationToken))
				{
					foreach (var person in session.GetParticipantFilesToArchive(model.GetType(), cancellationToken))
					{
						if (cancellationToken.IsCancellationRequested)
							throw new OperationCanceledException();
						if (!participants.Add(person.Key))
							continue;
						model.AddFileGroup(person.Key, person.Value, string.Format(fmt, person.Key));

						if (!contributorFiles.ContainsKey(person.Key))
							contributorFiles.Add(person.Key, new HashSet<string>());

						foreach (var file in person.Value)
						{
							if (cancellationToken.IsCancellationRequested)
								throw new OperationCanceledException();

							contributorFiles[person.Key].Add(file);
						}
					}
				}
			}
			else
			{
				Dictionary<string, HashSet<string>> contributorFiles = new Dictionary<string, HashSet<string>>();
				Type archiveType = model.GetType();
				foreach (var session in GetAllSessions(cancellationToken))
				{
					model.AddFileGroup(session.Id, session.GetSessionFilesToArchive(archiveType, cancellationToken),
						session.AddingSessionFilesProgressMsg);
					foreach (var person in session.GetParticipantFilesToArchive(model.GetType(), cancellationToken))
					{
						if (!contributorFiles.ContainsKey(person.Key))
							contributorFiles.Add(person.Key, new HashSet<string>());

						foreach (var file in person.Value)
							contributorFiles[person.Key].Add(file);
					}

					IArchivingSession s = ((IMDIArchivingDlgViewModel)model).AddSession(session.Id);
					s.Location.Address = session.MetaDataFile.GetStringValue(SessionFileType.kLocationFieldName, string.Empty);
					s.Title = session.Title;
				}

				// project description documents
				var docsPath = Combine(FolderPath, ProjectDescriptionDocsScreen.kFolderName);
				if (Directory.Exists(docsPath))
				{
					var files = Directory.GetFiles(docsPath, "*.*", SearchOption.TopDirectoryOnly);

					// the directory exists and contains files
					if (files.Length > 0)
					{
						model.AddFileGroup(ProjectDescriptionDocsScreen.kArchiveSessionName, files,
							LocalizationManager.GetString(
								"DialogBoxes.ArchivingDlg.AddingProjectDescriptionDocumentsToIMDIArchiveProgressMsg",
								"Adding Project Description Documents..."));
					}
				}

				// other project documents
				docsPath = Combine(FolderPath, ProjectOtherDocsScreen.kFolderName);
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
		}

		/// ------------------------------------------------------------------------------------
		public string AddingSessionFilesProgressMsg => string.Format(LocalizationManager.GetString(
				"DialogBoxes.ArchivingDlg.AddingSessionFilesProgressMsg",
				"Adding Files for Session '{0}'"), Title);

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetSessionFilesToArchive(Type typeOfArchive,
			CancellationToken cancellationToken)
		{
			var tempZipFilePath = ChangeExtension(GetTempFileName(), "zip");
			// RAMP packages must not be compressed or RAMP can't read them.
			ZipFile.CreateFromDirectory(FolderPath, tempZipFilePath, CompressionLevel.NoCompression, true);
			var zipFilePath = Combine(FolderPath, "Sessions.zip");
			RobustFile.Move(tempZipFilePath, zipFilePath, true);
			return Directory.GetFiles(FolderPath).Where(f => 
				ArchivingHelper.IncludeFileInArchive(f, typeOfArchive,
					Settings.Default.SessionFileExtension, CancellationToken.None));
		}
		#endregion
	}
}
