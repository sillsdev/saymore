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
		private ILifetimeScope _scope;

		public ProjectContext(string projectPath, IContainer parentContainer)
		{
			_scope = parentContainer.BeginLifetimeScope();

			var factory = parentContainer.Resolve<ProjectWindow.Factory>();
			ProjectWindow = factory(projectPath);
		}

		public ProjectWindow ProjectWindow{ get; private set;}

		public void Dispose()
		{
			_scope.Dispose();
			_scope = null;
		}
	}
}