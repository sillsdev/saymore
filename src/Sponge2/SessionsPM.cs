namespace Sponge2
{
	public class SessionsPM
	{
		private readonly SpongeProject _project;

		public SessionsPM(SpongeProject project)
		{
			_project = project;
		}

		public string TestLabel { get { return _project.Name; } }
	}
}