using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// A method class that contains the data search functionality for a project, specifically
	/// session metadata, annotation (transcription/translation) text, and contributor notes.
	/// </summary>
	/// <remarks>
	/// Eventually, this will likely be extended to include searching for matches in additional
	/// project data (see https://github.com/sillsdev/saymore/issues/262).
	/// </remarks>
	/// ----------------------------------------------------------------------------------------
	public class MetadataSearch
	{
		private readonly ProjectContext _projectContext;

		public MetadataSearch(ProjectContext projectContext)
		{
			_projectContext = projectContext;
		}

		// Overridable so tests can supply sessions without a full ProjectContext/Project.
		protected virtual IEnumerable<Session> GetAllSessions()
		{
			return _projectContext.Project.GetAllSessions(CancellationToken.None);
		}

		public IEnumerable<string> SearchSessions(string query)
		{
			return GetAllSessions().Where(s => s.IsMatch(query)).Select(s => s.Id);
		}
	}
}
