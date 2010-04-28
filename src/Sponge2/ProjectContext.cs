using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using SIL.Sponge;
using Autofac;
using Autofac.Core;
using Sponge2.Model;

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

		public ProjectContext(string projectPath, IContainer parentContainer)
		{
			_scope = parentContainer.BeginLifetimeScope();

			Project = _scope.Resolve<Func<string, Project>>()(projectPath);

			var factory = _scope.Resolve<ProjectWindow.Factory>();
			ProjectWindow = factory(projectPath);
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