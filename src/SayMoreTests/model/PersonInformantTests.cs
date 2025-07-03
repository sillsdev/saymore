using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.Model.Files.DataGathering;

namespace SayMoreTests.Model
{
	[TestFixture]
	public sealed class PersonInformantTests
	{
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPersonByName_PersonNotFound_ReturnsNull()
		{
			var repo = new Mock<ElementRepository<Person>>();
			repo.Setup(x => x.GetById("Joe")).Returns((Person)null);
			var informant = new PersonInformant(repo.Object, null);
			Assert.IsNull(informant.GetPersonByNameOrCode("Joe"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPersonByName_PersonFound_ReturnsPerson()
		{
			var componentFile = new Mock<ProjectElementComponentFile>();
			var person = new Mock<Person>();
			person.Setup(p => p.GetInformedConsentComponentFile()).Returns(componentFile.Object);
			var repo = new Mock<ElementRepository<Person>>();

			repo.Setup(x => x.GetById("Joe")).Returns(person.Object);
			var informant = new PersonInformant(repo.Object, null);
			Assert.AreEqual(person.Object, informant.GetPersonByNameOrCode("Joe"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetHasInformedConsent_PersonNotFound_ReturnsFalse()
		{
			var repo = new Mock<ElementRepository<Person>>();
			repo.Setup(x => x.GetById("Joe")).Returns((Person)null);
			var informant = new PersonInformant(repo.Object, null);
			Assert.IsFalse(informant.GetHasInformedConsent("Joe"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetHasInformedConsent_PersonDoesNotExist_ReturnsFalse()
		{
			var person = new Mock<Person>();
			person.Setup(p => p.GetInformedConsentComponentFile()).Returns((ComponentFile)null);
			var repo = new Mock<ElementRepository<Person>>();

			repo.Setup(x => x.GetById("Joe")).Returns(person.Object);
			var informant = new PersonInformant(repo.Object, null);
			Assert.IsFalse(informant.GetHasInformedConsent("Joe"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetHasInformedConsent_PersonDoesExist_ReturnsTrue()
		{
			var componentFile = new Mock<ProjectElementComponentFile>();
			var person = new Mock<Person>();
			person.Setup(p => p.GetInformedConsentComponentFile()).Returns(componentFile.Object);
			var repo = new Mock<ElementRepository<Person>>();

			repo.Setup(x => x.GetById("Joe")).Returns(person.Object);
			var informant = new PersonInformant(repo.Object, null);
			Assert.IsTrue(informant.GetHasInformedConsent("Joe"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetPeopleNamesFromRepository_ReturnsThem()
		{
			var p1 = new Mock<Person>();
			p1.Setup(p => p.Id).Returns("Sadie");

			var p2 = new Mock<Person>();
			p2.Setup(p => p.Id).Returns("Jack");

			var repo = new Mock<ElementRepository<Person>>();
			repo.Setup(r => r.AllItems).Returns(new[] { p1.Object, p2.Object });
			var informant = new PersonInformant(repo.Object, null);

			Assert.That(informant.GetPeopleNamesFromRepository(),
				Is.EquivalentTo(new [] {"Sadie", "Jack"}));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllPeopleNames_GathererIsNull_ReturnsNamesFromRepo()
		{
			var p1 = new Mock<Person>();
			p1.Setup(p => p.Id).Returns("Sadie");

			var p2 = new Mock<Person>();
			p2.Setup(p => p.Id).Returns("Jack");

			var repo = new Mock<ElementRepository<Person>>();
			repo.Setup(r => r.AllItems).Returns(new[] { p1.Object, p2.Object });
			var informant = new PersonInformant(repo.Object, null);

			Assert.That(informant.GetAllPeopleNames(), Is.EquivalentTo(new[] { "Sadie", "Jack" }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllPeopleNames_GathererReturnsNothing_ReturnsNamesFromRepo()
		{
			var p1 = new Mock<Person>();
			p1.Setup(p => p.Id).Returns("Sadie");

			var p2 = new Mock<Person>();
			p2.Setup(p => p.Id).Returns("Jack");

			var repo = new Mock<ElementRepository<Person>>();
			repo.Setup(r => r.AllItems).Returns(new[] { p1.Object, p2.Object });

			var gatherer = new Mock<AutoCompleteValueGatherer>(null, null, null);
			gatherer.Setup(g => g.GetValueLists(false)).Returns(new Dictionary<string, IEnumerable<string>>());

			var informant = new PersonInformant(repo.Object, gatherer.Object);

			Assert.That(informant.GetAllPeopleNames(), Is.EquivalentTo(new[] { "Sadie", "Jack" }));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// When there are names in the person repo. and from the gatherer, only those from
		/// the gatherer are returned. That is because when there is a gatherer, it will
		/// always return the same names found in the repo. plus others the user adds in
		/// places like the contribution list. To return both the names in the repo. and from
		/// the gatherer would duplicate the names from the repo.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllPeopleNames_GathererReturnsSome_ReturnsOnlySome()
		{
			var p1 = new Mock<Person>();
			p1.Setup(p => p.Id).Returns("Sadie");

			var p2 = new Mock<Person>();
			p2.Setup(p => p.Id).Returns("Jack");

			var repo = new Mock<ElementRepository<Person>>();
			repo.Setup(r => r.AllItems).Returns(new[] { p1.Object, p2.Object });

			var gatherer = new Mock<AutoCompleteValueGatherer>(null, null, null);
			var lists = new Dictionary<string, IEnumerable<string>>
			{
				["person"] = new[] { "bear", "dawson" }
			};

			gatherer.Setup(g => g.GetValueLists(false)).Returns(lists);
			var informant = new PersonInformant(repo.Object, gatherer.Object);

			Assert.That(informant.GetAllPeopleNames(),
				Is.EquivalentTo(new[] { "bear", "dawson" }));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAllPeopleNames_GathererContainsMoreThanPeopleNames_ReturnsOnlyPeopleNames()
		{
			var repo = new Mock<ElementRepository<Person>>();
			var gatherer = new Mock<AutoCompleteValueGatherer>(null, null, null);
			var lists = new Dictionary<string, IEnumerable<string>>
			{
				["decoy"] = new[] { "shouldNotFind1", "shouldNotFind2" },
				["person"] = new[] { "bear", "dawson" }
			};

			gatherer.Setup(g => g.GetValueLists(false)).Returns(lists);
			var informant = new PersonInformant(repo.Object, gatherer.Object);

			Assert.That(informant.GetAllPeopleNames(),
				Is.EquivalentTo(new[] { "bear", "dawson" }));
		}
	}
}
