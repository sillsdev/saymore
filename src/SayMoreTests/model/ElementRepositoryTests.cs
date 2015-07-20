using System.Linq;
using Moq;
using NUnit.Framework;
using SIL.TestUtilities;
using SayMore.Model;

namespace SayMoreTests.Model
{
	[TestFixture]
	public class ElementRepositoryTests
	{
		[Test]
		public void GetById_NotFound_ReturnsNull()
		{
			var repo = new ElementRepository<ProjectElement>();
			Assert.IsNull(repo.GetById("foo"));
		}

		[Test]
		public void GetById_Found_ReturnsItem()
		{
			var person = new Mock<ProjectElement>();
			person.Setup(p => p.Id).Returns("joe");
			using(var tempFolder = new TemporaryFolder("ElementRepoTestFolder"))
			{
				var repo = new ElementRepository<ProjectElement>(tempFolder.Path,
					"elementGroupName", null, (folder, id, idChangedAction) => person.Object);

				repo.CreateNew("joe");
				Assert.AreEqual(person.Object, repo.GetById("joe"));
			}
		}

		[Test]
		public void CreateNew_WithId_CreatesElement()
		{
			var person = new Mock<ProjectElement>();
			person.Setup(p => p.Id).Returns("joe");
			using (var tempFolder = new TemporaryFolder("ElementRepoTestFolder"))
			{
				var repo = new ElementRepository<ProjectElement>(tempFolder.Path,
					"elementGroupName", null, (folder, id, idChangedAction) => person.Object);

				repo.CreateNew("joe");
				Assert.IsTrue(repo.AllItems.Contains(person.Object));
			}
		}

		[Test]
		public void Remove_ById_RemovesItem()
		{
			var person = new Mock<ProjectElement>();
			person.Setup(p => p.Id).Returns("joe");
			person.Setup(p => p.FolderPath).Returns("*mocked*");
			using (var tempFolder = new TemporaryFolder("ElementRepoTestFolder"))
			{
				var repo = new ElementRepository<ProjectElement>(tempFolder.Path,
					"elementGroupName", null, (folder, id, idChangedAction) => person.Object);

				repo.CreateNew("joe");
				Assert.IsTrue(repo.AllItems.Contains(person.Object));
				repo.Remove("joe");
				Assert.IsFalse(repo.AllItems.Contains(person.Object));
			}
		}

		[Test]
		public void Remove_ByItem_RemovesItem()
		{
			var person = new Mock<ProjectElement>();
			person.Setup(p => p.Id).Returns("joe");
			person.Setup(p => p.FolderPath).Returns("*mocked*");
			using (var tempFolder = new TemporaryFolder("ElementRepoTestFolder"))
			{
				var repo = new ElementRepository<ProjectElement>(tempFolder.Path,
					"elementGroupName", null, (folder, id, idChangedAction) => person.Object);

				repo.CreateNew("joe");
				Assert.IsTrue(repo.AllItems.Contains(person.Object));
				repo.Remove(person.Object);
				Assert.IsFalse(repo.AllItems.Contains(person.Object));
			}
		}
	}
}
