using NUnit.Framework;
using System.Windows.Forms;

namespace SayMoreTests.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests for the ListPanel class
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class ListPanelTests
	{
        [Test]
        public void SearchRequested_Fires_Only_When_SearchText_Has_3_Or_More_Chars()
        {
            var panel = new SayMore.UI.LowLevelControls.ListPanel();

            bool fired = false;
            panel.SearchRequested += (s, e) => fired = true;

            // simulate typing 2 chars
            panel.SearchText = "ab";
            var mi = typeof(SayMore.UI.LowLevelControls.ListPanel)
                .GetMethod("_searchTextBox_KeyUp", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            Assert.IsNotNull(mi, "Could not find private _searchTextBox_KeyUp method via reflection");

            fired = false;
            mi.Invoke(panel, new object[] { panel, new KeyEventArgs(Keys.A) });
            Assert.IsFalse(fired, "SearchRequested should not fire for fewer than 3 characters.");

            // simulate typing 3 chars
            panel.SearchText = "abc";
            fired = false;
            mi.Invoke(panel, new object[] { panel, new KeyEventArgs(Keys.A) });
            Assert.IsTrue(fired, "SearchRequested should fire when there are 3 or more characters.");
        }
		//private ListPanel _lp;

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Runs before each test.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[SetUp]
		//public void TestSetup()
		//{
		//    _lp = new ListPanel();
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests that Items property.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void Items()
		//{
		//    Assert.AreEqual(0, _lp.Items.Length);

		//    _lp.Items = new[] { "cheese", "wine", "bread", "pickles" };
		//    Assert.AreEqual(4, _lp.Items.Length);
		//    Assert.AreEqual("bread", _lp.Items[0]);
		//    Assert.AreEqual("cheese", _lp.Items[1]);
		//    Assert.AreEqual("pickles", _lp.Items[2]);
		//    Assert.AreEqual("wine", _lp.Items[3]);

		//    _lp.Items = null;
		//    Assert.AreEqual(0, _lp.Items.Length);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests that CurrentItem property.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void CurrentItem()
		//{
		//    Assert.IsNull(_lp.CurrentItem);

		//    _lp.Items = new[] { "nose", "eyes", "ears", "mouth" };
		//    Assert.AreEqual("ears", _lp.CurrentItem);

		//    _lp.CurrentItem = "eyes";
		//    Assert.AreEqual("eyes", _lp.CurrentItem);

		//    _lp.CurrentItem = "lips";
		//    Assert.AreEqual("eyes", _lp.CurrentItem);

		//    _lp.CurrentItem = null;
		//    Assert.AreEqual("eyes", _lp.CurrentItem);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests the handler for clicking the delete button when the list is empty.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void AfterItemsDeleted_EmptyList()
		//{
		//    _lp.AfterItemsDeleted += delegate { throw new Exception("Should never get here!"); };
		//    ReflectionHelper.CallMethodWithThrow(_lp, "btnDelete_Click", new object[] { null, null });
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests the handler for clicking the delete button.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void AfterItemsDeleted()
		//{
		//    bool delegateCalled = false;

		//    _lp.AfterItemsDeleted += delegate(object sender, IEnumerable<object> itemsToDelete)
		//    {
		//        var toDelete = itemsToDelete.ToArray();

		//        Assert.AreEqual(3, toDelete.Length);
		//        Assert.AreEqual("rock", toDelete[0]);
		//        Assert.AreEqual("sky", toDelete[1]);
		//        Assert.AreEqual("water", toDelete[2]);
		//        delegateCalled = true;
		//    };

		//    _lp.Items = new[] { "sky", "earth", "water", "fire", "wind", "rock" };
		//    Assert.AreEqual(6, _lp.Items.Length);

		//    _lp.CurrentItem = "sky";
		//    Assert.AreEqual("sky", _lp.CurrentItem);

		//    _lp.ListView.Items[2].Selected = true;
		//    _lp.ListView.Items[3].Selected = true;
		//    _lp.ListView.Items[4].Selected = true;

		//    ReflectionHelper.CallMethod(_lp, "btnDelete_Click", new object[] { null, null });
		//    Assert.IsTrue(delegateCalled);

		//    Assert.AreEqual("wind", _lp.CurrentItem);

		//    // Make sure the 3 items selected got removed.
		//    var items = _lp.Items;
		//    Assert.AreEqual(3, items.Length);
		//    Assert.AreEqual("earth", items[0]);
		//    Assert.AreEqual("fire", items[1]);
		//    Assert.AreEqual("wind", items[2]);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests the handler for the selected item changing.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void SelectedItemChanged()
		//{
		//    string expectedItem = "flax";
		//    bool delegateCalled = false;

		//    _lp.SelectedItemChanged += delegate(object sender, object newItem)
		//    {
		//        Assert.AreEqual(expectedItem, newItem);
		//        delegateCalled = true;
		//    };

		//    _lp.Items = new[] { expectedItem, "oats", "flax", "rice" };
		//    Assert.AreEqual(4, _lp.Items.Length);

		//    expectedItem = "oats";
		//    _lp.CurrentItem = expectedItem;
		//    Assert.AreEqual(1, _lp.ListView.SelectedItems.Count);
		//    Assert.IsTrue(delegateCalled);
		//    delegateCalled = false;

		//    expectedItem = "rice";
		//    _lp.CurrentItem = expectedItem;
		//    Assert.AreEqual(1, _lp.ListView.SelectedItems.Count);
		//    Assert.IsTrue(delegateCalled);
		//    delegateCalled = false;

		//    expectedItem = "rice";
		//    _lp.CurrentItem = "wheat";
		//    Assert.AreEqual(1, _lp.ListView.SelectedItems.Count);
		//    Assert.IsTrue(delegateCalled);
		//}

		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Tests the handler for the new button.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void NewButtonClicked()
		//{
		//    string expectedNewItem = null;
		//    bool delegateCalled = false;

		//    _lp.NewButtonClicked += delegate
		//    {
		//        delegateCalled = true;
		//        return expectedNewItem + "-foobar";
		//    };

		//    expectedNewItem = "Batman";
		//    ReflectionHelper.CallMethod(_lp, "btnNew_Click", new object[] { null, null });
		//    Assert.IsTrue(delegateCalled);
		//    Assert.AreEqual(1, _lp.Items.Length);
		//    Assert.AreEqual("Batman-foobar", _lp.CurrentItem);
		//    delegateCalled = false;

		//    expectedNewItem = "Superman";
		//    ReflectionHelper.CallMethod(_lp, "btnNew_Click", new object[] { null, null });
		//    Assert.IsTrue(delegateCalled);
		//    Assert.AreEqual(2, _lp.Items.Length);
		//    Assert.AreEqual("Superman-foobar", _lp.CurrentItem);
		//}
	}
}