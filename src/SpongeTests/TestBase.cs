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
// File: TestBase.cs
// Responsibility: Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using SIL.Sponge.Model;
using SilUtils;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TestBase
	{
		protected const string kTestPrjName = "~~Moldy Sponge";
		protected const string kTestPrjFileName = "~~MoldySponge.sprj";
		protected const string kTestSessionName = "~~Fungus";

		protected SpongeProject m_prj;
		private string m_mainAppSettingsFldr;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs before each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[SetUp]
		public virtual void TestSetup()
		{
			m_mainAppSettingsFldr = Path.Combine(Path.GetTempPath(), "~SpongeTestProjects~");
			Directory.CreateDirectory(m_mainAppSettingsFldr);
			ReflectionHelper.SetField(typeof(Sponge), "s_mainAppSettingsFldr", m_mainAppSettingsFldr);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Runs after each test.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[TearDown]
		public virtual void TestTearDown()
		{
			if (m_mainAppSettingsFldr != null)
			{
				try
				{
					Directory.Delete(m_prj.ProjectPath, true);
				}
				catch { }
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a test project.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected void InitProject()
		{
			m_prj = ReflectionHelper.GetResult(typeof(SpongeProject),
				"Create", kTestPrjName) as SpongeProject;
		}
	}
}
