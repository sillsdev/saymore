using System;
using System.Collections;
using System.Collections.Generic;
using Sponge2.Model;

namespace Sponge2
{
	public class SessionsViewModel
	{
		private readonly Project _project;

		public SessionsViewModel(Project project)
		{
			_project = project;
		}

		public string TestLabel { get { return _project.FolderPath; } }

		public IEnumerable<Session> Sessions
		{
			get
			{
				return _project.Sessions;
			}
		}
	}
}