using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Palaso.TestUtilities;
using Sponge2;


namespace SpongeTests
{
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