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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NUnit.Framework;
using Palaso.TestUtilities;

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
		[SetUp]
		public void Setup()
		{
			Palaso.Reporting.ErrorReport.IsOkToInteractWithUser = false;
		}

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

		[Test]
		public void GetContextMenuItems_OddBallFile_GetJustTwoMenus()
		{
			var sf = new SessionFile();
			using (var f = new TempFile())
			{
				sf.FullFilePath = f.Path;
				Assert.AreEqual(2, sf.GetContextMenuItems("x").Count());
			}
		}

		[Test]
		public void GetContextMenuItems_AudioFile_GetSeveralRenamingMenus()
		{
			var sf = new SessionFile();
			using (var f = new TemporaryFolder("spongeTests"))
			{
				string path = f.Combine("foo.wav");//TODO: extract to TempWaveFile
				File.WriteAllText(path, "");
				sf.FullFilePath = path;
				Assert.IsTrue(sf.GetContextMenuItems("x").Count() > 4);
			}
		}

		[Test]
		public void IdentifyComponent_ShouldBeOk_Renames()
		{
			var sf = new SessionFile();
			using (var f = new TemporaryFolder("spongeTests"))
			{
				string path = f.Combine("foo.wav");
				File.WriteAllText(path, "");
				sf.FullFilePath = path;
				//couldn't use the menu directly from test code:  sf.GetContextMenuItems("x").ToArray()[3].Select();

				sf.IdentifyAsComponent(SessionComponentDefinition.CreateHardCodedDefinitions().First(), "x");
				Assert.AreEqual("x_Original.wav", sf.FileName);
			}

		}

	}
}
