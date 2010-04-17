using System;
using System.IO;

namespace Sponge2
{
	public class SpongeProject
	{
//		public delegate SpongeProject Factory(string path);//autofac uses this

		private readonly string _path;

		public SpongeProject(string path)
		{
			_path = path;
		}

		public string ProjectsFolder { get { return _path; } }

		public string Name
		{
			get { return Path.GetFileName(ProjectsFolder); }
		}
	}
}