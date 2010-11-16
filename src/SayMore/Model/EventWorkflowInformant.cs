using System;
using System.Collections.Generic;
using System.Linq;
using SayMore.Model.Fields;

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
		public IEnumerable<Event> GetEventsByStatus(Event.Status status)
		{
			return from evnt in _eventRepository.AllItems
				   orderby evnt.Id
				   where evnt.GetStatus() == status
				   select evnt;
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<Event> GetEventsByGenre(string genre)
		{
			return from evnt in _eventRepository.AllItems
				   orderby evnt.Id
				   where evnt.MetaDataFile.GetStringValue("genre", null) == genre
				   select evnt;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a dictionary of event lists; one list for each status.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IDictionary<Event.Status, IEnumerable<Event>> GetEventsForEachStatus()
		{
			// I'm sure there's a lambda expression to do all this,
			// but I can't seem to find the right way to write it.
			var list = new Dictionary<Event.Status, IEnumerable<Event>>();

			foreach (Event.Status status in Enum.GetValues(typeof(Event.Status)))
			{
				var eventList = GetEventsByStatus(status);
				if (eventList.Count() > 0)
					list[status] = eventList;
			}

			return list;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a dictionary of event lists; one list for each genre.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IDictionary<string, IEnumerable<Event>> GetEventsForEachGenre()
		{
			var list = new Dictionary<string, IEnumerable<Event>>();

			// REVIEW: By calling GetEventsByGenre in this loop, I may slow things down
			// (because it also iterates through AllItems), but for now, I leave it this way
			// until it proves to be too slow.
			foreach (var evnt in _eventRepository.AllItems)
			{
				var genre = (evnt.MetaDataFile.GetStringValue("genre", null) ??
					GenreDefinition.UnknownType.Name);

				if (!list.ContainsKey(genre))
					list[genre] = GetEventsByGenre(genre);
			}

			return list;
		}
	}
}