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
	/// This is a bootstrapper: the class which loads up a DI container.
	/// Here, we are doing something a bit unusual, in that we bootstrap everything based
	/// on the project we want to open.  So the client of this class should dispose of it
	/// when the project is closed, and it will see to the disposing of everything owned by
	/// the container.
	/// </summary>
	public class ProjectContext:IDisposable
	{
		private readonly string _projectPath;
		private IContainer _container;

		public ProjectContext(string projectPath)
		{
			_projectPath = projectPath;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Make the main window
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public ProjectWindow CreateProjectWindow()
		{
			var builder = new ContainerBuilder();

			////            builder.RegisterGeneratedFactory<NotesInProjectView.Factory>().ContainerScoped();

			//review: could have used a factory instead

			builder.Register(c =>
			{
				var p = c.Resolve<Project>(new Parameter[] { new PositionalParameter(0, _projectPath) });
				return p;
			});

			builder.Register(c => c.Resolve<ProjectWindow.Factory>()(_projectPath));

			var dataAccess = Assembly.GetExecutingAssembly();

			builder.RegisterAssemblyTypes(dataAccess);

			//because we're using xmlserializer, we can't depend on the constructor for injecting
			builder.RegisterType<Session>().PropertiesAutowired(true);

			//because we're using xmlserializer, we can't depend on the constructor for injecting
			builder.RegisterType<Person>().PropertiesAutowired(true);

			builder.RegisterInstance(FilesTypes).As(typeof(IEnumerable<FileType>));

			_container = builder.Build();

			var enumerable = _container.Resolve<IEnumerable<FileType>>();
			Debug.Assert(enumerable.Count() == 3);
			return _container.Resolve<ProjectWindow>();
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
	}
}