using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Autofac.Core;
using Sponge2.Model;
using Sponge2.UI.ProjectWindow;

namespace Sponge2
{
	/// <summary>
	/// TODO: it might be cleaner to remove this class and just have it all be in method
	/// on applicationContext
	/// </summary>
	public class ProjectContext:IDisposable
	{
		/// <summary>
		/// Any resources which belong only to this project will be tracked by this,
		/// and disposed of along with this ProjectContext class
		/// </summary>
		private ILifetimeScope _scope;

		public ProjectContext(string projectSettingsPath, IContainer parentContainer)
		{
			_scope = parentContainer.BeginLifetimeScope();

			//REVIEW: this is done because some things (like the repositories) will need it to exist
			//and they are actually created before the project at the moment.
			//Hopefully I'll think more clearly about this in the future




//			if (Directory.Exists(projectPath))
			{
				Project = _scope.Resolve<Func<string, Project>>()(projectSettingsPath);
			}
//			else
//			{
				//yuck
//				string projectName = Path.GetFileName(projectPath);
//				string parentDirectory = Path.GetDirectoryName(projectPath);
//				Project = _scope.Resolve<Func<string, string, Project>>()(parentDirectory,projectName);
//			}
			var repo = _scope.Resolve<Func<string, string, ElementRepository<Session>>>()(Path.GetDirectoryName(projectSettingsPath), "Sesssions");
			var factory = _scope.Resolve<ProjectWindow.Factory>();
			ProjectWindow = factory(projectSettingsPath);
		}


		public Project Project { get; private set; }

		public ProjectWindow ProjectWindow{ get; private set;}

		public void Dispose()
		{
			_scope.Dispose();
			_scope = null;
		}
	}
}