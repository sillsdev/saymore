using System.Linq;
using NUnit.Framework;
using SIL.Archiving.IMDI.Lists;

namespace SayMoreTests.model
{
	[TestFixture]
	class IMDITests
	{

		[Test]
		public void ListConstructorGetList_AddBlankItemTwice_AddsJustOneBlank()
		{
			var list = ListConstructor.GetList(ListType.ContentInteractivity).ToList();
			var originalCount = list.Count;

			list.Insert(0, new IMDIListItem(string.Empty, string.Empty)); // add a blank option
			Assert.AreEqual(originalCount + 1, list.Count);

			var list2 = ListConstructor.GetList(ListType.ContentInteractivity).ToList();
			list2.Insert(0, new IMDIListItem(string.Empty, string.Empty)); // add a blank option
			Assert.AreEqual(originalCount + 1, list2.Count);
		}
	}
}
