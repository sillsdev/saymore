using System.IO;
using Moq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.UI.ElementListScreen;

namespace SayMoreTests.UI.ElementListScreen
{
	[TestFixture]
	public class ElementListViewModelTests
	{
		TemporaryFolder _tmpFolder;
		ElementRepository<Person> _repo;
		ElementListViewModel<Person> _model;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;
			_tmpFolder = new TemporaryFolder("ElementRepoTestFolder");

			var peter = new Mock<Person>();
			peter.Setup(p => p.Id).Returns("peter");

			var paul = new Mock<Person>();
			paul.Setup(p => p.Id).Returns("paul");

			var mary = new Mock<Person>();
			mary.Setup(p => p.Id).Returns("mary");

			_repo = new ElementRepository<Person>(_tmpFolder.Path, "People", null,
			(folder, id, idChangedAction) =>
			{
				switch (id)
				{
					case "peter": return peter.Object;
					case "paul": return paul.Object;
					case "mary": return mary.Object;
				}

				return null;
			});

			Directory.CreateDirectory(_tmpFolder.Combine("People", "peter"));
			Directory.CreateDirectory(_tmpFolder.Combine("People", "paul"));
			Directory.CreateDirectory(_tmpFolder.Combine("People", "mary"));

			peter.Setup(p => p.FolderPath).Returns(_tmpFolder.Combine("People", "peter"));
			paul.Setup(p => p.FolderPath).Returns(_tmpFolder.Combine("People", "paul"));
			mary.Setup(p => p.FolderPath).Returns(_tmpFolder.Combine("People", "mary"));

			_repo.CreateNew("peter");
			_repo.CreateNew("paul");
			_repo.CreateNew("mary");

			_model = new ElementListViewModel<Person>(_repo);
		}

		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TearDown()
		{
			_tmpFolder.Dispose();
			_tmpFolder = null;
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyAllElementsStillExist_NoneRemoved_ReturnsTrue()
		{
			Assert.IsTrue(_model.VerifyAllElementsStillExist());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void VerifyAllElementsStillExist_OneRemoved_ReturnsFalse()
		{
			using (new Palaso.Reporting.ErrorReport.NonFatalErrorReportExpected())
			{
				Directory.Delete(_tmpFolder.Combine("People", "paul"));
				Assert.IsFalse(_model.VerifyAllElementsStillExist());
			}
		}
	}
}
