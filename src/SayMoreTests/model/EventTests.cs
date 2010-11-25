using System.Drawing;
using Moq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;
using System.Linq;
using System.Collections.Generic;

namespace SayMoreTests.Model
{
	[TestFixture]
	public class EventTests
	{
		private TemporaryFolder _parentFolder;

		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("eventTest");
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		private Event CreateEvent(IEnumerable<string>particpants)
		{
			ProjectElementComponentFile.Factory factory = (parentElement, fileType, fileSerializer, ootElementName) =>
			{
			  var file = new Mock<ProjectElementComponentFile>();
			  file.Setup(f => f.Save());
			  file.Setup(
				  f => f.GetStringValue("participants", string.Empty)).
				  Returns(particpants.Count()>0? particpants.Aggregate((a,b)=>a+","+b):string.Empty
				  );
			  return file.Object;
			};

			ComponentFile.Factory componentFactory = (parentElement, path) =>
			{
				var file = new Mock<ComponentFile>();
				//person.Setup(p => p.GetInformedConsentComponentFile()).Returns((ComponentFile)null);
				file.Setup(p => p.Save());
				return file.Object;
			};

			var personInformant = new Mock<PersonInformant>();
			foreach (var particpant in particpants)
			{
				personInformant.Setup(i => i.GetHasInformedConsent(particpant)).Returns(particpant.Contains("Consent"));
			}

			var componentRoles = new List<ComponentRole>();
			componentRoles.Add(new ComponentRole(null, "consent", null,
				ComponentRole.MeasurementTypes.None, null, null, Color.Empty, Color.Empty));

			return new Event(_parentFolder.Path, "dummyId", null,
				new EventFileType(() => null), componentFactory,
				new FileSerializer(), factory, componentRoles, personInformant.Object);

			//ComponentFile.CreateMinimalComponentFileForTests
		}

		/*
		 * THIS IS MOSTLY EMPTY BECAUSE MOST OF THE BEHAVIOR THUS FAR IS IN THE BASE CLASS,
		 * AND TESTED THERE, INSTEAD
		 */

		[Test]
		public void GetCompletedStages_NoParticpantsListed_NoConsent()
		{
			var stages = CreateEvent(new string[]{}).GetCompletedStages();
			Assert.IsFalse(stages.Any(s => s.Name == "consent"));
		}

		[Test]
		public void GetCompletedStages_ParticpantsListedButNotFound_NoConsent()
		{
			var stages = CreateEvent(new[] {"you", "me" }).GetCompletedStages();
			Assert.IsFalse(stages.Any(s => s.Name == "consent"));
		}
		[Test]
		public void GetCompletedStages_TwoParticpantsFoundOneLacksConsent_NoConsent()
		{
			var stages = CreateEvent(new[] { "oneWithConsent", "none" }).GetCompletedStages();
			Assert.IsFalse(stages.Any(s => s.Name == "consent"));
		}
		[Test]
		public void GetCompletedStages_TwoParticpantsFoundBothHaveConsent_ResultIncludesConsent()
		{
		  var stages = CreateEvent(new[] {"oneWithConsent", "anotherWithConsent" }).GetCompletedStages();
			Assert.IsFalse(stages.Any(s => s.Name == "consent"));
		}
	}
}
