using Sponge2.Model;

namespace Sponge2
{
	public class SessionsPM
	{
		private readonly Project _project;

		public SessionsPM(Project project)
		{
			_project = project;
		}

		public string TestLabel { get { return _project.FolderPath; } }
	}
}