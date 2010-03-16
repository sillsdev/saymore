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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NUnit.Framework;
using SIL.Sponge;
using SIL.Sponge.ConfigTools;
using SilUtils;

namespace SpongeTests.ConfigToolsTests
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	///
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class NewSessionDlgViewModelTests : TestBase
	{
		private NewSessionDlgViewModel _viewModel;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public override void TestSetup()
		{
			base.TestSetup();
			InitProject();

			_viewModel = new NewSessionDlgViewModel(_prj);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void DefaultNewSessionId_Correct()
		{
			_prj.IsoCode = "Steelers";
			Assert.AreEqual("Steelers", _viewModel.DefaultNewSessionId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void NewSessionId_InitialValue_Null()
		{
			Assert.IsNull(_viewModel.NewSessionId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsNewSessionIdValid_SetsNewSessionId()
		{
			_viewModel.IsNewSessionIdValid("Seahawks", new Label());
			Assert.AreEqual("Seahawks", _viewModel.NewSessionId);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsNewSessionIdValid_WhenIdIsNull_False()
		{
			Assert.IsFalse(_viewModel.IsNewSessionIdValid(null, new Label()));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsNewSessionIdValid_WhenIdIsEmpty_False()
		{
			Assert.IsFalse(_viewModel.IsNewSessionIdValid(string.Empty, new Label()));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsNewSessionIdValid_WhenIdDoesNotExist_True()
		{
			Assert.IsTrue(_viewModel.IsNewSessionIdValid("Pirates", new Label()));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void IsNewSessionIdValid_WhenIdExists_False()
		{
			Directory.CreateDirectory(Path.Combine(_prj.SessionsFolder, "Mariners"));
			Assert.IsFalse(_viewModel.IsNewSessionIdValid("Mariners", new Label()));
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void SessionFiles_InitialValue_Empty()
		{
			Assert.AreEqual(0, _viewModel.SessionFiles.Length);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFilesToSession_AddNullList_Empty()
		{
			ReflectionHelper.CallMethod(_viewModel, "AddFilesToSession", new object[] { null });
			Assert.AreEqual(0, _viewModel.SessionFiles.Length);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		///
		/// </summary>
		/// ------------------------------------------------------------------------------------
		[Test]
		public void AddFilesToSession_AfterAddingSomeFiles()
		{
			var files = new List<string>(new[] { "Sonics", "Huskies", "Cougars" });

			ReflectionHelper.CallMethod(_viewModel, "AddFilesToSession", files);

			Assert.AreEqual(3, _viewModel.SessionFiles.Length);
			Assert.AreEqual("Sonics", _viewModel.SessionFiles[0]);
			Assert.AreEqual("Huskies", _viewModel.SessionFiles[1]);
			Assert.AreEqual("Cougars", _viewModel.SessionFiles[2]);
		}
	}
}
