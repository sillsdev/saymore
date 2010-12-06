using System.Linq;
using NUnit.Framework;
using SayMore.ClearShare;

namespace SayMoreTests.ClearShare
{
	[TestFixture]
	public class ContributorsListControlViewModelTests
	{
		private ContributorsListControlViewModel _model;
		private string _xmlWorkBlob;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_model = new ContributorsListControlViewModel();

			var system = new OlacSystem();

			var work = new Work();
			work.Contributions.AddRange(new[]
			{
				new Contribution("Leroy", system.GetRoles().ElementAt(0)),
				new Contribution("Jed", system.GetRoles().ElementAt(1)),
				new Contribution("Art", system.GetRoles().ElementAt(2))
			});

			_xmlWorkBlob = system.GetXmlForWork(work);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCanDeleteContribution_NullList_ReturnsFalse()
		{
			Assert.IsFalse(_model.GetCanDeleteContribution(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCanDeleteContribution_EmptyList_ReturnsFalse()
		{
			_model.SetWorkFromXML(null);
			Assert.AreEqual(0, _model.Contributions.Count());
			Assert.IsFalse(_model.GetCanDeleteContribution(0));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCanDeleteContribution_InvalidTooSmall_ReturnsFalse()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.IsFalse(_model.GetCanDeleteContribution(-1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCanDeleteContribution_InvalidTooBig_ReturnsFalse()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.IsFalse(_model.GetCanDeleteContribution(3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCanDeleteContribution_ValidIndex_ReturnsTrue()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.IsTrue(_model.GetCanDeleteContribution(0));
			Assert.IsTrue(_model.GetCanDeleteContribution(1));
			Assert.IsTrue(_model.GetCanDeleteContribution(2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetWorkFromXML_NullXml_YieldsEmptyContributionList()
		{
			_model.SetWorkFromXML(null);
			Assert.AreEqual(0, _model.Contributions.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetWorkFromXML_EmptyXml_YieldsEmptyContributionList()
		{
			_model.SetWorkFromXML(string.Empty);
			Assert.AreEqual(0, _model.Contributions.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetWorkFromXML_ValidXml_YieldsCorrectContributionList()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.AreEqual(3, _model.Contributions.Count());
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Jed", _model.Contributions.ElementAt(1).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(2).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionCopy_IndexTooLow_ReturnsNull()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.IsNull(_model.GetContributionCopy(-1));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionCopy_IndexTooHigh_ReturnsNull()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.IsNull(_model.GetContributionCopy(3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionCopy_ValidIndex_ReturnsCopy()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			var copy = _model.GetContributionCopy(1);
			Assert.AreEqual(_model.Contributions.ElementAt(1).ContributorName, copy.ContributorName);
			Assert.AreEqual(_model.Contributions.ElementAt(1).Role.Code, copy.Role.Code);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionValue_IndexTooLow_ReturnsNull()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.IsNull(_model.GetContributionValue(-1, "name"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionValue_IndexTooHigh_ReturnsNull()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.IsNull(_model.GetContributionValue(3, "name"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionValue_ValidIndexNullValueName_ReturnsNull()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.IsNull(_model.GetContributionValue(1, null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetContributionValue_ValidIndexAndValueName_ReturnsValue()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.AreEqual("Jed", _model.GetContributionValue(1, "name"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_IndexTooLow_DoesNothing()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			_model.SetContributionValue(-1, "name", "Dusty");
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Jed", _model.Contributions.ElementAt(1).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(2).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_IndexTooHigh_DoesNothing()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			_model.SetContributionValue(5, "name", "Dusty");
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Jed", _model.Contributions.ElementAt(1).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(2).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_IndexOneGreaterThanCount_AddsContributor()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			_model.SetContributionValue(3, "name", "Dusty");
			Assert.AreEqual(4, _model.Contributions.Count());
			Assert.AreEqual("Dusty", _model.Contributions.ElementAt(3).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_ValidIndexNullValueName_DoesNothing()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			_model.SetContributionValue(1, null, "Dusty");
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Jed", _model.Contributions.ElementAt(1).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(2).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_ValidIndexAndValueName_SetsValue()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			_model.SetContributionValue(1, "name", "Dusty");
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Dusty", _model.Contributions.ElementAt(1).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(2).ContributorName);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetContributionValue_ChangeRole_ChangesRole()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.AreEqual(_model.OlacRoles.ElementAt(2).Code, _model.Contributions.ElementAt(2).Role.Code);
			_model.SetContributionValue(2, "role", "Editor");
			Assert.AreEqual("editor", _model.Contributions.ElementAt(2).Role.Code);
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteContribution_EmptyList_DoesNothing()
		{
			_model.SetWorkFromXML(null);
			Assert.AreEqual(0, _model.Contributions.Count());
			_model.DeleteContribution(1);
			Assert.AreEqual(0, _model.Contributions.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteContribution_IndexTooLow_DoesNothing()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.AreEqual(3, _model.Contributions.Count());
			_model.DeleteContribution(-1);
			Assert.AreEqual(3, _model.Contributions.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteContribution_IndexTooHigh_DoesNothing()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.AreEqual(3, _model.Contributions.Count());
			_model.DeleteContribution(3);
			Assert.AreEqual(3, _model.Contributions.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteContribution_ValidIndex_DeletesContributor()
		{
			_model.SetWorkFromXML(_xmlWorkBlob);
			Assert.AreEqual(3, _model.Contributions.Count());
			_model.DeleteContribution(1);
			Assert.AreEqual(2, _model.Contributions.Count());
			Assert.AreEqual("Leroy", _model.Contributions.ElementAt(0).ContributorName);
			Assert.AreEqual("Art", _model.Contributions.ElementAt(1).ContributorName);
		}
	}
}
