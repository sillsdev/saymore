using System;
using System.Linq;
using System.Collections.Generic;
using Localization;
using SayMore.Model.Fields;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// An event is recorded, documented, transcribed, etc.
	/// Each event is represented on disk as a single folder, with 1 or more files
	/// related to that even.  The one file it will always have is some meta data about
	/// the event.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class Event : ProjectElement
	{
		public enum Status
		{
			Incoming = 0,
			In_Progress,
			Finished,
			Skipped
		}

		//autofac uses this
		public delegate Event Factory(string parentElementFolder, string id);

		private readonly IEnumerable<ComponentRole> _componentRoles;
		private readonly PersonInformant _personInformant;

		[Obsolete("For Mocking Only")]
		public Event() { }

		/// ------------------------------------------------------------------------------------
		public Event(string parentElementFolder, string id,
			Action<ProjectElement, string, string> idChangedNotificationReceiver,
			EventFileType eventFileType, ComponentFile.Factory componentFileFactory,
			FileSerializer fileSerializer, ProjectElementComponentFile.Factory prjElementComponentFileFactory,
			IEnumerable<ComponentRole> componentRoles,
			PersonInformant personInformant)
			: base(parentElementFolder, id, idChangedNotificationReceiver, eventFileType,
				componentFileFactory, fileSerializer, prjElementComponentFileFactory)
		{
			_componentRoles = componentRoles;
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
			get { return "Event"; }
		}

		/// ------------------------------------------------------------------------------------
		protected static string ExtensionWithoutPeriodStatic
		{
			get { return Settings.Default.EventFileExtension.TrimStart('.'); }
		}

		/// ------------------------------------------------------------------------------------
		public override string DefaultElementNamePrefix
		{
			get
			{
				return LocalizationManager.LocalizeString(
					"EventsView.NewEventNamePrefix", "New Event");
			}
		}

		/// ------------------------------------------------------------------------------------
		protected override string NoIdSaveFailureMessage
		{
			get { return "You must specify a event id."; }
		}

		/// ------------------------------------------------------------------------------------
		protected override string AlreadyExistsSaveFailureMessage
		{
			get { return "Could not rename from {0} to {1} because there is already a event by that name."; }
		}

		/// ------------------------------------------------------------------------------------
		public override string DefaultStatusValue
		{
			get { return Status.Incoming.ToString(); }
		}

		#endregion

		/// ------------------------------------------------------------------------------------
		public override IEnumerable<ComponentRole> GetCompletedStages()
		{
			var list = base.GetCompletedStages().ToList();

			if (GetShouldReportHaveConsent())
			   list.Insert(0, _componentRoles.First(r => r.Id == "consent"));

			return list;
		}

		/// ------------------------------------------------------------------------------------
		private bool GetShouldReportHaveConsent()
		{
			var allParticipants = MetaDataFile.GetStringValue("participants", string.Empty);
			var personNames = FieldInstance.GetMultipleValuesFromText(allParticipants);
			bool allParticipantsHaveConsent = personNames.Count() > 0;

			return !personNames.Any(name => !_personInformant.GetHasInformedConsent(name)) &&
				allParticipantsHaveConsent;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// We get this message from the person informant when a person's name has changed.
		/// When that happens, we need to make sure we update the participant field in case
		/// it contains a name that changed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private void HandlePersonsNameChanged(ProjectElement element, string oldId, string newId)
		{
			var allParticipants = MetaDataFile.GetStringValue("participants", string.Empty);
			var personNames = FieldInstance.GetMultipleValuesFromText(allParticipants).ToList();
			var newNames = personNames.Select(name => (name == oldId ? newId : name));

			string failureMessage;
			MetaDataFile.SetStringValue("participants",
				FieldInstance.GetTextFromMultipleValues(newNames), out failureMessage);

			if (failureMessage != null)
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(failureMessage);
		}

		#region Static methods
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<string> GetStatusNames()
		{
			return Enum.GetNames(typeof(Status)).Select(x => x.ToString().Replace('_', ' '));
		}

		#endregion
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Comparer class to compare two string representations of event status'.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class EventStatusComparer : IComparer<string>
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

			if (!Enum.GetNames(typeof(Event.Status)).Contains(x))
				return 1;

			if (!Enum.GetNames(typeof(Event.Status)).Contains(y))
				return -1;

			return (int)Enum.Parse(typeof(Event.Status), x) -
				(int)Enum.Parse(typeof(Event.Status), y);
		}
	}
}
