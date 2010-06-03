using System;
using System.IO;
using Autofac;
using SayMore.Model;
using SayMore.UI.ElementListScreen;
using SayMore.UI.ProjectWindow;

namespace SayMore
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// TODO: it might be cleaner to remove this class and just have it all be in method
	/// on applicationContext
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ProjectContext : IDisposable
	{
		/// <summary>
		/// Any resources which belong only to this project will be tracked by this,
		/// and disposed of along with this ProjectContext class
		/// </summary>
		private ILifetimeScope _scope;

		public Project Project { get; private set; }
		public ProjectWindow ProjectWindow { get; private set; }

		/// ------------------------------------------------------------------------------------
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

			_scope.Resolve<ElementListViewModel<Session>>();
			_scope.Resolve<ElementListViewModel<Person>>();

			var factory = _scope.Resolve<ProjectWindow.Factory>();
			ProjectWindow = factory(projectSettingsPath);
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
			_scope.Dispose();
			_scope = null;
		}

		/// ------------------------------------------------------------------------------------
		public T ResolveForTests<T>() where T: class
		{
			return _scope.Resolve<T>();
		}
	}
}