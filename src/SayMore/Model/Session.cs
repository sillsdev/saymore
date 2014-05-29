using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using L10NSharp;
using Palaso.Reporting;
using Palaso.UI.WindowsForms.ClearShare;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A session is recorded, documented, transcribed, etc.
	/// Each session is represented on disk as a single folder, with 1 or more files
	/// related to that session.  The one file it will always have is some meta data about
	/// the session.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Session : ProjectElement
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
		public string Title
		{
			get { return MetaDataFile.GetStringValue(SessionFileType.kTitleFieldName, null) ?? Id; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string ExtensionWithoutPeriod
		{
			get { return ExtensionWithoutPeriodStatic; }
		}

		/// ------------------------------------------------------------------------------------
		public override string RootElementName
		{
			get { return "Session"; }
		}

		/// ------------------------------------------------------------------------------------
		protected static string ExtensionWithoutPeriodStatic
		{
			get { return Settings.Default.SessionFileExtension.TrimStart('.'); }
		}

		/// ------------------------------------------------------------------------------------
		public override string DefaultElementNamePrefix
		{
			get { return LocalizationManager.GetString("SessionsView.Miscellaneous.NewSessionNamePrefix", "New Session"); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string NoIdSaveFailureMessage
		{
			get { return LocalizationManager.GetString("SessionsView.Miscellaneous.NoIdSaveFailureMessage", "You must specify a session id."); }
		}

		/// ------------------------------------------------------------------------------------
		protected override string AlreadyExistsSaveFailureMessage
		{
			get
			{
				return LocalizationManager.GetString("SessionsView.Miscellaneous.SessionAlreadyExistsSaveFailureMessage",
					"Could not rename from {0} to {1} because there is already a session by that name.");
			}
		}

		/// ------------------------------------------------------------------------------------
		public override string DefaultStatusValue
		{
			get { return Status.Incoming.ToString(); }
		}

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

			Status status;
			if (Enum.TryParse(statusAsText, out status))
				return GetLocalizedStatus(status);
			throw new ArgumentException(string.Format("Value {0} is not valid status.", statusAsText), "statusAsText");
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
					throw new ArgumentException(string.Format("Value {0} is not valid status.", status), "status");
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

			return (modifyComputedListWithUserOverrides ?
				GetCompletedStagesModifedByUserOverrides(list) : list);
		}

		/// ------------------------------------------------------------------------------------
		private bool GetShouldReportHaveConsent()
		{
			var allParticipants = MetaDataFile.GetStringValue(SessionFileType.kParticipantsFieldName, string.Empty);
			var personNames = FieldInstance.GetMultipleValuesFromText(allParticipants).ToArray();
			bool allParticipantsHaveConsent = personNames.Length > 0;

			return personNames.All(name => _personInformant.GetHasInformedConsent(name)) &&
				allParticipantsHaveConsent;
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
			foreach (var file in GetComponentFiles().Where(f => (f.FileType as FileTypeWithContributors) != null))
			{
				var values = file.MetaDataFieldValues.FirstOrDefault(v => v.FieldId == "contributions");
				if (values == null)
					continue;

				var contributions = values.Value as ContributionCollection;
				if (contributions == null)
					continue;

				foreach (var contribution in contributions.Where(contribution => contribution.ContributorName == e.OldId))
					contribution.ContributorName = e.NewId;

				string failureMessage;
				file.SetValue("contributions", contributions, out failureMessage);

				if (failureMessage == null)
					file.Save();
				else
					ErrorReport.NotifyUserOfProblem(failureMessage);
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<string> GetAllParticipants()
		{
			var allParticipants = MetaDataFile.GetStringValue(SessionFileType.kParticipantsFieldName, string.Empty);
			return FieldInstance.GetMultipleValuesFromText(allParticipants);
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Person> GetAllPersonsInSession()
		{
			return GetAllParticipants().Select(n => _personInformant.GetPersonByNameOrCode(n)).Where(p => p != null).ToList();
		}

		/// ------------------------------------------------------------------------------------
		public override void Load()
		{
			base.Load();

			StageCompletedControlValues = ComponentRoles.ToDictionary(role => role.Id,
				role => (StageCompleteType)Enum.Parse(typeof(StageCompleteType),
					MetaDataFile.GetValue(SessionFileType.kStageFieldPrefix + role.Id, StageCompleteType.Auto.ToString()) as string));
		}

		/// ------------------------------------------------------------------------------------
		public string GetProjectName()
		{
			// Sessions directory
			var dir = ParentFolderPath;

			// Find the project file
			var file = Directory.GetParent(dir).GetFiles("*" + Settings.Default.ProjectFileExtension).FirstOrDefault();

			// The project name is the same as the project file name
			return file != null ? Path.GetFileNameWithoutExtension(file.Name) : null;
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

			Session.Status xStatus, yStatus;

			if (!Enum.TryParse(Session.GetStatusAsEnumParsableString(x), out xStatus))
				return 1;

			if (!Enum.TryParse(Session.GetStatusAsEnumParsableString(y), out yStatus))
				return -1;

			return (int)xStatus - (int)yStatus;
		}
	}
}
