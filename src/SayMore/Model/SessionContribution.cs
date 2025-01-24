using System;
using SIL.Core.ClearShare;

namespace SayMore.Model
{
	public class SessionContribution
	{
		private readonly Session _session;
		public string SessionId => _session.Id;
		public string SessionTitle => _session.Title;
		public DateTime SessionDate => _session.SessionDate;
		public string SpecificFileName { get; }
		public Contribution Contribution { get; }

		public SessionContribution(Session session, string specificFile, Contribution contribution)
		{
			_session = session;
			SpecificFileName = specificFile;
			Contribution = contribution;
		}
	}
}
