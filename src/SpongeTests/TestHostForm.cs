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
// File: TestHostForm.cs
// Responsibility: D. Olson
//
// <remarks>
// </remarks>
// ---------------------------------------------------------------------------------------------
using System.Windows.Forms;

namespace SIL.Sponge
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Encapsulates a class to host controls being tested. The reason they need to be hosted
	/// is because until a control is hosted on a visible form, the visible properties of
	/// its child controls are always false.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class TestHostForm : Form
	{
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="TestHostForm"/> class.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public TestHostForm()
		{
			StartPosition = FormStartPosition.Manual;
			ShowInTaskbar = false;
			ShowIcon = false;
			MaximizeBox = false;
			MinimizeBox = false;
			FormBorderStyle = FormBorderStyle.None;
			Width = 0;
			Height = 0;
			Top = -(Height + 100);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Never want to activate the form while running tests.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestHostForm));
			this.SuspendLayout();
			//
			// TestHostForm
			//
			this.Name = "TestHostForm";
			this.ResumeLayout(false);

		}
	}
}
