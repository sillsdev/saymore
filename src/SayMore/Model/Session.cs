using System;
using System.Linq;
using System.Collections.Generic;
using Localization;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.Utilities;

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
			FileSerializer fileSerializer,
			ProjectElementComponentFile.Factory prjElementComponentFileFactory,
			IEnumerable<ComponentRole> componentRoles,
			PersonInformant personInformant)
			: base(parentElementFolder, id, idChangedNotificationReceiver, sessionFileType,
				componentFileFactory, fileSerializer, prjElementComponentFileFactory, componentRoles)
		{
			_personInformant = personInformant;

			if (string.IsNullOrEmpty(MetaDataFile.GetStringValue("genre", null)))
			{
				string failureMsg;
				MetaDataFile.SetValue("genre", GenreDefinition.UnknownType.Name, out failureMsg);
				if (failureMsg == null)
					MetaDataFile.Save();
			}

			if (_personInformant != null)
				_personInformant.PersonNameChanged += HandlePersonsNameChanged;
		}

		#region Properties
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
			get { return LocalizationManager.GetString("SessionsView.Miscellaneous.NewEventNamePrefix", "New Session"); }
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
		public static string GetLocalizedStatus(string statusAsText)
		{
			statusAsText = statusAsText.Replace(' ', '_');

			if (statusAsText == Status.Incoming.ToString())
				return LocalizationManager.GetString("SessionsView.SessionStatus.Incoming", "Incoming");

			if (statusAsText == Status.In_Progress.ToString())
				return LocalizationManager.GetString("SessionsView.SessionStatus.InProgress", "In Progress");

			if (statusAsText == Status.Finished.ToString())
				return LocalizationManager.GetString("SessionsView.SessionStatus.Finished", "Finished");

			return LocalizationManager.GetString("SessionsView.SessionStatus.Skipped", "Skipped");
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
			var allParticipants = MetaDataFile.GetStringValue("participants", string.Empty);
			var personNames = FieldInstance.GetMultipleValuesFromText(allParticipants).ToArray();
			bool allParticipantsHaveConsent = personNames.Length > 0;

			return personNames.All(name => _personInformant.GetHasInformedConsent(name)) &&
				allParticipantsHaveConsent;
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

			string failureMessage;
			MetaDataFile.SetStringValue("participants",
				FieldInstance.GetTextFromMultipleValues(newNames), out failureMessage);

			if (failureMessage != null)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<string> GetAllParticipants()
		{
			var allParticipants = MetaDataFile.GetStringValue("participants", string.Empty);
			return FieldInstance.GetMultipleValuesFromText(allParticipants);
		}

		/// ------------------------------------------------------------------------------------
		public void CreateArchiveFile()
		{
			var helper = new ArchivingDlgViewModel(this, _personInformant);

			using (var dlg = new ArchivingDlg(helper))
				dlg.ShowDialog();
		}

		/// ------------------------------------------------------------------------------------
		public override void Load()
		{
			base.Load();

			StageCompletedControlValues = ComponentRoles.ToDictionary(role => role.Id,
				role => (StageCompleteType)Enum.Parse(typeof(StageCompleteType),
					MetaDataFile.GetValue("stage_" + role.Id, StageCompleteType.Auto.ToString()) as string));
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

			x = x.Replace(' ', '_');
			y = y.Replace(' ', '_');

			if (!Enum.GetNames(typeof(Session.Status)).Contains(x))
				return 1;

			if (!Enum.GetNames(typeof(Session.Status)).Contains(y))
				return -1;

			return (int)Enum.Parse(typeof(Session.Status), x) -
				(int)Enum.Parse(typeof(Session.Status), y);
		}
	}
}
