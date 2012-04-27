using System.Linq;
using NUnit.Framework;
using SayMore.Utilities.LowLevelControls;

namespace SayMoreTests.UI.LowLevelControls
{
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class MultiValuePickerPopupTests
	{
		private MultiValuePickerPopup _pickerPopup;

		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void Setup()
		{
			_pickerPopup = new MultiValuePickerPopup();
			Assert.AreEqual(0, _pickerPopup.AllItems.Count());
			Assert.AreEqual(0, _pickerPopup.CheckedItems.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Add_CallWithText_AddsToList()
		{
			_pickerPopup.Add("apples");
			Assert.AreEqual(1, _pickerPopup.AllItems.Count());
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x.Text == "apples"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Add_CallWithSameTextTwice_DoesNotAddAfterFirstTime()
		{
			_pickerPopup.Add("apples");
			Assert.AreEqual(1, _pickerPopup.AllItems.Count());
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x.Text == "apples"));

			_pickerPopup.Add("apples");
			Assert.AreEqual(1, _pickerPopup.AllItems.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Add_CallWithPickerItem_AddsToList()
		{
			var pickerItem = new PickerPopupItem { Text = "apples" };

			_pickerPopup.Add(pickerItem);
			Assert.AreEqual(1, _pickerPopup.AllItems.Count());
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x == pickerItem));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Add_CallWithSamePickerItem_DoesNotAddAfterFirstTime()
		{
			var pickerItem = new PickerPopupItem { Text = "apples" };

			_pickerPopup.Add(pickerItem);
			Assert.AreEqual(1, _pickerPopup.AllItems.Count());
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x == pickerItem));

			_pickerPopup.Add(pickerItem);
			Assert.AreEqual(1, _pickerPopup.AllItems.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddRange_CallWithArray_AddsToList()
		{
			var p1 = new PickerPopupItem { Text = "apples" };
			var p2 = new PickerPopupItem { Text = "cherries" };
			var p3 = new PickerPopupItem { Text = "mangos" };

			_pickerPopup.AddRange(new[] { p1, p2, p3 });
			Assert.AreEqual(3, _pickerPopup.AllItems.Count());
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x == p1));
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x == p2));
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x == p3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddRange_CallWithArrayWithSomeDups_AddsToListOnlyNewItems()
		{
			var p1 = new PickerPopupItem { Text = "apples" };
			var p2 = new PickerPopupItem { Text = "cherries" };

			_pickerPopup.AddRange(new[] { p1, p2 });
			Assert.AreEqual(2, _pickerPopup.AllItems.Count());
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x == p1));
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x == p2));

			var p3 = new PickerPopupItem { Text = "mangos" };
			_pickerPopup.AddRange(new[] { p1, p3 });
			Assert.AreEqual(3, _pickerPopup.AllItems.Count());
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x == p1));
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x == p2));
			Assert.IsNotNull(_pickerPopup.AllItems.Single(x => x == p3));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CheckItem_AddItemAndCheck_VerifyChecked()
		{
			_pickerPopup.Add("apples");
			_pickerPopup.Add("mangos");
			Assert.IsFalse(_pickerPopup.IsItemChecked("apples"));
			Assert.IsFalse(_pickerPopup.IsItemChecked("mangos"));

			_pickerPopup.CheckItem("apples");
			Assert.IsTrue(_pickerPopup.IsItemChecked("apples"));
			Assert.IsFalse(_pickerPopup.IsItemChecked("mangos"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void Add_AddItem_VerifyAddedUnChecked()
		{
			_pickerPopup.Add("apples");
			Assert.IsFalse(_pickerPopup.IsItemChecked("apples"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void UnCheckItem_AddItemCheckAndUnCheck_VerifyGetsCheckedAndUnChecked()
		{
			_pickerPopup.Add("apples");
			Assert.IsFalse(_pickerPopup.IsItemChecked("apples"));

			_pickerPopup.CheckItem("apples");
			Assert.IsTrue(_pickerPopup.IsItemChecked("apples"));

			_pickerPopup.UnCheckItem("apples");
			Assert.IsFalse(_pickerPopup.IsItemChecked("apples"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsItemChecked_StartEmptyList_False()
		{
			Assert.IsFalse(_pickerPopup.IsItemChecked("apples"));
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CheckItem_ItemNotInList_DoesNothing()
		{
			_pickerPopup.Add("orange");
			Assert.AreEqual(0, _pickerPopup.CheckedItems.Count());

			_pickerPopup.CheckItem("mango");
			Assert.AreEqual(0, _pickerPopup.CheckedItems.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void CheckItem_ReCheckItem_DoesNothing()
		{
			_pickerPopup.Add("orange");

			_pickerPopup.CheckItem("orange");
			Assert.AreEqual(1, _pickerPopup.CheckedItems.Count());

			_pickerPopup.CheckItem("orange");
			Assert.AreEqual(1, _pickerPopup.CheckedItems.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void UnCheckItem_ItemNotInList_DoesNothing()
		{
			_pickerPopup.Add("orange");

			_pickerPopup.CheckItem("orange");
			Assert.AreEqual(1, _pickerPopup.CheckedItems.Count());

			_pickerPopup.UnCheckItem("mango");
			Assert.AreEqual(1, _pickerPopup.CheckedItems.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void UnCheckValue_ReUncheckedItem_DoesNothing()
		{
			_pickerPopup.Add("orange");
			_pickerPopup.Add("apple");

			_pickerPopup.CheckItem("orange");
			_pickerPopup.CheckItem("apple");
			Assert.AreEqual(2, _pickerPopup.CheckedItems.Count());

			_pickerPopup.UnCheckItem("apple");
			Assert.AreEqual(1, _pickerPopup.CheckedItems.Count());

			_pickerPopup.UnCheckItem("apple");
			Assert.AreEqual(1, _pickerPopup.CheckedItems.Count());
		}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetCheckedItemsString_WithEmptyList_GetsEmptyString()
		//{
		//    Assert.IsEmpty(_model.GetCheckedItemsString(';'));
		//}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCheckedItemsString_WithSingleCheckedValue_GetsItemWithNoDelimiters()
		{
			_pickerPopup.Add("apples");
			_pickerPopup.Add("pears");
			_pickerPopup.CheckItem("apples");
			Assert.AreEqual("apples", _pickerPopup.GetCheckedItemsString());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCheckedItemsString_WithMultipleCheckedValues_GetsCorrectList()
		{
			_pickerPopup.Add("apples");
			_pickerPopup.Add("pears");
			_pickerPopup.Add("cherries");

			_pickerPopup.CheckItem("apples");
			_pickerPopup.CheckItem("cherries");

			Assert.AreEqual("apples; cherries", _pickerPopup.GetCheckedItemsString());
		}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void GetCheckedItemsString_UsingNonDefaultDelimiter_GetsCorrectList()
		//{
		//    _model.Add("apples");
		//    _model.Add("cherries");
		//    _model.Add("pears");

		//    _model.CheckItem("cherries");
		//    _model.CheckItem("pears");

		//    Assert.AreEqual("cherries, pears", _model.GetCheckedItemsString(','));
		//}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void GetCheckedItemsString_Call_GetsSortedList()
		{
			_pickerPopup.Add("pears");
			_pickerPopup.Add("cherries");
			_pickerPopup.Add("apples");

			_pickerPopup.CheckItem("pears");
			_pickerPopup.CheckItem("cherries");
			_pickerPopup.CheckItem("apples");

			Assert.AreEqual("apples; cherries; pears", _pickerPopup.GetCheckedItemsString());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetCheckedItemsFromDelimitedString_SendEmptyString_UnchecksAll()
		{
			_pickerPopup.Add("apples");
			_pickerPopup.Add("pears");

			_pickerPopup.CheckItem("apples");
			_pickerPopup.CheckItem("pears");

			Assert.AreEqual(2, _pickerPopup.CheckedItems.Count());

			_pickerPopup.SetCheckedItemsFromDelimitedString(string.Empty);
			Assert.AreEqual(0, _pickerPopup.CheckedItems.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetCheckedItemsFromDelimitedString_SendAllItemsWithDefaultDelim_ChecksAll()
		{
			_pickerPopup.Add("apples");
			_pickerPopup.Add("pears");

			_pickerPopup.SetCheckedItemsFromDelimitedString("apples; pears");

			Assert.AreEqual(2, _pickerPopup.CheckedItems.Count());
			Assert.IsNotNull(_pickerPopup.CheckedItems.Single(x => x.Text == "apples"));
			Assert.IsNotNull(_pickerPopup.CheckedItems.Single(x => x.Text == "pears"));
		}

		///// ------------------------------------------------------------------------------------
		//[Test]
		//public void SetCheckedItemsFromDelimitedString_SendAllItemsWithCustomDelim_ChecksAll()
		//{
		//    _model.Add("apples");
		//    _model.Add("pears");

		//    _model.SetCheckedItemsFromDelimitedString("apples, pears", ',');

		//    Assert.AreEqual(2, _model.CheckedItems.Count());
		//    Assert.IsNotNull(_model.CheckedItems.Single(x => x.Text == "apples"));
		//    Assert.IsNotNull(_model.CheckedItems.Single(x => x.Text == "pears"));
		//}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetCheckedItemsFromDelimitedString_WithOnlyDelimitersInString_TreatsLikeEmptyString()
		{
			_pickerPopup.Add("apples");
			_pickerPopup.Add("pears");

			_pickerPopup.SetCheckedItemsFromDelimitedString("; ;; ;;; ;");
			Assert.AreEqual(0, _pickerPopup.CheckedItems.Count());
		}

		/// ------------------------------------------------------------------------------------
		[Test]
		public void SetCheckedItemsFromDelimitedString_SpacesBeforeDelimiter_TrimsItem()
		{
			_pickerPopup.Add("apples");
			_pickerPopup.Add("pears");
			_pickerPopup.Add("cherries");

			_pickerPopup.SetCheckedItemsFromDelimitedString("apples ; pears ; cherries");
			Assert.IsNotNull(_pickerPopup.CheckedItems.Single(x => x.Text == "apples"));
			Assert.IsNotNull(_pickerPopup.CheckedItems.Single(x => x.Text == "pears"));
			Assert.IsNotNull(_pickerPopup.CheckedItems.Single(x => x.Text == "cherries"));
		}
	}
}
