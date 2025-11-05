using System;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore;
using SayMore.Model;
using SayMore.Model.Files;
using System.Threading;

namespace SayMoreTests
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

		/// <summary>
		/// This is mostly a "smoke test" for the Dependency Injection System
		/// </summary>
		[Test][Category("SkipOnCI")]
		public void CreateWelcomeDialog_NotNull()
		{
			using(var appContext = new ApplicationContainer())
			{
				 using(var form =appContext.CreateWelcomeDialog())
				 {
					 Assert.IsNotNull(form);
				 }
			}
		}

		[Test][Category("SkipOnCI")]
		public void CreateWelcomeDialog_CanCreateOnAfterAnother()
		{
			using (var appContext = new ApplicationContainer())
			{
				appContext.CreateWelcomeDialog().Dispose();
				appContext.CreateWelcomeDialog().Dispose();
			}
		}

		/// <summary>
		/// This is mostly a "smoke test" for the Dependency Injection System
		/// </summary>
		[Test, Apartment(ApartmentState.STA)]
		[Category("SkipOnCI")]
		public void CreateProjectContext_ProjectWindowIsNotNull()
		{
			using (var appContext = new ApplicationContainer())
			{
				using (var projectContext = CreateProjectContext(appContext))
				{
					Assert.IsNotNull(projectContext.ProjectWindow);
				}
			}
		}

		private ProjectContext CreateProjectContext(ApplicationContainer appContext)
		{
			return appContext.CreateProjectContext(_parentFolder.Combine("theProject", "theProject."+Project.ProjectSettingsFileExtension));
		}


		[Test, Apartment(ApartmentState.STA)]
		[Category("SkipOnCI")]
		public void CreateProjectContext_CanCreateTwoProjectsConsecutively()
		{
			using (var appContext = new ApplicationContainer())
			{
				using (var projectContext = CreateProjectContext(appContext))
				{
					Assert.IsNotNull(projectContext.ProjectWindow);
					projectContext.ProjectWindow.Dispose();
				}
				using (var projectContext = CreateProjectContext(appContext))
				{
					Assert.IsNotNull(projectContext.ProjectWindow);
					projectContext.ProjectWindow.Dispose();
				}
			}
		}

		[Test, Apartment(ApartmentState.STA)]
		[Category("SkipOnCI")]
		public void ContainerSanityCheck_CanGet_ComponentFile_Factory()
		{
			using (var appContext = new ApplicationContainer())
			{
				using (var projectContext = CreateProjectContext(appContext))
				{
					var factory = projectContext.ResolveForTests<Func<ProjectElement, string, ComponentFile>>();
					//will throw if the container couldn't put all the pieces together
					factory(null, _parentFolder.Combine("test.txt"));
				}
			}
		}
	}
}