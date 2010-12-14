using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SayMore.ClearShare;
using SayMore.Model.Fields;
using SayMore.Model.Files.DataGathering;

namespace SayMoreTests.ClearShare
{
	[TestFixture]
	public class ContributorsListControlViewModelTests
	{
		private ContributorsListControlViewModel _model;
		private ContributionCollection _contributions;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			var system = new OlacSystem();

			_model = new ContributorsListControlViewModel(null);
			_contributions = new ContributionCollection(new[]
			{
				new Contribution("Leroy", system.GetRoles().ElementAt(0)),
				new Contribution("Jed", system.GetRoles().ElementAt(1)),
				new Contribution("Art", system.GetRoles().ElementAt(2))
			});

			_model.SetContributionList(_contributions);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAutoCompleteNames_NullGatherer_ReturnsEmptyList()
		{
			Assert.AreEqual(0, _model.GetAutoCompleteNames().Count);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetAutoCompleteNames_HasGatherer_ReturnsNames()
		{
			var gatherer = new Mock<AutoCompleteValueGatherer>(null, null, null);
			var lists = new Dictionary<string, IEnumerable<string>>();
			lists["person"] = new[] { "jimmy", "tommy" };
			gatherer.Setup(g => g.GetValueLists(false)).Returns(lists);
			_model = new ContributorsListControlViewModel(gatherer.Object);

			var names = _model.GetAutoCompleteNames();
			Assert.AreEqual(2, names.Count);
			Assert.IsTrue(names.Contains("jimmy"));
			Assert.IsTrue(names.Contains("tommy"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCanDeleteContribution_NullList_ReturnsFalse()
		{
			_model.SetContributionList(null);
			Assert.IsFalse(_model.GetCanDeleteContribution(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCanDeleteContribution_EmptyList_ReturnsFalse()
		{
			_model.SetContributionList(new ContributionCollection());
			Assert.AreEqual(0, _model.Contributions.Count());
			Assert.IsFalse(_model.GetCanDeleteContribution(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCanDeleteContribution_InvalidTooSmall_ReturnsFalse()
		{
			Assert.IsFalse(_model.GetCanDeleteContribution(-1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCanDeleteContribution_InvalidTooBig_ReturnsFalse()
		{
			Assert.IsFalse(_model.GetCanDeleteContribution(3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCanDeleteContribution_ValidIndex_ReturnsTrue()
		{
			Assert.IsTrue(_model.GetCanDeleteContribution(0));
			Assert.IsTrue(_model.GetCanDeleteContribution(1));
			Assert.IsTrue(_model.GetCanDeleteContribution(2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionList_SetListToNull_YieldsEmptyList()
		{
			_model.SetContributionList(null);
			Assert.AreEqual(0, _model.Contributions.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionList_SetToValidList_YieldsCorrectList()
		{
			_model.SetContributionList(_contributions);
			Assert.AreEqual(3, _model.Contributions.Count());
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Jed", _model.Contributions.ElementAt(1).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(2).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionList_FiresEvent()
		{
			bool eventFired = false;
			_model.NewContributionListAvailable += ((o, a) => eventFired = true);
			_model.SetContributionList(null);
			Assert.IsTrue(eventFired);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionCopy_IndexTooLow_ReturnsNull()
		{
			Assert.IsNull(_model.GetContributionCopy(-1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionCopy_IndexTooHigh_ReturnsNull()
		{
			Assert.IsNull(_model.GetContributionCopy(3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionCopy_ValidIndex_ReturnsCopy()
		{
			var copy = _model.GetContributionCopy(1);
			Assert.AreEqual(_model.Contributions.ElementAt(1).ContributorName, copy.ContributorName);
			Assert.AreEqual(_model.Contributions.ElementAt(1).Role.Code, copy.Role.Code);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionValue_IndexTooLow_ReturnsNull()
		{
			Assert.IsNull(_model.GetContributionValue(-1, "name"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionValue_IndexTooHigh_ReturnsNull()
		{
			Assert.IsNull(_model.GetContributionValue(3, "name"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionValue_ValidIndexNullValueName_ReturnsNull()
		{
			Assert.IsNull(_model.GetContributionValue(1, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionValue_ValidIndexAndValueName_ReturnsValue()
		{
			Assert.AreEqual("Jed", _model.GetContributionValue(1, "name"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_IndexTooLow_DoesNothing()
		{
			_model.SetContributionValue(-1, "name", "Dusty");
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Jed", _model.Contributions.ElementAt(1).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(2).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_IndexTooHigh_DoesNothing()
		{
			_model.SetContributionValue(5, "name", "Dusty");
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Jed", _model.Contributions.ElementAt(1).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(2).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_IndexOneGreaterThanCount_AddsContributor()
		{
			_model.SetContributionValue(3, "name", "Dusty");
			Assert.AreEqual(4, _model.Contributions.Count());
			Assert.AreEqual("Dusty", _model.Contributions.ElementAt(3).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_ValidIndexNullValueName_DoesNothing()
		{
			_model.SetContributionValue(1, null, "Dusty");
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Jed", _model.Contributions.ElementAt(1).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(2).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_ValidIndexAndValueName_SetsValue()
		{
			_model.SetContributionValue(1, "name", "Dusty");
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Dusty", _model.Contributions.ElementAt(1).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(2).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_ChangeRole_ChangesRole()
		{
			Assert.AreEqual(_model.OlacRoles.ElementAt(2).Code, _model.Contributions.ElementAt(2).Role.Code);
			_model.SetContributionValue(2, "role", "Editor");
			Assert.AreEqual("editor", _model.Contributions.ElementAt(2).Role.Code);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteContribution_EmptyList_DoesNothing()
		{
			_model.SetContributionList(null);
			Assert.AreEqual(0, _model.Contributions.Count());
			_model.DeleteContribution(1);
			Assert.AreEqual(0, _model.Contributions.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteContribution_IndexTooLow_DoesNothing()
		{
			Assert.AreEqual(3, _model.Contributions.Count());
			_model.DeleteContribution(-1);
			Assert.AreEqual(3, _model.Contributions.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteContribution_IndexTooHigh_DoesNothing()
		{
			Assert.AreEqual(3, _model.Contributions.Count());
			_model.DeleteContribution(3);
			Assert.AreEqual(3, _model.Contributions.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteContribution_ValidIndex_DeletesContributor()
		{
			Assert.AreEqual(3, _model.Contributions.Count());
			_model.DeleteContribution(1);
			Assert.AreEqual(2, _model.Contributions.Count());
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(1).ContributorName);
		}
	}
}
