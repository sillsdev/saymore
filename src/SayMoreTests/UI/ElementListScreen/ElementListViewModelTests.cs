using System.IO;
using System.Linq;
using Moq;
using NUnit.Framework;
using Palaso.TestUtilities;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.UI.ElementListScreen;
using System.Collections.Generic;
using SayMoreTests.Model.Files;

namespace SayMoreTests.UI.ElementListScreen
{
	[TestFixture]
	public class ElementListViewModelTests
	{
		TemporaryFolder _tmpFolder;
		ElementRepository<Person> _repo;
		ElementListViewModel<Person> _model;

		// TODO: More methods in the model need to have tests.

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;
			_tmpFolder = new TemporaryFolder("elementRepoTestFolder");

			var peter = new Mock<Person>();
			peter.Setup(p => p.Id).Returns("peter");
			peter.Setup(p => p.GetComponentFiles()).Returns(new List<ComponentFile>(new[]
			{
				ComponentFileTests.CreateComponentFile(_tmpFolder, peter.Object, "peterSong1.mp3"),
				ComponentFileTests.CreateComponentFile(_tmpFolder, peter.Object, "peterSong2.mp3"),
			}));

			var paul = new Mock<Person>();
			paul.Setup(p => p.Id).Returns("paul");
			paul.Setup(p => p.GetComponentFiles()).Returns(new List<ComponentFile>(new[]
			{
				ComponentFileTests.CreateComponentFile(_tmpFolder, peter.Object, "paulSong1.mp3"),
				ComponentFileTests.CreateComponentFile(_tmpFolder, peter.Object, "paulSong2.mp3"),
			}));

			var mary = new Mock<Person>();
			mary.Setup(p => p.Id).Returns("mary");
			mary.Setup(p => p.GetComponentFiles()).Returns(new List<ComponentFile>(new[]
			{
				ComponentFileTests.CreateComponentFile(_tmpFolder, peter.Object, "marySong1.mp3"),
				ComponentFileTests.CreateComponentFile(_tmpFolder, peter.Object, "marySong2.mp3"),
			}));

			_repo = new ElementRepository<Person>(_tmpFolder.Path, "People", new PersonFileType(null),
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
		public void ElementFileType_Call_ReturnsCorrectType()
		{
			Assert.IsInstanceOf(typeof(PersonFileType), _model.ElementFileType);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Elements_ContainsCorrectCount()
		{
			Assert.AreEqual(3, _model.Elements.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Elements_ContainsCorrectValues()
		{
			Assert.IsNotNull(_model.Elements.SingleOrDefault(e => e.Id == "peter"));
			Assert.IsNotNull(_model.Elements.SingleOrDefault(e => e.Id == "paul"));
			Assert.IsNotNull(_model.Elements.SingleOrDefault(e => e.Id == "mary"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveByItem_TryNull_ReturnsFalse()
		{
			Assert.IsFalse(_model.Remove(null as Person));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveByItem_TryGoodElement_RemovesAndReturnsTrue()
		{
			var person = _model.Elements.Single(e => e.Id == "mary") as Person;
			Assert.IsTrue(_model.Remove(person));
			Assert.IsNull(_model.Elements.SingleOrDefault(e => e.Id == "mary"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveByItem_TrySelectedElement_ChangesSelectedToNullAndReturnsTrue()
		{
			var person = _model.Elements.Single(e => e.Id == "mary") as Person;
			Assert.IsTrue(_model.SetSelectedElement(person));
			Assert.IsTrue(_model.Remove(person));
			Assert.IsNull(_model.SelectedElement);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveByItem_TryNonSelectedElement_DoesNotChangeSelectedAndReturnsTrue()
		{
			var person = _model.Elements.Single(e => e.Id == "mary") as Person;
			_model.SetSelectedElement(person);
			Assert.IsTrue(_model.Remove(_model.Elements.Single(e => e.Id == "paul") as Person));
			Assert.AreEqual(person, _model.SelectedElement);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveById_TryNull_ReturnsFalse()
		{
			Assert.IsFalse(_model.Remove(null as string));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveById_TryGoodElement_RemovesAndReturnsTrue()
		{
			Assert.IsTrue(_model.Remove("mary"));
			Assert.IsNull(_model.Elements.SingleOrDefault(e => e.Id == "mary"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveById_TrySelectedElement_ChangesSelectedToNullAndReturnsTrue()
		{
			var person = _model.Elements.Single(e => e.Id == "mary") as Person;
			Assert.IsTrue(_model.SetSelectedElement(person));
			Assert.IsTrue(_model.Remove("mary"));
			Assert.IsNull(_model.SelectedElement);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void RemoveById_TryNonSelectedElement_DoesNotChangeSelectedAndReturnsTrue()
		{
			var person = _model.Elements.Single(e => e.Id == "mary") as Person;
			_model.SetSelectedElement(person);
			Assert.IsTrue(_model.Remove("paul"));
			Assert.AreEqual(person, _model.SelectedElement);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetSelectedElement_Set_SetsAndReturnsTrue()
		{
			var person = _model.Elements.Single(e => e.Id == "mary") as Person;
			Assert.IsTrue(_model.SetSelectedElement(person));
			Assert.AreEqual(person, _model.SelectedElement);

			person = _model.Elements.Single(e => e.Id == "paul") as Person;
			Assert.IsTrue(_model.SetSelectedElement(person));
			Assert.AreEqual(person, _model.SelectedElement);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetSelectedElement_SetToSameElement_ReturnsFalse()
		{
			var person = _model.Elements.Single(e => e.Id == "mary") as Person;
			Assert.IsTrue(_model.SetSelectedElement(person));
			Assert.IsFalse(_model.SetSelectedElement(person));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIndexOfSelectedElement_NoSelectedElement_ReturnsMinusOne()
		{
			Assert.AreEqual(-1, _model.GetIndexOfSelectedElement());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetIndexOfSelectedElement_HaveSelectedElement_ReturnsCorrectIndex()
		{
			var person = _model.Elements.Single(e => e.Id == "paul") as Person;
			_model.SetSelectedElement(person);
			Assert.AreEqual(person, _model.Elements.ElementAt(_model.GetIndexOfSelectedElement()));
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

		/// ------------------------------------------------------------------------------------
		[Test]
		[Category("SkipOnTeamCity")]
		public void DeleteComponentFile_PassesGoodFile_RemovesFile()
		{
			_model.SetSelectedElement(_model.Elements.ElementAt(1) as Person);
			var file = _model.GetComponentFile(1);
			Assert.IsTrue(_model.DeleteComponentFile(file, false));
			Assert.IsFalse(File.Exists(file.PathToAnnotatedFile));
			Assert.IsFalse(File.Exists(file.PathToAnnotatedFile + ".meta"));
		}
	}
}
