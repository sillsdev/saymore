using System;
using System.Collections.Generic;
using System.Reflection;
using SIL.Sponge;
using Autofac;
using Sponge2.Model;
using Sponge2.ProjectChoosingAndCreating;

namespace Sponge2
{
	public class Widget
	{
		public delegate Widget Factory(string foo);
		public Widget(string foo){}
	}
	/// <summary>
	/// This is a wrapper around the DI container
	/// </summary>
	public class ApplicationContext:IDisposable
	{
		private IContainer _container;

		public ApplicationContext()
		{
			var builder = new ContainerBuilder();
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t=>!t.Name.Contains("Factory"));

			builder.RegisterInstance(FilesTypes).As(typeof(IEnumerable<FileType>));

			builder.RegisterInstance<Func<string, Project>>(path=>
			{
				return Project.FromSettingsFilePath(path, InjectProjectStuff);
			});

			_container = builder.Build();
		}

		private Project InjectProjectStuff(Project project)
		{
			//review: is this going to work if we have an inner scope? Will these things come from the outer scope?
			return _container.InjectUnsetProperties(project);
		}

		public WelcomeDialog CreateWelcomeDialog()
		{
			return _container.Resolve<WelcomeDialog>();
		}

		private static IEnumerable<FileType> FilesTypes
		{
			get
			{
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

		public ProjectContext CreateProjectContext(string projectPath)
		{
			return new ProjectContext(projectPath,_container);
		}
	}
}