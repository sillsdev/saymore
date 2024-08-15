using SIL.Windows.Forms.ClearShare;

namespace SayMore.Model
{
	public class SessionContribution
	{
		private readonly Session _session;
		public string SessionId => _session.Id;
		public string SessionTitle => _session.Title;
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
