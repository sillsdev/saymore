using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Sponge2.Model;
using Sponge2.UI.ProjectChoosingAndCreating;

namespace Sponge2
{
	/// <summary>
	/// This is sortof a wrapper around the DI container. I'm not thrilled with the name I've
	/// used (jh).
	/// </summary>
	public class ApplicationContainer:IDisposable
	{
		private IContainer _container;

		public ApplicationContainer()
		{
			var builder = new ContainerBuilder();
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t=>!t.Name.Contains("Factory"));
			builder.RegisterType<ElementRepository<Session>>().InstancePerLifetimeScope();
			builder.RegisterType<ElementRepository<Person>>().InstancePerLifetimeScope();

			builder.RegisterInstance(FilesTypes).As(typeof(IEnumerable<FileType>));

			_container = builder.Build();
		}


		public WelcomeDialog CreateWelcomeDialog()
		{
			return _container.Resolve<WelcomeDialog>();
		}

		private static IEnumerable<FileType> FilesTypes
		{
			get
			{
				yield return FileType.Create("session", new[] {".session"});
				yield return FileType.Create("video", new[] {".avi", ".mov", ".mp4"});
				yield return FileType.Create("image", new[] {".jpg", ".tiff", ".bmp"});
				yield return FileType.Create("audio", new[] {".mp3", ".wav", ".ogg"});
			}
		}

		public void Dispose()
		{
			_container.Dispose();
			_container = null;
		}

		public ProjectContext CreateProjectContext(string projectSettingsPath)
		{
			return new ProjectContext(projectSettingsPath,_container);
		}
	}
}