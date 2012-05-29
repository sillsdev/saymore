using System;
using System.Collections.Generic;
using System.Linq;
using SayMore.Model.Files;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Provides access to session workflow information.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class SessionWorkflowInformant
	{
		private readonly ElementRepository<Session> _sessionRepository;
		private IEnumerable<ComponentRole> _componentRoles;

		[Obsolete("For mocking only")]
		public SessionWorkflowInformant(){}

		/// ------------------------------------------------------------------------------------
		public SessionWorkflowInformant(ElementRepository<Session> sessionRepository,
			IEnumerable<ComponentRole> componentRoles)
		{
			_sessionRepository = sessionRepository;
			_componentRoles = componentRoles;
		}

		/// ------------------------------------------------------------------------------------
		public int NumberOfSessions
		{
			get { return _sessionRepository.AllItems.Count(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a lists of sessions whose specified field contains the specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<Session> GetSessionsHavingFieldValue(string field, string value)
		{
			return GetSessionsFromListHavingFieldValue(_sessionRepository.AllItems, field, value);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a subset of the specified list of sessions whose specified field contains
		/// the specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static IEnumerable<Session> GetSessionsFromListHavingFieldValue(
			IEnumerable<Session> sessionList, string field, string value)
		{
			return from session in sessionList
				   orderby session.Id
				   where session.MetaDataFile.GetStringValue(field, null) == value
				   select session;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Returns a dictionary of session lists; one list for each unique value of the
		/// specified field.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IDictionary<string, IEnumerable<Session>> GetCategorizedSessionsByField(string field)
		{
			return GetCategorizedSessionsFromListByField(_sessionRepository.AllItems, field);
		}

		/// ------------------------------------------------------------------------------------
		public static IDictionary<string, IEnumerable<Session>> GetCategorizedSessionsFromListByField(
			IEnumerable<Session> sessionList, string field)
		{
			var list = new Dictionary<string, IEnumerable<Session>>();

			foreach (var session in sessionList)
			{
				var value = session.MetaDataFile.GetStringValue(field, null);
				if (!string.IsNullOrEmpty(value) && !list.ContainsKey(value))
					list[value] = GetSessionsFromListHavingFieldValue(sessionList, field, value);
			}

			return list;
		}

		/// ------------------------------------------------------------------------------------
		public IDictionary<string, IDictionary<string, IEnumerable<Session>>> GetCategorizedSessionsFromDoubleKey(
			string primaryField, string secondaryField)
		{
			var outerList = new Dictionary<string, IDictionary<string, IEnumerable<Session>>>();

			foreach (var kvp in GetCategorizedSessionsByField(primaryField))
				outerList[kvp.Key] = GetCategorizedSessionsFromListByField(kvp.Value, secondaryField);

			return outerList;
		}

		/// ------------------------------------------------------------------------------------
		public IDictionary<ComponentRole, IEnumerable<Session>> GetSessionsCategorizedByStage()
		{
			return _componentRoles.ToDictionary(role => role, role =>
				from session in _sessionRepository.AllItems
				where session.GetCompletedStages().SingleOrDefault(r => r.Id == role.Id) != null
				select session);
		}
	}
}