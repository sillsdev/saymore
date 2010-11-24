using System;
using System.Collections.Generic;
using System.Linq;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides access to event workflow information.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class EventWorkflowInformant
	{
		private readonly ElementRepository<Event> _eventRepository;

		[Obsolete("For mocking only")]
		public EventWorkflowInformant(){}

		/// ------------------------------------------------------------------------------------
		public EventWorkflowInformant(ElementRepository<Event> eventRepository)
		{
			_eventRepository = eventRepository;
		}

		/// ------------------------------------------------------------------------------------
		public int NumberOfEvents
		{
			get { return _eventRepository.AllItems.Count(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a lists of events whose specified field contains the specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<Event> GetEventsHavingFieldValue(string field, string value)
		{
			return GetEventsFromListHavingFieldValue(_eventRepository.AllItems, field, value);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a subset of the specified list of events based on those events whose
		/// specified field contains the specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<Event> GetEventsFromListHavingFieldValue(
			IEnumerable<Event> eventList, string field, string value)
		{
			return from evnt in eventList
				   orderby evnt.Id
				   where evnt.MetaDataFile.GetStringValue(field, null) == value
				   select evnt;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a dictionary of event lists; one list for each unique value of the
		/// specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IDictionary<string, IEnumerable<Event>> GetCategorizedEventsByField(string field)
		{
			return GetCategorizedEventsFromListByField(_eventRepository.AllItems, field);
		}

		/// ------------------------------------------------------------------------------------
		public static IDictionary<string, IEnumerable<Event>> GetCategorizedEventsFromListByField(
			IEnumerable<Event> eventList, string field)
		{
			var list = new Dictionary<string, IEnumerable<Event>>();

			foreach (var evnt in eventList)
			{
				var value = evnt.MetaDataFile.GetStringValue(field, null);
				if (!string.IsNullOrEmpty(value) && !list.ContainsKey(value))
					list[value] = GetEventsFromListHavingFieldValue(eventList, field, value);
			}

			return list;
		}

		/// ------------------------------------------------------------------------------------
		public IDictionary<string, IDictionary<string, IEnumerable<Event>>> GetCategorizedEventsFromDoubleKey(
			string primaryField, string secondaryField)
		{
			var outerList = new Dictionary<string, IDictionary<string, IEnumerable<Event>>>();

			foreach (var kvp in GetCategorizedEventsByField(primaryField))
				outerList[kvp.Key] = GetCategorizedEventsFromListByField(kvp.Value, secondaryField);

			return outerList;
		}
	}
}