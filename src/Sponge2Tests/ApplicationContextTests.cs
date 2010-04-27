using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Palaso.TestUtilities;
using Sponge2;
using Sponge2.Model;
using Autofac;
using Autofac.Core;


namespace SpongeTests
{
	public class Widget
	{
		//public System.Func<string, Widget> Factory;
		public delegate Widget Factory(string foo);

		public Widget(string foo)
		{

		}
	}

	[TestFixture]
	public class ApplicationContextTests
	{
		private TemporaryFolder _parentFolder;

		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("fileTypeTest");
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		[Test]
		public void FactoriesWithAutofac2()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<Widget>();
			var container = builder.Build();

			var x = container.Resolve<Widget.Factory>();
			x("hello");
		}

		[Test]
		public void FactoriesWithAutofac2_AnotherApproach()
		{
			var builder = new ContainerBuilder();
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
			var container = builder.Build();

			var x = container.Resolve<System.Func<string,Widget>>();
			x("hello");
		}

		[Test]
		public void CreateWelcomeDialog_NotNull()
		{
			using(var appContext = new ApplicationContext())
			{
				 using(var form =appContext.CreateWelcomeDialog())
				 {
					 Assert.IsNotNull(form);
				 }
			}
		}

		[Test]
		public void CreateProjectContext_ProjectWindowIsNotNull()
		{
			using (var appContext = new ApplicationContext())
			{
				using (var projectContext = appContext.CreateProjectContext(_parentFolder.Combine("theProject")))
				{
					Assert.IsNotNull(projectContext.ProjectWindow);
				}
			}
		}

	}
}