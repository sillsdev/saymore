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
	public class SpongeProjectTests
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Create method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Create()
		{
			var prj = ReflectionHelper.GetResult(typeof(SpongeProject),
				"Create", "moldysponge") as SpongeProject;

			try
			{
				Assert.IsNotNull(prj);
				string expectedPath = Path.Combine(SpongeProject.MainProjectsFolder, "moldysponge");
				Assert.AreEqual(expectedPath, prj.ProjectPath);
				Assert.IsTrue(Directory.Exists(prj.ProjectPath));
				expectedPath = Path.Combine(prj.ProjectPath, "Sessions");
				Assert.IsTrue(Directory.Exists(expectedPath));
			}
			finally
			{
				try
				{
					Directory.Delete(prj.ProjectPath, true);
				}
				catch { }
			}

			Assert.IsFalse(Directory.Exists(prj.ProjectPath));
		}
	}
}
