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
// File: PathValidatorTests.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.IO;
using NUnit.Framework;
using SilUtils;
using System.Windows.Forms;

namespace SIL.Sponge.ConfigTools
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Tests for the SIL.Sponge.ConfigTools.PathValidator class
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class PathValidatorTests
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs before each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public void TestSetup()
		{
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
		/// Tests that the PathOK method returns false when the base path argument is bad.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PathOK_BadBasePath()
		{
			var args = new[] { "   ", null };
			Assert.IsFalse(ReflectionHelper.GetBoolResult(typeof(PathValidator), "PathOK", args));

			var invalidChars = Path.GetInvalidFileNameChars();
			args[0] = "Sponge" + invalidChars[0] + "Bob";
			Assert.IsFalse(ReflectionHelper.GetBoolResult(typeof(PathValidator), "PathOK", args));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the PathOK method returns false when the relative path argument is bad.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PathOK_BadRelativePath()
		{
			var args = new[] { null, "  " };
			Assert.IsFalse(ReflectionHelper.GetBoolResult(typeof(PathValidator), "PathOK", args));

			var invalidChars = Path.GetInvalidFileNameChars();
			args[1] = "Sponge" + invalidChars[0] + "Bob";
			Assert.IsFalse(ReflectionHelper.GetBoolResult(typeof(PathValidator), "PathOK", args));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the PathOK method returns false when the target folder already exists.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PathOK_TargetFolderExists()
		{
			var basePath = Path.GetTempPath();
			var relPath = "SquidFace";
			Directory.CreateDirectory(Path.Combine(basePath, relPath));

			try
			{
				var args = new[] { basePath, relPath };
				Assert.IsFalse(ReflectionHelper.GetBoolResult(typeof(PathValidator), "PathOK", args));
			}
			finally
			{
				Directory.Delete(Path.Combine(basePath, relPath));
				Assert.IsFalse(Directory.Exists(Path.Combine(basePath, relPath)));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the PathOK method returns false when there is a file name with the
		/// same name as the target folder.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PathOK_TargetFolderExistsAsFile()
		{
			var basePath = Path.GetTempPath();
			var relPath = "SquidFace";
			File.CreateText(Path.Combine(basePath, relPath)).Close();

			try
			{
				var args = new[] { basePath, relPath };
				Assert.IsFalse(ReflectionHelper.GetBoolResult(typeof(PathValidator), "PathOK", args));
			}
			finally
			{
				File.Delete(Path.Combine(basePath, relPath));
				Assert.IsFalse(File.Exists(Path.Combine(basePath, relPath)));
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests that the PathOK method returns true when all is well with the base and
		/// relative paths.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void PathOK()
		{
			var args = new[] { Path.GetTempPath(), "SquidFace" };
			Assert.IsTrue(ReflectionHelper.GetBoolResult(typeof(PathValidator), "PathOK", args));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the ValidatePathEntry when a bad path is specified.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ValidatePathEntry_BadPath()
		{
			var lbl = new Label();
			Assert.IsFalse(PathValidator.ValidatePathEntry("  ", "?", lbl,
				string.Empty, "Bad path numbskull", null));

			Assert.AreEqual("Bad path numbskull", lbl.Text);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the ValidatePathEntry method.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void ValidatePathEntry()
		{
			var lbl = new Label();
			Assert.IsTrue(PathValidator.ValidatePathEntry(@"c:\sponge\bob\square\pants",
				"SquidFace", lbl, "Good path {0}", string.Empty, null));

			Assert.AreEqual(@"Good path square\pants\SquidFace", lbl.Text);
		}
	}
}
