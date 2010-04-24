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

			//review: could have used a factory instead

			builder.Register(c =>
			{
				var p = c.Resolve<SpongeProject>(new Parameter[] { new PositionalParameter(0, _projectPath) });
				return p;
			});

			builder.Register(c => c.Resolve<Shell.Factory>()(_projectPath));

			var dataAccess = Assembly.GetExecutingAssembly();

			builder.RegisterAssemblyTypes(dataAccess);

			var container = builder.Build();
			return container.Resolve<Shell>();
		}

		/// ------------------------------------------------------------------------------------
		public void Dispose()
		{
		}
	}
}