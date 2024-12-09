using NUnit.Framework;
using SayMore.Model.Files;
using SIL.Core.ClearShare;

namespace SayMoreTests.Model.Fields
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
			Assert.IsNull(_contributions.GetValueForKey(SessionFileType.kContributionsFieldName));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetValueForKey_CorrectKey_ReturnsNameString()
		{
			var names = _contributions.GetValueForKey(SessionFileType.kContributionsFieldName);
			Assert.IsTrue(names.Contains("Leroy"));
			Assert.IsTrue(names.Contains("Jed"));
			Assert.IsTrue(names.Contains("Art"));
		}
	}
}
