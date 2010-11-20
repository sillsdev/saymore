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

		///// ------------------------------------------------------------------------------------
		//public IEnumerable<Event> GetEventsByStatus(Event.Status status)
		//{
		//    return from evnt in _eventRepository.AllItems
		//           orderby evnt.Id
		//           where evnt.GetStatus() == status
		//           select evnt;
		//}

		///// ------------------------------------------------------------------------------------
		//public IEnumerable<Event> GetEventsByGenre(string genre)
		//{
		//    return from evnt in _eventRepository.AllItems
		//           orderby evnt.Id
		//           where evnt.MetaDataFile.GetStringValue("genre", null) == genre
		//           select evnt;
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Returns a dictionary of event lists; one list for each status.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public IDictionary<Event.Status, IEnumerable<Event>> GetEventsForEachStatus()
		//{
		//    // I'm sure there's a lambda expression to do all this,
		//    // but I can't seem to find the right way to write it.
		//    var list = new Dictionary<Event.Status, IEnumerable<Event>>();

		//    foreach (Event.Status status in Enum.GetValues(typeof(Event.Status)))
		//    {
		//        var eventList = GetEventsByStatus(status);
		//        if (eventList.Count() > 0)
		//            list[status] = eventList;
		//    }

		//    return list;
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Returns a dictionary of event lists; one list for each genre.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public IDictionary<string, IEnumerable<Event>> GetEventsForEachGenre()
		//{
		//    var list = new Dictionary<string, IEnumerable<Event>>();

		//    // REVIEW: By calling GetEventsByGenre in this loop, I may slow things down
		//    // (because it also iterates through AllItems), but for now, I leave it this way
		//    // until it proves to be too slow.
		//    foreach (var evnt in _eventRepository.AllItems)
		//    {
		//        var genre = (evnt.MetaDataFile.GetStringValue("genre", null) ??
		//            GenreDefinition.UnknownType.Name);

		//        if (!list.ContainsKey(genre))
		//            list[genre] = GetEventsByGenre(genre);
		//    }

		//    return list;
		//}

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
		protected static IEnumerable<Event> GetEventsFromListHavingFieldValue(
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
		protected static IDictionary<string, IEnumerable<Event>> GetCategorizedEventsFromListByField(
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