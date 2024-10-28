using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DesktopAnalytics;
using L10NSharp;
using SIL.Reporting;
using SIL.Archiving.Generic;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Properties;
using SIL.Archiving;
using SIL.Extensions;
using SIL.Core.ClearShare;
using System.Text.RegularExpressions;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A session is recorded, documented, transcribed, etc.
	/// Each session is represented on disk as a single folder, with 1 or more files
	/// related to that session. The one file it will always have is some metadata about
	/// the session.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Session : ProjectElement, IRAMPArchivable
	{
		public static string kFolderName = "Sessions";

		public enum Status
		{
			Incoming = 0,
			In_Progress,
			Finished,
			Skipped
		}

		//autofac uses this
		public delegate Session Factory(string parentElementFolder, string id);

		private readonly PersonInformant _personInformant;

		[Obsolete("For Mocking Only")]
		public Session() { }

		/// ------------------------------------------------------------------------------------
		public Session(string parentElementFolder, string id,
			Action<ProjectElement, string, string> idChangedNotificationReceiver,
			SessionFileType sessionFileType,
			Func<ProjectElement, string, ComponentFile> componentFileFactory,
			XmlFileSerializer xmlFileSerializer,
			ProjectElementComponentFile.Factory prjElementComponentFileFactory,
			IEnumerable<ComponentRole> componentRoles,
			PersonInformant personInformant, Project project)
			: base(parentElementFolder, id, idChangedNotificationReceiver, sessionFileType,
				componentFileFactory, xmlFileSerializer, prjElementComponentFileFactory, componentRoles)
		{
			_personInformant = personInformant;

			// ReSharper disable DoNotCallOverridableMethodsInConstructor

			// Using a 1-minute fudge factor is a bit of a kludge, but when a session is created from an
			// existing media file, it already has an ID, and there's no other way to tell it's "new".
			if (project != null &&
				(id == null || MetaDataFile.GetCreateDate().AddMinutes(1) > DateTime.Now) &&
				MetaDataFile.GetStringValue(SessionFileType.kCountryFieldName, null) == null &&
				MetaDataFile.GetStringValue(SessionFileType.kRegionFieldName, null) == null &&
				MetaDataFile.GetStringValue(SessionFileType.kContinentFieldName, null) == null &&
				MetaDataFile.GetStringValue(SessionFileType.kAddressFieldName, null) == null)
			{
				// SP-876: Project Data not displayed in new sessions until after a restart.
				Program.SaveProjectMetadata();

				if (!string.IsNullOrEmpty(project.Country))
					MetaDataFile.TrySetStringValue(SessionFileType.kCountryFieldName, project.Country);
				if (!string.IsNullOrEmpty(project.Region))
					MetaDataFile.TrySetStringValue(SessionFileType.kRegionFieldName, project.Region);
				if (!string.IsNullOrEmpty(project.Continent))
					MetaDataFile.TrySetStringValue(SessionFileType.kContinentFieldName, project.Continent);
				if (!string.IsNullOrEmpty(project.Location))
					MetaDataFile.TrySetStringValue(SessionFileType.kAddressFieldName, project.Location);
			}

			if (string.IsNullOrEmpty(MetaDataFile.GetStringValue(SessionFileType.kGenreFieldName, null)))
			{
				if (MetaDataFile.TrySetStringValue(SessionFileType.kGenreFieldName, GenreDefinition.UnknownType.Name))
					MetaDataFile.Save();
			}
// ReSharper restore DoNotCallOverridableMethodsInConstructor
			if (_personInformant != null)
			{
				_personInformant.PersonNameChanged += HandlePersonsNameChanged;
				_personInformant.PersonUiIdChanged += HandlePersonsUiIdChanged;
			}
		}

		/// ------------------------------------------------------------------------------------
		public override void Dispose()
		{
			base.Dispose();
			if (_personInformant != null)
			{
				_personInformant.PersonNameChanged -= HandlePersonsNameChanged;
				_personInformant.PersonUiIdChanged -= HandlePersonsUiIdChanged;
			}
		}

		#region Properties
		/// ------------------------------------------------------------------------------------
		public string Title =>
			MetaDataFile.GetStringValue(SessionFileType.kTitleFieldName, null) ?? Id;

		/// ------------------------------------------------------------------------------------
		public string DateField => MetaDataFile.GetStringValue(SessionFileType.kDateFieldName, null);

		public DateTime SessionDate => ParseDate(DateField);

		/// ------------------------------------------------------------------------------------
		protected override string ExtensionWithoutPeriod => ExtensionWithoutPeriodStatic;

		/// ------------------------------------------------------------------------------------
		public override string RootElementName => "Session";

		/// ------------------------------------------------------------------------------------
		protected static string ExtensionWithoutPeriodStatic => 
			Settings.Default.SessionFileExtension.TrimStart('.');

		/// ------------------------------------------------------------------------------------
		public override string DefaultElementNamePrefix => LocalizationManager.GetString(
			"SessionsView.Miscellaneous.NewSessionNamePrefix", "New Session");

		/// ------------------------------------------------------------------------------------
		protected override string NoIdSaveFailureMessage => LocalizationManager.GetString(
			"SessionsView.Miscellaneous.NoIdSaveFailureMessage", "You must specify a session id.");

		/// ------------------------------------------------------------------------------------
		protected override string AlreadyExistsSaveFailureMessage =>
			LocalizationManager.GetString(
				"SessionsView.Miscellaneous.SessionAlreadyExistsSaveFailureMessage",
				"Could not rename from {0} to {1} because there is already a session by that name.");

		/// ------------------------------------------------------------------------------------
		public override string DefaultStatusValue => Status.Incoming.ToString();

		#endregion

		/// ------------------------------------------------------------------------------------
		public static string GetStatusAsEnumParsableString(string statusAsText)
		{
			return statusAsText.Replace(' ', '_');
		}

		/// ------------------------------------------------------------------------------------
		public static string GetLocalizedStatus(string statusAsText)
		{
			statusAsText = GetStatusAsEnumParsableString(statusAsText);

			if (Enum.TryParse(statusAsText, out Status status))
				return GetLocalizedStatus(status);
			throw new ArgumentException($"Value {statusAsText} is not valid status.",
				nameof(statusAsText));
		}

		/// ------------------------------------------------------------------------------------
		public static string GetLocalizedStatus(Status status)
		{
			switch (status)
			{
				case Status.Incoming:
					return LocalizationManager.GetString("SessionsView.SessionStatus.Incoming", "Incoming");
				case Status.In_Progress:
					return LocalizationManager.GetString("SessionsView.SessionStatus.InProgress", "In Progress");
				case Status.Finished:
					return LocalizationManager.GetString("SessionsView.SessionStatus.Finished", "Finished");
				case Status.Skipped:
					return LocalizationManager.GetString("SessionsView.SessionStatus.Skipped", "Skipped");
				default:
					throw new ArgumentException($"Value {status} is not valid status.", nameof(status));
			}
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ComponentRole> GetCompletedStages()
		{
			return GetCompletedStages(true);
		}

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ComponentRole> GetCompletedStages(
			bool modifyComputedListWithUserOverrides)
		{
			var list = base.GetCompletedStages(modifyComputedListWithUserOverrides).ToList();

			if (GetShouldReportHaveConsent())
			   list.Insert(0, ComponentRoles.First(r => r.Id == ComponentRole.kConsentComponentRoleId));

			return modifyComputedListWithUserOverrides ?
				GetCompletedStagesModifiedByUserOverrides(list) : list;
		}

		/// ------------------------------------------------------------------------------------
		private bool GetShouldReportHaveConsent()
		{
			// MetaDataFile can be null if this Session is already disposed (when called via OnPaint). 
			var contributions = MetaDataFile?.GetValue(SessionFileType.kContributionsFieldName, null) as ContributionCollection;
			var personNames = contributions?.Select(c => c.ContributorName).ToArray();
			if (personNames == null)
				return false;
			bool allContributorsHaveConsent = personNames.Length > 0;

			return personNames.All(name => _personInformant.GetHasInformedConsent(name)) &&
			       allContributorsHaveConsent;
		}

		/// ------------------------------------------------------------------------------------
		public TimeSpan GetTotalDurationOfSourceMedia()
		{
			var totalDuration = TimeSpan.Zero;

			foreach (var duration in GetComponentFiles().Where(file =>
				file.GetAssignedRoles().Any(r =>
				r.Id == ComponentRole.kSourceComponentRoleId))
				.Select(f => f.DurationSeconds))
			{
				totalDuration += duration;
			}

			return totalDuration;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// We get this message from the person informant when a person's name has changed.
		/// When that happens, we need to make sure we update the participant field in case
		/// it contains a name that changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePersonsNameChanged(object sender, ElementIdChangedArgs e)
		{
			var allParticipants = GetAllParticipants();
			var newNames = allParticipants.Select(name => (name == e.OldId ? e.NewId : name));

			MetaDataFile.SetStringValue(SessionFileType.kParticipantsFieldName,
				FieldInstance.GetTextFromMultipleValues(newNames));

			MetaDataFile.Save();

			ProcessContributorNameChange(e);
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// We get this message from the person informant when a person's UI ID has changed.
		/// When that happens, we need to update any matching contributors in any metadata files
		/// for any of this session's media files.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePersonsUiIdChanged(object sender, ElementIdChangedArgs e)
		{
			ProcessContributorNameChange(e);
		}

		/// ------------------------------------------------------------------------------------
		private void ProcessContributorNameChange(ElementIdChangedArgs e)
		{
			foreach (var file in GetComponentFiles().Where(f => f.FileType is FileTypeWithContributors))
			{
				var values = file.MetaDataFieldValues.FirstOrDefault(v => v.FieldId == SessionFileType.kContributionsFieldName);
				if (values == null)
					continue;

				if (!(values.Value is ContributionCollection contributions))
					continue;

				foreach (var contribution in contributions.Where(contribution => contribution.ContributorName == e.OldId))
					contribution.ContributorName = e.NewId;

				file.SetValue(SessionFileType.kContributionsFieldName, contributions, out var failureMessage);

				if (failureMessage == null)
					file.Save();
				else
					ErrorReport.NotifyUserOfProblem(failureMessage);
			}
		}

		/// ------------------------------------------------------------------------------------
		private ContributionCollection SessionLevelContributionCollection => MetaDataFile.
			GetValue(SessionFileType.kContributionsFieldName, null) as ContributionCollection;

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<string> GetAllParticipants()
		{
			var contributions = SessionLevelContributionCollection;
			// don't return null; that isn't enumerable and callers will fail.
			return contributions == null ? Array.Empty<string>() : contributions.Select(c => c.ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Person> GetAllPersonsInSession()
		{
			return GetAllParticipants().Select(n => _personInformant.GetPersonByNameOrCode(n)).Where(p => p != null).ToList();
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<SessionContribution> GetAllContributionsFor(string personId,
			string personCode = null)
		{
			// We could make this method able to just return all contributions if personId is null,
			// but we have no known need for such functionality.
			if (personId == null)
				throw new ArgumentNullException(nameof(personId));

			var sessionLevelContributions = SessionLevelContributionCollection;
			if (sessionLevelContributions != null)
			{
				foreach (var contrib in sessionLevelContributions.Where(c =>
					c.ContributorName == personId ||
					(personCode != null && c.ContributorName == personCode)))
				{
					yield return new SessionContribution(this, null, contrib);
				}
			}

			// get the metadata files for this session
			var files = Directory.GetFiles(FolderPath, "*" + Settings.Default.MetadataFileExtension);

			var personMatch = Regex.Escape(personId);
			if (personCode != null)
				personMatch = $"|{Regex.Escape(personCode)}";
			var regexPattern = $"<name>(?<name>{personMatch})<\\/name>(?<details>(.|\\n)*?)<\\/contributor>";

			foreach (var file in files)
			{
				// Get contributions for this person
				foreach (var contrib in GetFileSpecificContributions(file, regexPattern))
					yield return contrib;
			}
		}

		private IEnumerable<SessionContribution> GetFileSpecificContributions(string fileName, string regexPattern)
		{
			var fileContents = File.ReadAllText(fileName);

			var matches = Regex.Matches(fileContents, regexPattern, RegexOptions.IgnoreCase);

			foreach (Match match in matches)
			{
				var testString = match.Groups["details"].Value;

				var role = GetRoleFromOlacList(GetValueFromXmlString(testString, "role"));
				var date = ParseDate(GetValueFromXmlString(testString, "date"));
				var note = GetValueFromXmlString(testString, "notes");

				yield return new SessionContribution(this, fileName,
					new Contribution(match.Groups["name"].Value, role)
						{ Comments = note , Date = date});
			}
		}
		
		internal static readonly OlacSystem OlacSystem = new OlacSystem();

		internal static Role GetRoleFromOlacList(string savedRole)
		{
			if (OlacSystem.TryGetRoleByCode(savedRole, out var role))
				return role;

			if (OlacSystem.TryGetRoleByName(savedRole, out role))
				return role;

			return new Role(savedRole, savedRole, null);
		}

		private static string GetValueFromXmlString(string xmlString, string valueName)
		{
			var pattern = string.Format("<{0}>(.*)</{0}>", valueName);
			var match = Regex.Match(xmlString, pattern);

			return match.Success ? match.Groups[1].Value : string.Empty;
		}

		private DateTime ParseDate(string dateString)
		{
			if (string.IsNullOrEmpty(dateString))
				return DateTime.MinValue;

			// older SayMore date problem due to saving localized date string rather than ISO8601
			return DateTimeExtensions.IsISO8601Date(dateString)
				? DateTime.Parse(dateString)
				: DateTimeExtensions.ParseDateTimePermissivelyWithException(dateString);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ArchivingActor> GetAllContributorsInSession()
		{
			// files that might have contributors
			foreach (var file in GetComponentFiles().Where(f => f.FileType is FileTypeWithContributors))
			{
				var values = file.MetaDataFieldValues
					.FirstOrDefault(v => v.FieldId == SessionFileType.kContributionsFieldName);
				if (values == null)
					continue;

				if (!(values.Value is ContributionCollection contributions))
					continue;

				foreach (var contribution in contributions)
				{
					yield return new ArchivingActor
					{
						FullName = contribution.ContributorName,
						Name = contribution.ContributorName,
						Role = contribution.Role.Name
					};
				}
			}
		}

		#region Archiving
		/// ------------------------------------------------------------------------------------
		public void ArchiveUsingRAMP(Form parentForm)
		{
			Analytics.Track("Archive Session using RAMP");
			ArchivingHelper.ArchiveUsingRAMP(this, parentForm);
		}

		/// ------------------------------------------------------------------------------------
		public void ArchiveUsingIMDI(Form parentForm)
		{
			Analytics.Track("Archive Session using IMDI");
			ArchivingHelper.ArchiveUsingIMDI(this, parentForm);
		}

		/// ------------------------------------------------------------------------------------
		public string ArchiveInfoDetails =>
			LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ArchivingInfoDetails",
				"The archive package will include all required files and data related to a session and its contributors.",
				"This sentence is inserted as a parameter in DialogBoxes.ArchivingDlg.xxxxOverviewText");

		/// ------------------------------------------------------------------------------------
		public IEnumerable<string> GetSessionFilesToArchive(Type typeOfArchive, CancellationToken cancellationToken)
		{
			var filesInDir = Directory.GetFiles(FolderPath);
			return filesInDir.Where(f => ArchivingHelper.IncludeFileInArchive(f, typeOfArchive,
				Settings.Default.SessionFileExtension, cancellationToken));
		}

		/// ------------------------------------------------------------------------------------
		public string AddingSessionFilesProgressMsg =>
			string.Format(LocalizationManager.GetString("DialogBoxes.ArchivingDlg.AddingSessionFilesProgressMsg",
				"Adding Files for Session '{0}'"), Title);

		/// ------------------------------------------------------------------------------------
		public IDictionary<string, IEnumerable<string>> GetParticipantFilesToArchive(
			Type typeOfArchive, CancellationToken cancellationToken)
		{
			Dictionary<string, IEnumerable<string>> d = new Dictionary<string, IEnumerable<string>>();

			foreach (var person in GetAllParticipants().Select(n => _personInformant.GetPersonByNameOrCode(n)).Where(p => p != null))
			{
				var filesInDir = Directory.GetFiles(person.FolderPath);
				d[person.Id] = filesInDir.Where(f => ArchivingHelper.IncludeFileInArchive(f,
					typeOfArchive, Settings.Default.PersonFileExtension, cancellationToken));
			}
			return d;
		}

		public void InitializeModel(ArchivingDlgViewModel model)
		{
			model.GetOverriddenPreArchivingMessages = GetOverriddenPreArchivingMessages;
			model.InitialFileGroupDisplayMessageType = ArchivingDlgViewModel.MessageType.Progress;
			model.OverrideGetFileGroupDisplayMessage = GetFileGroupDisplayMessage;
		}

		/// ------------------------------------------------------------------------------------
		public void SetFilesToArchive(ArchivingDlgViewModel model,
			CancellationToken cancellationToken)
		{
			model.AddFileGroup(string.Empty,
				GetSessionFilesToArchive(model.GetType(), cancellationToken),
				AddingSessionFilesProgressMsg);

			var fmt = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.AddingContributorFilesProgressMsg", "Adding Files for Contributor '{0}'");

			foreach (var person in GetParticipantFilesToArchive(model.GetType(), cancellationToken))
				model.AddFileGroup(person.Key, person.Value, string.Format(fmt, person.Key));
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Tuple<string, ArchivingDlgViewModel.MessageType>> GetOverriddenPreArchivingMessages(
			IDictionary<string, Tuple<IEnumerable<string>, string>> fileLists)
		{
			if (fileLists.Count > 1)
			{
				yield return new Tuple<string, ArchivingDlgViewModel.MessageType>(
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.PrearchivingStatusMsg1",
					"The following session and contributor files will be added to your archive."),
					ArchivingDlgViewModel.MessageType.Normal);
			}
			else
			{
				yield return new Tuple<string, ArchivingDlgViewModel.MessageType>(
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.NoContributorsForSessionMsg",
					"There are no contributors for this session."), ArchivingDlgViewModel.MessageType.Warning);

				yield return new Tuple<string, ArchivingDlgViewModel.MessageType>(
					LocalizationManager.GetString("DialogBoxes.ArchivingDlg.PrearchivingStatusMsg2",
					"The following session files will be added to your archive."), ArchivingDlgViewModel.MessageType.Progress);
			}
		}

		public string GetFileGroupDisplayMessage(string groupKey)
		{
			var fmt = LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ArchivingProgressMsg", "     {0}: {1}",
				"The first parameter is 'Session' or 'Contributor'. The second parameter is the session or contributor name.");

			var element = groupKey == string.Empty ?
				LocalizationManager.GetString("DialogBoxes.ArchivingDlg.SessionElementName", "Session") :
				LocalizationManager.GetString("DialogBoxes.ArchivingDlg.ContributorElementName", "Contributor");

			return string.Format(fmt, element, groupKey == string.Empty ? Title : groupKey);
		}

		/// ------------------------------------------------------------------------------------
		protected override IEnumerable<KeyValuePair<string, string>> GetFilesToCopy(IEnumerable<string> validComponentFilesToCopy)
		{
			if (GetCompletedStages(true).Any(s => s.Id == ComponentRole.kSourceComponentRoleId))
			{
				foreach (var kvp in base.GetFilesToCopy(validComponentFilesToCopy))
					yield return kvp;
			}
			else
			{
				var sourceRole = ComponentRoles.First(r => r.Id == ComponentRole.kSourceComponentRoleId);
				bool foundSourceFile = false;
				foreach (var srcFile in validComponentFilesToCopy)
				{
					var destFile = (foundSourceFile || !sourceRole.IsPotential(srcFile)) ? GetDestinationFilename(srcFile) :
						Path.Combine(FolderPath, sourceRole.GetCanoncialName(Path.GetFileNameWithoutExtension(srcFile), Path.GetFileName(srcFile)));
					if (!File.Exists(destFile))
					{
						yield return new KeyValuePair<string, string>(srcFile, destFile);
						foundSourceFile = true;
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public void SetAdditionalMetsData(RampArchivingDlgViewModel model)
		{
			model.SetScholarlyWorkType(ScholarlyWorkType.PrimaryData);
			model.SetDomains(SilDomain.Ling_LanguageDocumentation);

			var value = DateField;
			if (!string.IsNullOrEmpty(value))
				model.SetCreationDate(value);

			// Return the session's note as the abstract portion of the package's description.
			value = MetaDataFile.GetStringValue(SessionFileType.kSynopsisFieldName, null);
			if (!string.IsNullOrEmpty(value))
				model.SetAbstract(value, string.Empty);

			// Set contributors
			if (MetaDataFile.GetValue(SessionFileType.kContributionsFieldName, null) is
				ContributionCollection contributions && contributions.Count > 0)
			{
				model.SetContributors(contributions);
			}

			// Return total duration of source audio/video recordings.
			TimeSpan totalDuration = GetTotalDurationOfSourceMedia();
			if (totalDuration.Ticks > 0)
				model.SetAudioVideoExtent($"Total Length of Source Recordings: {totalDuration}");

			//model.SetSoftwareRequirements("SayMore");
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
		public void CustomFilenameNormalization(string key, string file, StringBuilder bldr)
		{
			if (key != string.Empty)
				bldr.Insert(0, "__Contributors__");
		}
		#endregion

		/// ------------------------------------------------------------------------------------
		public override void Load()
		{
			base.Load();

			StageCompletedControlValues = ComponentRoles.ToDictionary(role => role.Id,
				role => (StageCompleteType)Enum.Parse(typeof(StageCompleteType),
					(string)MetaDataFile.GetValue(SessionFileType.kStageFieldPrefix + role.Id,
						StageCompleteType.Auto.ToString())));
		}

		/// ------------------------------------------------------------------------------------
		public string GetProjectName()
		{
			// Sessions directory
			var dir = ParentFolderPath;
			Debug.Assert(dir != null);
			var parentDir = Directory.GetParent(dir);
			Debug.Assert(parentDir != null);

			// Find the project file
			var file = parentDir.GetFiles("*" + Settings.Default.ProjectFileExtension).FirstOrDefault();

			// The project name is the same as the project file name
			return Path.GetFileNameWithoutExtension(file?.Name);
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Comparer class to compare two string representations of session status'.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SessionStatusComparer : IComparer<string>
	{
		public int Compare(string x, string y)
		{
			if (x == y)
				return 0;

			if (x == null)
				return 1;

			if (y == null)
				return -1;

			if (!Enum.TryParse(Session.GetStatusAsEnumParsableString(x), out Session.Status xStatus))
				return 1;

			if (!Enum.TryParse(Session.GetStatusAsEnumParsableString(y), out Session.Status yStatus))
				return -1;

			return (int)xStatus - (int)yStatus;
		}
	}
}
