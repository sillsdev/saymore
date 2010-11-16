using Moq;
using NUnit.Framework;
using SayMore.Model;
using SayMore.Model.Files;

namespace SayMoreTests.model
{
	[TestFixture]
	public sealed class PersonInformantTests
	{
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetHasInformedConsent_PersonNotFound_ReturnsFalse()
		{
			var repo = new Mock<ElementRepository<Person>>();
			repo.Setup(x => x.GetById("Joe")).Returns((Person)null);
			var informant = new PersonInformant(repo.Object);
			Assert.IsFalse(informant.GetHasInformedConsent("Joe"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetHasInformedConsent_PersonDoesNot_ReturnsFalse()
		{
			var person = new Mock<Person>();
			person.Setup(p => p.GetInformedConsentComponentFile()).Returns((ComponentFile)null);
			var repo = new Mock<ElementRepository<Person>>();

			repo.Setup(x => x.GetById("Joe")).Returns(person.Object);
			var informant = new PersonInformant(repo.Object);
			Assert.IsFalse(informant.GetHasInformedConsent("Joe"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetHasInformedConsent_PersonDoes_ReturnsTrue()
		{
			var componentFile = new Mock<ProjectElementComponentFile>();
			var person = new Mock<Person>();
			person.Setup(p => p.GetInformedConsentComponentFile()).Returns(componentFile.Object);
			var repo = new Mock<ElementRepository<Person>>();

			repo.Setup(x => x.GetById("Joe")).Returns(person.Object);
			var informant = new PersonInformant(repo.Object);
			Assert.IsTrue(informant.GetHasInformedConsent("Joe"));
		}
	}
}
