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
			_scope = parentContainer.BeginLifetimeScope(builder =>
			{
				builder.Register(c => c.Resolve<ProjectWindow.Factory>()(projectPath));
			});
			ProjectWindow = _scope.Resolve<ProjectWindow>();
		}

		public ProjectWindow ProjectWindow{ get; private set;}

		public void Dispose()
		{
			_scope.Dispose();
			_scope = null;
		}
	}
}