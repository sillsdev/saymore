using NUnit.Framework;
using SayMore.ClearShare;
using SayMore.Model.Fields;

namespace SayMoreTests.model.Fields
{
	[TestFixture]
	public class ContributionCollectionTests
	{
		private ContributionCollection _contributions;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			_contributions = new ContributionCollection(new[]
			{
				new Contribution("Leroy", null),
				new Contribution("Jed", null),
				new Contribution("Art", null)
			});
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValueForKey_NullKey_ReturnsNull()
		{
			Assert.IsNull(_contributions.GetValueForKey(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValueForKey_WrongKey_ReturnsNull()
		{
			Assert.IsNull(_contributions.GetValueForKey("wrong key"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValueForKey_EmptyList_ReturnsNull()
		{
			_contributions = new ContributionCollection();
			Assert.IsNull(_contributions.GetValueForKey("contributions"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValueForKey_CorrectKey_ReturnsNameString()
		{
			var names = _contributions.GetValueForKey("contributions");
			Assert.IsTrue(names.Contains("Leroy"));
			Assert.IsTrue(names.Contains("Jed"));
			Assert.IsTrue(names.Contains("Art"));
		}
	}
}
