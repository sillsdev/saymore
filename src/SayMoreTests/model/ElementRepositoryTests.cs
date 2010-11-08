using Moq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;

namespace SayMoreTests.model
{
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
				var repo = new ElementRepository<ProjectElement>(tempFolder.Path, "elementGroupName", null,
																 (folder, id) => person.Object);

				repo.CreateNew("joe");
				Assert.AreEqual(person.Object, repo.GetById("joe"));
			}
		}
	}
}
