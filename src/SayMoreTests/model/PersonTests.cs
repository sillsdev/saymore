using System.IO;
using L10NSharp;
using NUnit.Framework;
using SIL.IO;
using SIL.Reporting;
using SIL.TestUtilities;

namespace SayMoreTests.Model
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PersonTests
	{
		private TemporaryFolder _parentFolder;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			LocalizationManager.StrictInitializationMode = false;
			ErrorReport.IsOkToInteractWithUser = false;
			_parentFolder = new TemporaryFolder("ProjectElementTest");
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetInformedConsentComponentFile_NonePresent_ReturnsNull()
		{
			using (var person = ProjectElementTests.CreatePerson(_parentFolder.Path, "soarbum"))
			{
				using (var fileToAdd1 = new TempFile())
				using (var fileToAdd2 = new TempFile())
				{
					person.AddComponentFiles(new[] { fileToAdd1.Path, fileToAdd2.Path });
					Assert.That(person.GetInformedConsentComponentFile(), Is.Null);
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetInformedConsentComponentFile_ConsentFileAdded_ReturnsThatFile()
		{
			using (var person = ProjectElementTests.CreatePerson(_parentFolder.Path, "soarbum"))
			{
				using (var fileToAdd1 = new TempFile())
				using (var fileToAdd2 = new TempFile())
				{
					var consentFileName = Path.GetDirectoryName(fileToAdd1.Path);
					consentFileName = Path.Combine(consentFileName, "ddo_consent.pdf");

					try
					{
						File.Move(fileToAdd1.Path, consentFileName);
						person.AddComponentFiles(new[] { consentFileName, fileToAdd2.Path });

						var componentFile = person.GetInformedConsentComponentFile();
						Assert.That(componentFile.PathToAnnotatedFile.EndsWith("ddo_consent.pdf"), Is.True);
					}
					catch
					{
						File.Delete(consentFileName);
					}
				}
			}
		}
	}
}
