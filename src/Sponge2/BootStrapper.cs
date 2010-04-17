using System;
using System.Reflection;
using System.Windows.Forms;
using Autofac.Core;
using SIL.Sponge;
using Autofac;

namespace Sponge2
{
	public class BootStrapper : IDisposable
	{
		private readonly string _projectPath;

		public BootStrapper(string projectPath)
		{
			_projectPath = projectPath;
		}

		public Shell CreateShell()
		{
			var builder = new Autofac.ContainerBuilder();

			//review: could have used a factory instead

			builder.Register<SpongeProject>(c =>
			{
				var p = c.Resolve<SpongeProject>(new Parameter[] { new Autofac.PositionalParameter(0, _projectPath) });
				return p;
			});

			builder.Register<Shell>(c =>
			{
				return c.Resolve<Shell.Factory>()(_projectPath);
			});

			var dataAccess = Assembly.GetExecutingAssembly();

			builder.RegisterAssemblyTypes(dataAccess);

			var container = builder.Build();
			var shell = container.Resolve<Shell>();

			return shell;
		}

		public void Dispose()
		{
		}
	}
}