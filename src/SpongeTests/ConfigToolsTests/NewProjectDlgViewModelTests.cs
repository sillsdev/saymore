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
// File: NewSessionDlgViewModelTests.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.IO;
using System.Windows.Forms;
using NUnit.Framework;
using SIL.Sponge;
using SIL.Sponge.ConfigTools;
using SIL.Sponge.Model;

namespace SpongeTests.ConfigToolsTests
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class NewProjectDlgViewModelTests : TestBase
	{
		private NewProjectDlgViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void TestSetup()
		{
			base.TestSetup();

			_viewModel = new NewProjectDlgViewModel();
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void NewProjectName_InitialValue_Null()
		{
			Assert.IsNull(_viewModel.NewProjectName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void NewProjectName_SetsNewProjectName()
		{
			_viewModel.IsNewProjectNameValid("Panthers", new Label());
			Assert.AreEqual("Panthers", _viewModel.NewProjectName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>h
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsNewProjectNameValid_WhenNameIsNull_False()
		{
			Assert.IsFalse(_viewModel.IsNewProjectNameValid(null, new Label()));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsNewProjectNameValid_WhenNameIsEmpty_False()
		{
			Assert.IsFalse(_viewModel.IsNewProjectNameValid(string.Empty, new Label()));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsNewSessionIdValid_WhenNameDoesNotExist_True()
		{
			Assert.IsTrue(_viewModel.IsNewProjectNameValid("Pirates", new Label()));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsNewProjectNameValid_WhenNameExists_False()
		{
			Directory.CreateDirectory(Path.Combine(SpongeProject.ProjectsFolder, "Cubs"));
			Assert.IsFalse(_viewModel.IsNewProjectNameValid("Cubs", new Label()));
		}
	}
}
