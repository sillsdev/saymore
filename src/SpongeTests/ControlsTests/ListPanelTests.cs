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
using System.IO;
using NUnit.Framework;
using SilUtils;
using System.Windows.Forms;

namespace SIL.Sponge.Controls
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests for the SIL.Sponge.ConfigTools.PathValidator class
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class ListPanelests
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
		/// Runs after each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTearDown()
		{
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
			Assert.AreEqual("cheese", m_lp.Items[0]);
			Assert.AreEqual("wine", m_lp.Items[1]);
			Assert.AreEqual("bread", m_lp.Items[2]);
			Assert.AreEqual("pickles", m_lp.Items[3]);

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

			m_lp.Items = new[] { "eyes", "ears", "nose", "mouth" };
			Assert.IsNull(m_lp.CurrentItem);

			m_lp.CurrentItem = "nose";
			Assert.AreEqual("nose", m_lp.CurrentItem);

			m_lp.CurrentItem = "lips";
			Assert.AreEqual("nose", m_lp.CurrentItem);

			m_lp.CurrentItem = null;
			Assert.AreEqual("nose", m_lp.CurrentItem);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the handler for clicking the delete button when the list is empty.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteClicked_EmptyList()
		{
			m_lp.DeleteButtonClicked += delegate { throw new Exception("Should never get here!"); };
			ReflectionHelper.CallMethodWithThrow(m_lp, "btnDelete_Click", new object[] { null, null });
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the handler for clicking the delete button.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DeleteClicked()
		{
			bool delegateCalled = false;

			m_lp.DeleteButtonClicked += delegate(object sender, List<string> itemsToDelete)
			{
				Assert.AreEqual(3, itemsToDelete.Count);
				Assert.AreEqual("water", itemsToDelete[0]);
				Assert.AreEqual("fire", itemsToDelete[1]);
				Assert.AreEqual("wind", itemsToDelete[2]);

				// Remove one of the selected items so it will not be removed from the list.
				itemsToDelete.Remove("fire");

				delegateCalled = true;
				return true;
			};

			m_lp.Items = new[] { "sky", "earth", "water", "fire", "wind", "rock" };
			Assert.AreEqual(6, m_lp.Items.Length);

			m_lp.CurrentItem = "fire";
			Assert.AreEqual("fire", m_lp.CurrentItem);

			m_lp.ListView.Items[2].Selected = true;
			m_lp.ListView.Items[3].Selected = true;
			m_lp.ListView.Items[4].Selected = true;

			ReflectionHelper.CallMethod(m_lp, "btnDelete_Click", new object[] { null, null });
			Assert.IsTrue(delegateCalled);

			Assert.AreEqual("fire", m_lp.CurrentItem);

			// Make sure only two of the 3 originally selected items got removed.
			var items = m_lp.Items;
			Assert.AreEqual(4, items.Length);
			Assert.AreEqual("sky", items[0]);
			Assert.AreEqual("earth", items[1]);
			Assert.AreEqual("fire", items[2]);
			Assert.AreEqual("rock", items[3]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the handler for the selected item changing.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SelectedItemChanged()
		{
			string expectedItem = null;
			bool delegateCalled = false;

			m_lp.SelectedItemChanged += delegate(object sender, string newItem)
			{
				Assert.AreEqual(expectedItem, newItem);
				delegateCalled = true;
			};

			m_lp.Items = new[] { "wheat", "oats", "flax", "rice" };
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

			expectedItem = "wheat";
			m_lp.CurrentItem = expectedItem;
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