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
	public class BootStrapper : IDisposable
	{
		private readonly string _projectPath;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public BootStrapper(string projectPath)
		{
			_projectPath = projectPath;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Shell CreateShell()
		{
			var builder = new ContainerBuilder();

			////            builder.RegisterGeneratedFactory<NotesInProjectView.Factory>().ContainerScoped();

			//review: could have used a factory instead

			builder.Register(c =>
			{
				var p = c.Resolve<Project>(new Parameter[] { new PositionalParameter(0, _projectPath) });
				return p;
			});

			builder.Register(c => c.Resolve<Shell.Factory>()(_projectPath));

			var dataAccess = Assembly.GetExecutingAssembly();

			builder.RegisterAssemblyTypes(dataAccess);

			//because we're using xmlserializer, we can't depend on the constructor for injecting
			builder.RegisterType<Session>().PropertiesAutowired(true);

			//because we're using xmlserializer, we can't depend on the constructor for injecting
			builder.RegisterType<Person>().PropertiesAutowired(true);

			builder.RegisterInstance(FileType.Create("video", new[] {".avi", ".mov", ".mp4"}));
			builder.RegisterInstance(FileType.Create("image", new[] { ".jpg", ".tiff", ".bmp" }));
			builder.RegisterInstance(FileType.Create("audio", new[] { ".mp3", ".wav", ".ogg" }));

			var container = builder.Build();

			var enumerable = container.Resolve<IEnumerable<FileType>>();
			Debug.Assert(enumerable.Count() == 3);
			return container.Resolve<Shell>();
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
		}
	}
}