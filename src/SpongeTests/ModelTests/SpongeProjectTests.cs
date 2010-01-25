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
		/// Tests the private Create method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Create()
		{
			var prj = ReflectionHelper.GetResult(typeof(SpongeProject),
				"Create", "Moldy Sponge") as SpongeProject;

			try
			{
				VerifyProject(prj, "Moldy Sponge");
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

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the Load method
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void Load()
		{
			var prj1 = ReflectionHelper.GetResult(typeof(SpongeProject),
				"Create", "Moldy Sponge") as SpongeProject;

			try
			{
				VerifyProject(prj1, "Moldy Sponge");
				var prj2 = SpongeProject.Load(prj1.FullProjectPath);
				VerifyProject(prj2, "Moldy Sponge");
			}
			finally
			{
				try
				{
					Directory.Delete(prj1.ProjectPath, true);
				}
				catch { }
			}

			Assert.IsFalse(Directory.Exists(prj1.ProjectPath));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Verifies the existence of the specified project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		private static void VerifyProject(SpongeProject prj, string expectedPrjName)
		{
			Assert.IsNotNull(prj);
			var expectedPath = Path.Combine(SpongeProject.ProjectsFolder, expectedPrjName);
			Assert.AreEqual(expectedPath, prj.ProjectPath);
			Assert.IsTrue(Directory.Exists(prj.ProjectPath));

			expectedPath = Path.Combine(prj.ProjectPath, "Sessions");
			Assert.AreEqual(expectedPath, prj.SessionsPath);
			Assert.IsTrue(Directory.Exists(prj.SessionsPath));

			expectedPath = Path.Combine(prj.ProjectPath, "MoldySponge.sprj");
			Assert.AreEqual(expectedPath, prj.FullProjectPath);
			Assert.IsTrue(File.Exists(prj.FullProjectPath));
		}
	}
}
