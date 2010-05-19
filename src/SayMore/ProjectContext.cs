using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Autofac.Core;
using SayMore.Model;
using SayMore.UI.ElementListScreen;
using SayMore.UI.ProjectWindow;

namespace SayMore
{
	/// <summary>
	/// TODO: it might be cleaner to remove this class and just have it all be in method
	/// on applicationContext
	/// </summary>
	public class ProjectContext : IDisposable
	{
		/// <summary>
		/// Any resources which belong only to this project will be tracked by this,
		/// and disposed of along with this ProjectContext class
		/// </summary>
		private ILifetimeScope _scope;

		public ProjectContext(string projectSettingsPath, IContainer parentContainer)
		{
			_scope = parentContainer.BeginLifetimeScope(builder=>
			{
				builder.RegisterType<ElementRepository<Session>>().InstancePerLifetimeScope();
				builder.RegisterType<ElementRepository<Person>>().InstancePerLifetimeScope();
				builder.RegisterType<ElementListViewModel<Session>>().InstancePerLifetimeScope();
				builder.RegisterType<ElementListViewModel<Person>>().InstancePerLifetimeScope();
			});

			Project = _scope.Resolve<Func<string, Project>>()(projectSettingsPath);

			var sessionRepoFactory = _scope.Resolve<ElementRepository<Session>.Factory>();
			sessionRepoFactory(Path.GetDirectoryName(projectSettingsPath), "Sessions");

			var peopleRepoFactory = _scope.Resolve<ElementRepository<Person>.Factory>();
			peopleRepoFactory(Path.GetDirectoryName(projectSettingsPath), "People");

			var s = _scope.Resolve<ElementListViewModel<Session>>();
			var p = _scope.Resolve<ElementListViewModel<Person>>();


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

		public T ResolveForTests<T>() where T: class
		{
			return _scope.Resolve<T>();
		}
	}
}