// ---------------------------------------------------------------------------------------------
#region // Copyright (c) 2010, SIL International. All Rights Reserved.
// <copyright from='2010' to='2010' company='SIL International'>
//		Copyright (c) 2010, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: ListPanelests.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using NUnit.Framework;
using SilUtils;

namespace SIL.Sponge.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests for the SIL.Sponge.ConfigTools.PathValidator class
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class ListPanelTests
	{
		private ListPanel m_lp;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs before each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			m_lp = new ListPanel();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that Items property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Items()
		{
			Assert.AreEqual(0, m_lp.Items.Length);

			m_lp.Items = new[] { "cheese", "wine", "bread", "pickles" };
			Assert.AreEqual(4, m_lp.Items.Length);
			Assert.AreEqual("bread", m_lp.Items[0]);
			Assert.AreEqual("cheese", m_lp.Items[1]);
			Assert.AreEqual("pickles", m_lp.Items[2]);
			Assert.AreEqual("wine", m_lp.Items[3]);

			m_lp.Items = null;
			Assert.AreEqual(0, m_lp.Items.Length);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that CurrentItem property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void CurrentItem()
		{
			Assert.IsNull(m_lp.CurrentItem);

			m_lp.Items = new[] { "nose", "eyes", "ears", "mouth" };
			Assert.AreEqual("ears", m_lp.CurrentItem);

			m_lp.CurrentItem = "eyes";
			Assert.AreEqual("eyes", m_lp.CurrentItem);

			m_lp.CurrentItem = "lips";
			Assert.AreEqual("eyes", m_lp.CurrentItem);

			m_lp.CurrentItem = null;
			Assert.AreEqual("eyes", m_lp.CurrentItem);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the handler for clicking the delete button when the list is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AfterItemsDeleted_EmptyList()
		{
			m_lp.AfterItemsDeleted += delegate { throw new Exception("Should never get here!"); };
			ReflectionHelper.CallMethodWithThrow(m_lp, "btnDelete_Click", new object[] { null, null });
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the handler for clicking the delete button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AfterItemsDeleted()
		{
			bool delegateCalled = false;

			m_lp.AfterItemsDeleted += delegate(object sender, List<object> itemsToDelete)
			{
				Assert.AreEqual(3, itemsToDelete.Count);
				Assert.AreEqual("rock", itemsToDelete[0]);
				Assert.AreEqual("sky", itemsToDelete[1]);
				Assert.AreEqual("water", itemsToDelete[2]);
				delegateCalled = true;
			};

			m_lp.Items = new[] { "sky", "earth", "water", "fire", "wind", "rock" };
			Assert.AreEqual(6, m_lp.Items.Length);

			m_lp.CurrentItem = "sky";
			Assert.AreEqual("sky", m_lp.CurrentItem);

			m_lp.ListView.Items[2].Selected = true;
			m_lp.ListView.Items[3].Selected = true;
			m_lp.ListView.Items[4].Selected = true;

			ReflectionHelper.CallMethod(m_lp, "btnDelete_Click", new object[] { null, null });
			Assert.IsTrue(delegateCalled);

			Assert.AreEqual("wind", m_lp.CurrentItem);

			// Make sure the 3 items selected got removed.
			var items = m_lp.Items;
			Assert.AreEqual(3, items.Length);
			Assert.AreEqual("earth", items[0]);
			Assert.AreEqual("fire", items[1]);
			Assert.AreEqual("wind", items[2]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the handler for the selected item changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SelectedItemChanged()
		{
			string expectedItem = "flax";
			bool delegateCalled = false;

			m_lp.SelectedItemChanged += delegate(object sender, object newItem)
			{
				Assert.AreEqual(expectedItem, newItem);
				delegateCalled = true;
			};

			m_lp.Items = new[] { expectedItem, "oats", "flax", "rice" };
			Assert.AreEqual(4, m_lp.Items.Length);

			expectedItem = "oats";
			m_lp.CurrentItem = expectedItem;
			Assert.AreEqual(1, m_lp.ListView.SelectedItems.Count);
			Assert.IsTrue(delegateCalled);
			delegateCalled = false;

			expectedItem = "rice";
			m_lp.CurrentItem = expectedItem;
			Assert.AreEqual(1, m_lp.ListView.SelectedItems.Count);
			Assert.IsTrue(delegateCalled);
			delegateCalled = false;

			expectedItem = "rice";
			m_lp.CurrentItem = "wheat";
			Assert.AreEqual(1, m_lp.ListView.SelectedItems.Count);
			Assert.IsTrue(delegateCalled);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the handler for the new button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void NewButtonClicked()
		{
			string expectedNewItem = null;
			bool delegateCalled = false;

			m_lp.NewButtonClicked += delegate
			{
				delegateCalled = true;
				return expectedNewItem + "-foobar";
			};

			expectedNewItem = "Batman";
			ReflectionHelper.CallMethod(m_lp, "btnNew_Click", new object[] { null, null });
			Assert.IsTrue(delegateCalled);
			Assert.AreEqual(1, m_lp.Items.Length);
			Assert.AreEqual("Batman-foobar", m_lp.CurrentItem);
			delegateCalled = false;

			expectedNewItem = "Superman";
			ReflectionHelper.CallMethod(m_lp, "btnNew_Click", new object[] { null, null });
			Assert.IsTrue(delegateCalled);
			Assert.AreEqual(2, m_lp.Items.Length);
			Assert.AreEqual("Superman-foobar", m_lp.CurrentItem);
		}
	}
}