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
// File: SpongeProjectTests.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.IO;
using NUnit.Framework;
using SilUtils;

namespace SIL.Sponge.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SessionFileTests
	{
		//private const string kTestPrjName = "~~Moldy Sponge";
		//private const string kTestPrjFileName = "~~MoldySponge.sprj";
		//private const string kTestSessionName = "~~Fungus";

		//private SpongeProject m_prj;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs before each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
			//m_prj = ReflectionHelper.GetResult(typeof(SpongeProject),
			//    "Create", kTestPrjName) as SpongeProject;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs after each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public void TestTearDown()
		{
			//try
			//{
			//    Directory.Delete(m_prj.ProjectPath, true);
			//}
			//catch { }
		}

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
