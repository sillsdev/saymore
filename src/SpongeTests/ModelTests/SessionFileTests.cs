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
// File: SessionFileTests.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using NUnit.Framework;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SessionFileTests : TestBase
	{
		///// ------------------------------------------------------------------------------------
		///// <summary>
		///// Runs before each test.
		///// </summary>
		///// ------------------------------------------------------------------------------------
		//public override void TestSetup()
		//{
		//    base.TestSetup();
		//}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the TagList property.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void TagList()
		{
			var sf = new SessionFile();
			sf.Tags = "classical; rock; ; jazz";
			Assert.AreEqual(3, sf.TagList.Count);
			Assert.AreEqual("classical", sf.TagList[0]);
			Assert.AreEqual("rock", sf.TagList[1]);
			Assert.AreEqual("jazz", sf.TagList[2]);
		}
	}
}
