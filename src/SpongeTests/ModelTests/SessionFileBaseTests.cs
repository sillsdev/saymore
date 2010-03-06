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
// File: SessionFileBaseTests.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
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
	public class SessionFileBaseTests
	{
		[Test]
		public void GetDisplayableFileSize_LessThan_1KB()
		{
			var result = ReflectionHelper.GetStrResult(typeof(SessionFileBase),
				"GetDisplayableFileSize", 999);

			Assert.AreEqual("999 B", result);
		}

		[Test]
		public void GetDisplayableFileSize_1KB_Exactly()
		{
			var result = ReflectionHelper.GetStrResult(typeof(SessionFileBase),
				"GetDisplayableFileSize", 1000);

			Assert.AreEqual("1 KB", result);
		}

		[Test]
		public void GetDisplayableFileSize_LessThan_1MB()
		{
			var result = ReflectionHelper.GetStrResult(typeof(SessionFileBase),
				"GetDisplayableFileSize", (long)Math.Pow(1024, 2) - 1);

			Assert.AreEqual("1024 KB", result);
		}

		//[Test]
		//public void GetDisplayableFileSize_LessThan_1MBx()
		//{
		//    long size = (long)Math.Pow(1024, 2) - 512;

		//    var result = ReflectionHelper.GetStrResult(typeof(SessionFileBase),
		//        "GetDisplayableFileSize", size);

		//    Assert.AreEqual("1023 KB", result);
		//}

		//[Test]
		//public void GetDisplayableFileSize_LessThan_1MBy()
		//{
		//    long size = (long)Math.Pow(1024, 2) - 128;

		//    var result = ReflectionHelper.GetStrResult(typeof(SessionFileBase),
		//        "GetDisplayableFileSize", size);

		//    Assert.AreEqual("1024 KB", result);
		//}

		[Test]
		public void GetDisplayableFileSize_1MB_Exactly()
		{
			var result = ReflectionHelper.GetStrResult(typeof(SessionFileBase),
				"GetDisplayableFileSize", (long)Math.Pow(1024, 2));

			Assert.AreEqual("1 MB", result);
		}

		[Test]
		public void GetDisplayableFileSize_LessThan_1GB()
		{
			var result = ReflectionHelper.GetStrResult(typeof(SessionFileBase),
				"GetDisplayableFileSize", (long)Math.Pow(1024, 3) - 1);

			Assert.AreEqual("1024 MB", result);
		}

		[Test]
		public void GetDisplayableFileSize_1GB_Exactly()
		{
			var result = ReflectionHelper.GetStrResult(typeof(SessionFileBase),
				"GetDisplayableFileSize", (long)Math.Pow(1024, 3));

			Assert.AreEqual("1 GB", result);
		}
	}
}
