using NUnit.Framework;
using SayMore.ClearShare;

namespace SayMoreTests.ClearShare
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class LicenseTests
	{
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AreContentsEqual_OtherIsNull_ReturnsFalse()
		{
			var l = License.CreativeCommons_Attribution_ShareAlike;
			Assert.IsFalse(l.AreContentsEqual(null));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AreContentsEqual_AreDifferent_ReturnsFalse()
		{
			var l1 = License.CreativeCommons_Attribution_ShareAlike;
			var l2 = License.CreativeCommons_Attribution;
			Assert.IsFalse(l1.AreContentsEqual(l2));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AreContentsEqual_AreSame_ReturnsTrue()
		{
			var l1 = License.CreativeCommons_Attribution_ShareAlike;
			var l2 = License.CreativeCommons_Attribution_ShareAlike;
			Assert.IsTrue(l1.AreContentsEqual(l2));
		}
	}
}
